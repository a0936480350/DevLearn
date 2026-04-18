using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml;
using DotNetLearning.Data;

namespace DotNetLearning.Controllers;

/// <summary>
/// SEO infrastructure endpoints:
///   GET /robots.txt   — crawler directives
///   GET /sitemap.xml  — dynamic sitemap of every published chapter + key landing pages
///
/// These are intentionally NOT under /api/ or static files because:
///   1. robots.txt must be at site root exactly
///   2. sitemap.xml benefits from a consistent URL for search engine registration
///   3. Dynamic generation means new chapters auto-appear without file edits
/// </summary>
public class SeoController : Controller
{
    private readonly AppDbContext _db;

    public SeoController(AppDbContext db) { _db = db; }

    [Route("robots.txt")]
    public IActionResult Robots()
    {
        var host = $"{Request.Scheme}://{Request.Host}";
        var body = $@"User-agent: *
Allow: /
Disallow: /Admin
Disallow: /Account/Login
Disallow: /Account/Register
Disallow: /Payment/EcpayReturn
Disallow: /Payment/EcpayOrderResult
Disallow: /SharedFile/Upload

Sitemap: {host}/sitemap.xml
";
        return Content(body, "text/plain", Encoding.UTF8);
    }

    [Route("sitemap.xml")]
    [ResponseCache(Duration = 3600)] // 1 hr cache
    public async Task<IActionResult> Sitemap()
    {
        var host = $"{Request.Scheme}://{Request.Host}";

        var chapters = await _db.Chapters
            .AsNoTracking()
            .Where(c => c.IsPublished)
            .Select(c => new { c.Slug, c.Category })
            .ToListAsync();

        var teachers = await _db.Teachers
            .AsNoTracking()
            .Where(t => t.IsApproved && t.IsActive)
            .Select(t => t.Id)
            .ToListAsync();

        using var ms = new MemoryStream();
        var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true, Async = true };
        using (var w = XmlWriter.Create(ms, settings))
        {
            await w.WriteStartDocumentAsync();
            await w.WriteStartElementAsync(null, "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            // Static landing pages
            string[] rootPaths =
            {
                "/", "/QnA", "/Buddy", "/Leaderboard",
                "/Teacher", "/Announcement", "/CodeTutor", "/Monopoly",
                "/BugDetective", "/Arena", "/Battle",
            };
            foreach (var p in rootPaths) WriteUrl(w, $"{host}{p}", "weekly", p == "/" ? "1.0" : "0.7");

            // Chapter pages — the core SEO asset
            foreach (var c in chapters)
            {
                var prio = c.Category switch
                {
                    "csharp" or "aspnet" => "0.9",   // main audience
                    _ => "0.7",
                };
                WriteUrl(w, $"{host}/Home/Chapter/{c.Slug}", "monthly", prio);
            }

            // Teacher detail pages
            foreach (var tId in teachers)
                WriteUrl(w, $"{host}/Teacher/Detail/{tId}", "monthly", "0.6");

            await w.WriteEndElementAsync();
            await w.WriteEndDocumentAsync();
        }

        return File(ms.ToArray(), "application/xml");
    }

    private static void WriteUrl(XmlWriter w, string loc, string changefreq, string priority)
    {
        w.WriteStartElement("url");
        w.WriteElementString("loc", loc);
        w.WriteElementString("lastmod", DateTime.UtcNow.ToString("yyyy-MM-dd"));
        w.WriteElementString("changefreq", changefreq);
        w.WriteElementString("priority", priority);
        w.WriteEndElement();
    }
}
