using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;

namespace DotNetLearning.Controllers;

public class PromotionController : Controller
{
    private readonly AppDbContext _db;
    public PromotionController(AppDbContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        var anonId = HttpContext.Request.Cookies["DotNetLearner"] ?? "";
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        ViewBag.User = user;
        ViewBag.TopReferrers = await _db.SiteUsers
            .Where(u => u.ReferralCount > 0)
            .OrderByDescending(u => u.ReferralCount)
            .Take(10)
            .ToListAsync();
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> TotalReferrals()
    {
        var count = await _db.SiteUsers.CountAsync(u => u.IsRegistered);
        return Json(new { count });
    }

    /// <summary>
    /// LINE 群組加入中繼頁：記錄用戶點擊後再跳轉到真正的 LINE 連結
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> JoinLine(string? ref_code)
    {
        var anonId = HttpContext.Request.Cookies["DotNetLearner"] ?? "";
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);

        // 記錄點擊（用 AuditLog）
        try
        {
            _db.AuditLogs.Add(new DotNetLearning.Models.AuditLog
            {
                UserId = anonId,
                UserName = user?.Nickname ?? "訪客",
                UserRole = user?.Role ?? "guest",
                Action = "JoinLINE",
                EntityType = "LINE",
                EntityId = 0,
                Details = $"ref={ref_code ?? "direct"}, ip={HttpContext.Connection.RemoteIpAddress}",
                CreatedAt = DateTime.Now
            });
            await _db.SaveChangesAsync();
        }
        catch { }

        ViewBag.User = user;
        ViewBag.RefCode = ref_code;
        return View();
    }
}
