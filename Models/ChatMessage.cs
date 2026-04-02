namespace DotNetLearning.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Message { get; set; } = "";
    public string AvatarEmoji { get; set; } = "\U0001F600";
    public DateTime SentAt { get; set; } = DateTime.Now;
    public int? ReplyToId { get; set; }
    public string? ReplyToNickname { get; set; }
    public string? ReplyToPreview { get; set; }
}

public class ChatReaction
{
    public int Id { get; set; }
    public int ChatMessageId { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Emoji { get; set; } = "";
    public DateTime ReactedAt { get; set; } = DateTime.Now;
}
