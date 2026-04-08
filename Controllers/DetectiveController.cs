using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;
using DotNetLearning.Filters;

namespace DotNetLearning.Controllers;

[RequireRegistration]
public class DetectiveController : Controller
{
    private readonly AppDbContext _db;
    private static readonly Random _rng = new();

    public DetectiveController(AppDbContext db) => _db = db;

    private string GetSessionId()
    {
        var sid = HttpContext.Session.GetString("sid");
        if (string.IsNullOrEmpty(sid))
        {
            sid = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("sid", sid);
        }
        return sid;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetChallenge(string difficulty)
    {
        var query = _db.BugChallenges.AsQueryable();

        if (!string.IsNullOrEmpty(difficulty) && difficulty != "all")
        {
            query = query.Where(c => c.Difficulty == difficulty);
        }

        var challenges = await query.ToListAsync();
        if (challenges.Count == 0)
            return Json(new { error = "no_challenges" });

        var challenge = challenges[_rng.Next(challenges.Count)];

        // Don't send FixedCode to the client
        return Json(new
        {
            challenge.Id,
            challenge.Title,
            challenge.BuggyCode,
            challenge.Language,
            challenge.Difficulty,
            challenge.Category,
            challenge.BugCount,
            challenge.Explanation
        });
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] BugSubmission submission)
    {
        var challenge = await _db.BugChallenges.FindAsync(submission.BugChallengeId);
        if (challenge == null)
            return Json(new { error = "not_found" });

        var sid = GetSessionId();

        // Normalize both strings for comparison
        var userAnswer = NormalizeCode(submission.UserAnswer ?? "");
        var fixedCode = NormalizeCode(challenge.FixedCode);

        bool correct = userAnswer == fixedCode;

        var attempt = new BugAttempt
        {
            SessionId = sid,
            BugChallengeId = challenge.Id,
            Solved = correct,
            TimeTakenSeconds = submission.TimeTakenSeconds,
            AttemptedAt = DateTime.Now
        };

        _db.BugAttempts.Add(attempt);
        await _db.SaveChangesAsync();

        return Json(new
        {
            correct,
            explanation = challenge.Explanation,
            fixedCode = challenge.FixedCode,
            timeTaken = submission.TimeTakenSeconds
        });
    }

    [HttpGet]
    public async Task<IActionResult> MyStats()
    {
        var sid = GetSessionId();

        var attempts = await _db.BugAttempts
            .Where(a => a.SessionId == sid)
            .ToListAsync();

        var solved = attempts.Count(a => a.Solved);
        var attempted = attempts.Count;
        var accuracy = attempted > 0 ? Math.Round((double)solved / attempted * 100, 1) : 0;
        var avgTime = attempts.Count > 0
            ? Math.Round(attempts.Average(a => a.TimeTakenSeconds), 0)
            : 0;

        return Json(new { solved, attempted, accuracy, avgTime });
    }

    private static string NormalizeCode(string code)
    {
        // Remove all whitespace for comparison
        return new string(code.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }
}

public record BugSubmission(int BugChallengeId, string UserAnswer, int TimeTakenSeconds = 0);
