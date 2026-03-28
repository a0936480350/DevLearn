using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_CSharpExtra
{
    public static List<Chapter> GetChapters()
    {
        return new List<Chapter>
        {
            // ── Chapter 103: 字串處理與正規表達式 ──
            new Chapter
            {
                Id = 103,
                Title = "字串處理與正規表達式",
                Slug = "csharp-string-regex",
                Category = "csharp",
                Order = 13,
                Level = "beginner",
                Icon = "📄",
                IsPublished = true,
                Content = @"# 📄 字串處理與正規表達式

## 📌 為什麼字串處理這麼重要？

字串就像日常生活中的**語言**——幾乎所有程式都需要處理文字。不論是讀取使用者輸入、處理檔案內容、還是組合 API 回應，字串無所不在。

想像你經營一家**郵局**：
- **字串方法**就像基本的信件處理工具（拆信、蓋章、裁剪）
- **正規表達式**就像進階的地址辨識系統（自動驗證地址格式是否正確）

---

## 📝 常用字串方法

### Contains、Replace、Split、Trim

```csharp
// 準備一個測試字串
string message = ""  Hello, World! Welcome to C#.  "";

// Trim：去除前後空白（像把信封邊緣多餘的紙裁掉）
string trimmed = message.Trim(); // ""Hello, World! Welcome to C#.""

// Contains：檢查是否包含某段文字（像在信裡找關鍵字）
bool hasWorld = trimmed.Contains(""World""); // true

// Replace：取代文字（像用修正帶把錯字蓋掉再寫上新字）
string replaced = trimmed.Replace(""World"", ""C# 開發者""); // ""Hello, C# 開發者! Welcome to C#.""

// Split：用指定字元切割字串（像把一長串珍珠項鍊拆成一顆顆珍珠）
string csv = ""蘋果,香蕉,橘子,葡萄"";
string[] fruits = csv.Split(','); // [""蘋果"", ""香蕉"", ""橘子"", ""葡萄""]

// 走訪切割後的每個元素
foreach (string fruit in fruits) // 逐一取出每顆水果
{
    Console.WriteLine(fruit); // 印出水果名稱
}
```

### Substring 與索引

```csharp
// 準備一個字串
string text = ""Hello, C# World"";

// Substring：擷取子字串（像從書中撕下某幾頁）
string sub1 = text.Substring(7);     // ""C# World""（從索引 7 開始到結尾）
string sub2 = text.Substring(7, 2);  // ""C#""（從索引 7 開始取 2 個字元）

// IndexOf：找出某段文字第一次出現的位置（像在書裡找某個詞在第幾頁）
int pos = text.IndexOf(""C#"");       // 7

// ToUpper / ToLower：大小寫轉換（像把小寫印章換成大寫印章）
string upper = text.ToUpper();       // ""HELLO, C# WORLD""
string lower = text.ToLower();       // ""hello, c# world""

// StartsWith / EndsWith：檢查開頭或結尾（像看信封上的郵遞區號）
bool startsH = text.StartsWith(""Hello""); // true（是否以 Hello 開頭）
bool endsW = text.EndsWith(""World"");     // true（是否以 World 結尾）
```

---

## 🔗 字串插值 vs String.Format

```csharp
// 基本資料
string name = ""小明""; // 姓名
int age = 25;          // 年齡
double score = 95.5;   // 分數

// 方法 1：字串插值 $""..."" — 最推薦！（像填空題，直接把答案寫在空格裡）
string msg1 = $""我叫 {name}，今年 {age} 歲，考了 {score} 分"";

// 方法 2：String.Format — 傳統方式（像用編號對應答案）
string msg2 = String.Format(""我叫 {0}，今年 {1} 歲，考了 {2} 分"", name, age, score);

// 字串插值支援格式化（像指定小數點要幾位）
string formatted = $""成績：{score:F1}，日期：{DateTime.Now:yyyy-MM-dd}"";
// 結果類似：""成績：95.5，日期：2024-01-15""

// 字串插值支援運算式（像計算機，直接在空格裡算）
string calc = $""{name} 明年 {age + 1} 歲""; // ""小明 明年 26 歲""
```

---

## ⚡ StringBuilder：效能的好幫手

```csharp
// ❌ 不好的做法：大量串接字串（像每次都重新抄一整封信再加一行）
string result = """"; // 空字串
for (int i = 0; i < 10000; i++) // 迴圈一萬次
{
    result += $""第 {i} 行\n""; // 每次都建立新的字串物件，非常慢！
}

// ✅ 好的做法：使用 StringBuilder（像在同一張紙上持續往下寫）
using System.Text; // 引用 StringBuilder 所在的命名空間

StringBuilder sb = new StringBuilder(); // 建立 StringBuilder 實例
for (int i = 0; i < 10000; i++) // 迴圈一萬次
{
    sb.AppendLine($""第 {i} 行""); // 在現有內容後面附加新行，不會建立新物件
}
string output = sb.ToString(); // 最後一次轉成字串

// StringBuilder 常用方法
StringBuilder builder = new StringBuilder(""Hello""); // 初始內容
builder.Append("" World"");    // 附加文字：""Hello World""
builder.Insert(5, "","");      // 在索引 5 插入逗號：""Hello, World""
builder.Replace(""World"", ""C#""); // 取代文字：""Hello, C#""
builder.Remove(5, 2);          // 從索引 5 移除 2 個字元：""HelloC#""
```

> 💡 **何時用 StringBuilder？** 當你需要在迴圈中拼接超過 **10 次**以上的字串時，就該考慮使用 StringBuilder。就像蓋房子，如果只釘一兩根釘子用手動就好，但要釘一萬根就需要電動釘槍。

---

## 🔍 正規表達式 (Regex)

正規表達式就像一個**超級搜尋引擎**——你給它一個「模式」，它就能在文字中找出所有符合模式的內容。

### 基本使用

```csharp
using System.Text.RegularExpressions; // 引用正規表達式命名空間

// IsMatch：檢查字串是否符合模式（像安檢門，通過就 true）
string phone = ""0912-345-678""; // 要驗證的電話號碼
bool isValid = Regex.IsMatch(phone, @""^\d{4}-\d{3}-\d{3}$""); // true
// ^ 表示開頭，\d 表示數字，{4} 表示剛好 4 個，$ 表示結尾

// Match：找出第一個符合的結果（像在人群中找到第一個穿紅衣的人）
string text = ""我的電話是 0912-345-678，辦公室是 02-1234-5678"";
Match match = Regex.Match(text, @""\d{4}-\d{3}-\d{3}""); // 找手機號碼模式
if (match.Success) // 如果有找到
{
    Console.WriteLine($""找到：{match.Value}""); // ""找到：0912-345-678""
}

// Matches：找出所有符合的結果（像找出人群中所有穿紅衣的人）
MatchCollection allMatches = Regex.Matches(text, @""\d+""); // 找出所有連續數字
foreach (Match m in allMatches) // 走訪每一個符合的結果
{
    Console.WriteLine(m.Value); // 印出：0912、345、678、02、1234、5678
}
```

### Replace 與常用模式

```csharp
// Regex.Replace：用模式取代文字（像自動修正所有錯別字）
string dirty = ""價格是   100    元""; // 多餘的空白
string clean = Regex.Replace(dirty, @""\s+"", "" ""); // 把多個空白變成一個
// 結果：""價格是 100 元""

// Email 驗證模式
string email = ""user@example.com""; // 要驗證的 email
string emailPattern = @""^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"";
bool isEmail = Regex.IsMatch(email, emailPattern); // true
// [a-zA-Z0-9._%+-]+ 表示使用者名稱可以包含英文、數字、點等字元

// 手機號碼驗證（台灣格式）
string phonePattern = @""^09\d{2}-?\d{3}-?\d{3}$""; // 09 開頭，可選的破折號
bool isPhone1 = Regex.IsMatch(""0912345678"", phonePattern);  // true（沒有破折號）
bool isPhone2 = Regex.IsMatch(""0912-345-678"", phonePattern); // true（有破折號）
```

---

## 📜 Verbatim 字串與 C# 11 原始字串

```csharp
// 一般字串：需要用 \\ 來表示反斜線（像要寫特殊符號時需要前面加暗號）
string path1 = ""C:\\Users\\Desktop\\file.txt""; // 需要跳脫每個反斜線

// Verbatim 字串 @""...""：所見即所得（像照片，拍什麼就是什麼）
string path2 = @""C:\Users\Desktop\file.txt""; // 不需要跳脫，直接寫路徑
string multiLine = @""第一行
第二行
第三行""; // 可以直接換行，不需要 \n

// 在 verbatim 字串中要表示雙引號，用兩個雙引號
string quote = @""他說：""""你好""""，然後離開了""; // 他說：""你好""，然後離開了

// C# 11 原始字串字面值 """"""...""""""（像超強的保護套，裡面寫什麼都不用跳脫）
string json = """"""
{
    ""name"": ""小明"",
    ""age"": 25
}
""""""; // 不需要跳脫任何雙引號

// 原始字串也支援插值
string name = ""小明"";
string rawInterpolated = $""""""
{{
    ""name"": ""{name}"",
    ""greeting"": ""你好""
}}
""""""; // 用雙大括號 {{ }} 表示 JSON 的大括號
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：混淆 null、空字串和空白字串

```csharp
// ❌ 錯誤寫法：用 == 檢查 null 和空字串
string input = GetUserInput(); // 取得使用者輸入
if (input == null || input == """") // 這樣寫可以，但不夠好
{
    Console.WriteLine(""沒有輸入""); // 印出提示
}
```

```csharp
// ✅ 正確寫法：使用 string.IsNullOrEmpty 或 string.IsNullOrWhiteSpace
string input = GetUserInput(); // 取得使用者輸入

// 檢查 null 或空字串
if (string.IsNullOrEmpty(input)) // 同時處理 null 和 """"
{
    Console.WriteLine(""沒有輸入""); // 印出提示
}

// 更嚴格：還能抓到只有空白的情況（如 ""   ""）
if (string.IsNullOrWhiteSpace(input)) // 同時處理 null、"""" 和 ""   ""
{
    Console.WriteLine(""沒有有效輸入""); // 印出提示
}
```

**解釋：** `string.IsNullOrWhiteSpace` 最安全，因為使用者可能只輸入空白鍵。就像檢查信封裡有沒有信，不只要看信封是不是空的，還要看裡面是不是只塞了白紙。

### ❌ 錯誤 2：字串比較時忽略文化差異

```csharp
// ❌ 錯誤寫法：直接用 == 比較使用者輸入（可能因大小寫不同而失敗）
string userInput = ""hello""; // 使用者輸入的文字
if (userInput == ""Hello"") // false！大小寫不同
{
    Console.WriteLine(""匹配""); // 永遠不會執行到這裡
}
```

```csharp
// ✅ 正確寫法：指定比較方式
string userInput = ""hello""; // 使用者輸入的文字

// 忽略大小寫比較
if (userInput.Equals(""Hello"", StringComparison.OrdinalIgnoreCase)) // true
{
    Console.WriteLine(""匹配""); // 會印出
}

// 或者用 ToLower/ToUpper 統一後再比（但效能較差，因為會建立新字串）
if (userInput.ToLower() == ""hello"") // true，但不推薦
{
    Console.WriteLine(""匹配""); // 會印出
}
```

**解釋：** 就像問路時，""台北車站"" 和 ""台北車站 "" 應該是同一個地方，但電腦會認為它們不同。使用 `StringComparison.OrdinalIgnoreCase` 可以讓比較更有彈性，不會因為大小寫而出錯。

### ❌ 錯誤 3：在迴圈中用 + 拼接大量字串

```csharp
// ❌ 錯誤寫法：在迴圈中用 + 拼接字串（每次都會建立新物件）
string html = """"; // 空字串
for (int i = 0; i < 1000; i++) // 迴圈一千次
{
    html += $""<li>項目 {i}</li>""; // 每次 += 都會建立一個新的字串物件
}
// 這會建立 1000 個暫時的字串物件，浪費記憶體！
```

```csharp
// ✅ 正確寫法：使用 StringBuilder
StringBuilder sb = new StringBuilder(); // 建立 StringBuilder
for (int i = 0; i < 1000; i++) // 迴圈一千次
{
    sb.Append($""<li>項目 {i}</li>""); // 在同一個緩衝區附加文字
}
string html = sb.ToString(); // 最後一次性轉為字串
```

**解釋：** 字串在 C# 中是**不可變的（immutable）**。每次 `+=` 都會建立一個全新的字串物件，舊的就變成垃圾等待回收。就像每次想加一頁就把整本書重新印一次——用 StringBuilder 就像在同一本筆記本上繼續寫，效率高很多。
"
            },

            // ── Chapter 104: 檔案 I/O 與序列化 ──
            new Chapter
            {
                Id = 104,
                Title = "檔案 I/O 與序列化",
                Slug = "csharp-file-io-serialization",
                Category = "csharp",
                Order = 14,
                Level = "intermediate",
                Icon = "📁",
                IsPublished = true,
                Content = @"# 📁 檔案 I/O 與序列化

## 📌 為什麼要學檔案操作？

程式中的資料都存在記憶體裡，一關機就消失了。檔案 I/O 讓你把資料**永久保存**到硬碟上。

想像你的程式是一個**廚師**：
- **記憶體**是工作檯面上的食材（隨時可用，但打烊就收走了）
- **檔案系統**是冰箱和儲藏室（可以長期保存，但取用要多花一點時間）
- **序列化**就是把食材打包裝袋的過程（把物件轉成可儲存的格式）

---

## 📖 基本檔案讀寫

### File.ReadAllText / WriteAllText

```csharp
// 寫入文字到檔案（像把信放進信封裡）
string content = ""Hello, C#！這是我的第一個檔案。""; // 要寫入的內容
File.WriteAllText(""hello.txt"", content); // 寫入檔案，如果檔案不存在會自動建立

// 讀取整個檔案內容（像把信從信封裡拿出來看）
string readContent = File.ReadAllText(""hello.txt""); // 讀取整個檔案
Console.WriteLine(readContent); // 印出檔案內容

// 寫入多行（像在紙上一行一行寫）
string[] lines = { ""第一行"", ""第二行"", ""第三行"" }; // 要寫入的多行文字
File.WriteAllLines(""lines.txt"", lines); // 每個元素寫成一行

// 讀取多行（像一行一行讀信）
string[] readLines = File.ReadAllLines(""lines.txt""); // 讀取所有行
foreach (string line in readLines) // 走訪每一行
{
    Console.WriteLine(line); // 印出每一行
}

// 附加內容到檔案末尾（像在信的最後面繼續寫）
File.AppendAllText(""hello.txt"", ""\n新增的內容""); // 不會覆蓋原本的內容
```

---

## 📊 StreamReader 與 StreamWriter

當檔案很大時，一次全部讀進記憶體不是好主意。Stream 就像一條**水管**——資料像水一樣一點一點流過來。

```csharp
// 使用 StreamWriter 寫入（像打開水龍頭，一滴一滴寫入）
using (StreamWriter writer = new StreamWriter(""log.txt"")) // using 確保寫完後自動關閉檔案
{
    writer.WriteLine(""2024-01-15 09:00 系統啟動""); // 寫入第一行
    writer.WriteLine(""2024-01-15 09:01 使用者登入""); // 寫入第二行
    writer.WriteLine(""2024-01-15 09:05 查詢商品""); // 寫入第三行
} // using 結束時自動呼叫 Dispose()，關閉檔案

// 使用 StreamReader 逐行讀取（像水管接水，一滴一滴接）
using (StreamReader reader = new StreamReader(""log.txt"")) // using 確保讀完後自動關閉
{
    string? line; // 儲存每一行的變數（可能是 null）
    while ((line = reader.ReadLine()) != null) // 一行一行讀，直到沒有資料
    {
        Console.WriteLine(line); // 印出每一行
    }
} // using 結束時自動關閉檔案

// 更簡潔的 using 宣告（C# 8+）
using var writer2 = new StreamWriter(""output.txt""); // 不需要大括號
writer2.WriteLine(""簡潔的寫法""); // 寫入一行
writer2.WriteLine(""到變數離開作用域時自動關閉""); // 寫入另一行
// writer2 在方法結束時自動 Dispose
```

---

## 📂 Path 類別：路徑的好幫手

```csharp
// Path 類別專門處理檔案路徑（像 GPS 導航，幫你組合正確的路徑）
string folder = @""C:\Users\Documents""; // 資料夾路徑
string fileName = ""report.xlsx"";       // 檔案名稱

// Combine：安全地組合路徑（不用自己加 \ 符號）
string fullPath = Path.Combine(folder, fileName); // ""C:\Users\Documents\report.xlsx""

// GetFileName：取得檔案名稱（從完整路徑中擷取最後一段）
string name = Path.GetFileName(fullPath); // ""report.xlsx""

// GetFileNameWithoutExtension：取得不含副檔名的檔案名稱
string nameOnly = Path.GetFileNameWithoutExtension(fullPath); // ""report""

// GetExtension：取得副檔名
string ext = Path.GetExtension(fullPath); // "".xlsx""

// GetDirectoryName：取得所在資料夾路徑
string dir = Path.GetDirectoryName(fullPath); // ""C:\Users\Documents""

// ChangeExtension：變更副檔名（像把 .doc 改成 .pdf）
string pdfPath = Path.ChangeExtension(fullPath, "".pdf""); // ""C:\Users\Documents\report.pdf""
```

---

## 📁 Directory 類別：資料夾操作

```csharp
// 建立資料夾（像蓋一個新房間，如果已經存在就不會出錯）
Directory.CreateDirectory(@""C:\MyApp\Data\Logs""); // 會自動建立中間不存在的資料夾

// 檢查資料夾是否存在（像看看房間在不在）
bool exists = Directory.Exists(@""C:\MyApp\Data""); // true 或 false

// 取得資料夾中所有檔案（像打開房間看裡面有什麼東西）
string[] files = Directory.GetFiles(@""C:\MyApp\Data""); // 取得所有檔案路徑
foreach (string file in files) // 走訪每個檔案
{
    Console.WriteLine(Path.GetFileName(file)); // 只印出檔案名稱
}

// 用篩選條件找特定類型的檔案（像只找房間裡的書）
string[] txtFiles = Directory.GetFiles(@""C:\MyApp\Data"", ""*.txt""); // 只找 .txt 檔案

// 遞迴搜尋所有子資料夾（像搜遍整棟大樓的每個房間）
string[] allFiles = Directory.GetFiles(
    @""C:\MyApp"",        // 從哪裡開始找
    ""*.log"",            // 要找什麼類型
    SearchOption.AllDirectories // 包含所有子資料夾
);

// 取得所有子資料夾（像看大樓裡有哪些房間）
string[] subDirs = Directory.GetDirectories(@""C:\MyApp""); // 取得子資料夾列表
```

---

## 🔄 JSON 序列化：System.Text.Json

序列化就是把程式中的**物件**變成**文字格式**（JSON），這樣才能存到檔案或透過網路傳送。就像把立體的蛋糕拍成**照片**，方便分享給別人，別人再根據照片（反序列化）重新做出蛋糕。

```csharp
using System.Text.Json; // 引用 JSON 序列化命名空間
using System.Text.Json.Serialization; // 引用 JSON 屬性標註命名空間

// 定義一個資料模型
public class Product // 商品類別
{
    [JsonPropertyName(""name"")]    // 指定 JSON 中的屬性名稱為 ""name""
    public string Name { get; set; } = """"; // 商品名稱

    [JsonPropertyName(""price"")]   // 指定 JSON 中的屬性名稱為 ""price""
    public decimal Price { get; set; } // 商品價格

    [JsonPropertyName(""inStock"")] // 指定 JSON 中的屬性名稱為 ""inStock""
    public bool InStock { get; set; } // 是否有庫存

    [JsonIgnore] // 序列化時忽略此屬性（不會出現在 JSON 中）
    public string InternalCode { get; set; } = """"; // 內部代碼
}

// 序列化：物件 → JSON 字串（拍照）
Product product = new Product // 建立商品物件
{
    Name = ""MacBook Pro"", // 設定名稱
    Price = 59900,          // 設定價格
    InStock = true          // 設定庫存狀態
};

// 設定序列化選項
var options = new JsonSerializerOptions
{
    WriteIndented = true,                               // 格式化輸出（縮排，方便閱讀）
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  // 屬性名稱用 camelCase
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // 允許中文不被編碼
};

// 將物件序列化為 JSON 字串
string json = JsonSerializer.Serialize(product, options); // 把物件變成 JSON
Console.WriteLine(json); // 印出 JSON 字串
// 輸出：
// {
//   ""name"": ""MacBook Pro"",
//   ""price"": 59900,
//   ""inStock"": true
// }

// 存到檔案
File.WriteAllText(""product.json"", json); // 將 JSON 字串寫入檔案
```

### 反序列化：JSON → 物件

```csharp
// 從檔案讀取 JSON 並還原成物件（看照片重做蛋糕）
string jsonFromFile = File.ReadAllText(""product.json""); // 從檔案讀取 JSON 字串

// 反序列化：JSON 字串 → 物件
Product? loaded = JsonSerializer.Deserialize<Product>(jsonFromFile, options);
// 用泛型指定要還原成什麼類型

if (loaded != null) // 確認反序列化成功（不是 null）
{
    Console.WriteLine($""商品：{loaded.Name}，價格：{loaded.Price}"");
    // 印出：""商品：MacBook Pro，價格：59900""
}

// 處理集合（多個物件）
List<Product> products = new List<Product> // 建立商品清單
{
    new Product { Name = ""iPhone"", Price = 35900, InStock = true },   // 第一個商品
    new Product { Name = ""iPad"", Price = 27900, InStock = false },    // 第二個商品
    new Product { Name = ""AirPods"", Price = 7490, InStock = true }    // 第三個商品
};

// 序列化整個清單
string jsonArray = JsonSerializer.Serialize(products, options); // 清單 → JSON 陣列
File.WriteAllText(""products.json"", jsonArray); // 存到檔案

// 反序列化 JSON 陣列
string arrayJson = File.ReadAllText(""products.json""); // 從檔案讀取
List<Product>? loadedList = JsonSerializer.Deserialize<List<Product>>(arrayJson, options);
// JSON 陣列 → List<Product>

if (loadedList != null) // 確認不是 null
{
    foreach (var p in loadedList) // 走訪每個商品
    {
        Console.WriteLine($""{p.Name}: ${p.Price}""); // 印出商品資訊
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：沒有使用 using 語句

```csharp
// ❌ 錯誤寫法：沒有用 using，檔案可能不會正確關閉
StreamWriter writer = new StreamWriter(""data.txt""); // 開啟檔案
writer.WriteLine(""重要資料""); // 寫入資料
// 忘記呼叫 writer.Close() 或 writer.Dispose()！
// 如果中間發生例外，檔案會被鎖住無法被其他程式存取
```

```csharp
// ✅ 正確寫法：使用 using 語句確保資源被釋放
using var writer = new StreamWriter(""data.txt""); // using 確保自動關閉
writer.WriteLine(""重要資料""); // 寫入資料
// 即使發生例外，using 也會確保檔案被正確關閉
```

**解釋：** 開啟檔案就像借了一把鑰匙——用完一定要歸還。`using` 語句就像自動歸還機制，不管發生什麼事都會把鑰匙還回去。如果忘記歸還（沒有 Dispose），其他人（程式）就無法使用這個檔案。

### ❌ 錯誤 2：硬編碼檔案路徑

```csharp
// ❌ 錯誤寫法：直接寫死路徑（換台電腦就爆了）
string path = @""C:\Users\小明\Desktop\data.txt""; // 只在小明的電腦上有效
File.ReadAllText(path); // 換台電腦就會 FileNotFoundException
```

```csharp
// ✅ 正確寫法：使用相對路徑或動態取得路徑
// 方法 1：使用應用程式所在目錄
string appDir = AppDomain.CurrentDomain.BaseDirectory; // 取得程式所在路徑
string path1 = Path.Combine(appDir, ""data.txt""); // 組合出完整路徑

// 方法 2：使用特殊資料夾路徑
string desktopPath = Environment.GetFolderPath(
    Environment.SpecialFolder.Desktop // 取得桌面路徑（每台電腦都不同）
);
string path2 = Path.Combine(desktopPath, ""data.txt""); // 組合路徑

// 方法 3：使用當前目錄
string currentDir = Directory.GetCurrentDirectory(); // 取得目前工作目錄
string path3 = Path.Combine(currentDir, ""data"", ""config.json""); // 組合多層路徑
```

**解釋：** 硬編碼路徑就像把你家地址刻在 GPS 上，然後把 GPS 借給住在不同城市的朋友用——當然會找不到路。使用 `Path.Combine` 和環境變數可以讓你的程式在任何電腦上都能正確找到檔案。

### ❌ 錯誤 3：沒有處理 FileNotFoundException

```csharp
// ❌ 錯誤寫法：直接讀取，不管檔案存不存在
string content = File.ReadAllText(""config.json""); // 如果檔案不存在就會爆炸！
```

```csharp
// ✅ 正確寫法：先檢查再讀取，或用 try-catch
// 方法 1：先檢查檔案是否存在
if (File.Exists(""config.json"")) // 先確認檔案存在
{
    string content = File.ReadAllText(""config.json""); // 才去讀取
    Console.WriteLine(content); // 印出內容
}
else
{
    Console.WriteLine(""設定檔不存在，使用預設值""); // 給個友善的訊息
}

// 方法 2：用 try-catch 捕捉例外
try
{
    string content = File.ReadAllText(""config.json""); // 嘗試讀取
    Console.WriteLine(content); // 印出內容
}
catch (FileNotFoundException ex) // 捕捉「檔案不存在」的例外
{
    Console.WriteLine($""找不到檔案：{ex.FileName}""); // 印出錯誤訊息
}
catch (UnauthorizedAccessException) // 捕捉「沒有權限」的例外
{
    Console.WriteLine(""沒有權限讀取此檔案""); // 印出提示
}
```

**解釋：** 不先確認檔案存在就直接讀取，就像不看路就過馬路一樣危險。永遠要假設檔案可能不存在、可能被佔用、可能沒有讀取權限。`try-catch` 就像安全氣囊，出意外時能保護你的程式不會整個崩潰。
"
            }
        };
    }
}
