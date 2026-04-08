using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;
using DotNetLearning.Filters;

namespace DotNetLearning.Controllers;

[RequireRegistration]
public class ArenaController : Controller
{
    private readonly AppDbContext _db;
    public ArenaController(AppDbContext db) => _db = db;

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
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Challenges()
    {
        var challenges = await _db.ArenaChallenges
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync();

        var challengeIds = challenges.Select(c => c.Id).ToList();
        var submissionCounts = await _db.ArenaSubmissions
            .Where(s => challengeIds.Contains(s.ArenaChallengeId))
            .GroupBy(s => s.ArenaChallengeId)
            .Select(g => new { ChallengeId = g.Key, Count = g.Count() })
            .ToListAsync();

        var countDict = submissionCounts.ToDictionary(x => x.ChallengeId, x => x.Count);

        var result = challenges.Select(c => new
        {
            c.Id,
            c.Title,
            c.Description,
            c.Category,
            c.Difficulty,
            c.StartDate,
            c.EndDate,
            c.IsActive,
            SubmissionCount = countDict.GetValueOrDefault(c.Id, 0)
        });

        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var challenge = await _db.ArenaChallenges.FindAsync(id);
        if (challenge == null) return NotFound();

        var submissions = await _db.ArenaSubmissions
            .Where(s => s.ArenaChallengeId == id)
            .OrderByDescending(s => s.Upvotes)
            .ThenByDescending(s => s.SubmittedAt)
            .ToListAsync();

        return Json(new
        {
            challenge = new
            {
                challenge.Id,
                challenge.Title,
                challenge.Description,
                challenge.Category,
                challenge.Difficulty,
                challenge.StartDate,
                challenge.EndDate,
                challenge.IsActive
            },
            submissions = submissions.Select(s => new
            {
                s.Id,
                s.Nickname,
                s.Code,
                s.Language,
                s.Explanation,
                s.Upvotes,
                s.SubmittedAt
            })
        });
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] ArenaSubmitRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Code))
            return BadRequest(new { error = "Code is required" });

        var challenge = await _db.ArenaChallenges.FindAsync(req.ArenaChallengeId);
        if (challenge == null) return NotFound(new { error = "Challenge not found" });

        var sessionId = GetSessionId();
        var submission = new ArenaSubmission
        {
            SessionId = sessionId,
            ArenaChallengeId = req.ArenaChallengeId,
            Nickname = string.IsNullOrWhiteSpace(req.Nickname) ? "Anonymous" : req.Nickname.Trim(),
            Code = req.Code,
            Language = req.Language ?? "csharp",
            Explanation = req.Explanation ?? "",
            Upvotes = 0,
            SubmittedAt = DateTime.Now
        };

        _db.ArenaSubmissions.Add(submission);
        await _db.SaveChangesAsync();

        return Json(new { success = true, id = submission.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Upvote([FromBody] ArenaUpvoteRequest req)
    {
        var submission = await _db.ArenaSubmissions.FindAsync(req.SubmissionId);
        if (submission == null) return NotFound(new { error = "Submission not found" });

        submission.Upvotes++;
        await _db.SaveChangesAsync();

        return Json(new { success = true, upvotes = submission.Upvotes });
    }

    [HttpGet]
    public async Task<IActionResult> MySubmissions()
    {
        var sessionId = GetSessionId();
        var submissions = await _db.ArenaSubmissions
            .Where(s => s.SessionId == sessionId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();

        return Json(submissions.Select(s => new
        {
            s.Id,
            s.ArenaChallengeId,
            s.Nickname,
            s.Code,
            s.Language,
            s.Explanation,
            s.Upvotes,
            s.SubmittedAt
        }));
    }
}

public record ArenaSubmitRequest(int ArenaChallengeId, string? Nickname, string Code, string? Language, string? Explanation);
public record ArenaUpvoteRequest(int SubmissionId);
