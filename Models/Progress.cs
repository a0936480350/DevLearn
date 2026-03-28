namespace DotNetLearning.Models;

public class Progress
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public int ChapterId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CompletedAt { get; set; }
}

public class QuizAttempt
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public int ChapterId { get; set; }
    public int Score { get; set; }
    public int Total { get; set; }
    public DateTime TakenAt { get; set; } = DateTime.Now;
}
