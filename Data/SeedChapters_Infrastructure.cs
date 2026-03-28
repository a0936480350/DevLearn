using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Infrastructure
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── 基礎建設 Chapter 300 ─────────────────────────────────────
        new() { Id=300, Category="infrastructure", Order=1, Level="intermediate", Icon="💾", Title="快取策略 Caching", Slug="infra-caching", IsPublished=true, Content=@"
# 快取策略 Caching

## 什麼是快取（Cache）？

快取就是把經常需要的資料「暫時存放在離你比較近的地方」，下次需要時就不用再跑一趟遠路。

> 💡 **比喻：便利商店 vs 工廠**
> 你想喝一瓶可樂：
> - **沒有快取** = 每次都開車去可口可樂工廠買（資料庫查詢）
> - **有快取** = 巷口便利商店就有（記憶體中的資料）
> - 便利商店的庫存有限，而且飲料會過期（快取有容量和時效限制）
> - 但是取貨速度快了 100 倍！

```
為什麼需要快取？

沒有快取：
使用者 → API → 資料庫查詢（10ms-100ms）→ 回傳結果
使用者 → API → 資料庫查詢（10ms-100ms）→ 回傳結果
每次都要查資料庫，資料庫壓力山大！

有快取：
使用者 → API → 快取命中（<1ms）→ 回傳結果 ✅ 超快！
使用者 → API → 快取未命中 → 資料庫查詢 → 存入快取 → 回傳結果
第一次慢，之後都超快！
```

---

## IMemoryCache：記憶體內快取

ASP.NET Core 內建的記憶體快取，資料存在應用程式的記憶體中。

```csharp
// 1. 在 Program.cs 註冊服務
// 加入記憶體快取服務到 DI 容器
builder.Services.AddMemoryCache();

// 2. 在 Controller 或 Service 中注入使用
using Microsoft.Extensions.Caching.Memory;

public class ProductService
{
    // 注入 IMemoryCache
    private readonly IMemoryCache _cache;
    // 注入資料庫 Context
    private readonly AppDbContext _db;

    // 透過建構式注入取得快取和資料庫
    public ProductService(IMemoryCache cache, AppDbContext db)
    {
        _cache = cache;
        _db = db;
    }

    // 取得所有產品（有快取版本）
    public async Task<List<Product>> GetAllProductsAsync()
    {
        // 定義快取的 Key（每種資料用不同的 Key）
        var cacheKey = ""products_all"";

        // TryGetValue：嘗試從快取取得資料
        // 如果快取有資料，直接回傳（不用查資料庫）
        if (_cache.TryGetValue(cacheKey, out List<Product>? products))
        {
            // 快取命中！直接回傳
            return products!;
        }

        // 快取未命中，從資料庫查詢
        products = await _db.Products.ToListAsync();

        // 設定快取選項
        var cacheOptions = new MemoryCacheEntryOptions()
            // 絕對過期時間：5 分鐘後一定過期
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            // 滑動過期時間：2 分鐘沒人存取就過期
            .SetSlidingExpiration(TimeSpan.FromMinutes(2));

        // 把資料存入快取
        _cache.Set(cacheKey, products, cacheOptions);

        // 回傳資料庫查詢的結果
        return products;
    }
}
```

---

## Set、Get、TryGetValue

```csharp
// IMemoryCache 的三個核心方法

// 1. Set：存入快取
// 把 ""hello"" 這個值存入快取，Key 是 ""greeting""
_cache.Set(""greeting"", ""hello"");

// Set 也可以指定過期時間
// 存入快取，10 分鐘後過期
_cache.Set(""user_count"", 42, TimeSpan.FromMinutes(10));

// Set 搭配 MemoryCacheEntryOptions 做更細的設定
var options = new MemoryCacheEntryOptions()
    // 快取大小（搭配 SizeLimit 使用）
    .SetSize(1)
    // 優先順序：記憶體不足時，Low 的會先被清除
    .SetPriority(CacheItemPriority.High);
// 用選項存入快取
_cache.Set(""important_data"", myData, options);

// 2. Get：取得快取（可能為 null）
// 從快取取值，如果不存在會回傳 null
var greeting = _cache.Get<string>(""greeting"");
// 要檢查是否為 null
if (greeting != null)
{
    // 使用快取的值
    Console.WriteLine(greeting);
}

// 3. TryGetValue：安全地取得快取（推薦用法）
// 回傳 bool 表示是否有找到，值透過 out 參數傳出
if (_cache.TryGetValue(""user_count"", out int count))
{
    // 有找到快取，count 已經有值了
    Console.WriteLine($""使用者數量：{count}"");
}
else
{
    // 快取中沒有這個 Key
    Console.WriteLine(""快取中找不到資料"");
}
```

---

## 快取過期策略：Absolute vs Sliding

```
兩種過期策略的比較：

絕對過期（Absolute Expiration）：
├── 設定一個固定的過期時間
├── 不管有沒有人存取，時間到就過期
├── 比喻：便當的有效期限，不管你有沒有吃，時間到就丟
└── 適合：資料有時效性，像是匯率、天氣

滑動過期（Sliding Expiration）：
├── 每次被存取就重新計時
├── 只有「連續 N 分鐘沒人存取」才會過期
├── 比喻：圖書館的書，有人借就延長，沒人借才下架
└── 適合：熱門資料，越多人用就越值得保留

最佳實踐：兩個一起用！
├── Sliding = 2 分鐘（沒人用就釋放記憶體）
└── Absolute = 30 分鐘（保證資料不會太舊）
```

```csharp
// 同時設定兩種過期策略
var options = new MemoryCacheEntryOptions()
    // 滑動過期：2 分鐘沒人存取就過期
    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
    // 絕對過期：不管多熱門，30 分鐘後一定更新
    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

// 存入快取
_cache.Set(""hot_data"", myData, options);

// 範例：GetOrCreate 簡化寫法（推薦！）
// GetOrCreateAsync 會自動處理「有就拿、沒有就建立」的邏輯
var products = await _cache.GetOrCreateAsync(""products"", async entry =>
{
    // 設定快取選項
    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
    entry.SlidingExpiration = TimeSpan.FromMinutes(2);

    // 這裡的程式碼只有在快取未命中時才會執行
    return await _db.Products.ToListAsync();
});
```

---

## 分散式快取：Redis 概念

```
記憶體快取的限制：
├── 只存在單一伺服器的記憶體中
├── 伺服器重啟就消失
└── 多台伺服器無法共享

分散式快取（Redis）：
├── 獨立的快取伺服器
├── 所有應用程式伺服器共享同一份快取
├── 伺服器重啟也不會消失
└── 支援更複雜的資料結構

比喻：
├── IMemoryCache = 每個員工桌上的便條紙（只有自己看得到）
└── Redis = 公司公告欄（所有人都看得到）

什麼時候用哪種？
├── 單一伺服器、資料量小 → IMemoryCache
├── 多台伺服器、需要共享 → Redis
└── 兩者可以搭配使用（多層快取）
```

```csharp
// ASP.NET Core 使用 Redis 分散式快取
// 1. 安裝套件：dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis

// 2. 在 Program.cs 註冊 Redis 快取服務
builder.Services.AddStackExchangeRedisCache(options =>
{
    // Redis 連線字串
    options.Configuration = ""localhost:6379"";
    // 設定 Key 的前綴（避免跟其他應用程式衝突）
    options.InstanceName = ""MyApp_"";
});

// 3. 注入 IDistributedCache 使用
using Microsoft.Extensions.Caching.Distributed;

public class ProductService
{
    // 分散式快取介面
    private readonly IDistributedCache _cache;

    // 透過建構式注入
    public ProductService(IDistributedCache cache)
    {
        _cache = cache;
    }

    // 取得產品（Redis 快取版本）
    public async Task<string?> GetProductJsonAsync(string productId)
    {
        // Redis 存的是 byte[] 或 string，不是物件
        var cachedJson = await _cache.GetStringAsync($""product:{productId}"");

        // 如果快取有資料就直接回傳
        if (cachedJson != null)
            return cachedJson;

        // 快取沒有，從資料庫查並存入快取
        var product = await _db.Products.FindAsync(productId);
        // 序列化成 JSON 字串
        var json = JsonSerializer.Serialize(product);

        // 設定分散式快取選項
        var options = new DistributedCacheEntryOptions
        {
            // 絕對過期時間
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        // 存入 Redis
        await _cache.SetStringAsync($""product:{productId}"", json, options);

        // 回傳 JSON
        return json;
    }
}
```

---

## Cache-Aside 模式

```
Cache-Aside（旁路快取）是最常見的快取模式：

讀取流程：
1. 先查快取
2. 快取命中 → 直接回傳
3. 快取未命中 → 查資料庫 → 結果存入快取 → 回傳

寫入流程：
1. 更新資料庫
2. 刪除（或更新）快取
3. 下次讀取時會自動重新建立快取

比喻：查字典
1. 先看筆記本有沒有抄過（快取）
2. 有 → 直接看筆記本
3. 沒有 → 查字典（資料庫）→ 抄到筆記本 → 告訴你答案
```

```csharp
// Cache-Aside 完整範例
public class OrderService
{
    // 快取服務
    private readonly IMemoryCache _cache;
    // 資料庫
    private readonly AppDbContext _db;

    // 建構式注入
    public OrderService(IMemoryCache cache, AppDbContext db)
    {
        _cache = cache;
        _db = db;
    }

    // 讀取：先查快取，沒有再查資料庫
    public async Task<Order?> GetOrderAsync(int orderId)
    {
        // 組合快取 Key
        var key = $""order:{orderId}"";

        // 使用 GetOrCreateAsync 簡化 Cache-Aside 邏輯
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            // 設定 10 分鐘過期
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

            // 只有快取沒有時才會執行資料庫查詢
            return await _db.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        });
    }

    // 寫入：更新資料庫後，清除快取
    public async Task UpdateOrderAsync(Order order)
    {
        // 先更新資料庫
        _db.Orders.Update(order);
        await _db.SaveChangesAsync();

        // 刪除快取（下次讀取時會重新建立）
        _cache.Remove($""order:{order.Id}"");
        // 也要清除相關的快取（例如訂單列表）
        _cache.Remove(""orders_all"");
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：Cache Stampede（快取雪崩）

```csharp
// ❌ 錯誤：快取過期時，大量請求同時打到資料庫
// 假設有 1000 個使用者同時存取
public async Task<List<Product>> GetProductsBadAsync()
{
    // 快取過期的瞬間，1000 個請求都發現快取沒了
    if (!_cache.TryGetValue(""products"", out List<Product>? products))
    {
        // 1000 個請求同時查資料庫！資料庫炸了！
        products = await _db.Products.ToListAsync();
        // 1000 個請求都在設定同一個快取
        _cache.Set(""products"", products, TimeSpan.FromMinutes(5));
    }
    return products!;
}

// ✅ 正確：用 SemaphoreSlim 防止重複查詢
private static readonly SemaphoreSlim _semaphore = new(1, 1);

public async Task<List<Product>> GetProductsGoodAsync()
{
    // 先嘗試取快取（不需要鎖）
    if (_cache.TryGetValue(""products"", out List<Product>? products))
        return products!;

    // 只讓一個請求去查資料庫，其他的等
    await _semaphore.WaitAsync();
    try
    {
        // 再次檢查快取（可能在等待期間已經被其他人設定了）
        if (_cache.TryGetValue(""products"", out products))
            return products!;

        // 只有一個請求會執行這裡
        products = await _db.Products.ToListAsync();
        _cache.Set(""products"", products, TimeSpan.FromMinutes(5));
        return products;
    }
    finally
    {
        // 記得釋放鎖
        _semaphore.Release();
    }
}
```

### ❌ 錯誤 2：快取了使用者專屬資料卻用通用 Key

```csharp
// ❌ 錯誤：所有使用者共用同一個快取 Key
public async Task<UserProfile> GetProfileBadAsync(int userId)
{
    // 所有使用者都用同一個 Key，會拿到別人的資料！
    return await _cache.GetOrCreateAsync(""user_profile"", async entry =>
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
        // 使用者 A 的資料被快取後，使用者 B 也會拿到 A 的資料！
        return await _db.Users.FindAsync(userId);
    });
}

