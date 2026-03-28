namespace DotNetLearning.Models;

// 每日簽到
public class CheckIn
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public DateTime CheckInDate { get; set; }  // 只存日期
    public int Streak { get; set; }            // 連續天數
    public int BonusPoints { get; set; }       // 當次獲得積分
}

// 成就
public class Achievement
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string AchievementKey { get; set; } = "";  // first_chapter, streak_7, quiz_100, etc.
    public string Title { get; set; } = "";
    public string Icon { get; set; } = "🏆";
    public string Description { get; set; } = "";
    public DateTime UnlockedAt { get; set; } = DateTime.Now;
}

// 學習時間記錄
public class StudyLog
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public DateTime LogDate { get; set; }      // 日期
    public int MinutesSpent { get; set; }      // 當天學習分鐘數
    public int ChaptersViewed { get; set; }    // 當天看了幾章
    public int QuizzesDone { get; set; }       // 當天做了幾次測驗
}

// 程式碼收藏
public class CodeSnippet
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public int? ChapterId { get; set; }
    public string ChapterTitle { get; set; } = "";
    public string Title { get; set; } = "";      // 自訂標題
    public string Code { get; set; } = "";        // 程式碼內容
    public string Language { get; set; } = "csharp";
    public string Note { get; set; } = "";        // 備註
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// 暗記卡片
public class Flashcard
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string Front { get; set; } = "";       // 正面（問題）
    public string Back { get; set; } = "";        // 背面（答案）
    public string Category { get; set; } = "";    // 分類
    public int ReviewCount { get; set; }          // 複習次數
    public int CorrectCount { get; set; }         // 答對次數
    public DateTime NextReview { get; set; } = DateTime.Now;  // 下次複習時間
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// 問答
public class QnA
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "匿名";
    public int? ChapterId { get; set; }
    public string ChapterTitle { get; set; } = "";
    public string Question { get; set; } = "";
    public bool IsSolved { get; set; }
    public int Upvotes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// 問答回答
public class QnAAnswer
{
    public int Id { get; set; }
    public int QnAId { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "匿名";
    public string Content { get; set; } = "";
    public bool IsAccepted { get; set; }
    public int Upvotes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
