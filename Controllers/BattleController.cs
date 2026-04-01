using Microsoft.AspNetCore.Mvc;
using DotNetLearning.Data;
using Microsoft.EntityFrameworkCore;

namespace DotNetLearning.Controllers;

public class BattleController : Controller
{
    private readonly AppDbContext _db;
    public BattleController(AppDbContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetString("SessionId") ?? "";
        var userName = HttpContext.Session.GetString("Nickname") ?? "訪客";
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

        ViewBag.UserId = userId;
        ViewBag.UserName = userName;

        var stat = await _db.BattleStats.FirstOrDefaultAsync(s => s.UserId == userId);
        ViewBag.MyStat = stat;

        // Leaderboard
        ViewBag.Leaderboard = await _db.BattleStats
            .OrderByDescending(s => s.BeginnerWins + s.IntermediateWins + s.AdvancedWins)
            .Take(10).ToListAsync();

        // Recent battles
        ViewBag.RecentBattles = await _db.BattleRecords
            .Where(r => r.Player1Id == userId || r.Player2Id == userId)
            .OrderByDescending(r => r.EndedAt)
            .Take(10).ToListAsync();

        return View();
    }

    public IActionResult Room()
    {
        var userId = HttpContext.Session.GetString("SessionId") ?? "";
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");
        return View();
    }

    public async Task<IActionResult> Leaderboard()
    {
        ViewBag.Stats = await _db.BattleStats
            .OrderByDescending(s => s.BeginnerWins + s.IntermediateWins * 2 + s.AdvancedWins * 3)
            .Take(50).ToListAsync();
        return View();
    }
}
