namespace DotNetLearning.Models;

public class Chapter
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Content { get; set; } = "";  // Markdown
    public string Category { get; set; } = ""; // C#, ASP.NET, DB, Network, Security, Docker
    public int Order { get; set; }
    public string Level { get; set; } = "beginner"; // beginner, intermediate, advanced
    public string Icon { get; set; } = "📄";
    public bool IsPublished { get; set; } = true;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
