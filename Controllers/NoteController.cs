using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using System.Text;

namespace DotNetLearning.Controllers;

public class NoteController : Controller
{
    private readonly AppDbContext _db;
    public NoteController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Download(string slug)
    {
        var chapter = await _db.Chapters.FirstOrDefaultAsync(c => c.Slug == slug);
        if (chapter is null) return NotFound();

        var content = $"# {chapter.Title}\n\n{chapter.Content}";
        var bytes = Encoding.UTF8.GetBytes(content);
        return File(bytes, "text/markdown; charset=utf-8", $"{slug}.md");
    }

    public async Task<IActionResult> DownloadAll(string category)
    {
        var chapters = await _db.Chapters
            .Where(c => c.Category == category && c.IsPublished)
            .OrderBy(c => c.Order)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine($"# .NET 學習筆記 - {GetCategoryName(category)}");
        sb.AppendLine($"\n> 匯出時間：{DateTime.Now:yyyy-MM-dd HH:mm}\n");
        sb.AppendLine("---\n");

        foreach (var ch in chapters)
        {
            sb.AppendLine($"# {ch.Icon} {ch.Title}\n");
            sb.AppendLine(ch.Content);
            sb.AppendLine("\n---\n");
        }

        var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/markdown; charset=utf-8", $"dotnet-notes-{category}.md");
    }

    private static string GetCategoryName(string category) => category switch
    {
        "csharp"   => "C# 基礎",
        "aspnet"   => "ASP.NET Core",
        "database" => "資料庫與 ORM",
        "network"  => "網路 TCP/IP",
        "security" => "資安",
        "docker"   => "Docker",
        _          => category
    };
}
