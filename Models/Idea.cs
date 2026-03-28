namespace DotNetLearning.Models;

public class Idea
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "匿名";
    public int? ChapterId { get; set; }       // null = 通用想法
    public string ChapterTitle { get; set; } = "";
    public string Content { get; set; } = "";
    public int Likes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
