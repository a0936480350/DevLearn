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

    // 取得可寫入的儲存根目錄。
    // Azure App Service 啟用 WEBSITE_RUN_FROM_PACKAGE=1 後，wwwroot 是唯讀，
    // 需要寫到 %HOME%\data\（永久儲存；跨部署保留）。
    // 本機 dev 時 %HOME% 不存在，fallback 到 ContentRoot/App_Data。
    private string GetStorageRoot()
    {
        var home = Environment.GetEnvironmentVariable("HOME"); // Azure: D:\home
        string root;
        if (!string.IsNullOrEmpty(home) && Directory.Exists(home))
        {
            root = Path.Combine(home, "data", "shared");
        }
        else
        {
            root = Path.Combine(_env.ContentRootPath, "App_Data", "shared");
        }
        Directory.CreateDirectory(root);
        return root;
    }

    // 解析舊/新格式的 StoragePath：
    //  新格式 = "20260418xxx_xxxx.pdf"   → {GetStorageRoot()}/{name}
    //  舊格式 = "uploads/shared/xxx.pdf" → {WebRoot}/uploads/shared/xxx.pdf（向下相容）
    private string ResolveStoragePath(string storagePath)
    {
        var cleaned = storagePath.TrimStart('/', '\\');
        if (cleaned.Contains('/') || cleaned.Contains('\\'))
        {
            return Path.Combine(_env.WebRootPath, cleaned);
        }
        return Path.Combine(GetStorageRoot(), cleaned);
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

        var fullPath = ResolveStoragePath(file.StoragePath);
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

        var fullPath = ResolveStoragePath(file.StoragePath);
        if (!System.IO.File.Exists(fullPath)) return NotFound();

        var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
        // inline 讓瀏覽器嘗試直接顯示，而不是下載
        Response.Headers["Content-Disposition"] = $"inline; filename=\"{Uri.EscapeDataString(file.FileName)}\"";
        return File(bytes, file.MimeType);
    }

    // ═══════════════════════════════════════════
    //  上傳 / 刪除（需登入的註冊者；admin 可刪除任何檔案）
    // ═══════════════════════════════════════════

    private bool IsAdmin()
    {
        return HttpContext.Request.Cookies.ContainsKey("AdminAuth")
            && HttpContext.Request.Cookies["AdminAuth"] == "pxmart-admin-verified-2026";
    }

    // 目前登入的註冊者（透過 DotNetLearner cookie）；沒登入 or 匿名使用者 回傳 null
    private async Task<DotNetLearning.Models.SiteUser?> GetLoggedInUserAsync()
    {
        var anonId = HttpContext.Request.Cookies["DotNetLearner"];
        if (string.IsNullOrEmpty(anonId)) return null;
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        return (user != null && user.IsRegistered) ? user : null;
    }

    public async Task<IActionResult> Upload()
    {
        var user = await GetLoggedInUserAsync();
        if (user == null && !IsAdmin())
        {
            TempData["Error"] = "請先登入後再上傳檔案";
            return RedirectToAction("Login", "Account");
        }
        return View();
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [RequestSizeLimit(MaxFileSize)]
    public async Task<IActionResult> Upload(IFormFile file, string title, string? description, string? category, string? tags)
    {
        var user = await GetLoggedInUserAsync();
        var admin = IsAdmin();
        if (user == null && !admin)
        {
            TempData["Error"] = "請先登入後再上傳檔案";
            return RedirectToAction("Login", "Account");
        }

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

        // 儲存到 Azure 可寫入路徑（%HOME%\data\shared）或本機 App_Data\shared
        var uploadDir = GetStorageRoot();

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
            StoragePath = safeFileName,
            FileSize = file.Length,
            MimeType = GetMimeType(ext),
            Category = string.IsNullOrWhiteSpace(category) ? "general" : category,
            UploadedBy = admin ? "admin" : (user?.Nickname ?? "user"),
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
        var file = await _db.SharedFiles.FindAsync(id);
        if (file == null) return NotFound();

        // 只有 admin 或「當初上傳者（對得上 Nickname）」可以刪除
        var user = await GetLoggedInUserAsync();
        var admin = IsAdmin();
        var canDelete = admin || (user != null && file.UploadedBy == user.Nickname);
        if (!canDelete) return Forbid();

        // 實體檔案也刪除
        var fullPath = ResolveStoragePath(file.StoragePath);
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
