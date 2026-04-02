namespace DotNetLearning.Models;

public class Announcement
{
    public int Id { get; set; }
    public string Title { get; set; } = "";       // 公告標題
    public string Content { get; set; } = "";     // 公告內容（支援簡單 HTML）
    public string Type { get; set; } = "info";    // info / success / warning / danger
    public bool IsPinned { get; set; } = false;   // 置頂
    public bool IsVisible { get; set; } = true;   // 顯示/隱藏
    public string CreatedBy { get; set; } = "Admin";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ExpiresAt { get; set; }      // null = 永不過期
}

public class ErrorLog
{
    public int Id { get; set; }
    public string PageUrl { get; set; } = "";
    public string ErrorMessage { get; set; } = "";
    public string StackTrace { get; set; } = "";
    public string Source { get; set; } = ""; // frontend / backend
    public string UserId { get; set; } = "";
    public string UserAgent { get; set; } = "";
    public string IpAddress { get; set; } = "";
    public bool IsResolved { get; set; } = false;
    public string ResolvedBy { get; set; } = ""; // AI / Admin
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class AIWorkLog
{
    public int Id { get; set; }
    public string TaskType { get; set; } = ""; // BugFix / Deploy / Maintenance
    public string Description { get; set; } = "";
    public string FilesModified { get; set; } = ""; // JSON array of files
    public string ErrorLogId { get; set; } = ""; // related error if bug fix
    public string Status { get; set; } = ""; // pending / working / completed / failed
    public string Result { get; set; } = "";
    public int DurationSeconds { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
}

public class ClaudeTask
{
    public int Id { get; set; }
    public string Prompt { get; set; } = "";
    public string Status { get; set; } = "pending"; // pending, running, done, failed
    public string Result { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
