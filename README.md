# 🎯 DevLearn — 程式設計學習平台

> 一位開發者、六個月、232 章教學、8 款遊戲、60+ 資料表。  
> 這不是一個 Todo App，這是一個真正有人在用的線上學習平台。

**🌐 立刻體驗**：[https://devlearn-dotnet.azurewebsites.net/](https://devlearn-dotnet.azurewebsites.net/)

![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core_MVC-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-0078D4?style=for-the-badge&logo=microsoft&logoColor=white)
![Azure](https://img.shields.io/badge/Azure-0078D4?style=for-the-badge&logo=microsoftazure&logoColor=white)

---

## 📑 快速導覽（面試官請點選）

- [💡 為什麼做這個專案？](#-為什麼做這個專案)
- [🎬 功能展示（5 分鐘體驗流程）](#-功能展示)
- [🛠️ 技術決策與思考](#️-技術決策與思考)
- [🔥 實作亮點（含程式碼）](#-實作亮點)
- [🧗 我解決了哪些難題？](#-我解決了哪些難題)
- [📊 專案規模](#-專案規模)
- [🚀 本地開發](#-本地開發)

---

## 💡 為什麼做這個專案？

我從物流系統開發轉型全端，在學習 ASP.NET Core 的過程中發現：
- 市面上的教學大多只是語法堆疊，缺乏「為什麼這樣做」的說明
- 單純做 Todo App 練習無法驗證自己是否真的能做一個完整產品
- 面試時被問到「DI 為什麼不直接 new？」才發現自己只會寫語法，不懂原理

**所以我決定做一個自己也會想用的學習平台**，把一路踩過的坑、想通的概念、練過的語法，全部整理成互動式內容。

過程中需要用到的每個技術（MVC、EF Core、SignalR、Phaser.js、Azure 部署...），都是在實際遇到需求時才學習，這樣每個知識點都有實戰應用，而不只是背誦。

---

## 🎬 功能展示

> 建議按順序體驗（共約 5 分鐘）

### 1️⃣ 首頁 — 章節導覽
🔗 [https://devlearn-dotnet.azurewebsites.net/](https://devlearn-dotnet.azurewebsites.net/)
- 232 章教學分成 25 個分類
- 核心路線（C# → ASP.NET → 資料庫）+ 進階模組
- 每章都是中斷點式逐行解析（不只講語法，還解釋「為什麼」）

### 2️⃣ 概念深入篇 ⭐（重點看這裡）
🔗 [後端概念深入](https://devlearn-dotnet.azurewebsites.net/Home/Chapter/concept-di-why)
- 「DI 依賴注入：為什麼不直接 new？」
- 「async/await 的真相：不是多執行緒」
- 「Middleware vs Filter：什麼時候用哪個？」
- 這些章節專門整理面試常考的觀念，不只是語法

### 3️⃣ AI 教你打程式（互動學習）
🔗 [https://devlearn-dotnet.azurewebsites.net/CodeTutor](https://devlearn-dotnet.azurewebsites.net/CodeTutor)
- 85 課互動式程式輸入，AI 一行一行教你打
- 7 個分類（C#、JS、HTML、SQL、Vue、API、Java）
- 即時答題驗證，準確率評分

### 4️⃣ 程式碼大富翁（遊戲化）
🔗 [https://devlearn-dotnet.azurewebsites.net/Monopoly](https://devlearn-dotnet.azurewebsites.net/Monopoly)
- Phaser.js 3 實作的 2D 棋盤遊戲
- 6 種角色 + vs AI 或多人本地對戰
- 36 格棋盤、命運卡陷阱、蓋房機制
- 答對題目才能買地

### 5️⃣ 即時聊天室（SignalR）
🔗 [https://devlearn-dotnet.azurewebsites.net/](https://devlearn-dotnet.azurewebsites.net/)（右下角）
- WebSocket 即時通訊
- 支援回覆、Emoji 反應、點擊 nickname 私訊

### 6️⃣ 程式碼對戰（PvP）
🔗 [https://devlearn-dotnet.azurewebsites.net/Battle](https://devlearn-dotnet.azurewebsites.net/Battle)
- SignalR 房間系統
- 即時配對、同步答題、結算排名

---

## 🛠️ 技術決策與思考

### 為什麼選 ASP.NET Core MVC（而不是 SPA + API）？

```
選擇的考量：
✅ 我的專長是 C#，一個人開發時用 MVC 最快
✅ 教學平台重視 SEO，SSR 天然優勢
✅ 不需要前後端分離的場景（不用手機 App）
✅ Razor Views 搭配 Vanilla JS 對學習者更友善（打開 F12 就能看）

取捨：
❌ 犧牲了「換前端技術」的彈性
❌ 如果未來要做手機 App 會需要重構成 API
```

### 為什麼選 PostgreSQL（而不是 SQL Server）？

```
✅ Azure PostgreSQL Flexible Server 比 SQL Server 便宜
✅ 開源、社群大、跨平台
✅ JSONB 欄位支援複雜資料（章節的 OptionsJson）
✅ Window Functions 原生支援好
```

### 為什麼用 Vanilla JS（而不是 React/Vue）？

```
✅ 教學平台的互動不複雜，用框架反而增加學習者理解負擔
✅ 載入速度快（不需要打包）
✅ 符合「教學相長」的精神：自己寫的也教給學習者

例外：
⚠️ 大富翁遊戲用了 Phaser.js 3（2D 遊戲引擎不可能用 Vanilla JS 實作）
```

---

## 🔥 實作亮點

### 🔹 SignalR 即時通訊架構

> 同時支援：聊天室、PvP 程式碼對戰、線上課堂

**設計重點**：每種即時服務都有獨立的 Hub，避免單一 Hub 肥大

```csharp
public class ChatHub : Hub
{
    public async Task SendMessage(string nickname, string message)
    {
        var msg = new ChatMessage { /* ... */ };
        _db.ChatMessages.Add(msg);
        await _db.SaveChangesAsync();

        // 廣播給所有連線的客戶端
        await Clients.All.SendAsync("ReceiveMessage", msg);
    }
}
```

### 🔹 背景服務自動掃描錯誤

> ErrorScannerService 每 6 小時自動掃描 ErrorLogs，自動分類瀏覽器雜訊

```csharp
public class ErrorScannerService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await ScanErrorsAsync();
            await Task.Delay(TimeSpan.FromHours(6), ct);
        }
    }
}
```

### 🔹 EF Core Migration 自動化

> 應用啟動時自動檢查並執行 Migration，不需要手動 `dotnet ef database update`

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    SeedData.Initialize(db);  // 匯入 232 章內容
}
```

### 🔹 Server-Side 答案驗證（防作弊）

> 測驗答案絕不傳給 client，避免 JS debug 取得答案

```csharp
[HttpPost]
public IActionResult CheckAnswer(int questionId, int answer)
{
    var q = _db.Questions.Find(questionId);
    // 答案在 server 驗證，client 只收到 true/false
    return Json(new { correct = q.CorrectIndex == answer });
}
```

### 🔹 Phaser.js 遊戲引擎整合

> 不用重寫現有 MVC 架構，Phaser 遊戲嵌在 Razor View 裡，跟後端 API 無縫對接

```javascript
// QuestionManager.js — 從 MVC Controller 取題目
class QuestionManager {
    async loadQuestions(category) {
        const res = await fetch(`/Monopoly/GetQuestions?category=${category}`);
        this.questions = await res.json();
    }
}
```

---

## 🧗 我解決了哪些難題？

### 1. Phaser Scale.FIT 的座標偏移問題
**問題**：Phaser canvas 被瀏覽器縮放後，`pointer.x/y` 座標不是我預期的值，導致骰子按鈕按不到  
**研究**：閱讀 Phaser Input Manager 原始碼，發現 `scrollFactor(0)` 物件的 hitArea 是 world 座標  
**解法**：改用 `container + update()` 每幀重新定位，讓 UI 永遠在相機中心

### 2. EF Core N+1 問題
**問題**：首頁載入 232 章時查了 232 次 DB（每章都 lazy load 作者）  
**解法**：改用 `Include()` 一次撈完，或用 `Select()` 只取需要的欄位

### 3. Azure Free Tier 冷啟動
**問題**：閒置 20 分鐘後重啟要 1-3 分鐘  
**解法**：GitHub Actions Cron Job 每 10 分鐘自動 ping 一次（`keep-alive.yml`）

### 4. SignalR 連線管理
**問題**：使用者開多個分頁時，訊息會重複發送  
**解法**：用 `Context.ConnectionId` + `Groups` 管理房間，而不是用 UserId

### 5. 部署時的資料保護
**問題**：部署新版時要保留正式資料，但 schema 有變動  
**解法**：Migration 自動執行，SeedData 只補不存在的資料（`ON CONFLICT DO NOTHING`）

---

## 🤖 CI/CD 自動化

使用 **GitHub Actions** 建立完整的 CI/CD pipeline：

### 🚀 Auto Deploy（`.github/workflows/deploy.yml`）
```
git push main
    ↓
GitHub Actions 自動觸發
    ├── ⬇️ Checkout code
    ├── 🛠️ Setup .NET 8
    ├── 📦 Restore + Build
    ├── 📤 Publish
    └── 🚀 Deploy to Azure App Service
         ↓
    正式站 3 分鐘內更新完成 ✅
```

**核心機制**：
- 使用 `azure/webapps-deploy@v3` action 透過 Azure Publish Profile 部署
- 敏感資訊（publish profile）存於 GitHub Secrets（`AZURE_PUBLISH_PROFILE`）
- 支援手動觸發（`workflow_dispatch`）

### ☕ Keep-Alive Cron（`.github/workflows/keep-alive.yml`）
```yaml
on:
  schedule:
    - cron: '*/10 * * * *'   # 每 10 分鐘
```
解決 Azure Free Tier 閒置 20 分鐘冷啟動問題。

### 成果
| 過去（手動） | 現在（CI/CD） |
|------------|--------------|
| 本機 `dotnet publish` → zip → `python deploy.py` → 等 5 分鐘 | `git push` → ☕ → 自動部署完成 |
| 5 分鐘人工操作 | 0 分鐘（全自動） |
| 有遺漏步驟風險 | 每次流程 100% 一致 |

---

## 📊 專案規模

| 項目 | 數量 |
|------|------|
| **程式碼** | 30+ Controllers、60+ Views、40+ Seed Files |
| **資料庫** | 60+ Tables、500+ Questions |
| **教學內容** | 232 章節、25 分類、28 章面試概念篇 |
| **遊戲** | 8 款（AI Code Tutor、Monopoly、Battle、Arena、Detective、Speed、Puzzle、SqlGame） |
| **即時功能** | 3 個 SignalR Hubs（Chat、Battle、Hub） |
| **Commit 數** | 100+ 次（含完整歷史） |
| **開發時間** | 6 個月 |
| **開發人數** | 1 人（獨立開發） |

---

## 🏗️ 技術架構

```
┌──────────────────────────────────────────────────┐
│  Browser                                          │
│  ├── Razor Views (Server-Side Rendered)          │
│  ├── Vanilla JS + jQuery (互動)                  │
│  ├── Phaser.js 3 (2D 遊戲引擎)                   │
│  └── SignalR Client (WebSocket)                  │
└────────────────────┬─────────────────────────────┘
                     │ HTTP / WebSocket
┌────────────────────▼─────────────────────────────┐
│  ASP.NET Core 8.0 MVC                            │
│  ├── Controllers (30+)                           │
│  ├── Razor Views (60+)                           │
│  ├── SignalR Hubs (ChatHub, BattleHub)          │
│  ├── Background Services                         │
│  │   ├── ErrorScannerService (每 6 小時)         │
│  │   └── CheckInResetService (每日)              │
│  ├── Middleware (認證、錯誤處理、日誌)            │
│  └── Dependency Injection (Scoped DbContext)     │
└────────────────────┬─────────────────────────────┘
                     │ EF Core 8.0
┌────────────────────▼─────────────────────────────┐
│  PostgreSQL 15 (Azure Flexible Server)           │
│  └── 60+ Tables (Users, Chapters, Games, Teachers) │
└──────────────────────────────────────────────────┘

部署：Azure App Service (Linux)
CI/CD：手動 publish + Python zip 自動部署
```

---

## 📸 截圖

> 以下為實際運行畫面（非 AI 生成）

### 首頁學習路徑
![首頁](./docs/screenshots/home.png)

### AI 教你打程式
![AI Code Tutor](./docs/screenshots/code-tutor.png)

### 程式碼大富翁
![Monopoly](./docs/screenshots/monopoly.png)

### 即時聊天室
![Chat](./docs/screenshots/chat.png)

> **說明：** 截圖檔尚未上傳。本機開發完成後，請到 live demo 截圖後放到 `/docs/screenshots/` 資料夾。

---

## 🚀 本地開發

### 環境需求
- .NET 8 SDK
- PostgreSQL 15+

### 快速開始

```bash
git clone https://github.com/a0936480350/DevLearn.git
cd DevLearn

# 設定資料庫連線（appsettings.Development.json）
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=devlearn;Username=postgres;Password=你的密碼"
  }
}

# 執行（首次啟動會自動建表、匯入 232 章內容）
dotnet run
```

### 管理員登入
- URL：`/Admin`
- 預設：`admin` / `1234`（上線前請改）

---

## 📂 專案結構

```
DotNetLearning/
├── Controllers/              # 30+ MVC Controllers
│   ├── HomeController.cs
│   ├── MonopolyController.cs         # Phaser 遊戲 API
│   ├── CodeTutorController.cs        # AI 教打程式
│   ├── BattleController.cs           # SignalR PvP
│   └── ...
├── Models/                   # EF Core Entities
├── Views/                    # Razor Views (60+)
├── Data/
│   ├── AppDbContext.cs       # 60+ DbSets
│   ├── SeedData.cs           # 匯入教學內容
│   └── SeedChapters_*.cs     # 40+ 章節種子檔
├── Services/                 # Background Services
│   └── ErrorScannerService.cs
├── Hubs/                     # SignalR Hubs
├── wwwroot/
│   ├── css/
│   ├── js/
│   │   ├── site.js
│   │   └── games/monopoly/   # Phaser 遊戲（10+ JS 檔）
│   └── lib/
└── Program.cs
```

---

## 🎓 我在這個專案學到什麼？

### 技術面
- **DI 容器** — 理解為什麼不直接 new，三種生命週期的取捨
- **async/await** — 不是多執行緒，是執行緒在等待時做別的事
- **EF Core** — Include、AsNoTracking、Migration、N+1 問題
- **SignalR** — WebSocket、Hub、Group、Connection 管理
- **Phaser.js** — 2D 遊戲引擎、Scene、Camera、Input 系統
- **Azure** — App Service、PostgreSQL Flexible Server、環境變數

### 架構面
- **MVC vs API** — 什麼場景該用哪個
- **Repository Pattern** — 不是每個專案都需要
- **Background Service** — 長時間任務的正確做法
- **Server-Side Validation** — 永遠不要相信 client

### 軟實力
- **從 0 到 1** — 一個人扛完需求、設計、實作、部署、維運
- **持續重構** — 不是一次寫對，是一直修正
- **產品思維** — 使用者實際會用到什麼，不是寫爽的

---

## 📬 聯絡

- 👤 **作者**：Mike ([@a0936480350](https://github.com/a0936480350))
- 🌐 **Live Demo**：[devlearn-dotnet.azurewebsites.net](https://devlearn-dotnet.azurewebsites.net/)
- 📱 **LINE 官方**：[lin.ee/68vD9ZW](https://lin.ee/68vD9ZW)

---

<sub>⭐ 如果這個專案對你有幫助，歡迎在 GitHub 給個 Star！</sub>
