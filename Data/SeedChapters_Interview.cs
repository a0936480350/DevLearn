using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Interview
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Interview Chapter 560 ────────────────────────────
        new() { Id=560, Category="interview", Order=1, Level="beginner", Icon="💼", Title="面試準備策略與自我介紹", Slug="interview-preparation-strategy", IsPublished=true, Content=@"
# 面試準備策略與自我介紹

## 📌 本章重點
> 面試不只是技術能力的展現，更是**溝通與行銷自己**的過程。
> 本章教你如何系統性地準備面試，從自我介紹到薪資談判。

---

## 技術面試流程解析

> 💡 **比喻：闖關遊戲**
> 技術面試就像一個多關卡的遊戲：
> - 第一關：履歷篩選（門票）
> - 第二關：電話/線上面試（資格賽）
> - 第三關：技術考試（正式比賽）
> - 第四關：主管面試（決賽）
> - 第五關：HR 面試（頒獎典禮前的確認）

### 典型 .NET 面試流程

```
面試流程圖：

投遞履歷 → HR 篩選 → 電話面試 → 技術筆試/線上測驗
                                        ↓
                                   技術面試（1-2 輪）
                                        ↓
                                   主管/團隊面試
                                        ↓
                                   HR 談薪資
                                        ↓
                                   發 Offer ✅
```

### 各階段準備重點

```csharp
// 面試各階段的準備清單（用 Dictionary 表示） // 中文註解
var interviewStages = new Dictionary<string, List<string>>() // 建立面試階段字典
{
    [""履歷篩選""] = new List<string> // 第一階段：履歷篩選
    {
        ""確保履歷包含關鍵字：C#, .NET, ASP.NET Core"", // 關鍵字很重要
        ""量化你的成就：提升效能 30%、減少 Bug 50%"", // 用數字說話
        ""GitHub 連結必須放上去"" // 展示實際作品
    },
    [""電話面試""] = new List<string> // 第二階段：電話面試
    {
        ""準備 30 秒自我介紹"", // 簡潔有力
        ""確認公司產品和技術棧"", // 做功課
        ""準備 2-3 個問公司的問題"" // 展現興趣
    },
    [""技術面試""] = new List<string> // 第三階段：技術面試
    {
        ""C# 基礎：值型別 vs 參考型別"", // 必考題
        ""ASP.NET Core 中介軟體、DI"", // 框架知識
        ""SQL 查詢與資料庫設計"", // 資料庫能力
        ""系統設計基礎"" // 架構思維
    },
    [""行為面試""] = new List<string> // 第四階段：行為面試
    {
        ""用 STAR 法則回答"", // 結構化回答
        ""準備衝突解決的故事"", // 團隊合作
        ""準備失敗經驗的故事"" // 成長心態
    }
};

foreach (var stage in interviewStages) // 遍歷每個階段
{
    Console.WriteLine($""📋 {stage.Key}：""); // 印出階段名稱
    foreach (var item in stage.Value) // 遍歷該階段的準備項目
    {
        Console.WriteLine($""  ✅ {item}""); // 印出每個準備項目
    }
}
```

---

## 如何準備自我介紹

> 💡 **比喻：30 秒電梯簡報**
> 想像你和面試官一起搭電梯，只有 30 秒的時間。
> 你必須在電梯到達目的樓層前，讓對方記住你。
> 不需要講完所有經歷，只要讓對方想「我要跟這個人多聊聊」。

### 自我介紹公式

```
自我介紹 = 我是誰 + 我做過什麼 + 我能帶來什麼

時間控制：
- 30 秒版：電話面試用
- 1 分鐘版：正式面試用
- 3 分鐘版：深度面試用
```

### 自我介紹範例

```csharp
// 建立自我介紹模板類別 // 中文註解
public class SelfIntroduction // 自我介紹類別
{
    public string Name { get; set; } = """"; // 姓名
    public int YearsOfExperience { get; set; } // 年資
    public List<string> CoreSkills { get; set; } = new(); // 核心技能
    public string MostProudProject { get; set; } = """"; // 最自豪的專案
    public string WhyThisCompany { get; set; } = """"; // 為什麼選這間公司

    public string Generate30SecVersion() // 產生 30 秒版本
    {
        return $""您好，我是{Name}，"" + // 開頭問候
               $""有 {YearsOfExperience} 年的 .NET 開發經驗。"" + // 經歷概述
               $""擅長 {string.Join(""、"", CoreSkills)}。"" + // 核心技能
               $""最近完成了{MostProudProject}，"" + // 代表作品
               $""{WhyThisCompany}""; // 動機
    }
}

// 使用範例 // 中文註解
var intro = new SelfIntroduction // 建立自我介紹實例
{
    Name = ""王小明"", // 設定姓名
    YearsOfExperience = 3, // 設定年資
    CoreSkills = new() { ""C#"", ""ASP.NET Core"", ""SQL Server"" }, // 設定技能
    MostProudProject = ""電商平台的訂單系統重構，將回應時間降低 40%"", // 設定代表作
    WhyThisCompany = ""貴公司的微服務架構轉型讓我非常感興趣"" // 設定動機
};

Console.WriteLine(intro.Generate30SecVersion()); // 輸出自我介紹
```

### ❌ 自我介紹地雷

```
地雷 1：背稿感太重
  ❌ 「您好我叫王小明我有三年經驗我會C#......」（像機器人）
  ✅ 用自然的語氣，像在跟朋友介紹自己的工作

地雷 2：講太多無關的事
  ❌ 「我大學讀化學系，後來轉行，當初是因為......」（面試官不在意）
  ✅ 聚焦在跟這個職位相關的經歷

地雷 3：太謙虛
  ❌ 「我只是個普通的工程師......」
  ✅ 「我在過去三年主導了三個核心系統的開發」
```

---

## 履歷撰寫技巧（針對 .NET 開發者）

### 履歷結構

```csharp
// 履歷結構類別 // 中文註解
public class DotNetResume // .NET 開發者履歷類別
{
    public string Summary { get; set; } = """"; // 個人摘要（2-3 句話）
    public List<string> TechnicalSkills { get; set; } = new(); // 技術技能清單
    public List<WorkExperience> Experiences { get; set; } = new(); // 工作經歷
    public List<string> Projects { get; set; } = new(); // 個人專案
    public string GitHubUrl { get; set; } = """"; // GitHub 連結
}

public class WorkExperience // 工作經歷類別
{
    public string Company { get; set; } = """"; // 公司名稱
    public string Title { get; set; } = """"; // 職稱
    public string Duration { get; set; } = """"; // 任職期間
    public List<string> Achievements { get; set; } = new(); // 具體成就（用數字量化）
}

// 好的工作經歷範例 // 中文註解
var goodExperience = new WorkExperience // 建立工作經歷實例
{
    Company = ""ABC 科技"", // 公司名稱
    Title = ""後端工程師"", // 職稱
    Duration = ""2023/01 - 2025/12"", // 任職期間
    Achievements = new() // 具體成就清單
    {
        ""使用 ASP.NET Core 開發 RESTful API，服務日均 10 萬次請求"", // 量化規模
        ""導入 Redis 快取機制，API 回應時間降低 60%"", // 量化改善
        ""建立 CI/CD 流程，部署時間從 2 小時縮短至 15 分鐘"", // 量化效率
        ""主導資料庫效能調校，查詢效能提升 3 倍"" // 量化成果
    }
};
```

### 技術技能呈現方式

```
✅ 好的寫法（分層呈現）：
  程式語言：C#, JavaScript, TypeScript
  後端框架：ASP.NET Core, Entity Framework Core, Dapper
  資料庫：SQL Server, PostgreSQL, Redis
  雲端/DevOps：Azure App Service, Docker, GitHub Actions
  工具：Git, Visual Studio, Rider

❌ 不好的寫法（流水帳）：
  技能：C#, Java, Python, JavaScript, HTML, CSS, SQL,
        React, Vue, Angular, Docker, K8s, AWS, Azure...
  （看起來什麼都會，但什麼都不精）
```

---

## GitHub Portfolio 建立

> 💡 **比喻：作品集展覽**
> GitHub 就是工程師的作品集。
> 面試官會去看你的 GitHub，就像看畫家的畫展。
> 與其放 100 個半成品，不如放 3 個完整的作品。

### 必備專案建議

```csharp
// GitHub Portfolio 建議清單 // 中文註解
var portfolioProjects = new List<(string Name, string Description, string Tech)> // 作品集專案清單
{
    ( // 第一個專案
        ""TodoApi"", // 專案名稱
        ""RESTful API with CRUD, Authentication, Pagination"", // 專案描述
        ""ASP.NET Core + EF Core + SQL Server"" // 使用技術
    ),
    ( // 第二個專案
        ""EShopMicroservice"", // 專案名稱
        ""微服務電商平台，包含訂單、庫存、付款服務"", // 專案描述
        ""ASP.NET Core + Docker + RabbitMQ"" // 使用技術
    ),
    ( // 第三個專案
        ""BlogEngine"", // 專案名稱
        ""部落格系統含 Markdown 編輯器與留言功能"", // 專案描述
        ""ASP.NET Core MVC + Razor Pages"" // 使用技術
    )
};

foreach (var project in portfolioProjects) // 遍歷每個專案
{
    Console.WriteLine($""📁 {project.Name}""); // 印出專案名稱
    Console.WriteLine($""   描述：{project.Description}""); // 印出描述
    Console.WriteLine($""   技術：{project.Tech}""); // 印出技術棧
}
```

### README 必備內容

```
每個 GitHub 專案的 README 應包含：

1. 📋 專案簡介（一句話說明）
2. 🖼️ 截圖或 GIF 展示
3. 🛠️ 技術棧
4. 📦 如何安裝與執行
5. 📁 專案架構說明
6. 🧪 測試方式
7. 📝 學到什麼（面試加分）
```

---

## 常見行為問題（STAR 法則）

> 💡 **比喻：說故事的框架**
> STAR 法則就像寫小說的大綱：
> - **S**ituation（背景）：故事發生在哪裡？
> - **T**ask（任務）：你要解決什麼問題？
> - **A**ction（行動）：你做了什麼？
> - **R**esult（結果）：結果如何？（要有數字）

### STAR 回答範例

```csharp
// STAR 法則回答模板 // 中文註解
public class StarAnswer // STAR 回答類別
{
    public string Question { get; set; } = """"; // 面試問題
    public string Situation { get; set; } = """"; // 情境背景
    public string Task { get; set; } = """"; // 任務目標
    public string Action { get; set; } = """"; // 採取行動
    public string Result { get; set; } = """"; // 具體結果

    public void Print() // 輸出完整回答
    {
        Console.WriteLine($""問題：{Question}""); // 印出問題
        Console.WriteLine($""S - {Situation}""); // 印出情境
        Console.WriteLine($""T - {Task}""); // 印出任務
        Console.WriteLine($""A - {Action}""); // 印出行動
        Console.WriteLine($""R - {Result}""); // 印出結果
    }
}

// 範例：描述一個你解決困難問題的經驗 // 中文註解
var conflictAnswer = new StarAnswer // 建立 STAR 回答實例
{
    Question = ""請描述一個你解決困難技術問題的經驗"", // 常見面試問題
    Situation = ""在前公司，我們的 API 在尖峰時段回應時間超過 5 秒"", // 背景描述
    Task = ""我被指派在兩週內找出瓶頸並將回應時間降到 1 秒以內"", // 任務說明
    Action = ""我用 Application Insights 分析後發現 N+1 查詢問題，"" + // 行動步驟
             ""重構了 EF Core 的查詢邏輯，並加入 Redis 快取"", // 具體做法
    Result = ""回應時間從 5 秒降到 300ms，使用者滿意度提升 25%"" // 量化結果
};

conflictAnswer.Print(); // 輸出回答
```

### 常見行為問題清單

```
1. 描述一個你與團隊成員意見不合的經驗
2. 你如何處理緊急的 production bug？
3. 描述一個你主動學習新技術的經驗
4. 你如何面對 deadline 壓力？
5. 描述一個你犯過的錯誤，以及你如何從中學習
```

---

## 薪資談判基礎

```csharp
// 薪資談判的基本原則 // 中文註解
public class SalaryNegotiation // 薪資談判類別
{
    public int CurrentSalary { get; set; } // 目前薪資
    public int MarketAverage { get; set; } // 市場行情
    public int TargetSalary { get; set; } // 目標薪資
    public List<string> Leverage { get; set; } = new(); // 談判籌碼

    public string GetNegotiationStrategy() // 取得談判策略
    {
        if (TargetSalary <= MarketAverage) // 如果目標低於市場行情
            return ""你的要求合理，有信心地提出""; // 合理範圍
        else if (TargetSalary <= MarketAverage * 1.2) // 如果目標高於行情 20% 以內
            return ""需要強調你的獨特價值和具體成就""; // 需要更多佐證
        else // 如果目標遠高於行情
            return ""建議重新評估，或準備非常有力的談判籌碼""; // 可能需要調整
    }
}

// 談判技巧 // 中文註解
var tips = new List<string> // 建立談判技巧清單
{
    ""永遠不要先說數字，讓對方先出價"", // 技巧一
    ""研究市場行情（104、Glassdoor）"", // 技巧二
    ""強調你能帶來的價值，而不是你需要多少錢"", // 技巧三
    ""考慮整體 package：獎金、股票、遠端、學習資源"", // 技巧四
    ""拿到 offer 後，禮貌地要求 2-3 天考慮"" // 技巧五
};
```

---

## 面試後 Follow-up

```csharp
// 面試後追蹤信模板 // 中文註解
public class FollowUpEmail // 追蹤信類別
{
    public string InterviewerName { get; set; } = """"; // 面試官姓名
    public string CompanyName { get; set; } = """"; // 公司名稱
    public string InterviewDate { get; set; } = """"; // 面試日期
    public string KeyDiscussion { get; set; } = """"; // 面試中討論的重點

    public string GenerateEmail() // 產生追蹤信內容
    {
        return $""Subject: 感謝 {InterviewDate} 的面試機會\n\n"" + // 信件主旨
               $""{InterviewerName} 您好，\n\n"" + // 稱呼
               $""感謝您撥出寶貴時間與我面試。"" + // 表達感謝
               $""關於我們討論的{KeyDiscussion}，"" + // 提及面試重點
               $""我非常期待能加入 {CompanyName} 的團隊。\n\n"" + // 表達期待
               $""如有任何需要補充的資訊，請隨時告訴我。\n"" + // 主動提供協助
               $""祝好""; // 結尾
    }
}
```

### Follow-up 時間表

```
面試後 24 小時內：寄感謝信
面試後 1 週：如未收到回覆，禮貌詢問進度
面試後 2 週：再次追蹤，表達持續的興趣
面試後 3 週：如仍無回覆，可以開始考慮其他機會
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：自我介紹沒有結構

```csharp
// ❌ 錯誤：沒有準備，想到什麼說什麼 // 中文註解
public string BadIntroduction() // 不好的自我介紹方法
{
    return ""嗯...我叫小明...然後...我會寫 C#..."" + // 沒有結構
           ""然後...之前做過一些專案...大概就這樣""; // 太模糊
}

// ✅ 正確：使用結構化的自我介紹 // 中文註解
public string GoodIntroduction() // 好的自我介紹方法
{
    var intro = new StringBuilder(); // 使用 StringBuilder 組合介紹
    intro.AppendLine(""您好，我是王小明""); // 第一句：我是誰
    intro.AppendLine(""有 3 年 .NET 後端開發經驗""); // 第二句：經歷
    intro.AppendLine(""最近主導了訂單系統重構，效能提升 40%""); // 第三句：代表作
    intro.AppendLine(""對貴公司的微服務轉型很感興趣""); // 第四句：為什麼來
    return intro.ToString(); // 回傳完整介紹
}
```

### ❌ 錯誤 2：履歷只列技術不列成就

```csharp
// ❌ 錯誤：只寫做了什麼，沒寫做得多好 // 中文註解
var badResume = new List<string> // 不好的履歷描述
{
    ""負責後端 API 開發"", // 太模糊，沒有量化
    ""維護資料庫"", // 看不出能力
    ""參與系統開發"" // 毫無亮點
};

// ✅ 正確：用數字量化成就 // 中文註解
var goodResume = new List<string> // 好的履歷描述
{
    ""使用 ASP.NET Core 開發 20+ 支 RESTful API，日均處理 10 萬次請求"", // 量化規模
    ""優化 SQL 查詢效能，平均查詢時間從 2 秒降至 200ms"", // 量化改善幅度
    ""主導 CI/CD 導入，部署頻率從每月 1 次提升至每日 3 次"" // 量化效率提升
};
```

### ❌ 錯誤 3：STAR 回答沒有具體數字

```csharp
// ❌ 錯誤：回答太抽象 // 中文註解
var badAnswer = ""我之前解決過一個效能問題，"" + // 沒有具體情境
               ""改了一些 code 就好了""; // 沒有說明做法和結果

// ✅ 正確：用 STAR 法則並加入數字 // 中文註解
var goodAnswer = ""在 ABC 公司時（S），"" + // 情境：具體公司
                 ""我們的 API 回應超過 5 秒（T），"" + // 任務：具體數字
                 ""我用 Profiler 找到 N+1 問題並重構查詢（A），"" + // 行動：具體做法
                 ""最終回應時間降到 300ms（R）""; // 結果：具體改善
```

---

## 📝 面試準備 Checklist

```
面試前一週：
  □ 研究公司產品與技術棧
  □ 準備 30 秒 / 1 分鐘自我介紹
  □ 複習 C# 和 .NET 核心考題
  □ 準備 3-5 個 STAR 故事
  □ 更新 GitHub Portfolio

面試前一天：
  □ 確認面試時間、地點、面試官姓名
  □ 準備要問面試官的問題
  □ 充足睡眠

面試當天：
  □ 提前 10 分鐘到達
  □ 帶好紙筆
  □ 保持自信微笑

面試後：
  □ 24 小時內寄感謝信
  □ 記錄面試問題（下次準備用）
```
" },

        // ── Interview Chapter 561 ────────────────────────────
        new() { Id=561, Category="interview", Order=2, Level="intermediate", Icon="🧩", Title="C# 與 .NET 常見考題", Slug="csharp-dotnet-interview-questions", IsPublished=true, Content=@"
# C# 與 .NET 常見考題

## 📌 本章重點
> 本章整理了 .NET 面試中最常被問到的技術題目。
> 每題都附上「標準答案」和「加分答案」，幫助你在面試中脫穎而出。

---

## 考題 1：值型別 vs 參考型別

### 面試官問法
> 「請解釋 C# 中值型別和參考型別的差異」

### 標準答案

```csharp
// 值型別（Value Type）：存在 Stack 上 // 中文註解
int a = 10; // a 直接存放數值 10
int b = a;  // b 複製 a 的值，兩者獨立
b = 20;     // 修改 b 不影響 a
Console.WriteLine(a); // 輸出 10，a 沒有被影響
Console.WriteLine(b); // 輸出 20，b 是獨立的副本

// 參考型別（Reference Type）：存在 Heap 上 // 中文註解
var list1 = new List<int> { 1, 2, 3 }; // list1 存放的是指向 Heap 的參考
var list2 = list1; // list2 複製的是「參考」，不是資料本身
list2.Add(4); // 透過 list2 修改，list1 也會被影響
Console.WriteLine(list1.Count); // 輸出 4，因為 list1 和 list2 指向同一個物件
```

### 加分答案

```csharp
// 加分：提到 struct 是值型別 // 中文註解
public struct Point // struct 是值型別
{
    public int X { get; set; } // X 座標
    public int Y { get; set; } // Y 座標
}

// 加分：提到 boxing/unboxing // 中文註解
int number = 42; // 值型別
object boxed = number; // Boxing：值型別被包裝成參考型別，有效能損耗
int unboxed = (int)boxed; // Unboxing：從參考型別取回值型別

// 加分：提到 Stack vs Heap 的效能差異 // 中文註解
// Stack：配置快、自動回收、大小有限（預設 1MB）// 堆疊特性
// Heap：配置慢、需要 GC 回收、大小彈性 // 堆積特性
```

```
記憶口訣：
  值型別 = 「影印一份給你」 → 改你的不影響我的
  參考型別 = 「給你同一份文件的地址」 → 你改了我也看得到
```

---

## 考題 2：abstract vs interface

### 面試官問法
> 「abstract class 和 interface 有什麼差異？什麼時候用哪個？」

### 標準答案

```csharp
// abstract class：可以有實作，只能單一繼承 // 中文註解
public abstract class Animal // 抽象類別：動物
{
    public string Name { get; set; } = """"; // 可以有屬性和欄位

    public void Breathe() // 可以有已實作的方法
    {
        Console.WriteLine($""{Name} 在呼吸""); // 所有動物都會呼吸
    }

    public abstract void MakeSound(); // 抽象方法：子類別必須實作
}

// interface：不能有狀態，可以多重實作 // 中文註解
public interface ISwimmable // 介面：可游泳的
{
    void Swim(); // 只定義契約，不能有欄位
}

public interface IFlyable // 介面：可飛行的
{
    void Fly(); // 介面方法
}

// 使用：一個類別可以繼承一個 abstract + 多個 interface // 中文註解
public class Duck : Animal, ISwimmable, IFlyable // 鴨子繼承動物，實作游泳和飛行
{
    public override void MakeSound() => Console.WriteLine(""呱呱""); // 實作抽象方法
    public void Swim() => Console.WriteLine(""鴨子在游泳""); // 實作游泳介面
    public void Fly() => Console.WriteLine(""鴨子在飛""); // 實作飛行介面
}
```

### 加分答案

```csharp
// 加分：C# 8.0 的 default interface method // 中文註解
public interface ILogger // 記錄介面
{
    void Log(string message); // 必須實作的方法

    void LogError(string message) // C# 8.0：介面可以有預設實作
    {
        Log($""[ERROR] {message}""); // 預設實作
    }
}

// 加分：提到選擇原則 // 中文註解
// 用 abstract class：當子類別之間有共同的狀態或行為（is-a 關係）// 繼承
// 用 interface：當不同類別需要共同的能力（can-do 關係）// 契約
```

---

## 考題 3：delegate vs event

### 面試官問法
> 「delegate 和 event 有什麼差異？」

### 標準答案

```csharp
// delegate：函式指標，可以指向任何符合簽章的方法 // 中文註解
public delegate void NotifyHandler(string message); // 定義委派型別

// event：基於 delegate 的封裝，限制外部只能 += 和 -= // 中文註解
public class OrderService // 訂單服務類別
{
    public event NotifyHandler? OnOrderPlaced; // event 限制外部存取

    public void PlaceOrder(string item) // 下訂單方法
    {
        Console.WriteLine($""訂單已建立：{item}""); // 建立訂單
        OnOrderPlaced?.Invoke(item); // 觸發事件通知所有訂閱者
    }
}

// 使用範例 // 中文註解
var service = new OrderService(); // 建立訂單服務
service.OnOrderPlaced += msg => Console.WriteLine($""寄 Email：{msg}""); // 訂閱事件
service.OnOrderPlaced += msg => Console.WriteLine($""寄簡訊：{msg}""); // 多個訂閱者
service.PlaceOrder(""筆電""); // 下訂單，觸發所有訂閱者
```

### 加分答案

```csharp
// 加分：解釋 event 的安全性 // 中文註解
// delegate 可以被外部直接 Invoke 或設為 null // 不安全
// event 只能被宣告它的類別 Invoke // 安全

// 加分：使用內建的 EventHandler<T> // 中文註解
public class Button // 按鈕類別
{
    public event EventHandler<string>? Clicked; // 使用內建泛型 EventHandler

    public void Click() // 點擊方法
    {
        Clicked?.Invoke(this, ""Button was clicked""); // 觸發事件
    }
}
```

---

## 考題 4：async/await 底層原理

### 面試官問法
> 「async/await 的底層是怎麼運作的？」

### 標準答案

```csharp
// async/await 會被編譯器轉換成 State Machine // 中文註解
public async Task<string> GetDataAsync() // 非同步方法
{
    Console.WriteLine(""開始""); // 狀態 0：同步執行
    var data = await FetchFromDbAsync(); // 狀態 1：遇到 await，讓出執行緒
    Console.WriteLine(""處理中""); // 狀態 2：await 完成後繼續
    return data; // 回傳結果
}

// 編譯器實際產生的概念（簡化版）// 中文註解
// 1. 編譯器建立一個實作 IAsyncStateMachine 的 struct // 狀態機結構
// 2. 每個 await 是一個「暫停點」// 切割成多個狀態
// 3. 方法被切割成多個狀態（state 0, 1, 2...）// 狀態轉換
// 4. 當 await 的 Task 完成時，回呼 MoveNext() 繼續執行 // 恢復執行
```

### 加分答案

```csharp
// 加分：提到 ConfigureAwait(false) // 中文註解
public async Task<string> GetDataFromApiAsync() // API 呼叫方法
{
    var client = new HttpClient(); // 建立 HTTP 客戶端
    var response = await client.GetStringAsync(""https://api.example.com/data"") // 非同步請求
        .ConfigureAwait(false); // 不需要回到原本的同步內容（Library 開發常用）
    return response; // 回傳結果
}

// 加分：提到 ValueTask 的效能優勢 // 中文註解
public ValueTask<int> GetCachedValueAsync(int key) // 使用 ValueTask 避免不必要的分配
{
    if (_cache.TryGetValue(key, out var value)) // 如果快取命中
        return new ValueTask<int>(value); // 同步回傳，不需要分配 Task
    return new ValueTask<int>(FetchAndCacheAsync(key)); // 快取未命中才真正非同步
}
```

---

## 考題 5：GC 垃圾回收機制

### 面試官問法
> 「請解釋 .NET 的垃圾回收機制」

### 標準答案

```csharp
// .NET GC 使用分代回收（Generational GC）// 中文註解
// Generation 0：新建立的物件，回收最頻繁 // 短命物件
// Generation 1：從 Gen 0 存活的物件，緩衝區 // 中等壽命
// Generation 2：從 Gen 1 存活的物件，回收最少 // 長命物件

// 範例：觀察 GC 行為 // 中文註解
var obj = new object(); // 新物件在 Gen 0
Console.WriteLine($""世代：{GC.GetGeneration(obj)}""); // 輸出 0

GC.Collect(); // 觸發垃圾回收
Console.WriteLine($""世代：{GC.GetGeneration(obj)}""); // 輸出 1（存活後升代）

GC.Collect(); // 再次觸發垃圾回收
Console.WriteLine($""世代：{GC.GetGeneration(obj)}""); // 輸出 2（再次存活後升代）
```

### 加分答案

```csharp
// 加分：提到 IDisposable 和 using // 中文註解
public class DatabaseConnection : IDisposable // 實作 IDisposable 管理非託管資源
{
    private bool _disposed = false; // 追蹤是否已釋放

    public void Dispose() // 釋放資源方法
    {
        if (!_disposed) // 避免重複釋放
        {
            // 釋放非託管資源（如資料庫連線）// 清理動作
            _disposed = true; // 標記為已釋放
            GC.SuppressFinalize(this); // 告訴 GC 不需要呼叫 Finalizer
        }
    }
}

// 使用 using 確保資源被釋放 // 中文註解
using var conn = new DatabaseConnection(); // using 會自動呼叫 Dispose
// 離開 scope 時自動釋放 // 安全的資源管理

// 加分：提到 LOH（Large Object Heap）// 中文註解
// 大於 85,000 bytes 的物件會被分配到 LOH // 大物件堆積
// LOH 不會被壓縮（compaction），可能造成記憶體碎片 // 效能注意事項
```

---

## 考題 6：IEnumerable vs IQueryable

### 面試官問法
> 「IEnumerable 和 IQueryable 有什麼差異？」

### 標準答案

```csharp
// IEnumerable<T>：在記憶體中過濾（Client-side） // 中文註解
List<Product> allProducts = GetAllProducts(); // 從資料庫載入所有資料到記憶體
IEnumerable<Product> filtered = allProducts // IEnumerable 在記憶體中過濾
    .Where(p => p.Price > 100); // 在 C# 端執行 Where 條件

// IQueryable<T>：在資料庫端過濾（Server-side） // 中文註解
IQueryable<Product> query = dbContext.Products // IQueryable 建立查詢表達式
    .Where(p => p.Price > 100); // 轉換成 SQL WHERE Price > 100
// 此時還沒有執行查詢！// 延遲執行（Deferred Execution）
var results = query.ToList(); // 呼叫 ToList() 才真正執行 SQL
```

### 加分答案

```csharp
// 加分：說明延遲執行的好處 // 中文註解
IQueryable<Product> query = dbContext.Products // 建立查詢
    .Where(p => p.Price > 100) // 條件 1：價格大於 100
    .Where(p => p.Category == ""電子產品"") // 條件 2：類別是電子產品
    .OrderBy(p => p.Name); // 排序

// 以上三個操作會合併成一條 SQL // 效能最佳化
// SELECT * FROM Products WHERE Price > 100 AND Category = '電子產品' ORDER BY Name
// 而不是載入所有資料再過濾 // 避免不必要的資料傳輸

// 加分：提到 AsNoTracking // 中文註解
var readOnlyData = dbContext.Products // 唯讀查詢
    .AsNoTracking() // 不追蹤變更，提升查詢效能
    .Where(p => p.Price > 100) // 條件過濾
    .ToList(); // 執行查詢
```

---

## 考題 7：string vs StringBuilder

### 面試官問法
> 「string 和 StringBuilder 有什麼差異？什麼時候用 StringBuilder？」

### 標準答案

```csharp
// string：不可變（Immutable），每次修改都建立新物件 // 中文註解
string result = """"; // 初始空字串
for (int i = 0; i < 10000; i++) // 迴圈 10000 次
{
    result += i.ToString(); // ❌ 每次 += 都建立新的 string 物件，效能很差
}
// 產生了大約 10000 個中間字串物件 // 記憶體浪費

// StringBuilder：可變（Mutable），在原有緩衝區上修改 // 中文註解
var sb = new StringBuilder(); // 建立 StringBuilder
for (int i = 0; i < 10000; i++) // 迴圈 10000 次
{
    sb.Append(i); // ✅ 在同一個緩衝區追加，效能好
}
string finalResult = sb.ToString(); // 最後才轉成 string
```

### 加分答案

```csharp
// 加分：提到具體的效能數據 // 中文註解
// string 串接 10000 次：約 1000ms+ // 效能差
// StringBuilder 串接 10000 次：約 1ms // 效能好

// 加分：提到 string interning // 中文註解
string a = ""hello""; // 字串常值會被 intern
string b = ""hello""; // b 和 a 指向同一個物件
Console.WriteLine(ReferenceEquals(a, b)); // True：因為 string interning

// 加分：提到使用時機 // 中文註解
// 少量串接（< 5 次）：用 string 或字串內插 $""{}"" // 簡單情境
// 大量串接或迴圈中：用 StringBuilder // 效能需求
// 固定格式：用 string.Format 或字串內插 // 可讀性
```

---

## 考題 8：Dependency Injection 生命週期

### 面試官問法
> 「請解釋 DI 的三種生命週期：Transient、Scoped、Singleton」

### 標準答案

```csharp
// DI 三種生命週期 // 中文註解
var builder = WebApplication.CreateBuilder(args); // 建立應用程式建構器

// Transient：每次注入都建立新實例 // 中文註解
builder.Services.AddTransient<IEmailService, EmailService>(); // 每次要求都 new 一個

// Scoped：每個 HTTP Request 共用一個實例 // 中文註解
builder.Services.AddScoped<IShoppingCart, ShoppingCart>(); // 同一個 Request 內共用

// Singleton：整個應用程式只有一個實例 // 中文註解
builder.Services.AddSingleton<ICacheService, CacheService>(); // 全域共用一個
```

### 加分答案

```csharp
// 加分：說明使用場景 // 中文註解
// Transient：輕量、無狀態的服務（如 Validator）// 每次新建最安全
// Scoped：需要 Request 內一致性的服務（如 DbContext）// 確保同一交易
// Singleton：昂貴或共享的資源（如 HttpClient、快取）// 避免重複建立

// 加分：提到常見的生命週期錯誤 // 中文註解
// ❌ 不要在 Singleton 中注入 Scoped 服務 // Captive Dependency 問題
// 因為 Singleton 永遠存在，但 Scoped 服務已經被釋放 // 會造成記憶體洩漏

// 驗證生命週期的方式 // 中文註解
public class LifetimeDemo // 生命週期示範類別
{
    private readonly Guid _id = Guid.NewGuid(); // 每個實例有唯一 ID

    public Guid GetId() => _id; // 回傳 ID 來驗證是否為同一實例
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：面試回答太短

```csharp
// ❌ 錯誤：只給一句話的回答 // 中文註解
// 面試官問：「什麼是 async/await？」 // 常見問題
// 回答：「就是非同步啊」 // 太短，沒有展現深度

// ✅ 正確：分層回答 // 中文註解
// 第一層：定義（async/await 是 C# 的非同步程式設計語法糖）// 基本定義
// 第二層：原理（編譯器會轉換成 State Machine）// 底層原理
// 第三層：使用場景（I/O 密集操作如 HTTP 請求、DB 查詢）// 實際應用
// 第四層：注意事項（避免 async void、注意 deadlock）// 經驗分享
```

### ❌ 錯誤 2：混淆 IEnumerable 和 IQueryable

```csharp
// ❌ 錯誤：在 IQueryable 上使用 IEnumerable 的思維 // 中文註解
var products = dbContext.Products.ToList() // 先載入所有資料到記憶體
    .Where(p => p.Price > 100); // 然後在記憶體中過濾，效能極差

// ✅ 正確：讓資料庫做過濾 // 中文註解
var products2 = dbContext.Products // 使用 IQueryable
    .Where(p => p.Price > 100) // 在資料庫端過濾
    .ToList(); // 最後才執行查詢並載入結果
```

### ❌ 錯誤 3：不理解 DI 生命週期

```csharp
// ❌ 錯誤：在 Singleton 服務中注入 Scoped 服務 // 中文註解
public class MySingleton // Singleton 服務
{
    private readonly MyDbContext _db; // ❌ DbContext 是 Scoped 的
    public MySingleton(MyDbContext db) // 這會導致 Captive Dependency
    {
        _db = db; // DbContext 會被 Singleton 永久持有，不會被正確釋放
    }
}

// ✅ 正確：使用 IServiceScopeFactory // 中文註解
public class MySingletonFixed // 修正後的 Singleton 服務
{
    private readonly IServiceScopeFactory _scopeFactory; // 注入 Scope 工廠

    public MySingletonFixed(IServiceScopeFactory scopeFactory) // 透過工廠建立 Scope
    {
        _scopeFactory = scopeFactory; // 儲存工廠參考
    }

    public async Task DoWorkAsync() // 工作方法
    {
        using var scope = _scopeFactory.CreateScope(); // 建立新的 Scope
        var db = scope.ServiceProvider.GetRequiredService<MyDbContext>(); // 從 Scope 取得 DbContext
        await db.SaveChangesAsync(); // 使用完畢後 Scope 會自動釋放
    }
}
```

---

## 📝 面試考題速查表

```
考題                        一句話回答
─────────────────────────────────────────────────────
值型別 vs 參考型別          Stack vs Heap，複製值 vs 複製參考
abstract vs interface      單繼承有實作 vs 多實作只有契約
delegate vs event          函式指標 vs 受限的函式指標（安全性）
async/await               語法糖，編譯成 State Machine
GC 機制                    分代回收：Gen 0/1/2
IEnumerable vs IQueryable  記憶體過濾 vs 資料庫過濾
string vs StringBuilder    不可變 vs 可變，大量串接用 SB
DI 生命週期                Transient/Scoped/Singleton
```
" },

        // ── Interview Chapter 562 ────────────────────────────
        new() { Id=562, Category="interview", Order=3, Level="intermediate", Icon="🗃️", Title="資料庫與 SQL 面試題", Slug="database-sql-interview-questions", IsPublished=true, Content=@"
# 資料庫與 SQL 面試題

## 📌 本章重點
> 資料庫是後端面試的必考領域。
> 本章涵蓋 SQL 查詢、索引原理、交易機制等常見考題，每題附標準答案。

---

## 考題 1：INNER JOIN vs LEFT JOIN

### 面試官問法
> 「請解釋 INNER JOIN 和 LEFT JOIN 的差異」

### 標準答案

```
圖解：

表 A（員工）        表 B（部門）
┌────┬────────┐    ┌────┬──────┐
│ ID │ Name   │    │ ID │ Dept │
├────┼────────┤    ├────┼──────┤
│ 1  │ Alice  │    │ 1  │ IT   │
│ 2  │ Bob    │    │ 2  │ HR   │
│ 3  │ Carol  │    │ 4  │ Sales│
└────┴────────┘    └────┴──────┘

INNER JOIN（交集）：只回傳兩邊都有的
結果：Alice-IT, Bob-HR（Carol 和 Sales 被排除）

LEFT JOIN（左表全部 + 右表匹配）：
結果：Alice-IT, Bob-HR, Carol-NULL（Carol 沒有對應部門）
```

```csharp
// 在 C# 中用 LINQ 表示 // 中文註解
var innerJoin = from e in employees // 從員工表開始
                join d in departments on e.DeptId equals d.Id // INNER JOIN 部門表
                select new { e.Name, d.DeptName }; // 只回傳有匹配的資料

var leftJoin = from e in employees // 從員工表開始
               join d in departments on e.DeptId equals d.Id // JOIN 部門表
               into deptGroup // 暫存為群組
               from d in deptGroup.DefaultIfEmpty() // LEFT JOIN：沒匹配的填 null
               select new { e.Name, DeptName = d?.DeptName ?? ""無部門"" }; // 處理 null 值
```

### SQL 範例

```csharp
// INNER JOIN 的 SQL // 中文註解
var innerJoinSql = @"" // SQL 查詢字串
    SELECT e.Name, d.DeptName -- 選取欄位
    FROM Employees e -- 員工表
    INNER JOIN Departments d ON e.DeptId = d.Id -- 內連接：只取交集
""; // INNER JOIN 結束

// LEFT JOIN 的 SQL // 中文註解
var leftJoinSql = @"" // SQL 查詢字串
    SELECT e.Name, ISNULL(d.DeptName, '無部門') AS DeptName -- 處理 NULL
    FROM Employees e -- 員工表（左表，全部保留）
    LEFT JOIN Departments d ON e.DeptId = d.Id -- 左連接：保留左表所有資料
""; // LEFT JOIN 結束
```

---

## 考題 2：索引原理（B+ Tree）

### 面試官問法
> 「請解釋資料庫索引的原理，以及什麼時候該建索引」

### 標準答案

```
B+ Tree 索引結構（簡化）：

                    [50]                    ← 根節點
                   /    \
            [20,35]      [70,85]            ← 中間節點
           /   |   \    /   |   \
     [10,15] [25,30] [40,45] [55,65] [75,80] [90,95]  ← 葉節點（存放資料指標）
      ↕        ↕       ↕       ↕       ↕       ↕
     Data    Data    Data    Data    Data    Data       ← 實際資料

查找 ID=35 的過程：
  根節點 → 35 ≤ 50 走左邊 → 35 ≥ 35 走右邊 → 找到！
  只需要 3 次比較，而不是掃描整個表
```

```csharp
// 索引的效果 // 中文註解
// 沒有索引：全表掃描（Full Table Scan），O(n) // 效能差
// 有索引：B+ Tree 查找，O(log n) // 效能好

// 在 EF Core 中建立索引 // 中文註解
public class ProductConfiguration : IEntityTypeConfiguration<Product> // 設定類別
{
    public void Configure(EntityTypeBuilder<Product> builder) // 設定方法
    {
        builder.HasIndex(p => p.Name); // 建立單一欄位索引
        builder.HasIndex(p => new { p.Category, p.Price }); // 建立複合索引
        builder.HasIndex(p => p.SKU).IsUnique(); // 建立唯一索引
    }
}
```

### 加分答案

```csharp
// 什麼時候該建索引？// 中文註解
var indexGuidelines = new Dictionary<string, string> // 索引建議
{
    [""WHERE 條件常用的欄位""] = ""✅ 建索引"", // 加速查詢
    [""JOIN 的 ON 條件欄位""] = ""✅ 建索引"", // 加速連接
    [""ORDER BY 的欄位""] = ""✅ 考慮建索引"", // 加速排序
    [""經常更新的欄位""] = ""⚠️ 謹慎建索引"", // 索引維護成本
    [""資料量很少的表""] = ""❌ 不需要索引"", // 全表掃描更快
    [""高基數（唯一值多）""] = ""✅ 適合索引"", // 索引效果好
    [""低基數（如性別）""] = ""❌ 不適合索引"" // 索引效果差
};
```

---

## 考題 3：正規化 1NF/2NF/3NF

### 面試官問法
> 「請解釋資料庫正規化的概念」

### 標準答案

```
❌ 未正規化的表：

┌────┬───────┬──────────┬──────────────┬──────────┐
│ ID │ 學生   │ 電話      │ 課程          │ 教授      │
├────┼───────┼──────────┼──────────────┼──────────┤
│ 1  │ Alice │ 02-1111  │ C#, SQL      │ 王教授    │
│ 2  │ Bob   │ 02-2222  │ C#           │ 王教授    │
└────┴───────┴──────────┴──────────────┴──────────┘
問題：課程欄位有多個值（非原子性）

✅ 1NF（第一正規化）：每個欄位都是原子值

┌────┬───────┬──────────┬──────────────┬──────────┐
│ ID │ 學生   │ 電話      │ 課程          │ 教授      │
├────┼───────┼──────────┼──────────────┼──────────┤
│ 1  │ Alice │ 02-1111  │ C#           │ 王教授    │
│ 1  │ Alice │ 02-1111  │ SQL          │ 李教授    │
│ 2  │ Bob   │ 02-2222  │ C#           │ 王教授    │
└────┴───────┴──────────┴──────────────┴──────────┘
問題：學生資料重複

✅ 2NF（第二正規化）：消除部分相依

學生表：                    選課表：
┌────┬───────┬──────────┐  ┌─────────┬────────┐
│ ID │ 學生   │ 電話      │  │ StudentId│ CourseId│
├────┼───────┼──────────┤  ├─────────┼────────┤
│ 1  │ Alice │ 02-1111  │  │ 1       │ 1      │
│ 2  │ Bob   │ 02-2222  │  │ 1       │ 2      │
└────┴───────┴──────────┘  │ 2       │ 1      │
                            └─────────┴────────┘

✅ 3NF（第三正規化）：消除遞移相依
   確保非主鍵欄位只依賴主鍵，不依賴其他非主鍵欄位
```

```csharp
// EF Core 中的正規化設計 // 中文註解
public class Student // 學生實體（已正規化）
{
    public int Id { get; set; } // 主鍵
    public string Name { get; set; } = """"; // 學生姓名
    public string Phone { get; set; } = """"; // 電話
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>(); // 選課記錄（導覽屬性）
}

public class Course // 課程實體
{
    public int Id { get; set; } // 主鍵
    public string CourseName { get; set; } = """"; // 課程名稱
    public int ProfessorId { get; set; } // 外鍵：教授
    public Professor Professor { get; set; } = null!; // 導覽屬性
}

public class Enrollment // 選課關聯表（多對多）
{
    public int StudentId { get; set; } // 外鍵：學生
    public int CourseId { get; set; } // 外鍵：課程
    public Student Student { get; set; } = null!; // 導覽屬性
    public Course Course { get; set; } = null!; // 導覽屬性
}
```

---

## 考題 4：交易隔離等級

### 面試官問法
> 「請解釋資料庫的四種隔離等級」

### 標準答案

```
隔離等級由低到高：

等級               髒讀    不可重複讀   幻讀    效能
──────────────────────────────────────────────────
Read Uncommitted   ✅ 可能  ✅ 可能    ✅ 可能  最快
Read Committed     ❌ 防止  ✅ 可能    ✅ 可能  快
Repeatable Read    ❌ 防止  ❌ 防止    ✅ 可能  中等
Serializable       ❌ 防止  ❌ 防止    ❌ 防止  最慢
```

```csharp
// 在 EF Core 中設定隔離等級 // 中文註解
using var transaction = await dbContext.Database // 取得資料庫連線
    .BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted); // 設定隔離等級為 Read Committed

try // 嘗試執行交易
{
    var product = await dbContext.Products // 查詢商品
        .FirstOrDefaultAsync(p => p.Id == 1); // 根據 ID 查找
    if (product != null) // 如果商品存在
    {
        product.Stock -= 1; // 減少庫存
        await dbContext.SaveChangesAsync(); // 儲存變更
    }
    await transaction.CommitAsync(); // 提交交易
}
catch (Exception) // 發生錯誤時
{
    await transaction.RollbackAsync(); // 回滾交易
    throw; // 重新拋出例外
}
```

### 加分答案

```csharp
// 加分：解釋三種問題 // 中文註解
// 髒讀（Dirty Read）：讀到別人還沒 commit 的資料 // 最不安全
// 不可重複讀（Non-Repeatable Read）：同一筆資料讀兩次結果不同 // 中等風險
// 幻讀（Phantom Read）：同一個查詢讀兩次，筆數不同 // 新增/刪除造成

// 加分：SQL Server 預設是 Read Committed // 中文註解
// PostgreSQL 預設也是 Read Committed // 大部分情況夠用
```

---

## 考題 5：死鎖原因與解法

### 面試官問法
> 「什麼是死鎖？如何避免？」

### 標準答案

```
死鎖情境：

交易 A：鎖住 表1 → 等待 表2 的鎖
交易 B：鎖住 表2 → 等待 表1 的鎖

  交易 A ──鎖住──→ 表1
    ↑                 ↓
  等待              等待
    ↑                 ↓
  表2 ←──鎖住── 交易 B

結果：互相等待，誰也無法繼續 → 死鎖！
```

```csharp
// 死鎖範例（概念）// 中文註解
// 交易 A // 第一個交易
// UPDATE Orders SET Status = 'Processing' WHERE Id = 1  -- 鎖住 Orders
// UPDATE Products SET Stock = Stock - 1 WHERE Id = 100  -- 等待 Products 的鎖

// 交易 B（同時執行）// 第二個交易
// UPDATE Products SET Stock = Stock - 1 WHERE Id = 100  -- 鎖住 Products
// UPDATE Orders SET Status = 'Shipped' WHERE Id = 1     -- 等待 Orders 的鎖

// 解決方案 // 中文註解
var deadlockSolutions = new List<string> // 死鎖解決方案清單
{
    ""1. 統一存取順序：所有交易都先鎖 Orders 再鎖 Products"", // 最常用的方法
    ""2. 縮短交易時間：減少持鎖時間"", // 降低死鎖機率
    ""3. 使用 NOLOCK 提示（犧牲一致性）"", // 適用於報表查詢
    ""4. 設定 Lock Timeout"", // 避免無限等待
    ""5. 使用樂觀並行控制（Optimistic Concurrency）"" // 適用於低衝突場景
};

// EF Core 樂觀並行控制 // 中文註解
public class Product // 商品實體
{
    public int Id { get; set; } // 主鍵
    public string Name { get; set; } = """"; // 商品名稱

    [ConcurrencyCheck] // 並行檢查標記
    public int Stock { get; set; } // 庫存數量（更新時會檢查是否被其他人修改）
}
```

---

## 考題 6：Stored Procedure vs ORM

### 面試官問法
> 「你覺得應該用 Stored Procedure 還是 ORM？」

### 標準答案

```csharp
// Stored Procedure 的優缺點 // 中文註解
var spPros = new List<string> // SP 優點
{
    ""效能好：預編譯、執行計畫快取"", // 效能優勢
    ""安全性：可以限制直接存取表"", // 權限控制
    ""減少網路傳輸：只傳參數和結果"" // 網路優勢
};
var spCons = new List<string> // SP 缺點
{
    ""難以版本控制"", // 不好管理
    ""不同資料庫語法不同"", // 可攜性差
    ""Business Logic 散落在 DB 和 App 中"" // 維護困難
};

// ORM（如 EF Core）的優缺點 // 中文註解
var ormPros = new List<string> // ORM 優點
{
    ""開發速度快：不用手寫 SQL"", // 開發效率
    ""型別安全：編譯時期檢查"", // 安全性
    ""易於維護：code 和 logic 在一起"", // 可維護性
    ""支援 Migration"" // 資料庫版本控制
};
var ormCons = new List<string> // ORM 缺點
{
    ""效能可能不如手寫 SQL"", // 效能考量
    ""複雜查詢寫起來很痛苦"", // 複雜度限制
    ""N+1 問題"" // 常見陷阱
};

// 建議：混合使用 // 中文註解
// 一般 CRUD → 用 EF Core // 簡單操作
// 複雜報表查詢 → 用 Dapper 或 Raw SQL // 複雜查詢
// 極端效能需求 → 用 Stored Procedure // 效能關鍵
```

---

## 考題 7：EF Core N+1 問題

### 面試官問法
> 「什麼是 N+1 問題？如何解決？」

### 標準答案

```csharp
// ❌ N+1 問題範例 // 中文註解
var orders = dbContext.Orders.ToList(); // 1 次查詢取得所有訂單
foreach (var order in orders) // 遍歷每個訂單
{
    // 每次存取 OrderItems 都會觸發一次查詢（N 次）// 延遲載入的陷阱
    Console.WriteLine($""訂單 {order.Id} 有 {order.OrderItems.Count} 個品項""); // N 次查詢
}
// 總共：1（取訂單）+ N（每個訂單取品項）= N+1 次查詢 // 效能災難

// ✅ 解法 1：Eager Loading（Include）// 中文註解
var ordersFixed = dbContext.Orders // 查詢訂單
    .Include(o => o.OrderItems) // 一次性載入所有關聯的品項
    .ToList(); // 只有 1 次查詢（含 JOIN）

// ✅ 解法 2：Explicit Loading // 中文註解
var ordersExplicit = dbContext.Orders.ToList(); // 先取訂單
dbContext.Entry(ordersExplicit.First()) // 針對特定訂單
    .Collection(o => o.OrderItems) // 明確載入品項
    .Load(); // 手動觸發載入

// ✅ 解法 3：Projection（Select）// 中文註解
var orderSummary = dbContext.Orders // 查詢訂單
    .Select(o => new // 投影到匿名型別
    {
        o.Id, // 訂單 ID
        ItemCount = o.OrderItems.Count, // 品項數量（在 SQL 端計算）
        Total = o.OrderItems.Sum(i => i.Price) // 總金額（在 SQL 端計算）
    })
    .ToList(); // 只產生一次有效率的 SQL 查詢
```

---

## 考題 8：SQL 實作題

### 面試官問法
> 「請手寫 SQL 解決以下問題」

### 常見實作題

```csharp
// 題目 1：找出每個部門薪資最高的員工 // 中文註解
var sql1 = @"" // SQL 查詢
    SELECT d.DeptName, e.Name, e.Salary -- 選取部門、姓名、薪資
    FROM Employees e -- 員工表
    INNER JOIN Departments d ON e.DeptId = d.Id -- 連接部門表
    WHERE e.Salary = ( -- 子查詢：找出該部門最高薪資
        SELECT MAX(e2.Salary) -- 最高薪資
        FROM Employees e2 -- 子查詢的員工表
        WHERE e2.DeptId = e.DeptId -- 同部門條件
    )
""; // 相關子查詢

// 用 LINQ 表示 // 中文註解
var topSalaries = dbContext.Employees // 從員工表開始
    .GroupBy(e => e.DeptId) // 按部門分組
    .Select(g => g.OrderByDescending(e => e.Salary).First()) // 每組取薪資最高的
    .ToList(); // 執行查詢

// 題目 2：找出連續三天都有登入的使用者 // 中文註解
var sql2 = @"" // SQL 查詢
    SELECT DISTINCT l1.UserId -- 選取不重複的使用者 ID
    FROM LoginLogs l1 -- 登入記錄表
    INNER JOIN LoginLogs l2 ON l1.UserId = l2.UserId -- 自連接：第二天
        AND DATEDIFF(day, l1.LoginDate, l2.LoginDate) = 1 -- 相差一天
    INNER JOIN LoginLogs l3 ON l1.UserId = l3.UserId -- 自連接：第三天
        AND DATEDIFF(day, l1.LoginDate, l3.LoginDate) = 2 -- 相差兩天
""; // 連續三天登入

// 題目 3：計算每月營收的月增率 // 中文註解
var sql3 = @"" // SQL 查詢
    WITH MonthlyRevenue AS ( -- CTE：計算每月營收
        SELECT -- 選取欄位
            FORMAT(OrderDate, 'yyyy-MM') AS Month, -- 格式化月份
            SUM(TotalAmount) AS Revenue -- 加總營收
        FROM Orders -- 訂單表
        GROUP BY FORMAT(OrderDate, 'yyyy-MM') -- 按月份分組
    )
    SELECT -- 選取結果
        curr.Month, -- 當月
        curr.Revenue, -- 當月營收
        prev.Revenue AS PrevRevenue, -- 上月營收
        ROUND((curr.Revenue - prev.Revenue) * 100.0 / prev.Revenue, 2) AS GrowthRate -- 計算月增率
    FROM MonthlyRevenue curr -- 當月資料
    LEFT JOIN MonthlyRevenue prev -- 連接上月資料
        ON DATEADD(MONTH, -1, CAST(curr.Month + '-01' AS DATE)) -- 上個月的日期
           = CAST(prev.Month + '-01' AS DATE) -- 匹配上月
""; // 月增率計算
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：不理解 JOIN 的差異

```csharp
// ❌ 錯誤：面試時說「JOIN 就是把兩個表合在一起」// 中文註解
// 太模糊，面試官會追問你知不知道差別 // 不夠深入

// ✅ 正確：清楚區分每種 JOIN // 中文註解
var joinTypes = new Dictionary<string, string> // JOIN 類型對照
{
    [""INNER JOIN""] = ""只回傳兩邊都有匹配的資料（交集）"", // 最常用
    [""LEFT JOIN""] = ""保留左表所有資料，右表沒匹配的填 NULL"", // 常用於保留主表
    [""RIGHT JOIN""] = ""保留右表所有資料，左表沒匹配的填 NULL"", // 較少用
    [""FULL OUTER JOIN""] = ""保留兩邊所有資料，沒匹配的填 NULL"", // 全外連接
    [""CROSS JOIN""] = ""笛卡兒積，兩表的所有組合"" // 很少用
};
```

### ❌ 錯誤 2：在高頻更新的欄位建索引

```csharp
// ❌ 錯誤：在 LastLoginTime 欄位建索引 // 中文註解
// 每次登入都會更新，索引維護成本非常高 // 效能反而變差

// ✅ 正確：只在查詢頻率高、更新頻率低的欄位建索引 // 中文註解
public class UserConfiguration : IEntityTypeConfiguration<User> // 使用者設定
{
    public void Configure(EntityTypeBuilder<User> builder) // 設定方法
    {
        builder.HasIndex(u => u.Email).IsUnique(); // ✅ Email 很少變動，適合建索引
        builder.HasIndex(u => u.Username); // ✅ 常用於查詢
        // builder.HasIndex(u => u.LastLoginTime); // ❌ 每次登入都更新，不適合
    }
}
```

### ❌ 錯誤 3：N+1 問題沒有意識到

```csharp
// ❌ 錯誤：不知道自己寫出了 N+1 // 中文註解
public async Task<List<OrderDto>> GetOrders() // 取得訂單方法
{
    var orders = await dbContext.Orders.ToListAsync(); // 查詢所有訂單
    var result = new List<OrderDto>(); // 建立結果清單
    foreach (var order in orders) // 遍歷每個訂單
    {
        var items = await dbContext.OrderItems // ❌ 每個訂單都額外查詢一次
            .Where(i => i.OrderId == order.Id) // 條件過濾
            .ToListAsync(); // 執行查詢（N 次！）
        result.Add(new OrderDto { Items = items }); // 加入結果
    }
    return result; // 回傳結果
}

// ✅ 正確：一次查詢搞定 // 中文註解
public async Task<List<OrderDto>> GetOrdersFixed() // 修正後的方法
{
    return await dbContext.Orders // 查詢訂單
        .Include(o => o.OrderItems) // 一次載入所有品項（JOIN）
        .Select(o => new OrderDto // 投影到 DTO
        {
            Id = o.Id, // 訂單 ID
            Items = o.OrderItems.Select(i => new OrderItemDto // 轉換品項
            {
                Name = i.ProductName, // 品項名稱
                Price = i.Price // 品項價格
            }).ToList() // 轉換為清單
        })
        .ToListAsync(); // 只有一次 SQL 查詢
}
```

---

## 📝 資料庫面試速查表

```
考題                  一句話標準答案
─────────────────────────────────────────────────
INNER vs LEFT JOIN   交集 vs 保留左表全部
索引原理             B+ Tree，加速查詢但增加寫入成本
正規化               消除重複：1NF 原子值、2NF 完全相依、3NF 無遞移相依
隔離等級             RU < RC < RR < S，越高越安全但越慢
死鎖                 互相等待鎖，解法：統一存取順序
SP vs ORM            簡單用 ORM，複雜報表用 Raw SQL
N+1 問題             用 Include 或 Select 投影解決
```
" },

        // ── Interview Chapter 563 ────────────────────────────
        new() { Id=563, Category="interview", Order=4, Level="advanced", Icon="🏗️", Title="系統設計與架構面試", Slug="system-design-architecture-interview", IsPublished=true, Content=@"
# 系統設計與架構面試

## 📌 本章重點
> 系統設計面試考的不是標準答案，而是你的**思考過程**。
> 本章教你系統設計面試的框架，並附上兩個經典設計題的參考答案。

---

## 系統設計面試框架（4 步驟法）

> 💡 **比喻：蓋房子**
> 你不會一開始就搬磚頭，而是先：
> 1. 問客戶要幾層樓、幾間房（需求確認）
> 2. 畫設計圖（高層架構）
> 3. 決定用什麼建材（技術選型）
> 4. 考慮防震、防火（瓶頸與擴展）

### 4 步驟框架

```
步驟 1：需求確認（5 分鐘）
  → 功能需求（Functional Requirements）
  → 非功能需求（Non-Functional Requirements）
  → 估算（Back-of-Envelope Estimation）

步驟 2：高層設計（10 分鐘）
  → 畫出主要元件
  → API 設計
  → 資料模型

步驟 3：深入設計（15 分鐘）
  → 核心流程的細節
  → 資料庫選型
  → 快取策略

步驟 4：收尾（5 分鐘）
  → 瓶頸分析
  → 擴展方案
  → 監控與告警
```

```csharp
// 系統設計面試框架類別 // 中文註解
public class SystemDesignFramework // 系統設計框架
{
    public List<string> FunctionalRequirements { get; set; } = new(); // 功能需求
    public List<string> NonFunctionalRequirements { get; set; } = new(); // 非功能需求
    public Dictionary<string, string> Estimations { get; set; } = new(); // 規模估算
    public List<string> Components { get; set; } = new(); // 系統元件
    public List<string> Bottlenecks { get; set; } = new(); // 潛在瓶頸

    public void PrintDesign() // 印出設計方案
    {
        Console.WriteLine(""=== 功能需求 ===""); // 印出標題
        FunctionalRequirements.ForEach(r => Console.WriteLine($""  - {r}"")); // 印出每個需求
        Console.WriteLine(""=== 非功能需求 ===""); // 印出標題
        NonFunctionalRequirements.ForEach(r => Console.WriteLine($""  - {r}"")); // 印出每個需求
        Console.WriteLine(""=== 規模估算 ===""); // 印出標題
        foreach (var est in Estimations) // 遍歷估算
            Console.WriteLine($""  {est.Key}: {est.Value}""); // 印出估算值
    }
}
```

---

## 設計題 1：短網址服務（URL Shortener）

### 步驟 1：需求確認

```csharp
// 需求分析 // 中文註解
var urlShortener = new SystemDesignFramework // 建立設計框架
{
    FunctionalRequirements = new() // 功能需求
    {
        ""將長網址轉換為短網址"", // 核心功能
        ""點擊短網址重導向到原始網址"", // 重導向功能
        ""短網址可設定過期時間"", // 過期機制
        ""統計點擊次數"" // 分析功能
    },
    NonFunctionalRequirements = new() // 非功能需求
    {
        ""高可用性（99.9%）"", // 可用性要求
        ""低延遲（< 100ms）"", // 效能要求
        ""短網址不可被猜測"" // 安全需求
    },
    Estimations = new() // 規模估算
    {
        [""每日新增短網址""] = ""100 萬"", // 寫入量
        [""每日重導向次數""] = ""1 億（讀寫比 100:1）"", // 讀取量
        [""QPS（寫入）""] = ""100 萬 / 86400 ≈ 12 QPS"", // 每秒寫入
        [""QPS（讀取）""] = ""12 * 100 = 1200 QPS"", // 每秒讀取
        [""5 年資料量""] = ""100 萬 * 365 * 5 = 18.25 億筆"" // 長期儲存
    }
};
```

### 步驟 2：高層架構

```
參考答案架構圖：

使用者 → Load Balancer → Web Server → 短網址服務
                                          ↓
                                     ┌─────────┐
                                     │ Cache    │ (Redis)
                                     │ (熱門網址)│
                                     └────┬────┘
                                          ↓
                                     ┌─────────┐
                                     │ Database │ (SQL/NoSQL)
                                     │ (所有網址)│
                                     └─────────┘

短網址生成流程：
  長網址 → Base62 編碼（或 Hash + 取前 7 碼）→ 短網址
  例如：https://example.com/very/long/url → https://short.url/aB3xY7z
```

### 步驟 3：核心實作

```csharp
// 短網址服務實作 // 中文註解
public class UrlShortenerService // 短網址服務類別
{
    private const string Base62Chars = // Base62 字元集
        ""0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz""; // 62 個字元
    private const int ShortUrlLength = 7; // 短網址長度（62^7 ≈ 3.5 兆種組合）

    public string GenerateShortUrl(long id) // 從 ID 生成短網址
    {
        var sb = new StringBuilder(); // 建立字串建構器
        while (id > 0) // 當 ID 大於 0
        {
            sb.Insert(0, Base62Chars[(int)(id % 62)]); // 取餘數對應的字元
            id /= 62; // 除以 62
        }
        return sb.ToString().PadLeft(ShortUrlLength, '0'); // 補零到固定長度
    }
}

// API 設計 // 中文註解
// POST /api/shorten { ""url"": ""https://example.com/long"" } // 建立短網址
// GET /{shortCode} → 301 Redirect // 重導向到原始網址
// GET /api/stats/{shortCode} // 查詢點擊統計

// 資料模型 // 中文註解
public class UrlMapping // 網址對應實體
{
    public long Id { get; set; } // 主鍵（自增長，用於 Base62 編碼）
    public string ShortCode { get; set; } = """"; // 短網址代碼
    public string OriginalUrl { get; set; } = """"; // 原始網址
    public DateTime CreatedAt { get; set; } // 建立時間
    public DateTime? ExpiresAt { get; set; } // 過期時間
    public long ClickCount { get; set; } // 點擊次數
}
```

### 步驟 4：擴展與瓶頸

```csharp
// 瓶頸分析與解決方案 // 中文註解
var bottlenecks = new Dictionary<string, string> // 瓶頸與對策
{
    [""讀取量大""] = ""加 Redis 快取熱門網址，快取命中率可達 80%+"", // 快取策略
    [""資料庫寫入瓶頸""] = ""使用預生成 ID 範圍（ID Range），避免競爭"", // 寫入優化
    [""單點故障""] = ""多台 Web Server + Load Balancer"", // 高可用
    [""資料量過大""] = ""依照短網址首字元分片（Sharding）"" // 水平擴展
};
```

---

## 設計題 2：聊天系統

### 步驟 1：需求確認

```csharp
// 聊天系統需求 // 中文註解
var chatSystem = new SystemDesignFramework // 建立設計框架
{
    FunctionalRequirements = new() // 功能需求
    {
        ""一對一聊天"", // 私人訊息
        ""群組聊天（最多 500 人）"", // 群組功能
        ""訊息狀態：已送達、已讀"", // 狀態追蹤
        ""離線訊息：上線後收到"", // 離線支援
        ""多裝置同步"" // 跨裝置
    },
    NonFunctionalRequirements = new() // 非功能需求
    {
        ""即時性：延遲 < 200ms"", // 低延遲
        ""高可用性：99.99%"", // 可用性
        ""訊息不丟失"", // 持久性
        ""支援 1000 萬同時在線"" // 並發量
    }
};
```

### 步驟 2：高層架構

```
參考答案架構圖：

使用者 A ──WebSocket──→ Chat Server 1 ──→ Message Queue ──→ Chat Server 2 ──WebSocket──→ 使用者 B
                              ↓                                    ↓
                        ┌──────────┐                         ┌──────────┐
                        │ User     │                         │ Message  │
                        │ Service  │                         │ Store    │
                        │（在線狀態）│                         │（訊息持久化）│
                        └──────────┘                         └──────────┘
                              ↓
                        ┌──────────┐
                        │ Push     │
                        │ Service  │
                        │（離線推播）│
                        └──────────┘

通訊協定選擇：
  HTTP Polling    ❌ 延遲高、浪費資源
  Long Polling    ⚠️ 可接受但不理想
  WebSocket       ✅ 全雙工、低延遲、最佳選擇
  Server-Sent Events ⚠️ 單向，不適合聊天
```

### 步驟 3：核心實作

```csharp
// WebSocket 聊天服務（ASP.NET Core SignalR）// 中文註解
public class ChatHub : Hub // SignalR Hub 類別
{
    private readonly IChatService _chatService; // 聊天服務

    public ChatHub(IChatService chatService) // 建構函式注入
    {
        _chatService = chatService; // 儲存服務參考
    }

    public async Task SendMessage(string receiverId, string message) // 傳送訊息方法
    {
        var chatMessage = new ChatMessage // 建立訊息物件
        {
            SenderId = Context.UserIdentifier!, // 傳送者 ID
            ReceiverId = receiverId, // 接收者 ID
            Content = message, // 訊息內容
            SentAt = DateTime.UtcNow, // 傳送時間
            Status = MessageStatus.Sent // 初始狀態
        };

        await _chatService.SaveMessageAsync(chatMessage); // 持久化訊息到資料庫

        await Clients.User(receiverId) // 找到接收者的連線
            .SendAsync(""ReceiveMessage"", chatMessage); // 即時推送訊息
    }

    public async Task MarkAsRead(string messageId) // 標記已讀方法
    {
        await _chatService.MarkAsReadAsync(messageId); // 更新資料庫
        var message = await _chatService.GetMessageAsync(messageId); // 取得訊息

        await Clients.User(message.SenderId) // 通知傳送者
            .SendAsync(""MessageRead"", messageId); // 已讀回執
    }

    public override async Task OnConnectedAsync() // 使用者連線事件
    {
        await _chatService.SetOnlineAsync(Context.UserIdentifier!); // 設定為在線
        var pendingMessages = await _chatService // 取得離線訊息
            .GetPendingMessagesAsync(Context.UserIdentifier!); // 查詢未送達的訊息

        foreach (var msg in pendingMessages) // 遍歷離線訊息
        {
            await Clients.Caller.SendAsync(""ReceiveMessage"", msg); // 逐一推送
        }
        await base.OnConnectedAsync(); // 呼叫基底方法
    }
}

// 訊息資料模型 // 中文註解
public class ChatMessage // 聊天訊息實體
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // 唯一 ID
    public string SenderId { get; set; } = """"; // 傳送者
    public string ReceiverId { get; set; } = """"; // 接收者
    public string Content { get; set; } = """"; // 訊息內容
    public DateTime SentAt { get; set; } // 傳送時間
    public MessageStatus Status { get; set; } // 訊息狀態
}

public enum MessageStatus // 訊息狀態列舉
{
    Sent,      // 已傳送
    Delivered, // 已送達
    Read       // 已讀
}
```

---

## CAP 定理

### 面試官問法
> 「請解釋 CAP 定理」

### 標準答案

```
CAP 定理：分散式系統只能同時滿足以下三個中的兩個

  C（Consistency）一致性：所有節點看到的資料相同
  A（Availability）可用性：每個請求都能收到回應
  P（Partition Tolerance）分區容錯：網路斷裂時系統仍能運作

三角形：
         C
        / \
       /   \
      /     \
     A ───── P

常見選擇：
  CP 系統：犧牲可用性（如 MongoDB、Redis Cluster）
  AP 系統：犧牲一致性（如 Cassandra、DynamoDB）
  CA 系統：不存在於分散式環境（單機 SQL 資料庫）
```

```csharp
// CAP 定理在實際系統中的選擇 // 中文註解
var capExamples = new Dictionary<string, string> // CAP 範例
{
    [""銀行轉帳系統""] = ""CP：寧可暫時不可用，也不能出錯"", // 一致性最重要
    [""社群媒體貼文""] = ""AP：可以晚幾秒看到新貼文，但不能掛掉"", // 可用性最重要
    [""電商庫存""] = ""CP：庫存必須準確，避免超賣"", // 一致性優先
    [""新聞推送""] = ""AP：寧可重複推送，也不要漏推"" // 可用性優先
};
```

---

## 微服務 vs 單體式架構

### 標準答案

```
單體式架構（Monolith）：
┌─────────────────────────────┐
│         一個應用程式          │
│  ┌─────┐ ┌─────┐ ┌─────┐  │
│  │使用者│ │訂單 │ │付款 │  │
│  │模組  │ │模組 │ │模組 │  │
│  └─────┘ └─────┘ └─────┘  │
│         共用資料庫           │
└─────────────────────────────┘

微服務架構（Microservices）：
┌────────┐  ┌────────┐  ┌────────┐
│使用者   │  │訂單    │  │付款    │
│服務    │  │服務    │  │服務    │
│  DB1   │  │  DB2   │  │  DB3   │
└───┬────┘  └───┬────┘  └───┬────┘
    └──────┬────┴──────┬────┘
       Message Queue / API Gateway
```

```csharp
// 微服務 vs 單體式的比較 // 中文註解
var comparison = new Dictionary<string, (string Monolith, string Microservice)> // 比較表
{
    [""開發速度""] = (""初期快"", ""初期慢，後期快""), // 開發效率
    [""部署""] = (""整體部署"", ""獨立部署""), // 部署方式
    [""擴展""] = (""整體擴展"", ""個別擴展""), // 擴展彈性
    [""技術棧""] = (""統一"", ""每個服務可不同""), // 技術多樣性
    [""複雜度""] = (""低"", ""高（分散式問題）""), // 系統複雜度
    [""團隊規模""] = (""< 10 人適用"", ""> 20 人適用"") // 團隊建議
};

// 面試加分回答：什麼時候從單體轉微服務？// 中文註解
var migrationSignals = new List<string> // 轉換信號
{
    ""部署頻率需要提高（不同模組有不同的發布節奏）"", // 部署需求
    ""團隊規模超過 20 人，溝通成本太高"", // 團隊規模
    ""某個模組需要獨立擴展（如搜尋服務）"", // 擴展需求
    ""技術棧需要多樣化"" // 技術需求
};
```

---

## 快取策略

### 標準答案

```
常見快取策略：

1. Cache-Aside（旁路快取）：最常用
   讀取：先查 Cache → 沒有 → 查 DB → 寫入 Cache
   寫入：更新 DB → 刪除 Cache（下次讀取時重建）

2. Write-Through（直寫快取）：
   寫入：同時寫 Cache 和 DB
   優點：資料一致性高
   缺點：寫入延遲較高

3. Write-Behind（延遲寫入）：
   寫入：先寫 Cache → 非同步批次寫入 DB
   優點：寫入超快
   缺點：可能丟資料
```

```csharp
// Cache-Aside 模式實作（最常用）// 中文註解
public class ProductService // 商品服務
{
    private readonly IDistributedCache _cache; // 分散式快取
    private readonly AppDbContext _db; // 資料庫上下文

    public ProductService(IDistributedCache cache, AppDbContext db) // 建構函式
    {
        _cache = cache; // 注入快取
        _db = db; // 注入資料庫
    }

    public async Task<Product?> GetProductAsync(int id) // 取得商品
    {
        var cacheKey = $""product:{id}""; // 快取鍵值
        var cached = await _cache.GetStringAsync(cacheKey); // 先查快取

        if (cached != null) // 快取命中
        {
            return JsonSerializer.Deserialize<Product>(cached); // 從快取反序列化
        }

        var product = await _db.Products.FindAsync(id); // 快取未命中，查資料庫
        if (product != null) // 如果資料庫有資料
        {
            var options = new DistributedCacheEntryOptions // 快取選項
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // 30 分鐘過期
            };
            await _cache.SetStringAsync( // 寫入快取
                cacheKey, // 鍵值
                JsonSerializer.Serialize(product), // 序列化
                options); // 過期設定
        }

        return product; // 回傳結果
    }

    public async Task UpdateProductAsync(Product product) // 更新商品
    {
        _db.Products.Update(product); // 更新資料庫
        await _db.SaveChangesAsync(); // 儲存變更
        await _cache.RemoveAsync($""product:{product.Id}""); // 刪除快取（下次讀取重建）
    }
}
```

---

## 訊息佇列

### 標準答案

```
訊息佇列的用途：

1. 非同步處理：下訂單 → 寄 Email（不需要同步等待）
2. 削峰填谷：流量高峰時先放入佇列，慢慢處理
3. 服務解耦：訂單服務不需要直接呼叫庫存服務

架構圖：
  生產者 ──→ [Message Queue] ──→ 消費者
  (Order)    (RabbitMQ /        (Email)
              Azure Service Bus) (SMS)
                                 (Inventory)
```

```csharp
// 使用 Azure Service Bus 的概念範例 // 中文註解
public class OrderEventPublisher // 訂單事件發布者
{
    private readonly ServiceBusSender _sender; // Service Bus 傳送器

    public OrderEventPublisher(ServiceBusClient client) // 建構函式
    {
        _sender = client.CreateSender(""order-events""); // 建立傳送器指向 order-events 佇列
    }

    public async Task PublishOrderCreatedAsync(Order order) // 發布訂單建立事件
    {
        var message = new ServiceBusMessage( // 建立訊息
            JsonSerializer.Serialize(new OrderCreatedEvent // 序列化事件
            {
                OrderId = order.Id, // 訂單 ID
                CustomerId = order.CustomerId, // 客戶 ID
                TotalAmount = order.TotalAmount, // 訂單金額
                CreatedAt = DateTime.UtcNow // 建立時間
            }));

        message.ApplicationProperties[""EventType""] = ""OrderCreated""; // 設定事件類型
        await _sender.SendMessageAsync(message); // 傳送訊息到佇列
    }
}

// 消費者 // 中文註解
public class EmailNotificationHandler // Email 通知處理器
{
    public async Task HandleOrderCreatedAsync(OrderCreatedEvent evt) // 處理訂單事件
    {
        await SendEmailAsync( // 寄送 Email
            evt.CustomerId, // 收件者
            $""您的訂單 {evt.OrderId} 已成立"" // 信件內容
        ); // 非同步處理，不影響訂單流程
    }

    private Task SendEmailAsync(string to, string body) // 寄信方法
    {
        Console.WriteLine($""寄信給 {to}：{body}""); // 模擬寄信
        return Task.CompletedTask; // 完成
    }
}
```

### RabbitMQ vs Azure Service Bus

```csharp
// 選型比較 // 中文註解
var mqComparison = new Dictionary<string, (string RabbitMQ, string AzureServiceBus)> // 比較
{
    [""部署方式""] = (""自架"", ""雲端託管""), // 部署差異
    [""成本""] = (""免費（自架需維護）"", ""按量計費""), // 成本考量
    [""協定""] = (""AMQP"", ""AMQP / HTTP""), // 通訊協定
    [""適用場景""] = (""自建基礎設施"", ""Azure 生態系""), // 使用場景
    [""學習曲線""] = (""中等"", ""低（Azure SDK）"") // 上手難度
};
```

---

## 面試中的白板技巧

> 💡 **比喻：開會用白板**
> 白板設計就像開會時在白板上畫架構圖。
> 面試官在意的是你的**思考過程**，不是畫得多漂亮。

### 白板技巧

```csharp
// 白板設計的注意事項 // 中文註解
var whiteboardTips = new List<(string Tip, string Reason)> // 白板技巧清單
{
    (""先畫大方塊再填細節"", ""避免一開始就陷入細節""), // 由上而下
    (""邊畫邊解釋你的思考"", ""面試官想聽你怎麼想""), // 展現思路
    (""用箭頭標示資料流向"", ""讓架構圖更清楚""), // 視覺化
    (""標示每個元件的技術選型"", ""展現技術廣度""), // 技術知識
    (""主動提出 trade-off"", ""展現成熟的工程思維""), // 權衡能力
    (""不懂的就說不確定但會怎麼查"", ""誠實比裝懂好"") // 坦誠
};

foreach (var (tip, reason) in whiteboardTips) // 遍歷每個技巧
{
    Console.WriteLine($""💡 {tip}""); // 印出技巧
    Console.WriteLine($""   原因：{reason}""); // 印出原因
}
```

### 常見 Trade-off 討論

```csharp
// 面試中常見的 trade-off 討論 // 中文註解
var tradeoffs = new Dictionary<string, (string OptionA, string OptionB, string When)> // 權衡選項
{
    [""SQL vs NoSQL""] = ( // 資料庫選型
        ""SQL：強一致性、關聯查詢"", // 選項 A
        ""NoSQL：高擴展性、彈性 Schema"", // 選項 B
        ""資料有關聯 → SQL；大量非結構化 → NoSQL"" // 選擇時機
    ),
    [""快取一致性 vs 效能""] = ( // 快取策略
        ""即時更新快取：一致性高但慢"", // 選項 A
        ""延遲更新快取：快但可能讀到舊資料"", // 選項 B
        ""金融系統 → 即時；社群媒體 → 延遲"" // 選擇時機
    ),
    [""同步 vs 非同步""] = ( // 處理方式
        ""同步：簡單但慢"", // 選項 A
        ""非同步：快但複雜"", // 選項 B
        ""使用者等待回應 → 同步；背景處理 → 非同步"" // 選擇時機
    )
};

foreach (var tf in tradeoffs) // 遍歷每個 trade-off
{
    Console.WriteLine($""📊 {tf.Key}""); // 印出議題
    Console.WriteLine($""   A: {tf.Value.OptionA}""); // 印出選項 A
    Console.WriteLine($""   B: {tf.Value.OptionB}""); // 印出選項 B
    Console.WriteLine($""   選擇：{tf.Value.When}""); // 印出選擇建議
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：一開始就進入細節

```csharp
// ❌ 錯誤：面試官說「設計一個短網址服務」// 中文註解
// 立刻開始寫 code 或討論資料庫 Schema // 太急了
// 面試官想看你的思考過程，不是 code // 方向錯誤

// ✅ 正確：先確認需求 // 中文註解
var firstQuestions = new List<string> // 先問這些問題
{
    ""預期的使用量是多少？（每日新增 / 每日讀取）"", // 確認規模
    ""短網址需要自訂嗎？還是系統自動生成？"", // 確認功能
    ""需要統計點擊次數嗎？"", // 確認附加功能
    ""短網址有過期時間嗎？"", // 確認生命週期
    ""可用性和一致性哪個優先？"" // 確認 trade-off
};

foreach (var question in firstQuestions) // 遍歷問題
{
    Console.WriteLine($""❓ {question}""); // 在設計前先問面試官
}
```

### ❌ 錯誤 2：忽略非功能需求

```csharp
// ❌ 錯誤：只討論功能，不討論效能和擴展 // 中文註解
// 「我用一個 SQL 資料庫就好了」// 沒考慮規模

// ✅ 正確：主動討論非功能需求 // 中文註解
var nfr = new Dictionary<string, string> // 非功能需求清單
{
    [""效能""] = ""API 回應時間 < 100ms"", // 效能要求
    [""可用性""] = ""99.9%（每年最多 8.76 小時停機）"", // 可用性目標
    [""擴展性""] = ""支援水平擴展，應對流量成長"", // 擴展策略
    [""安全性""] = ""防止惡意網址、Rate Limiting"", // 安全考量
    [""監控""] = ""Application Insights / Grafana"" // 監控方案
};
```

### ❌ 錯誤 3：不討論 Trade-off

```csharp
// ❌ 錯誤：只給一個方案，不解釋為什麼 // 中文註解
// 「用 Redis 做快取就好了」// 沒有比較

// ✅ 正確：分析 trade-off 後再做選擇 // 中文註解
Console.WriteLine(""快取方案比較：""); // 印出標題
Console.WriteLine(""方案 A：Redis（分散式快取）""); // 選項 A
Console.WriteLine(""  優點：跨伺服器共享、持久化、資料結構豐富""); // A 的優點
Console.WriteLine(""  缺點：額外維護成本、網路延遲""); // A 的缺點
Console.WriteLine(""方案 B：MemoryCache（本地快取）""); // 選項 B
Console.WriteLine(""  優點：零延遲、無需額外服務""); // B 的優點
Console.WriteLine(""  缺點：不跨伺服器、重啟後消失""); // B 的缺點
Console.WriteLine(""選擇：分散式環境用 Redis，單機用 MemoryCache""); // 結論
```

---

## 📝 系統設計面試速查表

```
設計題              關鍵元件                     核心 Trade-off
──────────────────────────────────────────────────────────────
短網址服務          Base62 + Cache + DB          讀寫比 100:1，重快取
聊天系統            WebSocket + MQ + DB          即時性 vs 可靠性
搜尋系統            Elasticsearch + Index        精確度 vs 召回率
社群動態            Push vs Pull + Fan-out       寫入成本 vs 讀取成本
限流器              Token Bucket / Sliding Window 精確度 vs 記憶體

面試口訣：
  1. 先問需求，不要急著畫圖
  2. 從高層開始，逐步深入
  3. 主動討論 trade-off
  4. 用數字說話（QPS、儲存量、延遲）
  5. 承認不確定的地方，展現學習能力
```
" }
    };
}