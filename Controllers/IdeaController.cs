using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class IdeaController : Controller
{
    private readonly AppDbContext _db;
    public IdeaController(AppDbContext db) => _db = db;

    // 取得某章節的想法
    [HttpGet]
    public async Task<IActionResult> ByChapter(int chapterId, int take = 20)
    {
        var ideas = await _db.Ideas
            .Where(i => i.ChapterId == chapterId)
            .OrderByDescending(i => i.Likes)
            .ThenByDescending(i => i.CreatedAt)
            .Take(take)
            .Select(i => new {
                i.Id, i.Nickname, i.Content, i.Likes, i.CreatedAt,
                timeAgo = FormatTimeAgo(i.CreatedAt),
                replyCount = _db.Replies.Count(r => r.IdeaId == i.Id)
            })
            .ToListAsync();
        return Json(ideas);
    }

    // 取得所有想法（首頁知識牆）
    [HttpGet]
    public async Task<IActionResult> Wall(int take = 30)
    {
        var ideas = await _db.Ideas
            .OrderByDescending(i => i.CreatedAt)
            .Take(take)
            .Select(i => new {
                i.Id, i.Nickname, i.Content, i.Likes, i.ChapterTitle, i.CreatedAt,
                timeAgo = FormatTimeAgo(i.CreatedAt),
                replyCount = _db.Replies.Count(r => r.IdeaId == i.Id)
            })
            .ToListAsync();
        return Json(ideas);
    }

    // 發布想法
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostIdeaRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Content))
            return BadRequest(new { error = "內容不能為空" });

        if (req.Content.Length > 1000)
            return BadRequest(new { error = "內容不能超過 1000 字" });

        var sessionId = HttpContext.Session.GetString("SessionId");
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("SessionId", sessionId);
        }

        // 查暱稱
        var nickname = req.Nickname;
        if (string.IsNullOrWhiteSpace(nickname))
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
            nickname = profile?.Nickname ?? "匿名學習者";
        }

        // 查章節名稱
        var chapterTitle = "";
        if (req.ChapterId.HasValue)
        {
            var ch = await _db.Chapters.FindAsync(req.ChapterId.Value);
            chapterTitle = ch?.Title ?? "";
        }

        var idea = new Idea
        {
            SessionId = sessionId,
            Nickname = nickname,
            ChapterId = req.ChapterId,
            ChapterTitle = chapterTitle,
            Content = req.Content.Trim(),
            CreatedAt = DateTime.Now
        };

        _db.Ideas.Add(idea);
        await _db.SaveChangesAsync();

        return Json(new { success = true, idea.Id, idea.Nickname, timeAgo = "剛剛" });
    }

    // 按讚
    [HttpPost]
    public async Task<IActionResult> Like([FromBody] LikeRequest req)
    {
        var idea = await _db.Ideas.FindAsync(req.IdeaId);
        if (idea == null) return NotFound();
        idea.Likes++;
        await _db.SaveChangesAsync();
        return Json(new { likes = idea.Likes });
    }

    // 取得某想法的回覆
    [HttpGet]
    public async Task<IActionResult> GetReplies(int ideaId)
    {
        var replies = await _db.Replies
            .Where(r => r.IdeaId == ideaId)
            .OrderBy(r => r.CreatedAt)
            .Select(r => new { r.Id, r.Nickname, r.Content, timeAgo = FormatTimeAgo(r.CreatedAt) })
            .ToListAsync();
        return Json(replies);
    }

    // 發布回覆
    [HttpPost]
    public async Task<IActionResult> PostReply([FromBody] PostReplyRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Content))
            return BadRequest(new { error = "內容不能為空" });

        var sessionId = HttpContext.Session.GetString("SessionId") ?? Guid.NewGuid().ToString();
        HttpContext.Session.SetString("SessionId", sessionId);

        var nickname = req.Nickname;
        if (string.IsNullOrWhiteSpace(nickname))
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
            nickname = profile?.Nickname ?? "匿名";
        }

        var reply = new Reply
        {
            IdeaId = req.IdeaId,
            SessionId = sessionId,
            Nickname = nickname,
            Content = req.Content.Trim(),
            CreatedAt = DateTime.Now
        };
        _db.Replies.Add(reply);
        await _db.SaveChangesAsync();

        return Json(new { success = true, reply.Id, reply.Nickname, timeAgo = "剛剛" });
    }

    private static string FormatTimeAgo(DateTime dt)
    {
        var diff = DateTime.Now - dt;
        if (diff.TotalMinutes < 1) return "剛剛";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} 分鐘前";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} 小時前";
        if (diff.TotalDays < 30) return $"{(int)diff.TotalDays} 天前";
        return dt.ToString("yyyy/MM/dd");
    }
}

public record PostIdeaRequest(string? Nickname, int? ChapterId, string Content);
public record LikeRequest(int IdeaId);
public record PostReplyRequest(int IdeaId, string? Nickname, string Content);
