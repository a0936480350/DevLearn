using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using System.Text.Json;

namespace DotNetLearning.Controllers;

public class MonopolyController : Controller
{
    private readonly AppDbContext _db;
    public MonopolyController(AppDbContext db) { _db = db; }

    public IActionResult Index() => View();

    /// <summary>取得題目（按難度）</summary>
    [HttpGet]
    public async Task<IActionResult> GetQuestions(int difficulty = 1, int count = 10)
    {
        difficulty = Math.Clamp(difficulty, 1, 3);

        // Question.Difficulty is int (1=easy, 2=medium, 3=hard)
        // Only get multiple-choice with 4 options (skip fillin, truefalse)
        var questions = await _db.Questions
            .Where(q => q.Difficulty == difficulty && q.Type == "multiple")
            .OrderBy(q => Guid.NewGuid())
            .Take(count * 3) // fetch more to filter
            .Select(q => new {
                q.Id,
                text = q.QuestionText,
                optionsJson = q.OptionsJson,
                difficulty
            })
            .ToListAsync();

        // Parse options, strip "A. " "B. " prefixes, only keep 4-option questions
        var result = questions
            .Select(q => {
                var opts = string.IsNullOrEmpty(q.optionsJson) ? Array.Empty<string>()
                    : JsonSerializer.Deserialize<string[]>(q.optionsJson) ?? Array.Empty<string>();
                // Strip "A. ", "B. ", "C. ", "D. " prefixes
                var cleaned = opts.Select(o => System.Text.RegularExpressions.Regex.Replace(o.Trim(), @"^[A-Da-d][.、]\s*", "")).ToArray();
                return new { q.Id, q.text, options = cleaned, q.difficulty };
            })
            .Where(q => q.options.Length >= 4) // must have 4 options
            .Take(count);

        return Json(result);
    }

    /// <summary>驗證答案（Server-side）</summary>
    [HttpPost]
    public async Task<IActionResult> CheckAnswer([FromBody] MonopolyAnswerRequest req)
    {
        var question = await _db.Questions.FindAsync(req.QuestionId);
        if (question == null) return Json(new { correct = false, correctAnswer = 0 });

        int correctIdx = 0;
        if (!string.IsNullOrEmpty(question.CorrectAnswer))
        {
            // CorrectAnswer might be "A","B","C","D" or "0","1","2","3" or the actual text
            var ca = question.CorrectAnswer.Trim().ToUpper();
            if (ca == "A" || ca == "0") correctIdx = 0;
            else if (ca == "B" || ca == "1") correctIdx = 1;
            else if (ca == "C" || ca == "2") correctIdx = 2;
            else if (ca == "D" || ca == "3") correctIdx = 3;
            else
            {
                // Match by text (strip A./B./C./D. prefixes)
                var options = !string.IsNullOrEmpty(question.OptionsJson)
                    ? JsonSerializer.Deserialize<string[]>(question.OptionsJson)
                    : Array.Empty<string>();
                if (options != null)
                {
                    var answerText = question.CorrectAnswer.Trim();
                    for (int i = 0; i < options.Length; i++)
                    {
                        var opt = System.Text.RegularExpressions.Regex.Replace(options[i].Trim(), @"^[A-Da-d][.、]\s*", "");
                        if (opt.Equals(answerText, StringComparison.OrdinalIgnoreCase) ||
                            options[i].Trim().Equals(answerText, StringComparison.OrdinalIgnoreCase))
                        {
                            correctIdx = i;
                            break;
                        }
                    }
                }
            }
        }

        var isCorrect = req.Answer == correctIdx;
        return Json(new { correct = isCorrect, correctAnswer = correctIdx });
    }
}

public class MonopolyAnswerRequest
{
    public int QuestionId { get; set; }
    public int Answer { get; set; }
}
