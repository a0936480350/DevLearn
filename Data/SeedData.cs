using DotNetLearning.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetLearning.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        db.Database.EnsureCreated();

        // 已有章節 → 不砍，只補新的
        var existingChapterIds = db.Chapters.Select(c => c.Id).ToHashSet();
        var existingQuestionIds = db.Questions.Select(q => q.Id).ToHashSet();

        // 從各分類檔案收集所有章節
        var chapters = new List<Chapter>();
        chapters.AddRange(SeedChapters_CSharpBase.GetChapters());
        chapters.AddRange(SeedChapters_CSharp.GetChapters());
        chapters.AddRange(SeedChapters_CSharpExtra.GetChapters());
        chapters.AddRange(SeedChapters_AspNet.GetChapters());
        chapters.AddRange(SeedChapters_AspNetExtra.GetChapters());
        chapters.AddRange(SeedChapters_Database.GetChapters());
        chapters.AddRange(SeedChapters_Network.GetChapters());
        chapters.AddRange(SeedChapters_Security.GetChapters());
        chapters.AddRange(SeedChapters_Docker.GetChapters());
        chapters.AddRange(SeedChapters_Patterns.GetChapters());
        chapters.AddRange(SeedChapters_Infrastructure.GetChapters());
        chapters.AddRange(SeedChapters_AI.GetChapters());
        chapters.AddRange(SeedChapters_Frontend.GetChapters());
        chapters.AddRange(SeedChapters_Git.GetChapters());
        chapters.AddRange(SeedChapters_Server.GetChapters());
        chapters.AddRange(SeedChapters_AIModel.GetChapters());
        chapters.AddRange(SeedChapters_ClaudeCode.GetChapters());
        chapters.AddRange(SeedChapters_Project.GetChapters());
        chapters.AddRange(SeedChapters_IoT.GetChapters());
        chapters.AddRange(SeedChapters_IoT2.GetChapters());
        chapters.AddRange(SeedChapters_IoT3.GetChapters());
        chapters.AddRange(SeedChapters_DevLearn.GetChapters());
        chapters.AddRange(SeedChapters_Whiteboard.GetChapters());
        chapters.AddRange(SeedChapters_React.GetChapters());
        chapters.AddRange(SeedChapters_Angular.GetChapters());
        chapters.AddRange(SeedChapters_Vue.GetChapters());

        // 只新增不存在的章節（不砍舊資料）
        var newChapters = chapters.Where(c => !existingChapterIds.Contains(c.Id)).ToList();
        if (newChapters.Count > 0)
        {
            db.Chapters.AddRange(newChapters);
            db.SaveChanges();
            Console.WriteLine($"[Seed] Added {newChapters.Count} new chapters (total: {db.Chapters.Count()})");
        }

        // 只新增不存在的測驗題
        var allChapterIds = db.Chapters.Select(c => c.Id).ToHashSet();
        var questions = SeedQuestions.GetQuestions()
            .Where(q => allChapterIds.Contains(q.ChapterId) && !existingQuestionIds.Contains(q.Id))
            .ToList();
        if (questions.Count > 0)
        {
            db.Questions.AddRange(questions);
            db.SaveChanges();
            Console.WriteLine($"[Seed] Added {questions.Count} new questions");
        }

        // IoT 測驗題
        var iotQuestions = SeedQuestions_IoT.GetQuestions()
            .Where(q => !existingQuestionIds.Contains(q.Id))
            .ToList();
        if (iotQuestions.Count > 0)
        {
            db.Questions.AddRange(iotQuestions);
            db.SaveChanges();
            Console.WriteLine($"[Seed] Added {iotQuestions.Count} IoT questions");
        }

        // 程式碼擂台挑戰（第二批）
        if (db.ArenaChallenges.Count() < 8)
        {
            db.ArenaChallenges.AddRange(SeedArenaChallenges2.GetChallenges());
            db.SaveChanges();
            Console.WriteLine("[Seed] Added arena challenges batch 2");
        }

        // 程式碼填字遊戲（只在沒有時才加）
        if (!db.CodePuzzles.Any())
        {
            db.CodePuzzles.AddRange(SeedCodePuzzles.GetPuzzles());
            db.SaveChanges();
            Console.WriteLine("[Seed] Added code puzzles");
        }

        // 程式碼偵探 Bug 挑戰（只在沒有時才加）
        if (!db.BugChallenges.Any())
        {
            db.BugChallenges.AddRange(SeedBugChallenges.GetChallenges());
            db.SaveChanges();
            Console.WriteLine("[Seed] Added bug challenges");
        }

        Console.WriteLine($"[Seed] Done. Chapters: {db.Chapters.Count()}, Questions: {db.Questions.Count()}");
    }
}