// ✅ 正確：每個使用者用不同的 Key
public async Task<UserProfile> GetProfileGoodAsync(int userId)
{
    // 用 userId 區分不同使用者的快取
    var key = $""user_profile:{userId}"";
    return await _cache.GetOrCreateAsync(key, async entry =>
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
        // 每個使用者各自有自己的快取
        return await _db.Users.FindAsync(userId);
    });
}
```

### ❌ 錯誤 3：快取了過時的資料卻不清除

```csharp
// ❌ 錯誤：更新資料庫但忘記清除快取
public async Task UpdateProductBadAsync(Product product)
{
    // 更新了資料庫
    _db.Products.Update(product);
    await _db.SaveChangesAsync();
    // 忘記清除快取！使用者會一直看到舊資料！
}

// ✅ 正確：更新資料後同時清除快取
public async Task UpdateProductGoodAsync(Product product)
{
    // 更新資料庫
    _db.Products.Update(product);
    await _db.SaveChangesAsync();

    // 清除相關快取（確保下次讀取時拿到最新資料）
    _cache.Remove($""product:{product.Id}"");
    // 也要清除列表快取
    _cache.Remove(""products_all"");
}
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| IMemoryCache | ASP.NET Core 內建的記憶體快取，適合單機 |
| Redis | 分散式快取，適合多伺服器架構 |
| Absolute Expiration | 固定時間後過期，不管有沒有被存取 |
| Sliding Expiration | 沒人存取才過期，被存取就重新計時 |
| Cache-Aside | 先查快取，沒有再查資料庫，結果存入快取 |
| Cache Stampede | 快取過期瞬間大量請求打到資料庫，需要加鎖防護 |
" },

        // ── 基礎建設 Chapter 301 ─────────────────────────────────────
        new() { Id=301, Category="infrastructure", Order=2, Level="intermediate", Icon="📝", Title="日誌系統 Logging", Slug="infra-logging", IsPublished=true, Content=@"
# 日誌系統 Logging

## 為什麼需要日誌（Logging）？

日誌就是你的程式在執行過程中留下的紀錄，讓你知道發生了什麼事。

> 💡 **比喻：飛機的黑盒子**
> - 飛機正常飛行時，黑盒子持續記錄各種數據
> - 一旦發生事故，調查員靠黑盒子還原當時的狀況
> - 日誌就是你程式的「黑盒子」
> - 系統上線後出了問題，你沒辦法用 breakpoint 除錯
> - 你只能靠日誌來還原問題發生的經過

```
沒有日誌時：
使用者：「你的系統剛剛壞了！」
工程師：「什麼時候？發生了什麼？」
使用者：「不知道，反正就是壞了。」
工程師：「……」（無從查起）

有日誌時：
使用者：「你的系統剛剛壞了！」
工程師：（打開日誌）
[2024-01-15 14:30:22] ERROR: 資料庫連線逾時，連線字串：Server=db01
[2024-01-15 14:30:22] ERROR: 重試 3 次後仍然失敗
[2024-01-15 14:30:23] CRITICAL: 訂單服務無法處理請求
工程師：「找到了！是資料庫 db01 掛了。」
```

---

## ILogger<T>：ASP.NET Core 內建日誌

```csharp
// ASP.NET Core 已經內建日誌功能，不需要額外安裝套件
// 透過 DI 注入 ILogger<T> 就能使用

using Microsoft.Extensions.Logging;

public class OrderController : ControllerBase
{
    // 注入 ILogger，泛型參數用目前的類別名稱
    // 這樣日誌會自動標記是哪個類別產生的
    private readonly ILogger<OrderController> _logger;

    // 建構式注入
    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderRequest request)
    {
        // 記錄一般資訊
        _logger.LogInformation(""開始建立訂單，使用者：{UserId}"", request.UserId);

        try
        {
            // 處理訂單邏輯...
            var order = await _orderService.CreateAsync(request);

            // 記錄成功訊息
            _logger.LogInformation(""訂單建立成功，訂單編號：{OrderId}"", order.Id);

            return Ok(order);
        }
        catch (Exception ex)
        {
            // 記錄錯誤訊息（包含例外物件）
            _logger.LogError(ex, ""建立訂單失敗，使用者：{UserId}"", request.UserId);

            return StatusCode(500, ""系統錯誤，請稍後再試"");
        }
    }
}
```

---

## Log Levels：日誌等級

```
日誌等級由低到高（越高越嚴重）：

等級          數值   用途                      比喻
──────────────────────────────────────────────────────
Trace          0    最詳細的追蹤資訊            偵探的隨身筆記（每個細節）
Debug          1    開發除錯用的資訊            工程師的草稿紙
Information    2    一般流程記錄                航海日誌（正常航行紀錄）
Warning        3    不正常但還能運作            黃燈警告（注意但不停車）
Error          4    發生錯誤，某個操作失敗       紅燈（出事了！）
Critical       5    系統即將崩潰的嚴重錯誤       火災警報（快逃！）

設定某個等級後，只有「等於或高於」該等級的日誌才會被記錄。
例如設定 Warning，則 Warning、Error、Critical 會被記錄，
Trace、Debug、Information 會被忽略。
```

```csharp
// 各種日誌等級的使用範例

public class PaymentService
{
    // 注入日誌服務
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(ILogger<PaymentService> logger)
    {
        _logger = logger;
    }

    public async Task ProcessPaymentAsync(PaymentRequest request)
    {
        // Trace：非常詳細的追蹤資訊（通常只在本機開發時開啟）
        _logger.LogTrace(""進入 ProcessPaymentAsync，參數：{@Request}"", request);

        // Debug：開發除錯用
        _logger.LogDebug(""開始驗證支付金額：{Amount}"", request.Amount);

        // Information：一般業務流程紀錄
        _logger.LogInformation(""處理付款，訂單：{OrderId}，金額：{Amount}"",
            request.OrderId, request.Amount);

        // Warning：不正常但系統還能運作
        if (request.Amount > 100000)
        {
            _logger.LogWarning(""大額交易警告！訂單：{OrderId}，金額：{Amount}"",
                request.OrderId, request.Amount);
        }

        try
        {
            // 呼叫金流 API...
            await CallPaymentGateway(request);
        }
        catch (TimeoutException ex)
        {
            // Error：操作失敗
            _logger.LogError(ex, ""付款閘道逾時，訂單：{OrderId}"", request.OrderId);
            throw;
        }
        catch (Exception ex)
        {
            // Critical：系統層級的嚴重錯誤
            _logger.LogCritical(ex, ""付款系統完全無法使用！"");
            throw;
        }
    }
}
```

---

## 結構化日誌（Structured Logging）

```
傳統日誌 vs 結構化日誌：

傳統日誌（純文字）：
""2024-01-15 使用者 123 購買了產品 456，金額 999 元""
→ 要搜尋「使用者 123 的所有訂單」很困難（只能用字串搜尋）

結構化日誌（有欄位）：
{
    ""Timestamp"": ""2024-01-15"",
    ""Message"": ""使用者購買產品"",
    ""UserId"": 123,
    ""ProductId"": 456,
    ""Amount"": 999
}
→ 可以直接查詢 WHERE UserId = 123（像查資料庫一樣！）
```

```csharp
// ASP.NET Core 的 ILogger 天生支援結構化日誌

// ❌ 錯誤：用字串拼接（無法被結構化解析）
_logger.LogInformation(""使用者 "" + userId + "" 購買了產品 "" + productId);
// 也不要用 $""..."" 字串插值
_logger.LogInformation($""使用者 {userId} 購買了產品 {productId}"");

// ✅ 正確：用佔位符（Placeholder），讓日誌框架結構化處理
// {UserId} 和 {ProductId} 會被當作獨立的欄位存儲
_logger.LogInformation(""使用者 {UserId} 購買了產品 {ProductId}"",
    userId, productId);

// 這樣在日誌查詢平台（如 Seq、Kibana）就能：
// - WHERE UserId = 123
// - GROUP BY ProductId
// - 對 Amount 做統計分析

// 記錄完整物件：用 @ 前綴做解構
var order = new { Id = 1, Total = 999, Items = 3 };
// @ 前綴會把物件序列化成 JSON 記錄
_logger.LogInformation(""訂單資訊：{@Order}"", order);
// 輸出：訂單資訊：{ Id: 1, Total: 999, Items: 3 }
```

---

## Serilog：更強大的日誌套件

```csharp
// 1. 安裝 Serilog 套件
// dotnet add package Serilog.AspNetCore
// dotnet add package Serilog.Sinks.Console
// dotnet add package Serilog.Sinks.File

// 2. 在 Program.cs 設定 Serilog
using Serilog;

// 設定 Serilog 日誌管線
Log.Logger = new LoggerConfiguration()
    // 最低記錄等級
    .MinimumLevel.Information()
    // 覆寫特定命名空間的等級（減少微軟框架的雜訊）
    .MinimumLevel.Override(""Microsoft"", Serilog.Events.LogEventLevel.Warning)
    // 輸出到 Console（有顏色標示）
    .WriteTo.Console()
    // 輸出到檔案（每天一個檔案，保留 30 天）
    .WriteTo.File(""logs/app-.log"",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    // 建立 Logger
    .CreateLogger();

// 把 Serilog 設定為 ASP.NET Core 的日誌提供者
builder.Host.UseSerilog();

// 3. 使用方式跟 ILogger 完全一樣！
// 因為 Serilog 實作了 ILogger 介面
// 你的 Controller 和 Service 不用改任何程式碼
```

---

## 日誌輸出目標（Sinks）

```csharp
// Serilog 可以同時輸出到多個目標

Log.Logger = new LoggerConfiguration()
    // 輸出到 Console（開發時方便看）
    .WriteTo.Console(
        // 自訂輸出格式
        outputTemplate: ""[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}""
    )
    // 輸出到檔案（正式環境基本需求）
    .WriteTo.File(""logs/app-.log"",
        // 每天換一個新檔案
        rollingInterval: RollingInterval.Day,
        // 單一檔案最大 10MB
        fileSizeLimitBytes: 10_000_000,
        // 超過大小就建新檔
        rollOnFileSizeLimit: true,
        // 保留最近 30 個檔案
        retainedFileCountLimit: 30
    )
    // 輸出到 Seq（結構化日誌查詢平台）
    // .WriteTo.Seq(""http://localhost:5341"")
    .CreateLogger();
```

---

## appsettings.json 設定日誌等級

```json
// appsettings.json - 透過設定檔控制日誌等級
{
  // 日誌設定區塊
  ""Logging"": {
    ""LogLevel"": {
      // 預設等級：Information（記錄一般資訊以上）
      ""Default"": ""Information"",
      // 微軟框架的日誌等級設高一點（減少雜訊）
      ""Microsoft.AspNetCore"": ""Warning"",
      // EF Core 的 SQL 查詢日誌（開發時可以打開看 SQL）
      ""Microsoft.EntityFrameworkCore.Database.Command"": ""Information""
    }
  },
  // Serilog 專用設定
  ""Serilog"": {
    ""MinimumLevel"": {
      // 預設最低等級
      ""Default"": ""Information"",
      // 覆寫特定命名空間
      ""Override"": {
        // 微軟框架只記錄 Warning 以上
        ""Microsoft"": ""Warning"",
        // EF Core 只記錄 Warning 以上
        ""Microsoft.EntityFrameworkCore"": ""Warning""
      }
    }
  }
}
```

```json
// appsettings.Development.json - 開發環境可以開更多日誌
{
  // 開發環境的日誌設定
  ""Logging"": {
    ""LogLevel"": {
      // 開發時記錄更詳細的資訊
      ""Default"": ""Debug"",
      // 可以看到 EF Core 產生的 SQL
      ""Microsoft.EntityFrameworkCore.Database.Command"": ""Information""
    }
  }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：在日誌中記錄敏感資料

```csharp
// ❌ 錯誤：把密碼、信用卡號寫進日誌
_logger.LogInformation(""使用者登入，帳號：{Email}，密碼：{Password}"",
    email, password);
// 日誌可能存在檔案、傳到遠端伺服器，任何看到日誌的人都能看到密碼！

// ❌ 錯誤：記錄完整的信用卡號
_logger.LogInformation(""付款成功，卡號：{CardNumber}"", cardNumber);

// ✅ 正確：永遠不要記錄敏感資料
_logger.LogInformation(""使用者登入，帳號：{Email}"", email);
// 信用卡只記錄後四碼
_logger.LogInformation(""付款成功，卡號尾碼：{CardLast4}"",
    cardNumber[^4..]);
```

### ❌ 錯誤 2：使用錯誤的日誌等級

```csharp
// ❌ 錯誤：所有東西都用 Information
_logger.LogInformation(""系統即將崩潰！記憶體不足！"");  // 這應該是 Critical！
_logger.LogInformation(""找不到使用者 123"");             // 這可能是 Warning
_logger.LogInformation(""變數 x 的值是 42"");             // 這應該是 Debug

// ✅ 正確：根據嚴重程度選擇適當的等級
_logger.LogCritical(""系統即將崩潰！記憶體不足！"");    // 最嚴重的錯誤
_logger.LogWarning(""找不到使用者 {UserId}"", 123);     // 不正常但不致命
_logger.LogDebug(""變數 x 的值是 {Value}"", 42);        // 除錯用的資訊
```

### ❌ 錯誤 3：不用結構化日誌

```csharp
// ❌ 錯誤：用字串拼接或插值
_logger.LogInformation($""使用者 {userId} 在 {DateTime.Now} 購買了 {productName}"");
// 問題 1：無法在日誌平台做結構化查詢
// 問題 2：效能差（即使日誌等級設定會跳過這條，字串還是會被拼接）

// ✅ 正確：用訊息模板（Message Template）
_logger.LogInformation(
    ""使用者 {UserId} 在 {PurchaseTime} 購買了 {ProductName}"",
    userId, DateTime.Now, productName);
// 每個佔位符都是可查詢的結構化欄位
// 如果日誌等級設定會跳過，佔位符不會被處理（效能更好）
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| ILogger<T> | ASP.NET Core 內建的日誌介面 |
| Log Levels | Trace < Debug < Information < Warning < Error < Critical |
| 結構化日誌 | 用佔位符而非字串拼接，讓日誌可以被查詢分析 |
| Serilog | 強大的第三方日誌套件，支援多種輸出目標 |
| Sinks | 日誌的輸出目標：Console、File、Seq、Elasticsearch 等 |
| appsettings.json | 透過設定檔控制不同命名空間的日誌等級 |
" },

        // ── 基礎建設 Chapter 302 ─────────────────────────────────────
        new() { Id=302, Category="infrastructure", Order=3, Level="advanced", Icon="🚀", Title="效能優化與 Profiling", Slug="infra-performance", IsPublished=true, Content=@"
# 效能優化與 Profiling

## 記憶體管理：Value Types vs Reference Types

在 .NET 中，了解資料存放在哪裡是效能優化的第一步。

> 💡 **比喻：口袋 vs 置物櫃**
> - **Value Type（值型別）** = 東西直接放口袋裡（Stack）
>   - 小東西：鑰匙、零錢（int, bool, struct）
>   - 拿出來就能用，不用跑去別的地方找
> - **Reference Type（參考型別）** = 口袋放一張「置物櫃號碼牌」（Stack 上放指標）
>   - 大東西放在置物櫃裡（Heap）
>   - 要先看號碼牌，再跑去置物櫃拿

```csharp
// Value Type：直接存在 Stack 上
// int, double, bool, char, struct, enum 都是 Value Type
int x = 42;        // 42 直接放在 Stack 上
int y = x;         // 複製一份 42 給 y
y = 100;           // 改 y 不影響 x
Console.WriteLine(x);  // 輸出 42（x 沒有被改到）

// Reference Type：Stack 上存指標，物件在 Heap 上
// class, string, array, delegate 都是 Reference Type
var list1 = new List<int> { 1, 2, 3 };  // 物件在 Heap，list1 是指標
var list2 = list1;  // 複製的是指標，不是物件！
list2.Add(4);       // 透過 list2 修改，list1 也會被影響
Console.WriteLine(list1.Count);  // 輸出 4！因為指向同一個物件

// 效能差異
// Value Type：複製很快（直接複製值）
// Reference Type：建立物件需要在 Heap 上分配記憶體
// Heap 分配比 Stack 慢，而且需要 GC 回收
```

---

## 垃圾回收 GC 的運作原理

> 💡 **比喻：垃圾車**
> - 你家不斷產生垃圾（new 出來的物件）
> - 垃圾車（GC）會定期來收垃圾
> - 但垃圾車來的時候，整條街的人都要停下來等（STW - Stop The World）
> - 所以垃圾產生得越多，垃圾車來得越頻繁，大家等越久

```
.NET GC 的三代回收機制：

Gen 0（第 0 代）：新生兒病房
├── 剛 new 出來的物件都在這裡
├── 回收最頻繁，但也最快
├── 大部分物件在這裡就被回收了（朝生暮死）
└── 比喻：紙杯，用完就丟

Gen 1（第 1 代）：觀察室
├── 從 Gen 0 存活下來的物件
├── 回收頻率適中
└── 比喻：通過面試的實習生，再觀察看看

Gen 2（第 2 代）：長期住戶
├── 長期存活的物件
├── 回收最慢，代價最高（Full GC）
├── 靜態變數、長壽物件都在這裡
└── 比喻：正式員工，解雇成本高

LOH（Large Object Heap）：大型物件堆
├── 超過 85,000 bytes 的物件
├── 直接進入 Gen 2
├── 回收代價非常高
└── 比喻：大型家具，搬運費很貴
```

```csharp
// 查看 GC 統計資訊
// 取得 Gen 0 回收次數
Console.WriteLine($""Gen 0 回收次數：{GC.CollectionCount(0)}"");
// 取得 Gen 1 回收次數
Console.WriteLine($""Gen 1 回收次數：{GC.CollectionCount(1)}"");
// 取得 Gen 2 回收次數
Console.WriteLine($""Gen 2 回收次數：{GC.CollectionCount(2)}"");
// 取得目前記憶體使用量（bytes）
Console.WriteLine($""記憶體使用量：{GC.GetTotalMemory(false):N0} bytes"");

// ⚠️ 不要手動呼叫 GC.Collect()！
// GC.Collect();  ← 除非你非常確定在做什麼，否則不要這樣做
// 手動觸發 Full GC 會造成效能問題
```

---

## Span<T> 與 Memory<T>

```csharp
// Span<T>：不需要複製就能操作陣列的片段
// 比喻：用手指框住書本的某幾行，不需要影印出來

// 傳統方式：要複製一份子陣列
var numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
// Substring 或 Array.Copy 會建立新的陣列（分配 Heap 記憶體）
var subset = numbers[3..7];  // 複製了一份新陣列 [4, 5, 6, 7]

// Span<T>：直接指向原始陣列的一部分（零複製！）
Span<int> span = numbers.AsSpan(3, 4);  // 指向 [4, 5, 6, 7]，沒有複製
// 可以讀取和修改（修改會影響原始陣列）
span[0] = 99;  // numbers[3] 也變成 99 了
Console.WriteLine(numbers[3]);  // 輸出 99

// 字串處理的效能提升
var text = ""Hello, World! Welcome to .NET"";

// 傳統方式：每次 Substring 都會建立新字串（Heap 分配）
var hello = text.Substring(0, 5);     // 新字串 ""Hello""
var world = text.Substring(7, 6);     // 新字串 ""World!""

// Span 方式：零記憶體分配
ReadOnlySpan<char> textSpan = text.AsSpan();
// 直接指向原始字串的片段，不建立新字串
var helloSpan = textSpan.Slice(0, 5);  // 指向 ""Hello""
var worldSpan = textSpan.Slice(7, 6);  // 指向 ""World!""

// Memory<T>：可以存在 Heap 上的 Span（可以用於 async 方法）
// Span<T> 只能存在 Stack 上，不能用在 async 方法中
// Memory<T> 解決了這個限制
Memory<int> memory = numbers.AsMemory(3, 4);
// 需要操作時再轉成 Span
Span<int> fromMemory = memory.Span;
```

---

## StringBuilder vs 字串串接

```csharp
// 字串是不可變的（Immutable）！
// 每次用 + 串接都會建立新字串

// ❌ 糟糕的效能：每次迴圈都建立新字串物件
string result = "";
for (int i = 0; i < 10000; i++)
{
    // 每次 += 都會：1. 建立新字串 2. 複製舊內容 3. 加上新內容
    // 10000 次迴圈 = 10000 個暫時字串物件（GC 壓力大）
    result += $""第 {i} 行\n"";
}
// 時間複雜度：O(n²)，因為每次都要複製越來越長的字串

// ✅ 正確：使用 StringBuilder
var sb = new StringBuilder();
for (int i = 0; i < 10000; i++)
{
    // StringBuilder 內部維護一個可變的 char 陣列
    // 不需要每次都建立新字串
    sb.AppendLine($""第 {i} 行"");
}
// 最後才轉成字串（只建立一次）
string result2 = sb.ToString();
// 時間複雜度：O(n)，效能差距可達 100 倍以上！

// 小量串接（2-3 個）用 + 就好，不需要 StringBuilder
// 編譯器會自動優化成 string.Concat
var name = firstName + "" "" + lastName;  // 這樣沒問題
```

---

## async 效能最佳實踐

```csharp
// 1. 不要在 async 方法中做不必要的 await
// ❌ 不必要的 async/await（多了一層狀態機的開銷）
async Task<int> GetValueBadAsync()
{
    // 如果只是回傳另一個 Task，不需要 async/await
    return await _service.GetValueAsync();
}

// ✅ 直接回傳 Task（少一層狀態機）
Task<int> GetValueGoodAsync()
{
    // 直接回傳 Task，讓呼叫端去 await
    return _service.GetValueAsync();
}
// ⚠️ 注意：如果有 try-catch 或 using，還是需要 async/await

// 2. 善用 Task.WhenAll 做平行處理
// ❌ 循序執行：每個 await 都要等前一個完成
async Task<DashboardData> GetDashboardBadAsync()
{
    // 三個獨立的查詢，卻是循序執行
    var users = await _db.Users.CountAsync();           // 等 100ms
    var orders = await _db.Orders.CountAsync();         // 再等 100ms
    var products = await _db.Products.CountAsync();     // 再等 100ms
    // 總共 300ms！
    return new DashboardData(users, orders, products);
}

// ✅ 平行執行：三個查詢同時進行
async Task<DashboardData> GetDashboardGoodAsync()
{
    // 先啟動所有 Task（不 await）
    var usersTask = _db.Users.CountAsync();
    var ordersTask = _db.Orders.CountAsync();
    var productsTask = _db.Products.CountAsync();

    // 等待所有 Task 同時完成
    await Task.WhenAll(usersTask, ordersTask, productsTask);
    // 總共只要 100ms（取最慢的那個）

    return new DashboardData(
        await usersTask,    // 已經完成了，不會再等
        await ordersTask,
        await productsTask
    );
}

// 3. ConfigureAwait(false) 在程式庫中使用
// 在沒有 UI 的程式庫中，不需要回到原始的 SynchronizationContext
async Task<string> LibraryMethodAsync()
{
    // ConfigureAwait(false) 告訴執行時不需要回到原始的執行緒
    var data = await _httpClient.GetStringAsync(""https://api.example.com"")
        .ConfigureAwait(false);
    // 在 ASP.NET Core 中通常不需要（因為沒有 SynchronizationContext）
    // 但在撰寫 NuGet 套件時建議加上
    return data;
}
```

---

## BenchmarkDotNet 基礎

```csharp
// 安裝：dotnet add package BenchmarkDotNet

// BenchmarkDotNet 幫你精確測量程式碼的效能
// 它會自動：暖機、多次執行、統計分析、排除雜訊

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

// 標記記憶體分配診斷（可以看到 GC 分配量）
[MemoryDiagnoser]
public class StringBenchmarks
{
    // 設定測試參數
    [Params(100, 1000, 10000)]
    public int N;

    // 測試方法 1：字串串接
    [Benchmark]
    public string StringConcat()
    {
        // 用 + 串接字串
        var result = """";
        for (int i = 0; i < N; i++)
            result += ""a"";
        return result;
    }

    // 測試方法 2：StringBuilder
    [Benchmark]
    public string StringBuilderAppend()
    {
        // 用 StringBuilder
        var sb = new StringBuilder();
        for (int i = 0; i < N; i++)
            sb.Append(""a"");
        return sb.ToString();
    }
}

// 在 Program.cs 執行基準測試
// 必須用 Release 模式執行：dotnet run -c Release
// var summary = BenchmarkRunner.Run<StringBenchmarks>();

// 輸出結果範例：
// |            Method |     N |         Mean |     Gen0 |   Allocated |
// |------------------ |------ |-------------:|---------:|------------:|
// |      StringConcat |   100 |     2.814 us |   3.6011 |    15,096 B |
// | StringBuilderApp. |   100 |     0.341 us |   0.0896 |       376 B |
// |      StringConcat | 10000 | 8,234.117 us | 848.9583 | 100,220 KB |
// | StringBuilderApp. | 10000 |    28.193 us |   1.9226 |    40,216 B |
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：不必要的裝箱（Boxing）

```csharp
// 裝箱：把 Value Type 放進 Reference Type 的容器
// 這會在 Heap 上分配記憶體，造成 GC 壓力

// ❌ 錯誤：用 ArrayList（非泛型集合），每次 Add 都會裝箱
var list = new System.Collections.ArrayList();
for (int i = 0; i < 10000; i++)
{
    // int（Value Type）被裝箱成 object（Reference Type）
    // 每次都在 Heap 上分配一個新的物件！
    list.Add(i);  // 裝箱！
}
// 取出來還要拆箱
int value = (int)list[0];  // 拆箱！

// ✅ 正確：使用泛型集合，完全不需要裝箱
var genericList = new List<int>();
for (int i = 0; i < 10000; i++)
{
    // int 直接存入，不需要裝箱
    genericList.Add(i);  // 沒有裝箱！
}
// 取出來也不需要拆箱
int value2 = genericList[0];  // 直接取值！

// ❌ 另一個常見的裝箱場景：字串格式化
int count = 42;
// 舊式寫法會裝箱（count 被轉成 object）
string bad = string.Format(""數量：{0}"", count);  // 裝箱！
// ✅ 字串插值不會裝箱（編譯器會優化）
string good = $""數量：{count}"";  // 不裝箱！
```

### ❌ 錯誤 2：大物件不斷進入 LOH

```csharp
// ❌ 錯誤：在迴圈中不斷建立大陣列
for (int i = 0; i < 100; i++)
{
    // 超過 85,000 bytes 的物件會進入 LOH（Large Object Heap）
    // LOH 的回收成本非常高！
    var largeArray = new byte[100_000];  // 100KB，進入 LOH！
    // 處理完就丟掉...但 LOH 回收代價很高
    ProcessData(largeArray);
}

// ✅ 正確：重複使用陣列，或使用 ArrayPool
using System.Buffers;

for (int i = 0; i < 100; i++)
{
    // 從池中租用陣列（不需要每次都 new）
    var rentedArray = ArrayPool<byte>.Shared.Rent(100_000);
    try
    {
        // 使用租來的陣列
        ProcessData(rentedArray);
    }
    finally
    {
        // 用完歸還到池中（不會被 GC 回收，可以重複使用）
        ArrayPool<byte>.Shared.Return(rentedArray);
    }
}
```

### ❌ 錯誤 3：Sync over Async（在同步中呼叫非同步）

```csharp
// ❌ 錯誤：用 .Result 或 .Wait() 強制同步等待
public string GetDataBad()
{
    // .Result 會阻塞執行緒，在 ASP.NET Core 中可能造成死鎖！
    var result = _httpClient.GetStringAsync(""https://api.example.com"").Result;
    return result;
}

// ❌ 錯誤：用 Task.Run 包裝 async 方法
public string GetDataAlsoBad()
{
    // Task.Run 會佔用一個 ThreadPool 執行緒去等待
    // 浪費資源，而且在高負載時會耗盡執行緒池
    return Task.Run(() => _httpClient.GetStringAsync(""https://api.example.com"")).Result;
}

// ✅ 正確：async all the way（一路 async 到底）
public async Task<string> GetDataGoodAsync()
{
    // 真正的非同步，不阻塞任何執行緒
    var result = await _httpClient.GetStringAsync(""https://api.example.com"");
    return result;
}
// 從 Controller 到 Service 到 Repository，全部都用 async/await
// 這是 ASP.NET Core 的最佳實踐
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Value Type vs Reference Type | Stack vs Heap，影響記憶體分配效能 |
| GC 三代回收 | Gen 0 最頻繁，Gen 2 最昂貴，減少物件分配可降低 GC 壓力 |
| Span<T> | 零複製的陣列片段操作，大幅減少記憶體分配 |
| StringBuilder | 大量字串串接時必用，效能可差 100 倍以上 |
| Task.WhenAll | 獨立的 async 操作應該平行執行 |
| BenchmarkDotNet | 精確的效能測量工具，別用 Stopwatch 猜測 |
| ArrayPool | 重複使用大型陣列，避免 LOH 壓力 |
" },

        // ── 基礎建設 Chapter 303 ─────────────────────────────────────
        new() { Id=303, Category="infrastructure", Order=4, Level="beginner", Icon="⚙️", Title=".NET 平台基礎", Slug="infra-dotnet-platform", IsPublished=true, Content=@"
# .NET 平台基礎

## 什麼是 .NET？

.NET 是微軟開發的應用程式開發平台，讓你可以用 C# 寫出各種應用程式。

> 💡 **比喻：廚房**
> - **.NET 平台** = 整個廚房（包含瓦斯爐、烤箱、冰箱、各種工具）
> - **C# 語言** = 你的廚藝（你用來做菜的技能）
> - **類別庫（BCL）** = 食材和調味料（現成可用的功能）
> - **CLR 執行環境** = 瓦斯爐（幫你把程式碼變成可執行的東西）
> - 有了好廚房 + 好廚藝，就能做出各種料理（應用程式）

---

## .NET 的演進歷史

```
.NET 的版本演進（不要搞混了！）：

.NET Framework（2002-2019）：
├── 只能在 Windows 上執行
├── 版本：1.0、2.0、3.5、4.0、4.5、4.6、4.7、4.8
├── 已經不再開發新功能（只做安全性更新）
└── 比喻：只能在一個國家使用的貨幣

.NET Core（2016-2020）：
├── 跨平台！Windows、Linux、macOS 都能用
├── 版本：1.0、2.0、2.1、3.0、3.1
├── 開源、效能更好
└── 比喻：可以在全世界使用的信用卡

.NET 5+（2020-至今）：統一品牌
├── 把 .NET Core 的名字簡化了（去掉 ""Core""）
├── .NET 5 → 6（LTS）→ 7 → 8（LTS）→ 9 → 10（LTS）
├── LTS = Long Term Support（長期支援版本，支援 3 年）
├── 偶數版本是 LTS，奇數版本只支援 18 個月
└── 比喻：每年出新手機，但只有某些型號有長期保固

現在該用哪個？
├── 新專案 → 一律用最新的 LTS 版本（目前是 .NET 8）
├── 舊專案 .NET Framework → 考慮遷移到 .NET 8
└── 千萬不要在 2024 年用 .NET Framework 開新專案！
```

---

## CLR：Common Language Runtime

```
CLR 是 .NET 程式的執行環境，負責管理程式的執行。

比喻：虛擬機器（但不是 VM）
├── 你的 C# 程式碼不是直接在電腦上跑
├── 而是在 CLR 這個「管家」的監督下執行
├── CLR 幫你做很多事情：
│   ├── 記憶體管理（自動 GC，不用手動 free）
│   ├── 型別安全（防止存取不該存取的記憶體）
│   ├── 例外處理（try-catch 機制）
│   └── 執行緒管理

程式碼的編譯和執行流程：

C# 原始碼（.cs 檔案）
    ↓ C# 編譯器（Roslyn）
IL 中間語言（.dll 檔案）← 這不是機器碼！
    ↓ CLR 的 JIT 編譯器
機器碼（CPU 可以直接執行的指令）
    ↓
執行！

為什麼要有中間語言？
├── 一次編譯，到處執行（跨平台）
├── C# 編譯成 IL → IL 在不同平台上由 JIT 轉成該平台的機器碼
└── 類似 Java 的 bytecode + JVM 概念
```

---

## JIT vs AOT 編譯

```
JIT（Just-In-Time）即時編譯：
├── 程式啟動時，CLR 才把 IL 編譯成機器碼
├── 優點：可以針對執行的硬體做優化
├── 缺點：第一次執行會比較慢（冷啟動）
├── 適合：長時間運行的伺服器應用程式
└── 比喻：現場翻譯（聽到才翻，但可以根據聽眾調整用詞）

AOT（Ahead-Of-Time）預先編譯：
├── 在發佈時就把 IL 編譯成機器碼
├── 優點：啟動速度快、檔案更小
├── 缺點：無法針對特定硬體優化、有些動態功能受限
├── .NET 8 開始支援 Native AOT
├── 適合：Lambda、CLI 工具、容器化的微服務
└── 比喻：事先翻好的翻譯稿（開場就能直接唸，但無法臨場調整）
```

```csharp
// AOT 發佈指令
// 發佈為 Native AOT（不需要安裝 .NET Runtime）
// dotnet publish -c Release -r win-x64 --self-contained /p:PublishAot=true

// 在 .csproj 中啟用 AOT
// <PropertyGroup>
//     <!-- 啟用 Native AOT 編譯 -->
//     <PublishAot>true</PublishAot>
// </PropertyGroup>

// ⚠️ AOT 的限制：
// 某些依賴反射（Reflection）的功能可能不支援
// 例如：動態載入組件、某些序列化框架
// 需要使用 Source Generator 替代反射
```

---

## NuGet 套件管理器

```
NuGet 是 .NET 的套件管理器，就像 npm（Node.js）或 pip（Python）。

比喻：應用程式商店
├── 別人寫好的功能打包成套件（Package）
├── 你透過 NuGet 下載安裝就能用
├── 不用自己從頭寫每個功能
└── 官方套件庫：nuget.org

常用的 NuGet 套件：
├── Newtonsoft.Json → JSON 處理（經典！）
├── Serilog → 日誌
├── AutoMapper → 物件對映
├── FluentValidation → 輸入驗證
├── Dapper → 輕量 ORM
├── MediatR → 中介者模式
└── Polly → 重試和斷路器
```

```csharp
// NuGet 套件管理指令

// 安裝套件
// dotnet add package Serilog.AspNetCore

// 安裝特定版本
// dotnet add package Serilog.AspNetCore --version 8.0.0

// 移除套件
// dotnet remove package Serilog.AspNetCore

// 列出已安裝的套件
// dotnet list package

// 更新套件
// dotnet add package Serilog.AspNetCore（會自動安裝最新版）

// 還原套件（從 .csproj 中的紀錄還原所有套件）
// dotnet restore
```

---

## .csproj 檔案結構

```xml
<!-- .csproj 是專案的設定檔，告訴 .NET 如何建置你的專案 -->
<Project Sdk=""Microsoft.NET.Sdk.Web"">
  <!-- 目標框架和基本設定 -->
  <PropertyGroup>
    <!-- 使用 .NET 8 -->
    <TargetFramework>net8.0</TargetFramework>
    <!-- 啟用 Nullable Reference Types（幫你抓空值錯誤） -->
    <Nullable>enable</Nullable>
    <!-- 啟用隱式 using（自動 using 常用命名空間） -->
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <!-- NuGet 套件參考 -->
  <ItemGroup>
    <!-- 安裝的套件會列在這裡 -->
    <PackageReference Include=""Serilog.AspNetCore"" Version=""8.0.0"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore"" Version=""8.0.0"" />
  </ItemGroup>

  <!-- 專案參考（參考同一方案中的其他專案） -->
  <ItemGroup>
    <ProjectReference Include=""..\MyApp.Core\MyApp.Core.csproj"" />
  </ItemGroup>
</Project>

<!-- 常見的 SDK 類型：
  Microsoft.NET.Sdk         → 一般類別庫、Console App
  Microsoft.NET.Sdk.Web     → ASP.NET Core Web 應用
  Microsoft.NET.Sdk.Worker  → 背景服務
-->
```

---

## SDK vs Runtime

```
SDK 和 Runtime 的差別：

SDK（Software Development Kit）：開發工具包
├── 包含編譯器、CLI 工具、專案模板
├── 用來「開發」程式
├── 比喻：完整的工具箱（鐵鎚、螺絲起子、電鑽都有）
└── 開發者的電腦必須安裝

Runtime（執行環境）：
├── 只包含執行程式所需的最小元件
├── 用來「執行」程式
├── 比喻：只有鑰匙（只能開門，不能修門）
└── 伺服器只需要安裝 Runtime

三種 Runtime：
├── .NET Runtime → 基本的 CLR + BCL
├── ASP.NET Core Runtime → 加上 Web 伺服器功能
└── .NET Desktop Runtime → 加上 WPF/WinForms（桌面應用）

檢查已安裝的版本：
├── dotnet --list-sdks    → 列出已安裝的 SDK
├── dotnet --list-runtimes → 列出已安裝的 Runtime
└── dotnet --version       → 目前使用的 SDK 版本
```

---

## dotnet CLI 常用指令

```csharp
// dotnet CLI 是 .NET 的命令列工具，開發的日常必備

// 1. dotnet new：建立新專案
// 建立一個 Web API 專案
// dotnet new webapi -n MyApi
// 建立一個 Console 專案
// dotnet new console -n MyConsoleApp
// 建立一個類別庫
// dotnet new classlib -n MyLibrary
// 列出所有可用的模板
// dotnet new list

// 2. dotnet build：編譯專案
// 在專案目錄中執行
// dotnet build
// 指定 Release 模式（正式環境用）
// dotnet build -c Release

// 3. dotnet run：編譯並執行
// 開發時最常用
// dotnet run
// 指定環境變數
// dotnet run --environment Development

// 4. dotnet publish：發佈（準備部署）
// 發佈為 Release 模式
// dotnet publish -c Release -o ./publish
// 發佈為獨立部署（不需要安裝 Runtime）
// dotnet publish -c Release -r linux-x64 --self-contained

// 5. dotnet test：執行測試
// 執行所有單元測試
// dotnet test
// 產生測試覆蓋率報告
// dotnet test --collect:""XPlat Code Coverage""

// 6. 其他常用指令
// dotnet restore：還原 NuGet 套件
// dotnet clean：清除編譯輸出
// dotnet watch run：檔案修改時自動重新編譯執行（開發超方便！）
// dotnet ef migrations add Init：EF Core 資料庫遷移
```

```
指令速查表：

指令                    用途                    常用參數
──────────────────────────────────────────────────────────
dotnet new             建立新專案               -n（名稱）、--no-https
dotnet build           編譯                    -c Release
dotnet run             編譯並執行               --urls http://localhost:5000
dotnet publish         發佈                    -c Release、-o ./publish
dotnet test            執行測試                 --filter ""ClassName""
dotnet watch run       監看模式執行              --no-hot-reload
dotnet add package     安裝 NuGet 套件          --version 8.0.0
dotnet ef              Entity Framework 指令    migrations、database
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：混用 .NET Framework 和 .NET Core 的套件

```csharp
// ❌ 錯誤：在 .NET 8 專案中安裝 .NET Framework 專用的套件
// 例如：System.Web（這是 .NET Framework 專用的）
// dotnet add package System.Web  ← 在 .NET 8 中不能用！

// 常見的 .NET Framework 專用命名空間（不能在 .NET 8 中使用）：
// System.Web           → 改用 Microsoft.AspNetCore.*
// System.Web.Http      → 改用 Microsoft.AspNetCore.Mvc
// System.Configuration → 改用 Microsoft.Extensions.Configuration

// ✅ 正確：確認套件支援的框架版本
// 在 nuget.org 上查看套件的「Frameworks」標籤
// 選擇支援 net8.0 或 netstandard2.0 的版本

// 比喻：
// .NET Framework 的套件 = 只能在 110V 國家用的電器
// .NET 8 的套件 = 國際通用電壓的電器
// 插錯電壓會燒壞！
```

### ❌ 錯誤 2：用錯 SDK 版本導致編譯失敗

```
❌ 常見錯誤訊息：
""The current .NET SDK does not support targeting .NET 8.0""

原因：你的電腦安裝的 SDK 版本太舊
├── 專案設定 <TargetFramework>net8.0</TargetFramework>
├── 但你的 SDK 只有 .NET 6 的版本
└── 就像用 Word 2010 開 Word 2024 的檔案，格式不支援

✅ 解決方式：
1. 檢查目前 SDK 版本
   dotnet --version

2. 去官網下載對應版本的 SDK
   https://dotnet.microsoft.com/download

3. 如果團隊要統一版本，可以用 global.json
```

```json
// global.json：鎖定專案使用的 SDK 版本
{
  // 指定要使用的 SDK 版本
  ""sdk"": {
    // 使用 8.0.100 版本的 SDK
    ""version"": ""8.0.100"",
    // rollForward 決定找不到指定版本時怎麼辦
    // latestPatch：允許使用同一個 minor 版本的最新 patch
    ""rollForward"": ""latestPatch""
  }
}
// 把 global.json 放在方案根目錄
// 這樣所有團隊成員都會用同一個版本的 SDK
```

### ❌ 錯誤 3：發佈時忘記考慮部署環境

```
❌ 錯誤：直接把 Debug 版本丟到伺服器上

Debug vs Release 模式的差別：
├── Debug：
│   ├── 沒有最佳化（程式碼可讀性高，方便除錯）
│   ├── 包含除錯符號（.pdb 檔案）
│   ├── 效能較差
│   └── 適合：開發時使用
├── Release：
│   ├── 有最佳化（編譯器會自動優化程式碼）
│   ├── 移除除錯資訊
│   ├── 效能較好（可能差 2-5 倍）
│   └── 適合：正式環境
└── 永遠用 Release 模式發佈到正式環境！

✅ 正確的發佈指令：
├── dotnet publish -c Release
├── 或在 CI/CD 管線中設定 -c Release
└── 確認 appsettings.json 中的環境設定正確
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| .NET Framework | 舊版，只支援 Windows，不建議新專案使用 |
| .NET 8 (LTS) | 最新的長期支援版本，跨平台 |
| CLR | 程式的執行環境，管理記憶體、型別安全 |
| JIT | 執行時才編譯，可針對硬體最佳化 |
| AOT | 發佈時預先編譯，啟動更快 |
| NuGet | .NET 的套件管理器，類似 npm |
| .csproj | 專案設定檔，定義框架版本和套件 |
| SDK vs Runtime | SDK 用來開發，Runtime 用來執行 |
| dotnet CLI | 命令列工具：new、build、run、publish、test |
" },
    };
}
