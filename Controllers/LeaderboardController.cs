using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class LeaderboardController : Controller
{
    private readonly AppDbContext _db;
    public LeaderboardController(AppDbContext db) => _db = db;

    private string GetSessionId()
    {
        var sid = HttpContext.Session.GetString("SessionId");
        if (string.IsNullOrEmpty(sid))
        {
            sid = Guid.NewGuid().ToString("N");
            HttpContext.Session.SetString("SessionId", sid);
        }
        return sid;
    }

    public async Task<IActionResult> Index()
    {
        var leaders = await _db.UserProfiles
            .OrderByDescending(u => u.TotalScore)
            .Take(50)
            .ToListAsync();
        return View(leaders);
    }

    [HttpPost]
    public async Task<IActionResult> SetNickname([FromBody] SetNicknameRequest req)
    {
        var sessionId = GetSessionId();
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        if (profile == null)
        {
            profile = new UserProfile { SessionId = sessionId, Nickname = req.Nickname };
            _db.UserProfiles.Add(profile);
        }
        else
        {
            profile.Nickname = req.Nickname;
        }
        await _db.SaveChangesAsync();
        return Ok(new { profile.Nickname, profile.TotalScore, profile.BadgeLevel });
    }

    [HttpGet]
    public async Task<IActionResult> MyProfile()
    {
        var sessionId = GetSessionId();
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        if (profile == null) return Json(new { exists = false });

        var rank = await _db.UserProfiles.CountAsync(u => u.TotalScore > profile.TotalScore) + 1;
        var totalUsers = await _db.UserProfiles.CountAsync();

        return Json(new {
            exists = true,
            profile.Nickname,
            profile.TotalScore,
            profile.QuizzesTaken,
            profile.ChaptersCompleted,
            profile.BadgeLevel,
            rank,
            totalUsers
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddScore([FromBody] AddScoreRequest req)
    {
        var sessionId = GetSessionId();
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        if (profile == null)
        {
            profile = new UserProfile { SessionId = sessionId, Nickname = "匿名學習者" };
            _db.UserProfiles.Add(profile);
        }

        int earnedPoints = req.CorrectCount * 10;
        if (req.CorrectCount == req.TotalCount && req.TotalCount > 0)
            earnedPoints += req.TotalCount * 5;

        profile.TotalScore += earnedPoints;
        profile.QuizzesTaken++;
        profile.LastActiveAt = DateTime.Now;

        profile.BadgeLevel = profile.TotalScore switch
        {
            >= 5000 => "master",
            >= 2000 => "expert",
            >= 1000 => "advanced",
            >= 500 => "intermediate",
            >= 100 => "beginner",
            _ => "newbie"
        };

        profile.ChaptersCompleted = await _db.Progresses
            .Where(p => p.SessionId == sessionId && p.IsCompleted)
            .CountAsync();

        // Sync scores to SiteUser table so Account/Me stays consistent
        var siteUser = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == sessionId);
        if (siteUser != null)
        {
            siteUser.TotalScore = profile.TotalScore;
            siteUser.QuizzesTaken = profile.QuizzesTaken;
            siteUser.ChaptersCompleted = profile.ChaptersCompleted;
            siteUser.BadgeLevel = profile.BadgeLevel;
            siteUser.LastActiveAt = DateTime.Now;
        }

        await _db.SaveChangesAsync();

        return Json(new {
            earnedPoints,
            profile.TotalScore,
            profile.BadgeLevel,
            isPerfect = req.CorrectCount == req.TotalCount
        });
    }
}

public record SetNicknameRequest(string Nickname);
public record AddScoreRequest(int CorrectCount, int TotalCount);
