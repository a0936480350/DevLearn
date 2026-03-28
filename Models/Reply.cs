namespace DotNetLearning.Models;

public class Reply
{
    public int Id { get; set; }
    public int IdeaId { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "匿名";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
