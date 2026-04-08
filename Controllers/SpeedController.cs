using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;
using DotNetLearning.Filters;
using System.Text.Json;

namespace DotNetLearning.Controllers;

[RequireRegistration]
public class SpeedController : Controller
{
    private readonly AppDbContext _db;
    private static readonly Random _rng = new();

    public SpeedController(AppDbContext db) => _db = db;

    // 主頁面
    public IActionResult Index() => View();

    // 取得隨機題目（不含正確答案）
    [HttpGet]
    public async Task<IActionResult> Start(string category = "all", int timeLimit = 60)
    {
        IQueryable<Question> query = _db.Questions.Include(q => q.Chapter);

        if (category != "all")
        {
            query = query.Where(q => q.Chapter.Category == category);
        }

        // 只取 multiple / truefalse 類型（有選項的）
        query = query.Where(q => q.Type == "multiple" || q.Type == "truefalse");

        var all = await query
            .Select(q => new
            {
                id = q.Id,
                questionText = q.QuestionText,
                optionsJson = q.OptionsJson,
                type = q.Type
            })
            .ToListAsync();

        var questions = all.OrderBy(_ => _rng.Next()).Take(30).ToList();

        return Json(new { questions, timeLimit });
    }

    // 提交結果
    [HttpPost]
    public async Task<IActionResult> Finish([FromBody] SpeedFinishRequest request)
    {
        if (request.Answers == null || request.Answers.Count == 0)
            return Json(new { correct = 0, total = 0, score = 0, percentile = 0, details = new List<object>() });

        var ids = request.Answers.Select(a => a.QuestionId).ToList();
        var questions = await _db.Questions.Where(q => ids.Contains(q.Id)).ToListAsync();

        var details = new List<object>();
        int correct = 0;

        foreach (var ans in request.Answers)
        {
            var q = questions.FirstOrDefault(x => x.Id == ans.QuestionId);
            if (q == null) continue;

            bool isCorrect = SmartAnswerMatch(ans.Answer, q.CorrectAnswer);

            if (isCorrect) correct++;

            details.Add(new
            {
                questionId = q.Id,
                correct = isCorrect,
                correctAnswer = q.CorrectAnswer,
                explanation = q.Explanation
            });
        }

        int total = request.Answers.Count;
        int baseScore = correct * 10;
        // 時間獎勵：剩餘時間越多獎勵越高（最多 60 秒省下 = 最多 60 額外分）
        int timeBonus = Math.Max(0, 60 - request.TimeTaken);
        int score = baseScore + (correct > 0 ? timeBonus : 0);

        // 儲存紀錄
        var sessionId = HttpContext.Session.GetString("SessionId") ?? Guid.NewGuid().ToString();
        HttpContext.Session.SetString("SessionId", sessionId);

        _db.SpeedRuns.Add(new SpeedRun
        {
            SessionId = sessionId,
            CorrectCount = correct,
            TotalCount = total,
            TimeLimitSeconds = 60,
            ScorePoints = score,
            Category = request.Category ?? "all",
            PlayedAt = DateTime.Now
        });
        await _db.SaveChangesAsync();

        // 計算百分位
        var allScores = await _db.SpeedRuns
            .Where(s => s.Category == (request.Category ?? "all"))
            .Select(s => s.ScorePoints)
            .ToListAsync();

        int beaten = allScores.Count(s => s < score);
        int percentile = allScores.Count > 1
            ? (int)Math.Round(beaten * 100.0 / (allScores.Count - 1))
            : 100;
        percentile = Math.Min(percentile, 100);

        return Json(new { correct, total, score, percentile, details });
    }

    // 排行榜
    [HttpGet]
    public async Task<IActionResult> Leaderboard(string category = "all")
    {
        var runs = await _db.SpeedRuns
            .Where(s => s.Category == category)
            .OrderByDescending(s => s.ScorePoints)
            .ThenBy(s => s.PlayedAt)
            .Take(20)
            .Select(s => new
            {
                nickname = string.IsNullOrEmpty(s.Nickname) ? "匿名挑戰者" : s.Nickname,
                score = s.ScorePoints,
                correct = s.CorrectCount,
                total = s.TotalCount,
                playedAt = s.PlayedAt.ToString("MM/dd HH:mm")
            })
            .ToListAsync();

        return Json(runs);
    }

    private static bool SmartAnswerMatch(string? userAnswer, string correctAnswer)
    {
        if (string.IsNullOrWhiteSpace(userAnswer)) return false;
        var ua = userAnswer.Trim();
        var ca = correctAnswer.Trim();
        if (string.Equals(ua, ca, StringComparison.OrdinalIgnoreCase)) return true;
        if (ca.Length == 1 && ua.Length > 1 && ua[1] is '.' or ' ' or '、'
            && char.ToUpper(ua[0]) == char.ToUpper(ca[0])) return true;
        if (ua.Length == 1 && ca.Length > 1 && ca[1] is '.' or ' ' or '、'
            && char.ToUpper(ca[0]) == char.ToUpper(ua[0])) return true;
        return false;
    }
}

public class SpeedFinishRequest
{
    public List<SpeedAnswer> Answers { get; set; } = new();
    public int TimeTaken { get; set; }
    public string? Category { get; set; }
}

public class SpeedAnswer
{
    public int QuestionId { get; set; }
    public string Answer { get; set; } = "";
}
