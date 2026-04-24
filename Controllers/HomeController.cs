using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using DotNetLearning.Data;
using DotNetLearning.Models;
using DotNetLearning.Filters;
using Markdig;

namespace DotNetLearning.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly IMemoryCache _cache;
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public HomeController(AppDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    // 側欄 / 首頁共用的 nav cache：只撈渲染需要的欄位 + 3 語言標題（Content 本文不進 cache）
    private Task<List<Chapter>?> GetNavChaptersAsync() =>
        _cache.GetOrCreateAsync("chapters:nav", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _db.Chapters
                .AsNoTracking()
                .Where(c => c.IsPublished)
                .OrderBy(c => c.Order)
                .Select(c => new Chapter
                {
                    Id = c.Id, Title = c.Title, Slug = c.Slug,
                    Category = c.Category, Order = c.Order,
                    Icon = c.Icon, Level = c.Level, IsPublished = c.IsPublished,
                    TitleJa = c.TitleJa, TitleEn = c.TitleEn,  // 側欄用來切語言標題
                })
                .ToListAsync();
        });

    public async Task<IActionResult> Index()
    {
        var chapters = await GetNavChaptersAsync();

        // 首頁也讀 cookie lang 讓卡片標題、首屏核心路線標題跟上
        var lang = Request.Query["lang"].ToString();
        if (string.IsNullOrEmpty(lang)) lang = Request.Cookies["lang"] ?? "zh";
        ViewBag.CurrentLang = lang;

        var now = DateTime.Now;
        ViewBag.Announcements = await _db.Announcements
            .AsNoTracking()
            .Where(a => a.IsVisible && (a.ExpiresAt == null || a.ExpiresAt > now))
            .OrderByDescending(a => a.IsPinned)
            .ThenByDescending(a => a.CreatedAt)
            .Take(5)
            .ToListAsync();

        return View(chapters);
    }

    [Route("Home/Chapter/{slug}")]
    public async Task<IActionResult> Chapter(string slug)
    {
        // Load chapter with questions (only this chapter, not all)
        var chapter = await _db.Chapters
            .AsNoTracking()
            .Include(c => c.Questions)
            .FirstOrDefaultAsync(c => c.Slug == slug && c.IsPublished);

        if (chapter is null) return NotFound();

        // ─── 語言切換：?lang=ja/en/zh 或 cookie "lang"，fallback 到繁中 ───
        var lang = Request.Query["lang"].ToString();
        if (string.IsNullOrEmpty(lang)) lang = Request.Cookies["lang"] ?? "zh";
        string? transTitle = null, transContent = null;
        if (lang == "ja") { transTitle = chapter.TitleJa; transContent = chapter.ContentJa; }
        else if (lang == "en") { transTitle = chapter.TitleEn; transContent = chapter.ContentEn; }
        var hasTranslation = !string.IsNullOrWhiteSpace(transContent);
        var effectiveLang = hasTranslation ? lang : "zh";   // 沒翻譯 → 走繁中
        var displayTitle = !string.IsNullOrWhiteSpace(transTitle) ? transTitle! : chapter.Title;
        var displayContent = hasTranslation ? transContent! : chapter.Content;

        // Session ID for progress tracking
        var sessionId = HttpContext.Session.GetString("SessionId");
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("SessionId", sessionId);
        }

        // Cache sidebar chapter list (共用 GetNavChaptersAsync 避免跟 Index 重複寫)
        var allChapters = await GetNavChaptersAsync();

        var completedIds = await _db.Progresses
            .AsNoTracking()
            .Where(p => p.SessionId == sessionId && p.IsCompleted)
            .Select(p => p.ChapterId)
            .ToListAsync();

        // Total progress for navbar badge
        var totalCount = allChapters!.Count;
        var doneCount = completedIds.Count;

        // 最佳成績
        var bestAttempt = await _db.QuizAttempts
            .AsNoTracking()
            .Where(a => a.SessionId == sessionId && a.ChapterId == chapter.Id && a.Total > 0)
            .OrderByDescending(a => a.Score * 100 / a.Total)
            .FirstOrDefaultAsync();

        // Cache rendered Markdown HTML per chapter/lang (expensive for large content)
        var cacheKey = effectiveLang == "zh"
            ? $"chapter:html:{chapter.Id}"
            : $"chapter:html:{chapter.Id}:{effectiveLang}";
        var contentHtml = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return Markdown.ToHtml(displayContent, _pipeline);
        });

        ViewBag.AllChapters = allChapters;
        ViewBag.CompletedChapters = completedIds;
        ViewBag.ContentHtml = contentHtml;
        ViewBag.DisplayTitle = displayTitle;
        ViewBag.CurrentLang = effectiveLang;
        ViewBag.HasJa = !string.IsNullOrWhiteSpace(chapter.ContentJa);
        ViewBag.HasEn = !string.IsNullOrWhiteSpace(chapter.ContentEn);
        ViewBag.HasQuiz = chapter.Questions.Any();
        ViewBag.QuestionCount = chapter.Questions.Count;
        ViewBag.SessionId = sessionId;
        ViewBag.TotalCount = totalCount;
        ViewBag.DoneCount = doneCount;
        ViewBag.BestScore = bestAttempt != null ? (int)(bestAttempt.Score * 100.0 / bestAttempt.Total) : -1;

        // ─── SEO per-page ───
        ViewData["Title"] = displayTitle;
        // Strip markdown to first 150 chars for description
        var plain = System.Text.RegularExpressions.Regex.Replace(displayContent ?? "", @"[#*`\[\]_>\-\n\r]+", " ");
        plain = System.Text.RegularExpressions.Regex.Replace(plain, @"\s+", " ").Trim();
        if (plain.Length > 155) plain = plain.Substring(0, 155) + "…";
        var fallback = effectiveLang switch
        {
            "ja" => $"DevLearn {displayTitle} — {chapter.Category} {chapter.Level} 無料日本語チュートリアル",
            "en" => $"DevLearn {displayTitle} — {chapter.Category} {chapter.Level} free English tutorial",
            _    => $"DevLearn {displayTitle} — {chapter.Category} {chapter.Level} 免費中文教學",
        };
        ViewData["Description"] = string.IsNullOrWhiteSpace(plain) ? fallback : plain;
        ViewData["Keywords"] = effectiveLang switch
        {
            "ja" => $"{displayTitle}, {chapter.Category}, {chapter.Level}, .NET チュートリアル, プログラミング, 日本語",
            "en" => $"{displayTitle}, {chapter.Category}, {chapter.Level}, .NET tutorial, programming, English",
            _    => $"{displayTitle}, {chapter.Category}, {chapter.Level}, .NET 教學, 程式教學, 中文",
        };
        ViewData["OgType"] = "article";

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
    [RequireRegistration]
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
            .AsNoTracking()
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
