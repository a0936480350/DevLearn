# 🎯 DevLearn — 程式設計學習平台

> 一個專為台灣開發者打造的免費程式學習平台，整合了互動教學、多款遊戲化學習、老師媒合、AI 輔助等功能。

**🌐 Live Demo**：[https://devlearn-dotnet.azurewebsites.net/](https://devlearn-dotnet.azurewebsites.net/)

---

## ✨ 功能特色

### 📚 教學內容（232 章，25 分類）
- **語法基礎篇**：C#、JavaScript、HTML/CSS、SQL、Vue、React、Angular、Docker、Git、Redis、微服務
- **概念深入篇**：後端架構思維、系統設計、前端概念（為面試準備）
- **IoT & POS 系統**：Raspberry Pi、硬體整合實戰
- **AI 輔助開發**：Prompt Engineering、Claude Code、MCP 系統

### 🎮 遊戲化學習（8 款遊戲）
- **AI 教你打程式** — 85 課互動式程式輸入（支援分類篩選）
- **程式碼大富翁** — Phaser.js 2D 棋盤遊戲（vs AI 或多人對戰）
- **SQL 模擬器** — 實戰 SQL 查詢挑戰
- **程式碼偵探** — 找出有 Bug 的程式碼
- **速度挑戰** — 限時答題排行榜
- **填字遊戲** — 補完程式碼
- **程式碼擂台** — Arena 演算法挑戰
- **⚔️ 對戰** — SignalR 即時 PvP 答題

### 👨‍🏫 老師媒合
- 老師註冊、個人頁面、時段管理
- 學生預約、評論、收藏
- 私訊系統、貼文討論

### 🤝 社群功能
- 討論區（Ideas / QnA）
- SignalR 即時聊天室（支援回覆、表情）
- 學伴媒合
- 每日簽到 + 簽到紀錄

### 🔐 帳號系統
- Email + 密碼註冊（BCrypt 加密）
- Google OAuth（可配置）
- 訪客模式（自動建立 AnonymousId）
- 忘記密碼重置

### 📊 管理後台
- 使用者管理、章節管理、公告管理
- 客服工單、錯誤掃描器（背景 Service）
- 每日統計、分類學習分析

---

## 🛠️ 技術架構

```
┌─────────────────────────────────────────┐
│   Browser (Razor View + Vanilla JS)     │
│   • 部分遊戲用 Phaser.js（大富翁）      │
│   • SignalR Client（聊天、對戰）         │
└──────────────┬──────────────────────────┘
               │ HTTP / WebSocket
┌──────────────▼──────────────────────────┐
│   ASP.NET Core 8.0 MVC                  │
│   • Controllers (30+)                   │
│   • Razor Views                         │
│   • SignalR Hubs                        │
│   • Background Services                 │
└──────────────┬──────────────────────────┘
               │ EF Core
┌──────────────▼──────────────────────────┐
│   PostgreSQL (Azure Flexible Server)    │
│   • 60+ tables                          │
└─────────────────────────────────────────┘
```

### 主要技術棧
| 層級 | 技術 |
|------|------|
| 後端 | ASP.NET Core 8.0 MVC |
| 前端 | Razor Views + Vanilla JS + Phaser.js 3 |
| 資料庫 | PostgreSQL 15 (via EF Core) |
| 即時通訊 | SignalR |
| 認證 | Cookie + BCrypt |
| 部署 | Azure App Service |
| CDN | jsDelivr (Phaser)、cdnjs (Prism, SignalR) |

---

## 🚀 本地開發

### 環境需求
- .NET 8 SDK
- PostgreSQL 15+（或用 SQLite 本地開發）
- Node.js（可選，僅用於部分工具）

### 快速開始

```bash
# 1. Clone repo
git clone https://github.com/你的帳號/DevLearn.git
cd DevLearn

# 2. 還原套件
dotnet restore

# 3. 設定資料庫連線（appsettings.Development.json）
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=devlearn;Username=postgres;Password=你的密碼"
  }
}

# 4. 執行
dotnet run
```

首次啟動會自動：
- 建立資料表
- 匯入 232 章教學內容
- 匯入 500+ 測驗題

### 管理員登入
- URL：`/Admin`
- 預設帳號：`admin` / `1234`（**上線前請改掉！**）

---

## 📁 專案結構

```
DotNetLearning/
├── Controllers/              # 30+ MVC Controllers
│   ├── HomeController.cs
│   ├── MonopolyController.cs
│   ├── CodeTutorController.cs
│   └── ...
├── Models/                   # EF Core Entities
│   ├── SiteUser.cs
│   ├── Chapter.cs
│   ├── Question.cs
│   └── ...
├── Views/                    # Razor Views
│   ├── Home/
│   ├── Monopoly/
│   └── Shared/_Layout.cshtml
├── Data/
│   ├── AppDbContext.cs       # EF Core DbContext
│   ├── SeedData.cs           # 匯入教學內容
│   └── SeedChapters_*.cs     # 40+ 章節種子檔
├── Services/                 # Background Services
│   ├── ErrorScannerService.cs
│   └── ...
├── Hubs/                     # SignalR Hubs
├── wwwroot/
│   ├── css/
│   ├── js/
│   │   ├── site.js
│   │   └── games/monopoly/   # Phaser 大富翁遊戲
│   └── lib/
└── Program.cs
```

---

## 🎓 教學內容分類

### 核心路線
🔷 **C# 基礎** → 🌐 **ASP.NET Core MVC** → 🗄️ **資料庫 & ORM**

### 進階模組
| 分類 | 章節數 |
|------|--------|
| 🎨 前端基礎 | 5 |
| 📘 SQL 查詢 | 21 |
| 🟨 JavaScript | 20 |
| 📄 HTML & CSS | 15 |
| 💚 Vue.js | 8 |
| ⚛️ React | 8 |
| 🅰️ Angular | 8 |
| 🏗️ 微服務架構 | 10 |
| 🔴 Redis 快取 | 10 |
| 🤖 AI 應用 | 6 |
| 🧠 AI 模型 | 4 |
| 🔌 Claude Code | 4 |
| 🛒 專案實戰 | 8 |
| 🍓 IoT & POS | 11 |
| 🎓 IPAS AI 證照 | 7 |
| **🧠 後端概念深入** | **10（面試準備）** |
| **🏗️ 架構概念深入** | **10（面試準備）** |
| **🎯 前端概念深入** | **8（面試準備）** |
| 其他（網路、資安、Docker、Git、設計模式…） | 30+ |

---

## 🔐 環境變數

可選的環境變數（沒設定也能跑）：

```bash
# Google OAuth（選用）
GOOGLE_CLIENT_ID=xxx
GOOGLE_CLIENT_SECRET=xxx

# SMTP（寄驗證信、密碼重置）
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=your@gmail.com
SMTP_PASS=your-app-password
SMTP_FROM=noreply@yourdomain.com

# 資料庫連線
ConnectionStrings__DefaultConnection=Host=...;Database=...;...
```

---

## 📝 已知限制

- Azure Free Tier 冷啟動需要 1-3 分鐘
- 大富翁 Phaser 遊戲骰子點擊在某些螢幕尺寸有 bug
- 尚未實作：金流串接（ECPay）、SEO 優化（sitemap、JSON-LD）

---

## 📄 授權

本專案為個人學習與作品集展示用途。

---

## 👤 作者

由 Mike 開發維護  
聯絡：透過平台的 LINE 官方帳號（[lin.ee/68vD9ZW](https://lin.ee/68vD9ZW)）

---

## 🙏 致謝

本專案大量使用 AI 輔助開發（Claude Code），展示了 AI-Assisted Development 的實際流程與價值。
