using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;
    public AccountController(AppDbContext db) => _db = db;

    private string GetAnonId() => HttpContext.Session.GetString("SessionId") ?? "";

    // 個人檔案頁面
    public async Task<IActionResult> Profile()
    {
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == GetAnonId());
        return View(user);
    }

    // 註冊（匿名→實名）
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string nickname, string email, string password, string confirmPassword)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null) return RedirectToAction("Register");

        // Password validation
        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
        {
            ViewBag.Error = "密碼至少需要 4 個字元";
            return View();
        }
        if (password != confirmPassword)
        {
            ViewBag.Error = "兩次輸入的密碼不一致";
            return View();
        }

        // Check if nickname already taken
        var nickTaken = await _db.SiteUsers.FirstOrDefaultAsync(u => u.Nickname == nickname && u.IsRegistered);
        if (nickTaken != null && nickTaken.AnonymousId != anonId)
        {
            ViewBag.Error = "此暱稱已被使用，請換一個";
            return View();
        }

        // Check if email already registered
        var existing = await _db.SiteUsers.FirstOrDefaultAsync(u => u.Email == email && u.IsRegistered);
        if (existing != null && existing.AnonymousId != anonId)
        {
            ViewBag.Error = "此 Email 已被註冊，請使用登入功能";
            return View();
        }

        user.Nickname = nickname;
        user.Email = email;
        user.IsRegistered = true;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        await _db.SaveChangesAsync();

        // 註冊完導到登入頁，讓用戶重新登入
        ViewBag.Success = "註冊成功！請使用暱稱和 Email 登入";
        return RedirectToAction("Login");
    }

    // 登入（用 Email 找回資料，合併到當前 cookie）
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string nickname, string email, string? password)
    {
        if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(email))
        {
            ViewBag.Error = "請輸入暱稱和 Email";
            return View();
        }

        var registered = await _db.SiteUsers.FirstOrDefaultAsync(u =>
            u.Email == email && u.Nickname == nickname && u.IsRegistered);
        if (registered == null)
        {
            ViewBag.Error = "暱稱或 Email 不正確，請確認後重試";
            return View();
        }

        // Password verification
        if (!string.IsNullOrEmpty(registered.PasswordHash))
        {
            if (string.IsNullOrWhiteSpace(password) || !BCrypt.Net.BCrypt.Verify(password, registered.PasswordHash))
            {
                ViewBag.Error = "密碼錯誤";
                return View();
            }
        }
        else
        {
            // Old user without password - allow login but prompt to set one
            ViewBag.Warning = "建議到個人資料頁設定密碼以提升帳號安全性";
        }

        // 把當前 cookie 的匿名 ID 替換成已註冊用戶的 ID
        var currentAnonId = GetAnonId();
        var currentUser = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == currentAnonId);

        // 更新 cookie 指向已註冊的 anonymousId
        Response.Cookies.Append("DotNetLearner", registered.AnonymousId, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            MaxAge = TimeSpan.FromDays(365),
            Path = "/"
        });
        HttpContext.Session.SetString("SessionId", registered.AnonymousId);
        HttpContext.Session.SetString("sid", registered.AnonymousId);

        // Delete the orphaned anonymous record (if different)
        if (currentUser != null && currentUser.Id != registered.Id && !currentUser.IsRegistered)
        {
            _db.SiteUsers.Remove(currentUser);
            await _db.SaveChangesAsync();
        }

        // 統一登入：判斷角色導向
        // Admin email check
        if (email == "1234@hotmail.com")
        {
            HttpContext.Response.Cookies.Append("AdminAuth", "pxmart-admin-verified-2026", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddHours(8),
                SameSite = SameSiteMode.Strict
            });
            return Redirect("/Admin/Dashboard");
        }

        // Teacher check
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == registered.Id && t.IsApproved);
        if (teacher != null)
        {
            return Redirect("/Teacher/Dashboard");
        }

        return RedirectToAction("Profile");
    }

    // 登出
    public IActionResult Logout()
    {
        // Clear the anonymous cookie - give new identity
        Response.Cookies.Delete("DotNetLearner");
        HttpContext.Session.Clear();
        return Redirect("/");
    }

    // 更新個人資料
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(string nickname, string email, IFormFile? avatar)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null || !user.IsRegistered) return RedirectToAction("Login");

        if (!string.IsNullOrEmpty(nickname)) user.Nickname = nickname;
        if (!string.IsNullOrEmpty(email)) user.Email = email;

        if (avatar != null && avatar.Length > 0 && avatar.Length <= 2 * 1024 * 1024)
        {
            using var ms = new MemoryStream();
            await avatar.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());
            var contentType = avatar.ContentType ?? "image/jpeg";
            user.AvatarUrl = $"data:{contentType};base64,{base64}";
        }

        await _db.SaveChangesAsync();
        return RedirectToAction("Profile");
    }

    // API: 取得用戶身份狀態（Admin / Registered / Guest）
    [HttpGet]
    public async Task<IActionResult> Status()
    {
        var anonId = HttpContext.Session.GetString("SessionId") ?? "";
        var isAdmin = HttpContext.Request.Cookies.ContainsKey("AdminAuth");
        var user = HttpContext.Items["CurrentUser"] as DotNetLearning.Models.SiteUser;

        var userId = user?.Id ?? 0;
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == userId && t.IsApproved);

        return Json(new {
            isAdmin,
            isTeacher = teacher != null,
            isRegistered = user?.IsRegistered ?? false,
            nickname = user?.Nickname ?? "訪客",
            anonymousId = anonId.Substring(0, Math.Min(8, anonId.Length))
        });
    }

    // API: 取得當前用戶資料
    [HttpGet]
    public async Task<IActionResult> Me()
    {
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == GetAnonId());
        if (user == null) return Json(new { exists = false });
        return Json(new {
            exists = true,
            user.Nickname,
            user.Email,
            user.IsRegistered,
            user.TotalScore,
            user.BadgeLevel,
            user.ChaptersCompleted,
            user.QuizzesTaken
        });
    }

    // API: 快速設定暱稱（不需要 email）
    [HttpPost]
    public async Task<IActionResult> SetNickname([FromBody] SetNickRequest req)
    {
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == GetAnonId());
        if (user == null) return BadRequest();
        user.Nickname = req.Nickname;
        await _db.SaveChangesAsync();
        return Ok(new { user.Nickname });
    }

    // ── 收藏老師 ──
    [HttpGet]
    public async Task<IActionResult> MyFavoriteTeachers()
    {
        var anonId = GetAnonId();
        var favs = await _db.FavoriteTeachers.Where(f => f.StudentId == anonId).ToListAsync();
        var teacherIds = favs.Select(f => f.TeacherId).ToList();
        var teachers = await _db.Teachers.Where(t => teacherIds.Contains(t.Id)).ToListAsync();
        return Json(teachers.Select(t => new {
            t.Id, t.Name, t.Title, t.AverageRating, t.HourlyRate, t.PhotoUrl,
            favId = favs.First(f => f.TeacherId == t.Id).Id
        }));
    }

    [HttpPost]
    public async Task<IActionResult> FavoriteTeacher([FromBody] FavTeacherReq req)
    {
        var anonId = GetAnonId();
        var exists = await _db.FavoriteTeachers.AnyAsync(f => f.StudentId == anonId && f.TeacherId == req.TeacherId);
        if (exists) return Ok(new { success = false, message = "已收藏" });
        _db.FavoriteTeachers.Add(new FavoriteTeacher { StudentId = anonId, TeacherId = req.TeacherId });
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> UnfavoriteTeacher([FromBody] UnfavReq req)
    {
        var anonId = GetAnonId();
        var fav = await _db.FavoriteTeachers.FirstOrDefaultAsync(f => f.Id == req.FavId && f.StudentId == anonId);
        if (fav == null) return NotFound();
        _db.FavoriteTeachers.Remove(fav);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ── 我的預約 ──
    [HttpGet]
    public async Task<IActionResult> MyBookings()
    {
        var anonId = GetAnonId();
        var bookings = await _db.Bookings.Where(b => b.StudentId == anonId)
            .OrderByDescending(b => b.CreatedAt).ToListAsync();
        var teacherIds = bookings.Select(b => b.TeacherId).Distinct().ToList();
        var teachers = await _db.Teachers.Where(t => teacherIds.Contains(t.Id)).ToDictionaryAsync(t => t.Id, t => t.Name);
        return Json(bookings.Select(b => new {
            b.Id, b.TeacherId, TeacherName = teachers.GetValueOrDefault(b.TeacherId, "未知"),
            b.BookingDate, b.TimeSlot, b.Status, b.CreatedAt,
            hasReview = _db.Reviews.Any(r => r.BookingId == b.Id && r.StudentId == anonId)
        }));
    }

    [HttpPost]
    public async Task<IActionResult> CancelBooking([FromBody] CancelBookingReq req)
    {
        var anonId = GetAnonId();
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == req.BookingId && b.StudentId == anonId);
        if (booking == null) return NotFound();
        if (booking.Status != "pending") return BadRequest(new { error = "只能取消待確認的預約" });
        booking.Status = "cancelled";
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ── 我的想法 ──
    [HttpGet]
    public async Task<IActionResult> MyIdeas()
    {
        var anonId = GetAnonId();
        var ideas = await _db.Ideas.Where(i => i.SessionId == anonId)
            .OrderByDescending(i => i.CreatedAt).Take(50).ToListAsync();
        return Json(ideas.Select(i => new { i.Id, i.Content, i.ChapterTitle, i.CreatedAt,
            replyCount = _db.Replies.Count(r => r.IdeaId == i.Id) }));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteIdea([FromBody] DeleteReq req)
    {
        var anonId = GetAnonId();
        var idea = await _db.Ideas.FirstOrDefaultAsync(i => i.Id == req.Id && i.SessionId == anonId);
        if (idea == null) return NotFound();
        _db.Replies.RemoveRange(_db.Replies.Where(r => r.IdeaId == req.Id));
        _db.Ideas.Remove(idea);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateIdea([FromBody] UpdateIdeaReq req)
    {
        var anonId = GetAnonId();
        var idea = await _db.Ideas.FirstOrDefaultAsync(i => i.Id == req.Id && i.SessionId == anonId);
        if (idea == null) return NotFound();
        idea.Content = req.Content;
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ── 我的問答 ──
    [HttpGet]
    public async Task<IActionResult> MyQuestions()
    {
        var anonId = GetAnonId();
        var questions = await _db.QnAs.Where(q => q.SessionId == anonId)
            .OrderByDescending(q => q.CreatedAt).Take(50).ToListAsync();
        return Json(questions.Select(q => new { q.Id, q.Question, q.IsSolved, q.CreatedAt,
            answerCount = _db.QnAAnswers.Count(a => a.QnAId == q.Id) }));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteQuestion([FromBody] DeleteReq req)
    {
        var anonId = GetAnonId();
        var q = await _db.QnAs.FirstOrDefaultAsync(q => q.Id == req.Id && q.SessionId == anonId);
        if (q == null) return NotFound();
        _db.QnAAnswers.RemoveRange(_db.QnAAnswers.Where(a => a.QnAId == req.Id));
        _db.QnAs.Remove(q);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ── 我的聊天 ──
    [HttpGet]
    public async Task<IActionResult> MyChats()
    {
        var anonId = GetAnonId();
        var chats = await _db.ChatMessages.Where(c => c.SessionId == anonId)
            .OrderByDescending(c => c.SentAt).Take(20).ToListAsync();
        return Json(chats.Select(c => new { c.Id, c.Message, c.SentAt }));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteChat([FromBody] DeleteReq req)
    {
        var anonId = GetAnonId();
        var chat = await _db.ChatMessages.FirstOrDefaultAsync(c => c.Id == req.Id && c.SessionId == anonId);
        if (chat == null) return NotFound();
        _db.ChatMessages.Remove(chat);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ── 我的測驗 ──
    [HttpGet]
    public async Task<IActionResult> MyQuizzes()
    {
        var anonId = GetAnonId();
        var quizzes = await _db.QuizAttempts.Where(q => q.SessionId == anonId)
            .OrderByDescending(q => q.TakenAt).Take(50).ToListAsync();
        return Json(quizzes.Select(q => new { q.Id, q.ChapterId, q.Score, q.Total, q.TakenAt }));
    }
}

public record SetNickRequest(string Nickname);
public record DeleteReq(int Id);
public record UpdateIdeaReq(int Id, string Content);
public record FavTeacherReq(int TeacherId);
public record UnfavReq(int FavId);
public record CancelBookingReq(int BookingId);
