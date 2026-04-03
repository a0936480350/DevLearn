using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;

namespace DotNetLearning.Controllers;

public class AnnouncementController : Controller
{
    private readonly AppDbContext _db;
    public AnnouncementController(AppDbContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        var now = DateTime.Now;
        var announcements = await _db.Announcements
            .Where(a => a.IsVisible && (a.ExpiresAt == null || a.ExpiresAt > now))
            .OrderByDescending(a => a.IsPinned)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync();
        return View(announcements);
    }
}
