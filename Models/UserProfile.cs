namespace DotNetLearning.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "";
    public int TotalScore { get; set; }
    public int QuizzesTaken { get; set; }
    public int ChaptersCompleted { get; set; }
    public string BadgeLevel { get; set; } = "newbie"; // newbie, beginner, intermediate, advanced, expert, master
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastActiveAt { get; set; } = DateTime.Now;
}
