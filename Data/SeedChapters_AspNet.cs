using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_AspNet
{
    public static List<Chapter> GetChapters()
    {
        var chapters = new List<Chapter>
        {
            // ── Chapter 12: 路由系統 Routing ──────────────────────────
            new() { Id=12, Category="aspnet", Order=3, Level="beginner", Icon="🛤️", Title="路由系統 Routing", Slug="aspnet-routing", IsPublished=true, Content=@"
# 路由系統 Routing

## 路由是什麼？

路由就像是**郵差送信**——用戶發出請求（寄信），路由系統根據 URL（地址）找到對應的 Controller Action（收件人）。

```
用戶請求 GET /Products/Details/5
        ↓
路由系統比對 {controller}/{action}/{id?}
        ↓
找到 ProductsController.Details(5)
        ↓
回傳結果
```

---

## 傳統路由 Conventional Routing

在 `Program.cs` 中設定路由模板：

```csharp
// Program.cs - 設定傳統路由
app.MapControllerRoute(
    name: ""default"",                          // 路由名稱
    pattern: ""{controller=Home}/{action=Index}/{id?}"" // 路由模板
);
// controller=Home → 預設控制器為 Home
// action=Index   → 預設動作為 Index
// id?            → id 是可選參數
```

### 路由比對範例

| URL | Controller | Action | id |
|-----|-----------|--------|-----|
| `/` | Home | Index | null |
| `/Products` | Products | Index | null |
| `/Products/Details` | Products | Details | null |
| `/Products/Details/5` | Products | Details | 5 |

---

## 屬性路由 Attribute Routing

直接在 Controller 或 Action 上標註路由：

```csharp
// 使用屬性路由的控制器
[Route(""api/[controller]"")]  // 基底路由，[controller] 會自動替換為類別名稱
public class ProductsController : Controller
{
    [Route("""")]              // 對應 GET /api/Products
    [Route(""list"")]          // 也對應 GET /api/Products/list
    public IActionResult Index()
    {
        return View();       // 回傳視圖
    }

    [Route(""{id:int}"")]      // 對應 GET /api/Products/5，id 必須是整數
    public IActionResult Details(int id)
    {
        return Content($""商品 ID：{id}""); // 回傳文字內容
    }
}
```

---

## 路由參數與限制條件

```csharp
// 路由限制條件範例
public class CatalogController : Controller
{
    // {id:int} → id 必須是整數
    [HttpGet(""catalog/{id:int}"")]
    public IActionResult ById(int id)
    {
        return Content($""用 ID 查詢：{id}""); // 整數 ID 查詢
    }

    // {name:alpha} → name 只能是英文字母
    [HttpGet(""catalog/{name:alpha}"")]
    public IActionResult ByName(string name)
    {
        return Content($""用名稱查詢：{name}""); // 名稱查詢
    }

    // {slug:regex(^[a-z0-9-]+$)} → 自訂正規表示式
    [HttpGet(""catalog/slug/{slug:regex(^[[a-z0-9-]]+$)}"")]
    public IActionResult BySlug(string slug)
    {
        return Content($""用 Slug 查詢：{slug}""); // Slug 查詢
    }

    // 可選參數用 ? 表示
    [HttpGet(""catalog/page/{page:int?}"")]
    public IActionResult List(int page = 1)
    {
        return Content($""第 {page} 頁""); // 預設為第 1 頁
    }
}
```

### 常見路由限制條件

| 限制條件 | 說明 | 範例 |
|---------|------|------|
| `{id:int}` | 整數 | `123` |
| `{name:alpha}` | 英文字母 | `hello` |
| `{price:decimal}` | 十進位數 | `9.99` |
| `{flag:bool}` | 布林值 | `true` |
| `{id:min(1)}` | 最小值 1 | `1`, `100` |
| `{name:maxlength(20)}` | 最大長度 20 | `short` |

---

## 多重路由模式

```csharp
// Program.cs - 設定多組路由
// 第一組：管理後台路由
app.MapControllerRoute(
    name: ""admin"",                             // 路由名稱
    pattern: ""admin/{controller=Dashboard}/{action=Index}/{id?}"" // 後台路由
);

// 第二組：預設路由
app.MapControllerRoute(
    name: ""default"",                           // 路由名稱
    pattern: ""{controller=Home}/{action=Index}/{id?}"" // 預設路由
);
```

---

## Area 路由

Area（區域）用來將大型專案分組：

```csharp
// Areas/Admin/Controllers/DashboardController.cs
[Area(""Admin"")]                      // 標記屬於 Admin 區域
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();               // 回傳 Areas/Admin/Views/Dashboard/Index.cshtml
    }
}
```

```csharp
// Program.cs - 設定 Area 路由
app.MapControllerRoute(
    name: ""areas"",                              // 路由名稱
    pattern: ""{area:exists}/{controller=Home}/{action=Index}/{id?}"" // Area 路由模板
);
// area:exists → 確認 Area 存在才比對
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：路由模板重複衝突

```csharp
// ❌ 兩個 Action 路由一模一樣，系統不知道要用哪個
[HttpGet(""products/{id}"")]
public IActionResult GetById(int id) => Content(""By ID"");

[HttpGet(""products/{name}"")]
public IActionResult GetByName(string name) => Content(""By Name"");
```

```csharp
// ✅ 用限制條件區分
[HttpGet(""products/{id:int}"")]           // id 必須是整數
public IActionResult GetById(int id) => Content(""By ID"");

[HttpGet(""products/{name:alpha}"")]       // name 必須是字母
public IActionResult GetByName(string name) => Content(""By Name"");
```

> **為什麼？** 沒有限制條件，`products/5` 同時符合兩個路由，會造成 `AmbiguousMatchException`。加上 `:int` 和 `:alpha` 就能明確區分。

### ❌ 錯誤 2：忘記在 Area Controller 加 [Area] 屬性

```csharp
// ❌ 少了 [Area] 屬性，路由找不到
public class AdminDashboardController : Controller
{
    public IActionResult Index() => View();
}
```

```csharp
// ✅ 加上 [Area(""Admin"")] 屬性
[Area(""Admin"")]                          // 標記屬於 Admin 區域
public class AdminDashboardController : Controller
{
    public IActionResult Index() => View(); // 正確對應 Area 路由
}
```

> **為什麼？** Area 路由需要 `[Area]` 屬性來比對 `{area:exists}`，沒標記就無法匹配路由。

### ❌ 錯誤 3：路由順序錯誤

```csharp
// ❌ 萬用路由放在前面，後面的特定路由永遠不會被匹配
app.MapControllerRoute(""catchall"", ""{*url}"", new { controller=""Home"", action=""NotFound"" });
app.MapControllerRoute(""default"", ""{controller=Home}/{action=Index}/{id?}"");
```

```csharp
// ✅ 特定路由放前面，萬用路由放最後
app.MapControllerRoute(""default"", ""{controller=Home}/{action=Index}/{id?}"");
app.MapControllerRoute(""catchall"", ""{*url}"", new { controller=""Home"", action=""NotFound"" });
```

> **為什麼？** 路由是依序比對的，萬用路由 `{*url}` 會匹配所有 URL，放在前面就會把所有請求攔截掉。
" },

            // ── Chapter 13: Views 與 Razor 語法 ──────────────────────
            new() { Id=13, Category="aspnet", Order=4, Level="beginner", Icon="🎨", Title="Views 與 Razor 語法", Slug="aspnet-views-razor", IsPublished=true, Content=@"
# Views 與 Razor 語法

## Razor 是什麼？

Razor 是 ASP.NET Core 的**模板引擎**，讓你在 HTML 中嵌入 C# 程式碼。想像它像是一台**翻譯機**——把混合了 C# 和 HTML 的 `.cshtml` 檔案翻譯成純 HTML 給瀏覽器看。

```
.cshtml 檔案（C# + HTML）
        ↓ Razor Engine 編譯
純 HTML 輸出
        ↓
瀏覽器顯示
```

---

## 基本 Razor 語法

```html
<!-- Views/Home/Index.cshtml -->
@model List<string>  <!-- 宣告此 View 的 Model 型別 -->

@{
    // 程式碼區塊（C# 程式碼）
    var title = ""我的網站"";          // 宣告變數
    var count = Model.Count;        // 取得 Model 的項目數
}

<h1>@title</h1>                     <!-- 輸出變數值 -->
<p>共有 @count 筆資料</p>           <!-- 輸出變數值 -->
<p>現在時間：@DateTime.Now</p>      <!-- 直接呼叫 C# -->

<!-- 條件判斷 -->
@if (count > 0)
{
    <ul>
        <!-- 迴圈 -->
        @foreach (var item in Model)
        {
            <li>@item</li>          <!-- 輸出每個項目 -->
        }
    </ul>
}
else
{
    <p>目前沒有資料。</p>            <!-- 沒有資料時顯示 -->
}
```

---

## ViewData、ViewBag、TempData vs 強型別 Model

### 傳遞資料的四種方式

```csharp
// Controller 中設定資料
public IActionResult Index()
{
    // 方式 1: ViewData（字典，需要轉型）
    ViewData[""Title""] = ""首頁"";                // 存入 ViewData

    // 方式 2: ViewBag（動態，不需轉型但無智慧提示）
    ViewBag.Message = ""歡迎光臨"";              // 存入 ViewBag

    // 方式 3: TempData（跨 Request，只能讀一次）
    TempData[""Alert""] = ""操作成功！"";          // 存入 TempData

    // 方式 4: 強型別 Model（推薦！有智慧提示）
    var products = GetProducts();              // 取得商品列表
    return View(products);                     // 傳入 Model
}
```

```html
<!-- View 中使用資料 -->
@model List<Product>                           <!-- 宣告強型別 Model -->

<h1>@ViewData[""Title""]</h1>                    <!-- 使用 ViewData -->
<p>@ViewBag.Message</p>                        <!-- 使用 ViewBag -->

@if (TempData[""Alert""] != null)
{
    <div class=""alert"">@TempData[""Alert""]</div> <!-- 使用 TempData -->
}

<!-- 使用強型別 Model（推薦！） -->
@foreach (var p in Model)
{
    <p>@p.Name - @p.Price 元</p>               <!-- 有智慧提示，不易打錯 -->
}
```

| 方式 | 型別安全 | 跨 Request | 推薦度 |
|------|---------|-----------|--------|
| ViewData | ❌ 需轉型 | ❌ | ⭐ |
| ViewBag | ❌ 動態 | ❌ | ⭐⭐ |
| TempData | ❌ 需轉型 | ✅ 一次 | ⭐⭐ |
| **Model** | ✅ 強型別 | ❌ | ⭐⭐⭐⭐⭐ |

---

## Partial View 部分檢視

```html
<!-- Views/Shared/_ProductCard.cshtml（部分檢視） -->
@model Product                                <!-- 接收單一商品 -->

<div class=""card"">
    <h3>@Model.Name</h3>                       <!-- 商品名稱 -->
    <p>NT$ @Model.Price</p>                    <!-- 商品價格 -->
    <a asp-action=""Details""                    <!-- 連結到詳細頁 -->
       asp-route-id=""@Model.Id"">查看詳情</a>
</div>
```

```html
<!-- 在主頁面中使用 Partial View -->
@model List<Product>

@foreach (var product in Model)
{
    <!-- 方式 1: Tag Helper -->
    <partial name=""_ProductCard"" model=""product"" />

    <!-- 方式 2: HTML Helper -->
    @await Html.PartialAsync(""_ProductCard"", product)
}
```

---

## _ViewImports 與 _ViewStart

```html
<!-- Views/_ViewImports.cshtml -->
@using DotNetLearning.Models              <!-- 全域引用命名空間 -->
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers  <!-- 啟用 Tag Helpers -->
```

```html
<!-- Views/_ViewStart.cshtml -->
@{
    Layout = ""_Layout"";                    <!-- 所有 View 預設使用 _Layout 版面 -->
}
```

---

## Tag Helpers 標籤輔助

Tag Helpers 讓 HTML 標籤變得更聰明：

```html
<!-- 表單 Tag Helpers -->
<form asp-controller=""Products""           <!-- 指定控制器 -->
      asp-action=""Create""                  <!-- 指定動作 -->
      method=""post"">                       <!-- HTTP 方法 -->

    <div>
        <label asp-for=""Name""></label>      <!-- 自動產生 label -->
        <input asp-for=""Name"" />            <!-- 自動產生 name、id、驗證屬性 -->
        <span asp-validation-for=""Name""></span> <!-- 驗證訊息 -->
    </div>

    <div>
        <label asp-for=""Price""></label>     <!-- 價格標籤 -->
        <input asp-for=""Price"" />           <!-- 價格輸入框 -->
    </div>

    <button type=""submit"">建立</button>    <!-- 送出按鈕 -->
</form>

<!-- 連結 Tag Helpers -->
<a asp-controller=""Products""               <!-- 連結到 Products 控制器 -->
   asp-action=""Details""                     <!-- 連結到 Details 動作 -->
   asp-route-id=""5"">查看商品 5</a>          <!-- 傳遞路由參數 id=5 -->
<!-- 產生: <a href=""/Products/Details/5"">查看商品 5</a> -->
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：忘記宣告 @model 導致 Model 為 null

```html
<!-- ❌ 沒有 @model 宣告，Model 是 object 型別 -->
<h1>@Model.Name</h1>   <!-- 執行時會出錯：NullReferenceException -->
```

```html
<!-- ✅ 加上 @model 宣告 -->
@model Product          <!-- 明確宣告 Model 型別 -->
<h1>@Model.Name</h1>   <!-- 有智慧提示，編譯期就能檢查 -->
```

> **為什麼？** 沒有 `@model` 宣告，Razor 不知道 Model 的型別，無法在編譯期檢查屬性是否存在。

### ❌ 錯誤 2：在 Razor 中混淆 @ 符號

```html
<!-- ❌ 想輸出電子郵件地址 -->
<p>聯絡信箱：user@example.com</p>  <!-- Razor 會把 @example 當成 C# 變數 -->
```

```html
<!-- ✅ 用 @@ 跳脫 -->
<p>聯絡信箱：user@@example.com</p> <!-- @@ 輸出為 @ -->
```

> **為什麼？** Razor 用 `@` 作為 C# 程式碼的起始符號，寫電子郵件時要用 `@@` 來跳脫。

### ❌ 錯誤 3：在 _ViewImports 忘記加 TagHelper

```html
<!-- ❌ 沒有 @addTagHelper，Tag Helper 不會生效 -->
<a asp-controller=""Home"" asp-action=""Index"">首頁</a>
<!-- 產生: <a asp-controller=""Home"" asp-action=""Index"">首頁</a>（原封不動！） -->
```

```html
<!-- ✅ 在 _ViewImports.cshtml 加上 -->
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

<!-- 現在 Tag Helper 才會生效 -->
<a asp-controller=""Home"" asp-action=""Index"">首頁</a>
<!-- 產生: <a href=""/"">首頁</a> -->
```

> **為什麼？** Tag Helpers 需要透過 `@addTagHelper` 註冊才會啟用，否則 `asp-*` 屬性只是普通的 HTML 屬性，不會被處理。
" },

            // ── Chapter 14: Middleware 管線 ──────────────────────────
            new() { Id=14, Category="aspnet", Order=5, Level="intermediate", Icon="🔌", Title="Middleware 管線", Slug="aspnet-middleware", IsPublished=true, Content=@"
# Middleware 管線

## Middleware 是什麼？

想像 ASP.NET Core 的請求處理就像一根**水管**——水（請求）從一端流入，經過一連串的**過濾器**（Middleware），最後流出另一端（回應）。

```
請求 Request →
    [中介軟體 1: 日誌記錄]
        → [中介軟體 2: 身份驗證]
            → [中介軟體 3: 授權]
                → [中介軟體 4: 路由]
                    → Controller Action
                ← [中介軟體 4]
            ← [中介軟體 3]
        ← [中介軟體 2]
    ← [中介軟體 1]
← 回應 Response
```

每個 Middleware 可以：
1. **在請求進入時**做某些事（例如記錄日誌）
2. **決定是否傳給下一個** Middleware
3. **在回應出去時**做某些事（例如加 Header）

---

## app.Use、app.Map、app.Run

```csharp
var app = builder.Build();

// app.Use → 處理請求，然後傳給下一個 Middleware
app.Use(async (context, next) =>
{
    Console.WriteLine(""1. 請求進入"");   // 請求進入時執行
    await next();                       // 呼叫下一個 Middleware
    Console.WriteLine(""4. 回應離開"");   // 回應離開時執行
});

// 第二個 Middleware
app.Use(async (context, next) =>
{
    Console.WriteLine(""2. 第二層進入"");  // 進入第二個 Middleware
    await next();                       // 繼續傳遞
    Console.WriteLine(""3. 第二層離開"");  // 離開第二個 Middleware
});

// app.Run → 終端 Middleware，不會再傳給下一個
app.Run(async context =>
{
    Console.WriteLine(""到達終點！"");     // 請求到達終點
    await context.Response.WriteAsync(""Hello!""); // 寫入回應
});

// 輸出順序：1 → 2 → 到達終點 → 3 → 4
```

```csharp
// app.Map → 根據路徑分支
app.Map(""/api"", apiApp =>
{
    apiApp.Run(async context =>
    {
        await context.Response.WriteAsync(""API 端點""); // /api 路徑的處理
    });
});

app.Map(""/health"", healthApp =>
{
    healthApp.Run(async context =>
    {
        await context.Response.WriteAsync(""OK"");      // /health 健康檢查
    });
});
```

---

## 自訂 Middleware 類別

```csharp
// Middleware/RequestTimingMiddleware.cs
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;       // 下一個 Middleware 的委派

    // 建構子注入 RequestDelegate
    public RequestTimingMiddleware(RequestDelegate next)
    {
        _next = next;                             // 保存下一個 Middleware
    }

    // 每個請求都會呼叫 InvokeAsync
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew(); // 開始計時

        await _next(context);                     // 呼叫下一個 Middleware

        stopwatch.Stop();                         // 停止計時
        var elapsed = stopwatch.ElapsedMilliseconds; // 取得經過時間

        // 加入自訂 Header 顯示處理時間
        context.Response.Headers[""X-Response-Time""] = $""{elapsed}ms"";
        Console.WriteLine($""請求 {context.Request.Path} 花了 {elapsed}ms""); // 記錄
    }
}

// 擴充方法讓註冊更方便
public static class RequestTimingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTiming(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTimingMiddleware>(); // 註冊 Middleware
    }
}
```

```csharp
// Program.cs - 使用自訂 Middleware
var app = builder.Build();
app.UseRequestTiming();                  // 使用計時 Middleware
app.UseStaticFiles();                    // 靜態檔案
app.UseRouting();                        // 路由
app.UseAuthorization();                  // 授權
```

---

## 順序很重要！

Middleware 的註冊**順序就是執行順序**：

```csharp
// Program.cs - 正確的 Middleware 順序
var app = builder.Build();

// 1. 例外處理（最外層，捕捉所有錯誤）
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler(""/Home/Error"");  // 錯誤處理

// 2. HTTPS 重新導向
app.UseHttpsRedirection();               // HTTP → HTTPS

// 3. 靜態檔案（不需驗證即可存取）
app.UseStaticFiles();                     // wwwroot 底下的檔案

// 4. 路由（決定要走哪個 Endpoint）
app.UseRouting();                         // 路由比對

// 5. CORS（跨域請求）
app.UseCors();                            // 跨來源資源共用

// 6. 身份驗證（你是誰？）
app.UseAuthentication();                  // 驗證身份

// 7. 授權（你能做什麼？）
app.UseAuthorization();                   // 檢查權限

// 8. Endpoint 執行
app.MapControllerRoute(                   // 對應到 Controller
    name: ""default"",
    pattern: ""{controller=Home}/{action=Index}/{id?}"");
```

---

## 內建常用 Middleware

| Middleware | 功能 | 順序建議 |
|-----------|------|---------|
| `UseExceptionHandler` | 全域例外處理 | 最先 |
| `UseHttpsRedirection` | HTTP 轉 HTTPS | 靠前 |
| `UseStaticFiles` | 提供靜態檔案 | 驗證前 |
| `UseRouting` | 路由比對 | 中間 |
| `UseCors` | 跨域設定 | 驗證前 |
| `UseAuthentication` | 身份驗證 | 授權前 |
| `UseAuthorization` | 授權檢查 | 驗證後 |

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：把 UseAuthorization 放在 UseRouting 前面

```csharp
// ❌ 順序錯誤！授權在路由前面
app.UseAuthorization();                   // 授權（但還不知道 Endpoint！）
app.UseRouting();                         // 路由
```

```csharp
// ✅ 正確順序
app.UseRouting();                         // 先比對路由
app.UseAuthorization();                   // 再檢查授權
```

> **為什麼？** `UseAuthorization` 需要知道目標 Endpoint 才能判斷授權策略，放在 `UseRouting` 前面就無法取得 Endpoint 資訊。

### ❌ 錯誤 2：忘記呼叫 next() 導致請求中斷

```csharp
// ❌ 忘記呼叫 next()，請求到這裡就停了
app.Use(async (context, next) =>
{
    Console.WriteLine(""記錄日誌"");        // 記錄後...什麼都沒做
    // 忘記 await next()!                 // 請求無法繼續！
});
```

```csharp
// ✅ 記得呼叫 next()
app.Use(async (context, next) =>
{
    Console.WriteLine(""記錄日誌"");        // 記錄日誌
    await next();                         // 傳給下一個 Middleware
});
```

> **為什麼？** `app.Use` 中如果不呼叫 `next()`，請求就不會傳到後面的 Middleware，用戶會收到空白回應。

### ❌ 錯誤 3：UseStaticFiles 放在 UseAuthentication 後面

```csharp
// ❌ 靜態檔案（CSS/JS/圖片）也要驗證身份
app.UseAuthentication();                  // 先驗證
app.UseStaticFiles();                     // 靜態檔案也被驗證擋住了！
```

```csharp
// ✅ 靜態檔案放在驗證前面
app.UseStaticFiles();                     // 靜態檔案不需驗證
app.UseAuthentication();                  // 只驗證動態請求
```

> **為什麼？** CSS、JavaScript、圖片等靜態資源不需要驗證身份，放在 `UseAuthentication` 後面會導致未登入用戶連頁面樣式都載入不了。
" },

            // ── Chapter 15: 依賴注入 DI ──────────────────────────────
            new() { Id=15, Category="aspnet", Order=6, Level="intermediate", Icon="💉", Title="依賴注入 DI", Slug="aspnet-dependency-injection", IsPublished=true, Content=@"
# 依賴注入 DI（Dependency Injection）

## 什麼是依賴注入？

想像你去**餐廳點餐**：
- ❌ **沒有 DI**：你自己走進廚房、找食材、自己煮（在程式碼裡自己 `new` 物件）
- ✅ **有 DI**：你跟服務生說你要什麼，餐廳幫你準備好送來（框架幫你建立物件）

```csharp
// ❌ 沒有 DI：Controller 自己建立服務
public class OrderController : Controller
{
    public IActionResult Index()
    {
        var service = new OrderService();        // 自己 new（緊耦合！）
        var db = new AppDbContext();              // 每次都要自己建
        return View(service.GetOrders());        // 取得訂單
    }
}

// ✅ 有 DI：框架自動注入
public class OrderController : Controller
{
    private readonly IOrderService _service;     // 只宣告介面

    public OrderController(IOrderService service) // 建構子注入
    {
        _service = service;                      // 框架會自動提供實例
    }

    public IActionResult Index()
    {
        return View(_service.GetOrders());       // 直接使用（鬆耦合！）
    }
}
```

---

## 三種生命週期

```csharp
// Program.cs - 註冊服務
var builder = WebApplication.CreateBuilder(args);

// Transient：每次注入都建立新實例（像即溶咖啡，每杯都新泡）
builder.Services.AddTransient<IEmailService, EmailService>();

// Scoped：每個 HTTP 請求共用一個實例（像餐廳一桌一壺茶）
builder.Services.AddScoped<IOrderService, OrderService>();

// Singleton：整個應用程式只有一個實例（像飲水機，大家共用）
builder.Services.AddSingleton<ICacheService, CacheService>();
```

### 什麼時候用哪個？

| 生命週期 | 說明 | 適合場景 | 比喻 |
|---------|------|---------|------|
| **Transient** | 每次都新建 | 輕量、無狀態的服務 | 即溶咖啡 |
| **Scoped** | 每個請求一個 | DbContext、購物車 | 一桌一壺茶 |
| **Singleton** | 全域唯一 | 快取、設定檔、Logger | 飲水機 |

---

## 建構子注入

```csharp
// 定義介面
public interface IProductService
{
    List<Product> GetAll();                       // 取得所有商品
    Product? GetById(int id);                     // 用 ID 取得商品
}

// 實作介面
public class ProductService : IProductService
{
    private readonly AppDbContext _db;             // 資料庫 Context

    public ProductService(AppDbContext db)         // 注入 DbContext
    {
        _db = db;                                 // 保存參考
    }

    public List<Product> GetAll()
    {
        return _db.Products.ToList();             // 從資料庫取得所有商品
    }

    public Product? GetById(int id)
    {
        return _db.Products.Find(id);             // 用主鍵查詢
    }
}
```

```csharp
// Program.cs - 註冊服務
builder.Services.AddScoped<IProductService, ProductService>(); // 註冊介面與實作
builder.Services.AddDbContext<AppDbContext>(options =>          // 註冊 DbContext
    options.UseSqlServer(connectionString));                    // 使用 SQL Server
```

```csharp
// Controller 中使用
public class ProductsController : Controller
{
    private readonly IProductService _productService; // 介面欄位

    // 建構子注入：框架會自動提供 IProductService 實例
    public ProductsController(IProductService productService)
    {
        _productService = productService;             // 保存注入的服務
    }

    public IActionResult Index()
    {
        var products = _productService.GetAll();      // 使用服務取得資料
        return View(products);                        // 傳給 View
    }
}
```

---

## IServiceCollection 常用方法

```csharp
// Program.cs - 各種註冊方式
var builder = WebApplication.CreateBuilder(args);

// 1. 基本註冊
builder.Services.AddTransient<IMyService, MyService>();    // 介面 → 實作

// 2. 註冊自己（沒有介面）
builder.Services.AddScoped<MyService>();                   // 直接註冊類別

// 3. 用工廠方法註冊
builder.Services.AddTransient<IMyService>(sp =>            // sp = ServiceProvider
{
    var config = sp.GetRequiredService<IConfiguration>();   // 取得其他服務
    return new MyService(config[""ApiKey""]!);                // 用設定值建立
});

// 4. 註冊多個實作
builder.Services.AddTransient<INotifier, EmailNotifier>(); // 第一個實作
builder.Services.AddTransient<INotifier, SmsNotifier>();   // 第二個實作
// 注入 IEnumerable<INotifier> 可以拿到所有實作
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：Captive Dependency（Singleton 持有 Scoped）

```csharp
// ❌ Singleton 服務注入 Scoped 服務（被困的依賴）
builder.Services.AddSingleton<ICacheService, CacheService>(); // Singleton
builder.Services.AddScoped<IDbContext, AppDbContext>();        // Scoped

public class CacheService : ICacheService
{
    private readonly IDbContext _db;               // ❌ Singleton 持有 Scoped！
    public CacheService(IDbContext db)             // DbContext 永遠不會被釋放
    {
        _db = db;                                  // 記憶體洩漏！
    }
}
```

```csharp
// ✅ 用 IServiceScopeFactory 手動建立 Scope
public class CacheService : ICacheService
{
    private readonly IServiceScopeFactory _factory; // 注入 Scope 工廠

    public CacheService(IServiceScopeFactory factory)
    {
        _factory = factory;                        // 保存工廠
    }

    public List<Product> GetCachedProducts()
    {
        using var scope = _factory.CreateScope();  // 建立新的 Scope
        var db = scope.ServiceProvider
            .GetRequiredService<IDbContext>();       // 從 Scope 取得 DbContext
        return db.Products.ToList();               // 使用後 Scope 會自動 Dispose
    }
}
```

> **為什麼？** Singleton 存活整個應用程式生命週期，但 Scoped 應該每個請求結束就釋放。Singleton 持有 Scoped 會導致 Scoped 永遠不被釋放，造成記憶體洩漏。

### ❌ 錯誤 2：忘記註冊服務

```csharp
// ❌ 忘記在 Program.cs 註冊 IOrderService
public class OrderController : Controller
{
    public OrderController(IOrderService service) { } // 執行時會報錯！
}
// 錯誤：InvalidOperationException: Unable to resolve service for type 'IOrderService'
```

```csharp
// ✅ 記得在 Program.cs 註冊
builder.Services.AddScoped<IOrderService, OrderService>(); // 註冊服務
```

> **為什麼？** DI 容器只認識你註冊過的服務，沒註冊就不知道怎麼建立實例，會在執行時丟出例外。

### ❌ 錯誤 3：在建構子裡做太多事

```csharp
// ❌ 建構子裡做複雜初始化（萬一失敗整個服務就壞了）
public class ReportService : IReportService
{
    private readonly List<Report> _reports;         // 報表快取

    public ReportService(IDbContext db)
    {
        _reports = db.Reports.ToList();             // ❌ 建構子裡查資料庫！
    }
}
```

```csharp
// ✅ 建構子只存參考，方法裡才做邏輯
public class ReportService : IReportService
{
    private readonly IDbContext _db;                 // 只存參考

    public ReportService(IDbContext db)
    {
        _db = db;                                   // 建構子只做簡單賦值
    }

    public List<Report> GetReports()
    {
        return _db.Reports.ToList();                // 方法裡才查詢
    }
}
```

> **為什麼？** 建構子應該只做欄位賦值，不該有商業邏輯或 I/O 操作。建構子失敗會導致整個 DI 解析失敗，很難除錯。
" },

            // ── Chapter 16: 身份驗證與授權 ──────────────────────────
            new() { Id=16, Category="aspnet", Order=7, Level="intermediate", Icon="🔐", Title="身份驗證與授權", Slug="aspnet-auth", IsPublished=true, Content=@"
# 身份驗證與授權

## 驗證 vs 授權

- **身份驗證 Authentication**：你是誰？（像門口的**保全**確認你的身份證）
- **授權 Authorization**：你能做什麼？（像**VIP 識別**決定你能進哪些區域）

```
用戶請求 → 身份驗證（你是誰？）→ 授權（你能做什麼？）→ 存取資源
         「請出示證件」      「確認你有 VIP 資格」
```

---

## Cookie 驗證

```csharp
// Program.cs - 設定 Cookie 驗證
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)  // 使用 Cookie 方案
    .AddCookie(options =>
    {
        options.LoginPath = ""/Account/Login"";           // 未登入時導向登入頁
        options.AccessDeniedPath = ""/Account/Denied"";   // 權限不足時導向
        options.ExpireTimeSpan = TimeSpan.FromHours(2);  // Cookie 2 小時過期
    });
```

```csharp
// AccountController.cs - 登入邏輯
public class AccountController : Controller
{
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // 驗證帳號密碼（這裡簡化示範）
        if (model.Username == ""admin"" && model.Password == ""pass123"")
        {
            // 建立 Claims（聲明）
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),     // 使用者名稱
                new Claim(ClaimTypes.Role, ""Admin""),             // 角色
                new Claim(""Department"", ""IT"")                    // 自訂 Claim
            };

            // 建立身份識別
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme); // 驗證方案

            // 登入（寫入 Cookie）
            await HttpContext.SignInAsync(
                new ClaimsPrincipal(identity));                     // 建立主體並登入

            return RedirectToAction(""Index"", ""Home"");              // 導向首頁
        }

        ModelState.AddModelError("""", ""帳號或密碼錯誤"");              // 驗證失敗
        return View(model);                                        // 回到登入頁
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();                          // 登出（清除 Cookie）
        return RedirectToAction(""Index"", ""Home"");                  // 導向首頁
    }
}
```

---

## JWT Token 驗證

```csharp
// Program.cs - 設定 JWT 驗證
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,                              // 驗證發行者
            ValidateAudience = true,                            // 驗證受眾
            ValidateLifetime = true,                            // 驗證有效期
            ValidateIssuerSigningKey = true,                    // 驗證簽章金鑰
            ValidIssuer = builder.Configuration[""Jwt:Issuer""],  // 合法發行者
            ValidAudience = builder.Configuration[""Jwt:Audience""], // 合法受眾
            IssuerSigningKey = new SymmetricSecurityKey(         // 簽章金鑰
                Encoding.UTF8.GetBytes(
                    builder.Configuration[""Jwt:Key""]!))          // 從設定檔讀取
        };
    });
```

```csharp
// 產生 JWT Token
public string GenerateToken(User user)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.Username),              // 使用者名稱
        new Claim(ClaimTypes.Role, user.Role),                  // 角色
        new Claim(JwtRegisteredClaimNames.Jti,
            Guid.NewGuid().ToString())                          // Token 唯一識別碼
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_config[""Jwt:Key""]!));           // 金鑰

    var token = new JwtSecurityToken(
        issuer: _config[""Jwt:Issuer""],                          // 發行者
        audience: _config[""Jwt:Audience""],                      // 受眾
        claims: claims,                                         // 聲明
        expires: DateTime.Now.AddHours(1),                      // 1 小時後過期
        signingCredentials: new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256));               // 簽章演算法

    return new JwtSecurityTokenHandler().WriteToken(token);      // 產生 Token 字串
}
```

---

## [Authorize] 與 [AllowAnonymous]

```csharp
// 需要登入才能存取的 Controller
[Authorize]                                     // 整個 Controller 需要登入
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        var name = User.Identity?.Name;         // 取得登入者名稱
        return View();                          // 回傳儀表板頁面
    }

    [AllowAnonymous]                            // 這個 Action 允許匿名存取
    public IActionResult PublicPage()
    {
        return View();                          // 不用登入也能看
    }

    [Authorize(Roles = ""Admin"")]                // 只有 Admin 角色可以存取
    public IActionResult AdminOnly()
    {
        return View();                          // 管理員專用
    }

    [Authorize(Policy = ""AtLeast18"")]           // 自訂授權策略
    public IActionResult AdultContent()
    {
        return View();                          // 符合策略才能存取
    }
}
```

```csharp
// Program.cs - 設定授權策略
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(""AtLeast18"", policy =>
        policy.RequireClaim(""Age"")                             // 需要 Age Claim
              .RequireAssertion(ctx =>
              {
                  var age = int.Parse(
                      ctx.User.FindFirst(""Age"")?.Value ?? ""0""); // 取得年齡
                  return age >= 18;                              // 年滿 18 歲
              }));
});
```

---

## ASP.NET Identity 基礎

```csharp
// Program.cs - 設定 ASP.NET Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;          // 密碼要有數字
    options.Password.RequiredLength = 8;            // 密碼至少 8 碼
    options.Password.RequireUppercase = true;       // 密碼要有大寫字母
    options.Lockout.MaxFailedAccessAttempts = 5;    // 失敗 5 次鎖定
    options.Lockout.DefaultLockoutTimeSpan =
        TimeSpan.FromMinutes(15);                   // 鎖定 15 分鐘
})
.AddRoles<IdentityRole>()                          // 啟用角色管理
.AddEntityFrameworkStores<AppDbContext>();           // 使用 EF Core 儲存
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：把密碼存成明文

```csharp
// ❌ 明文儲存密碼（超級危險！）
var user = new User
{
    Username = ""admin"",
    Password = ""mypassword123""   // ❌ 明文！資料庫被偷密碼就外洩了
};
db.Users.Add(user);
```

```csharp
// ✅ 使用雜湊（Hash）
var passwordHasher = new PasswordHasher<User>(); // 建立密碼雜湊器
var user = new User { Username = ""admin"" };       // 建立使用者
user.PasswordHash = passwordHasher.HashPassword(
    user, ""mypassword123"");                        // 雜湊後儲存
db.Users.Add(user);                               // 存入資料庫

// 驗證時
var result = passwordHasher.VerifyHashedPassword(
    user, user.PasswordHash, inputPassword);       // 比對雜湊值
```

> **為什麼？** 資料庫被入侵時，明文密碼會直接曝光。雜湊是單向的，即使被偷也無法還原。

### ❌ 錯誤 2：JWT 金鑰寫在程式碼裡

```csharp
// ❌ 金鑰寫死在程式碼中（推上 Git 就洩漏了）
var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(""my-super-secret-key-12345"")); // ❌ 硬編碼金鑰
```

```csharp
// ✅ 從設定檔或環境變數讀取
var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(
        builder.Configuration[""Jwt:Key""]!));               // ✅ 從設定檔讀取
```

```json
// appsettings.json（開發環境用）
{
    ""Jwt"": {
        ""Key"": ""development-only-key-do-not-use-in-prod""
    }
}
// 正式環境用環境變數或 Azure Key Vault
```

> **為什麼？** 金鑰寫在程式碼裡，推到 Git 就全世界都看得到。應該用設定檔或環境變數管理機密資訊。

### ❌ 錯誤 3：沒有驗證 JWT Token 的有效期

```csharp
// ❌ 停用有效期驗證（永遠不過期的 Token）
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateLifetime = false,  // ❌ 不檢查過期！Token 被偷就永遠能用
};
```

```csharp
// ✅ 啟用所有驗證
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateLifetime = true,                    // ✅ 檢查有效期
    ClockSkew = TimeSpan.FromMinutes(5),        // 允許 5 分鐘時鐘偏差
    ValidateIssuer = true,                      // 驗證發行者
    ValidateAudience = true,                    // 驗證受眾
    ValidateIssuerSigningKey = true,            // 驗證簽章
};
```

> **為什麼？** 停用有效期驗證等於 Token 永遠有效，一旦被竊取，攻擊者可以永遠冒充該用戶。
" },

            // ── Chapter 17: Web API 開發 ──────────────────────────
            new() { Id=17, Category="aspnet", Order=8, Level="intermediate", Icon="📡", Title="Web API 開發", Slug="aspnet-web-api", IsPublished=true, Content=@"
# Web API 開發

## Web API 是什麼？

Web API 就像是**餐廳的外送窗口**——不提供堂食（HTML 頁面），只提供**打包好的餐點**（JSON 資料）給外送平台（前端、手機 App）。

```
前端 App / 手機 App
    ↓ HTTP Request（JSON）
[ApiController]
    ↓ 處理
    ↑ HTTP Response（JSON）
前端 App / 手機 App
```

---

## [ApiController] 基礎

```csharp
// Controllers/ProductsApiController.cs
[ApiController]                                     // 標記為 API 控制器
[Route(""api/[controller]"")]                          // 路由：api/ProductsApi
public class ProductsApiController : ControllerBase  // 繼承 ControllerBase（不是 Controller）
{
    private readonly IProductService _service;        // 商品服務

    // 建構子注入
    public ProductsApiController(IProductService service)
    {
        _service = service;                           // 保存服務參考
    }

    // GET api/ProductsApi
    [HttpGet]                                        // 處理 GET 請求
    public ActionResult<List<Product>> GetAll()
    {
        var products = _service.GetAll();             // 取得所有商品
        return Ok(products);                          // 回傳 200 + JSON
    }

    // GET api/ProductsApi/5
    [HttpGet(""{id}"")]                                 // 路由參數
    public ActionResult<Product> GetById(int id)
    {
        var product = _service.GetById(id);           // 用 ID 查詢
        if (product == null)
            return NotFound();                        // 找不到回傳 404
        return Ok(product);                           // 回傳 200 + JSON
    }

    // POST api/ProductsApi
    [HttpPost]                                       // 處理 POST 請求
    public ActionResult<Product> Create(
        [FromBody] CreateProductDto dto)              // 從請求主體綁定
    {
        var product = _service.Create(dto);           // 建立商品
        return CreatedAtAction(                       // 回傳 201 Created
            nameof(GetById),                          // 指向 GetById Action
            new { id = product.Id },                  // 路由參數
            product);                                 // 回應主體
    }

    // PUT api/ProductsApi/5
    [HttpPut(""{id}"")]                                 // 處理 PUT 請求
    public IActionResult Update(
        int id,
        [FromBody] UpdateProductDto dto)              // 從請求主體綁定
    {
        if (!_service.Exists(id))
            return NotFound();                        // 找不到回傳 404
        _service.Update(id, dto);                     // 更新商品
        return NoContent();                           // 回傳 204 No Content
    }

    // DELETE api/ProductsApi/5
    [HttpDelete(""{id}"")]                              // 處理 DELETE 請求
    public IActionResult Delete(int id)
    {
        if (!_service.Exists(id))
            return NotFound();                        // 找不到回傳 404
        _service.Delete(id);                          // 刪除商品
        return NoContent();                           // 回傳 204 No Content
    }
}
```

---

## Model Binding 模型綁定

```csharp
// 各種綁定來源
[HttpGet(""search"")]
public IActionResult Search(
    [FromQuery] string keyword,                       // 從 URL 查詢字串：?keyword=手機
    [FromQuery] int page = 1,                         // 預設值為 1
    [FromHeader(Name = ""X-Api-Key"")] string? apiKey)  // 從 HTTP Header
{
    return Ok(new { keyword, page, apiKey });         // 回傳綁定結果
}

[HttpPost(""upload"")]
public IActionResult Upload(
    [FromForm] string description,                    // 從表單欄位
    [FromForm] IFormFile file)                         // 從表單檔案
{
    return Ok(new { description, file.FileName });    // 回傳檔案名稱
}

[HttpPut(""{id}"")]
public IActionResult Update(
    [FromRoute] int id,                               // 從路由參數
    [FromBody] UpdateDto dto)                          // 從請求主體（JSON）
{
    return Ok(new { id, dto });                       // 回傳更新資料
}
```

---

## DTO（Data Transfer Object）

```csharp
// DTOs/CreateProductDto.cs - 建立商品用的 DTO
public class CreateProductDto
{
    [Required(ErrorMessage = ""商品名稱必填"")]           // 必填驗證
    [StringLength(100, ErrorMessage = ""名稱最多 100 字"")]
    public string Name { get; set; } = """";             // 商品名稱

    [Range(0, 999999, ErrorMessage = ""價格必須在 0~999999"")]
    public decimal Price { get; set; }                  // 商品價格

    public string? Description { get; set; }            // 商品描述（可選）
}

// DTOs/ProductResponseDto.cs - 回應用的 DTO
public class ProductResponseDto
{
    public int Id { get; set; }                         // 商品 ID
    public string Name { get; set; } = """";              // 商品名稱
    public decimal Price { get; set; }                  // 價格
    // 注意：不包含敏感欄位如 Cost、Supplier 等
}
```

---

## ActionResult<T> 與 IActionResult

```csharp
// ActionResult<T> → 有明確回傳型別（Swagger 能自動產生文件）
[HttpGet(""{id}"")]
[ProducesResponseType(typeof(Product), 200)]           // 200 回傳 Product
[ProducesResponseType(404)]                            // 404 找不到
public ActionResult<Product> GetById(int id)
{
    var product = _service.GetById(id);                // 查詢商品
    if (product == null) return NotFound();            // 404
    return Ok(product);                                // 200 + Product JSON
}

// IActionResult → 回傳型別不固定
[HttpPost]
public IActionResult Create([FromBody] CreateProductDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);                 // 400 驗證失敗
    var product = _service.Create(dto);                // 建立商品
    return CreatedAtAction(                            // 201 Created
        nameof(GetById), new { id = product.Id }, product);
}
```

---

## API 版本控制

```csharp
// Program.cs - 設定 API 版本控制
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);  // 預設版本 1.0
    options.AssumeDefaultVersionWhenUnspecified = true; // 未指定時用預設版本
    options.ReportApiVersions = true;                  // 回應中回報版本資訊
});
```

```csharp
// v1 控制器
[ApiController]
[Route(""api/v{version:apiVersion}/products"")]         // URL 路徑版本控制
[ApiVersion(""1.0"")]                                    // 版本 1.0
public class ProductsV1Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(new { version = ""v1"", data = ""舊格式"" });   // v1 的回應格式
}

// v2 控制器
[ApiController]
[Route(""api/v{version:apiVersion}/products"")]
[ApiVersion(""2.0"")]                                    // 版本 2.0
public class ProductsV2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(new { version = ""v2"", items = ""新格式"" });   // v2 的新回應格式
}
```

---

## Swagger / OpenAPI

```csharp
// Program.cs - 設定 Swagger
builder.Services.AddEndpointsApiExplorer();            // API 探索器
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(""v1"", new OpenApiInfo            // Swagger 文件設定
    {
        Title = ""商品 API"",                              // API 標題
        Version = ""v1"",                                  // 版本號
        Description = ""商品管理 RESTful API""               // 說明
    });
});

var app = builder.Build();

// 只在開發環境啟用 Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                                  // 啟用 Swagger JSON
    app.UseSwaggerUI();                                // 啟用 Swagger UI
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：所有情況都回傳 200

```csharp
// ❌ 找不到也回傳 200（前端無法判斷是否成功）
[HttpGet(""{id}"")]
public IActionResult GetById(int id)
{
    var product = _service.GetById(id);                // 查詢商品
    return Ok(product);                                // ❌ product 可能是 null！
}
```

```csharp
// ✅ 正確使用 HTTP 狀態碼
[HttpGet(""{id}"")]
public IActionResult GetById(int id)
{
    var product = _service.GetById(id);                // 查詢商品
    if (product == null)
        return NotFound(new { message = ""商品不存在"" }); // 404 找不到
    return Ok(product);                                // 200 找到了
}
```

> **為什麼？** HTTP 狀態碼是 API 的通用語言，`200` 表示成功，`404` 表示找不到。前端靠狀態碼判斷如何處理回應。

### ❌ 錯誤 2：直接回傳 Entity（沒用 DTO）

```csharp
// ❌ 直接回傳資料庫實體（洩漏敏感資料）
[HttpGet(""{id}"")]
public ActionResult<User> GetUser(int id)
{
    var user = _db.Users.Find(id);                     // 查詢使用者
    return Ok(user);                                   // ❌ 包含 PasswordHash！
}
// JSON 回應會包含：{ ""passwordHash"": ""abc123..."", ... }
```

```csharp
// ✅ 使用 DTO 只回傳需要的欄位
[HttpGet(""{id}"")]
public ActionResult<UserDto> GetUser(int id)
{
    var user = _db.Users.Find(id);                     // 查詢使用者
    if (user == null) return NotFound();               // 找不到
    var dto = new UserDto                              // 建立 DTO
    {
        Id = user.Id,                                  // 只包含安全的欄位
        Username = user.Username,                      // 使用者名稱
        Email = user.Email                             // 電子郵件
    };
    return Ok(dto);                                    // 回傳 DTO
}
```

> **為什麼？** 資料庫實體可能包含密碼雜湊、內部 ID 等敏感資訊。DTO 只暴露前端需要的欄位，保護資料安全。

### ❌ 錯誤 3：API 沒有驗證輸入

```csharp
// ❌ 完全不驗證就直接用（可能收到垃圾資料）
[HttpPost]
public IActionResult Create([FromBody] CreateProductDto dto)
{
    _service.Create(dto);                              // ❌ dto 的欄位可能是 null！
    return Ok();
}
```

```csharp
// ✅ 用 DataAnnotation + ModelState 驗證
[HttpPost]
public IActionResult Create([FromBody] CreateProductDto dto)
{
    if (!ModelState.IsValid)                            // 檢查驗證結果
        return BadRequest(ModelState);                  // 回傳 400 + 錯誤訊息
    var product = _service.Create(dto);                 // 驗證通過才建立
    return CreatedAtAction(nameof(GetById),
        new { id = product.Id }, product);              // 回傳 201
}
```

> **為什麼？** 永遠不要信任前端傳來的資料！DataAnnotation 加上 ModelState 驗證可以在進入商業邏輯前就擋掉不合法的輸入。
" },

            // ── Chapter 18: SignalR 即時通訊 ──────────────────────────
            new() { Id=18, Category="aspnet", Order=9, Level="advanced", Icon="📨", Title="SignalR 即時通訊", Slug="aspnet-signalr", IsPublished=true, Content=@"
# SignalR 即時通訊

## SignalR 是什麼？

想像 **LINE 群組**——當有人發訊息時，群組裡的每個人都會**即時收到通知**，不需要一直重新整理。SignalR 就是讓伺服器能**主動推送**訊息給瀏覽器的技術。

```
傳統 HTTP（輪詢）：
瀏覽器：「有新訊息嗎？」 → 伺服器：「沒有」
瀏覽器：「有新訊息嗎？」 → 伺服器：「沒有」
瀏覽器：「有新訊息嗎？」 → 伺服器：「有！」
（浪費很多次查詢）

SignalR（即時推送）：
瀏覽器：「我要連線」 → 伺服器：「OK，建立連線」
... 過了一段時間 ...
伺服器：「有新訊息給你！」 → 瀏覽器立刻收到
（伺服器主動推送，不需要輪詢）
```

---

## Hub 類別

```csharp
// Hubs/ChatHub.cs
using Microsoft.AspNetCore.SignalR;

// Hub 是 SignalR 的核心，像是聊天室的「伺服器端管理員」
public class ChatHub : Hub
{
    // 當客戶端呼叫 SendMessage 時觸發
    public async Task SendMessage(string user, string message)
    {
        // 廣播給所有連線的客戶端
        await Clients.All.SendAsync(
            ""ReceiveMessage"",   // 客戶端要監聽的方法名稱
            user,               // 發送者
            message);           // 訊息內容
    }

    // 發送給特定使用者（私訊）
    public async Task SendPrivateMessage(
        string targetUser,
        string message)
    {
        await Clients.User(targetUser).SendAsync(
            ""ReceivePrivateMessage"",   // 私訊事件
            Context.User?.Identity?.Name, // 發送者名稱
            message);                    // 訊息內容
    }

    // 當客戶端連線時
    public override async Task OnConnectedAsync()
    {
        var user = Context.User?.Identity?.Name ?? ""匿名""; // 取得使用者名稱
        await Clients.All.SendAsync(
            ""UserJoined"",     // 通知所有人
            user);             // 誰加入了
        await base.OnConnectedAsync();                      // 呼叫基底方法
    }

    // 當客戶端斷線時
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.User?.Identity?.Name ?? ""匿名""; // 取得使用者名稱
        await Clients.All.SendAsync(
            ""UserLeft"",       // 通知所有人
            user);             // 誰離開了
        await base.OnDisconnectedAsync(exception);          // 呼叫基底方法
    }
}
```

```csharp
// Program.cs - 註冊 SignalR
builder.Services.AddSignalR();                    // 註冊 SignalR 服務

var app = builder.Build();
app.MapHub<ChatHub>(""/chatHub"");                  // 設定 Hub 端點路徑
```

---

## 客戶端 JavaScript

```html
<!-- 引入 SignalR 客戶端函式庫 -->
<script src=""https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.0/signalr.min.js""></script>

<script>
    // 建立連線
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(""/chatHub"")                      // 連線到 Hub 端點
        .withAutomaticReconnect()                 // 自動重新連線
        .build();

    // 監聽伺服器推送的訊息
    connection.on(""ReceiveMessage"", (user, message) => {
        // 收到訊息時在頁面上顯示
        const li = document.createElement(""li"");  // 建立清單項目
        li.textContent = `${user}: ${message}`;   // 設定內容
        document.getElementById(""messages"")
            .appendChild(li);                     // 加到訊息列表
    });

    // 監聽使用者加入
    connection.on(""UserJoined"", (user) => {
        console.log(`${user} 加入了聊天室`);       // 記錄到主控台
    });

    // 啟動連線
    connection.start()
        .then(() => console.log(""已連線到 SignalR""))  // 連線成功
        .catch(err => console.error(""連線失敗："", err)); // 連線失敗

    // 發送訊息
    function sendMessage() {
        const user = document.getElementById(""userInput"").value;    // 取得使用者名稱
        const message = document.getElementById(""messageInput"").value; // 取得訊息
        connection.invoke(""SendMessage"", user, message)              // 呼叫 Hub 方法
            .catch(err => console.error(""發送失敗："", err));             // 錯誤處理
    }
</script>
```

---

## 群組 Groups

```csharp
// Hubs/ChatHub.cs - 群組功能
public class ChatHub : Hub
{
    // 加入群組
    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,      // 目前連線的 ID
            roomName);                 // 群組名稱
        await Clients.Group(roomName).SendAsync(
            ""RoomNotification"",       // 群組通知
            $""{Context.User?.Identity?.Name} 加入了 {roomName}""); // 訊息
    }

    // 離開群組
    public async Task LeaveRoom(string roomName)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,      // 目前連線的 ID
            roomName);                 // 群組名稱
        await Clients.Group(roomName).SendAsync(
            ""RoomNotification"",       // 群組通知
            $""{Context.User?.Identity?.Name} 離開了 {roomName}""); // 訊息
    }

    // 發送訊息到特定群組
    public async Task SendToRoom(
        string roomName,
        string message)
    {
        await Clients.Group(roomName).SendAsync(
            ""ReceiveRoomMessage"",     // 群組訊息事件
            Context.User?.Identity?.Name, // 發送者
            roomName,                  // 群組名稱
            message);                  // 訊息內容
    }
}
```

---

## 強型別 Hub

```csharp
// 定義客戶端介面
public interface IChatClient
{
    Task ReceiveMessage(string user, string message);    // 接收訊息
    Task UserJoined(string user);                        // 使用者加入
    Task UserLeft(string user);                          // 使用者離開
    Task RoomNotification(string notification);          // 群組通知
}

// 使用強型別 Hub（有編譯期檢查！）
public class ChatHub : Hub<IChatClient>
{
    public async Task SendMessage(string user, string message)
    {
        // 有智慧提示！不會打錯方法名稱
        await Clients.All.ReceiveMessage(user, message); // 編譯期就能檢查
    }

    public override async Task OnConnectedAsync()
    {
        var name = Context.User?.Identity?.Name ?? ""匿名"";
        await Clients.All.UserJoined(name);              // 強型別，不會拼錯
        await base.OnConnectedAsync();
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：沒有處理重新連線

```javascript
// ❌ 沒有自動重連，斷線後就再也收不到訊息
const connection = new signalR.HubConnectionBuilder()
    .withUrl(""/chatHub"")
    .build();                                // ❌ 沒有 withAutomaticReconnect

connection.start();                          // 連上後斷線就沒了
```

```javascript
// ✅ 啟用自動重連 + 斷線處理
const connection = new signalR.HubConnectionBuilder()
    .withUrl(""/chatHub"")
    .withAutomaticReconnect([0, 2000, 5000, 10000]) // 重連間隔（毫秒）
    .build();

connection.onreconnecting((error) => {
    console.log(""正在重新連線..."");            // 通知使用者
    document.getElementById(""status"")
        .textContent = ""重新連線中..."";         // 更新 UI 狀態
});

connection.onreconnected((connectionId) => {
    console.log(""重新連線成功！"");              // 連線恢復
    document.getElementById(""status"")
        .textContent = ""已連線"";               // 更新 UI 狀態
});

connection.onclose((error) => {
    console.log(""連線已關閉"");                 // 完全斷線
    // 可以嘗試手動重連
    setTimeout(() => connection.start(), 5000); // 5 秒後重試
});

connection.start();                          // 啟動連線
```

> **為什麼？** 網路不穩定時連線會中斷，沒有重連機制的話用戶就收不到新訊息。`withAutomaticReconnect` 會自動嘗試重新連線。

### ❌ 錯誤 2：Hub 方法名稱拼錯（字串魔術）

```csharp
// ❌ 方法名稱用字串，容易拼錯
await Clients.All.SendAsync(""RecieveMessage"", user, message); // 拼錯了！
// 客戶端監聽 ""ReceiveMessage""，永遠收不到！
```

```csharp
// ✅ 使用強型別 Hub（編譯期檢查）
public class ChatHub : Hub<IChatClient>       // 使用介面
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.ReceiveMessage(user, message); // 拼錯會編譯失敗！
    }
}
```

> **為什麼？** 字串沒有編譯期檢查，拼錯也不會報錯，但客戶端就是收不到訊息。用強型別 Hub 可以在編譯時就發現錯誤。

### ❌ 錯誤 3：在 Hub 中保存狀態

```csharp
// ❌ 在 Hub 中存使用者列表（Hub 是 Transient 的！）
public class ChatHub : Hub
{
    private List<string> _onlineUsers = new(); // ❌ 每次呼叫都會重建！

    public async Task SendMessage(string user, string message)
    {
        _onlineUsers.Add(user);                // ❌ 加了也白加，下次呼叫就沒了
    }
}
```

```csharp
// ✅ 用 Singleton 服務或靜態欄位管理狀態
public class OnlineUserService
{
    private readonly ConcurrentDictionary<string, string> _users = new(); // 執行緒安全

    public void AddUser(string connectionId, string name) =>
        _users.TryAdd(connectionId, name);      // 新增使用者

    public void RemoveUser(string connectionId) =>
        _users.TryRemove(connectionId, out _);  // 移除使用者

    public List<string> GetAll() =>
        _users.Values.ToList();                 // 取得所有線上使用者
}

// Program.cs 註冊為 Singleton
builder.Services.AddSingleton<OnlineUserService>(); // 全域唯一
```

> **為什麼？** Hub 是 Transient 的，每次方法呼叫都會建立新的 Hub 實例。存在 Hub 欄位中的資料下次呼叫就消失了。
" },

            // ── Chapter 19: 單元測試與整合測試 ──────────────────────
            new() { Id=19, Category="aspnet", Order=10, Level="advanced", Icon="🧪", Title="單元測試與整合測試", Slug="aspnet-testing", IsPublished=true, Content=@"
# 單元測試與整合測試

## 為什麼要寫測試？

想像你蓋了一棟大樓：
- **沒有測試**：住進去才發現水管漏水、電線短路 💥
- **有測試**：蓋好一層就檢查一次，問題早早發現 ✅

測試就是你程式碼的**品質保證書**。

---

## xUnit 基礎

```csharp
// Tests/CalculatorTests.cs
using Xunit;

public class CalculatorTests
{
    // [Fact] → 無參數的單一測試案例
    [Fact]
    public void Add_TwoPositiveNumbers_ReturnsSum()
    {
        // Arrange（準備）
        var calculator = new Calculator();        // 建立受測物件

        // Act（執行）
        var result = calculator.Add(2, 3);        // 呼叫待測方法

        // Assert（驗證）
        Assert.Equal(5, result);                  // 預期結果是 5
    }

    [Fact]
    public void Add_NegativeNumbers_ReturnsCorrectSum()
    {
        // 準備
        var calculator = new Calculator();        // 建立受測物件

        // 執行
        var result = calculator.Add(-1, -2);      // 加兩個負數

        // 驗證
        Assert.Equal(-3, result);                 // 預期結果是 -3
    }

    // [Theory] + [InlineData] → 多組參數的參數化測試
    [Theory]
    [InlineData(1, 1, 2)]                         // 第一組測試資料
    [InlineData(0, 0, 0)]                         // 第二組測試資料
    [InlineData(-1, 1, 0)]                        // 第三組測試資料
    [InlineData(100, 200, 300)]                   // 第四組測試資料
    public void Add_VariousInputs_ReturnsExpected(
        int a, int b, int expected)
    {
        var calculator = new Calculator();        // 準備
        var result = calculator.Add(a, b);        // 執行
        Assert.Equal(expected, result);           // 驗證
    }
}
```

---

## Arrange-Act-Assert 模式

```csharp
// 每個測試都遵循 AAA 模式
[Fact]
public void GetDiscountedPrice_VipCustomer_Returns20PercentOff()
{
    // ═══ Arrange（準備）═══
    var service = new PricingService();           // 建立受測服務
    var product = new Product                     // 建立測試商品
    {
        Name = ""筆電"",                            // 商品名稱
        Price = 10000                              // 原價一萬
    };
    var customer = new Customer                   // 建立 VIP 客戶
    {
        IsVip = true                               // VIP 身份
    };

    // ═══ Act（執行）═══
    var result = service.GetDiscountedPrice(
        product, customer);                        // 計算折扣價

    // ═══ Assert（驗證）═══
    Assert.Equal(8000, result);                    // VIP 打八折 = 8000
}
```

### 常用 Assert 方法

```csharp
// 常用的斷言方法
Assert.Equal(expected, actual);                    // 值相等
Assert.NotEqual(unexpected, actual);               // 值不相等
Assert.True(condition);                            // 條件為真
Assert.False(condition);                           // 條件為假
Assert.Null(obj);                                  // 物件為 null
Assert.NotNull(obj);                               // 物件不為 null
Assert.Contains(""子字串"", fullString);              // 包含子字串
Assert.Empty(collection);                          // 集合為空
Assert.Throws<ArgumentException>(                  // 預期丟出例外
    () => service.DoSomething(null));
Assert.IsType<Product>(result);                    // 型別檢查
```

---

## Moq 模擬框架

```csharp
// 用 Moq 模擬依賴
using Moq;

[Fact]
public void GetProduct_ExistingId_ReturnsProduct()
{
    // Arrange - 建立 Mock 物件
    var mockRepo = new Mock<IProductRepository>();  // 模擬 Repository

    // Setup - 設定模擬行為
    mockRepo.Setup(r => r.GetById(1))              // 當呼叫 GetById(1) 時
        .Returns(new Product                        // 回傳假資料
        {
            Id = 1,                                 // 商品 ID
            Name = ""測試商品"",                       // 商品名稱
            Price = 100                              // 商品價格
        });

    var service = new ProductService(
        mockRepo.Object);                           // 注入 Mock 物件

    // Act
    var result = service.GetProduct(1);             // 呼叫待測方法

    // Assert
    Assert.NotNull(result);                         // 結果不為 null
    Assert.Equal(""測試商品"", result!.Name);          // 名稱正確
    Assert.Equal(100, result.Price);                // 價格正確

    // Verify - 驗證方法被呼叫
    mockRepo.Verify(
        r => r.GetById(1),                          // 確認 GetById 被呼叫
        Times.Once());                              // 而且只呼叫一次
}

[Fact]
public void GetProduct_NonExistingId_ReturnsNull()
{
    var mockRepo = new Mock<IProductRepository>();   // 建立 Mock

    mockRepo.Setup(r => r.GetById(999))             // 當查詢不存在的 ID
        .Returns((Product?)null);                    // 回傳 null

    var service = new ProductService(mockRepo.Object); // 注入 Mock

    var result = service.GetProduct(999);            // 查詢不存在的商品

    Assert.Null(result);                             // 結果應為 null
}
```

---

## 整合測試 WebApplicationFactory

```csharp
// Tests/IntegrationTests/ProductsApiTests.cs
using Microsoft.AspNetCore.Mvc.Testing;

// 整合測試：測試整個 HTTP 管線
public class ProductsApiTests :
    IClassFixture<WebApplicationFactory<Program>>    // 使用測試伺服器
{
    private readonly HttpClient _client;              // HTTP 客戶端

    public ProductsApiTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();             // 建立測試用 HttpClient
    }

    [Fact]
    public async Task GetProducts_ReturnsOk()
    {
        // Act - 發送真實 HTTP 請求
        var response = await _client.GetAsync(
            ""/api/Products"");                         // 呼叫 API

        // Assert - 檢查 HTTP 回應
        response.EnsureSuccessStatusCode();           // 確認 2xx 成功
        Assert.Equal(""application/json"",
            response.Content.Headers
                .ContentType?.MediaType);              // 確認回傳 JSON
    }

    [Fact]
    public async Task GetProduct_InvalidId_Returns404()
    {
        // Act
        var response = await _client.GetAsync(
            ""/api/Products/99999"");                   // 查詢不存在的 ID

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.NotFound,       // 預期 404
            response.StatusCode);                      // 實際狀態碼
    }

    [Fact]
    public async Task CreateProduct_ValidData_Returns201()
    {
        // Arrange
        var newProduct = new
        {
            Name = ""整合測試商品"",                      // 測試商品名稱
            Price = 299                                // 測試價格
        };
        var json = JsonSerializer.Serialize(newProduct); // 序列化為 JSON
        var content = new StringContent(
            json, Encoding.UTF8, ""application/json"");   // 建立請求內容

        // Act
        var response = await _client.PostAsync(
            ""/api/Products"", content);                   // 發送 POST

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Created,          // 預期 201
            response.StatusCode);                        // 實際狀態碼
    }
}
```

---

## 自訂 WebApplicationFactory

```csharp
// Tests/CustomWebApplicationFactory.cs
public class CustomWebApplicationFactory :
    WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 移除正式的 DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<AppDbContext>)); // 找到 DbContext 註冊
            if (descriptor != null)
                services.Remove(descriptor);                 // 移除正式版

            // 換成 In-Memory Database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase(""TestDb"");      // 使用記憶體資料庫
            });
        });
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：測試實作細節而不是行為

```csharp
// ❌ 測試內部實作（檢查私有方法被呼叫幾次）
[Fact]
public void CalculateTotal_ChecksInternalCounter()
{
    var service = new OrderService();                 // 建立服務
    service.CalculateTotal(items);                    // 計算
    Assert.Equal(3, service._internalCounter);        // ❌ 測試內部狀態！
}
```

```csharp
// ✅ 測試行為和結果
[Fact]
public void CalculateTotal_ThreeItems_ReturnsCorrectSum()
{
    var service = new OrderService();                 // 建立服務
    var items = new List<OrderItem>                   // 建立測試項目
    {
        new() { Price = 100, Quantity = 2 },          // 200
        new() { Price = 50, Quantity = 1 }            // 50
    };

    var total = service.CalculateTotal(items);        // 計算總價

    Assert.Equal(250, total);                         // ✅ 只關心結果是否正確
}
```

> **為什麼？** 測試內部實作會讓重構變得困難——一改內部邏輯測試就壞。應該測試「輸入什麼、期望什麼輸出」，這樣重構時只要行為不變，測試就不用改。

### ❌ 錯誤 2：測試之間互相依賴

```csharp
// ❌ 測試 B 依賴測試 A 的結果（測試順序不保證！）
private static int _createdId;                       // 共享狀態！

[Fact]
public void Test_A_CreateProduct()
{
    _createdId = _service.Create(dto).Id;            // ❌ 存到靜態變數
    Assert.True(_createdId > 0);
}

[Fact]
public void Test_B_GetProduct()
{
    var product = _service.GetById(_createdId);      // ❌ 依賴 Test_A 的結果
    Assert.NotNull(product);                         // 如果 A 沒先跑，這裡會失敗！
}
```

```csharp
// ✅ 每個測試獨立，自己準備資料
[Fact]
public void GetProduct_ExistingId_ReturnsProduct()
{
    // 每個測試自己準備需要的資料
    var created = _service.Create(
        new CreateProductDto { Name = ""測試"", Price = 100 }); // 自己建立

    var result = _service.GetById(created.Id);       // 用自己建立的 ID 查詢

    Assert.NotNull(result);                          // 驗證結果
    Assert.Equal(""測試"", result!.Name);              // 不依賴其他測試
}
```

> **為什麼？** xUnit 不保證測試執行順序，而且可能平行執行。每個測試必須獨立，自己準備（Arrange）需要的資料。

### ❌ 錯誤 3：Mock 設定太多導致測試脆弱

```csharp
// ❌ Mock 了太多細節，隨便改一點就壞
[Fact]
public void ProcessOrder_MockEverything()
{
    var mockRepo = new Mock<IOrderRepository>();
    var mockEmail = new Mock<IEmailService>();
    var mockLogger = new Mock<ILogger<OrderService>>();
    var mockCache = new Mock<ICacheService>();
    var mockConfig = new Mock<IConfiguration>();

    // 設定了一堆 Setup...
    mockRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns(new Order());
    mockEmail.Setup(e => e.Send(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
    mockCache.Setup(c => c.Get(It.IsAny<string>())).Returns((string?)null);
    // ... 設定越多越脆弱
}
```

```csharp
// ✅ 只 Mock 必要的依賴
[Fact]
public void ProcessOrder_ValidOrder_SendsConfirmationEmail()
{
    // 只 Mock 真正需要驗證的依賴
    var mockEmail = new Mock<IEmailService>();         // 只 Mock 郵件服務
    var service = new OrderService(
        new FakeOrderRepository(),                     // 用 Fake 取代 Mock
        mockEmail.Object);                             // 注入 Mock

    service.ProcessOrder(1);                           // 執行

    // 只驗證我們關心的行為
    mockEmail.Verify(
        e => e.Send(
            It.IsAny<string>(),                        // 任意收件人
            It.Is<string>(s => s.Contains(""訂單確認""))), // 郵件包含「訂單確認」
        Times.Once());                                 // 只寄一次
}
```

> **為什麼？** Mock 太多會讓測試變得脆弱又難維護。每個測試應該只 Mock 與該測試案例相關的依賴，用 Fake（假實作）取代不需要驗證的部分。
" }
        };

        return chapters;
    }
}
