using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_DevLearn
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── DevLearn Chapter 700 ────────────────────────────
        new() { Id=700, Category="project", Order=20, Level="advanced", Icon="🚀", Title="實戰：DevLearn 學習平台開發全紀錄", Slug="devlearn-platform-development", IsPublished=true, Content=@"
# 實戰：DevLearn 學習平台開發全紀錄

> **開發者：Mike（邱瀚賢）的 Side Project**
> 一個人從零到一，打造完整的 .NET 學習平台

---

## 📌 專案簡介

DevLearn 是一個完整的 .NET 學習平台，具備 **互動式教學、遊戲化學習、老師媒合、即時聊天** 等功能。
這不是教科書範例，而是一個真正上線、持續迭代的 Side Project。

### 技術棧總覽

```
前端：Razor Views + Vanilla JS + Prism.js 語法高亮 // 前端技術組合
後端：ASP.NET Core 8 MVC + EF Core 8               // 後端框架
資料庫：PostgreSQL（Railway / Azure 雲端）           // 資料庫選擇
即時通訊：SignalR（WebSocket / Long Polling）        // 即時功能
部署：Railway + Azure App Service 雙平台             // 部署方式
AI 助手：Claude Code（開發、除錯、程式碼產生）       // AI 輔助開發
```

---

## 🏗️ 架構設計

### 資料庫設計：25 個資料表

```csharp
// AppDbContext.cs — 定義所有資料表 // 資料庫上下文
public class AppDbContext : DbContext // 繼承 DbContext 基底類別
{
    // 核心學習模組 // Core learning tables
    public DbSet<Chapter> Chapters { get; set; }       // 教學章節（93+ 章）
    public DbSet<Question> Questions { get; set; }     // 測驗題目
    public DbSet<UserProgress> UserProgress { get; set; } // 使用者進度

    // 遊戲化模組 // Gamification tables
    public DbSet<CheckIn> CheckIns { get; set; }       // 每日簽到
    public DbSet<SpeedRecord> SpeedRecords { get; set; } // 速度挑戰紀錄
    public DbSet<CodePuzzle> CodePuzzles { get; set; } // 填字遊戲
    public DbSet<BugChallenge> BugChallenges { get; set; } // 程式碼偵探

    // 老師媒合模組 // Teacher matching tables
    public DbSet<Teacher> Teachers { get; set; }       // 老師資料
    public DbSet<TeacherSlot> TeacherSlots { get; set; } // 老師可預約時段
    public DbSet<Booking> Bookings { get; set; }       // 預約紀錄
    public DbSet<TeacherReview> TeacherReviews { get; set; } // 學生評價

    // 社群與即時通訊 // Community & chat tables
    public DbSet<ChatMessage> ChatMessages { get; set; } // 公開聊天訊息
    public DbSet<PrivateMessage> PrivateMessages { get; set; } // 私人訊息
    public DbSet<SupportTicket> SupportTickets { get; set; } // 客服工單

    // 使用者與權限 // User & permission tables
    public DbSet<SiteUser> SiteUsers { get; set; }     // 使用者帳號
    public DbSet<AuditLog> AuditLogs { get; set; }     // 操作審計日誌
    public DbSet<ErrorLog> ErrorLogs { get; set; }     // 錯誤日誌
}
```

### 4 種角色 RBAC 權限

```csharp
// 角色權限對照表 // Role-based access control
public enum UserRole // 定義使用者角色列舉
{
    Guest,   // 訪客：可瀏覽章節、公開聊天 // 最低權限
    Member,  // 會員：可測驗、簽到、收藏、私訊 // 基本權限
    Teacher, // 老師：可管理時段、回覆預約 // 教學權限
    Admin    // 管理員：完整後台 CRUD + 審核 // 最高權限
}

// Middleware 判斷角色 // 角色驗證中介軟體
public class RoleMiddleware // 自訂中介軟體類別
{
    public async Task InvokeAsync(HttpContext context) // 處理每個請求
    {
        var user = await GetCurrentUser(context); // 取得當前使用者
        context.Items[""CurrentUser""] = user;     // 存入 HttpContext
        if (user?.IsBanned == true)                // 檢查是否被封鎖
        {
            context.Response.StatusCode = 403;     // 回傳 403 禁止存取
            return;                                // 中斷請求
        }
        await _next(context);                      // 繼續下一個中介軟體
    }
}
```

### 雙平台部署架構

```
開發環境 (localhost:5001)
    │
    ├── Railway 部署（初期）     // 免費方案，自動 CI/CD
    │     └── PostgreSQL on Railway // 雲端資料庫
    │
    └── Azure App Service（正式）// 穩定、可擴展
          └── Azure PostgreSQL    // 正式環境資料庫

部署流程：
  dotnet publish -c Release -o ./publish  // 編譯發佈版本
  cd publish && zip -r ../app.zip .       // 打包成 zip
  az webapp deploy --name devlearn        // 部署到 Azure
      --resource-group myRG               // 指定資源群組
      --src-path ../app.zip               // 指定 zip 路徑
```

---

## 🎯 核心功能詳解

### 1. 動態章節系統（93+ 章，14 個分類）

```csharp
// SeedData.cs — 從多個檔案載入章節 // 種子資料載入
public static void Initialize(AppDbContext db) // 初始化資料庫
{
    var chapters = new List<Chapter>();             // 建立章節清單
    chapters.AddRange(SeedChapters_CSharpBase.GetChapters()); // C# 基礎
    chapters.AddRange(SeedChapters_AspNet.GetChapters());     // ASP.NET
    chapters.AddRange(SeedChapters_Database.GetChapters());   // 資料庫
    chapters.AddRange(SeedChapters_Docker.GetChapters());     // Docker
    chapters.AddRange(SeedChapters_AI.GetChapters());         // AI 應用
    // ... 共 20+ 個分類檔案 // 每個檔案包含多個章節
    db.Chapters.AddRange(chapters);                // 批次寫入資料庫
    db.SaveChanges();                              // 儲存變更
}

// 每個章節的資料結構 // Chapter model
public class Chapter // 章節模型
{
    public int Id { get; set; }          // 章節編號（唯一）
    public string Category { get; set; } // 分類（csharp, aspnet, docker...）
    public int Order { get; set; }       // 排序順序
    public string Level { get; set; }    // 難度（beginner/intermediate/advanced）
    public string Icon { get; set; }     // 圖示 emoji
    public string Title { get; set; }    // 章節標題
    public string Content { get; set; }  // Markdown 內容
    public bool IsPublished { get; set; } // 是否已發佈
}
```

### 2. 遊戲化學習（5 種機制）

```csharp
// 每日簽到系統 // Daily check-in system
[HttpPost] // 處理 POST 請求
public async Task<IActionResult> DoCheckIn() // 執行簽到
{
    var user = await GetOrCreateUser();     // 取得或建立使用者
    var today = DateTime.UtcNow.Date;       // 取得今天日期（UTC）
    var existing = await _db.CheckIns       // 查詢今日是否已簽到
        .FirstOrDefaultAsync(c => c.UserId == user.Id // 比對使用者
            && c.CheckInDate == today);     // 比對日期

    if (existing != null)                   // 如果已經簽到過
        return Json(new { success = false, message = ""今日已簽到"" }); // 回傳提示

    var streak = await CalcStreak(user.Id); // 計算連續簽到天數
    var bonus = streak >= 7 ? 20 : (streak >= 3 ? 10 : 5); // 連續獎勵加成

    _db.CheckIns.Add(new CheckIn           // 新增簽到紀錄
    {
        UserId = user.Id,                   // 使用者 ID
        CheckInDate = today,                // 簽到日期
        StreakDays = streak + 1,            // 連續天數 +1
        BonusPoints = bonus                 // 獎勵分數
    });
    user.TotalScore += bonus;               // 更新總分
    await _db.SaveChangesAsync();           // 儲存到資料庫
    return Json(new { success = true, streak = streak + 1, bonus }); // 回傳結果
}

// 速度挑戰 // Speed challenge
// 程式碼填字遊戲 // Code puzzle
// 每日一題 // Daily quiz
// 程式碼偵探（找 Bug）// Bug detective
```

### 3. 老師媒合系統

```csharp
// 老師申請流程 // Teacher application process
[HttpPost] // 處理老師申請
public async Task<IActionResult> Apply(TeacherApplication model) // 提交申請
{
    var user = await GetCurrentUser();         // 取得當前使用者
    if (!user.IsRegistered)                    // 檢查是否已註冊
        return Json(new { success = false, message = ""請先註冊"" }); // 未註冊不可申請

    var teacher = new Teacher                  // 建立老師資料
    {
        SiteUserId = user.Id,                  // 關聯使用者 ID
        DisplayName = model.DisplayName,       // 顯示名稱
        Expertise = model.Expertise,           // 專業領域
        Introduction = model.Introduction,     // 自我介紹
        HourlyRate = model.HourlyRate,         // 時薪
        IsApproved = false                     // 預設未審核
    };
    _db.Teachers.Add(teacher);                 // 新增到資料庫
    await _db.SaveChangesAsync();              // 儲存
    return Json(new { success = true });       // 回傳成功
}

// 預約流程：學生選時段 → 送出預約 → 老師確認 → 上課 → 評價
// Booking flow: select slot → book → teacher confirm → class → review
```

### 4. 即時聊天（SignalR 三分頁）

```csharp
// ChatHub.cs — SignalR 即時通訊中樞 // Real-time chat hub
public class ChatHub : Hub // 繼承 SignalR Hub
{
    public async Task SendMessage(           // 發送訊息方法
        string nickname,                     // 暱稱
        string message,                      // 訊息內容
        string avatarEmoji)                  // 頭像 emoji
    {
        var time = DateTime.Now.ToString(""HH:mm""); // 格式化時間
        await Clients.All.SendAsync(         // 廣播給所有連線用戶
            ""ReceiveMessage"",               // 事件名稱
            nickname, message,               // 暱稱和訊息
            avatarEmoji, time);              // 頭像和時間
        await SaveToDb(nickname, message, avatarEmoji); // 存入資料庫
    }

    public async Task GetRecentMessages()    // 取得歷史訊息
    {
        var msgs = await _db.ChatMessages    // 查詢聊天訊息
            .OrderByDescending(m => m.SentAt) // 按時間倒序
            .Take(50)                        // 取最近 50 筆
            .OrderBy(m => m.SentAt)          // 正序排列
            .ToListAsync();                  // 執行查詢
        await Clients.Caller.SendAsync(""LoadHistory"", msgs); // 回傳給請求者
    }

    // 三分頁：公開聊天 / 私人訊息 / 客服工單
    // Three tabs: Public chat / Private messages / Support tickets
}
```

### 5. 角色權限系統（RBAC + 封鎖）

```csharp
// AdminController.cs — 管理員操作 // Admin dashboard
[HttpPost] // 封鎖使用者
public async Task<IActionResult> BanUser(int userId, string reason) // 封鎖帳號
{
    var user = await _db.SiteUsers.FindAsync(userId); // 查詢使用者
    if (user == null) return NotFound();     // 找不到回傳 404
    user.IsBanned = true;                    // 設定封鎖狀態
    user.BanReason = reason;                 // 記錄封鎖原因
    user.BannedAt = DateTime.UtcNow;         // 記錄封鎖時間

    _db.AuditLogs.Add(new AuditLog           // 寫入審計日誌
    {
        Action = ""BanUser"",                 // 操作類型
        TargetId = userId.ToString(),        // 目標使用者
        Detail = reason,                     // 詳細原因
        CreatedAt = DateTime.UtcNow          // 操作時間
    });
    await _db.SaveChangesAsync();            // 儲存變更
    return Json(new { success = true });     // 回傳成功
}
```

### 6. 自動錯誤偵測（ErrorLog + AI 修復排程）

```csharp
// 前端自動回報錯誤 // Frontend auto error reporting
// _Layout.cshtml 中的全方位錯誤偵測腳本 // Error detection script
window.onerror = function(msg, url, line, col, err) { // 攔截 JS 錯誤
    fetch('/api/ErrorLog/report', {          // 呼叫後端 API
        method: 'POST',                      // 使用 POST 方法
        headers: {'Content-Type': 'application/json'}, // JSON 格式
        body: JSON.stringify({               // 組裝錯誤資料
            pageUrl: location.href,          // 發生錯誤的頁面
            errorMessage: msg,               // 錯誤訊息
            stackTrace: err?.stack || '',    // 堆疊追蹤
            source: 'frontend-js'            // 錯誤來源
        })
    });
};

// 後端 ErrorLog 控制器 // Backend error log controller
// 偵測範圍：JS 錯誤、Promise rejection、API 500、
//          資源載入失敗、慢速頁面、Console.error、
//          手機端觸控凍結、SignalR 斷線
// 共 8 大類自動偵測 // 8 categories of auto-detection
```

### 7. 管理後台（20+ 頁面）

```csharp
// 管理後台功能清單 // Admin dashboard features
public class AdminController : Controller   // 管理員控制器
{
    // 儀表板：即時統計 // Dashboard: real-time stats
    // 使用者管理：列表、封鎖、角色切換 // User management
    // 章節管理：新增、編輯、發佈 // Chapter management
    // 老師審核：申請審核、資料管理 // Teacher approval
    // 預約管理：查看、處理預約 // Booking management
    // 客服工單：回覆、關閉工單 // Support tickets
    // 錯誤日誌：查看、標記修復 // Error logs
    // 審計日誌：所有操作紀錄 // Audit logs
    // AI 工作紀錄：Claude 修復歷史 // AI work history

    [HttpGet] // 儀表板首頁
    public async Task<IActionResult> Dashboard() // 載入儀表板
    {
        var stats = new DashboardStats       // 建立統計物件
        {
            TotalUsers = await _db.SiteUsers.CountAsync(),     // 總使用者數
            TotalChapters = await _db.Chapters.CountAsync(),   // 總章節數
            TodayCheckIns = await _db.CheckIns                 // 今日簽到數
                .CountAsync(c => c.CheckInDate == DateTime.UtcNow.Date), // 比對日期
            OpenTickets = await _db.SupportTickets             // 未處理工單
                .CountAsync(t => t.Status == ""pending""),      // 狀態為待處理
            RecentErrors = await _db.ErrorLogs                 // 近期錯誤
                .OrderByDescending(e => e.CreatedAt)           // 按時間排序
                .Take(10).ToListAsync()                        // 取最近 10 筆
        };
        return View(stats);                  // 回傳檢視
    }
}
```

---

## 🚀 部署流程

```bash
# 1. 本地編譯發佈版本 // Build release version
dotnet publish -c Release -o ./publish     # 編譯到 publish 資料夾

# 2. 打包成 zip 檔 // Package as zip
cd publish && zip -r ../app.zip .          # 壓縮所有檔案

# 3. 部署到 Azure // Deploy to Azure App Service
az webapp deploy \                          # 使用 Azure CLI 部署
    --name devlearn-app \                   # 應用程式名稱
    --resource-group myResourceGroup \      # 資源群組名稱
    --src-path ../app.zip \                 # zip 檔路徑
    --type zip                              # 部署類型

# 4. 設定環境變數 // Configure environment variables
az webapp config appsettings set \          # 設定應用程式設定
    --name devlearn-app \                   # 應用程式名稱
    --settings \                            # 設定項目
    ConnectionStrings__Default=""Host=...""  # 資料庫連線字串

# 5. 檢查部署狀態 // Check deployment status
az webapp log tail --name devlearn-app      # 即時查看日誌
```

---

## 🔥 遇到的挑戰與解決方案

### 挑戰 1：Railway 部署超時 → 改用 Azure

```csharp
// 問題：Railway 免費方案有 500 小時/月限制 // Railway timeout issue
// 部署後經常 cold start 要 30+ 秒 // Cold start takes 30+ seconds
// 大量 Seed Data 初始化導致首次啟動更慢 // Seed data slows first boot

// 解決方案：遷移到 Azure App Service // Migrate to Azure
// Azure 的 Always On 功能避免 cold start // Always On prevents cold start
var builder = WebApplication.CreateBuilder(args); // 建立應用程式
builder.Services.AddResponseCompression();  // 加入回應壓縮
builder.Services.AddMemoryCache();          // 加入記憶體快取

// 加上健康檢查端點 // Health check endpoint
app.MapGet(""/health"", () => ""OK"");        // 讓 Azure 定期 ping
```

### 挑戰 2：SQLite 資料消失 → 改用 PostgreSQL

```csharp
// 問題：Railway 使用 SQLite，每次重新部署資料庫都被覆蓋 // SQLite data loss
// 使用者的簽到紀錄、學習進度全部消失 // All user data lost on redeploy

// 解決方案：改用 PostgreSQL 雲端資料庫 // Switch to PostgreSQL
// Program.cs 中的連線設定 // Connection configuration
var connStr = builder.Configuration          // 讀取設定
    .GetConnectionString(""Default"");        // 取得連線字串
builder.Services.AddDbContext<AppDbContext>(  // 註冊 DbContext
    options => options.UseNpgsql(connStr));   // 使用 Npgsql 驅動

// appsettings.json 設定 // Application settings
// ""Default"": ""Host=xxx;Database=devlearn;Username=xxx;Password=xxx""
// 資料庫在雲端，不會因重新部署而消失 // Cloud DB persists across deploys
```

### 挑戰 3：手機版 RWD 問題 → 漢堡選單 + safe-area

```csharp
// 問題：導覽列在手機上擠成一團，聊天室輸入框被虛擬鍵盤遮住 // Mobile layout issues

// 解決方案 1：漢堡選單 // Hamburger menu solution
// CSS：隱藏桌面導覽，顯示漢堡按鈕 // Hide desktop nav, show hamburger
@@media (max-width: 768px) {               // 手機版媒體查詢
    .nav-right { display: none; }          // 隱藏桌面選單
    .hamburger { display: flex; }          // 顯示漢堡按鈕
}

// 解決方案 2：safe-area 處理 // Safe area for notch phones
.chat-input-area {                         // 聊天輸入區域
    padding-bottom: calc(                  // 計算底部間距
        10px + env(safe-area-inset-bottom, 0px) // 加上安全區域
    );
}
```

### 挑戰 4：Session/Cookie 不同步 → Cookie-first Middleware

```csharp
// 問題：使用者登入後 Session 過期，但 Cookie 還在 // Session expired but cookie exists
// 導致使用者以為自己還在登入狀態 // User thinks they're still logged in

// 解決方案：Cookie-first 中介軟體 // Cookie-first middleware
public class CookieFirstMiddleware          // 自訂中介軟體
{
    public async Task InvokeAsync(HttpContext context) // 處理請求
    {
        var sessionId = context.Session      // 嘗試從 Session 取得
            .GetString(""SessionId"");        // 使用者識別碼
        var cookieId = context.Request       // 嘗試從 Cookie 取得
            .Cookies[""UserSession""];         // 使用者識別碼

        if (string.IsNullOrEmpty(sessionId)  // 如果 Session 為空
            && !string.IsNullOrEmpty(cookieId)) // 但 Cookie 有值
        {
            context.Session.SetString(       // 從 Cookie 恢復 Session
                ""SessionId"", cookieId);      // 保持使用者登入狀態
        }
        await _next(context);               // 繼續處理請求
    }
}
```

---

## 💡 開發心得：用 Claude Code 做 AI 開發助手

```csharp
// Claude Code 在這個專案中扮演的角色 // Claude Code's role in this project

// 1. 程式碼產生：快速產出 Controller、View、Model // Code generation
// 2. 除錯協助：分析錯誤日誌，提供修復建議 // Debugging help
// 3. 架構設計：討論資料表設計、API 設計 // Architecture design
// 4. 文件撰寫：產生教學章節內容（包含這一章！）// Documentation
// 5. 程式碼審查：發現潛在 Bug 和效能問題 // Code review

// 開發效率提升要點 // Development efficiency tips
var tips = new List<string>                 // 建立心得清單
{
    ""明確描述需求，AI 才能給出精確回答"",   // 清楚的 prompt 很重要
    ""先理解 AI 產出的程式碼，再貼入專案"",   // 不要盲目複製貼上
    ""利用 AI 產出測試案例，提高品質"",       // 用 AI 寫測試
    ""讓 AI 幫忙重構，但架構決策自己來"",     // AI 是助手不是主導
    ""定期回顧 AI 建議，學習新的寫法""        // 從 AI 身上學習
};

// 最終心得：AI 不會取代開發者 // Final thought
// 但會用 AI 的開發者，會取代不會用的 // AI-skilled devs will outpace others
// DevLearn 就是最好的證明 🚀 // DevLearn is living proof
```

---

## 🤔 我這樣寫為什麼會錯？

### 常見錯誤 1：部署時忘記設定環境變數

```csharp
// ❌ 錯誤寫法：把連線字串寫死在程式碼裡 // Hardcoded connection string
var connStr = ""Host=localhost;Database=dev;Password=123""; // 寫死的連線字串
// 部署到雲端後還是連 localhost，當然連不上！ // Will fail on cloud deployment

// ✅ 正確寫法：從環境變數或設定檔讀取 // Read from config
var connStr = builder.Configuration          // 從設定中讀取
    .GetConnectionString(""Default"");        // 取得連線字串
// 雲端透過環境變數覆寫 // Override via environment variables on cloud
```

### 常見錯誤 2：SignalR 在手機上連不上

```csharp
// ❌ 錯誤寫法：只用 WebSocket // WebSocket only
var connection = new signalR.HubConnectionBuilder() // 建立 SignalR 連線
    .withUrl('/chathub')                     // 只用預設 WebSocket
    .build();                                // 手機瀏覽器可能不支援

// ✅ 正確寫法：加上 Long Polling 作為備援 // Add Long Polling fallback
var connection = new signalR.HubConnectionBuilder() // 建立 SignalR 連線
    .withUrl('/chathub', {                   // 指定傳輸方式
        transport: signalR.HttpTransportType.LongPolling // 使用 Long Polling
    })
    .withAutomaticReconnect()                // 加上自動重連
    .build();                                // 確保手機端也能連線
```

### 常見錯誤 3：忘記處理 null 值

```csharp
// ❌ 錯誤寫法：直接存取可能為 null 的物件 // Accessing nullable without check
var nickname = user.Nickname;                // 如果 user 是 null 就炸了

// ✅ 正確寫法：使用 null 條件運算子 // Use null conditional operator
var nickname = user?.Nickname ?? ""訪客"";    // 如果 null 就用預設值
// 或者先檢查 // Or check first
if (user == null)                            // 先判斷是否為 null
    return Json(new { error = ""未登入"" });   // 回傳錯誤訊息
```

---

## 📊 專案數據總結

```
總程式碼行數：   50,000+ 行          // Total lines of code
教學章節：       93+ 章（14 分類）   // Teaching chapters
資料表：         25 個               // Database tables
Controller：     15+ 個              // API controllers
View 頁面：      30+ 個              // Razor views
管理後台頁面：   20+ 個              // Admin pages
遊戲化機制：     5 種                // Gamification types
即時功能：       聊天 + 私訊 + 客服  // Real-time features
AI 輔助修復：    自動偵測 + 排程修復 // Auto error detection
部署方式：       Azure App Service   // Deployment platform
開發期間：       持續迭代中 🔄       // Ongoing development
```

> **這個專案證明了：一個人 + AI 助手，也能打造出完整的全端應用。**
> **重點不是程式碼多完美，而是不斷迭代、持續改進。**

" },
    };
}
