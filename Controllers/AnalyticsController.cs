using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;

namespace DotNetLearning.Controllers;

public class AnalyticsController : Controller
{
    private readonly AppDbContext _db;

    private static readonly Dictionary<string, string> CategoryNames = new()
    {
        ["csharp"] = "C# 基礎",
        ["aspnet"] = "ASP.NET Core",
        ["database"] = "資料庫",
        ["network"] = "網路",
        ["security"] = "資安",
        ["docker"] = "Docker",
        ["patterns"] = "設計模式",
        ["infrastructure"] = "基礎設施",
        ["ai"] = "AI 應用",
        ["frontend"] = "前端",
        ["git"] = "Git",
        ["server"] = "Server",
        ["aimodel"] = "AI 模型",
        ["claudecode"] = "Claude Code",
        ["project"] = "專案實戰",
        ["interview"] = "面試"
    };

    public AnalyticsController(AppDbContext db) => _db = db;

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

    public IActionResult Index()
    {
        ViewData["Title"] = "學習分析";
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Heatmap(int? year)
    {
        var sessionId = GetSessionId();
        var targetYear = year ?? DateTime.Now.Year;
        var startDate = new DateTime(targetYear, 1, 1);
        var endDate = new DateTime(targetYear, 12, 31);

        // Query StudyLogs grouped by date
        var studyByDate = await _db.StudyLogs
            .Where(s => s.SessionId == sessionId && s.LogDate >= startDate && s.LogDate <= endDate)
            .GroupBy(s => s.LogDate.Date)
            .Select(g => new
            {
                Date = g.Key,
                Minutes = g.Sum(x => x.MinutesSpent),
                Chapters = g.Sum(x => x.ChaptersViewed),
                Quizzes = g.Sum(x => x.QuizzesDone)
            })
            .ToDictionaryAsync(x => x.Date);

        // Fallback: QuizAttempts by date
        var quizByDate = await _db.QuizAttempts
            .Where(a => a.SessionId == sessionId && a.TakenAt >= startDate && a.TakenAt <= endDate)
            .GroupBy(a => a.TakenAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Date);

        // Fallback: Progress completions by date
        var progressByDate = await _db.Progresses
            .Where(p => p.SessionId == sessionId && p.IsCompleted && p.CompletedAt >= startDate && p.CompletedAt <= endDate)
            .GroupBy(p => p.CompletedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Date);

        var result = new List<object>();
        for (var d = startDate; d <= endDate; d = d.AddDays(1))
        {
            int activityScore = 0;

            if (studyByDate.TryGetValue(d, out var study))
            {
                activityScore += study.Minutes + study.Chapters * 5 + study.Quizzes * 10;
            }

            if (quizByDate.TryGetValue(d, out var quiz))
            {
                activityScore += quiz.Count * 10;
            }

            if (progressByDate.TryGetValue(d, out var prog))
            {
                activityScore += prog.Count * 5;
            }

            int level = activityScore switch
            {
                0 => 0,
                < 10 => 1,
                < 30 => 2,
                < 60 => 3,
                _ => 4
            };

            result.Add(new { date = d.ToString("yyyy-MM-dd"), level });
        }

        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> WeakPoints()
    {
        var sessionId = GetSessionId();

        // Get quiz attempts with chapter info
        var attempts = await _db.QuizAttempts
            .Where(a => a.SessionId == sessionId && a.Total > 0)
            .Join(_db.Chapters,
                a => a.ChapterId,
                c => c.Id,
                (a, c) => new { a.Score, a.Total, c.Category })
            .ToListAsync();

        var grouped = attempts
            .GroupBy(x => x.Category)
            .Select(g =>
            {
                var totalQ = g.Sum(x => x.Total);
                var correctQ = g.Sum(x => x.Score);
                var accuracy = totalQ > 0 ? Math.Round(correctQ * 100.0 / totalQ, 1) : 0;
                return new
                {
                    category = g.Key,
                    categoryName = CategoryNames.GetValueOrDefault(g.Key, g.Key),
                    accuracy,
                    totalQuestions = totalQ,
                    correctCount = correctQ
                };
            })
            .OrderBy(x => x.accuracy)
            .ToList();

        return Json(grouped);
    }

    [HttpGet]
    public async Task<IActionResult> Stats()
    {
        var sessionId = GetSessionId();
        var now = DateTime.Now;

        // Total study minutes
        var totalMinutes = await _db.StudyLogs
            .Where(s => s.SessionId == sessionId)
            .SumAsync(s => (int?)s.MinutesSpent) ?? 0;

        // Total chapters completed
        var totalChapters = await _db.Progresses
            .CountAsync(p => p.SessionId == sessionId && p.IsCompleted);

        // Total quizzes taken
        var totalQuizzes = await _db.QuizAttempts
            .CountAsync(a => a.SessionId == sessionId);

        // Average quiz score
        var quizStats = await _db.QuizAttempts
            .Where(a => a.SessionId == sessionId && a.Total > 0)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                TotalScore = g.Sum(a => a.Score),
                TotalQuestions = g.Sum(a => a.Total)
            })
            .FirstOrDefaultAsync();

        var avgScore = quizStats != null && quizStats.TotalQuestions > 0
            ? Math.Round(quizStats.TotalScore * 100.0 / quizStats.TotalQuestions, 1)
            : 0;

        // Current streak: consecutive days with activity ending today or yesterday
        var activityDates = await _db.StudyLogs
            .Where(s => s.SessionId == sessionId)
            .Select(s => s.LogDate.Date)
            .Union(_db.QuizAttempts
                .Where(a => a.SessionId == sessionId)
                .Select(a => a.TakenAt.Date))
            .Union(_db.Progresses
                .Where(p => p.SessionId == sessionId && p.IsCompleted)
                .Select(p => p.CompletedAt.Date))
            .Distinct()
            .OrderByDescending(d => d)
            .ToListAsync();

        int streak = 0;
        var checkDate = now.Date;
        // Allow streak to start from today or yesterday
        if (activityDates.Count > 0 && activityDates[0] < checkDate.AddDays(-1))
        {
            streak = 0;
        }
        else
        {
            foreach (var d in activityDates)
            {
                if (d == checkDate || d == checkDate.AddDays(-1))
                {
                    streak++;
                    checkDate = d.AddDays(-1);
                }
                else
                {
                    break;
                }
            }
        }

        // Study days count (unique days with activity)
        var studyDays = activityDates.Count;

        // This week's activity
        var weekStart = now.Date.AddDays(-(int)now.DayOfWeek);
        var weekMinutes = await _db.StudyLogs
            .Where(s => s.SessionId == sessionId && s.LogDate >= weekStart)
            .SumAsync(s => (int?)s.MinutesSpent) ?? 0;
        var weekChapters = await _db.Progresses
            .CountAsync(p => p.SessionId == sessionId && p.IsCompleted && p.CompletedAt >= weekStart);
        var weekQuizzes = await _db.QuizAttempts
            .CountAsync(a => a.SessionId == sessionId && a.TakenAt >= weekStart);

        return Json(new
        {
            totalMinutes,
            totalChapters,
            totalQuizzes,
            avgScore,
            streak,
            studyDays,
            week = new
            {
                minutes = weekMinutes,
                chapters = weekChapters,
                quizzes = weekQuizzes
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> LogActivity()
    {
        var sessionId = GetSessionId();
        var today = DateTime.Now.Date;

        var log = await _db.StudyLogs
            .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.LogDate == today);

        if (log == null)
        {
            log = new Models.StudyLog
            {
                SessionId = sessionId,
                LogDate = today,
                MinutesSpent = 1,
                ChaptersViewed = 1,
                QuizzesDone = 0
            };
            _db.StudyLogs.Add(log);
        }
        else
        {
            log.MinutesSpent += 1;
            log.ChaptersViewed += 1;
        }

        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }
}
