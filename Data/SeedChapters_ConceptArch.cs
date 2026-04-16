using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_ConceptArch
{
    public static List<Chapter> GetChapters() => new()
    {
        new() { Id=1800, Category="concept-arch", Order=1, Level="intermediate", Icon="🌐", Title="RESTful API 設計哲學", Slug="concept-restful-philosophy", IsPublished=true, Content=@"
# RESTful API 設計哲學

## REST 不只是 URL 命名規則

很多人以為 REST 就是：
```
GET /api/users      → 取得所有
GET /api/users/1    → 取得單一
POST /api/users     → 新增
PUT /api/users/1    → 更新
DELETE /api/users/1 → 刪除
```

> 這只是表面。REST 的核心是**資源導向**和**無狀態**。

---

## 冪等性（Idempotency）

> 同一個請求執行一次和執行 N 次，結果一樣。

| HTTP 方法 | 冪等？ | 為什麼？ |
|-----------|--------|---------|
| GET | ✅ 是 | 只是查詢，不改變狀態 |
| PUT | ✅ 是 | 整個覆蓋，執行 N 次結果一樣 |
| DELETE | ✅ 是 | 刪第一次成功，之後都是「已刪除」 |
| POST | ❌ 否 | 每次都建立新資源 |
| PATCH | ❌ 否 | 部分更新，可能有副作用 |

```
為什麼重要？
→ 網路不穩定，Request 可能重送
→ 如果 POST 建立訂單被重送 3 次 → 建立 3 筆訂單！
→ 解法：客戶端帶 Idempotency-Key Header
```

---

## 狀態碼語意

```
2xx 成功：
200 OK           → 查詢、更新成功
201 Created      → 新增成功（回傳 Location Header）
204 No Content   → 刪除成功（不回傳 body）

4xx 客戶端錯誤：
400 Bad Request  → 參數格式錯誤
401 Unauthorized → 沒有身份（未登入）
403 Forbidden    → 有身份但沒權限
404 Not Found    → 資源不存在
409 Conflict     → 資源衝突（重複建立）
422 Unprocessable → 參數格式對但語意錯（如：年齡 = -5）

5xx 伺服器錯誤：
500 Internal Server Error → 程式碼有 bug
503 Service Unavailable   → 暫時無法服務
```

> **常見錯誤：所有錯誤都回 200 + `{ success: false }`。這讓客戶端無法用 HTTP 機制處理錯誤。**

---

## API 版本控制

```
方式 1：URL 路徑（最常見）
/api/v1/users
/api/v2/users

方式 2：Header
Accept: application/vnd.myapp.v2+json

方式 3：Query String
/api/users?version=2
```

> **建議用 URL 路徑**：最明確、最好 debug、容易做路由。

---

## PUT vs PATCH

```
PUT：整個資源覆蓋（你必須送完整的物件）
PUT /api/users/1  { name: ""小明"", age: 21, email: ""ming@test.com"" }
→ 漏送 email？email 就變 null

PATCH：部分更新（只送要改的欄位）
PATCH /api/users/1  { age: 21 }
→ 只改 age，其他不動
```

---

## 巢狀資源

```
// 取得使用者的訂單
GET /api/users/1/orders          ✅ 好
GET /api/orders?userId=1         ✅ 也可以

// 最多兩層巢狀
GET /api/users/1/orders/5        ✅ OK
GET /api/users/1/orders/5/items  ⚠️ 太深了

// 太深就改用 query parameter
GET /api/order-items?orderId=5   ✅ 簡潔
```
" },

        new() { Id=1801, Category="concept-arch", Order=2, Level="intermediate", Icon="🔐", Title="認證 vs 授權：Session / JWT / OAuth", Slug="concept-auth-comparison", IsPublished=true, Content=@"
# 認證 vs 授權：Session / JWT / OAuth

## 認證（Authentication）vs 授權（Authorization）

```
認證 = 你是誰？（登入）
授權 = 你能做什麼？（權限）

比喻：
認證 = 公司門禁卡（證明你是員工）
授權 = 門禁卡上的權限（你能進哪些樓層）
```

---

## Session-based 認證

```
1. 使用者登入 → 伺服器建立 Session（存在記憶體/Redis）
2. 回傳 Session ID（放在 Cookie）
3. 每次請求帶 Cookie → 伺服器查 Session 確認身份

優點：
- 伺服器有完全控制權（可以即時踢人下線）
- Cookie 自動帶，前端不用管

缺點：
- 伺服器要存 Session → 多台伺服器要共享（Redis）
- 不適合手機 App（Cookie 支援差）
- CSRF 攻擊風險（Cookie 自動帶）
```

## JWT（JSON Web Token）認證

```
1. 使用者登入 → 伺服器產生 JWT Token
2. 回傳 Token → 前端自己存（localStorage / memory）
3. 每次請求帶 Authorization: Bearer <token>
4. 伺服器驗證 Token 簽名 → 不需要查 DB

JWT 結構：
xxxxx.yyyyy.zzzzz
 ↓      ↓      ↓
Header.Payload.Signature

Payload 裡面有：
{ sub: ""user123"", name: ""小明"", role: ""admin"", exp: 1700000000 }
```

```
優點：
- 無狀態（伺服器不需要存 Session）
- 跨域 / 手機 App 友善
- 可以放自訂資料（Claims）

缺點：
- 無法即時撤銷（Token 發出後直到過期前都有效）
- Token 被偷就完了（要設短過期時間 + Refresh Token）
- 放 localStorage 有 XSS 風險
```

---

## OAuth 2.0

```
場景：「用 Google 登入」

你的網站不需要知道使用者的 Google 密碼
→ 使用者去 Google 登入
→ Google 回傳授權碼（Authorization Code）
→ 你的後端用授權碼換 Access Token
→ 用 Access Token 取得使用者資料

四種授權流程：
1. Authorization Code（最安全，後端用）
2. PKCE（前端 SPA 用）
3. Client Credentials（伺服器對伺服器）
4. Password（已棄用）
```

---

## 比較總覽

| | Session | JWT | OAuth 2.0 |
|---|---|---|---|
| 狀態 | 有狀態（伺服器存 Session） | 無狀態 | 看實作 |
| 適合 | 傳統 MVC 網站 | SPA、手機 App、API | 第三方登入 |
| 能即時撤銷？ | ✅ 刪 Session | ❌ 要等過期 | 看實作 |
| 跨域？ | ❌ Cookie 限制 | ✅ Header 帶 | ✅ |
| 安全風險 | CSRF | XSS（若存 localStorage） | 設定複雜 |

> **你的 DevLearn 用的是 Cookie + Session 模式（BCrypt 密碼 + 登入 Cookie），這對 MVC 網站是最適合的。**
" },

        new() { Id=1802, Category="concept-arch", Order=3, Level="advanced", Icon="⚡", Title="SQL 效能：索引原理與查詢計畫", Slug="concept-sql-index-explain", IsPublished=true, Content=@"
# SQL 效能：索引原理與查詢計畫

## 索引的本質：B-Tree

```
沒有索引的查詢：
┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
│1│2│3│4│5│6│7│8│9│10│  ← 一筆一筆掃描（Full Table Scan）
└─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
找 Id=7 → 掃描 7 次

有索引的查詢（B-Tree）：
        [5]
       /   \
    [2,4]  [7,9]
    / | \   / | \
  [1][3][4][6][8][10]
找 Id=7 → 只需 2 次比較（log₂N）

100 萬筆資料：
- Full Scan: 最多 1,000,000 次
- B-Tree Index: 最多 20 次（log₂ 1000000 ≈ 20）
```

---

## EXPLAIN ANALYZE 怎麼看

```sql
EXPLAIN ANALYZE SELECT * FROM users WHERE email = 'test@gmail.com';
```

```
-- ❌ 壞的查詢計畫（沒有索引）
Seq Scan on users (cost=0.00..1500.00 rows=1 width=100)
  Filter: (email = 'test@gmail.com')
  Rows Removed by Filter: 99999
  Execution Time: 150.5 ms

-- ✅ 好的查詢計畫（有索引）
Index Scan using idx_users_email on users (cost=0.42..8.44 rows=1 width=100)
  Index Cond: (email = 'test@gmail.com')
  Execution Time: 0.05 ms
```

### 關鍵字解讀

| 看到什麼 | 意思 | 好壞 |
|---------|------|------|
| Seq Scan | 全表掃描 | ❌ 慢（除非表很小） |
| Index Scan | 用索引查 | ✅ 快 |
| Bitmap Index Scan | 點陣圖索引 | ✅ 中等（多條件時） |
| Nested Loop | 巢狀迴圈 JOIN | ⚠️ 小表 OK，大表慢 |
| Hash Join | 雜湊 JOIN | ✅ 大表快 |
| Sort | 排序 | ⚠️ 沒索引就要排 |

---

## 索引什麼時候失效？

```sql
-- ❌ 在索引欄位上用函式
WHERE UPPER(name) = 'MIKE'     → 索引失效
WHERE YEAR(created_at) = 2024  → 索引失效

-- ✅ 改寫避免函式
WHERE name = 'Mike'
WHERE created_at >= '2024-01-01' AND created_at < '2025-01-01'

-- ❌ LIKE 開頭用 %
WHERE name LIKE '%明'   → 索引失效
WHERE name LIKE '小%'   → ✅ 索引有效

-- ❌ 隱式型別轉換
WHERE phone = 912345678  → phone 是 VARCHAR，數字觸發轉換 → 索引失效
WHERE phone = '912345678' → ✅
```

---

## 正規化 vs 反正規化

```
正規化（減少重複）：
Users 表：id, name, city_id
Cities 表：id, city_name
→ 要 JOIN 才能取得城市名稱
→ 資料一致性好，但查詢要 JOIN

反正規化（接受重複）：
Users 表：id, name, city_name
→ 不需要 JOIN，查詢快
→ 但城市改名要改所有使用者的記錄

何時反正規化？
- 讀遠多於寫（報表、分析）
- JOIN 太多影響效能
- 資料很少變動
```
" },

        new() { Id=1803, Category="concept-arch", Order=4, Level="intermediate", Icon="💾", Title="快取策略：什麼時候該快取？", Slug="concept-caching-strategy", IsPublished=true, Content=@"
# 快取策略：什麼時候該快取？

## 快取的本質

```
沒有快取：
使用者 → API → 資料庫 → API → 使用者（每次都查 DB，100ms）

有快取：
使用者 → API → 快取命中 → 使用者（1ms）
              ↘ 快取沒命中 → 資料庫 → 存入快取 → 使用者
```

---

## 快取策略比較

### Cache-Aside（旁路快取）— 最常用

```csharp
public async Task<User> GetUser(int id) {
    var cached = await _cache.GetAsync<User>($""user:{id}"");
    if (cached != null) return cached; // 快取命中

    var user = await _db.Users.FindAsync(id); // 查 DB
    await _cache.SetAsync($""user:{id}"", user, TimeSpan.FromMinutes(10)); // 存入快取
    return user;
}
```

```
優點：簡單、應用控制快取邏輯
缺點：第一次一定 miss、可能有短暫的資料不一致
```

### Write-Through（寫穿快取）

```
寫入時同時更新 DB 和快取
→ 快取永遠是最新的
→ 但寫入速度變慢（要寫兩個地方）
```

### Write-Behind（延遲寫入）

```
寫入先更新快取，異步再寫 DB
→ 寫入超快
→ 但有資料遺失風險（快取掛了，DB 還沒更新）
```

---

## 什麼時候該快取？什麼時候不該？

| ✅ 該快取 | ❌ 不該快取 |
|-----------|------------|
| 讀多寫少（商品列表） | 即時性要求高（帳戶餘額） |
| 計算成本高（報表） | 資料頻繁變動（聊天訊息） |
| 資料不常變動（設定檔） | 個人化資料（除非 key 包含 userId） |
| 外部 API 回應 | 安全敏感資料 |

---

## 快取失效策略

```
TTL（Time-To-Live）：設定過期時間
→ 最簡單，但過期前資料可能是舊的

主動失效：資料更新時刪除快取
→ 最精確，但要記得在所有更新點都清快取

版本號：快取 key 包含版本號
→ 資料變更時改版本號 → 舊快取自然失效
```

> **快取最難的不是「怎麼快取」，而是「怎麼失效」。**

---

## 快取穿透、雪崩、擊穿

```
穿透：查一個不存在的 key → 每次都打 DB
→ 解法：快取空值（null 也存，TTL 短一點）

雪崩：大量快取同時過期 → DB 瞬間被打爆
→ 解法：TTL 加隨機值（不要全部同時過期）

擊穿：一個熱點 key 過期 → 大量請求同時查 DB
→ 解法：互斥鎖（只讓一個請求去查 DB，其他等待）
```
" },

        new() { Id=1804, Category="concept-arch", Order=5, Level="advanced", Icon="🏗️", Title="微服務 vs 單體：怎麼選？", Slug="concept-monolith-vs-micro", IsPublished=true, Content=@"
# 微服務 vs 單體：怎麼選？

## 單體架構（Monolith）

```
一個應用程式 = 所有功能
┌──────────────────────┐
│  使用者模組           │
│  訂單模組             │
│  商品模組             │
│  支付模組             │
│  通知模組             │
│  ── 共用一個 DB ──    │
└──────────────────────┘
```

```
優點：
✅ 開發簡單、部署簡單（一個專案、一個部署）
✅ 本地函式呼叫，效能好
✅ 資料一致性容易（同一個 DB、同一個 Transaction）
✅ 除錯容易（一個 process）

缺點：
❌ 整個應用一起部署（改一行程式碼要部署全部）
❌ 一個模組出錯影響全部
❌ 團隊越大越難協作（合併衝突）
❌ 技術選擇被鎖定（整個用同一個框架）
```

---

## 微服務架構

```
每個功能是獨立的服務
┌─────────┐ ┌─────────┐ ┌─────────┐
│ 使用者   │ │ 訂單     │ │ 商品     │
│ Service  │ │ Service  │ │ Service  │
│ [DB1]    │ │ [DB2]    │ │ [DB3]    │
└─────────┘ └─────────┘ └─────────┘
     ↕ HTTP/gRPC/Message Queue ↕

優點：
✅ 獨立部署、獨立擴展
✅ 一個服務掛了不影響其他
✅ 團隊可以用不同技術
✅ 適合大團隊分工

缺點：
❌ 分散式系統的複雜性（網路延遲、服務發現、負載平衡）
❌ 跨服務的 Transaction 很難（Saga Pattern）
❌ 除錯困難（分散式追蹤、Log 聚合）
❌ 運維成本高（每個服務要監控、部署、擴展）
```

---

## 什麼時候該用微服務？

| 你的情況 | 建議 |
|---------|------|
| 1-5 人團隊 | 單體 |
| < 10 萬使用者 | 單體 |
| 新創 MVP | 單體（先驗證商業模式） |
| 10+ 人團隊 | 可以考慮微服務 |
| 需要獨立擴展某個功能 | 微服務 |
| 不同團隊負責不同功能 | 微服務 |

> **Martin Fowler：「幾乎所有成功的微服務架構，都是從單體架構演進過來的。」**

---

## 中間路線：模組化單體

```csharp
// 單體應用內部，按模組拆分
/Modules
  /Users      ← 使用者模組（有自己的 Service、Repository）
  /Orders     ← 訂單模組
  /Products   ← 商品模組
/Shared       ← 共用的東西

// 模組之間透過介面通訊，不直接存取對方的 DB Table
// 未來真的需要拆微服務時，每個模組獨立出去就好
```

> **你的 DevLearn 就是單體架構，而且這是對的。一個人開發的學習平台，微服務只會增加複雜度。**
" },

        new() { Id=1805, Category="concept-arch", Order=6, Level="intermediate", Icon="🐳", Title="Docker 與容器化：為什麼不用 VM？", Slug="concept-docker-why", IsPublished=true, Content=@"
# Docker 與容器化：為什麼不用 VM？

## VM vs Container

```
VM（虛擬機器）：
┌─────────────────┐
│ Your App        │
│ Libraries       │
│ Guest OS (整個作業系統)  │ ← 幾 GB
│ Hypervisor      │
│ Host OS         │
│ Hardware        │
└─────────────────┘
啟動：分鐘級

Container（容器）：
┌─────────────────┐
│ Your App        │
│ Libraries       │ ← 幾十 MB
│ Container Engine (Docker) │
│ Host OS         │
│ Hardware        │
└─────────────────┘
啟動：秒級
```

| | VM | Container |
|---|---|---|
| 大小 | GB 級 | MB 級 |
| 啟動 | 分鐘 | 秒 |
| 隔離性 | 完全隔離（各自有 OS） | 共享 Host OS kernel |
| 效能 | 有虛擬化開銷 | 接近原生效能 |
| 適合 | 需要不同 OS | 同一 OS 上跑多個應用 |

---

## Docker 解決什麼問題？

```
""在我的電腦上可以跑啊！""
↓
Docker：不管在誰的電腦上，環境都一樣

開發環境：macOS + .NET 8 + PostgreSQL 15
測試環境：Ubuntu + .NET 8 + PostgreSQL 15
正式環境：Ubuntu + .NET 8 + PostgreSQL 15
↓
全部用同一個 Docker Image → 環境完全一致
```

---

## Image vs Container

```
Image（映像檔）= 模板（類似 class）
→ 唯讀的
→ 用 Dockerfile 建立
→ 可以分享到 Docker Hub

Container（容器）= 執行中的實例（類似 object）
→ 從 Image 建立
→ 可以有多個 Container 用同一個 Image
→ 有自己的檔案系統和網路
```

---

## Layer（分層）

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0  ← Layer 1（基礎）
COPY ./publish /app                        ← Layer 2（你的程式）
WORKDIR /app                               ← Layer 3（設定）
ENTRYPOINT [""dotnet"", ""MyApp.dll""]        ← Layer 4（啟動指令）
```

```
每層都會被快取：
- 如果 Layer 1 沒變 → 直接用快取
- 只有改動的 Layer 會重新建立
- 所以 Dockerfile 要把不常改的放上面，常改的放下面
```

---

## 什麼時候不需要 Docker？

```
- 個人開發的小專案（直接跑就好）
- 學習階段（先學會跑，再學容器）
- 已經用 PaaS（Azure App Service 已經處理環境了）
- 團隊只有你一個人（沒有環境不一致的問題）
```

> **你的 DevLearn 部署到 Azure App Service，不需要 Docker。Azure 已經處理了環境問題。如果未來要用 Kubernetes 或多容器部署才需要。**
" },

        new() { Id=1806, Category="concept-arch", Order=7, Level="intermediate", Icon="📋", Title="Git 工作流：團隊怎麼協作？", Slug="concept-git-workflow", IsPublished=true, Content=@"
# Git 工作流：團隊怎麼協作？

## Git Flow vs Trunk-Based

### Git Flow

```
main ─────────────────────────────────── (正式版)
  ↕
develop ──┬──┬──┬──┬──┬──────────────── (開發版)
          │  │  │  │  │
feature/  │  │  │  │  │
login ────┘  │  │  │  │
feature/     │  │  │  │
cart ────────┘  │  │  │
release/       │  │  │
v1.0 ──────────┘  │  │
hotfix/           │  │
urgent ───────────┘  │
feature/             │
search ──────────────┘
```

```
適合：發版節奏固定、大型團隊
缺點：分支太多、合併複雜、容易衝突
```

### Trunk-Based（主流推薦）

```
main ─┬─┬─┬─┬─┬─┬─┬─┬─ (每天都可部署)
      │ │ │ │ │ │ │ │
      小 小 小 小 小 小 小 小
      PR PR PR PR PR PR PR PR
```

```
適合：CI/CD 成熟、小型敏捷團隊
原則：分支生命短（< 1 天）、頻繁合併到 main
優點：衝突少、部署快、code review 範圍小
```

---

## Rebase vs Merge

```
Merge：保留完整歷史
A─B─C─────F  (main)
     \   /
      D─E    (feature)
→ 多一個合併 commit（F），歷史有分岔

Rebase：線性歷史
A─B─C─D'─E'  (main)
→ feature 的 commit 被「重新播放」在 main 之後
→ 歷史乾淨，但改寫了 commit hash
```

| | Merge | Rebase |
|---|---|---|
| 歷史 | 保留分支記錄 | 線性乾淨 |
| 安全性 | ✅ 不改寫歷史 | ⚠️ 改寫 commit hash |
| 適合 | 多人協作的 feature branch | 個人 feature branch 整理 |
| 衝突 | 一次解決 | 可能要逐 commit 解決 |

> **金規：只 rebase 自己的 branch，不要 rebase 已經 push 的公共 branch。**

---

## Code Review 文化

```
好的 PR：
- 小（< 400 行）
- 一個 PR 做一件事
- 有清楚的描述（為什麼改、改了什麼）
- 自己先 review 一次

壞的 PR：
- 2000 行改動，看不完
- 標題寫「update」
- 混合了 feature + refactor + bug fix
```
" },

        new() { Id=1807, Category="concept-arch", Order=8, Level="advanced", Icon="🔄", Title="CI/CD：自動化部署為什麼重要？", Slug="concept-cicd", IsPublished=true, Content=@"
# CI/CD：自動化部署為什麼重要？

## CI/CD 是什麼？

```
CI（Continuous Integration）持續整合：
每次 commit → 自動 build → 自動跑測試 → 結果回報
→ 早期發現問題，不要等到部署才爆

CD（Continuous Delivery）持續交付：
CI 通過 → 自動部署到 Staging → 手動核准 → 部署到 Production

CD（Continuous Deployment）持續部署：
CI 通過 → 自動部署到 Production（完全自動，不需要人核准）
```

---

## 沒有 CI/CD 的痛

```
1. ""昨天還能跑的，今天怎麼壞了？""
   → 沒有自動測試，不知道哪個 commit 引入了 bug

2. 部署花 2 小時（手動 build → FTP 上傳 → 改設定 → 重啟）
   → 害怕部署 → 部署越少 → 每次部署的改動越多 → 越容易出錯

3. ""我本地跑得好好的！""
   → CI 確保程式碼在乾淨環境也能 build
```

---

## 你的 DevLearn 的部署流程

```
現在的流程（半自動）：
1. dotnet publish -c Release（手動）
2. python deploy.py（zip + az webapp deploy）
3. 等 Azure 啟動（3-5 分鐘）

理想的流程（CI/CD）：
1. git push → GitHub Actions 自動觸發
2. 自動 build + 自動跑測試
3. 測試通過 → 自動部署到 Azure
4. 部署完成 → 自動通知（LINE / Discord）
```

---

## 部署策略

```
Rolling：逐步更新（一台一台換）
→ 零停機，但新舊版本會同時存在

Blue-Green：兩套環境切換
→ Blue（舊版）跑著，Green（新版）準備好
→ 切換流量到 Green，有問題切回 Blue

Canary：先給少數使用者
→ 新版先給 5% 使用者
→ 沒問題再慢慢開到 100%
```

> **你目前的單一 Azure App Service + zip deploy 是最簡單的方式，對學習平台足夠了。**
" },

        new() { Id=1808, Category="concept-arch", Order=9, Level="advanced", Icon="📐", Title="系統設計基礎：如何思考架構？", Slug="concept-system-design-basics", IsPublished=true, Content=@"
# 系統設計基礎：如何思考架構？

## 設計的思考框架

```
1. 需求分析：系統要做什麼？（功能需求 + 非功能需求）
2. 估算規模：多少使用者？多少資料？QPS 多少？
3. 定義 API：對外提供什麼介面？
4. 資料模型：資料怎麼存？關聯怎麼設計？
5. 核心架構：高層設計圖
6. 細節深入：瓶頸在哪？怎麼優化？
```

---

## 擴展策略

### 垂直擴展（Scale Up）

```
CPU 不夠 → 換更強的 CPU
記憶體不夠 → 加更多 RAM
↓
簡單但有天花板（單台機器有極限）
```

### 水平擴展（Scale Out）

```
一台不夠 → 加更多台
→ 需要負載平衡（Load Balancer）
→ 需要 Session 共享（Redis）
→ 需要資料庫複製（Read Replica）
↓
理論上無限擴展，但複雜度高
```

---

## 常見架構元件

```
使用者
  ↓
CDN（靜態資源快取）
  ↓
Load Balancer（負載平衡）
  ↓
Web Server × N（水平擴展）
  ↓       ↓
Redis    Database
(快取)   (主從複製)
  ↓
Message Queue（非同步處理）
  ↓
Worker Service（背景任務）
```

---

## 資料庫擴展

```
讀寫分離：
Master（寫入）→ 複製到 → Replica 1, 2, 3（讀取）
→ 適合讀多寫少的應用

分片（Sharding）：
使用者 1-100萬 → DB Shard 1
使用者 100萬-200萬 → DB Shard 2
→ 適合資料量極大的場景
→ 但跨 Shard 查詢很困難

NoSQL：
MongoDB（文件型）、Redis（鍵值型）、Cassandra（列式）
→ 適合特定場景，不是替代 SQL
```

---

## CAP 定理

```
分散式系統只能同時滿足三個中的兩個：
C — Consistency（一致性）：所有節點看到的資料一樣
A — Availability（可用性）：每個請求都能得到回應
P — Partition Tolerance（分區容錯）：網路分區時系統繼續運作

實務上 P 必須要有（網路一定會出問題），所以選擇：
CP：保證一致性，犧牲可用性（如：銀行系統）
AP：保證可用性，犧牲一致性（如：社群平台的按讚數）
```

> **重點不是背這些，而是能分析特定場景的需求，選擇合適的架構。**
" },

        new() { Id=1809, Category="concept-arch", Order=10, Level="intermediate", Icon="🧪", Title="測試策略：該測什麼？測多少？", Slug="concept-testing-strategy", IsPublished=true, Content=@"
# 測試策略：該測什麼？測多少？

## 測試金字塔

```
        △
       / \
      / E2E \        ← 少（慢、貴、脆弱）
     /───────\
    /Integration\    ← 中等
   /─────────────\
  /  Unit Tests   \  ← 多（快、便宜、穩定）
 /─────────────────\
```

| 類型 | 測什麼 | 速度 | 數量 |
|------|--------|------|------|
| Unit | 單一方法/類別的邏輯 | 毫秒 | 多（70%） |
| Integration | 多個元件的互動（API + DB） | 秒 | 中等（20%） |
| E2E | 整個使用者流程 | 分鐘 | 少（10%） |

---

## 什麼該測、什麼不該測

```
✅ 該測：
- 商業邏輯（折扣計算、訂單狀態轉換）
- 邊界條件（空值、極端數字、空字串）
- 已知的 bug（寫測試防止再發生）

❌ 不該測：
- 框架本身（EF Core 的 CRUD 不需要你測）
- Getter/Setter（沒有邏輯的屬性）
- Private 方法（透過 public 方法間接測試）
- 外部 API（用 mock）
```

---

## 好的測試 vs 壞的測試

```csharp
// ❌ 壞的測試：測試實作細節
[Test]
public void CreateOrder_ShouldCallRepository() {
    _service.CreateOrder(order);
    _mockRepo.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
    // 如果改了內部實作（不用 Repository），測試就壞了
}

// ✅ 好的測試：測試行為結果
[Test]
public void CreateOrder_ShouldReturnOrderWithCorrectTotal() {
    var result = _service.CreateOrder(items);
    Assert.Equal(150m, result.Total); // 只關心結果，不關心怎麼做到的
}
```

---

## Mock vs 真實依賴

```
用 Mock：
- 外部 API（不想真的打）
- Email 服務（不想真的寄信）
- 不可控的依賴（時間、隨機數）

用真實依賴：
- 資料庫（Integration Test 用測試 DB）
- 檔案系統（簡單的讀寫可以用真的）

原則：Unit Test 用 Mock，Integration Test 用真實依賴
```

> **測試的目的是讓你有信心改程式碼，而不是追求 100% 覆蓋率。80% 有意義的覆蓋率 > 100% 無意義的覆蓋率。**
" },
    };
}
