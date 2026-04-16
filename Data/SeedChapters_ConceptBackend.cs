using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_ConceptBackend
{
    public static List<Chapter> GetChapters() => new()
    {
        new() { Id=1700, Category="concept-backend", Order=1, Level="intermediate", Icon="💉", Title="DI 依賴注入：為什麼不直接 new？", Slug="concept-di-why", IsPublished=true, Content=@"
# DI 依賴注入：為什麼不直接 new？

## 先看問題：直接 new 有什麼不好？

```csharp
public class OrderService {
    private readonly SqlOrderRepository _repo = new SqlOrderRepository(); // ← 直接 new
    private readonly SmtpEmailService _email = new SmtpEmailService();     // ← 直接 new
}
```

> 表面上可以動，但隱藏了三個嚴重問題：

### 問題 1：無法替換（緊耦合）

```
OrderService 直接依賴 SqlOrderRepository
→ 換成 MongoOrderRepository？要改 OrderService 的程式碼
→ 違反「開閉原則」（對擴展開放，對修改封閉）
```

### 問題 2：無法測試

```csharp
// 想測試 OrderService 的邏輯，但它直接 new 了真的資料庫連線
// 你不能注入一個假的 Repository → 無法做單元測試
[Test]
public void CreateOrder_ShouldSendEmail() {
    var service = new OrderService(); // ← 會真的連資料庫！測試環境沒有 DB 就爆了
}
```

### 問題 3：無法管理生命週期

```
new SqlOrderRepository() → 每次都建新連線
→ 連線池爆掉
→ 你無法控制「整個 Request 共用一個」或「全域只有一個」
```

---

## DI 怎麼解決？

### 核心概念：把依賴「注入」進來，不要自己 new

```csharp
// ❌ 直接 new（控制權在 OrderService 內部）
public class OrderService {
    private readonly SqlOrderRepository _repo = new SqlOrderRepository();
}

// ✅ 依賴注入（控制權在外部）
public class OrderService {
    private readonly IOrderRepository _repo;

    public OrderService(IOrderRepository repo) { // ← 從外部注入
        _repo = repo;
    }
}
```

> **控制反轉（IoC）**：「我不自己建立依賴，我要求別人給我。」

---

## 三種生命週期

```csharp
builder.Services.AddSingleton<ICacheService, RedisCacheService>();  // 全域一個
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>(); // 每個 Request 一個
builder.Services.AddTransient<IEmailService, SmtpEmailService>();   // 每次注入都 new 一個
```

| 生命週期 | 何時建立 | 何時銷毀 | 適合場景 |
|---------|---------|---------|---------|
| Singleton | 應用啟動時 | 應用關閉時 | 快取、設定、HttpClient |
| Scoped | 每個 HTTP Request | Request 結束時 | DbContext、Repository |
| Transient | 每次注入時 | 超出作用域時 | 輕量無狀態服務 |

### 常見陷阱：Captive Dependency

```
Singleton 注入 Scoped → ❌ Scoped 物件被 Singleton 抓住，永遠不會釋放
→ DbContext 被快取，資料永遠是舊的
→ ASP.NET Core 預設會拋出例外提醒你
```

---

## 不用 DI 的替代方案

| 方式 | 做法 | 缺點 |
|------|------|------|
| 直接 new | `new SqlRepo()` | 緊耦合、不可測試 |
| Service Locator | `ServiceLocator.Get<IRepo>()` | 隱藏依賴、難以追蹤 |
| Factory Pattern | `RepoFactory.Create()` | 比 DI 囉唆，但有時合理 |
| 靜態方法 | `OrderRepo.GetAll()` | 無法 mock、全域狀態 |

> DI 不是唯一選擇，但在 Web 應用中是**最適合的**，因為 HTTP Request 的生命週期天然適合 Scoped。

---

## 什麼時候「不需要」DI？

- 工具類（`Math.Max()`、`string.Format()`）→ 靜態就好
- 簡單的值物件（DTO、Record）→ 直接 new
- 一次性的 Console App → 過度設計
- 沒有替換需求、不需要測試的小程式

> **原則：如果一個類別有「行為」且你可能想替換或測試它，就用 DI。**

---

## 面試角度

```
Q: 為什麼要用 DI？
A: 解耦合、可測試、生命週期管理。讓類別不需要知道依賴的具體實作。

Q: Scoped 和 Singleton 差在哪？
A: Scoped 是每個 Request 一個實例（適合 DbContext），Singleton 全域共用（適合快取）。

Q: 什麼是 IoC？
A: 控制反轉。傳統是類別自己 new 依賴，IoC 是由外部（容器）提供依賴。DI 是 IoC 的一種實現方式。
```
" },

        new() { Id=1701, Category="concept-backend", Order=2, Level="intermediate", Icon="🏛️", Title="SOLID 原則：好的程式碼長怎樣？", Slug="concept-solid", IsPublished=true, Content=@"
# SOLID 原則：好的程式碼長怎樣？

> SOLID 不是教你「怎麼寫」，而是教你「什麼時候該拆、該抽、該改」。

---

## S — 單一職責原則（Single Responsibility）

> 一個類別只應該有一個改變的理由。

```csharp
// ❌ 壞：一個類別做太多事
public class UserService {
    public void CreateUser(User user) { /* 建立使用者 */ }
    public void SendWelcomeEmail(User user) { /* 寄信 */ }
    public string GenerateReport() { /* 產報表 */ }
}

// ✅ 好：拆成各自負責
public class UserService { public void CreateUser(User user) { } }
public class EmailService { public void SendWelcome(User user) { } }
public class ReportService { public string Generate() { } }
```

**判斷方法**：描述這個類別時，如果用到「而且」就該拆。
「UserService 負責建立使用者**而且**寄信**而且**產報表」→ 拆！

---

## O — 開閉原則（Open/Closed）

> 對擴展開放，對修改封閉。

```csharp
// ❌ 壞：每加一種折扣就要改 if-else
public decimal CalculateDiscount(string type, decimal price) {
    if (type == ""vip"") return price * 0.8m;
    if (type == ""student"") return price * 0.9m;
    // 新增類型 → 改這裡 → 可能影響其他
}

// ✅ 好：用多型擴展
public interface IDiscount { decimal Apply(decimal price); }
public class VipDiscount : IDiscount { public decimal Apply(decimal price) => price * 0.8m; }
public class StudentDiscount : IDiscount { public decimal Apply(decimal price) => price * 0.9m; }
// 新增折扣 → 新增類別 → 不改舊程式碼
```

---

## L — 里氏替換原則（Liskov Substitution）

> 子類別必須能完全替代父類別使用。

```csharp
// ❌ 壞：正方形繼承長方形，但行為不一致
class Rectangle { virtual void SetWidth(int w); virtual void SetHeight(int h); }
class Square : Rectangle {
    override void SetWidth(int w) { Width = w; Height = w; } // 改寬也改高 → 違反預期
}

// 呼叫端預期：改寬不影響高
Rectangle r = new Square();
r.SetWidth(5);
r.SetHeight(10);
// 預期面積 50，實際面積 100 → 破壞了替換性
```

**判斷方法**：把子類別的物件傳到只認識父類別的函式，行為會不會「出乎意料」？

---

## I — 介面隔離原則（Interface Segregation）

> 不要強迫類別實作它用不到的方法。

```csharp
// ❌ 壞：一個大介面
public interface IWorker {
    void Work();
    void Eat();
    void Sleep();
}
// 機器人要實作 Eat()？Sleep()？

// ✅ 好：拆成小介面
public interface IWorkable { void Work(); }
public interface IFeedable { void Eat(); }
public class Robot : IWorkable { public void Work() { } }
public class Human : IWorkable, IFeedable { public void Work() { } public void Eat() { } }
```

---

## D — 依賴反轉原則（Dependency Inversion）

> 高層模組不應依賴低層模組，兩者都該依賴抽象。

```
❌ OrderService → SqlOrderRepository（直接依賴具體類別）
✅ OrderService → IOrderRepository ← SqlOrderRepository（都依賴介面）
```

> 這就是 DI 的理論基礎。

---

## SOLID 不是教條

| 原則 | 過度使用的症狀 |
|------|-------------|
| SRP | 一個方法拆成 10 個類別，看不懂流程 |
| OCP | 三行 if-else 也要搞 Strategy Pattern |
| LSP | 完全不用繼承（其實簡單場景繼承很好用） |
| ISP | 每個介面只有一個方法，介面數量爆炸 |
| DIP | Console App 也搞 DI Container |

> **原則是指南，不是法律。先讓程式碼能動、好讀，再考慮 SOLID。**
" },

        new() { Id=1702, Category="concept-backend", Order=3, Level="advanced", Icon="⏳", Title="async/await 的真相：不是多執行緒", Slug="concept-async-truth", IsPublished=true, Content=@"
# async/await 的真相：不是多執行緒

## 最大的誤解

```
❌ ""async/await 會開新執行緒處理""
✅ ""async/await 讓執行緒在等待時去做別的事""
```

---

## 比喻

你在餐廳點餐：
- **同步**：你站在櫃檯等，餐好了才離開（執行緒卡住等 I/O）
- **非同步**：你點完拿號碼牌回座位，叫號再去拿（執行緒被釋放，I/O 完成後回來）

```csharp
// 同步：執行緒等 1 秒，這段時間什麼都不能做
var data = httpClient.GetStringAsync(url).Result; // ← 阻塞！

// 非同步：執行緒被釋放去處理其他 Request
var data = await httpClient.GetStringAsync(url); // ← 不阻塞
```

---

## I/O-bound vs CPU-bound

| 類型 | 例子 | 該用什麼 |
|------|------|---------|
| I/O-bound | 資料庫查詢、HTTP 請求、檔案讀寫 | `await`（釋放執行緒等 I/O） |
| CPU-bound | 影像處理、加密計算、大量迴圈 | `Task.Run()`（真的開新執行緒） |

```csharp
// I/O-bound → await
var users = await _db.Users.ToListAsync(); // 執行緒被釋放等 DB 回應

// CPU-bound → Task.Run
var hash = await Task.Run(() => ComputeExpensiveHash(data)); // 丟到背景執行緒
```

---

## 為什麼 Web 需要 async？

```
同步的 Web Server（假設 100 個執行緒）：
→ 100 個 Request 同時進來
→ 每個都在等 DB（500ms）
→ 第 101 個 Request：「沒有空的執行緒了！」→ 503 Service Unavailable

非同步的 Web Server：
→ 100 個 Request 同時進來
→ await DB 時，執行緒被釋放
→ 釋放的執行緒去處理第 101、102... 個 Request
→ DB 回應後，繼續處理原本的 Request
→ 同樣 100 個執行緒，能處理數千個並行 Request
```

---

## 常見陷阱

### 1. async void（別用）

```csharp
// ❌ async void：例外無法被 catch
async void BadMethod() { throw new Exception(); } // 直接崩潰

// ✅ async Task：例外會被 Task 包住
async Task GoodMethod() { throw new Exception(); } // 可以被 await catch
```

### 2. .Result / .Wait()（別用）

```csharp
// ❌ 同步等待非同步 → 可能死鎖
var data = GetDataAsync().Result; // ASP.NET 中會死鎖！

// ✅ 一路 await 到底
var data = await GetDataAsync();
```

### 3. 不需要 async 的 async

```csharp
// ❌ 多餘的 async（只是轉傳）
async Task<User> GetUser(int id) {
    return await _repo.GetAsync(id); // 多一層狀態機，沒意義
}

// ✅ 直接回傳 Task
Task<User> GetUser(int id) {
    return _repo.GetAsync(id); // 少一層包裝，效能更好
}
```

---

## 執行緒 vs Task

```
Thread：作業系統層級，建立成本高（~1MB stack）
Task：CLR 層級，使用 ThreadPool，輕量
async/await：語法糖，編譯器轉成狀態機（State Machine）
```

> await 不等於開新 Thread。await 只是告訴編譯器：「這裡可以暫停，等結果回來再繼續。」
" },

        new() { Id=1703, Category="concept-backend", Order=4, Level="intermediate", Icon="🔍", Title="LINQ 延遲執行與效能陷阱", Slug="concept-linq-deferred", IsPublished=true, Content=@"
# LINQ 延遲執行與效能陷阱

## 什麼是延遲執行？

```csharp
var query = db.Users.Where(u => u.Age > 18); // ← 這行不會查資料庫！
// query 只是一個「查詢計畫」，還沒執行

var list = query.ToList(); // ← 這行才真的送 SQL 到資料庫
```

> **延遲執行（Deferred Execution）**：LINQ 查詢只在你「列舉」時才執行。

---

## IEnumerable vs IQueryable

| | IEnumerable<T> | IQueryable<T> |
|---|---|---|
| 執行位置 | 記憶體（C# 端） | 資料庫（SQL 端） |
| 篩選時機 | 先全部載入，再篩選 | 篩選條件轉成 SQL |
| 適合 | 記憶體中的集合 | EF Core 資料庫查詢 |

```csharp
// ❌ 效能災難：載入全部使用者到記憶體再篩選
IEnumerable<User> users = db.Users; // 載入 100 萬筆
var adults = users.Where(u => u.Age > 18); // 在 C# 記憶體中篩選

// ✅ 正確：篩選條件在資料庫執行
IQueryable<User> users = db.Users; // 還沒查
var adults = users.Where(u => u.Age > 18).ToList(); // WHERE age > 18（SQL 端篩選）
```

---

## 常見陷阱

### 1. 多次列舉

```csharp
var query = db.Users.Where(u => u.IsActive);

var count = query.Count();     // ← 查一次 DB
var list = query.ToList();     // ← 又查一次 DB！
var first = query.First();     // ← 又查一次！

// ✅ 先 ToList()，再對記憶體操作
var list = query.ToList();     // 查一次
var count = list.Count;        // 記憶體操作
var first = list.First();      // 記憶體操作
```

### 2. Select N+1

```csharp
// ❌ 每個 Order 都會查一次 Customer（N+1 問題）
var orders = db.Orders.ToList();
foreach (var o in orders) {
    Console.WriteLine(o.Customer.Name); // 每次都查 DB！
}

// ✅ Include 一次載入
var orders = db.Orders.Include(o => o.Customer).ToList();
```

### 3. 在迴圈裡用 LINQ 查詢

```csharp
// ❌ 每次迴圈都送一次 SQL
foreach (var id in userIds) {
    var user = db.Users.FirstOrDefault(u => u.Id == id); // N 次查詢
}

// ✅ 一次撈完
var users = db.Users.Where(u => userIds.Contains(u.Id)).ToList(); // 1 次查詢
```

---

## ToList() vs AsEnumerable() vs AsNoTracking()

```csharp
.ToList()         // 立即執行，載入到記憶體（List<T>）
.AsEnumerable()   // 切換到 LINQ to Objects（之後的操作在記憶體）
.AsNoTracking()   // 不追蹤實體變化（唯讀查詢效能提升 30-50%）
```

> **原則：只讀不改的查詢，一律加 `.AsNoTracking()`。**
" },

        new() { Id=1704, Category="concept-backend", Order=5, Level="intermediate", Icon="🗃️", Title="EF Core：Repository Pattern 要不要用？", Slug="concept-ef-repository", IsPublished=true, Content=@"
# EF Core：Repository Pattern 要不要用？

## 爭議：DbContext 本身就是 Repository + Unit of Work

```
EF Core 的 DbContext 已經實作了：
- Repository Pattern → DbSet<T> 就是 Repository
- Unit of Work → SaveChanges() 就是 Commit

再包一層 Repository，不就是多此一舉？
```

---

## 支持用 Repository 的理由

```csharp
// 1. 抽象化：Controller 不直接依賴 EF Core
public interface IUserRepository {
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetActiveUsersAsync();
}

// 好處：換 ORM（Dapper）或換 DB（MongoDB）不影響上層
// 好處：單元測試可以 mock IUserRepository
```

## 反對用 Repository 的理由

```csharp
// 1. 「洩漏抽象」：你最終還是會暴露 IQueryable
public interface IUserRepository {
    IQueryable<User> GetAll(); // ← 這不就是 DbSet 嗎？
}

// 2. 重複包裝：每個 CRUD 都要寫一層
public async Task<User?> GetByIdAsync(int id) {
    return await _db.Users.FindAsync(id); // 一行就結束，何必包？
}

// 3. 複雜查詢怎麼辦？
// 最終 Repository 會變成一個巨大的「查詢方法集」
GetUsersByAgeAndCityAndStatusOrderByNamePaginated(...)
```

---

## 實務建議

| 場景 | 建議 |
|------|------|
| 小型專案（< 20 表） | 直接用 DbContext，不需要 Repository |
| 中型專案 | 用 Service Layer + DbContext |
| 需要換 ORM 的可能性 | 用 Repository |
| 需要大量單元測試 | 用 Repository（方便 mock） |
| 複雜的查詢邏輯 | 用 Specification Pattern 或 Query Object |

```csharp
// 推薦的中間路線：Service Layer（不需要 Repository）
public class UserService {
    private readonly AppDbContext _db;
    public UserService(AppDbContext db) => _db = db;

    public async Task<User?> GetActiveUser(int id) {
        return await _db.Users
            .AsNoTracking()
            .Where(u => u.Id == id && u.IsActive)
            .FirstOrDefaultAsync();
    }
}
// 直接在 Service 裡寫查詢邏輯，不多包一層
```

> **結論：沒有絕對的對錯。重要的是統一團隊風格，而不是追求「正確」的架構。**
" },

        new() { Id=1705, Category="concept-backend", Order=6, Level="intermediate", Icon="🔀", Title="Middleware vs Filter：什麼時候用哪個？", Slug="concept-middleware-vs-filter", IsPublished=true, Content=@"
# Middleware vs Filter：什麼時候用哪個？

## 執行順序

```
HTTP Request 進來
  ↓
Middleware 1（最外層）
  ↓
Middleware 2
  ↓
Middleware 3（路由、認證、授權...）
  ↓
[進入 MVC 管線]
  ↓
  Authorization Filter
  ↓
  Resource Filter（Before）
  ↓
  Action Filter（Before）
  ↓
  Controller Action 執行
  ↓
  Action Filter（After）
  ↓
  Result Filter
  ↓
  Resource Filter（After）
  ↓
[離開 MVC 管線]
  ↓
Middleware 3, 2, 1（反向回程）
  ↓
HTTP Response 出去
```

---

## 關鍵差異

| | Middleware | Filter |
|---|---|---|
| 作用範圍 | **所有 Request**（包含靜態檔案） | **只有 MVC/API Action** |
| 能存取什麼 | HttpContext | ActionContext、ModelState、ActionArguments |
| 適合做什麼 | 日誌、CORS、壓縮、認證 | 驗證、授權、快取、例外處理 |
| 可以短路嗎 | ✅ 不呼叫 next() | ✅ 設定 Result |
| 執行順序控制 | 註冊順序決定 | 可以設定 Order |
| 可以用 DI 嗎 | 建構函式注入 | 支援（用 ServiceFilter） |

---

## 選擇指南

```
要處理所有 HTTP 請求？ → Middleware
  例：全域日誌、CORS、Response 壓縮、速率限制

只處理 MVC Action？ → Filter
  例：模型驗證、授權檢查、Action 計時

需要存取 ModelState？ → Filter
需要存取 ActionArguments？ → Filter
需要在路由之前執行？ → Middleware
```

### 實際案例

```csharp
// Middleware：記錄每個 Request 的時間（包含靜態檔案）
app.Use(async (context, next) => {
    var sw = Stopwatch.StartNew();
    await next();
    Console.WriteLine($""{context.Request.Path} took {sw.ElapsedMilliseconds}ms"");
});

// Filter：檢查 Action 的模型是否有效
public class ValidateModelFilter : IActionFilter {
    public void OnActionExecuting(ActionExecutingContext context) {
        if (!context.ModelState.IsValid)
            context.Result = new BadRequestObjectResult(context.ModelState);
    }
    public void OnActionExecuted(ActionExecutedContext context) { }
}
```

> **簡單記法：Middleware 管 HTTP 層面，Filter 管 MVC 層面。**
" },

        new() { Id=1706, Category="concept-backend", Order=7, Level="intermediate", Icon="📦", Title="值型別 vs 參考型別：記憶體怎麼運作？", Slug="concept-value-vs-reference", IsPublished=true, Content=@"
# 值型別 vs 參考型別：記憶體怎麼運作？

## 核心差異

```
Stack（堆疊）     Heap（堆積）
├── int a = 5     ├── string s = ""hello"" → [h,e,l,l,o]
├── bool b = true ├── int[] arr = {1,2,3} → [1,2,3]
├── char c = 'A'  ├── User u = new() → {Name=""小明"", Age=20}
└── 快速、自動清理  └── 較慢、需要 GC 回收
```

| | 值型別（Value Type） | 參考型別（Reference Type） |
|---|---|---|
| 存在哪 | Stack | Heap（Stack 放指標） |
| 複製行為 | 複製整個值 | 複製指標（共享資料） |
| 例子 | int, bool, char, struct, enum | string, class, array, interface |
| 預設值 | 0, false, '\0' | null |

---

## 複製行為的差異（面試必考）

```csharp
// 值型別：複製值
int a = 5;
int b = a;    // b 是獨立副本
b = 10;
Console.WriteLine(a); // → 5（不受影響）

// 參考型別：複製指標
int[] arr1 = { 1, 2, 3 };
int[] arr2 = arr1;  // arr2 指向同一個陣列
arr2[0] = 99;
Console.WriteLine(arr1[0]); // → 99（被改了！）
```

---

## Boxing / Unboxing

```csharp
int x = 42;
object obj = x;      // Boxing：值型別 → 堆積（包裝成 object）
int y = (int)obj;    // Unboxing：堆積 → 值型別（拆包）
```

> Boxing 有效能成本：每次都要在 Heap 配置記憶體。大量 Boxing 會造成 GC 壓力。

---

## string 的特殊性

```csharp
string a = ""hello"";
string b = a;
b = ""world"";
Console.WriteLine(a); // → ""hello""（沒被改）
```

> string 是參考型別，但行為像值型別。因為 string 是**不可變的（immutable）**。
> 每次 ""修改"" string，其實是建立新的 string 物件。

```csharp
// ❌ 效能差：每次 += 都建立新 string
string result = """";
for (int i = 0; i < 10000; i++) {
    result += i.ToString(); // 10000 次記憶體配置！
}

// ✅ 用 StringBuilder
var sb = new StringBuilder();
for (int i = 0; i < 10000; i++) {
    sb.Append(i);
}
string result = sb.ToString(); // 一次配置
```

---

## struct vs class

```csharp
// struct（值型別）：小型、不可變的資料用
public struct Point { public int X; public int Y; }

// class（參考型別）：有行為、需要繼承、較大的物件用
public class User { public string Name; public int Age; }
```

| 用 struct | 用 class |
|-----------|----------|
| 小於 16 bytes | 大型物件 |
| 不需要繼承 | 需要繼承 |
| 不可變（immutable） | 可變（mutable） |
| 頻繁建立和銷毀 | 長生命週期 |
" },

        new() { Id=1707, Category="concept-backend", Order=8, Level="advanced", Icon="🧬", Title="泛型與反射：為什麼需要？", Slug="concept-generics-reflection", IsPublished=true, Content=@"
# 泛型與反射：為什麼需要？

## 泛型：一份程式碼，適用所有型別

### 沒有泛型的世界

```csharp
// 要為每種型別寫一個方法？
int MaxInt(int a, int b) => a > b ? a : b;
double MaxDouble(double a, double b) => a > b ? a : b;
string MaxString(string a, string b) => a.CompareTo(b) > 0 ? a : b;
// 無限重複...

// 或者用 object？
object Max(object a, object b) { ... }
// 問題：沒有型別安全、需要 Boxing、呼叫端要強制轉型
```

### 泛型解決方案

```csharp
T Max<T>(T a, T b) where T : IComparable<T> {
    return a.CompareTo(b) > 0 ? a : b;
}

// 一個方法適用所有可比較的型別
Max(5, 10);          // T = int
Max(3.14, 2.72);     // T = double
Max(""abc"", ""xyz"");   // T = string
```

### 泛型的好處

| 好處 | 說明 |
|------|------|
| 型別安全 | 編譯時期檢查，不用強制轉型 |
| 效能 | 值型別不需要 Boxing |
| 重用 | 一份程式碼適用所有型別 |
| 約束 | `where T : IComparable` 限制能用的型別 |

---

## 反射：在執行時期檢查和操作型別

### 什麼時候需要反射？

```csharp
// 1. 框架（ASP.NET、EF Core 大量使用反射）
// ASP.NET 怎麼知道你的 Controller 有哪些 Action？→ 反射掃描
// EF Core 怎麼知道你的 Entity 有哪些屬性？→ 反射讀取

// 2. 序列化
// JSON.Serialize 怎麼知道物件有哪些屬性？→ 反射

// 3. 外掛系統
// 動態載入 DLL，建立不知道型別的物件
```

### 反射的代價

```
反射比直接呼叫慢 10-100 倍
→ 不要在迴圈裡用反射
→ 框架會快取反射結果來減少開銷
→ 現在有 Source Generator 可以在編譯時期產生程式碼取代反射
```

> **原則：應用程式碼很少需要直接用反射。如果你在寫業務邏輯時用到反射，通常代表設計可以改進。**
" },

        new() { Id=1708, Category="concept-backend", Order=9, Level="intermediate", Icon="⚠️", Title="例外處理哲學：該 catch 什麼？", Slug="concept-exception-philosophy", IsPublished=true, Content=@"
# 例外處理哲學：該 catch 什麼？

## 原則：不要 catch 你不知道怎麼處理的例外

```csharp
// ❌ 最糟糕的寫法：吞掉例外
try {
    DoSomething();
} catch (Exception) {
    // 什麼都不做 → 錯誤被隱藏，debug 時找不到原因
}

// ❌ 也很糟：catch 所有例外然後記 log 繼續跑
try {
    ProcessOrder(order);
} catch (Exception ex) {
    _logger.LogError(ex, ""Error"");
    // 然後呢？訂單沒處理成功但程式繼續跑？
}

// ✅ 好：只 catch 你知道怎麼處理的
try {
    var data = await httpClient.GetAsync(url);
} catch (HttpRequestException ex) {
    // 網路錯誤 → 回傳快取的資料
    return _cache.Get(cacheKey);
}
// 其他例外讓它自然往上拋
```

---

## 全域例外處理

```csharp
// ASP.NET Core：用 Middleware 統一處理
app.UseExceptionHandler(""/Error"");

// 或自訂 Middleware
app.Use(async (context, next) => {
    try {
        await next();
    } catch (NotFoundException ex) {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    } catch (Exception ex) {
        _logger.LogError(ex, ""Unhandled"");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = ""伺服器錯誤"" });
    }
});
```

---

## 自訂例外 vs 回傳值

```csharp
// 方式 1：拋例外（適合「不應該發生」的錯誤）
public User GetUser(int id) {
    return _db.Users.Find(id) ?? throw new NotFoundException($""User {id} not found"");
}

// 方式 2：Result Pattern（適合「可預期的失敗」）
public Result<User> GetUser(int id) {
    var user = _db.Users.Find(id);
    return user != null ? Result.Ok(user) : Result.Fail(""找不到使用者"");
}
```

| | 拋例外 | Result Pattern |
|---|---|---|
| 適合場景 | 程式錯誤、不預期的狀況 | 業務邏輯的「失敗」 |
| 效能 | 例外有效能成本 | 無額外成本 |
| 強制處理 | 不強制（可能忘了 catch） | 強制（必須檢查 Result） |

> **簡單記法：「找不到使用者」是業務邏輯（用 Result），「資料庫連不上」是基礎設施錯誤（用例外）。**
" },

        new() { Id=1709, Category="concept-backend", Order=10, Level="intermediate", Icon="🎭", Title="設計模式：不要為了用而用", Slug="concept-design-patterns-practical", IsPublished=true, Content=@"
# 設計模式：不要為了用而用

## 什麼時候「不需要」設計模式？

```csharp
// ❌ 過度設計：三行邏輯套了 Strategy Pattern
public interface IGreetingStrategy { string Greet(string name); }
public class MorningGreeting : IGreetingStrategy { public string Greet(string name) => $""早安 {name}""; }
public class EveningGreeting : IGreetingStrategy { public string Greet(string name) => $""晚安 {name}""; }
public class GreetingContext { ... }

// ✅ 簡單就好
string Greet(string name, bool isMorning) => isMorning ? $""早安 {name}"" : $""晚安 {name}"";
```

> **先寫出能動的程式碼，遇到痛點時再用模式解決。**

---

## 最常用的 5 個模式（實際場景）

### 1. Strategy — 行為可替換

```
場景：支付方式（信用卡、Line Pay、Apple Pay）
痛點：if-else 越來越多，每次加支付方式都要改核心程式碼
解法：每種支付方式是一個 Strategy，OrderService 只依賴介面
```

### 2. Factory — 建立物件的邏輯複雜

```
場景：根據設定檔決定要用 SQL Server 還是 PostgreSQL
痛點：建立 DbContext 需要很多參數和判斷
解法：Factory 封裝建立邏輯，呼叫端只需要 factory.Create()
```

### 3. Observer — 事件通知

```
場景：訂單成立後要寄信、推播、記 log、更新庫存
痛點：OrderService 直接呼叫 4 個 Service，耦合嚴重
解法：OrderService 發出事件，各個 Observer 各自處理
（C# 中用 event、MediatR、或訊息佇列）
```

### 4. Decorator — 替現有功能加料

```
場景：Repository 加快取、加 log、加重試
痛點：不想改原本的 Repository 程式碼
解法：CachingRepository 包住原本的 Repository
```

### 5. Builder — 建立複雜物件

```
場景：建立一封 Email（有收件人、主旨、本文、附件、CC、BCC...）
痛點：建構函式參數太多
解法：EmailBuilder.To(...).Subject(...).Body(...).Build()
```

---

## 判斷要不要用模式

```
1. 這段程式碼現在有什麼問題？（不是「未來可能有」）
2. 用了模式會更好讀嗎？（還是更難懂？）
3. 團隊能理解嗎？（如果只有你懂，那是壞的架構）
```

> **最好的程式碼不是用了最多模式的，而是最容易理解的。**
" },
    };
}
