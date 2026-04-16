namespace DotNetLearning.Controllers;

/// <summary>
/// AI 教你打程式 — 擴充課程庫
/// 每個分類 20+ 課，從入門到進階
/// </summary>
public static class CodeTutorLessons
{
    public static List<CodeLesson> GetAllLessons()
    {
        var all = new List<CodeLesson>();
        all.AddRange(CSharpLessons());
        all.AddRange(JavaScriptLessons());
        all.AddRange(HtmlCssLessons());
        all.AddRange(SqlLessons());
        all.AddRange(VueLessons());
        all.AddRange(ApiLessons());
        all.AddRange(JavaLessons());
        return all;
    }

    // ═══════════════════════════════════════════
    //  C# 擴充（ID 101-112，補到 20 課）
    // ═══════════════════════════════════════════
    static List<CodeLesson> CSharpLessons() => new()
    {
        new() { Id=101, Category="csharp", Title="字串處理", Difficulty="beginner", Description="字串方法與格式化",
            Lines = new() {
                new() { Comment="// 字串長度與基本方法", Code="string name = \"Hello World\";", Explanation="💡 string 是 C# 中最常用的型別之一。用雙引號包住文字就是字串。" },
                new() { Comment="// 取得字串長度", Code="int len = name.Length;", Explanation="💡 Length 是字串的屬性（不是方法），所以不加括號。回傳字元數量。" },
                new() { Comment="// 轉大寫", Code="string upper = name.ToUpper();", Explanation="💡 ToUpper() 回傳新字串，原始字串不會被改變（字串是不可變的 immutable）。" },
                new() { Comment="// 擷取子字串", Code="string sub = name.Substring(0, 5);", Explanation="💡 Substring(起始位置, 長度)。索引從 0 開始，所以 Substring(0,5) 取前 5 個字元 = \"Hello\"。" },
                new() { Comment="// 取代文字", Code="string replaced = name.Replace(\"World\", \"C#\");", Explanation="💡 Replace 把所有符合的文字都替換掉。回傳新字串。" },
                new() { Comment="// 分割字串", Code="string[] words = name.Split(' ');", Explanation="💡 Split 用指定的分隔字元把字串切成陣列。\"Hello World\" 用空格分割得到 [\"Hello\", \"World\"]。" },
                new() { Comment="// 字串插值（推薦寫法）", Code="string msg = $\"名字是 {name}，長度 {len}\";", Explanation="💡 $ 開頭的字串可以用 {變數} 直接嵌入值，比 + 串接更清楚。這叫 String Interpolation。" },
                new() { Comment="// 檢查是否包含", Code="bool has = name.Contains(\"World\");", Explanation="💡 Contains 回傳 bool，區分大小寫。要忽略大小寫可用 name.Contains(\"world\", StringComparison.OrdinalIgnoreCase)。" },
            }
        },
        new() { Id=102, Category="csharp", Title="List<T> 泛型集合", Difficulty="beginner", Description="使用 List 動態管理資料",
            Lines = new() {
                new() { Comment="// 引入集合命名空間", Code="using System.Collections.Generic;", Explanation="💡 List<T> 在 System.Collections.Generic 命名空間。T 是泛型參數，代表「任意型別」。" },
                new() { Comment="// 建立整數 List", Code="List<int> numbers = new List<int>();", Explanation="💡 List<int> 表示「只能放 int 的清單」。泛型讓你在編譯時就能檢查型別安全。" },
                new() { Comment="// 新增元素", Code="numbers.Add(10);", Explanation="💡 Add() 在尾端新增一個元素。List 會自動擴展容量，不像陣列固定大小。" },
                new() { Comment="// 新增多個元素", Code="numbers.AddRange(new[] { 20, 30, 40 });", Explanation="💡 AddRange 一次加入多個元素。new[] { } 是陣列的簡寫語法。" },
                new() { Comment="// 移除元素", Code="numbers.Remove(20);", Explanation="💡 Remove 移除第一個符合的元素。回傳 bool 表示是否成功移除。" },
                new() { Comment="// 用索引存取", Code="int first = numbers[0];", Explanation="💡 和陣列一樣用 [] 存取，索引從 0 開始。超出範圍會拋出 ArgumentOutOfRangeException。" },
                new() { Comment="// 檢查元素數量", Code="int count = numbers.Count;", Explanation="💡 List 用 Count 屬性（不是 Length）。這是 List 和 Array 的差異之一。" },
                new() { Comment="// 排序", Code="numbers.Sort();", Explanation="💡 Sort() 會直接修改原 List（就地排序）。要降冪可以用 numbers.Sort((a,b) => b.CompareTo(a))。" },
            }
        },
        new() { Id=103, Category="csharp", Title="Dictionary 字典", Difficulty="intermediate", Description="鍵值對資料結構",
            Lines = new() {
                new() { Comment="// 建立字典：學生成績", Code="var scores = new Dictionary<string, int>();", Explanation="💡 Dictionary<TKey, TValue> 是鍵值對集合。TKey 是鍵的型別，TValue 是值的型別。鍵不能重複。" },
                new() { Comment="// 新增鍵值對", Code="scores[\"小明\"] = 95;", Explanation="💡 用 [] 可以新增或修改。如果鍵不存在就新增，存在就覆蓋。" },
                new() { Comment="// 也可以用 Add 方法", Code="scores.Add(\"小華\", 88);", Explanation="💡 Add 在鍵已存在時會拋出例外。如果不確定鍵是否存在，用 [] 更安全。" },
                new() { Comment="// 安全讀取", Code="if (scores.TryGetValue(\"小明\", out int score)) {", Explanation="💡 TryGetValue 嘗試取值，成功回傳 true 並透過 out 參數傳出值。比直接 [] 存取更安全。" },
                new() { Comment="// 印出成績", Code="    Console.WriteLine($\"小明的成績：{score}\");", Explanation="💡 out 參數在 TryGetValue 成功時會被賦值，可以直接在 if 區塊中使用。" },
                new() { Code="}" },
                new() { Comment="// 遍歷所有鍵值對", Code="foreach (var pair in scores) {", Explanation="💡 foreach 遍歷 Dictionary 得到 KeyValuePair<TKey,TValue>。可以用 pair.Key 和 pair.Value。" },
                new() { Code="    Console.WriteLine($\"{pair.Key}: {pair.Value}\");" },
                new() { Code="}" },
            }
        },
        new() { Id=104, Category="csharp", Title="例外處理 try/catch", Difficulty="intermediate", Description="處理程式中的錯誤",
            Lines = new() {
                new() { Comment="// 基本的 try-catch 結構", Code="try {", Explanation="💡 try 區塊放「可能出錯」的程式碼。如果出錯，程式不會崩潰，而是跳到 catch。" },
                new() { Comment="// 可能會出錯的操作", Code="    int result = int.Parse(\"abc\");", Explanation="💡 int.Parse 把字串轉成整數。\"abc\" 不是數字，所以會拋出 FormatException。" },
                new() { Code="} catch (FormatException ex) {", Explanation="💡 catch 指定要攔截的例外類型。FormatException 專門處理格式轉換錯誤。ex 是例外物件，包含錯誤資訊。" },
                new() { Code="    Console.WriteLine($\"格式錯誤：{ex.Message}\");", Explanation="💡 ex.Message 包含錯誤描述。實務上常用 ex.ToString() 取得完整堆疊追蹤（Stack Trace）。" },
                new() { Code="} catch (Exception ex) {", Explanation="💡 Exception 是所有例外的基底類別。放在最後當「通用攔截器」，前面放更具體的例外類型。" },
                new() { Code="    Console.WriteLine($\"未知錯誤：{ex.Message}\");" },
                new() { Code="} finally {", Explanation="💡 finally 不管有沒有出錯都會執行。常用來釋放資源（關閉檔案、資料庫連線等）。" },
                new() { Code="    Console.WriteLine(\"清理完成\");" },
                new() { Code="}" },
            }
        },
        new() { Id=105, Category="csharp", Title="介面 Interface", Difficulty="intermediate", Description="定義行為契約",
            Lines = new() {
                new() { Comment="// 定義介面：可以飛的東西", Code="interface IFlyable {", Explanation="💡 interface 定義「能做什麼」但不實作。慣例以 I 開頭命名。實作類別必須提供所有方法的實作。" },
                new() { Code="    void Fly();", Explanation="💡 介面中的方法沒有 body（大括號），只有簽章。這是一個「契約」，實作者保證會提供這個行為。" },
                new() { Code="    double GetAltitude();", Explanation="💡 介面可以有多個方法和屬性。所有成員預設都是 public。" },
                new() { Code="}" },
                new() { Comment="// 類別實作介面", Code="class Bird : IFlyable {", Explanation="💡 用冒號 : 實作介面，跟繼承語法一樣。一個類別可以實作多個介面（C# 不支援多重繼承但支援多介面）。" },
                new() { Code="    public void Fly() {", Explanation="💡 實作介面方法時必須加 public。方法簽章（名稱、參數、回傳值）必須完全一致。" },
                new() { Code="        Console.WriteLine(\"鳥在飛翔\");" },
                new() { Code="    }" },
                new() { Code="    public double GetAltitude() => 500.0;", Explanation="💡 expression-bodied member 語法：用 => 取代大括號，適合一行就能寫完的方法。" },
                new() { Code="}" },
            }
        },
        new() { Id=106, Category="csharp", Title="委派與事件", Difficulty="advanced", Description="理解 delegate 與 event 機制",
            Lines = new() {
                new() { Comment="// 定義委派型別", Code="delegate void Notify(string message);", Explanation="💡 delegate 定義方法的「型別」——什麼參數、什麼回傳值。它像是方法的指標，可以把方法當變數傳遞。" },
                new() { Comment="// 訂閱者的處理方法", Code="static void EmailNotify(string msg) {", Explanation="💡 這個方法的簽章（void + string 參數）符合 Notify 委派，所以可以被指派給 Notify 型別的變數。" },
                new() { Code="    Console.WriteLine($\"Email: {msg}\");" },
                new() { Code="}" },
                new() { Comment="// 使用委派", Code="Notify handler = EmailNotify;", Explanation="💡 把方法名稱（不加括號）指派給委派變數。handler 現在「指向」EmailNotify 這個方法。" },
                new() { Comment="// 透過委派呼叫方法", Code="handler(\"新訂單成立\");", Explanation="💡 透過委派呼叫，就像直接呼叫方法一樣。好處是可以動態切換要執行的方法。" },
                new() { Comment="// 多播委派：串接多個方法", Code="handler += (msg) => Console.WriteLine($\"SMS: {msg}\");", Explanation="💡 += 可以串接多個方法。觸發時會依序執行。Lambda (msg) => ... 是匿名方法的簡寫。" },
                new() { Comment="// 觸發所有訂閱者", Code="handler(\"商品已出貨\");", Explanation="💡 這次會同時觸發 EmailNotify 和 SMS Lambda。這就是觀察者模式（Observer Pattern）的基礎。" },
            }
        },
        new() { Id=107, Category="csharp", Title="async/await 非同步", Difficulty="advanced", Description="非同步程式設計核心",
            Lines = new() {
                new() { Comment="// 引入 HTTP 客戶端", Code="using System.Net.Http;", Explanation="💡 HttpClient 在 System.Net.Http 命名空間。它是 .NET 中發送 HTTP 請求的標準工具。" },
                new() { Comment="// 定義非同步方法", Code="static async Task<string> FetchDataAsync(string url) {", Explanation="💡 async 標記方法為非同步。Task<string> 表示「將來會回傳 string」。方法名加 Async 後綴是慣例。" },
                new() { Comment="// 建立 HTTP 客戶端", Code="    using var client = new HttpClient();", Explanation="💡 using var 確保 client 在離開作用域時自動釋放（Dispose）。HttpClient 實務上建議重用而非每次 new。" },
                new() { Comment="// 發送 GET 請求並等待回應", Code="    string result = await client.GetStringAsync(url);", Explanation="💡 await 暫停方法執行，等待非同步操作完成。期間執行緒被釋放去做其他工作，不會卡住。" },
                new() { Code="    return result;", Explanation="💡 非同步方法的 return 值會自動包成 Task<T>。呼叫端用 await 取得實際值。" },
                new() { Code="}" },
                new() { Comment="// 呼叫非同步方法", Code="string data = await FetchDataAsync(\"https://api.example.com\");", Explanation="💡 呼叫 async 方法時用 await 等待結果。不用 await 的話會得到 Task 物件而不是實際值。" },
                new() { Code="Console.WriteLine(data);", Explanation="💡 await 後面的程式碼會在非同步操作完成後繼續執行。整個過程不會阻塞主執行緒。" },
            }
        },
        new() { Id=108, Category="csharp", Title="泛型方法", Difficulty="advanced", Description="撰寫型別安全的通用方法",
            Lines = new() {
                new() { Comment="// 泛型方法：交換兩個值", Code="static void Swap<T>(ref T a, ref T b) {", Explanation="💡 <T> 是泛型參數，代表「任何型別」。ref 讓方法可以修改外部變數。一個方法適用所有型別！" },
                new() { Code="    T temp = a;", Explanation="💡 T 在呼叫時會被替換成實際型別。如果呼叫 Swap<int>，T 就是 int。" },
                new() { Code="    a = b;" },
                new() { Code="    b = temp;" },
                new() { Code="}" },
                new() { Comment="// 泛型方法搭配約束", Code="static T Max<T>(T a, T b) where T : IComparable<T> {", Explanation="💡 where T : IComparable<T> 約束 T 必須能比較大小。沒有約束的話無法用 > < 運算子。" },
                new() { Code="    return a.CompareTo(b) > 0 ? a : b;", Explanation="💡 CompareTo > 0 表示 a 大於 b。三元運算子 ? : 是 if-else 的簡寫。" },
                new() { Code="}" },
            }
        },
        new() { Id=109, Category="csharp", Title="Record 與模式比對", Difficulty="advanced", Description="C# 9+ 新語法",
            Lines = new() {
                new() { Comment="// Record：不可變的資料類型", Code="record Student(string Name, int Age);", Explanation="💡 record 自動生成 Equals、GetHashCode、ToString。參數是唯讀屬性。非常適合 DTO（Data Transfer Object）。" },
                new() { Comment="// 建立 record 實例", Code="var s1 = new Student(\"小明\", 20);", Explanation="💡 record 用建構函式建立。一旦建立，屬性不能修改（immutable）。" },
                new() { Comment="// with 表達式：複製並修改", Code="var s2 = s1 with { Age = 21 };", Explanation="💡 with 建立一個副本，只改指定的屬性。原始物件 s1 不受影響。" },
                new() { Comment="// 模式比對 switch expression", Code="string level = s1.Age switch {", Explanation="💡 switch expression 是 C# 8 新語法，比傳統 switch 更簡潔。直接回傳值。" },
                new() { Code="    < 18 => \"未成年\",", Explanation="💡 < 18 是關係模式（relational pattern）。不需要寫 case 和 break。" },
                new() { Code="    >= 18 and <= 25 => \"青年\",", Explanation="💡 and 是邏輯模式，組合多個條件。也可以用 or。" },
                new() { Code="    _ => \"成年\"", Explanation="💡 _ 是丟棄模式（discard），等同於 default。switch expression 必須涵蓋所有可能。" },
                new() { Code="};" },
            }
        },
        new() { Id=110, Category="csharp", Title="檔案讀寫", Difficulty="intermediate", Description="File 與 StreamReader/Writer",
            Lines = new() {
                new() { Comment="// 引入 IO 命名空間", Code="using System.IO;", Explanation="💡 System.IO 包含所有檔案和串流操作的類別。File 是最常用的靜態類別。" },
                new() { Comment="// 寫入文字檔（會覆蓋）", Code="File.WriteAllText(\"test.txt\", \"Hello C#\");", Explanation="💡 WriteAllText 一次寫入整個字串。如果檔案不存在會自動建立，存在則覆蓋。" },
                new() { Comment="// 附加寫入", Code="File.AppendAllText(\"test.txt\", \"\\n第二行\");", Explanation="💡 AppendAllText 在檔案尾端新增內容，不會覆蓋原有的。\\n 是換行字元。" },
                new() { Comment="// 讀取全部文字", Code="string content = File.ReadAllText(\"test.txt\");", Explanation="💡 ReadAllText 一次讀取整個檔案到記憶體。適合小檔案，大檔案建議用 StreamReader 逐行讀取。" },
                new() { Comment="// 讀取所有行", Code="string[] lines = File.ReadAllLines(\"test.txt\");", Explanation="💡 ReadAllLines 把每一行存成字串陣列。方便逐行處理。" },
                new() { Comment="// 檢查檔案是否存在", Code="if (File.Exists(\"test.txt\")) {", Explanation="💡 操作檔案前先檢查是否存在，避免 FileNotFoundException。" },
                new() { Code="    Console.WriteLine(\"檔案存在\");" },
                new() { Code="}" },
            }
        },
        new() { Id=111, Category="csharp", Title="Nullable 與空值處理", Difficulty="intermediate", Description="安全處理 null 值",
            Lines = new() {
                new() { Comment="// 可空值型別", Code="int? age = null;", Explanation="💡 int? 等同於 Nullable<int>，允許 int 變數存放 null。一般 int 不能是 null。" },
                new() { Comment="// 檢查是否有值", Code="if (age.HasValue) {", Explanation="💡 HasValue 是 bool 屬性。也可以用 age != null 或 age is not null。" },
                new() { Code="    Console.WriteLine(age.Value);" },
                new() { Code="}" },
                new() { Comment="// null 合併運算子", Code="int result = age ?? 0;", Explanation="💡 ?? 在左邊是 null 時使用右邊的值。常用來提供預設值。" },
                new() { Comment="// null 條件運算子", Code="string? name = null;", Explanation="💡 string? 表示可能是 null 的字串。啟用 nullable reference types 後，不加 ? 的 string 不應該是 null。" },
                new() { Code="int? len = name?.Length;", Explanation="💡 ?. 在 name 是 null 時直接回傳 null，不會拋出 NullReferenceException。整個表達式的型別變成 int?。" },
                new() { Comment="// null 合併指派", Code="name ??= \"預設名稱\";", Explanation="💡 ??= 只在變數是 null 時才賦值。等同於 if (name == null) name = \"預設名稱\"。" },
            }
        },
        new() { Id=112, Category="csharp", Title="Enum 列舉", Difficulty="beginner", Description="定義具名常數集合",
            Lines = new() {
                new() { Comment="// 定義列舉", Code="enum Season { Spring, Summer, Autumn, Winter }", Explanation="💡 enum 把一組相關的常數用有意義的名稱表示。預設值從 0 開始遞增。" },
                new() { Comment="// 使用列舉", Code="Season now = Season.Summer;", Explanation="💡 用列舉型別宣告變數，值只能是 enum 裡定義的成員，型別安全。" },
                new() { Comment="// 列舉搭配 switch", Code="string msg = now switch {", Explanation="💡 enum 非常適合搭配 switch。列舉的每個值都是明確的、有限的。" },
                new() { Code="    Season.Spring => \"春暖花開\"," },
                new() { Code="    Season.Summer => \"夏日炎炎\"," },
                new() { Code="    Season.Autumn => \"秋高氣爽\"," },
                new() { Code="    Season.Winter => \"冬日暖陽\"," },
                new() { Code="    _ => \"未知\"" },
                new() { Code="};" },
                new() { Comment="// 列舉轉字串", Code="Console.WriteLine(now.ToString());", Explanation="💡 ToString() 回傳列舉成員名稱 \"Summer\"。也可以用 (int)now 取得數值。" },
            }
        },
    };

