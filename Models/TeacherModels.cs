namespace DotNetLearning.Models;

public class Teacher
{
    public int Id { get; set; }
    public int SiteUserId { get; set; }
    public string Name { get; set; } = "";
    public string Title { get; set; } = ""; // e.g. "資深 .NET 工程師"
    public string Bio { get; set; } = "";
    public string PhotoUrl { get; set; } = "";
    public string VideoUrl { get; set; } = "";
    public string SkillsJson { get; set; } = "[]"; // ["C#","ASP.NET","SQL"]
    public int ExperienceYears { get; set; }
    public string Education { get; set; } = "";
    public int HourlyRate { get; set; } // NTD per hour
    public int TrialPrice { get; set; } // trial lesson price
    public bool IsApproved { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public double AverageRating { get; set; }
    public int TotalStudents { get; set; }
    public int TotalLessons { get; set; }
    public string DiplomaFileName { get; set; } = ""; // 學歷證書檔名
    public string PhotoFileName { get; set; } = ""; // 照片檔名
    public string CustomSkills { get; set; } = ""; // 自訂額外技能（逗號分隔）
    public string RejectReason { get; set; } = ""; // Admin 拒絕理由
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class TeacherPost
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = ""; // Markdown content
    public string Type { get; set; } = "article"; // article/video/resource
    public string VideoUrl { get; set; } = ""; // YouTube embed URL
    public string ResourceUrl { get; set; } = ""; // download link
    public int Views { get; set; }
    public int Likes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class TeacherPostComment
{
    public int Id { get; set; }
    public int TeacherPostId { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class TeacherSlot
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public int DayOfWeek { get; set; } // 0=Sun, 1=Mon...6=Sat
    public string StartTime { get; set; } = "09:00";
    public string EndTime { get; set; } = "10:00";
    public bool IsAvailable { get; set; } = true;
}

public class Booking
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public string StudentId { get; set; } = ""; // SiteUser.AnonymousId
    public string StudentName { get; set; } = "";
    public DateTime BookingDate { get; set; }
    public string TimeSlot { get; set; } = "";
    public string Status { get; set; } = "pending"; // pending/confirmed/completed/cancelled
    public string StudentNote { get; set; } = "";
    public string TeacherNote { get; set; } = "";
    public string StudentEmail { get; set; } = "";
    public string StudentPhone { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class Review
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public string StudentId { get; set; } = "";
    public string StudentName { get; set; } = "";
    public int BookingId { get; set; }
    public int Rating { get; set; } // 1-5
    public string Comment { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class FavoriteTeacher
{
    public int Id { get; set; }
    public string StudentId { get; set; } = ""; // SiteUser.AnonymousId
    public int TeacherId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class SupportTicket
{
    public int Id { get; set; }
    public string UserId { get; set; } = ""; // AnonymousId
    public string UserName { get; set; } = "";
    public string UserEmail { get; set; } = "";
    public string Category { get; set; } = ""; // 技術問題/付款問題/老師相關/預約問題/功能建議/其他
    public string Content { get; set; } = "";
    public string Status { get; set; } = "pending"; // pending/processing/resolved/closed
    public string AdminReply { get; set; } = "";
    public bool IsReplyRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? RepliedAt { get; set; }
}

public class PrivateMessage
{
    public int Id { get; set; }
    public string SenderId { get; set; } = ""; // AnonymousId
    public string SenderName { get; set; } = "";
    public string ReceiverId { get; set; } = ""; // AnonymousId
    public string ReceiverName { get; set; } = "";
    public int? TeacherId { get; set; } // which teacher conversation this belongs to
    public string Message { get; set; } = "";
    public bool IsRead { get; set; } = false;
    public DateTime SentAt { get; set; } = DateTime.Now;
}

public class AuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string UserRole { get; set; } = ""; // admin/teacher/student/guest
    public string Action { get; set; } = ""; // Create/Update/Delete/Login/Approve
    public string EntityType { get; set; } = ""; // Chapter/Question/User/Teacher/Booking
    public int EntityId { get; set; }
    public string Details { get; set; } = ""; // JSON details
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
