using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Redis
{
    public static List<Chapter> GetChapters()
    {
        return new List<Chapter>
        {
            // ── Chapter 1200: Redis 入門 ──
            new Chapter
            {
                Id = 1200,
                Title = "Redis 入門：什麼是快取？為什麼需要 Redis？",
                Slug = "redis-intro",
                Category = "redis",
                Order = 120,
                Level = "beginner",
                Icon = "🔴",
                IsPublished = true,
                Content = @"# 🔴 Redis 入門：什麼是快取？為什麼需要 Redis？

## 📌 什麼是快取（Cache）？

想像你在圖書館找一本書：
- **沒有快取** = 每次都要到倉庫裡翻找，花很多時間
- **有快取** = 把常用的書放在桌上，隨手就能拿到

> **快取就是把經常存取的資料放在「更快的地方」，避免每次都去存取較慢的資料來源（如資料庫）。**

### 為什麼需要快取？

```
使用者請求 → API Server → 資料庫查詢（慢！50ms+）
使用者請求 → API Server → 快取命中（快！<1ms）
```

| 情境 | 沒有快取 | 有快取 |
|------|---------|--------|
| 商品頁面載入 | 每次查 DB，50ms | 快取命中，<1ms |
| 首頁排行榜 | 聯合查詢，200ms | 快取命中，<1ms |
| 每秒 1000 請求 | DB 扛不住 | 快取擋住 90% |

---

## 📌 記憶體快取 vs 分散式快取

### 記憶體快取（In-Memory Cache）

```csharp
// ASP.NET Core 內建的 IMemoryCache
builder.Services.AddMemoryCache();

public class ProductService
{
    private readonly IMemoryCache _cache;

    public ProductService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Product GetProduct(int id)
    {
        // 嘗試從快取取得
        if (_cache.TryGetValue($""product:{id}"", out Product product))
            return product;

        // 快取沒有，查資料庫
        product = _db.Products.Find(id);

        // 放入快取，5 分鐘過期
        _cache.Set($""product:{id}"", product, TimeSpan.FromMinutes(5));
        return product;
    }
}
```

**限制：** 只存在單一伺服器的記憶體中，多台伺服器之間不共享。

### 分散式快取（Distributed Cache）

```
Server A ─┐
Server B ─┼──→ Redis（共用快取）
Server C ─┘
```

**優勢：** 所有伺服器共享同一份快取，資料一致。

---

## 📌 Redis 是什麼？

Redis = **Re**mote **Di**ctionary **S**erver（遠端字典伺服器）

- 開源的 **記憶體資料結構儲存庫**
- 支援多種資料型態（String、List、Hash、Set、Sorted Set）
- 單執行緒模型，避免鎖的問題
- 持久化支援（RDB / AOF）
- 支援主從複製、哨兵、Cluster

### Redis vs Memcached 比較

| 特性 | Redis | Memcached |
|------|-------|-----------|
| 資料型態 | 豐富（5+ 種） | 只有 String |
| 持久化 | 支援 RDB/AOF | 不支援 |
| 叢集 | 原生 Cluster | 客戶端分片 |
| Pub/Sub | 支援 | 不支援 |
| Lua 腳本 | 支援 | 不支援 |
| 記憶體效率 | 稍低 | 較高 |
| 多執行緒 | 單執行緒(6.0+ I/O 多執行緒) | 多執行緒 |

---

## 📌 Redis 安裝（Docker 方式）

```bash
# 拉取 Redis 映像
docker pull redis:latest

# 啟動 Redis 容器
docker run -d --name my-redis \
  -p 6379:6379 \
  redis:latest

# 驗證 Redis 是否運行
docker exec -it my-redis redis-cli ping
# 回應: PONG
```

### Docker Compose 版本

```yaml
# docker-compose.yml
version: '3.8'
services:
  redis:
    image: redis:latest
    ports:
      - ""6379:6379""
    volumes:
      - redis-data:/data
    command: redis-server --appendonly yes

volumes:
  redis-data:
```

---

## 📌 Redis CLI 基本操作

```bash
# 連線到 Redis
docker exec -it my-redis redis-cli

# SET：設定鍵值
SET mykey ""Hello Redis""
# OK

# GET：取得值
GET mykey
# ""Hello Redis""

# DEL：刪除鍵
DEL mykey
# (integer) 1

# KEYS：列出所有符合的鍵（生產環境慎用！）
KEYS *
KEYS user:*

# EXISTS：檢查鍵是否存在
EXISTS mykey
# (integer) 0

# TTL：查看剩餘存活時間
SET session:abc ""data"" EX 60
TTL session:abc
# (integer) 58
```

---

## 📌 Redis 適用場景

| 場景 | 說明 | 使用的資料型態 |
|------|------|---------------|
| **Session 儲存** | 分散式 Session 管理 | String / Hash |
| **排行榜** | 遊戲分數、熱門文章 | Sorted Set |
| **計數器** | 按讚數、瀏覽次數 | String (INCR) |
| **訊息佇列** | 非同步任務處理 | List / Stream |
| **即時通訊** | 聊天室、通知推送 | Pub/Sub |
| **限流** | API Rate Limiting | String (INCR + EXPIRE) |
| **地理位置** | 附近的人、店家搜尋 | Geo |

---

## 🔑 重點整理

1. **快取**是把常用資料放在更快的存取層，減少 DB 壓力
2. **記憶體快取**只在單機有效，**分散式快取**多台伺服器共享
3. **Redis** 是最流行的分散式快取，支援豐富資料型態
4. 用 **Docker** 可以快速啟動 Redis 開發環境
5. Redis CLI 的 **SET/GET/DEL/KEYS** 是最基本的操作
"
            },

            // ── Chapter 1201: Redis 資料型態 ──
            new Chapter
            {
                Id = 1201,
                Title = "Redis 資料型態：String、List、Hash、Set、Sorted Set",
                Slug = "redis-datatypes",
                Category = "redis",
                Order = 121,
                Level = "beginner",
                Icon = "📦",
                IsPublished = true,
                Content = @"# 📦 Redis 資料型態：String、List、Hash、Set、Sorted Set

## 📌 五大資料型態總覽

Redis 不只是簡單的 Key-Value 存儲，它支援 **五種核心資料型態**：

| 型態 | 說明 | 常見用途 |
|------|------|---------|
| String | 字串（最基本） | 快取、計數器、Session |
| List | 有序列表 | 訊息佇列、最新動態 |
| Hash | 雜湊表（欄位-值） | 物件儲存、使用者資料 |
| Set | 無序集合（不重複） | 標籤、共同好友 |
| Sorted Set | 有序集合（帶分數） | 排行榜、延遲佇列 |

---

## 📌 String：最基本的鍵值對

```bash
# 基本操作
SET name ""DevLearn""
GET name          # ""DevLearn""

# 設定過期時間（秒）
SET token ""abc123"" EX 3600

# 數值操作
SET counter 0
INCR counter      # 1
INCR counter      # 2
DECR counter      # 1
INCRBY counter 10 # 11

# 批量操作
MSET key1 ""val1"" key2 ""val2""
MGET key1 key2
```

### .NET 程式碼範例

```csharp
using StackExchange.Redis;

var redis = ConnectionMultiplexer.Connect(""localhost:6379"");
var db = redis.GetDatabase();

// 字串操作
await db.StringSetAsync(""name"", ""DevLearn"");
string name = await db.StringGetAsync(""name"");

// 帶過期時間
await db.StringSetAsync(""token"", ""abc123"", TimeSpan.FromHours(1));

// 計數器
await db.StringSetAsync(""views"", 0);
long newViews = await db.StringIncrementAsync(""views"");     // 1
long decreased = await db.StringDecrementAsync(""views"");     // 0
long addTen = await db.StringIncrementAsync(""views"", 10);   // 10
```

---

## 📌 List：有序列表

```bash
# 從左邊推入
LPUSH tasks ""task3"" ""task2"" ""task1""

# 從右邊推入
RPUSH tasks ""task4""

# 取得範圍（0-based，-1 代表最後一個）
LRANGE tasks 0 -1
# 1) ""task1""  2) ""task2""  3) ""task3""  4) ""task4""

# 從左邊彈出
LPOP tasks   # ""task1""

# 列表長度
LLEN tasks   # 3
```

### .NET 程式碼範例

```csharp
// List 操作
await db.ListLeftPushAsync(""notifications"", ""新訂單 #1001"");
await db.ListLeftPushAsync(""notifications"", ""新訂單 #1002"");

// 取得最新 10 筆通知
RedisValue[] recent = await db.ListRangeAsync(""notifications"", 0, 9);
foreach (var item in recent)
    Console.WriteLine(item);  // 新訂單 #1002, 新訂單 #1001

// 彈出（當作佇列使用）
string next = await db.ListRightPopAsync(""notifications"");
```

---

## 📌 Hash：雜湊表（最適合存物件）

```bash
# 設定多個欄位
HSET user:1001 name ""Mike"" email ""mike@test.com"" age ""30""

# 取得單一欄位
HGET user:1001 name        # ""Mike""

# 取得所有欄位
HGETALL user:1001
# 1) ""name""   2) ""Mike""
# 3) ""email""  4) ""mike@test.com""
# 5) ""age""    6) ""30""

# 增加數值欄位
HINCRBY user:1001 age 1    # 31
```

### .NET 程式碼範例

```csharp
// Hash 操作：儲存使用者資料
var userKey = ""user:1001"";
await db.HashSetAsync(userKey, new HashEntry[]
{
    new(""name"", ""Mike""),
    new(""email"", ""mike@test.com""),
    new(""age"", 30)
});

// 取得單一欄位
string userName = await db.HashGetAsync(userKey, ""name"");

// 取得所有欄位
HashEntry[] allFields = await db.HashGetAllAsync(userKey);
foreach (var field in allFields)
    Console.WriteLine($""{field.Name}: {field.Value}"");

// 數值遞增
await db.HashIncrementAsync(userKey, ""age"", 1);
```

---

## 📌 Set：無序集合（不重複）

```bash
# 新增成員
SADD tags:post:1 ""csharp"" ""dotnet"" ""redis""
SADD tags:post:2 ""csharp"" ""docker"" ""redis""

# 列出所有成員
SMEMBERS tags:post:1
# 1) ""csharp""  2) ""dotnet""  3) ""redis""

# 交集（兩篇文章共同的標籤）
SINTER tags:post:1 tags:post:2
# 1) ""csharp""  2) ""redis""

# 聯集
SUNION tags:post:1 tags:post:2

# 是否為成員
SISMEMBER tags:post:1 ""docker""  # 0 (false)
```

### .NET 程式碼範例

```csharp
// Set 操作
await db.SetAddAsync(""online:users"", ""user:1001"");
await db.SetAddAsync(""online:users"", ""user:1002"");
await db.SetAddAsync(""online:users"", ""user:1001""); // 重複不會加入

// 取得所有線上用戶
RedisValue[] onlineUsers = await db.SetMembersAsync(""online:users"");

// 檢查是否在線
bool isOnline = await db.SetContainsAsync(""online:users"", ""user:1001"");

// 交集：共同好友
await db.SetAddAsync(""friends:mike"", new RedisValue[] { ""alice"", ""bob"", ""charlie"" });
await db.SetAddAsync(""friends:jane"", new RedisValue[] { ""bob"", ""charlie"", ""dave"" });
RedisValue[] mutual = await db.SetCombineAsync(SetOperation.Intersect,
    ""friends:mike"", ""friends:jane"");
// [""bob"", ""charlie""]
```

---

## 📌 Sorted Set：有序集合（排行榜神器）

```bash
# 新增成員（帶分數）
ZADD leaderboard 1500 ""player:mike""
ZADD leaderboard 2000 ""player:jane""
ZADD leaderboard 1800 ""player:bob""

# 排名（由高到低）
ZREVRANGE leaderboard 0 -1 WITHSCORES
# 1) ""player:jane""  2) ""2000""
# 3) ""player:bob""   4) ""1800""
# 5) ""player:mike""  6) ""1500""

# 查排名（0-based）
ZREVRANK leaderboard ""player:mike""  # 2

# 增加分數
ZINCRBY leaderboard 600 ""player:mike""  # 2100
```

### .NET 程式碼範例

```csharp
// Sorted Set：排行榜
await db.SortedSetAddAsync(""leaderboard"", ""player:mike"", 1500);
await db.SortedSetAddAsync(""leaderboard"", ""player:jane"", 2000);
await db.SortedSetAddAsync(""leaderboard"", ""player:bob"", 1800);

// 取得 Top 10（分數由高到低）
var top10 = await db.SortedSetRangeByRankWithScoresAsync(
    ""leaderboard"", 0, 9, Order.Descending);
foreach (var entry in top10)
    Console.WriteLine($""{entry.Element}: {entry.Score}"");

// 查排名
long? rank = await db.SortedSetRankAsync(
    ""leaderboard"", ""player:mike"", Order.Descending);

// 增加分數
await db.SortedSetIncrementAsync(""leaderboard"", ""player:mike"", 600);
```

---

## 📌 如何選擇正確的資料型態？

```
需要儲存簡單值或計數？     → String
需要先進先出的佇列？       → List
需要儲存物件的多個欄位？   → Hash
需要不重複的集合運算？     → Set
需要排序或排行榜？         → Sorted Set
```

| 需求 | 推薦型態 | 範例 |
|------|---------|------|
| API 回應快取 | String | 整個 JSON 字串 |
| 使用者 Profile | Hash | name, email, age 各欄位 |
| 最新 100 筆通知 | List | LPUSH + LTRIM |
| 線上用戶列表 | Set | 自動去重 |
| 遊戲排行榜 | Sorted Set | 分數排序 |

---

## 🔑 重點整理

1. **String** 是最基本的型態，支援 INCR/DECR 做計數器
2. **List** 適合佇列場景，LPUSH + RPOP 就是一個簡單的 MQ
3. **Hash** 最適合存物件，每個欄位獨立操作
4. **Set** 用於集合運算，如交集找共同好友
5. **Sorted Set** 是排行榜的最佳解，自動依分數排序
"
            },

            // ── Chapter 1202: 在 ASP.NET Core 中使用 Redis ──
            new Chapter
            {
                Id = 1202,
                Title = "在 ASP.NET Core 中使用 Redis",
                Slug = "redis-dotnet",
                Category = "redis",
                Order = 122,
                Level = "beginner",
                Icon = "🔌",
                IsPublished = true,
                Content = @"# 🔌 在 ASP.NET Core 中使用 Redis

## 📌 安裝 NuGet 套件

```bash
# StackExchange.Redis — 最常用的 .NET Redis 客戶端
dotnet add package StackExchange.Redis

# Microsoft 官方的分散式快取支援
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
```

---

## 📌 ConnectionMultiplexer 連線管理

`ConnectionMultiplexer` 是 StackExchange.Redis 的核心類別，負責管理與 Redis 的連線。

```csharp
using StackExchange.Redis;

// ❌ 錯誤：每次都建立新連線（非常昂貴！）
public Product GetProduct(int id)
{
    var redis = ConnectionMultiplexer.Connect(""localhost:6379"");
    var db = redis.GetDatabase();
    // ...
}

// ✅ 正確：ConnectionMultiplexer 應該是 Singleton
public class RedisConnection
{
    private static readonly Lazy<ConnectionMultiplexer> _instance =
        new(() => ConnectionMultiplexer.Connect(""localhost:6379""));

    public static ConnectionMultiplexer Instance => _instance.Value;
}
```

### 在 DI 容器中註冊

```csharp
// Program.cs
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString(""Redis"")!));
```

---

## 📌 IDatabase 基本操作

```csharp
public class RedisCacheService
{
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    // 存入快取
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry);
    }

    // 從快取取得
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    // 刪除快取
    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    // 檢查是否存在
    public async Task<bool> ExistsAsync(string key)
    {
        return await _db.KeyExistsAsync(key);
    }
}
```

---

## 📌 使用 IDistributedCache 介面

ASP.NET Core 提供了 `IDistributedCache` 抽象介面，可以輕鬆切換快取實作：

```csharp
// Program.cs — 註冊 Redis 分散式快取
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration
        .GetConnectionString(""Redis"");
    options.InstanceName = ""DevLearn:"";
});
```

```csharp
// 使用 IDistributedCache
public class ProductService
{
    private readonly IDistributedCache _cache;
    private readonly AppDbContext _db;

    public ProductService(IDistributedCache cache, AppDbContext db)
    {
        _cache = cache;
        _db = db;
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        var cacheKey = $""product:{id}"";

        // 1. 嘗試從快取取得
        var cached = await _cache.GetStringAsync(cacheKey);
        if (cached != null)
            return JsonSerializer.Deserialize<Product>(cached);

        // 2. 快取沒有，查資料庫
        var product = await _db.Products.FindAsync(id);
        if (product == null) return null;

        // 3. 放入快取
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };
        await _cache.SetStringAsync(cacheKey,
            JsonSerializer.Serialize(product), options);

        return product;
    }
}
```

---

## 📌 appsettings.json 連線設定

```json
{
  ""ConnectionStrings"": {
    ""Redis"": ""localhost:6379,abortConnect=false,connectTimeout=5000""
  }
}
```

### 常用連線參數

| 參數 | 說明 | 預設值 |
|------|------|--------|
| `abortConnect` | 連線失敗是否拋例外 | true |
| `connectTimeout` | 連線逾時（ms） | 5000 |
| `password` | 密碼 | 無 |
| `ssl` | 是否用 SSL | false |
| `defaultDatabase` | 預設資料庫索引 | 0 |
| `asyncTimeout` | 非同步操作逾時 | 5000 |

---

## 📌 完整範例：Product API + Redis 快取

```csharp
// Controllers/ProductsController.cs
[ApiController]
[Route(""api/[controller]"")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        AppDbContext db,
        IDistributedCache cache,
        ILogger<ProductsController> logger)
    {
        _db = db;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet(""{id}"")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var cacheKey = $""product:{id}"";

        // 嘗試快取
        var cached = await _cache.GetStringAsync(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation(""Cache HIT for product {Id}"", id);
            return Ok(JsonSerializer.Deserialize<Product>(cached));
        }

        _logger.LogInformation(""Cache MISS for product {Id}"", id);

        // 查 DB
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        // 寫入快取（5 分鐘過期）
        await _cache.SetStringAsync(cacheKey,
            JsonSerializer.Serialize(product),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

        return Ok(product);
    }

    [HttpPut(""{id}"")]
    public async Task<IActionResult> UpdateProduct(int id, Product updated)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        product.Name = updated.Name;
        product.Price = updated.Price;
        await _db.SaveChangesAsync();

        // 更新後清除快取
        await _cache.RemoveAsync($""product:{id}"");
        _logger.LogInformation(""Cache INVALIDATED for product {Id}"", id);

        return Ok(product);
    }
}
```

---

## 🔑 重點整理

1. **StackExchange.Redis** 是 .NET 最常用的 Redis 客戶端
2. **ConnectionMultiplexer** 必須是 Singleton，避免重複建立連線
3. **IDistributedCache** 是 ASP.NET Core 的標準快取介面
4. 連線字串放在 **appsettings.json**，透過 DI 注入使用
5. 更新資料後，記得 **清除對應的快取**
"
            },

            // ── Chapter 1203: 快取策略模式 ──
            new Chapter
            {
                Id = 1203,
                Title = "快取策略模式：Cache-Aside、Write-Through、Write-Behind",
                Slug = "redis-caching-patterns",
                Category = "redis",
                Order = 123,
                Level = "intermediate",
                Icon = "🏗️",
                IsPublished = true,
                Content = @"# 🏗️ 快取策略模式：Cache-Aside、Write-Through、Write-Behind

## 📌 四種常見的快取策略

| 策略 | 讀取 | 寫入 | 適用場景 |
|------|------|------|---------|
| Cache-Aside | App 先查快取，Miss 再查 DB | App 寫 DB，再清快取 | 最通用 |
| Read-Through | 快取自動從 DB 載入 | - | 讀多寫少 |
| Write-Through | - | 同時寫快取和 DB | 資料一致性高 |
| Write-Behind | - | 先寫快取，非同步寫 DB | 寫入量大 |

---

## 📌 Cache-Aside（旁路快取）— 最常用！

### 工作流程

```
讀取：
1. App 先查 Redis
2. 命中 → 直接回傳
3. 未命中 → 查 DB → 寫入 Redis → 回傳

寫入：
1. App 更新 DB
2. 刪除 Redis 中的快取（而非更新）
```

### 為什麼是「刪除」而非「更新」快取？

> 更新快取可能導致 **並發問題**：
> 兩個請求同時更新，A 先寫 DB 但後寫快取，快取就會是舊資料。
> 刪除快取讓下一次讀取自動載入最新資料，更安全。

### .NET 完整實作

```csharp
public class OrderService
{
    private readonly AppDbContext _db;
    private readonly IDistributedCache _cache;
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(10);

    public OrderService(AppDbContext db, IDistributedCache cache)
    {
        _db = db;
        _cache = cache;
    }

    // 讀取：Cache-Aside 模式
    public async Task<Order?> GetOrderAsync(int orderId)
    {
        var cacheKey = $""order:{orderId}"";

        // Step 1: 查快取
        var cached = await _cache.GetStringAsync(cacheKey);
        if (cached != null)
        {
            Console.WriteLine($""[Cache HIT] order:{orderId}"");
            return JsonSerializer.Deserialize<Order>(cached);
        }

        // Step 2: 快取 Miss，查 DB
        Console.WriteLine($""[Cache MISS] order:{orderId}"");
        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) return null;

        // Step 3: 寫入快取
        await _cache.SetStringAsync(cacheKey,
            JsonSerializer.Serialize(order),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheExpiry
            });

        return order;
    }

    // 寫入：更新 DB 後刪除快取
    public async Task UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null) throw new Exception(""Order not found"");

        order.Status = status;
        await _db.SaveChangesAsync();

        // 刪除快取（不是更新！）
        await _cache.RemoveAsync($""order:{orderId}"");
        Console.WriteLine($""[Cache INVALIDATED] order:{orderId}"");
    }
}
```

---

## 📌 Read-Through（穿透讀取）

快取層自動處理 DB 查詢，應用程式只跟快取互動。

```csharp
// 概念實作：快取自動載入
public class ReadThroughCache<T>
{
    private readonly IDatabase _redis;
    private readonly Func<string, Task<T?>> _dataLoader;
    private readonly TimeSpan _ttl;

    public ReadThroughCache(
        IDatabase redis,
        Func<string, Task<T?>> dataLoader,
        TimeSpan ttl)
    {
        _redis = redis;
        _dataLoader = dataLoader;
        _ttl = ttl;
    }

    public async Task<T?> GetAsync(string key)
    {
        // 自動查快取
        var cached = await _redis.StringGetAsync(key);
        if (!cached.IsNullOrEmpty)
            return JsonSerializer.Deserialize<T>(cached!);

        // 自動從資料源載入
        var data = await _dataLoader(key);
        if (data != null)
        {
            await _redis.StringSetAsync(key,
                JsonSerializer.Serialize(data), _ttl);
        }
        return data;
    }
}

// 使用
var productCache = new ReadThroughCache<Product>(
    db, key => dbContext.Products.FindAsync(int.Parse(key.Split(':')[1])).AsTask(),
    TimeSpan.FromMinutes(10));

var product = await productCache.GetAsync(""product:42"");
```

---

## 📌 Write-Through（穿透寫入）

寫入時同時更新快取和 DB，確保兩者一致。

```csharp
public class WriteThroughService
{
    private readonly AppDbContext _db;
    private readonly IDatabase _redis;

    public async Task SaveProductAsync(Product product)
    {
        // 同時寫 DB 和快取
        _db.Products.Update(product);
        await _db.SaveChangesAsync();

        var cacheKey = $""product:{product.Id}"";
        await _redis.StringSetAsync(cacheKey,
            JsonSerializer.Serialize(product),
            TimeSpan.FromMinutes(30));

        // 兩者都成功才算完成
        Console.WriteLine($""[Write-Through] product:{product.Id} synced"");
    }
}
```

**優點：** 快取永遠是最新的。
**缺點：** 寫入延遲增加（要寫兩個地方）。

---

## 📌 Write-Behind / Write-Back（回寫）

先寫入快取，非同步批量寫入 DB。

```csharp
public class WriteBehindService
{
    private readonly IDatabase _redis;
    private readonly Channel<WriteTask> _writeQueue;

    // 寫入：只寫快取，任務放入佇列
    public async Task SaveAsync(string key, object data)
    {
        await _redis.StringSetAsync(key,
            JsonSerializer.Serialize(data));

        // 非同步寫入佇列
        await _writeQueue.Writer.WriteAsync(
            new WriteTask { Key = key, Data = data });
    }

    // 背景工作：批量寫入 DB
    public async Task ProcessWriteQueueAsync(CancellationToken ct)
    {
        var batch = new List<WriteTask>();

        await foreach (var task in _writeQueue.Reader.ReadAllAsync(ct))
        {
            batch.Add(task);

            // 累積 100 筆或每 5 秒批量寫入
            if (batch.Count >= 100)
            {
                await FlushToDatabase(batch);
                batch.Clear();
            }
        }
    }
}
```

**優點：** 寫入極快，DB 壓力小。
**缺點：** 快取掛掉可能丟失資料。

---

## 📌 策略比較表

| 特性 | Cache-Aside | Read-Through | Write-Through | Write-Behind |
|------|-------------|-------------|--------------|-------------|
| 實作複雜度 | 低 | 中 | 中 | 高 |
| 讀取效能 | 高 | 高 | 高 | 高 |
| 寫入效能 | 中 | - | 低 | 極高 |
| 資料一致性 | 最終一致 | 最終一致 | 強一致 | 弱一致 |
| 資料遺失風險 | 低 | 低 | 低 | 高 |
| 適用場景 | 通用 | 讀多寫少 | 一致性要求高 | 寫入量極大 |

---

## 🔑 重點整理

1. **Cache-Aside** 是最常用的策略，先查快取再查 DB
2. 寫入後應 **刪除快取**，而非更新快取，避免並發問題
3. **Write-Through** 適合需要高一致性的場景
4. **Write-Behind** 效能最好但有資料遺失風險
5. 大多數 .NET 專案用 **Cache-Aside** 就夠了
"
            },

            // ── Chapter 1204: 快取過期與失效 ──
            new Chapter
            {
                Id = 1204,
                Title = "快取過期與失效：TTL、LRU、主動清除",
                Slug = "redis-expiration",
                Category = "redis",
                Order = 124,
                Level = "intermediate",
                Icon = "⏰",
                IsPublished = true,
                Content = @"# ⏰ 快取過期與失效：TTL、LRU、主動清除

## 📌 TTL（Time To Live）存活時間

每個 Redis 鍵都可以設定「到期時間」，時間到了自動刪除。

```bash
# 設定鍵並指定 TTL（秒）
SET product:1001 ""{...}"" EX 300     # 300 秒後過期

# 設定鍵並指定 TTL（毫秒）
SET product:1001 ""{...}"" PX 300000  # 300000 毫秒後過期

# 對已存在的鍵設定 TTL
EXPIRE product:1001 300
PEXPIRE product:1001 300000

# 查看剩餘 TTL
TTL product:1001      # 返回剩餘秒數，-1 表示永不過期，-2 表示不存在
PTTL product:1001     # 毫秒精度

# 移除 TTL（變成永不過期）
PERSIST product:1001
```

---

## 📌 .NET 中設定過期時間

### 使用 StackExchange.Redis

```csharp
var db = redis.GetDatabase();

// 設定帶 TTL 的鍵
await db.StringSetAsync(""session:abc"", ""user-data"",
    TimeSpan.FromMinutes(30));

// 對已存在的鍵設定過期
await db.KeyExpireAsync(""product:1001"", TimeSpan.FromMinutes(5));

// 查看剩餘時間
TimeSpan? ttl = await db.KeyTimeToLiveAsync(""product:1001"");
Console.WriteLine($""剩餘: {ttl?.TotalSeconds} 秒"");

// 移除過期（永不過期）
await db.KeyPersistAsync(""product:1001"");
```

### 使用 IDistributedCache

```csharp
var options = new DistributedCacheEntryOptions();

// 絕對過期：從現在起 5 分鐘後過期（不管有沒有人存取）
options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

// 滑動過期：2 分鐘沒人存取就過期（每次存取會重設計時器）
options.SlidingExpiration = TimeSpan.FromMinutes(2);

// 兩者可以同時使用！
// 例：滑動 2 分鐘 + 絕對 5 分鐘
// → 持續被存取最多活 5 分鐘，2 分鐘沒人用就過期
options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
options.SlidingExpiration = TimeSpan.FromMinutes(2);

await _cache.SetStringAsync(""product:1001"", json, options);
```

---

## 📌 AbsoluteExpiration vs SlidingExpiration

```
絕對過期（Absolute）：
├── 設定 ──── 5 分鐘後 ──── 過期（不管有沒有人用）

滑動過期（Sliding）：
├── 設定 ── 存取 ── 存取 ── 2分鐘沒人用 ── 過期
│          ↑ 重設   ↑ 重設

同時使用：
├── 設定 ── 存取 ── 存取 ── 存取 ── 5分鐘到 ── 過期
│          ↑重設    ↑重設    ↑重設   （絕對上限）
```

| 類型 | 行為 | 適用場景 |
|------|------|---------|
| Absolute | 固定時間後過期 | 商品資料、設定檔 |
| Sliding | 一段時間沒存取才過期 | Session、使用者暫存 |
| 兩者合用 | Sliding + 絕對上限 | 最佳實踐 |

---

## 📌 記憶體淘汰策略

當 Redis 記憶體用滿時，會根據設定的策略淘汰舊資料。

```bash
# 設定最大記憶體
maxmemory 256mb

# 設定淘汰策略
maxmemory-policy allkeys-lru
```

### 八種淘汰策略

| 策略 | 說明 | 適用場景 |
|------|------|---------|
| `noeviction` | 不淘汰，滿了就報錯 | 不能丟資料 |
| `allkeys-lru` | **所有 key 中淘汰最近最少使用的** | **最常用** |
| `allkeys-lfu` | 所有 key 中淘汰最不常使用的 | 熱點資料場景 |
| `allkeys-random` | 隨機淘汰 | 隨意 |
| `volatile-lru` | 有設 TTL 的 key 中淘汰 LRU | 混合持久/快取 |
| `volatile-lfu` | 有設 TTL 的 key 中淘汰 LFU | 混合場景 |
| `volatile-random` | 有設 TTL 的 key 中隨機淘汰 | 混合場景 |
| `volatile-ttl` | 淘汰即將過期的 key | TTL 管理嚴格 |

> **推薦：** 純快取用 `allkeys-lru`，混合用途用 `volatile-lru`。

---

## 📌 主動清除快取的時機與方法

```csharp
public class CacheInvalidationService
{
    private readonly IDatabase _redis;

    // 1. 更新資料時清除
    public async Task OnProductUpdated(int productId)
    {
        await _redis.KeyDeleteAsync($""product:{productId}"");
    }

    // 2. 批量清除某個前綴的所有快取
    public async Task ClearProductCache()
    {
        var server = _redis.Multiplexer.GetServer(""localhost:6379"");
        var keys = server.Keys(pattern: ""product:*"").ToArray();

        if (keys.Length > 0)
            await _redis.KeyDeleteAsync(keys);

        Console.WriteLine($""已清除 {keys.Length} 筆商品快取"");
    }

    // 3. 使用 Pub/Sub 通知其他實例清除快取
    public async Task PublishCacheInvalidation(string key)
    {
        var sub = _redis.Multiplexer.GetSubscriber();
        await sub.PublishAsync(""cache:invalidate"", key);
    }
}
```

---

## 📌 範例：商品快取 + 30 秒 TTL

```csharp
public class ProductCacheService
{
    private readonly IDistributedCache _cache;
    private readonly AppDbContext _db;

    private static readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
        SlidingExpiration = TimeSpan.FromSeconds(10)
    };

    public async Task<Product?> GetProductAsync(int id)
    {
        var key = $""product:{id}"";
        var cached = await _cache.GetStringAsync(key);

        if (cached != null)
            return JsonSerializer.Deserialize<Product>(cached);

        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            await _cache.SetStringAsync(key,
                JsonSerializer.Serialize(product), _cacheOptions);
        }
        return product;
    }
}
```

---

## 🔑 重點整理

1. **TTL** 是快取過期的基礎，用 EX/PX 或 EXPIRE 設定
2. **AbsoluteExpiration** 固定時間過期，**SlidingExpiration** 閒置才過期
3. 最佳實踐是 **兩者合用**，Sliding + 絕對上限
4. 記憶體滿時，**allkeys-lru** 是最常用的淘汰策略
5. 資料更新後要 **主動清除快取**，保持一致性
"
            },

            // ── Chapter 1205: 快取三大問題 ──
            new Chapter
            {
                Id = 1205,
                Title = "快取三大問題：穿透、擊穿、雪崩",
                Slug = "redis-cache-problems",
                Category = "redis",
                Order = 125,
                Level = "intermediate",
                Icon = "⚠️",
                IsPublished = true,
                Content = @"# ⚠️ 快取三大問題：穿透、擊穿、雪崩

## 📌 問題概覽

| 問題 | 英文 | 原因 | 結果 |
|------|------|------|------|
| 穿透 | Cache Penetration | 查詢不存在的資料 | 每次都打 DB |
| 擊穿 | Cache Breakdown | 熱點 key 過期 | 瞬間大量打 DB |
| 雪崩 | Cache Avalanche | 大量 key 同時過期 | DB 被打爆 |

---

## 📌 Cache Penetration（快取穿透）

### 問題描述

```
請求 product:-1（不存在的 ID）
→ Redis 查不到 → DB 也查不到 → 不會寫入快取
→ 下次請求還是直接打 DB → 快取形同虛設！
```

攻擊者可以大量請求不存在的 ID，讓 DB 承受巨大壓力。

### 解法一：空值快取（Null Caching）

```csharp
public async Task<Product?> GetProductSafe(int id)
{
    var key = $""product:{id}"";
    var cached = await _cache.GetStringAsync(key);

    // 快取命中（包括空值標記）
    if (cached != null)
    {
        if (cached == ""__NULL__"") return null;  // 空值標記
        return JsonSerializer.Deserialize<Product>(cached);
    }

    // 查 DB
    var product = await _db.Products.FindAsync(id);

    if (product != null)
    {
        await _cache.SetStringAsync(key,
            JsonSerializer.Serialize(product),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
    }
    else
    {
        // 關鍵：不存在的 key 也快取，但過期時間短
        await _cache.SetStringAsync(key, ""__NULL__"",
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });
    }

    return product;
}
```

### 解法二：參數驗證

```csharp
public async Task<IActionResult> GetProduct(int id)
{
    // 先驗證 ID 合理性
    if (id <= 0 || id > 1_000_000)
        return BadRequest(""Invalid product ID"");

    var product = await _productService.GetProductSafe(id);
    return product == null ? NotFound() : Ok(product);
}
```

### 解法三：布隆過濾器（Bloom Filter）

```csharp
// 概念：用 BitArray 快速判斷 key 是否「可能存在」
// 如果布隆過濾器說不存在，就一定不存在
// 如果布隆過濾器說存在，有小機率是誤判

// 使用 Redis 的 BF 模組（需安裝 RedisBloom）
// BF.ADD product_filter 1001
// BF.EXISTS product_filter 9999  → 0 (一定不存在)

public async Task<Product?> GetProductWithBloom(int id)
{
    // 先問布隆過濾器
    bool mightExist = await _redis.ExecuteAsync(
        ""BF.EXISTS"", ""product_filter"", id.ToString());

    if (!mightExist)
        return null;  // 一定不存在，直接返回

    // 可能存在，走正常快取流程
    return await GetProductSafe(id);
}
```

---

## 📌 Cache Breakdown（快取擊穿）

### 問題描述

```
熱門商品 product:1001 的快取剛好過期
→ 同一瞬間 1000 個請求湧入
→ 全部 Cache Miss
→ 1000 個請求同時查 DB
→ DB 瞬間壓力爆表！
```

### 解法一：互斥鎖（Mutex Lock）

```csharp
public async Task<Product?> GetProductWithLock(int id)
{
    var key = $""product:{id}"";
    var lockKey = $""lock:product:{id}"";

    // 1. 嘗試從快取取得
    var cached = await _cache.GetStringAsync(key);
    if (cached != null)
        return JsonSerializer.Deserialize<Product>(cached);

    // 2. 嘗試取得鎖
    var db = _redis.GetDatabase();
    bool lockAcquired = await db.StringSetAsync(
        lockKey, ""1"",
        TimeSpan.FromSeconds(10),   // 鎖的過期時間
        When.NotExists);            // 只在 key 不存在時設定

    if (lockAcquired)
    {
        try
        {
            // 3. 取得鎖：查 DB 並更新快取
            var product = await _dbContext.Products.FindAsync(id);
            if (product != null)
            {
                await _cache.SetStringAsync(key,
                    JsonSerializer.Serialize(product),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
            }
            return product;
        }
        finally
        {
            await db.KeyDeleteAsync(lockKey);
        }
    }
    else
    {
        // 4. 沒取得鎖：等待後重試
        await Task.Delay(100);
        return await GetProductWithLock(id);  // 重試
    }
}
```

### 解法二：永不過期 + 非同步更新

```csharp
public class HotKeyService
{
    // 快取永不過期，但記錄「邏輯過期時間」
    public async Task<Product?> GetHotProduct(int id)
    {
        var key = $""product:{id}"";
        var data = await _redis.HashGetAllAsync(key);

        if (data.Length == 0) return null;

        var product = JsonSerializer.Deserialize<Product>(
            data.First(f => f.Name == ""data"").Value!);
        var expireAt = long.Parse(
            data.First(f => f.Name == ""expireAt"").Value!);

        // 檢查邏輯過期
        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expireAt)
        {
            // 已過期，非同步更新（不阻塞當前請求）
            _ = Task.Run(async () =>
            {
                var fresh = await _dbContext.Products.FindAsync(id);
                if (fresh != null)
                    await SetHotProduct(key, fresh);
            });
        }

        // 返回舊資料（可能略過時，但不會讓 DB 被打爆）
        return product;
    }
}
```

---

## 📌 Cache Avalanche（快取雪崩）

### 問題描述

```
凌晨 2:00 批量寫入 10000 筆商品快取，TTL 都設 8 小時
→ 上午 10:00，10000 筆快取同時過期
→ 大量請求同時穿透到 DB
→ DB 直接被打掛！
```

### 解法一：隨機過期時間

```csharp
public async Task CacheProduct(Product product)
{
    var key = $""product:{product.Id}"";
    var random = new Random();

    // 基礎 TTL + 隨機偏移（避免同時過期）
    var baseTtl = TimeSpan.FromMinutes(30);
    var jitter = TimeSpan.FromMinutes(random.Next(0, 10));
    var ttl = baseTtl + jitter;  // 30~40 分鐘

    await _cache.SetStringAsync(key,
        JsonSerializer.Serialize(product),
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        });
}
```

### 解法二：多層快取

```csharp
public class MultiLayerCache
{
    private readonly IMemoryCache _l1;          // L1: 本地記憶體
    private readonly IDistributedCache _l2;      // L2: Redis

    public async Task<Product?> GetProduct(int id)
    {
        var key = $""product:{id}"";

        // L1: 先查本地快取（超快）
        if (_l1.TryGetValue(key, out Product product))
            return product;

        // L2: 再查 Redis
        var cached = await _l2.GetStringAsync(key);
        if (cached != null)
        {
            product = JsonSerializer.Deserialize<Product>(cached)!;
            // 寫回 L1（短 TTL）
            _l1.Set(key, product, TimeSpan.FromSeconds(30));
            return product;
        }

        // L3: 查 DB
        product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            await _l2.SetStringAsync(key,
                JsonSerializer.Serialize(product),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
            _l1.Set(key, product, TimeSpan.FromSeconds(30));
        }
        return product;
    }
}
```

### 解法三：熔斷降級

```csharp
// 當 DB 壓力過大時，暫時返回預設值或舊資料
public async Task<Product?> GetProductWithFallback(int id)
{
    try
    {
        return await GetProductFromCacheOrDb(id);
    }
    catch (Exception ex) when (ex is TimeoutException || ex is DbException)
    {
        _logger.LogWarning(""DB 超時，返回降級資料"");
        return GetDefaultProduct(id);  // 返回預設值
    }
}
```

---

## 📌 三大問題對比

| 問題 | 觸發條件 | 核心解法 |
|------|---------|---------|
| 穿透 | 查不存在的 key | 空值快取 + 參數驗證 |
| 擊穿 | 熱點 key 過期 | 互斥鎖 / 永不過期 |
| 雪崩 | 大量 key 同時過期 | 隨機 TTL / 多層快取 |

---

## 🔑 重點整理

1. **穿透**用空值快取擋住不存在的 key
2. **擊穿**用互斥鎖確保只有一個請求查 DB
3. **雪崩**用隨機 TTL 分散過期時間
4. **多層快取** (L1 本地 + L2 Redis) 是終極防線
5. 永遠要有 **降級方案**，DB 掛了也能回應
"
            },

            // ── Chapter 1206: Redis 分散式架構 ──
            new Chapter
            {
                Id = 1206,
                Title = "Redis 分散式架構：主從複製、哨兵、Cluster",
                Slug = "redis-distributed",
                Category = "redis",
                Order = 126,
                Level = "advanced",
                Icon = "🌐",
                IsPublished = true,
                Content = @"# 🌐 Redis 分散式架構：主從複製、哨兵、Cluster

## 📌 單機 Redis 的限制

| 問題 | 說明 |
|------|------|
| **單點故障** | Redis 掛了，整個快取失效 |
| **容量限制** | 受限於單機記憶體 |
| **效能瓶頸** | 讀寫都在同一台 |

解決方案：**分散式架構**。

---

## 📌 主從複製（Replication）

```
寫入請求 → Master（主節點）
                ↓ 自動同步
讀取請求 → Slave 1（從節點）
讀取請求 → Slave 2（從節點）
```

### 設定方式

```bash
# slave 的 redis.conf
replicaof 192.168.1.100 6379

# 或在 Redis CLI 中動態設定
REPLICAOF 192.168.1.100 6379

# 查看複製資訊
INFO replication
```

### Docker Compose 範例

```yaml
version: '3.8'
services:
  redis-master:
    image: redis:latest
    ports:
      - ""6379:6379""

  redis-slave-1:
    image: redis:latest
    ports:
      - ""6380:6379""
    command: redis-server --replicaof redis-master 6379

  redis-slave-2:
    image: redis:latest
    ports:
      - ""6381:6379""
    command: redis-server --replicaof redis-master 6379
```

### .NET 讀寫分離

```csharp
// 連線字串指定多個節點
var config = ConfigurationOptions.Parse(
    ""master:6379,slave1:6380,slave2:6381"");
var redis = ConnectionMultiplexer.Connect(config);

// 讀取：自動走 Slave
var db = redis.GetDatabase();
var value = await db.StringGetAsync(""key"",
    CommandFlags.PreferReplica);

// 寫入：一定走 Master
await db.StringSetAsync(""key"", ""value"");
```

---

## 📌 Redis Sentinel（哨兵）

主從複製的問題：**Master 掛了怎麼辦？** → Sentinel 自動故障轉移！

```
Sentinel 1 ──┐
Sentinel 2 ──┼── 監控 Master + Slaves
Sentinel 3 ──┘        ↓
              Master 掛了！
                ↓
        Sentinel 投票選新 Master
                ↓
        Slave 1 升級為 Master
        Slave 2 指向新 Master
```

### 設定方式

```bash
# sentinel.conf
sentinel monitor mymaster 192.168.1.100 6379 2
sentinel down-after-milliseconds mymaster 5000
sentinel failover-timeout mymaster 60000
sentinel parallel-syncs mymaster 1
```

### .NET 連接 Sentinel

```csharp
var config = new ConfigurationOptions
{
    ServiceName = ""mymaster"",
    CommandMap = CommandMap.Sentinel
};
config.EndPoints.Add(""sentinel1:26379"");
config.EndPoints.Add(""sentinel2:26379"");
config.EndPoints.Add(""sentinel3:26379"");

var redis = ConnectionMultiplexer.Connect(config);
// 自動連接到 Master，故障轉移時自動切換
```

---

## 📌 Redis Cluster

當資料量超過單機記憶體時，需要 **分片（Sharding）**。

```
Redis Cluster 共有 16384 個 Slot

Node A: Slot 0 ~ 5460
Node B: Slot 5461 ~ 10922
Node C: Slot 10923 ~ 16383

key ""user:1001"" → CRC16(""user:1001"") % 16384 → Slot 8732 → Node B
key ""user:2001"" → CRC16(""user:2001"") % 16384 → Slot 3291 → Node A
```

### 特點

- 資料自動分片到多個節點
- 每個 Master 有 Slave 備援
- 客戶端透過 **MOVED** 重導向找到正確節點
- 最少 **3 個 Master + 3 個 Slave** = 6 個節點

### Docker Compose 搭建

```yaml
version: '3.8'
services:
  redis-1:
    image: redis:latest
    command: >
      redis-server
      --cluster-enabled yes
      --cluster-config-file nodes.conf
      --cluster-node-timeout 5000
      --port 7001
    ports:
      - ""7001:7001""

  redis-2:
    image: redis:latest
    command: >
      redis-server
      --cluster-enabled yes
      --cluster-config-file nodes.conf
      --cluster-node-timeout 5000
      --port 7002
    ports:
      - ""7002:7002""

  redis-3:
    image: redis:latest
    command: >
      redis-server
      --cluster-enabled yes
      --cluster-config-file nodes.conf
      --cluster-node-timeout 5000
      --port 7003
    ports:
      - ""7003:7003""
```

```bash
# 建立 Cluster
redis-cli --cluster create \
  redis-1:7001 redis-2:7002 redis-3:7003 \
  --cluster-replicas 0
```

### .NET 連接 Cluster

```csharp
var config = ConfigurationOptions.Parse(
    ""node1:7001,node2:7002,node3:7003"");
var redis = ConnectionMultiplexer.Connect(config);

// 操作方式完全相同，StackExchange.Redis 自動處理重導向
var db = redis.GetDatabase();
await db.StringSetAsync(""key"", ""value"");
```

---

## 📌 一致性雜湊（Consistent Hashing）

Redis Cluster 使用 **CRC16 + Slot** 分片。但了解一致性雜湊的概念很重要：

```
傳統雜湊：hash(key) % N
  → 增減節點時，幾乎所有 key 都要重新分配

一致性雜湊：hash(key) 映射到環上
  → 增減節點只影響相鄰的一小段
  → 大幅減少資料遷移量
```

---

## 📌 架構選擇指南

| 架構 | 適用場景 | 節點數 |
|------|---------|--------|
| 單機 | 開發/測試 | 1 |
| 主從 | 讀多寫少，需讀寫分離 | 1 Master + N Slave |
| Sentinel | 需要高可用（自動故障轉移） | 3 Sentinel + 主從 |
| Cluster | 大資料量 + 高可用 | 至少 6 (3M + 3S) |

---

## 🔑 重點整理

1. **主從複製** 解決讀取效能，但無法自動故障轉移
2. **Sentinel** 監控主從，Master 掛了自動選新 Master
3. **Cluster** 將資料分片到多個節點，解決容量問題
4. StackExchange.Redis **自動處理** Cluster 重導向
5. 生產環境至少用 **Sentinel**，大規模用 **Cluster**
"
            },

            // ── Chapter 1207: Redis Pub/Sub 與 Stream ──
            new Chapter
            {
                Id = 1207,
                Title = "Redis Pub/Sub 與 Stream：即時訊息與事件驅動",
                Slug = "redis-pubsub",
                Category = "redis",
                Order = 127,
                Level = "advanced",
                Icon = "📡",
                IsPublished = true,
                Content = @"# 📡 Redis Pub/Sub 與 Stream：即時訊息與事件驅動

## 📌 Pub/Sub 發布訂閱模式

```
Publisher ──→ Channel ""news"" ──→ Subscriber A
                              ──→ Subscriber B
                              ──→ Subscriber C
```

- **發布者** 向頻道發送訊息
- **訂閱者** 監聽頻道接收訊息
- 訊息即時推送，**不會持久化**

### Redis CLI 操作

```bash
# 終端 1：訂閱頻道
SUBSCRIBE news
# 等待訊息...

# 終端 2：發布訊息
PUBLISH news ""Breaking: Redis 8.0 released!""
# (integer) 1  ← 表示有 1 個訂閱者收到

# 模式訂閱（萬用字元）
PSUBSCRIBE news.*
# 會收到 news.tech, news.sports 等所有子頻道
```

---

## 📌 .NET 中使用 Pub/Sub

### 發布訊息

```csharp
public class NotificationPublisher
{
    private readonly IConnectionMultiplexer _redis;

    public NotificationPublisher(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task PublishOrderCreated(Order order)
    {
        var subscriber = _redis.GetSubscriber();
        var message = JsonSerializer.Serialize(new
        {
            EventType = ""OrderCreated"",
            OrderId = order.Id,
            Amount = order.Total,
            Timestamp = DateTime.UtcNow
        });

        await subscriber.PublishAsync(""orders"", message);
        Console.WriteLine($""已發布訂單事件: {order.Id}"");
    }
}
```

### 訂閱訊息

```csharp
public class NotificationSubscriber : BackgroundService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<NotificationSubscriber> _logger;

    public NotificationSubscriber(
        IConnectionMultiplexer redis,
        ILogger<NotificationSubscriber> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var subscriber = _redis.GetSubscriber();

        // 訂閱 orders 頻道
        await subscriber.SubscribeAsync(""orders"", (channel, message) =>
        {
            _logger.LogInformation(
                ""收到訂單事件: {Message}"", message.ToString());

            // 處理事件（發送 Email、更新庫存等）
            ProcessOrderEvent(message!);
        });

        // 模式訂閱
        await subscriber.SubscribeAsync(""notifications.*"",
            (channel, message) =>
            {
                _logger.LogInformation(
                    ""頻道 {Channel}: {Message}"", channel, message);
            });

        _logger.LogInformation(""Pub/Sub 訂閱已啟動"");
        await Task.Delay(Timeout.Infinite, ct);
    }

    private void ProcessOrderEvent(string message)
    {
        var order = JsonSerializer.Deserialize<JsonElement>(message);
        // 處理邏輯...
    }
}

// 註冊背景服務
builder.Services.AddHostedService<NotificationSubscriber>();
```

---

## 📌 Pub/Sub 的限制

| 限制 | 說明 |
|------|------|
| 不持久化 | 訊息發出就消失，離線的訂閱者收不到 |
| 無確認機制 | 不知道訂閱者是否成功處理 |
| 無重播 | 不能回溯歷史訊息 |

→ 需要持久化和可靠傳遞？使用 **Redis Streams**！

---

## 📌 Redis Streams：持久化的訊息佇列

```
Producer ──XADD──→ Stream ""orders""
                    ├── 1609459200000-0: {orderId:1, amount:100}
                    ├── 1609459200001-0: {orderId:2, amount:200}
                    └── 1609459200002-0: {orderId:3, amount:150}
                              ↑
Consumer Group ""processors""
  ├── Consumer A: 處理 msg 0,1
  └── Consumer B: 處理 msg 2
```

### Redis CLI 操作

```bash
# 新增訊息到 Stream
XADD orders * orderId 1001 amount 99.99 status created
# ""1609459200000-0""  ← 自動生成的訊息 ID

# 讀取訊息
XRANGE orders - +           # 全部訊息
XRANGE orders - + COUNT 10  # 最新 10 筆

# 建立消費者群組
XGROUP CREATE orders processors $ MKSTREAM

# 消費者讀取
XREADGROUP GROUP processors consumer-1 COUNT 1 BLOCK 5000 STREAMS orders >

# 確認已處理
XACK orders processors 1609459200000-0
```

---

## 📌 .NET 中使用 Streams

```csharp
public class OrderStreamProducer
{
    private readonly IDatabase _db;

    public async Task PublishOrder(Order order)
    {
        await _db.StreamAddAsync(""orders"", new NameValueEntry[]
        {
            new(""orderId"", order.Id.ToString()),
            new(""amount"", order.Total.ToString()),
            new(""status"", ""created""),
            new(""timestamp"", DateTime.UtcNow.ToString(""O""))
        });
    }
}

public class OrderStreamConsumer : BackgroundService
{
    private readonly IDatabase _db;
    private readonly string _groupName = ""processors"";
    private readonly string _consumerName;

    public OrderStreamConsumer(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
        _consumerName = $""consumer-{Environment.MachineName}"";
        EnsureGroupExists().Wait();
    }

    private async Task EnsureGroupExists()
    {
        try
        {
            await _db.StreamCreateConsumerGroupAsync(
                ""orders"", _groupName, ""$"", true);
        }
        catch (RedisServerException) { /* 群組已存在 */ }
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var entries = await _db.StreamReadGroupAsync(
                ""orders"", _groupName, _consumerName,
                "">"",      // 只讀未分配的新訊息
                count: 10);

            foreach (var entry in entries)
            {
                try
                {
                    var orderId = entry.Values
                        .First(v => v.Name == ""orderId"").Value;
                    Console.WriteLine($""處理訂單: {orderId}"");

                    // 處理完成，確認
                    await _db.StreamAcknowledgeAsync(
                        ""orders"", _groupName, entry.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($""處理失敗: {ex.Message}"");
                    // 不 ACK，訊息會重新分配
                }
            }

            if (entries.Length == 0)
                await Task.Delay(1000, ct);  // 沒有新訊息時等待
        }
    }
}
```

---

## 📌 對比 RabbitMQ / Kafka

| 特性 | Redis Pub/Sub | Redis Streams | RabbitMQ | Kafka |
|------|-------------|-------------|----------|-------|
| 持久化 | ❌ | ✅ | ✅ | ✅ |
| 消費者群組 | ❌ | ✅ | ✅ | ✅ |
| 訊息確認 | ❌ | ✅ | ✅ | ✅ |
| 效能 | 極高 | 高 | 中 | 高 |
| 複雜度 | 低 | 中 | 高 | 高 |
| 適用場景 | 即時通知 | 輕量 MQ | 企業級 MQ | 大數據串流 |

---

## 📌 範例：即時通知系統

```csharp
// 混合使用 Pub/Sub + Streams
public class NotificationService
{
    private readonly ISubscriber _pubsub;
    private readonly IDatabase _db;

    // 即時推送用 Pub/Sub（在線用戶立即收到）
    public async Task SendRealtimeNotification(
        string userId, string message)
    {
        await _pubsub.PublishAsync(
            $""user:{userId}:notifications"", message);
    }

    // 持久化用 Streams（離線用戶上線後讀取）
    public async Task SaveNotification(
        string userId, string message)
    {
        await _db.StreamAddAsync(
            $""notifications:{userId}"",
            new NameValueEntry[]
            {
                new(""message"", message),
                new(""timestamp"", DateTime.UtcNow.ToString(""O"")),
                new(""read"", ""false"")
            },
            maxLength: 100);  // 保留最近 100 筆
    }
}
```

---

## 🔑 重點整理

1. **Pub/Sub** 適合即時推送，但訊息不持久化
2. **Streams** 是 Redis 5.0+ 的持久化訊息佇列
3. **Consumer Group** 讓多個消費者分攤訊息處理
4. 簡單場景用 Redis，複雜場景考慮 **RabbitMQ / Kafka**
5. 實際系統常 **混合使用** Pub/Sub + Streams
"
            },

            // ── Chapter 1208: Redis Session 管理與分散式鎖 ──
            new Chapter
            {
                Id = 1208,
                Title = "Redis Session 管理與分散式鎖",
                Slug = "redis-session",
                Category = "redis",
                Order = 128,
                Level = "intermediate",
                Icon = "🔐",
                IsPublished = true,
                Content = @"# 🔐 Redis Session 管理與分散式鎖

## 📌 ASP.NET Core 分散式 Session

### 為什麼需要分散式 Session？

```
單機 Session（In-Memory）：
User → Server A (Session 在這) ✅
User → Server B (沒有 Session) ❌  ← 負載平衡切換後 Session 不見了

分散式 Session（Redis）：
User → Server A → Redis (Session) ✅
User → Server B → Redis (Session) ✅  ← 所有 Server 共用 Session
```

---

## 📌 設定 Redis Session Provider

```bash
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
```

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 1. 註冊 Redis 分散式快取
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration
        .GetConnectionString(""Redis"");
    options.InstanceName = ""DevLearn:Session:"";
});

// 2. 註冊 Session 服務
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

// 3. 啟用 Session 中介軟體
app.UseSession();
```

### 使用 Session

```csharp
// 設定 Session
public IActionResult Login(string username)
{
    HttpContext.Session.SetString(""Username"", username);
    HttpContext.Session.SetInt32(""LoginCount"",
        (HttpContext.Session.GetInt32(""LoginCount"") ?? 0) + 1);

    return Ok(""登入成功"");
}

// 讀取 Session
public IActionResult Profile()
{
    var username = HttpContext.Session.GetString(""Username"");
    if (username == null)
        return Unauthorized(""請先登入"");

    return Ok(new { Username = username });
}

// 存物件（需要 Extension Method）
public static class SessionExtensions
{
    public static void SetObject<T>(
        this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? GetObject<T>(
        this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}

// 使用
HttpContext.Session.SetObject(""Cart"", new ShoppingCart { Items = items });
var cart = HttpContext.Session.GetObject<ShoppingCart>(""Cart"");
```

---

## 📌 Session vs JWT 的取捨

| 特性 | Session (Redis) | JWT |
|------|----------------|-----|
| 狀態 | 有狀態（存在 Server） | 無狀態（存在 Client） |
| 儲存 | Redis | Cookie / LocalStorage |
| 撤銷 | 容易（刪除 Session） | 困難（需黑名單） |
| 效能 | 每次要查 Redis | 不需查 Server |
| 大小 | Cookie 只有 Session ID | Token 可能很大 |
| 適用 | 傳統 Web | API / 微服務 |

> **建議：** 傳統 MVC 用 Session，SPA/API 用 JWT，也可以混合使用。

---

## 📌 分散式鎖（Distributed Lock）

### 為什麼需要分散式鎖？

```
沒有鎖的情況：
User A: 讀取庫存(10) → 扣減 → 寫入庫存(9)
User B: 讀取庫存(10) → 扣減 → 寫入庫存(9)  ← 應該是 8！
→ 超賣問題！

有分散式鎖：
User A: 取得鎖 → 讀取(10) → 扣減 → 寫入(9) → 釋放鎖
User B: 等待鎖... → 取得鎖 → 讀取(9) → 扣減 → 寫入(8) → 釋放鎖
→ 正確！
```

### 基本實作：SETNX

```csharp
public class RedisDistributedLock
{
    private readonly IDatabase _db;

    public RedisDistributedLock(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    // 取得鎖
    public async Task<bool> AcquireLockAsync(
        string lockKey, string lockValue, TimeSpan expiry)
    {
        // SETNX：只在 key 不存在時設定
        return await _db.StringSetAsync(
            lockKey, lockValue, expiry, When.NotExists);
    }

    // 釋放鎖（用 Lua 確保原子性）
    public async Task<bool> ReleaseLockAsync(
        string lockKey, string lockValue)
    {
        // 只有持有者才能釋放（防止誤刪別人的鎖）
        var script = @""
            if redis.call('get', KEYS[1]) == ARGV[1] then
                return redis.call('del', KEYS[1])
            else
                return 0
            end"";

        var result = await _db.ScriptEvaluateAsync(
            script,
            new RedisKey[] { lockKey },
            new RedisValue[] { lockValue });

        return (int)result == 1;
    }
}
```

### 使用範例：庫存扣減

```csharp
public class InventoryService
{
    private readonly RedisDistributedLock _lock;
    private readonly AppDbContext _db;

    public async Task<bool> DeductStock(int productId, int quantity)
    {
        var lockKey = $""lock:inventory:{productId}"";
        var lockValue = Guid.NewGuid().ToString();
        var lockExpiry = TimeSpan.FromSeconds(10);

        // 嘗試取得鎖
        if (!await _lock.AcquireLockAsync(lockKey, lockValue, lockExpiry))
        {
            // 取不到鎖，可以重試或回傳失敗
            return false;
        }

        try
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null || product.Stock < quantity)
                return false;

            product.Stock -= quantity;
            await _db.SaveChangesAsync();
            return true;
        }
        finally
        {
            // 一定要釋放鎖！
            await _lock.ReleaseLockAsync(lockKey, lockValue);
        }
    }
}
```

---

## 📌 RedLock 演算法

在 Redis Cluster 環境中，單個 Redis 節點的鎖不夠可靠。
**RedLock** 要在多數節點上取得鎖才算成功。

```
RedLock 流程：
1. 取得當前時間 T1
2. 在 N 個 Redis 節點上嘗試取得鎖（短超時）
3. 在多數節點（N/2 + 1）成功取得 → 鎖定成功
4. 有效時間 = 初始 TTL - (T2 - T1)
5. 如果失敗 → 在所有節點釋放鎖
```

```csharp
// 使用 RedLock.net 套件
// dotnet add package RedLock.net

using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

var endPoints = new List<RedLockMultiplexer>
{
    ConnectionMultiplexer.Connect(""redis1:6379""),
    ConnectionMultiplexer.Connect(""redis2:6379""),
    ConnectionMultiplexer.Connect(""redis3:6379"")
};

var redlockFactory = RedLockFactory.Create(endPoints);

// 取得分散式鎖
await using var redLock = await redlockFactory.CreateLockAsync(
    resource: ""inventory:product:1001"",
    expiryTime: TimeSpan.FromSeconds(30));

if (redLock.IsAcquired)
{
    // 安全地執行庫存扣減
    await DeductStock(1001, 1);
}
else
{
    Console.WriteLine(""無法取得鎖，請稍後重試"");
}
```

---

## 📌 避免死鎖的技巧

| 技巧 | 說明 |
|------|------|
| **設定 TTL** | 鎖一定要有過期時間，避免持有者掛掉永遠不釋放 |
| **唯一 lockValue** | 用 GUID 標識持有者，避免誤刪別人的鎖 |
| **Lua 原子釋放** | 用 Lua 腳本確保「檢查 + 刪除」是原子操作 |
| **重試機制** | 取不到鎖時，用指數退避重試 |
| **看門狗** | 長任務自動延長鎖的 TTL |

```csharp
// 指數退避重試
public async Task<bool> AcquireWithRetry(
    string lockKey, string lockValue,
    TimeSpan expiry, int maxRetries = 3)
{
    for (int i = 0; i < maxRetries; i++)
    {
        if (await _lock.AcquireLockAsync(lockKey, lockValue, expiry))
            return true;

        // 指數退避：100ms, 200ms, 400ms...
        await Task.Delay(TimeSpan.FromMilliseconds(100 * Math.Pow(2, i)));
    }
    return false;
}
```

---

## 🔑 重點整理

1. **Redis Session** 讓多台 Server 共享 Session 資料
2. **Session vs JWT** 各有優缺，按場景選擇
3. **分散式鎖** 用 SETNX + TTL + Lua 實作
4. **RedLock** 適用於 Redis Cluster 的可靠鎖定
5. 鎖一定要設 **TTL** 和 **唯一標識**，避免死鎖
"
            },

            // ── Chapter 1209: Redis 效能優化與監控 ──
            new Chapter
            {
                Id = 1209,
                Title = "Redis 效能優化與監控",
                Slug = "redis-performance",
                Category = "redis",
                Order = 129,
                Level = "advanced",
                Icon = "🚀",
                IsPublished = true,
                Content = @"# 🚀 Redis 效能優化與監控

## 📌 Pipeline 批量操作

每個 Redis 命令都是一次網路往返（RTT），大量命令時網路延遲會成為瓶頸。

```
沒有 Pipeline：
Client → SET key1 → Server → OK → Client
Client → SET key2 → Server → OK → Client
Client → SET key3 → Server → OK → Client
= 3 次 RTT

有 Pipeline：
Client → SET key1, SET key2, SET key3 → Server
Server → OK, OK, OK → Client
= 1 次 RTT
```

### .NET Pipeline 實作

```csharp
var db = redis.GetDatabase();

// ❌ 逐一操作（慢）
for (int i = 0; i < 1000; i++)
    await db.StringSetAsync($""key:{i}"", $""value:{i}"");

// ✅ Pipeline 批量操作（快 10 倍以上）
var batch = db.CreateBatch();
var tasks = new List<Task>();

for (int i = 0; i < 1000; i++)
{
    tasks.Add(batch.StringSetAsync($""key:{i}"", $""value:{i}""));
}

batch.Execute();
await Task.WhenAll(tasks);

// ✅ 或使用 FireAndForget（不需等待結果）
for (int i = 0; i < 1000; i++)
{
    db.StringSet($""key:{i}"", $""value:{i}"",
        flags: CommandFlags.FireAndForget);
}
```

---

## 📌 Lua Script 原子操作

Redis 執行 Lua 腳本時是 **原子性** 的，不會被其他命令打斷。

```csharp
// 範例：限流器（每分鐘最多 100 次請求）
public class RateLimiter
{
    private readonly IDatabase _db;

    private const string LuaScript = @""
        local key = KEYS[1]
        local limit = tonumber(ARGV[1])
        local window = tonumber(ARGV[2])

        local current = tonumber(redis.call('GET', key) or '0')

        if current < limit then
            redis.call('INCR', key)
            if current == 0 then
                redis.call('EXPIRE', key, window)
            end
            return 1  -- 允許
        else
            return 0  -- 拒絕
        end"";

    public async Task<bool> IsAllowed(string clientId)
    {
        var result = await _db.ScriptEvaluateAsync(
            LuaScript,
            new RedisKey[] { $""ratelimit:{clientId}"" },
            new RedisValue[] { 100, 60 });  // 每 60 秒 100 次

        return (int)result == 1;
    }
}

// 在 Middleware 中使用
app.Use(async (context, next) =>
{
    var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? ""unknown"";
    var limiter = context.RequestServices.GetRequiredService<RateLimiter>();

    if (!await limiter.IsAllowed(clientIp))
    {
        context.Response.StatusCode = 429;
        await context.Response.WriteAsync(""Too Many Requests"");
        return;
    }

    await next();
});
```

---

## 📌 大 Key 問題與解法

大 Key 會導致：記憶體不均、網路阻塞、刪除時阻塞其他操作。

```bash
# 找出大 Key
redis-cli --bigkeys

# 查看特定 Key 的記憶體用量
MEMORY USAGE user:sessions
```

### 解法

```csharp
// ❌ 一個 key 存大量資料
await db.StringSetAsync(""all_products"",
    JsonSerializer.Serialize(allProducts));  // 可能 50MB！

// ✅ 拆分成多個小 key
foreach (var product in allProducts)
{
    await db.StringSetAsync(
        $""product:{product.Id}"",
        JsonSerializer.Serialize(product));
}

// ❌ 一個 Hash 存百萬欄位
await db.HashSetAsync(""user_scores"",
    scores.Select(s => new HashEntry(s.UserId, s.Score)).ToArray());

// ✅ 分桶（Bucket）
foreach (var score in scores)
{
    var bucket = score.UserId % 100;  // 分成 100 個桶
    await db.HashSetAsync(
        $""user_scores:{bucket}"",
        score.UserId.ToString(), score.Score);
}
```

---

## 📌 慢查詢日誌 SLOWLOG

```bash
# 設定慢查詢閾值（微秒，10000 = 10ms）
CONFIG SET slowlog-log-slower-than 10000

# 設定保留的慢查詢數量
CONFIG SET slowlog-max-len 128

# 查看慢查詢
SLOWLOG GET 10

# 查看慢查詢數量
SLOWLOG LEN

# 清除
SLOWLOG RESET
```

### 常見慢操作

| 操作 | 時間複雜度 | 建議 |
|------|-----------|------|
| `KEYS *` | O(N) | 用 SCAN 替代 |
| `HGETALL` (大 Hash) | O(N) | 只取需要的欄位 |
| `SMEMBERS` (大 Set) | O(N) | 用 SSCAN 替代 |
| `DEL` (大 key) | O(N) | 用 UNLINK 非同步刪除 |
| `FLUSHDB` | O(N) | 用 FLUSHDB ASYNC |

```csharp
// ✅ 用 SCAN 替代 KEYS（不阻塞）
var server = redis.GetServer(""localhost:6379"");
await foreach (var key in server.KeysAsync(pattern: ""product:*""))
{
    Console.WriteLine(key);
}

// ✅ 用 UNLINK 替代 DEL（非同步刪除大 key）
await db.KeyDeleteAsync(""big_key"", CommandFlags.FireAndForget);
```

---

## 📌 Redis INFO 監控指標

```bash
# 查看所有資訊
INFO

# 查看特定區段
INFO memory
INFO stats
INFO clients
INFO replication
```

### 重要監控指標

| 指標 | 說明 | 警戒值 |
|------|------|--------|
| `used_memory` | 已用記憶體 | 接近 maxmemory |
| `connected_clients` | 連線數 | > 1000 |
| `instantaneous_ops_per_sec` | 每秒操作數 | 接近瓶頸 |
| `hit_rate` | 命中率 | < 90% |
| `evicted_keys` | 被淘汰的 key 數 | > 0 |
| `blocked_clients` | 阻塞的客戶端 | > 0 |

```csharp
// .NET 中取得 Redis 資訊
var server = redis.GetServer(""localhost:6379"");
var info = await server.InfoAsync();

foreach (var group in info)
{
    Console.WriteLine($""=== {group.Key} ==="");
    foreach (var pair in group)
        Console.WriteLine($""  {pair.Key}: {pair.Value}"");
}

// 取得特定指標
var memoryInfo = (await server.InfoAsync(""memory""))
    .SelectMany(g => g)
    .ToDictionary(p => p.Key, p => p.Value);
Console.WriteLine($""已用記憶體: {memoryInfo[""used_memory_human""]}"");
```

---

## 📌 持久化策略：RDB vs AOF

| 特性 | RDB | AOF |
|------|-----|-----|
| 方式 | 定時快照 | 追加寫入日誌 |
| 檔案大小 | 較小 | 較大 |
| 恢復速度 | 快 | 慢 |
| 資料安全 | 可能丟失最近快照後的資料 | 最多丟 1 秒 |
| 效能影響 | fork 時短暫阻塞 | 持續寫入 |

```bash
# RDB 設定（redis.conf）
save 900 1      # 900 秒內至少 1 次修改則快照
save 300 10     # 300 秒內至少 10 次修改則快照
save 60 10000   # 60 秒內至少 10000 次修改則快照

# AOF 設定
appendonly yes
appendfsync everysec   # 每秒同步（推薦）
# appendfsync always   # 每次寫入都同步（最安全但慢）
# appendfsync no       # 由 OS 決定（最快但不安全）
```

> **推薦：** 同時開啟 RDB + AOF，兼顧效能和安全。

---

## 📌 Azure Cache for Redis

```csharp
// appsettings.json
{
  ""ConnectionStrings"": {
    ""Redis"": ""your-cache.redis.cache.windows.net:6380,password=xxx,ssl=True,abortConnect=False""
  }
}
```

### Azure 方案比較

| 方案 | 記憶體 | 價格/月 | 適用 |
|------|-------|---------|------|
| Basic C0 | 250MB | ~$16 | 開發測試 |
| Standard C1 | 1GB | ~$60 | 小型生產 |
| Premium P1 | 6GB | ~$200 | 企業級 |
| Enterprise E10 | 12GB | ~$400 | 大規模 |

---

## 📌 .NET 效能最佳實踐

```csharp
// 1. ConnectionMultiplexer 必須是 Singleton
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(config));

// 2. 避免使用 KEYS，改用 SCAN
// ❌ db.Execute(""KEYS"", ""*"");
// ✅ server.Keys(pattern: ""prefix:*"");

// 3. 序列化用 System.Text.Json（比 Newtonsoft.Json 快）
var json = JsonSerializer.Serialize(obj);

// 4. 大量讀取用 Pipeline
var batch = db.CreateBatch();
var tasks = keys.Select(k => batch.StringGetAsync(k)).ToArray();
batch.Execute();
var results = await Task.WhenAll(tasks);

// 5. 不需要結果時用 FireAndForget
await db.StringSetAsync(key, value, flags: CommandFlags.FireAndForget);

// 6. 合理設定 TTL，避免記憶體膨脹
await db.StringSetAsync(key, value, TimeSpan.FromMinutes(30));

// 7. 使用 Hash 存物件（比整個 JSON 更省記憶體）
await db.HashSetAsync(""user:1001"", entries);

// 8. 監控連線池狀態
var status = redis.GetStatus();
Console.WriteLine(status);
```

---

## 🔑 重點整理

1. **Pipeline** 把多個命令合併成一次網路往返，大幅提升效能
2. **Lua Script** 保證原子性，適合限流器、分散式鎖等場景
3. 避免 **大 Key**，用拆分或分桶策略
4. 用 **SCAN** 替代 **KEYS**，用 **UNLINK** 替代 **DEL**
5. 同時開啟 **RDB + AOF** 持久化
6. Azure Cache for Redis 是雲端最方便的方案
"
            },
        };
    }
}
