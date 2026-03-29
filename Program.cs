using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 資料庫：有 DATABASE_URL 用 PostgreSQL（Railway），沒有用 SQLite（本機開發）
var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(dbUrl))
{
    // Railway PostgreSQL: postgres://user:pass@host:port/db → 轉成 Npgsql 格式
    var uri = new Uri(dbUrl);
    var userInfo = uri.UserInfo.Split(':');
    var connStr = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connStr));
    // PostgreSQL 要求所有 DateTime 都用 UTC
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    Console.WriteLine($"[DB] Using PostgreSQL: {uri.Host}");
}
else
{
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=learning.db"));
    Console.WriteLine("[DB] Using SQLite (local dev)");
}

builder.Services.AddSession(opt => {
    opt.IdleTimeout = TimeSpan.FromDays(30); // 30 天不過期
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
    opt.Cookie.MaxAge = TimeSpan.FromDays(365); // Cookie 存一年
});
builder.Services.AddSignalR();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // PostgreSQL: EnsureCreated 只在 DB 不存在時建表
    // 所以用 GetPendingMigrations 前先確保所有表都存在
    try
    {
        // 嘗試用 EnsureCreated（新 DB 時有效）
        db.Database.EnsureCreated();
    }
    catch { /* DB 已存在，忽略 */ }

    // 手動建立缺少的表（EnsureCreated 不會更新已存在的 DB）
    try
    {
        db.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""SiteUsers"" (
                ""Id"" SERIAL PRIMARY KEY,
                ""AnonymousId"" TEXT NOT NULL DEFAULT '',
                ""Nickname"" TEXT NOT NULL DEFAULT '匿名學習者',
                ""Email"" TEXT,
                ""IsRegistered"" BOOLEAN NOT NULL DEFAULT FALSE,
                ""TotalScore"" INTEGER NOT NULL DEFAULT 0,
                ""QuizzesTaken"" INTEGER NOT NULL DEFAULT 0,
                ""ChaptersCompleted"" INTEGER NOT NULL DEFAULT 0,
                ""BadgeLevel"" TEXT NOT NULL DEFAULT 'newbie',
                ""CreatedAt"" TIMESTAMP NOT NULL DEFAULT NOW(),
                ""LastActiveAt"" TIMESTAMP NOT NULL DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""BugChallenges"" (
                ""Id"" SERIAL PRIMARY KEY, ""Title"" TEXT DEFAULT '', ""BuggyCode"" TEXT DEFAULT '',
                ""FixedCode"" TEXT DEFAULT '', ""Explanation"" TEXT DEFAULT '', ""Language"" TEXT DEFAULT 'csharp',
                ""Difficulty"" TEXT DEFAULT 'beginner', ""Category"" TEXT DEFAULT '', ""BugCount"" INTEGER DEFAULT 1
            );
            CREATE TABLE IF NOT EXISTS ""BugAttempts"" (
                ""Id"" SERIAL PRIMARY KEY, ""SessionId"" TEXT DEFAULT '', ""BugChallengeId"" INTEGER DEFAULT 0,
                ""Solved"" BOOLEAN DEFAULT FALSE, ""TimeTakenSeconds"" INTEGER DEFAULT 0, ""AttemptedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""SpeedRuns"" (
                ""Id"" SERIAL PRIMARY KEY, ""SessionId"" TEXT DEFAULT '', ""Nickname"" TEXT DEFAULT '',
                ""CorrectCount"" INTEGER DEFAULT 0, ""TotalCount"" INTEGER DEFAULT 0, ""TimeLimitSeconds"" INTEGER DEFAULT 60,
                ""ScorePoints"" INTEGER DEFAULT 0, ""Category"" TEXT DEFAULT 'all', ""PlayedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""DailyChallenges"" (
                ""Id"" SERIAL PRIMARY KEY, ""ChallengeDate"" TIMESTAMP NOT NULL, ""QuestionId"" INTEGER DEFAULT 0, ""Category"" TEXT DEFAULT ''
            );
            CREATE TABLE IF NOT EXISTS ""DailyAttempts"" (
                ""Id"" SERIAL PRIMARY KEY, ""SessionId"" TEXT DEFAULT '', ""DailyChallengeId"" INTEGER DEFAULT 0,
                ""IsCorrect"" BOOLEAN DEFAULT FALSE, ""UserAnswer"" TEXT DEFAULT '', ""StreakDays"" INTEGER DEFAULT 0, ""AnsweredAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""CodePuzzles"" (
                ""Id"" SERIAL PRIMARY KEY, ""Title"" TEXT DEFAULT '', ""FullCode"" TEXT DEFAULT '',
                ""BlankPositionsJson"" TEXT DEFAULT '[]', ""Language"" TEXT DEFAULT 'csharp',
                ""Difficulty"" TEXT DEFAULT 'beginner', ""Category"" TEXT DEFAULT '', ""Hint"" TEXT DEFAULT ''
            );
            CREATE TABLE IF NOT EXISTS ""PuzzleAttempts"" (
                ""Id"" SERIAL PRIMARY KEY, ""SessionId"" TEXT DEFAULT '', ""CodePuzzleId"" INTEGER DEFAULT 0,
                ""CorrectBlanks"" INTEGER DEFAULT 0, ""TotalBlanks"" INTEGER DEFAULT 0, ""TimeTakenSeconds"" INTEGER DEFAULT 0, ""AttemptedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""ArenaChallenges"" (
                ""Id"" SERIAL PRIMARY KEY, ""Title"" TEXT DEFAULT '', ""Description"" TEXT DEFAULT '',
                ""Category"" TEXT DEFAULT '', ""Difficulty"" TEXT DEFAULT 'intermediate',
                ""StartDate"" TIMESTAMP DEFAULT NOW(), ""EndDate"" TIMESTAMP DEFAULT NOW(), ""IsActive"" BOOLEAN DEFAULT TRUE
            );
            CREATE TABLE IF NOT EXISTS ""ArenaSubmissions"" (
                ""Id"" SERIAL PRIMARY KEY, ""SessionId"" TEXT DEFAULT '', ""Nickname"" TEXT DEFAULT '',
                ""ArenaChallengeId"" INTEGER DEFAULT 0, ""Code"" TEXT DEFAULT '', ""Language"" TEXT DEFAULT 'csharp',
                ""Explanation"" TEXT DEFAULT '', ""Upvotes"" INTEGER DEFAULT 0, ""SubmittedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""StudyBuddies"" (
                ""Id"" SERIAL PRIMARY KEY, ""SessionId"" TEXT DEFAULT '', ""Nickname"" TEXT DEFAULT '',
                ""Level"" TEXT DEFAULT 'beginner', ""InterestsJson"" TEXT DEFAULT '[]', ""Goal"" TEXT DEFAULT '',
                ""ContactInfo"" TEXT DEFAULT '', ""IsLookingForBuddy"" BOOLEAN DEFAULT TRUE, ""RegisteredAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""BuddyMatches"" (
                ""Id"" SERIAL PRIMARY KEY, ""BuddyAId"" INTEGER DEFAULT 0, ""BuddyBId"" INTEGER DEFAULT 0,
                ""MatchedAt"" TIMESTAMP DEFAULT NOW(), ""IsActive"" BOOLEAN DEFAULT TRUE
            );
            CREATE TABLE IF NOT EXISTS ""ChatMessages"" (
                ""Id"" SERIAL PRIMARY KEY, ""SessionId"" TEXT DEFAULT '', ""Nickname"" TEXT DEFAULT '',
                ""Message"" TEXT DEFAULT '', ""AvatarEmoji"" TEXT DEFAULT '😀', ""SentAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""Teachers"" (
                ""Id"" SERIAL PRIMARY KEY, ""SiteUserId"" INTEGER DEFAULT 0, ""Name"" TEXT DEFAULT '',
                ""Title"" TEXT DEFAULT '', ""Bio"" TEXT DEFAULT '', ""PhotoUrl"" TEXT DEFAULT '',
                ""VideoUrl"" TEXT DEFAULT '', ""SkillsJson"" TEXT DEFAULT '[]', ""ExperienceYears"" INTEGER DEFAULT 0,
                ""Education"" TEXT DEFAULT '', ""HourlyRate"" INTEGER DEFAULT 0, ""TrialPrice"" INTEGER DEFAULT 0,
                ""IsApproved"" BOOLEAN DEFAULT FALSE, ""IsActive"" BOOLEAN DEFAULT TRUE,
                ""AverageRating"" DOUBLE PRECISION DEFAULT 0, ""TotalStudents"" INTEGER DEFAULT 0,
                ""TotalLessons"" INTEGER DEFAULT 0, ""CreatedAt"" TIMESTAMP DEFAULT NOW(), ""UpdatedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""TeacherSlots"" (
                ""Id"" SERIAL PRIMARY KEY, ""TeacherId"" INTEGER DEFAULT 0, ""DayOfWeek"" INTEGER DEFAULT 0,
                ""StartTime"" TEXT DEFAULT '09:00', ""EndTime"" TEXT DEFAULT '10:00', ""IsAvailable"" BOOLEAN DEFAULT TRUE
            );
            CREATE TABLE IF NOT EXISTS ""Bookings"" (
                ""Id"" SERIAL PRIMARY KEY, ""TeacherId"" INTEGER DEFAULT 0, ""StudentId"" TEXT DEFAULT '',
                ""StudentName"" TEXT DEFAULT '', ""BookingDate"" TIMESTAMP DEFAULT NOW(), ""TimeSlot"" TEXT DEFAULT '',
                ""Status"" TEXT DEFAULT 'pending', ""StudentNote"" TEXT DEFAULT '', ""TeacherNote"" TEXT DEFAULT '',
                ""StudentEmail"" TEXT DEFAULT '', ""StudentPhone"" TEXT DEFAULT '', ""CreatedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""Reviews"" (
                ""Id"" SERIAL PRIMARY KEY, ""TeacherId"" INTEGER DEFAULT 0, ""StudentId"" TEXT DEFAULT '',
                ""StudentName"" TEXT DEFAULT '', ""BookingId"" INTEGER DEFAULT 0, ""Rating"" INTEGER DEFAULT 5,
                ""Comment"" TEXT DEFAULT '', ""CreatedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""AuditLogs"" (
                ""Id"" SERIAL PRIMARY KEY, ""UserId"" TEXT DEFAULT '', ""UserName"" TEXT DEFAULT '',
                ""UserRole"" TEXT DEFAULT '', ""Action"" TEXT DEFAULT '', ""EntityType"" TEXT DEFAULT '',
                ""EntityId"" INTEGER DEFAULT 0, ""Details"" TEXT DEFAULT '', ""CreatedAt"" TIMESTAMP DEFAULT NOW()
            );
            ALTER TABLE ""Teachers"" ADD COLUMN IF NOT EXISTS ""DiplomaFileName"" TEXT DEFAULT '';
            ALTER TABLE ""Teachers"" ADD COLUMN IF NOT EXISTS ""PhotoFileName"" TEXT DEFAULT '';
            ALTER TABLE ""Teachers"" ADD COLUMN IF NOT EXISTS ""CustomSkills"" TEXT DEFAULT '';
            ALTER TABLE ""Teachers"" ADD COLUMN IF NOT EXISTS ""RejectReason"" TEXT DEFAULT '';
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""AvatarUrl"" TEXT;
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""PasswordHash"" TEXT DEFAULT '';
            ALTER TABLE ""SupportTickets"" ADD COLUMN IF NOT EXISTS ""IsReplyRead"" BOOLEAN DEFAULT FALSE;
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""Role"" TEXT DEFAULT 'guest';
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""IsBanned"" BOOLEAN DEFAULT FALSE;
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""BanReason"" TEXT DEFAULT '';
            CREATE TABLE IF NOT EXISTS ""TeacherPosts"" (
                ""Id"" SERIAL PRIMARY KEY, ""TeacherId"" INTEGER DEFAULT 0, ""Title"" TEXT DEFAULT '',
                ""Content"" TEXT DEFAULT '', ""Type"" TEXT DEFAULT 'article', ""VideoUrl"" TEXT DEFAULT '',
                ""ResourceUrl"" TEXT DEFAULT '', ""Views"" INTEGER DEFAULT 0, ""Likes"" INTEGER DEFAULT 0,
                ""CreatedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""TeacherPostComments"" (
                ""Id"" SERIAL PRIMARY KEY, ""TeacherPostId"" INTEGER DEFAULT 0, ""SessionId"" TEXT DEFAULT '',
                ""Nickname"" TEXT DEFAULT '', ""Content"" TEXT DEFAULT '', ""CreatedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""FavoriteTeachers"" (
                ""Id"" SERIAL PRIMARY KEY, ""StudentId"" TEXT DEFAULT '', ""TeacherId"" INTEGER DEFAULT 0, ""CreatedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""PrivateMessages"" (
                ""Id"" SERIAL PRIMARY KEY, ""SenderId"" TEXT DEFAULT '', ""SenderName"" TEXT DEFAULT '',
                ""ReceiverId"" TEXT DEFAULT '', ""ReceiverName"" TEXT DEFAULT '', ""TeacherId"" INTEGER,
                ""Message"" TEXT DEFAULT '', ""IsRead"" BOOLEAN DEFAULT FALSE, ""SentAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""SupportTickets"" (
                ""Id"" SERIAL PRIMARY KEY, ""UserId"" TEXT DEFAULT '', ""UserName"" TEXT DEFAULT '',
                ""UserEmail"" TEXT DEFAULT '', ""Category"" TEXT DEFAULT '', ""Content"" TEXT DEFAULT '',
                ""Status"" TEXT DEFAULT 'pending', ""AdminReply"" TEXT DEFAULT '',
                ""IsReplyRead"" BOOLEAN DEFAULT FALSE,
                ""CreatedAt"" TIMESTAMP DEFAULT NOW(), ""RepliedAt"" TIMESTAMP
            );
            CREATE TABLE IF NOT EXISTS ""ErrorLogs"" (
                ""Id"" SERIAL PRIMARY KEY, ""PageUrl"" TEXT DEFAULT '', ""ErrorMessage"" TEXT DEFAULT '',
                ""StackTrace"" TEXT DEFAULT '', ""Source"" TEXT DEFAULT '', ""UserId"" TEXT DEFAULT '',
                ""UserAgent"" TEXT DEFAULT '', ""IpAddress"" TEXT DEFAULT '',
                ""IsResolved"" BOOLEAN DEFAULT FALSE, ""ResolvedBy"" TEXT DEFAULT '',
                ""CreatedAt"" TIMESTAMP DEFAULT NOW()
            );
            CREATE TABLE IF NOT EXISTS ""AIWorkLogs"" (
                ""Id"" SERIAL PRIMARY KEY, ""TaskType"" TEXT DEFAULT '', ""Description"" TEXT DEFAULT '',
                ""FilesModified"" TEXT DEFAULT '', ""ErrorLogId"" TEXT DEFAULT '',
                ""Status"" TEXT DEFAULT '', ""Result"" TEXT DEFAULT '',
                ""DurationSeconds"" INTEGER DEFAULT 0, ""StartedAt"" TIMESTAMP DEFAULT NOW(),
                ""CompletedAt"" TIMESTAMP
            );
        ");
        Console.WriteLine("[DB] All tables ensured.");
    }
    catch (Exception ex) { Console.WriteLine($"[DB] Table creation note: {ex.Message}"); }

    SeedData.Initialize(db);

    // 確保 Admin 帳號存在
    if (!db.SiteUsers.Any(u => u.Email == "1234@hotmail.com"))
    {
        db.SiteUsers.Add(new DotNetLearning.Models.SiteUser
        {
            AnonymousId = "admin-master-001",
            Nickname = "admin",
            Email = "1234@hotmail.com",
            IsRegistered = true,
            Role = "admin"
        });
        db.SaveChanges();
        Console.WriteLine("[DB] Admin account created.");
    }

    // Migrate existing data: set Role for registered users and admin
    try
    {
        db.Database.ExecuteSqlRaw(@"UPDATE ""SiteUsers"" SET ""Role"" = 'member' WHERE ""IsRegistered"" = TRUE AND (""Role"" = 'guest' OR ""Role"" IS NULL OR ""Role"" = '');");
        db.Database.ExecuteSqlRaw(@"UPDATE ""SiteUsers"" SET ""Role"" = 'admin' WHERE ""Email"" = '1234@hotmail.com';");
        Console.WriteLine("[DB] Role migration completed.");
    }
    catch (Exception ex) { Console.WriteLine($"[DB] Role migration note: {ex.Message}"); }
}

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

// 404 自訂頁面
app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.ContentType = "text/html; charset=utf-8";
        var filePath = Path.Combine(app.Environment.ContentRootPath, "Views", "Shared", "Error404.cshtml");
        // 直接讀取 HTML（Error404.cshtml 不用 Layout，是純 HTML）
        var html = await File.ReadAllTextAsync(filePath);
        // Razor 的 @@ 在靜態讀取時要還原成 @
        html = html.Replace("@@keyframes", "@keyframes");
        await context.HttpContext.Response.WriteAsync(html);
    }
});

// 後端全域錯誤自動記錄到 DB
app.Use(async (context, next) =>
{
    try
    {
        await next();
        // 偵測 4xx/5xx 回應（非 API 的頁面錯誤）
        if (context.Response.StatusCode >= 500 && !context.Request.Path.StartsWithSegments("/api"))
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DotNetLearning.Data.AppDbContext>();
            db.ErrorLogs.Add(new DotNetLearning.Models.ErrorLog
            {
                PageUrl = context.Request.Path + context.Request.QueryString,
                ErrorMessage = $"HTTP {context.Response.StatusCode}",
                Source = "backend-http",
                UserId = context.Session.GetString("SessionId") ?? "",
                UserAgent = context.Request.Headers.UserAgent.ToString(),
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? ""
            });
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        // 記錄未處理的例外
        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DotNetLearning.Data.AppDbContext>();
            db.ErrorLogs.Add(new DotNetLearning.Models.ErrorLog
            {
                PageUrl = context.Request.Path + context.Request.QueryString,
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace?.Substring(0, Math.Min(ex.StackTrace.Length, 2000)) ?? "",
                Source = "backend-exception",
                UserId = context.Session.GetString("SessionId") ?? "",
                UserAgent = context.Request.Headers.UserAgent.ToString(),
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? ""
            });
            await db.SaveChangesAsync();
        }
        catch { /* 避免記錄本身出錯造成無限迴圈 */ }
        throw; // 重新拋出讓 ExceptionHandler 處理
    }
});

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

// 匿名 ID 系統：Cookie 持久化（365天）+ 自動建立資料庫記錄
app.Use(async (context, next) =>
{
    var cookieName = "DotNetLearner";
    var anonymousId = context.Request.Cookies[cookieName];

    if (string.IsNullOrEmpty(anonymousId))
    {
        anonymousId = Guid.NewGuid().ToString("N");
    }

    // 每次請求都重新寫 Cookie（確保不過期、不丟失）
    context.Response.Cookies.Append(cookieName, anonymousId, new CookieOptions
    {
        HttpOnly = true,
        Secure = false, // Railway 反向代理內部是 HTTP
        SameSite = SameSiteMode.Lax,
        MaxAge = TimeSpan.FromDays(365),
        Path = "/"
    });

    // 存到 Session 讓所有 Controller 都能用
    context.Session.SetString("SessionId", anonymousId);
    context.Session.SetString("sid", anonymousId);

    // 確保資料庫有這個用戶
    using var scope = context.RequestServices.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DotNetLearning.Data.AppDbContext>();
    var user = await db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonymousId);
    if (user == null)
    {
        user = new DotNetLearning.Models.SiteUser { AnonymousId = anonymousId };
        db.SiteUsers.Add(user);
        await db.SaveChangesAsync();
    }
    else
    {
        user.LastActiveAt = DateTime.Now;
        await db.SaveChangesAsync();
    }

    // Store user info in HttpContext.Items for easy access
    context.Items["CurrentUser"] = user;
    context.Items["AnonymousId"] = anonymousId;

    // Check if user is banned
    if (user.IsBanned)
    {
        context.Items["IsBanned"] = true;
    }

    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chathub");

app.Run();
