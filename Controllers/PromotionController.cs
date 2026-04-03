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
}
