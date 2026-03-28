using DotNetLearning.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetLearning.Data;

public static class SeedData
{
    private const int EXPECTED_CHAPTERS = 999; // 強制重建以加入新章節

    public static void Initialize(AppDbContext db)
    {
        db.Database.EnsureCreated();

        // 已有正確數量 + 有 questions → 跳過
        var chCount = db.Chapters.Count();
        var qCount = db.Questions.Count();
        if (chCount >= EXPECTED_CHAPTERS && qCount > 0) return;

        // 需要重建 → 砍掉 DB 重來
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

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

        db.Chapters.AddRange(chapters);
        db.SaveChanges();

        // 測驗題（過濾掉已移除分類的題目）
        var validChapterIds = chapters.Select(c => c.Id).ToHashSet();
        var questions = SeedQuestions.GetQuestions()
            .Where(q => validChapterIds.Contains(q.ChapterId))
            .ToList();
        db.Questions.AddRange(questions);
        db.SaveChanges();

        // 程式碼填字遊戲
        db.CodePuzzles.AddRange(SeedCodePuzzles.GetPuzzles());
        db.SaveChanges();

        // 程式碼偵探 Bug 挑戰
        db.BugChallenges.AddRange(SeedBugChallenges.GetChallenges());
        db.SaveChanges();
    }
}
