namespace DotNetLearning.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Message { get; set; } = "";
    public string AvatarEmoji { get; set; } = "\U0001F600";
    public DateTime SentAt { get; set; } = DateTime.Now;
}
