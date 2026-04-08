using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;
using System.Text.Json;

namespace DotNetLearning.Controllers;

public class QuizController : Controller
{
    private readonly AppDbContext _db;
    private static readonly Random _rng = new();
    public QuizController(AppDbContext db) => _db = db;

    // 舊的 Index endpoint（quiz.js 用的 HTML 版）
    public async Task<IActionResult> Index(int chapterId, int count = 5)
    {
        var all = await _db.Questions
            .Where(q => q.ChapterId == chapterId)
            .Select(q => new { q.Id, q.QuestionText, q.Type, q.OptionsJson, q.Difficulty })
            .ToListAsync();

        var questions = all.OrderBy(_ => _rng.Next()).Take(count).ToList();
        var json = JsonSerializer.Serialize(questions);
        return Content($"<div id='quiz-data' style='display:none'>{System.Web.HttpUtility.HtmlEncode(json)}</div>", "text/html");
    }

    // 隨機挑戰 — 跨章節隨機出題
    [HttpGet]
    public async Task<IActionResult> Random(int count = 10)
    {
        var all = await _db.Questions
            .Select(q => new { q.Id, q.QuestionText, q.Type, q.Difficulty, q.OptionsJson })
            .ToListAsync();

        var raw = all.OrderBy(_ => _rng.Next()).Take(count).ToList();

        var questions = raw.Select(q => {
            var opts = new List<string>();
            if (q.Type != "fillin" && !string.IsNullOrEmpty(q.OptionsJson))
            {
                try { opts = JsonSerializer.Deserialize<List<string>>(q.OptionsJson) ?? new List<string>(); }
                catch { }
            }
            return new { q.Id, q.QuestionText, q.Type, q.Difficulty, options = opts };
        }).Where(q => q.Type == "fillin" || q.options.Count > 0).ToList();

        return Json(new { questions });
    }

    // 隨機挑戰提交
    [HttpPost]
    public async Task<IActionResult> SubmitRandom([FromBody] RandomQuizSubmission submission)
    {
        var ids = submission.Answers.Keys.Select(k => int.TryParse(k, out var v) ? v : 0).Where(v => v > 0).ToList();
        var questions = await _db.Questions.Where(q => ids.Contains(q.Id)).ToListAsync();

        var details = new List<object>();
        int correct = 0;

        foreach (var q in questions)
        {
            submission.Answers.TryGetValue(q.Id.ToString(), out var answer);
            bool isCorrect = CheckAnswer(answer, q.CorrectAnswer, q.OptionsJson);
            if (isCorrect) correct++;
            details.Add(new {
                questionText = q.QuestionText,
                yourAnswer = answer ?? "",
                correctAnswer = q.CorrectAnswer,
                isCorrect,
                explanation = q.Explanation
            });
        }

        // 儲存記錄
        var sessionId2 = HttpContext.Session.GetString("SessionId") ?? "";
        if (!string.IsNullOrEmpty(sessionId2))
        {
            _db.QuizAttempts.Add(new QuizAttempt
            {
                SessionId = sessionId2,
                ChapterId = 0,
                Score = correct,
                Total = questions.Count,
                TakenAt = DateTime.Now
            });

            // Sync scores to UserProfile and SiteUser
            int earnedPoints = correct * 10;
            if (correct == questions.Count && questions.Count > 0)
                earnedPoints += questions.Count * 5;

            var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId2);
            if (profile == null)
            {
                profile = new UserProfile { SessionId = sessionId2, Nickname = "匿名學習者" };
                _db.UserProfiles.Add(profile);
            }
            profile.TotalScore += earnedPoints;
            profile.QuizzesTaken++;
            profile.LastActiveAt = DateTime.Now;
            profile.BadgeLevel = profile.TotalScore switch
            {
                >= 5000 => "master",
                >= 2000 => "expert",
                >= 1000 => "advanced",
                >= 500 => "intermediate",
                >= 100 => "beginner",
                _ => "newbie"
            };

            var siteUser = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == sessionId2);
            if (siteUser != null)
            {
                siteUser.TotalScore = profile.TotalScore;
                siteUser.QuizzesTaken = profile.QuizzesTaken;
                siteUser.BadgeLevel = profile.BadgeLevel;
                siteUser.LastActiveAt = DateTime.Now;
            }

