using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_AspNetExtra
{
    public static List<Chapter> GetChapters()
    {
        var chapters = new List<Chapter>
        {
            // ── Chapter 120: Minimal API ──────────────────────────
            new() { Id=120, Category="aspnet", Order=11, Level="intermediate", Icon="⚡", Title="Minimal API", Slug="aspnet-minimal-api", IsPublished=true, Content=@"
# ⚡ Minimal API

## 📌 什麼是 Minimal API？

Minimal API 是 ASP.NET Core 6 引入的一種**輕量級 API 開發方式**，不需要 Controller、不需要一堆檔案，直接在 `Program.cs` 裡就能定義 API 端點。

想像你經營兩種餐廳：
- **Controller-based API** 像是大型連鎖餐廳——有經理（Controller）、服務生（Action Method）、菜單系統（Routing），分工明確但架構龐大
- **Minimal API** 像是路邊攤——老闆一個人搞定點餐和出餐，快速、簡單、直接

如果你用過 Node.js 的 **Express.js**，Minimal API 的風格會讓你感到非常熟悉！

---

## 🚀 基本用法：MapGet / MapPost / MapPut / MapDelete

```csharp
// Program.cs - 這就是你的整個 API！
var builder = WebApplication.CreateBuilder(args); // 建立應用程式建構器
var app = builder.Build(); // 建構應用程式

// GET：取得資料（像點菜單上的餐點）
app.MapGet(""/api/hello"", () => ""你好，世界！""); // 最簡單的 GET 端點

// GET：取得所有商品
app.MapGet(""/api/products"", () =>
{
    // 回傳商品清單（實際上會從資料庫取得）
    var products = new[]
    {
        new { Id = 1, Name = ""筆電"", Price = 30000 },   // 第一個商品
        new { Id = 2, Name = ""滑鼠"", Price = 500 },     // 第二個商品
        new { Id = 3, Name = ""鍵盤"", Price = 2000 }     // 第三個商品
    };
    return Results.Ok(products); // 回傳 200 OK 和商品清單
});

// GET：根據 ID 取得單一商品
app.MapGet(""/api/products/{id}"", (int id) =>
{
    // 用 id 去查找商品（這裡用假資料示範）
    if (id == 1) // 如果找到了
        return Results.Ok(new { Id = 1, Name = ""筆電"", Price = 30000 }); // 回傳商品
    return Results.NotFound(new { Message = $""找不到 ID 為 {id} 的商品"" }); // 回傳 404
});

// POST：建立新資料（像填寫點餐單送到廚房）
app.MapPost(""/api/products"", (Product product) =>
{
    // product 參數會自動從 Request Body 的 JSON 反序列化
    Console.WriteLine($""收到新商品：{product.Name}""); // 印出商品名稱
    return Results.Created($""/api/products/{product.Id}"", product); // 回傳 201 Created
});

// PUT：更新資料（像修改已經送出的訂單）
app.MapPut(""/api/products/{id}"", (int id, Product product) =>
{
    // id 從路由參數來，product 從 body 來
    Console.WriteLine($""更新商品 {id}：{product.Name}""); // 印出更新資訊
    return Results.NoContent(); // 回傳 204 No Content（更新成功，不需要回傳內容）
});

// DELETE：刪除資料（像取消訂單）
app.MapDelete(""/api/products/{id}"", (int id) =>
{
    Console.WriteLine($""刪除商品 {id}""); // 印出刪除資訊
    return Results.NoContent(); // 回傳 204 No Content
});

app.Run(); // 啟動應用程式
```

---

## 🔗 參數繫結：資料從哪裡來？

```csharp
// [FromQuery]：從 URL 的查詢字串取得（像在網址列輸入搜尋條件）
// 請求：GET /api/search?keyword=筆電&page=1
app.MapGet(""/api/search"", ([FromQuery] string keyword, [FromQuery] int page) =>
{
    // keyword = ""筆電""，page = 1（自動從 URL 取得）
    return Results.Ok(new { Keyword = keyword, Page = page }); // 回傳搜尋條件
});

// [FromRoute]：從路由取得（像從地址中取出門牌號碼）
// 請求：GET /api/users/42
app.MapGet(""/api/users/{userId}"", ([FromRoute] int userId) =>
{
    return Results.Ok(new { UserId = userId }); // 回傳使用者 ID
});

// [FromBody]：從請求主體取得（像打開包裹取出裡面的東西）
// 請求：POST /api/orders，Body 是 JSON
app.MapPost(""/api/orders"", ([FromBody] Order order) =>
{
    // order 物件會自動從 JSON 反序列化
    return Results.Created($""/api/orders/{order.Id}"", order); // 回傳建立結果
});

// [FromHeader]：從 HTTP 標頭取得（像看信封上的寄件人資訊）
app.MapGet(""/api/protected"", ([FromHeader(Name = ""X-Api-Key"")] string apiKey) =>
{
    if (apiKey != ""my-secret-key"") // 驗證 API Key
        return Results.Unauthorized(); // 未授權
    return Results.Ok(""歡迎！""); // 授權成功
});
```

---

## 📦 使用 MapGroup 分組

```csharp
// MapGroup 讓你把相關的 API 端點分組（像把同一類的菜放在菜單的同一頁）
var productGroup = app.MapGroup(""/api/products""); // 建立 /api/products 分組

// 以下所有路由都會自動加上 /api/products 前綴
productGroup.MapGet(""/"", () => Results.Ok(""取得所有商品"")); // GET /api/products/
productGroup.MapGet(""/{id}"", (int id) => Results.Ok($""取得商品 {id}"")); // GET /api/products/{id}
productGroup.MapPost(""/"", (Product p) => Results.Created($""/api/products/{p.Id}"", p)); // POST /api/products/
productGroup.MapDelete(""/{id}"", (int id) => Results.NoContent()); // DELETE /api/products/{id}

// 巢狀分組（像菜單裡的子分類）
var adminGroup = app.MapGroup(""/api/admin"") // 管理員 API 分組
    .RequireAuthorization(); // 這個分組下的所有端點都需要授權

adminGroup.MapGet(""/users"", () => Results.Ok(""管理員：取得所有使用者"")); // 需要授權才能存取
adminGroup.MapDelete(""/users/{id}"", (int id) => Results.NoContent()); // 需要授權才能刪除
```

---

## 🔧 Minimal API 的 Filters

```csharp
// 端點篩選器（像餐廳門口的安檢，進去前先檢查一下）
app.MapGet(""/api/items"", () => Results.Ok(""通過檢查！""))
    .AddEndpointFilter(async (context, next) =>
    {
        // 在端點執行「之前」做的事
        Console.WriteLine(""進入端點前...\n""); // 記錄日誌

        var result = await next(context); // 執行端點處理（像放行讓客人進去）

        // 在端點執行「之後」做的事
        Console.WriteLine(""離開端點後...\n""); // 記錄日誌

        return result; // 回傳結果
    });

// 自訂驗證篩選器（像門口的保鏢，檢查你的證件）
app.MapPost(""/api/items"", (Item item) => Results.Ok(item))
    .AddEndpointFilter(async (context, next) =>
    {
        var item = context.GetArgument<Item>(0); // 取得第一個參數
        if (string.IsNullOrEmpty(item.Name)) // 如果名稱為空
        {
            return Results.BadRequest(""商品名稱不可為空""); // 回傳 400 錯誤
        }
        return await next(context); // 驗證通過，繼續執行
    });
```

---

## 📊 Minimal API vs Controller-based API 比較

| 項目 | Minimal API | Controller-based API |
|------|------------|---------------------|
| 程式碼量 | 少，適合小型 API | 多，但結構清晰 |
| 學習曲線 | 低，快速上手 | 較高，需理解 MVC 模式 |
| 檔案結構 | 可以全部寫在 Program.cs | 需要 Controller 資料夾和檔案 |
| 適用場景 | 微服務、小型 API、原型 | 大型企業應用、複雜 API |
| 模型驗證 | 需手動或用 Filter | 內建 [ApiController] 自動驗證 |
| Swagger | 支援，但需額外設定 | 自動整合 |
| 可測試性 | 可測試，但需要技巧 | 容易透過 DI 測試 |

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：沒有做輸入驗證

```csharp
// ❌ 錯誤寫法：直接信任使用者輸入（像不檢查就讓所有人進門）
app.MapPost(""/api/users"", (User user) =>
{
    // 沒有驗證 user 的內容就直接存入資料庫
    db.Users.Add(user); // 如果 user.Name 是 null 呢？如果 email 格式不對呢？
    db.SaveChanges(); // 存入垃圾資料！
    return Results.Created($""/api/users/{user.Id}"", user); // 回傳
});
```

```csharp
// ✅ 正確寫法：先驗證再處理
app.MapPost(""/api/users"", (User user) =>
{
    // 驗證必填欄位
    if (string.IsNullOrWhiteSpace(user.Name)) // 名稱不可為空
        return Results.BadRequest(new { Error = ""名稱為必填"" }); // 回傳 400

    if (string.IsNullOrWhiteSpace(user.Email)) // Email 不可為空
        return Results.BadRequest(new { Error = ""Email 為必填"" }); // 回傳 400

    // 驗證通過才存入
    db.Users.Add(user); // 存入資料庫
    db.SaveChanges(); // 儲存變更
    return Results.Created($""/api/users/{user.Id}"", user); // 回傳 201
});
```

**解釋：** 不驗證輸入就像不鎖門就出門——遲早會出問題。使用者可能送來空值、超長字串、或惡意內容，永遠不要信任來自外部的資料。

### ❌ 錯誤 2：沒有處理例外

```csharp
// ❌ 錯誤寫法：沒有 try-catch（像在高速公路上不繫安全帶）
app.MapGet(""/api/data/{id}"", (int id) =>
{
    var data = db.Items.Find(id); // 如果資料庫連線失敗？
    return Results.Ok(data); // data 可能是 null！
});
```

```csharp
// ✅ 正確寫法：處理各種可能的錯誤情況
app.MapGet(""/api/data/{id}"", (int id) =>
{
    try
    {
        var data = db.Items.Find(id); // 嘗試查找資料
        if (data == null) // 資料不存在
            return Results.NotFound(new { Error = $""找不到 ID={id} 的資料"" }); // 404
        return Results.Ok(data); // 200 OK
    }
    catch (Exception ex) // 捕捉所有例外
    {
        return Results.Problem($""伺服器錯誤：{ex.Message}""); // 回傳 500
    }
});
```

**解釋：** API 是對外的窗口，任何未處理的例外都會導致回傳 500 錯誤，還可能洩漏程式內部資訊。就像餐廳廚房失火了，不能直接讓客人看到火焰，要先處理好再告知客人「抱歉，暫時無法供餐」。
" },

            // ── Chapter 121: Razor Pages ──────────────────────────
            new() { Id=121, Category="aspnet", Order=12, Level="beginner", Icon="📑", Title="Razor Pages", Slug="aspnet-razor-pages", IsPublished=true, Content=@"
# 📑 Razor Pages

## 📌 什麼是 Razor Pages？

Razor Pages 是 ASP.NET Core 提供的一種**以頁面為中心**的開發模式，比 MVC 更簡單直觀。

想像你在蓋房子：
- **MVC 模式** 像是找三個不同的工匠——一個畫設計圖（View）、一個管工地（Controller）、一個準備材料（Model），分工合作
- **Razor Pages** 像是一個全能工匠——每一面牆（頁面）都由一個人負責設計和施工，簡單的案子效率更高

Razor Pages 特別適合**以頁面為主**的網站（如部落格、表單頁面、報表頁面），不需要 Controller 的額外層級。

---

## 🏗️ Razor Pages 基本結構

```
Pages/                          # 所有頁面都放在 Pages 資料夾
├── Index.cshtml               # 首頁的 HTML 模板（像牆壁的外觀）
├── Index.cshtml.cs            # 首頁的邏輯程式碼（像牆壁的內部結構）
├── About.cshtml               # 關於頁面
├── About.cshtml.cs            # 關於頁面的邏輯
├── Products/                  # 子資料夾 = URL 路徑的一部分
│   ├── Index.cshtml           # /Products 頁面
│   ├── Details.cshtml         # /Products/Details 頁面
│   └── Create.cshtml          # /Products/Create 頁面
└── Shared/                    # 共用元件
    └── _Layout.cshtml         # 版面配置（像房子的骨架）
```

---

## 📄 PageModel 類別

```csharp
// Pages/Products/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;           // 引用 MVC 命名空間
using Microsoft.AspNetCore.Mvc.RazorPages; // 引用 Razor Pages 命名空間

public class IndexModel : PageModel // 繼承 PageModel 基底類別
{
    // 屬性：提供資料給頁面（像準備好的食材放在工作台上）
    public List<Product> Products { get; set; } = new(); // 商品清單

    public string Message { get; set; } = """"; // 顯示訊息

    // OnGet：處理 GET 請求（使用者進入頁面時執行）
    public void OnGet() // 當使用者瀏覽此頁面時
    {
        Message = ""歡迎來到商品列表！""; // 設定訊息
        Products = new List<Product>       // 準備商品資料
        {
            new Product { Id = 1, Name = ""筆電"", Price = 30000 },  // 第一個商品
            new Product { Id = 2, Name = ""手機"", Price = 25000 },  // 第二個商品
            new Product { Id = 3, Name = ""平板"", Price = 18000 }   // 第三個商品
        };
    }

    // OnPost：處理 POST 請求（使用者提交表單時執行）
    public IActionResult OnPost() // 當使用者送出表單時
    {
        if (!ModelState.IsValid) // 如果資料驗證失敗
        {
            return Page(); // 回到同一頁，顯示錯誤訊息
        }
        // 處理表單資料...
        return RedirectToPage(""./Index""); // 重新導向到清單頁面
    }
}
```

---

## 🎨 Razor 頁面模板 (.cshtml)

```html
@* Pages/Products/Index.cshtml *@
@page                                    @* 這行超重要！標記此檔案是 Razor Page *@
@model IndexModel                        @* 綁定對應的 PageModel 類別 *@

<h1>@Model.Message</h1>                @* 顯示 PageModel 中的 Message 屬性 *@

@* 用表格顯示商品清單 *@
<table class=""table"">
    <thead>
        <tr>
            <th>ID</th>
            <th>商品名稱</th>
            <th>價格</th>
            <th>操作</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var product in Model.Products) @* 走訪每個商品 *@
        {
            <tr>
                <td>@product.Id</td>         @* 顯示商品 ID *@
                <td>@product.Name</td>       @* 顯示商品名稱 *@
                <td>@product.Price 元</td>   @* 顯示商品價格 *@
                <td>
                    @* asp-page 指向另一個 Razor Page *@
                    <a asp-page=""./Details""
                       asp-route-id=""@product.Id"">
                        詳細資料
                    </a>
                </td>
            </tr>
        }
    </tbody>
</table>

@* 新增商品的表單 *@
<form method=""post"">                    @* POST 表單會觸發 OnPost 方法 *@
    <div>
        <label>商品名稱</label>
        <input asp-for=""NewProduct.Name"" /> @* 綁定到 PageModel 的屬性 *@
    </div>
    <button type=""submit"">新增</button>  @* 送出按鈕 *@
</form>
```

---

## 🔄 OnGet 與 OnPost 處理器

```csharp
// Pages/Contact.cshtml.cs - 聯絡表單範例
public class ContactModel : PageModel // 聯絡頁面的 PageModel
{
    [BindProperty] // 自動將表單資料綁定到此屬性（像自動拆信取出內容）
    public ContactForm ContactForm { get; set; } = new(); // 表單資料

    public string SuccessMessage { get; set; } = """"; // 成功訊息

    // 處理 GET 請求：顯示空白表單
    public void OnGet() // 使用者開啟頁面時
    {
        // 不需要做什麼，顯示空白表單即可
    }

    // 處理 POST 請求：接收表單資料
    public IActionResult OnPost() // 使用者提交表單時
    {
        if (!ModelState.IsValid) // 驗證是否通過
        {
            return Page(); // 驗證失敗，回到同一頁顯示錯誤
        }

        // 處理表單資料（例如寄送 Email）
        SuccessMessage = $""感謝 {ContactForm.Name}，我們已收到您的訊息！""; // 設定成功訊息
        return Page(); // 回到同一頁顯示成功訊息
    }

    // 也可以有多個 POST Handler（像一個頁面有多個按鈕）
    public IActionResult OnPostDelete(int id) // 當按下「刪除」按鈕時
    {
        // 刪除指定的資料
        Console.WriteLine($""刪除 ID：{id}""); // 記錄刪除操作
        return RedirectToPage(); // 重新導向回同一頁
    }

    // 非同步版本
    public async Task<IActionResult> OnPostAsync() // 非同步的 POST 處理器
    {
        if (!ModelState.IsValid) // 驗證
            return Page(); // 失敗就回到頁面

        await SaveToDatabase(ContactForm); // 非同步存入資料庫
        return RedirectToPage(""./ThankYou""); // 導向感謝頁面
    }
}
```

---

## 🏷️ @page 指令與 asp-page Tag Helper

```csharp
// @page 指令可以自訂路由模板
// Pages/Products/Details.cshtml
@page ""{id:int}""    // URL 為 /Products/Details/5，id 必須是整數
@model DetailsModel  // 綁定 PageModel
```

```csharp
// Pages/Products/Details.cshtml.cs
public class DetailsModel : PageModel // 商品詳情的 PageModel
{
    public Product Product { get; set; } = new(); // 商品資料

    public void OnGet(int id) // id 會自動從路由參數取得
    {
        // 根據 id 查詢商品
        Product = GetProductById(id); // 從資料庫取得商品
    }
}
```

```html
@* 使用 asp-page 在頁面之間導航（像建築物裡的門，連接不同房間） *@

@* 連結到同一層的頁面 *@
<a asp-page=""./Create"">新增商品</a>          @* 連到 /Products/Create *@

@* 帶路由參數的連結 *@
<a asp-page=""./Details""
   asp-route-id=""5"">查看商品 5</a>            @* 連到 /Products/Details/5 *@

@* 連結到根層的頁面 *@
<a asp-page=""/Index"">回首頁</a>              @* 連到 / *@

@* 帶查詢字串的連結 *@
<a asp-page=""./Index""
   asp-route-search=""手機"">搜尋手機</a>       @* 連到 /Products?search=手機 *@
```

---

## 📊 Razor Pages vs MVC：何時用哪個？

| 情境 | 推薦方式 | 原因 |
|------|---------|------|
| 簡單的表單頁面 | Razor Pages | 一個檔案搞定，不需要 Controller |
| 部落格或內容網站 | Razor Pages | 以頁面為中心，結構直觀 |
| 複雜的 Web API | MVC (Controller) | Controller 更適合 API 設計 |
| 大型企業應用 | MVC | 更好的關注點分離 |
| 學習 ASP.NET | Razor Pages | 入門更簡單 |
| CRUD 表單 | Razor Pages | 內建 Handler 模式很適合 |

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：忘記加 @page 指令

```html
@* ❌ 錯誤寫法：忘記 @page 指令 *@
@model IndexModel   @* 只寫了 model，沒有 @page *@

<h1>我的頁面</h1>
@* 這個頁面無法被路由到！因為缺少 @page 指令 *@
@* ASP.NET 會把它當成一般的 View，而不是 Razor Page *@
```

```html
@* ✅ 正確寫法：第一行就要有 @page *@
@page                @* 告訴 ASP.NET 這是一個 Razor Page *@
@model IndexModel    @* 綁定 PageModel *@

<h1>我的頁面</h1>
@* 現在可以透過 URL 正確存取了 *@
```

**解釋：** `@page` 就像門牌號碼——沒有門牌，郵差（路由系統）就找不到你家（頁面）。忘記加 `@page` 是 Razor Pages 最常見的錯誤，頁面會變成一般的 View，只能被 Controller 引用，無法直接透過 URL 存取。

### ❌ 錯誤 2：沒有使用 [BindProperty]

```csharp
// ❌ 錯誤寫法：POST 表單的資料沒有綁定
public class CreateModel : PageModel // 新增頁面
{
    public Product Product { get; set; } = new(); // 沒有加 [BindProperty]

    public IActionResult OnPost() // 接收表單
    {
        // Product 永遠是空的！因為沒有 [BindProperty]
        Console.WriteLine(Product.Name); // 永遠是 """"（空字串）
        return Page(); // 回到頁面
    }
}
```

```csharp
// ✅ 正確寫法：用 [BindProperty] 標記要綁定的屬性
public class CreateModel : PageModel // 新增頁面
{
    [BindProperty] // 告訴 ASP.NET 自動將表單資料填入這個屬性
    public Product Product { get; set; } = new(); // 會自動接收表單資料

    public IActionResult OnPost() // 接收表單
    {
        // Product 已經自動填入表單的資料了
        Console.WriteLine(Product.Name); // 正確取得使用者輸入的名稱
        return Page(); // 回到頁面
    }
}
```

**解釋：** `[BindProperty]` 就像自動拆信機——沒有它，信（表單資料）送到了但沒人拆開來看，所以你的程式什麼都收不到。對於 POST 表單來說，`[BindProperty]` 是必要的。

### ❌ 錯誤 3：GET 請求用 [BindProperty] 卻沒設定 SupportsGet

```csharp
// ❌ 錯誤寫法：想在 GET 請求中取得查詢字串參數
public class SearchModel : PageModel // 搜尋頁面
{
    [BindProperty] // 預設只在 POST 時綁定
    public string Keyword { get; set; } = """"; // GET 請求時永遠是空的

    public void OnGet() // GET /Search?Keyword=手機
    {
        // Keyword 仍然是空字串！[BindProperty] 預設不支援 GET
    }
}
```

```csharp
// ✅ 正確寫法：加上 SupportsGet = true
public class SearchModel : PageModel // 搜尋頁面
{
    [BindProperty(SupportsGet = true)] // 明確允許 GET 請求的繫結
    public string Keyword { get; set; } = """"; // 現在 GET 時也能取得了

    public void OnGet() // GET /Search?Keyword=手機
    {
        // Keyword = ""手機""，正確取得查詢字串參數！
    }
}
```

**解釋：** 預設情況下，`[BindProperty]` 只處理 POST 請求。如果你想在 GET 請求中也能自動綁定查詢字串參數，必須加上 `SupportsGet = true`。這是一個安全設計，避免 GET 請求意外修改了資料。
" },

            // ── Chapter 122: Filters 與 Action 篩選器 ──────────────────────────
            new() { Id=122, Category="aspnet", Order=13, Level="intermediate", Icon="🔍", Title="Filters 與 Action 篩選器", Slug="aspnet-filters", IsPublished=true, Content=@"
# 🔍 Filters 與 Action 篩選器

## 📌 什麼是 Filter？

Filter（篩選器）就像**機場安檢**——在你的 Action 方法執行前後，自動幫你做一些額外的工作。

想像搭飛機的流程：
1. **Authorization Filter** = 護照檢查（你有沒有資格搭這班飛機？）
2. **Resource Filter** = 行李寄放櫃檯（可以快取，不用每次都重新打包）
3. **Action Filter** = 安檢門（檢查你帶了什麼，出來時再檢查一次）
4. **Exception Filter** = 緊急處理中心（出事了！在這裡統一處理）
5. **Result Filter** = 登機門檢查（最後確認一切沒問題）

---

## 📋 Filter 的五種類型

```
請求進來
    │
    ▼
┌─────────────────────┐
│ Authorization Filter │ ← 第 1 關：有沒有權限？
└──────────┬──────────┘
           ▼
┌─────────────────────┐
│   Resource Filter    │ ← 第 2 關：快取？資源處理？
│   (OnExecuting)      │
└──────────┬──────────┘
           ▼
┌─────────────────────┐
│   Action Filter      │ ← 第 3 關：Action 前檢查
│   (OnExecuting)      │
└──────────┬──────────┘
           ▼
┌─────────────────────┐
│   Action 方法執行     │ ← 你寫的程式碼在這裡執行
└──────────┬──────────┘
           ▼
┌─────────────────────┐
│   Action Filter      │ ← 第 3 關：Action 後檢查
│   (OnExecuted)       │
└──────────┬──────────┘
           ▼
┌─────────────────────┐
│   Result Filter      │ ← 第 5 關：結果處理
└──────────┬──────────┘
           ▼
      回傳結果
```

> 如果中間有 **Exception Filter**，任何一關出錯都會被它接住。

---

## ⚙️ IActionFilter 與 IAsyncActionFilter

```csharp
// 同步版本：IActionFilter
public class LogActionFilter : IActionFilter // 實作 IActionFilter 介面
{
    private readonly ILogger<LogActionFilter> _logger; // 注入日誌服務

    public LogActionFilter(ILogger<LogActionFilter> logger) // 建構函式
    {
        _logger = logger; // 儲存日誌服務
    }

    // Action 執行「前」呼叫（像安檢門前的金屬探測器）
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName; // 取得 Action 名稱
        _logger.LogInformation($""開始執行：{actionName}""); // 記錄開始時間
    }

    // Action 執行「後」呼叫（像離開安檢門時的檢查）
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName; // 取得 Action 名稱
        if (context.Exception != null) // 如果有例外發生
        {
            _logger.LogError($""執行 {actionName} 時發生錯誤""); // 記錄錯誤
        }
        else
        {
            _logger.LogInformation($""完成執行：{actionName}""); // 記錄完成
        }
    }
}

// 非同步版本：IAsyncActionFilter（推薦使用）
public class AsyncLogFilter : IAsyncActionFilter // 實作非同步介面
{
    private readonly ILogger<AsyncLogFilter> _logger; // 注入日誌服務

    public AsyncLogFilter(ILogger<AsyncLogFilter> logger) // 建構函式
    {
        _logger = logger; // 儲存日誌服務
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,   // 執行前的上下文
        ActionExecutionDelegate next)     // 代表下一步（執行 Action）
    {
        // === 在 Action 執行「前」做的事 ===
        _logger.LogInformation(""Action 即將執行""); // 記錄日誌
        var stopwatch = System.Diagnostics.Stopwatch.StartNew(); // 開始計時

        var resultContext = await next(); // 執行 Action（像按下開始鍵）

        // === 在 Action 執行「後」做的事 ===
        stopwatch.Stop(); // 停止計時
        _logger.LogInformation($""Action 執行完成，花費 {stopwatch.ElapsedMilliseconds} ms"");
    }
}
```

---

## 🛠️ 實用範例：效能計時 Filter

```csharp
// 計時 Filter：測量每個 API 端點的執行時間（像比賽用的計時器）
public class PerformanceFilter : IAsyncActionFilter // 非同步 Action Filter
{
    private readonly ILogger<PerformanceFilter> _logger; // 日誌服務

    public PerformanceFilter(ILogger<PerformanceFilter> logger) // 建構函式注入
    {
        _logger = logger; // 儲存日誌服務
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, // 執行前上下文
        ActionExecutionDelegate next)  // 下一步
    {
        var actionName = context.ActionDescriptor.DisplayName; // 取得 Action 名稱
        var stopwatch = System.Diagnostics.Stopwatch.StartNew(); // 按下碼表

        var result = await next(); // 執行 Action

        stopwatch.Stop(); // 停止碼表
        var elapsed = stopwatch.ElapsedMilliseconds; // 取得經過時間

        if (elapsed > 500) // 如果超過 500 毫秒（太慢了！）
        {
            _logger.LogWarning($""⚠️ 慢速 Action：{actionName} 花了 {elapsed}ms""); // 警告
        }
        else
        {
            _logger.LogInformation($""✅ {actionName} 完成，{elapsed}ms""); // 正常記錄
        }
    }
}
```

---

## 🎯 Filter 的套用層級

```csharp
// 層級 1：全域 Filter（所有 Controller 的所有 Action 都會經過）
// 在 Program.cs 中註冊
builder.Services.AddControllers(options =>
{
    options.Filters.Add<PerformanceFilter>(); // 全域套用效能計時 Filter
    options.Filters.Add(new RequireHttpsAttribute()); // 全域要求 HTTPS
});

// 層級 2：Controller 級 Filter（該 Controller 的所有 Action）
[ServiceFilter(typeof(LogActionFilter))] // 整個 Controller 套用日誌 Filter
public class ProductsController : Controller // 商品控制器
{
    // 這裡的所有 Action 都會被 LogActionFilter 過濾
    public IActionResult Index() // 清單頁面
    {
        return View(); // 回傳視圖
    }

    public IActionResult Details(int id) // 詳情頁面
    {
        return View(); // 回傳視圖
    }
}

// 層級 3：Action 級 Filter（只影響單一 Action）
public class OrdersController : Controller // 訂單控制器
{
    [ServiceFilter(typeof(PerformanceFilter))] // 只有這個 Action 套用計時 Filter
    public IActionResult GetReport() // 取得報表（可能很慢，需要計時）
    {
        return View(); // 回傳視圖
    }

    public IActionResult Index() // 這個 Action 不會被 PerformanceFilter 過濾
    {
        return View(); // 回傳視圖
    }
}
```

---

## 🔧 [ServiceFilter] 與 [TypeFilter]

```csharp
// [ServiceFilter]：Filter 從 DI 容器取得（需要先在 Program.cs 註冊）
// 步驟 1：在 Program.cs 註冊 Filter
builder.Services.AddScoped<LogActionFilter>(); // 註冊到 DI 容器

// 步驟 2：用 [ServiceFilter] 使用
[ServiceFilter(typeof(LogActionFilter))] // 從 DI 容器取得 Filter 實例
public class MyController : Controller { } // 控制器

// [TypeFilter]：不需要先在 DI 中註冊，還能傳遞額外參數
[TypeFilter(typeof(CustomFilter), Arguments = new object[] { ""特殊參數"" })]
public class AnotherController : Controller { } // 可以傳參數的 Filter

// CustomFilter 可以接收建構函式參數
public class CustomFilter : IActionFilter // 自訂 Filter
{
    private readonly string _prefix; // 前綴參數
    private readonly ILogger<CustomFilter> _logger; // 注入的日誌服務

    public CustomFilter(string prefix, ILogger<CustomFilter> logger)
    {
        _prefix = prefix;   // 從 [TypeFilter] 的 Arguments 傳入
        _logger = logger;   // 從 DI 容器自動注入
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($""[{_prefix}] Action 開始""); // 使用前綴記錄
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($""[{_prefix}] Action 完成""); // 使用前綴記錄
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：搞錯 Filter 的執行順序

```csharp
// ❌ 錯誤理解：以為 Filter 只有 ""先進先出""
// 實際上 Filter 的執行順序像洋蔥——一層一層包裹，執行時先進後出

// 全域 Filter A (OnExecuting) → 進入
//   Controller Filter B (OnExecuting) → 進入
//     Action Filter C (OnExecuting) → 進入
//       === Action 執行 ===
//     Action Filter C (OnExecuted) → 離開
//   Controller Filter B (OnExecuted) → 離開
// 全域 Filter A (OnExecuted) → 離開
```

```csharp
// ✅ 正確理解：用 Order 屬性控制順序（數字越小越先執行）
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FilterA>(1); // Order=1，最先執行 OnExecuting，最後執行 OnExecuted
    options.Filters.Add<FilterB>(2); // Order=2，第二個執行
    options.Filters.Add<FilterC>(3); // Order=3，最後執行 OnExecuting，最先執行 OnExecuted
});
```

**解釋：** Filter 的執行順序像俄羅斯套娃——進去時從外到內（OnExecuting），出來時從內到外（OnExecuted）。如果你以為 A 先執行完才輪到 B，就會搞混執行順序。

### ❌ 錯誤 2：在 Filter 中產生副作用

```csharp
// ❌ 錯誤寫法：在 Filter 中修改資料庫（Filter 應該做的是「檢查」而非「修改」）
public class BadFilter : IActionFilter // 不好的 Filter
{
    private readonly MyDbContext _db; // 資料庫上下文

    public BadFilter(MyDbContext db) { _db = db; } // 注入資料庫

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // 在 Filter 裡直接修改資料庫——這不是 Filter 該做的事！
        _db.Logs.Add(new Log { Message = ""有人來了"" }); // 插入日誌
        _db.SaveChanges(); // 儲存變更（如果這裡失敗會怎樣？）
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
```

```csharp
// ✅ 正確寫法：Filter 只做輕量的檢查和記錄
public class GoodFilter : IActionFilter // 好的 Filter
{
    private readonly ILogger<GoodFilter> _logger; // 只用日誌服務

    public GoodFilter(ILogger<GoodFilter> logger) { _logger = logger; } // 注入日誌

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation(""請求進入""); // 只記錄日誌，不修改資料庫
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
```

**解釋：** Filter 就像機場安檢——安檢員的工作是「檢查」而不是「幫你重新打包行李」。如果在 Filter 裡做太多事（例如修改資料庫），不但違反單一職責原則，還可能因為 Filter 的執行順序導致難以預測的問題。
" },

            // ── Chapter 123: Blazor 基礎入門 ──────────────────────────
            new() { Id=123, Category="aspnet", Order=14, Level="advanced", Icon="🌐", Title="Blazor 基礎入門", Slug="aspnet-blazor-basics", IsPublished=true, Content=@"
# 🌐 Blazor 基礎入門

## 📌 什麼是 Blazor？

Blazor 讓你**用 C# 寫前端**！不用學 JavaScript，也能打造互動式的網頁應用程式。

想像以前蓋網站就像經營一家**中日合作餐廳**：
- 前台（前端）說日文（JavaScript）
- 後台（後端）說中文（C#）
- 兩邊溝通需要翻譯（API 呼叫）

有了 Blazor，就像整間餐廳**都說中文**——前後台無障礙溝通，用同一種語言（C#）搞定一切！

---

## 🔀 Blazor Server vs Blazor WebAssembly

```
Blazor Server（伺服器模式）：
┌──────────┐    SignalR    ┌──────────┐
│  瀏覽器   │ ◄──────────► │   伺服器   │
│ （只負責   │   即時連線    │ （負責所有  │
│  顯示畫面）│              │  邏輯計算） │
└──────────┘              └──────────┘
像遙控電視——遙控器（瀏覽器）按鈕，電視（伺服器）換台

Blazor WebAssembly（瀏覽器模式）：
┌─────────────────────────────┐
│          瀏覽器               │
│  ┌───────┐  ┌────────────┐  │
│  │ .NET  │  │ 你的 C# 程式│  │
│  │Runtime│  │  整個跑在    │  │
│  │       │  │  瀏覽器裡   │  │
│  └───────┘  └────────────┘  │
└─────────────────────────────┘
像離線遊戲——整個程式下載到你的電腦上執行
```

| 項目 | Blazor Server | Blazor WebAssembly |
|------|-------------|-------------------|
| 首次載入 | 快（只下載少量 JS） | 慢（要下載 .NET Runtime） |
| 執行位置 | 伺服器 | 瀏覽器 |
| 離線支援 | 不支援 | 支援 |
| 伺服器負擔 | 高（每個使用者都佔連線） | 低（邏輯在用戶端） |
| 適用場景 | 企業內部系統 | 公開面向使用者的應用 |

---

## 🧩 Component 元件基礎

```csharp
@* Components/Counter.razor - Blazor 元件範例 *@
@* 每個 .razor 檔案就是一個元件（像樂高積木，可以組合使用） *@

<h3>計數器</h3>

<p>目前計數：@currentCount</p>  @* 用 @ 符號顯示 C# 變數 *@

<button class=""btn btn-primary""
        @onclick=""IncrementCount"">  @* 按鈕點擊事件綁定到 C# 方法 *@
    點我加一
</button>

@code {
    // 元件內的 C# 程式碼區塊
    private int currentCount = 0; // 計數變數

    private void IncrementCount() // 按鈕點擊時執行的方法
    {
        currentCount++; // 計數加一
        // Blazor 會自動更新畫面！不需要手動操作 DOM
    }
}
```

---

## 🔄 元件生命週期

```csharp
@* Components/LifecycleDemo.razor - 生命週期範例 *@

@implements IDisposable  @* 實作 IDisposable 以便清理資源 *@

<h3>@Title</h3>
<p>資料：@data</p>

@code {
    [Parameter] // 標記為元件參數（像函式的參數，由父元件傳入）
    public string Title { get; set; } = """"; // 接收父元件傳來的標題

    private string data = ""載入中...""; // 資料狀態

    // 1. OnInitialized：元件初始化時呼叫（像開店前的準備工作）
    protected override void OnInitialized()
    {
        Console.WriteLine(""元件已初始化""); // 只在第一次建立時呼叫
    }

    // 1b. 非同步版本（適合需要呼叫 API 的情況）
    protected override async Task OnInitializedAsync()
    {
        data = await LoadDataFromApi(); // 從 API 載入資料
    }

    // 2. OnParametersSet：參數變更時呼叫（像收到新的訂單就更新菜單）
    protected override void OnParametersSet()
    {
        Console.WriteLine($""參數已更新：Title = {Title}""); // 每次參數改變都會呼叫
    }

    // 3. OnAfterRender：畫面渲染完成後呼叫（像裝潢完成後的驗收）
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender) // 只在第一次渲染後執行
        {
            Console.WriteLine(""第一次渲染完成！""); // 適合做 JS Interop
        }
    }

    // 4. Dispose：元件被移除時呼叫（像關店時的清理工作）
    public void Dispose()
    {
        Console.WriteLine(""元件已銷毀，清理資源""); // 取消訂閱、關閉連線等
    }

    private async Task<string> LoadDataFromApi() // 模擬 API 呼叫
    {
        await Task.Delay(1000); // 模擬延遲 1 秒
        return ""資料載入完成！""; // 回傳資料
    }
}
```

---

## 🔗 @bind 雙向綁定

```csharp
@* Components/BindingDemo.razor - 資料綁定範例 *@

<h3>雙向綁定示範</h3>

@* 雙向綁定：輸入框的值和 C# 變數自動同步（像對講機，兩邊都能講） *@
<input @bind=""userName"" />       @* 當輸入框改變時，userName 自動更新 *@
<p>你好，@userName！</p>         @* userName 改變時，畫面自動更新 *@

@* 指定綁定事件：oninput 表示每打一個字就更新（像即時翻譯） *@
<input @bind=""searchText""
       @bind:event=""oninput"" />  @* 預設是 onchange（失去焦點時才更新） *@
<p>搜尋：@searchText</p>

@* 綁定到不同的資料型別 *@
<input type=""number"" @bind=""quantity"" />  @* 綁定到整數 *@
<input type=""date"" @bind=""selectedDate"" /> @* 綁定到日期 *@
<input type=""checkbox"" @bind=""isChecked"" /> @* 綁定到布林值 *@

@* 下拉選單綁定 *@
<select @bind=""selectedCity"">                  @* 綁定選中的值 *@
    <option value="""">請選擇城市</option>       @* 預設選項 *@
    <option value=""taipei"">台北</option>       @* 選項 1 *@
    <option value=""taichung"">台中</option>     @* 選項 2 *@
    <option value=""kaohsiung"">高雄</option>    @* 選項 3 *@
</select>
<p>你選的城市：@selectedCity</p>

@code {
    private string userName = ""世界""; // 使用者名稱
    private string searchText = """";   // 搜尋文字
    private int quantity = 1;          // 數量
    private DateTime selectedDate = DateTime.Today; // 選擇日期
    private bool isChecked = false;    // 是否勾選
    private string selectedCity = """"; // 選擇的城市
}
```

---

## 📡 EventCallback：父子元件溝通

```csharp
@* Components/ChildButton.razor - 子元件 *@
@* 子元件像一個按鈕零件，按下去會通知父元件 *@

<button class=""btn btn-success""
        @onclick=""OnButtonClick"">   @* 按鈕被點擊時執行 *@
    @ButtonText                     @* 顯示按鈕文字 *@
</button>

@code {
    [Parameter] // 從父元件接收按鈕文字
    public string ButtonText { get; set; } = ""按我""; // 預設文字

    [Parameter] // EventCallback：當事件發生時通知父元件（像對講機呼叫總部）
    public EventCallback<string> OnClicked { get; set; } // 可以傳送字串訊息

    private async Task OnButtonClick() // 按鈕點擊處理
    {
        await OnClicked.InvokeAsync(""子元件被點擊了！""); // 通知父元件，傳送訊息
    }
}
```

```csharp
@* Pages/ParentPage.razor - 父元件 *@
@page ""/parent""

<h3>父元件</h3>
<p>收到的訊息：@message</p>

@* 使用子元件，並監聽事件 *@
<ChildButton ButtonText=""點擊我""
             OnClicked=""HandleChildClick"" />  @* 當子元件觸發事件時呼叫此方法 *@

@code {
    private string message = ""（等待點擊）""; // 訊息狀態

    private void HandleChildClick(string msg) // 處理子元件的事件
    {
        message = msg; // 更新訊息
        // 畫面會自動更新，顯示新的訊息
    }
}
```

---

## 💉 在元件中使用依賴注入

```csharp
@* Components/WeatherDisplay.razor - 使用 DI 的元件 *@
@inject HttpClient Http         @* 注入 HttpClient 服務 *@
@inject ILogger<WeatherDisplay> Logger  @* 注入日誌服務 *@

<h3>天氣預報</h3>

@if (forecasts == null) // 如果資料還沒載入
{
    <p>載入中...</p>    @* 顯示載入提示 *@
}
else
{
    <table class=""table"">
        <thead>
            <tr>
                <th>日期</th>
                <th>溫度 (°C)</th>
                <th>天氣</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var f in forecasts) @* 走訪每筆天氣資料 *@
            {
                <tr>
                    <td>@f.Date.ToShortDateString()</td> @* 顯示日期 *@
                    <td>@f.TemperatureC</td>             @* 顯示溫度 *@
                    <td>@f.Summary</td>                  @* 顯示天氣描述 *@
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private WeatherForecast[]? forecasts; // 天氣資料陣列

    protected override async Task OnInitializedAsync() // 元件初始化時
    {
        try
        {
            Logger.LogInformation(""正在載入天氣資料...""); // 記錄日誌
            forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>(""api/weather"");
            // 從 API 取得天氣資料
        }
        catch (Exception ex) // 如果載入失敗
        {
            Logger.LogError($""載入天氣資料失敗：{ex.Message}""); // 記錄錯誤
            forecasts = Array.Empty<WeatherForecast>(); // 設定為空陣列
        }
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：在 OnInitializedAsync 中阻塞 UI 執行緒

```csharp
@code {
    // ❌ 錯誤寫法：用 .Result 阻塞（像在高速公路上停車等人）
    protected override void OnInitialized()
    {
        var data = Http.GetFromJsonAsync<Data[]>(""api/data"").Result; // 阻塞 UI！
        // 整個頁面會凍結，使用者什麼都不能做
    }
}
```

```csharp
@code {
    // ✅ 正確寫法：使用 async/await（像預約好時間再去取餐）
    protected override async Task OnInitializedAsync()
    {
        var data = await Http.GetFromJsonAsync<Data[]>(""api/data""); // 非阻塞
        // 頁面會先顯示「載入中...」，資料來了再自動更新
    }
}
```

**解釋：** Blazor 的 UI 渲染和你的程式碼跑在同一條線上。如果你用 `.Result` 或 `.Wait()` 阻塞，就像在單線道上停車——所有人（包括畫面更新）都被堵住了。使用 `async/await` 就像設置一個等候區，不影響其他車輛通行。

### ❌ 錯誤 2：沒有清理資源（Dispose）

```csharp
@code {
    // ❌ 錯誤寫法：訂閱了事件但沒有取消訂閱
    private Timer? _timer; // 計時器

    protected override void OnInitialized()
    {
        _timer = new Timer(UpdateTime, null, 0, 1000); // 每秒更新一次
        // 當元件被移除時，Timer 還在跑！記憶體洩漏！
    }
}
```

```csharp
@implements IDisposable  @* 實作 IDisposable *@

@code {
    // ✅ 正確寫法：在 Dispose 中清理資源
    private Timer? _timer; // 計時器

    protected override void OnInitialized()
    {
        _timer = new Timer(UpdateTime, null, 0, 1000); // 每秒更新
    }

    public void Dispose() // 元件被移除時自動呼叫
    {
        _timer?.Dispose(); // 停止並釋放計時器
    }
}
```

**解釋：** 不清理資源就像退房時不還鑰匙——你已經離開了，但鑰匙（Timer、事件訂閱）還在佔用資源。久了就會造成記憶體洩漏（Memory Leak），整個應用程式越來越慢。

### ❌ 錯誤 3：忘記通知 Blazor 更新畫面

```csharp
@code {
    // ❌ 錯誤情境：在非 Blazor 事件中修改狀態（像背景工作完成後）
    private string status = ""等待中""; // 狀態

    protected override void OnInitialized()
    {
        var timer = new System.Threading.Timer(_ =>
        {
            status = ""已更新""; // 修改了變數，但畫面不會自動更新！
            // 因為這不是 Blazor 的事件，Blazor 不知道要重新渲染
        }, null, 3000, Timeout.Infinite);
    }
}
```

```csharp
@code {
    // ✅ 正確寫法：呼叫 StateHasChanged 通知 Blazor
    private string status = ""等待中""; // 狀態

    protected override void OnInitialized()
    {
        var timer = new System.Threading.Timer(_ =>
        {
            InvokeAsync(() =>             // 切回 Blazor 的同步上下文
            {
                status = ""已更新"";       // 修改狀態
                StateHasChanged();        // 通知 Blazor 重新渲染畫面
            });
        }, null, 3000, Timeout.Infinite);
    }
}
```

**解釋：** Blazor 只有在自己的事件（`@onclick`、`OnInitializedAsync` 等）觸發後才會自動更新畫面。如果是外部的 Timer 或背景工作修改了資料，必須用 `InvokeAsync` + `StateHasChanged` 手動通知 Blazor「嘿，資料變了，該重新畫畫面了！」。
" },

            // ── Chapter 124: REST API 設計最佳實踐 ──────────────────────────
            new() { Id=124, Category="aspnet", Order=15, Level="intermediate", Icon="🔗", Title="REST API 設計最佳實踐", Slug="aspnet-rest-api-best-practices", IsPublished=true, Content=@"
# 🔗 REST API 設計最佳實踐

## 📌 什麼是 REST？

REST（Representational State Transfer）是一種設計 API 的**風格**，不是一種技術。它定義了如何透過 HTTP 協議來操作資源。

想像你經營一家**圖書館**：
- **資源（Resource）** = 書本、會員、借閱記錄
- **URL** = 書本的分類編號（告訴你去哪個書架找）
- **HTTP 方法** = 你要做什麼操作（借書、還書、查書）
- **回應** = 圖書館員給你的答覆和書本

---

## 📜 REST 的六大原則

```
1. 客戶端-伺服器（Client-Server）
   → 像餐廳：客人點餐，廚房做菜，分工明確

2. 無狀態（Stateless）
   → 像售票機：每次購票都要重新選擇，不會記住你上次買了什麼

3. 可快取（Cacheable）
   → 像報紙：今天的頭條可以看好幾次，不用每次都重新印

4. 統一介面（Uniform Interface）
   → 像 USB 接口：不管什麼裝置，插口都一樣

5. 分層系統（Layered System）
   → 像大公司的組織架構：經理不需要知道工廠的細節

6. 按需代碼（Code on Demand，選擇性）
   → 像餐廳提供的食譜：客人可以自己回家做，也可以不用
```

---

## 🛤️ URL 命名規範

```csharp
// ✅ 好的 URL 設計（名詞、複數、小寫、用破折號分隔）
// 控制器範例
[Route(""api/[controller]"")] // 基底路由
[ApiController] // 標記為 API 控制器
public class ProductsController : ControllerBase // 商品控制器
{
    // GET /api/products          → 取得所有商品
    [HttpGet] // 對應 GET 方法
    public IActionResult GetAll() => Ok(products); // 回傳所有商品

    // GET /api/products/5        → 取得單一商品
    [HttpGet(""{id}"")] // 路由參數
    public IActionResult GetById(int id) => Ok(product); // 回傳指定商品

    // POST /api/products         → 建立新商品
    [HttpPost] // 對應 POST 方法
    public IActionResult Create(Product p) => Created($""/api/products/{p.Id}"", p);

    // PUT /api/products/5        → 更新整個商品
    [HttpPut(""{id}"")] // 對應 PUT 方法
    public IActionResult Update(int id, Product p) => NoContent(); // 更新商品

    // DELETE /api/products/5     → 刪除商品
    [HttpDelete(""{id}"")] // 對應 DELETE 方法
    public IActionResult Delete(int id) => NoContent(); // 刪除商品
}
```

```
❌ 不好的 URL 設計：
/api/getProducts         → 動詞不該出現在 URL 中
/api/product             → 應該用複數 products
/api/Products            → 應該用小寫 products
/api/delete-product/5    → 用 HTTP DELETE 方法，不要把動詞放在 URL
/api/product_list        → 用破折號，不要用底線

✅ 好的 URL 設計：
/api/products            → 取得所有商品（GET）
/api/products/5          → 取得 ID=5 的商品（GET）
/api/products            → 建立新商品（POST）
/api/products/5          → 更新 ID=5 的商品（PUT）
/api/products/5          → 刪除 ID=5 的商品（DELETE）
/api/products/5/reviews  → 取得商品 5 的所有評論（子資源）
```

---

## 📄 分頁、篩選與排序

```csharp
// API 端點：支援分頁、篩選和排序
[HttpGet] // GET /api/products?page=1&pageSize=10&sort=price&order=desc&category=electronics
public IActionResult GetProducts(
    [FromQuery] int page = 1,         // 第幾頁（預設第 1 頁）
    [FromQuery] int pageSize = 10,    // 每頁幾筆（預設 10 筆）
    [FromQuery] string? sort = null,  // 排序欄位
    [FromQuery] string? order = ""asc"", // 排序方向：asc 或 desc
    [FromQuery] string? category = null) // 篩選條件
{
    var query = _db.Products.AsQueryable(); // 建立可查詢的集合

    // 篩選（像圖書館用分類找書）
    if (!string.IsNullOrEmpty(category)) // 如果有指定分類
    {
        query = query.Where(p => p.Category == category); // 篩選符合的商品
    }

    // 排序（像圖書館按照書名或出版日期排列）
    query = sort?.ToLower() switch
    {
        ""price"" => order == ""desc""
            ? query.OrderByDescending(p => p.Price)  // 價格由高到低
            : query.OrderBy(p => p.Price),           // 價格由低到高
        ""name"" => order == ""desc""
            ? query.OrderByDescending(p => p.Name)   // 名稱 Z 到 A
            : query.OrderBy(p => p.Name),            // 名稱 A 到 Z
        _ => query.OrderBy(p => p.Id)                // 預設用 ID 排序
    };

    // 計算總數（在分頁之前）
    var totalCount = query.Count(); // 總共有幾筆資料

    // 分頁（像一本書分成好幾頁，一次只看一頁）
    var items = query
        .Skip((page - 1) * pageSize) // 跳過前面的資料
        .Take(pageSize)              // 只取這一頁的數量
        .ToList();                   // 執行查詢

    // 回傳分頁資訊
    var result = new
    {
        Data = items,              // 這一頁的資料
        Page = page,               // 目前第幾頁
        PageSize = pageSize,       // 每頁幾筆
        TotalCount = totalCount,   // 總筆數
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize) // 總頁數
    };

    return Ok(result); // 回傳 200 OK
}
```

---

## 🔗 HATEOAS 概念

```csharp
// HATEOAS（Hypermedia As The Engine Of Application State）
// 簡單說：API 回應中包含「接下來可以做什麼」的連結
// 像餐廳菜單上不只有菜名，還有「加點」「套餐升級」的選項

[HttpGet(""{id}"")] // 取得單一商品
public IActionResult GetById(int id)
{
    var product = _db.Products.Find(id); // 查找商品
    if (product == null) // 如果找不到
        return NotFound(); // 回傳 404

    // 回傳資料時附帶相關操作的連結
    var result = new
    {
        Data = product, // 商品資料
        Links = new[]   // 可以執行的操作連結
        {
            new { Rel = ""self"", Href = $""/api/products/{id}"", Method = ""GET"" },
            new { Rel = ""update"", Href = $""/api/products/{id}"", Method = ""PUT"" },
            new { Rel = ""delete"", Href = $""/api/products/{id}"", Method = ""DELETE"" },
            new { Rel = ""reviews"", Href = $""/api/products/{id}/reviews"", Method = ""GET"" }
        }
    };

    return Ok(result); // 回傳 200 OK 和連結
}
```

---

## 🏷️ API 版本控制策略

```csharp
// 策略 1：URL 路徑版本控制（最常用，像書的版次）
[Route(""api/v1/products"")] // 第一版
public class ProductsV1Controller : ControllerBase
{
    [HttpGet] // GET /api/v1/products
    public IActionResult GetAll() => Ok(""V1 格式的商品清單""); // V1 格式回傳
}

[Route(""api/v2/products"")] // 第二版
public class ProductsV2Controller : ControllerBase
{
    [HttpGet] // GET /api/v2/products
    public IActionResult GetAll() => Ok(""V2 格式的商品清單（更多欄位）""); // V2 格式回傳
}

// 策略 2：HTTP Header 版本控制
// 請求時帶上 Header：api-version: 2
[HttpGet]
public IActionResult GetAll([FromHeader(Name = ""api-version"")] int version = 1)
{
    if (version == 2) // 如果要求 V2
        return Ok(""V2 回應""); // 回傳 V2 格式
    return Ok(""V1 回應"");     // 預設回傳 V1 格式
}

// 策略 3：Query String 版本控制
// GET /api/products?api-version=2
[HttpGet]
public IActionResult GetAll([FromQuery(Name = ""api-version"")] int version = 1)
{
    return version switch
    {
        2 => Ok(""V2 回應""),   // 第二版
        _ => Ok(""V1 回應"")    // 預設第一版
    };
}
```

---

## ⚠️ 錯誤回應格式：RFC 7807 Problem Details

```csharp
// ASP.NET Core 內建支援 Problem Details（標準化的錯誤回應格式）
// 就像醫院的病歷表——每個欄位都有固定的格式，方便任何醫生閱讀

[HttpGet(""{id}"")]
public IActionResult GetById(int id)
{
    var product = _db.Products.Find(id); // 查找商品

    if (product == null) // 如果找不到
    {
        return Problem(
            detail: $""找不到 ID 為 {id} 的商品，請確認商品 ID 是否正確"",  // 詳細說明
            title: ""找不到資源"",           // 錯誤標題
            statusCode: 404,               // HTTP 狀態碼
            instance: $""/api/products/{id}"" // 發生問題的 URL
        );
        // 回傳格式：
        // {
        //   ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.4"",
        //   ""title"": ""找不到資源"",
        //   ""status"": 404,
        //   ""detail"": ""找不到 ID 為 5 的商品..."",
        //   ""instance"": ""/api/products/5""
        // }
    }

    return Ok(product); // 回傳 200 OK
}

// 在 Program.cs 啟用全域 Problem Details
builder.Services.AddProblemDetails(); // 註冊 Problem Details 服務
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：在 URL 中使用動詞

```csharp
// ❌ 錯誤寫法：URL 用了動詞（像把說明書印在包裝上，多此一舉）
[HttpGet(""api/getProducts"")]       // 不要用 get 動詞
public IActionResult GetProducts() => Ok(); // GET 本身就代表「取得」

[HttpPost(""api/createProduct"")]    // 不要用 create 動詞
public IActionResult CreateProduct(Product p) => Ok(); // POST 就代表「建立」

[HttpPost(""api/deleteProduct/{id}"")] // 更不該用 POST 來刪除！
public IActionResult DeleteProduct(int id) => Ok(); // 應該用 DELETE 方法
```

```csharp
// ✅ 正確寫法：URL 只用名詞，讓 HTTP 方法表達動作
[Route(""api/products"")] // 名詞、複數、小寫
[ApiController] // API 控制器
public class ProductsController : ControllerBase
{
    [HttpGet]          // GET /api/products → 取得所有
    public IActionResult GetAll() => Ok(); // HTTP 方法已經表達了「取得」

    [HttpGet(""{id}"")] // GET /api/products/5 → 取得單一
    public IActionResult GetById(int id) => Ok(); // 用路由參數指定資源

    [HttpPost]         // POST /api/products → 建立
    public IActionResult Create(Product p) => Created($""/api/products/{p.Id}"", p);

    [HttpDelete(""{id}"")] // DELETE /api/products/5 → 刪除
    public IActionResult Delete(int id) => NoContent(); // 用正確的 HTTP 方法
}
```

**解釋：** HTTP 方法本身就代表動作——GET=取得、POST=建立、PUT=更新、DELETE=刪除。在 URL 裡再加上動詞就像說「我要 GET 取得（getProducts）」——重複了。URL 應該像門牌地址，只告訴你「什麼東西在哪裡」，不用告訴你「怎麼拿」。

### ❌ 錯誤 2：不一致的命名風格

```csharp
// ❌ 錯誤寫法：整個 API 的 URL 風格不統一（像一棟大樓每層的門牌格式都不同）
[HttpGet(""api/Products"")]         // 大寫開頭
[HttpGet(""api/user-profiles"")]    // 小寫加破折號
[HttpGet(""api/order_items"")]      // 底線分隔
[HttpGet(""api/getShippingInfo"")]  // 駝峰式加動詞
// 用你 API 的開發者會崩潰！
```

```csharp
// ✅ 正確寫法：統一使用小寫加破折號
[Route(""api/products"")]           // 統一風格
[Route(""api/user-profiles"")]      // 統一風格
[Route(""api/order-items"")]        // 統一風格
[Route(""api/shipping-info"")]      // 統一風格
// 一致的命名讓 API 更好用、更好記
```

**解釋：** API 的 URL 就像城市的路名——如果「中山路」有的地方寫「ZhongShan Road」、有的寫「zhong_shan_rd」、有的寫「中山-路」，開車的人一定會迷路。選擇一種風格，然後整個 API 都用同一種。

### ❌ 錯誤 3：不使用正確的 HTTP 狀態碼

```csharp
// ❌ 錯誤寫法：不管發生什麼都回傳 200（像不管你問什麼，醫生都說「你很健康」）
[HttpGet(""{id}"")]
public IActionResult GetById(int id)
{
    var product = _db.Products.Find(id); // 查找商品
    if (product == null)
        return Ok(new { Error = ""找不到商品"" }); // 明明找不到卻回 200？
    return Ok(product); // 找到了回 200
}
```

```csharp
// ✅ 正確寫法：用正確的狀態碼告訴客戶端實際情況
[HttpGet(""{id}"")]
public IActionResult GetById(int id)
{
    var product = _db.Products.Find(id); // 查找商品
    if (product == null)
        return NotFound(new { Error = $""找不到 ID={id} 的商品"" }); // 404
    return Ok(product); // 200
}

// 常用狀態碼對照表：
// 200 OK              → 成功取得資料
// 201 Created         → 成功建立新資源
// 204 No Content      → 成功但沒有回傳內容（用於 PUT/DELETE）
// 400 Bad Request     → 客戶端送的資料有問題
// 401 Unauthorized    → 未登入（沒有提供身分證明）
// 403 Forbidden       → 已登入但沒有權限（有身分證但不能進 VIP 室）
// 404 Not Found       → 找不到資源
// 409 Conflict        → 資源衝突（例如重複建立）
// 500 Internal Error  → 伺服器自己出問題了
```

**解釋：** HTTP 狀態碼就像交通號誌——綠燈（200）代表通行、紅燈（4xx/5xx）代表有問題。如果不管什麼情況都亮綠燈，前端開發者就無法正確判斷結果，只能去解析回應內容才知道有沒有錯，增加了不必要的複雜度。
" }
        };

        return chapters;
    }
}
