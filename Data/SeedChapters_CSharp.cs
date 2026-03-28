using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_CSharp
{
    public static List<Chapter> GetChapters()
    {
        return new List<Chapter>
        {
            // ── Chapter 8: 陣列、集合與泛型 ──
            new Chapter
            {
                Id = 8,
                Title = "陣列、集合與泛型",
                Slug = "csharp-arrays-collections-generics",
                Category = "csharp",
                Order = 8,
                Level = "intermediate",
                Icon = "📚",
                IsPublished = true,
                Content = @"# 📚 陣列、集合與泛型

## 📌 什麼是陣列？

陣列就像一排**置物櫃**，每個櫃子都有編號（索引），你可以透過編號快速找到裡面的東西。

### 一維陣列

```csharp
// 宣告一個可以放 5 個整數的陣列（就像準備 5 個櫃子）
int[] numbers = new int[5];

// 把數字放進第一個櫃子（索引從 0 開始）
numbers[0] = 10;

// 把數字放進第二個櫃子
numbers[1] = 20;

// 也可以在宣告時直接放入資料
int[] scores = { 90, 85, 78, 92, 88 };

// 用 for 迴圈走訪每個櫃子
for (int i = 0; i < scores.Length; i++) // Length 回傳陣列的長度
{
    // 印出每個櫃子裡的分數
    Console.WriteLine($""第 {i} 個分數：{scores[i]}"");
}
```

### 二維陣列

```csharp
// 宣告一個 3x3 的二維陣列（像一張表格，3 列 3 行）
int[,] matrix = new int[3, 3];

// 在第 0 列、第 1 行放入數字 5
matrix[0, 1] = 5;

// 也可以直接初始化整張表格
int[,] grid = {
    { 1, 2, 3 },  // 第 0 列的三個值
    { 4, 5, 6 },  // 第 1 列的三個值
    { 7, 8, 9 }   // 第 2 列的三個值
};

// 用雙層迴圈走訪整張表格
for (int row = 0; row < grid.GetLength(0); row++) // GetLength(0) 取得列數
{
    for (int col = 0; col < grid.GetLength(1); col++) // GetLength(1) 取得行數
    {
        // 印出每個格子的值，用 tab 分隔
        Console.Write($""{grid[row, col]}\t"");
    }
    // 每一列印完後換行
    Console.WriteLine();
}
```

### 不規則陣列（Jagged Array）

```csharp
// 不規則陣列像是每一列長度可以不同的表格（像階梯）
int[][] jagged = new int[3][]; // 宣告有 3 列

// 第 0 列有 2 個元素
jagged[0] = new int[] { 1, 2 };

// 第 1 列有 4 個元素
jagged[1] = new int[] { 3, 4, 5, 6 };

// 第 2 列有 1 個元素
jagged[2] = new int[] { 7 };

// 走訪不規則陣列
for (int i = 0; i < jagged.Length; i++) // 外層走每一列
{
    for (int j = 0; j < jagged[i].Length; j++) // 內層走該列的每個元素
    {
        // 印出每個元素
        Console.Write($""{jagged[i][j]} "");
    }
    // 每一列印完換行
    Console.WriteLine();
}
```

---

## 📌 常用集合類別

### List&lt;T&gt; — 動態陣列

```csharp
// List 就像一個可以自動伸縮的書架
List<string> fruits = new List<string>(); // 建立一個空書架

// 加入水果（往書架放書）
fruits.Add(""蘋果""); // 加入第一個元素
fruits.Add(""香蕉""); // 加入第二個元素
fruits.Add(""橘子""); // 加入第三個元素

// 移除指定的水果
fruits.Remove(""香蕉""); // 把香蕉從書架拿走

// 檢查是否包含某水果
bool hasApple = fruits.Contains(""蘋果""); // 回傳 true

// 取得元素數量
int count = fruits.Count; // 回傳 2（因為香蕉已移除）

// 用 foreach 走訪所有水果
foreach (string fruit in fruits) // 逐一取出每個水果
{
    // 印出水果名稱
    Console.WriteLine(fruit);
}
```

### Dictionary&lt;TKey, TValue&gt; — 字典

```csharp
// Dictionary 就像一本電話簿，用名字（Key）查電話（Value）
Dictionary<string, int> phoneBook = new Dictionary<string, int>();

// 加入聯絡人和電話號碼
phoneBook[""小明""] = 12345678; // 小明的電話
phoneBook[""小華""] = 87654321; // 小華的電話

// 用名字查電話
if (phoneBook.ContainsKey(""小明"")) // 先確認電話簿裡有沒有小明
{
    // 印出小明的電話號碼
    Console.WriteLine($""小明的電話：{phoneBook[""小明""]}"");
}

// 安全地取值（推薦用法）
if (phoneBook.TryGetValue(""小華"", out int phone)) // 嘗試取值，成功則放入 phone
{
    // 取值成功，印出電話
    Console.WriteLine($""小華的電話：{phone}"");
}

// 走訪所有鍵值對
foreach (var pair in phoneBook) // pair 包含 Key 和 Value
{
    // 印出每個人的名字和電話
    Console.WriteLine($""{pair.Key}: {pair.Value}"");
}
```

### HashSet&lt;T&gt;、Queue&lt;T&gt;、Stack&lt;T&gt;

```csharp
// HashSet 像是一個「不允許重複」的集合（像一組不重複的印章）
HashSet<int> uniqueNumbers = new HashSet<int>();
uniqueNumbers.Add(1);  // 加入 1，成功
uniqueNumbers.Add(2);  // 加入 2，成功
uniqueNumbers.Add(1);  // 再加入 1，會被忽略（已存在）

// Queue 是先進先出（像排隊買票，先到先服務）
Queue<string> line = new Queue<string>();
line.Enqueue(""第一位客人""); // 排入隊伍
line.Enqueue(""第二位客人""); // 排入隊伍
string first = line.Dequeue(); // 取出最前面的人（第一位客人）

// Stack 是後進先出（像疊盤子，最上面的先拿）
Stack<string> plates = new Stack<string>();
plates.Push(""盤子A""); // 放入最底層
plates.Push(""盤子B""); // 放在 A 上面
string top = plates.Pop(); // 拿走最上面的（盤子B）
```

---

## 📌 泛型（Generics）

泛型就像是一個「萬用模具」，你可以決定要用它來做什麼形狀的餅乾。

### 泛型方法

```csharp
// T 是型別參數，呼叫時才決定實際型別
static T GetMax<T>(T a, T b) where T : IComparable<T> // 約束 T 必須可比較
{
    // 如果 a 大於 b，回傳 a；否則回傳 b
    return a.CompareTo(b) > 0 ? a : b;
}

// 使用泛型方法
int maxInt = GetMax(10, 20);         // T 被推斷為 int，回傳 20
string maxStr = GetMax(""abc"", ""xyz""); // T 被推斷為 string，回傳 ""xyz""
```

### 泛型類別

```csharp
// 泛型類別：一個可以裝任何型別的箱子
public class Box<T> // T 代表箱子裡要裝什麼型別的東西
{
    // 箱子裡的物品
    public T Item { get; set; }

    // 建構函式，建立箱子時放入物品
    public Box(T item)
    {
        Item = item; // 把物品放進箱子
    }

    // 顯示箱子裡的物品
    public void ShowItem()
    {
        // 印出物品的內容
        Console.WriteLine($""箱子裡有：{Item}"");
    }
}

// 建立一個裝 int 的箱子
Box<int> intBox = new Box<int>(42);
// 建立一個裝 string 的箱子
Box<string> strBox = new Box<string>(""Hello"");
```

### 泛型約束

```csharp
// where T : class → T 必須是參考型別（類別）
public class Repository<T> where T : class
{
    private List<T> _items = new List<T>(); // 儲存資料的清單

    public void Add(T item) // 加入一筆資料
    {
        _items.Add(item); // 把資料加入清單
    }
}

// where T : struct → T 必須是值型別（int, bool 等）
// where T : new() → T 必須有無參數建構函式
// where T : IComparable → T 必須實作 IComparable 介面
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：索引超出範圍

```csharp
// ❌ 錯誤寫法
int[] arr = { 1, 2, 3 }; // 陣列只有索引 0, 1, 2
Console.WriteLine(arr[3]); // 💥 IndexOutOfRangeException！索引 3 不存在
```

```csharp
// ✅ 正確寫法
int[] arr = { 1, 2, 3 }; // 陣列只有索引 0, 1, 2
if (arr.Length > 3) // 先檢查長度是否足夠
{
    Console.WriteLine(arr[3]); // 安全存取
}
// 記住：陣列索引從 0 開始，最大索引是 Length - 1
```

**解釋：** 陣列的索引從 0 開始，長度為 3 的陣列最大索引是 2。存取索引 3 就像去找第 4 個櫃子，但你只有 3 個。

### ❌ 錯誤 2：在 foreach 中修改集合

```csharp
// ❌ 錯誤寫法
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
foreach (int n in numbers) // 正在走訪集合
{
    if (n % 2 == 0) // 如果是偶數
    {
        numbers.Remove(n); // 💥 InvalidOperationException！不能在走訪時修改
    }
}
```

```csharp
// ✅ 正確寫法：用 RemoveAll 或反向 for 迴圈
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
numbers.RemoveAll(n => n % 2 == 0); // 一次移除所有偶數，安全又簡潔
```

**解釋：** foreach 走訪時，集合的結構不能被改變。就像你在數書架上的書時，別人不能同時抽走其中一本。

### ❌ 錯誤 3：泛型約束錯誤

```csharp
// ❌ 錯誤寫法
public class Cache<T> where T : struct // 約束 T 為值型別
{
    public T Data { get; set; }
}

Cache<string> cache = new Cache<string>(); // 💥 編譯錯誤！string 是參考型別，不符合 struct 約束
```

```csharp
// ✅ 正確寫法：根據需求選擇正確的約束
public class Cache<T> where T : class // 改為 class 約束，允許參考型別
{
    public T Data { get; set; } // 現在可以用 string 了
}

Cache<string> cache = new Cache<string>(); // ✅ 編譯通過
```

**解釋：** `struct` 約束只允許值型別（int, bool, struct 等），而 `string` 是參考型別。選錯約束就像把方形積木塞進圓形洞裡。
"
            },

            // ── Chapter 9: 例外處理與除錯 ──
            new Chapter
            {
                Id = 9,
                Title = "例外處理與除錯",
                Slug = "csharp-exception-handling-debugging",
                Category = "csharp",
                Order = 9,
                Level = "intermediate",
                Icon = "⚠️",
                IsPublished = true,
                Content = @"# ⚠️ 例外處理與除錯

## 📌 什麼是例外（Exception）？

例外就像開車時遇到的「路障」。如果你沒有準備好應對方案，程式就會直接撞上去然後崩潰。**例外處理**就是幫你設計繞路方案。

---

## 📌 try / catch / finally

```csharp
try // 嘗試執行可能出錯的程式碼（像是試著過馬路）
{
    // 嘗試把字串轉成數字
    string input = ""abc"";
    int number = int.Parse(input); // 💥 這行會出錯，因為 ""abc"" 不是數字
    Console.WriteLine(number); // 這行不會執行
}
catch (FormatException ex) // 捕捉「格式錯誤」的例外
{
    // 印出錯誤訊息，告訴使用者輸入格式不對
    Console.WriteLine($""格式錯誤：{ex.Message}"");
}
catch (OverflowException ex) // 捕捉「數字溢位」的例外
{
    // 數字太大或太小時會觸發
    Console.WriteLine($""數字超出範圍：{ex.Message}"");
}
finally // 不管有沒有出錯，這裡的程式碼都會執行
{
    // 通常用來釋放資源（關閉檔案、資料庫連線等）
    Console.WriteLine(""不管成功失敗，我都會執行"");
}
```

---

## 📌 多重 catch 與 when 子句

```csharp
try // 嘗試執行程式碼
{
    // 模擬根據錯誤碼拋出不同例外
    int errorCode = 404;
    throw new HttpRequestException($""HTTP 錯誤：{errorCode}""); // 手動拋出例外
}
catch (HttpRequestException ex) when (ex.Message.Contains(""404"")) // 只捕捉包含 404 的例外
{
    // 處理找不到頁面的情況
    Console.WriteLine(""頁面不存在（404）"");
}
catch (HttpRequestException ex) when (ex.Message.Contains(""500"")) // 只捕捉包含 500 的例外
{
    // 處理伺服器錯誤的情況
    Console.WriteLine(""伺服器錯誤（500）"");
}
catch (Exception ex) // 捕捉所有其他例外（最通用的放最後）
{
    // 處理未預期的錯誤
    Console.WriteLine($""未預期的錯誤：{ex.Message}"");
}
```

**重點：** `when` 子句讓你可以對同一種例外類型做更細緻的分類處理，就像醫院的分級診療。

---

## 📌 Exception 階層架構

```csharp
// Exception 的繼承關係（像家族樹）
// Exception                         ← 所有例外的祖先
//   ├── SystemException             ← 系統層級例外
//   │     ├── NullReferenceException  ← 物件為 null 時存取其成員
//   │     ├── IndexOutOfRangeException ← 陣列索引超出範圍
//   │     ├── InvalidOperationException ← 操作在目前狀態下無效
//   │     └── ArgumentException       ← 傳入的參數不合法
//   │           └── ArgumentNullException ← 參數為 null
//   └── ApplicationException        ← 應用程式層級例外（較少用）
```

---

## 📌 自訂例外

```csharp
// 建立自訂例外類別（繼承 Exception）
public class InsufficientBalanceException : Exception // 餘額不足例外
{
    // 目前餘額
    public decimal CurrentBalance { get; }

    // 嘗試提領的金額
    public decimal WithdrawAmount { get; }

    // 建構函式，接收餘額和提領金額
    public InsufficientBalanceException(decimal balance, decimal amount)
        : base($""餘額不足！目前餘額：{balance}，嘗試提領：{amount}"") // 呼叫父類建構函式設定訊息
    {
        CurrentBalance = balance;  // 儲存目前餘額
        WithdrawAmount = amount;   // 儲存提領金額
    }
}

// 使用自訂例外
public class BankAccount // 銀行帳戶類別
{
    // 帳戶餘額
    public decimal Balance { get; private set; } = 1000m;

    // 提款方法
    public void Withdraw(decimal amount)
    {
        if (amount > Balance) // 如果提領金額超過餘額
        {
            // 拋出自訂例外，附帶餘額和金額資訊
            throw new InsufficientBalanceException(Balance, amount);
        }
        Balance -= amount; // 扣除餘額
    }
}
```

---

## 📌 throw vs throw ex 的差異

```csharp
// ❌ 使用 throw ex — 會遺失原始堆疊追蹤
try
{
    // 呼叫某個可能出錯的方法
    SomeMethod();
}
catch (Exception ex) // 捕捉到例外
{
    // 記錄錯誤日誌
    Console.WriteLine(ex.Message);
    throw ex; // ❌ 堆疊追蹤會從這裡重新開始，原始錯誤位置遺失
}

// ✅ 使用 throw — 保留完整的堆疊追蹤
try
{
    // 呼叫某個可能出錯的方法
    SomeMethod();
}
catch (Exception ex) // 捕捉到例外
{
    // 記錄錯誤日誌
    Console.WriteLine(ex.Message);
    throw; // ✅ 保留原始堆疊追蹤，可以追溯到真正出錯的地方
}
```

**比喻：** `throw ex` 就像把犯罪現場的指紋擦掉再報警，警察就找不到原始線索了。`throw` 則是完整保留犯罪現場。

---

## 📌 除錯技巧

### 中斷點（Breakpoint）

在 Visual Studio 中，點擊程式碼左邊的灰色區域即可設定中斷點。程式執行到該行時會暫停，讓你檢查變數的值。

### 監看式（Watch）

在中斷點暫停時，可以把變數加入「監看式」視窗，即時觀察變數的變化。

### 即時運算視窗（Immediate Window）

在除錯暫停時，可以在即時運算視窗輸入 C# 運算式來測試：

```csharp
// 在 Immediate Window 中可以這樣輸入：
// ? myVariable          ← 查看變數的值
// ? myList.Count        ← 查看集合的元素數量
// ? myObject.ToString() ← 呼叫物件的方法
// myVariable = 42       ← 即時修改變數的值（測試用）
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：捕捉範圍太廣

```csharp
// ❌ 錯誤寫法：捕捉所有 Exception，吞掉所有錯誤
try
{
    // 執行某個操作
    ProcessData();
}
catch (Exception) // 捕捉所有例外，但什麼都不做
{
    // 空的 catch 區塊！錯誤被默默吞掉，你永遠不知道出了什麼問題
}
```

```csharp
// ✅ 正確寫法：捕捉具體的例外，並適當處理
try
{
    // 執行某個操作
    ProcessData();
}
catch (FileNotFoundException ex) // 只捕捉檔案不存在的例外
{
    // 記錄錯誤並通知使用者
    Console.WriteLine($""找不到檔案：{ex.FileName}"");
}
catch (Exception ex) // 其他未預期的例外
{
    // 至少要記錄錯誤日誌
    Console.WriteLine($""未預期錯誤：{ex}"");
    throw; // 重新拋出，讓上層處理
}
```

**解釋：** 空的 catch 區塊就像把所有警報都關掉，表面上一切正常，但問題其實在暗處惡化。

### ❌ 錯誤 2：throw new Exception() 包裝錯誤

```csharp
// ❌ 錯誤寫法：用新的 Exception 包裝，遺失原始資訊
try
{
    // 執行可能出錯的操作
    ConnectToDatabase();
}
catch (Exception ex) // 捕捉到例外
{
    // 建立全新的例外，原始的例外類型和堆疊追蹤全部遺失
    throw new Exception(""連線失敗""); // ❌ 遺失原始例外的所有資訊
}
```

```csharp
// ✅ 正確寫法：用 inner exception 保留原始例外
try
{
    // 執行可能出錯的操作
    ConnectToDatabase();
}
catch (Exception ex) // 捕捉到例外
{
    // 建立新例外時，把原始例外作為 inner exception 傳入
    throw new InvalidOperationException(""資料庫連線失敗"", ex); // ✅ 保留原始例外
}
```

**解釋：** 就像轉述別人的話時，不只說結論，也要說明原始來源，這樣才能追溯問題根源。

### ❌ 錯誤 3：在 finally 中 return

```csharp
// ❌ 錯誤寫法：在 finally 中使用 return
static int GetValue()
{
    try
    {
        return 1; // 嘗試回傳 1
    }
    finally
    {
        return 2; // ❌ finally 中的 return 會覆蓋 try 中的 return（C# 不允許此寫法）
    }
}
```

```csharp
// ✅ 正確寫法：finally 只做清理工作
static int GetValue()
{
    int result = 0; // 宣告結果變數
    try
    {
        result = 1; // 設定結果
        return result; // 回傳結果
    }
    finally
    {
        // finally 只做清理，不要 return
        Console.WriteLine(""清理完成""); // 釋放資源等操作
    }
}
```

**解釋：** `finally` 的職責是清理資源（像是打掃戰場），不應該用來改變程式的回傳值。
"
            },

            // ── Chapter 100: 委派、事件與 Lambda ──
            new Chapter
            {
                Id = 100,
                Title = "委派、事件與 Lambda",
                Slug = "csharp-delegates-events-lambda",
                Category = "csharp",
                Order = 10,
                Level = "advanced",
                Icon = "🎯",
                IsPublished = true,
                Content = @"# 🎯 委派、事件與 Lambda

## 📌 什麼是委派（Delegate）？

委派就像一張**外送訂單**。你不需要自己去餐廳拿餐，只要把訂單（方法的參考）交給外送員（委派），他就會幫你執行。

### 基本委派

```csharp
// 宣告一個委派型別（定義訂單的格式：接收兩個 int，回傳一個 int）
delegate int MathOperation(int a, int b);

// 定義一個加法方法（符合委派的格式）
static int Add(int x, int y)
{
    return x + y; // 回傳兩數相加的結果
}

// 定義一個乘法方法（也符合委派的格式）
static int Multiply(int x, int y)
{
    return x * y; // 回傳兩數相乘的結果
}

// 使用委派
MathOperation operation = Add; // 把 Add 方法指派給委派（外送員接了加法的單）
int result1 = operation(3, 4); // 呼叫委派，等於呼叫 Add(3, 4)，結果為 7

operation = Multiply; // 現在改指派 Multiply 方法（外送員接了乘法的單）
int result2 = operation(3, 4); // 呼叫委派，等於呼叫 Multiply(3, 4)，結果為 12
```

---

## 📌 內建泛型委派

C# 提供了三個常用的內建委派，你不需要自己宣告 delegate 型別：

```csharp
// Action<T>：接收參數但不回傳值（像是下命令）
Action<string> greet = (name) => // 接收一個 string 參數
{
    // 印出歡迎訊息
    Console.WriteLine($""你好，{name}！"");
};
greet(""小明""); // 呼叫 Action，印出「你好，小明！」

// Func<T, TResult>：接收參數並回傳值（像是問問題然後得到答案）
Func<int, int, int> add = (a, b) => // 接收兩個 int，回傳一個 int
{
    return a + b; // 回傳相加結果
};
int sum = add(10, 20); // 呼叫 Func，得到 30

// Predicate<T>：接收參數並回傳 bool（像是一個判斷條件）
Predicate<int> isEven = (number) => // 接收一個 int
{
    return number % 2 == 0; // 判斷是否為偶數，回傳 true 或 false
};
bool check = isEven(4); // 呼叫 Predicate，得到 true

// 實際應用：用 Predicate 篩選清單
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 }; // 建立數字清單
List<int> evenNumbers = numbers.FindAll(isEven); // 找出所有偶數：2, 4, 6
```

---

## 📌 事件（Events）

事件就像**訂閱 YouTube 頻道**。當頻道發布新影片（事件觸發），所有訂閱者（事件處理器）都會收到通知。

### 自訂事件

```csharp
// 自訂事件參數類別（描述事件的詳細資訊）
public class OrderEventArgs : EventArgs // 繼承 EventArgs
{
    // 訂單編號
    public string OrderId { get; set; }

    // 訂單金額
    public decimal Amount { get; set; }
}

// 訂單處理器類別（YouTube 頻道）
public class OrderProcessor
{
    // 宣告事件（這是頻道的「訂閱」功能）
    public event EventHandler<OrderEventArgs> OrderCompleted;

    // 處理訂單的方法
    public void ProcessOrder(string orderId, decimal amount)
    {
        // 處理訂單邏輯...
        Console.WriteLine($""正在處理訂單 {orderId}...""); // 印出處理中訊息

        // 訂單完成後，觸發事件（發布新影片通知訂閱者）
        OnOrderCompleted(new OrderEventArgs
        {
            OrderId = orderId, // 設定訂單編號
            Amount = amount    // 設定訂單金額
        });
    }

    // 觸發事件的保護方法（標準做法）
    protected virtual void OnOrderCompleted(OrderEventArgs e)
    {
        // 使用 ?. 確保有訂閱者才觸發（避免 null）
        OrderCompleted?.Invoke(this, e); // 通知所有訂閱者
    }
}

// 使用事件
OrderProcessor processor = new OrderProcessor(); // 建立訂單處理器

// 訂閱事件（像是按下 YouTube 的訂閱鈕）
processor.OrderCompleted += (sender, e) =>
{
    // 收到通知後，印出訂單完成的訊息
    Console.WriteLine($""訂單 {e.OrderId} 已完成，金額：{e.Amount}"");
};

// 訂閱第二個處理器（寄送通知信）
processor.OrderCompleted += (sender, e) =>
{
    // 收到通知後，寄送 Email
    Console.WriteLine($""已寄送確認信給客戶，訂單：{e.OrderId}"");
};

// 處理訂單（觸發事件，通知所有訂閱者）
processor.ProcessOrder(""ORD-001"", 1500m);
```

---

## 📌 Lambda 表達式

Lambda 就像一張**便利貼上的簡短指令**，不需要另外寫一個完整的方法。

```csharp
// 傳統寫法：定義完整的方法
static bool IsPositive(int n)
{
    return n > 0; // 判斷是否為正數
}

// Lambda 寫法：簡潔的匿名方法
Func<int, bool> isPositive = n => n > 0; // n 是參數，n > 0 是回傳值

// 多個參數的 Lambda
Func<int, int, int> multiply = (a, b) => a * b; // 兩個參數相乘

// 多行 Lambda（需要大括號和 return）
Func<int, string> classify = (n) =>
{
    if (n > 0) return ""正數"";  // 大於 0 回傳正數
    if (n < 0) return ""負數"";  // 小於 0 回傳負數
    return ""零"";               // 等於 0 回傳零
};

// Lambda 在 LINQ 中的實際應用
List<int> numbers = new List<int> { -3, -1, 0, 2, 5, 8 }; // 建立數字清單

// 用 Lambda 篩選正數
var positives = numbers.Where(n => n > 0).ToList(); // 結果：2, 5, 8

// 用 Lambda 轉換每個數字
var doubled = numbers.Select(n => n * 2).ToList(); // 每個數字乘以 2

// 用 Lambda 排序
var sorted = numbers.OrderByDescending(n => n).ToList(); // 由大到小排序
```

---

## 📌 閉包（Closure）與捕獲變數

```csharp
// 閉包：Lambda 可以「記住」外部變數
int multiplier = 3; // 外部變數

Func<int, int> tripler = n => n * multiplier; // Lambda 捕獲了 multiplier 變數

Console.WriteLine(tripler(5)); // 印出 15（5 * 3）

multiplier = 10; // 修改外部變數
Console.WriteLine(tripler(5)); // 印出 50（5 * 10）！因為 Lambda 記住的是變數本身，不是值
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：事件處理器造成記憶體洩漏

```csharp
// ❌ 錯誤寫法：訂閱了事件卻忘記取消訂閱
public class Listener // 監聽者類別
{
    public Listener(OrderProcessor processor) // 建構函式
    {
        // 訂閱事件，但從來不取消
        processor.OrderCompleted += OnOrderCompleted; // ❌ 永遠不會被垃圾回收
    }

    private void OnOrderCompleted(object sender, OrderEventArgs e)
    {
        Console.WriteLine(""收到訂單通知""); // 處理事件
    }
}
```

```csharp
// ✅ 正確寫法：實作 IDisposable，在不需要時取消訂閱
public class Listener : IDisposable // 實作 IDisposable
{
    private readonly OrderProcessor _processor; // 保存處理器的參考

    public Listener(OrderProcessor processor)
    {
        _processor = processor; // 儲存參考以便之後取消訂閱
        _processor.OrderCompleted += OnOrderCompleted; // 訂閱事件
    }

    private void OnOrderCompleted(object sender, OrderEventArgs e)
    {
        Console.WriteLine(""收到訂單通知""); // 處理事件
    }

    public void Dispose() // 清理資源
    {
        _processor.OrderCompleted -= OnOrderCompleted; // ✅ 取消訂閱，避免記憶體洩漏
    }
}
```

**解釋：** 事件訂閱就像租房子，退租時要把鑰匙還回去。不取消訂閱，物件就永遠不會被回收。

### ❌ 錯誤 2：迴圈中的閉包陷阱

```csharp
// ❌ 錯誤寫法：迴圈中捕獲的是變數 i，不是值
List<Action> actions = new List<Action>(); // 建立動作清單
for (int i = 0; i < 3; i++) // 迴圈 0, 1, 2
{
    // 每個 Lambda 都捕獲同一個變數 i
    actions.Add(() => Console.WriteLine(i)); // ❌ 全部都會印出 3！
}
foreach (var action in actions) // 執行所有動作
{
    action(); // 印出 3, 3, 3（不是預期的 0, 1, 2）
}
```

```csharp
// ✅ 正確寫法：在迴圈內建立區域變數
List<Action> actions = new List<Action>(); // 建立動作清單
for (int i = 0; i < 3; i++) // 迴圈 0, 1, 2
{
    int captured = i; // ✅ 每次迴圈建立新的區域變數，捕獲當時的值
    actions.Add(() => Console.WriteLine(captured)); // 捕獲 captured 而非 i
}
foreach (var action in actions) // 執行所有動作
{
    action(); // ✅ 正確印出 0, 1, 2
}
```

**解釋：** 迴圈變數 `i` 只有一個，所有 Lambda 共用它。迴圈結束後 `i` 變成 3，所以全部印 3。建立區域變數就像拍照留念，把當下的值記錄下來。

### ❌ 錯誤 3：混淆 Func 和 Action

```csharp
// ❌ 錯誤寫法：Func 需要回傳值，Action 不需要
Func<int, int> doubleIt = (n) =>
{
    Console.WriteLine(n * 2); // ❌ 編譯錯誤！Func 必須有 return
};
```

```csharp
// ✅ 正確寫法：根據需求選擇正確的委派型別
// 如果需要回傳值，用 Func
Func<int, int> doubleIt = (n) =>
{
    return n * 2; // ✅ Func 必須回傳值
};

// 如果不需要回傳值，用 Action
Action<int> printDouble = (n) =>
{
    Console.WriteLine(n * 2); // ✅ Action 不需要 return
};
```

**解釋：** `Func` 像是一個有回傳值的「問答題」，`Action` 像是一個「指令」。搞混就像拿選擇題的答案卡去寫作文。
"
            },

            // ── Chapter 101: 反射與特性 ──
            new Chapter
            {
                Id = 101,
                Title = "反射與特性 (Reflection & Attributes)",
                Slug = "csharp-reflection-attributes",
                Category = "csharp",
                Order = 11,
                Level = "advanced",
                Icon = "🔮",
                IsPublished = true,
                Content = @"# 🔮 反射與特性 (Reflection & Attributes)

## 📌 什麼是反射（Reflection）？

反射就像一面**魔鏡**，讓你在程式執行時可以「照」出任何物件的內部結構：它有哪些屬性、方法、欄位，甚至可以動態呼叫它們。

---

## 📌 取得型別資訊

```csharp
// 方法 1：透過 typeof 取得型別（編譯時期就知道型別）
Type stringType = typeof(string); // 取得 string 的型別資訊
Console.WriteLine(stringType.FullName); // 印出 ""System.String""

// 方法 2：透過物件實例的 GetType() 取得（執行時期才知道）
object myObj = ""Hello""; // 宣告一個 object 變數
Type objType = myObj.GetType(); // 取得實際的型別資訊
Console.WriteLine(objType.Name); // 印出 ""String""

// 方法 3：透過型別名稱字串取得（完全動態）
Type typeByName = Type.GetType(""System.Int32""); // 用字串取得 int 的型別
Console.WriteLine(typeByName.Name); // 印出 ""Int32""
```

---

## 📌 檢查屬性與方法

```csharp
// 定義一個簡單的類別作為示範
public class Student
{
    // 學生姓名
    public string Name { get; set; }

    // 學生年齡
    public int Age { get; set; }

    // 私有欄位：學號
    private string _studentId = ""S001"";

    // 公開方法：打招呼
    public string SayHello()
    {
        return $""我是 {Name}，今年 {Age} 歲""; // 回傳自我介紹
    }

    // 私有方法：取得學號
    private string GetStudentId()
    {
        return _studentId; // 回傳學號
    }
}

// 使用反射檢查 Student 類別
Type studentType = typeof(Student); // 取得 Student 的型別資訊

// 取得所有公開屬性
Console.WriteLine(""=== 公開屬性 ===""); // 標題
foreach (var prop in studentType.GetProperties()) // 走訪所有屬性
{
    // 印出屬性名稱和型別
    Console.WriteLine($""  {prop.Name} ({prop.PropertyType.Name})"");
}

// 取得所有公開方法
Console.WriteLine(""=== 公開方法 ===""); // 標題
foreach (var method in studentType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)) // 只取自己宣告的公開實例方法
{
    // 印出方法名稱和回傳型別
    Console.WriteLine($""  {method.Name}() -> {method.ReturnType.Name}"");
}

// 取得私有成員（需要特殊的 BindingFlags）
Console.WriteLine(""=== 私有欄位 ===""); // 標題
foreach (var field in studentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)) // 取得私有實例欄位
{
    // 印出私有欄位名稱
    Console.WriteLine($""  {field.Name}"");
}
```

---

## 📌 動態建立物件與呼叫方法

```csharp
// 動態建立物件（不用直接 new）
Type type = typeof(Student); // 取得型別
object instance = Activator.CreateInstance(type); // 動態建立 Student 實例

// 動態設定屬性值
PropertyInfo nameProp = type.GetProperty(""Name""); // 取得 Name 屬性的資訊
nameProp.SetValue(instance, ""小明""); // 把 ""小明"" 設定給 Name 屬性

PropertyInfo ageProp = type.GetProperty(""Age""); // 取得 Age 屬性的資訊
ageProp.SetValue(instance, 20); // 把 20 設定給 Age 屬性

// 動態呼叫方法
MethodInfo sayHello = type.GetMethod(""SayHello""); // 取得 SayHello 方法的資訊
object result = sayHello.Invoke(instance, null); // 呼叫方法（null 表示沒有參數）
Console.WriteLine(result); // 印出 ""我是 小明，今年 20 歲""

// 動態呼叫私有方法
MethodInfo getId = type.GetMethod(""GetStudentId"", BindingFlags.NonPublic | BindingFlags.Instance); // 取得私有方法
object id = getId.Invoke(instance, null); // 呼叫私有方法
Console.WriteLine($""學號：{id}""); // 印出 ""學號：S001""
```

---

## 📌 特性（Attributes）

特性就像**貼在程式碼上的便利貼**，用來標記額外的資訊。程式本身不會直接受影響，但其他程式碼（例如框架）可以讀取這些便利貼。

### 內建特性

```csharp
// [Obsolete] 標記方法已過時
[Obsolete(""請改用 NewMethod()，此方法將在 v3.0 移除"")] // 標記為過時
public void OldMethod()
{
    // 舊的實作方式
    Console.WriteLine(""這是舊方法"");
}

// [Serializable] 標記類別可以被序列化
[Serializable] // 標記此類別的實例可以被序列化
public class Config
{
    // 設定名稱
    public string Name { get; set; }
}
```

### 自訂特性

```csharp
// 建立自訂特性類別
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] // 限制只能用在屬性上，且不能重複
public class ValidateRangeAttribute : Attribute // 繼承 Attribute 基底類別
{
    // 最小值
    public int Min { get; }

    // 最大值
    public int Max { get; }

    // 建構函式，設定範圍
    public ValidateRangeAttribute(int min, int max)
    {
        Min = min; // 儲存最小值
        Max = max; // 儲存最大值
    }

    // 驗證方法
    public bool IsValid(int value)
    {
        return value >= Min && value <= Max; // 檢查值是否在範圍內
    }
}

// 使用自訂特性
public class Product
{
    // 商品名稱
    public string Name { get; set; }

    // 價格必須在 1 到 99999 之間
    [ValidateRange(1, 99999)] // 使用自訂特性標記驗證規則
    public int Price { get; set; }

    // 數量必須在 0 到 1000 之間
    [ValidateRange(0, 1000)] // 使用自訂特性標記驗證規則
    public int Quantity { get; set; }
}

// 用反射讀取自訂特性並執行驗證
public static bool ValidateProduct(Product product)
{
    Type type = product.GetType(); // 取得物件的型別資訊

    foreach (var prop in type.GetProperties()) // 走訪所有屬性
    {
        // 嘗試取得 ValidateRange 特性
        var attr = prop.GetCustomAttribute<ValidateRangeAttribute>();

        if (attr != null) // 如果有標記 ValidateRange
        {
            int value = (int)prop.GetValue(product); // 取得屬性的值
            if (!attr.IsValid(value)) // 驗證是否在範圍內
            {
                // 驗證失敗，印出錯誤訊息
                Console.WriteLine($""{prop.Name} 的值 {value} 不在 {attr.Min}~{attr.Max} 範圍內"");
                return false; // 回傳驗證失敗
            }
        }
    }

    return true; // 所有驗證通過
}
```

---

## 📌 何時該用反射？何時不該用？

```csharp
// ✅ 適合使用反射的情境：
// 1. 框架與函式庫開發（例如 ASP.NET MVC 的 Model Binding）
// 2. 序列化 / 反序列化（JSON、XML）
// 3. 依賴注入容器（DI Container）
// 4. 單元測試中存取私有成員
// 5. 外掛（Plugin）系統，動態載入組件

// ❌ 不適合使用反射的情境：
// 1. 效能敏感的程式碼（反射比直接呼叫慢 10~100 倍）
// 2. 可以用介面（Interface）或泛型解決的問題
// 3. 簡單的物件建立（直接 new 就好）
// 4. 頻繁呼叫的路徑（hot path）
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：BindingFlags 用錯

```csharp
// ❌ 錯誤寫法：想取得私有方法，但沒加正確的 BindingFlags
Type type = typeof(Student); // 取得型別
MethodInfo method = type.GetMethod(""GetStudentId""); // ❌ 回傳 null！預設只搜尋公開方法
method.Invoke(new Student(), null); // 💥 NullReferenceException！
```

```csharp
// ✅ 正確寫法：加上 NonPublic 和 Instance 旗標
Type type = typeof(Student); // 取得型別
MethodInfo method = type.GetMethod(
    ""GetStudentId"",
    BindingFlags.NonPublic | BindingFlags.Instance // ✅ 指定搜尋非公開的實例方法
);
if (method != null) // 先確認方法存在
{
    object result = method.Invoke(new Student(), null); // 安全地呼叫
    Console.WriteLine(result); // 印出結果
}
```

**解釋：** `GetMethod()` 預設只搜尋公開方法。要找私有方法必須明確告訴它搜尋範圍，就像在圖書館找書要去對的樓層。

### ❌ 錯誤 2：反射效能問題

```csharp
// ❌ 錯誤寫法：在迴圈中反覆使用反射取得屬性
for (int i = 0; i < 100000; i++) // 十萬次迴圈
{
    Type type = student.GetType(); // 每次都重新取得型別（浪費效能）
    PropertyInfo prop = type.GetProperty(""Name""); // 每次都重新搜尋屬性
    string name = (string)prop.GetValue(student); // 每次都透過反射取值
}
```

```csharp
// ✅ 正確寫法：快取反射結果，避免重複搜尋
Type type = student.GetType(); // 只取得一次型別
PropertyInfo prop = type.GetProperty(""Name""); // 只搜尋一次屬性

for (int i = 0; i < 100000; i++) // 十萬次迴圈
{
    string name = (string)prop.GetValue(student); // 重複使用已快取的 PropertyInfo
}
```

**解釋：** 反射的搜尋過程很耗效能。每次在迴圈裡重新搜尋就像每次打電話都重新查電話簿，直接把號碼記下來（快取）會快很多。

### ❌ 錯誤 3：AttributeUsage 設定錯誤

```csharp
// ❌ 錯誤寫法：特性標記在錯誤的目標上
[AttributeUsage(AttributeTargets.Method)] // 限制只能用在方法上
public class MyAttribute : Attribute { }

[MyAttribute] // 💥 編譯錯誤！MyAttribute 只能用在方法上，不能用在類別上
public class MyClass { }
```

```csharp
// ✅ 正確寫法：根據需求設定正確的 AttributeTargets
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)] // ✅ 允許用在類別和方法上
public class MyAttribute : Attribute { }

[MyAttribute] // ✅ 可以用在類別上
public class MyClass
{
    [MyAttribute] // ✅ 也可以用在方法上
    public void MyMethod() { }
}
```

**解釋：** `AttributeTargets` 決定你的便利貼可以貼在哪裡。貼錯地方就像把「小心地滑」的牌子掛在天花板上。
"
            },

            // ── Chapter 102: 多執行緒與平行處理 ──
            new Chapter
            {
                Id = 102,
                Title = "多執行緒與平行處理",
                Slug = "csharp-multithreading-parallel",
                Category = "csharp",
                Order = 12,
                Level = "advanced",
                Icon = "🧵",
                IsPublished = true,
                Content = @"# 🧵 多執行緒與平行處理

## 📌 什麼是多執行緒？

想像你在一家餐廳工作。**單執行緒**就像只有一個廚師，要一道一道菜做。**多執行緒**就像有多個廚師，可以同時做不同的菜，大幅提升效率。

---

## 📌 Thread 基礎

```csharp
// 建立一個新的執行緒（雇一個新廚師）
Thread thread = new Thread(() => // 用 Lambda 定義要執行的工作
{
    // 模擬耗時的工作
    for (int i = 0; i < 5; i++) // 迴圈 5 次
    {
        Console.WriteLine($""背景執行緒：{i}""); // 印出目前進度
        Thread.Sleep(1000); // 暫停 1 秒（模擬耗時工作）
    }
});

thread.IsBackground = true; // 設定為背景執行緒（主程式結束時會自動停止）
thread.Start(); // 啟動執行緒（新廚師開始工作）

// 主執行緒繼續做自己的事
Console.WriteLine(""主執行緒繼續執行""); // 這行會馬上印出

thread.Join(); // 等待背景執行緒完成（等新廚師做完才繼續）
Console.WriteLine(""所有工作完成""); // 背景執行緒做完後才印出
```

---

## 📌 ThreadPool 與 Task.Run

```csharp
// ThreadPool：由系統管理的執行緒池（像是外包的廚師團隊）
ThreadPool.QueueUserWorkItem(state => // 把工作丟給執行緒池
{
    // 在執行緒池中執行工作
    Console.WriteLine($""ThreadPool 執行緒 ID：{Thread.CurrentThread.ManagedThreadId}"");
});

// Task.Run：現代化的做法（推薦使用）
Task task = Task.Run(() => // 把工作丟給背景執行緒執行
{
    // 模擬耗時計算
    int result = 0; // 初始化結果
    for (int i = 0; i < 1000000; i++) // 計算一百萬次
    {
        result += i; // 累加
    }
    Console.WriteLine($""計算結果：{result}""); // 印出結果
});

// Task.Run 搭配回傳值
Task<int> taskWithResult = Task.Run(() => // 泛型版本可以回傳值
{
    // 模擬耗時計算
    Thread.Sleep(2000); // 暫停 2 秒
    return 42; // 回傳計算結果
});

int answer = taskWithResult.Result; // 取得結果（會阻塞直到完成）
Console.WriteLine($""答案是：{answer}""); // 印出 42
```

---

## 📌 同步機制：保護共享資源

### lock 關鍵字

```csharp
// 沒有 lock 的危險情況：多個廚師搶同一把刀
public class Counter
{
    private int _count = 0; // 共享的計數器
    private readonly object _lockObj = new object(); // 鎖定物件（像是一把鑰匙）

    // 安全的遞增方法
    public void SafeIncrement()
    {
        lock (_lockObj) // 進入前先拿到鑰匙（一次只有一個執行緒能進入）
        {
            _count++; // 安全地修改共享資源
        } // 離開時自動歸還鑰匙
    }

    // 取得目前計數
    public int GetCount()
    {
        lock (_lockObj) // 讀取時也要鎖定，確保讀到正確的值
        {
            return _count; // 回傳計數值
        }
    }
}

// 使用範例
Counter counter = new Counter(); // 建立計數器

// 建立多個 Task 同時遞增
Task[] tasks = new Task[10]; // 準備 10 個任務
for (int i = 0; i < 10; i++) // 迴圈建立任務
{
    tasks[i] = Task.Run(() => // 每個任務都會遞增 1000 次
    {
        for (int j = 0; j < 1000; j++) // 迴圈 1000 次
        {
            counter.SafeIncrement(); // 安全地遞增
        }
    });
}

Task.WaitAll(tasks); // 等待所有任務完成
Console.WriteLine($""最終計數：{counter.GetCount()}""); // 應該印出 10000
```

### Monitor、Mutex、SemaphoreSlim

```csharp
// Monitor：跟 lock 類似，但提供更多控制
object monitorObj = new object(); // 建立監視器物件
bool lockTaken = false; // 追蹤是否成功取得鎖定

try
{
    Monitor.TryEnter(monitorObj, TimeSpan.FromSeconds(5), ref lockTaken); // 嘗試在 5 秒內取得鎖定
    if (lockTaken) // 如果成功取得鎖定
    {
        // 安全地存取共享資源
        Console.WriteLine(""已取得鎖定，執行工作中..."");
    }
    else // 如果 5 秒內沒取得鎖定
    {
        Console.WriteLine(""無法取得鎖定，跳過""); // 超時處理
    }
}
finally
{
    if (lockTaken) // 如果有取得鎖定
    {
        Monitor.Exit(monitorObj); // 釋放鎖定
    }
}

// SemaphoreSlim：限制同時存取的數量（像是停車場的車位）
SemaphoreSlim semaphore = new SemaphoreSlim(3); // 最多允許 3 個執行緒同時進入

async Task AccessResource(int id)
{
    await semaphore.WaitAsync(); // 等待取得一個「車位」
    try
    {
        Console.WriteLine($""執行緒 {id} 進入（剩餘車位：{semaphore.CurrentCount}）""); // 印出進入訊息
        await Task.Delay(2000); // 模擬使用資源 2 秒
    }
    finally
    {
        semaphore.Release(); // 離開時歸還「車位」
        Console.WriteLine($""執行緒 {id} 離開""); // 印出離開訊息
    }
}
```

---

## 📌 Parallel 與 PLINQ

```csharp
// Parallel.ForEach：平行處理集合中的每個元素
List<string> urls = new List<string> // 要處理的 URL 清單
{
    ""https://example1.com"", // 第一個網址
    ""https://example2.com"", // 第二個網址
    ""https://example3.com""  // 第三個網址
};

Parallel.ForEach(urls, url => // 平行處理每個 URL
{
    // 每個 URL 會在不同的執行緒上同時處理
    Console.WriteLine($""正在處理 {url}（執行緒 {Thread.CurrentThread.ManagedThreadId}）"");
    // 模擬下載
    Thread.Sleep(1000); // 模擬耗時 1 秒
});

// PLINQ：用 LINQ 語法做平行查詢
List<int> numbers = Enumerable.Range(1, 1000000).ToList(); // 建立一百萬個數字

// 用 AsParallel() 把 LINQ 變成平行版本
var evenSquares = numbers
    .AsParallel() // 啟用平行處理
    .Where(n => n % 2 == 0) // 篩選偶數（平行執行）
    .Select(n => n * n) // 計算平方（平行執行）
    .ToList(); // 收集結果

Console.WriteLine($""偶數平方的數量：{evenSquares.Count}""); // 印出結果數量
```

---

## 📌 async vs 多執行緒的差異

```csharp
// 多執行緒（Threading）：建立新的工人去做事
// 適合：CPU 密集的計算（壓縮、加密、數學運算）
Task.Run(() =>
{
    // 這裡會在另一個執行緒上執行
    // 適合做大量計算
    double result = 0;
    for (int i = 0; i < 10000000; i++) // 大量計算
    {
        result += Math.Sqrt(i); // CPU 密集的工作
    }
});

// async/await：不建立新工人，而是讓現有工人在等待時去做別的事
// 適合：I/O 操作（網路請求、檔案讀寫、資料庫查詢）
async Task<string> FetchDataAsync()
{
    using HttpClient client = new HttpClient(); // 建立 HTTP 客戶端
    // await 時執行緒可以去做別的事（不會閒置等待）
    string data = await client.GetStringAsync(""https://api.example.com/data""); // 等待網路回應
    return data; // 回傳資料
}

// 簡單比喻：
// 多執行緒 = 雇更多廚師來做菜（增加人力）
// async/await = 一個廚師在等水燒開的時候去切菜（提高效率）
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：死結（Deadlock）

```csharp
// ❌ 錯誤寫法：兩個執行緒互相等待對方的鎖
object lockA = new object(); // 鎖 A
object lockB = new object(); // 鎖 B

// 執行緒 1：先拿 A 再拿 B
Task task1 = Task.Run(() =>
{
    lock (lockA) // 取得鎖 A
    {
        Thread.Sleep(100); // 短暫等待，增加死結機率
        lock (lockB) // 嘗試取得鎖 B → 💥 但鎖 B 被執行緒 2 拿走了！
        {
            Console.WriteLine(""執行緒 1 完成""); // 永遠不會執行
        }
    }
});

// 執行緒 2：先拿 B 再拿 A（順序相反）
Task task2 = Task.Run(() =>
{
    lock (lockB) // 取得鎖 B
    {
        Thread.Sleep(100); // 短暫等待
        lock (lockA) // 嘗試取得鎖 A → 💥 但鎖 A 被執行緒 1 拿走了！
        {
            Console.WriteLine(""執行緒 2 完成""); // 永遠不會執行
        }
    }
});
// 兩個執行緒互相等待，永遠不會結束 → 死結！
```

```csharp
// ✅ 正確寫法：統一鎖定順序
object lockA = new object(); // 鎖 A
object lockB = new object(); // 鎖 B

// 所有執行緒都按照相同順序取得鎖（先 A 後 B）
Task task1 = Task.Run(() =>
{
    lock (lockA) // 先取得鎖 A
    {
        lock (lockB) // 再取得鎖 B
        {
            Console.WriteLine(""執行緒 1 完成""); // ✅ 可以正常執行
        }
    }
});

Task task2 = Task.Run(() =>
{
    lock (lockA) // ✅ 也是先取得鎖 A（相同順序）
    {
        lock (lockB) // 再取得鎖 B
        {
            Console.WriteLine(""執行緒 2 完成""); // ✅ 可以正常執行
        }
    }
});
```

**解釋：** 死結就像兩個人在窄巷中面對面，都堅持對方先讓路，結果誰都走不了。解決方法：規定大家都靠右走（統一順序）。

### ❌ 錯誤 2：競爭條件（Race Condition）

```csharp
// ❌ 錯誤寫法：多個執行緒同時修改共享變數，沒有保護
int sharedCounter = 0; // 共享的計數器

Task[] tasks = new Task[10]; // 建立 10 個任務
for (int i = 0; i < 10; i++) // 迴圈建立任務
{
    tasks[i] = Task.Run(() =>
    {
        for (int j = 0; j < 10000; j++) // 每個任務遞增一萬次
        {
            sharedCounter++; // ❌ 不是原子操作，會產生競爭條件
        }
    });
}

Task.WaitAll(tasks); // 等待所有任務完成
Console.WriteLine($""結果：{sharedCounter}""); // ❌ 結果會小於 100000！
```

```csharp
// ✅ 正確寫法：使用 Interlocked 進行原子操作
int sharedCounter = 0; // 共享的計數器

Task[] tasks = new Task[10]; // 建立 10 個任務
for (int i = 0; i < 10; i++) // 迴圈建立任務
{
    tasks[i] = Task.Run(() =>
    {
        for (int j = 0; j < 10000; j++) // 每個任務遞增一萬次
        {
            Interlocked.Increment(ref sharedCounter); // ✅ 原子操作，保證執行緒安全
        }
    });
}

Task.WaitAll(tasks); // 等待所有任務完成
Console.WriteLine($""結果：{sharedCounter}""); // ✅ 正確印出 100000
```

**解釋：** `++` 操作其實分成三步：讀取、加一、寫回。兩個執行緒可能同時讀到相同的值，各自加一後寫回，導致只加了一次。就像兩個人同時從提款機領錢，可能只扣了一次帳。

### ❌ 錯誤 3：在背景執行緒存取 UI

```csharp
// ❌ 錯誤寫法（WinForms/WPF）：從背景執行緒直接修改 UI
Task.Run(() =>
{
    // 在背景執行緒做完計算後...
    string result = ""計算完成""; // 計算結果
    label1.Text = result; // 💥 InvalidOperationException！不能從非 UI 執行緒修改控制項
});
```

```csharp
// ✅ 正確寫法：使用 Invoke 切回 UI 執行緒
Task.Run(() =>
{
    // 在背景執行緒做完計算後...
    string result = ""計算完成""; // 計算結果

    // 使用 Invoke 切回 UI 執行緒來更新介面
    this.Invoke(() =>
    {
        label1.Text = result; // ✅ 在 UI 執行緒上安全地修改控制項
    });
});

// 或者更好的做法：使用 async/await（自動回到 UI 執行緒）
async void Button_Click(object sender, EventArgs e)
{
    // await 之後會自動回到 UI 執行緒
    string result = await Task.Run(() =>
    {
        // 背景計算
        return ""計算完成""; // 回傳結果
    });

    label1.Text = result; // ✅ 自動在 UI 執行緒上執行
}
```

**解釋：** UI 控制項只能由建立它的執行緒（UI 執行緒）來修改。就像餐廳的點餐系統只能由前台操作，廚師（背景執行緒）做好菜要透過前台（Invoke）才能送到客人桌上。
"
            }
        };
    }
}
