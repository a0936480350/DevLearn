using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using DotNetLearning.Data;
using DotNetLearning.Models;
using DotNetLearning.Services;

namespace DotNetLearning.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;
    private readonly EmailService _email;
    public AccountController(AppDbContext db, EmailService email) { _db = db; _email = email; }

    private string GetAnonId() => HttpContext.Session.GetString("SessionId") ?? "";

    private async Task<bool> IsBanned()
    {
        var user = HttpContext.Items["CurrentUser"] as SiteUser;
        return user?.IsBanned ?? false;
    }

    // 個人檔案頁面
    public async Task<IActionResult> Profile()
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        ViewBag.BattleStat = await _db.BattleStats.FirstOrDefaultAsync(s => s.UserId == anonId);
        return View(user);
    }

    // 註冊（匿名→實名）
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string passwordConfirm, string? referralCode)
    {
        // Validate email format
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@') || !email.Contains('.'))
        {
            ViewBag.Error = "請輸入有效的 Email 地址";
            return View();
        }

        // Validate password match
        if (password != passwordConfirm)
        {
            ViewBag.Error = "密碼不一致";
            return View();
        }

        // Validate password rules
        if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
        {
            ViewBag.Error = "密碼需至少 8 字元，包含大寫、小寫字母和數字";
            return View();
        }

        // Check email not already registered
        if (await _db.SiteUsers.AnyAsync(u => u.Email == email && u.IsRegistered))
        {
            ViewBag.Error = "此 Email 已被註冊，請使用登入功能";
            return View();
        }

        // Get current anonymous user via cookie
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null) return RedirectToAction("Register");

        // Generate nickname from email
        var nickname = email.Split('@')[0];
        if (nickname.Length > 20) nickname = nickname[..20];

        user.Email = email;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        user.Nickname = nickname;
        user.IsRegistered = true;
        user.Role = "member";
        user.LoginMethod = "email";
        user.EmailVerified = false;

        // Generate referral code
        user.ReferralCode = (user.Nickname.Length >= 4 ? user.Nickname[..4] : user.Nickname).ToUpper() + new Random().Next(1000, 9999);

        // Handle referral
        if (!string.IsNullOrWhiteSpace(referralCode))
        {
            var referrer = await _db.SiteUsers.FirstOrDefaultAsync(u => u.ReferralCode == referralCode);
            if (referrer != null)
            {
                user.ReferredBy = referralCode;
                referrer.ReferralCount++;
            }
        }

        // Generate verification token
        var token = Guid.NewGuid().ToString("N");
        user.VerificationToken = token;
        user.VerificationExpiry = DateTime.UtcNow.AddMinutes(1);

        await _db.SaveChangesAsync();

        // Send verification email
        await _email.SendVerificationEmailAsync(email, token, $"{Request.Scheme}://{Request.Host}");

        return View("VerifyEmailSent");
    }

    // 登入（用 Email 找回資料，合併到當前 cookie）
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "請輸入 Email 和密碼";
            return View();
        }

        var registered = await _db.SiteUsers.FirstOrDefaultAsync(u => u.Email == email && u.IsRegistered);
        if (registered == null)
        {
            ViewBag.Error = "帳號不存在";
            return View();
        }

        // Google-only account
        if (registered.LoginMethod == "google" && string.IsNullOrEmpty(registered.PasswordHash))
        {
            ViewBag.Error = "此帳號使用 Google 登入，請點擊 Google 按鈕";
            return View();
        }

        // Legacy accounts (old nickname+email login)
        if (registered.LoginMethod == "legacy")
        {
            if (!string.IsNullOrEmpty(registered.PasswordHash))
            {
                if (!BCrypt.Net.BCrypt.Verify(password, registered.PasswordHash))
                {
                    ViewBag.Error = "密碼錯誤";
                    return View();
                }
            }
            // else: legacy account without password — allow email-only match for backward compat
            // Show hint to set password
            TempData["Info"] = "建議您設定密碼以提升帳號安全性";
        }

        // Email login method
        if (registered.LoginMethod == "email")
        {
            if (!registered.EmailVerified)
            {
                ViewBag.Message = "您的 Email 尚未驗證，請先完成驗證。";
                return View("VerifyEmailSent");
            }
            if (!BCrypt.Net.BCrypt.Verify(password, registered.PasswordHash))
            {
                ViewBag.Error = "密碼錯誤";
                return View();
            }
        }

        // --- Common login flow (set cookies, session, role-based redirect) ---
        var currentAnonId = GetAnonId();
        var currentUser = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == currentAnonId);

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

        registered.LastActiveAt = DateTime.Now;

        if (currentUser != null && currentUser.Id != registered.Id && !currentUser.IsRegistered)
        {
            _db.SiteUsers.Remove(currentUser);
        }
        await _db.SaveChangesAsync();

        // Admin email check
        if (email == "1234@hotmail.com")
        {
            registered.Role = "admin";
            await _db.SaveChangesAsync();
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
            registered.Role = "teacher";
            await _db.SaveChangesAsync();
            return Redirect("/Teacher/Dashboard");
        }

        return RedirectToAction("Profile");
    }

    // Legacy login (backward compat for old nickname+email accounts)
    [HttpPost]
    public async Task<IActionResult> LegacyLogin(string nickname, string email)
    {
        if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(email))
        {
            ViewBag.Error = "請輸入暱稱和 Email";
            return View("Login");
        }

        var registered = await _db.SiteUsers.FirstOrDefaultAsync(u =>
            u.Email == email && u.Nickname == nickname && u.IsRegistered && u.LoginMethod == "legacy");
        if (registered == null)
        {
            ViewBag.Error = "暱稱或 Email 不正確，請確認後重試";
            return View("Login");
        }

        var currentAnonId = GetAnonId();
        var currentUser = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == currentAnonId);

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

        registered.LastActiveAt = DateTime.Now;

        if (currentUser != null && currentUser.Id != registered.Id && !currentUser.IsRegistered)
        {
            _db.SiteUsers.Remove(currentUser);
        }
        await _db.SaveChangesAsync();

        if (email == "1234@hotmail.com")
        {
            registered.Role = "admin";
            await _db.SaveChangesAsync();
            HttpContext.Response.Cookies.Append("AdminAuth", "pxmart-admin-verified-2026", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddHours(8),
                SameSite = SameSiteMode.Strict
            });
            return Redirect("/Admin/Dashboard");
        }

        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == registered.Id && t.IsApproved);
        if (teacher != null)
        {
            registered.Role = "teacher";
            await _db.SaveChangesAsync();
            return Redirect("/Teacher/Dashboard");
        }

        return RedirectToAction("Profile");
    }

    // Google OAuth Login
    [HttpGet]
    public IActionResult GoogleLogin(string? returnUrl = null)
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleCallback", new { returnUrl })
        };
        return Challenge(props, "Google");
    }

    // Google OAuth Callback
    [HttpGet]
    public async Task<IActionResult> GoogleCallback(string? returnUrl = null)
    {
        var result = await HttpContext.AuthenticateAsync("Google");
        if (!result.Succeeded) return RedirectToAction("Login");

        var claims = result.Principal!.Claims;
        var googleId = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var gEmail = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
        var picture = claims.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value
                      ?? claims.FirstOrDefault(c => c.Type == "picture")?.Value;

        if (string.IsNullOrEmpty(googleId) || string.IsNullOrEmpty(gEmail))
            return RedirectToAction("Login");

        // Find existing user by GoogleId or Email
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.GoogleId == googleId)
                   ?? await _db.SiteUsers.FirstOrDefaultAsync(u => u.Email == gEmail && u.IsRegistered);

        if (user == null)
        {
            // Create new user from current anonymous user
            var anonId = HttpContext.Request.Cookies["DotNetLearner"] ?? Guid.NewGuid().ToString("N");
            user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
            if (user == null)
            {
                user = new SiteUser { AnonymousId = anonId };
                _db.SiteUsers.Add(user);
            }
            user.Email = gEmail;
            user.Nickname = name ?? gEmail.Split('@')[0];
            user.IsRegistered = true;
            user.Role = "member";
            user.LoginMethod = "google";
            user.EmailVerified = true;
            user.GoogleId = googleId;
            if (!string.IsNullOrEmpty(picture)) user.AvatarUrl = picture;
            user.ReferralCode = (user.Nickname.Length >= 4 ? user.Nickname[..4] : user.Nickname).ToUpper() + new Random().Next(1000, 9999);
        }
        else
        {
            // Link Google account to existing user
            user.GoogleId = googleId;
            if (string.IsNullOrEmpty(user.AvatarUrl) && !string.IsNullOrEmpty(picture))
                user.AvatarUrl = picture;
            user.LoginMethod = "google";
            user.EmailVerified = true;
        }

        user.LastActiveAt = DateTime.Now;
        await _db.SaveChangesAsync();

        // Set cookies
        HttpContext.Response.Cookies.Append("DotNetLearner", user.AnonymousId, new CookieOptions { MaxAge = TimeSpan.FromDays(365), HttpOnly = true, SameSite = SameSiteMode.Lax });
        HttpContext.Session.SetString("SessionId", user.AnonymousId);
        HttpContext.Session.SetString("sid", user.AnonymousId);

        // Admin check
        if (user.Email == "1234@hotmail.com")
        {
            user.Role = "admin";
            await _db.SaveChangesAsync();
            HttpContext.Response.Cookies.Append("AdminAuth", "pxmart-admin-verified-2026", new CookieOptions { MaxAge = TimeSpan.FromHours(8), HttpOnly = true });
            return Redirect("/Admin/Dashboard");
        }

        // Teacher check
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id && t.IsApproved);
        if (teacher != null) { user.Role = "teacher"; await _db.SaveChangesAsync(); return Redirect("/Teacher/Dashboard"); }

        return Redirect(returnUrl ?? "/Account/Profile");
    }

    // Email Verification
    [HttpGet]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.VerificationToken == token && u.VerificationExpiry > DateTime.UtcNow);
        if (user == null)
        {
            ViewBag.Error = "驗證連結已過期或無效，請重新發送驗證信。";
            return View("VerifyEmailSent");
        }
        user.EmailVerified = true;
        user.VerificationToken = null;
        user.VerificationExpiry = null;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Email 驗證成功！請登入。";
        return RedirectToAction("Login");
    }

    // Verify Email Sent page
    [HttpGet]
    public IActionResult VerifyEmailSent()
    {
        return View();
    }

    // Resend Verification
    [HttpPost]
    public async Task<IActionResult> ResendVerification(string email)
    {
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.Email == email && u.IsRegistered && !u.EmailVerified);
        if (user != null)
        {
            user.VerificationToken = Guid.NewGuid().ToString("N");
            user.VerificationExpiry = DateTime.UtcNow.AddMinutes(1);
            await _db.SaveChangesAsync();
            await _email.SendVerificationEmailAsync(email, user.VerificationToken, $"{Request.Scheme}://{Request.Host}");
        }
        ViewBag.Message = "如果此 Email 已註冊，驗證信已重新發送。";
        return View("VerifyEmailSent");
    }

    // Forgot Password
    [HttpGet]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.Email == email && u.IsRegistered);
        if (user != null)
        {
            user.VerificationToken = Guid.NewGuid().ToString("N");
            user.VerificationExpiry = DateTime.UtcNow.AddMinutes(1);
            await _db.SaveChangesAsync();
            await _email.SendPasswordResetEmailAsync(email, user.VerificationToken, $"{Request.Scheme}://{Request.Host}");
        }
        ViewBag.Message = "如果此 Email 已註冊，重設密碼信已發送。";
        return View();
    }

    // Reset Password
    [HttpGet]
    public async Task<IActionResult> ResetPassword(string token)
    {
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.VerificationToken == token && u.VerificationExpiry > DateTime.UtcNow);
        if (user == null) { TempData["Error"] = "連結已過期，請重新申請。"; return RedirectToAction("ForgotPassword"); }
        ViewBag.Token = token;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(string token, string password, string passwordConfirm)
    {
        if (password != passwordConfirm) { ViewBag.Error = "密碼不一致"; ViewBag.Token = token; return View(); }
        if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
        { ViewBag.Error = "密碼需至少 8 字元，包含大小寫和數字"; ViewBag.Token = token; return View(); }

        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.VerificationToken == token && u.VerificationExpiry > DateTime.UtcNow);
        if (user == null) { TempData["Error"] = "連結已過期"; return RedirectToAction("ForgotPassword"); }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        user.LoginMethod = "email";
        user.EmailVerified = true;
        user.VerificationToken = null;
        user.VerificationExpiry = null;
        await _db.SaveChangesAsync();
        TempData["Success"] = "密碼已重設，請登入。";
        return RedirectToAction("Login");
    }

    // 登出
    public IActionResult Logout()
    {
        // 完全清除所有身份，乾淨登出
        Response.Cookies.Delete("DotNetLearner");
        Response.Cookies.Delete("AdminAuth");
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
        var isAdminCookie = HttpContext.Request.Cookies.ContainsKey("AdminAuth");
        var user = HttpContext.Items["CurrentUser"] as DotNetLearning.Models.SiteUser;

        var userId = user?.Id ?? 0;
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == userId && t.IsApproved);

        return Json(new {
            isAdmin = user?.Role == "admin" || isAdminCookie,
            isTeacher = user?.Role == "teacher",
            isRegistered = user?.IsRegistered ?? false,
            isBanned = user?.IsBanned ?? false,
            role = user?.Role ?? "guest",
            nickname = user?.Nickname ?? "訪客",
            email = user?.Email ?? "",
            anonymousId = anonId.Length > 8 ? anonId.Substring(0, 8) : anonId
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

    // API: 用暱稱查詢用戶 AnonymousId（供私訊功能使用）
    [HttpGet]
    public async Task<IActionResult> LookupByNickname(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
            return Json(new { found = false });

        var user = await _db.SiteUsers
            .FirstOrDefaultAsync(u => u.Nickname == nickname && u.IsRegistered);

        if (user == null)
            return Json(new { found = false });

        return Json(new { found = true, anonymousId = user.AnonymousId, nickname = user.Nickname });
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
