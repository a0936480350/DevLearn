using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;

namespace DotNetLearning.Controllers;

/// <summary>
/// LifeQuest 衛星 APP（或其他未來整合方）要用的唯讀整合 API。
/// 所有端點放在 /api/integration/* 命名空間下，方便日後加 rate-limit / audit / versioning。
///
/// 身分識別約定：
///   - 主要靠 DevLearn 的 cookie  "DotNetLearner" = SiteUser.AnonymousId
///   - 或 query ?anonId=xxx 作為 Phase 1 過渡用的明文方式（之後會收斂掉）
/// </summary>
[ApiController]
[Route("api/integration")]
public class IntegrationController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<IntegrationController> _logger;

    public IntegrationController(AppDbContext db, ILogger<IntegrationController> logger)
    {
        _db = db;
        _logger = logger;
    }

    // ───────────────────────────────────────────────────────────
    //  GET /api/integration/me
    //  依 cookie（或 ?anonId=）解析目前 DevLearn 使用者
    //  回傳可安全公開給衛星服務的身分資料
    // ───────────────────────────────────────────────────────────
    [HttpGet("me")]
    public async Task<IActionResult> Me([FromQuery] string? anonId = null)
    {
        // 1. cookie 優先；若無，允許 ?anonId= 作為 Phase 1 的明文 fallback
        var candidate = Request.Cookies["DotNetLearner"];
        if (string.IsNullOrWhiteSpace(candidate))
            candidate = anonId;

        if (string.IsNullOrWhiteSpace(candidate))
        {
            return Unauthorized(new
            {
                error = "not_logged_in",
                message = "請先登入 DevLearn，或在 query string 帶 anonId=xxx（Phase 1 過渡）"
            });
        }

        var user = await _db.SiteUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.AnonymousId == candidate);

        if (user == null)
        {
            return Unauthorized(new
            {
                error = "user_not_found",
                message = "此 AnonymousId 找不到對應的 SiteUser"
            });
        }

        if (!user.IsRegistered)
        {
            // 允許 guest cookie 存在，但告知對方：尚未註冊
            return Ok(new
            {
                anonymousId = user.AnonymousId,
                nickname = user.Nickname,
                email = (string?)null,
                isRegistered = false,
                role = user.Role,
                emailVerified = false,
                loginMethod = user.LoginMethod,
                badgeLevel = user.BadgeLevel
            });
        }

        return Ok(new
        {
            anonymousId = user.AnonymousId,
            nickname = user.Nickname,
            email = user.Email,
            isRegistered = user.IsRegistered,
            role = user.Role,
            emailVerified = user.EmailVerified,
            loginMethod = user.LoginMethod,
            badgeLevel = user.BadgeLevel
        });
    }

    // ───────────────────────────────────────────────────────────
    //  GET /api/integration/chapters
    //  回傳已發布章節清單（LifeQuest 用來 sync 成 Quest）
    // ───────────────────────────────────────────────────────────
    [HttpGet("chapters")]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)] // 章節不常變，快取 5 分鐘
    public async Task<IActionResult> Chapters([FromQuery] bool publishedOnly = true)
    {
        var q = _db.Chapters.AsNoTracking();
        if (publishedOnly) q = q.Where(c => c.IsPublished);

        var chapters = await q
            .OrderBy(c => c.Category)
            .ThenBy(c => c.Order)
            .ThenBy(c => c.Id)
            .Select(c => new
            {
                id = c.Id,
                title = c.Title,
                slug = c.Slug,
                category = c.Category,
                level = c.Level,            // "beginner" / "intermediate" / "advanced"
                order = c.Order,
                icon = c.Icon,
                isPublished = c.IsPublished
            })
            .ToListAsync();

        return Ok(new
        {
            count = chapters.Count,
            chapters
        });
    }

    // ───────────────────────────────────────────────────────────
    //  GET /api/integration/chapters/progress
    //  回傳目前使用者的章節完成狀態
    //  資料源：Progresses 表（SessionId == AnonymousId）
    // ───────────────────────────────────────────────────────────
    [HttpGet("chapters/progress")]
    public async Task<IActionResult> ChaptersProgress([FromQuery] string? anonId = null)
    {
        var candidate = Request.Cookies["DotNetLearner"];
        if (string.IsNullOrWhiteSpace(candidate))
            candidate = anonId;

        if (string.IsNullOrWhiteSpace(candidate))
        {
            return Unauthorized(new { error = "not_logged_in" });
        }

        // Progress.SessionId 其實就是 AnonymousId（DevLearn 前台混用這個命名）
        var rows = await _db.Progresses
            .AsNoTracking()
            .Where(p => p.SessionId == candidate && p.IsCompleted)
            .ToListAsync();

        // 合併 QuizAttempt 最佳分數（若有）
        var quizScores = await _db.QuizAttempts
            .AsNoTracking()
            .Where(q => q.SessionId == candidate)
            .GroupBy(q => q.ChapterId)
            .Select(g => new
            {
                ChapterId = g.Key,
                BestScore = g.Max(x => x.Score),
                Total = g.Max(x => x.Total)
            })
            .ToDictionaryAsync(x => x.ChapterId);

        var result = rows
            .GroupBy(p => p.ChapterId)
            .Select(g =>
            {
                var latest = g.OrderByDescending(x => x.CompletedAt).First();
                quizScores.TryGetValue(g.Key, out var qs);
                return new
                {
                    chapterId = g.Key,
                    completed = true,
                    completedAt = latest.CompletedAt,
                    bestQuizScore = qs?.BestScore,
                    bestQuizTotal = qs?.Total
                };
            })
            .ToList();

        return Ok(new
        {
            anonymousId = candidate,
            totalCompleted = result.Count,
            items = result
        });
    }

    // ───────────────────────────────────────────────────────────
    //  GET /api/integration/health  — 偵測用，順便驗證 CORS
    // ───────────────────────────────────────────────────────────
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "ok", service = "DevLearn Integration API", version = "1.0" });
}
