using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class FlashcardController : Controller
{
    private readonly AppDbContext _db;
    public FlashcardController(AppDbContext db) => _db = db;

    public IActionResult Index()
    {
        ViewData["Title"] = "暗記卡片";
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> MyCards()
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        var cards = await _db.Flashcards
            .Where(f => f.SessionId == sessionId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
        return Json(cards);
    }

    [HttpGet]
    public async Task<IActionResult> Review(int count = 10)
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        var now = DateTime.Now;

        var cards = await _db.Flashcards
            .Where(f => f.SessionId == sessionId && f.NextReview <= now)
            .ToListAsync();

        // Shuffle and take requested count
        var rng = new Random();
        var selected = cards.OrderBy(_ => rng.Next()).Take(count).ToList();

        return Json(selected);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFlashcardRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Front) || string.IsNullOrWhiteSpace(req.Back))
            return BadRequest(new { error = "正面和背面不可為空" });

        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";

        var card = new Flashcard
        {
            SessionId = sessionId,
            Front = req.Front,
            Back = req.Back,
            Category = req.Category ?? "",
            ReviewCount = 0,
            CorrectCount = 0,
            NextReview = DateTime.Now,
            CreatedAt = DateTime.Now
        };

        _db.Flashcards.Add(card);
        await _db.SaveChangesAsync();
        return Json(new { ok = true, id = card.Id });
    }

    [HttpPost]
    public async Task<IActionResult> MarkReview([FromBody] MarkReviewRequest req)
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        var card = await _db.Flashcards
            .FirstOrDefaultAsync(f => f.Id == req.Id && f.SessionId == sessionId);

        if (card == null) return NotFound();

        var now = DateTime.Now;

        if (req.Correct)
        {
            card.CorrectCount++;
            card.ReviewCount++;
            var days = Math.Min(card.CorrectCount, 30);
            card.NextReview = now.AddDays(days);
        }
        else
        {
            card.ReviewCount++;
            card.CorrectCount = 0;
            card.NextReview = now.AddMinutes(10);
        }

        await _db.SaveChangesAsync();
        return Json(new { ok = true, nextReview = card.NextReview });
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] FlashcardDeleteRequest req)
    {
        var sessionId = HttpContext.Session.GetString("SessionId") ?? "";
        var card = await _db.Flashcards
            .FirstOrDefaultAsync(f => f.Id == req.Id && f.SessionId == sessionId);

        if (card == null) return NotFound();

        _db.Flashcards.Remove(card);
        await _db.SaveChangesAsync();
        return Json(new { ok = true });
    }
}

public class CreateFlashcardRequest
{
    public string Front { get; set; } = "";
    public string Back { get; set; } = "";
    public string? Category { get; set; }
}

public class MarkReviewRequest
{
    public int Id { get; set; }
    public bool Correct { get; set; }
}

public class FlashcardDeleteRequest
{
    public int Id { get; set; }
}
