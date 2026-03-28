using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;
using Markdig;

namespace DotNetLearning.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var chapters = await _db.Chapters
            .Where(c => c.IsPublished)
            .OrderBy(c => c.Order)
            .ToListAsync();
        return View(chapters);
    }

    [Route("Home/Chapter/{slug}")]
    public async Task<IActionResult> Chapter(string slug)
    {
        var chapter = await _db.Chapters
            .Include(c => c.Questions)
            .FirstOrDefaultAsync(c => c.Slug == slug && c.IsPublished);

        if (chapter is null) return NotFound();

        // Session ID for progress tracking
        var sessionId = HttpContext.Session.GetString("SessionId");
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("SessionId", sessionId);
        }

        var allChapters = await _db.Chapters
            .Where(c => c.IsPublished)
            .OrderBy(c => c.Order)
            .ToListAsync();

        var completedIds = await _db.Progresses
            .Where(p => p.SessionId == sessionId && p.IsCompleted)
            .Select(p => p.ChapterId)
            .ToListAsync();

        // Total progress for navbar badge
        var totalCount = allChapters.Count;
        var doneCount = completedIds.Count;

        // 最佳成績
        var bestAttempt = await _db.QuizAttempts
            .Where(a => a.SessionId == sessionId && a.ChapterId == chapter.Id && a.Total > 0)
            .OrderByDescending(a => a.Score * 100 / a.Total)
            .FirstOrDefaultAsync();

        ViewBag.AllChapters = allChapters;
        ViewBag.CompletedChapters = completedIds;
        ViewBag.ContentHtml = Markdown.ToHtml(chapter.Content, _pipeline);
        ViewBag.HasQuiz = chapter.Questions.Any();
        ViewBag.QuestionCount = chapter.Questions.Count;
        ViewBag.SessionId = sessionId;
        ViewBag.TotalCount = totalCount;
        ViewBag.DoneCount = doneCount;
        ViewBag.BestScore = bestAttempt != null ? (int)(bestAttempt.Score * 100.0 / bestAttempt.Total) : -1;

        return View(chapter);
    }

    public async Task<IActionResult> ProgressSummary()
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        var total = await _db.Chapters.CountAsync(c => c.IsPublished);
        var done = await _db.Progresses.CountAsync(p => p.SessionId == sessionId && p.IsCompleted);
        return Json(new { total, done });
    }

    [HttpPost]
    public async Task<IActionResult> MarkComplete([FromBody] MarkCompleteRequest req)
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? req.SessionId;

        var existing = await _db.Progresses
            .FirstOrDefaultAsync(p => p.SessionId == sessionId && p.ChapterId == req.ChapterId);

        if (existing is null)
        {
            _db.Progresses.Add(new Progress
            {
                SessionId = sessionId,
                ChapterId = req.ChapterId,
                IsCompleted = true,
                CompletedAt = DateTime.Now
            });
            await _db.SaveChangesAsync();
        }

        return Ok();
    }

    // 動態分類 API（供 Admin 新增章節用）
    [HttpGet]
    public async Task<IActionResult> Categories()
    {
        var categories = await _db.Chapters
            .Select(c => c.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
        return Json(categories);
    }

    public async Task<IActionResult> Search(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Json(new List<object>());

        var lower = q.ToLower();
        var results = await _db.Chapters
            .Where(c => c.IsPublished &&
                        (c.Title.ToLower().Contains(lower) || c.Content.ToLower().Contains(lower)))
            .OrderBy(c => c.Order)
            .Select(c => new { c.Slug, c.Title, c.Icon, c.Category })
            .Take(10)
            .ToListAsync();

        var categoryNames = new Dictionary<string, string>
        {
            ["csharp"]="C# 基礎", ["aspnet"]="ASP.NET Core", ["database"]="資料庫",
            ["network"]="網路", ["security"]="資安", ["docker"]="Docker",
            ["patterns"]="設計模式", ["infrastructure"]="基礎設施", ["ai"]="AI 應用",
            ["frontend"]="前端", ["git"]="Git", ["server"]="Server",
            ["aimodel"]="AI 模型", ["claudecode"]="Claude Code",
            ["project"]="專案實戰", ["interview"]="面試"
        };

        return Json(results.Select(r => new
        {
            r.Slug,
            r.Title,
            r.Icon,
            category = categoryNames.GetValueOrDefault(r.Category, r.Category)
        }));
    }
}

public record MarkCompleteRequest(string SessionId, int ChapterId);
