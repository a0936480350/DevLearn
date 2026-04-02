# DevLearn Claude Context

## 專案概要
DevLearn 是 ASP.NET Core MVC .NET 8 學習平台。
- 本地路徑：`C:\Users\user\DotNetLearning\DotNetLearning\`
- 網站：https://devlearn-dotnet.azurewebsites.net
- 資料庫：PostgreSQL (devlearn-pg.postgres.database.azure.com, DB: devlearn)
- 部署：Azure App Service (devlearn-dotnet, devlearn-rg, East Asia)

## 部署指令（在 ~/DotNetLearning 目錄執行）
```
dotnet publish DotNetLearning/DotNetLearning.csproj -c Release -o publish-out --nologo
python -c "import zipfile,os; z=zipfile.ZipFile('deploy-new.zip','w',zipfile.ZIP_DEFLATED); [z.write(os.path.join(r,f),os.path.relpath(os.path.join(r,f),'publish-out')) for r,d,fs in os.walk('publish-out') for f in fs]; z.close(); print('done')"
az webapp deploy --resource-group devlearn-rg --name devlearn-dotnet --src-path "C:/Users/user/DotNetLearning/deploy-new.zip" --type zip --async true
```
驗證：`python -c "import urllib.request,json,subprocess; t=json.loads(subprocess.run(['C:/Program Files/Microsoft SDKs/Azure/CLI2/wbin/az.cmd','account','get-access-token','--output','json'],capture_output=True,text=True).stdout)['accessToken']; r=urllib.request.urlopen(urllib.request.Request('https://devlearn-dotnet.scm.azurewebsites.net/api/deployments/latest',headers={'Authorization':'Bearer '+t}),timeout=30); print(json.loads(r.read()))"`

## 主要功能模組
Account, Admin, Analytics, Arena, Buddy, Battle, CheckIn, Daily, Detective, Flashcard, Home, Idea, Leaderboard, Message, Note, Puzzle, QnA, Quiz, Scores, Snippet, Speed, Support, Teacher, ClaudeTask

## 已完成功能（2026-04）
- 對戰系統：PvP即時+AI對戰（初/中/高）、55題、SignalR BattleHub、戰績積分
- 首頁公告欄：類型/釘選/到期日，Admin CRUD，✕只是localStorage dismiss
- 語音辨識：「選擇XX輸入框」「輸入XXX」「在XX輸入YYY」「清空」
- Admin 浮動語音按鈕（所有 Layout=null 的 Admin 頁）
- Admin 側欄 Scrollbar（height:100vh）
- 手機指令佇列：/claude-task（密碼:devlearn2026）

## 關鍵檔案
- Models/LogModels.cs：所有 Model（含 ClaudeTask, Announcement, BattleRecord 等）
- Data/AppDbContext.cs：所有 DbSet
- Program.cs：DB 建表 SQL + 種子資料 + middleware
- Hubs/BattleHub.cs：SignalR 對戰邏輯
- wwwroot/js/site.js：translations + changeLang() + voice recognition
- Views/Shared/_Layout.cshtml：全域 layout + 語音辨識 JS（~1600行後）

## Admin
- URL: /Admin/Login
- 密碼在 Program.cs 裡的 AdminPassword 設定

## 技術注意事項
- `Context.Session` not `HttpContext.Session` in Razor views
- 所有 Admin 頁用 `Layout = null`（獨立 HTML）
- PostgreSQL 用 `"雙引號"` 包欄位名
- az CLI exit 1 但 Kudu Status=4 = 部署成功
- python3 在此環境 exit 49，要用 python（不加3）

## 新 session 快速啟動 prompt
```
繼續開發 DevLearn 專案（C:\Users\user\DotNetLearning）。
讀 C:\Users\user\.claude\projects\C--Users-user\memory\project_devlearn.md 取得完整背景。
任務：[在這裡填入需求]
```
