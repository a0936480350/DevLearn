using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class SnippetController : Controller
{
    private readonly AppDbContext _db;
    public SnippetController(AppDbContext db) => _db = db;

    public IActionResult Index()
    {
        ViewData["Title"] = "程式碼收藏";
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> MySnippets()
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        var snippets = await _db.CodeSnippets
            .Where(s => s.SessionId == sessionId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
        return Json(snippets);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SaveSnippetRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Title) || string.IsNullOrWhiteSpace(req.Code))
            return BadRequest(new { error = "標題和程式碼不可為空" });

        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";

        string chapterTitle = "";
        if (req.ChapterId.HasValue)
        {
            var chapter = await _db.Chapters.FindAsync(req.ChapterId.Value);
            chapterTitle = chapter?.Title ?? "";
        }

        var snippet = new CodeSnippet
        {
            SessionId = sessionId,
            ChapterId = req.ChapterId,
            ChapterTitle = chapterTitle,
            Title = req.Title,
            Code = req.Code,
            Language = req.Language ?? "csharp",
            Note = req.Note ?? "",
            CreatedAt = DateTime.Now
        };

        _db.CodeSnippets.Add(snippet);
        await _db.SaveChangesAsync();
        return Json(new { ok = true, id = snippet.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteRequest req)
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        var snippet = await _db.CodeSnippets
            .FirstOrDefaultAsync(s => s.Id == req.Id && s.SessionId == sessionId);
        if (snippet == null) return NotFound();

        _db.CodeSnippets.Remove(snippet);
        await _db.SaveChangesAsync();
        return Json(new { ok = true });
    }
}

public class SaveSnippetRequest
{
    public string Title { get; set; } = "";
    public string Code { get; set; } = "";
    public string? Language { get; set; }
    public string? Note { get; set; }
    public int? ChapterId { get; set; }
}

public class DeleteRequest
{
    public int Id { get; set; }
}
