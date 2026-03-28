using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_ClaudeCode
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Claude Code Chapter 540 ────────────────────────────
        new() { Id=540, Category="claudecode", Order=1, Level="beginner", Icon="🤖", Title="Claude Code 完整指南", Slug="claude-code-complete-guide", IsPublished=true, Content=@"
# Claude Code 完整指南

## Claude Code 是什麼？

> 💡 **比喻：你的 AI 程式設計師助手**
> 想像你請了一個全天候的程式設計師助手：
> - 他坐在你旁邊，可以直接動你的電腦
> - 你用中文說「幫我改那個檔案」，他就改
> - 他可以讀你的整個專案、寫檔案、跑指令
> - 但他不會自己做決定——你說什麼，他做什麼
>
> 這就是 Claude Code。不是聊天機器人，是**能動手做事的 AI 助手**。

### Claude Code vs 其他 AI 工具

```
工具              能讀檔案嗎    能改檔案嗎    能跑指令嗎    能操作瀏覽器嗎
──────────────────────────────────────────────────────────────────────
ChatGPT 網頁版    ❌           ❌           ❌           ❌
GitHub Copilot    ✅ 當前檔案   ✅ 建議補全   ❌           ❌
Cursor            ✅ 專案      ✅ 直接改     ❌           ❌
Claude Code       ✅ 專案      ✅ 直接改     ✅           ✅（透過 MCP）
```

> 簡單說：Claude Code 是目前功能最完整的 AI 開發工具之一。

---

## 安裝與設定流程

### 前置需求

你只需要裝好 **Node.js**，然後在終端機執行一行指令就好。

> 💡 **比喻：裝 App**
> 安裝 Claude Code 就像在手機上裝 App：
> - Node.js = 你的手機作業系統（基礎環境）
> - npm install = 去 App Store 下載
> - claude = 打開 App

### 安裝步驟

```bash
npm install -g @anthropic-ai/claude-code  # 全域安裝 Claude Code 套件
claude --version                           # 確認安裝成功，顯示版本號
```

### 第一次啟動

```bash
cd C:/Users/user/MyProject  # 進入你的專案資料夾
claude                       # 啟動 Claude Code 互動介面
```

第一次啟動會要求你登入 Anthropic 帳號並設定 API Key。

### 設定 API Key

```bash
# 方法 1：環境變數（推薦）
export ANTHROPIC_API_KEY=""sk-ant-xxxxx""  # 設定你的 API 金鑰為環境變數

# 方法 2：啟動時自動讀取
# Claude Code 會自動偵測環境變數中的金鑰
```

---

## 基本操作：讀檔、寫檔、搜尋、編輯

> 💡 **比喻：遙控器**
> Claude Code 的每個操作就像遙控器上的按鈕：
> - 📖 讀檔 = 按「資訊」鍵，看電視節目介紹
> - ✏️ 寫檔 = 按「錄影」鍵，錄下節目
> - 🔍 搜尋 = 按「搜尋」鍵，找想看的節目
> - 📝 編輯 = 按「編輯」鍵，修改錄影設定

### 讀取檔案

```
你說：""幫我看一下 Program.cs 的內容""
AI 回應：讀取 Program.cs，顯示完整內容並解釋每一段的用途

你說：""這個專案有哪些 Controller？""
AI 回應：自動搜尋所有 *Controller.cs 檔案，列出清單
```

### 寫入新檔案

```
你說：""幫我建一個 ProductController.cs，要有 Index 和 Details action""
AI 回應：建立新檔案，寫入完整的 Controller 程式碼

你說：""幫我建一個 Product Model，有 Id、Name、Price 欄位""
AI 回應：建立 Models/Product.cs，包含所有屬性
```

### 搜尋程式碼

```
你說：""搜尋所有用到 DbContext 的地方""
AI 回應：列出所有引用 DbContext 的檔案和行數

你說：""找出所有 TODO 註解""
AI 回應：掃描整個專案，列出所有 TODO
```

### 編輯現有檔案

```
你說：""把 HomeController 的 Index action 改成回傳 JSON""
AI 回應：找到檔案，修改指定的程式碼，顯示修改前後的差異
```

---

## Session vs Project 概念

> 💡 **比喻：工作日 vs 辦公室**
> - **Session（會話）** = 今天的工作日。你進辦公室開始工作，下班就結束。
>   明天是新的工作日，但辦公室還是同一個。
> - **Project（專案）** = 你的辦公室。不管哪一天來，資料和環境都還在。

### Session（會話）

```
特性：
- 每次啟動 claude 就是一個新的 Session       # 類似開新的聊天視窗
- Session 結束後，對話歷史不會保留             # 就像關掉聊天視窗
- 但檔案修改是永久的                          # AI 改的檔案不會消失
- 可以用 /clear 清除當前 Session 的記憶       # 重新開始對話
```

### Project（專案）

```
特性：
- 專案是你的程式碼資料夾                      # 例如 C:/Users/user/MyProject
- CLAUDE.md 是專案的永久記憶                  # 每次啟動都會讀取
- .claude/ 資料夾存放專案設定                  # 包含 MCP 設定等
- 不管開幾次 Session，專案設定都一樣           # 持久化的環境
```

---

## CLAUDE.md 設定（記憶與規則）

> 💡 **比喻：員工手冊**
> CLAUDE.md 就像公司的員工手冊：
> - 新員工（新 Session）報到第一天就要讀
> - 裡面寫了公司規則（程式風格）
> - 寫了常見 SOP（專案慣例）
> - 每次來上班都要遵守

### CLAUDE.md 的位置

```
你的專案/
├── CLAUDE.md          # 放在專案根目錄，每次啟動自動讀取
├── Program.cs         # 你的程式碼
├── Controllers/       # Controller 資料夾
└── .claude/           # Claude Code 的設定資料夾
```

### CLAUDE.md 範例內容

```markdown
# 專案規則

## 技術棧
- ASP.NET Core 8.0 MVC                    # 使用的框架版本
- Entity Framework Core + SQLite           # ORM 和資料庫
- Bootstrap 5                              # 前端 CSS 框架

## 程式風格
- 所有註解用繁體中文                        # 註解語言規範
- Controller 名稱用英文                     # 命名規範
- 每個 public method 都要寫 XML 文件註解    # 文件要求

## 專案結構
- Controllers/ 放所有 Controller            # 資料夾用途說明
- Models/ 放所有資料模型                     # 讓 AI 知道檔案放哪
- Views/ 按 Controller 名稱分子資料夾        # View 的組織方式

## 注意事項
- 不要刪除任何現有的 Migration               # 重要警告
- 密碼和 API Key 用環境變數                  # 安全規範
- 每次修改完要執行 dotnet build 確認編譯通過  # 品質要求
```

### 為什麼 CLAUDE.md 很重要？

```
沒有 CLAUDE.md：
Session 1：""用繁體中文寫註解""   → AI 照做
Session 2：（忘了）               → AI 用英文寫註解
Session 3：""用繁體中文寫註解""   → 又要重複說一次

有 CLAUDE.md：
Session 1：自動讀取規則 → AI 用繁體中文寫註解
Session 2：自動讀取規則 → AI 用繁體中文寫註解（不用重複說）
Session 3：自動讀取規則 → AI 用繁體中文寫註解（永遠記得）
```

---

## 權限管理 (bypassPermissions)

> 💡 **比喻：免蓋章放行**
> 正常情況下，AI 要改檔案時會問你：「可以改嗎？」
> 就像公司裡每個動作都要蓋章批准。
> 開啟 bypassPermissions 就像給 AI 一張「免蓋章通行證」——
> 他可以直接動手，不用每次都問你。

### 預設行為（需要確認）

```
你說：""把 HomeController 的標題改成 '歡迎'""
AI 回應：""我想要修改 HomeController.cs，可以嗎？"" [Y/n]    # 每次都要按 Y
你按：Y
AI 才開始改
```

### 開啟 bypassPermissions

在 CLAUDE.md 或設定中加入：

```json
{
  ""permissions"": {
    ""allow"": [""Read"", ""Write"", ""Edit"", ""Bash""],  // 允許讀寫編輯和執行指令
    ""deny"": []                                          // 沒有禁止的操作
  }
}
```

### 什麼時候該開？

```
✅ 適合開啟的情況：
- 你在自己的開發環境                          # 低風險環境
- 你熟悉 AI 的操作模式                        # 你知道它會做什麼
- 你想要快速開發，不想一直按 Y                 # 提高效率

❌ 不適合開啟的情況：
- 生產環境                                    # 風險太高
- 你剛開始用 Claude Code                       # 還不熟悉
- 涉及敏感資料或金流                           # 需要人工審核
```

---

## 常用斜線指令

> 💡 **比喻：快捷鍵**
> 斜線指令就像鍵盤快捷鍵：
> - /help = F1（求助）
> - /status = Ctrl+I（看資訊）
> - /clear = Ctrl+L（清畫面）

### 常用指令一覽

```
指令          功能                          使用時機
──────────────────────────────────────────────────────────
/help        顯示所有可用指令               不知道能做什麼的時候
/status      顯示當前狀態                   想知道 Session 資訊
/clear       清除對話記憶                   對話太長想重新開始
/compact     壓縮對話歷史                   對話太長但不想清除
/init        初始化 CLAUDE.md               第一次建立專案記憶
/cost        顯示 API 使用量                想知道花了多少錢
/model       切換使用的模型                 想換不同的 AI 模型
```

### 使用範例

```bash
# 查看所有可用指令
/help                                      # 顯示完整的指令說明

# 查看當前 Session 狀態
/status                                    # 顯示模型、token 使用量等資訊

# 對話太長時壓縮記憶
/compact                                   # 保留重要資訊，壓縮其餘部分

# 重新開始對話
/clear                                     # 清除所有對話歷史

# 自動產生 CLAUDE.md
/init                                      # AI 分析專案結構，自動寫 CLAUDE.md
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：指令太模糊，AI 做出來的不是你要的

```
❌ 你說：""幫我做一個網站""                   # 太模糊，AI 不知道你要什麼
AI 可能做出一個空白的 Hello World 頁面        # 因為沒有具體需求

✅ 正確做法：
""用 ASP.NET Core 8.0 MVC 做一個吉他教學網站，  # 指定技術棧
  功能：首頁、課程列表、聯絡表單，                # 指定功能
  用 Bootstrap 5，暖米色系，                      # 指定風格
  資料庫用 SQLite + Entity Framework Core""       # 指定資料庫

💡 記住：你給 AI 的指令越具體，結果越接近你要的。
```

### ❌ 錯誤 2：沒有建 CLAUDE.md，每次都要重複說規則

```
❌ 常見情況：
Session 1：""註解用繁體中文""    → AI 照做      # 第一次有說
Session 2：忘了說               → AI 用英文寫   # 忘了就沒有
Session 3：""上次不是說好了嗎？"" → AI 不記得     # Session 間沒有記憶

✅ 正確做法：
1. 執行 /init 讓 AI 自動建立 CLAUDE.md         # 或自己手動建
2. 在 CLAUDE.md 寫下所有規則                    # 一次寫好，永久生效
3. 以後每次啟動都會自動讀取                      # 不用再重複說明
```

### ❌ 錯誤 3：開了 bypassPermissions 卻不檢查 AI 的修改

```
❌ 危險情況：
開啟 bypassPermissions                          # AI 不需要確認就能改檔案
→ AI 誤解需求，改錯檔案                          # 沒有人工審核
→ 改壞了才發現                                   # 來不及了

✅ 正確做法：
1. 開啟 bypassPermissions 沒關係                 # 提高效率是好事
2. 但要搭配 Git 版本控制                          # 每次改完先 commit
3. 改完後用 git diff 檢查修改內容                  # 養成檢查的習慣
4. 有問題就 git checkout 回復                      # 隨時可以反悔
```

---

## 本章重點整理

| 主題 | 重點 |
|------|------|
| Claude Code 是什麼 | 能讀寫檔案、跑指令的 AI 開發助手 |
| 安裝方式 | npm install -g @anthropic-ai/claude-code |
| 基本操作 | 用自然語言指揮 AI 讀檔、寫檔、搜尋、編輯 |
| Session vs Project | Session 是單次對話，Project 是持久化環境 |
| CLAUDE.md | 專案的永久記憶，每次啟動自動讀取 |
| bypassPermissions | 讓 AI 不用每次問確認，搭配 Git 使用 |
| 斜線指令 | /help, /status, /clear, /compact, /init |
" },

        // ── Claude Code Chapter 541 ────────────────────────────
        new() { Id=541, Category="claudecode", Order=2, Level="intermediate", Icon="🔌", Title="MCP 擴充與 Skills 系統", Slug="mcp-extensions-and-skills", IsPublished=true, Content=@"
# MCP 擴充與 Skills 系統

## MCP (Model Context Protocol) 概念

> 💡 **比喻：幫 AI 裝手和眼睛**
> Claude Code 本身就像一個很聰明的大腦，但它只能「想」。
> MCP 就是幫這個大腦裝上「手」和「眼睛」：
> - 🖐️ **filesystem MCP** = 裝上手，可以碰你的檔案
> - 👀 **Chrome MCP** = 裝上眼睛，可以看到瀏覽器畫面
> - 🧠 **memory MCP** = 裝上記憶體，可以記住跨 Session 的資訊
> - 📂 **Google Drive MCP** = 裝上雲端手臂，可以存取你的雲端文件
>
> 沒有 MCP，AI 只能跟你聊天。有了 MCP，AI 可以動手做事。

### MCP 架構圖

```
你（使用者）
  │
  ▼
Claude Code（AI 大腦）
  │
  ├── filesystem MCP ──→ 讀寫你電腦上的檔案     # 基本的檔案操作能力
  ├── Chrome MCP ──────→ 操作瀏覽器（點擊、截圖）# 看網頁、測試網站
  ├── memory MCP ──────→ 長期記憶儲存            # 跨 Session 記住資訊
  ├── Google Drive MCP →  存取雲端文件           # 讀取 Google 文件
  └── 其他 MCP ────────→ 無限擴充可能            # 社群開發的各種工具
```

### 為什麼需要 MCP？

```
沒有 MCP 的 AI：                               # 只能純文字對話
""幫我看看網站長什麼樣子""
→ ""我沒辦法看網站，請你截圖給我""                # 無法操作瀏覽器

有 Chrome MCP 的 AI：                           # 可以操作瀏覽器
""幫我看看網站長什麼樣子""
→ AI 自動打開瀏覽器 → 截圖 → 分析畫面            # 直接動手看
→ ""網站目前有一個 Navbar 和三張卡片，配色是...""  # 回報結果
```

---

## 常用 MCP 介紹

### 1. filesystem MCP（檔案系統）

```
功能：讀取、寫入、搜尋檔案                       # 最基本的 MCP
用途：讓 AI 操作你電腦上的檔案                     # 不限於專案目錄

常用操作：
- read_file：讀取檔案內容                         # 看程式碼
- write_file：寫入新檔案                          # 建立新檔案
- list_directory：列出資料夾內容                   # 看有哪些檔案
- search_files：用 glob 搜尋檔案                  # 找特定檔案
- edit_file：編輯現有檔案                          # 修改程式碼
```

### 2. Chrome MCP（瀏覽器操作）

```
功能：操作 Chrome 瀏覽器                          # 裝了就能看網頁
用途：預覽網站、截圖、點擊、填表單                 # 測試和預覽

常用操作：
- navigate：導航到指定 URL                        # 打開網頁
- screenshot：擷取畫面截圖                        # 看網站長什麼樣
- left_click：滑鼠點擊                            # 模擬使用者操作
- type：輸入文字                                  # 填寫表單
- read_page：讀取頁面結構                          # 分析 DOM 元素
```

### 3. memory MCP（長期記憶）

```
功能：建立知識圖譜，儲存跨 Session 的資訊          # 比 CLAUDE.md 更結構化
用途：記住使用者偏好、專案重要資訊                 # 長期記憶

常用操作：
- create_entities：建立知識節點                    # 新增一條記憶
- search_nodes：搜尋記憶                          # 找之前記住的事
- create_relations：建立節點關係                   # 記住 A 和 B 有關
- read_graph：讀取整個知識圖譜                     # 看所有記憶
```

### 4. Google Drive MCP（雲端文件）

```
功能：搜尋和讀取 Google Drive 文件                 # 存取雲端
用途：讀取規格文件、設計稿、會議記錄               # 讀取雲端文件

常用操作：
- google_drive_search：搜尋 Drive 文件             # 找文件
- google_drive_fetch：讀取文件內容                  # 看文件內容
```

---

## MCP 安裝設定（.mcp.json 格式）

> 💡 **比喻：安裝外掛**
> 安裝 MCP 就像在遊戲裡安裝外掛（mod）：
> - .mcp.json = 外掛清單（告訴遊戲要載入哪些外掛）
> - 每個 MCP = 一個外掛（提供特定功能）
> - 安裝完重新啟動就能用

### .mcp.json 檔案位置

```
你的專案/
├── .claude/
│   └── .mcp.json      # 放在 .claude 資料夾裡                # 專案級別設定
├── CLAUDE.md           # 專案記憶檔                           # 和 MCP 設定分開
└── Program.cs          # 你的程式碼                           # 正常的專案檔案
```

### .mcp.json 格式範例

```json
{
  ""mcpServers"": {                                // MCP 伺服器清單
    ""filesystem"": {                              // filesystem MCP 設定
      ""command"": ""npx"",                        // 用 npx 執行
      ""args"": [                                  // 執行參數
        ""-y"",                                    // 自動確認安裝
        ""@anthropic-ai/mcp-filesystem"",          // MCP 套件名稱
        ""C:/Users/user""                          // 允許存取的路徑
      ]
    },
    ""memory"": {                                  // memory MCP 設定
      ""command"": ""npx"",                        // 用 npx 執行
      ""args"": [                                  // 執行參數
        ""-y"",                                    // 自動確認安裝
        ""@anthropic-ai/mcp-memory""               // MCP 套件名稱
      ]
    }
  }
}
```

### 安裝新的 MCP

```bash
# 方法 1：手動編輯 .mcp.json（加入新的 MCP 設定）  # 直接改設定檔
# 方法 2：用 Claude Code 指令安裝                   # 更方便
claude mcp add filesystem npx @anthropic-ai/mcp-filesystem C:/Users/user

# 安裝完重啟 Claude Code                            # 重新啟動讓設定生效
claude
```

---

## Skills 是什麼（預寫好的 Prompt 模板）

> 💡 **比喻：食譜卡**
> Skills 就像廚房裡的食譜卡：
> - 每張卡片 = 一個 Skill（例如「做番茄炒蛋」）
> - 卡片上寫了步驟和材料
> - 你只要說「照食譜做」，AI 就知道該怎麼做
> - 不用每次都從頭解釋

### Skill 的用途

```
沒有 Skill：
每次都要說：""幫我建一個 ASP.NET Core MVC 專案，  # 每次都要打很長的指令
  用 Bootstrap 5，SQLite，                         # 重複的需求描述
  繁體中文註解，暖米色系...""                       # 浪費時間

有 Skill：
直接說：""用 /create-project skill""               # 一句話搞定
→ AI 自動按照 Skill 定義的模板執行                  # 不用重複描述
```

---

## SKILL.md 格式與建立

### SKILL.md 的結構

```markdown
---
description: 建立 ASP.NET Core 專案的標準流程       # Skill 的簡短說明
---

# 建立 ASP.NET Core 專案                           # Skill 的標題

## 步驟                                             # 詳細步驟
1. 用 dotnet new mvc 建立專案                       # 第一步
2. 安裝 Entity Framework Core 和 SQLite             # 第二步
3. 建立基本的 Model 和 DbContext                     # 第三步
4. 設定 Program.cs 的 DI 註冊                       # 第四步
5. 建立初始 Migration                               # 第五步

## 規則                                             # 要遵守的規則
- 所有註解用繁體中文                                 # 註解語言
- 用 Bootstrap 5 做前端                             # 前端框架
- 資料庫用 SQLite                                   # 資料庫選擇
```

### SKILL.md 的存放位置

```
你的專案/
├── .claude/
│   └── skills/
│       ├── create-project/
│       │   └── SKILL.md       # 建立專案的 Skill      # 每個 Skill 一個資料夾
│       ├── deploy/
│       │   └── SKILL.md       # 部署的 Skill           # 可以有多個 Skill
│       └── fix-bug/
│           └── SKILL.md       # 修 Bug 的 Skill        # 按功能分類
└── Program.cs
```

### 建立自己的 Skill

```bash
# 方法 1：請 AI 幫你建                              # 最簡單的方式
> ""幫我建一個 Skill，功能是自動建立 ASP.NET Core MVC 專案""

# 方法 2：手動建立                                   # 自己控制內容
mkdir -p .claude/skills/create-project               # 建立資料夾
# 然後在裡面建 SKILL.md                              # 寫 Skill 內容
```

---

## Hooks 自動化（Stop hook, Start hook）

> 💡 **比喻：自動門感應器**
> Hooks 就像商店的自動門感應器：
> - **Start Hook** = 有人進門時，自動開燈、播放歡迎語
> - **Stop Hook** = 最後一個人離開時，自動關燈、鎖門
> - 你不用手動操作，一切自動發生

### 什麼是 Hook？

```
Hook = 在特定時機自動執行的腳本                      # 自動化的關鍵

常用 Hook：
- PreToolUse：AI 使用工具之前觸發                    # 在操作前攔截
- PostToolUse：AI 使用工具之後觸發                   # 操作完成後檢查
- Notification：AI 需要通知時觸發                    # 發送通知
- Stop：AI 停止回應時觸發                            # 結束時自動執行
```

### Hook 設定範例

```json
{
  ""hooks"": {                                       // Hook 設定區塊
    ""PostToolUse"": [                               // 工具使用後觸發
      {
        ""matcher"": ""Write|Edit"",                 // 當寫入或編輯檔案時
        ""command"": ""dotnet build""                // 自動執行 build
      }
    ],
    ""Stop"": [                                      // AI 停止回應時觸發
      {
        ""command"": ""git add -A && git commit -m 'auto-save'"" // 自動存檔
      }
    ]
  }
}
```

### Hook 的實際用途

```
場景 1：自動 build
每次 AI 改完程式碼 → 自動 dotnet build → 馬上知道有沒有編譯錯誤
                                                    # 不用手動執行
場景 2：自動測試
每次 AI 改完程式碼 → 自動跑 dotnet test → 確認沒有破壞現有功能
                                                    # 持續整合概念
場景 3：自動 commit
AI 完成一個任務 → 自動 git commit → 不怕改壞回不去
                                                    # 自動版本控制
```

---

## 實際案例：自動部署 Hook

### 情境說明

```
目標：每次 AI 完成修改，自動部署到 Railway              # 改完即部署
流程：AI 改 code → 自動 build → 自動 test → 自動部署   # 全自動化
```

### 設定步驟

```json
{
  ""hooks"": {                                         // Hook 設定
    ""Stop"": [                                        // AI 停止時觸發
      {
        ""command"": ""dotnet build && dotnet test && git add -A && git commit -m 'auto-deploy' && git push""
                                                       // 依序執行 build、test、commit、push
      }
    ]
  }
}
```

### 執行流程

```
你說：""幫我在首頁加一個輪播圖""                        # 只需描述需求
  ↓
AI 開始工作                                            # 自動讀取、修改檔案
  ↓
AI 修改 Views/Home/Index.cshtml                        # 加入輪播圖 HTML
AI 修改 wwwroot/css/site.css                           # 加入輪播圖樣式
  ↓
AI 完成，觸發 Stop Hook                                # 自動觸發
  ↓
dotnet build → 編譯通過 ✅                              # 確認沒有語法錯誤
dotnet test  → 測試通過 ✅                              # 確認沒有破壞功能
git commit   → 自動存檔 ✅                              # 版本控制
git push     → 推到 GitHub ✅                           # 觸發 Railway 部署
  ↓
Railway 偵測到 push → 自動部署 🚀                       # 網站自動更新
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：MCP 設定路徑寫錯

```json
❌ 錯誤的 .mcp.json：
{
  ""mcpServers"": {
    ""filesystem"": {
      ""command"": ""npx"",
      ""args"": [""-y"", ""@anthropic-ai/mcp-filesystem"", ""C:\\Users\\user""]
                                                       // Windows 反斜線會出錯
    }
  }
}

✅ 正確做法：
{
  ""mcpServers"": {
    ""filesystem"": {
      ""command"": ""npx"",
      ""args"": [""-y"", ""@anthropic-ai/mcp-filesystem"", ""C:/Users/user""]
                                                       // 用正斜線，避免跳脫問題
    }
  }
}

💡 在 JSON 中，路徑建議都用正斜線 /，避免反斜線的跳脫問題。
```

### ❌ 錯誤 2：Hook 指令有錯但不知道

```bash
❌ 常見情況：
Hook 設定了 ""dotnet biuld""                           # 打錯字：biuld → build
→ 每次 AI 完成都會跳出錯誤                              # 但你不知道為什麼
→ ""command not found"" 但你以為是 AI 的問題             # 其實是打字錯誤

✅ 正確做法：
1. 先手動在終端機測試 Hook 指令                          # 確認指令可以正常執行
2. 一個一個加 Hook，不要一次加太多                       # 方便找出問題
3. 看 Claude Code 的輸出 log                            # 會顯示 Hook 執行結果
```

### ❌ 錯誤 3：Skill 寫太模糊，AI 每次做出來都不一樣

```markdown
❌ 模糊的 SKILL.md：
---
description: 建立專案
---
建一個專案                                             # 太簡短，沒有具體步驟

✅ 正確的 SKILL.md：
---
description: 建立 ASP.NET Core 8.0 MVC 專案（含 EF Core + SQLite）
---
# 建立專案步驟                                         # 清楚的標題
1. 使用 dotnet new mvc 建立專案                        # 每一步都寫清楚
2. 安裝 NuGet：EntityFrameworkCore.Sqlite              # 指定套件名稱
3. 建立 Models/Product.cs（Id, Name, Price）           # 指定 Model 欄位
4. 建立 Data/AppDbContext.cs                           # 指定檔案路徑
5. Program.cs 註冊 DbContext                           # 指定設定位置
6. dotnet ef migrations add Init                       # 指定 Migration 指令
7. dotnet ef database update                           # 指定更新指令

💡 Skill 越詳細，AI 每次執行的結果越一致。
```

---

## 本章重點整理

| 主題 | 重點 |
|------|------|
| MCP 概念 | 幫 AI 裝上手和眼睛，讓它能操作外部工具 |
| 常用 MCP | filesystem（檔案）、Chrome（瀏覽器）、memory（記憶）、Google Drive（雲端） |
| MCP 設定 | .mcp.json 放在 .claude/ 資料夾，JSON 格式 |
| Skills | 預寫好的 Prompt 模板，放在 .claude/skills/ 資料夾 |
| SKILL.md | Skill 的定義檔，要寫清楚步驟和規則 |
| Hooks | 在特定時機自動執行腳本（Start, Stop, PostToolUse） |
| 自動部署 | 用 Stop Hook 串接 build → test → commit → push |
" },

        // ── Claude Code Chapter 542 ────────────────────────────
        new() { Id=542, Category="claudecode", Order=3, Level="intermediate", Icon="🏗️", Title="不用 CMD 做專案的完整流程", Slug="build-projects-without-cmd", IsPublished=true, Content=@"
# 不用 CMD 做專案的完整流程

## 前言：為什麼可以不用 CMD？

> 💡 **比喻：自動駕駛**
> 傳統寫程式就像手排車——你要自己踩離合器、換檔、打方向燈。
> 用 Claude Code 就像自動駕駛——你只要說「我要去台北」，車子自己開。
>
> CMD（命令列）就是那些「手動換檔」的操作。
> 有了 Claude Code，你用中文告訴 AI 你要什麼，它會幫你處理所有 CMD 指令。

### 傳統流程 vs Claude Code 流程

```
傳統流程（需要 CMD）：
1. 打開命令提示字元                                    # 要知道怎麼開
2. 輸入 dotnet new mvc -n MyProject                    # 要記指令
3. 輸入 cd MyProject                                   # 要懂路徑切換
4. 輸入 dotnet add package Microsoft.EntityFrameworkCore # 要知道套件名稱
5. 手動編輯 Program.cs                                 # 要知道寫什麼
6. 輸入 dotnet build                                   # 要記 build 指令
7. 輸入 dotnet run                                     # 要記 run 指令

Claude Code 流程（不需要記 CMD）：
1. 啟動 Claude Code                                    # 雙擊或一行指令
2. 說：""幫我建一個 ASP.NET Core MVC 專案""              # 用中文描述
3. AI 自動執行所有 CMD 指令                             # 你不用記任何指令
4. 說：""幫我跑起來看看""                                # AI 自動 dotnet run
```

---

## 用 Claude Code 建立 ASP.NET Core 專案

### 步驟 1：啟動 Claude Code

```bash
claude                                                 # 啟動 Claude Code
```

### 步驟 2：用自然語言描述需求

```
你說：
""幫我在 C:/Users/user/ 建立一個 ASP.NET Core 8.0 MVC 專案，
  專案名稱：GuitarSchool
  功能需求：
  1. 首頁有 Hero Banner 和課程精選
  2. 課程列表頁可以篩選難度（初級/中級/高級）
  3. 聯絡表單（姓名、Email、留言）
  技術需求：
  - Entity Framework Core + SQLite
  - Bootstrap 5，暖米色系（背景 #F5F1EB）
  - 響應式設計
  規則：
  - 所有註解用繁體中文
  - Controller 和 Model 名稱用英文""
```

### AI 會自動執行的操作

```
AI 的工作流程：                                        # 你不需要手動操作任何一步
1. dotnet new mvc -n GuitarSchool                      # 建立 MVC 專案
2. cd GuitarSchool                                     # 進入專案目錄
3. dotnet add package Microsoft.EntityFrameworkCore.Sqlite  # 安裝 EF Core
4. dotnet add package Microsoft.EntityFrameworkCore.Design  # 安裝設計工具
5. 建立 Models/Course.cs                               # 建立資料模型
6. 建立 Data/AppDbContext.cs                            # 建立資料庫上下文
7. 修改 Program.cs                                     # 註冊 DI 服務
8. 建立 Controllers/CourseController.cs                 # 建立控制器
9. 建立 Views/Course/Index.cshtml                       # 建立頁面
10. 修改 Views/Shared/_Layout.cshtml                    # 套用暖米色系
11. dotnet ef migrations add InitialCreate              # 建立資料庫遷移
12. dotnet ef database update                           # 更新資料庫
13. dotnet build                                        # 確認編譯通過
```

> 你只說了一段中文，AI 就執行了 13 個步驟。
> 這就是「不用 CMD」的意思——你不需要記這些指令。

---

## 用自然語言描述需求，AI 生成程式碼

### 範例 1：建立 Model

```
你說：""幫我建一個 Course Model，欄位有：               # 用中文描述你要什麼
  - Id (int, 主鍵)
  - Title (string, 課程標題)
  - Description (string, 課程介紹)
  - Level (string, 難度：Beginner/Intermediate/Advanced)
  - Price (decimal, 價格)
  - ImageUrl (string, 封面圖片網址)
  - CreatedAt (DateTime, 建立時間)""
```

AI 會自動生成：

```csharp
namespace GuitarSchool.Models;  // 命名空間，對應專案結構

public class Course  // 課程資料模型
{
    public int Id { get; set; }  // 主鍵，自動遞增
    public string Title { get; set; } = """";  // 課程標題，預設空字串
    public string Description { get; set; } = """";  // 課程介紹
    public string Level { get; set; } = ""Beginner"";  // 難度等級，預設初級
    public decimal Price { get; set; }  // 課程價格
    public string ImageUrl { get; set; } = """";  // 封面圖片網址
    public DateTime CreatedAt { get; set; } = DateTime.Now;  // 建立時間，預設現在
}
```

### 範例 2：建立 Controller

```
你說：""幫我建一個 CourseController，要有：              # 描述需要的功能
  - Index：列出所有課程，可以用 query string 篩選難度
  - Details：顯示單一課程詳情
  - 用 EF Core 讀取資料庫""
```

AI 會自動生成完整的 Controller，包含注入 DbContext、LINQ 查詢、錯誤處理。

### 範例 3：修改樣式

```
你說：""把網站的 Navbar 改成深色風格：                   # 用中文描述視覺需求
  - 背景色 #2C3E50
  - 文字白色
  - hover 時文字變成 #F39C12
  - 加上陰影效果""
```

AI 會自動修改 CSS 和 HTML，你不用知道 CSS 的語法。

---

## 用 Chrome MCP 預覽網站

> 💡 **比喻：監視器**
> Chrome MCP 就像你辦公桌上的監視器：
> - AI 改完程式碼後，自動在監視器上顯示結果
> - 你不用自己開瀏覽器、不用自己輸入網址
> - AI 可以幫你截圖、檢查畫面、甚至點擊測試

### 預覽流程

```
你說：""幫我啟動網站，然後截圖給我看""                   # 一句話搞定
  ↓
AI 執行 dotnet run                                     # 啟動網站
  ↓
AI 透過 Chrome MCP 打開 https://localhost:5001          # 自動開瀏覽器
  ↓
AI 截圖並分析畫面                                       # 自動擷取畫面
  ↓
AI 回報：""網站已啟動，首頁有 Hero Banner、                # 告訴你結果
  三張課程卡片、底部有聯絡表單。
  發現一個問題：手機版的卡片沒有正確排列。""               # 還會主動發現問題
```

### 互動式測試

```
你說：""幫我測試聯絡表單，填入測試資料然後送出""          # 用中文描述測試步驟
  ↓
AI 透過 Chrome MCP：                                   # AI 自動操作瀏覽器
1. 導航到聯絡表單頁面                                   # 打開頁面
2. 在姓名欄位輸入 ""測試用戶""                            # 填寫表單
3. 在 Email 欄位輸入 ""test@example.com""                # 填寫 Email
4. 在留言欄位輸入 ""這是測試留言""                        # 填寫留言
5. 點擊送出按鈕                                        # 提交表單
6. 截圖顯示結果                                        # 擷取結果畫面
  ↓
AI 回報：""表單送出成功，頁面顯示感謝訊息。""              # 告訴你測試結果
```

---

## 用 Railway / Azure 一鍵部署

> 💡 **比喻：寄快遞**
> 部署網站就像寄快遞：
> - 傳統方式：自己開車送到倉庫（設定伺服器、FTP 上傳）
> - Railway/Azure：叫快遞來收（推程式碼到 GitHub，平台自動部署）
> - Claude Code：連叫快遞都幫你打電話（AI 幫你設定一切）

### Railway 部署（最簡單）

```
你說：""幫我把這個專案部署到 Railway""                    # 一句話
  ↓
AI 執行的步驟：                                         # 你不需要手動操作
1. 建立 .gitignore                                     # 忽略不需要的檔案
2. 建立 Dockerfile 或 railway.json                      # 部署設定
3. git init && git add . && git commit                  # 初始化 Git
4. 建立 GitHub repository（如果需要）                    # 建立遠端倉庫
5. git push                                            # 推送程式碼
6. 連接 Railway 和 GitHub                               # 設定自動部署
```

### Azure 部署

```
你說：""幫我部署到 Azure App Service""                   # 描述部署目標
  ↓
AI 執行的步驟：                                         # 自動化流程
1. 建立 Azure 的部署設定檔                               # 設定檔案
2. 設定 publish profile                                # 發布設定
3. dotnet publish -c Release                           # 編譯發布版本
4. 透過 Azure CLI 部署                                  # 執行部署
```

---

## 環境變數設定（不碰 CMD）

> 💡 **比喻：保險箱**
> 環境變數就像保險箱裡的密碼：
> - 不寫在紙上（不寫在程式碼裡）
> - 放在安全的地方（系統環境變數或雲端平台設定）
> - 需要的時候才拿出來用

### 本地開發的環境變數

```
你說：""幫我設定資料庫連線字串，用 dotnet user-secrets""  # 用中文描述

AI 自動執行：
dotnet user-secrets init                               # 初始化密鑰管理
dotnet user-secrets set ""ConnectionStrings:Default"" ""Data Source=guitar.db""
                                                       # 儲存連線字串到安全的地方
```

### 雲端平台的環境變數

```
你說：""幫我在 Railway 設定環境變數：                     # 描述要設定的變數
  - ASPNETCORE_ENVIRONMENT = Production
  - ConnectionStrings__Default = （Railway 的資料庫連線）""

AI 會告訴你操作步驟，或直接透過 Railway CLI 設定          # 不用手動登入後台
```

### 在程式碼中讀取環境變數

```csharp
// ❌ 錯誤：把密碼寫死在程式碼裡
var connectionString = ""Server=mydb;Password=abc123"";  // 密碼會被看到

// ✅ 正確：從環境變數讀取
var connectionString = builder.Configuration  // 從設定中讀取
    .GetConnectionString(""Default"");         // 讀取名為 Default 的連線字串
```

---

## 錯誤排查：看 AI 的錯誤訊息學習

> 💡 **比喻：看醫生**
> 錯誤訊息就像身體的症狀：
> - 你不需要自己當醫生（不用自己看懂錯誤）
> - 把症狀告訴醫生（把錯誤訊息給 AI）
> - 醫生會診斷並開藥（AI 會分析並修復）
> - 但你要知道基本的健康知識（了解常見的錯誤模式）

### 常見錯誤類型

```
錯誤類型 1：編譯錯誤（Compile Error）
""error CS0246: The type or namespace 'Product' could not be found""
                                                       # 找不到 Product 這個類型
你說：""dotnet build 出現這個錯誤，幫我修""              # 把錯誤訊息給 AI
AI 會：檢查 using 語句、檢查 namespace、自動修復         # AI 分析並修復

錯誤類型 2：執行時錯誤（Runtime Error）
""System.NullReferenceException""                       # 空參考例外
你說：""跑起來出現 NullReferenceException""              # 描述問題
AI 會：找到出錯的程式碼、分析為什麼是 null、修復         # AI 自動追蹤

錯誤類型 3：資料庫錯誤（Database Error）
""SQLite Error 1: no such table: Courses""              # 找不到資料表
你說：""資料庫出錯了""                                   # 簡單描述
AI 會：檢查 Migration、重新建立資料庫、修復              # AI 處理資料庫問題
```

### 學習技巧

```
每次 AI 修完 bug，問它：                                # 把修 bug 當學習機會
""為什麼會出這個錯？以後怎麼避免？""

AI 會解釋：
1. 錯誤的原因（為什麼壞了）                             # 理解問題本質
2. 修復的邏輯（怎麼修的）                               # 學習解決方法
3. 預防的方法（以後怎麼避免）                            # 避免重複犯錯
```

---

## 完整案例：從零建立吉他教學網站

### 第一步：描述需求

```
你說：
""幫我從零開始建一個吉他教學網站 GuitarSchool，          # 完整的需求描述
  用 ASP.NET Core 8.0 MVC。

  功能：
  1. 首頁：Hero Banner + 精選課程（3 張卡片）
  2. 課程列表：所有課程，可篩選難度
  3. 課程詳情：課程介紹 + 報名按鈕
  4. 聯絡我們：姓名、Email、留言的表單

  技術：
  - SQLite + Entity Framework Core
  - Bootstrap 5
  - 暖米色系（背景 #F5F1EB，主色 #8B6914）

  規則：
  - 繁體中文註解
  - 響應式設計（手機要能看）
  - Seed Data 要有 6 門範例課程""
```

### 第二步：AI 開始工作

```
AI 會自動完成所有步驟：                                 # 你只需要看著

✅ 建立專案結構                                        # dotnet new mvc
✅ 安裝 NuGet 套件                                     # EF Core, SQLite
✅ 建立 Course Model                                   # 資料模型
✅ 建立 AppDbContext                                   # 資料庫上下文
✅ 建立 Seed Data                                      # 6 門範例課程
✅ 設定 Program.cs                                     # DI 註冊
✅ 建立 CourseController                               # CRUD 控制器
✅ 建立 Views（首頁、列表、詳情、聯絡）                  # 所有頁面
✅ 套用 Bootstrap 5 + 暖米色系                          # 樣式設計
✅ 建立 Migration + 更新資料庫                          # 資料庫初始化
✅ dotnet build 確認編譯通過                            # 品質檢查
```

### 第三步：預覽和微調

```
你說：""跑起來給我看看""                                 # 啟動預覽
AI 執行 dotnet run + Chrome MCP 截圖                    # 自動預覽

你說：""首頁的卡片間距太小，加大一點""                     # 微調樣式
AI 修改 CSS → 重新截圖                                 # 即時修改

你說：""聯絡表單送出後要顯示成功訊息""                    # 加新功能
AI 加上 TempData + 成功提示 → 測試                     # 快速迭代

你說：""幫我部署到 Railway""                             # 一鍵部署
AI 設定 Dockerfile + git push + 部署                   # 自動部署上線
```

### 第四步：持續迭代

```
部署完成後，你隨時可以加功能：                           # 持續改進

你說：""加一個學生評價功能""                              # 新功能
→ AI 建 Review Model + Controller + View               # 自動完成

你說：""首頁加上最新評價區塊""                            # 整合功能
→ AI 修改首頁，加入評價輪播                              # 自動修改

你說：""重新部署""                                       # 更新上線
→ AI 執行 git push → Railway 自動部署                   # 一句話更新
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：需求描述太模糊，AI 做出不預期的結果

```
❌ 你說：""幫我做一個網站""                              # 太模糊
→ AI 可能做出一個只有 Hello World 的空白頁面              # 不是你要的

✅ 正確做法：
""用 ASP.NET Core 8.0 MVC 做吉他教學網站，              # 指定技術和主題
  功能：首頁、課程列表、聯絡表單，                        # 指定功能
  Bootstrap 5，暖米色系（#F5F1EB），                     # 指定風格
  SQLite + EF Core，繁體中文註解""                       # 指定技術細節

💡 需求描述的四個要素：技術棧、功能、風格、規則。
   缺一個，AI 就可能做錯一部分。
```

### ❌ 錯誤 2：一次給 AI 太多需求，結果一塌糊塗

```
❌ 你說：
""幫我做一個網站，要有會員系統、購物車、金流、
  後台管理、聊天室、通知系統、搜尋引擎...""               # 一次太多功能
→ AI 會崩潰或做出一堆有 bug 的程式碼                     # 品質無法保證

✅ 正確做法：分批進行
第一批：""先做首頁和課程列表""                            # 核心功能
→ 確認沒問題，commit

第二批：""加上聯絡表單""                                  # 次要功能
→ 確認沒問題，commit

第三批：""加上會員註冊""                                  # 進階功能
→ 確認沒問題，commit

💡 一次一個功能，每次都 build + 測試 + commit。
   這叫「迭代開發」，是業界標準做法。
```

### ❌ 錯誤 3：部署前沒有檢查環境變數

```
❌ 常見情況：
本地開發用 SQLite，部署到 Railway 也用 SQLite             # 本地沒問題
但 Railway 每次重新部署會清空檔案系統                      # SQLite 檔案被刪除
→ 資料全部消失                                           # 上線後資料不見了

✅ 正確做法：
1. 本地開發用 SQLite（方便）                              # 開發環境
2. 部署時用 PostgreSQL（Railway 提供免費方案）             # 生產環境
3. 用環境變數切換連線字串                                  # 不用改程式碼
4. 告訴 AI：""幫我設定環境變數切換資料庫""                  # 讓 AI 處理

💡 開發環境和生產環境的設定應該不同，
   用環境變數來切換是標準做法。
```

---

## 本章重點整理

| 步驟 | 重點 |
|------|------|
| 建立專案 | 用自然語言描述需求，AI 自動執行所有 CMD 指令 |
| 描述需求 | 要具體：技術棧 + 功能 + 風格 + 規則 |
| 預覽網站 | 用 Chrome MCP 截圖預覽，不用自己開瀏覽器 |
| 部署上線 | Railway 或 Azure，一句話就能部署 |
| 環境變數 | 用 user-secrets（本地）或平台設定（雲端） |
| 錯誤排查 | 把錯誤訊息給 AI，順便學習錯誤原因 |
| 迭代開發 | 一次一個功能，每次 build + test + commit |
" },

        // ── Claude Code Chapter 543 ────────────────────────────
        new() { Id=543, Category="claudecode", Order=4, Level="beginner", Icon="🧰", Title="No-Code / Low-Code 工具比較", Slug="no-code-low-code-comparison", IsPublished=true, Content=@"
# No-Code / Low-Code 工具比較

## No-Code vs Low-Code vs Full-Code 定義

> 💡 **比喻：蓋房子**
> - **No-Code** = 買現成的組合屋。你選款式、選顏色，工人幫你組裝。
>   你完全不用懂建築，但能客製化的程度有限。
> - **Low-Code** = 半自建房。基礎結構有模板，但你可以改隔間、加陽台。
>   需要懂一點建築知識，但比從零蓋快很多。
> - **Full-Code** = 完全自建房。從打地基開始，一磚一瓦自己來。
>   可以蓋任何你想要的，但需要完整的建築知識和時間。

### 三者的差異

```
特性            No-Code         Low-Code         Full-Code
──────────────────────────────────────────────────────────────
需要寫程式嗎？  完全不用         少量             大量
學習門檻        極低             中等             高
客製化程度      低               中高             完全自由
開發速度        最快             快               慢
維護成本        低               中               高
效能控制        有限             部分             完全
適合場景        簡單網站/MVP     企業應用         複雜系統
代表工具        Bubble, Webflow  Retool, PowerApps ASP.NET, React
```

### 你該選哪一種？

```
問自己三個問題：                                       # 簡單的決策流程

1. 你的需求複雜嗎？
   簡單（Landing Page、表單）→ No-Code               # 用最簡單的工具
   中等（會員系統、後台管理）→ Low-Code               # 需要一些程式邏輯
   複雜（金流、即時通訊、大量數據）→ Full-Code        # 需要完全控制

2. 你有多少時間？
   一天內完成 → No-Code                              # 最快上線
   一週內完成 → Low-Code                             # 有時間微調
   一個月以上 → Full-Code                            # 精雕細琢

3. 你的預算是？
   免費或很低 → No-Code（有免費方案）                 # 省錢
   中等 → Low-Code（部分需要付費）                    # 合理投資
   有預算 → Full-Code（可能需要雲端主機）              # 完全控制
```

---

## 常見工具比較

### No-Code 工具

#### 1. Bubble

```
定位：No-Code 網頁應用開發平台                        # 可以做完整的 Web App
優點：
- 不用寫任何程式碼                                    # 真正的零程式碼
- 拖拉式介面設計                                      # 像在用 PowerPoint
- 內建資料庫和使用者系統                                # 不用另外設定
- 可以做出複雜的邏輯流程                                # 視覺化邏輯編輯器

缺點：
- 效能有上限                                          # 大量使用者時會慢
- 離開平台很困難（vendor lock-in）                     # 搬家很痛苦
- 複雜邏輯用視覺化編輯器反而更麻煩                      # 有時寫程式比較快
- 付費方案較貴                                        # 商用需要升級

適合：MVP 驗證、小型商業應用、個人專案                   # 快速測試商業想法
```

#### 2. Webflow

```
定位：No-Code 網站設計工具                             # 專注於視覺設計
優點：
- 設計自由度極高（接近手寫 CSS）                       # 設計師最愛
- 產生的 HTML/CSS 品質好                              # 效能不錯
- 內建 CMS（內容管理系統）                             # 可以管理文章
- SEO 友善                                           # 搜尋引擎優化

缺點：
- 不適合做複雜的後端邏輯                               # 主要是前端工具
- 動態功能有限                                        # 需要整合其他服務
- 學習曲線比其他 No-Code 工具稍高                      # 需要懂設計概念

適合：品牌官網、作品集、行銷 Landing Page                # 以設計為主的網站
```

### Low-Code 工具

#### 3. Retool

```
定位：Low-Code 內部工具開發平台                        # 做企業後台超快
優點：
- 快速建立管理後台                                     # 拖拉式介面
- 可以直接連接資料庫                                    # 支援 PostgreSQL、MySQL
- 支援 JavaScript 客製化                               # 需要時可以寫程式
- 內建大量元件（表格、圖表、表單）                       # 現成的 UI 組件

缺點：
- 主要做內部工具，不適合面向使用者                       # 不是做公開網站的
- 付費方案較貴                                         # 商用價格高
- 設計自由度有限                                       # 長得都差不多

適合：企業內部管理系統、資料看板、客服後台                # B2B 內部工具
```

#### 4. Power Apps

```
定位：微軟的 Low-Code 開發平台                         # 微軟生態系整合
優點：
- 和 Microsoft 365 深度整合                            # 用 Excel 當資料庫
- 企業級安全性和權限管理                                # 大公司的首選
- 可以做手機 App                                      # 跨平台支援
- 有 Power Automate 自動化流程                         # 搭配自動化很強

缺點：
- 介面設計比較陽春                                     # 不太好看
- 效能有限制                                          # 大量數據時會慢
- 需要 Microsoft 365 授權                              # 要付費
- 學習曲線比想像中陡                                    # 功能太多反而複雜

適合：已經用 Microsoft 365 的企業                       # 善用現有工具
```

---

## AI 輔助開發工具

> 💡 **比喻：交通工具**
> - **Copilot** = 副駕駛。你在開車，它幫你看路、提建議。
> - **Cursor** = 高級自動駕駛。大部分路段自動開，你偶爾接手。
> - **Claude Code** = 專屬司機。你說目的地，它幫你開到。
> - **v0** = 計程車。你說要什麼，它直接載你到。

### GitHub Copilot

```
定位：程式碼自動補全工具                               # 寫程式時即時建議
運作方式：你打字 → 它猜你要寫什麼 → 按 Tab 接受        # 像手機打字預測

優點：
- 深度整合在 VS Code / Visual Studio                   # 不用離開編輯器
- 即時補全，速度極快                                    # 打幾個字就能補完
- 支援幾乎所有程式語言                                 # 通用性高

缺點：
- 只能補全程式碼，不能執行指令                          # 不能跑 dotnet build
- 不能讀取整個專案結構                                  # 上下文有限
- 不能操作檔案系統或瀏覽器                              # 功能單一

適合：已經會寫程式，想要加速的開發者                     # 進階使用者
```

### Cursor

```
定位：AI 驅動的程式碼編輯器                            # VS Code 的 AI 強化版
運作方式：在編輯器內和 AI 對話 → AI 直接改程式碼        # 聊天式開發

優點：
- 可以讀取整個專案結構                                  # 理解完整脈絡
- 可以直接修改多個檔案                                  # 批次修改
- 有內建的 AI Chat 和 Composer 功能                    # 多種互動方式
- 介面直覺，適合視覺型使用者                            # 圖形化介面

缺點：
- 不能執行終端機指令                                    # 不能跑 build
- 不能操作瀏覽器                                       # 不能預覽網站
- 免費版有使用限制                                     # 高頻使用需付費

適合：想要視覺化 AI 開發體驗的人                        # 介面友善
```

### Claude Code

```
定位：CLI 版的 AI 全端開發工具                         # 功能最完整
運作方式：在終端機和 AI 對話 → AI 讀檔、改檔、跑指令    # 全自動化

優點：
- 可以讀寫檔案、執行指令、操作瀏覽器                    # 功能最全面
- MCP 擴充系統無限擴充                                  # 可以裝各種外掛
- CLAUDE.md 持久化記憶                                 # 跨 Session 記住規則
- Skills 和 Hooks 自動化                               # 自動化流程

缺點：
- 需要用終端機介面                                     # 沒有圖形化介面
- 需要 API Key（按使用量計費）                          # 不是免費的
- 對新手來說終端機可能不太習慣                           # 需要適應

適合：想要完全自動化開發流程的人                         # 追求效率的開發者
```

### v0 (by Vercel)

```
定位：AI 生成前端 UI 的工具                            # 專注於畫面生成
運作方式：描述你要的畫面 → AI 生成 React 元件           # 一句話出畫面

優點：
- 專精於前端 UI 生成                                   # 畫面品質很高
- 生成的程式碼可以直接用                                # 複製貼上就能跑
- 支援 React / Next.js                                # 現代前端框架
- 有即時預覽功能                                       # 馬上看到結果

缺點：
- 只做前端，不做後端                                   # 需要自己處理後端
- 只支援 React 生態系                                  # 不支援 ASP.NET
- 免費版有使用次數限制                                  # 高頻使用需付費

適合：需要快速做出漂亮前端的人                          # 前端為主的專案
```

### 工具比較總表

```
工具          能讀專案  能改檔案  能跑指令  能看網頁  需要寫程式
─────────────────────────────────────────────────────────────
Copilot       部分     建議    ❌       ❌       ✅ 必須
Cursor        ✅      ✅     ❌       ❌       ✅ 部分
Claude Code   ✅      ✅     ✅       ✅       ❌ 可不用
v0            ❌      ❌     ❌       ✅ 預覽  ❌ 可不用
```

---

## 什麼時候該用 No-Code vs 寫程式

### 決策流程圖

```
你的需求是什麼？
│
├── 簡單的靜態網站（Landing Page、作品集）
│   └── → Webflow 或 No-Code 工具                     # 最快最省
│
├── 需要使用者登入、資料庫
│   ├── 資料量小、邏輯簡單
│   │   └── → Bubble 或 Low-Code                      # 夠用就好
│   └── 資料量大、邏輯複雜
│       └── → Full-Code（Claude Code 輔助）            # 需要完全控制
│
├── 企業內部工具
│   ├── 用 Microsoft 365 的公司
│   │   └── → Power Apps                              # 生態系整合
│   └── 其他
│       └── → Retool                                  # 快速建後台
│
└── 需要高效能、高客製化
    └── → Full-Code（Claude Code 輔助）                # 沒有替代方案
```

### 實際案例判斷

```
案例 1：個人部落格
→ No-Code（Webflow）                                  # 不用寫程式
理由：靜態內容，不需要複雜邏輯                          # 簡單需求用簡單工具

案例 2：小型電商（< 100 個商品）
→ No-Code（Shopify）                                  # 專門的電商工具
理由：金流和物流都有現成方案                             # 不用自己做

案例 3：客製化學習平台
→ Low-Code + AI（Claude Code）                        # 需要一些客製化
理由：需要自訂學習進度邏輯，但 UI 可以用模板              # 混合策略

案例 4：大型企業 ERP 系統
→ Full-Code                                           # 完全客製化
理由：複雜的業務邏輯、大量資料、高安全性需求              # 不能妥協
```

---

## 混合策略：No-Code 前端 + API 後端

> 💡 **比喻：餐廳分工**
> - **No-Code 前端** = 外場服務生。面對客人、點餐、送餐。
>   不需要會煮菜，只要會接待客人。
> - **API 後端** = 內場廚師。在廚房裡準備食材、烹飪。
>   不需要面對客人，專心把菜做好。
> - 兩者透過「出菜口」（API）溝通。

### 混合架構圖

```
使用者（瀏覽器）
    │
    ▼
No-Code 前端（Webflow / Bubble）                      # 負責畫面和互動
    │
    │  透過 API 呼叫
    ▼
ASP.NET Core Web API 後端                             # 負責邏輯和資料
    │
    ▼
資料庫（PostgreSQL / SQLite）                          # 負責儲存資料
```

### 為什麼要混合？

```
純 No-Code 的限制：                                    # 簡單但有天花板
- 複雜的業務邏輯做不了                                  # 邏輯太複雜
- 資料處理效能不夠                                     # 資料量太大
- 安全性無法完全控制                                    # 無法自訂安全機制

純 Full-Code 的成本：                                  # 強大但花時間
- 前端 UI 要花很多時間                                  # 刻畫面很慢
- 設計能力要求高                                       # 不是每個人都會設計
- 維護成本高                                          # 什麼都要自己來

混合策略的優勢：                                       # 兩全其美
- 前端用 No-Code 快速做出漂亮畫面                       # 省時間
- 後端用 Full-Code 處理複雜邏輯                         # 有完全控制權
- 各自發揮所長                                        # 效率最高
```

### 實作範例

```
前端（Webflow）：                                      # 拖拉式設計
- 設計精美的課程列表頁面                                # 不用寫 HTML/CSS
- 表單送出時呼叫後端 API                               # 透過 Webflow 的整合功能

後端（ASP.NET Core Web API）：                         # Claude Code 輔助開發
- 課程 CRUD API                                       # 資料操作
- 使用者驗證                                          # 安全性
- 付款處理                                            # 複雜邏輯
```

```csharp
// 後端 API Controller 範例
[ApiController]  // 標記為 API 控制器
[Route(""api/[controller]"")]  // 設定路由前綴
public class CoursesController : ControllerBase  // 課程 API 控制器
{
    private readonly AppDbContext _context;  // 資料庫上下文

    public CoursesController(AppDbContext context)  // 建構式注入
    {
        _context = context;  // 儲存資料庫上下文
    }

    [HttpGet]  // 處理 GET 請求
    public async Task<IActionResult> GetAll()  // 取得所有課程
    {
        var courses = await _context.Courses.ToListAsync();  // 從資料庫讀取所有課程
        return Ok(courses);  // 回傳 200 OK 和課程資料
    }

    [HttpGet(""{id}"")]  // 處理 GET /api/courses/{id} 請求
    public async Task<IActionResult> GetById(int id)  // 依 ID 取得課程
    {
        var course = await _context.Courses.FindAsync(id);  // 查找指定 ID 的課程
        if (course == null) return NotFound();  // 找不到就回傳 404
        return Ok(course);  // 找到就回傳課程資料
    }
}
```

---

## 未來趨勢：AI 時代的開發者角色

### 開發者角色的轉變

```
過去的開發者：                                         # 傳統模式
- 角色：打字員（把邏輯打成程式碼）                       # 重點在「寫」
- 技能：熟記語法、框架 API、設計模式                     # 記憶型技能
- 工作：8 小時有 6 小時在打字                           # 產出 = 打字速度

現在的開發者：                                         # 轉型中
- 角色：指揮官（告訴 AI 要做什麼）                       # 重點在「想」
- 技能：需求分析、系統設計、品質審核                     # 思考型技能
- 工作：8 小時有 6 小時在思考和審核                      # 產出 = 思考品質

未來的開發者：                                         # 趨勢預測
- 角色：架構師 + 品管                                   # 重點在「審」
- 技能：商業理解、使用者體驗、AI 協作                    # 綜合型技能
- 工作：定義需求、審核 AI 產出、確保品質                  # 產出 = 決策品質
```

### 不會被 AI 取代的技能

```
1. 商業邏輯理解                                        # AI 不懂你的商業模式
   ""這個功能該不該做？值不值得投資？""                   # 需要商業判斷

2. 使用者體驗設計                                       # AI 不懂人的感受
   ""使用者會怎麼用這個介面？哪裡會卡住？""               # 需要同理心

3. 系統架構決策                                        # AI 不知道你的限制
   ""用 Microservice 還是 Monolith？""                  # 需要經驗和判斷

4. 安全性思維                                          # AI 可能忽略安全漏洞
   ""這個 API 有沒有被攻擊的風險？""                     # 需要安全意識

5. 程式碼審查能力                                      # AI 寫的不一定對
   ""這段程式碼有沒有隱藏的 bug？""                      # 需要批判性思維

6. 溝通和團隊協作                                      # AI 不能取代人際互動
   ""如何和設計師、PM 溝通需求？""                       # 需要軟實力
```

### 給初學者的建議

```
1. 不要害怕 AI 取代你                                  # AI 是工具，不是對手
   → AI 取代的是「打字」，不是「思考」                   # 你的價值在腦袋

2. 基礎還是要學                                        # 不能完全依賴 AI
   → 至少要看得懂 AI 寫的程式碼                          # 不然怎麼審查？
   → 至少要知道基本的架構概念                            # 不然怎麼設計？
   → 至少要會 Debug（AI 也會寫錯）                       # 不然怎麼修？

3. 善用 AI 學習                                        # 把 AI 當最好的老師
   → 每次 AI 寫完，問它：""為什麼這樣寫？""               # 學習最佳實踐
   → 每次 AI 修完 bug，問它：""為什麼會錯？""              # 學習避免錯誤
   → 每次 AI 選了某個架構，問它：""為什麼選這個？""         # 學習決策邏輯

4. 掌握 AI 協作技巧                                    # 新時代的必備技能
   → 學會寫好的 Prompt（需求描述）                       # 溝通的藝術
   → 學會審核 AI 的產出                                  # 品管的能力
   → 學會迭代開發流程                                    # 效率的方法
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：用 No-Code 做需要複雜後端邏輯的專案

```
❌ 常見情況：
用 Bubble 做一個需要即時通訊的社交平台                    # No-Code 做不好的事
→ 效能不夠，使用者多了就卡                               # 技術限制
→ 想加自訂演算法，但 Bubble 做不到                       # 客製化限制
→ 想搬到自己的伺服器，但程式碼帶不走                      # vendor lock-in

✅ 正確做法：
1. 先想清楚需求的複雜度                                  # 評估再決定
2. 簡單需求 → No-Code                                  # 合適的工具
3. 中等需求 → Low-Code 或 AI 輔助                       # 彈性方案
4. 複雜需求 → Full-Code + AI 輔助                       # 完全控制

💡 選工具要看需求，不是看哪個最流行。
```

### ❌ 錯誤 2：以為 AI 可以取代學習程式

```
❌ 錯誤觀念：
""有了 AI 就不用學程式了""                               # 危險的想法
→ AI 寫了一段有 bug 的程式碼                             # AI 不是完美的
→ 你看不懂，不知道有 bug                                # 因為你沒學過
→ 上線後出問題，你不知道怎麼修                           # 沒有能力處理
→ 損失慘重                                             # 最後果

✅ 正確觀念：
""AI 讓我學得更快、做得更多""                             # 健康的心態
→ 用 AI 寫程式碼，同時學習每一段的意思                    # 邊做邊學
→ 看得懂 AI 寫的程式碼，能判斷對不對                     # 品質控制
→ 出問題時知道方向在哪                                   # 有能力處理

💡 AI 是加速器，不是替代品。基礎知識是你的安全網。
```

### ❌ 錯誤 3：一開始就選最複雜的工具

```
❌ 常見情況：
初學者想做一個個人部落格                                  # 簡單的需求
→ 直接學 ASP.NET Core + React + Docker + Kubernetes     # 超級複雜的技術棧
→ 學了三個月還沒做出來                                   # 學不完
→ 放棄了                                               # 太挫折

✅ 正確做法：
1. 先用 No-Code 工具做出來（1 天）                       # 快速實現
2. 覺得需要更多功能 → 用 AI 輔助寫簡單的後端             # 漸進式學習
3. 有基礎了 → 學更進階的技術                             # 循序漸進
4. 最終：你懂原理，也會用工具                            # 全方位能力

💡 從最簡單的工具開始，需要時再升級。
   這叫「漸進式複雜化」，比一步到位更有效。
```

---

## 本章重點整理

| 主題 | 重點 |
|------|------|
| No-Code | 不用寫程式，快速做簡單網站和 MVP |
| Low-Code | 少量程式碼，適合企業內部工具 |
| Full-Code | 完全控制，適合複雜和高效能需求 |
| AI 輔助工具 | Copilot（補全）、Cursor（編輯器）、Claude Code（全能）、v0（前端） |
| 選擇策略 | 根據需求複雜度、時間、預算來決定 |
| 混合策略 | No-Code 前端 + API 後端，各取所長 |
| 未來趨勢 | 開發者從「打字員」轉變為「指揮官」和「品管」 |
| 學習建議 | AI 是加速器不是替代品，基礎知識仍然重要 |
" },
    };
}