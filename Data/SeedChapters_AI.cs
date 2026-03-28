using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_AI
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── AI Application Chapter 400 ────────────────────────────
        new() { Id=400, Category="ai", Order=1, Level="beginner", Icon="🤖", Title="AI 輔助開發入門", Slug="ai-assisted-dev-intro", IsPublished=true, Content=@"
# AI 輔助開發入門

## 什麼是 AI 輔助開發？

> 💡 **比喻：超強實習生**
> 想像你有一個實習生，他：
> - 讀過幾乎所有程式語言的文件
> - 打字速度無限快
> - 24 小時不休息
> - 但他**不會主動思考**，你說什麼他做什麼
>
> 這就是 AI 輔助開發的本質——你是老闆，AI 是超強實習生。
> 你負責**想**，他負責**做**。

### 傳統開發 vs AI 輔助開發

```
傳統開發流程：
想需求 → Google 搜尋 → 看 Stack Overflow → 複製貼上 → 改到能跑 → Debug

AI 輔助開發流程：
想需求 → 告訴 AI → AI 寫好 → 你檢查 → 微調 → 完成
```

**重要觀念：AI 輔助開發不是「不用學程式」，而是「學更快、做更快」。**

你還是需要：
- 看得懂 AI 寫的 code（不然怎麼檢查？）
- 知道基本架構（不然怎麼描述需求？）
- 會 Debug（AI 也會寫錯）

---

## Claude Code 是什麼？

Claude Code 是 Anthropic 推出的 **CLI（命令列）AI 開發工具**。

> 💡 **比喻：對講機**
> 你可以想像 Claude Code 是一個對講機，
> 你對著它說：「幫我把 HomeController 的 Index 改成回傳 JSON」，
> 它就直接幫你改檔案。不用開網頁、不用複製貼上。

### Claude Code vs ChatGPT 的差別

```
功能              ChatGPT 網頁版          Claude Code CLI
─────────────────────────────────────────────────────────
改檔案            複製貼上               直接改你的檔案
讀專案            要手動貼 code          自動讀整個專案
執行指令          不行                   可以跑 dotnet build
Git 操作          不行                   可以 commit、push
瀏覽器操作        不行                   可以（透過 MCP）
記憶              每次重來               CLAUDE.md 永久記憶
```

### 安裝 Claude Code

```bash
# 安裝 Claude Code（需要先有 Node.js）
npm install -g @anthropic-ai/claude-code

# 確認安裝成功
claude --version

# 在專案目錄啟動
cd C:/Users/user/MyProject
claude
```

---

## 基本操作：開啟、對話、讓 AI 改 Code

### 開啟 Claude Code

```bash
# 進入你的專案目錄
cd C:/Users/user/DotNetLearning

# 啟動 Claude Code
claude
```

啟動後你會看到一個互動式的對話介面，直接打字就可以跟 AI 對話。

### 常用操作範例

```bash
# 讓 AI 讀懂你的專案
> 幫我看一下這個專案的架構，列出所有 Controller 和 View

# 讓 AI 改 code
> 把 HomeController.cs 的 Index action 改成回傳 ""Hello World""

# 讓 AI 建新檔案
> 幫我建一個 ProductController，有 Index 和 Details 兩個 action

# 讓 AI 跑指令
> 執行 dotnet build 看有沒有編譯錯誤

# 讓 AI 修 bug
> dotnet run 出現 NullReferenceException，幫我找原因
```

### AI 會做的事

當你給 AI 指令，它會：
1. **讀取**相關檔案（自動找到需要的檔案）
2. **分析**程式碼（理解邏輯和架構）
3. **修改**檔案（直接寫入你的檔案系統）
4. **執行**指令（如果需要的話）
5. **回報**結果（告訴你改了什麼）

---

## 如何描述需求讓 AI 理解

> 💡 **比喻：點餐**
> - ❌ 模糊：「給我吃的」→ AI 不知道你要什麼
> - ✅ 具體：「一碗牛肉麵，不要香菜，小辣，加滷蛋」→ AI 精準執行

### 具體 > 模糊

```
❌ 模糊的需求：
""幫我做一個網站""
→ AI：什麼網站？什麼功能？什麼技術？什麼風格？😵

✅ 具體的需求：
""用 ASP.NET Core MVC 做一個吉他教學網站，
  功能：首頁、課程列表、聯絡表單，
  風格：暖米色系，響應式設計，
  資料庫：SQLite""
→ AI：收到！馬上開始 💪
```

### 好需求的四個要素

```
1. 技術棧：用什麼框架、什麼語言
2. 功能：要做什麼、有哪些頁面
3. 風格：長什麼樣子、什麼色系
4. 限制：不要什麼、要注意什麼
```

### 範例對比

```
❌ ""幫我建一個 ASP.NET Core MVC 專案""
→ AI 會建一個空的預設專案，什麼都沒有

✅ ""幫我建一個 ASP.NET Core MVC 專案，要有：
   - HomeController 有 Index 和 About action
   - 用 Bootstrap 5 的 Navbar
   - SQLite 資料庫，Entity Framework Core
   - 一個 Product model（Id, Name, Price, Description）
   - ProductController 有 CRUD 功能""
→ AI 會建一個完整可用的專案
```

---

## AI 不是萬能的

### 適合交給 AI 的事

```
✅ 重複性工作（建 CRUD、寫 Model）
✅ 套版面（CSS、HTML 排版）
✅ 寫測試（Unit Test、Integration Test）
✅ 重構（改變數名、拆函式）
✅ 查錯（讀 error log、找 bug）
✅ 學習（解釋程式碼、教概念）
```

### 不適合交給 AI 的事

```
❌ 商業邏輯決策（這個功能該不該做？）
❌ 系統架構設計（要用 Microservice 還是 Monolith？）
❌ 安全性審核（AI 可能忽略漏洞）
❌ 效能調校（需要實際測量和經驗）
❌ 程式碼審查的最終判斷（你才是負責人）
```

> ⚠️ **重要原則：AI 寫的每一行 code，你都要看得懂、負得起責任。**

---

## 範例對比：好指令 vs 壞指令

### 範例 1：建專案

```
❌ 壞指令：
""建一個網站""

✅ 好指令：
""用 ASP.NET Core 8.0 MVC 建一個吉他教學網站 GuitarSchool，
  包含以下功能：
  1. 首頁（Hero banner + 課程精選）
  2. 課程列表頁（可篩選難度）
  3. 聯絡表單（Name, Email, Message）
  4. 用 SQLite + EF Core
  5. Bootstrap 5，暖米色系（#F5F1EB 背景）
  6. 響應式設計，手機要能看""
```

### 範例 2：修 Bug

```
❌ 壞指令：
""幫我修 bug""

✅ 好指令：
""HomeController.cs 第 42 行出現 NullReferenceException，
  我在 Index action 裡呼叫 _context.Products.ToList()，
  但 _context 是 null。
  請檢查 DI 註冊（Program.cs）有沒有加 AddDbContext。""
```

### 範例 3：改樣式

```
❌ 壞指令：
""把網站弄好看一點""

✅ 好指令：
""把 Navbar 的背景色改成 #2C3E50（深藍灰），
  文字顏色 #ECF0F1（淺灰白），
  hover 時文字變成 #E74C3C（紅色），
  加上 0.3s 的 transition 動畫""
```

---

## 🤔 常見錯誤

### 錯誤 1：指令太模糊

```
❌ 你說：""幫我做一個功能""
AI 回覆：""什麼功能？要用在哪裡？有什麼需求？""
→ 浪費時間在來回問答

✅ 改成：""在 ProductController 加一個 Search action，
  接收 query 參數，搜尋 Product 的 Name 欄位，
  回傳符合的產品列表到 Search.cshtml View""
```

### 錯誤 2：不檢查 AI 的輸出

```
❌ AI 寫完 code 後直接 deploy
→ 結果 AI 用了已棄用的 API、忘記處理 null、SQL Injection 漏洞

✅ 正確做法：
1. 讀一遍 AI 寫的 code
2. 跑 dotnet build 確認編譯通過
3. 跑測試確認功能正常
4. 自己手動測一次
```

### 錯誤 3：過度依賴 AI

```
❌ 完全不學程式，所有東西都叫 AI 做
→ 出問題時完全不知道怎麼修

✅ 正確做法：
- 用 AI 加速學習，但自己也要理解
- 看 AI 的 code 時順便學習寫法
- 遇到看不懂的，問 AI：""這段 code 在做什麼？為什麼這樣寫？""
```

---

## 本章重點整理

| 概念 | 說明 |
|------|------|
| AI 輔助開發 | 你想、AI 做，加速開發流程 |
| Claude Code | CLI 工具，直接在終端機跟 AI 對話 |
| 具體需求 | 技術棧 + 功能 + 風格 + 限制 |
| AI 的限制 | 不做決策、不保證安全、需要人類審查 |
| 黃金原則 | 每行 code 你都要看得懂、負得起責任 |
" },

        // ── AI Application Chapter 401 ────────────────────────────
        new() { Id=401, Category="ai", Order=2, Level="intermediate", Icon="📝", Title="Prompt Engineering 提示工程", Slug="prompt-engineering", IsPublished=true, Content=@"
# Prompt Engineering 提示工程

## 什麼是 Prompt？

> 💡 **比喻：跟外國人溝通**
> Prompt 就像跟一個聽得懂中文但「不會猜你心思」的外國人說話。
> 你說得越清楚，他做得越好。
> 你說「那個東西放那邊」，他會傻住。
> 你說「把紅色的杯子放到桌上左邊第二格」，他馬上做到。

**Prompt = 你給 AI 的指令。指令的品質決定 AI 輸出的品質。**

這就是為什麼 Prompt Engineering（提示工程）是 AI 時代最重要的技能之一。

---

## 好 Prompt vs 壞 Prompt 對比

### 範例 1：修 Bug

```
❌ 壞 Prompt：
""幫我修 bug""

問題：什麼 bug？在哪個檔案？什麼錯誤訊息？
AI 只能猜，猜錯就浪費時間。
```

```
✅ 好 Prompt：
""HomeController.cs 第 42 行出現 NullReferenceException，
  Model 是 null。
  請檢查 Index action 有沒有回傳正確的 ViewModel。
  我的 Model 是 ProductViewModel，在 Models/ProductViewModel.cs。
  資料庫用 ApplicationDbContext。""

結果：AI 馬上定位問題，幾秒鐘就修好。
```

### 範例 2：建網站

```
❌ 壞 Prompt：
""做一個網站""

問題：什麼網站？給誰用？長什麼樣？
這就像跟裝潢師傅說「幫我裝潢」但不說要什麼風格。
```

```
✅ 好 Prompt：
""用 ASP.NET Core MVC 做一個吉他教學網站，
  功能：首頁（Hero banner）、課程列表（可按難度篩選）、聯絡表單
  技術：SQLite + EF Core，Bootstrap 5
  風格：暖米色系（背景 #F5F1EB），圓角按鈕
  部署：Railway，要有 Dockerfile""

結果：AI 一次到位，產出完整可用的專案。
```

### 範例 3：寫 Code

```
❌ 壞 Prompt：
""寫一個 API""

✅ 好 Prompt：
""在 Controllers/ 資料夾建一個 ApiProductController.cs，
  繼承 ControllerBase，加上 [ApiController] 和 [Route(""api/products"")]。
  用 ApplicationDbContext 注入。
  實作以下 endpoints：
  - GET /api/products → 回傳所有產品（JSON）
  - GET /api/products/{id} → 回傳單一產品
  - POST /api/products → 新增產品（接收 ProductDto）
  - PUT /api/products/{id} → 更新產品
  - DELETE /api/products/{id} → 刪除產品
  每個 action 都要有適當的 HTTP status code 回傳。""
```

---

## Prompt 公式

一個好的 Prompt 通常包含五個元素：

```
角色 + 背景 + 任務 + 格式 + 限制

角色：你是一個資深 ASP.NET Core 開發者
背景：我正在做一個學習平台，用 .NET 8 + SQLite + EF Core
任務：幫我建一個 ChapterController 有完整的 CRUD 功能
格式：每個 action 加上中文註解，用 async/await
限制：不要用 Repository Pattern，直接用 DbContext
```

### 實際範例

```
你是一個資深 ASP.NET Core 開發者。
我正在做一個吉他教學網站 GuitarSchool，
技術棧是 .NET 8 + SQLite + EF Core + Bootstrap 5。

請幫我做以下事情：
1. 建立 Course model（Id, Title, Description, Level, Price, ImageUrl）
2. 在 ApplicationDbContext 加上 DbSet<Course>
3. 建立 CourseController 有 Index 和 Details action
4. 建立對應的 Views（用 Bootstrap 卡片排版）

注意事項：
- 所有 action 用 async/await
- View 裡的文字用繁體中文
- 不需要建 Repository，直接用 DbContext
```

---

## Chain of Thought（思維鏈）

> 💡 **比喻：考試寫計算題**
> 老師說「寫出計算過程」，你就不會跳步驟算錯。
> Chain of Thought 就是告訴 AI「一步一步想」，
> 讓它不要跳躍邏輯直接給答案。

### 怎麼用

```
普通 Prompt：
""這段 code 有什麼問題？""
→ AI 可能直接說「沒問題」或給一個模糊的答案

Chain of Thought Prompt：
""請一步一步分析這段 code：
  1. 先看每個變數的型別
  2. 追蹤資料流（從 input 到 output）
  3. 檢查每個可能的 null 情況
  4. 檢查 Exception 處理
  5. 最後告訴我有什麼問題""
→ AI 會系統性地分析，找到更深層的問題
```

### 在 Claude Code 中使用

```
> 我想在 ProductController 加上搜尋功能。
> 請先想一下：
> 1. 需要改哪些檔案？
> 2. 需要什麼 NuGet 套件？
> 3. SQL 查詢怎麼寫最有效率？
> 4. 前端搜尋欄怎麼設計？
> 想好之後再開始寫 code。
```

在 Claude Code 中，你還可以用 **Plan Mode（規劃模式）**：

```
> 用 plan mode 先幫我規劃怎麼實作購物車功能，
> 不要直接寫 code，先列出步驟。
```

AI 會進入規劃模式，先列出完整的實作計畫，你確認後再執行。

---

## Few-shot（給範例讓 AI 學）

> 💡 **比喻：教新員工**
> 與其跟新員工說「幫我寫報告」，
> 不如給他一份範本：「照這個格式寫」。
> Few-shot 就是給 AI 範例，讓它學會你要的格式。

### 怎麼用

```
請幫我寫 Model 的中文註解，格式如下：

範例：
/// <summary>
/// 產品編號（主鍵，自動遞增）
/// </summary>
public int Id { get; set; }

/// <summary>
/// 產品名稱（必填，最大長度 100 字）
/// </summary>
[Required, MaxLength(100)]
public string Name { get; set; } = """";

現在請用同樣的格式，幫 Course model 的每個屬性加上中文註解。
Course 的屬性有：Id, Title, Description, Level, Price, ImageUrl
```

### 另一個範例：CSS 風格統一

```
我的 CSS 命名風格如下：

範例：
.card-guitar { ... }         /* 元件-用途 */
.btn-primary-warm { ... }    /* 元件-狀態-風格 */
.section-hero-home { ... }   /* 區塊-類型-頁面 */

請用同樣的命名風格，幫我寫課程列表頁的 CSS。
需要：課程卡片、篩選按鈕、分頁按鈕。
```

---

## 中文 vs 英文 Prompt 差異

```
語言      優點                        缺點
────────────────────────────────────────────────────────
中文      表達需求更自然               技術術語可能翻譯不精確
          溝通更快速                   某些 AI 的中文理解稍弱
          適合描述業務邏輯

英文      技術術語更精確               需要英文能力
          AI 訓練資料更多              描述業務邏輯較不自然
          錯誤訊息通常是英文
```

### 實用建議

```
最佳實踐：中英混用

""幫我在 HomeController 的 Index action 加上 [Authorize] attribute，
  只允許 Role 是 'Admin' 的使用者存取。
  如果未授權，redirect 到 /Account/Login。""

技術名詞用英文（Controller, Action, Attribute, Authorize）
描述邏輯用中文（加上、只允許、如果未授權）
```

---

## 實際範例：用一個 Prompt 完成整個功能

```
你是一個資深 ASP.NET Core 開發者。
我的專案是 DotNetLearning，.NET 8 + SQLite + EF Core。

請幫我實作「我的最愛」功能：

Model：
- Favorite（Id, UserId, ChapterId, CreatedAt）
- 跟 Chapter 是多對一關係

Controller：
- FavoriteController
- POST /Favorite/Toggle/{chapterId} → 切換最愛狀態
- GET /Favorite/MyList → 顯示我的最愛清單

View：
- 每個章節卡片上加一個愛心按鈕 ♥
- 點了變紅色，再點取消
- 用 AJAX 不刷新頁面

注意：
- 要先檢查使用者是否登入（[Authorize]）
- 用 async/await
- 加上適當的中文註解
- 不用 Repository Pattern，直接用 DbContext
```

這個 Prompt 包含了：角色、背景、任務（Model + Controller + View）、格式（async、中文註解）、限制（不用 Repository）。

---

## 🤔 常見錯誤

### 錯誤 1：一次給太多任務

```
❌ 一個 Prompt 要求：
""幫我做會員系統、購物車、結帳流程、
  金流串接、Email 通知、管理後台""
→ AI 會混亂、遺漏、品質下降

✅ 拆成多個 Prompt：
第一輪：""先幫我做會員系統（註冊、登入、個人資料）""
第二輪：""現在加購物車功能（加入、移除、數量修改）""
第三輪：""接下來做結帳流程（訂單建立、庫存扣除）""
→ 每一輪都能高品質完成
```

### 錯誤 2：不給上下文

```
❌ ""幫我修這個 bug""
→ AI：什麼 bug？在哪裡？什麼錯誤訊息？

✅ ""ProductController.cs 的 Details action，
  當 id 不存在時會回傳 500 而不是 404。
  請加上 null check，如果找不到就回傳 NotFound()。
  我的 DbContext 是 ApplicationDbContext。""
```

### 錯誤 3：不驗證結果

```
❌ AI 說「改好了」就直接部署
→ 可能：編譯錯誤、Runtime 錯誤、邏輯錯誤、安全漏洞

✅ 驗證清單：
□ dotnet build 編譯通過？
□ dotnet run 啟動正常？
□ 手動測試功能正確？
□ Edge case 有處理嗎？（null、空字串、負數）
□ 有安全疑慮嗎？（SQL Injection、XSS）
```

---

## 本章重點整理

| 技巧 | 說明 |
|------|------|
| Prompt 公式 | 角色 + 背景 + 任務 + 格式 + 限制 |
| Chain of Thought | 「一步一步想」讓 AI 更精確 |
| Few-shot | 給範例讓 AI 學會你的風格 |
| 中英混用 | 技術名詞英文，業務邏輯中文 |
| 拆解任務 | 一次一個功能，品質更好 |
| 驗證結果 | build → run → test → review |
" },

        // ── AI Application Chapter 402 ────────────────────────────
        new() { Id=402, Category="ai", Order=3, Level="intermediate", Icon="🧠", Title="CLAUDE.md 與 AI 記憶系統", Slug="claude-md-memory-system", IsPublished=true, Content=@"
# CLAUDE.md 與 AI 記憶系統

## 為什麼 AI 每次對話都會「失憶」？

> 💡 **比喻：金魚的記憶**
> 每次你開一個新的 AI 對話，就像跟一條金魚說話——
> 它完全不記得上次你們聊了什麼。
> 你上次花了 30 分鐘解釋專案架構，這次又要重新來一遍。
>
> CLAUDE.md 就是給金魚裝了一個小抄本，
> 每次對話開始前，它會先讀小抄，瞬間回憶起所有重要的事。

### AI 的記憶問題

```
對話 1：""我的專案用 .NET 8 + SQLite + EF Core""
對話 2：""我的專案是什麼技術棧？"" → AI：""我不知道""
對話 3：""幫我加一個功能"" → AI：""你的專案是什麼？用什麼框架？""

每次都要重新解釋 = 浪費時間 + 浪費 Token
```

解決方案就是 **記憶系統**。Claude Code 有三層記憶架構，讓 AI 越用越聰明。

---

## 第一層：CLAUDE.md（專案級永久記憶）

CLAUDE.md 是放在**專案根目錄**的檔案，每次開啟 Claude Code 時會**自動載入**。

### 為什麼 CLAUDE.md 這麼重要？

```
沒有 CLAUDE.md：
每次對話都要重新解釋 → 浪費 Token、浪費時間

有 CLAUDE.md：
自動載入 → AI 馬上知道你的專案 → 直接開始工作
而且 CLAUDE.md 有快取機制，幾乎不花 Token！
```

### CLAUDE.md 放什麼？

適合放**穩定不常變動**的資訊：

```markdown
# GuitarSchool 專案

## 技術棧
- .NET 8 + ASP.NET Core MVC
- SQLite + Entity Framework Core
- Bootstrap 5 + 自訂 CSS
- 部署：Railway（Docker）

## 專案架構
```
GuitarSchool/
├── Controllers/     # MVC Controller
├── Models/          # 資料模型
├── Views/           # Razor View
├── Data/            # DbContext + Seed Data
├── wwwroot/css/     # 樣式表
└── Program.cs       # 應用程式進入點
```

## CSS 變數（統一色系）
```css
:root {
  --bg-warm: #F5F1EB;      /* 暖米色背景 */
  --text-dark: #2C3E50;     /* 深色文字 */
  --accent: #E67E22;        /* 橘色強調 */
  --accent-hover: #D35400;  /* 橘色 hover */
}
```

## 部署指令
```bash
docker build -t guitarschool .
railway up
```

## 注意事項
- View 裡的文字全部用繁體中文
- 不用 Repository Pattern，直接用 DbContext
- CSS 用 BEM-like 命名（.card-guitar, .btn-primary-warm）
- 所有 action 用 async/await
```

### CLAUDE.md 的快取機制

```
第一次對話：載入 CLAUDE.md → 花少量 Token 讀取
第二次對話：CLAUDE.md 沒變 → 從快取讀取 → 幾乎 0 Token！
修改後：  偵測到變動 → 重新載入 → 更新快取

這就是為什麼 CLAUDE.md 是最省 Token 的記憶方式。
```

### CLAUDE.md 的位置

```
專案根目錄的 CLAUDE.md → 這個專案的所有對話都會載入
~/.claude/CLAUDE.md → 全域設定，所有專案都會載入

建議：專案特定的放專案根目錄，通用偏好放全域
```

---

## 第二層：Auto Memory（動態自動記憶）

> 💡 **比喻：秘書的筆記本**
> 如果 CLAUDE.md 是「公司手冊」（正式、穩定），
> Auto Memory 就是「秘書的筆記本」（隨時記錄、動態更新）。
> AI 會自動判斷哪些資訊值得記住，存到筆記本裡。

### Auto Memory 怎麼運作？

```
位置：~/.claude/projects/<你的專案路徑>/memory/

結構：
memory/
├── MEMORY.md           # 索引檔（列出所有記憶）
├── preference-001.md   # 記憶：用戶喜歡用 tab 縮排
├── workflow-002.md     # 記憶：部署流程是 build → test → push
└── context-003.md      # 記憶：正在做購物車功能
```

### 什麼會被自動記住？

```
AI 會自動記住：
- 你的偏好（""我喜歡用 4 空格縮排""）
- 重複的指示（""不要用 var，用明確型別""）
- 進行中的工作（""我們正在做第三章的內容""）
- 常見的操作（""每次改完 code 都要跑 dotnet build""）

AI 不會記住：
- 一次性的對話內容
- 太瑣碎的細節
- 跟專案無關的閒聊
```

### Auto Memory vs CLAUDE.md 的差別

```
特性          CLAUDE.md              Auto Memory
──────────────────────────────────────────────────
維護方式      你手動寫               AI 自動存
載入時機      每次對話自動載入        按需讀取
Token 消耗    有快取，幾乎 0         讀取時花 Token
適合內容      穩定的專案資訊          動態的偏好和進度
位置          專案根目錄             ~/.claude/projects/.../memory/
```

---

## 第三層：Memory MCP（跨專案知識圖譜）

> 💡 **比喻：公司的知識庫**
> CLAUDE.md 是「部門手冊」（只給這個專案用），
> Auto Memory 是「個人筆記」（動態記錄），
> Memory MCP 就是「公司知識庫」（所有專案都能查、跨專案共享）。

### Memory MCP 怎麼運作？

Memory MCP 是一個 **知識圖譜（Knowledge Graph）**，用「實體」和「關係」來儲存知識。

```
實體（Entity）：
- 名稱：""GuitarSchool""
- 類型：""Project""
- 觀察：[""用 .NET 8"", ""部署在 Railway"", ""暖米色系""]

關係（Relation）：
- GuitarSchool → uses → ""SQLite""
- GuitarSchool → deployed_on → ""Railway""
- User → prefers → ""繁體中文""
```

### 設定 Memory MCP

在 `~/.claude/settings.local.json` 加上：

```json
{
  ""mcpServers"": {
    ""memory"": {
      ""command"": ""cmd"",
      ""args"": [""/c"", ""npx"", ""-y"", ""@anthropic-ai/memory-mcp""]
    }
  }
}
```

### Memory MCP 的用法

```
> 幫我記住：我所有的專案都用 4 空格縮排，不用 tab

AI 會自動呼叫 Memory MCP 存入：
Entity: ""UserPreference_Indentation""
Type: ""CodingPreference""
Observations: [""使用 4 空格縮排"", ""不使用 tab""]

以後在任何專案，AI 都會記得這個偏好。
```

---

## 三層記憶架構設計

```
┌─────────────────────────────────────────────────┐
│           CLAUDE.md（第一層）                      │
│     快取 ≈ 0 Token → 穩定資訊                     │
│     專案架構、部署指令、CSS 變數、技術棧             │
├─────────────────────────────────────────────────┤
│           Auto Memory（第二層）                    │
│     按需讀取 → 動態偏好                            │
│     用戶偏好、進行中的工作、常用操作                  │
├─────────────────────────────────────────────────┤
│           Memory MCP（第三層）                     │
│     查詢用 → 跨專案知識                            │
│     通用偏好、跨專案經驗、歷史決策                   │
└─────────────────────────────────────────────────┘
```

### 最省 Token 的設計原則

```
1. 穩定資訊放 CLAUDE.md（快取 ≈ 0 Token）
   → 技術棧、專案架構、部署指令

2. 動態偏好靠 Auto Memory（按需讀取）
   → AI 自動管理，不用手動維護

3. 跨專案知識用 Memory MCP（查詢才花 Token）
   → 通用偏好、經驗累積
```

---

## 實際範例：設計一個完整的 CLAUDE.md

以下是一個真實專案的 CLAUDE.md 範例：

```markdown
# DotNetLearning 專案

## 專案概述
一個 .NET 學習平台，提供互動式教學內容和測驗。

## 技術棧
- .NET 8 + ASP.NET Core MVC
- SQLite + Entity Framework Core 8
- Bootstrap 5.3 + 自訂 CSS
- Markdig（Markdown 渲染）

## 架構
- Controllers/：MVC Controller（Home, Chapter, Question）
- Models/：Chapter, Question, UserProgress
- Views/：Razor View（_Layout 用 Bootstrap）
- Data/：ApplicationDbContext + SeedChapters_*.cs

## 開發指令
```bash
dotnet build                    # 編譯
dotnet run                      # 啟動（https://localhost:5001）
dotnet ef database update       # 更新資料庫
dotnet ef migrations add XXX    # 新增 migration
```

## 程式風格
- async/await 所有資料庫操作
- 不用 Repository Pattern
- View 文字用繁體中文
- 中文註解在每行 code

## 部署
- 平台：Railway
- Dockerfile 在根目錄
- 環境變數：ASPNETCORE_ENVIRONMENT=Production
```

### 設計重點

```
✅ 簡潔：只放最重要的資訊
✅ 結構化：用 Markdown 標題分類
✅ 可執行：部署指令可以直接複製使用
✅ 不超過 200 行：太長會被截斷

❌ 不要放：程式碼片段（太長）
❌ 不要放：對話歷史（不穩定）
❌ 不要放：暫時性的 TODO（用 Auto Memory）
```

---

## 🤔 常見錯誤

### 錯誤 1：把所有東西塞進 CLAUDE.md

```
❌ CLAUDE.md 塞了 500 行
→ 超過 200 行可能被截斷
→ 太多雜訊反而讓 AI 找不到重點

✅ 只放最穩定、最重要的資訊
→ 技術棧（10 行）
→ 架構（15 行）
→ 指令（10 行）
→ 風格（10 行）
→ 總共不超過 100 行最佳
```

### 錯誤 2：記憶存重複資訊

```
❌ CLAUDE.md 寫了「用 .NET 8」
   Auto Memory 也存了「用 .NET 8」
   Memory MCP 又存了「用 .NET 8」
→ 三個地方存同樣的東西，浪費又混亂

✅ 分層存放：
   CLAUDE.md → 專案用 .NET 8（穩定）
   Auto Memory → 最近在做 Chapter 5（動態）
   Memory MCP → 所有專案都用 4 空格縮排（跨專案）
```

### 錯誤 3：不維護 CLAUDE.md

```
❌ 專案已經從 .NET 7 升級到 .NET 8
   但 CLAUDE.md 還是寫 .NET 7
→ AI 會用錯誤的資訊工作

✅ 定期更新 CLAUDE.md
→ 技術棧升級時更新
→ 架構變動時更新
→ 部署方式改變時更新
```

---

## 本章重點整理

| 記憶層級 | 位置 | Token 消耗 | 適合內容 |
|----------|------|-----------|----------|
| CLAUDE.md | 專案根目錄 | 快取 ≈ 0 | 專案架構、技術棧 |
| Auto Memory | ~/.claude/projects/ | 按需讀取 | 偏好、進度 |
| Memory MCP | 知識圖譜 | 查詢時 | 跨專案知識 |
" },

        // ── AI Application Chapter 403 ────────────────────────────
        new() { Id=403, Category="ai", Order=4, Level="intermediate", Icon="🔧", Title="MCP 與 Skills 擴充系統", Slug="mcp-skills-extension", IsPublished=true, Content=@"
# MCP 與 Skills 擴充系統

## MCP 是什麼？

> 💡 **比喻：手機的 App Store**
> AI 本身就像一支剛買的新手機——功能有限。
> MCP（Model Context Protocol）就像 App Store，
> 讓你安裝各種「插件」，讓 AI 能做更多事。
>
> 沒裝 App 的手機：只能打電話、傳簡訊
> 裝了 App 的手機：能叫車、點餐、付款、拍照修圖
>
> 沒有 MCP 的 AI：只能讀文字、寫文字
> 有 MCP 的 AI：能開瀏覽器、讀檔案、查資料庫、控制應用程式

### 沒有 MCP vs 有 MCP

```
沒有 MCP：
你：""幫我看桌面上有什麼檔案""
AI：""抱歉，我無法存取你的檔案系統""

有 MCP（filesystem）：
你：""幫我看桌面上有什麼檔案""
AI：""你的桌面有以下檔案：
     - project.docx
     - notes.txt
     - photo.jpg""
```

```
沒有 MCP：
你：""幫我打開 Google 搜尋 ASP.NET Core 教學""
AI：""抱歉，我無法操作瀏覽器""

有 MCP（Chrome）：
你：""幫我打開 Google 搜尋 ASP.NET Core 教學""
AI：[打開瀏覽器] → [輸入搜尋] → [回傳結果]
```

---

## 常用 MCP 介紹

### 1. Filesystem MCP（檔案系統）

```
功能：讀寫本機檔案和資料夾
用途：讓 AI 能操作你電腦上的檔案

可以做的事：
- 讀取檔案內容
- 建立新檔案
- 修改現有檔案
- 列出資料夾內容
- 搜尋檔案
```

### 2. Memory MCP（跨對話記憶）

```
功能：建立跨 session 的知識圖譜
用途：讓 AI 記住跨對話的資訊

可以做的事：
- 儲存實體和關係
- 搜尋已儲存的知識
- 跨專案共享記憶
```

### 3. Chrome MCP（瀏覽器自動化）

```
功能：控制 Chrome 瀏覽器
用途：讓 AI 能瀏覽網頁、截圖、填表單

可以做的事：
- 開啟網頁
- 截取螢幕截圖
- 點擊按鈕和連結
- 填寫表單
- 讀取頁面內容
```

### 4. GitHub MCP（代碼管理）

```
功能：操作 GitHub Repository
用途：讓 AI 能管理你的 GitHub

可以做的事：
- 建立 Pull Request
- 查看 Issues
- 管理 Branch
- Code Review
```

---

## 如何安裝 MCP

MCP 的設定檔是 `.mcp.json`，可以放在**專案根目錄**或**使用者設定**。

### 專案級設定（.mcp.json）

放在專案根目錄，只對這個專案生效：

```json
{
  ""mcpServers"": {
    ""filesystem"": {
      ""command"": ""cmd"",
      ""args"": [""/c"", ""npx"", ""-y"", ""@modelcontextprotocol/server-filesystem"", ""C:/Users/user/Desktop""]
    }
  }
}
```

### 全域設定（~/.claude/settings.local.json）

放在使用者設定，所有專案都能用：

```json
{
  ""mcpServers"": {
    ""memory"": {
      ""command"": ""cmd"",
      ""args"": [""/c"", ""npx"", ""-y"", ""@anthropic-ai/memory-mcp""]
    },
    ""filesystem"": {
      ""command"": ""cmd"",
      ""args"": [""/c"", ""npx"", ""-y"", ""@modelcontextprotocol/server-filesystem"", ""C:/Users/user/projects""]
    }
  }
}
```

### Windows 特別注意

```
⚠️ Windows 上一定要用 ""cmd"" + ""/c"" 來包裝 npx！

❌ 錯誤（Linux/Mac 的寫法）：
""command"": ""npx""
→ Windows 上會報錯：spawn npx ENOENT

✅ 正確（Windows 的寫法）：
""command"": ""cmd""
""args"": [""/c"", ""npx"", ""-y"", ""...""]
→ 用 cmd /c 來執行 npx
```

### 安裝完成後

```bash
# 重啟 Claude Code 讓設定生效
# 在專案目錄重新啟動
claude

# 啟動時會看到 MCP 載入訊息
# 如果成功，會顯示可用的 MCP 工具列表
```

---

## Skills 是什麼？

> 💡 **比喻：巨集（Macro）**
> 如果 MCP 是「讓 AI 有手腳」，
> Skills 就是「預寫好的動作指令」。
> 就像 Excel 的巨集——錄好一次，以後一鍵執行。

### Skills 的概念

Skills 是預寫好的 Prompt 指令，存在 `.claude/skills/` 資料夾中。

```
.claude/
└── skills/
    ├── deploy/
    │   └── SKILL.md     # 部署技能
    ├── setup-project/
    │   └── SKILL.md     # 專案初始化技能
    └── fix-css/
        └── SKILL.md     # CSS 修復技能
```

### SKILL.md 格式範例

```markdown
---
description: 自動部署到 Railway
command: deploy
---

# 部署流程

請執行以下步驟：

1. 執行 `dotnet build -c Release` 確認編譯成功
2. 執行 `dotnet test` 確認測試通過
3. 建立 git commit（訊息：""deploy: auto deploy via skill""）
4. 執行 `git push origin main`
5. 確認 Railway 自動部署啟動

如果任何步驟失敗，停下來回報錯誤，不要繼續。
```

### 使用 Skill

```bash
# 在 Claude Code 中輸入 slash command
> /deploy

# AI 會自動載入 SKILL.md 的內容並執行
# 就像按了一個「一鍵部署」的按鈕
```

### 更多 Skill 範例

```markdown
---
description: 新增一個 CRUD Controller
command: new-crud
---

# 建立 CRUD Controller

請依照以下步驟：

1. 詢問使用者 Model 名稱
2. 在 Models/ 建立 Model 檔案（Id + 使用者指定的欄位）
3. 在 ApplicationDbContext 加上 DbSet
4. 建立 Controller（所有 CRUD action，async/await）
5. 建立 Views/（Index, Details, Create, Edit, Delete）
6. 建立 Migration 並更新資料庫
7. 執行 dotnet build 確認編譯成功
```

---

## Hooks：事件觸發自動執行

> 💡 **比喻：自動門**
> 你不需要手動推門，走到門前它就自動開了。
> Hooks 就是「自動門」——當某個事件發生時，自動執行指定的動作。

### 什麼是 Hooks？

Hooks 是在特定事件發生時**自動執行**的腳本。

```
可用的 Hook 事件：
- Stop：每次 AI 回應結束後觸發
- PreToolUse：AI 使用工具前觸發
- PostToolUse：AI 使用工具後觸發
- Notification：通知事件觸發
```

### 設定 Hooks

在 `settings.local.json` 中設定：

```json
{
  ""hooks"": {
    ""Stop"": [
      {
        ""matcher"": """",
        ""hooks"": [
          {
            ""type"": ""command"",
            ""command"": ""cd /path/to/project && dotnet build""
          }
        ]
      }
    ]
  }
}
```

### 實用 Hook 範例：對話結束後自動 Build

```json
{
  ""hooks"": {
    ""Stop"": [
      {
        ""matcher"": """",
        ""hooks"": [
          {
            ""type"": ""command"",
            ""command"": ""cd C:/Users/user/GuitarSchool && dotnet build -c Release""
          }
        ]
      }
    ]
  }
}
```

這樣每次 AI 回應結束後，都會自動跑 `dotnet build`，確保程式碼沒有編譯錯誤。

---

## 完整設定流程：MCP + Skill + Hook

### Step 1：建立 MCP 設定

在專案根目錄建立 `.mcp.json`：

```json
{
  ""mcpServers"": {
    ""filesystem"": {
      ""command"": ""cmd"",
      ""args"": [""/c"", ""npx"", ""-y"", ""@modelcontextprotocol/server-filesystem"", ""C:/Users/user/GuitarSchool""]
    }
  }
}
```

### Step 2：建立 Skill

建立 `.claude/skills/deploy/SKILL.md`：

```markdown
---
description: 一鍵部署到 Railway
command: deploy
---

# 部署流程

1. 執行 dotnet build -c Release
2. 執行 dotnet test（如果有測試的話）
3. git add -A && git commit -m ""deploy: auto deploy""
4. git push origin main
5. 回報部署狀態
```

### Step 3：設定 Hook

在 `~/.claude/settings.local.json` 加上：

```json
{
  ""hooks"": {
    ""Stop"": [
      {
        ""matcher"": """",
        ""hooks"": [
          {
            ""type"": ""command"",
            ""command"": ""echo 'AI 回應結束'""
          }
        ]
      }
    ]
  }
}
```

### Step 4：重啟 Claude Code

```bash
# 關閉目前的 Claude Code
# 重新啟動
claude
```

---

## bypassPermissions：免問許可

Claude Code 預設會在執行某些操作前問你「可以嗎？」，
如果你信任 AI 的操作，可以設定 `bypassPermissions` 跳過確認。

### 設定方式

在 `~/.claude/settings.local.json`：

```json
{
  ""permissions"": {
    ""allow"": [
      ""Bash(dotnet build*)"",
      ""Bash(dotnet run*)"",
      ""Bash(dotnet test*)"",
      ""Bash(git *)"",
      ""Read(*)"",
      ""Write(*.cs)"",
      ""Write(*.cshtml)""
    ]
  }
}
```

```
allow 清單說明：
- ""Bash(dotnet build*)"" → 允許執行 dotnet build 相關指令
- ""Read(*)"" → 允許讀取任何檔案
- ""Write(*.cs)"" → 允許寫入 .cs 檔案
- ""Bash(git *)"" → 允許執行 git 指令

⚠️ 注意：不要加 ""Bash(rm *)"" 或 ""Bash(del *)""！
    讓 AI 有刪除權限很危險。
```

---

## 🤔 常見錯誤

### 錯誤 1：Windows 上忘記用 cmd /c

```json
❌ 錯誤：
{
  ""mcpServers"": {
    ""filesystem"": {
      ""command"": ""npx"",
      ""args"": [""-y"", ""@modelcontextprotocol/server-filesystem""]
    }
  }
}
→ 錯誤訊息：spawn npx ENOENT

✅ 正確：
{
  ""mcpServers"": {
    ""filesystem"": {
      ""command"": ""cmd"",
      ""args"": [""/c"", ""npx"", ""-y"", ""@modelcontextprotocol/server-filesystem""]
    }
  }
}
```

### 錯誤 2：.mcp.json 放錯位置

```
❌ 放在 src/ 子目錄
❌ 放在 home 目錄（那是給 settings.local.json 的）
❌ 檔名打錯（mcp.json 少了前面的點）

✅ 專案級：放在專案根目錄 → /path/to/project/.mcp.json
✅ 全域級：放在 ~/.claude/settings.local.json 的 mcpServers 裡
```

### 錯誤 3：沒有重啟讓設定生效

```
❌ 改了 .mcp.json 但沒有重啟 Claude Code
→ 新設定不會載入，MCP 不會生效

✅ 改完設定後：
1. 關閉 Claude Code（Ctrl+C 或輸入 /exit）
2. 重新啟動 Claude Code
3. 確認 MCP 工具列表有顯示新的 MCP
```

---

## 本章重點整理

| 概念 | 說明 |
|------|------|
| MCP | 讓 AI 有手腳的插件系統 |
| .mcp.json | MCP 設定檔，放在專案根目錄 |
| Skills | 預寫好的 Prompt 指令（巨集） |
| Hooks | 事件觸發自動執行的腳本 |
| bypassPermissions | 跳過 AI 操作確認 |
| cmd /c | Windows 上執行 npx 的必要包裝 |
" },

        // ── AI Application Chapter 404 ────────────────────────────
        new() { Id=404, Category="ai", Order=5, Level="advanced", Icon="💰", Title="省 Token 技巧與效率最大化", Slug="token-saving-efficiency", IsPublished=true, Content=@"
# 省 Token 技巧與效率最大化

## Token 是什麼？

> 💡 **比喻：計程車跳錶**
> 每次你跟 AI 對話，就像搭計程車——
> 你說的每個字（輸入 Token）和 AI 回覆的每個字（輸出 Token）都在跳錶。
> 說越多、回越長，費用越高。
>
> 省 Token = 省錢 = 用更少的「油」跑更遠的「路」。

### Token 的計算方式

```
什麼是 Token？
- 英文：大約 1 個單字 = 1 Token（""hello"" = 1 Token）
- 中文：大約 1 個字 = 1.5~2 Token（""你好"" ≈ 3 Token）
- 程式碼：依長度而定（一行 code ≈ 5-15 Token）

Token 的兩種類型：
- 輸入 Token（Input）：你說的話 + CLAUDE.md + 讀取的檔案
- 輸出 Token（Output）：AI 回覆的內容 + 修改的檔案

輸出 Token 通常比輸入 Token 貴 3-5 倍！
所以讓 AI 「少回廢話」比「你少打字」更省錢。
```

### Token 費用示意

```
操作                          大約 Token 消耗
────────────────────────────────────────────────
讀取 CLAUDE.md（有快取）      ≈ 0（幾乎免費！）
簡單問答                      ≈ 500-1,000
修改一個檔案                  ≈ 2,000-5,000
建立完整 CRUD                 ≈ 10,000-30,000
大型重構                      ≈ 50,000-100,000

Claude Opus 4 大約價格參考：
輸入：$15 / 百萬 Token
輸出：$75 / 百萬 Token
```

---

## 省 Token 的 7 個技巧

### 技巧 1：CLAUDE.md 用快取

```
❌ 每次對話都重新解釋專案：
""我的專案用 .NET 8，SQLite，EF Core，
  有 HomeController, ProductController...""
→ 每次花 500+ Token 重複說明

✅ 寫好 CLAUDE.md：
AI 自動載入，有快取機制 ≈ 0 Token
→ 省下 95% 的重複說明 Token
```

**這是最重要的省 Token 技巧，沒有之一。**

### 技巧 2：精準 Prompt（不說廢話）

```
❌ 浪費 Token 的寫法（87 個字）：
""你好，我想請你幫我一個忙，
  如果你方便的話，可以幫我看一下
  HomeController.cs 這個檔案嗎？
  我覺得裡面可能有一些問題，
  你看看能不能幫我改一下，
  謝謝你喔！""

✅ 省 Token 的寫法（29 個字）：
""HomeController.cs 的 Index action
  回傳 null，改成回傳 View(model)""
→ 省了 60% 的輸入 Token，結果一樣好
```

### 技巧 3：SubAgent 多工

```
❌ 一個一個做：
""改 HomeController"" → 等 → ""改 ProductController"" → 等 → ""改 CSS""
→ 每次等待都在消耗 context

✅ 讓 AI 用 SubAgent 並行處理：
""同時幫我做以下三件事：
  1. HomeController 加上搜尋功能
  2. ProductController 加上分頁
  3. site.css 加上 dark mode 樣式""
→ AI 會分配 SubAgent 並行處理
→ 各自獨立 context，不互相干擾
→ 更快完成，Token 更少
```

### 技巧 4：一次給完整需求

```
❌ 來回問答（浪費 Token）：
你：""幫我建一個 Controller""
AI：""什麼 Controller？""
你：""ProductController""
AI：""要什麼 action？""
你：""CRUD""
AI：""用什麼 DbContext？""
→ 6 次往返 = 6 倍的 context 載入

✅ 一次到位：
""建立 ProductController，
  用 ApplicationDbContext，
  有完整 CRUD（Index, Details, Create, Edit, Delete），
  所有 action 用 async/await，
  加上中文註解""
→ 1 次往返 = 最少的 Token
```

### 技巧 5：用 Glob/Grep 而非讓 AI 猜

```
❌ 讓 AI 猜檔案位置：
""找一下有沒有用到 DbContext 的地方""
→ AI 會一個一個資料夾搜尋，讀很多不相關的檔案
→ 每個讀取的檔案都消耗 Token

✅ 明確告訴 AI：
""在 Controllers/ 資料夾裡，
  找出所有用到 ApplicationDbContext 的 Controller""
→ AI 用 Glob 精準搜尋，不浪費 Token
```

你也可以在 Prompt 中指示 AI 用特定工具：

```
""用 Grep 搜尋所有包含 'TODO' 的 .cs 檔案""
→ AI 會用 Grep 工具，比逐一讀檔案快且省 Token
```

### 技巧 6：bypassPermissions 避免重複確認

```
❌ 每次操作都要確認：
AI：""我要修改 HomeController.cs，可以嗎？""
你：""可以""
AI：""我要執行 dotnet build，可以嗎？""
你：""可以""
→ 每次確認都多一輪對話 = 多花 Token

✅ 設定 bypassPermissions：
在 settings.local.json 加上允許清單
→ AI 直接執行，不問許可
→ 省下確認來回的 Token
```

### 技巧 7：Hook 自動化

```
❌ 每次手動指示：
""改完後幫我 build""
""改完後幫我 build""（每次都要說）
→ 重複指令浪費 Token

✅ 設定 Stop Hook：
每次 AI 回應結束自動執行 dotnet build
→ 不用每次提醒 → 省 Token
```

---

## Context Window 管理

### 什麼是 Context Window？

```
Context Window = AI 的「工作記憶」
就像你桌上能同時攤開的文件數量有限

Claude Opus：最大 1M Token（約 75 萬個中文字）
→ 聽起來很多，但實際上很容易用完

消耗來源：
- CLAUDE.md 內容
- 對話歷史（越長消耗越多）
- 讀取的檔案內容
- AI 的回覆
```

### Compaction（自動壓縮）

```
當對話太長時，Claude Code 會自動壓縮對話歷史。

壓縮前：完整的對話記錄（包含所有細節）
壓縮後：摘要版本（可能丟失某些細節）

⚠️ 壓縮可能造成的問題：
- 之前的修改細節被遺忘
- 特定的程式碼片段被摘要掉
- AI 可能重複做已經做過的事
```

### 如何避免 Compaction 問題

```
1. 對話太長時開新 session
   → 有 CLAUDE.md 在，新 session 也不需要重新解釋

2. 重要的決定寫進 CLAUDE.md
   → 壓縮不影響 CLAUDE.md

3. 一個 session 做一個主題
   → 做完購物車 → 開新 session → 做結帳功能
```

---

## SubAgent 多工原理

> 💡 **比喻：老闆分配工作**
> 你是老闆，你有三件事要做。
> 方法 A：你自己一件一件做 → 慢
> 方法 B：你找三個員工，每人做一件 → 快
>
> SubAgent 就是方法 B。主 Agent（老闆）把工作分給 SubAgent（員工），
> 他們各自獨立完成，互不干擾。

### SubAgent 的運作方式

```
主 Agent（你的對話）
├── SubAgent 1：修改 HomeController.cs
├── SubAgent 2：修改 ProductController.cs
└── SubAgent 3：修改 site.css

每個 SubAgent 有獨立的 context
→ 不會互相占用 Token
→ 可以並行處理，更快完成
```

### 適合用 SubAgent 的情境

```
✅ 適合：
- 不同檔案的獨立修改（各改各的，不互相影響）
- 平行建立多個功能（各自獨立的功能）
- 同時搜尋多個來源（各自搜尋不同地方）

❌ 不適合：
- 需要互相參考的修改（Controller 要看 Model 的改動）
- 有先後順序的工作（要先建 Model 才能建 Controller）
- 需要統一風格的修改（可能各自寫出不同風格）
```

### 如何觸發 SubAgent

你不需要手動觸發 SubAgent，只要在 Prompt 中描述多個獨立任務：

```
""請同時幫我做以下事情：
  1. 在 Models/ 建立 Review.cs（Id, Content, Rating, ProductId）
  2. 在 wwwroot/css/review.css 寫評價區的樣式
  3. 在 Views/Shared/_Layout.cshtml 加上 review.css 的連結""

AI 會自動判斷這三件事可以並行，分配 SubAgent 處理。
```

---

## 實際範例：省 Token 寫法 vs 浪費 Token 寫法

### 情境：幫專案加上搜尋功能

```
❌ 浪費 Token 的對話（5 次往返）：

你：""我想加搜尋功能""
AI：""搜尋什麼？""
你：""搜尋課程""
AI：""在哪個頁面？""
你：""課程列表頁""
AI：""用什麼方式搜尋？""
你：""標題關鍵字""
AI：[開始實作]

總計：5 次往返 × 每次載入 context = 大量 Token
```

```
✅ 省 Token 的對話（1 次往返）：

你：""在 CourseController 的 Index action 加上搜尋功能，
  接收 query 字串參數，
  用 LIKE 搜尋 Course 的 Title 欄位，
  搜尋框放在 Views/Course/Index.cshtml 頂部，
  用 form GET 提交，保留搜尋結果""
AI：[直接實作完成]

總計：1 次往返 = 最少 Token
```

### Token 節省對比

```
方式              預估 Token 消耗     花費時間
──────────────────────────────────────────────
浪費寫法（5 往返） ≈ 15,000 Token     10 分鐘
省 Token 寫法      ≈ 4,000 Token      2 分鐘

節省：73% Token + 80% 時間
```

---

## 🤔 常見錯誤

### 錯誤 1：對話太長不開新 Session

```
❌ 一個 session 做了 3 小時
→ context 爆滿 → 自動 compaction → 丟失細節
→ AI 開始忘記之前的修改 → 重複工作 → 更浪費 Token

✅ 一個功能做完就開新 session
→ 做完購物車 → /exit → claude → 做結帳功能
→ 有 CLAUDE.md 在，新 session 也能馬上進入狀態
```

### 錯誤 2：每次重複解釋專案背景

```
❌ 每次對話都打：
""我的專案用 .NET 8，有 HomeController...""
→ 重複了 100 次 = 浪費幾萬 Token

✅ 寫一次 CLAUDE.md：
以後每次對話自動載入（快取 ≈ 0 Token）
→ 一勞永逸
```

### 錯誤 3：不用 SubAgent 全部自己跑

```
❌ 一個一個指令：
""改 HomeController"" → 等 5 分鐘
""改 ProductController"" → 等 5 分鐘
""改 CSS"" → 等 5 分鐘
→ 總共 15 分鐘

✅ 同時指派多個獨立任務：
""同時改 HomeController、ProductController、CSS""
→ AI 分 SubAgent 並行 → 5 分鐘搞定
→ 時間省 2/3，Token 也更少
```

---

## 本章重點整理

| 技巧 | 省 Token 幅度 | 難度 |
|------|-------------|------|
| CLAUDE.md 快取 | ★★★★★ | 簡單 |
| 精準 Prompt | ★★★★ | 中等 |
| 一次給完整需求 | ★★★★ | 簡單 |
| SubAgent 多工 | ★★★ | 中等 |
| Glob/Grep 搜尋 | ★★★ | 簡單 |
| bypassPermissions | ★★ | 簡單 |
| Hook 自動化 | ★★ | 中等 |
" },

        // ── AI Application Chapter 405 ────────────────────────────
        new() { Id=405, Category="ai", Order=6, Level="intermediate", Icon="🎯", Title="AI 協作實戰：從需求到部署", Slug="ai-collaboration-practice", IsPublished=true, Content=@"
# AI 協作實戰：從需求到部署

## 完整案例：用 AI 從零建一個吉他教學網站

> 💡 **比喻：蓋房子**
> 傳統方式：你一個人搬磚、砌牆、裝修 → 很慢
> AI 協作：你當建築師畫設計圖，AI 幫你施工 → 很快
>
> 但你還是要會看設計圖（看得懂 code），
> 知道什麼結構安全（理解架構），
> 以及驗收品質（測試和 Review）。

接下來我們用一個**真實案例**，示範從需求描述到成功部署的完整流程。

---

## Step 1：寫好 Prompt（需求描述）

一切的起點是一個好的需求描述。

### 初始 Prompt 範例

```
你是一個資深 ASP.NET Core 開發者。

請幫我從零建立一個吉他教學網站 ""GuitarSchool""：

【技術棧】
- .NET 8 + ASP.NET Core MVC
- SQLite + Entity Framework Core
- Bootstrap 5 + 自訂 CSS

【功能需求】
1. 首頁：Hero Banner + 精選課程 + 老師介紹
2. 課程列表：依難度篩選（初學/中級/進階），卡片排版
3. 課程詳情：Markdown 內容渲染、影片嵌入
4. 聯絡表單：Name, Email, Phone, Message
5. 關於頁面：學校介紹 + Google Map

【設計風格】
- 暖米色系（背景 #F5F1EB，文字 #2C3E50）
- 圓角按鈕、卡片陰影
- 響應式設計（手機、平板、桌面）
- 字體：Google Fonts Noto Sans TC

【部署方式】
- Docker container
- Railway 平台
- 需要 Dockerfile

【注意事項】
- View 文字全部繁體中文
- 所有 action 用 async/await
- 加上中文註解
- Seed Data 至少 6 堂課程
```

### 為什麼這個 Prompt 有效？

```
✅ 明確的角色：「資深 ASP.NET Core 開發者」
✅ 完整的技術棧：.NET 8、SQLite、Bootstrap 5
✅ 具體的功能：5 個頁面，每個都有描述
✅ 設計規範：色碼、圓角、字體
✅ 部署資訊：Docker + Railway
✅ 限制條件：繁體中文、async/await、註解
```

---

## Step 2：AI 規劃架構

不要讓 AI 直接寫 code！先讓它規劃。

### 使用 Plan Mode

```
> 先用 plan mode 規劃這個專案的架構，
> 不要直接寫 code。
> 列出：
> 1. 資料夾結構
> 2. 所有 Model
> 3. 所有 Controller 和 Action
> 4. 所有 View
> 5. 實作順序
```

AI 會回傳一個完整的規劃，像這樣：

```
實作順序：
1. 建立專案 + 基本架構
2. Model + DbContext + Migration
3. Seed Data（課程資料）
4. HomeController + 首頁 View
5. CourseController + 課程列表/詳情 View
6. ContactController + 聯絡表單
7. CSS 調整 + 響應式設計
8. Dockerfile + 部署設定
```

你確認計畫 OK 後，AI 才開始實作。

---

## Step 3：分批執行

> 💡 **比喻：吃飯**
> 不要把整桌菜一口塞進嘴裡，一道一道吃。
> 不要一次給 AI 所有工作，分批做品質更好。

### 批次執行策略

```
第一批：基礎建設
""建立專案 + Model + DbContext + Migration + Seed Data""
→ 確認 dotnet build 成功

第二批：頁面開發
""建立 HomeController + 首頁 View + 課程列表 View""
→ 確認 dotnet run 後頁面正常

第三批：功能完善
""加上聯絡表單 + 課程詳情 + 關於頁面""
→ 手動測試每個功能

第四批：樣式調整
""CSS 調整：暖米色系、圓角、陰影、響應式""
→ 用不同裝置尺寸檢查

第五批：部署
""建立 Dockerfile + docker-compose.yml + Railway 設定""
→ 本地 Docker 測試 → 部署到 Railway
```

### 每批完成後的檢查清單

```
□ dotnet build 編譯成功？
□ dotnet run 啟動正常？
□ 頁面載入正確？
□ 功能操作正確？
□ 手機版顯示正常？
□ 沒有 console 錯誤？
□ git commit 備份？
```

---

## Step 4：遇到問題怎麼辦

真實開發一定會遇到問題。以下是常見問題和解決方式。

### 問題 1：500 Internal Server Error

```
你：""網站出現 500 錯誤，幫我找原因""

AI 的處理流程：
1. 讀取 log 檔案或 console 輸出
2. 找到錯誤訊息：
   ""System.InvalidOperationException:
    The ForwardedHeaders middleware must be added...""
3. 定位問題：Railway 用 reverse proxy，
   需要設定 ForwardedHeaders
4. 修復：在 Program.cs 加上 ForwardedHeaders 設定

修復後你只需要確認：頁面正常載入了嗎？
```

### 問題 2：部署失敗

```
你：""Railway 部署後網站無法登入，Cookie 有問題""

AI 的處理流程：
1. 分析問題：Railway 用 HTTPS proxy
   但 Cookie 的 SameSite 設定不相容
2. 修復：把 Cookie SameSite 改成 SameSiteMode.Lax
   或在 production 環境設定正確的 Cookie policy
3. 重新部署

你只需要確認：登入功能正常了嗎？
```

### 問題 3：CSS 跑版

```
你：""手機版的 Navbar 跑版，漢堡選單打不開""

好的問題描述：
""手機版（<768px）的 Navbar 有兩個問題：
  1. 漢堡選單按鈕點擊沒反應
  2. 下拉選單的背景色是透明的，看不到文字
  請檢查 Bootstrap JS 有沒有正確引入，
  以及 navbar-collapse 的 CSS""
```

---

## Step 5：自動化設定

當網站基本完成後，設定自動化讓開發更順暢。

### 設定 CLAUDE.md

```markdown
# GuitarSchool 專案

## 技術棧
- .NET 8 + ASP.NET Core MVC
- SQLite + EF Core
- Bootstrap 5
- 部署：Railway

## 重要指令
- dotnet build：編譯
- dotnet run：啟動（port 5001）
- docker build -t guitarschool .：建 Docker image

## CSS 色碼
- 背景：#F5F1EB
- 文字：#2C3E50
- 強調：#E67E22

## 注意
- 繁體中文
- async/await
- 不用 Repository Pattern
```

### 設定 bypassPermissions

```json
{
  ""permissions"": {
    ""allow"": [
      ""Bash(dotnet *)"",
      ""Bash(git *)"",
      ""Read(*)"",
      ""Write(*.cs)"",
      ""Write(*.cshtml)"",
      ""Write(*.css)"",
      ""Write(*.json)""
    ]
  }
}
```

### 設定 Deploy Skill

建立 `.claude/skills/deploy/SKILL.md`：

```markdown
---
description: 部署 GuitarSchool 到 Railway
command: deploy
---

1. dotnet build -c Release
2. dotnet test（如果有測試）
3. git add -A
4. git commit -m ""deploy: update""
5. git push origin main
6. 回報部署結果
```

以後只要在 Claude Code 輸入 `/deploy` 就能一鍵部署。

---

## Step 6：持續迭代

網站上線後，你還可以持續用 AI 加功能。

### 一句話加功能

```
""幫我在首頁加上老師介紹區塊，
  三個老師的照片和簡介，用 Bootstrap Card 排版""

""幫我加上 SEO meta tags，
  每個頁面都要有 title 和 description""

""幫我在課程詳情頁加上價格表，
  分成單堂/10堂/20堂，用表格呈現""

""幫我把聯絡表單改成能寄 Email，
  用 MailKit 套件，SMTP 設定放環境變數""
```

每個需求都只要一句話，因為 AI 已經透過 CLAUDE.md 知道你的專案。

### 迭代的最佳實踐

```
1. 每次只做一個功能
2. 做完就 commit（備份）
3. 測試完再進入下一個功能
4. 重大變更前先開 branch
5. 定期更新 CLAUDE.md
```

---

## 給新手的建議

### 建議 1：從小任務開始

```
不要一開始就叫 AI 做一整個系統。
先從小任務建立信心：

Week 1：""幫我改按鈕顏色""
Week 2：""幫我加一個頁面""
Week 3：""幫我建一個 CRUD 功能""
Week 4：""幫我從零建一個小專案""

像學開車一樣，先在停車場練習，再上馬路。
```

### 建議 2：習慣看 AI 的程式碼

```
❌ AI 寫完 → 直接用 → 出問題不知道怎麼修

✅ AI 寫完 → 你看一遍 → 理解邏輯 → 有問題問 AI：
""這段 code 的 .Where() 為什麼用 Contains 而不是 ==？""
""這個 async Task<IActionResult> 是什麼意思？""

→ 每次 Review 都是學習機會
```

### 建議 3：學會用 Git 備份

```
AI 也會犯錯。它可能：
- 不小心刪掉你的檔案
- 大改架構後編譯不過
- 改壞一個功能的同時影響另一個功能

保護自己的方法：

# 每次讓 AI 大改前
git add -A && git commit -m ""backup before AI changes""

# AI 改壞了？一鍵還原
git checkout .

# 更安全：開一個新 branch
git checkout -b feature/new-feature
# 讓 AI 在 branch 上改
# 確認沒問題後再 merge
```

### 建議 4：建立自己的 Prompt 模板

```
隨著經驗累積，你會發現某些 Prompt 模式特別有效。
把它們存起來：

模板 1：新功能
""在 {Controller} 加上 {功能名稱}，
  Model 是 {Model}（{欄位列表}），
  View 用 {排版方式}，
  注意 {限制條件}""

模板 2：修 Bug
""{檔案} 第 {行數} 出現 {錯誤類型}，
  {變數/物件} 是 {狀態}，
  請檢查 {可能原因}""

模板 3：改樣式
""把 {元件} 的 {屬性} 改成 {值}，
  hover 時 {變化}，
  加上 {動畫}""
```

---

## 完整工作流程圖

```
┌─────────────────────────────────────────────────────┐
│                    AI 協作開發流程                      │
├─────────────────────────────────────────────────────┤
│                                                      │
│  1. 準備階段                                          │
│     ├── 寫好 CLAUDE.md（專案記憶）                     │
│     ├── 設定 .mcp.json（MCP 插件）                     │
│     ├── 設定 bypassPermissions（免問許可）              │
│     └── 建立 Skills（常用指令）                         │
│                                                      │
│  2. 開發階段                                          │
│     ├── 寫好 Prompt（具體需求）                         │
│     ├── 讓 AI 規劃（Plan Mode）                        │
│     ├── 分批執行（一次一個功能）                         │
│     ├── 每批檢查（build + run + test）                  │
│     └── git commit 備份                                │
│                                                      │
│  3. 問題排除                                          │
│     ├── 看 error log                                  │
│     ├── 描述具體錯誤給 AI                               │
│     ├── AI 修復 → 你驗證                                │
│     └── 修不好？還原 git → 換方式                        │
│                                                      │
│  4. 部署上線                                           │
│     ├── Docker build 測試                               │
│     ├── 部署到 Railway                                  │
│     ├── 線上測試所有功能                                 │
│     └── 設定自動部署 Hook                                │
│                                                      │
│  5. 持續迭代                                           │
│     ├── 一句話加新功能                                   │
│     ├── 每次 commit 備份                                │
│     ├── 定期更新 CLAUDE.md                               │
│     └── 經驗存入 Memory MCP                              │
│                                                      │
└─────────────────────────────────────────────────────┘
```

---

## 🤔 常見錯誤

### 錯誤 1：不備份就讓 AI 大改

```
❌ 直接跟 AI 說「重構整個專案」
→ AI 改壞了 → 沒有備份 → 全部重來

✅ 大改之前一定先備份：
git add -A && git commit -m ""backup before refactor""
→ 改壞了隨時還原
```

### 錯誤 2：不測試就部署

```
❌ AI 說「改好了」→ 直接部署到線上
→ 線上爆炸 → 用戶看到 500 錯誤

✅ 部署前檢查清單：
□ dotnet build 成功？
□ dotnet run 本地正常？
□ Docker build 成功？
□ Docker run 本地正常？
□ 所有頁面都能載入？
□ 表單提交正常？
□ 手機版正常？
→ 全部通過才部署
```

### 錯誤 3：把 API Key 給 AI 貼在公開地方

```
❌ 在 Prompt 裡告訴 AI：
""SMTP 密碼是 abc123，寫在 appsettings.json""
→ 如果 appsettings.json 被 commit 到 GitHub → 密碼外洩

✅ 正確做法：
""SMTP 設定用環境變數，
  本地用 dotnet user-secrets，
  Railway 用 Variables 設定，
  不要寫在任何會被 commit 的檔案裡""
```

---

## 本章重點整理

| 步驟 | 重點 |
|------|------|
| 需求描述 | Prompt 要具體：技術棧 + 功能 + 風格 + 限制 |
| 規劃架構 | 先用 Plan Mode 規劃，確認後再實作 |
| 分批執行 | 一次一個功能，每批都要 build + test |
| 問題排除 | 給 AI 具體錯誤訊息，不要只說「壞了」 |
| 自動化 | CLAUDE.md + Skills + Hooks + bypassPermissions |
| 持續迭代 | 一句話加功能，每次都 commit 備份 |
| 安全原則 | 備份、測試、不洩漏 API Key |
" },
    };
}