using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;
using System.Text.Json;

namespace DotNetLearning.Controllers;

public class PuzzleController : Controller
{
    private readonly AppDbContext _db;
    private static readonly Random _rng = new();

    public PuzzleController(AppDbContext db) => _db = db;

    private string SessionId
    {
        get
        {
            var id = HttpContext.Session.GetString("sid");
            if (id == null)
            {
                id = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("sid", id);
            }
            return id;
        }
    }

    // GET /Puzzle
    public IActionResult Index() => View();

    // GET /Puzzle/GetPuzzle?difficulty=beginner
    [HttpGet]
    public async Task<IActionResult> GetPuzzle(string difficulty = "beginner")
    {
        var puzzles = await _db.CodePuzzles
            .Where(p => p.Difficulty == difficulty)
            .ToListAsync();

        if (puzzles.Count == 0)
            return Json(new { error = "沒有找到題目" });

        var puzzle = puzzles[_rng.Next(puzzles.Count)];

        var blanks = JsonSerializer.Deserialize<List<BlankPosition>>(puzzle.BlankPositionsJson)
            ?? new List<BlankPosition>();

        // Build code with blanks replaced by ___
        var code = puzzle.FullCode;
        var blankInfos = new List<object>();
        // Process from end to start so positions stay valid
        var sorted = blanks.OrderByDescending(b => b.start).ToList();
        foreach (var b in sorted)
        {
            code = code.Substring(0, b.start) + "___" + code.Substring(b.end);
        }

        // Build blank info in original order
        int offset = 0;
        var orderedBlanks = blanks.OrderBy(b => b.start).ToList();
        for (int i = 0; i < orderedBlanks.Count; i++)
        {
            var b = orderedBlanks[i];
            int newStart = b.start + offset;
            int originalLen = b.end - b.start;
            int newLen = 3; // "___"
            blankInfos.Add(new { index = i, length = originalLen });
            offset += (newLen - originalLen);
        }

        return Json(new
        {
            id = puzzle.Id,
            title = puzzle.Title,
            codeWithBlanks = code,
            blanks = blankInfos,
            hint = puzzle.Hint,
            difficulty = puzzle.Difficulty,
            category = puzzle.Category
        });
    }

    // POST /Puzzle/Submit
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] PuzzleSubmitRequest request)
    {
        var puzzle = await _db.CodePuzzles.FindAsync(request.CodePuzzleId);
        if (puzzle == null)
            return Json(new { error = "題目不存在" });

        var blanks = JsonSerializer.Deserialize<List<BlankPosition>>(puzzle.BlankPositionsJson)
            ?? new List<BlankPosition>();
        var orderedBlanks = blanks.OrderBy(b => b.start).ToList();

        var details = new List<object>();
        int correctCount = 0;

        for (int i = 0; i < orderedBlanks.Count; i++)
        {
            var blank = orderedBlanks[i];
            string userAnswer = (i < request.Answers.Count) ? request.Answers[i].Trim() : "";
            bool isCorrect = string.Equals(userAnswer, blank.answer.Trim(), StringComparison.OrdinalIgnoreCase);
            if (isCorrect) correctCount++;
            details.Add(new
            {
                blankIndex = i,
                userAnswer,
                correctAnswer = blank.answer,
                isCorrect
            });
        }

        var attempt = new PuzzleAttempt
        {
            SessionId = SessionId,
            CodePuzzleId = request.CodePuzzleId,
            CorrectBlanks = correctCount,
            TotalBlanks = orderedBlanks.Count,
            TimeTakenSeconds = request.TimeTaken,
            AttemptedAt = DateTime.Now
        };
        _db.PuzzleAttempts.Add(attempt);
        await _db.SaveChangesAsync();

        return Json(new
        {
            correctCount,
            totalBlanks = orderedBlanks.Count,
            details,
            timeTaken = request.TimeTaken
        });
    }

    // GET /Puzzle/MyStats
    [HttpGet]
    public async Task<IActionResult> MyStats()
    {
        var sid = SessionId;
        var attempts = await _db.PuzzleAttempts
            .Where(a => a.SessionId == sid)
            .OrderByDescending(a => a.AttemptedAt)
            .ToListAsync();

        var totalAttempts = attempts.Count;
        var totalCorrect = attempts.Sum(a => a.CorrectBlanks);
        var totalBlanks = attempts.Sum(a => a.TotalBlanks);
        var avgTime = totalAttempts > 0 ? attempts.Average(a => a.TimeTakenSeconds) : 0;
        var perfectCount = attempts.Count(a => a.CorrectBlanks == a.TotalBlanks);

        return Json(new
        {
            totalAttempts,
            totalCorrect,
            totalBlanks,
            accuracy = totalBlanks > 0 ? Math.Round((double)totalCorrect / totalBlanks * 100, 1) : 0,
            averageTimeSeconds = Math.Round(avgTime, 1),
            perfectCount,
            recentAttempts = attempts.Take(10).Select(a => new
            {
                a.CorrectBlanks,
                a.TotalBlanks,
                a.TimeTakenSeconds,
                a.AttemptedAt
            })
        });
    }

    // Helper classes
    private class BlankPosition
    {
        public int start { get; set; }
        public int end { get; set; }
        public string answer { get; set; } = "";
    }

    public class PuzzleSubmitRequest
    {
        public int CodePuzzleId { get; set; }
        public List<string> Answers { get; set; } = new();
        public int TimeTaken { get; set; }
    }
}
