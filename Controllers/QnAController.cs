using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class QnAController : Controller
{
    private readonly AppDbContext _db;
    public QnAController(AppDbContext db) => _db = db;

    // 問答列表頁
    public IActionResult Index() => View();

    // 問題詳情頁
    public async Task<IActionResult> Detail(int id)
    {
        var q = await _db.QnAs.FindAsync(id);
        if (q == null) return NotFound();
        ViewBag.Question = q;
        return View();
    }

    // JSON API: 問題列表（篩選 + 分頁）
    [HttpGet]
    public async Task<IActionResult> List(string? category, bool? unsolved, int page = 1)
    {
        const int pageSize = 20;

        var query = _db.QnAs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(q => q.ChapterTitle == category);

        if (unsolved == true)
            query = query.Where(q => !q.IsSolved);

        var total = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        var qnaIds = await query
            .OrderByDescending(q => q.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(q => q.Id)
            .ToListAsync();

        // Get answer counts in one query
        var answerCounts = await _db.QnAAnswers
            .Where(a => qnaIds.Contains(a.QnAId))
            .GroupBy(a => a.QnAId)
            .Select(g => new { QnAId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.QnAId, x => x.Count);

        var questions = await query
            .OrderByDescending(q => q.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(q => new
            {
                q.Id,
                q.Nickname,
                q.ChapterTitle,
                q.Question,
                q.IsSolved,
                q.Upvotes,
                q.CreatedAt,
                timeAgo = FormatTimeAgo(q.CreatedAt)
            })
            .ToListAsync();

        var result = questions.Select(q => new
        {
            q.Id,
            q.Nickname,
            q.ChapterTitle,
            q.Question,
            q.IsSolved,
            q.Upvotes,
            q.CreatedAt,
            q.timeAgo,
            answerCount = answerCounts.ContainsKey(q.Id) ? answerCounts[q.Id] : 0
        });

        return Json(new { questions = result, totalPages, currentPage = page });
    }

    // JSON API: 取得某問題的回答
    [HttpGet]
    public async Task<IActionResult> Answers(int qnaId)
    {
        var answers = await _db.QnAAnswers
            .Where(a => a.QnAId == qnaId)
            .OrderByDescending(a => a.IsAccepted)
            .ThenByDescending(a => a.Upvotes)
            .ThenByDescending(a => a.CreatedAt)
            .Select(a => new
            {
                a.Id,
                a.QnAId,
                a.SessionId,
                a.Nickname,
                a.Content,
                a.IsAccepted,
                a.Upvotes,
                a.CreatedAt,
                timeAgo = FormatTimeAgo(a.CreatedAt)
            })
            .ToListAsync();

        return Json(answers);
    }

    // 發問
    [HttpPost]
    public async Task<IActionResult> Ask([FromBody] AskRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Question))
            return BadRequest(new { error = "問題不能為空" });

        var sessionId = HttpContext.Session.GetString("SessionId");
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("SessionId", sessionId);
        }

        var nickname = req.Nickname;
        if (string.IsNullOrWhiteSpace(nickname))
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
            nickname = profile?.Nickname ?? "匿名";
        }

        var chapterTitle = "";
        if (req.ChapterId.HasValue)
        {
            var ch = await _db.Chapters.FindAsync(req.ChapterId.Value);
            chapterTitle = ch?.Title ?? "";
        }

        var qna = new QnA
        {
            SessionId = sessionId,
            Nickname = nickname,
            ChapterId = req.ChapterId,
            ChapterTitle = chapterTitle,
            Question = req.Question.Trim(),
            CreatedAt = DateTime.Now
        };

        _db.QnAs.Add(qna);
        await _db.SaveChangesAsync();

        return Json(new { success = true, id = qna.Id });
    }

    // 回答問題
    [HttpPost]
    public async Task<IActionResult> Answer([FromBody] AnswerRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Content))
            return BadRequest(new { error = "回答不能為空" });

        var q = await _db.QnAs.FindAsync(req.QnAId);
        if (q == null) return NotFound(new { error = "問題不存在" });

        var sessionId = HttpContext.Session.GetString("SessionId");
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("SessionId", sessionId);
        }

        var nickname = req.Nickname;
        if (string.IsNullOrWhiteSpace(nickname))
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
            nickname = profile?.Nickname ?? "匿名";
        }

        var answer = new QnAAnswer
        {
            QnAId = req.QnAId,
            SessionId = sessionId,
            Nickname = nickname,
            Content = req.Content.Trim(),
            CreatedAt = DateTime.Now
        };

        _db.QnAAnswers.Add(answer);
        await _db.SaveChangesAsync();

        return Json(new { success = true, id = answer.Id });
    }

    // 點讚
    [HttpPost]
    public async Task<IActionResult> Upvote([FromBody] UpvoteRequest req)
    {
        if (req.Type == "question")
        {
            var q = await _db.QnAs.FindAsync(req.Id);
            if (q == null) return NotFound();
            q.Upvotes++;
            await _db.SaveChangesAsync();
            return Json(new { upvotes = q.Upvotes });
        }
        else if (req.Type == "answer")
        {
            var a = await _db.QnAAnswers.FindAsync(req.Id);
            if (a == null) return NotFound();
            a.Upvotes++;
            await _db.SaveChangesAsync();
            return Json(new { upvotes = a.Upvotes });
        }

        return BadRequest(new { error = "type 必須是 question 或 answer" });
    }

    // 標記已解決 + 採納答案
    [HttpPost]
    public async Task<IActionResult> MarkSolved([FromBody] MarkSolvedRequest req)
    {
        var q = await _db.QnAs.FindAsync(req.QnAId);
        if (q == null) return NotFound(new { error = "問題不存在" });

        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        if (q.SessionId != sessionId)
            return BadRequest(new { error = "只有發問者可以標記已解決" });

        var answer = await _db.QnAAnswers.FindAsync(req.AnswerId);
        if (answer == null || answer.QnAId != req.QnAId)
            return NotFound(new { error = "回答不存在" });

        q.IsSolved = true;
        answer.IsAccepted = true;
        await _db.SaveChangesAsync();

        return Json(new { success = true });
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

public record AskRequest(string? Nickname, int? ChapterId, string Question);
public record AnswerRequest(int QnAId, string? Nickname, string Content);
public record UpvoteRequest(string Type, int Id);
public record MarkSolvedRequest(int QnAId, int AnswerId);
