using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

// CORS: 只開放 /api/integration/* 給外部衛星 App（LifeQuest 等）
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("IntegrationPolicy", p => p
        .AllowAnyOrigin()           // Phase 1 先全開；之後應該換成 WithOrigins(...)
        .AllowAnyMethod()
        .AllowAnyHeader());
});

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

// Authentication: Google OAuth + Cookie
builder.Services.AddAuthentication(options => {
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Google";
})
.AddCookie("Cookies")
.AddGoogle("Google", options => {
    options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? "placeholder";
    options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? "placeholder";
    options.CallbackPath = "/signin-google";
});

builder.Services.AddSingleton<DotNetLearning.Services.EmailService>();
builder.Services.AddSingleton<DotNetLearning.Services.EcpayService>();
builder.Services.AddHostedService<DotNetLearning.Services.ErrorScannerService>();

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
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""ReferralCode"" TEXT DEFAULT '';
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""ReferredBy"" TEXT;
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""ReferralCount"" INTEGER DEFAULT 0;
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""GoogleId"" TEXT;
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""EmailVerified"" BOOLEAN DEFAULT FALSE;
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""VerificationToken"" TEXT;
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""VerificationExpiry"" TIMESTAMP;
            ALTER TABLE ""SiteUsers"" ADD COLUMN IF NOT EXISTS ""LoginMethod"" TEXT DEFAULT 'legacy';
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
            CREATE TABLE IF NOT EXISTS ""Announcements"" (
                ""Id"" SERIAL PRIMARY KEY, ""Title"" TEXT DEFAULT '', ""Content"" TEXT DEFAULT '',
                ""Type"" TEXT DEFAULT 'info', ""IsPinned"" BOOLEAN DEFAULT false,
                ""IsVisible"" BOOLEAN DEFAULT true, ""CreatedBy"" TEXT DEFAULT 'Admin',
                ""CreatedAt"" TIMESTAMP DEFAULT NOW(), ""ExpiresAt"" TIMESTAMP
            );
            CREATE TABLE IF NOT EXISTS ""BattleQuestions"" (""Id"" SERIAL PRIMARY KEY, ""Title"" TEXT DEFAULT '', ""Description"" TEXT DEFAULT '', ""StarterCode"" TEXT DEFAULT '', ""SampleInput"" TEXT DEFAULT '', ""SampleOutput"" TEXT DEFAULT '', ""ValidationKeywords"" TEXT DEFAULT '', ""Difficulty"" TEXT DEFAULT 'beginner', ""TimeLimitSeconds"" INT DEFAULT 120, ""Tags"" TEXT DEFAULT '');
            CREATE TABLE IF NOT EXISTS ""BattleRecords"" (""Id"" SERIAL PRIMARY KEY, ""Player1Id"" TEXT DEFAULT '', ""Player1Name"" TEXT DEFAULT '', ""Player2Id"" TEXT DEFAULT '', ""Player2Name"" TEXT DEFAULT '', ""WinnerId"" TEXT DEFAULT '', ""Difficulty"" TEXT DEFAULT '', ""QuestionId"" INT DEFAULT 0, ""QuestionTitle"" TEXT DEFAULT '', ""Player1TimeSeconds"" INT DEFAULT 0, ""Player2TimeSeconds"" INT DEFAULT 0, ""Player1Accuracy"" INT DEFAULT 0, ""Player2Accuracy"" INT DEFAULT 0, ""IsAIMatch"" BOOLEAN DEFAULT false, ""AILevel"" TEXT DEFAULT '', ""StartedAt"" TIMESTAMP DEFAULT NOW(), ""EndedAt"" TIMESTAMP);
            CREATE TABLE IF NOT EXISTS ""BattleStats"" (""Id"" SERIAL PRIMARY KEY, ""UserId"" TEXT DEFAULT '', ""UserName"" TEXT DEFAULT '', ""BeginnerWins"" INT DEFAULT 0, ""IntermediateWins"" INT DEFAULT 0, ""AdvancedWins"" INT DEFAULT 0, ""BeginnerLosses"" INT DEFAULT 0, ""IntermediateLosses"" INT DEFAULT 0, ""AdvancedLosses"" INT DEFAULT 0, ""UpdatedAt"" TIMESTAMP DEFAULT NOW());
            CREATE TABLE IF NOT EXISTS ""ClaudeTasks"" (
                ""Id"" SERIAL PRIMARY KEY,
                ""Prompt"" TEXT NOT NULL DEFAULT '',
                ""Status"" VARCHAR(20) NOT NULL DEFAULT 'pending',
                ""Result"" TEXT NOT NULL DEFAULT '',
                ""CreatedAt"" TIMESTAMP NOT NULL DEFAULT NOW(),
                ""StartedAt"" TIMESTAMP,
                ""CompletedAt"" TIMESTAMP
            );
            CREATE TABLE IF NOT EXISTS ""SharedFiles"" (
                ""Id"" SERIAL PRIMARY KEY,
                ""Title"" TEXT NOT NULL DEFAULT '',
                ""Description"" TEXT NOT NULL DEFAULT '',
                ""FileName"" TEXT NOT NULL DEFAULT '',
                ""StoragePath"" TEXT NOT NULL DEFAULT '',
                ""FileSize"" BIGINT NOT NULL DEFAULT 0,
                ""MimeType"" TEXT NOT NULL DEFAULT '',
                ""Category"" TEXT NOT NULL DEFAULT 'general',
                ""UploadedBy"" TEXT NOT NULL DEFAULT 'admin',
                ""CreatedAt"" TIMESTAMP NOT NULL DEFAULT NOW(),
                ""DownloadCount"" INTEGER NOT NULL DEFAULT 0,
                ""IsPublic"" BOOLEAN NOT NULL DEFAULT true,
                ""Tags"" TEXT
            );
            CREATE TABLE IF NOT EXISTS ""Payments"" (
                ""Id"" SERIAL PRIMARY KEY,
                ""OrderId"" TEXT NOT NULL DEFAULT '',
                ""EcpayTradeNo"" TEXT,
                ""UserAnonymousId"" TEXT NOT NULL DEFAULT '',
                ""ProductType"" TEXT NOT NULL DEFAULT 'teacher_monthly',
                ""ProductName"" TEXT NOT NULL DEFAULT '',
                ""Amount"" INTEGER NOT NULL DEFAULT 0,
                ""Status"" VARCHAR(20) NOT NULL DEFAULT 'pending',
                ""PaymentMethod"" TEXT NOT NULL DEFAULT '',
                ""CreatedAt"" TIMESTAMP NOT NULL DEFAULT NOW(),
                ""PaidAt"" TIMESTAMP,
                ""RawCallbackPayload"" TEXT NOT NULL DEFAULT '',
                ""EffectiveFrom"" TIMESTAMP,
                ""EffectiveUntil"" TIMESTAMP
            );
            CREATE UNIQUE INDEX IF NOT EXISTS ""IX_Payments_OrderId"" ON ""Payments"" (""OrderId"");
            CREATE INDEX IF NOT EXISTS ""IX_Payments_UserAnonymousId"" ON ""Payments"" (""UserAnonymousId"");
            CREATE TABLE IF NOT EXISTS ""TeacherSubscriptions"" (
                ""Id"" SERIAL PRIMARY KEY,
                ""UserAnonymousId"" TEXT NOT NULL DEFAULT '',
                ""Tier"" VARCHAR(20) NOT NULL DEFAULT 'free',
                ""ActiveUntil"" TIMESTAMP,
                ""TotalPaidAmount"" INTEGER NOT NULL DEFAULT 0,
                ""UpdatedAt"" TIMESTAMP NOT NULL DEFAULT NOW()
            );
            CREATE UNIQUE INDEX IF NOT EXISTS ""IX_TeacherSubscriptions_UserAnonymousId"" ON ""TeacherSubscriptions"" (""UserAnonymousId"");
        ");
        Console.WriteLine("[DB] All tables ensured.");
    }
    catch (Exception ex) { Console.WriteLine($"[DB] Table creation note: {ex.Message}"); }

    // Chapter Japanese translation columns
    try
    {
        db.Database.ExecuteSqlRaw(@"ALTER TABLE ""Chapters"" ADD COLUMN IF NOT EXISTS ""TitleJa"" TEXT;");
        db.Database.ExecuteSqlRaw(@"ALTER TABLE ""Chapters"" ADD COLUMN IF NOT EXISTS ""ContentJa"" TEXT;");
        Console.WriteLine("[DB] Chapter Ja columns ensured.");
    }
    catch (Exception ex) { Console.WriteLine($"[DB] Chapter Ja migration note: {ex.Message}"); }

    // Chat reply & reaction columns/tables
    try
    {
        db.Database.ExecuteSqlRaw(@"ALTER TABLE ""ChatMessages"" ADD COLUMN IF NOT EXISTS ""ReplyToId"" INTEGER;");
        db.Database.ExecuteSqlRaw(@"ALTER TABLE ""ChatMessages"" ADD COLUMN IF NOT EXISTS ""ReplyToNickname"" TEXT;");
        db.Database.ExecuteSqlRaw(@"ALTER TABLE ""ChatMessages"" ADD COLUMN IF NOT EXISTS ""ReplyToPreview"" TEXT;");

        db.Database.ExecuteSqlRaw(@"
CREATE TABLE IF NOT EXISTS ""ChatReactions"" (
    ""Id"" SERIAL PRIMARY KEY,
    ""ChatMessageId"" INTEGER NOT NULL DEFAULT 0,
    ""SessionId"" TEXT DEFAULT '',
    ""Nickname"" TEXT DEFAULT '',
    ""Emoji"" TEXT DEFAULT '',
    ""ReactedAt"" TIMESTAMP DEFAULT NOW()
);");

        db.Database.ExecuteSqlRaw(@"
CREATE UNIQUE INDEX IF NOT EXISTS ""IX_ChatReactions_Unique""
    ON ""ChatReactions"" (""ChatMessageId"", ""SessionId"", ""Emoji"");");

        Console.WriteLine("[DB] Chat reply & reaction migration done.");
    }
    catch (Exception ex) { Console.WriteLine($"[DB] Chat reply/reaction migration note: {ex.Message}"); }

    // ⚠️ REMOVED 2026-04-19:
    //   舊的 "Clean up teacher demo data" block 本來是 one-time 清除，但放在
    //   startup 等於每次重啟都會全刪 Teachers / Bookings / Reviews / UserProfiles。
    //   Mike 上架自己的老師資料後每次 Azure 重啟都消失 → 改成只在 env var 打開時才跑。
    if (Environment.GetEnvironmentVariable("CLEAN_DEMO_DATA_ON_STARTUP") == "true")
    {
        try
        {
            db.Database.ExecuteSqlRaw(@"DELETE FROM ""TeacherPostComments"";");
            db.Database.ExecuteSqlRaw(@"DELETE FROM ""TeacherPosts"";");
            db.Database.ExecuteSqlRaw(@"DELETE FROM ""Reviews"";");
            db.Database.ExecuteSqlRaw(@"DELETE FROM ""Bookings"";");
            db.Database.ExecuteSqlRaw(@"DELETE FROM ""TeacherSlots"";");
            db.Database.ExecuteSqlRaw(@"DELETE FROM ""FavoriteTeachers"";");
            db.Database.ExecuteSqlRaw(@"DELETE FROM ""Teachers"";");
            db.Database.ExecuteSqlRaw(@"DELETE FROM ""UserProfiles"";");
            Console.WriteLine("[DB] Cleaned up demo data (env var enabled).");
        }
        catch (Exception ex) { Console.WriteLine($"[DB] Cleanup note: {ex.Message}"); }
    }

    // Fix chapter categories for Vue/React/Angular (move from 'frontend' to dedicated categories)
    try
    {
        db.Database.ExecuteSqlRaw(@"UPDATE ""Chapters"" SET ""Category"" = 'vue' WHERE ""Slug"" LIKE 'vue-%' AND ""Category"" = 'frontend';");
        db.Database.ExecuteSqlRaw(@"UPDATE ""Chapters"" SET ""Category"" = 'react' WHERE ""Slug"" LIKE 'react-%' AND ""Category"" = 'frontend';");
        db.Database.ExecuteSqlRaw(@"UPDATE ""Chapters"" SET ""Category"" = 'angular' WHERE ""Slug"" LIKE 'angular-%' AND ""Category"" = 'frontend';");
        Console.WriteLine("[DB] Framework chapter categories updated.");
    }
    catch (Exception ex) { Console.WriteLine($"[DB] Category migration note: {ex.Message}"); }

    // 確保 Admin 帳號存在
    if (!db.SiteUsers.Any(u => u.Email == "admin@hotmail.com"))
    {
        db.SiteUsers.Add(new DotNetLearning.Models.SiteUser
        {
            AnonymousId = "admin-master-001",
            Nickname = "admin",
            Email = "admin@hotmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("abcd1234"),
            IsRegistered = true,
            Role = "admin",
            LoginMethod = "email",
            EmailVerified = true
        });
        db.SaveChanges();
        Console.WriteLine("[DB] Admin account created.");
    }

    // Seed 公告欄初始資料（逐筆檢查，避免刪除後無法恢復）
    var defaultAnns = new[]
    {
        new DotNetLearning.Models.Announcement {
            Title = "🎲 兩款全新遊戲上線：程式碼大富翁 & SQL 模擬器！",
            Content = "🎲 程式碼大富翁：擲骰子在 2D 棋盤上闖關，答對程式題拿金幣、踩陷阱扣血！📊 SQL 查詢模擬器：看著虛擬資料表寫真正的 SQL，從 SELECT 到 JOIN 共 8 個挑戰。從導覽列「遊戲」選單進入！",
            Type = "success", IsPinned = true, CreatedAt = new DateTime(2026, 4, 3, 12, 0, 0)
        },
        new DotNetLearning.Models.Announcement {
            Title = "🤖 AI 教你打程式：20 堂課全語言覆蓋！",
            Content = "AI 教練一行一行帶你寫！涵蓋 C#、JavaScript、jQuery、HTML、CSS、Vue、React、Angular、SQL 共 20 堂課。每行程式碼都有「💡語法說明」同步解說為什麼這樣寫。從「遊戲」選單進入！",
            Type = "success", IsPinned = true, CreatedAt = new DateTime(2026, 4, 3)
        },
        new DotNetLearning.Models.Announcement {
            Title = "🔴 Redis 快取 + 🏗️ 微服務教學上線（20 篇）",
            Content = "Redis 快取 10 篇：從入門到分散式架構、Pub/Sub、效能優化。微服務 10 篇：DDD、API Gateway、Docker、Polly、Saga、K8s。全部用 .NET 實作，完整學習路徑！",
            Type = "info", IsPinned = false, CreatedAt = new DateTime(2026, 4, 3)
        },
        new DotNetLearning.Models.Announcement {
            Title = "⚛️ Vue / React / Angular 前端框架教學上線（24 篇）",
            Content = "三大前端框架完整教學：Vue 8 篇、React 8 篇、Angular 8 篇，從入門到全端整合。每篇都強調框架不是原生 JS，底層是 JavaScript，附大量程式碼範例與原生 JS 對比。",
            Type = "info", IsPinned = false, CreatedAt = new DateTime(2026, 4, 2)
        },
        new DotNetLearning.Models.Announcement {
            Title = "🎉 全新對戰系統上線！",
            Content = "支援真人 PvP 即時配對對戰。挑選難度（初級 / 中級 / 高級），手打程式碼比拼速度與準確度，贏得戰績積分！",
            Type = "success", IsPinned = false, CreatedAt = new DateTime(2026, 3, 31)
        },
        new DotNetLearning.Models.Announcement {
            Title = "🎤 語音辨識大幅強化",
            Content = "新增模糊比對技術，現在可以說「點登入」、「按開始學習」直接觸發按鈕。支援語音填表（帳號是 XXX / 密碼是 XXX）、語音打字模式，麥克風狀態即時顯示。",
            Type = "info", CreatedAt = new DateTime(2026, 3, 31)
        },
        new DotNetLearning.Models.Announcement {
            Title = "🌐 三語系支援完整上線（中 / 英 / 日）",
            Content = "首頁、章節頁、Profile 頁等全站 101 個章節標題、18 個分類名稱、所有學習路徑名稱均支援繁體中文 / English / 日本語切換。",
            Type = "info", CreatedAt = new DateTime(2026, 3, 30)
        },
        new DotNetLearning.Models.Announcement {
            Title = "📷 鏡頭手勢控制修復",
            Content = "修復手勢辨識頁面永遠卡在讀取中的問題（Canvas getContext 方法錯誤），Arena、Speed Run、Detective 等遊戲功能現已正常運作。",
            Type = "warning", CreatedAt = new DateTime(2026, 3, 29)
        },
        new DotNetLearning.Models.Announcement {
            Title = "🤖 自動錯誤掃描服務啟動",
            Content = "系統每 6 小時自動掃描錯誤日誌，自動清除瀏覽器/網路類非真正 Bug，分類統計真正需要修復的錯誤。Admin 後台可查看掃描紀錄。",
            Type = "info", CreatedAt = new DateTime(2026, 3, 28)
        },
        new DotNetLearning.Models.Announcement {
            Title = "📢 首頁公告欄上線",
            Content = "首頁頂端新增公告欄，即時顯示最新功能更新與重要通知。Admin 後台可新增、編輯、隱藏或刪除公告。",
            Type = "info", CreatedAt = new DateTime(2026, 4, 1)
        },
    };
    foreach (var ann in defaultAnns)
    {
        if (!db.Announcements.Any(a => a.Title == ann.Title))
        {
            db.Announcements.Add(ann);
        }
    }
    db.SaveChanges();
    Console.WriteLine("[DB] Announcements ensured.");

    // Migrate existing data: set Role for registered users and admin
    try
    {
        db.Database.ExecuteSqlRaw(@"UPDATE ""SiteUsers"" SET ""Role"" = 'member' WHERE ""IsRegistered"" = TRUE AND (""Role"" = 'guest' OR ""Role"" IS NULL OR ""Role"" = '');");
        db.Database.ExecuteSqlRaw(@"UPDATE ""SiteUsers"" SET ""Role"" = 'admin' WHERE ""Email"" = 'admin@hotmail.com';");
        // Migrate old admin email to new + set password (pre-computed BCrypt hash for "abcd1234")
        db.Database.ExecuteSqlRaw(
            @"UPDATE ""SiteUsers"" SET ""Email"" = 'admin@hotmail.com', ""LoginMethod"" = 'email', ""EmailVerified"" = TRUE WHERE ""Email"" = 'admin@hotmail.com' OR ""Email"" = '1234@hotmail.com'");
        // Set admin password if not already set
        var adminUser = db.SiteUsers.FirstOrDefault(u => u.Email == "admin@hotmail.com");
        if (adminUser != null && string.IsNullOrEmpty(adminUser.PasswordHash))
        {
            adminUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("abcd1234");
            db.SaveChanges();
        }
        Console.WriteLine("[DB] Role + admin migration completed.");

        // LoginMethod migration: set legacy for existing registered users
        db.Database.ExecuteSqlRaw(@"UPDATE ""SiteUsers"" SET ""LoginMethod"" = 'legacy' WHERE ""IsRegistered"" = TRUE AND (""LoginMethod"" IS NULL OR ""LoginMethod"" = '');");
        Console.WriteLine("[DB] LoginMethod migration completed.");
    }
    catch (Exception ex) { Console.WriteLine($"[DB] Role migration note: {ex.Message}"); }

    try { SeedData.Initialize(db); Console.WriteLine("[Seed] Seed completed."); }
    catch (Exception ex) { Console.WriteLine($"[Seed] Seed error: {ex.Message}"); }

    try { await SeedBattleQuestions.SeedAsync(db); Console.WriteLine("[Seed] Battle questions seeded."); }
    catch (Exception ex) { Console.WriteLine($"[Seed] Battle questions seed error: {ex.Message}"); }
}

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

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
app.UseCors("IntegrationPolicy");
app.UseSession();
app.UseAuthentication();

// 匿名 ID 系統：Cookie 持久化（365天）+ 自動建立資料庫記錄
// Throttle LastActiveAt writes: only update DB at most once every 5 minutes per user
var _lastActiveSeen = new System.Collections.Concurrent.ConcurrentDictionary<string, DateTime>();

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
        // Throttle LastActiveAt DB write to once per 5 minutes per user
        var now = DateTime.Now;
        if (!_lastActiveSeen.TryGetValue(anonymousId, out var lastSeen) || (now - lastSeen).TotalMinutes >= 5)
        {
            user.LastActiveAt = now;
            await db.SaveChangesAsync();
            _lastActiveSeen[anonymousId] = now;
        }
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

// Attribute-routed API controllers（例如 /api/integration/*）
app.MapControllers();

app.MapHub<ChatHub>("/chathub");
app.MapHub<DotNetLearning.Hubs.BattleHub>("/battleHub");

app.Run();
