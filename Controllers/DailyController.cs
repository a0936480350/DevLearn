using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;
using System.Text.Json;

namespace DotNetLearning.Controllers;

public class DailyController : Controller
{
    private readonly AppDbContext _db;
    private static readonly Random _rng = new();

    public DailyController(AppDbContext db) => _db = db;

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
    public async Task<IActionResult> Today()
    {
        var today = DateTime.Today;
        var sessionId = GetSessionId();

        // Get or create today's challenge
        var challenge = await _db.DailyChallenges
            .FirstOrDefaultAsync(d => d.ChallengeDate.Date == today);

        if (challenge == null)
        {
            // Pick a random question
            var questionIds = await _db.Questions.Select(q => q.Id).ToListAsync();
            if (questionIds.Count == 0)
                return Json(new { error = "No questions available" });

            var randomId = questionIds[_rng.Next(questionIds.Count)];
            var q = await _db.Questions.Include(q => q.Chapter).FirstAsync(q => q.Id == randomId);

            challenge = new DailyChallenge
            {
                ChallengeDate = today,
                QuestionId = randomId,
                Category = q.Chapter?.Category ?? ""
            };
            _db.DailyChallenges.Add(challenge);
            await _db.SaveChangesAsync();
        }

        var question = await _db.Questions.Include(q => q.Chapter)
            .FirstOrDefaultAsync(q => q.Id == challenge.QuestionId);

        if (question == null)
            return Json(new { error = "Question not found" });

        // Check if already answered today
        var attempt = await _db.DailyAttempts
            .FirstOrDefaultAsync(a => a.SessionId == sessionId && a.DailyChallengeId == challenge.Id);

        var streakDays = await CalculateStreak(sessionId);

        var result = new
        {
            challengeId = challenge.Id,
            questionText = question.QuestionText,
            type = question.Type,
            optionsJson = question.OptionsJson,
            category = challenge.Category,
            alreadyAnswered = attempt != null,
            wasCorrect = attempt?.IsCorrect ?? false,
            userAnswer = attempt?.UserAnswer ?? "",
            correctAnswer = attempt != null ? question.CorrectAnswer : (string?)null,
            explanation = attempt != null ? question.Explanation : (string?)null,
            streakDays
        };

        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] DailySubmission submission)
    {
        var sessionId = GetSessionId();

        var challenge = await _db.DailyChallenges.FindAsync(submission.DailyChallengeId);
        if (challenge == null)
            return Json(new { error = "Challenge not found" });

        // Check if already answered
        var existing = await _db.DailyAttempts
            .FirstOrDefaultAsync(a => a.SessionId == sessionId && a.DailyChallengeId == challenge.Id);
        if (existing != null)
            return Json(new { error = "Already answered today" });

        var question = await _db.Questions.FindAsync(challenge.QuestionId);
        if (question == null)
            return Json(new { error = "Question not found" });

        bool correct = string.Equals(
            submission.Answer?.Trim(),
            question.CorrectAnswer.Trim(),
            StringComparison.OrdinalIgnoreCase);

        // Calculate streak before saving
        var previousStreak = await CalculateStreak(sessionId);
        var newStreak = correct ? previousStreak + 1 : 0;

        var attempt = new DailyAttempt
        {
            SessionId = sessionId,
            DailyChallengeId = challenge.Id,
            IsCorrect = correct,
            UserAnswer = submission.Answer ?? "",
            StreakDays = newStreak,
            AnsweredAt = DateTime.Now
        };

        _db.DailyAttempts.Add(attempt);
        await _db.SaveChangesAsync();

        // Check if new record
        var bestStreak = await _db.DailyAttempts
            .Where(a => a.SessionId == sessionId)
            .MaxAsync(a => (int?)a.StreakDays) ?? 0;

        return Json(new
        {
            correct,
            correctAnswer = question.CorrectAnswer,
            explanation = question.Explanation,
            streak = newStreak,
            isNewRecord = newStreak > 0 && newStreak >= bestStreak
        });
    }

    [HttpGet]
    public async Task<IActionResult> History(int days = 30)
    {
        var sessionId = GetSessionId();
        var since = DateTime.Today.AddDays(-(days - 1));

        var attempts = await (
            from a in _db.DailyAttempts
            join c in _db.DailyChallenges on a.DailyChallengeId equals c.Id
            where a.SessionId == sessionId && c.ChallengeDate >= since
            select new
            {
                date = c.ChallengeDate.ToString("yyyy-MM-dd"),
                isCorrect = a.IsCorrect,
                streakDays = a.StreakDays
            }
        ).ToListAsync();

        return Json(attempts);
    }

    [HttpGet]
    public async Task<IActionResult> Stats()
    {
        var sessionId = GetSessionId();

        var allAttempts = await (
            from a in _db.DailyAttempts
            join c in _db.DailyChallenges on a.DailyChallengeId equals c.Id
            where a.SessionId == sessionId
            orderby c.ChallengeDate descending
            select new { a.IsCorrect, a.StreakDays, c.ChallengeDate }
        ).ToListAsync();

        var totalAttempted = allAttempts.Count;
        var totalCorrect = allAttempts.Count(a => a.IsCorrect);
        var currentStreak = await CalculateStreak(sessionId);
        var bestStreak = allAttempts.Count > 0 ? allAttempts.Max(a => a.StreakDays) : 0;
        var accuracy = totalAttempted > 0 ? Math.Round(totalCorrect * 100.0 / totalAttempted, 1) : 0;

        return Json(new
        {
            currentStreak,
            bestStreak,
            totalCorrect,
            totalAttempted,
            accuracy
        });
    }

    private async Task<int> CalculateStreak(string sessionId)
    {
        // Calculate consecutive days answered correctly, counting backwards from yesterday
        var attempts = await (
            from a in _db.DailyAttempts
            join c in _db.DailyChallenges on a.DailyChallengeId equals c.Id
            where a.SessionId == sessionId
            orderby c.ChallengeDate descending
            select new { a.IsCorrect, c.ChallengeDate }
        ).ToListAsync();

        int streak = 0;
        var checkDate = DateTime.Today.AddDays(-1); // Start from yesterday

        foreach (var att in attempts)
        {
            if (att.ChallengeDate.Date == checkDate)
            {
                if (att.IsCorrect)
                {
                    streak++;
                    checkDate = checkDate.AddDays(-1);
                }
                else
                {
                    break;
                }
            }
            else if (att.ChallengeDate.Date < checkDate)
            {
                break; // Gap in days
            }
        }

        return streak;
    }
}

public record DailySubmission(int DailyChallengeId, string Answer);
