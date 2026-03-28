using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class CheckInController : Controller
{
    private readonly AppDbContext _db;
    public CheckInController(AppDbContext db) => _db = db;

    // ── 成就定義 ──────────────────────────────────────
    private static readonly List<(string Key, string Title, string Icon, string Desc)> AllAchievements = new()
    {
        ("first_login",   "初次登入", "🎯", "歡迎來到學習平台"),
        ("streak_3",      "三日連續", "🔥", "連續簽到 3 天"),
        ("streak_7",      "一週全勤", "⭐", "連續簽到 7 天"),
        ("streak_30",     "月度達人", "👑", "連續簽到 30 天"),
        ("first_chapter", "初探者",   "📖", "完成第一個章節"),
        ("chapter_10",    "學習家",   "📚", "完成 10 個章節"),
        ("chapter_50",    "知識王",   "🏆", "完成 50 個章節"),
        ("quiz_first",    "初試身手", "🎯", "完成第一次測驗"),
        ("quiz_10",       "測驗達人", "💪", "完成 10 次測驗"),
        ("quiz_100",      "百題斬",   "⚔️", "完成 100 次測驗"),
        ("perfect_score", "完美滿分", "💯", "測驗獲得滿分"),
        ("score_500",     "積分新星", "✨", "累積 500 積分"),
        ("score_2000",    "積分大師", "🌟", "累積 2000 積分"),
        ("idea_first",    "思想家",   "💡", "發布第一則想法"),
    };

    // ── 頁面 ──────────────────────────────────────────
    public IActionResult Index()
    {
        ViewData["Title"] = "簽到 & 成就";
        return View();
    }

    // ── 簽到 ──────────────────────────────────────────
    [HttpPost]
    public async Task<IActionResult> DoCheckIn()
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        if (string.IsNullOrEmpty(sessionId))
            return Json(new { success = false, message = "請先設定暱稱" });

        var today = DateTime.Today;

        // 已簽到？
        var already = await _db.CheckIns
            .AnyAsync(c => c.SessionId == sessionId && c.CheckInDate == today);
        if (already)
            return Json(new { alreadyDone = true });

        // 計算連續天數
        int streak = 1;
        var yesterday = today.AddDays(-1);
        var prev = await _db.CheckIns
            .Where(c => c.SessionId == sessionId && c.CheckInDate == yesterday)
            .FirstOrDefaultAsync();
        if (prev != null)
            streak = prev.Streak + 1;

        // 計算積分: base 10 + streak bonus (streak * 5, max 50)
        int streakBonus = Math.Min(streak * 5, 50);
        int points = 10 + streakBonus;

        // 7 天連續額外 100
        if (streak == 7)
            points += 100;

        // 寫入簽到記錄
        _db.CheckIns.Add(new CheckIn
        {
            SessionId = sessionId,
            CheckInDate = today,
            Streak = streak,
            BonusPoints = points
        });

        // 更新 UserProfile 積分
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        if (profile != null)
        {
            profile.TotalScore += points;
            profile.LastActiveAt = DateTime.Now;
        }

        await _db.SaveChangesAsync();

        // 檢查成就
        var newAchievements = await CheckAndUnlockAchievements(sessionId, streak);

        return Json(new { success = true, streak, points, newAchievements });
    }

    // ── 狀態 ──────────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> Status()
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        if (string.IsNullOrEmpty(sessionId))
            return Json(new { checkedInToday = false, streak = 0, totalCheckIns = 0, todayPoints = 0 });

        var today = DateTime.Today;
        var todayRecord = await _db.CheckIns
            .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.CheckInDate == today);

        int streak = 0;
        if (todayRecord != null)
        {
            streak = todayRecord.Streak;
        }
        else
        {
            // 看昨天的 streak
            var yesterday = today.AddDays(-1);
            var prev = await _db.CheckIns
                .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.CheckInDate == yesterday);
            if (prev != null)
                streak = prev.Streak;
        }

        var totalCheckIns = await _db.CheckIns.CountAsync(c => c.SessionId == sessionId);
        int todayPoints = todayRecord?.BonusPoints ?? 0;

        return Json(new
        {
            checkedInToday = todayRecord != null,
            streak,
            totalCheckIns,
            todayPoints
        });
    }

    // ── 月曆 ──────────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> Calendar(int year, int month)
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";

        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1);

        var records = await _db.CheckIns
            .Where(c => c.SessionId == sessionId && c.CheckInDate >= startDate && c.CheckInDate < endDate)
            .Select(c => new { date = c.CheckInDate.ToString("yyyy-MM-dd"), c.Streak })
            .ToListAsync();

        return Json(records);
    }

    // ── 我的成就 ──────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> MyAchievements()
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";

        var unlocked = await _db.Achievements
            .Where(a => a.SessionId == sessionId)
            .Select(a => new { a.AchievementKey, a.Title, a.Icon, a.Description, a.UnlockedAt })
            .ToListAsync();

        var unlockedKeys = unlocked.Select(a => a.AchievementKey).ToHashSet();

        var all = AllAchievements.Select(a => new
        {
            key = a.Key,
            title = a.Title,
            icon = a.Icon,
            description = a.Desc,
            unlocked = unlockedKeys.Contains(a.Key),
            unlockedAt = unlocked.FirstOrDefault(u => u.AchievementKey == a.Key)?.UnlockedAt
        });

        return Json(all);
    }

    // ── 內部：檢查並解鎖成就 ──────────────────────────
    private async Task<List<object>> CheckAndUnlockAchievements(string sessionId, int streak)
    {
        var existing = await _db.Achievements
            .Where(a => a.SessionId == sessionId)
            .Select(a => a.AchievementKey)
            .ToListAsync();
        var existingSet = existing.ToHashSet();

        var newOnes = new List<object>();

        // first_login
        await TryUnlock("first_login", true);

        // streak
        await TryUnlock("streak_3", streak >= 3);
        await TryUnlock("streak_7", streak >= 7);
        await TryUnlock("streak_30", streak >= 30);

        // 章節完成數
        var chaptersCompleted = await _db.Progresses
            .CountAsync(p => p.SessionId == sessionId && p.IsCompleted);
        await TryUnlock("first_chapter", chaptersCompleted >= 1);
        await TryUnlock("chapter_10", chaptersCompleted >= 10);
        await TryUnlock("chapter_50", chaptersCompleted >= 50);

        // 測驗次數
        var quizCount = await _db.QuizAttempts.CountAsync(q => q.SessionId == sessionId);
        await TryUnlock("quiz_first", quizCount >= 1);
        await TryUnlock("quiz_10", quizCount >= 10);
        await TryUnlock("quiz_100", quizCount >= 100);

        // 完美滿分
        var hasPerfect = await _db.QuizAttempts
            .AnyAsync(q => q.SessionId == sessionId && q.Score == q.Total && q.Total > 0);
        await TryUnlock("perfect_score", hasPerfect);

        // 積分
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        int totalScore = profile?.TotalScore ?? 0;
        await TryUnlock("score_500", totalScore >= 500);
        await TryUnlock("score_2000", totalScore >= 2000);

        // 想法
        var ideaCount = await _db.Ideas.CountAsync(i => i.SessionId == sessionId);
        await TryUnlock("idea_first", ideaCount >= 1);

        if (newOnes.Count > 0)
            await _db.SaveChangesAsync();

        return newOnes;

        // ── local function ──
        async Task TryUnlock(string key, bool condition)
        {
            if (!condition || existingSet.Contains(key)) return;
            var def = AllAchievements.FirstOrDefault(a => a.Key == key);
            if (def == default) return;

            _db.Achievements.Add(new Achievement
            {
                SessionId = sessionId,
                AchievementKey = key,
                Title = def.Title,
                Icon = def.Icon,
                Description = def.Desc,
                UnlockedAt = DateTime.Now
            });
            existingSet.Add(key);
            newOnes.Add(new { key, title = def.Title, icon = def.Icon, description = def.Desc });
        }
    }
}
