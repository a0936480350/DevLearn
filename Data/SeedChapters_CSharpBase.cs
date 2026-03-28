using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_CSharpBase
{
    public static List<Chapter> GetChapters()
    {
        return new List<Chapter>
        {
            // ── Chapter 1: C# 介紹與環境設置 ──
            new Chapter
            {
                Id = 1,
                Title = "C# 介紹與環境設置",
                Slug = "csharp-intro",
                Category = "csharp",
                Order = 1,
                Level = "beginner",
                Icon = "🚀",
                IsPublished = true,
                Content = @"# 🚀 C# 介紹與環境設置

## 📌 什麼是 C#？

C#（讀作 ""C Sharp""）是由微軟（Microsoft）在 2000 年推出的現代化程式語言。
它是一種**強型別**、**物件導向**的語言，廣泛用於：

- 🌐 網頁應用（ASP.NET Core）
- 🖥️ 桌面應用（WPF、WinForms）
- 🎮 遊戲開發（Unity）
- 📱 手機應用（MAUI）
- ☁️ 雲端服務（Azure）

### C# 的歷史

| 年份 | 版本 | 重要功能 |
|------|------|----------|
| 2002 | C# 1.0 | 基本語言功能 |
| 2005 | C# 2.0 | 泛型（Generics） |
| 2007 | C# 3.0 | LINQ、Lambda |
| 2012 | C# 5.0 | async/await |
| 2019 | C# 8.0 | 可空參考型別 |
| 2023 | C# 12 | 主要建構子 |

### C# 的特色

```csharp
// C# 是強型別語言 — 每個變數都必須有明確的型別
int age = 25;          // 整數型別
string name = ""小明""; // 字串型別
bool isStudent = true; // 布林型別

// C# 支援自動記憶體管理（垃圾回收 GC）
// 你不需要手動釋放記憶體，GC 會幫你處理
```

## 📌 安裝開發環境

### 第一步：安裝 .NET SDK

.NET SDK 是開發 C# 程式的必備工具包。

```bash
# 到 https://dotnet.microsoft.com/download 下載最新版 .NET SDK
# 安裝完成後，打開終端機（Terminal）確認安裝成功
dotnet --version  # 顯示已安裝的 .NET 版本號碼
```

### 第二步：安裝 VS Code

VS Code 是一個輕量但功能強大的程式碼編輯器。

```bash
# 到 https://code.visualstudio.com 下載 VS Code
# 安裝後，打開 VS Code 並安裝以下擴充功能：
# 1. C# Dev Kit — 提供 C# 語法高亮和智慧提示
# 2. .NET Install Tool — 管理 .NET 版本
```

### 第三步：確認環境

```bash
# 確認 dotnet 命令可以使用
dotnet --info  # 顯示完整的 .NET 環境資訊
```

## 📌 第一個程式：Hello World

### 建立新專案

```bash
# 建立一個新的主控台應用程式（Console App）
dotnet new console -n MyFirstApp  # -n 指定專案名稱

# 進入專案資料夾
cd MyFirstApp  # cd 是切換目錄的命令

# 執行程式
dotnet run  # 編譯並執行程式
```

### 程式碼詳解

```csharp
// 這是 C# 最簡單的程式（頂層陳述式 Top-level statements）
// 從 C# 9 開始，你可以省略 namespace 和 Main 方法

// Console 是系統提供的類別，用來處理控制台輸入輸出
// WriteLine 是 Console 的方法，會印出文字並換行
Console.WriteLine(""Hello, World!""); // 印出 Hello, World! 到螢幕

// 你也可以用 Write 方法（不換行）
Console.Write(""你好，"");  // 印出「你好，」但不換行
Console.Write(""世界！"");  // 接在同一行印出「世界！」
```

### 傳統寫法（了解即可）

```csharp
// 這是 C# 9 之前的標準寫法
// using 關鍵字引入 System 命名空間
using System;

// namespace 定義命名空間，用來組織程式碼
namespace MyFirstApp
{
    // class 定義一個類別
    class Program
    {
        // Main 是程式的進入點（Entry Point）
        // static 表示不需要建立物件就能呼叫
        // void 表示這個方法不回傳任何值
        // string[] args 接收命令列參數
        static void Main(string[] args)
        {
            // 印出歡迎訊息
            Console.WriteLine(""Hello, World!"");
        }
    }
}
```

## 📌 常用 dotnet 命令

```bash
# 建立新專案
dotnet new console -n 專案名稱    # 建立主控台應用程式
dotnet new webapi -n 專案名稱     # 建立 Web API 專案
dotnet new mvc -n 專案名稱        # 建立 MVC 網頁專案

# 管理專案
dotnet build      # 編譯專案（將 C# 編譯成可執行檔）
dotnet run        # 編譯並執行專案
dotnet clean      # 清除編譯產生的檔案

# 管理套件（NuGet）
dotnet add package Newtonsoft.Json  # 安裝 JSON 處理套件
dotnet list package                 # 列出已安裝的套件
dotnet remove package 套件名稱     # 移除指定套件
```

## 📌 字串插值（String Interpolation）

```csharp
// 字串插值是 C# 6 引入的功能，用 $ 符號開頭
string name = ""小明"";  // 宣告一個字串變數
int age = 20;            // 宣告一個整數變數

// 使用 $ 開頭，大括號 {} 內放入變數或運算式
Console.WriteLine($""我的名字是 {name}，今年 {age} 歲"");
// 輸出：我的名字是 小明，今年 20 歲

// 大括號內也可以放運算式
Console.WriteLine($""明年我就 {age + 1} 歲了"");
// 輸出：明年我就 21 歲了

// 格式化數字
double price = 1234.5;  // 宣告一個雙精度浮點數
Console.WriteLine($""價格：{price:C}"");   // C 格式 → 貨幣格式
Console.WriteLine($""價格：{price:F2}"");  // F2 格式 → 小數點兩位
```

## 📌 使用者輸入

```csharp
// Console.ReadLine() 會等待使用者輸入一行文字
Console.Write(""請輸入你的名字："");  // 提示使用者
string? input = Console.ReadLine();   // 讀取使用者輸入（可能為 null）

// 用 ?? 運算子提供預設值（當 input 為 null 時使用 ""訪客""）
string userName = input ?? ""訪客"";

// 印出問候語
Console.WriteLine($""歡迎你，{userName}！"");

// 讀取數字（需要轉換型別）
Console.Write(""請輸入你的年齡："");       // 提示使用者
string? ageInput = Console.ReadLine();     // 讀取字串
int userAge = int.Parse(ageInput ?? ""0""); // 將字串轉成整數

// 更安全的寫法：TryParse（轉換失敗不會拋出例外）
Console.Write(""請輸入你的身高(cm)："");    // 提示使用者
string? heightInput = Console.ReadLine();   // 讀取字串
if (int.TryParse(heightInput, out int height)) // TryParse 回傳是否成功
{
    Console.WriteLine($""你的身高是 {height} 公分""); // 轉換成功
}
else
{
    Console.WriteLine(""輸入的不是有效數字！"");       // 轉換失敗
}
```

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：忘記安裝 .NET SDK

```bash
# 錯誤訊息：'dotnet' is not recognized
# 原因：還沒安裝 .NET SDK，或是環境變數沒設好
# 解法：到 https://dotnet.microsoft.com/download 下載安裝
```

### ❌ 錯誤：大小寫搞混

```csharp
// ❌ 錯誤：C# 區分大小寫！
console.writeline(""Hello"");  // console 和 writeline 開頭應該大寫

// ✅ 正確：
Console.WriteLine(""Hello"");  // Console 和 WriteLine 首字母大寫
```

### ❌ 錯誤：忘記分號

```csharp
// ❌ 錯誤：每行陳述式結尾都要加分號
Console.WriteLine(""Hello"")   // 少了分號 → 編譯錯誤

// ✅ 正確：
Console.WriteLine(""Hello"");  // 記得加上分號 ;
```

### ❌ 錯誤：字串沒有用雙引號

```csharp
// ❌ 錯誤：字串必須用雙引號包起來
string name = 小明;         // 編譯器會以為 小明 是變數名稱

// ✅ 正確：
string name = ""小明"";     // 用雙引號包起來才是字串
```

### ❌ 錯誤：在錯誤的目錄執行 dotnet run

```bash
# ❌ 錯誤：不在專案資料夾中執行
# 錯誤訊息：Couldn't find a project to run
# 解法：先 cd 到包含 .csproj 檔案的資料夾
cd MyFirstApp     # 切換到專案目錄
dotnet run        # 再執行程式
```
"
            },

            // ── Chapter 2: 變數、型別與運算子 ──
            new Chapter
            {
                Id = 2,
                Title = "變數、型別與運算子",
                Slug = "csharp-variables",
                Category = "csharp",
                Order = 2,
                Level = "beginner",
                Icon = "📦",
                IsPublished = true,
                Content = @"# 📦 變數、型別與運算子

## 📌 什麼是變數？

變數就像一個**有標籤的盒子**，你可以在裡面放東西（資料），標籤就是變數名稱。

```csharp
// 宣告變數的基本語法：型別 變數名稱 = 初始值;
int age = 25;              // 一個裝整數的盒子，標籤叫 age
string name = ""小明"";    // 一個裝字串的盒子，標籤叫 name
double height = 175.5;     // 一個裝小數的盒子，標籤叫 height
bool isAlive = true;       // 一個裝布林值的盒子，標籤叫 isAlive
```

## 📌 值型別（Value Types）

值型別直接儲存資料本身，就像盒子裡直接放了東西。

```csharp
// 整數型別
byte small = 255;          // 0 到 255（1 byte = 8 位元）
short medium = 32767;      // -32768 到 32767（2 bytes）
int normal = 2147483647;   // 最常用的整數型別（4 bytes）
long big = 9223372036854775807L; // 超大整數，後面加 L（8 bytes）

// 浮點數型別
float price = 19.99f;      // 單精度浮點數，後面加 f（4 bytes）
double pi = 3.14159265;    // 雙精度浮點數，最常用的小數型別（8 bytes）
decimal money = 1000.50m;  // 精確小數，適合金融計算，後面加 m（16 bytes）

// 布林型別
bool isOpen = true;        // 只有 true（真）或 false（假）兩個值

// 字元型別
char grade = 'A';          // 用單引號，只能放一個字元
char heart = '♥';          // 也可以放 Unicode 字元

// 為什麼要用 decimal 而不是 double 算錢？
Console.WriteLine(0.1 + 0.2);          // 輸出 0.30000000000000004（不精確！）
Console.WriteLine(0.1m + 0.2m);        // 輸出 0.3（精確！）
```

## 📌 參考型別（Reference Types）

參考型別儲存的是資料的「地址」，就像盒子裡放了一張紙條，寫著東西放在哪裡。

```csharp
// string 字串 — 最常用的參考型別
string greeting = ""你好世界"";    // 字串用雙引號包起來
string empty = """";               // 空字串（有盒子，但裡面沒東西）
string? nullable = null;           // null 表示「沒有盒子」

// object — 所有型別的基底類別
object anything = 42;              // 可以放任何東西
anything = ""現在變成字串了"";     // 可以改放不同型別的值

// dynamic — 動態型別（執行時才檢查型別）
dynamic flexible = 100;            // 目前是整數
flexible = ""hello"";              // 現在變成字串（編譯時不會報錯）
// flexible.SomeMethod();          // 如果方法不存在，執行時才會錯
```

## 📌 var 關鍵字與型別推斷

```csharp
// var 讓編譯器自動推斷變數的型別
var count = 10;            // 編譯器推斷為 int
var message = ""Hello"";   // 編譯器推斷為 string
var ratio = 3.14;          // 編譯器推斷為 double
var isReady = false;       // 編譯器推斷為 bool

// ⚠️ 使用 var 的注意事項
// var 必須在宣告時就給初始值
// var x;                  // ❌ 錯誤！編譯器無法推斷型別
var x = 0;                 // ✅ 正確！

// var 不等於 dynamic，var 在編譯時就確定型別
var num = 10;              // num 就是 int，不能再賦值為其他型別
// num = ""hello"";        // ❌ 錯誤！不能把字串賦值給 int
```

## 📌 算術運算子

```csharp
int a = 10;  // 宣告變數 a 並賦值 10
int b = 3;   // 宣告變數 b 並賦值 3

// 基本運算
Console.WriteLine(a + b);   // 加法：13
Console.WriteLine(a - b);   // 減法：7
Console.WriteLine(a * b);   // 乘法：30
Console.WriteLine(a / b);   // 整數除法：3（小數部分被捨去！）
Console.WriteLine(a % b);   // 取餘數（模運算）：1

// 注意：整數除法會捨去小數
Console.WriteLine(10 / 3);      // 輸出 3（不是 3.333...）
Console.WriteLine(10.0 / 3);    // 輸出 3.333...（其中一個是小數就會保留）

// 遞增與遞減
int counter = 0;             // 計數器初始值為 0
counter++;                   // counter 變成 1（等於 counter = counter + 1）
counter--;                   // counter 變回 0（等於 counter = counter - 1）

// 複合賦值運算子
int score = 100;             // 分數初始值 100
score += 10;                 // score 變成 110（等於 score = score + 10）
score -= 20;                 // score 變成 90
score *= 2;                  // score 變成 180
score /= 3;                  // score 變成 60
```

## 📌 比較運算子

```csharp
int x = 10;  // 宣告變數
int y = 20;  // 宣告變數

// 比較運算子回傳 bool（true 或 false）
Console.WriteLine(x == y);   // 等於：false（10 不等於 20）
Console.WriteLine(x != y);   // 不等於：true（10 確實不等於 20）
Console.WriteLine(x > y);    // 大於：false
Console.WriteLine(x < y);    // 小於：true
Console.WriteLine(x >= 10);  // 大於等於：true
Console.WriteLine(x <= 5);   // 小於等於：false
```

## 📌 邏輯運算子

```csharp
bool sunny = true;    // 今天是晴天
bool warm = false;    // 今天不暖和

// && （AND）— 兩個都要 true 結果才是 true
Console.WriteLine(sunny && warm);    // false（晴天但不暖和）

// || （OR）— 其中一個 true 結果就是 true
Console.WriteLine(sunny || warm);    // true（至少是晴天）

// ! （NOT）— 反轉 true/false
Console.WriteLine(!sunny);           // false（把 true 反轉成 false）

// 實際應用範例
int age = 20;                        // 年齡
bool hasLicense = true;              // 有駕照

// 判斷是否可以開車：年滿 18 歲「且」有駕照
bool canDrive = age >= 18 && hasLicense;  // true
Console.WriteLine($""可以開車：{canDrive}""); // 輸出：可以開車：True
```

## 📌 型別轉換

```csharp
// 隱式轉換（小型別 → 大型別，安全不會遺失資料）
int smallNumber = 100;               // int 佔 4 bytes
long bigNumber = smallNumber;        // long 佔 8 bytes，自動轉換
float f = smallNumber;               // float 佔 4 bytes，可以放 int
double d = f;                        // double 佔 8 bytes，可以放 float

// 顯式轉換（大型別 → 小型別，可能遺失資料，需要強制轉換）
double pi = 3.14159;                 // 雙精度浮點數
int rounded = (int)pi;               // 強制轉換，小數部分被捨去 → 3
Console.WriteLine(rounded);          // 輸出：3

long hugeValue = 999999999999L;      // 超大的 long 值
int truncated = (int)hugeValue;      // ⚠️ 溢位！結果不正確

// Convert 類別（最安全的轉換方式）
string numberText = ""42"";          // 這是字串 ""42""，不是數字
int number = Convert.ToInt32(numberText);  // 轉換成整數 42
double dec = Convert.ToDouble(""3.14"");   // 轉換成浮點數 3.14
bool flag = Convert.ToBoolean(""true"");   // 轉換成布林值 true
string back = Convert.ToString(42);        // 轉換回字串 ""42""

// Parse 與 TryParse
int parsed = int.Parse(""123"");           // 轉換成功 → 123
// int fail = int.Parse(""abc"");          // ❌ 會拋出 FormatException

// TryParse 是最安全的方式（不會拋出例外）
if (int.TryParse(""456"", out int result)) // 嘗試轉換
{
    Console.WriteLine($""轉換成功：{result}""); // 輸出：轉換成功：456
}
else
{
    Console.WriteLine(""轉換失敗"");             // 如果輸入不是數字
}
```

## 📌 常數與唯讀

```csharp
// const — 編譯時常數（值在編譯時就確定，永遠不能改變）
const double PI = 3.14159265358979;  // 圓周率是不變的
const int MAX_SCORE = 100;           // 最高分是固定的
// PI = 3.14;                        // ❌ 錯誤！常數不能修改

// readonly — 執行時常數（可以在建構子中設定值）
// readonly 只能用在類別的欄位，不能用在方法內
```

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：整數除法的陷阱

```csharp
// ❌ 期望得到 3.333... 但得到 3
int a = 10;       // 整數
int b = 3;        // 整數
var result = a / b; // 整數 / 整數 = 整數（小數被捨去）
// result 是 3，不是 3.333...

// ✅ 正確：至少一個運算元要是小數型別
double result2 = (double)a / b;  // 先把 a 轉成 double 再除
// result2 是 3.333...
```

### ❌ 錯誤：float 和 decimal 忘記加後綴

```csharp
// ❌ 錯誤：
// float price = 19.99;    // 錯誤！19.99 預設是 double
// decimal money = 100.50; // 錯誤！100.50 預設是 double

// ✅ 正確：
float price = 19.99f;      // 加上 f 後綴
decimal money = 100.50m;   // 加上 m 後綴
```

### ❌ 錯誤：null 與空字串搞混

```csharp
string? a = null;   // a 沒有指向任何東西（沒有盒子）
string b = """";    // b 指向一個空字串（有盒子，但裡面是空的）

// a.Length          // ❌ NullReferenceException！null 沒有 Length
Console.WriteLine(b.Length); // ✅ 輸出 0（空字串長度為 0）

// 安全存取：用 ?. 運算子
Console.WriteLine(a?.Length); // 輸出 null（不會報錯）
```

### ❌ 錯誤：== 和 = 搞混

```csharp
int x = 10;        // = 是「賦值」（把 10 放進 x）
// if (x = 5)      // ❌ 錯誤！這是賦值，不是比較
if (x == 5)        // ✅ 正確！== 是「比較」（x 是否等於 5）
{
    Console.WriteLine(""x 等於 5"");
}
```
"
            },

            // ── Chapter 3: 控制流程 ──
            new Chapter
            {
                Id = 3,
                Title = "控制流程",
                Slug = "csharp-control-flow",
                Category = "csharp",
                Order = 3,
                Level = "beginner",
                Icon = "🔀",
                IsPublished = true,
                Content = @"# 🔀 控制流程

## 📌 什麼是控制流程？

程式預設是**從上到下**一行一行執行。控制流程讓你可以：
- **做選擇**（如果...就...否則...）
- **重複執行**（迴圈）
- **跳過或中斷**（break、continue）

## 📌 if / else if / else 條件判斷

```csharp
int score = 85;  // 學生的成績

// if 是最基本的條件判斷
if (score >= 90) // 如果成績大於等於 90
{
    Console.WriteLine(""優秀！"");  // 印出「優秀！」
}
else if (score >= 80) // 否則如果成績大於等於 80
{
    Console.WriteLine(""良好！"");  // 印出「良好！」
}
else if (score >= 60) // 否則如果成績大於等於 60
{
    Console.WriteLine(""及格"");    // 印出「及格」
}
else // 以上條件都不符合時
{
    Console.WriteLine(""不及格"");  // 印出「不及格」
}
// 輸出：良好！（因為 85 >= 80）

// 巢狀 if（if 裡面再放 if）
int age = 20;          // 年齡
bool hasMoney = true;  // 有沒有錢

if (age >= 18) // 先判斷是否成年
{
    if (hasMoney) // 再判斷有沒有錢
    {
        Console.WriteLine(""可以買東西""); // 成年且有錢
    }
    else
    {
        Console.WriteLine(""成年但沒錢""); // 成年但沒錢
    }
}

// 更簡潔的寫法：用 && 合併條件
if (age >= 18 && hasMoney) // 成年「且」有錢
{
    Console.WriteLine(""可以買東西"");
}
```

## 📌 switch 表達式（C# 8+）

```csharp
// 傳統 switch 語句
int dayNumber = 3;  // 星期幾（1-7）

switch (dayNumber) // 根據 dayNumber 的值來判斷
{
    case 1:                             // 如果是 1
        Console.WriteLine(""星期一"");  // 印出「星期一」
        break;                          // break 表示結束這個 case
    case 2:                             // 如果是 2
        Console.WriteLine(""星期二"");
        break;
    case 3:                             // 如果是 3
        Console.WriteLine(""星期三"");
        break;
    case 6:                             // 如果是 6
    case 7:                             // 或 7（可以合併多個 case）
        Console.WriteLine(""週末"");
        break;
    default:                            // 以上都不是
        Console.WriteLine(""其他"");
        break;
}

// C# 8+ switch 表達式（更簡潔的寫法）
string dayName = dayNumber switch  // 根據 dayNumber 回傳對應的字串
{
    1 => ""星期一"",    // 如果是 1，回傳「星期一」
    2 => ""星期二"",    // 如果是 2，回傳「星期二」
    3 => ""星期三"",    // 如果是 3，回傳「星期三」
    4 => ""星期四"",
    5 => ""星期五"",
    6 or 7 => ""週末"", // 6 或 7 都回傳「週末」
    _ => ""無效""       // _ 是預設值（等同 default）
};

Console.WriteLine(dayName); // 輸出：星期三

// 模式匹配（Pattern Matching）— 更進階的 switch
int temperature = 35;  // 氣溫

string weather = temperature switch
{
    <= 0 => ""結冰了！"",          // 小於等於 0 度
    > 0 and <= 15 => ""很冷"",    // 0 到 15 度
    > 15 and <= 25 => ""舒適"",   // 15 到 25 度
    > 25 and <= 35 => ""很熱"",   // 25 到 35 度
    > 35 => ""超級熱！"",         // 超過 35 度
};
Console.WriteLine($""氣溫 {temperature}°C：{weather}""); // 輸出：氣溫 35°C：很熱
```

## 📌 for 迴圈

```csharp
// for 迴圈：當你知道要重複幾次時使用
// for (初始值; 條件; 更新) { 要重複的程式碼 }

for (int i = 0; i < 5; i++)  // i 從 0 開始，每次 +1，到 5 停止
{
    Console.WriteLine($""第 {i} 次"");  // 印出目前是第幾次
}
// 輸出：第 0 次、第 1 次、第 2 次、第 3 次、第 4 次

// 倒數迴圈
for (int i = 10; i >= 1; i--)  // i 從 10 開始，每次 -1，到 1 停止
{
    Console.WriteLine($""倒數：{i}""); // 印出倒數
}

// 九九乘法表（巢狀迴圈）
for (int i = 1; i <= 9; i++)       // 外層：被乘數 1-9
{
    for (int j = 1; j <= 9; j++)   // 內層：乘數 1-9
    {
        // \t 是 Tab 字元，用來對齊
        Console.Write($""{i}x{j}={i * j}\t""); // 印出乘法結果
    }
    Console.WriteLine(); // 每一列結束後換行
}
```

## 📌 foreach 迴圈

```csharp
// foreach 用來走訪集合中的每一個元素
string[] fruits = { ""蘋果"", ""香蕉"", ""橘子"", ""芒果"" }; // 水果陣列

foreach (string fruit in fruits) // 依序取出每個水果
{
    Console.WriteLine($""我喜歡吃 {fruit}""); // 印出每個水果
}
// 輸出：我喜歡吃 蘋果、我喜歡吃 香蕉、...

// foreach 也可以用 var
foreach (var fruit in fruits) // 讓編譯器自動推斷型別
{
    Console.WriteLine(fruit); // 印出水果名稱
}

// 走訪字串中的每個字元
string word = ""Hello"";  // 一個英文字串
foreach (char c in word)  // 字串是字元的集合
{
    Console.Write($""{c} ""); // 印出每個字元
}
// 輸出：H e l l o
```

## 📌 while 與 do-while 迴圈

```csharp
// while 迴圈：先判斷條件，再執行
int count = 0;           // 計數器從 0 開始

while (count < 3)        // 當 count 小於 3 時持續執行
{
    Console.WriteLine($""count = {count}""); // 印出目前的 count
    count++;             // count 加 1（很重要！否則會無窮迴圈）
}
// 輸出：count = 0、count = 1、count = 2

// do-while 迴圈：先執行一次，再判斷條件
string? input;           // 使用者輸入的字串

do
{
    Console.Write(""請輸入 'exit' 離開："");  // 提示使用者
    input = Console.ReadLine();              // 讀取輸入
} while (input != ""exit"");                 // 當輸入不是 exit 時繼續
// do-while 保證至少執行一次

Console.WriteLine(""程式結束"");  // 使用者輸入 exit 後到這裡
```

## 📌 break、continue、return

```csharp
// break — 立刻跳出迴圈
for (int i = 0; i < 10; i++) // 預計跑 10 次
{
    if (i == 5)              // 但當 i 等於 5 時
    {
        break;               // 立刻停止迴圈
    }
    Console.WriteLine(i);    // 只會印出 0, 1, 2, 3, 4
}

// continue — 跳過這次，繼續下一次
for (int i = 0; i < 10; i++) // 跑 10 次
{
    if (i % 2 == 0)          // 如果 i 是偶數
    {
        continue;            // 跳過這次迴圈
    }
    Console.WriteLine(i);    // 只會印出 1, 3, 5, 7, 9（奇數）
}

// return — 結束整個方法
// return 會在「方法與函式」章節詳細說明
```

## 📌 三元運算子 ? :

```csharp
// 三元運算子是 if/else 的簡化寫法
// 語法：條件 ? 條件為 true 時的值 : 條件為 false 時的值

int age = 20;  // 年齡

// 傳統 if/else 寫法
string status;
if (age >= 18)
{
    status = ""成年"";
}
else
{
    status = ""未成年"";
}

// 三元運算子寫法（一行搞定！）
string status2 = age >= 18 ? ""成年"" : ""未成年"";  // 條件成立回傳「成年」
Console.WriteLine(status2); // 輸出：成年

// 也可以直接在 Console.WriteLine 中使用
int score = 75; // 分數
Console.WriteLine(score >= 60 ? ""及格 ✅"" : ""不及格 ❌"");
// 輸出：及格 ✅

// ⚠️ 不要過度使用三元運算子
// 巢狀的三元運算子很難讀懂：
// string result = a > b ? (a > c ? ""a最大"" : ""c最大"") : (b > c ? ""b最大"" : ""c最大"");
// 建議複雜條件還是用 if/else 或 switch
```

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：忘記 break 導致穿透

```csharp
// ❌ 錯誤：C# 的 switch 每個 case 都需要 break
switch (day)
{
    case 1:
        Console.WriteLine(""星期一"");
        // 忘記 break → 編譯錯誤！（C# 不允許隱式穿透）
    case 2:
        Console.WriteLine(""星期二"");
        break;
}

// ✅ 正確：每個 case 都要有 break（或 return）
switch (day)
{
    case 1:
        Console.WriteLine(""星期一"");
        break;  // 記得加 break
    case 2:
        Console.WriteLine(""星期二"");
        break;
}
```

### ❌ 錯誤：無窮迴圈

```csharp
// ❌ 錯誤：忘記更新計數器
int i = 0;
while (i < 5)
{
    Console.WriteLine(i);
    // 忘記 i++，i 永遠是 0，迴圈永遠不會停！
}

// ✅ 正確：
int j = 0;
while (j < 5)
{
    Console.WriteLine(j);
    j++;  // 記得更新計數器！
}
```

### ❌ 錯誤：在 foreach 中修改集合

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 }; // 建立一個 List

// ❌ 錯誤：不能在 foreach 中修改正在走訪的集合
// foreach (var item in list)
// {
//     if (item == 3) list.Remove(item); // 會拋出 InvalidOperationException
// }

// ✅ 正確：用 RemoveAll 或先轉成新的集合
list.RemoveAll(x => x == 3);  // 移除所有等於 3 的元素
```
"
            },

            // ── Chapter 4: 方法與函式 ──
            new Chapter
            {
                Id = 4,
                Title = "方法與函式",
                Slug = "csharp-methods",
                Category = "csharp",
                Order = 4,
                Level = "beginner",
                Icon = "🔧",
                IsPublished = true,
                Content = @"# 🔧 方法與函式

## 📌 什麼是方法？

方法（Method）就像一台**自動販賣機**：
- 你投入硬幣（**參數**）
- 機器處理（**方法本體**）
- 吐出飲料（**回傳值**）

## 📌 方法宣告

```csharp
// 方法的基本語法
// 存取修飾詞 回傳型別 方法名稱(參數列表)
// {
//     方法本體
//     return 回傳值;
// }

// 有回傳值的方法
static int Add(int a, int b)  // 接收兩個整數，回傳一個整數
{
    return a + b;  // 回傳兩數相加的結果
}

// 沒有回傳值的方法（void）
static void SayHello(string name)  // 接收一個字串，不回傳任何值
{
    Console.WriteLine($""你好，{name}！"");  // 印出問候語
}

// 呼叫方法
int sum = Add(3, 5);        // 呼叫 Add 方法，傳入 3 和 5
Console.WriteLine(sum);      // 輸出：8

SayHello(""小明"");          // 呼叫 SayHello 方法
// 輸出：你好，小明！
```

## 📌 ref、out、params 關鍵字

```csharp
// ref — 傳參考（方法內的修改會影響外面的變數）
static void Double(ref int number)  // ref 表示傳入變數的參考
{
    number *= 2;  // 直接修改原來的變數
}

int value = 10;                  // 原始值是 10
Double(ref value);               // 用 ref 傳入
Console.WriteLine(value);        // 輸出：20（原始變數被修改了！）

// out — 輸出參數（方法必須在裡面賦值）
static void Divide(int a, int b, out int quotient, out int remainder)
{
    quotient = a / b;   // 商數（必須賦值）
    remainder = a % b;  // 餘數（必須賦值）
}

Divide(10, 3, out int q, out int r);  // 呼叫時用 out 宣告變數
Console.WriteLine($""商：{q}，餘：{r}""); // 輸出：商：3，餘：1

// 實際範例：int.TryParse 就是用 out 的好例子
if (int.TryParse(""42"", out int parsed))  // out 接收轉換結果
{
    Console.WriteLine($""轉換成功：{parsed}""); // 輸出：轉換成功：42
}

// params — 可變數量參數（接受任意數量的參數）
static int Sum(params int[] numbers)  // params 讓你傳入任意多個整數
{
    int total = 0;                     // 總和初始值
    foreach (int n in numbers)         // 走訪所有傳入的數字
    {
        total += n;                    // 累加
    }
    return total;                      // 回傳總和
}

Console.WriteLine(Sum(1, 2, 3));           // 傳入 3 個參數 → 6
Console.WriteLine(Sum(1, 2, 3, 4, 5));     // 傳入 5 個參數 → 15
Console.WriteLine(Sum());                   // 傳入 0 個參數 → 0
```

## 📌 選擇性參數與具名引數

```csharp
// 選擇性參數（Optional Parameters）— 有預設值的參數
static void Greet(string name, string greeting = ""你好"", int times = 1)
{
    for (int i = 0; i < times; i++)          // 重複印出 times 次
    {
        Console.WriteLine($""{greeting}，{name}！""); // 印出問候語
    }
}

Greet(""小明"");                    // 使用所有預設值 → 你好，小明！
Greet(""小明"", ""哈囉"");          // 覆蓋 greeting → 哈囉，小明！
Greet(""小明"", ""嗨"", 3);         // 覆蓋兩個預設值，印 3 次

// 具名引數（Named Arguments）— 指定參數名稱
Greet(name: ""小美"", times: 2);    // 跳過 greeting，只設 times
Greet(times: 3, name: ""小華"", greeting: ""早安""); // 順序可以不同
```

## 📌 方法多載（Method Overloading）

```csharp
// 方法多載：同名方法，但參數不同
static int Multiply(int a, int b)       // 整數版本
{
    return a * b; // 兩個整數相乘
}

static double Multiply(double a, double b)  // 浮點數版本
{
    return a * b; // 兩個浮點數相乘
}

static int Multiply(int a, int b, int c)    // 三個參數版本
{
    return a * b * c; // 三個整數相乘
}

// 編譯器會根據傳入的參數自動選擇正確的版本
Console.WriteLine(Multiply(3, 4));         // 呼叫 int 版本 → 12
Console.WriteLine(Multiply(2.5, 3.0));     // 呼叫 double 版本 → 7.5
Console.WriteLine(Multiply(2, 3, 4));      // 呼叫三參數版本 → 24
```

## 📌 表達式主體方法（Expression-bodied Methods）

```csharp
// 如果方法本體只有一行，可以用 => 簡化
static int Square(int x) => x * x;  // 回傳 x 的平方

static bool IsEven(int n) => n % 2 == 0;  // 判斷是否為偶數

static string GetGreeting(string name) => $""Hello, {name}!""; // 回傳問候語

static void PrintLine() => Console.WriteLine(""---"");  // 印出分隔線

// 使用
Console.WriteLine(Square(5));           // 輸出：25
Console.WriteLine(IsEven(4));           // 輸出：True
Console.WriteLine(GetGreeting(""小明"")); // 輸出：Hello, 小明!
```

## 📌 區域函式（Local Functions）

```csharp
// 區域函式：在方法裡面定義的方法
static int Factorial(int n) // 計算階乘（n!）
{
    // 驗證輸入
    if (n < 0) throw new ArgumentException(""n 不能是負數""); // 檢查參數

    // 區域函式：只有 Factorial 方法內部能使用
    int Calculate(int num) // 遞迴計算階乘
    {
        if (num <= 1) return 1;            // 基底條件：0! = 1! = 1
        return num * Calculate(num - 1);    // 遞迴：n! = n * (n-1)!
    }

    return Calculate(n);  // 呼叫區域函式
}

Console.WriteLine(Factorial(5));  // 5! = 5*4*3*2*1 = 120

// 區域函式的好處：
// 1. 把輔助邏輯封裝在使用它的方法裡
// 2. 可以存取外層方法的變數
// 3. 比 private 方法更能表達「這只是內部使用」
static void ProcessData(int[] data) // 處理資料的方法
{
    int threshold = 50;  // 門檻值

    // 區域函式可以存取外層的 threshold 變數
    bool IsAboveThreshold(int value) => value > threshold; // 判斷是否超過門檻

    foreach (var item in data) // 走訪每個資料
    {
        if (IsAboveThreshold(item))   // 使用區域函式判斷
        {
            Console.WriteLine($""{item} 超過門檻 {threshold}""); // 印出結果
        }
    }
}
```

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：忘記 return

```csharp
// ❌ 錯誤：宣告了回傳型別但沒有 return
static int GetMax(int a, int b)
{
    if (a > b)
    {
        return a;   // 有 return
    }
    // 如果 a <= b 呢？沒有 return！→ 編譯錯誤
}

// ✅ 正確：所有分支都要有 return
static int GetMaxFixed(int a, int b)
{
    if (a > b)
    {
        return a;  // a 較大時回傳 a
    }
    return b;      // 否則回傳 b
}
```

### ❌ 錯誤：params 不是放在最後

```csharp
// ❌ 錯誤：params 必須是最後一個參數
// static void Test(params int[] numbers, string name) { }

// ✅ 正確：params 放最後
static void Test(string name, params int[] numbers) // params 在最後
{
    Console.WriteLine($""{name}: {numbers.Length} 個數字"");
}
```

### ❌ 錯誤：out 參數沒有賦值

```csharp
// ❌ 錯誤：out 參數必須在方法內賦值
// static void GetValue(out int x)
// {
//     // 忘記給 x 賦值 → 編譯錯誤
// }

// ✅ 正確：一定要給 out 參數賦值
static void GetValue(out int x)
{
    x = 42;  // out 參數一定要賦值
}
```
"
            },

            // ── Chapter 5: 物件導向 OOP ──
            new Chapter
            {
                Id = 5,
                Title = "物件導向 OOP",
                Slug = "csharp-oop",
                Category = "csharp",
                Order = 5,
                Level = "intermediate",
                Icon = "🏗️",
                IsPublished = true,
                Content = @"# 🏗️ 物件導向程式設計（OOP）

## 📌 什麼是物件導向？

物件導向就像**用積木蓋房子**：
- **類別（Class）** = 積木的設計圖（藍圖）
- **物件（Object）** = 根據設計圖做出的實際積木
- 一張設計圖可以做出很多個積木

## 📌 類別與物件

```csharp
// 定義一個「學生」類別（設計圖）
class Student
{
    // 屬性（Properties）— 學生有什麼特徵
    public string Name { get; set; } = """";  // 姓名（自動屬性）
    public int Age { get; set; }              // 年齡
    public double GPA { get; set; }           // 成績

    // 建構子（Constructor）— 建立物件時自動執行
    public Student(string name, int age, double gpa)
    {
        Name = name;   // 設定姓名
        Age = age;     // 設定年齡
        GPA = gpa;     // 設定成績
    }

    // 無參數建構子
    public Student() { }  // 什麼都不做，使用預設值

    // 方法（Methods）— 學生能做什麼
    public void Introduce()  // 自我介紹的方法
    {
        Console.WriteLine($""我是 {Name}，{Age} 歲，GPA {GPA}"");
    }

    public bool IsPassing() => GPA >= 2.0;  // 是否及格
}

// 建立物件（根據設計圖做出實際的積木）
Student s1 = new Student(""小明"", 20, 3.5);  // 用建構子建立
s1.Introduce();  // 輸出：我是 小明，20 歲，GPA 3.5

Student s2 = new Student  // 用物件初始化語法建立
{
    Name = ""小美"",     // 設定姓名
    Age = 19,            // 設定年齡
    GPA = 3.8            // 設定成績
};
Console.WriteLine(s2.IsPassing());  // 輸出：True
```

## 📌 屬性（Properties）

```csharp
class Product
{
    // 自動屬性（最常用）
    public string Name { get; set; } = """";  // 可讀可寫
    public decimal Price { get; set; }        // 可讀可寫

    // 唯讀屬性（只有 get）
    public DateTime CreatedAt { get; } = DateTime.Now;  // 建立後不能改

    // 計算屬性（根據其他屬性動態計算）
    public decimal Tax => Price * 0.05m;  // 稅金 = 價格 * 5%
    public decimal Total => Price + Tax;  // 總價 = 價格 + 稅金

    // 有驗證邏輯的屬性
    private int _stock;  // 私有欄位（backing field）
    public int Stock     // 公開屬性
    {
        get => _stock;   // 取得庫存數量
        set              // 設定庫存數量（帶驗證）
        {
            if (value < 0)  // 庫存不能是負數
                throw new ArgumentException(""庫存不能為負數"");
            _stock = value; // 驗證通過才設定
        }
    }
}

var product = new Product { Name = ""筆電"", Price = 30000 }; // 建立商品
Console.WriteLine($""商品：{product.Name}"");        // 輸出：商品：筆電
Console.WriteLine($""稅金：{product.Tax}"");         // 輸出：稅金：1500
Console.WriteLine($""總價：{product.Total}"");       // 輸出：總價：31500
```

## 📌 繼承（Inheritance）

```csharp
// 基底類別（父類別）— 動物
class Animal
{
    public string Name { get; set; } = """";  // 動物名字
    public int Age { get; set; }              // 動物年齡

    // virtual 表示子類別可以覆寫這個方法
    public virtual void Speak()  // 說話（虛擬方法）
    {
        Console.WriteLine(""..."");  // 預設沒有聲音
    }

    public void Eat()  // 吃東西（一般方法）
    {
        Console.WriteLine($""{Name} 正在吃東西"");
    }
}

// 衍生類別（子類別）— 狗繼承動物
class Dog : Animal  // Dog 繼承 Animal（用 : 表示繼承）
{
    public string Breed { get; set; } = """";  // 品種（Dog 獨有的屬性）

    // override 覆寫父類別的 virtual 方法
    public override void Speak()  // 狗的叫聲
    {
        Console.WriteLine($""{Name} 說：汪汪！"");  // 狗會汪汪叫
    }

    public void Fetch()  // 撿球（Dog 獨有的方法）
    {
        Console.WriteLine($""{Name} 去撿球了！"");
    }
}

// 衍生類別 — 貓繼承動物
class Cat : Animal  // Cat 繼承 Animal
{
    public override void Speak()  // 貓的叫聲
    {
        Console.WriteLine($""{Name} 說：喵～"");  // 貓會喵喵叫
    }
}

// 使用
Dog dog = new Dog { Name = ""小白"", Age = 3, Breed = ""柴犬"" };
dog.Speak();  // 輸出：小白 說：汪汪！
dog.Eat();    // 輸出：小白 正在吃東西（繼承自 Animal）
dog.Fetch();  // 輸出：小白 去撿球了！
```

## 📌 抽象類別與介面

```csharp
// 抽象類別（Abstract Class）— 不能直接建立物件
abstract class Shape  // 形狀（抽象概念）
{
    public string Color { get; set; } = ""黑色"";  // 一般屬性

    // 抽象方法：只有宣告，沒有實作（子類別必須實作）
    public abstract double GetArea();  // 取得面積

    // 一般方法：有完整的實作
    public void PrintInfo()  // 印出資訊
    {
        Console.WriteLine($""顏色：{Color}，面積：{GetArea()}"");
    }
}

class Circle : Shape  // 圓形繼承形狀
{
    public double Radius { get; set; }  // 半徑

    // 必須實作抽象方法
    public override double GetArea() => Math.PI * Radius * Radius; // 圓面積公式
}

class Rectangle : Shape  // 矩形繼承形狀
{
    public double Width { get; set; }   // 寬
    public double Height { get; set; }  // 高

    public override double GetArea() => Width * Height;  // 矩形面積公式
}

// 介面（Interface）— 定義「能做什麼」的契約
interface IMovable  // 介面名稱慣例以 I 開頭
{
    void Move(int x, int y);  // 移動（只有宣告，沒有實作）
    double Speed { get; set; } // 速度屬性
}

interface IResizable  // 可調整大小的介面
{
    void Resize(double factor);  // 調整大小
}

// 一個類別可以實作多個介面（但只能繼承一個類別）
class GameCharacter : IMovable, IResizable  // 遊戲角色
{
    public string Name { get; set; } = """";   // 角色名稱
    public double Speed { get; set; } = 1.0;   // 移動速度

    public void Move(int x, int y)  // 實作 IMovable 的 Move
    {
        Console.WriteLine($""{Name} 移動到 ({x}, {y})"");
    }

    public void Resize(double factor)  // 實作 IResizable 的 Resize
    {
        Console.WriteLine($""{Name} 大小變為 {factor} 倍"");
    }
}
```

## 📌 封裝與存取修飾詞

```csharp
class BankAccount  // 銀行帳戶
{
    // private — 只有這個類別內部能存取
    private decimal _balance;  // 餘額（私有，外部不能直接碰）

    // public — 任何地方都能存取
    public string Owner { get; set; } = """";  // 帳戶持有人

    // protected — 這個類別和子類別能存取
    protected string AccountNumber { get; set; } = """";  // 帳號

    // internal — 同一個專案內能存取
    internal DateTime CreatedDate { get; set; } = DateTime.Now;

    // 建構子
    public BankAccount(string owner, decimal initialBalance)
    {
        Owner = owner;           // 設定持有人
        _balance = initialBalance; // 設定初始餘額
    }

    // 公開方法控制對私有資料的存取
    public decimal GetBalance() => _balance;  // 查詢餘額

    public void Deposit(decimal amount)  // 存款
    {
        if (amount <= 0) // 驗證金額
            throw new ArgumentException(""存款金額必須大於 0"");
        _balance += amount; // 增加餘額
        Console.WriteLine($""存入 {amount}，餘額 {_balance}"");
    }

    public void Withdraw(decimal amount)  // 提款
    {
        if (amount > _balance) // 檢查餘額是否足夠
            throw new InvalidOperationException(""餘額不足"");
        _balance -= amount; // 扣除餘額
        Console.WriteLine($""提領 {amount}，餘額 {_balance}"");
    }
}

var account = new BankAccount(""小明"", 1000);  // 建立帳戶
account.Deposit(500);    // 存入 500 → 餘額 1500
account.Withdraw(200);   // 提領 200 → 餘額 1300
// account._balance = 0; // ❌ 編譯錯誤！_balance 是 private
```

## 📌 多型（Polymorphism）

```csharp
// 多型：同一個方法呼叫，不同物件有不同行為
List<Animal> animals = new List<Animal>  // 建立動物清單
{
    new Dog { Name = ""小白"" },  // 加入一隻狗
    new Cat { Name = ""咪咪"" },  // 加入一隻貓
    new Dog { Name = ""大黃"" },  // 再加入一隻狗
};

foreach (Animal animal in animals) // 走訪每隻動物
{
    animal.Speak(); // 同樣是 Speak()，但每隻動物的叫聲不同！
}
// 輸出：
// 小白 說：汪汪！
// 咪咪 說：喵～
// 大黃 說：汪汪！

// 多型讓你用「基底類別」的變數來操作「衍生類別」的物件
Animal myPet = new Dog { Name = ""旺財"" };  // Animal 變數裝 Dog 物件
myPet.Speak();  // 輸出：旺財 說：汪汪！（執行的是 Dog 的版本）

// is 和 as 運算子
if (myPet is Dog dogRef)  // 檢查 myPet 是否是 Dog，並轉型
{
    dogRef.Fetch();  // 可以使用 Dog 獨有的方法
}
```

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：忘記 override

```csharp
class MyDog : Animal
{
    // ❌ 用 new 只是隱藏父類別的方法，不是覆寫
    // 多型時不會正確呼叫
    public new void Speak() { Console.WriteLine(""汪！""); }
}

// 當用 Animal 型別的變數時：
Animal a = new MyDog { Name = ""test"" };
a.Speak(); // 輸出：...（呼叫的是 Animal 的版本，不是 MyDog 的！）

// ✅ 正確：用 override 覆寫 virtual 方法
class MyDogFixed : Animal
{
    public override void Speak() { Console.WriteLine(""汪！""); }
}
```

### ❌ 錯誤：想建立抽象類別的實例

```csharp
// ❌ 錯誤：抽象類別不能直接建立物件
// Shape shape = new Shape(); // 編譯錯誤！

// ✅ 正確：建立具體子類別的物件
Shape shape = new Circle { Radius = 5 };  // 用子類別
```
"
            },

            // ── Chapter 6: LINQ 查詢語言 ──
            new Chapter
            {
                Id = 6,
                Title = "LINQ 查詢語言",
                Slug = "csharp-linq",
                Category = "csharp",
                Order = 6,
                Level = "intermediate",
                Icon = "🔍",
                IsPublished = true,
                Content = @"# 🔍 LINQ 查詢語言

## 📌 什麼是 LINQ？

LINQ（Language Integrated Query）就像是 **C# 裡的 SQL**。
它讓你可以用簡潔的語法對集合（陣列、List 等）進行查詢、篩選、排序。

```csharp
// 想像你有一堆學生成績，要找出及格的人
int[] scores = { 85, 42, 97, 63, 28, 74, 91, 55 }; // 成績陣列

// 不用 LINQ 的寫法（傳統迴圈）
List<int> passingOld = new List<int>();  // 建立新 List
foreach (int score in scores)            // 走訪每個成績
{
    if (score >= 60)                     // 如果及格
    {
        passingOld.Add(score);           // 加入 List
    }
}

// 用 LINQ 的寫法（一行搞定！）
var passing = scores.Where(s => s >= 60).ToList(); // 篩選出 >= 60 的成績
// passing = [85, 97, 63, 74, 91, 55]
```

## 📌 方法語法 vs 查詢語法

```csharp
var students = new List<(string Name, int Score)>  // 學生清單（用 Tuple）
{
    (""小明"", 85),   // 小明 85 分
    (""小美"", 92),   // 小美 92 分
    (""小華"", 68),   // 小華 68 分
    (""小強"", 45),   // 小強 45 分
    (""小芳"", 78),   // 小芳 78 分
};

// 方法語法（Method Syntax）— 最常用
var topStudents = students
    .Where(s => s.Score >= 70)          // 篩選 70 分以上
    .OrderByDescending(s => s.Score)    // 依分數降冪排序
    .Select(s => s.Name)               // 只取名字
    .ToList();                          // 轉成 List
// topStudents = [""小美"", ""小明"", ""小芳""]

// 查詢語法（Query Syntax）— 像 SQL
var topStudents2 =
    (from s in students                 // 從 students 中
     where s.Score >= 70                // 篩選 70 分以上
     orderby s.Score descending         // 依分數降冪排序
     select s.Name)                     // 只取名字
    .ToList();                          // 轉成 List
// 結果和方法語法一樣
```

## 📌 Where — 篩選

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; // 1 到 10

// 篩選偶數
var evens = numbers.Where(n => n % 2 == 0);  // n 除以 2 餘 0 就是偶數
// evens = [2, 4, 6, 8, 10]

// 篩選大於 5 的數
var big = numbers.Where(n => n > 5);  // n 大於 5
// big = [6, 7, 8, 9, 10]

// 多條件篩選
var filtered = numbers.Where(n => n > 3 && n < 8);  // 3 < n < 8
// filtered = [4, 5, 6, 7]
```

## 📌 Select — 轉換

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };  // 原始數字

// 每個數字乘以 2
var doubled = numbers.Select(n => n * 2);  // 轉換每個元素
// doubled = [2, 4, 6, 8, 10]

// 轉換成字串
var strings = numbers.Select(n => $""數字 {n}"");  // 轉換成描述文字
// strings = [""數字 1"", ""數字 2"", ...]

// 轉換成匿名物件
var objects = numbers.Select(n => new  // 轉換成物件
{
    Value = n,              // 原始值
    Square = n * n,         // 平方
    IsEven = n % 2 == 0     // 是否為偶數
});

foreach (var obj in objects)  // 走訪每個物件
{
    Console.WriteLine($""{obj.Value}: 平方={obj.Square}, 偶數={obj.IsEven}"");
}
```

## 📌 OrderBy / GroupBy / Join

```csharp
var products = new List<(string Name, string Category, decimal Price)>
{
    (""蘋果"", ""水果"", 30),     // 水果類
    (""牛奶"", ""飲料"", 45),     // 飲料類
    (""香蕉"", ""水果"", 20),     // 水果類
    (""咖啡"", ""飲料"", 60),     // 飲料類
    (""橘子"", ""水果"", 25),     // 水果類
};

// OrderBy — 排序
var sorted = products.OrderBy(p => p.Price);  // 依價格升冪排序
var sortedDesc = products.OrderByDescending(p => p.Price);  // 降冪排序

// 多欄位排序
var multiSort = products
    .OrderBy(p => p.Category)           // 先依類別排序
    .ThenByDescending(p => p.Price);    // 同類別再依價格降冪

// GroupBy — 分組
var groups = products.GroupBy(p => p.Category);  // 依類別分組

foreach (var group in groups)  // 走訪每個分組
{
    Console.WriteLine($""== {group.Key} =="");  // Key 是分組的依據
    foreach (var item in group)  // 走訪分組內的每個項目
    {
        Console.WriteLine($""  {item.Name}: {item.Price} 元"");
    }
}
// 輸出：
// == 水果 ==
//   蘋果: 30 元
//   香蕉: 20 元
//   橘子: 25 元
// == 飲料 ==
//   牛奶: 45 元
//   咖啡: 60 元

// Join — 合併兩個集合
var categories = new[] { (""水果"", ""🍎""), (""飲料"", ""🥤"") };  // 類別與圖示

var joined = products.Join(
    categories,                     // 要合併的集合
    p => p.Category,                // 左邊的 key
    c => c.Item1,                   // 右邊的 key
    (p, c) => $""{c.Item2} {p.Name} - {p.Price}元""  // 合併後的結果
);
// joined = [""🍎 蘋果 - 30元"", ""🥤 牛奶 - 45元"", ...]
```

## 📌 First / Any / Count 等查詢方法

```csharp
int[] numbers = { 5, 3, 8, 1, 9, 2, 7 };  // 數字陣列

// First / FirstOrDefault — 取第一個
int first = numbers.First();                    // 取第一個元素 → 5
int firstBig = numbers.First(n => n > 6);       // 第一個 > 6 的 → 8
int firstOrDef = numbers.FirstOrDefault(n => n > 100);  // 找不到 → 回傳 0

// Single — 取唯一一個（如果有多個會拋出例外）
// int single = numbers.Single(n => n > 8);    // 只有 9 符合 → 9
// int singleFail = numbers.Single(n => n > 5); // 多個符合 → 拋出例外！

// Any / All — 是否存在 / 是否全部符合
bool hasNegative = numbers.Any(n => n < 0);     // 有負數嗎？→ false
bool allPositive = numbers.All(n => n > 0);     // 全部是正數？→ true

// Count — 計數
int total = numbers.Count();                     // 總共幾個 → 7
int bigCount = numbers.Count(n => n > 5);        // > 5 的有幾個 → 3
```

## 📌 聚合方法

```csharp
int[] scores = { 85, 92, 68, 45, 78 };  // 成績陣列

// 基本聚合
int sum = scores.Sum();                  // 總和 → 368
double avg = scores.Average();           // 平均 → 73.6
int max = scores.Max();                  // 最大值 → 92
int min = scores.Min();                  // 最小值 → 45

// Aggregate — 自定義聚合（進階）
// 把所有名字用逗號串起來
string[] names = { ""小明"", ""小美"", ""小華"" };
string joined = names.Aggregate((a, b) => $""{a}, {b}"");
// joined = ""小明, 小美, 小華""

// 用 Aggregate 計算階乘
int factorial = Enumerable.Range(1, 5)  // 產生 1, 2, 3, 4, 5
    .Aggregate((a, b) => a * b);        // 1*2*3*4*5 = 120
Console.WriteLine($""5! = {factorial}""); // 輸出：5! = 120
```

## 📌 鏈式操作

```csharp
// LINQ 最強大的地方：把多個操作串在一起
var result = Enumerable.Range(1, 100)   // 產生 1 到 100
    .Where(n => n % 3 == 0)             // 篩選 3 的倍數
    .Select(n => new { Value = n, Square = n * n }) // 計算平方
    .OrderByDescending(x => x.Square)   // 依平方降冪排序
    .Take(5)                            // 只取前 5 個
    .ToList();                          // 轉成 List

foreach (var item in result)  // 走訪結果
{
    Console.WriteLine($""{item.Value} 的平方 = {item.Square}"");
}
// 輸出（3 的倍數中平方最大的前 5 個）：
// 99 的平方 = 9801
// 96 的平方 = 9216
// 93 的平方 = 8649
// 90 的平方 = 8100
// 87 的平方 = 7569
```

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：忘記 LINQ 是延遲執行

```csharp
var numbers = new List<int> { 1, 2, 3 };
var query = numbers.Where(n => n > 1);  // 這裡還沒真正執行查詢！

numbers.Add(4);  // 加入新元素

// ❌ 以為 query 只有 [2, 3]
// ✅ 實際上 query = [2, 3, 4]（因為 LINQ 是在 foreach 時才執行）
foreach (var n in query) Console.Write($""{n} "");
// 輸出：2 3 4

// 解法：如果要立即執行，加上 .ToList()
var list = numbers.Where(n => n > 1).ToList();  // 立即執行並存結果
numbers.Add(5);  // 這次不會影響 list
```

### ❌ 錯誤：First() 找不到元素

```csharp
int[] nums = { 1, 2, 3 };

// ❌ 錯誤：First() 找不到符合條件的元素會拋出 InvalidOperationException
// int x = nums.First(n => n > 100);  // 拋出例外！

// ✅ 正確：用 FirstOrDefault()
int x = nums.FirstOrDefault(n => n > 100);  // 找不到回傳 0
```
"
            },

            // ── Chapter 7: Async / Await 非同步 ──
            new Chapter
            {
                Id = 7,
                Title = "Async / Await 非同步",
                Slug = "csharp-async",
                Category = "csharp",
                Order = 7,
                Level = "intermediate",
                Icon = "⚡",
                IsPublished = true,
                Content = @"# ⚡ Async / Await 非同步程式設計

## 📌 為什麼需要非同步？

想像你是一個**餐廳服務生**：

**同步（Synchronous）**：
你幫客人 A 點餐 → 站在廚房門口等菜做好 → 上菜 → 然後才去服務客人 B。
（效率很差！等菜的時間完全浪費了）

**非同步（Asynchronous）**：
你幫客人 A 點餐 → 把單子送進廚房 → **不等了，先去服務客人 B** →
廚房做好了再回來上菜。
（效率高！等待的時間可以做別的事）

```csharp
// 同步版本 — 一個一個等
void SyncExample()  // 同步方法
{
    Console.WriteLine(""開始下載檔案 A..."");
    Thread.Sleep(3000);  // 模擬等待 3 秒（什麼事都不能做！）
    Console.WriteLine(""檔案 A 下載完成"");

    Console.WriteLine(""開始下載檔案 B..."");
    Thread.Sleep(3000);  // 再等 3 秒
    Console.WriteLine(""檔案 B 下載完成"");
    // 總共花了 6 秒
}

// 非同步版本 — 同時進行
async Task AsyncExample()  // async 標記這是非同步方法
{
    Console.WriteLine(""開始下載檔案 A..."");
    Task taskA = Task.Delay(3000);  // 開始等待但不阻塞

    Console.WriteLine(""開始下載檔案 B..."");
    Task taskB = Task.Delay(3000);  // 同時開始等待

    await Task.WhenAll(taskA, taskB);  // 等兩個都完成
    Console.WriteLine(""兩個檔案都下載完成！"");
    // 總共只花了 3 秒（同時進行！）
}
```

## 📌 async / await 關鍵字

```csharp
// async 放在方法宣告前面，表示這是一個非同步方法
// await 放在非同步操作前面，表示「等這個做完再繼續」

// 非同步方法的回傳型別
async Task DoSomethingAsync()           // 不回傳值用 Task
{
    await Task.Delay(1000);             // 等待 1 秒（非阻塞）
    Console.WriteLine(""做完了！"");      // 1 秒後執行
}

async Task<int> GetNumberAsync()         // 回傳 int 用 Task<int>
{
    await Task.Delay(500);               // 等待 0.5 秒
    return 42;                           // 回傳結果
}

async Task<string> GetGreetingAsync(string name) // 回傳 string 用 Task<string>
{
    await Task.Delay(100);               // 模擬非同步操作
    return $""你好，{name}！"";           // 回傳問候語
}

// 呼叫非同步方法
async Task Main()  // Main 方法也可以是 async
{
    await DoSomethingAsync();            // 等待完成
    int number = await GetNumberAsync(); // 等待並取得結果
    Console.WriteLine(number);           // 輸出：42

    string greeting = await GetGreetingAsync(""小明""); // 等待並取得結果
    Console.WriteLine(greeting);         // 輸出：你好，小明！
}
```

## 📌 Task 與 Task<T>

```csharp
// Task 代表一個「正在進行的工作」
// 就像你在餐廳點了餐，拿到一張號碼牌（Task）
// 號碼牌本身不是食物，但你可以用它來等食物做好

// 建立 Task 的方式
Task task1 = Task.Run(() =>              // 用 Task.Run 在背景執行
{
    Console.WriteLine(""背景工作開始"");   // 在背景執行緒上跑
    Thread.Sleep(1000);                   // 模擬耗時操作
    Console.WriteLine(""背景工作完成"");
});

Task<int> task2 = Task.Run(() =>         // 有回傳值的 Task
{
    int sum = 0;                          // 計算總和
    for (int i = 0; i < 1000000; i++)     // 大量計算
    {
        sum += i;                         // 累加
    }
    return sum;                           // 回傳結果
});

await task1;                              // 等待 task1 完成
int result = await task2;                 // 等待 task2 完成並取得結果
Console.WriteLine($""計算結果：{result}""); // 印出結果
```

## 📌 Task.WhenAll 與 Task.WhenAny

```csharp
// Task.WhenAll — 等所有工作都完成
async Task DownloadAllAsync()  // 同時下載多個檔案
{
    Task<string> file1 = DownloadFileAsync(""file1.txt""); // 開始下載 1
    Task<string> file2 = DownloadFileAsync(""file2.txt""); // 開始下載 2
    Task<string> file3 = DownloadFileAsync(""file3.txt""); // 開始下載 3

    // WhenAll 等所有 Task 都完成，回傳所有結果
    string[] results = await Task.WhenAll(file1, file2, file3);

    foreach (string r in results)     // 走訪所有結果
    {
        Console.WriteLine(r);         // 印出每個下載結果
    }
}

// Task.WhenAny — 等任何一個工作完成就好
async Task GetFastestAsync()  // 取得最快的回應
{
    Task<string> server1 = FetchFromServerAsync(""https://server1.com"");
    Task<string> server2 = FetchFromServerAsync(""https://server2.com"");

    // WhenAny 只要有一個完成就回傳
    Task<string> fastest = await Task.WhenAny(server1, server2);
    string result = await fastest;        // 取得最快完成的結果
    Console.WriteLine($""最快回應：{result}""); // 使用最快的結果
}

// 模擬下載檔案
async Task<string> DownloadFileAsync(string fileName)
{
    var random = new Random();
    int delay = random.Next(500, 2000);       // 隨機延遲 0.5-2 秒
    await Task.Delay(delay);                   // 模擬下載時間
    return $""{fileName} 下載完成（耗時 {delay}ms）""; // 回傳結果
}

// 模擬從伺服器取得資料
async Task<string> FetchFromServerAsync(string url)
{
    var random = new Random();
    await Task.Delay(random.Next(100, 1000));  // 模擬網路延遲
    return $""來自 {url} 的回應"";              // 回傳回應
}
```

## 📌 HttpClient 非同步範例

```csharp
// HttpClient 是 .NET 內建的 HTTP 客戶端
// 所有 HTTP 操作都是非同步的

async Task FetchWebPageAsync()  // 抓取網頁內容
{
    // 建議用 static 或 DI 管理 HttpClient（不要每次都 new）
    using HttpClient client = new HttpClient(); // 建立 HTTP 客戶端

    try // 網路操作可能失敗，要用 try-catch
    {
        // GetStringAsync 是非同步方法
        string content = await client.GetStringAsync(""https://api.github.com"");
        Console.WriteLine($""取得 {content.Length} 個字元""); // 印出內容長度

        // 也可以取得完整的 HttpResponseMessage
        HttpResponseMessage response = await client.GetAsync(""https://api.github.com"");
        if (response.IsSuccessStatusCode) // 檢查是否成功（200-299）
        {
            string body = await response.Content.ReadAsStringAsync(); // 讀取內容
            Console.WriteLine(body); // 印出回應內容
        }
        else
        {
            Console.WriteLine($""請求失敗：{response.StatusCode}""); // 印出錯誤狀態碼
        }
    }
    catch (HttpRequestException ex) // 捕捉 HTTP 相關的例外
    {
        Console.WriteLine($""網路錯誤：{ex.Message}""); // 印出錯誤訊息
    }
}

// POST 請求範例
async Task PostDataAsync()  // 送出資料
{
    using HttpClient client = new HttpClient();  // 建立客戶端

    var data = new { name = ""小明"", age = 20 };  // 要送出的資料
    string json = System.Text.Json.JsonSerializer.Serialize(data); // 轉成 JSON
    var content = new StringContent(json, System.Text.Encoding.UTF8, ""application/json"");

    HttpResponseMessage response = await client.PostAsync(""https://api.example.com/users"", content);
    string result = await response.Content.ReadAsStringAsync(); // 讀取回應
    Console.WriteLine(result); // 印出結果
}
```

## 📌 ConfigureAwait(false)

```csharp
// ConfigureAwait 控制 await 之後要在哪個執行緒繼續

// 預設：await 之後回到原來的執行緒（UI 執行緒）
async Task UpdateUIAsync()  // UI 應用程式中使用
{
    string data = await GetDataAsync();  // 預設會回到 UI 執行緒
    // label.Text = data;                // 可以安全地更新 UI
}

// ConfigureAwait(false)：不需要回到原來的執行緒
async Task<string> GetDataFromApiAsync()  // 程式庫 / 非 UI 程式碼
{
    using HttpClient client = new HttpClient();
    // ConfigureAwait(false) 告訴系統：我不需要回到原來的執行緒
    // 這在程式庫中可以避免死鎖，也能提升效能
    string result = await client.GetStringAsync(""https://api.example.com"")
        .ConfigureAwait(false);  // 不需要回到原來的 context
    return result; // 處理結果
}

// 何時用 ConfigureAwait(false)：
// ✅ 在程式庫（Library）中 — 幾乎都要用
// ✅ 在非 UI 的應用程式中
// ❌ 在 UI 事件處理器中不要用（否則不能更新 UI）
// ❌ 在 ASP.NET Core 中通常不需要（已經沒有 SynchronizationContext）
```

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：async void（最常見的錯誤！）

```csharp
// ❌ 錯誤：不要用 async void（除了事件處理器）
async void BadMethod()  // async void 無法被 await
{
    await Task.Delay(1000);
    throw new Exception(""這個例外無法被捕捉！""); // 會直接崩潰！
}

// ✅ 正確：用 async Task
async Task GoodMethod()  // async Task 可以被 await
{
    await Task.Delay(1000);
    throw new Exception(""這個例外可以被 try-catch 捕捉"");
}

// 唯一可以用 async void 的地方：UI 事件處理器
// async void Button_Click(object sender, EventArgs e) { ... }
```

### ❌ 錯誤：死鎖（Deadlock）

```csharp
// ❌ 錯誤：在同步方法中用 .Result 或 .Wait() 等待非同步方法
void DeadlockExample()  // 同步方法
{
    // 在 UI 或 ASP.NET (非 Core) 中，這會造成死鎖！
    // var result = GetDataAsync().Result;  // ❌ 死鎖！
    // GetDataAsync().Wait();               // ❌ 死鎖！
}

// ✅ 正確：一路 async/await 到底（""async all the way""）
async Task CorrectExample()  // 非同步方法
{
    var result = await GetDataAsync();  // ✅ 用 await
    Console.WriteLine(result);
}

// 如果真的必須在同步中呼叫非同步方法：
void WorkaroundExample()
{
    // 用 Task.Run 包裝（避免在原來的 context 上等待）
    var result = Task.Run(() => GetDataAsync()).Result;
}
```

### ❌ 錯誤：忘記 await

```csharp
// ❌ 錯誤：忘記 await，方法會立刻回傳，不等結果
async Task ForgotAwait()
{
    // Task.Delay(5000);  // ❌ 沒有 await，不會等 5 秒
    // 上面的 Task.Delay 被忽略了，程式直接往下跑

    await Task.Delay(5000);  // ✅ 有 await，會等 5 秒
}
```

### ❌ 錯誤：在迴圈中逐一 await

```csharp
// ❌ 效率差：一個一個等
async Task SlowVersion()
{
    var urls = new[] { ""url1"", ""url2"", ""url3"" };
    foreach (var url in urls)
    {
        await FetchFromServerAsync(url);  // 等完一個才做下一個
    }
    // 如果每個要 1 秒，總共要 3 秒
}

// ✅ 效率好：同時進行
async Task FastVersion()
{
    var urls = new[] { ""url1"", ""url2"", ""url3"" };
    var tasks = urls.Select(url => FetchFromServerAsync(url)); // 同時開始
    await Task.WhenAll(tasks);  // 等所有完成
    // 如果每個要 1 秒，總共只要 1 秒（同時進行）
}
```
"
            },
        };
    }
}
