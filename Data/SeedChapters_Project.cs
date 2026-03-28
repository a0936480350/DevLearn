using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Project
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Project Chapter 550 ────────────────────────────
        new() { Id=550, Category="project", Order=1, Level="beginner", Icon="📐", Title="專案規劃與需求分析", Slug="project-planning-analysis", IsPublished=true, Content=@"
# 專案規劃與需求分析

## 為什麼需要規劃？

> 💡 **比喻：蓋房子先畫藍圖**
> 你不會叫工人直接搬磚頭開始蓋房子吧？
> 一定是先找建築師畫好藍圖，確認幾間房間、幾間廁所、電線怎麼走。
> 寫程式也一樣——先規劃好，才不會蓋到一半才發現要拆牆重來。

### 沒有規劃的專案會怎樣？

```
沒有規劃的專案：
第 1 週：興致勃勃開始寫 code ✨
第 2 週：發現資料庫設計不對，砍掉重來 💣
第 3 週：客戶說「我要的不是這個」 😱
第 4 週：放棄 💀

有規劃的專案：
第 1 週：寫需求文件 + 畫流程圖 📐
第 2 週：確認規格 + 設計 Schema 📋
第 3 週：開始寫 code（方向清楚） 💻
第 4 週：第一版完成 + Demo 🎉
```

---

## 從想法到規格書

### 第一步：把想法寫下來

```csharp
// 定義專案想法的類別 // Define project idea class
public class ProjectIdea // 專案想法類別
{
    public string Name { get; set; } = """"; // 專案名稱
    public string Description { get; set; } = """"; // 專案描述
    public string TargetAudience { get; set; } = """"; // 目標使用者
    public string ProblemToSolve { get; set; } = """"; // 要解決的問題
    public List<string> CoreFeatures { get; set; } = new(); // 核心功能清單
}

// 範例：電商網站的專案想法 // Example: e-commerce project idea
var idea = new ProjectIdea // 建立電商專案想法
{
    Name = ""小農直售平台"", // 設定專案名稱
    Description = ""讓小農可以直接賣農產品給消費者"", // 設定專案描述
    TargetAudience = ""小農 + 注重食安的消費者"", // 設定目標族群
    ProblemToSolve = ""中間商抽太多，小農沒賺到錢"", // 設定要解決的問題
    CoreFeatures = new List<string> // 列出核心功能
    {
        ""商品上架管理"", // 功能一：商品管理
        ""購物車與結帳"", // 功能二：購物流程
        ""訂單追蹤"", // 功能三：訂單管理
        ""會員系統"" // 功能四：會員功能
    }
};
```

---

## User Story 撰寫法

> 💡 **比喻：點菜單**
> User Story 就像餐廳的菜單——寫清楚「誰要吃什麼」以及「為什麼想吃」。
> 格式固定：**身為（角色），我想要（功能），以便（目的）**

### User Story 範本

```csharp
// 定義 User Story 類別 // Define User Story class
public class UserStory // 使用者故事類別
{
    public string Role { get; set; } = """"; // 角色（誰在用）
    public string Want { get; set; } = """"; // 想要什麼功能
    public string SoThat { get; set; } = """"; // 目的是什麼
    public string Priority { get; set; } = """"; // 優先順序
    public List<string> AcceptanceCriteria { get; set; } = new(); // 驗收標準
}

// 撰寫 User Story 範例 // Write User Story examples
var stories = new List<UserStory> // 建立使用者故事清單
{
    new UserStory // 第一個故事：商品瀏覽
    {
        Role = ""消費者"", // 角色是消費者
        Want = ""瀏覽商品清單並篩選分類"", // 想要瀏覽和篩選商品
        SoThat = ""快速找到想買的農產品"", // 目的是快速找到商品
        Priority = ""Must"", // 優先順序：必須有
        AcceptanceCriteria = new List<string> // 驗收標準清單
        {
            ""能顯示所有商品的卡片式列表"", // 標準一：卡片式顯示
            ""能按分類篩選（蔬菜、水果、米糧）"", // 標準二：分類篩選
            ""能按價格排序（高到低、低到高）"", // 標準三：價格排序
            ""每頁顯示 12 筆，有分頁功能"" // 標準四：分頁功能
        }
    },
    new UserStory // 第二個故事：購物車
    {
        Role = ""消費者"", // 角色是消費者
        Want = ""將商品加入購物車並調整數量"", // 想要管理購物車
        SoThat = ""一次購買多樣商品"", // 目的是批量購買
        Priority = ""Must"", // 優先順序：必須有
        AcceptanceCriteria = new List<string> // 驗收標準清單
        {
            ""點擊加入購物車後顯示成功提示"", // 標準一：加入回饋
            ""購物車頁面可修改數量"", // 標準二：修改數量
            ""能顯示小計與總計金額"", // 標準三：金額計算
            ""能刪除購物車中的商品"" // 標準四：刪除功能
        }
    }
};

// 輸出 User Story 格式 // Output formatted User Story
foreach (var story in stories) // 逐一輸出每個故事
{
    var output = $""身為 {story.Role}，"" + // 組合角色部分
                 $""我想要 {story.Want}，"" + // 組合功能部分
                 $""以便 {story.SoThat}""; // 組合目的部分
    Console.WriteLine(output); // 印出完整的 User Story
    Console.WriteLine($""優先順序：{story.Priority}""); // 印出優先順序
}
```

---

## 功能拆解與優先排序 (MoSCoW)

> 💡 **比喻：搬家打包**
> 搬家時你會分類：**一定要帶的**（身分證、錢包）、**應該帶的**（換洗衣物）、
> **可以帶的**（書本）、**不帶的**（舊雜誌）。
> MoSCoW 就是幫功能做一樣的分類。

```csharp
// 定義 MoSCoW 優先順序列舉 // Define MoSCoW priority enum
public enum MoSCoWPriority // MoSCoW 優先順序
{
    Must,   // 必須有：沒有這個系統不能用 // Must have
    Should, // 應該有：很重要但可以晚一點做 // Should have
    Could,  // 可以有：有的話更好 // Could have
    Wont    // 不做：這次不做，未來再說 // Won't have
}

// 定義功能項目類別 // Define feature item class
public class FeatureItem // 功能項目類別
{
    public string Name { get; set; } = """"; // 功能名稱
    public MoSCoWPriority Priority { get; set; } // MoSCoW 優先等級
    public int EstimatedDays { get; set; } // 預估天數
    public string Note { get; set; } = """"; // 備註說明
}

// 電商網站的功能拆解 // Feature breakdown for e-commerce
var features = new List<FeatureItem> // 建立功能清單
{
    // Must（必須有） // 沒這些就不能上線
    new() { Name=""會員註冊登入"", Priority=MoSCoWPriority.Must, EstimatedDays=3, Note=""用 Identity Framework"" }, // 會員系統
    new() { Name=""商品 CRUD"", Priority=MoSCoWPriority.Must, EstimatedDays=4, Note=""含圖片上傳"" }, // 商品管理
    new() { Name=""購物車"", Priority=MoSCoWPriority.Must, EstimatedDays=3, Note=""Session + DB 雙軌"" }, // 購物車功能
    new() { Name=""訂單建立"", Priority=MoSCoWPriority.Must, EstimatedDays=3, Note=""含庫存扣減"" }, // 訂單功能

    // Should（應該有） // 第一版之後盡快做
    new() { Name=""商品搜尋"", Priority=MoSCoWPriority.Should, EstimatedDays=2, Note=""關鍵字 + 分類"" }, // 搜尋功能
    new() { Name=""Email 通知"", Priority=MoSCoWPriority.Should, EstimatedDays=2, Note=""訂單確認信"" }, // 通知功能
    new() { Name=""訂單狀態追蹤"", Priority=MoSCoWPriority.Should, EstimatedDays=2, Note=""狀態機設計"" }, // 追蹤功能

    // Could（可以有） // 有時間再做
    new() { Name=""商品評價"", Priority=MoSCoWPriority.Could, EstimatedDays=3, Note=""星等 + 文字評論"" }, // 評價功能
    new() { Name=""推薦系統"", Priority=MoSCoWPriority.Could, EstimatedDays=5, Note=""基於購買紀錄"" }, // 推薦功能

    // Won't（不做） // 這次不做
    new() { Name=""即時聊天"", Priority=MoSCoWPriority.Wont, EstimatedDays=7, Note=""改用 LINE 客服"" }, // 聊天功能
};

// 統計各優先級的工作天數 // Calculate total days per priority
var summary = features // 從功能清單開始
    .GroupBy(f => f.Priority) // 依優先順序分組
    .Select(g => new // 建立統計物件
    {
        Priority = g.Key, // 優先等級
        Count = g.Count(), // 功能數量
        TotalDays = g.Sum(f => f.EstimatedDays) // 總天數
    })
    .OrderBy(s => s.Priority); // 依優先順序排序

foreach (var item in summary) // 逐一輸出統計結果
{
    Console.WriteLine($""{item.Priority}: {item.Count} 個功能，共 {item.TotalDays} 天""); // 印出統計
}
```

---

## 資料庫 Schema 設計流程

> 💡 **比喻：收納箱貼標籤**
> 設計 Schema 就像整理房間——先決定要幾個收納箱（Table），
> 每個箱子貼上標籤（Column），箱子之間用線連起來（Relationship）。

```csharp
// 設計電商網站的核心 Entity // Design core entities for e-commerce
// 第一步：識別核心實體 // Step 1: Identify core entities

public class Product // 商品實體
{
    public int Id { get; set; } // 主鍵
    public string Name { get; set; } = """"; // 商品名稱
    public string Description { get; set; } = """"; // 商品描述
    public decimal Price { get; set; } // 售價
    public int Stock { get; set; } // 庫存數量
    public int CategoryId { get; set; } // 外鍵：分類 ID
    public Category Category { get; set; } = null!; // 導覽屬性：分類
    public DateTime CreatedAt { get; set; } // 建立時間
    public bool IsActive { get; set; } = true; // 是否上架
}

public class Category // 分類實體
{
    public int Id { get; set; } // 主鍵
    public string Name { get; set; } = """"; // 分類名稱
    public string Slug { get; set; } = """"; // 網址用的代稱
    public List<Product> Products { get; set; } = new(); // 導覽屬性：此分類的商品
}

public class Order // 訂單實體
{
    public int Id { get; set; } // 主鍵
    public string UserId { get; set; } = """"; // 會員 ID
    public DateTime OrderDate { get; set; } // 訂單日期
    public decimal TotalAmount { get; set; } // 訂單總金額
    public string Status { get; set; } = ""Pending""; // 訂單狀態
    public List<OrderItem> Items { get; set; } = new(); // 導覽屬性：訂單明細
}

public class OrderItem // 訂單明細實體
{
    public int Id { get; set; } // 主鍵
    public int OrderId { get; set; } // 外鍵：訂單 ID
    public int ProductId { get; set; } // 外鍵：商品 ID
    public int Quantity { get; set; } // 購買數量
    public decimal UnitPrice { get; set; } // 當時單價（記錄歷史價格）
    public Order Order { get; set; } = null!; // 導覽屬性：所屬訂單
    public Product Product { get; set; } = null!; // 導覽屬性：商品資訊
}
```

### Entity Relationship 關係圖

```
┌─────────────┐     ┌──────────────┐     ┌──────────────┐
│  Category   │────<│   Product    │>────│  OrderItem   │
│             │ 1:N │              │ 1:N │              │
│ Id          │     │ Id           │     │ Id           │
│ Name        │     │ Name         │     │ OrderId (FK) │
│ Slug        │     │ Price        │     │ ProductId(FK)│
└─────────────┘     │ CategoryId   │     │ Quantity     │
                    │ Stock        │     │ UnitPrice    │
                    └──────────────┘     └──────┬───────┘
                                                │ N:1
                                         ┌──────┴───────┐
                                         │    Order     │
                                         │ Id           │
                                         │ UserId       │
                                         │ TotalAmount  │
                                         │ Status       │
                                         └──────────────┘
```

---

## API 端點規劃 (RESTful)

```csharp
// RESTful API 端點規劃表 // RESTful API endpoint planning
// 規則：用名詞不用動詞，用 HTTP Method 區分操作 // Use nouns, not verbs

public class ApiEndpoint // API 端點類別
{
    public string Method { get; set; } = """"; // HTTP 方法
    public string Path { get; set; } = """"; // 路徑
    public string Description { get; set; } = """"; // 說明
    public string Auth { get; set; } = """"; // 驗證需求
}

var endpoints = new List<ApiEndpoint> // 建立端點清單
{
    // 商品相關 API // Product endpoints
    new() { Method=""GET"", Path=""/api/products"", Description=""取得商品清單（支援分頁、篩選）"", Auth=""無"" }, // 公開取得商品
    new() { Method=""GET"", Path=""/api/products/{id}"", Description=""取得單一商品詳情"", Auth=""無"" }, // 公開取得商品詳情
    new() { Method=""POST"", Path=""/api/products"", Description=""新增商品"", Auth=""Admin"" }, // 管理員新增商品
    new() { Method=""PUT"", Path=""/api/products/{id}"", Description=""更新商品資訊"", Auth=""Admin"" }, // 管理員更新商品
    new() { Method=""DELETE"", Path=""/api/products/{id}"", Description=""刪除商品"", Auth=""Admin"" }, // 管理員刪除商品

    // 購物車相關 API // Cart endpoints
    new() { Method=""GET"", Path=""/api/cart"", Description=""取得購物車內容"", Auth=""User"" }, // 會員取得購物車
    new() { Method=""POST"", Path=""/api/cart/items"", Description=""加入商品到購物車"", Auth=""User"" }, // 會員加入商品
    new() { Method=""PUT"", Path=""/api/cart/items/{id}"", Description=""更新購物車商品數量"", Auth=""User"" }, // 會員修改數量
    new() { Method=""DELETE"", Path=""/api/cart/items/{id}"", Description=""移除購物車商品"", Auth=""User"" }, // 會員移除商品

    // 訂單相關 API // Order endpoints
    new() { Method=""POST"", Path=""/api/orders"", Description=""建立訂單（從購物車結帳）"", Auth=""User"" }, // 會員下單
    new() { Method=""GET"", Path=""/api/orders"", Description=""取得我的訂單列表"", Auth=""User"" }, // 會員查詢訂單
    new() { Method=""GET"", Path=""/api/orders/{id}"", Description=""取得訂單詳情"", Auth=""User"" }, // 會員查詢訂單詳情
};

// 印出 API 規劃表 // Print API plan
foreach (var ep in endpoints) // 逐一輸出端點
{
    Console.WriteLine($""{ep.Method,-8} {ep.Path,-30} {ep.Description}""); // 格式化輸出
}
```

---

## 線框圖 (Wireframe) 工具推薦

```csharp
// 線框圖工具比較 // Wireframe tool comparison
var tools = new Dictionary<string, string> // 建立工具字典
{
    [""Figma""] = ""免費版功能強大，支援團隊協作，業界標準"", // Figma：最推薦
    [""Excalidraw""] = ""免費開源，手繪風格，適合快速草圖"", // Excalidraw：快速草圖
    [""draw.io""] = ""免費，適合畫流程圖和架構圖"", // draw.io：流程圖
    [""Balsamiq""] = ""付費，低保真度線框圖專用"", // Balsamiq：專業線框圖
    [""紙和筆""] = ""最快！先在紙上畫，確認後再用工具"" // 紙筆：最快速
};

foreach (var tool in tools) // 逐一輸出工具資訊
{
    Console.WriteLine($""🔧 {tool.Key}: {tool.Value}""); // 印出工具名稱和說明
}
```

---

## 時程估算技巧

> 💡 **比喻：做菜估時間**
> 你覺得炒盤菜 10 分鐘，但你忘了算洗菜、切菜、洗鍋子的時間。
> 程式估時也一樣——**寫 code 只佔 30%**，測試、除錯、調整佔 70%。

```csharp
// 時程估算工具類別 // Time estimation utility class
public class TaskEstimator // 任務估算器
{
    public string TaskName { get; set; } = """"; // 任務名稱
    public double OptimisticDays { get; set; } // 樂觀估計（天）
    public double PessimisticDays { get; set; } // 悲觀估計（天）
    public double MostLikelyDays { get; set; } // 最可能估計（天）

    // PERT 估算法：(樂觀 + 4*最可能 + 悲觀) / 6 // PERT estimation formula
    public double PertEstimate => // 計算 PERT 估計值
        (OptimisticDays + 4 * MostLikelyDays + PessimisticDays) / 6; // 加權平均公式
}

// 使用 PERT 估算法 // Apply PERT estimation
var tasks = new List<TaskEstimator> // 建立任務清單
{
    new() { TaskName=""會員系統"", OptimisticDays=2, MostLikelyDays=3, PessimisticDays=7 }, // 估算會員系統
    new() { TaskName=""商品 CRUD"", OptimisticDays=3, MostLikelyDays=4, PessimisticDays=8 }, // 估算商品功能
    new() { TaskName=""購物車"", OptimisticDays=2, MostLikelyDays=3, PessimisticDays=6 }, // 估算購物車
    new() { TaskName=""訂單流程"", OptimisticDays=3, MostLikelyDays=5, PessimisticDays=10 }, // 估算訂單功能
    new() { TaskName=""前端頁面"", OptimisticDays=5, MostLikelyDays=7, PessimisticDays=14 }, // 估算前端開發
    new() { TaskName=""測試與修正"", OptimisticDays=3, MostLikelyDays=5, PessimisticDays=10 }, // 估算測試時間
};

double totalDays = 0; // 初始化總天數
foreach (var task in tasks) // 逐一計算每個任務
{
    var estimate = Math.Ceiling(task.PertEstimate); // 無條件進位取整數天
    totalDays += estimate; // 累加到總天數
    Console.WriteLine($""{task.TaskName}: 約 {estimate} 天""); // 印出估算結果
}

// 加上緩衝時間（建議加 20-30%） // Add buffer time (20-30% recommended)
var buffer = Math.Ceiling(totalDays * 0.25); // 加 25% 緩衝
Console.WriteLine($""預估總天數：{totalDays} 天 + 緩衝 {buffer} 天 = {totalDays + buffer} 天""); // 印出最終估計
```

**黃金法則：初學者把你的估計乘以 2，中級乘以 1.5，資深乘以 1.2。**

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：沒有寫驗收標準

```csharp
// ❌ 錯誤：User Story 太模糊 // Mistake: vague User Story
var badStory = ""使用者可以買東西""; // 這太模糊了，什麼叫買東西？

// ✅ 正確：要有明確的驗收標準 // Correct: include acceptance criteria
var goodStory = new UserStory // 建立清楚的使用者故事
{
    Role = ""消費者"", // 明確的角色
    Want = ""用信用卡結帳購物車的商品"", // 明確的功能
    SoThat = ""快速完成購買流程"", // 明確的目的
    AcceptanceCriteria = new List<string> // 列出可驗證的標準
    {
        ""支援 Visa/MasterCard"", // 驗收條件一
        ""結帳失敗要顯示錯誤訊息"", // 驗收條件二
        ""成功後寄確認信"" // 驗收條件三
    }
};
```

### ❌ 錯誤 2：所有功能都標 Must

```csharp
// ❌ 錯誤：每個功能都是 Must // Mistake: everything is Must priority
var badPriority = new List<FeatureItem> // 全部都標 Must 等於沒排序
{
    new() { Name=""AI 推薦"", Priority=MoSCoWPriority.Must, EstimatedDays=10 }, // AI 推薦不是第一版必須
    new() { Name=""多國語言"", Priority=MoSCoWPriority.Must, EstimatedDays=5 }, // 多語言也不是第一版必須
    new() { Name=""基本登入"", Priority=MoSCoWPriority.Must, EstimatedDays=2 }, // 這才是真正的 Must
};
// 當所有功能都是 Must，就等於沒有優先排序 // If everything is Must, nothing is Must
// 問自己：「沒有這個功能，系統能不能上線？」 // Ask: can the system launch without this?
```

### ❌ 錯誤 3：忘記記錄歷史價格

```csharp
// ❌ 錯誤：訂單明細只記商品 ID // Mistake: only storing product ID in order
public class BadOrderItem // 糟糕的訂單明細設計
{
    public int ProductId { get; set; } // 只存商品 ID
    public int Quantity { get; set; } // 只存數量
    // 問題：商品漲價後，舊訂單金額也跟著變！ // Bug: old orders change when price updates!
}

// ✅ 正確：記錄當時的價格快照 // Correct: store price snapshot
public class GoodOrderItem // 正確的訂單明細設計
{
    public int ProductId { get; set; } // 商品 ID
    public int Quantity { get; set; } // 數量
    public decimal UnitPrice { get; set; } // 下單當時的單價（快照）
    public string ProductName { get; set; } = """"; // 下單當時的商品名稱（快照）
}
```

---

## 📋 本章重點

| 步驟 | 做什麼 | 產出物 |
|------|--------|--------|
| 1 | 寫下想法 | 專案企劃書 |
| 2 | 寫 User Story | 使用者故事清單 |
| 3 | MoSCoW 排序 | 功能優先排序表 |
| 4 | 設計 Schema | ER 圖 + Migration |
| 5 | 規劃 API | API 端點文件 |
| 6 | 畫 Wireframe | 線框圖 |
| 7 | 估算時程 | 甘特圖 / 時程表 |

> 🎯 **下一步**：拿著這份規劃書，我們開始實作電商網站！
" },

        // ── Project Chapter 551 ────────────────────────────
        new() { Id=551, Category="project", Order=2, Level="intermediate", Icon="🛒", Title="實戰：電商網站開發", Slug="project-ecommerce", IsPublished=true, Content=@"
# 實戰：電商網站開發

## 專案架構（三層式 + Repository Pattern）

> 💡 **比喻：餐廳運作**
> - **Controller（外場服務生）**：接受客人點餐、送菜上桌
> - **Service（廚房主廚）**：處理商業邏輯，決定怎麼煮
> - **Repository（倉庫管理員）**：負責進出貨，拿食材
>
> 每一層各司其職，服務生不需要知道食材從哪來，主廚不需要知道客人坐哪桌。

### 專案資料夾結構

```
EShop/
├── Controllers/          ← 外場（接收 HTTP 請求）
│   ├── ProductController.cs
│   ├── CartController.cs
│   └── OrderController.cs
├── Services/             ← 廚房（商業邏輯）
│   ├── IProductService.cs
│   ├── ProductService.cs
│   ├── ICartService.cs
│   ├── CartService.cs
│   ├── IOrderService.cs
│   └── OrderService.cs
├── Repositories/         ← 倉庫（資料存取）
│   ├── IRepository.cs
│   ├── ProductRepository.cs
│   └── OrderRepository.cs
├── Models/               ← 食材（資料模型）
│   ├── Product.cs
│   ├── CartItem.cs
│   └── Order.cs
├── ViewModels/           ← 菜單（呈現用的模型）
│   ├── ProductListVM.cs
│   └── CheckoutVM.cs
└── Data/                 ← 冰箱（資料庫設定）
    └── AppDbContext.cs
```

### Repository Pattern 基本實作

```csharp
// 定義泛型 Repository 介面 // Define generic repository interface
public interface IRepository<T> where T : class // 泛型 Repository 介面
{
    Task<List<T>> GetAllAsync(); // 取得所有資料
    Task<T?> GetByIdAsync(int id); // 依 ID 取得單筆
    Task AddAsync(T entity); // 新增一筆資料
    Task UpdateAsync(T entity); // 更新一筆資料
    Task DeleteAsync(int id); // 刪除一筆資料
    Task<bool> ExistsAsync(int id); // 檢查是否存在
}

// 實作泛型 Repository // Implement generic repository
public class Repository<T> : IRepository<T> where T : class // 泛型 Repository 實作
{
    protected readonly AppDbContext _context; // 資料庫上下文
    protected readonly DbSet<T> _dbSet; // 資料表集合

    public Repository(AppDbContext context) // 建構函式注入 DbContext
    {
        _context = context; // 儲存 DbContext 參考
        _dbSet = context.Set<T>(); // 取得對應的 DbSet
    }

    public async Task<List<T>> GetAllAsync() // 取得所有資料的方法
    {
        return await _dbSet.ToListAsync(); // 從資料庫取得全部
    }

    public async Task<T?> GetByIdAsync(int id) // 依 ID 查詢的方法
    {
        return await _dbSet.FindAsync(id); // 用主鍵查詢
    }

    public async Task AddAsync(T entity) // 新增資料的方法
    {
        await _dbSet.AddAsync(entity); // 加入追蹤
        await _context.SaveChangesAsync(); // 儲存到資料庫
    }

    public async Task UpdateAsync(T entity) // 更新資料的方法
    {
        _dbSet.Update(entity); // 標記為已修改
        await _context.SaveChangesAsync(); // 儲存變更
    }

    public async Task DeleteAsync(int id) // 刪除資料的方法
    {
        var entity = await _dbSet.FindAsync(id); // 先找到該筆資料
        if (entity != null) // 如果找到了
        {
            _dbSet.Remove(entity); // 標記為刪除
            await _context.SaveChangesAsync(); // 執行刪除
        }
    }

    public async Task<bool> ExistsAsync(int id) // 檢查是否存在
    {
        return await _dbSet.FindAsync(id) != null; // 回傳是否找得到
    }
}
```

---

## 商品 CRUD 完整實作

### Model 定義

```csharp
// 商品 Model // Product model
public class Product // 商品類別
{
    public int Id { get; set; } // 主鍵
    [Required(ErrorMessage = ""商品名稱必填"")] // 驗證：必填
    [StringLength(100)] // 驗證：最長 100 字
    public string Name { get; set; } = """"; // 商品名稱

    public string? Description { get; set; } // 商品描述（可為空）

    [Required] // 驗證：必填
    [Range(1, 999999, ErrorMessage = ""價格需在 1~999999 之間"")] // 驗證：範圍
    public decimal Price { get; set; } // 售價

    [Range(0, int.MaxValue)] // 驗證：不可負數
    public int Stock { get; set; } // 庫存量

    public string? ImageUrl { get; set; } // 商品圖片網址

    public int CategoryId { get; set; } // 分類 ID（外鍵）
    public Category? Category { get; set; } // 導覽屬性：分類

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 建立時間
    public bool IsActive { get; set; } = true; // 是否上架
}
```

### Service 層

```csharp
// 商品 Service 介面 // Product service interface
public interface IProductService // 定義商品服務的合約
{
    Task<List<Product>> GetProductsAsync(string? category, string? search, int page, int pageSize); // 取得商品清單
    Task<Product?> GetProductAsync(int id); // 取得單一商品
    Task<Product> CreateProductAsync(Product product); // 建立商品
    Task UpdateProductAsync(Product product); // 更新商品
    Task DeleteProductAsync(int id); // 刪除商品
    Task<int> GetTotalCountAsync(string? category, string? search); // 取得總筆數
}

// 商品 Service 實作 // Product service implementation
public class ProductService : IProductService // 實作商品服務
{
    private readonly IRepository<Product> _repo; // 商品 Repository

    public ProductService(IRepository<Product> repo) // 建構函式注入 Repository
    {
        _repo = repo; // 儲存 Repository 參考
    }

    public async Task<List<Product>> GetProductsAsync( // 取得商品清單方法
        string? category, string? search, int page, int pageSize) // 支援篩選和分頁
    {
        var query = _context.Products // 從商品資料表開始
            .Include(p => p.Category) // 載入分類資訊
            .Where(p => p.IsActive) // 只顯示上架商品
            .AsQueryable(); // 轉為可查詢物件

        if (!string.IsNullOrEmpty(category)) // 如果有指定分類
        {
            query = query.Where(p => p.Category!.Slug == category); // 篩選該分類
        }

        if (!string.IsNullOrEmpty(search)) // 如果有搜尋關鍵字
        {
            query = query.Where(p => p.Name.Contains(search) // 搜尋名稱
                || p.Description!.Contains(search)); // 或搜尋描述
        }

        return await query // 執行查詢
            .OrderByDescending(p => p.CreatedAt) // 依建立時間降序
            .Skip((page - 1) * pageSize) // 跳過前幾筆（分頁）
            .Take(pageSize) // 取指定筆數
            .ToListAsync(); // 轉為 List 回傳
    }

    public async Task<Product> CreateProductAsync(Product product) // 建立商品方法
    {
        product.CreatedAt = DateTime.UtcNow; // 設定建立時間
        await _repo.AddAsync(product); // 透過 Repository 新增
        return product; // 回傳建立好的商品
    }

    public async Task UpdateProductAsync(Product product) // 更新商品方法
    {
        var existing = await _repo.GetByIdAsync(product.Id); // 先取得現有資料
        if (existing == null) // 如果找不到
            throw new KeyNotFoundException($""找不到商品 ID: {product.Id}""); // 拋出例外

        existing.Name = product.Name; // 更新名稱
        existing.Price = product.Price; // 更新價格
        existing.Stock = product.Stock; // 更新庫存
        existing.Description = product.Description; // 更新描述
        existing.CategoryId = product.CategoryId; // 更新分類

        await _repo.UpdateAsync(existing); // 儲存更新
    }

    public async Task DeleteProductAsync(int id) // 刪除商品方法
    {
        await _repo.DeleteAsync(id); // 透過 Repository 刪除
    }

    public async Task<Product?> GetProductAsync(int id) // 取得單一商品
    {
        return await _repo.GetByIdAsync(id); // 透過 Repository 查詢
    }

    public async Task<int> GetTotalCountAsync(string? category, string? search) // 取得總筆數
    {
        var query = _context.Products.Where(p => p.IsActive).AsQueryable(); // 查詢上架商品
        if (!string.IsNullOrEmpty(category)) // 如果有分類條件
            query = query.Where(p => p.Category!.Slug == category); // 加上分類篩選
        if (!string.IsNullOrEmpty(search)) // 如果有搜尋條件
            query = query.Where(p => p.Name.Contains(search)); // 加上搜尋篩選
        return await query.CountAsync(); // 回傳總筆數
    }
}
```

### Controller 層

```csharp
// 商品 Controller // Product controller
public class ProductController : Controller // 繼承 Controller 基底類別
{
    private readonly IProductService _service; // 商品服務

    public ProductController(IProductService service) // 建構函式注入服務
    {
        _service = service; // 儲存服務參考
    }

    // GET /Products?category=fruit&search=蘋果&page=1 // 商品列表頁
    public async Task<IActionResult> Index( // 列表 Action
        string? category, string? search, int page = 1) // 接收篩選參數
    {
        int pageSize = 12; // 每頁 12 筆
        var products = await _service.GetProductsAsync( // 取得商品
            category, search, page, pageSize); // 傳入篩選條件
        var total = await _service.GetTotalCountAsync(category, search); // 取得總筆數

        ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize); // 計算總頁數
        ViewBag.CurrentPage = page; // 目前頁碼
        ViewBag.Category = category; // 目前分類
        ViewBag.Search = search; // 目前搜尋詞

        return View(products); // 回傳 View
    }

    // GET /Products/Details/5 // 商品詳情頁
    public async Task<IActionResult> Details(int id) // 詳情 Action
    {
        var product = await _service.GetProductAsync(id); // 取得商品
        if (product == null) return NotFound(); // 找不到回 404
        return View(product); // 回傳 View
    }

    // POST /Products/Create // 建立商品
    [HttpPost] // 限定 POST 方法
    [ValidateAntiForgeryToken] // 防止 CSRF 攻擊
    [Authorize(Roles = ""Admin"")] // 限定 Admin 角色
    public async Task<IActionResult> Create(Product product) // 建立 Action
    {
        if (!ModelState.IsValid) return View(product); // 驗證失敗回表單
        await _service.CreateProductAsync(product); // 建立商品
        TempData[""Success""] = ""商品建立成功！""; // 設定成功訊息
        return RedirectToAction(nameof(Index)); // 導回列表頁
    }
}
```

---

## 購物車功能（Session + DB）

> 💡 **比喻：實體購物車 vs 線上清單**
> 在超市推購物車（Session）= 還沒結帳，離開就沒了。
> 加入線上願望清單（DB）= 登入後永遠都在。
> 我們兩個都做，未登入用 Session，登入後同步到 DB。

```csharp
// 購物車項目 Model // Cart item model
public class CartItem // 購物車項目類別
{
    public int Id { get; set; } // 主鍵
    public int ProductId { get; set; } // 商品 ID
    public string ProductName { get; set; } = """"; // 商品名稱（快照）
    public decimal UnitPrice { get; set; } // 單價（快照）
    public int Quantity { get; set; } // 數量
    public string? UserId { get; set; } // 會員 ID（登入後才有）
    public string SessionId { get; set; } = """"; // Session ID（未登入用）
    public decimal Subtotal => UnitPrice * Quantity; // 小計（計算屬性）
}

// 購物車 Service // Cart service
public class CartService : ICartService // 實作購物車服務
{
    private readonly AppDbContext _context; // 資料庫上下文
    private readonly IHttpContextAccessor _http; // HTTP 上下文存取器

    public CartService(AppDbContext context, IHttpContextAccessor http) // 建構函式
    {
        _context = context; // 儲存 DbContext
        _http = http; // 儲存 HTTP 存取器
    }

    private string GetCartId() // 取得購物車識別 ID
    {
        var session = _http.HttpContext!.Session; // 取得 Session
        var cartId = session.GetString(""CartId""); // 嘗試讀取 CartId
        if (string.IsNullOrEmpty(cartId)) // 如果沒有 CartId
        {
            cartId = Guid.NewGuid().ToString(); // 產生新的 GUID
            session.SetString(""CartId"", cartId); // 存入 Session
        }
        return cartId; // 回傳 CartId
    }

    public async Task AddToCartAsync(int productId, int quantity = 1) // 加入購物車方法
    {
        var product = await _context.Products.FindAsync(productId); // 查詢商品
        if (product == null) throw new KeyNotFoundException(""商品不存在""); // 找不到就報錯
        if (product.Stock < quantity) throw new InvalidOperationException(""庫存不足""); // 庫存不夠也報錯

        var cartId = GetCartId(); // 取得購物車 ID
        var existingItem = await _context.CartItems // 查詢購物車中是否已有此商品
            .FirstOrDefaultAsync(c => c.SessionId == cartId // 比對 Session ID
                && c.ProductId == productId); // 比對商品 ID

        if (existingItem != null) // 如果已經在購物車中
        {
            existingItem.Quantity += quantity; // 增加數量
        }
        else // 如果是新商品
        {
            _context.CartItems.Add(new CartItem // 建立新的購物車項目
            {
                ProductId = productId, // 設定商品 ID
                ProductName = product.Name, // 記錄商品名稱快照
                UnitPrice = product.Price, // 記錄當前價格快照
                Quantity = quantity, // 設定數量
                SessionId = cartId // 設定 Session ID
            });
        }

        await _context.SaveChangesAsync(); // 儲存到資料庫
    }

    public async Task<List<CartItem>> GetCartItemsAsync() // 取得購物車內容
    {
        var cartId = GetCartId(); // 取得購物車 ID
        return await _context.CartItems // 查詢購物車項目
            .Where(c => c.SessionId == cartId) // 篩選此購物車
            .ToListAsync(); // 回傳清單
    }

    public async Task<decimal> GetTotalAsync() // 計算購物車總金額
    {
        var items = await GetCartItemsAsync(); // 取得所有項目
        return items.Sum(i => i.Subtotal); // 加總所有小計
    }

    public async Task MergeCartAsync(string userId) // 登入後合併購物車
    {
        var cartId = GetCartId(); // 取得 Session 購物車 ID
        var sessionItems = await _context.CartItems // 取得 Session 中的項目
            .Where(c => c.SessionId == cartId && c.UserId == null) // 未登入的項目
            .ToListAsync(); // 轉為清單

        foreach (var item in sessionItems) // 逐一處理每個項目
        {
            item.UserId = userId; // 綁定會員 ID
        }
        await _context.SaveChangesAsync(); // 儲存變更
    }
}
```

---

## 會員系統（Identity Framework）

```csharp
// 在 Program.cs 中設定 Identity // Configure Identity in Program.cs
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => // 加入 Identity 服務
{
    options.Password.RequireDigit = true; // 密碼需包含數字
    options.Password.RequiredLength = 8; // 密碼最少 8 字元
    options.Password.RequireUppercase = false; // 不強制大寫（對中文使用者友善）
    options.Password.RequireNonAlphanumeric = false; // 不強制特殊字元
    options.User.RequireUniqueEmail = true; // Email 不可重複
    options.SignIn.RequireConfirmedEmail = false; // 先不要求 Email 驗證
})
.AddEntityFrameworkStores<AppDbContext>() // 使用 EF Core 儲存
.AddDefaultTokenProviders(); // 加入預設 Token 產生器

// 設定 Cookie // Configure authentication cookie
builder.Services.ConfigureApplicationCookie(options => // 設定驗證 Cookie
{
    options.LoginPath = ""/Account/Login""; // 未登入導向登入頁
    options.LogoutPath = ""/Account/Logout""; // 登出路徑
    options.AccessDeniedPath = ""/Account/AccessDenied""; // 權限不足頁面
    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Cookie 有效期 7 天
});

// 註冊 ViewModel // Register view model
public class RegisterVM // 註冊用的 ViewModel
{
    [Required(ErrorMessage = ""Email 必填"")] // 驗證：必填
    [EmailAddress(ErrorMessage = ""Email 格式不正確"")] // 驗證：Email 格式
    public string Email { get; set; } = """"; // Email 欄位

    [Required(ErrorMessage = ""密碼必填"")] // 驗證：必填
    [MinLength(8, ErrorMessage = ""密碼至少 8 個字元"")] // 驗證：最少 8 字
    [DataType(DataType.Password)] // 指定為密碼類型
    public string Password { get; set; } = """"; // 密碼欄位

    [Compare(""Password"", ErrorMessage = ""密碼不一致"")] // 驗證：與密碼比對
    [DataType(DataType.Password)] // 指定為密碼類型
    public string ConfirmPassword { get; set; } = """"; // 確認密碼欄位
}
```

---

## 訂單流程（狀態機設計）

> 💡 **比喻：快遞包裹追蹤**
> 你的包裹狀態：已下單 → 已付款 → 出貨中 → 配送中 → 已送達。
> 每個狀態只能往下一步走，不能跳步也不能倒退（除了取消）。

```csharp
// 訂單狀態列舉 // Order status enum
public enum OrderStatus // 訂單狀態
{
    Pending,    // 待付款：剛建立訂單
    Paid,       // 已付款：付款成功
    Processing, // 處理中：賣家準備出貨
    Shipped,    // 已出貨：交給物流
    Delivered,  // 已送達：買家收到
    Cancelled,  // 已取消：訂單取消
    Refunded    // 已退款：退款完成
}

// 訂單狀態機 // Order state machine
public class OrderStateMachine // 訂單狀態機類別
{
    // 定義合法的狀態轉換 // Define valid state transitions
    private static readonly Dictionary<OrderStatus, List<OrderStatus>> _transitions = // 狀態轉換表
        new() // 初始化狀態轉換規則
        {
            [OrderStatus.Pending] = new() { OrderStatus.Paid, OrderStatus.Cancelled }, // 待付款可以→已付款或取消
            [OrderStatus.Paid] = new() { OrderStatus.Processing, OrderStatus.Refunded }, // 已付款可以→處理中或退款
            [OrderStatus.Processing] = new() { OrderStatus.Shipped, OrderStatus.Refunded }, // 處理中可以→已出貨或退款
            [OrderStatus.Shipped] = new() { OrderStatus.Delivered }, // 已出貨只能→已送達
            [OrderStatus.Delivered] = new() { OrderStatus.Refunded }, // 已送達可以→退款
            [OrderStatus.Cancelled] = new(), // 已取消：終態，不能再變
            [OrderStatus.Refunded] = new(), // 已退款：終態，不能再變
        };

    public static bool CanTransition(OrderStatus from, OrderStatus to) // 檢查能否轉換狀態
    {
        return _transitions.ContainsKey(from) // 確認來源狀態存在
            && _transitions[from].Contains(to); // 確認目標狀態合法
    }

    public static void Transition(Order order, OrderStatus newStatus) // 執行狀態轉換
    {
        if (!CanTransition(order.Status, newStatus)) // 檢查是否可以轉換
        {
            throw new InvalidOperationException( // 不合法就拋出例外
                $""無法從 {order.Status} 變更為 {newStatus}""); // 說明錯誤原因
        }
        order.Status = newStatus; // 更新訂單狀態
        order.UpdatedAt = DateTime.UtcNow; // 記錄更新時間
    }
}

// 建立訂單的 Service 方法 // Order creation service method
public async Task<Order> CreateOrderAsync(string userId) // 建立訂單
{
    var cartItems = await _cartService.GetCartItemsAsync(); // 取得購物車項目
    if (!cartItems.Any()) // 如果購物車是空的
        throw new InvalidOperationException(""購物車是空的""); // 拋出例外

    var order = new Order // 建立新訂單
    {
        UserId = userId, // 設定會員 ID
        OrderDate = DateTime.UtcNow, // 設定訂單日期
        Status = OrderStatus.Pending, // 初始狀態：待付款
        Items = cartItems.Select(ci => new OrderItem // 將購物車轉為訂單明細
        {
            ProductId = ci.ProductId, // 商品 ID
            ProductName = ci.ProductName, // 商品名稱快照
            Quantity = ci.Quantity, // 數量
            UnitPrice = ci.UnitPrice // 單價快照
        }).ToList(), // 轉為清單
        TotalAmount = cartItems.Sum(ci => ci.Subtotal) // 計算訂單總金額
    };

    _context.Orders.Add(order); // 加入訂單
    await _context.SaveChangesAsync(); // 儲存到資料庫
    await _cartService.ClearCartAsync(); // 清空購物車
    return order; // 回傳訂單
}
```

---

## 金流串接概念（綠界 ECPay 範例）

```csharp
// 綠界金流串接服務 // ECPay integration service
public class EcpayService // 綠界金流服務類別
{
    private readonly IConfiguration _config; // 設定檔

    public EcpayService(IConfiguration config) // 建構函式注入設定
    {
        _config = config; // 儲存設定參考
    }

    public Dictionary<string, string> BuildPaymentForm(Order order) // 建立付款表單參數
    {
        var merchantId = _config[""ECPay:MerchantID""]; // 取得特店編號
        var hashKey = _config[""ECPay:HashKey""]; // 取得 HashKey
        var hashIv = _config[""ECPay:HashIV""]; // 取得 HashIV

        var parameters = new Dictionary<string, string> // 建立參數字典
        {
            [""MerchantID""] = merchantId!, // 特店編號
            [""MerchantTradeNo""] = $""ORDER{order.Id:D10}"", // 交易編號（補零到 10 位）
            [""MerchantTradeDate""] = DateTime.Now.ToString(""yyyy/MM/dd HH:mm:ss""), // 交易時間
            [""PaymentType""] = ""aio"", // 付款類型：all in one
            [""TotalAmount""] = ((int)order.TotalAmount).ToString(), // 總金額（整數）
            [""TradeDesc""] = ""小農直售平台訂單"", // 交易描述
            [""ItemName""] = string.Join(""#"", order.Items // 商品名稱用 # 分隔
                .Select(i => $""{i.ProductName} x{i.Quantity}"")), // 格式：名稱 x 數量
            [""ReturnURL""] = _config[""ECPay:ReturnURL""]!, // 付款結果通知網址
            [""OrderResultURL""] = _config[""ECPay:OrderResultURL""]!, // 付款完成導回網址
            [""ChoosePayment""] = ""ALL"" // 付款方式：全部開放
        };

        // 產生檢查碼 // Generate check mac value
        var checkMac = GenerateCheckMacValue(parameters, hashKey!, hashIv!); // 計算檢查碼
        parameters[""CheckMacValue""] = checkMac; // 加入檢查碼

        return parameters; // 回傳完整參數
    }

    private string GenerateCheckMacValue( // 產生綠界檢查碼
        Dictionary<string, string> parameters, // 參數字典
        string hashKey, string hashIv) // 金鑰
    {
        var raw = string.Join(""&"", parameters // 將參數排序並組合
            .OrderBy(p => p.Key) // 依 Key 排序
            .Select(p => $""{p.Key}={p.Value}"")); // 組合 Key=Value

        raw = $""HashKey={hashKey}&{raw}&HashIV={hashIv}""; // 前後加上金鑰
        raw = WebUtility.UrlEncode(raw).ToLower(); // URL 編碼後轉小寫

        using var sha256 = SHA256.Create(); // 建立 SHA256 雜湊
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw)); // 計算雜湊值
        return BitConverter.ToString(hash).Replace(""-"", """").ToUpper(); // 轉為大寫 16 進位
    }
}
```

---

## 部署到 Railway

```csharp
// appsettings.Production.json 設定 // Production settings
// Railway 會提供 DATABASE_URL 環境變數 // Railway provides DATABASE_URL env var
// 在 Program.cs 中讀取 // Read in Program.cs

var connectionString = Environment.GetEnvironmentVariable(""DATABASE_URL"") // 讀取環境變數
    ?? builder.Configuration.GetConnectionString(""DefaultConnection""); // 或用設定檔

builder.Services.AddDbContext<AppDbContext>(options => // 設定 DbContext
{
    if (connectionString!.StartsWith(""postgres://"")) // 如果是 PostgreSQL 格式
    {
        options.UseNpgsql(ConvertPostgresUrl(connectionString)); // 轉換並使用 Npgsql
    }
    else // 如果是一般格式
    {
        options.UseSqlServer(connectionString); // 使用 SQL Server
    }
});

// 轉換 Railway 的 PostgreSQL URL // Convert Railway PostgreSQL URL
static string ConvertPostgresUrl(string url) // URL 格式轉換方法
{
    var uri = new Uri(url); // 解析 URL
    var userInfo = uri.UserInfo.Split(':'); // 分離帳號密碼
    return $""Host={uri.Host};"" + // 主機
           $""Port={uri.Port};"" + // 連接埠
           $""Database={uri.AbsolutePath.TrimStart('/')};"" + // 資料庫名稱
           $""Username={userInfo[0]};"" + // 使用者名稱
           $""Password={userInfo[1]};"" + // 密碼
           $""SSL Mode=Require;Trust Server Certificate=true""; // SSL 設定
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：Controller 直接操作 DbContext

```csharp
// ❌ 錯誤：Controller 直接查資料庫 // Mistake: Controller directly queries DB
public class BadController : Controller // 不好的 Controller
{
    private readonly AppDbContext _context; // 直接注入 DbContext
    public async Task<IActionResult> Index() // 直接在 Controller 查詢
    {
        var products = await _context.Products.ToListAsync(); // 這裡沒有商業邏輯層
        return View(products); // 小專案可以但不好擴充
    }
}

// ✅ 正確：透過 Service 層 // Correct: go through Service layer
public class GoodController : Controller // 正確的 Controller
{
    private readonly IProductService _service; // 注入服務介面
    public async Task<IActionResult> Index() // 透過 Service 取資料
    {
        var products = await _service.GetProductsAsync(null, null, 1, 12); // 呼叫 Service 方法
        return View(products); // 回傳 View
    }
}
```

### ❌ 錯誤 2：購物車沒有存價格快照

```csharp
// ❌ 錯誤：結帳時才去查價格 // Mistake: query price at checkout time
public decimal CalculateTotal(List<CartItem> items) // 計算總金額
{
    decimal total = 0; // 初始化總額
    foreach (var item in items) // 逐一計算
    {
        var product = _context.Products.Find(item.ProductId); // 查詢當前價格
        total += product!.Price * item.Quantity; // 用當前價格計算
        // 問題：如果商品在購物車期間漲價，客人會嚇到！ // Bug: price may have changed!
    }
    return total; // 回傳可能不正確的金額
}

// ✅ 正確：加入購物車時就記錄價格 // Correct: record price when adding to cart
public async Task AddToCartAsync(int productId, int qty) // 加入購物車
{
    var product = await _context.Products.FindAsync(productId); // 查詢商品
    _context.CartItems.Add(new CartItem // 建立購物車項目
    {
        ProductId = productId, // 商品 ID
        UnitPrice = product!.Price, // 記錄當時的價格快照
        ProductName = product.Name, // 記錄當時的名稱快照
        Quantity = qty // 數量
    });
    await _context.SaveChangesAsync(); // 儲存
}
```

### ❌ 錯誤 3：訂單狀態沒有檢查就直接改

```csharp
// ❌ 錯誤：不檢查就直接改狀態 // Mistake: changing status without validation
public async Task UpdateStatus(int orderId, OrderStatus newStatus) // 更新訂單狀態
{
    var order = await _context.Orders.FindAsync(orderId); // 查詢訂單
    order!.Status = newStatus; // 直接改（已送達可以變回待付款？！）
    await _context.SaveChangesAsync(); // 儲存不合理的狀態
}

// ✅ 正確：用狀態機檢查 // Correct: use state machine
public async Task UpdateStatus(int orderId, OrderStatus newStatus) // 更新訂單狀態
{
    var order = await _context.Orders.FindAsync(orderId); // 查詢訂單
    OrderStateMachine.Transition(order!, newStatus); // 透過狀態機檢查並更新
    await _context.SaveChangesAsync(); // 儲存合法的狀態變更
}
```

---

## 📋 本章重點

| 層級 | 負責的事 | 不該做的事 |
|------|----------|------------|
| Controller | 接收請求、回傳結果 | 不該有商業邏輯 |
| Service | 商業邏輯、規則驗證 | 不該直接操作 HTTP |
| Repository | 資料存取（CRUD） | 不該有業務規則 |

> 🎯 **下一步**：用相同的架構來做部落格 CMS 系統！
" },

        // ── Project Chapter 552 ────────────────────────────
        new() { Id=552, Category="project", Order=3, Level="intermediate", Icon="📝", Title="實戰：部落格 CMS 系統", Slug="project-blog-cms", IsPublished=true, Content=@"
# 實戰：部落格 CMS 系統

## 專案概覽

> 💡 **比喻：經營一本雜誌**
> CMS（內容管理系統）就像經營一本雜誌：
> - **編輯器** = 記者寫稿的桌子
> - **分類與標籤** = 雜誌的不同專欄
> - **留言系統** = 讀者投書專區
> - **SEO** = 讓更多人在書店找到你的雜誌
> - **管理後台** = 總編輯的辦公室

### 專案結構

```
BlogCMS/
├── Controllers/
│   ├── BlogController.cs      ← 前台文章展示
│   ├── AdminController.cs     ← 後台管理
│   └── CommentController.cs   ← 留言管理
├── Services/
│   ├── IPostService.cs        ← 文章服務介面
│   ├── PostService.cs         ← 文章服務實作
│   ├── IImageService.cs       ← 圖片服務介面
│   └── ImageService.cs        ← 圖片服務實作
├── Models/
│   ├── Post.cs                ← 文章 Model
│   ├── Tag.cs                 ← 標籤 Model
│   └── Comment.cs             ← 留言 Model
├── ViewModels/
│   ├── PostEditorVM.cs        ← 編輯器 ViewModel
│   └── PostListVM.cs          ← 文章列表 ViewModel
└── wwwroot/
    └── uploads/               ← 圖片上傳目錄
```

---

## Markdown 編輯器整合

```csharp
// 文章 Model（支援 Markdown） // Post model with Markdown support
public class Post // 文章類別
{
    public int Id { get; set; } // 主鍵
    [Required(ErrorMessage = ""標題必填"")] // 驗證：標題必填
    [StringLength(200)] // 限制長度
    public string Title { get; set; } = """"; // 文章標題

    [Required] // 必填
    public string Slug { get; set; } = """"; // URL 友善的代稱

    [Required(ErrorMessage = ""內容必填"")] // 驗證：內容必填
    public string MarkdownContent { get; set; } = """"; // Markdown 原始內容

    public string HtmlContent { get; set; } = """"; // 轉換後的 HTML

    public string? Excerpt { get; set; } // 文章摘要
    public string? FeaturedImage { get; set; } // 精選圖片

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 建立時間
    public DateTime? PublishedAt { get; set; } // 發佈時間
    public bool IsPublished { get; set; } = false; // 是否已發佈

    public int CategoryId { get; set; } // 分類 ID
    public Category? Category { get; set; } // 導覽屬性：分類

    public List<PostTag> PostTags { get; set; } = new(); // 多對多：文章標籤
    public List<Comment> Comments { get; set; } = new(); // 一對多：留言
    public int ViewCount { get; set; } = 0; // 瀏覽次數
}

// Slug 產生器 // Slug generator utility
public static class SlugGenerator // Slug 產生器靜態類別
{
    public static string Generate(string title) // 從標題產生 Slug
    {
        var slug = title.ToLower().Trim(); // 轉小寫並去除頭尾空白
        slug = Regex.Replace(slug, @""[^\w\u4e00-\u9fff\s-]"", """"); // 移除特殊字元（保留中文）
        slug = Regex.Replace(slug, @""[\s]+"", ""-""); // 空白替換為連字號
        slug = Regex.Replace(slug, @""-+"", ""-""); // 多個連字號合併
        slug = slug.Trim('-'); // 去除頭尾連字號
        return slug; // 回傳 Slug
    }
}

// Markdown 轉 HTML 服務（使用 Markdig） // Markdown to HTML service
public class MarkdownService // Markdown 轉換服務
{
    private readonly MarkdownPipeline _pipeline; // Markdig 管線

    public MarkdownService() // 建構函式
    {
        _pipeline = new MarkdownPipelineBuilder() // 建立管線建構器
            .UseAdvancedExtensions() // 啟用進階擴充（表格、任務列表等）
            .UseEmojiAndSmiley() // 支援 Emoji
            .UseSyntaxHighlighting() // 程式碼語法高亮
            .Build(); // 建置管線
    }

    public string ToHtml(string markdown) // Markdown 轉 HTML 方法
    {
        if (string.IsNullOrEmpty(markdown)) return """"; // 空內容回傳空字串
        return Markdown.ToHtml(markdown, _pipeline); // 使用 Markdig 轉換
    }

    public string ToPlainText(string markdown, int maxLength = 200) // 產生摘要
    {
        var html = ToHtml(markdown); // 先轉 HTML
        var text = Regex.Replace(html, ""<[^>]+>"", """"); // 移除 HTML 標籤
        text = WebUtility.HtmlDecode(text); // 解碼 HTML 實體
        return text.Length > maxLength // 如果超過長度限制
            ? text[..maxLength] + ""..."" // 截斷並加省略號
            : text; // 否則回傳完整文字
    }
}
```

### 前端整合 EasyMDE 編輯器

```csharp
// PostEditorVM：傳給 View 的 ViewModel // Editor ViewModel
public class PostEditorVM // 文章編輯器 ViewModel
{
    public int? Id { get; set; } // 文章 ID（新增時為 null）
    [Required(ErrorMessage = ""標題必填"")] // 驗證
    public string Title { get; set; } = """"; // 文章標題
    [Required(ErrorMessage = ""內容必填"")] // 驗證
    public string MarkdownContent { get; set; } = """"; // Markdown 內容
    public int CategoryId { get; set; } // 分類 ID
    public string TagIds { get; set; } = """"; // 標籤 ID（逗號分隔）
    public IFormFile? FeaturedImage { get; set; } // 精選圖片上傳
    public bool IsPublished { get; set; } // 是否立即發佈
    public List<Category> AvailableCategories { get; set; } = new(); // 可選分類
    public List<Tag> AvailableTags { get; set; } = new(); // 可選標籤
}
```

---

## 文章 CRUD + 分類標籤

### 多對多關係：文章與標籤

```csharp
// 標籤 Model // Tag model
public class Tag // 標籤類別
{
    public int Id { get; set; } // 主鍵
    public string Name { get; set; } = """"; // 標籤名稱
    public string Slug { get; set; } = """"; // URL 代稱
    public List<PostTag> PostTags { get; set; } = new(); // 多對多關聯
}

// 多對多中間表 // Many-to-many join table
public class PostTag // 文章標籤關聯表
{
    public int PostId { get; set; } // 文章 ID
    public Post Post { get; set; } = null!; // 導覽屬性：文章
    public int TagId { get; set; } // 標籤 ID
    public Tag Tag { get; set; } = null!; // 導覽屬性：標籤
}

// DbContext 設定多對多關係 // Configure many-to-many in DbContext
protected override void OnModelCreating(ModelBuilder builder) // 設定模型
{
    base.OnModelCreating(builder); // 呼叫基底方法

    builder.Entity<PostTag>() // 設定 PostTag 實體
        .HasKey(pt => new { pt.PostId, pt.TagId }); // 複合主鍵

    builder.Entity<PostTag>() // 設定 Post 端的關係
        .HasOne(pt => pt.Post) // 一篇文章
        .WithMany(p => p.PostTags) // 有多個標籤關聯
        .HasForeignKey(pt => pt.PostId); // 外鍵是 PostId

    builder.Entity<PostTag>() // 設定 Tag 端的關係
        .HasOne(pt => pt.Tag) // 一個標籤
        .WithMany(t => t.PostTags) // 有多個文章關聯
        .HasForeignKey(pt => pt.TagId); // 外鍵是 TagId

    builder.Entity<Post>() // 設定 Post 的 Slug 索引
        .HasIndex(p => p.Slug) // 對 Slug 建立索引
        .IsUnique(); // 設定為唯一索引
}
```

### 文章 Service 完整實作

```csharp
// 文章 Service // Post service
public class PostService : IPostService // 實作文章服務
{
    private readonly AppDbContext _context; // 資料庫上下文
    private readonly MarkdownService _markdown; // Markdown 服務

    public PostService(AppDbContext context, MarkdownService markdown) // 建構函式
    {
        _context = context; // 儲存 DbContext
        _markdown = markdown; // 儲存 Markdown 服務
    }

    public async Task<Post> CreatePostAsync(PostEditorVM vm) // 建立文章方法
    {
        var post = new Post // 建立文章物件
        {
            Title = vm.Title, // 設定標題
            Slug = SlugGenerator.Generate(vm.Title), // 自動產生 Slug
            MarkdownContent = vm.MarkdownContent, // 儲存 Markdown 原始碼
            HtmlContent = _markdown.ToHtml(vm.MarkdownContent), // 轉換為 HTML
            Excerpt = _markdown.ToPlainText(vm.MarkdownContent), // 產生摘要
            CategoryId = vm.CategoryId, // 設定分類
            IsPublished = vm.IsPublished, // 設定是否發佈
            PublishedAt = vm.IsPublished ? DateTime.UtcNow : null, // 發佈時間
        };

        // 處理標籤 // Handle tags
        if (!string.IsNullOrEmpty(vm.TagIds)) // 如果有選標籤
        {
            var tagIds = vm.TagIds.Split(',') // 分割標籤 ID 字串
                .Select(int.Parse) // 轉為整數
                .ToList(); // 轉為清單
            post.PostTags = tagIds.Select(tid => new PostTag // 建立關聯
            {
                TagId = tid // 設定標籤 ID
            }).ToList(); // 轉為清單
        }

        _context.Posts.Add(post); // 加入追蹤
        await _context.SaveChangesAsync(); // 儲存到資料庫
        return post; // 回傳建立的文章
    }

    public async Task<List<Post>> GetPublishedPostsAsync( // 取得已發佈文章
        int page, int pageSize, string? category = null, string? tag = null) // 支援篩選
    {
        var query = _context.Posts // 從文章資料表開始
            .Include(p => p.Category) // 載入分類
            .Include(p => p.PostTags).ThenInclude(pt => pt.Tag) // 載入標籤
            .Where(p => p.IsPublished) // 只取已發佈的
            .AsQueryable(); // 轉為可查詢物件

        if (!string.IsNullOrEmpty(category)) // 如果有指定分類
            query = query.Where(p => p.Category!.Slug == category); // 篩選分類

        if (!string.IsNullOrEmpty(tag)) // 如果有指定標籤
            query = query.Where(p => p.PostTags.Any(pt => pt.Tag.Slug == tag)); // 篩選標籤

        return await query // 執行查詢
            .OrderByDescending(p => p.PublishedAt) // 依發佈時間降序
            .Skip((page - 1) * pageSize) // 分頁跳過
            .Take(pageSize) // 取指定筆數
            .ToListAsync(); // 回傳結果
    }

    public async Task<Post?> GetPostBySlugAsync(string slug) // 依 Slug 取得文章
    {
        var post = await _context.Posts // 查詢文章
            .Include(p => p.Category) // 載入分類
            .Include(p => p.PostTags).ThenInclude(pt => pt.Tag) // 載入標籤
            .Include(p => p.Comments.Where(c => c.IsApproved)) // 載入已審核的留言
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished); // 找到符合的文章

        if (post != null) // 如果找到文章
        {
            post.ViewCount++; // 增加瀏覽次數
            await _context.SaveChangesAsync(); // 儲存瀏覽次數
        }

        return post; // 回傳文章
    }
}
```

---

## 留言系統

```csharp
// 留言 Model // Comment model
public class Comment // 留言類別
{
    public int Id { get; set; } // 主鍵
    [Required(ErrorMessage = ""請輸入留言內容"")] // 驗證
    [StringLength(1000, ErrorMessage = ""留言最多 1000 字"")] // 長度限制
    public string Content { get; set; } = """"; // 留言內容

    [Required(ErrorMessage = ""請輸入暱稱"")] // 驗證
    [StringLength(50)] // 長度限制
    public string AuthorName { get; set; } = """"; // 留言者暱稱

    [EmailAddress] // Email 驗證
    public string? AuthorEmail { get; set; } // 留言者 Email（選填）

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 留言時間
    public bool IsApproved { get; set; } = false; // 是否已審核

    public int PostId { get; set; } // 所屬文章 ID
    public Post? Post { get; set; } // 導覽屬性：文章

    public int? ParentId { get; set; } // 父留言 ID（回覆用）
    public Comment? Parent { get; set; } // 導覽屬性：父留言
    public List<Comment> Replies { get; set; } = new(); // 子留言清單
}

// 留言 Service // Comment service
public class CommentService // 留言服務類別
{
    private readonly AppDbContext _context; // 資料庫上下文

    public CommentService(AppDbContext context) // 建構函式
    {
        _context = context; // 儲存 DbContext
    }

    public async Task<Comment> AddCommentAsync(Comment comment) // 新增留言
    {
        // 簡單的垃圾留言過濾 // Simple spam filter
        var spamWords = new[] { ""casino"", ""viagra"", ""loan"" }; // 垃圾關鍵字清單
        var contentLower = comment.Content.ToLower(); // 轉小寫比對
        if (spamWords.Any(w => contentLower.Contains(w))) // 檢查是否含垃圾字
        {
            throw new InvalidOperationException(""留言內容包含不允許的字詞""); // 拒絕垃圾留言
        }

        comment.IsApproved = false; // 預設未審核
        comment.CreatedAt = DateTime.UtcNow; // 設定留言時間
        _context.Comments.Add(comment); // 加入追蹤
        await _context.SaveChangesAsync(); // 儲存到資料庫
        return comment; // 回傳留言
    }

    public async Task<List<Comment>> GetCommentsForPostAsync(int postId) // 取得文章留言
    {
        return await _context.Comments // 查詢留言
            .Where(c => c.PostId == postId && c.IsApproved) // 篩選已審核的
            .Where(c => c.ParentId == null) // 只取頂層留言
            .Include(c => c.Replies.Where(r => r.IsApproved)) // 載入已審核的回覆
            .OrderByDescending(c => c.CreatedAt) // 依時間降序
            .ToListAsync(); // 回傳結果
    }

    public async Task ApproveCommentAsync(int commentId) // 審核留言
    {
        var comment = await _context.Comments.FindAsync(commentId); // 找到留言
        if (comment != null) // 如果找到
        {
            comment.IsApproved = true; // 設定為已審核
            await _context.SaveChangesAsync(); // 儲存
        }
    }
}
```

---

## 圖片上傳與管理

```csharp
// 圖片上傳 Service // Image upload service
public class ImageService : IImageService // 實作圖片服務
{
    private readonly IWebHostEnvironment _env; // 網站環境
    private readonly long _maxFileSize = 5 * 1024 * 1024; // 最大 5MB
    private readonly string[] _allowedExts = { "".jpg"", "".jpeg"", "".png"", "".gif"", "".webp"" }; // 允許的副檔名

    public ImageService(IWebHostEnvironment env) // 建構函式
    {
        _env = env; // 儲存環境參考
    }

    public async Task<string> UploadImageAsync(IFormFile file) // 上傳圖片方法
    {
        // 驗證檔案大小 // Validate file size
        if (file.Length == 0) throw new ArgumentException(""檔案是空的""); // 檔案不可為空
        if (file.Length > _maxFileSize) throw new ArgumentException(""檔案大小不可超過 5MB""); // 大小限制

        // 驗證副檔名 // Validate extension
        var ext = Path.GetExtension(file.FileName).ToLower(); // 取得副檔名
        if (!_allowedExts.Contains(ext)) // 檢查副檔名
            throw new ArgumentException($""不支援的檔案格式：{ext}""); // 格式不允許

        // 產生唯一檔名 // Generate unique filename
        var fileName = $""{Guid.NewGuid()}{ext}""; // 用 GUID 產生唯一檔名
        var uploadDir = Path.Combine(_env.WebRootPath, ""uploads""); // 上傳目錄路徑

        if (!Directory.Exists(uploadDir)) // 如果目錄不存在
            Directory.CreateDirectory(uploadDir); // 建立上傳目錄

        var filePath = Path.Combine(uploadDir, fileName); // 完整檔案路徑

        // 儲存檔案 // Save file
        using var stream = new FileStream(filePath, FileMode.Create); // 建立檔案串流
        await file.CopyToAsync(stream); // 複製上傳檔案到磁碟

        return $""/uploads/{fileName}""; // 回傳相對路徑
    }

    public void DeleteImage(string imageUrl) // 刪除圖片方法
    {
        if (string.IsNullOrEmpty(imageUrl)) return; // 空路徑直接返回
        var filePath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/')); // 組合完整路徑
        if (File.Exists(filePath)) File.Delete(filePath); // 如果存在就刪除
    }
}

// Markdown 編輯器的圖片上傳 API // Image upload API for editor
[HttpPost(""/api/upload/image"")] // 圖片上傳端點
[Authorize] // 需要登入
public async Task<IActionResult> UploadImage(IFormFile file) // 上傳 Action
{
    try // 嘗試上傳
    {
        var url = await _imageService.UploadImageAsync(file); // 呼叫上傳服務
        return Ok(new { url }); // 回傳圖片 URL（給 EasyMDE 用）
    }
    catch (ArgumentException ex) // 捕捉驗證錯誤
    {
        return BadRequest(new { error = ex.Message }); // 回傳 400 錯誤
    }
}
```

---

## SEO 優化 (meta tags, sitemap, robots.txt)

```csharp
// SEO 元資料 ViewModel // SEO metadata ViewModel
public class SeoMetadata // SEO 元資料類別
{
    public string Title { get; set; } = """"; // 頁面標題
    public string Description { get; set; } = """"; // 描述
    public string? OgImage { get; set; } // Open Graph 圖片
    public string? CanonicalUrl { get; set; } // 標準網址
    public string? Author { get; set; } // 作者
}

// Sitemap 產生器 // Sitemap generator
[Route(""sitemap.xml"")] // 設定路由
public async Task<IActionResult> Sitemap() // Sitemap Action
{
    var posts = await _context.Posts // 查詢所有已發佈文章
        .Where(p => p.IsPublished) // 只取已發佈的
        .Select(p => new { p.Slug, p.PublishedAt }) // 只取需要的欄位
        .ToListAsync(); // 執行查詢

    var sb = new StringBuilder(); // 建立字串建構器
    sb.AppendLine(""<?xml version=\""1.0\"" encoding=\""UTF-8\""?>""); // XML 宣告
    sb.AppendLine(""<urlset xmlns=\""http://www.sitemaps.org/schemas/sitemap/0.9\"">""); // Sitemap 根元素

    // 首頁 // Homepage
    sb.AppendLine(""  <url>""); // URL 元素開始
    sb.AppendLine($""    <loc>{Request.Scheme}://{Request.Host}/</loc>""); // 首頁網址
    sb.AppendLine(""    <changefreq>daily</changefreq>""); // 更新頻率：每天
    sb.AppendLine(""    <priority>1.0</priority>""); // 優先度：最高
    sb.AppendLine(""  </url>""); // URL 元素結束

    foreach (var post in posts) // 逐一加入文章
    {
        sb.AppendLine(""  <url>""); // URL 元素開始
        sb.AppendLine($""    <loc>{Request.Scheme}://{Request.Host}/blog/{post.Slug}</loc>""); // 文章網址
        sb.AppendLine($""    <lastmod>{post.PublishedAt:yyyy-MM-dd}</lastmod>""); // 最後修改日
        sb.AppendLine(""    <changefreq>monthly</changefreq>""); // 更新頻率：每月
        sb.AppendLine(""    <priority>0.8</priority>""); // 優先度
        sb.AppendLine(""  </url>""); // URL 元素結束
    }

    sb.AppendLine(""</urlset>""); // 根元素結束
    return Content(sb.ToString(), ""application/xml""); // 回傳 XML
}

// robots.txt // robots.txt endpoint
[Route(""robots.txt"")] // 設定路由
public IActionResult Robots() // robots.txt Action
{
    var content = $""User-agent: *\n"" + // 所有搜尋引擎
                  $""Allow: /\n"" + // 允許存取所有頁面
                  $""Disallow: /admin/\n"" + // 禁止存取管理後台
                  $""Disallow: /api/\n"" + // 禁止存取 API
                  $""Sitemap: {Request.Scheme}://{Request.Host}/sitemap.xml""; // Sitemap 位置
    return Content(content, ""text/plain""); // 回傳純文字
}
```

---

## RSS Feed 產生

```csharp
// RSS Feed 產生器 // RSS feed generator
[Route(""feed.xml"")] // 設定路由
public async Task<IActionResult> RssFeed() // RSS Feed Action
{
    var posts = await _context.Posts // 查詢文章
        .Where(p => p.IsPublished) // 已發佈的
        .OrderByDescending(p => p.PublishedAt) // 依發佈時間降序
        .Take(20) // 取最新 20 篇
        .ToListAsync(); // 執行查詢

    var sb = new StringBuilder(); // 建立字串建構器
    sb.AppendLine(""<?xml version=\""1.0\"" encoding=\""UTF-8\""?>""); // XML 宣告
    sb.AppendLine(""<rss version=\""2.0\"">""); // RSS 根元素
    sb.AppendLine(""  <channel>""); // 頻道開始
    sb.AppendLine(""    <title>我的部落格</title>""); // 頻道標題
    sb.AppendLine($""    <link>{Request.Scheme}://{Request.Host}</link>""); // 網站網址
    sb.AppendLine(""    <description>技術部落格</description>""); // 頻道描述
    sb.AppendLine($""    <language>zh-TW</language>""); // 語言

    foreach (var post in posts) // 逐一加入文章
    {
        sb.AppendLine(""    <item>""); // 項目開始
        sb.AppendLine($""      <title>{WebUtility.HtmlEncode(post.Title)}</title>""); // 文章標題
        sb.AppendLine($""      <link>{Request.Scheme}://{Request.Host}/blog/{post.Slug}</link>""); // 文章連結
        sb.AppendLine($""      <description>{WebUtility.HtmlEncode(post.Excerpt ?? """")}</description>""); // 摘要
        sb.AppendLine($""      <pubDate>{post.PublishedAt:R}</pubDate>""); // 發佈日期（RFC 822）
        sb.AppendLine($""      <guid>{Request.Scheme}://{Request.Host}/blog/{post.Slug}</guid>""); // 唯一識別碼
        sb.AppendLine(""    </item>""); // 項目結束
    }

    sb.AppendLine(""  </channel>""); // 頻道結束
    sb.AppendLine(""</rss>""); // RSS 結束
    return Content(sb.ToString(), ""application/rss+xml""); // 回傳 RSS XML
}
```

---

## 管理員後台

```csharp
// 管理後台 Controller // Admin controller
[Authorize(Roles = ""Admin"")] // 整個 Controller 限定 Admin
[Route(""admin"")] // 路由前綴
public class AdminController : Controller // 管理後台 Controller
{
    private readonly IPostService _postService; // 文章服務
    private readonly CommentService _commentService; // 留言服務

    public AdminController(IPostService postService, CommentService commentService) // 建構函式
    {
        _postService = postService; // 儲存文章服務
        _commentService = commentService; // 儲存留言服務
    }

    // 後台首頁：儀表板 // Dashboard
    [HttpGet("""")] // admin/
    public async Task<IActionResult> Dashboard() // 儀表板 Action
    {
        var stats = new DashboardVM // 建立儀表板 ViewModel
        {
            TotalPosts = await _postService.GetTotalPostCountAsync(), // 文章總數
            PublishedPosts = await _postService.GetPublishedCountAsync(), // 已發佈數
            DraftPosts = await _postService.GetDraftCountAsync(), // 草稿數
            PendingComments = await _commentService.GetPendingCountAsync(), // 待審核留言
            TotalViews = await _postService.GetTotalViewsAsync(), // 總瀏覽次數
        };
        return View(stats); // 回傳儀表板
    }

    // 文章管理列表 // Post management list
    [HttpGet(""posts"")] // admin/posts
    public async Task<IActionResult> Posts(int page = 1) // 文章管理 Action
    {
        var posts = await _postService.GetAllPostsAsync(page, 20); // 取得所有文章（含草稿）
        return View(posts); // 回傳文章列表
    }

    // 待審核留言 // Pending comments
    [HttpGet(""comments/pending"")] // admin/comments/pending
    public async Task<IActionResult> PendingComments() // 待審核留言 Action
    {
        var comments = await _commentService.GetPendingCommentsAsync(); // 取得待審核留言
        return View(comments); // 回傳留言列表
    }

    // 審核留言 // Approve comment
    [HttpPost(""comments/{id}/approve"")] // admin/comments/5/approve
    public async Task<IActionResult> ApproveComment(int id) // 審核 Action
    {
        await _commentService.ApproveCommentAsync(id); // 審核通過
        TempData[""Success""] = ""留言已審核通過""; // 成功訊息
        return RedirectToAction(nameof(PendingComments)); // 導回待審核頁
    }
}

// 儀表板 ViewModel // Dashboard ViewModel
public class DashboardVM // 儀表板資料
{
    public int TotalPosts { get; set; } // 文章總數
    public int PublishedPosts { get; set; } // 已發佈數
    public int DraftPosts { get; set; } // 草稿數
    public int PendingComments { get; set; } // 待審核留言數
    public int TotalViews { get; set; } // 總瀏覽次數
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：Slug 沒有做唯一性檢查

```csharp
// ❌ 錯誤：直接用標題產生 Slug，沒檢查重複 // Mistake: no uniqueness check
var slug = SlugGenerator.Generate(post.Title); // 如果兩篇文章同名就爆了
post.Slug = slug; // 直接存入

// ✅ 正確：檢查重複並加上後綴 // Correct: check and append suffix
public async Task<string> GenerateUniqueSlugAsync(string title) // 產生唯一 Slug
{
    var slug = SlugGenerator.Generate(title); // 先產生基本 Slug
    var baseSlug = slug; // 保存原始 Slug
    var counter = 1; // 計數器

    while (await _context.Posts.AnyAsync(p => p.Slug == slug)) // 如果已存在
    {
        slug = $""{baseSlug}-{counter}""; // 加上數字後綴
        counter++; // 計數器加一
    }
    return slug; // 回傳唯一的 Slug
}
```

### ❌ 錯誤 2：圖片上傳沒有驗證內容

```csharp
// ❌ 錯誤：只檢查副檔名 // Mistake: only checking extension
if (file.FileName.EndsWith("".jpg"")) // 只看副檔名
{
    // 有人可以把 .exe 改名為 .jpg 上傳！ // Someone could rename .exe to .jpg!
}

// ✅ 正確：也要檢查檔案內容 // Correct: also check file content
public bool IsValidImage(IFormFile file) // 驗證是否為真正的圖片
{
    var ext = Path.GetExtension(file.FileName).ToLower(); // 檢查副檔名
    if (!_allowedExts.Contains(ext)) return false; // 副檔名不對就拒絕

    using var reader = new BinaryReader(file.OpenReadStream()); // 讀取檔案內容
    var headerBytes = reader.ReadBytes(4); // 讀取前 4 個位元組
    var header = BitConverter.ToString(headerBytes); // 轉為 16 進位字串

    return header.StartsWith(""FF-D8"") // JPEG 檔頭
        || header.StartsWith(""89-50-4E-47"") // PNG 檔頭
        || header.StartsWith(""47-49-46""); // GIF 檔頭
}
```

### ❌ 錯誤 3：留言沒有防止 XSS 攻擊

```csharp
// ❌ 錯誤：直接顯示留言內容 // Mistake: rendering raw comment content
@Html.Raw(comment.Content) // 如果留言包含 <script> 標籤就完蛋了！

// ✅ 正確：一定要 HTML 編碼 // Correct: always HTML encode
@comment.Content // Razor 預設會自動編碼
// 或者手動編碼 // Or manually encode
@Html.Encode(comment.Content) // 手動 HTML 編碼
```

---

## 📋 本章重點

| 功能 | 關鍵技術 | 注意事項 |
|------|----------|----------|
| Markdown 編輯 | Markdig + EasyMDE | 儲存原始碼和 HTML |
| 多對多標籤 | PostTag 中間表 | 設定複合主鍵 |
| 圖片上傳 | IFormFile + GUID 檔名 | 驗證副檔名和內容 |
| SEO | Sitemap + meta tags | Slug 要唯一 |
| 留言 | 審核機制 + 防 XSS | 永遠不用 Html.Raw |

> 🎯 **下一步**：來建構 RESTful API 微服務！
" },

        // ── Project Chapter 553 ────────────────────────────
        new() { Id=553, Category="project", Order=4, Level="advanced", Icon="🔌", Title="實戰：RESTful API 微服務", Slug="project-restful-api-microservice", IsPublished=true, Content=@"
# 實戰：RESTful API 微服務

## API 專案架構 (Clean Architecture)

> 💡 **比喻：洋蔥的分層**
> Clean Architecture 像洋蔥一樣層層包裹：
> - **最內層（核心）**：Domain 實體和商業規則，不依賴任何外部套件
> - **第二層**：Application 層，定義 Use Case
> - **第三層**：Infrastructure，實作資料庫、API 等
> - **最外層**：Presentation（Controller），接收 HTTP 請求
>
> 關鍵原則：**內層不知道外層的存在**，依賴方向永遠是由外往內。

### 專案結構

```
MyApi.sln
├── src/
│   ├── MyApi.Domain/              ← 核心層：實體 + 介面
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   └── TodoItem.cs
│   │   └── Interfaces/
│   │       ├── IUserRepository.cs
│   │       └── ITodoRepository.cs
│   ├── MyApi.Application/         ← 應用層：Use Case + DTO
│   │   ├── DTOs/
│   │   │   ├── TodoCreateDto.cs
│   │   │   └── TodoResponseDto.cs
│   │   ├── Services/
│   │   │   └── TodoService.cs
│   │   └── Validators/
│   │       └── TodoValidator.cs
│   ├── MyApi.Infrastructure/      ← 基礎層：EF Core + 外部服務
│   │   ├── Data/
│   │   │   └── AppDbContext.cs
│   │   └── Repositories/
│   │       └── TodoRepository.cs
│   └── MyApi.Api/                 ← 表現層：Controller + Middleware
│       ├── Controllers/
│       │   └── TodoController.cs
│       ├── Middleware/
│       │   └── ExceptionMiddleware.cs
│       └── Program.cs
└── tests/
    └── MyApi.Tests/               ← 測試專案
        ├── UnitTests/
        └── IntegrationTests/
```

### Domain 層實作

```csharp
// Domain 實體：TodoItem // Domain entity: TodoItem
public class TodoItem // 待辦事項實體
{
    public int Id { get; set; } // 主鍵
    public string Title { get; set; } = """"; // 標題
    public string? Description { get; set; } // 描述
    public bool IsCompleted { get; set; } = false; // 是否完成
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 建立時間
    public DateTime? CompletedAt { get; set; } // 完成時間
    public string UserId { get; set; } = """"; // 所屬使用者 ID
    public TodoPriority Priority { get; set; } = TodoPriority.Medium; // 優先等級

    public void MarkComplete() // 標記完成方法（Domain 邏輯）
    {
        if (IsCompleted) throw new InvalidOperationException(""已經完成了""); // 防止重複標記
        IsCompleted = true; // 設定為完成
        CompletedAt = DateTime.UtcNow; // 記錄完成時間
    }
}

public enum TodoPriority { Low, Medium, High, Urgent } // 優先等級列舉

// Repository 介面（定義在 Domain 層） // Repository interface in Domain layer
public interface ITodoRepository // 待辦事項 Repository 介面
{
    Task<TodoItem?> GetByIdAsync(int id); // 依 ID 查詢
    Task<List<TodoItem>> GetByUserIdAsync(string userId, bool? isCompleted = null); // 依使用者查詢
    Task<TodoItem> CreateAsync(TodoItem item); // 建立
    Task UpdateAsync(TodoItem item); // 更新
    Task DeleteAsync(int id); // 刪除
    Task<bool> ExistsAsync(int id); // 是否存在
}
```

### Application 層：DTO 和 Service

```csharp
// DTO：Data Transfer Object // DTO classes
public class TodoCreateDto // 建立待辦事項的 DTO
{
    public string Title { get; set; } = """"; // 標題
    public string? Description { get; set; } // 描述
    public TodoPriority Priority { get; set; } = TodoPriority.Medium; // 優先等級
}

public class TodoResponseDto // 回應用的 DTO
{
    public int Id { get; set; } // 主鍵
    public string Title { get; set; } = """"; // 標題
    public string? Description { get; set; } // 描述
    public bool IsCompleted { get; set; } // 是否完成
    public string Priority { get; set; } = """"; // 優先等級（字串）
    public DateTime CreatedAt { get; set; } // 建立時間
    public DateTime? CompletedAt { get; set; } // 完成時間
}

// Application Service // Application service
public class TodoService // 待辦事項服務
{
    private readonly ITodoRepository _repo; // Repository

    public TodoService(ITodoRepository repo) // 建構函式注入
    {
        _repo = repo; // 儲存 Repository
    }

    public async Task<TodoResponseDto> CreateAsync(string userId, TodoCreateDto dto) // 建立待辦
    {
        var item = new TodoItem // 從 DTO 建立實體
        {
            Title = dto.Title, // 設定標題
            Description = dto.Description, // 設定描述
            Priority = dto.Priority, // 設定優先等級
            UserId = userId // 設定所屬使用者
        };

        var created = await _repo.CreateAsync(item); // 透過 Repository 建立
        return MapToDto(created); // 轉為 DTO 回傳
    }

    public async Task<List<TodoResponseDto>> GetUserTodosAsync( // 取得使用者的待辦
        string userId, bool? isCompleted = null) // 可選篩選條件
    {
        var items = await _repo.GetByUserIdAsync(userId, isCompleted); // 查詢資料
        return items.Select(MapToDto).ToList(); // 轉為 DTO 清單
    }

    public async Task CompleteAsync(int id, string userId) // 完成待辦
    {
        var item = await _repo.GetByIdAsync(id); // 查詢待辦
        if (item == null) throw new KeyNotFoundException(""找不到此待辦事項""); // 不存在就報錯
        if (item.UserId != userId) throw new UnauthorizedAccessException(""無權操作""); // 不是自己的就拒絕
        item.MarkComplete(); // 呼叫 Domain 方法
        await _repo.UpdateAsync(item); // 儲存更新
    }

    private static TodoResponseDto MapToDto(TodoItem item) => new() // 實體轉 DTO
    {
        Id = item.Id, // 對應 ID
        Title = item.Title, // 對應標題
        Description = item.Description, // 對應描述
        IsCompleted = item.IsCompleted, // 對應完成狀態
        Priority = item.Priority.ToString(), // 列舉轉字串
        CreatedAt = item.CreatedAt, // 對應建立時間
        CompletedAt = item.CompletedAt // 對應完成時間
    };
}
```

---

## JWT 身份驗證完整實作

> 💡 **比喻：遊樂園手環**
> JWT 就像遊樂園的手環——入場時蓋章（登入取得 Token），
> 之後每個設施只要看你的手環（驗證 Token）就讓你進。
> 你不用每次都回售票亭買票（重新登入）。

```csharp
// JWT 設定 // JWT configuration in Program.cs
builder.Services.AddAuthentication(options => // 設定驗證方案
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // 預設使用 JWT
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // 挑戰也用 JWT
})
.AddJwtBearer(options => // 設定 JWT Bearer
{
    options.TokenValidationParameters = new TokenValidationParameters // 設定驗證參數
    {
        ValidateIssuer = true, // 驗證發行者
        ValidateAudience = true, // 驗證接收者
        ValidateLifetime = true, // 驗證有效期限
        ValidateIssuerSigningKey = true, // 驗證簽名金鑰
        ValidIssuer = builder.Configuration[""Jwt:Issuer""], // 有效發行者
        ValidAudience = builder.Configuration[""Jwt:Audience""], // 有效接收者
        IssuerSigningKey = new SymmetricSecurityKey( // 簽名金鑰
            Encoding.UTF8.GetBytes(builder.Configuration[""Jwt:Key""]!)), // 從設定檔讀取
        ClockSkew = TimeSpan.Zero // 不允許時間偏差
    };
});

// JWT Token 產生服務 // JWT token generation service
public class JwtService // JWT 服務類別
{
    private readonly IConfiguration _config; // 設定檔

    public JwtService(IConfiguration config) // 建構函式
    {
        _config = config; // 儲存設定
    }

    public string GenerateToken(User user) // 產生 Token 方法
    {
        var claims = new List<Claim> // 建立 Claims 清單
        {
            new(ClaimTypes.NameIdentifier, user.Id), // 使用者 ID
            new(ClaimTypes.Email, user.Email), // Email
            new(ClaimTypes.Name, user.UserName), // 使用者名稱
            new(ClaimTypes.Role, user.Role), // 角色
            new(""jti"", Guid.NewGuid().ToString()) // JWT ID（唯一識別碼）
        };

        var key = new SymmetricSecurityKey( // 建立簽名金鑰
            Encoding.UTF8.GetBytes(_config[""Jwt:Key""]!)); // 從設定讀取密鑰
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // 設定簽名演算法

        var token = new JwtSecurityToken( // 建立 JWT Token
            issuer: _config[""Jwt:Issuer""], // 發行者
            audience: _config[""Jwt:Audience""], // 接收者
            claims: claims, // Claims
            expires: DateTime.UtcNow.AddHours(2), // 2 小時後過期
            signingCredentials: creds // 簽名憑證
        );

        return new JwtSecurityTokenHandler().WriteToken(token); // 序列化為字串
    }

    public string GenerateRefreshToken() // 產生 Refresh Token
    {
        var randomBytes = new byte[64]; // 建立 64 bytes 陣列
        using var rng = RandomNumberGenerator.Create(); // 建立安全亂數產生器
        rng.GetBytes(randomBytes); // 填入隨機位元組
        return Convert.ToBase64String(randomBytes); // 轉為 Base64 字串
    }
}

// 登入 API Controller // Auth controller
[ApiController] // 標記為 API Controller
[Route(""api/[controller]"")] // 路由：api/auth
public class AuthController : ControllerBase // 繼承 ControllerBase
{
    private readonly UserManager<User> _userManager; // Identity 使用者管理
    private readonly JwtService _jwtService; // JWT 服務

    public AuthController(UserManager<User> userManager, JwtService jwtService) // 建構函式
    {
        _userManager = userManager; // 儲存 UserManager
        _jwtService = jwtService; // 儲存 JWT 服務
    }

    [HttpPost(""login"")] // POST api/auth/login
    public async Task<IActionResult> Login([FromBody] LoginDto dto) // 登入 Action
    {
        var user = await _userManager.FindByEmailAsync(dto.Email); // 用 Email 找使用者
        if (user == null) return Unauthorized(new { message = ""帳號或密碼錯誤"" }); // 找不到

        var isValid = await _userManager.CheckPasswordAsync(user, dto.Password); // 驗證密碼
        if (!isValid) return Unauthorized(new { message = ""帳號或密碼錯誤"" }); // 密碼錯誤

        var token = _jwtService.GenerateToken(user); // 產生 JWT Token
        var refreshToken = _jwtService.GenerateRefreshToken(); // 產生 Refresh Token

        user.RefreshToken = refreshToken; // 儲存 Refresh Token
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // 設定 7 天有效期
        await _userManager.UpdateAsync(user); // 更新使用者資料

        return Ok(new // 回傳 Token
        {
            token, // JWT Token
            refreshToken, // Refresh Token
            expiresIn = 7200 // 有效秒數
        });
    }
}
```

---

## Swagger/OpenAPI 文件

```csharp
// Program.cs 設定 Swagger // Configure Swagger in Program.cs
builder.Services.AddSwaggerGen(options => // 加入 Swagger 產生器
{
    options.SwaggerDoc(""v1"", new OpenApiInfo // 設定文件資訊
    {
        Title = ""MyApi"", // API 標題
        Version = ""v1"", // 版本號
        Description = ""我的 RESTful API 服務"", // 描述
        Contact = new OpenApiContact // 聯絡資訊
        {
            Name = ""開發者"", // 聯絡人名稱
            Email = ""dev@example.com"" // 聯絡 Email
        }
    });

    // 設定 JWT 驗證 // Configure JWT auth in Swagger
    options.AddSecurityDefinition(""Bearer"", new OpenApiSecurityScheme // 定義安全機制
    {
        Name = ""Authorization"", // Header 名稱
        Type = SecuritySchemeType.Http, // 類型：HTTP
        Scheme = ""bearer"", // 方案：bearer
        BearerFormat = ""JWT"", // 格式：JWT
        In = ParameterLocation.Header, // 位置：Header
        Description = ""請輸入 JWT Token"" // 說明
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement // 設定安全需求
    {
        {
            new OpenApiSecurityScheme // 參考定義
            {
                Reference = new OpenApiReference // 參考
                {
                    Type = ReferenceType.SecurityScheme, // 類型
                    Id = ""Bearer"" // 參考 ID
                }
            },
            Array.Empty<string>() // 不需要額外的 scopes
        }
    });

    // 載入 XML 文件註解 // Include XML comments
    var xmlFile = $""{Assembly.GetExecutingAssembly().GetName().Name}.xml""; // XML 檔名
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); // XML 路徑
    if (File.Exists(xmlPath)) options.IncludeXmlComments(xmlPath); // 載入註解
});
```

---

## API 版本控制

```csharp
// 設定 API 版本控制 // Configure API versioning
builder.Services.AddApiVersioning(options => // 加入版本控制服務
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // 預設版本 1.0
    options.AssumeDefaultVersionWhenUnspecified = true; // 未指定時用預設版本
    options.ReportApiVersions = true; // 在回應 Header 報告版本
    options.ApiVersionReader = ApiVersionReader.Combine( // 組合多種版本讀取器
        new UrlSegmentApiVersionReader(), // 從 URL 讀取：/api/v1/todos
        new HeaderApiVersionReader(""X-Api-Version""), // 從 Header 讀取
        new QueryStringApiVersionReader(""api-version"")); // 從 Query String 讀取
});

// V1 Controller // Version 1 controller
[ApiController] // API Controller
[ApiVersion(""1.0"")] // 版本 1.0
[Route(""api/v{version:apiVersion}/[controller]"")] // 路由包含版本號
public class TodosV1Controller : ControllerBase // V1 Controller
{
    [HttpGet] // GET api/v1/todos
    public async Task<IActionResult> GetAll() // 取得所有待辦（V1）
    {
        var items = await _service.GetUserTodosAsync(UserId); // 查詢待辦
        return Ok(items); // 回傳 V1 格式
    }
}

// V2 Controller（加入分頁） // Version 2 controller with pagination
[ApiController] // API Controller
[ApiVersion(""2.0"")] // 版本 2.0
[Route(""api/v{version:apiVersion}/[controller]"")] // 路由包含版本號
public class TodosV2Controller : ControllerBase // V2 Controller
{
    [HttpGet] // GET api/v2/todos?page=1&pageSize=10
    public async Task<IActionResult> GetAll( // 取得所有待辦（V2，含分頁）
        [FromQuery] int page = 1, // 頁碼參數
        [FromQuery] int pageSize = 10) // 每頁筆數參數
    {
        var result = await _service.GetPagedAsync(UserId, page, pageSize); // 查詢分頁結果
        return Ok(new // 回傳 V2 格式（含分頁資訊）
        {
            data = result.Items, // 資料
            pagination = new // 分頁資訊
            {
                currentPage = page, // 目前頁碼
                pageSize, // 每頁筆數
                totalItems = result.TotalCount, // 總筆數
                totalPages = result.TotalPages // 總頁數
            }
        });
    }
}
```

---

## 中間件管線設計

```csharp
// 全域例外處理中間件 // Global exception handling middleware
public class ExceptionMiddleware // 例外處理中間件
{
    private readonly RequestDelegate _next; // 下一個中間件
    private readonly ILogger<ExceptionMiddleware> _logger; // 日誌記錄器

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) // 建構函式
    {
        _next = next; // 儲存下一個中間件
        _logger = logger; // 儲存日誌記錄器
    }

    public async Task InvokeAsync(HttpContext context) // 中間件執行方法
    {
        try // 嘗試執行後續管線
        {
            await _next(context); // 執行下一個中間件
        }
        catch (KeyNotFoundException ex) // 捕捉找不到的例外
        {
            _logger.LogWarning(ex, ""資源不存在""); // 記錄警告
            context.Response.StatusCode = 404; // 設定 404 狀態碼
            await context.Response.WriteAsJsonAsync(new // 回傳 JSON 錯誤
            {
                status = 404, // 狀態碼
                message = ex.Message // 錯誤訊息
            });
        }
        catch (UnauthorizedAccessException ex) // 捕捉未授權例外
        {
            _logger.LogWarning(ex, ""未授權的存取""); // 記錄警告
            context.Response.StatusCode = 403; // 設定 403 狀態碼
            await context.Response.WriteAsJsonAsync(new // 回傳 JSON 錯誤
            {
                status = 403, // 狀態碼
                message = ""您沒有權限執行此操作"" // 錯誤訊息
            });
        }
        catch (Exception ex) // 捕捉所有其他例外
        {
            _logger.LogError(ex, ""未預期的錯誤""); // 記錄錯誤
            context.Response.StatusCode = 500; // 設定 500 狀態碼
            await context.Response.WriteAsJsonAsync(new // 回傳 JSON 錯誤
            {
                status = 500, // 狀態碼
                message = ""伺服器發生錯誤，請稍後再試"" // 不要暴露內部錯誤
            });
        }
    }
}

// 請求計時中間件 // Request timing middleware
public class RequestTimingMiddleware // 請求計時中間件
{
    private readonly RequestDelegate _next; // 下一個中間件
    private readonly ILogger<RequestTimingMiddleware> _logger; // 日誌記錄器

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger) // 建構函式
    {
        _next = next; // 儲存下一個中間件
        _logger = logger; // 儲存日誌記錄器
    }

    public async Task InvokeAsync(HttpContext context) // 中間件執行方法
    {
        var sw = Stopwatch.StartNew(); // 開始計時
        await _next(context); // 執行後續管線
        sw.Stop(); // 停止計時

        var elapsed = sw.ElapsedMilliseconds; // 取得經過毫秒數
        if (elapsed > 500) // 如果超過 500ms
        {
            _logger.LogWarning(""慢請求：{Method} {Path} 花了 {Elapsed}ms"", // 記錄慢請求
                context.Request.Method, context.Request.Path, elapsed); // 記錄方法、路徑、時間
        }

        context.Response.Headers[""X-Response-Time""] = $""{elapsed}ms""; // 加入回應時間 Header
    }
}

// 在 Program.cs 中註冊中間件順序 // Register middleware pipeline in Program.cs
app.UseMiddleware<RequestTimingMiddleware>(); // 1. 請求計時（最外層）
app.UseMiddleware<ExceptionMiddleware>(); // 2. 例外處理
app.UseAuthentication(); // 3. 驗證
app.UseAuthorization(); // 4. 授權
app.MapControllers(); // 5. 路由到 Controller
```

---

## Docker 容器化 + docker-compose

```csharp
// Dockerfile // Dockerfile for .NET API
// --- 以下是 Dockerfile 的內容範例 --- // --- Dockerfile content example ---

// 多階段建構：第一階段（建置） // Multi-stage build: build stage
// FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build // 使用 .NET SDK 映像
// WORKDIR /src // 設定工作目錄
// COPY *.sln . // 複製方案檔
// COPY src/MyApi.Api/*.csproj src/MyApi.Api/ // 複製專案檔
// COPY src/MyApi.Domain/*.csproj src/MyApi.Domain/ // 複製 Domain 專案檔
// COPY src/MyApi.Application/*.csproj src/MyApi.Application/ // 複製 Application 專案檔
// COPY src/MyApi.Infrastructure/*.csproj src/MyApi.Infrastructure/ // 複製 Infrastructure 專案檔
// RUN dotnet restore // 還原 NuGet 套件
// COPY . . // 複製所有檔案
// RUN dotnet publish src/MyApi.Api -c Release -o /app // 發佈為 Release 版本

// 第二階段（執行） // Runtime stage
// FROM mcr.microsoft.com/dotnet/aspnet:8.0 // 使用 ASP.NET Runtime 映像
// WORKDIR /app // 設定工作目錄
// COPY --from=build /app . // 從建置階段複製產出
// EXPOSE 8080 // 開放 8080 埠
// ENTRYPOINT [""dotnet"", ""MyApi.Api.dll""] // 啟動指令

// docker-compose.yml 對應的 C# 設定 // Docker compose related C# config
// 在 Program.cs 中讀取容器環境變數 // Read container env vars in Program.cs
var dbHost = Environment.GetEnvironmentVariable(""DB_HOST"") ?? ""localhost""; // 資料庫主機
var dbPort = Environment.GetEnvironmentVariable(""DB_PORT"") ?? ""5432""; // 資料庫埠
var dbName = Environment.GetEnvironmentVariable(""DB_NAME"") ?? ""myapi""; // 資料庫名稱
var dbUser = Environment.GetEnvironmentVariable(""DB_USER"") ?? ""postgres""; // 資料庫使用者
var dbPass = Environment.GetEnvironmentVariable(""DB_PASSWORD"") ?? ""password""; // 資料庫密碼

var connStr = $""Host={dbHost};Port={dbPort};Database={dbName};"" + // 組合連線字串
              $""Username={dbUser};Password={dbPass}""; // 加上帳密

builder.Services.AddDbContext<AppDbContext>(options => // 設定 DbContext
    options.UseNpgsql(connStr)); // 使用 PostgreSQL

// 健康檢查端點（給 docker-compose 的 healthcheck 用） // Health check endpoint
builder.Services.AddHealthChecks() // 加入健康檢查服務
    .AddNpgSql(connStr, name: ""postgresql""); // 加入 PostgreSQL 健康檢查

app.MapHealthChecks(""/health""); // 對應到 /health 端點
```

### docker-compose.yml 說明

```csharp
// docker-compose.yml 中的服務對應 // Services in docker-compose.yml
// 用 C# 物件來理解結構 // Understand structure using C# objects

public class DockerComposeService // Docker Compose 服務類別
{
    public string Name { get; set; } = """"; // 服務名稱
    public string Image { get; set; } = """"; // 映像來源
    public List<string> Ports { get; set; } = new(); // 埠對應
    public Dictionary<string, string> Environment { get; set; } = new(); // 環境變數
    public List<string> DependsOn { get; set; } = new(); // 依賴的服務
}

var services = new List<DockerComposeService> // Docker Compose 服務清單
{
    new() // API 服務
    {
        Name = ""api"", // 服務名稱
        Image = ""build: ."", // 從 Dockerfile 建置
        Ports = new() { ""8080:8080"" }, // 埠對應
        Environment = new() // 環境變數
        {
            [""DB_HOST""] = ""db"", // 資料庫主機（用服務名稱）
            [""DB_PORT""] = ""5432"", // 資料庫埠
            [""DB_NAME""] = ""myapi"", // 資料庫名稱
            [""DB_USER""] = ""postgres"", // 使用者
            [""DB_PASSWORD""] = ""postgres"", // 密碼
            [""Jwt__Key""] = ""your-super-secret-key-at-least-32-chars"", // JWT 金鑰
        },
        DependsOn = new() { ""db"" } // 依賴資料庫服務
    },
    new() // PostgreSQL 服務
    {
        Name = ""db"", // 服務名稱
        Image = ""postgres:16"", // PostgreSQL 映像
        Ports = new() { ""5432:5432"" }, // 埠對應
        Environment = new() // 環境變數
        {
            [""POSTGRES_DB""] = ""myapi"", // 預設資料庫
            [""POSTGRES_USER""] = ""postgres"", // 預設使用者
            [""POSTGRES_PASSWORD""] = ""postgres"" // 預設密碼
        }
    }
};
```

---

## 整合測試 (WebApplicationFactory)

```csharp
// 整合測試基底類別 // Integration test base class
public class ApiTestBase : IClassFixture<WebApplicationFactory<Program>> // 測試基底
{
    protected readonly HttpClient _client; // HTTP 客戶端
    protected readonly WebApplicationFactory<Program> _factory; // Web 應用程式工廠

    public ApiTestBase(WebApplicationFactory<Program> factory) // 建構函式
    {
        _factory = factory.WithWebHostBuilder(builder => // 自訂 Web Host
        {
            builder.ConfigureServices(services => // 設定測試用的服務
            {
                // 移除正式的 DbContext // Remove production DbContext
                var descriptor = services.SingleOrDefault( // 找到 DbContext 的註冊
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)); // 比對型別
                if (descriptor != null) services.Remove(descriptor); // 移除它

                // 改用 In-Memory 資料庫 // Use in-memory database
                services.AddDbContext<AppDbContext>(options => // 重新註冊 DbContext
                {
                    options.UseInMemoryDatabase(""TestDb""); // 使用記憶體資料庫
                });
            });
        });
        _client = _factory.CreateClient(); // 建立測試用的 HTTP 客戶端
    }

    protected async Task<string> GetTokenAsync() // 取得測試用的 JWT Token
    {
        var loginDto = new { Email = ""test@test.com"", Password = ""Test1234!"" }; // 測試帳號
        var response = await _client.PostAsJsonAsync(""/api/auth/login"", loginDto); // 登入
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>(); // 讀取回應
        return result!.Token; // 回傳 Token
    }

    protected void SetAuthHeader(string token) // 設定驗證 Header
    {
        _client.DefaultRequestHeaders.Authorization = // 設定 Authorization Header
            new AuthenticationHeaderValue(""Bearer"", token); // Bearer Token
    }
}

// Todo API 整合測試 // Todo API integration tests
public class TodoApiTests : ApiTestBase // 繼承測試基底
{
    public TodoApiTests(WebApplicationFactory<Program> factory) : base(factory) { } // 建構函式

    [Fact] // 標記為測試方法
    public async Task CreateTodo_WithValidData_ReturnsCreated() // 測試：有效資料回傳 201
    {
        // Arrange // 準備
        var token = await GetTokenAsync(); // 取得 Token
        SetAuthHeader(token); // 設定驗證
        var dto = new { Title = ""寫測試"", Priority = ""High"" }; // 建立測試資料

        // Act // 執行
        var response = await _client.PostAsJsonAsync(""/api/v1/todos"", dto); // 發送 POST 請求

        // Assert // 驗證
        Assert.Equal(HttpStatusCode.Created, response.StatusCode); // 狀態碼應為 201
        var todo = await response.Content.ReadFromJsonAsync<TodoResponseDto>(); // 讀取回應
        Assert.Equal(""寫測試"", todo!.Title); // 標題應該正確
        Assert.False(todo.IsCompleted); // 預設未完成
    }

    [Fact] // 標記為測試方法
    public async Task GetTodos_WithoutAuth_ReturnsUnauthorized() // 測試：未驗證回傳 401
    {
        // Arrange // 準備（不設定 Token）
        _client.DefaultRequestHeaders.Authorization = null; // 清除驗證 Header

        // Act // 執行
        var response = await _client.GetAsync(""/api/v1/todos""); // 發送 GET 請求

        // Assert // 驗證
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode); // 應回傳 401
    }

    [Fact] // 標記為測試方法
    public async Task CompleteTodo_OwnedByUser_ReturnsOk() // 測試：完成自己的待辦
    {
        // Arrange // 準備
        var token = await GetTokenAsync(); // 取得 Token
        SetAuthHeader(token); // 設定驗證
        var createResponse = await _client.PostAsJsonAsync(""/api/v1/todos"", // 先建立一個待辦
            new { Title = ""測試完成功能"" }); // 設定標題
        var created = await createResponse.Content.ReadFromJsonAsync<TodoResponseDto>(); // 讀取建立結果

        // Act // 執行
        var response = await _client.PatchAsync( // 發送 PATCH 請求
            $""/api/v1/todos/{created!.Id}/complete"", null); // 完成待辦

        // Assert // 驗證
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // 應回傳 200
    }
}
```

---

## GitHub Actions CI/CD

```csharp
// GitHub Actions 工作流程對應的 C# 概念 // GitHub Actions workflow concept
// .github/workflows/ci-cd.yml // CI/CD configuration file

// 用 C# 物件理解 CI/CD 流程 // Understand CI/CD using C# objects
public class CiCdPipeline // CI/CD 管線類別
{
    public string Name { get; set; } = ""CI/CD Pipeline""; // 管線名稱
    public List<string> Triggers { get; set; } = new() { ""push to main"", ""pull request"" }; // 觸發條件

    public List<PipelineJob> Jobs { get; set; } = new() // 工作清單
    {
        new PipelineJob // 第一個工作：建置與測試
        {
            Name = ""build-and-test"", // 工作名稱
            RunsOn = ""ubuntu-latest"", // 執行環境
            Steps = new() // 步驟清單
            {
                ""Checkout code (actions/checkout@v4)"", // 步驟 1：取出程式碼
                ""Setup .NET 8 (actions/setup-dotnet@v4)"", // 步驟 2：安裝 .NET
                ""Restore dependencies (dotnet restore)"", // 步驟 3：還原套件
                ""Build (dotnet build --no-restore)"", // 步驟 4：編譯
                ""Run tests (dotnet test --no-build)"", // 步驟 5：執行測試
            }
        },
        new PipelineJob // 第二個工作：部署
        {
            Name = ""deploy"", // 工作名稱
            RunsOn = ""ubuntu-latest"", // 執行環境
            DependsOn = ""build-and-test"", // 依賴建置工作
            Condition = ""main branch only"", // 只在 main 分支執行
            Steps = new() // 步驟清單
            {
                ""Login to Docker Hub"", // 步驟 1：登入 Docker Hub
                ""Build Docker image"", // 步驟 2：建置 Docker 映像
                ""Push to registry"", // 步驟 3：推送到 Registry
                ""Deploy to server"", // 步驟 4：部署到伺服器
            }
        }
    };
}

public class PipelineJob // 工作類別
{
    public string Name { get; set; } = """"; // 工作名稱
    public string RunsOn { get; set; } = """"; // 執行環境
    public string? DependsOn { get; set; } // 依賴的工作
    public string? Condition { get; set; } // 執行條件
    public List<string> Steps { get; set; } = new(); // 步驟清單
}

// 在測試中驗證 CI/CD 所需的健康檢查 // Health check for CI/CD
[Fact] // 標記為測試方法
public async Task HealthCheck_ReturnsHealthy() // 健康檢查測試
{
    var response = await _client.GetAsync(""/health""); // 發送健康檢查請求
    Assert.Equal(HttpStatusCode.OK, response.StatusCode); // 應回傳 200
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：JWT 金鑰寫在程式碼裡

```csharp
// ❌ 錯誤：金鑰硬編碼 // Mistake: hardcoded secret key
var key = new SymmetricSecurityKey( // 建立金鑰
    Encoding.UTF8.GetBytes(""my-secret-key-12345"")); // 金鑰直接寫在程式碼裡！
// 這個金鑰會被 commit 到 Git，所有人都看得到 // This key is visible in Git!

// ✅ 正確：從環境變數或設定檔讀取 // Correct: read from environment
var key = new SymmetricSecurityKey( // 建立金鑰
    Encoding.UTF8.GetBytes( // 從設定讀取
        builder.Configuration[""Jwt:Key""] // 從 appsettings 讀
        ?? Environment.GetEnvironmentVariable(""JWT_KEY"") // 或從環境變數讀
        ?? throw new InvalidOperationException(""JWT Key 未設定""))); // 都沒有就報錯
```

### ❌ 錯誤 2：API 回傳 Entity 而不是 DTO

```csharp
// ❌ 錯誤：直接回傳資料庫 Entity // Mistake: returning DB entity directly
[HttpGet(""{id}"")] // GET api/todos/5
public async Task<IActionResult> Get(int id) // 取得待辦
{
    var todo = await _context.Todos // 查詢資料庫
        .Include(t => t.User) // 載入使用者（包含密碼 Hash！）
        .FirstOrDefaultAsync(t => t.Id == id); // 找到資料
    return Ok(todo); // 回傳含有敏感資料的 Entity
    // 問題：User 物件裡的 PasswordHash 也被序列化送出去了！ // Bug: PasswordHash leaked!
}

// ✅ 正確：回傳 DTO // Correct: return DTO
[HttpGet(""{id}"")] // GET api/todos/5
public async Task<IActionResult> Get(int id) // 取得待辦
{
    var dto = await _service.GetByIdAsync(id); // 透過 Service 取得 DTO
    if (dto == null) return NotFound(); // 找不到回 404
    return Ok(dto); // 回傳只包含需要欄位的 DTO
}
```

### ❌ 錯誤 3：沒有做輸入驗證

```csharp
// ❌ 錯誤：直接信任使用者輸入 // Mistake: trusting user input
[HttpPost] // POST api/todos
public async Task<IActionResult> Create([FromBody] TodoCreateDto dto) // 建立待辦
{
    // 沒有任何驗證就直接存入資料庫 // No validation at all!
    var item = new TodoItem { Title = dto.Title }; // 直接用輸入值
    _context.Todos.Add(item); // 直接存入
    await _context.SaveChangesAsync(); // 儲存
    return Ok(item); // 回傳
}

// ✅ 正確：做好輸入驗證 // Correct: validate input
[HttpPost] // POST api/todos
public async Task<IActionResult> Create([FromBody] TodoCreateDto dto) // 建立待辦
{
    if (!ModelState.IsValid) // 檢查 Model 驗證
        return BadRequest(ModelState); // 驗證失敗回 400

    if (string.IsNullOrWhiteSpace(dto.Title)) // 額外檢查標題
        return BadRequest(new { message = ""標題不可為空"" }); // 空白也不行

    if (dto.Title.Length > 200) // 檢查長度
        return BadRequest(new { message = ""標題不可超過 200 字"" }); // 太長也不行

    var result = await _service.CreateAsync(UserId, dto); // 透過 Service 建立
    return CreatedAtAction(nameof(Get), new { id = result.Id }, result); // 回傳 201
}
```

---

## 📋 本章重點

| 主題 | 關鍵要點 | 工具/套件 |
|------|----------|-----------|
| Clean Architecture | 依賴方向由外往內 | 多專案分層 |
| JWT 驗證 | Token + Refresh Token | Microsoft.AspNetCore.Authentication.JwtBearer |
| Swagger | API 文件自動產生 | Swashbuckle.AspNetCore |
| API 版本控制 | URL / Header / Query | Asp.Versioning.Mvc |
| 中間件 | 例外處理 + 計時 | 自訂 Middleware |
| Docker | 多階段建構 | Dockerfile + docker-compose |
| 整合測試 | In-Memory DB 測試 | WebApplicationFactory |
| CI/CD | 自動建置 + 部署 | GitHub Actions |

> 🎯 **恭喜完成所有專案章節！** 你現在有能力從零開始規劃並建構完整的 .NET 應用程式了！
" },
    };
}
