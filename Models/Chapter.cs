namespace DotNetLearning.Models;

public class Chapter
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Content { get; set; } = "";  // Markdown (zh-Hant)
    public string? TitleJa { get; set; }       // 日本語タイトル
    public string? ContentJa { get; set; }     // 日本語 Markdown（空→fallback to Content）
    public string? TitleEn { get; set; }       // English title
    public string? ContentEn { get; set; }     // English Markdown (empty → fallback to Content)
    public string Category { get; set; } = ""; // C#, ASP.NET, DB, Network, Security, Docker
    public int Order { get; set; }
    public string Level { get; set; } = "beginner"; // beginner, intermediate, advanced
    public string Icon { get; set; } = "📄";
    public bool IsPublished { get; set; } = true;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
