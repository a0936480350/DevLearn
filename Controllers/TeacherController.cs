using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class TeacherController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;
    public TeacherController(AppDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

    private string GetAnonId() => HttpContext.Session.GetString("SessionId") ?? "";

    // ========== 公開頁面 ==========

    // 老師列表（公開）
    public async Task<IActionResult> Index(string? search, string? skill, int? minPrice, int? maxPrice)
    {
        var query = _db.Teachers.Where(t => t.IsApproved && t.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Name.Contains(search) || t.Title.Contains(search) || t.Bio.Contains(search));

        if (!string.IsNullOrWhiteSpace(skill))
            query = query.Where(t => t.SkillsJson.Contains(skill));

        if (minPrice.HasValue)
            query = query.Where(t => t.HourlyRate >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(t => t.HourlyRate <= maxPrice.Value);

        ViewBag.Teachers = await query.OrderByDescending(t => t.AverageRating).ThenByDescending(t => t.TotalStudents).ToListAsync();
        ViewBag.Search = search;
        ViewBag.Skill = skill;
        ViewBag.MinPrice = minPrice;
        ViewBag.MaxPrice = maxPrice;
        return View();
    }

    // 老師詳情（公開）
    public async Task<IActionResult> Detail(int id)
    {
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == id && t.IsApproved && t.IsActive);
        if (teacher == null) return RedirectToAction("Index");

        var teacherUser = await _db.SiteUsers.FirstOrDefaultAsync(u => u.Id == teacher.SiteUserId);
        ViewBag.TeacherAnonymousId = teacherUser?.AnonymousId ?? "";

        ViewBag.Teacher = teacher;
        ViewBag.Slots = await _db.TeacherSlots.Where(s => s.TeacherId == id && s.IsAvailable).OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime).ToListAsync();
        ViewBag.Reviews = await _db.Reviews.Where(r => r.TeacherId == id).OrderByDescending(r => r.CreatedAt).Take(20).ToListAsync();
        return View();
    }

    // ========== 申請成為老師 ==========

    [HttpGet]
    public IActionResult Apply()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Apply(string name, string title, string bio, string skillsJson,
        string customSkills, int experienceYears, string education, int hourlyRate, int trialPrice,
        IFormFile? photo, IFormFile? diploma)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null || !user.IsRegistered)
        {
            ViewBag.Error = "請先註冊帳號後再申請成為老師";
            return View();
        }

        // Check if already applied
        var existing = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id);
        if (existing != null)
        {
            ViewBag.Error = "您已經申請過了，請等待審核";
            return View();
        }

        var teacher = new Teacher
        {
            SiteUserId = user.Id,
            Name = name,
            Title = title,
            Bio = bio,
            SkillsJson = skillsJson ?? "[]",
            CustomSkills = customSkills ?? "",
            ExperienceYears = experienceYears,
            Education = education,
            HourlyRate = hourlyRate,
            TrialPrice = trialPrice,
            IsApproved = false,
            IsActive = true
        };

        // Handle photo upload (store as Base64 data URL in DB)
        if (photo != null && photo.Length > 0 && photo.Length <= 2 * 1024 * 1024)
        {
            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());
            var contentType = photo.ContentType ?? "image/jpeg";
            teacher.PhotoUrl = $"data:{contentType};base64,{base64}";
            teacher.PhotoFileName = photo.FileName;
        }

        // Handle diploma upload (store as Base64 data URL in DB)
        if (diploma != null && diploma.Length > 0 && diploma.Length <= 2 * 1024 * 1024)
        {
            using var ms2 = new MemoryStream();
            await diploma.CopyToAsync(ms2);
            var base64d = Convert.ToBase64String(ms2.ToArray());
            var contentTyped = diploma.ContentType ?? "application/pdf";
            teacher.DiplomaFileName = $"data:{contentTyped};base64,{base64d}";
        }

        _db.Teachers.Add(teacher);
        await _db.SaveChangesAsync();

        ViewBag.Success = "申請已送出，請等待管理員審核！";
        return View();
    }

    // ========== 老師後台 ==========

    public async Task<IActionResult> Dashboard()
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null) return Redirect("/Account/Login");

        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id && t.IsApproved);
        if (teacher == null) return Redirect("/Teacher/Apply");

        ViewBag.Teacher = teacher;
        ViewBag.Slots = await _db.TeacherSlots.Where(s => s.TeacherId == teacher.Id).OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime).ToListAsync();
        ViewBag.UpcomingBookings = await _db.Bookings.Where(b => b.TeacherId == teacher.Id && (b.Status == "pending" || b.Status == "confirmed")).OrderBy(b => b.BookingDate).ToListAsync();
        ViewBag.CompletedBookings = await _db.Bookings.Where(b => b.TeacherId == teacher.Id && b.Status == "completed").OrderByDescending(b => b.BookingDate).Take(20).ToListAsync();
        ViewBag.Reviews = await _db.Reviews.Where(r => r.TeacherId == teacher.Id).OrderByDescending(r => r.CreatedAt).Take(10).ToListAsync();

        return View();
    }

    // 老師更新自己的資料
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(string name, string title, string bio,
        string education, int experienceYears, int hourlyRate, int trialPrice,
        string customSkills, IFormFile? photo, IFormFile? diploma)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null) return RedirectToAction("Dashboard");
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id);
        if (teacher == null) return RedirectToAction("Dashboard");

        teacher.Name = name ?? teacher.Name;
        teacher.Title = title ?? teacher.Title;
        teacher.Bio = bio ?? teacher.Bio;
        teacher.Education = education ?? teacher.Education;
        teacher.ExperienceYears = experienceYears;
        teacher.HourlyRate = hourlyRate;
        teacher.TrialPrice = trialPrice;
        teacher.CustomSkills = customSkills ?? teacher.CustomSkills;
        teacher.UpdatedAt = DateTime.Now;

        if (photo != null && photo.Length > 0 && photo.Length <= 2 * 1024 * 1024)
        {
            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());
            var contentType = photo.ContentType ?? "image/jpeg";
            teacher.PhotoUrl = $"data:{contentType};base64,{base64}";
            teacher.PhotoFileName = photo.FileName;
        }

        if (diploma != null && diploma.Length > 0 && diploma.Length <= 2 * 1024 * 1024)
        {
            using var ms2 = new MemoryStream();
            await diploma.CopyToAsync(ms2);
            var base64d = Convert.ToBase64String(ms2.ToArray());
            var contentTyped = diploma.ContentType ?? "application/pdf";
            teacher.DiplomaFileName = $"data:{contentTyped};base64,{base64d}";
        }

        await _db.SaveChangesAsync();
        return RedirectToAction("Dashboard");
    }

    // ========== 時段管理 ==========

    // 取得老師所有時段
    [HttpGet]
    public async Task<IActionResult> MySlots()
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        var userId = user?.Id ?? 0;
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == userId);
        if (teacher == null) return Json(new List<object>());

        var slots = await _db.TeacherSlots.Where(s => s.TeacherId == teacher.Id).ToListAsync();
        return Json(slots.Select(s => new { s.Id, s.DayOfWeek, s.StartTime, s.EndTime, s.IsAvailable }));
    }

    // 切換時段（開放/關閉）
    [HttpPost]
    public async Task<IActionResult> ToggleSlot([FromBody] ToggleSlotReq req)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        var toggleUserId = user?.Id ?? 0;
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == toggleUserId);
        if (teacher == null) return Unauthorized();

        var existing = await _db.TeacherSlots.FirstOrDefaultAsync(s =>
            s.TeacherId == teacher.Id && s.DayOfWeek == req.DayOfWeek && s.StartTime == req.StartTime);

        if (existing != null)
        {
            // Toggle off - remove slot
            _db.TeacherSlots.Remove(existing);
        }
        else
        {
            // Toggle on - add slot
            _db.TeacherSlots.Add(new TeacherSlot
            {
                TeacherId = teacher.Id,
                DayOfWeek = req.DayOfWeek,
                StartTime = req.StartTime,
                EndTime = req.StartTime.Split(':')[0] + ":50", // 50 min lesson
                IsAvailable = true
            });
        }
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // 公開：取得老師可預約時段（排除已被預約的）
    [HttpGet]
    public async Task<IActionResult> AvailableSlots(int teacherId)
    {
        var slots = await _db.TeacherSlots.Where(s => s.TeacherId == teacherId && s.IsAvailable).ToListAsync();

        // 找出已被預約的時段（status = pending or confirmed）
        var bookedSlots = await _db.Bookings
            .Where(b => b.TeacherId == teacherId && (b.Status == "pending" || b.Status == "confirmed"))
            .Select(b => new { b.BookingDate, b.TimeSlot })
            .ToListAsync();

        return Json(new { slots = slots.Select(s => new { s.DayOfWeek, s.StartTime, s.EndTime }), bookedSlots });
    }

    [HttpGet]
    public async Task<IActionResult> ManageSlots()
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null) return Unauthorized();

        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id && t.IsApproved);
        if (teacher == null) return Redirect("/Teacher/Apply");

        var slots = await _db.TeacherSlots.Where(s => s.TeacherId == teacher.Id).OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime).ToListAsync();
        return Json(slots);
    }

    [HttpPost]
    public async Task<IActionResult> SaveSlot([FromBody] SaveSlotReq req)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null) return Unauthorized();

        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id && t.IsApproved);
        if (teacher == null) return Unauthorized();

        var slot = new TeacherSlot
        {
            TeacherId = teacher.Id,
            DayOfWeek = req.DayOfWeek,
            StartTime = req.StartTime,
            EndTime = req.EndTime,
            IsAvailable = true
        };
        _db.TeacherSlots.Add(slot);
        await _db.SaveChangesAsync();
        return Ok(new { success = true, id = slot.Id });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSlot([FromBody] DeleteSlotReq req)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null) return Unauthorized();

        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id && t.IsApproved);
        if (teacher == null) return Unauthorized();

        var slot = await _db.TeacherSlots.FirstOrDefaultAsync(s => s.Id == req.Id && s.TeacherId == teacher.Id);
        if (slot == null) return NotFound();

        _db.TeacherSlots.Remove(slot);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // ========== 預約功能 ==========

    [HttpPost]
    public async Task<IActionResult> Book([FromBody] BookReq req)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null || !user.IsRegistered)
            return Ok(new { success = false, requireRegister = true, message = "請先註冊才能預約課程" });

        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == req.TeacherId && t.IsApproved && t.IsActive);
        if (teacher == null) return NotFound();

        if (!DateTime.TryParse(req.BookingDate, out var bookingDate))
            return Ok(new { success = false, message = "日期格式錯誤" });

        // 檢查老師是否有開放該時段
        var hour = req.TimeSlot?.Split('-')[0] ?? "";
        var dayOfWeek = (int)bookingDate.DayOfWeek; // 0=Sun
        var hasSlot = await _db.TeacherSlots.AnyAsync(s =>
            s.TeacherId == req.TeacherId && s.DayOfWeek == dayOfWeek && s.StartTime == hour && s.IsAvailable);
        if (!hasSlot)
            return Ok(new { success = false, message = "老師尚未開放此時段，請選擇綠色的可預約時段" });

        // 檢查是否已被預約
        var alreadyBooked = await _db.Bookings.AnyAsync(b =>
            b.TeacherId == req.TeacherId && b.BookingDate.Date == bookingDate.Date &&
            b.TimeSlot == req.TimeSlot && (b.Status == "pending" || b.Status == "confirmed"));
        if (alreadyBooked)
            return Ok(new { success = false, message = "此時段已被預約" });

        var booking = new Booking
        {
            TeacherId = req.TeacherId,
            StudentId = anonId,
            StudentName = req.StudentName,
            BookingDate = bookingDate,
            TimeSlot = req.TimeSlot,
            StudentNote = req.StudentNote ?? "",
            StudentEmail = req.StudentEmail ?? "",
            StudentPhone = req.StudentPhone ?? "",
            Status = "pending"
        };
        _db.Bookings.Add(booking);

        // 通知聊天室
        _db.ChatMessages.Add(new ChatMessage
        {
            SessionId = "system",
            Nickname = "📢 系統通知",
            Message = $"📅 {booking.StudentName} 預約了 {teacher.Name} 老師的課程：{booking.BookingDate:M/d} {booking.TimeSlot}，等待老師確認",
            AvatarEmoji = "🔔"
        });

        await _db.SaveChangesAsync();
        return Ok(new { success = true, bookingId = booking.Id });
    }

    // 老師回應預約
    [HttpPost]
    public async Task<IActionResult> RespondBooking([FromBody] RespondBookingReq req)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null) return Unauthorized();

        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == user.Id && t.IsApproved);
        if (teacher == null) return Unauthorized();

        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == req.BookingId && b.TeacherId == teacher.Id);
        if (booking == null) return NotFound();

        booking.Status = req.Status; // confirmed / cancelled / completed
        if (!string.IsNullOrEmpty(req.TeacherNote)) booking.TeacherNote = req.TeacherNote;

        // Update teacher stats if completed
        if (req.Status == "completed")
        {
            teacher.TotalLessons++;
            // Count unique students
            teacher.TotalStudents = await _db.Bookings
                .Where(b => b.TeacherId == teacher.Id && b.Status == "completed")
                .Select(b => b.StudentId).Distinct().CountAsync();
        }

        await _db.SaveChangesAsync();

        // 自動發送通知到聊天室
        var statusText = req.Status switch
        {
            "confirmed" => $"📅 {teacher.Name} 老師已確認 {booking.StudentName} 的預約：{booking.BookingDate:M/d} {booking.TimeSlot}",
            "cancelled" => $"❌ {teacher.Name} 老師已拒絕預約" + (string.IsNullOrEmpty(req.TeacherNote) ? "" : $"，原因：{req.TeacherNote}"),
            "completed" => $"✅ {teacher.Name} 老師與 {booking.StudentName} 的課程已完成！",
            _ => ""
        };
        if (!string.IsNullOrEmpty(statusText))
        {
            _db.ChatMessages.Add(new ChatMessage
            {
                SessionId = "system",
                Nickname = "📢 系統通知",
                Message = statusText,
                AvatarEmoji = "🔔"
            });
            await _db.SaveChangesAsync();
        }

        return Ok(new { success = true });
    }

    // ========== 評價功能 ==========

    [HttpPost]
    public async Task<IActionResult> SubmitReview([FromBody] SubmitReviewReq req)
    {
        var anonId = GetAnonId();

        // Check booking exists and is completed
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == req.BookingId && b.StudentId == anonId && b.Status == "completed");
        if (booking == null) return BadRequest(new { error = "只能對已完成的課程進行評價" });

        // Check not already reviewed
        var existingReview = await _db.Reviews.FirstOrDefaultAsync(r => r.BookingId == req.BookingId);
        if (existingReview != null) return BadRequest(new { error = "此課程已經評價過了" });

        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);

        var review = new Review
        {
            TeacherId = booking.TeacherId,
            StudentId = anonId,
            StudentName = user?.Nickname ?? "匿名",
            BookingId = req.BookingId,
            Rating = Math.Clamp(req.Rating, 1, 5),
            Comment = req.Comment ?? ""
        };
        _db.Reviews.Add(review);

        // Update teacher average rating
        var teacher = await _db.Teachers.FindAsync(booking.TeacherId);
        if (teacher != null)
        {
            var allReviews = await _db.Reviews.Where(r => r.TeacherId == teacher.Id).ToListAsync();
            allReviews.Add(review);
            teacher.AverageRating = allReviews.Average(r => r.Rating);
        }

        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> GetReviews(int teacherId)
    {
        var reviews = await _db.Reviews.Where(r => r.TeacherId == teacherId)
            .OrderByDescending(r => r.CreatedAt).Take(50).ToListAsync();
        return Json(reviews.Select(r => new {
            r.Id, r.StudentName, r.Rating, r.Comment,
            createdAt = r.CreatedAt.ToString("yyyy-MM-dd")
        }));
    }

    // ========== 老師內容分享 ==========

    // 老師發布免費內容
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostReq req)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        var userId = user?.Id ?? 0;
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == userId);
        if (teacher == null || !teacher.IsApproved) return Unauthorized();

        var post = new TeacherPost
        {
            TeacherId = teacher.Id,
            Title = req.Title,
            Content = req.Content,
            Type = req.Type ?? "article",
            VideoUrl = req.VideoUrl ?? "",
            ResourceUrl = req.ResourceUrl ?? ""
        };
        _db.TeacherPosts.Add(post);
        await _db.SaveChangesAsync();
        return Ok(new { success = true, id = post.Id });
    }

    // 取得老師的所有文章
    [HttpGet]
    public async Task<IActionResult> Posts(int teacherId)
    {
        var posts = await _db.TeacherPosts
            .Where(p => p.TeacherId == teacherId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return Json(posts);
    }

    // 按讚
    [HttpPost]
    public async Task<IActionResult> LikePost([FromBody] LikePostReq req)
    {
        var post = await _db.TeacherPosts.FindAsync(req.PostId);
        if (post == null) return NotFound();
        post.Likes++;
        await _db.SaveChangesAsync();
        return Ok(new { likes = post.Likes });
    }

    // 留言
    [HttpPost]
    public async Task<IActionResult> CommentPost([FromBody] CommentPostReq req)
    {
        var anonId = GetAnonId();
        var comment = new TeacherPostComment
        {
            TeacherPostId = req.PostId,
            SessionId = anonId,
            Nickname = req.Nickname ?? "匿名",
            Content = req.Content
        };
        _db.TeacherPostComments.Add(comment);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // 取得文章留言
    [HttpGet]
    public async Task<IActionResult> PostComments(int postId)
    {
        var comments = await _db.TeacherPostComments
            .Where(c => c.TeacherPostId == postId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
        return Json(comments);
    }

    // 刪除文章
    [HttpPost]
    public async Task<IActionResult> DeletePost([FromBody] DeletePostReq req)
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        var userId = user?.Id ?? 0;
        var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == userId);
        if (teacher == null) return Unauthorized();

        var post = await _db.TeacherPosts.FirstOrDefaultAsync(p => p.Id == req.PostId && p.TeacherId == teacher.Id);
        if (post == null) return NotFound();

        _db.TeacherPosts.Remove(post);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    // 取得其他老師（排除自己）
    [HttpGet]
    public async Task<IActionResult> OtherTeachers()
    {
        var anonId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        var userId = user?.Id ?? 0;
        var myTeacher = await _db.Teachers.FirstOrDefaultAsync(t => t.SiteUserId == userId);
        var myTeacherId = myTeacher?.Id ?? 0;

        var teachers = await _db.Teachers
            .Where(t => t.IsApproved && t.IsActive && t.Id != myTeacherId)
            .OrderByDescending(t => t.AverageRating)
            .ToListAsync();

        return Json(teachers.Select(t => new {
            t.Id, t.Name, t.Title, t.AverageRating, t.HourlyRate, t.TrialPrice,
            t.PhotoUrl, t.TotalStudents, t.TotalLessons
        }));
    }

    // 最新文章（首頁用）
    [HttpGet]
    public async Task<IActionResult> LatestPosts()
    {
        var posts = await _db.TeacherPosts
            .OrderByDescending(p => p.CreatedAt)
            .Take(4)
            .ToListAsync();

        var teacherIds = posts.Select(p => p.TeacherId).Distinct().ToList();
        var teachers = await _db.Teachers.Where(t => teacherIds.Contains(t.Id)).ToListAsync();

        var result = posts.Select(p => new {
            p.Id, p.Title, p.Type, p.Likes, p.Views,
            p.TeacherId,
            teacherName = teachers.FirstOrDefault(t => t.Id == p.TeacherId)?.Name ?? "",
            createdAt = p.CreatedAt.ToString("yyyy-MM-dd"),
            preview = p.Content.Length > 80 ? p.Content.Substring(0, 80) + "..." : p.Content
        });
        return Json(result);
    }
}

// Request models
public record UpdateTeacherProfileReq(string? Name, string? Title, string? Bio, string? SkillsJson,
    int? ExperienceYears, string? Education, int? HourlyRate, int? TrialPrice, string? PhotoUrl);
public record SaveSlotReq(int DayOfWeek, string StartTime, string EndTime);
public record DeleteSlotReq(int Id);
public record BookReq(int TeacherId, string StudentName, string BookingDate, string TimeSlot,
    string? StudentNote, string? StudentEmail, string? StudentPhone);
public record RespondBookingReq(int BookingId, string Status, string? TeacherNote);
public record SubmitReviewReq(int BookingId, int Rating, string? Comment);
public record CreatePostReq(string Title, string Content, string? Type, string? VideoUrl, string? ResourceUrl);
public record LikePostReq(int PostId);
public record CommentPostReq(int PostId, string? Nickname, string Content);
public record DeletePostReq(int PostId);
public record ToggleSlotReq(int DayOfWeek, string StartTime);