    // ═══════════════════════════════════════════
    //  JavaScript 擴充（ID 201-220）
    // ═══════════════════════════════════════════
    static List<CodeLesson> JavaScriptLessons() => new()
    {
        new() { Id=201, Category="javascript", Title="變數與型態", Difficulty="beginner", Description="let、const 與資料型態",
            Lines = new() {
                new() { Comment="// let 宣告可變的變數", Code="let age = 25;", Explanation="💡 let 是 ES6 新增的宣告方式，有區塊作用域（block scope）。比 var 更安全。" },
                new() { Comment="// const 宣告不可變的常數", Code="const PI = 3.14159;", Explanation="💡 const 宣告後不能重新賦值。但如果是物件或陣列，內容還是可以修改。" },
                new() { Comment="// 字串", Code="let name = \"小明\";", Explanation="💡 字串可用單引號 '、雙引號 \" 或反引號 ` 包住。反引號支持模板字串。" },
                new() { Comment="// 模板字串", Code="let greeting = `你好，${name}，今年 ${age} 歲`;", Explanation="💡 反引號 ` 搭配 ${} 可以嵌入變數和表達式。比字串串接 + 更易讀。" },
                new() { Comment="// 布林值", Code="let isStudent = true;", Explanation="💡 布林值只有 true 和 false。注意 JS 的 falsy 值：0, '', null, undefined, NaN 都會被當作 false。" },
                new() { Comment="// typeof 檢查型態", Code="console.log(typeof age);", Explanation="💡 typeof 回傳型態字串：'number'、'string'、'boolean'、'object'、'undefined'、'function'。" },
                new() { Comment="// 嚴格相等", Code="console.log(25 === '25');", Explanation="💡 === 嚴格相等，不會轉型（結果 false）。== 寬鬆相等會自動轉型（結果 true）。永遠用 === ！" },
            }
        },
        new() { Id=202, Category="javascript", Title="箭頭函式", Difficulty="beginner", Description="Arrow Function 語法",
            Lines = new() {
                new() { Comment="// 傳統函式", Code="function add(a, b) {", Explanation="💡 function 關鍵字定義函式。函式是 JS 的一等公民，可以當作變數傳遞。" },
                new() { Code="    return a + b;" },
                new() { Code="}" },
                new() { Comment="// 箭頭函式（完整寫法）", Code="const add2 = (a, b) => {", Explanation="💡 箭頭函式用 => 取代 function 關鍵字。它沒有自己的 this，會繼承外層的 this。" },
                new() { Code="    return a + b;" },
                new() { Code="};", Explanation="💡 箭頭函式賦值給 const，確保函式不會被意外覆蓋。" },
                new() { Comment="// 箭頭函式（簡寫：一行可省略 return）", Code="const add3 = (a, b) => a + b;", Explanation="💡 只有一個表達式時可以省略 {} 和 return。隱式回傳該表達式的值。" },
                new() { Comment="// 只有一個參數可省略括號", Code="const double = n => n * 2;", Explanation="💡 只有一個參數時括號可省。零個或多個參數必須加括號。" },
                new() { Comment="// 回傳物件要加括號", Code="const makeUser = (name) => ({ name, age: 0 });", Explanation="💡 回傳物件字面值時要用 () 包住，否則 {} 會被當作函式 body。" },
            }
        },
        new() { Id=203, Category="javascript", Title="陣列方法 map/filter/reduce", Difficulty="intermediate", Description="函式式陣列處理",
            Lines = new() {
                new() { Comment="// 原始資料", Code="const nums = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];", Explanation="💡 JS 陣列用 [] 建立，可以存放任意型態混合的值。" },
                new() { Comment="// map：轉換每個元素", Code="const doubled = nums.map(n => n * 2);", Explanation="💡 map 對每個元素套用函式，回傳新陣列。原陣列不變。[2, 4, 6, ...]" },
                new() { Comment="// filter：過濾元素", Code="const evens = nums.filter(n => n % 2 === 0);", Explanation="💡 filter 保留回傳 true 的元素。[2, 4, 6, 8, 10]。原陣列不變。" },
                new() { Comment="// reduce：累計計算", Code="const sum = nums.reduce((acc, n) => acc + n, 0);", Explanation="💡 reduce 把陣列「摺疊」成一個值。acc 是累計器，0 是初始值。結果是 55。" },
                new() { Comment="// find：找第一個符合的", Code="const found = nums.find(n => n > 5);", Explanation="💡 find 回傳第一個讓函式回傳 true 的元素。找不到回傳 undefined。結果是 6。" },
                new() { Comment="// some / every：檢查條件", Code="const hasNeg = nums.some(n => n < 0);", Explanation="💡 some 有任何一個符合就 true。every 全部都要符合才 true。" },
                new() { Comment="// 鏈式呼叫", Code="const result = nums.filter(n => n > 3).map(n => n * 10).reduce((a, b) => a + b, 0);", Explanation="💡 鏈式呼叫：先 filter → 再 map → 最後 reduce。每步都回傳新陣列/值，可以一直串下去。" },
            }
        },
        new() { Id=204, Category="javascript", Title="物件解構與展開", Difficulty="intermediate", Description="Destructuring 與 Spread",
            Lines = new() {
                new() { Comment="// 物件解構", Code="const { name, age, email = 'N/A' } = { name: '小明', age: 20 };", Explanation="💡 解構從物件中取出屬性到同名變數。email = 'N/A' 是預設值，物件沒有 email 時用。" },
                new() { Comment="// 重新命名", Code="const { name: userName } = { name: '小華' };", Explanation="💡 name: userName 表示取出 name 屬性，但存到 userName 變數。" },
                new() { Comment="// 陣列解構", Code="const [first, second, ...rest] = [1, 2, 3, 4, 5];", Explanation="💡 陣列解構按位置取值。...rest 收集剩餘元素成新陣列 [3, 4, 5]。" },
                new() { Comment="// 展開運算子（陣列）", Code="const merged = [...[1, 2], ...[3, 4]];", Explanation="💡 ... 展開陣列中的元素。結果是 [1, 2, 3, 4]，不是巢狀陣列。" },
                new() { Comment="// 展開運算子（物件）", Code="const updated = { ...{ a: 1, b: 2 }, b: 3, c: 4 };", Explanation="💡 後面的屬性會覆蓋前面的。結果 { a:1, b:3, c:4 }。常用於更新 state。" },
                new() { Comment="// 函式參數解構", Code="function greet({ name, age }) {", Explanation="💡 函式參數直接解構，不用在函式內部再取屬性。呼叫時傳入物件。" },
                new() { Code="    console.log(`${name}, ${age}歲`);" },
                new() { Code="}" },
            }
        },
        new() { Id=205, Category="javascript", Title="Promise 基礎", Difficulty="intermediate", Description="建立與使用 Promise",
            Lines = new() {
                new() { Comment="// 建立 Promise", Code="const fetchData = new Promise((resolve, reject) => {", Explanation="💡 Promise 接收一個函式，有兩個參數：resolve（成功）和 reject（失敗）。" },
                new() { Comment="// 模擬非同步操作", Code="    setTimeout(() => {", Explanation="💡 setTimeout 模擬網路請求的延遲。實際開發中，fetch API 會回傳 Promise。" },
                new() { Code="        const success = true;" },
                new() { Code="        if (success) resolve({ id: 1, name: '小明' });", Explanation="💡 resolve 把 Promise 狀態設為 fulfilled（成功），並傳出資料。" },
                new() { Code="        else reject(new Error('取得資料失敗'));", Explanation="💡 reject 把 Promise 狀態設為 rejected（失敗），傳出錯誤。" },
                new() { Code="    }, 1000);" },
                new() { Code="});" },
                new() { Comment="// 使用 Promise", Code="fetchData.then(data => console.log(data)).catch(err => console.error(err));", Explanation="💡 .then 處理成功、.catch 處理失敗。Promise 解決了回呼地獄問題。" },
            }
        },
        new() { Id=206, Category="javascript", Title="async/await", Difficulty="intermediate", Description="更直覺的非同步語法",
            Lines = new() {
                new() { Comment="// async 函式", Code="async function getUser(id) {", Explanation="💡 async 關鍵字讓函式自動回傳 Promise。可以在裡面使用 await。" },
                new() { Comment="// try-catch 處理錯誤", Code="    try {", Explanation="💡 async/await 搭配 try-catch 處理錯誤，比 .then/.catch 更像同步程式碼。" },
                new() { Code="        const res = await fetch(`/api/users/${id}`);", Explanation="💡 await 暫停函式執行，等待 Promise 完成。執行緒不會被阻塞。" },
                new() { Code="        if (!res.ok) throw new Error(`HTTP ${res.status}`);", Explanation="💡 fetch 只在網路錯誤時 reject。HTTP 404、500 不算錯誤，要自己檢查 res.ok。" },
                new() { Code="        const user = await res.json();", Explanation="💡 res.json() 也是非同步的，解析 response body 為 JSON 物件。" },
                new() { Code="        return user;" },
                new() { Code="    } catch (err) {" },
                new() { Code="        console.error('Error:', err.message);" },
                new() { Code="        return null;" },
                new() { Code="    }" },
                new() { Code="}" },
            }
        },
        new() { Id=207, Category="javascript", Title="Class 類別", Difficulty="intermediate", Description="ES6 Class 語法",
            Lines = new() {
                new() { Comment="// 定義類別", Code="class Animal {", Explanation="💡 class 是 ES6 語法糖，底層還是 prototype 繼承。但語法更清楚。" },
                new() { Code="    constructor(name) {", Explanation="💡 constructor 在 new 時自動呼叫。用 this 設定實例屬性。" },
                new() { Code="        this.name = name;" },
                new() { Code="    }" },
                new() { Code="    speak() {", Explanation="💡 方法直接寫在 class body 裡，不需要 function 關鍵字。" },
                new() { Code="        return `${this.name} makes a sound`;" },
                new() { Code="    }" },
                new() { Code="}" },
                new() { Comment="// 繼承", Code="class Dog extends Animal {", Explanation="💡 extends 關鍵字實現繼承。Dog 繼承 Animal 的所有屬性和方法。" },
                new() { Code="    speak() {", Explanation="💡 子類別可以覆寫（override）父類別的方法。" },
                new() { Code="        return `${this.name} barks`;" },
                new() { Code="    }" },
                new() { Code="}" },
            }
        },
        new() { Id=208, Category="javascript", Title="錯誤處理", Difficulty="intermediate", Description="try/catch 與自訂錯誤",
            Lines = new() {
                new() { Comment="// 自訂錯誤類別", Code="class ValidationError extends Error {", Explanation="💡 繼承 Error 建立自訂錯誤類別，讓 catch 可以區分不同類型的錯誤。" },
                new() { Code="    constructor(field, message) {" },
                new() { Code="        super(message);", Explanation="💡 super() 呼叫父類別 Error 的 constructor，設定 message 屬性。" },
                new() { Code="        this.field = field;" },
                new() { Code="        this.name = 'ValidationError';" },
                new() { Code="    }" },
                new() { Code="}" },
                new() { Comment="// 使用自訂錯誤", Code="function validate(age) {" },
                new() { Code="    if (typeof age !== 'number') throw new ValidationError('age', '必須是數字');", Explanation="💡 throw 拋出錯誤，程式會跳到最近的 catch。" },
                new() { Code="    if (age < 0) throw new ValidationError('age', '不能是負數');" },
                new() { Code="}" },
            }
        },
        new() { Id=209, Category="javascript", Title="LocalStorage", Difficulty="beginner", Description="瀏覽器端資料儲存",
            Lines = new() {
                new() { Comment="// 儲存字串", Code="localStorage.setItem('username', '小明');", Explanation="💡 localStorage 存在瀏覽器中，關閉後資料還在。只能存字串。" },
                new() { Comment="// 讀取", Code="const name = localStorage.getItem('username');", Explanation="💡 getItem 回傳字串。如果 key 不存在回傳 null。" },
                new() { Comment="// 儲存物件（需要 JSON）", Code="const user = { name: '小明', age: 20 };", Explanation="💡 localStorage 只能存字串，要存物件必須先序列化成 JSON。" },
                new() { Code="localStorage.setItem('user', JSON.stringify(user));", Explanation="💡 JSON.stringify 把物件轉成 JSON 字串。" },
                new() { Comment="// 讀取物件", Code="const saved = JSON.parse(localStorage.getItem('user'));", Explanation="💡 JSON.parse 把 JSON 字串轉回物件。如果字串不是有效 JSON 會拋出錯誤。" },
                new() { Comment="// 刪除", Code="localStorage.removeItem('username');", Explanation="💡 removeItem 刪除指定 key。清空全部用 localStorage.clear()。" },
            }
        },
        new() { Id=210, Category="javascript", Title="正則表達式", Difficulty="advanced", Description="RegExp 模式匹配",
            Lines = new() {
                new() { Comment="// 建立正則", Code="const emailRegex = /^[\\w.-]+@[\\w.-]+\\.[a-zA-Z]{2,}$/;", Explanation="💡 / / 之間是正則表達式。^ 開頭、$ 結尾、\\w 字母數字、+ 一個以上。" },
                new() { Comment="// 測試是否匹配", Code="console.log(emailRegex.test('test@gmail.com'));", Explanation="💡 test() 回傳 boolean。這是驗證 Email 格式的常見寫法。" },
                new() { Comment="// 手機號碼驗證", Code="const phoneRegex = /^09\\d{8}$/;", Explanation="💡 \\d 代表數字，{8} 代表剛好 8 個。整體匹配 09 開頭的 10 位數字。" },
                new() { Comment="// match 找出匹配", Code="const matches = 'Hello 123 World 456'.match(/\\d+/g);", Explanation="💡 match 回傳所有匹配的陣列。g flag 代表全域搜尋。結果 ['123', '456']。" },
                new() { Comment="// replace 取代", Code="const cleaned = 'Hello   World'.replace(/\\s+/g, ' ');", Explanation="💡 \\s+ 匹配一個或多個空白。替換成單一空格，結果 'Hello World'。" },
                new() { Comment="// 捕獲組", Code="const [, year, month] = '2024-01-15'.match(/(\\d{4})-(\\d{2})/);", Explanation="💡 () 是捕獲組。match 回傳 [完整匹配, 組1, 組2...]。用解構取出年和月。" },
            }
        },
        new() { Id=211, Category="javascript", Title="模組 import/export", Difficulty="intermediate", Description="ES Module 模組系統",
            Lines = new() {
                new() { Comment="// 具名匯出（utils.js）", Code="export function add(a, b) { return a + b; }", Explanation="💡 export 讓函式可以被其他檔案 import。一個檔案可以有多個具名匯出。" },
                new() { Comment="// 具名匯出常數", Code="export const PI = 3.14159;", Explanation="💡 常數、類別、函式都可以 export。" },
                new() { Comment="// 預設匯出", Code="export default class Calculator { }", Explanation="💡 default export 每個檔案只能有一個。import 時不需要大括號。" },
                new() { Comment="// 具名匯入", Code="import { add, PI } from './utils.js';", Explanation="💡 {} 內放要匯入的名稱，必須和匯出時的名稱一致。" },
                new() { Comment="// 預設匯入（名字隨便取）", Code="import Calculator from './Calculator.js';", Explanation="💡 default export 匯入時不用 {}，名字可以自訂。" },
                new() { Comment="// 動態匯入（需要時才載入）", Code="const { Chart } = await import('./chart.js');", Explanation="💡 動態 import 回傳 Promise，搭配 await 使用。適合按需載入大型模組。" },
            }
        },
        new() { Id=212, Category="javascript", Title="Map 與 Set", Difficulty="intermediate", Description="ES6 新資料結構",
            Lines = new() {
                new() { Comment="// Map：任何型別都能當 key", Code="const map = new Map();", Explanation="💡 Map 比普通物件更靈活，key 可以是任何型別（物件、函式、數字都行）。" },
                new() { Code="map.set('name', '小明');", Explanation="💡 set 新增鍵值對，get 取值，has 檢查，delete 刪除。" },
                new() { Code="map.set(42, 'the answer');" },
                new() { Code="console.log(map.get(42));", Explanation="💡 取值用 get(key)。注意物件當 key 時，必須是同一個參照。" },
                new() { Comment="// Set：不重複的集合", Code="const set = new Set([1, 2, 2, 3, 3, 3]);", Explanation="💡 Set 自動去重複。結果只有 {1, 2, 3}。" },
                new() { Comment="// 陣列去重複的最快寫法", Code="const unique = [...new Set([1, 1, 2, 2, 3])];", Explanation="💡 先轉 Set 去重，再展開回陣列。一行搞定，非常常用。結果 [1, 2, 3]。" },
                new() { Code="set.add(4);", Explanation="💡 add 新增、has 檢查、delete 刪除、size 數量。" },
            }
        },
        new() { Id=213, Category="javascript", Title="事件委派", Difficulty="intermediate", Description="Event Delegation 模式",
            Lines = new() {
                new() { Comment="// 在父元素監聽事件（事件委派）", Code="document.querySelector('ul').addEventListener('click', (e) => {", Explanation="💡 事件委派：在父元素上監聽，利用事件冒泡。動態新增的子元素也能自動被處理。" },
                new() { Comment="// 檢查是否點到 li", Code="    if (e.target.tagName === 'LI') {", Explanation="💡 e.target 是實際被點擊的元素（可能是子元素）。e.currentTarget 是綁定事件的元素（ul）。" },
                new() { Code="        console.log('Clicked:', e.target.textContent);" },
                new() { Code="    }" },
                new() { Code="});" },
                new() { Comment="// 防抖：停止操作後才執行", Code="function debounce(fn, delay) {", Explanation="💡 搜尋框輸入時，等使用者停止打字 300ms 後才發送請求，避免頻繁呼叫 API。" },
                new() { Code="    let timer;" },
                new() { Code="    return (...args) => {" },
                new() { Code="        clearTimeout(timer);" },
                new() { Code="        timer = setTimeout(() => fn(...args), delay);" },
                new() { Code="    };" },
                new() { Code="}" },
            }
        },
    };

