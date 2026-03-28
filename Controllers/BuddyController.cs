using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;
using System.Text.Json;

namespace DotNetLearning.Controllers;

public class BuddyController : Controller
{
    private readonly AppDbContext _db;
    public BuddyController(AppDbContext db) => _db = db;

    private string GetSessionId()
    {
        var sid = HttpContext.Session.GetString("SessionId");
        if (string.IsNullOrEmpty(sid))
        {
            sid = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("SessionId", sid);
        }
        return sid;
    }

    // ── 主頁面 ──
    public IActionResult Index()
    {
        ViewData["Title"] = "學習夥伴配對";
        return View();
    }

    // ── 註冊 ──
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] BuddyRegisterRequest req)
    {
        var sessionId = GetSessionId();

        // 已註冊？
        var existing = await _db.StudyBuddies.FirstOrDefaultAsync(b => b.SessionId == sessionId);
        if (existing != null)
            return Json(new { success = false, message = "你已經註冊過了" });

        var buddy = new StudyBuddy
        {
            SessionId = sessionId,
            Nickname = req.Nickname?.Trim() ?? "匿名學伴",
            Level = req.Level ?? "beginner",
            InterestsJson = JsonSerializer.Serialize(req.Interests ?? new List<string>()),
            Goal = req.Goal?.Trim() ?? "",
            ContactInfo = req.ContactInfo?.Trim() ?? "",
            IsLookingForBuddy = true,
            RegisteredAt = DateTime.Now
        };

        _db.StudyBuddies.Add(buddy);
        await _db.SaveChangesAsync();

        // 自動配對
        var matches = await AutoMatch(buddy);

        return Json(new
        {
            success = true,
            profile = MapBuddyProfile(buddy),
            matches = matches
        });
    }

    // ── 尋找配對 ──
    [HttpGet]
    public async Task<IActionResult> FindMatches()
    {
        var sessionId = GetSessionId();
        var me = await _db.StudyBuddies.FirstOrDefaultAsync(b => b.SessionId == sessionId);
        if (me == null)
            return Json(new List<object>());

        var myInterests = JsonSerializer.Deserialize<List<string>>(me.InterestsJson) ?? new();

        var candidates = await _db.StudyBuddies
            .Where(b => b.SessionId != sessionId && b.IsLookingForBuddy)
            .ToListAsync();

        // 已配對的 buddy IDs
        var matchedIds = await GetMatchedBuddyIds(me.Id);

        var results = candidates
            .Where(c => !matchedIds.Contains(c.Id))
            .Where(c =>
            {
                if (c.Level == me.Level) return true;
                var theirInterests = JsonSerializer.Deserialize<List<string>>(c.InterestsJson) ?? new();
                return myInterests.Intersect(theirInterests).Any();
            })
            .Select(c => MapBuddyPublic(c))
            .ToList();

        return Json(results);
    }

    // ── 我的個人資料 ──
    [HttpGet]
    public async Task<IActionResult> MyProfile()
    {
        var sessionId = GetSessionId();
        var me = await _db.StudyBuddies.FirstOrDefaultAsync(b => b.SessionId == sessionId);
        if (me == null)
            return Json(new { registered = false });

        // 取得已配對的夥伴（含 contactInfo）
        var matchedIds = await GetMatchedBuddyIds(me.Id);
        var matchedBuddies = await _db.StudyBuddies
            .Where(b => matchedIds.Contains(b.Id))
            .ToListAsync();

        return Json(new
        {
            registered = true,
            profile = MapBuddyProfile(me),
            matches = matchedBuddies.Select(b => new
            {
                id = b.Id,
                nickname = b.Nickname,
                level = b.Level,
                interests = JsonSerializer.Deserialize<List<string>>(b.InterestsJson),
                goal = b.Goal,
                contactInfo = b.ContactInfo
            })
        });
    }

    // ── 更新個人資料 ──
    [HttpPost]
    public async Task<IActionResult> UpdateProfile([FromBody] BuddyRegisterRequest req)
    {
        var sessionId = GetSessionId();
        var me = await _db.StudyBuddies.FirstOrDefaultAsync(b => b.SessionId == sessionId);
        if (me == null)
            return Json(new { success = false, message = "尚未註冊" });

        if (!string.IsNullOrWhiteSpace(req.Nickname)) me.Nickname = req.Nickname.Trim();
        if (!string.IsNullOrWhiteSpace(req.Level)) me.Level = req.Level;
        if (req.Interests != null) me.InterestsJson = JsonSerializer.Serialize(req.Interests);
        if (req.Goal != null) me.Goal = req.Goal.Trim();
        if (req.ContactInfo != null) me.ContactInfo = req.ContactInfo.Trim();

        await _db.SaveChangesAsync();
        return Json(new { success = true, profile = MapBuddyProfile(me) });
    }

    // ── 所有正在找夥伴的人 ──
    [HttpGet]
    public async Task<IActionResult> AllBuddies(string level = "all")
    {
        var sessionId = GetSessionId();

        var query = _db.StudyBuddies
            .Where(b => b.IsLookingForBuddy && b.SessionId != sessionId);

        if (level != "all")
            query = query.Where(b => b.Level == level);

        var buddies = await query
            .OrderByDescending(b => b.RegisteredAt)
            .ToListAsync();

        return Json(buddies.Select(b => MapBuddyPublic(b)));
    }

    // ── 手動配對 ──
    [HttpPost]
    public async Task<IActionResult> MatchWith([FromBody] MatchRequest req)
    {
        var sessionId = GetSessionId();
        var me = await _db.StudyBuddies.FirstOrDefaultAsync(b => b.SessionId == sessionId);
        if (me == null)
            return Json(new { success = false, message = "請先註冊" });

        var target = await _db.StudyBuddies.FindAsync(req.BuddyId);
        if (target == null)
            return Json(new { success = false, message = "找不到對方" });

        // 檢查是否已配對
        var alreadyMatched = await _db.BuddyMatches.AnyAsync(m =>
            (m.BuddyAId == me.Id && m.BuddyBId == target.Id) ||
            (m.BuddyAId == target.Id && m.BuddyBId == me.Id));
        if (alreadyMatched)
            return Json(new { success = false, message = "你們已經配對了" });

        _db.BuddyMatches.Add(new BuddyMatch
        {
            BuddyAId = me.Id,
            BuddyBId = target.Id,
            MatchedAt = DateTime.Now,
            IsActive = true
        });
        await _db.SaveChangesAsync();

        return Json(new
        {
            success = true,
            matchedBuddy = new
            {
                id = target.Id,
                nickname = target.Nickname,
                level = target.Level,
                interests = JsonSerializer.Deserialize<List<string>>(target.InterestsJson),
                goal = target.Goal,
                contactInfo = target.ContactInfo
            }
        });
    }

    // ── 輔助方法 ──
    private async Task<List<object>> AutoMatch(StudyBuddy me)
    {
        var myInterests = JsonSerializer.Deserialize<List<string>>(me.InterestsJson) ?? new();
        var candidates = await _db.StudyBuddies
            .Where(b => b.Id != me.Id && b.IsLookingForBuddy)
            .ToListAsync();

        var matched = new List<object>();

        foreach (var c in candidates)
        {
            var theirInterests = JsonSerializer.Deserialize<List<string>>(c.InterestsJson) ?? new();
            bool sameLevel = c.Level == me.Level;
            bool overlap = myInterests.Intersect(theirInterests).Any();

            if (sameLevel || overlap)
            {
                // 避免重複配對
                var exists = await _db.BuddyMatches.AnyAsync(m =>
                    (m.BuddyAId == me.Id && m.BuddyBId == c.Id) ||
                    (m.BuddyAId == c.Id && m.BuddyBId == me.Id));
                if (exists) continue;

                _db.BuddyMatches.Add(new BuddyMatch
                {
                    BuddyAId = me.Id,
                    BuddyBId = c.Id,
                    MatchedAt = DateTime.Now,
                    IsActive = true
                });

                matched.Add(new
                {
                    id = c.Id,
                    nickname = c.Nickname,
                    level = c.Level,
                    interests = theirInterests,
                    goal = c.Goal,
                    contactInfo = c.ContactInfo
                });

                if (matched.Count >= 3) break; // 最多自動配 3 人
            }
        }

        if (matched.Count > 0)
            await _db.SaveChangesAsync();

        return matched;
    }

    private async Task<HashSet<int>> GetMatchedBuddyIds(int myId)
    {
        var matchedA = await _db.BuddyMatches
            .Where(m => m.BuddyAId == myId && m.IsActive)
            .Select(m => m.BuddyBId)
            .ToListAsync();
        var matchedB = await _db.BuddyMatches
            .Where(m => m.BuddyBId == myId && m.IsActive)
            .Select(m => m.BuddyAId)
            .ToListAsync();
        return matchedA.Concat(matchedB).ToHashSet();
    }

    private static object MapBuddyProfile(StudyBuddy b) => new
    {
        id = b.Id,
        nickname = b.Nickname,
        level = b.Level,
        interests = JsonSerializer.Deserialize<List<string>>(b.InterestsJson),
        goal = b.Goal,
        contactInfo = b.ContactInfo,
        isLookingForBuddy = b.IsLookingForBuddy,
        registeredAt = b.RegisteredAt
    };

    private static object MapBuddyPublic(StudyBuddy b) => new
    {
        id = b.Id,
        nickname = b.Nickname,
        level = b.Level,
        interests = JsonSerializer.Deserialize<List<string>>(b.InterestsJson),
        goal = b.Goal
        // contactInfo 不公開
    };
}

public record BuddyRegisterRequest(
    string? Nickname,
    string? Level,
    List<string>? Interests,
    string? Goal,
    string? ContactInfo
);

public record MatchRequest(int BuddyId);
