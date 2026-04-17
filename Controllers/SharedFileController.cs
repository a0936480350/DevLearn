using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class SharedFileController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;
    private const long MaxFileSize = 50 * 1024 * 1024; // 50 MB

    // 允許的副檔名
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".docx", ".doc", ".pptx", ".ppt", ".xlsx", ".xls",
        ".png", ".jpg", ".jpeg", ".gif", ".webp",
        ".txt", ".md", ".zip", ".7z"
    };

    public SharedFileController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // 公開列表：大家都能看
    public async Task<IActionResult> Index(string? category = null, string? search = null)
    {
        var query = _db.SharedFiles.Where(f => f.IsPublic);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(f => f.Category == category);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(f => f.Title.Contains(search) || f.Description.Contains(search));

        var files = await query.OrderByDescending(f => f.CreatedAt).ToListAsync();

        ViewBag.Category = category;
        ViewBag.Search = search;
        return View(files);
    }

    // 下載（公開，會計次）
    public async Task<IActionResult> Download(int id)
    {
        var file = await _db.SharedFiles.FindAsync(id);
        if (file == null || !file.IsPublic) return NotFound();

        var fullPath = Path.Combine(_env.WebRootPath, file.StoragePath.TrimStart('/'));
        if (!System.IO.File.Exists(fullPath)) return NotFound("檔案已不存在");

        // 計數
        file.DownloadCount++;
        await _db.SaveChangesAsync();

        var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
        return File(bytes, file.MimeType, file.FileName);
    }

    // 內嵌預覽（for PDF / Image，讓瀏覽器直接開）
    public async Task<IActionResult> Preview(int id)
    {
        var file = await _db.SharedFiles.FindAsync(id);
        if (file == null || !file.IsPublic) return NotFound();

        var fullPath = Path.Combine(_env.WebRootPath, file.StoragePath.TrimStart('/'));
        if (!System.IO.File.Exists(fullPath)) return NotFound();

        var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
        // inline 讓瀏覽器嘗試直接顯示，而不是下載
        Response.Headers["Content-Disposition"] = $"inline; filename=\"{Uri.EscapeDataString(file.FileName)}\"";
        return File(bytes, file.MimeType);
    }

    // ═══════════════════════════════════════════
    //  Admin 功能（需登入 admin）
    // ═══════════════════════════════════════════

    private bool IsAdmin()
    {
        return HttpContext.Request.Cookies.ContainsKey("AdminAuth")
            && HttpContext.Request.Cookies["AdminAuth"] == "pxmart-admin-verified-2026";
    }

    public IActionResult Upload()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Admin");
        return View();
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [RequestSizeLimit(MaxFileSize)]
    public async Task<IActionResult> Upload(IFormFile file, string title, string? description, string? category, string? tags)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Admin");

        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "請選擇檔案";
            return View();
        }

        if (file.Length > MaxFileSize)
        {
            TempData["Error"] = $"檔案超過 {MaxFileSize / 1024 / 1024} MB 限制";
            return View();
        }

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
        {
            TempData["Error"] = $"不支援的檔案類型：{ext}";
            return View();
        }

        // 儲存到 wwwroot/uploads/shared/
        var uploadDir = Path.Combine(_env.WebRootPath, "uploads", "shared");
        Directory.CreateDirectory(uploadDir);

        var safeFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(uploadDir, safeFileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var sharedFile = new SharedFile
        {
            Title = string.IsNullOrWhiteSpace(title) ? Path.GetFileNameWithoutExtension(file.FileName) : title,
            Description = description ?? "",
            FileName = file.FileName,
            StoragePath = $"uploads/shared/{safeFileName}",
            FileSize = file.Length,
            MimeType = GetMimeType(ext),
            Category = string.IsNullOrWhiteSpace(category) ? "general" : category,
            UploadedBy = "admin",
            Tags = tags,
            IsPublic = true,
        };

        _db.SharedFiles.Add(sharedFile);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"上傳成功：{sharedFile.Title}";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAdmin()) return Forbid();

        var file = await _db.SharedFiles.FindAsync(id);
        if (file == null) return NotFound();

        // 實體檔案也刪除
        var fullPath = Path.Combine(_env.WebRootPath, file.StoragePath.TrimStart('/'));
        if (System.IO.File.Exists(fullPath))
        {
            try { System.IO.File.Delete(fullPath); } catch { }
        }

        _db.SharedFiles.Remove(file);
        await _db.SaveChangesAsync();

        TempData["Success"] = "已刪除";
        return RedirectToAction(nameof(Index));
    }

    private static string GetMimeType(string ext) => ext.ToLowerInvariant() switch
    {
        ".pdf" => "application/pdf",
        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        ".doc" => "application/msword",
        ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        ".ppt" => "application/vnd.ms-powerpoint",
        ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        ".xls" => "application/vnd.ms-excel",
        ".png" => "image/png",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".gif" => "image/gif",
        ".webp" => "image/webp",
        ".txt" => "text/plain",
        ".md" => "text/markdown",
        ".zip" => "application/zip",
        ".7z" => "application/x-7z-compressed",
        _ => "application/octet-stream",
    };
}
