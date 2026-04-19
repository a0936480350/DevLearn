using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AdminController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    private bool IsAdmin()
    {
        return HttpContext.Request.Cookies.ContainsKey("AdminAuth")
            && HttpContext.Request.Cookies["AdminAuth"] == "pxmart-admin-verified-2026";
    }

    private IActionResult RedirectIfNotAdmin()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        return null!;
    }

    // ========== Login / Logout ==========

    [HttpGet]
    public IActionResult Login()
    {
        if (IsAdmin()) return RedirectToAction("Dashboard");
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var adminUser = _config["Admin:Username"];
        var adminPass = _config["Admin:Password"];

        if (username == adminUser && password == adminPass)
        {
            HttpContext.Response.Cookies.Append("AdminAuth", "pxmart-admin-verified-2026", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddHours(8),
                SameSite = SameSiteMode.Strict
            });
            return RedirectToAction("Dashboard");
        }

        ViewBag.Error = "帳號或密碼錯誤";
        return View();
    }

    public IActionResult Logout()
    {
        // 清除所有 cookie 和 session，完全登出
        HttpContext.Response.Cookies.Delete("AdminAuth");
        HttpContext.Response.Cookies.Delete("DotNetLearner");
        HttpContext.Session.Clear();
        return Redirect("/");
    }

    // ========== Dashboard ==========

    public async Task<IActionResult> Dashboard()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var today = DateTime.Today;

        ViewBag.TotalChapters = await _db.Chapters.CountAsync();
        ViewBag.TotalQuestions = await _db.Questions.CountAsync();
        ViewBag.TotalUsers = await _db.UserProfiles.CountAsync();
        ViewBag.TodayCheckIns = await _db.CheckIns.CountAsync(c => c.CheckInDate.Date == today);
        ViewBag.TotalQuizAttempts = await _db.QuizAttempts.CountAsync();
        ViewBag.TotalIdeas = await _db.Ideas.CountAsync();

        // Last 7 days activity
        var last7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(-6 + i)).ToList();
        var quizByDay = await _db.QuizAttempts
            .Where(q => q.TakenAt.Date >= today.AddDays(-6))
            .GroupBy(q => q.TakenAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();
        var checkInByDay = await _db.CheckIns
            .Where(c => c.CheckInDate.Date >= today.AddDays(-6))
            .GroupBy(c => c.CheckInDate.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();

        ViewBag.Last7Days = last7Days;
        ViewBag.QuizByDay = last7Days.Select(d => quizByDay.FirstOrDefault(q => q.Date == d)?.Count ?? 0).ToList();
        ViewBag.CheckInByDay = last7Days.Select(d => checkInByDay.FirstOrDefault(c => c.Date == d)?.Count ?? 0).ToList();

        // Category stats
        ViewBag.CategoryStats = await _db.Chapters
            .GroupBy(c => c.Category)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .ToListAsync();

        // Recent 20 quiz attempts
        ViewBag.RecentQuizzes = await _db.QuizAttempts
            .OrderByDescending(q => q.TakenAt)
            .Take(20)
            .ToListAsync();

        // Recent 10 chat messages
        ViewBag.RecentChats = await _db.ChatMessages
            .OrderByDescending(c => c.SentAt)
            .Take(10)
            .ToListAsync();

        // Recent 10 ideas
        var recentIdeas = await _db.Ideas
            .OrderByDescending(i => i.CreatedAt)
            .Take(10)
            .ToListAsync();
        var ideaIds = recentIdeas.Select(i => i.Id).ToList();
        var replyCounts = await _db.Replies
            .Where(r => ideaIds.Contains(r.IdeaId))
            .GroupBy(r => r.IdeaId)
            .Select(g => new { IdeaId = g.Key, Count = g.Count() })
            .ToListAsync();
        ViewBag.RecentIdeas = recentIdeas;
        ViewBag.ReplyCounts = replyCounts.ToDictionary(r => r.IdeaId, r => r.Count);

        // Speed run top 10
        ViewBag.SpeedRunTop10 = await _db.SpeedRuns
            .OrderByDescending(s => s.ScorePoints)
            .Take(10)
            .ToListAsync();

        // Daily challenge stats
        var todayChallenge = await _db.DailyChallenges
            .FirstOrDefaultAsync(d => d.ChallengeDate.Date == today);
        ViewBag.TodayChallenge = todayChallenge;
        if (todayChallenge != null)
        {
            var dailyAttempts = await _db.DailyAttempts
                .Where(a => a.DailyChallengeId == todayChallenge.Id)
                .ToListAsync();
            ViewBag.DailyTotalAttempts = dailyAttempts.Count;
            ViewBag.DailyAccuracy = dailyAttempts.Count > 0
                ? (int)Math.Round(dailyAttempts.Count(a => a.IsCorrect) * 100.0 / dailyAttempts.Count)
                : 0;
        }
        else
        {
            ViewBag.DailyTotalAttempts = 0;
            ViewBag.DailyAccuracy = 0;
        }

        // System health metrics
        var now = DateTime.Now;
        var last24h = now.AddHours(-24);
        var lastHour = now.AddHours(-1);

        // Error stats
        ViewBag.TotalErrors = await _db.ErrorLogs.CountAsync();
        ViewBag.UnresolvedErrors = await _db.ErrorLogs.CountAsync(e => !e.IsResolved);
        ViewBag.ErrorsLast24h = await _db.ErrorLogs.CountAsync(e => e.CreatedAt >= last24h);
        ViewBag.ErrorsLastHour = await _db.ErrorLogs.CountAsync(e => e.CreatedAt >= lastHour);

        // AI Work stats
        ViewBag.AIWorkTotal = await _db.AIWorkLogs.CountAsync();
        ViewBag.AIWorkCompleted = await _db.AIWorkLogs.CountAsync(w => w.Status == "completed");
        ViewBag.AIWorkFailed = await _db.AIWorkLogs.CountAsync(w => w.Status == "failed");

        // Recent errors for quick view
        ViewBag.RecentErrors = await _db.ErrorLogs
            .OrderByDescending(e => e.CreatedAt)
            .Take(5)
            .ToListAsync();

        // Error Scanner stats
        ViewBag.UnresolvedErrors = await _db.ErrorLogs.CountAsync(e => !e.IsResolved);
        ViewBag.AutoResolvedErrors = await _db.ErrorLogs.CountAsync(e => e.IsResolved && e.ResolvedBy == "AutoScanner");
        ViewBag.LastScanLog = await _db.AIWorkLogs
            .Where(w => w.TaskType == "ErrorScan")
            .OrderByDescending(w => w.CompletedAt)
            .FirstOrDefaultAsync();

        // 客戶回報統計
        ViewBag.TotalTickets = await _db.SupportTickets.CountAsync();
        ViewBag.PendingTickets = await _db.SupportTickets.CountAsync(t => t.Status == "pending");
        ViewBag.RecentTickets = await _db.SupportTickets
            .OrderByDescending(t => t.CreatedAt)
            .Take(5)
            .ToListAsync();

        // System stats
        ViewBag.TotalSiteUsers = await _db.SiteUsers.CountAsync();
        ViewBag.RegisteredUsers = await _db.SiteUsers.CountAsync(u => u.IsRegistered);
        ViewBag.MemberCount = await _db.SiteUsers.CountAsync(u => u.Role == "member");
        ViewBag.TeacherRoleCount = await _db.SiteUsers.CountAsync(u => u.Role == "teacher");
        ViewBag.AdminCount = await _db.SiteUsers.CountAsync(u => u.Role == "admin");
        ViewBag.BannedCount = await _db.SiteUsers.CountAsync(u => u.IsBanned);
        ViewBag.TotalTeachers = await _db.Teachers.CountAsync();
        ViewBag.ApprovedTeachers = await _db.Teachers.CountAsync(t => t.IsApproved);
        ViewBag.TotalBookings = await _db.Bookings.CountAsync();
        ViewBag.PendingBookings = await _db.Bookings.CountAsync(b => b.Status == "pending");

        return View();
    }

    // ========== Users ==========

    public async Task<IActionResult> Users(string? search)
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var query = _db.SiteUsers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u => u.Nickname.Contains(search));

        ViewBag.Search = search;
        ViewBag.SiteUsers = await query.OrderByDescending(u => u.LastActiveAt).ToListAsync();

        // Keep UserProfiles for backward compatibility
        var profileQuery = _db.UserProfiles.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            profileQuery = profileQuery.Where(u => u.Nickname.Contains(search));
        ViewBag.Users = await profileQuery.OrderByDescending(u => u.LastActiveAt).ToListAsync();
        return View();
    }

    // ========== Chapters ==========

    public async Task<IActionResult> Chapters()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var chapters = await _db.Chapters.OrderBy(c => c.Category).ThenBy(c => c.Order).ToListAsync();
        var totalUsers = await _db.UserProfiles.CountAsync();
        var completionCounts = await _db.Progresses
            .Where(p => p.IsCompleted)
            .GroupBy(p => p.ChapterId)
            .Select(g => new { ChapterId = g.Key, Count = g.Count() })
            .ToListAsync();

        ViewBag.Chapters = chapters;
        ViewBag.TotalUsers = totalUsers;
        ViewBag.CompletionCounts = completionCounts.ToDictionary(c => c.ChapterId, c => c.Count);
        return View();
    }

    // ========== Questions ==========

    public async Task<IActionResult> Questions()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        ViewBag.Questions = await _db.Questions
            .Include(q => q.Chapter)
            .OrderBy(q => q.ChapterId)
            .ToListAsync();
        return View();
    }

    // ========== Quizzes ==========

    public async Task<IActionResult> Quizzes()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        ViewBag.Quizzes = await _db.QuizAttempts
            .OrderByDescending(q => q.TakenAt)
            .Take(200)
            .ToListAsync();
        return View();
    }

    // ========== Ideas ==========

    public async Task<IActionResult> Ideas()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var ideas = await _db.Ideas.OrderByDescending(i => i.CreatedAt).ToListAsync();
        var ideaIds = ideas.Select(i => i.Id).ToList();
        var replyCounts = await _db.Replies
            .Where(r => ideaIds.Contains(r.IdeaId))
            .GroupBy(r => r.IdeaId)
            .Select(g => new { IdeaId = g.Key, Count = g.Count() })
            .ToListAsync();

        ViewBag.Ideas = ideas;
        ViewBag.ReplyCounts = replyCounts.ToDictionary(r => r.IdeaId, r => r.Count);
        return View();
    }

    // ========== Chat Messages ==========

    public async Task<IActionResult> ChatMessages()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        ViewBag.Messages = await _db.ChatMessages
            .OrderByDescending(c => c.SentAt)
            .Take(200)
            .ToListAsync();
        return View();
    }

    // ========== Bug Challenges ==========

    public async Task<IActionResult> BugChallenges()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var challenges = await _db.BugChallenges.ToListAsync();
        var attemptStats = (await _db.BugAttempts.ToListAsync())
            .GroupBy(a => a.BugChallengeId)
            .Select(g => new { ChallengeId = g.Key, Total = g.Count(), Solved = g.Count(a => a.Solved) })
            .ToList();

        ViewBag.Challenges = challenges;
        ViewBag.StatsTotal = attemptStats.ToDictionary(a => a.ChallengeId, a => a.Total);
        ViewBag.StatsSolved = attemptStats.ToDictionary(a => a.ChallengeId, a => a.Solved);
        return View();
    }

    // ========== Speed Runs ==========

    public async Task<IActionResult> SpeedRuns()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        ViewBag.SpeedRuns = await _db.SpeedRuns
            .OrderByDescending(s => s.ScorePoints)
            .Take(200)
            .ToListAsync();
        return View();
    }

    // ========== Daily Challenges ==========

    public async Task<IActionResult> DailyChallenges()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var challenges = await _db.DailyChallenges
            .OrderByDescending(d => d.ChallengeDate)
            .Take(60)
            .ToListAsync();

        var challengeIds = challenges.Select(c => c.Id).ToList();
        var attemptStats = (await _db.DailyAttempts
            .Where(a => challengeIds.Contains(a.DailyChallengeId))
            .ToListAsync())
            .GroupBy(a => a.DailyChallengeId)
            .Select(g => new { ChallengeId = g.Key, Total = g.Count(), Correct = g.Count(a => a.IsCorrect) })
            .ToList();

        ViewBag.Challenges = challenges;
        ViewBag.StatsTotal = attemptStats.ToDictionary(a => a.ChallengeId, a => a.Total);
        ViewBag.StatsCorrect = attemptStats.ToDictionary(a => a.ChallengeId, a => a.Correct);
        return View();
    }

    // ========== Arena Submissions ==========

    public async Task<IActionResult> ArenaSubmissions()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        ViewBag.Challenges = await _db.ArenaChallenges.OrderByDescending(a => a.StartDate).ToListAsync();
        ViewBag.Submissions = await _db.ArenaSubmissions
            .OrderByDescending(s => s.SubmittedAt)
            .Take(200)
            .ToListAsync();
        return View();
    }

    // ========== Study Buddies ==========

    public async Task<IActionResult> StudyBuddies()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        ViewBag.Buddies = await _db.StudyBuddies.OrderByDescending(b => b.RegisteredAt).ToListAsync();
        ViewBag.Matches = await _db.BuddyMatches.OrderByDescending(m => m.MatchedAt).ToListAsync();
        return View();
    }

    // ========== 老師管理 ==========

    public async Task<IActionResult> Teachers()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        ViewBag.Teachers = await _db.Teachers.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ApproveTeacher([FromBody] AdminDeleteReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var t = await _db.Teachers.FindAsync(req.Id);
        if (t == null) return NotFound();
        t.IsApproved = true;
        await LogAudit("Approve", "Teacher", req.Id, $"Approved teacher: {t.Name}");
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> RejectTeacher([FromBody] RejectTeacherReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var t = await _db.Teachers.FindAsync(req.Id);
        if (t == null) return NotFound();
        t.IsApproved = false;
        t.IsActive = false;
        t.RejectReason = req.Reason ?? "未通過審核";
        await LogAudit("Reject", "Teacher", req.Id, $"Rejected: {t.Name}, reason: {t.RejectReason}");
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ========== 預約管理 ==========

    public async Task<IActionResult> Bookings()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        ViewBag.Bookings = await _db.Bookings.OrderByDescending(b => b.CreatedAt).Take(200).ToListAsync();
        return View();
    }

    // ========== 操作紀錄 ==========

    public async Task<IActionResult> AuditLogs()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        ViewBag.Logs = await _db.AuditLogs.OrderByDescending(l => l.CreatedAt).Take(500).ToListAsync();
        return View();
    }

    // ========== 客服工單 ==========

    public async Task<IActionResult> SupportTickets()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        ViewBag.Tickets = await _db.SupportTickets
            .OrderByDescending(t => t.CreatedAt)
            .Take(200)
            .ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ReplyTicket([FromBody] ReplyTicketReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var ticket = await _db.SupportTickets.FindAsync(req.Id);
        if (ticket == null) return NotFound();
        ticket.AdminReply = req.Reply;
        ticket.Status = "resolved";
        ticket.RepliedAt = DateTime.Now;
        await LogAudit("Reply", "SupportTicket", req.Id, $"Replied to ticket from {ticket.UserName}");
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ========== 錯誤日誌 ==========

    public async Task<IActionResult> ErrorLogs()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        ViewBag.Logs = await _db.ErrorLogs.OrderByDescending(l => l.CreatedAt).Take(500).ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResolveError([FromBody] AdminDeleteReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var log = await _db.ErrorLogs.FindAsync(req.Id);
        if (log == null) return NotFound();
        log.IsResolved = true;
        log.ResolvedBy = "Admin";
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ========== 公告欄 CRUD ==========

    public async Task<IActionResult> Announcements()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        ViewBag.List = await _db.Announcements.OrderByDescending(a => a.IsPinned).ThenByDescending(a => a.CreatedAt).ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AnnouncementCreate([FromBody] AnnouncementReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var ann = new DotNetLearning.Models.Announcement
        {
            Title = req.Title ?? "",
            Content = req.Content ?? "",
            Type = req.Type ?? "info",
            IsPinned = req.IsPinned,
            IsVisible = req.IsVisible,
            CreatedBy = "Admin",
            CreatedAt = DateTime.Now,
            ExpiresAt = req.ExpiresAt
        };
        _db.Announcements.Add(ann);
        await _db.SaveChangesAsync();
        return Ok(new { success = true, id = ann.Id });
    }

    [HttpPost]
    public async Task<IActionResult> AnnouncementUpdate([FromBody] AnnouncementReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var ann = await _db.Announcements.FindAsync(req.Id);
        if (ann == null) return NotFound();
        ann.Title = req.Title ?? ann.Title;
        ann.Content = req.Content ?? ann.Content;
        ann.Type = req.Type ?? ann.Type;
        ann.IsPinned = req.IsPinned;
        ann.IsVisible = req.IsVisible;
        ann.ExpiresAt = req.ExpiresAt;
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> AnnouncementDelete([FromBody] AdminDeleteReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var ann = await _db.Announcements.FindAsync(req.Id);
        if (ann == null) return NotFound();
        _db.Announcements.Remove(ann);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> AnnouncementToggle([FromBody] AdminDeleteReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var ann = await _db.Announcements.FindAsync(req.Id);
        if (ann == null) return NotFound();
        ann.IsVisible = !ann.IsVisible;
        await _db.SaveChangesAsync();
        return Ok(new { success = true, isVisible = ann.IsVisible });
    }

    // ========== AI 工作紀錄 ==========

    public async Task<IActionResult> AIWorkLogs()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        ViewBag.Logs = await _db.AIWorkLogs.OrderByDescending(l => l.StartedAt).Take(200).ToListAsync();
        return View();
    }

    // ========== Notifications API ==========

    [HttpGet]
    public async Task<IActionResult> Notifications()
    {
        if (!IsAdmin()) return Unauthorized();

        var now = DateTime.UtcNow;
        var last24h = now.AddHours(-24);

        var notifications = new List<object>();

        // 新預約（pending 狀態）
        var pendingBookings = await _db.Bookings
            .Where(b => b.Status == "pending")
            .OrderByDescending(b => b.CreatedAt)
            .Take(5)
            .ToListAsync();
        foreach (var b in pendingBookings)
        {
            var teacher = await _db.Teachers.FindAsync(b.TeacherId);
            notifications.Add(new {
                type = "booking",
                icon = "\uD83D\uDCC5",
                message = $"{b.StudentName} \u9810\u7D04\u4E86 {teacher?.Name ?? "\u8001\u5E2B"} {b.TimeSlot}",
                time = b.CreatedAt,
                actionUrl = "/Admin/Bookings"
            });
        }

        // 待審核老師
        var pendingTeachers = await _db.Teachers
            .Where(t => !t.IsApproved && t.IsActive)
            .OrderByDescending(t => t.CreatedAt)
            .Take(5)
            .ToListAsync();
        foreach (var t in pendingTeachers)
        {
            notifications.Add(new {
                type = "teacher",
                icon = "\uD83D\uDC68\u200D\uD83C\uDFEB",
                message = $"{t.Name} \u7533\u8ACB\u6210\u70BA\u8001\u5E2B\uFF0C\u7B49\u5F85\u5BE9\u6838",
                time = t.CreatedAt,
                actionUrl = "/Admin/Teachers"
            });
        }

        // 新客服工單（pending）
        var pendingTickets = await _db.SupportTickets
            .Where(t => t.Status == "pending")
            .OrderByDescending(t => t.CreatedAt)
            .Take(5)
            .ToListAsync();
        foreach (var t in pendingTickets)
        {
            notifications.Add(new {
                type = "support",
                icon = "\uD83D\uDCDE",
                message = $"\u65B0\u5BA2\u670D\u5DE5\u55AE\uFF1A{t.Category} - {t.Content.Substring(0, Math.Min(30, t.Content.Length))}...",
                time = t.CreatedAt,
                actionUrl = "/Admin/SupportTickets"
            });
        }

        // 未解決錯誤
        var unresolvedErrors = await _db.ErrorLogs
            .Where(e => !e.IsResolved)
            .OrderByDescending(e => e.CreatedAt)
            .Take(3)
            .ToListAsync();
        foreach (var e in unresolvedErrors)
        {
            notifications.Add(new {
                type = "error",
                icon = "\uD83D\uDEA8",
                message = $"\u932F\u8AA4\uFF1A{e.ErrorMessage.Substring(0, Math.Min(40, e.ErrorMessage.Length))}",
                time = e.CreatedAt,
                actionUrl = "/Admin/ErrorLogs"
            });
        }

        // 最近聊天（24小時內）
        var recentChatsCount = await _db.ChatMessages
            .Where(c => c.SentAt > last24h)
            .CountAsync();
        if (recentChatsCount > 0)
        {
            notifications.Add(new {
                type = "chat",
                icon = "\uD83D\uDCAC",
                message = $"\u904E\u53BB 24 \u5C0F\u6642\u6709 {recentChatsCount} \u5247\u65B0\u804A\u5929\u8A0A\u606F",
                time = now,
                actionUrl = "/Admin/ChatMessages"
            });
        }

        // Sort by time descending
        var sorted = notifications
            .OrderByDescending(n => ((dynamic)n).time)
            .Take(15)
            .ToList();

        return Json(new { count = sorted.Count, items = sorted });
    }

    // ========== User Role & Ban Management ==========

    [HttpPost]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var user = await _db.SiteUsers.FindAsync(req.Id);
        if (user == null) return NotFound();

        var oldRole = user.Role;
        user.Role = req.Role;

        // If promoting to teacher, check if Teacher record exists
        if (req.Role == "teacher")
        {
            user.IsRegistered = true;
            var existingTeacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id);
            if (existingTeacher == null)
            {
                _db.Teachers.Add(new Teacher { SiteUserId = user.Id, Name = user.Nickname, IsApproved = true, IsActive = true });
            }
            else
            {
                existingTeacher.IsApproved = true;
                existingTeacher.IsActive = true;
            }
        }

        // If promoting to admin
        if (req.Role == "admin")
        {
            user.IsRegistered = true;
        }

        // If demoting from teacher
        if (oldRole == "teacher" && req.Role != "teacher")
        {
            var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id);
            if (teacher != null)
            {
                teacher.IsActive = false;
            }
        }

        await LogAudit("Update", "User", req.Id, $"Changed role: {oldRole} → {req.Role} for {user.Nickname}");
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> BanUser([FromBody] BanUserReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var user = await _db.SiteUsers.FindAsync(req.Id);
        if (user == null) return NotFound();
        user.IsBanned = req.Ban;
        user.BanReason = req.Reason ?? "";
        await LogAudit(req.Ban ? "Ban" : "Unban", "User", req.Id, $"{(req.Ban ? "Banned" : "Unbanned")} user: {user.Nickname}, reason: {req.Reason}");
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ========== Audit Helper ==========

    private async Task LogAudit(string action, string entityType, int entityId, string details)
    {
        var anonId = HttpContext.Session.GetString("SessionId") ?? "";
        var user = HttpContext.Items["CurrentUser"] as DotNetLearning.Models.SiteUser;
        _db.AuditLogs.Add(new AuditLog
        {
            UserId = anonId,
            UserName = user?.Nickname ?? "Admin",
            UserRole = HttpContext.Request.Cookies.ContainsKey("AdminAuth") ? "admin" : "user",
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details
        });
        await _db.SaveChangesAsync();
    }

    // ========== Delete Actions (Moderation) ==========

    [HttpPost]
    public async Task<IActionResult> DeleteIdea(int id)
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var idea = await _db.Ideas.FindAsync(id);
        if (idea != null)
        {
            var replies = _db.Replies.Where(r => r.IdeaId == id);
            _db.Replies.RemoveRange(replies);
            _db.Ideas.Remove(idea);
            await LogAudit("Delete", "Idea", id, $"Deleted idea: {idea.Content.Substring(0, Math.Min(50, idea.Content.Length))}");
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Ideas");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteChat(int id)
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var msg = await _db.ChatMessages.FindAsync(id);
        if (msg != null)
        {
            _db.ChatMessages.Remove(msg);
            await LogAudit("Delete", "ChatMessage", id, $"Deleted chat by {msg.Nickname}");
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("ChatMessages");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteQnA(int id)
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var qna = await _db.QnAs.FindAsync(id);
        if (qna != null)
        {
            var answers = _db.QnAAnswers.Where(a => a.QnAId == id);
            _db.QnAAnswers.RemoveRange(answers);
            _db.QnAs.Remove(qna);
            await LogAudit("Delete", "QnA", id, $"Deleted QnA: {qna.Question.Substring(0, Math.Min(50, qna.Question.Length))}");
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Dashboard");
    }

    // ========== Chapter CRUD ==========

    // 新增章節頁面
    public IActionResult CreateChapter()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        return View();
    }

    // 新增章節 POST
    [HttpPost]
    public async Task<IActionResult> CreateChapter(string title, string slug, string content,
        string category, string level, string icon, int order)
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var chapter = new Chapter
        {
            Title = title,
            Slug = slug ?? title.ToLower().Replace(" ", "-"),
            Content = content ?? "",
            Category = category ?? "csharp",
            Level = level ?? "beginner",
            Icon = icon ?? "📖",
            Order = order,
            IsPublished = true
        };
        _db.Chapters.Add(chapter);
        await LogAudit("Create", "Chapter", 0, $"Created chapter: {title}");
        await _db.SaveChangesAsync();
        return RedirectToAction("Chapters");
    }

    // 編輯章節頁面
    public async Task<IActionResult> EditChapter(int id)
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        var chapter = await _db.Chapters.FindAsync(id);
        if (chapter == null) return RedirectToAction("Chapters");
        return View(chapter);
    }

    // 編輯章節 POST
    [HttpPost]
    public async Task<IActionResult> EditChapter(int id, string title, string slug, string content,
        string category, string level, string icon, int order, bool isPublished)
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        var chapter = await _db.Chapters.FindAsync(id);
        if (chapter == null) return RedirectToAction("Chapters");

        chapter.Title = title;
        chapter.Slug = slug;
        chapter.Content = content;
        chapter.Category = category;
        chapter.Level = level;
        chapter.Icon = icon;
        chapter.Order = order;
        chapter.IsPublished = isPublished;

        await LogAudit("Update", "Chapter", id, $"Updated chapter: {title}");
        await _db.SaveChangesAsync();
        return RedirectToAction("Chapters");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateChapter([FromBody] UpdateChapterReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var chapter = await _db.Chapters.FindAsync(req.Id);
        if (chapter == null) return NotFound();
        if (!string.IsNullOrEmpty(req.Title)) chapter.Title = req.Title;
        if (!string.IsNullOrEmpty(req.Content)) chapter.Content = req.Content;
        if (req.IsPublished.HasValue) chapter.IsPublished = req.IsPublished.Value;
        await LogAudit("Update", "Chapter", req.Id, $"Updated chapter: {chapter.Title}");
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteChapter([FromBody] AdminDeleteReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var chapter = await _db.Chapters.FindAsync(req.Id);
        if (chapter == null) return NotFound();
        await LogAudit("Delete", "Chapter", req.Id, $"Deleted chapter: {chapter.Title}");
        _db.Questions.RemoveRange(_db.Questions.Where(q => q.ChapterId == req.Id));
        _db.Progresses.RemoveRange(_db.Progresses.Where(p => p.ChapterId == req.Id));
        _db.QuizAttempts.RemoveRange(_db.QuizAttempts.Where(q => q.ChapterId == req.Id));
        _db.Chapters.Remove(chapter);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteQuestion([FromBody] AdminDeleteReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var q = await _db.Questions.FindAsync(req.Id);
        if (q == null) return NotFound();
        await LogAudit("Delete", "Question", req.Id, $"Deleted question ID: {req.Id}");
        _db.Questions.Remove(q);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser([FromBody] AdminDeleteReq req)
    {
        if (!IsAdmin()) return Unauthorized();
        var profile = await _db.UserProfiles.FindAsync(req.Id);
        if (profile != null)
        {
            var sessionId = profile.SessionId;
            await LogAudit("Delete", "User", req.Id, $"Deleted user: {profile.Nickname}");

            // Clean up all related data for this user
            _db.Bookings.RemoveRange(_db.Bookings.Where(b => b.StudentId == sessionId));
            _db.ChatMessages.RemoveRange(_db.ChatMessages.Where(c => c.SessionId == sessionId));
            _db.QuizAttempts.RemoveRange(_db.QuizAttempts.Where(q => q.SessionId == sessionId));
            _db.Progresses.RemoveRange(_db.Progresses.Where(p => p.SessionId == sessionId));
            _db.FavoriteTeachers.RemoveRange(_db.FavoriteTeachers.Where(f => f.StudentId == sessionId));
            _db.PrivateMessages.RemoveRange(_db.PrivateMessages.Where(m => m.SenderId == sessionId || m.ReceiverId == sessionId));
            _db.SupportTickets.RemoveRange(_db.SupportTickets.Where(t => t.UserId == sessionId));
            _db.CheckIns.RemoveRange(_db.CheckIns.Where(c => c.SessionId == sessionId));

            // Clean up ideas and their replies
            var ideaIds = await _db.Ideas.Where(i => i.SessionId == sessionId).Select(i => i.Id).ToListAsync();
            _db.Replies.RemoveRange(_db.Replies.Where(r => ideaIds.Contains(r.IdeaId)));
            _db.Ideas.RemoveRange(_db.Ideas.Where(i => i.SessionId == sessionId));

            // Clean up QnAs and their answers
            var qnaIds = await _db.QnAs.Where(q => q.SessionId == sessionId).Select(q => q.Id).ToListAsync();
            _db.QnAAnswers.RemoveRange(_db.QnAAnswers.Where(a => qnaIds.Contains(a.QnAId)));
            _db.QnAs.RemoveRange(_db.QnAs.Where(q => q.SessionId == sessionId));

            // Also delete corresponding SiteUser if exists
            var siteUser = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == sessionId);
            if (siteUser != null) _db.SiteUsers.Remove(siteUser);

            _db.UserProfiles.Remove(profile);
            await _db.SaveChangesAsync();
            return Ok(new { success = true });
        }
        return NotFound();
    }

    // ════════════════════════════════════════════════════════
    //  一次性：建立「Mike 老師」帳號（與 admin 平台帳號分離）
    //  呼叫方式（帶 AdminAuth cookie）：
    //    POST /Admin/SetupMikeTeacher?photoUrl=/SharedFile/Preview/2
    //
    //  身分架構：
    //    admin-master-001  → 平台管理者（純 admin 後台用，不是某個人）
    //    mike-teacher      → Mike 的實際使用者身分（teacher role，可接受預約）
    //                        Teacher record 掛在這個帳號下
    //
    //  Mike 日常使用：
    //    /Admin/Login    → 帶 AdminAuth cookie 進後台
    //    /Account/Login  → email=a0936480350@hotmail.com, password=abcd1234
    //                       → 拿到 DotNetLearner cookie = mike-teacher 身分
    //    兩個 cookie 同時帶 → 既是管理員也是 Mike 老師
    // ════════════════════════════════════════════════════════
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SetupMikeTeacher([FromQuery] string? photoUrl = null)
    {
        if (!IsAdmin()) return Unauthorized(new { error = "admin_only" });

        // ───── 1. 還原 admin-master-001 為純 admin（不是 Mike 本人） ─────
        var adminAcc = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == "admin-master-001");
        if (adminAcc == null)
        {
            adminAcc = new SiteUser { AnonymousId = "admin-master-001" };
            _db.SiteUsers.Add(adminAcc);
        }
        adminAcc.Nickname = "Admin";
        adminAcc.Email = "admin@hotmail.com";
        adminAcc.IsRegistered = true;
        adminAcc.EmailVerified = true;
        adminAcc.Role = "admin";
        adminAcc.LoginMethod = "legacy";
        adminAcc.BadgeLevel = "master";
        if (string.IsNullOrEmpty(adminAcc.PasswordHash))
            adminAcc.PasswordHash = BCrypt.Net.BCrypt.HashPassword("abcd1234");

        // ───── 2. 建立 / 更新 mike-teacher 帳號 ─────
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == "mike-teacher" || u.Email == "mike@hotmail.com");
        if (user == null)
        {
            user = new SiteUser { AnonymousId = "mike-teacher" };
            _db.SiteUsers.Add(user);
        }
        user.AnonymousId = "mike-teacher";
        user.Nickname = "邱瀚賢 Mike";
        user.Email = "mike@hotmail.com";
        user.IsRegistered = true;
        user.EmailVerified = true;
        user.Role = "teacher";
        user.LoginMethod = "email";
        user.BadgeLevel = "master";
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("abcd1234");
        await _db.SaveChangesAsync();

        // ───── 2.5 清掉舊的 admin 底下掛的 Teacher 紀錄（避免重複） ─────
        if (adminAcc.Id != 0)
        {
            var orphanTeachers = await _db.Teachers.Where(t => t.SiteUserId == adminAcc.Id).ToListAsync();
            if (orphanTeachers.Any())
            {
                _db.Teachers.RemoveRange(orphanTeachers);
                await _db.SaveChangesAsync();
            }
        }

        // ───── 2. 老師資料 ─────
        var bio = @"在音樂、語言、程式三個領域橫跨十多年實戰，把學生一路帶到能自己獨立思考的程度，是我一貫的風格。

【音樂教學｜2012 起，累積百位以上學生】
• 電木吉他、烏克麗麗、爵士鼓、錄音工程、創作編曲
• 文藻外語大學畢、日本 OSM 大阪音樂專門學校、東京国立音楽院
• 師承：四分衛吉他手小郭、法蘭黛江鎮宇、日本 OSM 井上央一、菊田俊介、台灣 JAZZ 莊智淵
• 教學據點：AmazingTalker 線上、文藻熱音社、吉他補給站、夢響佳音、多間補習班
• 英國皇家樂理檢定五級

【日文教學｜JLPT N1，文藻外語大學日文系畢業】
• 日本工作 5 年（大阪 / 東京）、台灣純日商 4 年
• N1 / N2 / N3 檢定準備、商務日文、日常會話、履歷面試輔導
• 實際處理過日方客戶往來、社長翻譯、合約書信、會議同步
• 日本工商販売士 3 級

【程式教學｜2023 起投入，目前任職全聯資訊部全端工程師】
• C# / ASP.NET Core MVC / Minimal API / WinForms / .NET MAUI
• SQL Server / PostgreSQL / SQLite、EF Core、Redis
• HTML / CSS / JavaScript / Vue 3 / jQuery
• 實戰經驗：全聯 WMS 倉儲、ERP 模組、台灣萬事達金流（GOMYPAY / LinePay / Apple Pay 串接）
• 本平台 DevLearn 就是我自己從零做出來的作品

【教學風格】
重視基本功 + 思考邏輯，不死背。24 小時內回覆課後問題。
課程可客製化：從零開始、面試衝刺、證照班、深度專案陪跑都接。

【授課方式】
線上（Google Meet / Zoom）或實體（台北市可面授）。";

        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id);
        if (teacher == null)
        {
            teacher = new Teacher { SiteUserId = user.Id, CreatedAt = DateTime.Now };
            _db.Teachers.Add(teacher);
        }
        teacher.Name = "邱瀚賢 Mike";
        teacher.Title = "音樂／日文／程式 跨領域教師";
        teacher.Bio = bio;
        teacher.PhotoUrl = photoUrl ?? "";
        teacher.VideoUrl = "https://www.youtube.com/watch?v=Ozm6jYHIctY";
        teacher.SkillsJson = System.Text.Json.JsonSerializer.Serialize(new[] {
            "電木吉他", "烏克麗麗", "爵士鼓", "樂理", "錄音工程", "創作編曲",
            "日文 N1", "商務日文", "日檢輔導",
            "C#", "ASP.NET Core", "SQL", "Vue", "全端開發", "金流串接"
        });
        teacher.ExperienceYears = 10;
        teacher.Education = "文藻外語大學日文系 · 東京国立音楽院 総合音楽 · OSM 大阪音樂專門學校";
        teacher.HourlyRate = 600;
        teacher.TrialPrice = 400;
        teacher.IsApproved = true;
        teacher.IsActive = true;
        teacher.PhotoFileName = "";
        teacher.DiplomaFileName = "";
        teacher.CustomSkills = "音樂教學10年, JLPT N1, 日本5年工作經驗, 全端工程師";
        teacher.UpdatedAt = DateTime.Now;

        await _db.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            admin = new { adminAcc.Id, adminAcc.AnonymousId, adminAcc.Nickname, adminAcc.Email, adminAcc.Role },
            teacher_account = new { user.Id, user.AnonymousId, user.Nickname, user.Email, user.Role, password = "abcd1234" },
            teacher = new { teacher.Id, teacher.Name, teacher.Title, teacher.HourlyRate, teacher.TrialPrice, teacher.IsApproved, teacher.PhotoUrl },
            login_url = "/Account/Login"
        });
    }
}

public record UpdateChapterReq(int Id, string? Title, string? Content, bool? IsPublished);
public record AdminDeleteReq(int Id);
public record RejectTeacherReq(int Id, string? Reason);
public record ReplyTicketReq(int Id, string Reply);
public record AnnouncementReq(int Id, string? Title, string? Content, string? Type, bool IsPinned, bool IsVisible, DateTime? ExpiresAt);
public record ChangeRoleReq(int Id, string Role);
public record BanUserReq(int Id, bool Ban, string? Reason);