    // ═══════════════════════════════════════════
    //  HTML/CSS 擴充（ID 301-320）
    // ═══════════════════════════════════════════
    static List<CodeLesson> HtmlCssLessons() => new()
    {
        new() { Id=301, Category="html", Title="HTML 表單", Difficulty="beginner", Description="input、select、textarea 元素",
            Lines = new() {
                new() { Comment="<!-- 表單結構 -->", Code="<form action=\"/api/register\" method=\"POST\">", Explanation="💡 form 是表單容器。action 是送出的 URL，method 是 HTTP 方法（GET 或 POST）。" },
                new() { Comment="<!-- 文字輸入 -->", Code="    <label for=\"name\">姓名</label>", Explanation="💡 label 的 for 屬性要對應 input 的 id，點擊 label 可以聚焦 input。" },
                new() { Code="    <input type=\"text\" id=\"name\" name=\"name\" required>", Explanation="💡 type 決定輸入類型，name 是送出時的欄位名，required 表示必填。" },
                new() { Comment="<!-- Email 輸入 -->", Code="    <input type=\"email\" id=\"email\" name=\"email\" placeholder=\"your@email.com\">", Explanation="💡 type=\"email\" 會自動驗證 Email 格式。placeholder 是灰色提示文字。" },
                new() { Comment="<!-- 下拉選單 -->", Code="    <select name=\"city\">", Explanation="💡 select 建立下拉選單。每個 option 是一個選項，value 是送出的值。" },
                new() { Code="        <option value=\"\">請選擇城市</option>" },
                new() { Code="        <option value=\"taipei\">台北</option>" },
                new() { Code="    </select>" },
                new() { Comment="<!-- 送出按鈕 -->", Code="    <button type=\"submit\">送出</button>", Explanation="💡 type=\"submit\" 送出表單，type=\"button\" 是普通按鈕不會送出。" },
                new() { Code="</form>" },
            }
        },
        new() { Id=302, Category="html", Title="HTML 語意標籤", Difficulty="beginner", Description="header、nav、main、footer",
            Lines = new() {
                new() { Comment="<!-- 語意化的頁面結構 -->", Code="<header>", Explanation="💡 header 是頁首區塊，通常放 Logo、導覽列。比 <div class=\"header\"> 更有語意。" },
                new() { Code="    <nav>", Explanation="💡 nav 專門放導覽連結。螢幕閱讀器和搜尋引擎能理解這是導覽區。" },
                new() { Code="        <a href=\"/\">首頁</a>" },
                new() { Code="        <a href=\"/about\">關於</a>" },
                new() { Code="    </nav>" },
                new() { Code="</header>" },
                new() { Code="<main>", Explanation="💡 main 包含頁面的主要內容，一頁只能有一個 main。搜尋引擎會優先關注 main 裡的內容。" },
                new() { Code="    <article>", Explanation="💡 article 是獨立的內容區塊（文章、貼文）。可以被單獨分享或引用。" },
                new() { Code="        <h1>文章標題</h1>" },
                new() { Code="    </article>" },
                new() { Code="    <aside>側邊欄</aside>", Explanation="💡 aside 放附帶內容，如側邊欄、廣告、相關連結。" },
                new() { Code="</main>" },
                new() { Code="<footer>&copy; 2024 DevLearn</footer>", Explanation="💡 footer 是頁尾，放版權、聯絡方式。&copy; 是 © 的 HTML 實體。" },
            }
        },
        new() { Id=303, Category="html", Title="CSS Flexbox", Difficulty="beginner", Description="彈性盒子排版",
            Lines = new() {
                new() { Comment="/* 啟用 Flex 排版 */", Code=".container { display: flex; }", Explanation="💡 display: flex 讓子元素自動水平排列。這是現代 CSS 排版最常用的方式。" },
                new() { Comment="/* 主軸對齊 */", Code=".container { justify-content: space-between; }", Explanation="💡 justify-content 控制主軸（水平）對齊。space-between 讓元素分散在兩端。" },
                new() { Comment="/* 交叉軸對齊 */", Code=".container { align-items: center; }", Explanation="💡 align-items 控制交叉軸（垂直）對齊。center 讓所有子元素垂直置中。" },
                new() { Comment="/* 水平垂直完美置中 */", Code=".center { display: flex; justify-content: center; align-items: center; height: 100vh; }", Explanation="💡 這三行是 CSS 最經典的置中技巧。100vh = 視窗高度 100%。" },
                new() { Comment="/* 換行 */", Code=".container { flex-wrap: wrap; gap: 16px; }", Explanation="💡 flex-wrap: wrap 允許換行。gap 設定元素間距，比用 margin 更方便。" },
                new() { Comment="/* 子元素等分 */", Code=".item { flex: 1; }", Explanation="💡 flex: 1 讓所有子元素等分剩餘空間。flex: 0 0 200px 表示固定 200px 不伸縮。" },
                new() { Comment="/* 垂直排列 */", Code=".column { display: flex; flex-direction: column; }", Explanation="💡 flex-direction: column 改為垂直排列。預設是 row（水平）。" },
            }
        },
        new() { Id=304, Category="html", Title="CSS Grid", Difficulty="intermediate", Description="格線排版系統",
            Lines = new() {
                new() { Comment="/* 基本 Grid */", Code=".grid { display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 16px; }", Explanation="💡 grid-template-columns 定義欄數。1fr 1fr 1fr = 三等分。fr 是比例單位。" },
                new() { Comment="/* repeat 簡寫 */", Code=".grid { grid-template-columns: repeat(3, 1fr); }", Explanation="💡 repeat(3, 1fr) 等同於 1fr 1fr 1fr。更簡潔。" },
                new() { Comment="/* 自動響應式 */", Code=".grid { grid-template-columns: repeat(auto-fill, minmax(250px, 1fr)); }", Explanation="💡 auto-fill + minmax 是響應式神器！每欄最小 250px，自動計算能放幾欄。" },
                new() { Comment="/* 跨欄 */", Code=".header { grid-column: 1 / -1; }", Explanation="💡 1 / -1 從第一條線到最後一條線 = 佔滿整列。也可以用 span 2 跨兩欄。" },
                new() { Comment="/* Grid Areas 命名排版 */", Code=".page { grid-template-areas: 'header header' 'sidebar main' 'footer footer'; }", Explanation="💡 template-areas 用字串畫出版面。子元素用 grid-area: header 指定位置。最直覺的排版方式。" },
                new() { Comment="/* 格子內容置中 */", Code=".grid { place-items: center; }", Explanation="💡 place-items 是 align-items + justify-items 的簡寫。center 讓格子內容水平垂直置中。" },
            }
        },
        new() { Id=305, Category="html", Title="CSS 響應式設計", Difficulty="intermediate", Description="Media Query 與 RWD",
            Lines = new() {
                new() { Comment="/* 手機優先：基本樣式 */", Code=".container { width: 100%; padding: 0 16px; }", Explanation="💡 Mobile First 策略：先寫手機版，再用 min-width 往上加。" },
                new() { Comment="/* 平板以上 */", Code="@media (min-width: 768px) { .container { max-width: 720px; margin: 0 auto; } }", Explanation="💡 @media 媒體查詢。min-width: 768px 表示螢幕寬度 >= 768px 時才套用。" },
                new() { Comment="/* 桌面以上 */", Code="@media (min-width: 1024px) { .container { max-width: 1200px; } }", Explanation="💡 常見斷點：768px（平板）、1024px（桌面）、1440px（大螢幕）。" },
                new() { Comment="/* 響應式字體 */", Code="h1 { font-size: clamp(24px, 5vw, 48px); }", Explanation="💡 clamp(最小, 理想, 最大) 讓字體在範圍內自適應。vw 是視窗寬度百分比。" },
                new() { Comment="/* 響應式圖片 */", Code="img { max-width: 100%; height: auto; }", Explanation="💡 圖片不超過容器寬度，高度自動按比例縮放。這是 RWD 的基本設定。" },
                new() { Comment="/* 手機隱藏側邊欄 */", Code="@media (max-width: 767px) { .sidebar { display: none; } }", Explanation="💡 max-width 反過來，表示小於 768px 時隱藏。配合漢堡選單使用。" },
            }
        },
        new() { Id=306, Category="html", Title="CSS 動畫", Difficulty="intermediate", Description="transition 與 @keyframes",
            Lines = new() {
                new() { Comment="/* transition：狀態轉換動畫 */", Code=".btn { transition: all 0.3s ease; }", Explanation="💡 transition 讓 CSS 屬性改變時有動畫效果。all = 所有屬性，0.3s = 持續時間，ease = 緩動函式。" },
                new() { Code=".btn:hover { transform: scale(1.05); background: #58a6ff; }", Explanation="💡 hover 時放大 1.05 倍並變色。搭配 transition 會有平滑的過渡效果。" },
                new() { Comment="/* @keyframes：自訂動畫 */", Code="@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }", Explanation="💡 @keyframes 定義動畫關鍵幀。from/to 是 0%/100% 的簡寫。" },
                new() { Code=".element { animation: fadeIn 0.5s ease forwards; }", Explanation="💡 animation 套用 keyframes。forwards 表示動畫結束後保持最終狀態。" },
                new() { Comment="/* 旋轉動畫 */", Code="@keyframes spin { to { transform: rotate(360deg); } }", Explanation="💡 只寫 to 的話 from 就是元素的初始狀態。rotate(360deg) 轉一圈。" },
                new() { Code=".loading { animation: spin 1s linear infinite; }", Explanation="💡 linear = 等速，infinite = 無限循環。常用於 loading 動畫。" },
            }
        },
        new() { Id=307, Category="html", Title="HTML 表格", Difficulty="beginner", Description="thead、tbody、合併儲存格",
            Lines = new() {
                new() { Code="<table>", Explanation="💡 table 是表格容器。表格用於展示結構化資料，不要用來排版！" },
                new() { Code="    <thead>", Explanation="💡 thead 包含表頭列。瀏覽器和螢幕閱讀器能區分表頭和資料。" },
                new() { Code="        <tr><th>姓名</th><th>分數</th></tr>", Explanation="💡 th 是表頭儲存格（header cell），預設粗體置中。tr 是表格列（row）。" },
                new() { Code="    </thead>" },
                new() { Code="    <tbody>", Explanation="💡 tbody 包含資料列。即使不寫，瀏覽器也會自動包一個 tbody。" },
                new() { Code="        <tr><td>小明</td><td>95</td></tr>", Explanation="💡 td 是資料儲存格（data cell）。" },
                new() { Code="        <tr><td colspan=\"2\">共 1 筆</td></tr>", Explanation="💡 colspan=\"2\" 讓儲存格橫跨 2 欄。rowspan 則是跨列。" },
                new() { Code="    </tbody>" },
                new() { Code="</table>" },
            }
        },
        new() { Id=308, Category="html", Title="Meta 與 SEO", Difficulty="intermediate", Description="meta 標籤與社群分享",
            Lines = new() {
                new() { Code="<meta charset=\"UTF-8\">", Explanation="💡 設定字元編碼為 UTF-8，支援中文和各種語言。必放在 head 最前面。" },
                new() { Code="<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">", Explanation="💡 手機必備！沒有這行，手機會用桌面寬度顯示，畫面會很小。" },
                new() { Code="<title>DevLearn - 免費學程式</title>", Explanation="💡 title 是最重要的 SEO 元素。搜尋結果會顯示這個標題。建議 50-60 字元。" },
                new() { Code="<meta name=\"description\" content=\"免費學習 C#、SQL、JavaScript 程式設計\">", Explanation="💡 description 是搜尋結果下方的描述文字。120-160 字元最佳。" },
                new() { Comment="<!-- Open Graph：社群分享 -->", Code="<meta property=\"og:title\" content=\"DevLearn\">", Explanation="💡 og: 開頭的是 Open Graph 標籤。Facebook、LINE、Discord 分享連結時會顯示這些資訊。" },
                new() { Code="<meta property=\"og:image\" content=\"https://devlearn.com/og-image.jpg\">", Explanation="💡 og:image 是分享時顯示的預覽圖。建議 1200x630px。" },
            }
        },
        new() { Id=309, Category="html", Title="CSS 變數與主題", Difficulty="intermediate", Description="CSS Custom Properties",
            Lines = new() {
                new() { Comment="/* 在根元素定義變數 */", Code=":root { --primary: #58a6ff; --bg: #0d1117; --text: #e6edf3; }", Explanation="💡 CSS 變數用 -- 開頭定義在 :root 上。所有子元素都能使用。" },
                new() { Comment="/* 使用變數 */", Code=".btn { background: var(--primary); color: var(--text); }", Explanation="💡 var(--name) 使用變數。可以加第二個參數作為後備值：var(--primary, blue)。" },
                new() { Comment="/* 深色主題 */", Code="[data-theme='dark'] { --bg: #0d1117; --text: #e6edf3; }", Explanation="💡 用 data 屬性切換主題。JS 只需要改 document.documentElement.dataset.theme = 'dark'。" },
                new() { Comment="/* 淺色主題 */", Code="[data-theme='light'] { --bg: #ffffff; --text: #24292f; }", Explanation="💡 只改變數值，所有使用變數的樣式自動更新。這就是 CSS 變數的威力。" },
                new() { Code="body { background: var(--bg); color: var(--text); transition: all 0.3s; }", Explanation="💡 加 transition 讓主題切換有平滑過渡動畫。" },
            }
        },
        new() { Id=310, Category="html", Title="連結與圖片", Difficulty="beginner", Description="a、img、figure 標籤",
            Lines = new() {
                new() { Comment="<!-- 外部連結 -->", Code="<a href=\"https://google.com\" target=\"_blank\" rel=\"noopener\">Google</a>", Explanation="💡 target=\"_blank\" 新分頁開啟。rel=\"noopener\" 防止安全漏洞（新頁面無法存取原頁面）。" },
                new() { Comment="<!-- 圖片 -->", Code="<img src=\"photo.jpg\" alt=\"風景照\" width=\"400\" loading=\"lazy\">", Explanation="💡 alt 是替代文字（SEO + 無障礙必備）。loading=\"lazy\" 延遲載入，頁面更快。" },
                new() { Comment="<!-- 響應式圖片 -->", Code="<picture>", Explanation="💡 picture 元素讓瀏覽器根據條件選擇最適合的圖片。" },
                new() { Code="    <source media=\"(max-width: 768px)\" srcset=\"small.jpg\">", Explanation="💡 手機載入小圖，省流量。media 條件和 CSS @media 一樣。" },
                new() { Code="    <img src=\"large.jpg\" alt=\"響應式圖片\">", Explanation="💡 img 是後備，不支援 picture 的瀏覽器會用 img。" },
                new() { Code="</picture>" },
                new() { Comment="<!-- 圖片 + 說明 -->", Code="<figure><img src=\"chart.png\" alt=\"圖表\"><figcaption>圖 1</figcaption></figure>", Explanation="💡 figure 把圖片和標題綁在一起。語意上比 div + p 更好。" },
            }
        },
    };

    // ═══════════════════════════════════════════
    //  SQL 擴充（ID 401-420）
    // ═══════════════════════════════════════════
    static List<CodeLesson> SqlLessons() => new()
    {
        new() { Id=401, Category="sql", Title="SELECT 基礎", Difficulty="beginner", Description="查詢特定欄位與別名",
            Lines = new() {
                new() { Comment="-- 查詢所有欄位", Code="SELECT * FROM students;", Explanation="💡 * 代表所有欄位。FROM 指定表格。分號結尾。" },
                new() { Comment="-- 查詢特定欄位", Code="SELECT name, age FROM students;", Explanation="💡 只選需要的欄位，效能比 SELECT * 好。" },
                new() { Comment="-- 欄位別名", Code="SELECT name AS 姓名, age AS 年齡 FROM students;", Explanation="💡 AS 給欄位取別名，方便閱讀。只影響顯示，不影響資料庫。" },
                new() { Comment="-- 去除重複", Code="SELECT DISTINCT city FROM students;", Explanation="💡 DISTINCT 去除重複值。如果有 3 個台北，只顯示 1 個。" },
                new() { Comment="-- 計算欄位", Code="SELECT name, age, age + 1 AS 明年年齡 FROM students;", Explanation="💡 SELECT 裡可以做運算，結果成為新欄位。" },
                new() { Comment="-- LIMIT 取前 N 筆", Code="SELECT * FROM students ORDER BY age DESC LIMIT 5;", Explanation="💡 ORDER BY DESC 降冪排序，LIMIT 5 只取前 5 筆。常用於排行榜。" },
            }
        },
        new() { Id=402, Category="sql", Title="WHERE 條件篩選", Difficulty="beginner", Description="比較、邏輯、LIKE、IN",
            Lines = new() {
                new() { Comment="-- 等於", Code="SELECT * FROM students WHERE age = 20;", Explanation="💡 WHERE 過濾資料。SQL 用 = 而不是 ==。" },
                new() { Comment="-- AND 組合條件", Code="SELECT * FROM students WHERE age >= 20 AND city = '台北';", Explanation="💡 AND 兩個條件都要成立。OR 則是任一成立即可。" },
                new() { Comment="-- IN 列表查詢", Code="SELECT * FROM students WHERE city IN ('台北', '高雄', '台中');", Explanation="💡 IN 比寫一堆 OR 更簡潔。等同 city='台北' OR city='高雄' OR city='台中'。" },
                new() { Comment="-- BETWEEN 範圍", Code="SELECT * FROM students WHERE age BETWEEN 18 AND 25;", Explanation="💡 BETWEEN 包含頭尾。等同 age >= 18 AND age <= 25。" },
                new() { Comment="-- LIKE 模糊比對", Code="SELECT * FROM students WHERE name LIKE '小%';", Explanation="💡 % 代表任意字元。'小%' = 小開頭。'%明' = 明結尾。'%中%' = 包含中。" },
                new() { Comment="-- IS NULL", Code="SELECT * FROM students WHERE email IS NULL;", Explanation="💡 檢查空值要用 IS NULL，不能用 = NULL！這是 SQL 新手最常犯的錯。" },
            }
        },
        new() { Id=403, Category="sql", Title="聚合函數", Difficulty="beginner", Description="COUNT、SUM、AVG、MAX、MIN",
            Lines = new() {
                new() { Comment="-- 計算總人數", Code="SELECT COUNT(*) AS total FROM students;", Explanation="💡 COUNT(*) 計算所有列數。COUNT(email) 不計算 NULL。" },
                new() { Comment="-- 平均分數", Code="SELECT ROUND(AVG(score), 2) AS avg_score FROM students;", Explanation="💡 AVG 計算平均值。ROUND 四捨五入到小數 2 位。" },
                new() { Comment="-- 最高最低分", Code="SELECT MAX(score) AS highest, MIN(score) AS lowest FROM students;", Explanation="💡 MAX/MIN 也可以用在日期和字串（按排序順序）。" },
                new() { Comment="-- 分組統計", Code="SELECT city, COUNT(*) AS cnt, AVG(score) AS avg FROM students GROUP BY city;", Explanation="💡 GROUP BY 把相同值的列歸為一組，每組算一次聚合函數。" },
                new() { Comment="-- HAVING 過濾分組", Code="SELECT city, COUNT(*) AS cnt FROM students GROUP BY city HAVING COUNT(*) > 5;", Explanation="💡 HAVING 在 GROUP BY 之後過濾。WHERE 過濾列，HAVING 過濾組。" },
                new() { Comment="-- 排序結果", Code="SELECT city, AVG(score) AS avg FROM students GROUP BY city ORDER BY avg DESC;", Explanation="💡 SQL 執行順序：FROM → WHERE → GROUP BY → HAVING → SELECT → ORDER BY → LIMIT。" },
            }
        },
        new() { Id=404, Category="sql", Title="多表 JOIN", Difficulty="intermediate", Description="INNER/LEFT/RIGHT JOIN",
            Lines = new() {
                new() { Comment="-- INNER JOIN：只取有配對的", Code="SELECT s.name, c.course_name FROM students s INNER JOIN enrollments e ON s.id = e.student_id INNER JOIN courses c ON e.course_id = c.id;", Explanation="💡 INNER JOIN 只回傳兩邊都有對應的資料。用別名 s, e, c 讓語法更簡潔。" },
                new() { Comment="-- LEFT JOIN：左邊全部保留", Code="SELECT s.name, e.course_id FROM students s LEFT JOIN enrollments e ON s.id = e.student_id;", Explanation="💡 LEFT JOIN 保留左表所有資料。右表沒配對的欄位填 NULL。適合找「沒有選課的學生」。" },
                new() { Comment="-- 找出沒有選課的學生", Code="SELECT s.name FROM students s LEFT JOIN enrollments e ON s.id = e.student_id WHERE e.id IS NULL;", Explanation="💡 LEFT JOIN + WHERE NULL 是找「缺少關聯」的經典技巧。" },
                new() { Comment="-- JOIN 搭配聚合", Code="SELECT s.name, COUNT(e.id) AS courses FROM students s LEFT JOIN enrollments e ON s.id = e.student_id GROUP BY s.name;", Explanation="💡 用 COUNT(e.id) 而不是 COUNT(*)，因為 NULL 不會被計算。" },
                new() { Comment="-- 自連接", Code="SELECT e.name AS employee, m.name AS manager FROM employees e INNER JOIN employees m ON e.manager_id = m.id;", Explanation="💡 同一張表 JOIN 自己！常用於階層結構（員工→主管、留言→回覆）。" },
            }
        },
        new() { Id=405, Category="sql", Title="子查詢與 CTE", Difficulty="intermediate", Description="Subquery 與 WITH",
            Lines = new() {
                new() { Comment="-- WHERE 子查詢", Code="SELECT * FROM students WHERE score > (SELECT AVG(score) FROM students);", Explanation="💡 子查詢先算出平均分，外層再用這個值做比較。像俄羅斯娃娃一樣由內到外執行。" },
                new() { Comment="-- IN 子查詢", Code="SELECT * FROM students WHERE id IN (SELECT student_id FROM enrollments WHERE course_id = 1);", Explanation="💡 IN 搭配子查詢，找出「有選課程 1 的學生」。" },
                new() { Comment="-- EXISTS 檢查存在", Code="SELECT * FROM students s WHERE EXISTS (SELECT 1 FROM enrollments e WHERE e.student_id = s.id);", Explanation="💡 EXISTS 只關心有沒有結果，不在乎是什麼。效能通常比 IN 好。" },
                new() { Comment="-- CTE 通用表達式", Code="WITH top_students AS (SELECT * FROM students WHERE score >= 90)", Explanation="💡 WITH AS 定義臨時結果集。比巢狀子查詢更好讀，而且可以被多次引用。" },
                new() { Code="SELECT * FROM top_students ORDER BY score DESC;", Explanation="💡 CTE 像是把複雜查詢「分段」。每段有名字，主查詢引用名字。" },
            }
        },
        new() { Id=406, Category="sql", Title="INSERT/UPDATE/DELETE", Difficulty="beginner", Description="資料新增、修改、刪除",
            Lines = new() {
                new() { Comment="-- 新增一筆", Code="INSERT INTO students (name, age, city) VALUES ('小明', 20, '台北');", Explanation="💡 指定欄位和對應的值。字串用單引號。Id 通常是自動遞增不需要填。" },
                new() { Comment="-- 新增多筆", Code="INSERT INTO students (name, age) VALUES ('小華', 22), ('小美', 21);", Explanation="💡 多筆 INSERT 用逗號分隔每組 VALUES。比一筆一筆插入快很多。" },
                new() { Comment="-- 修改資料（⚠️ 一定要加 WHERE）", Code="UPDATE students SET age = 21, city = '高雄' WHERE name = '小明';", Explanation="💡 忘了加 WHERE 會改掉全部資料！好習慣：先用 SELECT 確認條件對不對。" },
                new() { Comment="-- 刪除資料（⚠️ 一定要加 WHERE）", Code="DELETE FROM students WHERE age < 18;", Explanation="💡 忘了加 WHERE 會刪光全部！建議用 Transaction 包起來，確認後再 COMMIT。" },
                new() { Comment="-- RETURNING 取回結果（PostgreSQL）", Code="INSERT INTO students (name, age) VALUES ('小新', 19) RETURNING id;", Explanation="💡 RETURNING 回傳新增/修改/刪除的資料。不用再 SELECT 一次。PostgreSQL 專用。" },
            }
        },
        new() { Id=407, Category="sql", Title="CREATE TABLE 與約束", Difficulty="intermediate", Description="建表與 PRIMARY KEY、FOREIGN KEY",
            Lines = new() {
                new() { Comment="-- 建立表格", Code="CREATE TABLE orders (", Explanation="💡 CREATE TABLE 定義表格結構。每個欄位要指定名稱和型態。" },
                new() { Code="    id SERIAL PRIMARY KEY,", Explanation="💡 SERIAL 自動遞增整數。PRIMARY KEY 是唯一識別碼，每筆資料都不同。" },
                new() { Code="    customer_id INT NOT NULL,", Explanation="💡 NOT NULL 表示這欄不能為空。確保每筆訂單都有客戶。" },
                new() { Code="    amount DECIMAL(10,2) CHECK (amount > 0),", Explanation="💡 CHECK 約束確保值符合條件。amount > 0 表示金額必須是正數。" },
                new() { Code="    status VARCHAR(20) DEFAULT 'pending',", Explanation="💡 DEFAULT 設定預設值。不填的話自動用 'pending'。" },
                new() { Code="    FOREIGN KEY (customer_id) REFERENCES customers(id)", Explanation="💡 FOREIGN KEY 建立表格之間的關聯。確保 customer_id 一定存在於 customers 表。" },
                new() { Code=");" },
            }
        },
        new() { Id=408, Category="sql", Title="Window Functions", Difficulty="advanced", Description="ROW_NUMBER、RANK、LAG",
            Lines = new() {
                new() { Comment="-- ROW_NUMBER：排名（不重複）", Code="SELECT name, score, ROW_NUMBER() OVER (ORDER BY score DESC) AS rank FROM students;", Explanation="💡 OVER() 定義窗口。ROW_NUMBER 給每列一個唯一編號。不會縮減列數（和 GROUP BY 不同）。" },
                new() { Comment="-- RANK：同分同名次", Code="SELECT name, score, RANK() OVER (ORDER BY score DESC) AS rank FROM students;", Explanation="💡 RANK 同分同排名，下一名跳號（1,2,2,4）。DENSE_RANK 不跳號（1,2,2,3）。" },
                new() { Comment="-- 分組排名", Code="SELECT name, city, score, RANK() OVER (PARTITION BY city ORDER BY score DESC) AS city_rank FROM students;", Explanation="💡 PARTITION BY 在每個城市內各自排名。就像 GROUP BY 但保留每一列。" },
                new() { Comment="-- LAG：看前一列", Code="SELECT name, score, LAG(score, 1) OVER (ORDER BY score DESC) AS prev_score FROM students;", Explanation="💡 LAG(欄, N) 取前 N 列的值。LEAD 取後 N 列。常用於計算差距、趨勢。" },
                new() { Comment="-- 累計加總", Code="SELECT order_date, amount, SUM(amount) OVER (ORDER BY order_date) AS running_total FROM orders;", Explanation="💡 SUM 搭配 OVER(ORDER BY) 算累計加總。每列都能看到截至目前的總和。" },
            }
        },
        new() { Id=409, Category="sql", Title="INDEX 索引", Difficulty="intermediate", Description="加速查詢的關鍵",
            Lines = new() {
                new() { Comment="-- 建立索引", Code="CREATE INDEX idx_student_email ON students (email);", Explanation="💡 索引像書的目錄，讓 WHERE 查詢從全表掃描變成快速定位。" },
                new() { Comment="-- 唯一索引（不可重複）", Code="CREATE UNIQUE INDEX idx_email_unique ON students (email);", Explanation="💡 UNIQUE INDEX 確保值不重複。等同於 UNIQUE 約束。" },
                new() { Comment="-- 複合索引", Code="CREATE INDEX idx_city_score ON students (city, score);", Explanation="💡 複合索引遵循「最左前綴原則」。WHERE city = '台北' 能用，WHERE score > 80 不能用。" },
                new() { Comment="-- 查看查詢計畫", Code="EXPLAIN ANALYZE SELECT * FROM students WHERE email = 'test@gmail.com';", Explanation="💡 EXPLAIN ANALYZE 顯示查詢怎麼執行。看到 Seq Scan（全表掃描）就該建索引。" },
                new() { Comment="-- 刪除索引", Code="DROP INDEX idx_student_email;", Explanation="💡 太多索引會拖慢 INSERT/UPDATE 速度。只在常查詢的欄位建索引。" },
            }
        },
        new() { Id=410, Category="sql", Title="Transaction 交易", Difficulty="advanced", Description="ACID 與交易控制",
            Lines = new() {
                new() { Comment="-- 開始交易", Code="BEGIN;", Explanation="💡 BEGIN 開始一個交易。從此開始的所有操作都是「暫定的」，可以 COMMIT 或 ROLLBACK。" },
                new() { Comment="-- A 帳戶扣款", Code="UPDATE accounts SET balance = balance - 1000 WHERE id = 1;", Explanation="💡 轉帳需要兩步：A 扣款 + B 入款。必須全部成功或全部失敗。" },
                new() { Comment="-- B 帳戶入款", Code="UPDATE accounts SET balance = balance + 1000 WHERE id = 2;", Explanation="💡 如果 A 扣成功但 B 入失敗，錢就消失了！Transaction 確保原子性（Atomicity）。" },
                new() { Comment="-- 確認沒問題，提交", Code="COMMIT;", Explanation="💡 COMMIT 把所有變更永久寫入。如果發現問題，用 ROLLBACK 取消全部。" },
                new() { Comment="-- 或取消全部變更", Code="-- ROLLBACK;", Explanation="💡 ROLLBACK 回到 BEGIN 之前的狀態。像是按了「復原」按鈕。" },
            }
        },
    };

    // ═══════════════════════════════════════════
    //  Vue 擴充（ID 501-520）
    // ═══════════════════════════════════════════
    static List<CodeLesson> VueLessons() => new()
    {
        new() { Id=501, Category="vue", Title="Vue 3 基本結構", Difficulty="beginner", Description="createApp 與模板語法",
            Lines = new() {
                new() { Comment="// 建立 Vue App", Code="import { createApp, ref } from 'vue';", Explanation="💡 Vue 3 用 createApp 建立應用。ref 是 Composition API 的響應式工具。" },
                new() { Code="const app = createApp({", Explanation="💡 createApp 接收根組件。setup 函式是 Composition API 的入口。" },
                new() { Code="    setup() {" },
                new() { Comment="// ref 建立響應式資料", Code="        const count = ref(0);", Explanation="💡 ref(0) 建立一個響應式的值。在 JS 中用 count.value，在模板中直接用 count。" },
                new() { Code="        const increment = () => count.value++;", Explanation="💡 修改 ref 要用 .value。Vue 會自動偵測變化並更新畫面。" },
                new() { Code="        return { count, increment };", Explanation="💡 setup 回傳的值可以在模板中使用。" },
                new() { Code="    }," },
                new() { Code="    template: `<button @click=\"increment\">{{ count }}</button>`", Explanation="💡 {{ }} 顯示資料，@click 是 v-on:click 的縮寫。" },
                new() { Code="});" },
                new() { Code="app.mount('#app');", Explanation="💡 mount 把 Vue 掛載到 HTML 的 #app 元素上。" },
            }
        },
        new() { Id=502, Category="vue", Title="computed 與 watch", Difficulty="beginner", Description="計算屬性與監聽器",
            Lines = new() {
                new() { Code="import { ref, computed, watch } from 'vue';", Explanation="💡 computed 是自動計算的值，watch 是資料變化時的副作用。" },
                new() { Comment="// 響應式資料", Code="const firstName = ref('小');", Explanation="💡 ref 可以存放任何型別的值。" },
                new() { Code="const lastName = ref('明');" },
                new() { Comment="// computed：自動計算", Code="const fullName = computed(() => firstName.value + lastName.value);", Explanation="💡 computed 會快取結果。只有依賴的 ref 改變時才重新計算。比在模板中寫表達式更高效。" },
                new() { Comment="// watch：監聽變化", Code="watch(fullName, (newVal, oldVal) => {", Explanation="💡 watch 在值改變時執行副作用（API 呼叫、localStorage 儲存等）。" },
                new() { Code="    console.log(`名字從 ${oldVal} 變成 ${newVal}`);" },
                new() { Code="});" },
                new() { Comment="// watchEffect：立即執行並自動追蹤", Code="watchEffect(() => console.log(`目前：${fullName.value}`));", Explanation="💡 watchEffect 不需要指定監聽對象，它會自動追蹤回呼函式中用到的所有響應式值。" },
            }
        },
        new() { Id=503, Category="vue", Title="v-if / v-for 指令", Difficulty="beginner", Description="條件渲染與列表",
            Lines = new() {
                new() { Comment="// 條件渲染", Code="<div v-if=\"score >= 90\">優秀</div>", Explanation="💡 v-if 條件為 false 時元素完全不存在於 DOM。適合不常切換的情況。" },
                new() { Code="<div v-else-if=\"score >= 60\">及格</div>", Explanation="💡 v-else-if 和 v-else 必須緊接在 v-if 後面（中間不能有其他元素）。" },
                new() { Code="<div v-else>不及格</div>" },
                new() { Comment="// v-show：用 CSS 控制顯隱", Code="<div v-show=\"isVisible\">切換顯示</div>", Explanation="💡 v-show 只切換 display: none。元素永遠在 DOM 中。適合頻繁切換的情況。" },
                new() { Comment="// 列表渲染", Code="<ul><li v-for=\"item in items\" :key=\"item.id\">{{ item.name }}</li></ul>", Explanation="💡 v-for 遍歷陣列。:key 是必要的，幫助 Vue 追蹤每個元素的身份，提升渲染效能。" },
                new() { Comment="// 帶索引的 v-for", Code="<li v-for=\"(item, index) in items\" :key=\"item.id\">{{ index }}: {{ item.name }}</li>", Explanation="💡 第二個參數是索引。也可以遍歷物件：v-for=\"(value, key) in obj\"。" },
            }
        },
        new() { Id=504, Category="vue", Title="組件通訊 Props/Emit", Difficulty="intermediate", Description="父子組件資料傳遞",
            Lines = new() {
                new() { Comment="// 子組件：接收 props", Code="const ChildComponent = {", Explanation="💡 組件是 Vue 的核心。把 UI 拆成獨立、可重用的小塊。" },
                new() { Code="    props: { title: String, count: { type: Number, default: 0 } },", Explanation="💡 props 定義組件接收的屬性。可以指定型別和預設值。Props 是唯讀的（單向資料流）。" },
                new() { Code="    emits: ['update'],", Explanation="💡 emits 宣告組件會發出的事件。讓父組件知道可以監聽什麼。" },
                new() { Code="    setup(props, { emit }) {", Explanation="💡 setup 的第二個參數是 context，包含 emit、attrs、slots。" },
                new() { Code="        const onClick = () => emit('update', props.count + 1);", Explanation="💡 emit('事件名', 資料) 向父組件發出事件。子組件不直接修改 props。" },
                new() { Code="        return { onClick };" },
                new() { Code="    }," },
                new() { Code="    template: `<button @click=\"onClick\">{{ title }}: {{ count }}</button>`" },
                new() { Code="};" },
                new() { Comment="// 父組件使用子組件", Code="<ChildComponent title=\"按鈕\" :count=\"num\" @update=\"num = $event\" />", Explanation="💡 :count 綁定資料，@update 監聽事件。$event 是 emit 傳出的值。" },
            }
        },
        new() { Id=505, Category="vue", Title="Pinia 狀態管理", Difficulty="intermediate", Description="Vue 3 官方狀態管理",
            Lines = new() {
                new() { Comment="// 定義 Store", Code="import { defineStore } from 'pinia';", Explanation="💡 Pinia 是 Vue 3 官方狀態管理庫，取代 Vuex。更簡單、支援 TypeScript。" },
                new() { Code="export const useCounterStore = defineStore('counter', {", Explanation="💡 'counter' 是 store 的唯一 ID。useXxxStore 是命名慣例。" },
                new() { Code="    state: () => ({ count: 0, name: 'DevLearn' }),", Explanation="💡 state 用函式回傳，確保每個組件得到獨立的資料副本。" },
                new() { Code="    getters: { doubleCount: (state) => state.count * 2 },", Explanation="💡 getters 等同於 computed。會自動快取結果。" },
                new() { Code="    actions: { increment() { this.count++; } }", Explanation="💡 actions 是修改 state 的方法。可以是同步或非同步（async/await）。" },
                new() { Code="});" },
                new() { Comment="// 在組件中使用", Code="const store = useCounterStore();", Explanation="💡 呼叫 useCounterStore() 取得 store 實例。state、getters、actions 都可以直接存取。" },
                new() { Code="console.log(store.count, store.doubleCount);", Explanation="💡 store 是響應式的。修改 state 會自動更新所有使用它的組件。" },
            }
        },
        new() { Id=506, Category="vue", Title="Vue Router", Difficulty="intermediate", Description="單頁應用路由管理",
            Lines = new() {
                new() { Code="import { createRouter, createWebHistory } from 'vue-router';", Explanation="💡 vue-router 是 Vue 官方路由庫。createWebHistory 使用 HTML5 History API。" },
                new() { Code="const routes = [", Explanation="💡 routes 陣列定義 URL 和組件的對應關係。" },
                new() { Code="    { path: '/', component: Home },", Explanation="💡 path 是 URL 路徑，component 是要顯示的組件。" },
                new() { Code="    { path: '/user/:id', component: User, props: true },", Explanation="💡 :id 是動態路由參數。props: true 把參數作為 props 傳入組件。" },
                new() { Code="    { path: '/:pathMatch(.*)*', component: NotFound }", Explanation="💡 萬用路由，匹配所有未定義的路徑。放在最後面當 404 頁面。" },
                new() { Code="];" },
                new() { Code="const router = createRouter({ history: createWebHistory(), routes });", Explanation="💡 createRouter 建立路由實例。還有 createWebHashHistory 用 # 模式。" },
                new() { Comment="// 導航守衛", Code="router.beforeEach((to, from) => { if (to.meta.auth && !isLoggedIn()) return '/login'; });", Explanation="💡 beforeEach 在每次導航前執行。常用於權限檢查。回傳 false 或路由路徑可以取消或重定向。" },
            }
        },
        new() { Id=507, Category="vue", Title="組合式函式 Composable", Difficulty="advanced", Description="可重用的邏輯封裝",
            Lines = new() {
                new() { Comment="// 封裝可重用的滑鼠位置邏輯", Code="function useMouse() {", Explanation="💡 Composable 是 Vue 3 的邏輯重用方式。命名慣例：useXxx。取代 Vue 2 的 Mixins。" },
                new() { Code="    const x = ref(0);", Explanation="💡 在 composable 裡使用 ref、computed、watch 等 Composition API。" },
                new() { Code="    const y = ref(0);" },
                new() { Code="    onMounted(() => {", Explanation="💡 onMounted 是生命週期鉤子。組件掛載到 DOM 後執行。" },
                new() { Code="        window.addEventListener('mousemove', e => { x.value = e.pageX; y.value = e.pageY; });" },
                new() { Code="    });" },
                new() { Code="    return { x, y };", Explanation="💡 回傳響應式的值。使用端可以直接解構。" },
                new() { Code="}" },
                new() { Comment="// 在組件中使用", Code="const { x, y } = useMouse();", Explanation="💡 一行就能在任何組件中重用滑鼠追蹤邏輯。不會像 Mixin 有命名衝突問題。" },
            }
        },
    };

    // ═══════════════════════════════════════════
    //  ASP.NET Core API 擴充（ID 601-620）
    // ═══════════════════════════════════════════
    static List<CodeLesson> ApiLessons() => new()
    {
        new() { Id=601, Category="api", Title="第一個 API Controller", Difficulty="beginner", Description="建立 RESTful API 端點",
            Lines = new() {
                new() { Comment="// API Controller 基本結構", Code="[ApiController]", Explanation="💡 ApiController 屬性啟用 API 專屬功能：自動模型驗證、自動 400 回應等。" },
                new() { Code="[Route(\"api/[controller]\")]", Explanation="💡 [controller] 會被替換成類別名稱去掉 Controller 後綴。如 UsersController → api/users。" },
                new() { Code="public class UsersController : ControllerBase {", Explanation="💡 API Controller 繼承 ControllerBase（不是 Controller）。不需要 View 相關功能。" },
                new() { Comment="// GET api/users", Code="    [HttpGet]", Explanation="💡 HttpGet 屬性標記這個方法處理 GET 請求。RESTful 慣例：GET = 查詢。" },
                new() { Code="    public IActionResult GetAll() {", Explanation="💡 IActionResult 是回傳型別，可以回傳 Ok()、NotFound()、BadRequest() 等。" },
                new() { Code="        return Ok(new[] { \"Alice\", \"Bob\" });", Explanation="💡 Ok() 回傳 HTTP 200 + JSON 資料。ASP.NET 自動把物件序列化成 JSON。" },
                new() { Code="    }" },
                new() { Code="}" },
            }
        },
        new() { Id=602, Category="api", Title="CRUD API", Difficulty="beginner", Description="完整的增刪改查 API",
            Lines = new() {
                new() { Comment="// GET api/users/{id}", Code="[HttpGet(\"{id}\")]", Explanation="💡 {id} 是路由參數。URL api/users/5 會把 5 傳入 id 參數。" },
                new() { Code="public IActionResult GetById(int id) {", Explanation="💡 參數名稱必須和路由 {id} 一致。ASP.NET 自動做型別轉換。" },
                new() { Code="    var user = _db.Users.Find(id);", Explanation="💡 Find() 用主鍵查詢，效率最高。找不到回傳 null。" },
                new() { Code="    return user == null ? NotFound() : Ok(user);", Explanation="💡 NotFound() 回傳 HTTP 404。三元運算讓程式碼更簡潔。" },
                new() { Code="}" },
                new() { Comment="// POST api/users", Code="[HttpPost]", Explanation="💡 POST = 新增資源。request body 包含新資料的 JSON。" },
                new() { Code="public IActionResult Create([FromBody] User user) {", Explanation="💡 [FromBody] 告訴 ASP.NET 從 request body 反序列化 JSON 到 User 物件。" },
                new() { Code="    _db.Users.Add(user);", Explanation="💡 Add() 把實體加入追蹤。SaveChanges() 才真的寫入資料庫。" },
                new() { Code="    _db.SaveChanges();", Explanation="💡 SaveChanges() 執行所有待處理的資料庫操作（INSERT/UPDATE/DELETE）。" },
                new() { Code="    return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);", Explanation="💡 CreatedAtAction 回傳 HTTP 201 + Location header 指向新資源的 URL。RESTful 標準做法。" },
                new() { Code="}" },
            }
        },
        new() { Id=603, Category="api", Title="Middleware 中介軟體", Difficulty="intermediate", Description="請求管線處理",
            Lines = new() {
                new() { Comment="// 自訂 Middleware", Code="public class LoggingMiddleware {", Explanation="💡 Middleware 是請求管線上的處理器。每個請求都會依序通過所有 Middleware。" },
                new() { Code="    private readonly RequestDelegate _next;", Explanation="💡 RequestDelegate 是管線中的下一個 Middleware。Middleware 像洋蔥的層。" },
                new() { Code="    public LoggingMiddleware(RequestDelegate next) => _next = next;" },
                new() { Code="    public async Task InvokeAsync(HttpContext context) {", Explanation="💡 InvokeAsync 是 Middleware 的入口。HttpContext 包含 Request 和 Response。" },
                new() { Code="        Console.WriteLine($\"{context.Request.Method} {context.Request.Path}\");", Explanation="💡 記錄 HTTP 方法和路徑。例如 GET /api/users。" },
                new() { Code="        await _next(context);", Explanation="💡 呼叫 _next 把請求傳給下一個 Middleware。不呼叫就中斷管線（短路）。" },
                new() { Code="        Console.WriteLine($\"Response: {context.Response.StatusCode}\");", Explanation="💡 _next 回來後，Response 已經被設定。可以在這裡做後處理。" },
                new() { Code="    }" },
                new() { Code="}" },
            }
        },
        new() { Id=604, Category="api", Title="依賴注入 DI", Difficulty="intermediate", Description="ASP.NET Core DI 容器",
            Lines = new() {
                new() { Comment="// 定義服務介面", Code="public interface IEmailService { Task SendAsync(string to, string body); }", Explanation="💡 定義介面而非直接依賴實作類別。這讓程式碼更容易測試和替換。" },
                new() { Comment="// 實作服務", Code="public class SmtpEmailService : IEmailService {", Explanation="💡 實作類別提供具體的 Email 發送邏輯。可以有多個實作（SMTP、SendGrid 等）。" },
                new() { Code="    public async Task SendAsync(string to, string body) {", Explanation="💡 async Task 表示非同步方法。發送 Email 是 I/O 操作，應該非同步。" },
                new() { Code="        Console.WriteLine($\"Sending to {to}: {body}\");" },
                new() { Code="    }" },
                new() { Code="}" },
                new() { Comment="// 在 Program.cs 註冊", Code="builder.Services.AddScoped<IEmailService, SmtpEmailService>();", Explanation="💡 AddScoped 每個 HTTP 請求建立一個實例。也有 AddSingleton（全域一個）和 AddTransient（每次注入新建）。" },
                new() { Comment="// Controller 自動注入", Code="public class OrderController(IEmailService email) : ControllerBase { }", Explanation="💡 C# 12 主要建構函式（Primary Constructor）。ASP.NET 自動注入 IEmailService 實例。" },
            }
        },
        new() { Id=605, Category="api", Title="Entity Framework Core", Difficulty="intermediate", Description="ORM 資料庫操作",
            Lines = new() {
                new() { Comment="// 定義資料模型", Code="public class Product {", Explanation="💡 EF Core 用 class 對應資料庫的 table。每個屬性對應一個欄位。" },
                new() { Code="    public int Id { get; set; }", Explanation="💡 Id 或 [類別名]Id 會被自動識別為主鍵。" },
                new() { Code="    public string Name { get; set; } = \"\";" },
                new() { Code="    public decimal Price { get; set; }" },
                new() { Code="}" },
                new() { Comment="// DbContext", Code="public class AppDb : DbContext {", Explanation="💡 DbContext 是 EF Core 的核心。管理資料庫連線和實體追蹤。" },
                new() { Code="    public DbSet<Product> Products { get; set; }", Explanation="💡 DbSet<T> 對應一張表。可以用 LINQ 查詢。" },
                new() { Code="}" },
                new() { Comment="// LINQ 查詢（會轉成 SQL）", Code="var cheap = await db.Products.Where(p => p.Price < 100).OrderBy(p => p.Name).ToListAsync();", Explanation="💡 EF Core 把 LINQ 轉成 SQL 送到資料庫執行。ToListAsync 才真的執行查詢。" },
            }
        },
        new() { Id=606, Category="api", Title="JWT 認證", Difficulty="advanced", Description="Token 身份驗證",
            Lines = new() {
                new() { Comment="// 產生 JWT Token", Code="var claims = new[] { new Claim(ClaimTypes.Name, user.Name), new Claim(\"role\", \"admin\") };", Explanation="💡 Claims 是 Token 中的資訊片段。Name、Role 等。Token 解碼後可以取出這些資訊。" },
                new() { Code="var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(\"YourSuperSecretKey123!\"));", Explanation="💡 對稱金鑰用來簽名 Token。實務上要從設定檔讀取，不要硬編碼。至少 32 字元。" },
                new() { Code="var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);", Explanation="💡 HMAC-SHA256 是最常用的 JWT 簽名演算法。" },
                new() { Code="var token = new JwtSecurityToken(issuer: \"DevLearn\", audience: \"DevLearn\", claims: claims, expires: DateTime.Now.AddHours(24), signingCredentials: creds);", Explanation="💡 設定發行者、接收者、過期時間。expires 控制 Token 有效期。" },
                new() { Code="return new JwtSecurityTokenHandler().WriteToken(token);", Explanation="💡 WriteToken 把 Token 物件序列化成 JWT 字串（xxx.yyy.zzz 格式）。" },
            }
        },
        new() { Id=607, Category="api", Title="SignalR 即時通訊", Difficulty="advanced", Description="WebSocket 即時推送",
            Lines = new() {
                new() { Comment="// 定義 Hub（伺服器端）", Code="public class ChatHub : Hub {", Explanation="💡 Hub 是 SignalR 的核心類別。它管理客戶端連線和訊息廣播。" },
                new() { Code="    public async Task SendMessage(string user, string message) {", Explanation="💡 客戶端可以呼叫這個方法。Hub 方法可以是 async。" },
                new() { Code="        await Clients.All.SendAsync(\"ReceiveMessage\", user, message);", Explanation="💡 Clients.All 廣播給所有連線的客戶端。也可以用 Clients.Caller（只回覆發送者）。" },
                new() { Code="    }" },
                new() { Code="}" },
                new() { Comment="// 客戶端 JavaScript", Code="const connection = new signalR.HubConnectionBuilder().withUrl('/chatHub').build();", Explanation="💡 建立 SignalR 連線。withUrl 指向伺服器的 Hub 端點。" },
                new() { Code="connection.on('ReceiveMessage', (user, msg) => console.log(`${user}: ${msg}`));", Explanation="💡 on 監聽伺服器推送的事件。名稱必須和 SendAsync 的第一個參數一致。" },
                new() { Code="await connection.start();", Explanation="💡 start() 建立 WebSocket 連線。SignalR 會自動降級到 Server-Sent Events 或 Long Polling。" },
            }
        },
    };

    // ═══════════════════════════════════════════
    //  Java 擴充（ID 701-720）
    // ═══════════════════════════════════════════
    static List<CodeLesson> JavaLessons() => new()
    {
        new() { Id=701, Category="java", Title="Hello World", Difficulty="beginner", Description="你的第一個 Java 程式",
            Lines = new() {
                new() { Comment="// Java 程式從 class 開始", Code="public class Main {", Explanation="💡 Java 的每個程式都必須放在 class 裡面。檔案名必須和 public class 名稱一致（Main.java）。" },
                new() { Comment="// main 方法是程式進入點", Code="    public static void main(String[] args) {", Explanation="💡 main 是程式的進入點。public static void 是固定寫法。String[] args 接收命令列參數。" },
                new() { Code="        System.out.println(\"Hello, World!\");", Explanation="💡 System.out.println 印出文字並換行。類似 C# 的 Console.WriteLine。" },
                new() { Code="    }" },
                new() { Code="}" },
            }
        },
        new() { Id=702, Category="java", Title="變數與型態", Difficulty="beginner", Description="基本資料型態與變數宣告",
            Lines = new() {
                new() { Comment="// 整數型態", Code="int age = 25;", Explanation="💡 Java 的 int 是 32 位元整數。不像 JavaScript，Java 區分整數和浮點數。" },
                new() { Comment="// 浮點數", Code="double price = 99.99;", Explanation="💡 double 是 64 位元浮點數。float 是 32 位元（要加 f 後綴：99.99f）。" },
                new() { Comment="// 字串（不是基本型態）", Code="String name = \"小明\";", Explanation="💡 String 是物件型態（大寫 S），不是基本型態。字串是不可變的（immutable）。" },
                new() { Comment="// 布林值", Code="boolean isStudent = true;", Explanation="💡 Java 用 boolean（不是 bool）。只有 true 和 false，不像 JS 有 truthy/falsy。" },
                new() { Comment="// 字元", Code="char grade = 'A';", Explanation="💡 char 用單引號，String 用雙引號。char 只能存一個字元。" },
                new() { Comment="// 型態轉換", Code="int num = (int) 3.14;", Explanation="💡 (int) 是強制轉型（casting）。3.14 會變成 3（截斷小數）。" },
                new() { Comment="// var（Java 10+）", Code="var list = new ArrayList<String>();", Explanation="💡 var 讓編譯器自動推斷型態。只能用在區域變數，不能用在方法參數或欄位。" },
            }
        },
        new() { Id=703, Category="java", Title="if/else 與迴圈", Difficulty="beginner", Description="流程控制與迴圈",
            Lines = new() {
                new() { Comment="// if-else", Code="if (score >= 90) {", Explanation="💡 Java 的 if-else 和 C# 幾乎一樣。條件必須是 boolean（不能像 JS 用數字或字串）。" },
                new() { Code="    System.out.println(\"優秀\");" },
                new() { Code="} else if (score >= 60) {" },
                new() { Code="    System.out.println(\"及格\");" },
                new() { Code="} else {" },
                new() { Code="    System.out.println(\"不及格\");" },
                new() { Code="}" },
                new() { Comment="// for 迴圈", Code="for (int i = 0; i < 5; i++) {", Explanation="💡 標準 for 迴圈：初始化; 條件; 遞增。和 C#、JavaScript 語法相同。" },
                new() { Code="    System.out.println(i);" },
                new() { Code="}" },
                new() { Comment="// enhanced for（foreach）", Code="for (String name : names) {", Explanation="💡 Java 的 foreach 用冒號 :。等同 C# 的 foreach (var name in names)。" },
                new() { Code="    System.out.println(name);" },
                new() { Code="}" },
            }
        },
        new() { Id=704, Category="java", Title="方法（Method）", Difficulty="beginner", Description="定義與呼叫方法",
            Lines = new() {
                new() { Comment="// 靜態方法", Code="public static int add(int a, int b) {", Explanation="💡 public = 公開存取，static = 不需要物件即可呼叫，int = 回傳整數。" },
                new() { Code="    return a + b;", Explanation="💡 return 回傳結果並結束方法。回傳型態必須和宣告一致。" },
                new() { Code="}" },
                new() { Comment="// 方法多載（Overloading）", Code="public static double add(double a, double b) {", Explanation="💡 同名方法可以有不同的參數型態。Java 根據傳入的參數自動選擇正確的方法。" },
                new() { Code="    return a + b;" },
                new() { Code="}" },
                new() { Comment="// 呼叫方法", Code="int result = add(3, 5);", Explanation="💡 傳入 int → 呼叫 int 版本。傳入 double → 呼叫 double 版本。" },
                new() { Comment="// void 方法（不回傳值）", Code="public static void greet(String name) {", Explanation="💡 void 表示不回傳值。方法結束或遇到空 return 就結束。" },
                new() { Code="    System.out.println(\"Hello, \" + name);" },
                new() { Code="}" },
            }
        },
        new() { Id=705, Category="java", Title="類別與物件", Difficulty="intermediate", Description="OOP 基礎：class、constructor",
            Lines = new() {
                new() { Comment="// 定義類別", Code="public class Student {", Explanation="💡 Java 每個 public class 必須在自己的 .java 檔案中。檔名和類名一致。" },
                new() { Comment="// 私有欄位", Code="    private String name;", Explanation="💡 private 封裝：外部不能直接存取。這是 OOP 封裝（Encapsulation）的基礎。" },
                new() { Code="    private int age;" },
                new() { Comment="// 建構子", Code="    public Student(String name, int age) {", Explanation="💡 constructor 和類名相同，沒有回傳型態。new Student() 時自動呼叫。" },
                new() { Code="        this.name = name;", Explanation="💡 this.name 是欄位，name 是參數。this 用來區分同名的欄位和參數。" },
                new() { Code="        this.age = age;" },
                new() { Code="    }" },
                new() { Comment="// Getter", Code="    public String getName() { return name; }", Explanation="💡 Java 沒有 C# 的 property 語法，用 getter/setter 方法。IDE 可以自動生成。" },
                new() { Comment="// toString 覆寫", Code="    public String toString() { return name + \" (\" + age + \")\"; }", Explanation="💡 覆寫 toString 讓 println 印出有意義的文字。Java 建議加 @Override 註解。" },
                new() { Code="}" },
            }
        },
        new() { Id=706, Category="java", Title="介面與繼承", Difficulty="intermediate", Description="interface、extends、implements",
            Lines = new() {
                new() { Comment="// 定義介面", Code="interface Flyable {", Explanation="💡 interface 定義行為契約。Java 支持多介面實作（但不支持多重繼承）。" },
                new() { Code="    void fly();", Explanation="💡 介面方法預設是 public abstract。Java 8+ 可以用 default 提供預設實作。" },
                new() { Code="}" },
                new() { Comment="// 抽象類別", Code="abstract class Animal {", Explanation="💡 abstract class 可以有實作和抽象方法。不能被 new。介於 class 和 interface 之間。" },
                new() { Code="    abstract void speak();", Explanation="💡 abstract 方法沒有 body，子類別必須實作。" },
                new() { Code="}" },
                new() { Comment="// 繼承 + 實作介面", Code="class Bird extends Animal implements Flyable {", Explanation="💡 extends 繼承類別（只能一個），implements 實作介面（可以多個）。" },
                new() { Code="    public void speak() { System.out.println(\"啾啾\"); }", Explanation="💡 實作抽象方法。Java 建議加 @Override 註解。" },
                new() { Code="    public void fly() { System.out.println(\"飛行中\"); }" },
                new() { Code="}" },
            }
        },
        new() { Id=707, Category="java", Title="ArrayList 與泛型", Difficulty="beginner", Description="動態陣列與 Generic",
            Lines = new() {
                new() { Comment="// 引入 ArrayList", Code="import java.util.ArrayList;", Explanation="💡 ArrayList 在 java.util 套件中。Java 的 import 類似 C# 的 using。" },
                new() { Comment="// 建立泛型 List", Code="ArrayList<String> names = new ArrayList<>();", Explanation="💡 <String> 限制只能放字串。<> 是菱形推斷（Diamond Inference），Java 7+。" },
                new() { Code="names.add(\"小明\");", Explanation="💡 add 在尾端新增。Java 的 ArrayList 對應 C# 的 List<T>。" },
                new() { Code="names.add(\"小華\");" },
                new() { Code="String first = names.get(0);", Explanation="💡 get(index) 取值。Java 用 get()，C# 用 []。" },
                new() { Code="names.remove(\"小華\");", Explanation="💡 remove 移除第一個符合的元素。也可以 remove(index) 移除指定位置。" },
                new() { Code="int size = names.size();", Explanation="💡 size() 回傳元素數量。注意 Java 是 size()，C# 是 Count。" },
                new() { Comment="// forEach（Java 8+）", Code="names.forEach(n -> System.out.println(n));", Explanation="💡 Lambda 表達式：n -> 等同 C# 的 n => 。forEach 對每個元素執行。" },
            }
        },
        new() { Id=708, Category="java", Title="HashMap", Difficulty="intermediate", Description="鍵值對資料結構",
            Lines = new() {
                new() { Code="import java.util.HashMap;", Explanation="💡 HashMap 是 Java 最常用的 Map 實作。對應 C# 的 Dictionary。" },
                new() { Code="HashMap<String, Integer> scores = new HashMap<>();", Explanation="💡 <Key型態, Value型態>。注意用 Integer 不是 int（泛型不支持基本型態）。" },
                new() { Code="scores.put(\"小明\", 95);", Explanation="💡 put 新增或更新鍵值對。對應 C# 的 [] 或 Add。" },
                new() { Code="scores.put(\"小華\", 88);" },
                new() { Code="int score = scores.getOrDefault(\"小明\", 0);", Explanation="💡 getOrDefault 找不到時回傳預設值。比 get 更安全（get 找不到回傳 null）。" },
                new() { Code="scores.forEach((k, v) -> System.out.println(k + \": \" + v));", Explanation="💡 forEach 遍歷所有鍵值對。Lambda 有兩個參數：key 和 value。" },
                new() { Comment="// 檢查是否有 key", Code="boolean has = scores.containsKey(\"小美\");", Explanation="💡 containsKey 檢查鍵是否存在。containsValue 檢查值。" },
            }
        },
        new() { Id=709, Category="java", Title="例外處理", Difficulty="intermediate", Description="try/catch/finally 與自訂例外",
            Lines = new() {
                new() { Code="try {", Explanation="💡 Java 的 try-catch 和 C# 幾乎一樣。Java 有 checked exception（必須處理或宣告）。" },
                new() { Code="    int result = Integer.parseInt(\"abc\");", Explanation="💡 parseInt 把字串轉成 int。\"abc\" 不是數字，拋出 NumberFormatException。" },
                new() { Code="} catch (NumberFormatException e) {", Explanation="💡 catch 指定要攔截的例外型態。e 是例外物件。" },
                new() { Code="    System.out.println(\"格式錯誤: \" + e.getMessage());" },
                new() { Code="} catch (Exception e) {", Explanation="💡 Exception 是所有例外的父類別。放在最後當通用攔截器。" },
                new() { Code="    e.printStackTrace();", Explanation="💡 printStackTrace 印出完整的錯誤堆疊追蹤。偵錯時非常有用。" },
                new() { Code="} finally {", Explanation="💡 finally 不管有沒有例外都會執行。常用來關閉檔案、資料庫連線。" },
                new() { Code="    System.out.println(\"清理完成\");" },
                new() { Code="}" },
            }
        },
        new() { Id=710, Category="java", Title="Stream API", Difficulty="advanced", Description="Java 8 函式式資料處理",
            Lines = new() {
                new() { Code="import java.util.List;", Explanation="💡 Stream API 是 Java 8 的重大功能。類似 C# 的 LINQ 和 JS 的 map/filter/reduce。" },
                new() { Code="List<Integer> numbers = List.of(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);", Explanation="💡 List.of 建立不可變的 List（Java 9+）。" },
                new() { Comment="// filter + map + collect", Code="var evens = numbers.stream().filter(n -> n % 2 == 0).map(n -> n * 10).toList();", Explanation="💡 stream() 開始串流。filter 過濾、map 轉換、toList 收集結果。和 JS 的鏈式呼叫很像。" },
                new() { Comment="// reduce 累計", Code="int sum = numbers.stream().reduce(0, Integer::sum);", Explanation="💡 reduce(初始值, 累加函式)。Integer::sum 是方法參考，等同 (a,b) -> a+b。" },
                new() { Comment="// 統計", Code="var stats = numbers.stream().mapToInt(Integer::intValue).summaryStatistics();", Explanation="💡 summaryStatistics 一次取得 count、sum、min、max、average。非常方便。" },
                new() { Code="System.out.println(\"avg: \" + stats.getAverage());", Explanation="💡 getCount()、getSum()、getMin()、getMax()、getAverage() 都可以取。" },
            }
        },
        new() { Id=711, Category="java", Title="Lambda 與函式介面", Difficulty="intermediate", Description="Functional Interface 與 Lambda",
            Lines = new() {
                new() { Comment="// 函式介面（只有一個抽象方法）", Code="@FunctionalInterface", Explanation="💡 @FunctionalInterface 確保介面只有一個抽象方法。Lambda 只能用在函式介面上。" },
                new() { Code="interface MathOperation { int operate(int a, int b); }" },
                new() { Comment="// Lambda 實作", Code="MathOperation add = (a, b) -> a + b;", Explanation="💡 Lambda 是匿名函式的簡寫。(參數) -> 表達式。類似 C# 的 (a, b) => a + b。" },
                new() { Code="MathOperation multiply = (a, b) -> a * b;" },
                new() { Code="System.out.println(add.operate(5, 3));", Explanation="💡 呼叫 Lambda 就像呼叫介面方法。結果是 8。" },
                new() { Comment="// 內建函式介面", Code="java.util.function.Predicate<String> isLong = s -> s.length() > 5;", Explanation="💡 Java 提供常用的函式介面：Predicate（判斷）、Function（轉換）、Consumer（消費）、Supplier（生產）。" },
                new() { Code="System.out.println(isLong.test(\"Hello World\"));", Explanation="💡 Predicate.test() 回傳 boolean。常搭配 filter 使用。" },
            }
        },
        new() { Id=712, Category="java", Title="Spring Boot 基礎", Difficulty="advanced", Description="RESTful API 快速入門",
            Lines = new() {
                new() { Comment="// Spring Boot 主類別", Code="@SpringBootApplication", Explanation="💡 @SpringBootApplication 組合了 @Configuration + @EnableAutoConfiguration + @ComponentScan。一個註解搞定所有設定。" },
                new() { Code="public class Application {" },
                new() { Code="    public static void main(String[] args) { SpringApplication.run(Application.class, args); }", Explanation="💡 SpringApplication.run 啟動內嵌的 Tomcat 伺服器和 Spring 容器。" },
                new() { Code="}" },
                new() { Comment="// REST Controller", Code="@RestController", Explanation="💡 @RestController = @Controller + @ResponseBody。方法回傳值自動序列化成 JSON。" },
                new() { Code="@RequestMapping(\"/api/users\")", Explanation="💡 @RequestMapping 設定路由前綴。類似 ASP.NET 的 [Route]。" },
                new() { Code="public class UserController {" },
                new() { Code="    @GetMapping", Explanation="💡 @GetMapping 處理 GET 請求。也有 @PostMapping、@PutMapping、@DeleteMapping。" },
                new() { Code="    public List<String> getAll() { return List.of(\"Alice\", \"Bob\"); }" },
                new() { Code="}" },
            }
        },
    };
}
