namespace DotNetLearning.Models;

public class SiteUser
{
    public int Id { get; set; }
    public string AnonymousId { get; set; } = ""; // cookie-based unique ID
    public string Nickname { get; set; } = "匿名學習者";
    public string? Email { get; set; }
    public bool IsRegistered { get; set; } = false;
    public int TotalScore { get; set; }
    public int QuizzesTaken { get; set; }
    public int ChaptersCompleted { get; set; }
    public string BadgeLevel { get; set; } = "newbie";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastActiveAt { get; set; } = DateTime.Now;
    public string? AvatarUrl { get; set; }
    public string PasswordHash { get; set; } = ""; // BCrypt hashed password
}
