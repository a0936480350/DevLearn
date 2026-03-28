using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Git
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Git Chapter 510 ────────────────────────────
        new() { Id=510, Category="git", Order=1, Level="beginner", Icon="📝", Title="Git 基礎：版本控制入門", Slug="git-basics", IsPublished=true, Content=@"
# Git 基礎：版本控制入門

## 為什麼需要版本控制？

> 💡 **比喻：遊戲存檔點**
> 想像你在玩 RPG 遊戲，每到一個關鍵時刻都會「存檔」，
> 如果打輸了 Boss，可以回到存檔點重來。
> Git 就是你程式碼的「存檔系統」——
> 每次 commit 就像按下存檔鍵，
> 隨時可以回到任何一個存檔點。

### 沒有版本控制的悲劇

```
專案資料夾/
├── MyProject_v1.zip          # 第一版
├── MyProject_v2.zip          # 改了登入功能
├── MyProject_v2_final.zip    # 老闆說的最終版
├── MyProject_v2_final2.zip   # 真的最終版
├── MyProject_v2_真的最終版.zip  # 拜託這次是真的
└── MyProject_別再改了.zip      # 你已經崩潰了
```

### 有了 Git 之後

```bash
git log --oneline  # 查看所有版本紀錄
# a1b2c3d 新增登入功能
# e4f5g6h 修復首頁 Bug
# i7j8k9l 初始化專案

git checkout a1b2c3d  # 回到任何版本，就像讀取存檔
```

---

## 安裝與設定 Git

### 安裝 Git

```bash
# Windows：從官網下載安裝程式
# https://git-scm.com/download/win

# macOS：使用 Homebrew 安裝
brew install git  # 透過 Homebrew 套件管理器安裝 Git

# Linux（Ubuntu/Debian）：使用 apt 安裝
sudo apt install git  # 透過 apt 套件管理器安裝 Git

# 確認安裝成功
git --version  # 顯示 Git 版本號，確認安裝成功
```

### 初始設定（只需做一次）

```bash
# 設定使用者名稱（會出現在每次 commit 紀錄中）
git config --global user.name ""你的名字""  # 設定全域使用者名稱

# 設定電子信箱（建議用 GitHub 註冊的信箱）
git config --global user.email ""you@example.com""  # 設定全域電子信箱

# 設定預設編輯器為 VS Code
git config --global core.editor ""code --wait""  # 設定 VS Code 為預設編輯器

# 設定預設分支名稱為 main
git config --global init.defaultBranch main  # 新建 repo 時預設分支名為 main

# 查看所有設定
git config --list  # 列出目前所有 Git 設定值
```

---

## 工作區、暫存區、儲存庫

> 💡 **比喻：寄包裹的流程**
> 1. **工作區（Working Directory）** = 你的書桌，正在寫信
> 2. **暫存區（Staging Area）** = 把信放進信封，準備寄出
> 3. **儲存庫（Repository）** = 郵局收件，正式寄出紀錄

```
┌─────────────┐    git add    ┌─────────────┐   git commit   ┌─────────────┐
│   工作區     │ ──────────→  │   暫存區     │ ─────────────→ │   儲存庫     │
│ Working Dir  │              │ Staging Area │               │ Repository  │
│  （你的書桌）│              │ （信封裡）   │               │ （郵局紀錄）│
└─────────────┘              └─────────────┘               └─────────────┘
       ↑                                                          │
       └──────────────── git checkout ────────────────────────────┘
                        （讀取舊存檔）
```

### 實際操作流程

```bash
# 步驟 1：建立新專案資料夾
mkdir my-project  # 建立新資料夾
cd my-project  # 進入資料夾

# 步驟 2：初始化 Git 儲存庫
git init  # 在當前資料夾建立 .git 子資料夾，開始版本控制

# 步驟 3：建立檔案（工作區）
echo ""Hello Git"" > hello.txt  # 建立一個新檔案

# 步驟 4：查看目前狀態
git status  # 顯示哪些檔案被修改、新增、刪除

# 步驟 5：加入暫存區
git add hello.txt  # 把 hello.txt 放進暫存區（信封裡）

# 步驟 6：提交到儲存庫
git commit -m ""新增 hello.txt 檔案""  # 正式存檔，附上說明訊息
```

---

## 常用 Git 指令

### git status — 查看狀態

```bash
# 查看目前工作區和暫存區的狀態
git status  # 顯示檔案的追蹤狀態

# 簡短格式（更簡潔）
git status -s  # 用縮寫顯示狀態（M=修改, A=新增, ?=未追蹤）

# 輸出範例：
#  M README.md        ← 已修改但未暫存
# A  新增的檔案.cs    ← 已加入暫存區
# ?? 未追蹤的檔案.txt ← Git 還不認識的檔案
```

### git add — 加入暫存區

```bash
# 加入單一檔案
git add index.html  # 把 index.html 加入暫存區

# 加入多個檔案
git add file1.cs file2.cs  # 同時把兩個檔案加入暫存區

# 加入所有變更的檔案
git add .  # 把目前目錄下所有變更都加入暫存區

# 加入特定類型的檔案
git add ""*.cs""  # 把所有 .cs 檔案加入暫存區

# 互動式加入（選擇部分變更）
git add -p  # 逐段檢視變更，選擇要暫存哪些部分
```

### git commit — 提交存檔

```bash
# 基本提交（附上訊息）
git commit -m ""修復登入頁面的驗證 Bug""  # 提交暫存區的所有變更

# 多行提交訊息
git commit -m ""修復登入驗證 Bug"" -m ""詳細說明：密碼欄位未做空值檢查""  # 第二個 -m 會成為說明內文

# 跳過暫存區，直接提交所有已追蹤檔案的修改
git commit -am ""快速修復 typo""  # -a 自動暫存已追蹤的修改檔案，然後提交

# 修改最近一次的提交訊息
git commit --amend -m ""修正：登入頁面驗證 Bug""  # 覆蓋上一次的 commit 訊息
```

### git log — 查看歷史紀錄

```bash
# 查看完整紀錄
git log  # 顯示所有 commit 的詳細資訊

# 一行顯示（簡潔）
git log --oneline  # 每個 commit 只顯示一行摘要

# 圖形化顯示分支
git log --oneline --graph --all  # 用 ASCII 圖形顯示所有分支的合併歷史

# 顯示最近 5 筆
git log -5  # 只顯示最近 5 次的 commit

# 顯示每次 commit 修改了哪些檔案
git log --stat  # 列出每次 commit 影響的檔案和行數統計

# 搜尋特定作者的 commit
git log --author=""小明""  # 只顯示作者名稱包含「小明」的 commit
```

---

## .gitignore 設定

> 💡 **比喻：搬家時的「不帶清單」**
> 搬家不會把垃圾桶、用過的衛生紙一起搬走，
> .gitignore 就是告訴 Git 哪些檔案不需要追蹤。

### .NET 專案常用 .gitignore

```bash
# 建立 .gitignore 檔案
touch .gitignore  # 建立空的 .gitignore 檔案

# 使用 dotnet CLI 自動產生（推薦）
dotnet new gitignore  # 自動產生適合 .NET 專案的 .gitignore
```

### 常用規則範例

```gitignore
# === 編譯輸出 ===
bin/          # 忽略編譯輸出資料夾
obj/          # 忽略中間編譯檔案資料夾
*.dll         # 忽略所有動態連結庫檔案
*.exe         # 忽略所有可執行檔

# === IDE 設定 ===
.vs/          # 忽略 Visual Studio 設定資料夾
.vscode/      # 忽略 VS Code 設定資料夾（視情況）
*.suo          # 忽略 Visual Studio 使用者選項檔
*.user         # 忽略使用者特定設定檔

# === 敏感資訊 ===
appsettings.Development.json  # 忽略開發環境的敏感設定
*.pfx          # 忽略憑證檔案
secrets.json   # 忽略機密設定檔

# === 系統檔案 ===
.DS_Store      # 忽略 macOS 系統檔案
Thumbs.db      # 忽略 Windows 縮圖快取

# === 套件 ===
node_modules/  # 忽略 Node.js 套件資料夾（前端專案）
packages/      # 忽略 NuGet 套件資料夾

# === 日誌 ===
*.log          # 忽略所有日誌檔案
logs/          # 忽略日誌資料夾
```

### 已經追蹤的檔案如何移除

```bash
# 如果檔案已經被 Git 追蹤了，光加 .gitignore 沒用
# 需要先從追蹤清單中移除

git rm --cached appsettings.Development.json  # 從追蹤清單移除，但保留本地檔案
git commit -m ""移除敏感設定檔的追蹤""  # 提交這個變更
```

---

## VS Code 與 Visual Studio 的 Git 整合

### VS Code Git 整合

```
VS Code 左側邊欄：
┌──────────────────────────┐
│ 📁 Explorer              │ ← 檔案總管
│ 🔍 Search                │ ← 搜尋
│ 🔀 Source Control (Ctrl+Shift+G) │ ← Git 面板 ⭐
│ 🐛 Run and Debug         │ ← 除錯
│ 📦 Extensions            │ ← 擴充套件
└──────────────────────────┘

Source Control 面板功能：
- 查看所有變更的檔案（相當於 git status）
- 點擊 + 號暫存檔案（相當於 git add）
- 輸入訊息後按 ✓ 提交（相當於 git commit）
- 底部狀態列顯示目前分支名稱
```

### 推薦 VS Code 擴充套件

```
Git 相關擴充套件：
┌──────────────────────────────────────────────┐
│ GitLens           │ 顯示每行程式碼的作者和時間 │
│ Git Graph         │ 圖形化顯示分支和合併歷史   │
│ Git History       │ 查看檔案的修改歷史         │
│ GitHub PR         │ 在 VS Code 中管理 PR       │
└──────────────────────────────────────────────┘
```

### Visual Studio Git 整合

```
Visual Studio 的 Git 功能：
┌────────────────────────────────────────────────┐
│ Git > Changes（Git 變更視窗）                  │
│  - 相當於 git status + git add + git commit    │
│                                                │
│ Git > Manage Branches（管理分支）               │
│  - 圖形化建立、切換、合併分支                   │
│                                                │
│ View > Git Repository（Git 儲存庫視窗）         │
│  - 查看完整的 commit 歷史和分支圖               │
│                                                │
│ 右鍵 > Git > View History                      │
│  - 查看單一檔案的修改歷史                       │
└────────────────────────────────────────────────┘
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：忘記設定使用者資訊就 commit

```bash
# 錯誤：沒設定 user.name 和 user.email 就提交
git commit -m ""第一次提交""  # 會出錯或顯示預設的不正確資訊

# ✅ 正確：先設定使用者資訊
git config --global user.name ""你的名字""  # 先設定名稱
git config --global user.email ""you@example.com""  # 再設定信箱
git commit -m ""第一次提交""  # 然後才提交
```

**為什麼錯？** Git 需要知道「誰」做了這次變更。就像寄信不寫寄件人，郵局不知道是誰寄的。

### ❌ 錯誤 2：把敏感資訊提交到 Git

```bash
# 錯誤：把含有密碼的設定檔提交了
git add appsettings.Development.json  # 這個檔案可能包含資料庫密碼
git commit -m ""新增設定""  # 密碼就被記錄到歷史中了

# ✅ 正確：先設定 .gitignore
echo ""appsettings.Development.json"" >> .gitignore  # 先加入忽略清單
git add .gitignore  # 暫存 .gitignore
git commit -m ""新增 .gitignore""  # 提交忽略規則
```

**為什麼錯？** 即使後來刪除檔案，Git 歷史紀錄中仍然保留密碼。推上 GitHub 後全世界都看得到！

### ❌ 錯誤 3：不看 status 就 add .

```bash
# 錯誤：不檢查就全部加入
git add .  # 可能把暫時檔案、測試資料、甚至密鑰都加進去了
git commit -m ""更新""  # 模糊的 commit 訊息

# ✅ 正確：先看狀態，再選擇性加入
git status  # 先檢查有哪些變更
git add src/LoginController.cs  # 只加入需要的檔案
git commit -m ""修復登入頁面的密碼驗證邏輯""  # 寫清楚做了什麼
```

**為什麼錯？** `git add .` 會把所有東西都加進去，可能包含不該追蹤的檔案。好的習慣是先 `git status`，確認變更內容後再有選擇地 `git add`。

---

## 📝 本章重點整理

| 指令 | 用途 | 比喻 |
|------|------|------|
| `git init` | 初始化儲存庫 | 買一本新的筆記本 |
| `git add` | 加入暫存區 | 把信放進信封 |
| `git commit` | 提交到儲存庫 | 把信交給郵局 |
| `git status` | 查看狀態 | 檢查書桌上還有什麼 |
| `git log` | 查看歷史 | 翻看寄件紀錄 |
| `.gitignore` | 忽略特定檔案 | 搬家時的「不帶清單」 |
" },

        // ── Git Chapter 511 ────────────────────────────
        new() { Id=511, Category="git", Order=2, Level="beginner", Icon="🌿", Title="分支與合併", Slug="git-branching", IsPublished=true, Content=@"
# 分支與合併

## 什麼是分支？

> 💡 **比喻：平行宇宙**
> 想像你在寫一篇小說，突然想試試「主角變壞人」的劇情，
> 但又不想改壞原本的故事。
> 分支就像開啟一個平行宇宙——
> 你可以在新宇宙裡隨便改，
> 改得好就合併回主宇宙，改得爛就丟掉，完全不影響原本的故事。

```
主線（main）    ──●──●──●──●──●──●──
                        \         ↗
功能分支（feature）      ●──●──●
                       在平行宇宙開發新功能
```

---

## 分支基本操作

### 建立與切換分支

```bash
# 查看所有分支（* 標記目前所在分支）
git branch  # 列出本地所有分支

# 建立新分支
git branch feature/login  # 建立一個叫 feature/login 的新分支

# 切換到新分支（傳統方式）
git checkout feature/login  # 切換到 feature/login 分支

# 切換到新分支（新語法，推薦）
git switch feature/login  # 切換到 feature/login 分支（更直覺）

# 建立並同時切換（傳統方式）
git checkout -b feature/signup  # 建立 feature/signup 並立即切換過去

# 建立並同時切換（新語法，推薦）
git switch -c feature/signup  # 建立 feature/signup 並立即切換過去

# 查看所有分支（包含遠端）
git branch -a  # 列出本地和遠端的所有分支

# 刪除已合併的分支
git branch -d feature/login  # 刪除已合併的分支（安全刪除）

# 強制刪除分支（未合併也刪）
git branch -D feature/abandoned  # 強制刪除分支（即使未合併）

# 重新命名分支
git branch -m old-name new-name  # 把分支 old-name 改名為 new-name
```

### 分支命名慣例

```
常用的分支命名規則：
┌───────────────────────────────────────────────────┐
│ 類型             │ 命名格式              │ 範例                    │
├───────────────────────────────────────────────────┤
│ 功能開發         │ feature/功能名稱       │ feature/login           │
│ Bug 修復         │ bugfix/問題描述        │ bugfix/login-crash      │
│ 緊急修復         │ hotfix/問題描述        │ hotfix/security-patch   │
│ 版本發佈         │ release/版本號         │ release/v1.2.0          │
│ 實驗性功能       │ experiment/描述        │ experiment/new-ui       │
└───────────────────────────────────────────────────┘
```

---

## Merge 合併

> 💡 **比喻：樹枝嫁接**
> Merge 就像把一根樹枝嫁接回主幹，
> 兩邊的「生長紀錄」都會保留下來，
> 歷史中可以清楚看到分支從哪裡分出、又在哪裡合併。

### 基本合併操作

```bash
# 步驟 1：先切換到要合併「進去」的目標分支
git switch main  # 切換到 main 分支

# 步驟 2：執行合併
git merge feature/login  # 把 feature/login 的變更合併進 main

# 合併後刪除分支（保持整潔）
git branch -d feature/login  # 刪除已合併的 feature/login 分支
```

### 合併的兩種情況

```
Fast-forward 合併（快進合併）：
main 沒有新的 commit，直接「快進」到 feature 的最新位置

合併前：
main     ──●──●
                \
feature          ●──●──●

合併後（fast-forward）：
main     ──●──●──●──●──●
                        ↑
                   直接快進到這裡

---

Three-way 合併（三方合併）：
main 也有新的 commit，需要建立一個「合併 commit」

合併前：
main     ──●──●──●──●
                \
feature          ●──●──●

合併後（three-way merge）：
main     ──●──●──●──●──●──M  ← 合併 commit
                \         ↗
feature          ●──●──●
```

```bash
# 強制建立合併 commit（即使可以 fast-forward）
git merge --no-ff feature/login  # 保留分支歷史，建立合併 commit

# 查看合併歷史圖形
git log --oneline --graph --all  # 圖形化顯示分支和合併的歷史
```

---

## Rebase 變基

> 💡 **比喻：搬家重蓋**
> Rebase 就像把你蓋在舊地基上的房子，
> 整棟搬到新地基上重蓋。
> 結果看起來就像是你一開始就蓋在新地基上一樣，
> 歷史紀錄變得很乾淨、一條直線。

```
Rebase 前：
main     ──●──●──A──B
                \
feature          ●──●──●

Rebase 後（把 feature 搬到 main 最新的 B 上面）：
main     ──●──●──A──B
                      \
feature                ●'──●'──●'
                       重新套用在 B 之後
```

```bash
# 在 feature 分支上執行 rebase
git switch feature/login  # 先切換到 feature 分支
git rebase main  # 把 feature 的 commit 重新套用在 main 最新的基礎上

# 然後切換回 main 合併（這時會 fast-forward）
git switch main  # 切換回 main
git merge feature/login  # 因為已經 rebase 過，會是 fast-forward
```

### Merge vs Rebase 比較

```
┌──────────────┬──────────────────────┬──────────────────────┐
│              │ Merge                │ Rebase               │
├──────────────┼──────────────────────┼──────────────────────┤
│ 歷史紀錄     │ 保留分支結構         │ 線性歷史（一條線）   │
│ 安全性       │ 不會改寫歷史         │ 會改寫 commit hash   │
│ 適用場景     │ 公開分支/團隊協作    │ 個人分支/整理歷史    │
│ 比喻         │ 樹枝嫁接             │ 搬家重蓋             │
│ 黃金規則     │ 隨時都可以用         │ 不要對已推送的分支用 │
└──────────────┴──────────────────────┴──────────────────────┘
```

---

## 解決合併衝突

> 💡 **比喻：兩人同時改同一份文件**
> 你和同事同時改了同一行程式碼，
> Git 無法決定要用誰的版本，
> 所以它把兩個版本都標出來，讓你手動選擇。

### 衝突發生時的樣子

```bash
# 嘗試合併時出現衝突
git merge feature/login  # Git 回報：CONFLICT (content): Merge conflict in Program.cs
```

```csharp
// Program.cs 中衝突的樣子：
<<<<<<< HEAD
// 這是 main 分支的版本
var greeting = ""你好，歡迎回來！"";  // main 的歡迎訊息
=======
// 這是 feature/login 分支的版本
var greeting = ""嗨，歡迎登入！"";  // feature 的歡迎訊息
>>>>>>> feature/login
```

### 解決衝突的步驟

```bash
# 步驟 1：查看哪些檔案有衝突
git status  # 顯示「both modified」的檔案就是有衝突的

# 步驟 2：打開有衝突的檔案，手動編輯
# 移除 <<<<<<<、=======、>>>>>>> 標記
# 選擇要保留的版本（或兩個都保留/合併）

# 步驟 3：解決後加入暫存區
git add Program.cs  # 標記衝突已解決

# 步驟 4：完成合併提交
git commit -m ""解決 Program.cs 的合併衝突""  # 提交合併結果

# 如果想放棄這次合併
git merge --abort  # 取消合併，回到合併前的狀態
```

### VS Code 衝突解決工具

```
VS Code 會自動偵測衝突並提供按鈕：
┌──────────────────────────────────────────────┐
│ Accept Current Change   │ 使用目前分支的版本 │
│ Accept Incoming Change  │ 使用合併進來的版本 │
│ Accept Both Changes     │ 兩個都保留         │
│ Compare Changes         │ 並排比較兩個版本   │
└──────────────────────────────────────────────┘
```

---

## 分支策略

### Git Flow

```
Git Flow 分支策略：
┌────────────────────────────────────────────────────────────┐
│ main        ──●────────────────●────────────●──            │
│                \              ↗              ↑              │
│ release        ●──●──●──●──●        release/v2             │
│                ↑              \                              │
│ develop    ──●──●──●──●──●──●──●──●──●──●──                │
│               \  ↗  \     ↗                                 │
│ feature       ●──●    ●──●                                  │
│              login    signup                                 │
│                                                             │
│ hotfix     ────────────────────────●──●                     │
│                                        \→ main & develop    │
└────────────────────────────────────────────────────────────┘

- main：正式發佈版本
- develop：開發主線
- feature/*：功能分支（從 develop 分出）
- release/*：發佈準備
- hotfix/*：緊急修復（從 main 分出）
```

### GitHub Flow（簡化版）

```
GitHub Flow 分支策略（適合小團隊和持續部署）：
┌────────────────────────────────────────┐
│ main       ──●──●──●──M──●──M──●──    │
│                  \    ↗    \  ↗        │
│ feature-1         ●──●      │         │
│                              │         │
│ feature-2                   ●──●      │
│                                        │
│ 規則：                                 │
│ 1. main 永遠可部署                     │
│ 2. 從 main 建立 feature 分支           │
│ 3. 開 Pull Request 請人 review         │
│ 4. Review 通過後合併回 main            │
│ 5. 合併後立即部署                      │
└────────────────────────────────────────┘
```

---

## Cherry-pick 與 Stash

### Cherry-pick（摘櫻桃）

> 💡 **比喻：從其他分支「摘」一個特定的 commit**
> 就像從別人的果園摘一顆特別好的櫻桃到自己的籃子裡，
> 不需要把整棵樹搬過來。

```bash
# 從其他分支挑選特定 commit 套用到目前分支
git cherry-pick abc1234  # 把 commit abc1234 的變更套用到目前分支

# 挑選多個 commit
git cherry-pick abc1234 def5678  # 依序套用兩個 commit

# 只套用變更但不自動提交
git cherry-pick --no-commit abc1234  # 套用變更到工作區，但不自動 commit

# 放棄 cherry-pick
git cherry-pick --abort  # 取消目前的 cherry-pick 操作
```

### Stash（暫時擱置）

> 💡 **比喻：把桌上的東西先塞進抽屜**
> 你正在寫功能 A，突然要緊急修 Bug，
> 但功能 A 還沒寫完不想 commit。
> Stash 就是把半成品先「塞進抽屜」，
> 修完 Bug 後再「從抽屜拿出來」繼續做。

```bash
# 暫時擱置目前的變更
git stash  # 把所有未 commit 的變更存起來，工作區變乾淨

# 擱置時附上說明
git stash push -m ""做到一半的登入功能""  # 加上說明方便之後辨識

# 查看所有暫存的東西
git stash list  # 列出所有 stash，顯示編號和說明

# 恢復最新的 stash（並從清單移除）
git stash pop  # 拿出最新的 stash 並從清單中刪除

# 恢復最新的 stash（但保留在清單中）
git stash apply  # 套用最新的 stash，但不從清單刪除

# 恢復特定的 stash
git stash apply stash@{2}  # 套用編號 2 的 stash

# 刪除特定的 stash
git stash drop stash@{0}  # 刪除編號 0 的 stash

# 清空所有 stash
git stash clear  # 刪除所有暫存的變更
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：在 main 分支上直接開發

```bash
# 錯誤：直接在 main 上改程式
git switch main  # 切換到 main
# ... 開始寫新功能 ...
git commit -m ""新增登入功能""  # 直接在 main 上提交

# ✅ 正確：先建立分支再開發
git switch main  # 先確認在 main 上
git switch -c feature/login  # 建立並切換到功能分支
# ... 開始寫新功能 ...
git commit -m ""新增登入功能""  # 在功能分支上提交
```

**為什麼錯？** 直接在 main 上開發，萬一寫到一半要緊急修 Bug，你的半成品程式碼就會混在一起。用分支可以隔離不同的工作。

### ❌ 錯誤 2：對已推送的公開分支做 rebase

```bash
# 錯誤：對已經 push 到遠端的分支做 rebase
git switch main  # 切換到 main
git rebase feature/login  # 改寫了 main 的歷史！

# ✅ 正確：公開分支用 merge
git switch main  # 切換到 main
git merge feature/login  # 用 merge 合併，不改寫歷史
```

**為什麼錯？** Rebase 會改寫 commit 的 hash，如果其他人已經基於舊的 hash 在工作，他們的歷史就會跟你的對不上，造成混亂。黃金規則：**不要 rebase 已經推送到遠端的 commit**。

### ❌ 錯誤 3：衝突解決後忘記 commit

```bash
# 錯誤：解決衝突後忘記提交
git merge feature/login  # 發生衝突
# ... 手動解決衝突 ...
git add Program.cs  # 標記已解決
# 忘記 git commit 了！

# ✅ 正確：解決衝突後要完成提交
git merge feature/login  # 發生衝突
# ... 手動解決衝突 ...
git add Program.cs  # 標記已解決
git commit -m ""解決合併衝突：整合登入功能""  # 完成合併提交
```

**為什麼錯？** 衝突解決後沒有 commit，Git 仍然處於「合併中」的狀態，後續操作會出問題。

---

## 📝 本章重點整理

| 指令 | 用途 | 比喻 |
|------|------|------|
| `git branch` | 管理分支 | 開啟平行宇宙 |
| `git switch` | 切換分支 | 跳到另一個宇宙 |
| `git merge` | 合併分支 | 樹枝嫁接 |
| `git rebase` | 變基 | 搬家重蓋 |
| `git cherry-pick` | 挑選 commit | 摘櫻桃 |
| `git stash` | 暫時擱置 | 塞進抽屜 |
" },

        // ── Git Chapter 512 ────────────────────────────
        new() { Id=512, Category="git", Order=3, Level="intermediate", Icon="🤝", Title="團隊協作與 GitHub", Slug="git-collaboration", IsPublished=true, Content=@"
# 團隊協作與 GitHub

## 遠端儲存庫（Remote Repository）

> 💡 **比喻：雲端硬碟**
> 本地的 Git 儲存庫就像你電腦上的檔案，
> 遠端儲存庫（如 GitHub）就像 Google Drive 或 OneDrive，
> 把程式碼備份到雲端，讓團隊成員都能存取。

### Remote 基本操作

```bash
# 查看目前設定的遠端儲存庫
git remote -v  # 列出所有遠端儲存庫的名稱和網址

# 新增遠端儲存庫（通常叫 origin）
git remote add origin https://github.com/username/repo.git  # 新增名為 origin 的遠端連結

# 修改遠端網址
git remote set-url origin https://github.com/username/new-repo.git  # 更改 origin 的網址

# 移除遠端連結
git remote remove origin  # 移除名為 origin 的遠端連結

# 查看遠端詳細資訊
git remote show origin  # 顯示 origin 的詳細分支追蹤資訊
```

### Push、Pull、Fetch

```bash
# === Push（推送）===
# 把本地的 commit 上傳到遠端
git push origin main  # 把本地 main 分支推送到遠端 origin

# 第一次推送時設定追蹤關係（之後只需 git push）
git push -u origin main  # -u 設定 upstream，之後可以簡寫 git push

# 推送所有分支
git push --all origin  # 把所有本地分支都推送到遠端

# 推送標籤
git push origin --tags  # 把所有標籤推送到遠端

# === Pull（拉取 + 合併）===
# 從遠端下載最新的變更並合併到目前分支
git pull origin main  # 從 origin 拉取 main 的變更並合併

# 使用 rebase 方式拉取（保持歷史乾淨）
git pull --rebase origin main  # 拉取後用 rebase 而非 merge 整合

# === Fetch（只下載，不合併）===
# 只下載遠端的變更，但不自動合併
git fetch origin  # 下載 origin 所有分支的最新狀態

# 查看遠端有什麼新變更
git fetch origin  # 先下載遠端最新狀態
git log main..origin/main --oneline  # 比較本地和遠端 main 的差異

# Fetch vs Pull 的差別
# fetch = 只下載，你可以檢查後再決定要不要合併
# pull  = fetch + merge，自動下載並合併
```

### 從 GitHub 複製（Clone）專案

```bash
# 複製遠端儲存庫到本地（HTTPS 方式）
git clone https://github.com/username/repo.git  # 複製整個儲存庫到本地

# 複製到指定資料夾
git clone https://github.com/username/repo.git my-folder  # 複製到 my-folder 資料夾

# 複製遠端儲存庫到本地（SSH 方式，推薦）
git clone git@github.com:username/repo.git  # 用 SSH 複製（不需每次輸入密碼）

# 只複製最新的一次 commit（淺複製，適合大型專案）
git clone --depth 1 https://github.com/username/repo.git  # 淺複製，只下載最新版本
```

---

## Pull Request 工作流程

> 💡 **比喻：審稿流程**
> 你寫了一篇文章（程式碼變更），
> 不是直接發表，而是先交給編輯（審查者）審核，
> 編輯提出修改意見，你修改後再正式發表（合併到 main）。

### 完整 PR 流程

```bash
# 步驟 1：從 main 建立功能分支
git switch main  # 先確認在 main 上
git pull origin main  # 拉取最新的 main
git switch -c feature/user-profile  # 建立並切換到功能分支

# 步驟 2：開發功能，多次 commit
git add .  # 暫存變更
git commit -m ""新增用戶個人資料頁面""  # 提交第一步
# ... 繼續開發 ...
git add .  # 暫存更多變更
git commit -m ""新增頭像上傳功能""  # 提交第二步

# 步驟 3：推送到遠端
git push -u origin feature/user-profile  # 推送功能分支到遠端

# 步驟 4：在 GitHub 上建立 Pull Request
# 前往 GitHub 網站，點擊 ""Compare & pull request""
# 填寫 PR 標題和說明

# 步驟 5：Code Review 後可能需要修改
git add .  # 暫存修改
git commit -m ""根據 review 意見修改驗證邏輯""  # 提交修改
git push  # 推送到遠端，PR 會自動更新

# 步驟 6：合併後清理
git switch main  # 切回 main
git pull origin main  # 拉取包含 PR 合併後的最新 main
git branch -d feature/user-profile  # 刪除本地功能分支
```

### PR 說明模板

```markdown
## 變更說明
簡要描述這個 PR 做了什麼

## 變更類型
- [ ] 新功能（New Feature）
- [ ] Bug 修復（Bug Fix）
- [ ] 重構（Refactoring）
- [ ] 文件更新（Documentation）

## 測試方式
描述如何測試這些變更

## 截圖（如果有 UI 變更）
貼上前後對比截圖

## 相關 Issue
Closes #123
```

---

## Code Review 最佳實踐

### 給予 Review 的原則

```
Code Review 檢查清單：
┌────────────────────────────────────────────────────────┐
│ ✅ 程式邏輯正確嗎？有沒有邊界條件沒處理？              │
│ ✅ 命名清楚嗎？其他人看得懂變數和函式的用途？           │
│ ✅ 有重複的程式碼嗎？能不能抽取共用方法？               │
│ ✅ 有寫測試嗎？測試有覆蓋主要情境嗎？                   │
│ ✅ 安全性：有 SQL Injection、XSS 等風險嗎？             │
│ ✅ 效能：有不必要的迴圈或資料庫查詢嗎？                 │
│ ✅ 是否遵循團隊的 coding style？                        │
│ ✅ commit 訊息清楚嗎？PR 說明完整嗎？                   │
└────────────────────────────────────────────────────────┘
```

### Review 留言語氣

```
❌ 不好的留言：
""這段程式碼很爛""
""你不懂怎麼寫嗎？""

✅ 好的留言：
""建議這裡可以用 Dictionary 取代 List，查找效率會從 O(n) 提升到 O(1)""
""這個方法超過 50 行了，考慮拆成幾個小方法提升可讀性？""
""nit: 這個變數名稱 d 建議改成 dayCount，更好理解""
```

---

## GitHub Actions CI/CD 基礎

> 💡 **比喻：自動化工廠流水線**
> 每次你把程式碼推送到 GitHub，
> GitHub Actions 就像一條自動化流水線，
> 幫你自動編譯、測試、部署，不需要人工介入。

### 基本 .NET 專案的 CI 設定

```yaml
# .github/workflows/dotnet-ci.yml
# 工作流程名稱
name: .NET CI

# 觸發條件：push 到 main 或建立 PR 時
on:
  push:
    branches: [ main ]  # 推送到 main 時觸發
  pull_request:
    branches: [ main ]  # 對 main 建立 PR 時觸發

# 定義工作
jobs:
  build-and-test:  # 工作名稱
    runs-on: ubuntu-latest  # 執行環境：最新版 Ubuntu

    steps:
    # 步驟 1：取出程式碼
    - uses: actions/checkout@v4  # 從儲存庫取出程式碼

    # 步驟 2：設定 .NET SDK
    - name: Setup .NET  # 步驟顯示名稱
      uses: actions/setup-dotnet@v4  # 安裝 .NET SDK
      with:
        dotnet-version: '8.0.x'  # 指定 .NET 8 版本

    # 步驟 3：還原套件
    - name: Restore  # 步驟顯示名稱
      run: dotnet restore  # 執行 NuGet 套件還原

    # 步驟 4：編譯
    - name: Build  # 步驟顯示名稱
      run: dotnet build --no-restore  # 編譯專案（不重複還原）

    # 步驟 5：執行測試
    - name: Test  # 步驟顯示名稱
      run: dotnet test --no-build --verbosity normal  # 執行單元測試
```

### 常用 CI/CD 工作流程

```
CI/CD 流水線常見階段：
┌─────────┐   ┌─────────┐   ┌─────────┐   ┌─────────┐
│ Restore │ → │  Build  │ → │  Test   │ → │ Deploy  │
│ 還原套件│   │  編譯   │   │  測試   │   │  部署   │
└─────────┘   └─────────┘   └─────────┘   └─────────┘
     │              │             │              │
   自動下載       自動編譯     自動跑測試    自動部署到
   NuGet 套件    程式碼       確認沒有 Bug   正式環境
```

---

## Issue、Milestone、Project Board

### Issue（議題）

```
GitHub Issue 的用途：
┌────────────────────────────────────────────────┐
│ 🐛 Bug Report    │ 回報程式中發現的錯誤        │
│ ✨ Feature Request│ 提出新功能的需求            │
│ 📝 Task          │ 一般工作任務                │
│ ❓ Question      │ 提出技術問題                │
│ 📖 Documentation │ 文件改善需求                │
└────────────────────────────────────────────────┘

Label（標籤）分類：
- bug：程式錯誤
- enhancement：功能改善
- good first issue：適合新手的任務
- help wanted：需要幫助
- priority: high/medium/low：優先度
```

### Milestone 與 Project Board

```
Milestone（里程碑）：
┌────────────────────────────────────────┐
│ v1.0 Release（截止日：2024-06-30）    │
│ ├── #12 使用者登入 ✅                 │
│ ├── #13 使用者註冊 ✅                 │
│ ├── #14 個人資料頁 🔄                 │
│ └── #15 忘記密碼   📋                 │
│ 進度：50%  ████░░░░                   │
└────────────────────────────────────────┘

Project Board（看板）：
┌──────────┬──────────┬──────────┬──────────┐
│ 待辦      │ 進行中   │ 審查中   │ 完成     │
│ (To Do)  │(In Progress)│(In Review)│(Done)  │
├──────────┼──────────┼──────────┼──────────┤
│ #15      │ #14      │          │ #12      │
│ 忘記密碼 │ 個人資料 │          │ 登入     │
│          │          │          │ #13      │
│          │          │          │ 註冊     │
└──────────┴──────────┴──────────┴──────────┘
```

---

## SSH Key 設定

> 💡 **比喻：門禁卡**
> SSH Key 就像你公司的門禁卡，
> 你只要設定一次，之後進出（push/pull）都不用再輸入密碼。
> 私鑰（Private Key）= 你的門禁卡（絕對不能給別人）
> 公鑰（Public Key）= 門禁系統的紀錄（給 GitHub 保管）

### 產生 SSH Key

```bash
# 步驟 1：產生 SSH Key（使用 Ed25519 演算法，較安全）
ssh-keygen -t ed25519 -C ""you@example.com""  # 產生新的 SSH 金鑰對

# 按 Enter 使用預設路徑（~/.ssh/id_ed25519）
# 設定密碼短語（passphrase）或直接 Enter 跳過

# 步驟 2：啟動 SSH Agent
eval ""$(ssh-agent -s)""  # 啟動 SSH Agent 背景服務

# 步驟 3：新增私鑰到 SSH Agent
ssh-add ~/.ssh/id_ed25519  # 把私鑰加入 SSH Agent

# 步驟 4：複製公鑰（要貼到 GitHub）
# Windows（Git Bash）
cat ~/.ssh/id_ed25519.pub  # 顯示公鑰內容，手動複製

# macOS
pbcopy < ~/.ssh/id_ed25519.pub  # 自動複製公鑰到剪貼簿

# Linux
xclip -sel clip < ~/.ssh/id_ed25519.pub  # 自動複製公鑰到剪貼簿
```

### 在 GitHub 上設定

```
設定步驟：
1. 登入 GitHub
2. 點擊右上角頭像 → Settings（設定）
3. 左側選單 → SSH and GPG keys
4. 點擊 ""New SSH key""
5. Title 填入識別名稱（如 ""我的筆電""）
6. Key type 選 ""Authentication Key""
7. 貼上剛才複製的公鑰
8. 點擊 ""Add SSH key""
```

```bash
# 測試 SSH 連線
ssh -T git@github.com  # 測試是否能成功連線到 GitHub
# 成功會顯示：Hi username! You've successfully authenticated...

# 把現有的 HTTPS 遠端改為 SSH
git remote set-url origin git@github.com:username/repo.git  # 改用 SSH 連線
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：pull 之前先 push 導致被拒絕

```bash
# 錯誤：直接 push，但遠端已經有別人的新 commit
git push origin main  # 被拒絕！ rejected（non-fast-forward）

# ✅ 正確：先 pull 再 push
git pull origin main  # 先拉取遠端的最新變更
# 如果有衝突就解決衝突
git push origin main  # 再推送自己的變更
```

**為什麼錯？** 遠端有你沒有的 commit，Git 不讓你直接覆蓋。必須先把遠端的變更整合到本地，確認沒有衝突後再推送。

### ❌ 錯誤 2：直接推送到 main 分支

```bash
# 錯誤：跳過 PR 直接推 main
git switch main  # 切到 main
git commit -m ""新增購物車功能""  # 在 main 上提交
git push origin main  # 直接推到遠端 main

# ✅ 正確：用 PR 流程
git switch -c feature/cart  # 建立功能分支
git commit -m ""新增購物車功能""  # 在功能分支上提交
git push -u origin feature/cart  # 推送功能分支
# 然後在 GitHub 上建立 Pull Request
```

**為什麼錯？** 直接推到 main 跳過了 Code Review，可能引入 Bug 或破壞性變更。團隊協作應該透過 PR 流程，讓其他人有機會審查程式碼。

### ❌ 錯誤 3：把 SSH 私鑰分享給別人

```bash
# 錯誤：把私鑰檔案傳給同事
# ""你拿去用我的 SSH Key 就可以 push 了""

# ✅ 正確：每個人產生自己的 SSH Key
ssh-keygen -t ed25519 -C ""colleague@example.com""  # 每人自己產生金鑰
# 把自己的公鑰加到 GitHub
```

**為什麼錯？** 私鑰就像你的密碼，給了別人等於讓別人能用你的身份操作所有 GitHub 儲存庫。每個人都應該有自己的 SSH Key。

---

## 📝 本章重點整理

| 概念 | 用途 | 比喻 |
|------|------|------|
| `git remote` | 管理遠端連結 | 設定雲端硬碟 |
| `git push` | 推送到遠端 | 上傳到雲端 |
| `git pull` | 拉取並合併 | 從雲端下載 |
| `git fetch` | 只下載不合併 | 先看看雲端有什麼新的 |
| Pull Request | 程式碼審查流程 | 投稿審稿流程 |
| GitHub Actions | 自動化流水線 | 自動化工廠 |
| SSH Key | 免密碼認證 | 門禁卡 |
" },

        // ── Git Chapter 513 ────────────────────────────
        new() { Id=513, Category="git", Order=4, Level="intermediate", Icon="🔧", Title="Git 進階技巧", Slug="git-advanced", IsPublished=true, Content=@"
# Git 進階技巧

## Interactive Rebase（互動式變基）

> 💡 **比喻：出版前的校稿**
> 你寫了好幾章草稿（多個 commit），
> 在出版前（push 前）想要重新整理：
> 合併幾章、修改標題、調整順序。
> Interactive Rebase 就是讓你在「出版前」做最後的編輯。

### 常用操作

```bash
# 互動式修改最近 3 個 commit
git rebase -i HEAD~3  # 開啟編輯器，讓你修改最近 3 個 commit

# 編輯器會顯示類似這樣的內容：
# pick abc1234 新增登入頁面
# pick def5678 修復 typo
# pick ghi9012 新增登入驗證

# 可用的指令：
# pick   = 保留這個 commit（不做任何修改）
# reword = 保留但修改 commit 訊息
# squash = 與前一個 commit 合併（保留訊息）
# fixup  = 與前一個 commit 合併（丟棄訊息）
# drop   = 刪除這個 commit
# edit   = 暫停在這個 commit，讓你修改內容
```

### Squash — 合併多個 commit

```bash
# 場景：把零碎的 commit 合併成有意義的一個
git rebase -i HEAD~4  # 修改最近 4 個 commit

# 編輯器中修改：
# pick abc1234 新增登入頁面 HTML
# squash def5678 新增登入頁面 CSS
# squash ghi9012 新增登入頁面 JavaScript
# squash jkl3456 修復登入頁面 typo

# 儲存後會跳出另一個編輯器讓你寫合併後的 commit 訊息
# 最終結果：4 個 commit 變成 1 個

# 快速 squash（合併到前一個 commit，不修改訊息）
git commit --fixup abc1234  # 建立一個 fixup commit，標記要合併到 abc1234
git rebase -i --autosquash HEAD~5  # 自動排序 fixup commit 到對應位置
```

### Reword — 修改 commit 訊息

```bash
# 場景：修改之前的 commit 訊息（不只是最新的）
git rebase -i HEAD~3  # 修改最近 3 個 commit

# 編輯器中修改：
# pick abc1234 新增登入頁面
# reword def5678 修固 typo    ← 把 pick 改成 reword
# pick ghi9012 新增驗證

# 儲存後會跳出編輯器讓你修改該 commit 的訊息
# 修改為：""修復登入頁面的 typo""
```

---

## git bisect — 二分搜尋找 Bug

> 💡 **比喻：翻書找錯字**
> 你知道第 1 頁沒有錯字，第 100 頁有錯字，
> 不用一頁一頁翻，直接翻到第 50 頁：
> 有錯字？那問題在 1-50 頁之間，再翻第 25 頁...
> 這就是二分搜尋法，Git bisect 用同樣的方式找出引入 Bug 的 commit。

### 使用 bisect 找 Bug

```bash
# 步驟 1：開始 bisect
git bisect start  # 開始二分搜尋

# 步驟 2：標記目前版本是壞的
git bisect bad  # 告訴 Git：目前的版本有 Bug

# 步驟 3：標記一個已知沒問題的舊版本
git bisect good v1.0  # 告訴 Git：v1.0 版本是好的、沒有 Bug

# Git 會自動 checkout 到中間的 commit
# 你測試一下，然後告訴 Git 結果：

# 步驟 4：測試並回報
git bisect good  # 這個版本沒問題（Git 會再跳到下一個中間點）
git bisect bad   # 這個版本有問題（Git 會縮小搜尋範圍）

# 重複步驟 4 直到找到罪魁禍首
# Git 會顯示：abc1234 is the first bad commit

# 步驟 5：結束 bisect
git bisect reset  # 結束二分搜尋，回到原本的分支
```

### 自動化 bisect

```bash
# 如果有自動化測試，可以讓 bisect 自動跑
git bisect start  # 開始二分搜尋
git bisect bad HEAD  # 目前版本有 Bug
git bisect good v1.0  # v1.0 沒問題

# 自動執行測試腳本（回傳 0 = good，非 0 = bad）
git bisect run dotnet test  # 自動用測試結果來判斷好壞

# Git 會自動找出引入 Bug 的 commit！
git bisect reset  # 結束後回到原本的分支
```

---

## git reflog — 救回誤刪的東西

> 💡 **比喻：資源回收筒中的回收筒**
> 即使你 `reset --hard` 或不小心刪了分支，
> Git 的 reflog 會記錄你最近 90 天內所有的操作歷史，
> 像是一個「超級資源回收筒」，幾乎什麼都能救回來。

### 查看 reflog

```bash
# 查看所有操作歷史
git reflog  # 顯示 HEAD 的所有移動紀錄

# 輸出範例：
# abc1234 HEAD@{0}: commit: 新增功能
# def5678 HEAD@{1}: checkout: moving from main to feature
# ghi9012 HEAD@{2}: commit: 修復 Bug
# jkl3456 HEAD@{3}: reset: moving to HEAD~2  ← 這裡做了 reset！
```

### 救回誤刪的 commit

```bash
# 場景：不小心做了 git reset --hard，丟掉了重要的 commit
git reflog  # 查看歷史，找到被丟掉的 commit hash

# 方法 1：直接 checkout 到那個 commit
git checkout abc1234  # 切換到被丟掉的 commit（detached HEAD 狀態）
git switch -c recovered-branch  # 建立新分支來保存它

# 方法 2：用 reset 回到那個狀態
git reset --hard abc1234  # 把目前分支強制指向那個 commit

# 方法 3：用 cherry-pick 撿回特定的 commit
git cherry-pick abc1234  # 把特定的 commit 套用到目前分支
```

### 救回刪掉的分支

```bash
# 場景：不小心刪了一個還沒合併的分支
git branch -D feature/important  # 糟糕！刪掉了重要分支

# 步驟 1：用 reflog 找到分支最後的 commit
git reflog  # 找到 feature/important 最後一個 commit 的 hash

# 步驟 2：從那個 commit 建立新分支
git branch feature/important abc1234  # 用找到的 hash 重新建立分支

# 分支就救回來了！
git switch feature/important  # 切換到救回的分支確認內容
```

---

## Submodule vs Subtree

### Submodule（子模組）

> 💡 **比喻：引用外部函式庫**
> Submodule 像是在你的專案中放一個「書籤」指向另一個儲存庫，
> 專案不包含那個儲存庫的檔案本體，只記錄指向哪個版本。

```bash
# 新增子模組
git submodule add https://github.com/lib/shared-utils.git libs/shared-utils  # 新增子模組到 libs 資料夾

# 複製含有子模組的專案
git clone --recurse-submodules https://github.com/user/project.git  # 連同子模組一起複製

# 如果已經 clone 但忘記加 --recurse-submodules
git submodule init  # 初始化子模組設定
git submodule update  # 下載子模組的內容

# 更新子模組到最新版本
git submodule update --remote  # 把所有子模組更新到遠端最新版本

# 查看子模組狀態
git submodule status  # 顯示每個子模組目前指向的 commit
```

### Subtree（子樹）

> 💡 **比喻：把外部程式碼直接複製進來**
> Subtree 是直接把另一個儲存庫的檔案合併到你的專案中，
> 不需要額外的設定步驟，使用起來比 submodule 簡單。

```bash
# 新增子樹
git subtree add --prefix=libs/shared-utils https://github.com/lib/shared-utils.git main --squash  # 把外部 repo 的 main 分支加入到 libs 資料夾

# 更新子樹
git subtree pull --prefix=libs/shared-utils https://github.com/lib/shared-utils.git main --squash  # 拉取外部 repo 的最新變更

# 推送子樹的變更回去
git subtree push --prefix=libs/shared-utils https://github.com/lib/shared-utils.git main  # 把對子樹的修改推送回原始 repo
```

### Submodule vs Subtree 比較

```
┌───────────────┬────────────────────────┬────────────────────────┐
│               │ Submodule              │ Subtree                │
├───────────────┼────────────────────────┼────────────────────────┤
│ 概念          │ 指向另一個 repo 的指標 │ 直接合併程式碼進來     │
│ 設定複雜度    │ 較複雜                 │ 較簡單                 │
│ Clone         │ 需要 --recurse         │ 正常 clone 即可        │
│ 更新          │ submodule update       │ subtree pull           │
│ 適用場景      │ 大型專案、嚴格版本控制 │ 小型共用程式庫         │
│ 新成員上手    │ 容易忘記初始化         │ 零額外步驟             │
└───────────────┴────────────────────────┴────────────────────────┘
```

---

## Git Hooks（鉤子）

> 💡 **比喻：保全系統**
> Git Hooks 就像門口的保全系統，
> 在你「進門」（commit）或「出門」（push）之前，
> 自動檢查你有沒有帶齊東西、有沒有可疑物品。

### 常用 Hooks

```bash
# Hook 檔案位於 .git/hooks/ 資料夾
ls .git/hooks/  # 查看可用的 hook 範本檔

# 常用的 Hook：
# pre-commit   → commit 之前執行（適合做程式碼檢查）
# commit-msg   → 檢查 commit 訊息格式
# pre-push     → push 之前執行（適合跑測試）
# post-merge   → merge 之後執行（適合自動安裝套件）
```

### pre-commit Hook 範例

```bash
#!/bin/sh
# .git/hooks/pre-commit
# 在 commit 之前自動檢查程式碼品質

echo ""🔍 正在檢查程式碼...""  # 顯示檢查開始訊息

# 執行 .NET 格式檢查
dotnet format --verify-no-changes  # 檢查程式碼是否符合格式規範
if [ $? -ne 0 ]; then  # 如果格式檢查失敗
    echo ""❌ 程式碼格式不符，請先執行 dotnet format""  # 顯示錯誤訊息
    exit 1  # 回傳非零值，中止 commit
fi

# 執行單元測試
dotnet test --no-build  # 執行測試但不重新編譯
if [ $? -ne 0 ]; then  # 如果測試失敗
    echo ""❌ 測試未通過，請修復後再 commit""  # 顯示錯誤訊息
    exit 1  # 回傳非零值，中止 commit
fi

echo ""✅ 所有檢查通過！""  # 顯示檢查通過訊息
exit 0  # 回傳零值，允許 commit
```

### 使用 Husky（跨平台 Hook 管理工具）

```bash
# 安裝 Husky（Node.js 專案）
npm install husky --save-dev  # 安裝 Husky 到開發依賴

# 初始化 Husky
npx husky init  # 建立 .husky 資料夾和基本設定

# 新增 pre-commit hook
echo ""dotnet format --verify-no-changes"" > .husky/pre-commit  # 設定 commit 前自動檢查格式

# .NET 專案可以用 dotnet-format 的 local tool
dotnet tool install dotnet-format  # 安裝 dotnet-format 工具
```

### commit-msg Hook（檢查 commit 訊息格式）

```bash
#!/bin/sh
# .git/hooks/commit-msg
# 檢查 commit 訊息是否符合 Conventional Commits 格式

commit_msg=$(cat ""$1"")  # 讀取 commit 訊息內容

# 檢查格式：type(scope): description
pattern=""^(feat|fix|docs|style|refactor|test|chore)(\(.+\))?: .{1,72}$""  # 定義允許的格式模式

if ! echo ""$commit_msg"" | grep -qE ""$pattern""; then  # 如果訊息不符合格式
    echo ""❌ commit 訊息格式不正確！""  # 顯示錯誤訊息
    echo ""正確格式：type(scope): description""  # 顯示正確格式
    echo ""例如：feat(login): 新增密碼重設功能""  # 顯示範例
    echo ""允許的 type：feat, fix, docs, style, refactor, test, chore""  # 列出允許的類型
    exit 1  # 中止 commit
fi

exit 0  # 格式正確，允許 commit
```

---

## 大檔案處理：Git LFS

> 💡 **比喻：大行李用託運**
> 搭飛機時，小包包可以手提帶上機（一般 Git），
> 但大行李箱要託運（Git LFS）。
> Git LFS 把大檔案「託運」到專門的伺服器，
> Git 只記錄一個「託運單號」（指標），讓 repo 保持輕量。

### 安裝與設定 Git LFS

```bash
# 安裝 Git LFS
# Windows：Git for Windows 通常已內建
# macOS
brew install git-lfs  # 透過 Homebrew 安裝 Git LFS

# Linux
sudo apt install git-lfs  # 透過 apt 安裝 Git LFS

# 初始化 Git LFS（每台電腦做一次）
git lfs install  # 在系統層級啟用 Git LFS

# 追蹤特定類型的大檔案
git lfs track ""*.psd""  # 追蹤所有 Photoshop 檔案
git lfs track ""*.zip""  # 追蹤所有 ZIP 壓縮檔
git lfs track ""*.mp4""  # 追蹤所有影片檔案
git lfs track ""*.dll""  # 追蹤所有 DLL 檔案

# 確認 .gitattributes 已被追蹤
git add .gitattributes  # 把 LFS 的追蹤設定加入版本控制

# 查看目前 LFS 追蹤的檔案類型
git lfs track  # 列出所有被 LFS 追蹤的模式

# 查看 LFS 管理的檔案
git lfs ls-files  # 列出所有被 LFS 管理的檔案
```

### LFS 日常使用

```bash
# 正常使用 git add / commit / push，LFS 會自動處理
git add design.psd  # 加入大檔案（LFS 自動接管）
git commit -m ""新增設計稿""  # 正常提交
git push origin main  # 推送時 LFS 會自動上傳大檔案到 LFS 伺服器

# 複製含有 LFS 的專案
git clone https://github.com/user/project.git  # 正常 clone，LFS 自動下載大檔案

# 如果只想下載指標（不下載大檔案內容）
GIT_LFS_SKIP_SMUDGE=1 git clone https://github.com/user/project.git  # 跳過 LFS 檔案下載

# 之後需要時再手動下載
git lfs pull  # 手動下載所有 LFS 管理的檔案
```

---

## .gitattributes 設定

> 💡 **比喻：入境申報表**
> .gitattributes 就像海關的申報表，
> 告訴 Git 每種檔案要怎麼處理：
> 文字檔要自動轉換換行符號、二進位檔不要嘗試做 diff。

### 常用 .gitattributes 設定

```bash
# .gitattributes 檔案內容

# === 文字檔案處理 ===
# 自動偵測文字檔並統一換行符號
* text=auto  # 讓 Git 自動判斷檔案類型並處理換行符號

# 明確指定文字檔案（使用 LF 換行）
*.cs text eol=lf  # C# 檔案統一用 LF 換行
*.csproj text eol=lf  # 專案檔統一用 LF 換行
*.sln text eol=lf  # 解決方案檔統一用 LF 換行
*.json text eol=lf  # JSON 檔案統一用 LF 換行
*.md text eol=lf  # Markdown 檔案統一用 LF 換行
*.yml text eol=lf  # YAML 檔案統一用 LF 換行
*.yaml text eol=lf  # YAML 檔案統一用 LF 換行

# Windows 批次檔需要用 CRLF
*.bat text eol=crlf  # 批次檔統一用 CRLF 換行
*.cmd text eol=crlf  # 命令檔統一用 CRLF 換行

# === 二進位檔案（不做 diff）===
*.png binary  # PNG 圖片標記為二進位
*.jpg binary  # JPG 圖片標記為二進位
*.gif binary  # GIF 圖片標記為二進位
*.ico binary  # 圖示檔標記為二進位
*.pdf binary  # PDF 文件標記為二進位
*.zip binary  # ZIP 壓縮檔標記為二進位

# === Git LFS（大檔案）===
*.psd filter=lfs diff=lfs merge=lfs -text  # Photoshop 檔案用 LFS 管理
*.ai filter=lfs diff=lfs merge=lfs -text  # Illustrator 檔案用 LFS 管理
*.mp4 filter=lfs diff=lfs merge=lfs -text  # 影片檔案用 LFS 管理

# === 語言統計（GitHub 語言識別）===
docs/api/** linguist-generated=true  # 自動產生的 API 文件不計入語言統計
*.min.js linguist-vendored=true  # 壓縮的 JS 標記為第三方檔案
```

### .NET 專案建議的 .gitattributes

```bash
# .NET 專案的 .gitattributes
# 自動偵測文字檔
* text=auto  # Git 自動判斷並處理換行符號

# .NET 原始碼
*.cs text diff=csharp  # C# 檔案用 csharp diff 演算法
*.cshtml text diff=html  # Razor 檔案用 html diff 演算法
*.razor text diff=html  # Blazor 元件用 html diff 演算法
*.xaml text  # XAML 檔案標記為文字
*.csproj text  # 專案檔標記為文字
*.sln text  # 解決方案檔標記為文字

# 設定檔
*.json text  # JSON 標記為文字
*.xml text  # XML 標記為文字
*.config text  # 設定檔標記為文字

# 指令碼
*.sh text eol=lf  # Shell 腳本強制 LF 換行
*.ps1 text eol=crlf  # PowerShell 強制 CRLF 換行
*.bat text eol=crlf  # 批次檔強制 CRLF 換行

# 二進位
*.dll binary  # DLL 標記為二進位
*.exe binary  # EXE 標記為二進位
*.nupkg binary  # NuGet 套件標記為二進位
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：對已推送的 commit 做 interactive rebase

```bash
# 錯誤：已經 push 到遠端的 commit，用 rebase 改寫歷史
git push origin feature/login  # 已經推送了
git rebase -i HEAD~3  # 改寫了已推送的 commit 歷史
git push --force origin feature/login  # 強制推送覆蓋遠端

# ✅ 正確：只對「還沒 push」的 commit 做 rebase
# 或者使用 --force-with-lease（較安全的強制推送）
git push --force-with-lease origin feature/login  # 只有在遠端沒有新 commit 時才會成功
```

**為什麼錯？** 改寫已推送的歷史，其他正在這個分支上工作的人會遇到歷史不一致的問題。如果必須強制推送，用 `--force-with-lease` 比 `--force` 安全，因為它會檢查遠端是否有你不知道的新 commit。

### ❌ 錯誤 2：大檔案沒用 LFS 直接 commit

```bash
# 錯誤：直接把大檔案加入 Git
git add training-data.zip  # 500MB 的檔案直接加入 Git
git commit -m ""新增訓練資料""  # 提交後 repo 暴增 500MB
git push origin main  # push 超慢，而且歷史中永遠佔 500MB

# ✅ 正確：先設定 LFS 再加入
git lfs track ""*.zip""  # 先設定 LFS 追蹤 zip 檔案
git add .gitattributes  # 加入 LFS 設定
git add training-data.zip  # 這次 Git 會用 LFS 處理大檔案
git commit -m ""新增訓練資料（LFS）""  # 提交，Git 只存指標
```

**為什麼錯？** Git 不適合存放大型二進位檔案。每個版本都會完整保存，即使後來刪除，歷史中仍然佔用空間。repo 會越來越大，clone 和 push 都會變慢。

### ❌ 錯誤 3：不知道 reflog，以為 reset --hard 就沒救了

```bash
# 錯誤：以為 reset --hard 後資料就永遠消失了
git reset --hard HEAD~3  # 丟掉最近 3 個 commit
# ""完了完了，我的程式碼不見了...""

# ✅ 正確：用 reflog 救回來
git reflog  # 查看操作歷史
# abc1234 HEAD@{0}: reset: moving to HEAD~3
# def5678 HEAD@{1}: commit: 重要的功能  ← 找到了！

git reset --hard def5678  # 回到 reset 之前的狀態，資料全部救回來
```

**為什麼錯？** 很多人以為 `reset --hard` 是不可逆的操作，其實 Git 的 reflog 會保留所有操作紀錄約 90 天。只要知道 reflog，幾乎所有操作都可以復原。

---

## 📝 本章重點整理

| 技巧 | 用途 | 適用情境 |
|------|------|----------|
| Interactive Rebase | 整理 commit 歷史 | Push 前整理零碎的 commit |
| `git bisect` | 二分搜尋找 Bug | 知道某版本有 Bug 但不確定是哪個 commit |
| `git reflog` | 救回誤刪的資料 | reset --hard 或刪錯分支後的救命工具 |
| Submodule/Subtree | 引用外部儲存庫 | 多專案共用程式碼 |
| Git Hooks | 自動化檢查 | commit 前自動跑測試/格式檢查 |
| Git LFS | 大檔案管理 | 追蹤圖片、影片等大型二進位檔 |
| .gitattributes | 檔案處理規則 | 統一換行符號、標記二進位檔 |
" },
    };
}