            await _db.SaveChangesAsync();
        }

        return Json(new { score = correct, total = questions.Count, details });
    }

    // 章節測驗 JSON API
    [HttpGet]
    public async Task<IActionResult> ChapterQuiz(int chapterId, int count = 5)
    {
        var all = await _db.Questions
            .Where(q => q.ChapterId == chapterId)
            .Select(q => new { q.Id, q.QuestionText, q.Type, q.Difficulty, q.OptionsJson })
            .ToListAsync();

        var raw = all.OrderBy(_ => _rng.Next()).Take(count).ToList();

        var questions = raw.Select(q => {
            var opts = new List<string>();
            if (q.Type != "fillin" && !string.IsNullOrEmpty(q.OptionsJson))
            {
                try { opts = JsonSerializer.Deserialize<List<string>>(q.OptionsJson) ?? new List<string>(); }
                catch { }
            }
            return new { q.Id, q.QuestionText, q.Type, q.Difficulty, options = opts };
        }).ToList();

        return Json(new { chapterId, questions });
    }

    // 章節測驗提交
    [HttpPost]
    public async Task<IActionResult> SubmitChapterQuiz([FromBody] QuizSubmission submission)
    {
        var ids = submission.Answers.Keys.Select(k => int.TryParse(k, out var v) ? v : 0).Where(v => v > 0).ToList();
        var questions = await _db.Questions.Where(q => ids.Contains(q.Id)).ToListAsync();

        var details = new List<object>();
        int correct = 0;

        foreach (var q in questions)
        {
            submission.Answers.TryGetValue(q.Id.ToString(), out var answer);
            bool isCorrect = CheckAnswer(answer, q.CorrectAnswer, q.OptionsJson);
            if (isCorrect) correct++;
            details.Add(new {
                questionText = q.QuestionText,
                yourAnswer = answer ?? "",
                correctAnswer = q.CorrectAnswer,
                isCorrect,
                explanation = q.Explanation
            });
        }

        // Save attempt
        var sessionId = HttpContext.Session.GetString("SessionId") ?? Guid.NewGuid().ToString();
        _db.QuizAttempts.Add(new QuizAttempt
        {
            SessionId = sessionId,
            ChapterId = submission.ChapterId,
            Score = correct,
            Total = questions.Count,
            TakenAt = DateTime.Now
        });
        await _db.SaveChangesAsync();

        int pct = questions.Count > 0 ? (int)(correct * 100.0 / questions.Count) : 0;
        bool passed = pct >= 60;

        // 通過就標記完成
        if (passed)
        {
            var existing = await _db.Progresses
                .FirstOrDefaultAsync(p => p.SessionId == sessionId && p.ChapterId == submission.ChapterId);
            if (existing == null)
            {
                _db.Progresses.Add(new Progress
                {
                    SessionId = sessionId,
                    ChapterId = submission.ChapterId,
                    IsCompleted = true,
                    CompletedAt = DateTime.Now
                });
                await _db.SaveChangesAsync();
            }
        }

        // Sync scores to UserProfile and SiteUser so leaderboard stays consistent
        int earnedPoints = correct * 10;
        if (correct == questions.Count && questions.Count > 0)
            earnedPoints += questions.Count * 5;

        var profile = await _db.UserProfiles.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        if (profile == null)
        {
            profile = new UserProfile { SessionId = sessionId, Nickname = "匿名學習者" };
            _db.UserProfiles.Add(profile);
        }
        profile.TotalScore += earnedPoints;
        profile.QuizzesTaken++;
        profile.LastActiveAt = DateTime.Now;
        profile.BadgeLevel = profile.TotalScore switch
        {
            >= 5000 => "master",
            >= 2000 => "expert",
            >= 1000 => "advanced",
            >= 500 => "intermediate",
            >= 100 => "beginner",
            _ => "newbie"
        };
        profile.ChaptersCompleted = await _db.Progresses
            .Where(p => p.SessionId == sessionId && p.IsCompleted)
            .CountAsync();

        var siteUser = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == sessionId);
        if (siteUser != null)
        {
            siteUser.TotalScore = profile.TotalScore;
            siteUser.QuizzesTaken = profile.QuizzesTaken;
            siteUser.ChaptersCompleted = profile.ChaptersCompleted;
            siteUser.BadgeLevel = profile.BadgeLevel;
            siteUser.LastActiveAt = DateTime.Now;
        }

        await _db.SaveChangesAsync();

        return Json(new { score = correct, total = questions.Count, percentage = pct, passed, details, earnedPoints });
    }

    // 保留舊 Submit endpoint
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] QuizSubmission submission)
    {
        return await SubmitChapterQuiz(submission);
    }

    // Smart answer matching: supports both full option text ("B. Microsoft") and letter-only ("B")
    private static bool CheckAnswer(string? userAnswer, string correctAnswer, string? optionsJson)
    {
        if (string.IsNullOrWhiteSpace(userAnswer)) return false;
        var ua = userAnswer.Trim();
        var ca = correctAnswer.Trim();

        // Direct match (old-style: full text match)
        if (string.Equals(ua, ca, StringComparison.OrdinalIgnoreCase)) return true;

        // User sent full option text like "D. xxx", correct is just "D"
        if (ca.Length == 1 && ua.Length > 1 && ua[1] is '.' or ' ' or '、'
            && char.ToUpper(ua[0]) == char.ToUpper(ca[0])) return true;

        // User sent just "D", correct is full text "D. something"
        if (ua.Length == 1 && ca.Length > 1 && ca[1] is '.' or ' ' or '、'
            && char.ToUpper(ca[0]) == char.ToUpper(ua[0])) return true;

        return false;
    }
}

public record QuizSubmission(int ChapterId, Dictionary<string, string> Answers);
public record RandomQuizSubmission(Dictionary<string, string> Answers);
public record QuizResult(int QuestionId, string QuestionText, string UserAnswer, string CorrectAnswer, bool IsCorrect, string Explanation);
