using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Microservices
{
    public static List<Chapter> GetChapters()
    {
        return new List<Chapter>
        {
            // ── Chapter 1100: 微服務架構入門：從單體到微服務 ──
            new Chapter
            {
                Id = 1100,
                Title = "微服務架構入門：從單體到微服務",
                Slug = "micro-intro",
                Category = "microservices",
                Order = 110,
                Level = "beginner",
                Icon = "🏗️",
                IsPublished = true,
                Content = @"# 🏗️ 微服務架構入門：從單體到微服務

## 📌 什麼是微服務？

微服務（Microservices）是一種軟體架構風格，將應用程式拆分成**一組小型、獨立的服務**，每個服務圍繞特定的業務功能建構，可以獨立開發、部署和擴展。

> **核心概念：** 每個微服務就像一個獨立的小團隊，負責一件事，做好做滿。

---

## 📌 單體架構 vs 微服務架構

### 單體架構 (Monolithic Architecture)

所有功能都打包在**一個應用程式**中：

```
┌─────────────────────────────────┐
│         單體應用程式              │
│  ┌─────┐ ┌─────┐ ┌─────┐       │
│  │用戶  │ │訂單  │ │支付  │       │
│  │管理  │ │處理  │ │系統  │       │
│  └──┬──┘ └──┬──┘ └──┬──┘       │
│     └───────┼───────┘           │
│          共用資料庫               │
│       ┌──────────┐              │
│       │ Database │              │
│       └──────────┘              │
└─────────────────────────────────┘
```

### 微服務架構 (Microservices Architecture)

每個服務是**獨立的應用程式**，有自己的資料庫：

```
┌──────────┐  ┌──────────┐  ┌──────────┐
│ 用戶服務  │  │ 訂單服務  │  │ 支付服務  │
│          │  │          │  │          │
│ ┌──────┐ │  │ ┌──────┐ │  │ ┌──────┐ │
│ │ DB1  │ │  │ │ DB2  │ │  │ │ DB3  │ │
│ └──────┘ │  │ └──────┘ │  │ └──────┘ │
└──────────┘  └──────────┘  └──────────┘
     ↕              ↕              ↕
  ═══════════ API Gateway ═══════════
```

---

## 📌 為什麼要用微服務？優缺點分析

### ✅ 優點

| 優點 | 說明 |
|------|------|
| **獨立部署** | 修改訂單服務不需要重新部署整個系統 |
| **技術多樣性** | 用戶服務用 C#、推薦服務用 Python，各取所長 |
| **獨立擴展** | 促銷時只需擴展訂單服務，不用擴展整個系統 |
| **故障隔離** | 支付服務掛了，用戶瀏覽商品不受影響 |
| **團隊自治** | 每個團隊負責自己的服務，降低協調成本 |

### ❌ 缺點

| 缺點 | 說明 |
|------|------|
| **複雜度高** | 分散式系統帶來網路、一致性等挑戰 |
| **維運成本** | 需要監控、日誌、追蹤等基礎設施 |
| **資料一致性** | 跨服務交易很困難（不能用單一 Transaction） |
| **團隊要求** | 需要 DevOps 文化和自動化能力 |

---

## 📌 微服務的核心原則

### 1. 單一職責原則 (Single Responsibility)

```csharp
// ✅ 好的拆分：每個服務只負責一個業務領域
public class OrderService     // 只處理訂單相關邏輯
public class InventoryService // 只處理庫存相關邏輯
public class PaymentService   // 只處理支付相關邏輯

// ❌ 不好的拆分：一個服務做太多事
public class EverythingService // 訂單 + 庫存 + 支付 + 用戶 + ...
```

### 2. 獨立部署 (Independent Deployment)

每個服務都有自己的：
- **程式碼倉庫**（或 monorepo 中的獨立專案）
- **CI/CD Pipeline**
- **部署環境**

### 3. 去中心化治理 (Decentralized Governance)

```csharp
// 每個服務可以選擇最適合的技術棧
// 訂單服務：ASP.NET Core + SQL Server
builder.Services.AddDbContext<OrderDbContext>(opt =>
    opt.UseSqlServer(connectionString));

// 快取服務：可以用不同的資料庫
// 例如 Redis、MongoDB 等
```

---

## 📌 真實世界案例

### Netflix
- 從單體 Java 應用遷移到 **700+ 微服務**
- 每天處理超過 **20 億** API 請求
- 使用自研的 Eureka（服務發現）、Zuul（API Gateway）

### Amazon
- 從龐大的單體架構拆分成微服務
- ""兩個披薩團隊""原則：每個服務團隊不超過兩個披薩能餵飽的人數
- 這直接催生了 AWS 雲端服務

### Uber
- 原本單體架構導致部署時間長達數小時
- 拆分為微服務後，各團隊可以**獨立且頻繁地部署**

---

## 📌 什麼時候不該用微服務？

> **重要提醒：** 微服務不是銀彈！

```
❌ 不適合微服務的情境：
├── 小型專案或 MVP（單體就夠了）
├── 團隊人數少於 5 人
├── 業務邊界不明確（還在探索階段）
├── 沒有 DevOps 自動化能力
└── 對分散式系統缺乏經驗
```

### 建議的演進路線

```csharp
// 階段 1：先用模組化的單體架構
// 將程式碼按業務邊界分模組，但部署為單一應用
public class ModularMonolith
{
    // 模組 A：訂單
    // 模組 B：庫存
    // 模組 C：支付
    // 各模組透過介面溝通，為未來拆分做準備
}

// 階段 2：識別出需要獨立擴展的模組，逐步拆出
// 例如：訂單模組流量特別大 → 拆成獨立服務
```

---

## 📌 .NET 微服務生態系工具概覽

| 工具 / 框架 | 用途 |
|-------------|------|
| **ASP.NET Core** | 建立 RESTful API / gRPC 服務 |
| **YARP / Ocelot** | API Gateway 反向代理 |
| **MassTransit** | 訊息匯流排（RabbitMQ / Azure Service Bus） |
| **Polly** | 韌性與容錯（重試、斷路器） |
| **OpenTelemetry** | 分散式追蹤與可觀測性 |
| **Docker** | 容器化打包 |
| **Kubernetes** | 容器編排與自動擴展 |
| **Dapr** | 分散式應用執行環境 |
| **HealthChecks** | 服務健康狀態監控 |

---

## 📌 本系列學習路線圖

```
1. 微服務入門（本章）
2. DDD 與邊界劃分
3. 建立微服務 API
4. 服務間通訊
5. API Gateway
6. Docker 容器化
7. 韌性模式（Polly）
8. 資料管理（Saga）
9. 可觀測性
10. Kubernetes 部署
```

> **下一章：** 我們將深入 Domain-Driven Design（DDD），學習如何正確劃分微服務的邊界。
"
            },

            // ── Chapter 1101: 微服務設計原則：DDD 與邊界劃分 ──
            new Chapter
            {
                Id = 1101,
                Title = "微服務設計原則：DDD 與邊界劃分",
                Slug = "micro-design",
                Category = "microservices",
                Order = 111,
                Level = "beginner",
                Icon = "📐",
                IsPublished = true,
                Content = @"# 📐 微服務設計原則：DDD 與邊界劃分

## 📌 Domain-Driven Design (DDD) 基礎概念

DDD 是一種軟體設計方法，**以業務領域為核心**來組織程式碼。在微服務架構中，DDD 幫助我們找到正確的服務邊界。

> **為什麼 DDD 對微服務這麼重要？** 因為微服務拆分的依據不是技術層（Controller、Service、Repository），而是**業務邊界**。

---

## 📌 核心概念：通用語言 (Ubiquitous Language)

開發團隊與業務專家使用**同一套語言**來描述系統：

```csharp
// ❌ 技術導向的命名
public class DataProcessor
{
    public void ProcessRecord(int recordId) { }
}

// ✅ 業務導向的命名（通用語言）
public class OrderService
{
    public void PlaceOrder(PlaceOrderCommand command) { }
    public void CancelOrder(Guid orderId, string reason) { }
    public void ShipOrder(Guid orderId, ShippingInfo shipping) { }
}
```

---

## 📌 Bounded Context 限界上下文

**限界上下文**是 DDD 中最重要的概念之一。同一個名詞在不同的上下文中可能有不同的含義。

### 例子：""產品"" 在不同上下文的意義

```csharp
// ── 商品目錄上下文 (Catalog Context) ──
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal ListPrice { get; set; }
    public List<string> Images { get; set; }
    public Category Category { get; set; }
}

// ── 訂單上下文 (Order Context) ──
// 同樣叫 ""Product""，但只需要訂單相關的資訊
public class OrderItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }  // 快照，不是引用
    public decimal UnitPrice { get; set; }   // 下單時的價格
    public int Quantity { get; set; }
}

// ── 庫存上下文 (Inventory Context) ──
public class StockItem
{
    public Guid ProductId { get; set; }
    public string Sku { get; set; }
    public int QuantityOnHand { get; set; }
    public int ReorderThreshold { get; set; }
    public string WarehouseLocation { get; set; }
}
```

> **重點：** 每個限界上下文有自己的 ""Product"" 模型，只包含該上下文需要的屬性。

---

## 📌 聚合根、實體與值物件

### 聚合根 (Aggregate Root)

聚合根是**外部存取聚合的唯一入口**，確保業務規則的一致性。

```csharp
// Order 是聚合根
public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    private readonly List<OrderLine> _lines = new();
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();

    // 業務邏輯都透過聚合根來操作
    public void AddItem(Guid productId, string productName,
                        decimal unitPrice, int quantity)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException(""只有草稿狀態的訂單可以加入商品"");

        var existing = _lines.FirstOrDefault(l => l.ProductId == productId);
        if (existing != null)
        {
            existing.IncreaseQuantity(quantity);
        }
        else
        {
            _lines.Add(new OrderLine(productId, productName, unitPrice, quantity));
        }
    }

    public void Submit()
    {
        if (!_lines.Any())
            throw new InvalidOperationException(""訂單至少要有一個商品"");

        Status = OrderStatus.Submitted;
        // 發出領域事件
        AddDomainEvent(new OrderSubmittedEvent(Id, CustomerId, GetTotal()));
    }

    public decimal GetTotal() => _lines.Sum(l => l.SubTotal);
}
```

### 實體 (Entity) — 有唯一識別的物件

```csharp
// OrderLine 是實體（有 Id），屬於 Order 聚合
public class OrderLine
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal SubTotal => UnitPrice * Quantity;

    public OrderLine(Guid productId, string productName,
                     decimal unitPrice, int quantity)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0) throw new ArgumentException(""數量必須大於 0"");
        Quantity += amount;
    }
}
```

### 值物件 (Value Object) — 用值來比較的物件

```csharp
// Address 是值物件：沒有 Id，用值來比較
public record Address(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country)
{
    // C# record 自動實作值比較
    // new Address(""信義路"", ""台北"", ...) == new Address(""信義路"", ""台北"", ...)
}

// Money 也是值物件
public record Money(decimal Amount, string Currency)
{
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException(""不同幣別無法直接相加"");
        return new Money(a.Amount + b.Amount, a.Currency);
    }
}
```

---

## 📌 如何劃分微服務的邊界

### 步驟 1：識別業務領域

```
電商系統的業務領域：
├── 用戶管理（註冊、登入、個人資料）
├── 商品目錄（瀏覽、搜尋、分類）
├── 訂單管理（下單、取消、查詢）
├── 庫存管理（庫存數量、進出貨）
├── 支付處理（付款、退款）
├── 物流配送（出貨、追蹤）
└── 通知服務（Email、SMS、推播）
```

### 步驟 2：畫出上下文映射圖 (Context Map)

```
┌──────────┐    同步呼叫    ┌──────────┐
│ 訂單服務  │ ───────────→ │ 庫存服務  │
│          │              │          │
└────┬─────┘              └──────────┘
     │ 發布事件
     ↓
┌──────────┐              ┌──────────┐
│ 支付服務  │              │ 通知服務  │
│          │              │ (訂閱事件)│
└──────────┘              └──────────┘
```

---

## 📌 資料庫分離策略：Database per Service

```csharp
// 每個微服務有自己的 DbContext 和資料庫
// ── 訂單服務 ──
public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(""Host=order-db;Database=OrderDb"");
}

// ── 庫存服務 ──
public class InventoryDbContext : DbContext
{
    public DbSet<StockItem> StockItems { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(""Host=inventory-db;Database=InventoryDb"");
}
```

> **黃金法則：** 服務之間**絕不直接存取**對方的資料庫，只透過 API 或事件溝通。

---

## 📌 範例：電商系統的服務拆分

| 服務 | 職責 | 資料庫 | 主要 API |
|------|------|--------|---------|
| **UserService** | 用戶註冊、登入、Profile | PostgreSQL | `POST /api/users`, `GET /api/users/{id}` |
| **CatalogService** | 商品 CRUD、搜尋 | PostgreSQL + Elasticsearch | `GET /api/products`, `GET /api/products/{id}` |
| **OrderService** | 下單、訂單狀態管理 | PostgreSQL | `POST /api/orders`, `GET /api/orders/{id}` |
| **InventoryService** | 庫存管理、扣減 | PostgreSQL | `POST /api/inventory/reserve`, `PUT /api/inventory/release` |
| **PaymentService** | 支付、退款 | PostgreSQL | `POST /api/payments`, `POST /api/refunds` |
| **NotificationService** | 發送通知 | MongoDB | 透過訊息佇列觸發 |

> **下一章：** 我們將動手用 ASP.NET Core 建立第一個微服務 API。
"
            },

            // ── Chapter 1102: 用 ASP.NET Core 建立微服務 API ──
            new Chapter
            {
                Id = 1102,
                Title = "用 ASP.NET Core 建立微服務 API",
                Slug = "micro-api",
                Category = "microservices",
                Order = 112,
                Level = "beginner",
                Icon = "🔌",
                IsPublished = true,
                Content = @"# 🔌 用 ASP.NET Core 建立微服務 API

## 📌 Minimal API vs Controller-based API

ASP.NET Core 提供兩種建立 API 的方式：

```csharp
// ── 方式 1：Minimal API（簡潔、適合小型微服務） ──
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet(""/api/products"", async (ProductDbContext db) =>
    await db.Products.ToListAsync());

app.MapGet(""/api/products/{id}"", async (int id, ProductDbContext db) =>
    await db.Products.FindAsync(id) is Product p
        ? Results.Ok(p)
        : Results.NotFound());

app.Run();

// ── 方式 2：Controller-based API（結構化、適合大型服務） ──
[ApiController]
[Route(""api/[controller]"")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet(""{id}"")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }
}
```

| 比較 | Minimal API | Controller-based |
|------|-------------|-----------------|
| 程式碼量 | 少 | 多 |
| 學習曲線 | 低 | 中 |
| 適用場景 | 小型微服務 | 大型、複雜的服務 |
| 過濾器 | EndpointFilter | ActionFilter |
| API 版本控制 | 支援 | 支援 |

---

## 📌 建立第一個微服務 API

### 步驟 1：建立專案

```bash
# 建立方案與專案
dotnet new sln -n EShop
dotnet new webapi -n ProductService --no-https
dotnet sln add ProductService/ProductService.csproj

# 加入必要套件
cd ProductService
dotnet add package Microsoft.EntityFrameworkCore.Npgsql
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package AspNetCore.HealthChecks.NpgSql
```

### 步驟 2：定義領域模型

```csharp
// Models/Product.cs
namespace ProductService.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
```

---

## 📌 RESTful 設計原則

```
RESTful API 設計規範：
├── 使用名詞而非動詞：/api/products（不是 /api/getProducts）
├── 使用複數形式：/api/products（不是 /api/product）
├── 使用 HTTP 方法表達操作：
│   ├── GET    /api/products      → 取得列表
│   ├── GET    /api/products/42   → 取得單筆
│   ├── POST   /api/products      → 建立
│   ├── PUT    /api/products/42   → 完整更新
│   ├── PATCH  /api/products/42   → 部分更新
│   └── DELETE /api/products/42   → 刪除
├── 使用正確的 HTTP 狀態碼
└── 使用巢狀路由表達關係：/api/orders/42/items
```

---

## 📌 DTO 與 AutoMapper

```csharp
// DTOs/ProductDto.cs — 回傳給客戶端的資料
public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    bool IsActive);

// DTOs/CreateProductDto.cs — 建立時的輸入
public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string Category);

// DTOs/UpdateProductDto.cs — 更新時的輸入
public record UpdateProductDto(
    string? Name,
    string? Description,
    decimal? Price,
    int? StockQuantity,
    string? Category);

// Profiles/ProductProfile.cs — AutoMapper 映射設定
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                srcMember != null)); // 只更新非 null 的欄位
    }
}
```

---

## 📌 健康檢查 (Health Checks)

微服務必須提供健康檢查端點，讓 API Gateway 和 Kubernetes 知道服務狀態。

```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: ""database"")
    .AddCheck(""self"", () => HealthCheckResult.Healthy());

app.MapHealthChecks(""/health"", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = ""application/json"";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.TotalMilliseconds
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

// /health/ready — 完整就緒檢查（包含資料庫）
app.MapHealthChecks(""/health/ready"", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains(""ready"")
});

// /health/live — 存活檢查（只檢查應用本身）
app.MapHealthChecks(""/health/live"", new HealthCheckOptions
{
    Predicate = _ => false // 不執行任何外部檢查
});
```

---

## 📌 Swagger / OpenAPI 文件

```csharp
// Program.cs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(""v1"", new OpenApiInfo
    {
        Title = ""Product Service API"",
        Version = ""v1"",
        Description = ""電商微服務 - 商品管理 API""
    });
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(""/swagger/v1/swagger.json"", ""Product Service v1"");
    c.RoutePrefix = string.Empty; // Swagger 作為首頁
});
```

---

## 📌 完整範例：Product Service

```csharp
// Program.cs — 完整的 Product 微服務
var builder = WebApplication.CreateBuilder(args);

// 服務注冊
builder.Services.AddDbContext<ProductDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString(""DefaultConnection"")));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString(""DefaultConnection"")!);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ── API 端點 ──
var products = app.MapGroup(""/api/products"");

products.MapGet(""/"", async (IProductRepository repo, IMapper mapper) =>
{
    var items = await repo.GetAllAsync();
    return Results.Ok(mapper.Map<List<ProductDto>>(items));
});

products.MapGet(""/{id}"", async (int id, IProductRepository repo, IMapper mapper) =>
{
    var product = await repo.GetByIdAsync(id);
    return product is null ? Results.NotFound() : Results.Ok(mapper.Map<ProductDto>(product));
});

products.MapPost(""/"", async (CreateProductDto dto, IProductRepository repo, IMapper mapper) =>
{
    var product = mapper.Map<Product>(dto);
    await repo.AddAsync(product);
    var result = mapper.Map<ProductDto>(product);
    return Results.Created($""/api/products/{product.Id}"", result);
});

app.MapHealthChecks(""/health"");

app.Run();
```

> **下一章：** 我們將學習微服務之間如何溝通 — 同步的 HTTP/gRPC 和非同步的訊息佇列。
"
            },

            // ── Chapter 1103: 微服務通訊：同步 vs 非同步 ──
            new Chapter
            {
                Id = 1103,
                Title = "微服務通訊：同步 vs 非同步",
                Slug = "micro-communication",
                Category = "microservices",
                Order = 113,
                Level = "intermediate",
                Icon = "📡",
                IsPublished = true,
                Content = @"# 📡 微服務通訊：同步 vs 非同步

## 📌 微服務通訊方式概覽

```
微服務通訊模式
├── 同步通訊（請求-回應）
│   ├── HTTP / REST
│   └── gRPC
└── 非同步通訊（事件驅動）
    ├── Message Queue（RabbitMQ、Azure Service Bus）
    └── Event Streaming（Kafka）
```

| 方式 | 耦合度 | 即時性 | 可靠性 | 適用場景 |
|------|--------|--------|--------|---------|
| HTTP REST | 中 | 高 | 依賴對方在線 | 查詢、CRUD |
| gRPC | 中 | 高 | 依賴對方在線 | 服務間高效能通訊 |
| Message Queue | 低 | 低 | 高（訊息持久化） | 事件通知、長流程 |

---

## 📌 同步通訊：HttpClient + HttpClientFactory

### 為什麼要用 HttpClientFactory？

```csharp
// ❌ 直接 new HttpClient — 會導致 Socket 耗盡！
var client = new HttpClient(); // 每次都建立新連線
var response = await client.GetAsync(""http://product-service/api/products"");
// 連線不會被正確釋放 → Socket exhaustion

// ✅ 使用 HttpClientFactory — 連線池管理
// Program.cs 註冊
builder.Services.AddHttpClient(""ProductService"", client =>
{
    client.BaseAddress = new Uri(""http://product-service"");
    client.DefaultRequestHeaders.Add(""Accept"", ""application/json"");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// 使用
public class OrderService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OrderService(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    public async Task<ProductDto?> GetProductAsync(int productId)
    {
        var client = _httpClientFactory.CreateClient(""ProductService"");
        var response = await client.GetAsync($""/api/products/{productId}"");

        if (!response.IsSuccessStatusCode) return null;

        return await response.Content
            .ReadFromJsonAsync<ProductDto>();
    }
}
```

### 具型別 HttpClient (Typed Client)

```csharp
// 更推薦的方式：具型別 HttpClient
public class ProductServiceClient
{
    private readonly HttpClient _client;

    public ProductServiceClient(HttpClient client)
    {
        client.BaseAddress = new Uri(""http://product-service"");
        _client = client;
    }

    public async Task<ProductDto?> GetProductAsync(int id)
        => await _client.GetFromJsonAsync<ProductDto>($""/api/products/{id}"");

    public async Task<List<ProductDto>> GetAllProductsAsync()
        => await _client.GetFromJsonAsync<List<ProductDto>>(""/api/products"") ?? new();

    public async Task<bool> CheckStockAsync(int productId, int quantity)
    {
        var response = await _client.GetAsync(
            $""/api/products/{productId}/stock?required={quantity}"");
        return response.IsSuccessStatusCode;
    }
}

// 註冊
builder.Services.AddHttpClient<ProductServiceClient>();
```

---

## 📌 gRPC 在 .NET 中的實作

gRPC 使用 Protocol Buffers 進行序列化，效能比 JSON 高出很多。

```protobuf
// Protos/product.proto
syntax = ""proto3"";
option csharp_namespace = ""ProductService.Grpc"";

package product;

service ProductGrpc {
  rpc GetProduct (GetProductRequest) returns (ProductReply);
  rpc GetProducts (GetProductsRequest) returns (stream ProductReply);
}

message GetProductRequest {
  int32 id = 1;
}

message GetProductsRequest {
  string category = 1;
  int32 page_size = 2;
}

message ProductReply {
  int32 id = 1;
  string name = 2;
  string description = 3;
  double price = 4;
  int32 stock_quantity = 5;
}
```

### gRPC 服務端

```csharp
public class ProductGrpcService : ProductGrpc.ProductGrpcBase
{
    private readonly ProductDbContext _db;

    public ProductGrpcService(ProductDbContext db) => _db = db;

    public override async Task<ProductReply> GetProduct(
        GetProductRequest request, ServerCallContext context)
    {
        var product = await _db.Products.FindAsync(request.Id)
            ?? throw new RpcException(
                new Status(StatusCode.NotFound, ""產品不存在""));

        return new ProductReply
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = (double)product.Price,
            StockQuantity = product.StockQuantity
        };
    }
}
```

### gRPC 客戶端

```csharp
// 在 Order Service 中呼叫 Product Service 的 gRPC
builder.Services.AddGrpcClient<ProductGrpc.ProductGrpcClient>(options =>
{
    options.Address = new Uri(""http://product-service:5001"");
});

public class OrderService
{
    private readonly ProductGrpc.ProductGrpcClient _productClient;

    public OrderService(ProductGrpc.ProductGrpcClient productClient)
        => _productClient = productClient;

    public async Task<ProductReply> GetProductInfoAsync(int productId)
    {
        return await _productClient.GetProductAsync(
            new GetProductRequest { Id = productId });
    }
}
```

---

## 📌 非同步通訊：RabbitMQ

### 安裝 RabbitMQ

```bash
# 用 Docker 啟動 RabbitMQ
docker run -d --name rabbitmq \
  -p 5672:5672 -p 15672:15672 \
  rabbitmq:3-management
```

### 使用 MassTransit 整合 RabbitMQ

```csharp
// 定義事件（共用的契約）
public record OrderCreatedEvent(
    Guid OrderId,
    Guid CustomerId,
    List<OrderItemInfo> Items,
    decimal TotalAmount,
    DateTime CreatedAt);

public record OrderItemInfo(int ProductId, int Quantity, decimal UnitPrice);

// ── 發布者（Order Service） ──
public class OrderService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderService(IPublishEndpoint publishEndpoint)
        => _publishEndpoint = publishEndpoint;

    public async Task CreateOrderAsync(CreateOrderCommand command)
    {
        // 1. 儲存訂單到資料庫
        var order = new Order { /* ... */ };
        await _orderRepo.AddAsync(order);

        // 2. 發布事件 — 其他服務會自動收到
        await _publishEndpoint.Publish(new OrderCreatedEvent(
            order.Id, order.CustomerId, order.Items.Select(i =>
                new OrderItemInfo(i.ProductId, i.Quantity, i.UnitPrice)).ToList(),
            order.TotalAmount, DateTime.UtcNow));
    }
}

// ── 消費者（Inventory Service） ──
public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IInventoryService _inventory;

    public OrderCreatedConsumer(IInventoryService inventory)
        => _inventory = inventory;

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        foreach (var item in message.Items)
        {
            await _inventory.ReserveStockAsync(
                item.ProductId, item.Quantity);
        }
    }
}
```

### 註冊 MassTransit

```csharp
// Program.cs — Inventory Service
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(""rabbitmq"", h =>
        {
            h.Username(""guest"");
            h.Password(""guest"");
        });
        cfg.ConfigureEndpoints(context);
    });
});
```

---

## 📌 範例比較：訂單建立的同步 vs 非同步

```csharp
// ── 同步方式：Order Service 直接呼叫 Inventory Service ──
public async Task<OrderResult> CreateOrderSync(CreateOrderCommand cmd)
{
    // 1. 呼叫庫存服務檢查 → 等待回應
    var stockOk = await _inventoryClient.CheckStockAsync(cmd.ProductId, cmd.Quantity);
    if (!stockOk) return OrderResult.Fail(""庫存不足"");

    // 2. 呼叫庫存服務扣減 → 等待回應
    await _inventoryClient.ReserveStockAsync(cmd.ProductId, cmd.Quantity);

    // 3. 儲存訂單
    var order = CreateOrder(cmd);
    await _orderRepo.AddAsync(order);

    return OrderResult.Success(order.Id);
    // 缺點：庫存服務掛了 → 整個下單流程失敗
}

// ── 非同步方式：透過事件通知 ──
public async Task<OrderResult> CreateOrderAsync(CreateOrderCommand cmd)
{
    // 1. 直接儲存訂單（狀態：Pending）
    var order = CreateOrder(cmd, OrderStatus.Pending);
    await _orderRepo.AddAsync(order);

    // 2. 發布事件 → 不等待回應
    await _publishEndpoint.Publish(new OrderCreatedEvent(order.Id, /* ... */));

    return OrderResult.Success(order.Id);
    // 優點：庫存服務暫時掛了也沒關係，訊息會排隊等待處理
}
```

> **下一章：** 我們將學習 API Gateway，統一管理所有微服務的入口。
"
            },

            // ── Chapter 1104: API Gateway：Ocelot 與 YARP ──
            new Chapter
            {
                Id = 1104,
                Title = "API Gateway：Ocelot 與 YARP",
                Slug = "micro-gateway",
                Category = "microservices",
                Order = 114,
                Level = "intermediate",
                Icon = "🚪",
                IsPublished = true,
                Content = @"# 🚪 API Gateway：Ocelot 與 YARP

## 📌 為什麼需要 API Gateway？

沒有 Gateway 時，前端需要知道每個微服務的位址：

```
前端直接呼叫多個微服務（❌ 不好的做法）：
├── http://user-service:5001/api/users
├── http://product-service:5002/api/products
├── http://order-service:5003/api/orders
└── http://payment-service:5004/api/payments
```

有 Gateway 後，前端只需要知道一個位址：

```
前端只呼叫 Gateway（✅ 推薦做法）：
└── http://api-gateway:5000/api/...
    ├── /api/users    → 轉發到 user-service
    ├── /api/products → 轉發到 product-service
    ├── /api/orders   → 轉發到 order-service
    └── /api/payments → 轉發到 payment-service
```

### API Gateway 的功能

| 功能 | 說明 |
|------|------|
| **路由** | 根據 URL 轉發到對應的微服務 |
| **負載平衡** | 將請求分散到多個服務實例 |
| **認證授權** | 統一在 Gateway 驗證 JWT Token |
| **速率限制** | 防止 API 被濫用 |
| **快取** | 快取常用的回應 |
| **聚合** | 合併多個服務的回應為一個 |
| **日誌** | 集中記錄所有 API 請求 |

---

## 📌 Ocelot 設定與路由

### 安裝與設定

```bash
dotnet new webapi -n ApiGateway
cd ApiGateway
dotnet add package Ocelot
```

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(""ocelot.json"", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();
await app.UseOcelot();
app.Run();
```

### ocelot.json 路由設定

```json
{
  ""Routes"": [
    {
      ""DownstreamPathTemplate"": ""/api/products/{everything}"",
      ""DownstreamScheme"": ""http"",
      ""DownstreamHostAndPorts"": [
        { ""Host"": ""product-service"", ""Port"": 5002 }
      ],
      ""UpstreamPathTemplate"": ""/api/products/{everything}"",
      ""UpstreamHttpMethod"": [ ""GET"", ""POST"", ""PUT"", ""DELETE"" ]
    },
    {
      ""DownstreamPathTemplate"": ""/api/orders/{everything}"",
      ""DownstreamScheme"": ""http"",
      ""DownstreamHostAndPorts"": [
        { ""Host"": ""order-service"", ""Port"": 5003 }
      ],
      ""UpstreamPathTemplate"": ""/api/orders/{everything}"",
      ""UpstreamHttpMethod"": [ ""GET"", ""POST"" ],
      ""AuthenticationOptions"": {
        ""AuthenticationProviderKey"": ""Bearer""
      }
    }
  ],
  ""GlobalConfiguration"": {
    ""BaseUrl"": ""http://localhost:5000""
  }
}
```

---

## 📌 YARP — 微軟官方反向代理

YARP（Yet Another Reverse Proxy）是微軟官方的高效能反向代理，比 Ocelot 更適合大規模生產環境。

### 安裝

```bash
dotnet add package Yarp.ReverseProxy
```

### 設定 YARP

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection(""ReverseProxy""));

var app = builder.Build();
app.MapReverseProxy();
app.Run();
```

### appsettings.json 設定

```json
{
  ""ReverseProxy"": {
    ""Routes"": {
      ""product-route"": {
        ""ClusterId"": ""product-cluster"",
        ""Match"": {
          ""Path"": ""/api/products/{**catch-all}""
        },
        ""Transforms"": [
          { ""PathPattern"": ""/api/products/{**catch-all}"" }
        ]
      },
      ""order-route"": {
        ""ClusterId"": ""order-cluster"",
        ""Match"": {
          ""Path"": ""/api/orders/{**catch-all}""
        }
      }
    },
    ""Clusters"": {
      ""product-cluster"": {
        ""LoadBalancingPolicy"": ""RoundRobin"",
        ""Destinations"": {
          ""product-1"": { ""Address"": ""http://product-service-1:5002"" },
          ""product-2"": { ""Address"": ""http://product-service-2:5002"" }
        }
      },
      ""order-cluster"": {
        ""Destinations"": {
          ""order-1"": { ""Address"": ""http://order-service:5003"" }
        }
      }
    }
  }
}
```

---

## 📌 Ocelot vs YARP 比較

| 功能 | Ocelot | YARP |
|------|--------|------|
| 維護者 | 社群 | 微軟官方 |
| 效能 | 中 | 高 |
| 設定方式 | JSON 檔案 | JSON / 程式碼 |
| 負載平衡 | 內建 | 內建（更多策略） |
| 熱更新設定 | 支援 | 支援 |
| 自訂中介軟體 | 有限 | 靈活 |
| 適用場景 | 中小型專案 | 大型生產環境 |

---

## 📌 負載平衡與速率限制

### YARP 負載平衡策略

```csharp
// 支援的負載平衡策略
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection(""ReverseProxy""));

// 可用的策略：
// - RoundRobin：輪流分配
// - Random：隨機
// - LeastRequests：最少請求
// - PowerOfTwoChoices：隨機選兩個，選負載低的
// - FirstAlphabetical：按字母順序（測試用）
```

### 速率限制

```csharp
// 使用 ASP.NET Core 內建的速率限制
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(""fixed"", opt =>
    {
        opt.PermitLimit = 100;           // 每個窗口 100 個請求
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
    });

    options.AddSlidingWindowLimiter(""sliding"", opt =>
    {
        opt.PermitLimit = 60;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.SegmentsPerWindow = 6;       // 每 10 秒一個段落
    });
});

app.UseRateLimiter();
```

---

## 📌 認證與授權在 Gateway 層

```csharp
// Program.cs — API Gateway
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = ""http://identity-service"";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(""AdminOnly"", policy =>
        policy.RequireRole(""admin""));
});

// 在 YARP 路由中套用授權
// appsettings.json 中設定 AuthorizationPolicy
```

---

## 📌 範例：聚合多個服務的回應

```csharp
// 自訂聚合端點：一次呼叫取得訂單 + 產品 + 用戶資訊
app.MapGet(""/api/aggregation/order-details/{orderId}"",
    async (Guid orderId, IHttpClientFactory factory) =>
{
    var orderClient = factory.CreateClient(""OrderService"");
    var productClient = factory.CreateClient(""ProductService"");
    var userClient = factory.CreateClient(""UserService"");

    // 平行呼叫三個服務
    var orderTask = orderClient.GetFromJsonAsync<OrderDto>(
        $""/api/orders/{orderId}"");
    var order = await orderTask;
    if (order is null) return Results.NotFound();

    var userTask = userClient.GetFromJsonAsync<UserDto>(
        $""/api/users/{order.CustomerId}"");
    var productTasks = order.Items.Select(item =>
        productClient.GetFromJsonAsync<ProductDto>(
            $""/api/products/{item.ProductId}""));

    await Task.WhenAll(userTask, Task.WhenAll(productTasks));

    return Results.Ok(new
    {
        Order = order,
        Customer = userTask.Result,
        Products = productTasks.Select(t => t.Result)
    });
});
```

> **下一章：** 我們將學習如何用 Docker 容器化微服務。
"
            },

            // ── Chapter 1105: 微服務容器化：Docker 與 Docker Compose ──
            new Chapter
            {
                Id = 1105,
                Title = "微服務容器化：Docker 與 Docker Compose",
                Slug = "micro-docker",
                Category = "microservices",
                Order = 115,
                Level = "intermediate",
                Icon = "🐳",
                IsPublished = true,
                Content = @"# 🐳 微服務容器化：Docker 與 Docker Compose

## 📌 每個微服務一個容器

容器化是微服務的最佳搭檔：每個服務打包成獨立的 Docker Image，確保開發、測試、生產環境一致。

```
微服務容器化架構：
┌─────────┐ ┌─────────┐ ┌─────────┐
│ Product │ │  Order  │ │ Payment │
│ Service │ │ Service │ │ Service │
│ :5002   │ │ :5003   │ │ :5004   │
└────┬────┘ └────┬────┘ └────┬────┘
     │           │           │
     └───────────┼───────────┘
                 │
          ┌──────┴──────┐
          │ API Gateway │
          │    :5000    │
          └─────────────┘
```

---

## 📌 Dockerfile 撰寫：多階段建置

```dockerfile
# ── ProductService/Dockerfile ──

# 階段 1：建置 (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 先複製 csproj 並還原套件（利用 Docker 快取層）
COPY [""ProductService.csproj"", "".""]
RUN dotnet restore

# 複製所有程式碼並建置
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# 階段 2：執行 (Runtime) — 用小型映像檔
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# 建立非 root 用戶（安全性）
RUN adduser --disabled-password --gecos """" appuser
USER appuser

COPY --from=build /app/publish .

# 健康檢查
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT [""dotnet"", ""ProductService.dll""]
```

### .dockerignore

```
# .dockerignore — 不要把不必要的檔案複製進容器
**/bin/
**/obj/
**/.vs/
**/.git
**/node_modules/
**/*.user
**/*.dbmdl
**/Dockerfile*
**/docker-compose*
```

---

## 📌 Docker Compose 編排多個服務

```yaml
# docker-compose.yml
version: '3.8'

services:
  # ── API Gateway ──
  api-gateway:
    build:
      context: ./ApiGateway
      dockerfile: Dockerfile
    ports:
      - ""5000:8080""
    depends_on:
      product-service:
        condition: service_healthy
      order-service:
        condition: service_healthy
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
    networks:
      - eshop-network

  # ── Product Service ──
  product-service:
    build:
      context: ./ProductService
      dockerfile: Dockerfile
    environment:
      - ConnectionStrings__DefaultConnection=Host=product-db;Database=ProductDb;Username=postgres;Password=postgres123
      - ASPNETCORE_ENVIRONMENT=Development
    depends_on:
      product-db:
        condition: service_healthy
    healthcheck:
      test: [""CMD"", ""curl"", ""-f"", ""http://localhost:8080/health""]
      interval: 10s
      timeout: 5s
      retries: 3
      start_period: 15s
    networks:
      - eshop-network

  # ── Order Service ──
  order-service:
    build:
      context: ./OrderService
      dockerfile: Dockerfile
    environment:
      - ConnectionStrings__DefaultConnection=Host=order-db;Database=OrderDb;Username=postgres;Password=postgres123
      - RabbitMq__Host=rabbitmq
      - ASPNETCORE_ENVIRONMENT=Development
    depends_on:
      order-db:
        condition: service_healthy
      rabbitmq:
        condition: service_healthy
    healthcheck:
      test: [""CMD"", ""curl"", ""-f"", ""http://localhost:8080/health""]
      interval: 10s
      timeout: 5s
      retries: 3
    networks:
      - eshop-network

  # ── Payment Service ──
  payment-service:
    build:
      context: ./PaymentService
      dockerfile: Dockerfile
    environment:
      - ConnectionStrings__DefaultConnection=Host=payment-db;Database=PaymentDb;Username=postgres;Password=postgres123
      - RabbitMq__Host=rabbitmq
    depends_on:
      - payment-db
      - rabbitmq
    networks:
      - eshop-network

  # ── 資料庫 ──
  product-db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: ProductDb
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres123
    volumes:
      - product-data:/var/lib/postgresql/data
    healthcheck:
      test: [""CMD-SHELL"", ""pg_isready -U postgres""]
      interval: 5s
      timeout: 3s
      retries: 5
    networks:
      - eshop-network

  order-db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: OrderDb
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres123
    volumes:
      - order-data:/var/lib/postgresql/data
    healthcheck:
      test: [""CMD-SHELL"", ""pg_isready -U postgres""]
      interval: 5s
      timeout: 3s
      retries: 5
    networks:
      - eshop-network

  payment-db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: PaymentDb
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres123
    volumes:
      - payment-data:/var/lib/postgresql/data
    networks:
      - eshop-network

  # ── 基礎設施 ──
  rabbitmq:
    image: rabbitmq:3-management-alpine
    ports:
      - ""15672:15672""  # 管理介面
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest
    healthcheck:
      test: [""CMD"", ""rabbitmq-diagnostics"", ""check_port_connectivity""]
      interval: 10s
      timeout: 5s
      retries: 5
    networks:
      - eshop-network

  redis:
    image: redis:7-alpine
    ports:
      - ""6379:6379""
    networks:
      - eshop-network

volumes:
  product-data:
  order-data:
  payment-data:

networks:
  eshop-network:
    driver: bridge
```

---

## 📌 常用 Docker Compose 指令

```bash
# 啟動所有服務（背景執行）
docker compose up -d

# 查看所有服務的狀態
docker compose ps

# 查看特定服務的日誌
docker compose logs -f product-service

# 只重建並重啟某個服務
docker compose up -d --build product-service

# 停止並移除所有容器
docker compose down

# 停止並移除所有容器 + 資料卷（清除資料庫）
docker compose down -v

# 擴展某個服務的實例數量
docker compose up -d --scale product-service=3
```

---

## 📌 環境變數管理

```yaml
# docker-compose.override.yml（開發環境覆蓋設定）
services:
  product-service:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - Logging__LogLevel__Default=Debug
    ports:
      - ""5002:8080""  # 開發時暴露埠口方便偵錯
```

```csharp
// Program.cs — 在程式碼中讀取環境變數
var connectionString = builder.Configuration
    .GetConnectionString(""DefaultConnection"")
    ?? throw new InvalidOperationException(""Missing connection string"");

var rabbitHost = builder.Configuration[""RabbitMq:Host""] ?? ""localhost"";
```

---

## 📌 服務間網路通訊

```yaml
# 在 docker-compose 中，服務名稱就是 DNS 名稱
# product-service 可以透過 http://order-service:8080 呼叫 order-service
# 不需要知道實際的 IP 地址

# 自訂網路讓服務隔離
networks:
  eshop-network:
    driver: bridge
  monitoring-network:
    driver: bridge
```

> **下一章：** 我們將學習微服務的韌性模式，用 Polly 處理網路故障和服務不可用的狀況。
"
            },

            // ── Chapter 1106: 微服務韌性模式：Polly 與容錯設計 ──
            new Chapter
            {
                Id = 1106,
                Title = "微服務韌性模式：Polly 與容錯設計",
                Slug = "micro-resilience",
                Category = "microservices",
                Order = 116,
                Level = "intermediate",
                Icon = "🛡️",
                IsPublished = true,
                Content = @"# 🛡️ 微服務韌性模式：Polly 與容錯設計

## 📌 為什麼微服務需要韌性？

在分散式系統中，**網路是不可靠的**。服務可能暫時不可用、回應緩慢或回傳錯誤。如果不處理這些情況，一個服務的故障會像骨牌效應般擴散到整個系統。

```
沒有韌性的微服務：
用戶 → Gateway → 訂單服務 → 庫存服務（掛了 💥）
                        ↓
                   訂單服務超時等待...
                        ↓
                   Gateway 超時等待...
                        ↓
                   用戶看到 500 錯誤 😡
```

```
有韌性的微服務：
用戶 → Gateway → 訂單服務 → 庫存服務（掛了 💥）
                        ↓
                   斷路器啟動：直接回傳降級回應
                        ↓
                   用戶看到：""庫存確認中，稍後通知""
```

---

## 📌 安裝 Polly

```bash
# .NET 8 推薦使用新版 Polly v8
dotnet add package Microsoft.Extensions.Http.Polly
dotnet add package Microsoft.Extensions.Http.Resilience
```

---

## 📌 重試 (Retry) 策略

暫時性故障（網路閃斷、服務重啟中）通常重試就能解決。

```csharp
// ── 基本重試 ──
builder.Services.AddHttpClient(""ProductService"")
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt =>
                TimeSpan.FromSeconds(Math.Pow(2, attempt)), // 指數退避：2s, 4s, 8s
            onRetry: (outcome, delay, attempt, context) =>
            {
                Console.WriteLine($""重試第 {attempt} 次，等待 {delay.TotalSeconds}s"");
            }));

// ── .NET 8 新版 Resilience Pipeline ──
builder.Services.AddHttpClient(""ProductService"")
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.MaxRetryAttempts = 3;
        options.Retry.Delay = TimeSpan.FromSeconds(1);
        options.Retry.BackoffType = DelayBackoffType.Exponential;
        options.Retry.UseJitter = true; // 加入抖動，避免多個客戶端同時重試
    });
```

### 何時該重試？何時不該？

```
✅ 適合重試的場景：
├── HTTP 408 Request Timeout
├── HTTP 429 Too Many Requests
├── HTTP 500/502/503/504 伺服器錯誤
├── 網路連線逾時
└── 資料庫暫時性錯誤

❌ 不適合重試的場景：
├── HTTP 400 Bad Request（請求有誤，重試也沒用）
├── HTTP 401/403 認證失敗
├── HTTP 404 Not Found
├── 業務邏輯錯誤（庫存不足等）
└── 非冪等操作（要特別小心！）
```

---

## 📌 斷路器 (Circuit Breaker) 模式

當某個服務持續失敗時，**斷路器會暫時切斷呼叫**，避免浪費資源。

```csharp
builder.Services.AddHttpClient(""InventoryService"")
    .AddTransientHttpErrorPolicy(policy =>
        policy.CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,  // 連續 5 次失敗後斷路
            durationOfBreak: TimeSpan.FromSeconds(30), // 斷路 30 秒
            onBreak: (outcome, duration) =>
                Console.WriteLine($""斷路器開啟！暫停 {duration.TotalSeconds}s""),
            onReset: () =>
                Console.WriteLine(""斷路器關閉，恢復正常""),
            onHalfOpen: () =>
                Console.WriteLine(""斷路器半開，嘗試一個請求..."")
        ));
```

### 斷路器的三種狀態

```
┌──────────────────────────────────────────────┐
│              斷路器狀態機                      │
│                                              │
│  ┌────────┐  連續失敗  ┌────────┐            │
│  │ Closed │ ────────→ │  Open  │            │
│  │ (正常)  │           │ (斷路)  │            │
│  └───┬────┘  ←──────  └───┬────┘            │
│      ↑       成功重試      │                  │
│      │                    │ 等待超時           │
│      │       ┌────────┐   │                  │
│      └────── │HalfOpen│ ←─┘                  │
│       失敗   │(半開放) │                      │
│       →Open  └────────┘                      │
└──────────────────────────────────────────────┘
```

---

## 📌 超時 (Timeout) 與逾時處理

```csharp
// 設定請求超時
builder.Services.AddHttpClient(""PaymentService"")
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(
        TimeSpan.FromSeconds(5),
        TimeoutStrategy.Optimistic,
        onTimeoutAsync: (context, timespan, task) =>
        {
            Console.WriteLine($""請求超時！等待了 {timespan.TotalSeconds}s"");
            return Task.CompletedTask;
        }));
```

---

## 📌 艙壁隔離 (Bulkhead) 模式

限制對某個服務的並行請求數，防止一個慢服務拖垮整個系統。

```csharp
// 最多允許 10 個並行請求呼叫庫存服務
// 超過的最多排隊 5 個，其餘直接拒絕
builder.Services.AddHttpClient(""InventoryService"")
    .AddTransientHttpErrorPolicy(policy =>
        policy.BulkheadAsync(
            maxParallelization: 10,
            maxQueuingActions: 5,
            onBulkheadRejectedAsync: (context) =>
            {
                Console.WriteLine(""艙壁隔離：請求被拒絕（並行數已滿）"");
                return Task.CompletedTask;
            }));
```

```
艙壁隔離的概念（來自船艦設計）：
┌─────────────────────────────────┐
│ 微服務系統                       │
│ ┌─────┐ ┌─────┐ ┌─────┐        │
│ │ 10  │ │ 20  │ │ 10  │ ← 最大並行數 │
│ │ 請求 │ │ 請求 │ │ 請求 │        │
│ │     │ │     │ │     │        │
│ │商品  │ │訂單  │ │庫存  │        │
│ └─────┘ └─────┘ └─────┘        │
│ 即使庫存服務很慢，也不會影響商品服務 │
└─────────────────────────────────┘
```

---

## 📌 完整範例：HttpClient + Polly 的韌性設定

```csharp
// Program.cs — 完整的韌性配置
builder.Services.AddHttpClient<InventoryServiceClient>(client =>
{
    client.BaseAddress = new Uri(""http://inventory-service"");
})
.AddStandardResilienceHandler(options =>
{
    // 重試策略
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromMilliseconds(500);
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.Retry.UseJitter = true;

    // 斷路器
    options.CircuitBreaker.FailureRatio = 0.5;  // 50% 失敗率觸發
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(10);
    options.CircuitBreaker.MinimumThroughput = 8;
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);

    // 總超時
    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(15);

    // 單次請求超時
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(5);
});

// ── 搭配降級回應 ──
public class InventoryServiceClient
{
    private readonly HttpClient _client;
    private readonly ILogger<InventoryServiceClient> _logger;

    public InventoryServiceClient(HttpClient client,
        ILogger<InventoryServiceClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<StockInfo> GetStockAsync(int productId)
    {
        try
        {
            return await _client.GetFromJsonAsync<StockInfo>(
                $""/api/inventory/{productId}"")
                ?? new StockInfo(productId, 0, false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, ""無法取得庫存資訊，回傳降級回應"");
            // 降級回應：假設有庫存，後續再非同步確認
            return new StockInfo(productId, -1, true);
        }
    }
}

public record StockInfo(int ProductId, int Quantity, bool IsDegraded);
```

> **下一章：** 我們將學習微服務中最棘手的問題 — 分散式資料管理與 Saga 模式。
"
            },

            // ── Chapter 1107: 微服務資料管理：Saga 與最終一致性 ──
            new Chapter
            {
                Id = 1107,
                Title = "微服務資料管理：Saga 與最終一致性",
                Slug = "micro-data",
                Category = "microservices",
                Order = 117,
                Level = "advanced",
                Icon = "🗄️",
                IsPublished = true,
                Content = @"# 🗄️ 微服務資料管理：Saga 與最終一致性

## 📌 分散式交易的挑戰

在單體架構中，一個資料庫交易就能保證資料一致性：

```csharp
// 單體架構：簡單的資料庫交易
using var transaction = await _db.Database.BeginTransactionAsync();
try
{
    // 1. 建立訂單
    _db.Orders.Add(order);

    // 2. 扣減庫存
    var stock = await _db.Stocks.FindAsync(productId);
    stock.Quantity -= quantity;

    // 3. 建立付款記錄
    _db.Payments.Add(payment);

    await _db.SaveChangesAsync();
    await transaction.CommitAsync(); // 全部成功，一次提交
}
catch
{
    await transaction.RollbackAsync(); // 任何失敗，全部回滾
}
```

但在微服務中，**每個服務有自己的資料庫**，無法使用單一交易！

---

## 📌 兩階段提交 (2PC) 的問題

```
兩階段提交：
1. 準備階段：協調者問所有參與者 ""你準備好了嗎？""
2. 提交階段：所有人都說 OK → 提交；任何人說不行 → 全部回滾

問題：
├── 同步阻塞：所有參與者必須等待最慢的那個
├── 單點故障：協調者掛了，所有參與者都卡住
├── 效能瓶頸：鎖定資源直到交易完成
└── 不適合微服務的去中心化理念
```

---

## 📌 Saga 模式

Saga 是一種**將分散式交易拆分為一系列本地交易**的模式，每個本地交易都有對應的**補償操作**。

### Choreography（編舞）模式

每個服務監聽事件並自主決定下一步。

```csharp
// ── 訂單建立的 Saga（Choreography） ──

// 步驟 1：Order Service 建立訂單
public class OrderService
{
    public async Task CreateOrderAsync(CreateOrderCommand cmd)
    {
        var order = new Order { Status = OrderStatus.Pending };
        await _repo.AddAsync(order);
        await _publisher.Publish(new OrderCreatedEvent(order.Id, cmd.Items));
    }
}

// 步驟 2：Inventory Service 監聽並扣減庫存
public class OrderCreatedHandler : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> ctx)
    {
        try
        {
            foreach (var item in ctx.Message.Items)
                await _inventory.ReserveStockAsync(item.ProductId, item.Quantity);

            await ctx.Publish(new StockReservedEvent(ctx.Message.OrderId));
        }
        catch (InsufficientStockException)
        {
            await ctx.Publish(new StockReservationFailedEvent(ctx.Message.OrderId));
        }
    }
}

// 步驟 3：Payment Service 監聽並處理付款
public class StockReservedHandler : IConsumer<StockReservedEvent>
{
    public async Task Consume(ConsumeContext<StockReservedEvent> ctx)
    {
        try
        {
            await _payment.ChargeAsync(ctx.Message.OrderId);
            await ctx.Publish(new PaymentCompletedEvent(ctx.Message.OrderId));
        }
        catch (PaymentFailedException)
        {
            await ctx.Publish(new PaymentFailedEvent(ctx.Message.OrderId));
        }
    }
}

// 補償：庫存服務監聽付款失敗，釋放庫存
public class PaymentFailedHandler : IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> ctx)
    {
        await _inventory.ReleaseStockAsync(ctx.Message.OrderId);
        await ctx.Publish(new StockReleasedEvent(ctx.Message.OrderId));
    }
}
```

### Orchestration（指揮）模式

由一個中央的 Saga 協調者來控制流程。

```csharp
// ── Saga 協調者 ──
public class OrderSagaOrchestrator : ISaga<OrderSagaState>
{
    public async Task HandleAsync(OrderCreatedEvent evt, ISagaContext context)
    {
        // 步驟 1：要求庫存服務預留庫存
        await context.Send(new ReserveStockCommand(evt.OrderId, evt.Items));
    }

    public async Task HandleAsync(StockReservedEvent evt, ISagaContext context)
    {
        // 步驟 2：要求付款服務扣款
        await context.Send(new ProcessPaymentCommand(evt.OrderId));
    }

    public async Task HandleAsync(PaymentCompletedEvent evt, ISagaContext context)
    {
        // 步驟 3：訂單確認完成
        await context.Send(new ConfirmOrderCommand(evt.OrderId));
    }

    // ── 補償流程 ──
    public async Task HandleAsync(PaymentFailedEvent evt, ISagaContext context)
    {
        // 補償：釋放庫存 → 取消訂單
        await context.Send(new ReleaseStockCommand(evt.OrderId));
        await context.Send(new CancelOrderCommand(evt.OrderId, ""付款失敗""));
    }

    public async Task HandleAsync(StockReservationFailedEvent evt, ISagaContext context)
    {
        // 補償：直接取消訂單
        await context.Send(new CancelOrderCommand(evt.OrderId, ""庫存不足""));
    }
}
```

---

## 📌 最終一致性 (Eventual Consistency)

微服務架構接受：資料**不會立即一致**，但**最終會一致**。

```
強一致性（單體）：下單 → 扣庫存 → 扣款 → 全部完成（同步）
最終一致性（微服務）：
  t=0s  訂單建立（Pending）
  t=1s  庫存扣減（Processing）
  t=3s  付款完成（Confirmed）← 最終一致
```

---

## 📌 Outbox Pattern — 可靠事件發布

確保資料庫操作和事件發布的原子性。

```csharp
// ── 問題：資料庫寫入成功但事件發布失敗 ──
await _db.Orders.AddAsync(order);
await _db.SaveChangesAsync();        // ✅ 成功
await _publisher.Publish(event);      // ❌ RabbitMQ 掛了！事件遺失！

// ── 解決方案：Outbox Pattern ──
public class OutboxMessage
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

// 在同一個資料庫交易中寫入訂單和 Outbox 訊息
using var transaction = await _db.Database.BeginTransactionAsync();

await _db.Orders.AddAsync(order);
await _db.OutboxMessages.AddAsync(new OutboxMessage
{
    Id = Guid.NewGuid(),
    EventType = nameof(OrderCreatedEvent),
    Payload = JsonSerializer.Serialize(new OrderCreatedEvent(order.Id)),
    CreatedAt = DateTime.UtcNow
});

await _db.SaveChangesAsync();
await transaction.CommitAsync(); // 兩者在同一個交易中，保證原子性

// ── 背景服務定期掃描 Outbox 並發布事件 ──
public class OutboxProcessor : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var messages = await _db.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .OrderBy(m => m.CreatedAt)
                .Take(20)
                .ToListAsync(ct);

            foreach (var msg in messages)
            {
                await _publisher.PublishRawAsync(msg.EventType, msg.Payload);
                msg.ProcessedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync(ct);
            await Task.Delay(TimeSpan.FromSeconds(5), ct);
        }
    }
}
```

---

## 📌 CQRS（命令查詢職責分離）

```csharp
// ── 寫入端（Command） ──
public class CreateOrderCommandHandler
{
    private readonly OrderDbContext _writeDb;

    public async Task HandleAsync(CreateOrderCommand cmd)
    {
        var order = Order.Create(cmd.CustomerId, cmd.Items);
        _writeDb.Orders.Add(order);
        await _writeDb.SaveChangesAsync();
        // 發布事件給讀取端
    }
}

// ── 讀取端（Query） ──
public class OrderQueryService
{
    private readonly IReadOnlyRepository _readDb; // 可以是不同的資料庫

    public async Task<OrderDetailDto> GetOrderDetailAsync(Guid orderId)
    {
        // 讀取端的資料模型是為查詢最佳化的（反正規化）
        return await _readDb.GetAsync<OrderDetailDto>(orderId);
    }
}
```

---

## 📌 Event Sourcing 事件溯源基礎

```csharp
// 不儲存當前狀態，而是儲存所有發生過的事件
public class OrderEventStore
{
    public async Task AppendAsync(Guid orderId, IDomainEvent @event)
    {
        await _store.AppendToStreamAsync($""order-{orderId}"", new EventData
        {
            EventType = @event.GetType().Name,
            Data = JsonSerializer.Serialize(@event),
            Timestamp = DateTime.UtcNow
        });
    }

    public async Task<Order> LoadAsync(Guid orderId)
    {
        var events = await _store.ReadStreamAsync($""order-{orderId}"");
        var order = new Order();
        foreach (var evt in events)
        {
            order.Apply(evt); // 重播事件來重建狀態
        }
        return order;
    }
}
```

> **下一章：** 我們將學習微服務的可觀測性 — 日誌、追蹤與指標的完整方案。
"
            },

            // ── Chapter 1108: 微服務可觀測性：日誌、追蹤與指標 ──
            new Chapter
            {
                Id = 1108,
                Title = "微服務可觀測性：日誌、追蹤與指標",
                Slug = "micro-observability",
                Category = "microservices",
                Order = 118,
                Level = "advanced",
                Icon = "🔍",
                IsPublished = true,
                Content = @"# 🔍 微服務可觀測性：日誌、追蹤與指標

## 📌 可觀測性三大支柱

```
可觀測性 (Observability)
├── 📋 Logs（日誌）：記錄發生了什麼事
│   └── ""訂單 #123 建立成功""
├── 🔗 Traces（追蹤）：追蹤請求在服務間的流動
│   └── Gateway → OrderService → InventoryService → PaymentService
└── 📊 Metrics（指標）：量化系統的狀態
    └── 請求數/秒、延遲 P99、錯誤率
```

| 支柱 | 回答的問題 | 工具 |
|------|-----------|------|
| **Logs** | 發生了什麼？為什麼出錯？ | Serilog + Seq / ELK |
| **Traces** | 請求經過了哪些服務？哪裡慢？ | OpenTelemetry + Jaeger |
| **Metrics** | 系統整體表現如何？需要擴展嗎？ | Prometheus + Grafana |

---

## 📌 集中式日誌：Serilog

### 為什麼需要集中式日誌？

微服務的日誌分散在多個容器中，需要集中收集才能有效除錯。

```csharp
// ── 安裝 Serilog ──
// dotnet add package Serilog.AspNetCore
// dotnet add package Serilog.Sinks.Seq
// dotnet add package Serilog.Enrichers.Environment
// dotnet add package Serilog.Enrichers.Thread

// Program.cs
builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithProperty(""ServiceName"", ""OrderService"")
        .WriteTo.Console(outputTemplate:
            ""[{Timestamp:HH:mm:ss} {Level:u3}] {ServiceName} | {Message:lj}{NewLine}{Exception}"")
        .WriteTo.Seq(""http://seq:5341""); // 集中式日誌伺服器
});

// 使用結構化日誌
public class OrderService
{
    private readonly ILogger<OrderService> _logger;

    public async Task<Order> CreateOrderAsync(CreateOrderCommand cmd)
    {
        _logger.LogInformation(""開始建立訂單 CustomerId={CustomerId}, Items={ItemCount}"",
            cmd.CustomerId, cmd.Items.Count);

        try
        {
            var order = new Order { /* ... */ };
            await _repo.AddAsync(order);

            _logger.LogInformation(""訂單建立成功 OrderId={OrderId}, Total={Total}"",
                order.Id, order.TotalAmount);

            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ""訂單建立失敗 CustomerId={CustomerId}"",
                cmd.CustomerId);
            throw;
        }
    }
}
```

---

## 📌 關聯 ID (Correlation ID) 追蹤請求鏈

```csharp
// ── 中介軟體：為每個請求加上 Correlation ID ──
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationHeader = ""X-Correlation-ID"";

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // 從上游取得 Correlation ID，沒有就建立新的
        if (!context.Request.Headers.TryGetValue(
            CorrelationHeader, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        context.Items[CorrelationHeader] = correlationId.ToString();

        // 加入回應標頭
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationHeader] = correlationId;
            return Task.CompletedTask;
        });

        // 加入日誌上下文
        using (LogContext.PushProperty(""CorrelationId"", correlationId.ToString()))
        {
            await _next(context);
        }
    }
}

// 呼叫下游服務時傳遞 Correlation ID
public class CorrelationIdDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdDelegatingHandler(
        IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct)
    {
        var correlationId = _httpContextAccessor.HttpContext?
            .Items[""X-Correlation-ID""]?.ToString();

        if (!string.IsNullOrEmpty(correlationId))
            request.Headers.Add(""X-Correlation-ID"", correlationId);

        return await base.SendAsync(request, ct);
    }
}
```

---

## 📌 分散式追蹤：OpenTelemetry

```csharp
// ── 安裝 ──
// dotnet add package OpenTelemetry.Extensions.Hosting
// dotnet add package OpenTelemetry.Instrumentation.AspNetCore
// dotnet add package OpenTelemetry.Instrumentation.Http
// dotnet add package OpenTelemetry.Instrumentation.EntityFrameworkCore
// dotnet add package OpenTelemetry.Exporter.OtlpProtobuf

// Program.cs
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource.AddService(""OrderService""))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddSource(""OrderService.Activities"")
            .AddOtlpExporter(opt =>
            {
                opt.Endpoint = new Uri(""http://jaeger:4317"");
            });
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter();
    });

// 使用 Prometheus 端點
app.MapPrometheusScrapingEndpoint();
```

### 自訂追蹤 Span

```csharp
public class OrderService
{
    private static readonly ActivitySource _activitySource =
        new(""OrderService.Activities"");

    public async Task<Order> CreateOrderAsync(CreateOrderCommand cmd)
    {
        using var activity = _activitySource.StartActivity(""CreateOrder"");
        activity?.SetTag(""order.customer_id"", cmd.CustomerId.ToString());
        activity?.SetTag(""order.item_count"", cmd.Items.Count);

        // 驗證步驟
        using (var validateActivity = _activitySource.StartActivity(""ValidateOrder""))
        {
            await ValidateAsync(cmd);
            validateActivity?.SetTag(""validation.result"", ""success"");
        }

        // 儲存步驟
        using (var saveActivity = _activitySource.StartActivity(""SaveOrder""))
        {
            var order = new Order { /* ... */ };
            await _repo.AddAsync(order);
            activity?.SetTag(""order.id"", order.Id.ToString());
            return order;
        }
    }
}
```

---

## 📌 健康指標：Prometheus + Grafana

```csharp
// 自訂業務指標
public class OrderMetrics
{
    private readonly Counter<long> _ordersCreated;
    private readonly Histogram<double> _orderProcessingDuration;
    private readonly UpDownCounter<long> _activeOrders;

    public OrderMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(""OrderService"");

        _ordersCreated = meter.CreateCounter<long>(
            ""orders.created"",
            description: ""建立的訂單數量"");

        _orderProcessingDuration = meter.CreateHistogram<double>(
            ""orders.processing.duration"",
            unit: ""ms"",
            description: ""訂單處理耗時"");

        _activeOrders = meter.CreateUpDownCounter<long>(
            ""orders.active"",
            description: ""進行中的訂單數量"");
    }

    public void OrderCreated(string category)
    {
        _ordersCreated.Add(1, new KeyValuePair<string, object?>(""category"", category));
        _activeOrders.Add(1);
    }

    public void RecordDuration(double milliseconds)
        => _orderProcessingDuration.Record(milliseconds);

    public void OrderCompleted()
        => _activeOrders.Add(-1);
}
```

---

## 📌 Docker Compose：完整可觀測性堆疊

```yaml
# 加入可觀測性服務
services:
  # 集中日誌
  seq:
    image: datalust/seq:latest
    environment:
      - ACCEPT_EULA=Y
    ports:
      - ""5341:5341""  # 接收日誌
      - ""8081:80""    # Web UI

  # 分散式追蹤
  jaeger:
    image: jaegertracing/all-in-one:latest
    ports:
      - ""16686:16686""  # Web UI
      - ""4317:4317""    # OTLP gRPC

  # 指標收集
  prometheus:
    image: prom/prometheus:latest
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml
    ports:
      - ""9090:9090""

  # 指標儀表板
  grafana:
    image: grafana/grafana:latest
    ports:
      - ""3000:3000""
    environment:
      - GF_SECURITY_ADMIN_PASSWORD=admin
```

> **下一章：** 我們將學習如何將微服務部署到 Kubernetes，建立完整的 CI/CD Pipeline。
"
            },

            // ── Chapter 1109: 微服務部署：Kubernetes 與 CI/CD ──
            new Chapter
            {
                Id = 1109,
                Title = "微服務部署：Kubernetes 與 CI/CD",
                Slug = "micro-deploy",
                Category = "microservices",
                Order = 119,
                Level = "advanced",
                Icon = "🚀",
                IsPublished = true,
                Content = @"# 🚀 微服務部署：Kubernetes 與 CI/CD

## 📌 Kubernetes 基礎概念

```
Kubernetes (K8s) 核心元件：
├── Pod：最小部署單位，包含一個或多個容器
├── Service：穩定的網路端點，負載平衡到 Pod
├── Deployment：管理 Pod 的副本數量和更新策略
├── ConfigMap / Secret：外部化設定
├── Ingress：外部流量的入口（類似 API Gateway）
└── Namespace：邏輯隔離（dev、staging、production）
```

```
K8s 架構圖：
┌─────────────────────────────────────────┐
│ Kubernetes Cluster                       │
│                                         │
│  ┌─────────────┐  ┌─────────────┐       │
│  │  Ingress    │  │  Ingress    │       │
│  │  (外部入口)  │  │  Controller │       │
│  └──────┬──────┘  └─────────────┘       │
│         │                               │
│  ┌──────┴──────┐                        │
│  │   Service    │ ← 負載平衡             │
│  └──────┬──────┘                        │
│         │                               │
│  ┌──────┴───────┬──────────┐            │
│  │   Pod 1     │  Pod 2   │  Pod 3     │
│  │ OrderSvc    │ OrderSvc │ OrderSvc   │
│  └─────────────┴──────────┴────────┘    │
└─────────────────────────────────────────┘
```

---

## 📌 將 .NET 微服務部署到 K8s

### Deployment 設定

```yaml
# k8s/product-service/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: product-service
  namespace: eshop
  labels:
    app: product-service
spec:
  replicas: 3
  selector:
    matchLabels:
      app: product-service
  template:
    metadata:
      labels:
        app: product-service
    spec:
      containers:
        - name: product-service
          image: myregistry.azurecr.io/product-service:v1.0.0
          ports:
            - containerPort: 8080
          env:
            - name: ASPNETCORE_ENVIRONMENT
              value: ""Production""
            - name: ConnectionStrings__DefaultConnection
              valueFrom:
                secretKeyRef:
                  name: product-db-secret
                  key: connection-string
          resources:
            requests:
              cpu: ""100m""
              memory: ""128Mi""
            limits:
              cpu: ""500m""
              memory: ""512Mi""
          livenessProbe:
            httpGet:
              path: /health/live
              port: 8080
            initialDelaySeconds: 15
            periodSeconds: 10
          readinessProbe:
            httpGet:
              path: /health/ready
              port: 8080
            initialDelaySeconds: 5
            periodSeconds: 5
          startupProbe:
            httpGet:
              path: /health/live
              port: 8080
            failureThreshold: 30
            periodSeconds: 10
```

### Service 設定

```yaml
# k8s/product-service/service.yaml
apiVersion: v1
kind: Service
metadata:
  name: product-service
  namespace: eshop
spec:
  selector:
    app: product-service
  ports:
    - port: 80
      targetPort: 8080
  type: ClusterIP
```

### Ingress 設定

```yaml
# k8s/ingress.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: eshop-ingress
  namespace: eshop
  annotations:
    nginx.ingress.kubernetes.io/rewrite-target: /$2
spec:
  ingressClassName: nginx
  rules:
    - host: api.eshop.example.com
      http:
        paths:
          - path: /products(/|$)(.*)
            pathType: ImplementationSpecific
            backend:
              service:
                name: product-service
                port:
                  number: 80
          - path: /orders(/|$)(.*)
            pathType: ImplementationSpecific
            backend:
              service:
                name: order-service
                port:
                  number: 80
```

---

## 📌 Helm Charts 管理

Helm 是 Kubernetes 的套件管理工具，用模板化的方式管理 K8s 資源。

```yaml
# helm/product-service/values.yaml
replicaCount: 3

image:
  repository: myregistry.azurecr.io/product-service
  tag: ""v1.0.0""
  pullPolicy: IfNotPresent

service:
  type: ClusterIP
  port: 80

resources:
  requests:
    cpu: 100m
    memory: 128Mi
  limits:
    cpu: 500m
    memory: 512Mi

autoscaling:
  enabled: true
  minReplicas: 2
  maxReplicas: 10
  targetCPUUtilizationPercentage: 70
```

```yaml
# helm/product-service/templates/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: {{ include ""product-service.fullname"" . }}
spec:
  replicas: {{ .Values.replicaCount }}
  selector:
    matchLabels:
      app: {{ include ""product-service.name"" . }}
  template:
    spec:
      containers:
        - name: {{ .Chart.Name }}
          image: ""{{ .Values.image.repository }}:{{ .Values.image.tag }}""
          resources:
            {{- toYaml .Values.resources | nindent 12 }}
```

```bash
# Helm 常用指令
helm install product-service ./helm/product-service -n eshop
helm upgrade product-service ./helm/product-service -n eshop
helm rollback product-service 1 -n eshop  # 回滾到版本 1
helm list -n eshop                         # 列出所有部署
```

---

## 📌 CI/CD Pipeline（GitHub Actions）

```yaml
# .github/workflows/deploy-product-service.yml
name: Deploy Product Service

on:
  push:
    branches: [main]
    paths:
      - 'src/ProductService/**'

env:
  REGISTRY: myregistry.azurecr.io
  IMAGE_NAME: product-service

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'

      - name: Run Tests
        run: |
          cd src/ProductService
          dotnet test --configuration Release

      - name: Build Docker Image
        run: |
          docker build -t $REGISTRY/$IMAGE_NAME:${{ github.sha }} \
            -t $REGISTRY/$IMAGE_NAME:latest \
            ./src/ProductService

      - name: Login to Container Registry
        uses: docker/login-action@v3
        with:
          registry: ${{ env.REGISTRY }}
          username: ${{ secrets.ACR_USERNAME }}
          password: ${{ secrets.ACR_PASSWORD }}

      - name: Push Docker Image
        run: |
          docker push $REGISTRY/$IMAGE_NAME:${{ github.sha }}
          docker push $REGISTRY/$IMAGE_NAME:latest

      - name: Deploy to Kubernetes
        uses: azure/k8s-deploy@v5
        with:
          namespace: eshop
          manifests: k8s/product-service/
          images: |
            ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}:${{ github.sha }}
```

---

## 📌 藍綠部署 / 金絲雀發布

### 金絲雀發布（Canary Deployment）

```yaml
# 先部署少量新版本 Pod 測試
apiVersion: apps/v1
kind: Deployment
metadata:
  name: product-service-canary
spec:
  replicas: 1  # 只部署 1 個新版本 Pod
  selector:
    matchLabels:
      app: product-service
      version: canary
  template:
    metadata:
      labels:
        app: product-service
        version: canary
    spec:
      containers:
        - name: product-service
          image: myregistry.azurecr.io/product-service:v2.0.0-rc1
```

```csharp
// 在 .NET 中配合 Feature Flags 控制流量
builder.Services.AddFeatureManagement()
    .AddFeatureFilter<PercentageFilter>();

// appsettings.json
// ""FeatureManagement"": {
//   ""NewCheckoutFlow"": {
//     ""EnabledFor"": [{
//       ""Name"": ""Percentage"",
//       ""Parameters"": { ""Value"": 10 }  // 10% 流量使用新版
//     }]
//   }
// }
```

---

## 📌 Service Mesh 概念

### Dapr（分散式應用執行環境）

```csharp
// 使用 Dapr 簡化微服務開發
// 不需要直接依賴 RabbitMQ、Redis 等具體技術

// 透過 Dapr 呼叫其他服務
app.MapGet(""/api/orders/{id}"", async (int id, DaprClient dapr) =>
{
    // Dapr 自動處理服務發現和負載平衡
    var product = await dapr.InvokeMethodAsync<ProductDto>(
        HttpMethod.Get,
        ""product-service"",  // 服務名稱，不需要知道 IP/Port
        $""api/products/{id}"");

    return Results.Ok(product);
});

// 透過 Dapr 發布事件
app.MapPost(""/api/orders"", async (CreateOrderDto dto, DaprClient dapr) =>
{
    var order = CreateOrder(dto);

    // Dapr 抽象了 Message Broker（RabbitMQ、Kafka、Azure Service Bus...）
    await dapr.PublishEventAsync(
        ""pubsub"",           // Pub/Sub 組件名稱
        ""order-created"",    // Topic 名稱
        new OrderCreatedEvent(order.Id, order.Items));

    return Results.Created($""/api/orders/{order.Id}"", order);
});
```

---

## 📌 從程式碼到部署的全流程總結

```
完整的微服務開發到部署流程：

1. 設計階段
   └── DDD 分析 → 定義 Bounded Context → 劃分服務

2. 開發階段
   ├── 建立 ASP.NET Core API
   ├── 實作業務邏輯
   ├── 撰寫單元測試與整合測試
   └── 設定韌性策略（Polly）

3. 容器化
   ├── 撰寫 Dockerfile
   ├── 建立 docker-compose.yml
   └── 本地測試所有服務

4. 可觀測性
   ├── 設定 Serilog + Seq
   ├── 設定 OpenTelemetry + Jaeger
   └── 設定 Prometheus + Grafana

5. 部署
   ├── 建立 K8s manifests / Helm Charts
   ├── 設定 CI/CD Pipeline
   ├── 金絲雀發布
   └── 監控與告警

Done! 🎉 你已經完成了微服務架構的完整學習路線！
```

> **恭喜完成！** 你已經學會了從微服務設計到 Kubernetes 部署的所有核心知識。建議從小型專案開始實踐，逐步累積經驗。
"
            },
        };
    }
}
