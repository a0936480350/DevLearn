using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Patterns
{
    public static List<Chapter> GetChapters()
    {
        return new List<Chapter>
        {
            // ── Chapter 200: SOLID 五大原則 ──
            new Chapter
            {
                Id = 200,
                Title = "SOLID 五大原則",
                Slug = "solid-principles",
                Category = "patterns",
                Order = 1,
                Level = "intermediate",
                Icon = "🏛️",
                IsPublished = true,
                Content = @"# 🏛️ SOLID 五大原則

## 📌 什麼是 SOLID？

SOLID 是五個物件導向設計原則的縮寫，就像蓋房子的**五大基本功法**，遵守它們可以讓你的程式碼更容易維護、擴充和測試。

> 想像你在經營一家餐廳，SOLID 就是經營的五大黃金法則！

---

## 🔤 S — Single Responsibility Principle（單一職責原則）

### 💡 核心概念

**一個類別只做一件事**，就像專業廚師：炒菜的廚師不應該同時負責洗碗和收銀。

### ❌ 違反的程式碼

```csharp
// 這個類別做太多事了！又管員工資料、又算薪水、又存資料庫
public class Employee
{
    public string Name { get; set; } = """"; // 員工姓名
    public decimal Salary { get; set; }      // 員工薪資

    // 計算薪水 — 這是商業邏輯
    public decimal CalculateBonus()
    {
        return Salary * 0.1m; // 獎金為薪水的 10%
    }

    // 儲存到資料庫 — 這是資料存取邏輯
    public void SaveToDatabase()
    {
        // 直接寫 SQL 存到資料庫（不應該在這裡做！）
        Console.WriteLine(""儲存員工資料到資料庫""); // 模擬存檔
    }

    // 產生報表 — 這是呈現邏輯
    public string GenerateReport()
    {
        // 產生 HTML 報表（也不應該在這裡！）
        return $""<h1>{Name}</h1><p>薪資：{Salary}</p>""; // 回傳報表
    }
}
```

### ✅ 遵守的程式碼

```csharp
// 員工類別：只負責管理員工資料
public class Employee
{
    public string Name { get; set; } = """"; // 員工姓名
    public decimal Salary { get; set; }      // 員工薪資
}

// 薪資計算服務：只負責計算薪資相關邏輯
public class SalaryCalculator
{
    // 計算獎金：傳入員工，回傳獎金金額
    public decimal CalculateBonus(Employee employee)
    {
        return employee.Salary * 0.1m; // 獎金為薪水的 10%
    }
}

// 員工倉儲：只負責資料存取
public class EmployeeRepository
{
    // 儲存員工資料到資料庫
    public void Save(Employee employee)
    {
        Console.WriteLine($""儲存 {employee.Name} 到資料庫""); // 模擬存檔
    }
}

// 報表產生器：只負責產生報表
public class EmployeeReportGenerator
{
    // 產生 HTML 報表
    public string Generate(Employee employee)
    {
        return $""<h1>{employee.Name}</h1>""; // 回傳簡單報表
    }
}
```

### 📖 解釋

把一個「什麼都做」的大類別拆成多個「各司其職」的小類別。每個類別只有**一個改變的理由**：薪資算法改了只改 `SalaryCalculator`，資料庫換了只改 `EmployeeRepository`。

---

## 🔤 O — Open-Closed Principle（開放封閉原則）

### 💡 核心概念

**對擴充開放，對修改封閉**。就像樂高積木：你可以一直往上加新的積木（擴充），但不需要拆掉原本的結構（修改）。

### ❌ 違反的程式碼

```csharp
// 每次新增折扣類型，都要修改這個方法 — 很危險！
public class DiscountCalculator
{
    // 計算折扣：每次加新類型都要改這裡
    public decimal Calculate(string customerType, decimal price)
    {
        if (customerType == ""Regular"")    // 一般客戶
            return price * 0.9m;           // 打 9 折
        else if (customerType == ""VIP"")   // VIP 客戶
            return price * 0.8m;           // 打 8 折
        else if (customerType == ""SVIP"")  // 超級 VIP
            return price * 0.7m;           // 打 7 折
        // 每次新增客戶類型都要加 else if... 容易改壞！
        return price; // 預設不打折
    }
}
```

### ✅ 遵守的程式碼

```csharp
// 定義折扣策略介面：所有折扣都要實作這個合約
public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal price); // 計算折扣後的價格
}

// 一般客戶的折扣策略
public class RegularDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal price)
    {
        return price * 0.9m; // 一般客戶打 9 折
    }
}

// VIP 客戶的折扣策略
public class VipDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal price)
    {
        return price * 0.8m; // VIP 客戶打 8 折
    }
}

// 新增 SVIP 折扣時，只需要加一個新類別，不用改原本的程式碼！
public class SvipDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal price)
    {
        return price * 0.7m; // SVIP 客戶打 7 折
    }
}

// 折扣計算器：接受任何折扣策略
public class DiscountCalculator
{
    // 傳入不同的折扣策略，就能算不同的折扣
    public decimal Calculate(IDiscountStrategy strategy, decimal price)
    {
        return strategy.ApplyDiscount(price); // 委派給策略物件處理
    }
}
```

### 📖 解釋

透過介面（interface）和多型（polymorphism），新增功能時只需要**增加新類別**，不用修改原本已經測試過的程式碼。就像手機裝 App — 手機本身不用改，裝上新 App 就有新功能！

---

## 🔤 L — Liskov Substitution Principle（里氏替換原則）

### 💡 核心概念

**子類別必須能完全替代父類別**，不能讓使用者嚇一跳。就像你點了一杯「飲料」，不管送來的是果汁還是咖啡，都應該能喝，不會送來一塊石頭。

### ❌ 違反的程式碼

```csharp
// 鳥類的基底類別
public class Bird
{
    public virtual void Fly() // 所有鳥都能飛...真的嗎？
    {
        Console.WriteLine(""我在飛！""); // 印出飛行訊息
    }
}

// 企鵝是鳥，但企鵝不會飛！
public class Penguin : Bird
{
    public override void Fly()
    {
        // 企鵝不會飛，所以丟出例外 — 這就違反了 LSP！
        throw new NotSupportedException(""企鵝不會飛！"");
    }
}

// 使用時會出問題
public class BirdWatcher
{
    public void WatchBirdFly(Bird bird) // 傳入任何鳥
    {
        bird.Fly(); // 如果傳入企鵝就會爆炸！💥
    }
}
```

### ✅ 遵守的程式碼

```csharp
// 基底類別：所有鳥都有的行為
public abstract class Bird
{
    public abstract void Move(); // 所有鳥都會移動（但方式不同）
}

// 會飛的鳥
public class Sparrow : Bird
{
    public override void Move()
    {
        Console.WriteLine(""麻雀在天上飛！""); // 麻雀用飛的移動
    }
}

// 企鵝也是鳥，但用走的移動
public class Penguin : Bird
{
    public override void Move()
    {
        Console.WriteLine(""企鵝在地上走！""); // 企鵝用走的移動
    }
}

// 如果需要「飛」的行為，用介面來區分
public interface IFlyable
{
    void Fly(); // 只有會飛的才實作這個介面
}

// 麻雀會飛，所以實作 IFlyable
public class FlyingSparrow : Bird, IFlyable
{
    public override void Move()
    {
        Console.WriteLine(""麻雀在移動""); // 基本移動行為
    }

    public void Fly()
    {
        Console.WriteLine(""麻雀在天上飛翔！""); // 飛行行為
    }
}
```

### 📖 解釋

重新設計繼承階層，讓 `Move()` 成為所有鳥共有的行為，而 `Fly()` 透過介面只給會飛的鳥。這樣任何 `Bird` 的子類別都能安全替換父類別，不會出現意外。

---

## 🔤 I — Interface Segregation Principle（介面隔離原則）

### 💡 核心概念

**介面不要太胖**！不要強迫類別實作它不需要的方法。就像餐廳菜單：素食者不應該被迫點牛排。

### ❌ 違反的程式碼

```csharp
// 太胖的介面！不是每台機器都需要所有功能
public interface IMachine
{
    void Print();   // 列印
    void Scan();    // 掃描
    void Fax();     // 傳真
    void Staple();  // 裝訂
}

// 舊式印表機只能列印，但被迫實作所有方法
public class OldPrinter : IMachine
{
    public void Print()
    {
        Console.WriteLine(""列印文件""); // 這個沒問題
    }

    public void Scan()
    {
        throw new NotSupportedException(""我不會掃描！""); // 被迫實作但做不到
    }

    public void Fax()
    {
        throw new NotSupportedException(""我不會傳真！""); // 被迫實作但做不到
    }

    public void Staple()
    {
        throw new NotSupportedException(""我不會裝訂！""); // 被迫實作但做不到
    }
}
```

### ✅ 遵守的程式碼

```csharp
// 把大介面拆成小介面，各司其職
public interface IPrinter
{
    void Print(); // 列印功能
}

public interface IScanner
{
    void Scan(); // 掃描功能
}

public interface IFaxMachine
{
    void Fax(); // 傳真功能
}

// 舊式印表機只實作需要的介面
public class OldPrinter : IPrinter
{
    public void Print()
    {
        Console.WriteLine(""列印文件""); // 只做自己會的事
    }
}

// 多功能事務機實作多個介面
public class MultiFunctionPrinter : IPrinter, IScanner, IFaxMachine
{
    public void Print()
    {
        Console.WriteLine(""列印文件""); // 列印功能
    }

    public void Scan()
    {
        Console.WriteLine(""掃描文件""); // 掃描功能
    }

    public void Fax()
    {
        Console.WriteLine(""傳真文件""); // 傳真功能
    }
}
```

### 📖 解釋

把一個「大而全」的介面拆成多個「小而專」的介面。每個類別只需要實作自己真正需要的功能，不用被迫寫一堆 `throw NotSupportedException()`。

---

## 🔤 D — Dependency Inversion Principle（依賴反轉原則）

### 💡 核心概念

**高層模組不應該依賴低層模組，兩者都應該依賴抽象**。就像電器和插座：吹風機不需要知道電力來自火力發電還是太陽能，只要有標準插座（介面）就能用。

### ❌ 違反的程式碼

```csharp
// 低層模組：寄送 Email
public class EmailService
{
    public void SendEmail(string message)
    {
        Console.WriteLine($""寄送 Email：{message}""); // 寄出 Email
    }
}

// 高層模組：直接依賴具體的 EmailService — 耦合太緊！
public class OrderProcessor
{
    private readonly EmailService _emailService; // 直接依賴具體類別

    public OrderProcessor()
    {
        _emailService = new EmailService(); // 自己 new — 無法替換！
    }

    public void ProcessOrder(string orderId)
    {
        Console.WriteLine($""處理訂單：{orderId}""); // 處理訂單邏輯
        _emailService.SendEmail($""訂單 {orderId} 已處理""); // 寄送通知
    }
}
```

### ✅ 遵守的程式碼

```csharp
// 定義抽象：通知服務的介面
public interface INotificationService
{
    void Send(string message); // 發送通知（不管用什麼方式）
}

// 實作一：Email 通知
public class EmailNotification : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($""📧 Email 通知：{message}""); // 用 Email 寄送
    }
}

// 實作二：簡訊通知
public class SmsNotification : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($""📱 簡訊通知：{message}""); // 用簡訊寄送
    }
}

// 高層模組：依賴抽象（介面），不依賴具體實作
public class OrderProcessor
{
    private readonly INotificationService _notification; // 依賴介面

    // 透過建構子注入依賴（DI）
    public OrderProcessor(INotificationService notification)
    {
        _notification = notification; // 從外部注入，可以隨時替換
    }

    public void ProcessOrder(string orderId)
    {
        Console.WriteLine($""處理訂單：{orderId}""); // 處理訂單邏輯
        _notification.Send($""訂單 {orderId} 已處理""); // 發送通知
    }
}
```

### 📖 解釋

透過**依賴注入（Dependency Injection）**，高層模組只認識介面，不認識具體實作。想從 Email 改成簡訊通知？換一行注入的程式碼就好，`OrderProcessor` 完全不用改！

---

## 🤔 我這樣寫為什麼會錯？

### 錯誤一：一個類別超過 500 行

```csharp
// ❌ 一個 UserService 做了所有事情
public class UserService
{
    public void Register() { }    // 註冊
    public void Login() { }       // 登入
    public void SendEmail() { }   // 寄信
    public void GeneratePdf() { } // 產生 PDF
    public void UploadFile() { }  // 上傳檔案
    // ... 還有 50 個方法 😱
}
// ✅ 拆成 UserAuthService、EmailService、PdfService 等
```

### 錯誤二：用 if-else 處理所有情況

```csharp
// ❌ 每次加新的付款方式都要改這裡
public decimal ProcessPayment(string method, decimal amount)
{
    if (method == ""CreditCard"") return amount * 0.97m;     // 信用卡手續費
    else if (method == ""LinePay"") return amount * 0.98m;   // Line Pay 手續費
    else if (method == ""ApplePay"") return amount * 0.985m; // Apple Pay 手續費
    // 每次都要加 else if...
    return amount; // 預設金額
}
// ✅ 用 IPaymentStrategy 介面 + 各自的實作類別
```

### 錯誤三：在建構子裡 new 所有相依物件

```csharp
// ❌ 寫死相依性，無法單元測試
public class ReportService
{
    private readonly DatabaseContext _db = new DatabaseContext();   // 寫死
    private readonly EmailService _email = new EmailService();     // 寫死
    private readonly PdfGenerator _pdf = new PdfGenerator();       // 寫死
}
// ✅ 改用建構子注入（Constructor Injection）
// public ReportService(IDatabase db, IEmailService email, IPdfGenerator pdf)
```

---

## 📝 SOLID 速查表

| 原則 | 一句話記憶 | 比喻 |
|------|-----------|------|
| **S** — 單一職責 | 一個類別只做一件事 | 專業廚師各司其職 |
| **O** — 開放封閉 | 加新功能不改舊程式碼 | 樂高積木往上疊 |
| **L** — 里氏替換 | 子類別能安全替換父類別 | 飲料都能喝 |
| **I** — 介面隔離 | 介面小而專 | 素食者不用點牛排 |
| **D** — 依賴反轉 | 依賴抽象不依賴具體 | 電器只認插座 |
"
            },

            // ── Chapter 201: 常用建立型模式 ──
            new Chapter
            {
                Id = 201,
                Title = "常用建立型模式",
                Slug = "creational-design-patterns",
                Category = "patterns",
                Order = 2,
                Level = "intermediate",
                Icon = "🏭",
                IsPublished = true,
                Content = @"# 🏭 常用建立型模式

## 📌 什麼是建立型模式？

建立型模式（Creational Patterns）專門處理**物件怎麼被建立**的問題。就像工廠有不同的生產線，每種模式都是一種聰明的「生產方式」。

> 不要直接 `new` 物件！讓建立型模式幫你優雅地管理物件的誕生。

---

## 🔷 Singleton Pattern（單例模式）

### 💡 比喻

全公司只有**一個總經理**，不管誰問「總經理是誰？」，答案永遠是同一個人。

### 🎯 問題場景

有些物件在系統中應該只存在一個實體，例如：設定檔管理器、資料庫連線池、日誌記錄器。

### 🏗️ UML 概念

```
┌──────────────────────┐
│     Singleton         │
├──────────────────────┤
│ - instance: Singleton │  ← 靜態私有欄位，存放唯一實體
│ - Singleton()         │  ← 私有建構子，外部不能 new
│ + GetInstance()        │  ← 公開靜態方法，取得唯一實體
└──────────────────────┘
```

### 📝 C# 實作

```csharp
// 基本版 Singleton — 非執行緒安全（有問題的版本）
public class BasicSingleton
{
    private static BasicSingleton? _instance; // 靜態欄位存放唯一實體

    private BasicSingleton() // 私有建構子：外面不能 new
    {
        Console.WriteLine(""Singleton 被建立了""); // 只會印一次
    }

    public static BasicSingleton GetInstance() // 取得唯一實體的方法
    {
        if (_instance == null) // 如果還沒建立過
        {
            _instance = new BasicSingleton(); // 就建立一個
        }
        return _instance; // 回傳唯一的實體
    }
}
```

### ⚠️ 多執行緒安全版本

```csharp
// 執行緒安全的 Singleton — 使用 lock
public sealed class ThreadSafeSingleton
{
    private static ThreadSafeSingleton? _instance; // 唯一實體
    private static readonly object _lock = new();  // 鎖定物件

    private ThreadSafeSingleton() // 私有建構子
    {
        Console.WriteLine(""安全的 Singleton 被建立了""); // 只會印一次
    }

    public static ThreadSafeSingleton Instance // 屬性方式取得實體
    {
        get
        {
            if (_instance == null) // 第一次檢查（避免不必要的 lock）
            {
                lock (_lock) // 鎖住！同一時間只有一個執行緒能進入
                {
                    if (_instance == null) // 第二次檢查（Double-Check Locking）
                    {
                        _instance = new ThreadSafeSingleton(); // 建立實體
                    }
                }
            }
            return _instance; // 回傳唯一實體
        }
    }
}
```

### 🎯 最推薦的寫法：用 Lazy<T>

```csharp
// 最簡潔的執行緒安全 Singleton — 用 Lazy<T>
public sealed class ModernSingleton
{
    // Lazy<T> 保證只會在第一次存取時建立，而且執行緒安全
    private static readonly Lazy<ModernSingleton> _lazy =
        new(() => new ModernSingleton()); // 延遲初始化

    private ModernSingleton() // 私有建構子
    {
        Console.WriteLine(""Modern Singleton 誕生！""); // 只會執行一次
    }

    public static ModernSingleton Instance => _lazy.Value; // 取得唯一實體
}
```

### ✅ 何時用 / ❌ 何時不用

| 適合使用 | 不適合使用 |
|---------|-----------|
| 全域設定管理 | 需要多個實體的場景 |
| 日誌記錄器 | 有狀態且會被多執行緒修改 |
| 快取管理 | 單元測試中（難以 Mock） |

---

## 🔷 Factory Pattern（工廠模式）

### 💡 比喻

工廠依訂單生產不同產品：你跟工廠說「我要一台筆電」，工廠就生產筆電；說「我要手機」，就生產手機。你不需要知道生產細節。

### 🎯 問題場景

當你需要根據條件建立不同類型的物件，又不想在呼叫端寫一堆 `if-else` 和 `new`。

### 📝 C# 實作

```csharp
// 產品介面：所有通知都要能「發送」
public interface INotification
{
    void Send(string message); // 發送通知
}

// 具體產品一：Email 通知
public class EmailNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($""📧 Email：{message}""); // 用 Email 發送
    }
}

// 具體產品二：簡訊通知
public class SmsNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($""📱 簡訊：{message}""); // 用簡訊發送
    }
}

// 具體產品三：推播通知
public class PushNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($""🔔 推播：{message}""); // 用推播發送
    }
}

// 工廠類別：根據類型建立對應的通知物件
public static class NotificationFactory
{
    // 根據傳入的類型字串，回傳對應的通知物件
    public static INotification Create(string type)
    {
        return type.ToLower() switch // 比對類型（轉小寫避免大小寫問題）
        {
            ""email"" => new EmailNotification(), // 建立 Email 通知
            ""sms""   => new SmsNotification(),   // 建立簡訊通知
            ""push""  => new PushNotification(),   // 建立推播通知
            _ => throw new ArgumentException(      // 未知類型就丟例外
                $""不支援的通知類型：{type}"")
        };
    }
}

// 使用方式
// var notification = NotificationFactory.Create(""email""); // 取得 Email 通知
// notification.Send(""你的訂單已出貨"");                     // 發送通知
```

### ✅ 何時用 / ❌ 何時不用

| 適合使用 | 不適合使用 |
|---------|-----------|
| 建立邏輯複雜且需要集中管理 | 只有一種產品類型 |
| 需要根據設定檔決定建立哪種物件 | 建立邏輯非常簡單 |
| 想要隱藏建立細節 | 物件不需要多型 |

---

## 🔷 Builder Pattern（建造者模式）

### 💡 比喻

就像去速食店點餐：先選主餐（漢堡），再加配菜（薯條），最後選飲料（可樂），一步一步組合出你想要的套餐。

### 🎯 問題場景

當一個物件有很多可選參數，建構子會變得又臭又長（Telescoping Constructor 問題）。

### 📝 C# 實作

```csharp
// 產品：一份完整的報表設定
public class ReportConfig
{
    public string Title { get; set; } = """";     // 報表標題
    public string Format { get; set; } = ""PDF"";  // 輸出格式
    public bool IncludeChart { get; set; }         // 是否包含圖表
    public bool IncludeHeader { get; set; }        // 是否包含頁首
    public bool IncludeFooter { get; set; }        // 是否包含頁尾
    public int FontSize { get; set; } = 12;        // 字體大小
    public string DateRange { get; set; } = """";  // 日期範圍

    // 顯示設定內容（方便除錯）
    public override string ToString()
    {
        return $""報表：{Title}，格式：{Format}，字體：{FontSize}pt""; // 回傳摘要
    }
}

// 建造者：一步一步建立 ReportConfig
public class ReportConfigBuilder
{
    private readonly ReportConfig _config = new(); // 內部持有要建立的產品

    // 設定標題（回傳 this 以支援鏈式呼叫）
    public ReportConfigBuilder SetTitle(string title)
    {
        _config.Title = title; // 設定報表標題
        return this;           // 回傳自己，支援鏈式呼叫
    }

    // 設定輸出格式
    public ReportConfigBuilder SetFormat(string format)
    {
        _config.Format = format; // 設定格式（PDF、Excel 等）
        return this;              // 回傳自己
    }

    // 加入圖表
    public ReportConfigBuilder WithChart()
    {
        _config.IncludeChart = true; // 啟用圖表
        return this;                  // 回傳自己
    }

    // 加入頁首
    public ReportConfigBuilder WithHeader()
    {
        _config.IncludeHeader = true; // 啟用頁首
        return this;                   // 回傳自己
    }

    // 加入頁尾
    public ReportConfigBuilder WithFooter()
    {
        _config.IncludeFooter = true; // 啟用頁尾
        return this;                   // 回傳自己
    }

    // 設定字體大小
    public ReportConfigBuilder SetFontSize(int size)
    {
        _config.FontSize = size; // 設定字型大小
        return this;              // 回傳自己
    }

    // 最終建立產品
    public ReportConfig Build()
    {
        return _config; // 回傳組裝完成的報表設定
    }
}

// 使用方式 — 鏈式呼叫，清楚易讀
// var config = new ReportConfigBuilder()
//     .SetTitle(""月報表"")       // 設定標題
//     .SetFormat(""Excel"")      // 選擇格式
//     .WithChart()              // 要圖表
//     .WithHeader()             // 要頁首
//     .SetFontSize(14)          // 字體 14pt
//     .Build();                 // 組裝完成！
```

---

## 🔷 Abstract Factory（抽象工廠模式）

### 💡 比喻

想像你在裝潢房子，選了「北歐風格」，那所有家具（沙發、桌子、燈）都會是北歐風的；選了「工業風格」，所有家具就都是工業風的。抽象工廠確保**同一個系列的產品風格一致**。

### 📝 C# 實作

```csharp
// 抽象產品一：按鈕
public interface IButton
{
    void Render(); // 渲染按鈕的方法
}

// 抽象產品二：文字輸入框
public interface ITextBox
{
    void Render(); // 渲染輸入框的方法
}

// 具體產品：Windows 風格的按鈕
public class WindowsButton : IButton
{
    public void Render()
    {
        Console.WriteLine(""[Windows 按鈕]""); // 渲染 Windows 風格按鈕
    }
}

// 具體產品：Windows 風格的輸入框
public class WindowsTextBox : ITextBox
{
    public void Render()
    {
        Console.WriteLine(""[Windows 輸入框]""); // 渲染 Windows 風格輸入框
    }
}

// 具體產品：Mac 風格的按鈕
public class MacButton : IButton
{
    public void Render()
    {
        Console.WriteLine(""(Mac 按鈕)""); // 渲染 Mac 風格按鈕
    }
}

// 具體產品：Mac 風格的輸入框
public class MacTextBox : ITextBox
{
    public void Render()
    {
        Console.WriteLine(""(Mac 輸入框)""); // 渲染 Mac 風格輸入框
    }
}

// 抽象工廠：定義建立 UI 元件的合約
public interface IUIFactory
{
    IButton CreateButton();   // 建立按鈕
    ITextBox CreateTextBox(); // 建立輸入框
}

// 具體工廠：Windows 風格的 UI 工廠
public class WindowsUIFactory : IUIFactory
{
    public IButton CreateButton() => new WindowsButton();   // 建立 Windows 按鈕
    public ITextBox CreateTextBox() => new WindowsTextBox(); // 建立 Windows 輸入框
}

// 具體工廠：Mac 風格的 UI 工廠
public class MacUIFactory : IUIFactory
{
    public IButton CreateButton() => new MacButton();   // 建立 Mac 按鈕
    public ITextBox CreateTextBox() => new MacTextBox(); // 建立 Mac 輸入框
}

// 使用方式：傳入不同工廠，就能建立不同風格的 UI
// IUIFactory factory = new WindowsUIFactory(); // 選擇 Windows 風格
// var button = factory.CreateButton();          // 建立按鈕
// var textBox = factory.CreateTextBox();        // 建立輸入框
// button.Render();                              // 渲染按鈕
// textBox.Render();                             // 渲染輸入框
```

---

## 🤔 我這樣寫為什麼會錯？

### 錯誤一：Singleton 在多執行緒環境沒有加鎖

```csharp
// ❌ 多個執行緒同時進入，可能建立多個實體！
public static Singleton GetInstance()
{
    if (_instance == null)             // 執行緒 A 和 B 同時到這裡
    {
        _instance = new Singleton();   // 兩個都會 new — 不再是 Singleton！
    }
    return _instance; // 回傳的可能不是同一個
}
// ✅ 解法：使用 lock 或 Lazy<T>（參考上面的安全版本）
```

### 錯誤二：工廠方法裡用字串比對，打錯字就爆炸

```csharp
// ❌ 字串容易打錯且沒有編譯期檢查
var service = Factory.Create(""emal""); // 打錯字！應該是 ""email""
// ✅ 解法：改用 enum 或泛型
// var service = Factory.Create<EmailService>(); // 編譯期就會檢查
```

### 錯誤三：Builder 沒有驗證必填欄位

```csharp
// ❌ 忘記設定必填的 Title 就呼叫 Build()
var config = new ReportConfigBuilder()
    .SetFontSize(14)  // 設定了字體大小
    .Build();          // 但忘了設定標題！報表沒有名字 😱
// ✅ 解法：在 Build() 裡檢查必填欄位
// if (string.IsNullOrEmpty(_config.Title))
//     throw new InvalidOperationException(""報表標題為必填！"");
```

---

## 📝 建立型模式速查表

| 模式 | 一句話記憶 | 適用場景 |
|------|-----------|---------|
| **Singleton** | 全公司只有一個總經理 | 全域唯一的服務 |
| **Factory** | 工廠依訂單生產 | 根據條件建立不同物件 |
| **Builder** | 點餐式組合 | 物件有很多可選參數 |
| **Abstract Factory** | 整套風格一致 | 建立系列相關物件 |
"
            },

            // ── Chapter 202: 常用結構型與行為型模式 ──
            new Chapter
            {
                Id = 202,
                Title = "常用結構型與行為型模式",
                Slug = "structural-behavioral-patterns",
                Category = "patterns",
                Order = 3,
                Level = "advanced",
                Icon = "🔧",
                IsPublished = true,
                Content = @"# 🔧 常用結構型與行為型模式

## 📌 結構型 vs 行為型模式

- **結構型模式**：關注類別和物件的**組合方式**，就像積木怎麼拼在一起
- **行為型模式**：關注物件之間的**溝通方式**，就像人跟人怎麼合作

---

## 🔷 Strategy Pattern（策略模式）— 行為型

### 💡 比喻

就像導航 APP 可以切換路線策略：最短路線、最快路線、避開高速公路。目的地不變，但走法可以隨時切換！

### 🎯 問題場景

同一個操作有多種演算法可以選擇，而且可能需要在**執行時期動態切換**。

### 📝 C# 實作

```csharp
// 策略介面：定義排序的合約
public interface ISortStrategy
{
    void Sort(List<int> data); // 對資料進行排序
}

// 具體策略一：泡沫排序（適合小量資料）
public class BubbleSort : ISortStrategy
{
    public void Sort(List<int> data)
    {
        Console.WriteLine(""使用泡沫排序...""); // 印出使用哪種排序
        for (int i = 0; i < data.Count - 1; i++) // 外層迴圈
        {
            for (int j = 0; j < data.Count - i - 1; j++) // 內層迴圈
            {
                if (data[j] > data[j + 1]) // 如果前面比後面大
                {
                    (data[j], data[j + 1]) = (data[j + 1], data[j]); // 交換位置
                }
            }
        }
    }
}

// 具體策略二：快速排序（適合大量資料）
public class QuickSort : ISortStrategy
{
    public void Sort(List<int> data)
    {
        Console.WriteLine(""使用快速排序...""); // 印出使用哪種排序
        QuickSortRecursive(data, 0, data.Count - 1); // 呼叫遞迴排序
    }

    private void QuickSortRecursive(List<int> data, int low, int high)
    {
        if (low < high) // 還有元素需要排序
        {
            int pivot = data[high];   // 選最後一個當基準點
            int i = low - 1;          // 小於基準點的區域邊界
            for (int j = low; j < high; j++) // 走訪每個元素
            {
                if (data[j] <= pivot) // 如果比基準點小
                {
                    i++; // 擴大小區域
                    (data[i], data[j]) = (data[j], data[i]); // 交換到小區域
                }
            }
            (data[i + 1], data[high]) = (data[high], data[i + 1]); // 基準點歸位
            QuickSortRecursive(data, low, i);      // 排序左半邊
            QuickSortRecursive(data, i + 2, high); // 排序右半邊
        }
    }
}

// 上下文：使用策略的類別
public class DataProcessor
{
    private ISortStrategy _strategy; // 持有策略的參考

    public DataProcessor(ISortStrategy strategy)
    {
        _strategy = strategy; // 從建構子注入策略
    }

    // 可以在執行時期切換策略！
    public void SetStrategy(ISortStrategy strategy)
    {
        _strategy = strategy; // 替換新的排序策略
    }

    // 處理資料：用當前策略排序
    public void Process(List<int> data)
    {
        _strategy.Sort(data); // 委派給策略物件執行排序
        Console.WriteLine(string.Join("", "", data)); // 印出排序結果
    }
}
```

### ✅ 何時用

- 需要在執行時期切換演算法
- 有一系列相似的行為，用 if-else 切換太醜
- 想避免大量的條件判斷語句

---

## 🔷 Observer Pattern（觀察者模式）— 行為型

### 💡 比喻

就像 YouTube 訂閱：你訂閱了一個頻道，每當頻道上傳新影片，所有訂閱者都會**自動收到通知**。不用自己每天去檢查有沒有新影片！

### 📝 C# 實作

```csharp
// 觀察者介面：所有訂閱者都要實作
public interface ISubscriber
{
    void Update(string channel, string videoTitle); // 收到新影片通知
}

// 被觀察者：YouTube 頻道
public class YouTubeChannel
{
    private readonly string _name;                        // 頻道名稱
    private readonly List<ISubscriber> _subscribers = []; // 訂閱者清單

    public YouTubeChannel(string name)
    {
        _name = name; // 設定頻道名稱
    }

    // 訂閱：加入訂閱者清單
    public void Subscribe(ISubscriber subscriber)
    {
        _subscribers.Add(subscriber); // 把訂閱者加進來
        Console.WriteLine(""新增一位訂閱者""); // 通知有人訂閱了
    }

    // 取消訂閱：從清單移除
    public void Unsubscribe(ISubscriber subscriber)
    {
        _subscribers.Remove(subscriber); // 把訂閱者移除
        Console.WriteLine(""移除一位訂閱者""); // 通知有人退訂了
    }

    // 上傳新影片：通知所有訂閱者
    public void UploadVideo(string title)
    {
        Console.WriteLine($""頻道 [{_name}] 上傳新影片：{title}""); // 印出新影片資訊
        foreach (var subscriber in _subscribers) // 走訪每個訂閱者
        {
            subscriber.Update(_name, title); // 逐一通知
        }
    }
}

// 具體觀察者一：Email 訂閱者
public class EmailSubscriber : ISubscriber
{
    private readonly string _email; // 信箱地址

    public EmailSubscriber(string email)
    {
        _email = email; // 設定信箱
    }

    public void Update(string channel, string videoTitle)
    {
        // 收到通知後寄 Email
        Console.WriteLine($""📧 寄送到 {_email}：{channel} 上傳了 {videoTitle}"");
    }
}

// 具體觀察者二：App 推播訂閱者
public class AppSubscriber : ISubscriber
{
    private readonly string _userId; // 使用者 ID

    public AppSubscriber(string userId)
    {
        _userId = userId; // 設定使用者 ID
    }

    public void Update(string channel, string videoTitle)
    {
        // 收到通知後推播到 App
        Console.WriteLine($""🔔 推播給 {_userId}：{channel} 上傳了 {videoTitle}"");
    }
}
```

### ✅ 何時用

- 一個物件狀態改變需要通知多個其他物件
- 不知道有多少物件需要被通知（動態增減）
- 事件驅動架構、發布-訂閱模式

---

## 🔷 Decorator Pattern（裝飾者模式）— 結構型

### 💡 比喻

像**俄羅斯套娃**（或珍珠奶茶加料）：基本款是紅茶，加珍珠變珍珠紅茶，再加椰果變珍珠椰果紅茶，一層一層往外包！

### 📝 C# 實作

```csharp
// 基底介面：飲料
public interface IBeverage
{
    string GetDescription(); // 取得飲料描述
    decimal GetCost();       // 取得飲料價格
}

// 具體元件：基本紅茶
public class BlackTea : IBeverage
{
    public string GetDescription()
    {
        return ""紅茶""; // 基本飲料名稱
    }

    public decimal GetCost()
    {
        return 30m; // 基本價格 30 元
    }
}

// 裝飾者基底類別：所有加料都繼承這個
public abstract class BeverageDecorator : IBeverage
{
    protected readonly IBeverage _beverage; // 被裝飾的飲料

    protected BeverageDecorator(IBeverage beverage)
    {
        _beverage = beverage; // 保存被裝飾的飲料
    }

    public abstract string GetDescription(); // 子類別實作描述
    public abstract decimal GetCost();       // 子類別實作價格
}

// 具體裝飾者一：加珍珠
public class PearlDecorator : BeverageDecorator
{
    public PearlDecorator(IBeverage beverage) : base(beverage) { } // 傳入被裝飾的飲料

    public override string GetDescription()
    {
        return _beverage.GetDescription() + "" + 珍珠""; // 在原本描述後面加上珍珠
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 10m; // 原價 + 珍珠加 10 元
    }
}

// 具體裝飾者二：加椰果
public class CoconutJellyDecorator : BeverageDecorator
{
    public CoconutJellyDecorator(IBeverage beverage) : base(beverage) { } // 傳入被裝飾的飲料

    public override string GetDescription()
    {
        return _beverage.GetDescription() + "" + 椰果""; // 加上椰果描述
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 15m; // 原價 + 椰果加 15 元
    }
}

// 具體裝飾者三：加鮮奶
public class MilkDecorator : BeverageDecorator
{
    public MilkDecorator(IBeverage beverage) : base(beverage) { } // 傳入被裝飾的飲料

    public override string GetDescription()
    {
        return _beverage.GetDescription() + "" + 鮮奶""; // 加上鮮奶描述
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 20m; // 原價 + 鮮奶加 20 元
    }
}

// 使用方式：一層一層包起來
// IBeverage drink = new BlackTea();                    // 基本紅茶：30 元
// drink = new PearlDecorator(drink);                   // 加珍珠：40 元
// drink = new MilkDecorator(drink);                    // 再加鮮奶：60 元
// Console.WriteLine($""{drink.GetDescription()}"");     // 紅茶 + 珍珠 + 鮮奶
// Console.WriteLine($""價格：{drink.GetCost()} 元"");   // 價格：60 元
```

---

## 🔷 Repository Pattern（倉儲模式）— 結構型

### 💡 比喻

Repository 就像圖書館的**圖書管理員**：你不需要知道書放在哪個書架、哪一層，只要跟管理員說「我要借《哈利波特》」，他就會幫你找到。

### 📝 C# 實作

```csharp
// 實體類別：產品
public class Product
{
    public int Id { get; set; }            // 產品編號
    public string Name { get; set; } = """"; // 產品名稱
    public decimal Price { get; set; }      // 產品價格
}

// 泛型倉儲介面：定義通用的 CRUD 操作
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);        // 根據 ID 查詢單筆
    Task<IEnumerable<T>> GetAllAsync();   // 查詢全部
    Task AddAsync(T entity);              // 新增一筆
    Task UpdateAsync(T entity);           // 更新一筆
    Task DeleteAsync(int id);             // 刪除一筆
}

// 產品專用的倉儲介面（可擴充特定查詢）
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByPriceRangeAsync( // 根據價格範圍查詢
        decimal minPrice, decimal maxPrice);
}

// 具體實作（以記憶體模擬，實際會用 EF Core）
public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = []; // 用 List 模擬資料庫
    private int _nextId = 1;                        // 自動遞增的 ID

    public Task<Product?> GetByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id); // 根據 ID 找產品
        return Task.FromResult(product); // 回傳找到的產品（或 null）
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Product>>(_products); // 回傳所有產品
    }

    public Task AddAsync(Product product)
    {
        product.Id = _nextId++; // 指定新的 ID
        _products.Add(product);  // 加入清單
        return Task.CompletedTask; // 完成
    }

    public Task UpdateAsync(Product product)
    {
        var index = _products.FindIndex(p => p.Id == product.Id); // 找到索引
        if (index >= 0) // 如果找到了
            _products[index] = product; // 更新資料
        return Task.CompletedTask; // 完成
    }

    public Task DeleteAsync(int id)
    {
        _products.RemoveAll(p => p.Id == id); // 移除符合 ID 的產品
        return Task.CompletedTask; // 完成
    }

    public Task<IEnumerable<Product>> GetByPriceRangeAsync(
        decimal minPrice, decimal maxPrice)
    {
        var result = _products.Where(              // 篩選價格範圍內的產品
            p => p.Price >= minPrice && p.Price <= maxPrice);
        return Task.FromResult(result); // 回傳結果
    }
}
```

---

## 🔷 Adapter Pattern（轉接器模式）— 結構型

### 💡 比喻

就像 **USB-C 轉 HDMI 的轉接頭**：你的筆電只有 USB-C 孔，但螢幕需要 HDMI 線。轉接頭讓兩個不相容的介面可以一起工作！

### 📝 C# 實作

```csharp
// 舊系統的介面（不能修改，就像 HDMI 孔不能改）
public class OldPaymentSystem
{
    // 舊系統用 XML 格式處理付款
    public void ProcessXmlPayment(string xmlData)
    {
        Console.WriteLine($""舊系統處理 XML 付款：{xmlData}""); // 用 XML 處理
    }
}

// 新系統期望的介面（就像筆電的 USB-C）
public interface IModernPayment
{
    void Pay(string jsonData); // 新系統用 JSON 格式
}

// 轉接器：讓舊系統配合新介面使用
public class PaymentAdapter : IModernPayment
{
    private readonly OldPaymentSystem _oldSystem; // 持有舊系統的參考

    public PaymentAdapter(OldPaymentSystem oldSystem)
    {
        _oldSystem = oldSystem; // 注入舊系統
    }

    public void Pay(string jsonData)
    {
        // 把 JSON 轉成 XML（轉接的核心工作！）
        var xmlData = ConvertJsonToXml(jsonData); // 格式轉換
        _oldSystem.ProcessXmlPayment(xmlData);    // 交給舊系統處理
    }

    private string ConvertJsonToXml(string json)
    {
        // 簡化的轉換邏輯（實際會用 JSON/XML 解析器）
        return $""<payment>{json}</payment>""; // 模擬 JSON 轉 XML
    }
}

// 使用方式：新程式碼只認識 IModernPayment
// var oldSystem = new OldPaymentSystem();              // 舊系統實體
// IModernPayment payment = new PaymentAdapter(oldSystem); // 用轉接器包裝
// payment.Pay(""{ \""amount\"": 100 }"");                   // 用新介面呼叫
```

### ✅ 何時用

- 需要整合第三方套件或舊系統
- 兩個已存在的介面不相容但需要合作
- 想在不修改原始碼的情況下讓不同系統串接

---

## 🤔 我這樣寫為什麼會錯？

### 錯誤一：Strategy 模式中把策略寫死在類別裡

```csharp
// ❌ 策略寫死，無法動態切換
public class Sorter
{
    public void Sort(List<int> data, string method)
    {
        if (method == ""bubble"") { /* 泡沫排序 */ }       // 寫死在這
        else if (method == ""quick"") { /* 快速排序 */ }   // 寫死在這
    }
}
// ✅ 把每種排序抽成獨立的 ISortStrategy 實作
```

### 錯誤二：Observer 沒有取消訂閱導致記憶體洩漏

```csharp
// ❌ 訂閱者被銷毀了但沒有取消訂閱
public class SomeComponent
{
    public SomeComponent(YouTubeChannel channel)
    {
        channel.Subscribe(this); // 訂閱了
        // 但從來沒有 Unsubscribe — 物件無法被 GC 回收！
    }
    // ✅ 實作 IDisposable，在 Dispose() 中呼叫 Unsubscribe()
}
```

### 錯誤三：Repository 把商業邏輯放在裡面

```csharp
// ❌ Repository 不應該包含商業邏輯
public class OrderRepository
{
    public void CreateOrder(Order order)
    {
        if (order.Total > 1000)                    // 商業邏輯不該在這裡！
            order.Discount = order.Total * 0.1m;   // 折扣計算應該在 Service 層
        _context.Orders.Add(order);                // 這才是 Repository 該做的
        _context.SaveChanges();                    // 儲存到資料庫
    }
}
// ✅ Repository 只做 CRUD，商業邏輯放在 Service 層
```

---

## 📝 模式速查表

| 模式 | 類型 | 一句話記憶 | 比喻 |
|------|------|-----------|------|
| **Strategy** | 行為型 | 動態切換演算法 | 導航 APP 切路線 |
| **Observer** | 行為型 | 訂閱自動通知 | YouTube 訂閱 |
| **Decorator** | 結構型 | 一層層加功能 | 珍奶加料 |
| **Repository** | 結構型 | 資料存取中間層 | 圖書管理員 |
| **Adapter** | 結構型 | 轉接不相容介面 | USB-C 轉 HDMI |
"
            },

            // ── Chapter 203: Clean Architecture 與專案架構 ──
            new Chapter
            {
                Id = 203,
                Title = "Clean Architecture 與專案架構",
                Slug = "clean-architecture",
                Category = "patterns",
                Order = 4,
                Level = "advanced",
                Icon = "🏗️",
                IsPublished = true,
                Content = @"# 🏗️ Clean Architecture 與專案架構

## 📌 為什麼需要好的架構？

好的架構就像蓋房子的**設計藍圖**，沒有藍圖就動工，蓋出來的房子可能牆歪、水管漏、電線亂接。軟體也一樣！

> 好的架構讓你的程式碼：容易理解、容易測試、容易擴充、容易維護。

---

## 🔷 傳統三層式架構（Layered Architecture）

### 💡 比喻

就像一棟三層樓的辦公大樓：
- **1 樓（Presentation）**：接待大廳，負責接待訪客（使用者）
- **2 樓（Business Logic）**：辦公區，負責處理業務
- **3 樓（Data Access）**：檔案室，負責存取資料

### 📝 結構

```
┌─────────────────────────┐
│    Presentation Layer    │  ← Controller、View（面對使用者）
├─────────────────────────┤
│   Business Logic Layer   │  ← Service、商業規則（核心邏輯）
├─────────────────────────┤
│    Data Access Layer     │  ← Repository、EF Core（存取資料庫）
└─────────────────────────┘
```

### 📝 C# 範例

```csharp
// === Data Access Layer（資料存取層）===
// 負責跟資料庫溝通
public class ProductRepository
{
    private readonly AppDbContext _context; // 資料庫上下文

    public ProductRepository(AppDbContext context)
    {
        _context = context; // 注入資料庫上下文
    }

    // 根據 ID 取得產品
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id); // 從資料庫查詢
    }

    // 取得所有產品
    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync(); // 回傳所有產品
    }
}

// === Business Logic Layer（商業邏輯層）===
// 負責處理商業規則
public class ProductService
{
    private readonly ProductRepository _repository; // 依賴資料存取層

    public ProductService(ProductRepository repository)
    {
        _repository = repository; // 注入 Repository
    }

    // 取得產品，加上商業規則（例如計算折扣）
    public async Task<ProductDto?> GetProductAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id); // 從 Repository 取資料
        if (product == null) return null;                  // 找不到就回傳 null

        return new ProductDto // 轉換成 DTO 回傳
        {
            Id = product.Id,             // 產品 ID
            Name = product.Name,         // 產品名稱
            Price = product.Price,       // 原價
            FinalPrice = product.Price * 0.9m // 商業邏輯：9 折優惠
        };
    }
}

// === Presentation Layer（呈現層）===
// 負責接收請求和回傳結果
// [ApiController]
// public class ProductController : ControllerBase
// {
//     private readonly ProductService _service; // 依賴商業邏輯層
//
//     public ProductController(ProductService service)
//     {
//         _service = service; // 注入 Service
//     }
//
//     [HttpGet(""{id}"")]
//     public async Task<IActionResult> Get(int id)
//     {
//         var product = await _service.GetProductAsync(id); // 呼叫 Service
//         if (product == null) return NotFound();            // 找不到回傳 404
//         return Ok(product);                                // 回傳產品資料
//     }
// }
```

### ⚠️ 三層式的問題

三層式架構的**依賴方向**是：Presentation → Business → Data Access。這表示商業邏輯**依賴**資料存取層。如果要換資料庫，商業邏輯也要跟著改！

---

## 🔷 Clean Architecture（整潔架構 / 洋蔥模型）

### 💡 比喻

想像一顆**洋蔥**：最核心的那一層是最重要的商業邏輯，外面一層一層包裹著基礎設施。核心不依賴外層，外層依賴核心！

### 📝 四層結構

```
            ┌──────────────────────────────┐
            │       Presentation           │  ← API Controller、Blazor 頁面
            │   （最外層：面對使用者）        │
            ├──────────────────────────────┤
            │       Infrastructure         │  ← EF Core、外部 API、檔案系統
            │   （外層：技術實作細節）        │
            ├──────────────────────────────┤
            │       Application            │  ← Use Case、DTO、介面定義
            │   （中層：應用程式邏輯）        │
            ├──────────────────────────────┤
            │         Domain               │  ← Entity、Value Object、商業規則
            │   （核心：最重要的商業邏輯）    │
            └──────────────────────────────┘
```

### 🔑 核心原則：依賴方向由外向內

```
Presentation → Application → Domain ← Infrastructure
                    ↑                        │
                    └────────────────────────┘
    Infrastructure 實作 Application 定義的介面
```

### 📝 各層的 C# 範例

```csharp
// ============================================================
// 🟡 Domain Layer（領域層）— 最核心，不依賴任何其他層
// ============================================================

// 領域實體：訂單
public class Order
{
    public int Id { get; private set; }                     // 訂單 ID
    public string CustomerName { get; private set; } = """"; // 客戶名稱
    public List<OrderItem> Items { get; private set; } = []; // 訂單項目清單
    public DateTime CreatedAt { get; private set; }         // 建立時間
    public OrderStatus Status { get; private set; }         // 訂單狀態

    // 建構子：建立訂單時必須有客戶名稱
    public Order(string customerName)
    {
        CustomerName = customerName;   // 設定客戶名稱
        CreatedAt = DateTime.UtcNow;   // 記錄建立時間
        Status = OrderStatus.Pending;  // 預設狀態為「待處理」
    }

    // 商業邏輯：新增訂單項目
    public void AddItem(string productName, decimal price, int quantity)
    {
        if (quantity <= 0) // 數量必須大於 0
            throw new ArgumentException(""數量必須大於零""); // 違反規則就丟例外

        Items.Add(new OrderItem(productName, price, quantity)); // 加入項目
    }

    // 商業邏輯：計算訂單總金額
    public decimal GetTotal()
    {
        return Items.Sum(item => item.Price * item.Quantity); // 加總所有項目
    }

    // 商業邏輯：確認訂單
    public void Confirm()
    {
        if (Items.Count == 0) // 沒有項目不能確認
            throw new InvalidOperationException(""空訂單無法確認""); // 丟例外
        Status = OrderStatus.Confirmed; // 改為已確認
    }
}

// 值物件：訂單項目
public class OrderItem
{
    public string ProductName { get; } // 產品名稱
    public decimal Price { get; }      // 單價
    public int Quantity { get; }       // 數量

    public OrderItem(string productName, decimal price, int quantity)
    {
        ProductName = productName; // 設定產品名稱
        Price = price;             // 設定單價
        Quantity = quantity;       // 設定數量
    }
}

// 列舉：訂單狀態
public enum OrderStatus
{
    Pending,    // 待處理
    Confirmed,  // 已確認
    Shipped,    // 已出貨
    Delivered,  // 已送達
    Cancelled   // 已取消
}

// ============================================================
// 🟢 Application Layer（應用層）— 定義介面和使用案例
// ============================================================

// 定義倉儲介面（在 Application 層定義，在 Infrastructure 層實作）
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);      // 根據 ID 查詢訂單
    Task<List<Order>> GetAllAsync();        // 查詢所有訂單
    Task AddAsync(Order order);            // 新增訂單
    Task SaveChangesAsync();               // 儲存變更
}

// 定義通知服務介面
public interface INotificationService
{
    Task SendOrderConfirmationAsync(       // 發送訂單確認通知
        string customerName, int orderId);
}

// DTO（資料傳輸物件）：用來回傳給外層
public class OrderDto
{
    public int Id { get; set; }            // 訂單 ID
    public string CustomerName { get; set; } = """"; // 客戶名稱
    public decimal Total { get; set; }     // 訂單總金額
    public string Status { get; set; } = """"; // 訂單狀態
}

// Use Case（使用案例）：建立訂單的流程
public class CreateOrderUseCase
{
    private readonly IOrderRepository _repository;        // 倉儲介面
    private readonly INotificationService _notification;  // 通知服務介面

    // 注入介面，不是具體實作！
    public CreateOrderUseCase(
        IOrderRepository repository,
        INotificationService notification)
    {
        _repository = repository;       // 注入倉儲
        _notification = notification;   // 注入通知服務
    }

    // 執行建立訂單的流程
    public async Task<OrderDto> ExecuteAsync(
        string customerName, List<(string Name, decimal Price, int Qty)> items)
    {
        var order = new Order(customerName); // 建立新訂單

        foreach (var item in items) // 逐一加入訂單項目
        {
            order.AddItem(item.Name, item.Price, item.Qty); // 加入項目
        }

        order.Confirm(); // 確認訂單（商業規則檢查）

        await _repository.AddAsync(order);       // 儲存訂單
        await _repository.SaveChangesAsync();    // 寫入資料庫

        await _notification.SendOrderConfirmationAsync( // 發送通知
            customerName, order.Id);

        return new OrderDto // 回傳 DTO
        {
            Id = order.Id,                       // 訂單 ID
            CustomerName = order.CustomerName,   // 客戶名稱
            Total = order.GetTotal(),            // 訂單總金額
            Status = order.Status.ToString()     // 訂單狀態
        };
    }
}
```

---

## 🔷 CQRS 概念（Command Query Responsibility Segregation）

### 💡 比喻

就像銀行的**存款窗口**和**查詢窗口**分開：存款（寫入）走一個流程，查餘額（讀取）走另一個流程。讀寫分離，各自優化！

### 📝 C# 概念範例

```csharp
// === Command（命令）：負責寫入操作 ===

// 建立訂單的命令
public class CreateOrderCommand
{
    public string CustomerName { get; set; } = """"; // 客戶名稱
    public List<OrderItemDto> Items { get; set; } = []; // 訂單項目
}

// 命令處理器：處理建立訂單的邏輯
public class CreateOrderHandler
{
    private readonly IOrderRepository _repository; // 寫入用的倉儲

    public CreateOrderHandler(IOrderRepository repository)
    {
        _repository = repository; // 注入倉儲
    }

    public async Task<int> HandleAsync(CreateOrderCommand command)
    {
        var order = new Order(command.CustomerName); // 建立訂單
        // ... 加入項目並儲存 ...
        await _repository.AddAsync(order);          // 寫入資料庫
        await _repository.SaveChangesAsync();       // 儲存變更
        return order.Id;                             // 回傳訂單 ID
    }
}

// === Query（查詢）：負責讀取操作 ===

// 查詢訂單的請求
public class GetOrderQuery
{
    public int OrderId { get; set; } // 要查詢的訂單 ID
}

// 查詢處理器：處理查詢訂單的邏輯
public class GetOrderHandler
{
    private readonly IOrderRepository _repository; // 讀取用的倉儲

    public GetOrderHandler(IOrderRepository repository)
    {
        _repository = repository; // 注入倉儲
    }

    public async Task<OrderDto?> HandleAsync(GetOrderQuery query)
    {
        var order = await _repository.GetByIdAsync(query.OrderId); // 查詢訂單
        if (order == null) return null; // 找不到回傳 null

        return new OrderDto // 轉成 DTO 回傳
        {
            Id = order.Id,                       // 訂單 ID
            CustomerName = order.CustomerName,   // 客戶名稱
            Total = order.GetTotal(),            // 總金額
            Status = order.Status.ToString()     // 狀態
        };
    }
}
```

---

## 🔷 Repository + Unit of Work

### 💡 比喻

**Repository** 像各科的老師，每個老師管一科（一個資料表）。**Unit of Work** 像校長，負責統一說「大家一起交成績單（SaveChanges）」，確保所有操作要嘛全部成功、要嘛全部失敗。

### 📝 C# 實作

```csharp
// Unit of Work 介面：統一管理所有 Repository 的交易
public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }     // 訂單倉儲
    IProductRepository Products { get; } // 產品倉儲
    Task<int> SaveChangesAsync();        // 統一儲存所有變更
}

// 使用方式
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork; // Unit of Work

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork; // 注入 Unit of Work
    }

    public async Task CreateOrderAsync(string customerName, int productId)
    {
        var product = await _unitOfWork.Products // 查詢產品
            .GetByIdAsync(productId);

        if (product == null) // 產品不存在
            throw new Exception(""產品不存在""); // 丟例外

        var order = new Order(customerName); // 建立訂單
        order.AddItem(product.Name, product.Price, 1); // 加入產品

        await _unitOfWork.Orders.AddAsync(order); // 新增訂單

        // 統一儲存：訂單和產品的變更一起成功或一起失敗
        await _unitOfWork.SaveChangesAsync(); // 一次性寫入資料庫
    }
}
```

---

## 🔷 實際專案資料夾結構範例

```
MyProject/                              # 方案根目錄
├── MyProject.sln                       # 方案檔
├── src/                                # 原始碼目錄
│   ├── MyProject.Domain/              # 🟡 領域層
│   │   ├── Entities/                  #    實體類別
│   │   │   ├── Order.cs               #    訂單實體
│   │   │   └── Product.cs             #    產品實體
│   │   ├── ValueObjects/             #    值物件
│   │   │   └── Money.cs               #    金額值物件
│   │   ├── Enums/                    #    列舉
│   │   │   └── OrderStatus.cs         #    訂單狀態
│   │   └── Interfaces/               #    領域介面
│   │       └── IDomainEvent.cs        #    領域事件介面
│   ├── MyProject.Application/        # 🟢 應用層
│   │   ├── DTOs/                     #    資料傳輸物件
│   │   │   ├── OrderDto.cs            #    訂單 DTO
│   │   │   └── ProductDto.cs          #    產品 DTO
│   │   ├── Interfaces/               #    倉儲及服務介面
│   │   │   ├── IOrderRepository.cs    #    訂單倉儲介面
│   │   │   └── INotificationService.cs#    通知服務介面
│   │   ├── UseCases/                 #    使用案例
│   │   │   ├── CreateOrderUseCase.cs  #    建立訂單
│   │   │   └── GetOrderUseCase.cs     #    查詢訂單
│   │   └── Mappings/                 #    物件對應設定
│   │       └── OrderProfile.cs        #    訂單的 AutoMapper 設定
│   ├── MyProject.Infrastructure/     # 🔵 基礎設施層
│   │   ├── Data/                     #    資料庫相關
│   │   │   ├── AppDbContext.cs        #    EF Core DbContext
│   │   │   └── Migrations/           #    資料庫遷移
│   │   ├── Repositories/            #    倉儲實作
│   │   │   └── OrderRepository.cs     #    訂單倉儲實作
│   │   └── Services/                #    外部服務實作
│   │       └── EmailService.cs        #    Email 服務實作
│   └── MyProject.WebApi/            # 🔴 呈現層
│       ├── Controllers/             #    API 控制器
│       │   └── OrderController.cs     #    訂單 API
│       ├── Program.cs               #    應用程式進入點
│       └── appsettings.json         #    設定檔
└── tests/                            # 測試目錄
    ├── MyProject.UnitTests/          #    單元測試
    └── MyProject.IntegrationTests/   #    整合測試
```

---

## 🤔 我這樣寫為什麼會錯？

### 錯誤一：把商業邏輯放在 Controller 裡

```csharp
// ❌ Controller 裡面寫了一堆商業邏輯
// [HttpPost]
// public async Task<IActionResult> CreateOrder(OrderRequest request)
// {
//     if (request.Items.Count == 0)        // 商業規則不該在這裡！
//         return BadRequest(""訂單不能為空"");
//     var total = request.Items.Sum(i => i.Price * i.Qty); // 計算邏輯也不該在這裡！
//     if (total > 10000)                   // 折扣邏輯更不該在這裡！
//         total *= 0.9m;
//     // ... 存資料庫 ...
// }
// ✅ Controller 只負責接收請求和回傳結果
// ✅ 商業邏輯放在 Domain 或 Application 層
```

### 錯誤二：Domain 層依賴 Infrastructure 層

```csharp
// ❌ Domain 實體直接使用 EF Core — 依賴方向錯了！
using Microsoft.EntityFrameworkCore; // Domain 層不應該引用這個！

public class Order
{
    public void Save(AppDbContext context) // Domain 不應該知道 DbContext
    {
        context.Orders.Add(this);  // 這是 Infrastructure 的工作！
        context.SaveChanges();     // Domain 不該碰資料庫！
    }
}
// ✅ Domain 層完全不知道資料庫的存在
// ✅ 在 Application 層定義 IOrderRepository 介面
// ✅ 在 Infrastructure 層實作 IOrderRepository
```

### 錯誤三：所有程式碼都塞在同一個專案裡

```
// ❌ 一個專案包含所有東西
MyProject/
├── Controllers/     # 呈現邏輯
├── Models/          # 混合了 Entity 和 DTO
├── Services/        # 混合了商業邏輯和資料存取
└── Data/            # 資料庫相關
// 結果：改一個功能要翻遍整個專案，測試也很難寫

// ✅ 依照 Clean Architecture 分成多個專案
// 每層各自獨立，依賴方向清楚，容易維護和測試
```

---

## 📝 架構選擇指南

| 專案規模 | 推薦架構 | 理由 |
|---------|---------|------|
| 小型專案（PoC、工具） | 單層或三層式 | 快速開發，不過度設計 |
| 中型專案（企業內部系統） | 三層式 + Repository | 結構清楚又不會太複雜 |
| 大型專案（商業產品） | Clean Architecture | 高度解耦，易於測試和維護 |
| 高流量系統 | Clean Architecture + CQRS | 讀寫分離，各自優化效能 |

> 💡 記住：**沒有最好的架構，只有最適合的架構**。不要為了一個小工具套用 Clean Architecture，也不要用三層式架構去做大型商業系統！
"
            }
        };
    }
}
