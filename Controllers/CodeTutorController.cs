using Microsoft.AspNetCore.Mvc;
using DotNetLearning.Data;
using Microsoft.EntityFrameworkCore;

namespace DotNetLearning.Controllers;

public class CodeTutorController : Controller
{
    private readonly AppDbContext _db;
    public CodeTutorController(AppDbContext db) { _db = db; }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetLessons()
    {
        // Return predefined lessons
        var lessons = GetAllLessons();
        return Json(lessons.Select(l => new { l.Id, l.Title, l.Difficulty, l.Description }));
    }

    [HttpGet]
    public IActionResult GetLesson(int id)
    {
        var lessons = GetAllLessons();
        var lesson = lessons.FirstOrDefault(l => l.Id == id);
        if (lesson == null) return NotFound();
        return Json(lesson);
    }

    private List<CodeLesson> GetAllLessons()
    {
        return new List<CodeLesson>
        {
            new() { Id = 1, Title = "Hello World", Difficulty = "beginner", Description = "你的第一個 C# 程式",
                Lines = new List<CodeLine> {
                    new() { Comment = "// 引入 System 命名空間，這樣才能使用 Console", Code = "using System;", Explanation = "💡 using 是 C# 的引入指令，類似 Python 的 import。System 命名空間包含了 Console、Math 等基礎類別。沒有 using System，你就要寫 System.Console.WriteLine() 全名。" },
                    new() { Comment = "// 定義一個類別，C# 的程式碼都要放在類別裡面", Code = "class Program {", Explanation = "💡 C# 是物件導向語言，所有程式碼都必須放在 class（類別）裡面。Program 是慣例名稱，你也可以叫其他名字。大括號 { } 定義了類別的範圍。" },
                    new() { Comment = "// Main 是程式的進入點，程式從這裡開始執行", Code = "    static void Main() {", Explanation = "💡 Main 是程式的進入點（Entry Point），程式執行時會從這裡開始。static 表示不需要建立物件就能呼叫。void 表示這個方法不回傳值。常見錯誤：把 Main 寫成 main（大小寫敏感）。" },
                    new() { Comment = "// 用 Console.WriteLine 在螢幕上印出文字", Code = "        Console.WriteLine(\"Hello World!\");", Explanation = "💡 Console.WriteLine() 會輸出文字並換行，類似 Python 的 print()。注意字串要用雙引號 \"\" 包起來，結尾要加分號 ;。常見錯誤：忘記分號、用單引號（單引號是 char 型別）。" },
                    new() { Comment = "// 關閉 Main 方法的大括號", Code = "    }" },
                    new() { Comment = "// 關閉 Program 類別的大括號", Code = "}" },
                }
            },
            new() { Id = 2, Title = "變數與型別", Difficulty = "beginner", Description = "宣告變數並使用不同資料型別",
                Lines = new List<CodeLine> {
                    new() { Comment = "// 引入必要的命名空間", Code = "using System;", Explanation = "💡 每個 C# 檔案開頭通常都會有 using 語句。System 是最基礎的命名空間，幾乎每個程式都需要它。這跟 Java 的 import java.lang.* 概念類似。" },
                    new() { Comment = "// 建立程式的主類別", Code = "class Program {", Explanation = "💡 class 是 C# 的基本組織單位。即使是簡單的程式也需要一個類別。在 C# 10+ 可以用「頂層語句」省略 class，但學習時建議先寫完整結構。" },
                    new() { Comment = "// 程式進入點", Code = "    static void Main() {", Explanation = "💡 每個 C# 應用程式只能有一個 Main 方法作為進入點。縮排用 4 個空格是 C# 的慣例（不像 Python 強制要求，但良好的縮排讓程式更易讀）。" },
                    new() { Comment = "// 用 int 宣告整數變數，存放年齡", Code = "        int age = 25;", Explanation = "💡 int 是 32 位元整數，範圍約 -21 億到 21 億。C# 是強型別語言，宣告時必須指定型別（不像 Python 可以直接 age = 25）。也可以用 var 讓編譯器自動推斷型別：var age = 25;" },
                    new() { Comment = "// 用 string 宣告字串變數，存放名字", Code = "        string name = \"Mike\";", Explanation = "💡 string 是參考型別，用雙引號包裹。C# 的 string 其實是 System.String 的別名。常見錯誤：用單引號（'Mike' 是錯的，單引號只能用於 char，如 'M'）。" },
                    new() { Comment = "// 用 double 宣告浮點數，存放身高", Code = "        double height = 175.5;", Explanation = "💡 double 是 64 位元浮點數，精度約 15-16 位數字。如果需要更高精度（如金融計算），應該用 decimal。float 則是 32 位元，需加 f 後綴：float h = 175.5f;" },
                    new() { Comment = "// 用 bool 宣告布林值，true 或 false", Code = "        bool isStudent = true;", Explanation = "💡 bool 只有 true 和 false 兩個值。注意 C# 的布林值是小寫（true/false），不像 Python 用大寫（True/False）。命名慣例：布林變數常用 is、has、can 開頭。" },
                    new() { Comment = "// 用字串插值 $\"\" 組合變數輸出", Code = "        Console.WriteLine($\"姓名：{name}，年齡：{age}\");", Explanation = "💡 字串插值（$\"\"）是 C# 6.0 引入的語法糖，比 string.Format() 更簡潔。{} 裡可以放任何表達式，如 {age + 1}。類似 Python 的 f-string：f\"姓名：{name}\"。" },
                    new() { Comment = "// 輸出身高和學生狀態", Code = "        Console.WriteLine($\"身高：{height}，是學生：{isStudent}\");", Explanation = "💡 Console.WriteLine 可以輸出任何型別，因為所有型別都有 ToString() 方法。bool 輸出為 True/False（注意首字母大寫）。double 會自動格式化，如需控制小數位數可用 {height:F1}。" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 3, Title = "if / else 條件判斷", Difficulty = "beginner", Description = "學習條件分支邏輯",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;", Explanation = "💡 using System; 讓我們可以直接使用 Console 類別。如果省略這行，每次都要寫 System.Console.WriteLine()，非常冗長。" },
                    new() { Code = "class Program {", Explanation = "💡 所有 C# 程式碼都必須在類別中。Program 是主程式類別的慣例名稱。" },
                    new() { Code = "    static void Main() {", Explanation = "💡 Main 方法是程式的起點。static 讓它不需要實例化就能被系統呼叫。" },
                    new() { Comment = "// 宣告一個分數變數", Code = "        int score = 85;", Explanation = "💡 這裡宣告了一個 int 變數。在實際應用中，這個值可能來自使用者輸入或資料庫。你可以試著改成不同數值看看結果會怎樣。" },
                    new() { Comment = "// if 判斷：分數 >= 90 是 A", Code = "        if (score >= 90) {", Explanation = "💡 if 語句用小括號 () 包條件，大括號 {} 包程式碼區塊。>= 是「大於等於」運算子。注意：C# 的 if 條件必須是 bool 型別，不能像 JavaScript 用 if (1) 這種寫法。" },
                    new() { Code = "            Console.WriteLine(\"等級：A\");", Explanation = "💡 這行只有在 score >= 90 時才會執行。注意縮排增加了一層（12 個空格），表示它在 if 區塊裡面。良好的縮排讓程式邏輯一目了然。" },
                    new() { Comment = "// else if：分數 >= 80 是 B", Code = "        } else if (score >= 80) {", Explanation = "💡 } else if 必須寫在同一行（C# 風格慣例）。條件判斷是由上到下，一旦某個條件成立就不會再檢查下面的。所以這裡不用寫 score >= 80 && score < 90。" },
                    new() { Code = "            Console.WriteLine(\"等級：B\");", Explanation = "💡 因為 score = 85 符合 >= 80 這個條件，所以程式會執行這一行。前面的 >= 90 不成立，所以跳到這裡。" },
                    new() { Comment = "// else if：分數 >= 70 是 C", Code = "        } else if (score >= 70) {", Explanation = "💡 可以串接多個 else if 來處理多種情況。比起寫很多獨立的 if，用 if-else if-else 鏈更有效率，因為只要一個條件成立就會跳出。" },
                    new() { Code = "            Console.WriteLine(\"等級：C\");", Explanation = "💡 如果 score 是 75，就會執行這行。每個分支只會執行一個，這就是 else if 的好處。" },
                    new() { Comment = "// 其他情況都是 F", Code = "        } else {", Explanation = "💡 else 是「以上都不符合」的最終分支，不需要條件。它就像一個安全網，確保所有情況都被處理到。建議養成習慣：多分支判斷一定要有 else。" },
                    new() { Code = "            Console.WriteLine(\"等級：F\");", Explanation = "💡 當所有 if 和 else if 條件都不成立時，才會執行 else 裡的程式碼。" },
                    new() { Code = "        }" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 4, Title = "for 迴圈", Difficulty = "beginner", Description = "用迴圈重複執行程式碼",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;", Explanation = "💡 引入 System 命名空間，提供 Console 等基本功能。" },
                    new() { Code = "class Program {", Explanation = "💡 C# 程式的基本結構：所有程式碼都放在類別裡。" },
                    new() { Code = "    static void Main() {", Explanation = "💡 程式進入點。static 讓 CLR（Common Language Runtime）可以直接呼叫，不需要先建立 Program 物件。" },
                    new() { Comment = "// for 迴圈：i 從 1 到 5，每次 +1", Code = "        for (int i = 1; i <= 5; i++) {", Explanation = "💡 for 迴圈有三個部分：(初始化; 條件; 遞增)。int i = 1 是起始值，i <= 5 是繼續條件，i++ 是每次加 1。類似 Python 的 for i in range(1, 6)，但 C# 給你更多控制權。" },
                    new() { Comment = "// 印出目前的 i 值", Code = "            Console.WriteLine($\"第 {i} 次\");", Explanation = "💡 每次迴圈執行時，{i} 會被替換成目前的值（1, 2, 3, 4, 5）。字串插值 $ 讓我們可以直接在字串中嵌入變數。" },
                    new() { Code = "        }", Explanation = "💡 這個 } 對應 for 迴圈的 {。迴圈體結束後，i++ 會執行，然後檢查 i <= 5，如果成立就再執行一次。" },
                    new() { Comment = "// 計算 1+2+3+...+10 的總和", Code = "        int sum = 0;", Explanation = "💡 在迴圈外面先宣告累加器（accumulator），初始值設為 0。這是一個非常常見的程式設計模式。如果在迴圈裡面宣告，每次都會被重置為 0。" },
                    new() { Comment = "// 用迴圈累加", Code = "        for (int i = 1; i <= 10; i++) {", Explanation = "💡 第二個迴圈從 1 到 10。注意可以在不同的 for 迴圈中重新宣告 int i，因為 i 的作用域僅限於該迴圈。" },
                    new() { Code = "            sum += i;", Explanation = "💡 sum += i 等同於 sum = sum + i。+= 是複合賦值運算子。每次迴圈：sum 從 0 → 1 → 3 → 6 → 10 → ... → 55。這就是高斯求和！" },
                    new() { Code = "        }" },
                    new() { Comment = "// 輸出結果", Code = "        Console.WriteLine($\"1到10的總和：{sum}\");", Explanation = "💡 迴圈結束後 sum = 55（即 1+2+3+...+10）。這個結果可以用高斯公式 n*(n+1)/2 = 10*11/2 = 55 驗證。" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 5, Title = "陣列與 foreach", Difficulty = "intermediate", Description = "使用陣列存放多筆資料",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;", Explanation = "💡 引入 System 命名空間。Array.Sort() 也在 System 裡面。" },
                    new() { Code = "class Program {", Explanation = "💡 主程式類別。" },
                    new() { Code = "    static void Main() {", Explanation = "💡 程式進入點。" },
                    new() { Comment = "// 宣告字串陣列，存放水果名稱", Code = "        string[] fruits = { \"蘋果\", \"香蕉\", \"橘子\", \"芒果\" };", Explanation = "💡 陣列用 型別[] 宣告，用 { } 初始化。陣列大小固定，一旦建立就不能增減元素。如果需要動態大小，應該用 List<string>。注意：陣列索引從 0 開始，fruits[0] 是「蘋果」。" },
                    new() { Comment = "// 用 foreach 遍歷每個水果", Code = "        foreach (string fruit in fruits) {", Explanation = "💡 foreach 會自動遍歷集合中的每個元素，比 for 迴圈更簡潔安全（不用擔心索引越界）。類似 Python 的 for fruit in fruits:。fruit 是每次迭代的臨時變數。" },
                    new() { Code = "            Console.WriteLine(fruit);", Explanation = "💡 每次迴圈印出一個水果名稱。foreach 裡不能修改集合（不能新增或刪除元素），這是它和 for 的主要差異。" },
                    new() { Code = "        }" },
                    new() { Comment = "// 宣告整數陣列", Code = "        int[] numbers = { 3, 7, 1, 9, 4 };", Explanation = "💡 整數陣列的宣告方式和字串陣列相同。也可以寫成 int[] numbers = new int[] { 3, 7, 1, 9, 4 }; 或指定大小 new int[5]（預設值全為 0）。" },
                    new() { Comment = "// 用 Array.Sort 排序", Code = "        Array.Sort(numbers);", Explanation = "💡 Array.Sort() 是原地排序（in-place），會直接修改原陣列，不會產生新陣列。預設是升序排列。如果要降序，可以排序後用 Array.Reverse(numbers)。" },
                    new() { Comment = "// 輸出排序後的結果", Code = "        Console.WriteLine(string.Join(\", \", numbers));", Explanation = "💡 string.Join() 把陣列元素用指定分隔符串接成一個字串。類似 Python 的 ', '.join()，但 C# 版本可以接受非字串陣列（會自動呼叫 ToString()）。" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 6, Title = "方法（函式）", Difficulty = "intermediate", Description = "把程式碼包成可重複使用的方法",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;", Explanation = "💡 引入 System 命名空間。" },
                    new() { Code = "class Program {", Explanation = "💡 所有方法都必須定義在類別裡面，這是 C# 的規則（不像 Python 可以在檔案層級定義函式）。" },
                    new() { Comment = "// 定義一個計算加法的方法，接收兩個 int 參數", Code = "    static int Add(int a, int b) {", Explanation = "💡 方法簽名：static 表示靜態方法，int 是回傳型別，Add 是方法名，(int a, int b) 是參數列表。C# 方法名慣例用 PascalCase（首字母大寫），不像 Python 用 snake_case。" },
                    new() { Comment = "// return 回傳計算結果", Code = "        return a + b;", Explanation = "💡 return 會立即結束方法並回傳值。回傳型別必須和方法宣告的一致（這裡是 int）。如果方法宣告為 void，就不需要 return 值（但可以用 return; 提早結束）。" },
                    new() { Code = "    }" },
                    new() { Comment = "// 定義一個打招呼方法，接收名字參數", Code = "    static void Greet(string name) {", Explanation = "💡 void 表示此方法不回傳任何值，只執行動作。類似 Python 中沒有 return 的函式。參數 name 的型別必須明確指定為 string。" },
                    new() { Code = "        Console.WriteLine($\"你好，{name}！\");", Explanation = "💡 方法內部可以使用參數 name。方法的好處是「寫一次，到處呼叫」——如果要改打招呼的格式，只需改這一個地方。" },
                    new() { Code = "    }" },
                    new() { Code = "    static void Main() {", Explanation = "💡 Main 也是一個方法！它是特殊的進入點方法。注意 Main 可以放在 Add 和 Greet 的前面或後面，順序不影響（不像 Python 需要先定義再呼叫）。" },
                    new() { Comment = "// 呼叫 Add 方法並存放結果", Code = "        int result = Add(3, 5);", Explanation = "💡 呼叫方法時傳入實際值（引數 arguments）。3 對應參數 a，5 對應參數 b。回傳值存入 result 變數。方法呼叫就像數學：Add(3, 5) 會「變成」8。" },
                    new() { Code = "        Console.WriteLine($\"3 + 5 = {result}\");", Explanation = "💡 result 的值是 8（由 Add 方法回傳）。用字串插值把結果嵌入輸出字串。" },
                    new() { Comment = "// 呼叫 Greet 方法", Code = "        Greet(\"Mike\");", Explanation = "💡 呼叫 void 方法時不需要接收回傳值。Greet 會直接印出「你好，Mike！」。如果寫 int x = Greet(\"Mike\") 會編譯錯誤，因為 void 沒有回傳值。" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 7, Title = "類別與物件", Difficulty = "intermediate", Description = "OOP 物件導向基礎",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;", Explanation = "💡 引入 System 命名空間。" },
                    new() { Comment = "// 定義一個 Dog 類別", Code = "class Dog {", Explanation = "💡 類別（class）是物件的藍圖/模板。Dog 類別描述了「狗」應該有什麼屬性和行為。一個檔案可以有多個類別。類似 Python 的 class Dog:。" },
                    new() { Comment = "// 屬性：狗的名字", Code = "    public string Name { get; set; }", Explanation = "💡 這是 C# 的自動屬性（Auto Property）。{ get; set; } 自動產生 getter 和 setter。public 表示外部可以存取。比起 public string Name;（欄位），屬性提供更好的封裝性。" },
                    new() { Comment = "// 屬性：狗的年齡", Code = "    public int Age { get; set; }", Explanation = "💡 同樣是自動屬性。如果要限制年齡不能為負數，可以改用完整屬性寫法，在 set 中加入驗證邏輯。注意結尾沒有 () ——屬性不是方法。" },
                    new() { Comment = "// 方法：狗叫", Code = "    public void Bark() {", Explanation = "💡 這是實例方法（沒有 static），必須透過物件呼叫（myDog.Bark()）。public 讓外部可以呼叫此方法。void 表示不回傳值。方法定義了物件的「行為」。" },
                    new() { Code = "        Console.WriteLine($\"{Name} 說：汪汪！\");", Explanation = "💡 在實例方法中可以直接存取自己的屬性 Name（等同於 this.Name）。不需要傳參數，因為方法已經知道自己是哪個物件。" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                    new() { Code = "class Program {", Explanation = "💡 Program 是另一個類別，包含 Main 進入點。一個 C# 檔案可以定義多個類別。" },
                    new() { Code = "    static void Main() {", Explanation = "💡 程式進入點。我們會在這裡建立 Dog 物件並使用它。" },
                    new() { Comment = "// 用 new 建立 Dog 物件", Code = "        Dog myDog = new Dog();", Explanation = "💡 new Dog() 會在記憶體中建立一個 Dog 物件（實例化）。類別是藍圖，物件是根據藍圖造出來的實體。類似 Python 的 my_dog = Dog()。每個 new 都會建立獨立的物件。" },
                    new() { Comment = "// 設定屬性值", Code = "        myDog.Name = \"小白\";", Explanation = "💡 用 . 運算子存取物件的屬性。這會呼叫 Name 屬性的 set 存取子。也可以在建立時直接設定：new Dog { Name = \"小白\", Age = 3 }（物件初始化語法）。" },
                    new() { Code = "        myDog.Age = 3;", Explanation = "💡 設定 Age 屬性。如果 Age 沒有 set 存取子（唯讀屬性），這行會編譯錯誤。" },
                    new() { Comment = "// 呼叫方法", Code = "        myDog.Bark();", Explanation = "💡 透過物件呼叫實例方法。myDog.Bark() 會印出「小白 說：汪汪！」——因為這個物件的 Name 是「小白」。不同物件呼叫 Bark() 會印出不同名字。" },
                    new() { Code = "        Console.WriteLine($\"{myDog.Name} 今年 {myDog.Age} 歲\");", Explanation = "💡 可以在字串插值中直接讀取物件的屬性。這會輸出「小白 今年 3 歲」。物件把相關的資料和行為組織在一起，這就是 OOP 的核心概念。" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 8, Title = "LINQ 查詢", Difficulty = "advanced", Description = "用 LINQ 優雅地處理資料集合",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;", Explanation = "💡 引入 System 命名空間，提供基礎類別。" },
                    new() { Comment = "// 引入 LINQ 命名空間", Code = "using System.Linq;", Explanation = "💡 LINQ（Language Integrated Query）是 C# 最強大的功能之一！它讓你用類似 SQL 的語法查詢任何集合。必須引入 System.Linq 才能使用 Where、Select、OrderBy 等方法。" },
                    new() { Code = "class Program {", Explanation = "💡 主程式類別。" },
                    new() { Code = "    static void Main() {", Explanation = "💡 程式進入點。" },
                    new() { Comment = "// 建立一個數字清單", Code = "        int[] numbers = { 1, 5, 3, 8, 2, 9, 4, 7, 6 };", Explanation = "💡 建立一個未排序的整數陣列作為資料來源。LINQ 可以用在任何實作 IEnumerable 介面的集合上（陣列、List、Dictionary 等）。" },
                    new() { Comment = "// Where：篩選大於 5 的數字", Code = "        var big = numbers.Where(n => n > 5);", Explanation = "💡 Where() 是篩選方法，類似 SQL 的 WHERE。n => n > 5 是 Lambda 表達式（匿名函式），n 是參數，n > 5 是條件。var 讓編譯器自動推斷型別。注意：LINQ 是延遲執行（Lazy），這行不會立即計算！" },
                    new() { Code = "        Console.WriteLine(\"大於5：\" + string.Join(\", \", big));", Explanation = "💡 當我們遍歷 big（透過 string.Join）時，LINQ 才會真正執行篩選。這就是「延遲執行」——只在需要結果時才計算。結果是 8, 9, 7, 6。" },
                    new() { Comment = "// OrderBy：排序", Code = "        var sorted = numbers.OrderBy(n => n);", Explanation = "💡 OrderBy() 用來排序，預設升序。n => n 表示「按元素本身排序」。如果要降序用 OrderByDescending()。可以鏈接使用：numbers.Where(n => n > 3).OrderBy(n => n)。類似 Python 的 sorted()。" },
                    new() { Code = "        Console.WriteLine(\"排序：\" + string.Join(\", \", sorted));", Explanation = "💡 輸出排序後的結果：1, 2, 3, 4, 5, 6, 7, 8, 9。注意原始 numbers 陣列不會被改變——LINQ 會產生新的序列。" },
                    new() { Comment = "// Select：轉換每個元素（乘以 2）", Code = "        var doubled = numbers.Select(n => n * 2);", Explanation = "💡 Select() 是轉換/映射方法，類似 Python 的 map() 或 JavaScript 的 .map()。它對每個元素套用一個函式，產生新的序列。n => n * 2 把每個數字乘以 2。" },
                    new() { Code = "        Console.WriteLine(\"乘2：\" + string.Join(\", \", doubled));", Explanation = "💡 輸出每個元素乘 2 的結果。LINQ 方法可以鏈式呼叫：numbers.Where(n => n > 3).Select(n => n * 2).OrderBy(n => n)，非常優雅！" },
                    new() { Comment = "// 聚合：Sum, Average, Max, Min", Code = "        Console.WriteLine($\"總和={numbers.Sum()}, 平均={numbers.Average():F1}\");", Explanation = "💡 LINQ 提供多種聚合方法：Sum() 求和、Average() 平均、Max() 最大值、Min() 最小值、Count() 計數。:F1 是格式化字串，表示保留 1 位小數。這些方法會立即執行（不是延遲的）。" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            // ===== JavaScript =====
            new() { Id = 9, Title = "JavaScript 基礎", Difficulty = "beginner", Description = "認識 JavaScript 的基本語法與函式",
                Lines = new List<CodeLine> {
                    new() { Comment = "// 用 console.log 在主控台印出訊息", Code = "console.log(\"Hello JavaScript!\");", Explanation = "console.log 是 JS 最常用的除錯工具，類似 C# 的 Console.WriteLine。括號內放要印出的內容。" },
                    new() { Comment = "// 用 let 宣告可變變數", Code = "let name = \"Mike\";", Explanation = "let 宣告的變數可以重新賦值。ES6 之後建議用 let 取代 var，因為 let 有區塊作用域，不容易產生 bug。" },
                    new() { Comment = "// 用 const 宣告不可變常數", Code = "const PI = 3.14159;", Explanation = "const 宣告後不能重新賦值，適合用在不會改變的值。注意：如果是物件或陣列，內容仍可修改，只是不能重新指向。" },
                    new() { Comment = "// var 是舊式宣告方式（不建議使用）", Code = "var age = 25;", Explanation = "var 是函式作用域，容易造成變數提升 (hoisting) 的問題。現代 JS 開發中應盡量避免使用 var。" },
                    new() { Comment = "// 用反引號 `` 建立模板字串", Code = "let greeting = `你好，${name}，你 ${age} 歲`;", Explanation = "模板字串用反引號包起來，${} 內可放變數或表達式。比用 + 串接字串更易讀，也支援多行文字。" },
                    new() { Comment = "// 印出模板字串結果", Code = "console.log(greeting);", Explanation = "將組合好的問候語印到主控台，確認模板字串是否正確組合。" },
                    new() { Comment = "// 用 function 關鍵字定義函式", Code = "function add(a, b) {", Explanation = "function 是傳統的函式宣告方式，會被提升 (hoisting)，所以可以在宣告前呼叫。參數放在括號內用逗號分隔。" },
                    new() { Code = "    return a + b;", Explanation = "return 回傳計算結果。如果沒有 return，函式會回傳 undefined。" },
                    new() { Code = "}" },
                    new() { Comment = "// 用箭頭函式 => 簡寫", Code = "const multiply = (a, b) => a * b;", Explanation = "箭頭函式是 ES6 的簡寫語法。單行可省略 {} 和 return。注意：箭頭函式沒有自己的 this，這在事件處理時很重要。" },
                    new() { Comment = "// 呼叫函式並印出結果", Code = "console.log(add(3, 5));", Explanation = "呼叫 add 函式傳入 3 和 5，結果 8 會被印出。函式呼叫時要注意參數的順序和數量。" },
                    new() { Comment = "// 呼叫箭頭函式", Code = "console.log(multiply(4, 6));", Explanation = "箭頭函式的呼叫方式和一般函式相同，只是定義方式不同。" },
                }
            },
            new() { Id = 10, Title = "JS DOM 操作", Difficulty = "intermediate", Description = "用 JavaScript 操作網頁元素",
                Lines = new List<CodeLine> {
                    new() { Comment = "// 用 getElementById 取得指定 id 的元素", Code = "let title = document.getElementById(\"title\");", Explanation = "getElementById 是最快的 DOM 查詢方法，傳入元素的 id 字串。如果找不到會回傳 null，使用前最好先檢查。" },
                    new() { Comment = "// 用 querySelector 取得第一個符合的元素", Code = "let btn = document.querySelector(\".btn-primary\");", Explanation = "querySelector 接受 CSS 選擇器語法，比 getElementById 更靈活。只回傳第一個符合的元素，要全部用 querySelectorAll。" },
                    new() { Comment = "// 用 innerHTML 修改元素的 HTML 內容", Code = "title.innerHTML = \"<strong>新標題</strong>\";", Explanation = "innerHTML 可以設定包含 HTML 標籤的內容。注意：直接放入使用者輸入可能造成 XSS 攻擊，不信任的內容應用 textContent。" },
                    new() { Comment = "// 用 textContent 安全地設定純文字", Code = "title.textContent = \"安全的純文字標題\";", Explanation = "textContent 只處理純文字，不會解析 HTML 標籤，比 innerHTML 更安全。效能也較好。" },
                    new() { Comment = "// 用 addEventListener 綁定點擊事件", Code = "btn.addEventListener(\"click\", function() {", Explanation = "addEventListener 是綁定事件的標準方式。第一個參數是事件名稱，第二個是回呼函式。比 onclick 屬性更好，因為可以綁定多個處理器。" },
                    new() { Code = "    alert(\"按鈕被點擊了！\");", Explanation = "alert 會彈出對話框。實務上通常用其他 UI 回饋方式取代 alert，因為它會阻塞頁面。" },
                    new() { Code = "});" },
                    new() { Comment = "// 用 createElement 建立新元素", Code = "let newDiv = document.createElement(\"div\");", Explanation = "createElement 在記憶體中建立新元素，但還沒加入頁面。要呼叫 appendChild 或 append 才會顯示。" },
                    new() { Comment = "// 設定新元素的 class 和內容", Code = "newDiv.className = \"card\";", Explanation = "className 設定元素的 CSS class。也可以用 classList.add() 來新增 class，較適合多個 class 的操作。" },
                    new() { Code = "newDiv.textContent = \"這是新建的卡片\";", Explanation = "設定新元素的文字內容。" },
                    new() { Comment = "// 用 appendChild 把新元素加到頁面中", Code = "document.body.appendChild(newDiv);", Explanation = "appendChild 將子元素加到父元素的最後面。如果要插入到特定位置，可以用 insertBefore 或 insertAdjacentElement。" },
                }
            },
            new() { Id = 11, Title = "JS 非同步：Promise 與 async/await", Difficulty = "advanced", Description = "掌握 JavaScript 非同步程式設計",
                Lines = new List<CodeLine> {
                    new() { Comment = "// setTimeout 延遲執行（非同步的基礎）", Code = "setTimeout(() => console.log(\"2秒後執行\"), 2000);", Explanation = "setTimeout 是最基本的非同步操作，第一個參數是要執行的函式，第二個是毫秒數。它不會阻塞後續程式碼的執行。" },
                    new() { Comment = "// 建立一個 Promise 物件", Code = "const myPromise = new Promise((resolve, reject) => {", Explanation = "Promise 代表一個非同步操作的最終結果。resolve 表示成功，reject 表示失敗。建立後立即開始執行。" },
                    new() { Code = "    let success = true;", Explanation = "模擬非同步操作的成功或失敗條件。" },
                    new() { Code = "    if (success) resolve(\"操作成功！\");", Explanation = "呼叫 resolve 表示 Promise 成功完成，傳入的值可以在 .then() 中取得。" },
                    new() { Code = "    else reject(\"操作失敗\");", Explanation = "呼叫 reject 表示 Promise 失敗，傳入的錯誤可以在 .catch() 中處理。" },
                    new() { Code = "});" },
                    new() { Comment = "// 用 .then() 和 .catch() 處理結果", Code = "myPromise.then(msg => console.log(msg)).catch(err => console.error(err));", Explanation = ".then() 處理成功結果，.catch() 處理錯誤。可以鏈式呼叫多個 .then()。常見錯誤是忘記加 .catch() 導致錯誤被吞掉。" },
                    new() { Comment = "// 用 async/await 語法（更易讀）", Code = "async function fetchData() {", Explanation = "async 關鍵字讓函式回傳 Promise。內部可以使用 await 暫停執行直到 Promise 完成。這讓非同步程式碼看起來像同步的。" },
                    new() { Comment = "// try/catch 處理非同步錯誤", Code = "    try {", Explanation = "async/await 中要用 try/catch 來捕捉錯誤，取代 .catch()。這和同步程式碼的錯誤處理方式一致。" },
                    new() { Comment = "// 用 fetch 呼叫 API", Code = "        const response = await fetch(\"https://api.example.com/data\");", Explanation = "fetch 是瀏覽器內建的 HTTP 請求方法。await 會等待回應完成。注意：fetch 只在網路錯誤時 reject，HTTP 404/500 不會。" },
                    new() { Comment = "// 將回應轉成 JSON", Code = "        const data = await response.json();", Explanation = "response.json() 也是非同步的，需要 await。它會解析 JSON 字串成 JavaScript 物件。" },
                    new() { Code = "        console.log(data);", Explanation = "印出從 API 取得的資料，方便除錯確認。" },
                    new() { Code = "    } catch (error) {" },
                    new() { Code = "        console.error(\"取得資料失敗：\", error);", Explanation = "捕捉到的錯誤會包含錯誤訊息，用 console.error 印出以便除錯。" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            // ===== jQuery =====
            new() { Id = 13, Title = "jQuery 入門", Difficulty = "beginner", Description = "學習 jQuery 簡化 DOM 操作",
                Lines = new List<CodeLine> {
                    new() { Comment = "// 等 DOM 載入完成後再執行", Code = "$(document).ready(function() {", Explanation = "$(document).ready 確保 DOM 完全載入後才執行程式碼。可以簡寫成 $(function(){})。這是 jQuery 程式的標準起手式。" },
                    new() { Comment = "// 用 $() 選取元素（類似 CSS 選擇器）", Code = "    let title = $(\"#title\");", Explanation = "$() 是 jQuery 的核心函式，接受 CSS 選擇器字串。# 選 id、. 選 class。回傳的是 jQuery 物件，不是原生 DOM 元素。" },
                    new() { Comment = "// 用 .html() 取得或設定 HTML 內容", Code = "    title.html(\"<em>新標題</em>\");", Explanation = ".html() 不傳參數是取值，傳參數是設值。類似原生的 innerHTML。注意 XSS 風險，不信任的內容用 .text()。" },
                    new() { Comment = "// 用 .css() 動態修改樣式", Code = "    title.css(\"color\", \"blue\");", Explanation = ".css() 可以傳兩個參數（屬性、值）或一個物件設定多個樣式。建議大量樣式變更用 .addClass() 取代。" },
                    new() { Comment = "// 用 .on() 綁定事件", Code = "    $(\".btn\").on(\"click\", function() {", Explanation = ".on() 是綁定事件的推薦方式，取代了舊版的 .bind() 和 .click()。也支援事件委派，適合動態產生的元素。" },
                    new() { Code = "        alert(\"按鈕被點擊！\");", Explanation = "事件處理函式中的 this 指向被點擊的 DOM 元素，用 $(this) 可以轉成 jQuery 物件操作。" },
                    new() { Code = "    });" },
                    new() { Comment = "// 用 .addClass() 新增 CSS class", Code = "    $(\"#box\").addClass(\"highlight\");", Explanation = ".addClass() 不會覆蓋原有的 class，只會新增。對應的 .removeClass() 移除 class，.toggleClass() 切換。" },
                    new() { Comment = "// .hide() 和 .show() 控制顯示隱藏", Code = "    $(\"#message\").hide();", Explanation = ".hide() 設定 display:none 隱藏元素。可以傳入動畫時間如 .hide(400)。對應 .show() 顯示，.toggle() 切換。" },
                    new() { Code = "    $(\"#message\").show();", Explanation = ".show() 將元素恢復為原本的 display 值。搭配 .hide() 可做簡單的顯示切換效果。" },
                    new() { Comment = "// 用 $.ajax 發送 HTTP 請求", Code = "    $.ajax({", Explanation = "$.ajax 是 jQuery 的 AJAX 方法，用來非同步取得伺服器資料。雖然現代開發多用 fetch，但在使用 jQuery 的專案中仍然常見。" },
                    new() { Code = "        url: \"/api/data\",", Explanation = "url 指定請求的目標網址。可以是相對路徑或絕對路徑。" },
                    new() { Code = "        method: \"GET\",", Explanation = "method 指定 HTTP 方法：GET（取得）、POST（新增）、PUT（更新）、DELETE（刪除）。" },
                    new() { Code = "        success: function(data) { console.log(data); }", Explanation = "success 回呼在請求成功時執行。data 參數是伺服器回傳的資料，jQuery 會自動解析 JSON。" },
                    new() { Code = "    });" },
                    new() { Code = "});" },
                }
            },
            // ===== HTML =====
            new() { Id = 14, Title = "HTML 基礎結構", Difficulty = "beginner", Description = "學習 HTML 網頁的基本架構與常用標籤",
                Lines = new List<CodeLine> {
                    new() { Comment = "<!-- 宣告文件類型為 HTML5 -->", Code = "<!DOCTYPE html>", Explanation = "<!DOCTYPE html> 告訴瀏覽器這是 HTML5 文件。必須放在最前面，不區分大小寫。少了它瀏覽器可能進入怪異模式 (quirks mode)。" },
                    new() { Comment = "<!-- html 是整個網頁的根元素 -->", Code = "<html lang=\"zh-TW\">", Explanation = "lang 屬性設定網頁語言，幫助搜尋引擎和螢幕閱讀器判斷內容語言。zh-TW 是繁體中文。" },
                    new() { Comment = "<!-- head 放網頁的設定資訊 -->", Code = "<head>", Explanation = "<head> 內的內容不會顯示在頁面上，而是網頁的中繼資料，包括編碼、標題、CSS 連結等。" },
                    new() { Comment = "<!-- 設定字元編碼為 UTF-8 -->", Code = "    <meta charset=\"UTF-8\">", Explanation = "UTF-8 編碼支援中文等多國語言。必須放在 <head> 的最前面，避免亂碼問題。" },
                    new() { Comment = "<!-- 設定瀏覽器標題列的文字 -->", Code = "    <title>我的網頁</title>", Explanation = "<title> 的文字會顯示在瀏覽器頁籤上，也是搜尋引擎結果的標題。每頁應該有獨特的標題。" },
                    new() { Code = "</head>" },
                    new() { Comment = "<!-- body 放網頁的可見內容 -->", Code = "<body>", Explanation = "<body> 內的所有內容會顯示在瀏覽器視窗中。每個 HTML 文件只能有一個 <body>。" },
                    new() { Comment = "<!-- h1 是最大的標題 -->", Code = "    <h1>歡迎光臨</h1>", Explanation = "HTML 有 h1 到 h6 六個層級的標題。h1 最重要，每頁建議只用一個。對 SEO 和無障礙功能很重要。" },
                    new() { Comment = "<!-- h3 是較小的標題 -->", Code = "    <h3>關於我們</h3>", Explanation = "標題層級要按順序使用，不要為了字體大小跳級。用 CSS 調整大小更正確。" },
                    new() { Comment = "<!-- p 是段落標籤 -->", Code = "    <p>這是一段介紹文字。</p>", Explanation = "<p> 標籤定義一個段落，瀏覽器會自動在段落前後加上間距。段落內不應包含其他區塊元素。" },
                    new() { Comment = "<!-- a 是超連結 -->", Code = "    <a href=\"https://example.com\">點我前往</a>", Explanation = "href 屬性指定連結目標。加上 target=\"_blank\" 可在新分頁開啟。外部連結建議加 rel=\"noopener noreferrer\" 增加安全性。" },
                    new() { Comment = "<!-- img 是圖片標籤（自閉合）-->", Code = "    <img src=\"photo.jpg\" alt=\"風景照片\">", Explanation = "src 指定圖片路徑，alt 是替代文字（圖片無法顯示時出現）。alt 對 SEO 和視障用戶很重要，不要省略。" },
                    new() { Comment = "<!-- ul 無序清單搭配 li 項目 -->", Code = "    <ul>", Explanation = "<ul> 是無序清單（圓點），<ol> 是有序清單（數字）。清單項目都用 <li> 包起來。" },
                    new() { Code = "        <li>項目一</li>", Explanation = "<li> 只能放在 <ul> 或 <ol> 內部。裡面可以放文字、連結或其他元素。" },
                    new() { Code = "    </ul>" },
                    new() { Comment = "<!-- form 表單搭配 input 和 button -->", Code = "    <form>", Explanation = "<form> 定義一個表單區域。action 屬性設定送出目標，method 設定 HTTP 方法（GET 或 POST）。" },
                    new() { Code = "        <input type=\"text\" placeholder=\"請輸入姓名\">", Explanation = "input 是表單輸入元素。type 決定輸入類型（text、email、password 等），placeholder 是提示文字。" },
                    new() { Code = "        <button type=\"submit\">送出</button>", Explanation = "type=\"submit\" 的按鈕會觸發表單送出。type=\"button\" 則不會，適合用 JS 處理。" },
                    new() { Code = "    </form>" },
                    new() { Code = "</body>" },
                    new() { Code = "</html>" },
                }
            },
            // ===== CSS =====
            new() { Id = 15, Title = "CSS 樣式基礎", Difficulty = "beginner", Description = "學習 CSS 選擇器與常用樣式屬性",
                Lines = new List<CodeLine> {
                    new() { Comment = "/* 選取 body 元素設定全域樣式 */", Code = "body {", Explanation = "CSS 規則由選擇器和宣告區塊組成。body 選擇器套用到整個頁面，常用來設定預設字型和背景色。" },
                    new() { Code = "    color: #333;", Explanation = "color 設定文字顏色。#333 是深灰色的十六進位色碼。也可以用 rgb()、hsl() 或顏色名稱。" },
                    new() { Code = "    background-color: #f5f5f5;", Explanation = "background-color 設定背景色。淺灰背景配深灰文字比純黑白組合更舒適。" },
                    new() { Code = "    font-size: 16px;", Explanation = "font-size 設定字體大小。16px 是瀏覽器預設大小。建議用 rem 單位（相對於根元素），更利於響應式設計。" },
                    new() { Code = "}" },
                    new() { Comment = "/* 用 class 選擇器設定卡片樣式 */", Code = ".card {", Explanation = ".card 選取所有 class=\"card\" 的元素。class 選擇器以 . 開頭，是最常用的選擇器。比 id 選擇器更靈活可重用。" },
                    new() { Code = "    padding: 20px;", Explanation = "padding 是元素內邊距（內容到邊框的距離）。可以分別設定四個方向：padding-top/right/bottom/left。" },
                    new() { Code = "    margin: 10px auto;", Explanation = "margin 是外邊距。兩個值時：上下 10px、左右 auto。margin: auto 可以讓區塊元素水平置中。" },
                    new() { Code = "    border: 1px solid #ddd;", Explanation = "border 簡寫設定邊框：寬度 樣式 顏色。solid 是實線，也可用 dashed（虛線）、dotted（點線）等。" },
                    new() { Code = "    border-radius: 8px;", Explanation = "border-radius 設定圓角。值越大越圓，設為 50% 可以變成圓形。現代 UI 設計常用圓角增加柔和感。" },
                    new() { Code = "    box-shadow: 0 2px 8px rgba(0,0,0,0.1);", Explanation = "box-shadow 加上陰影效果：水平偏移 垂直偏移 模糊半徑 顏色。rgba 的第四個值是透明度，0.1 是很淡的陰影。" },
                    new() { Code = "}" },
                    new() { Comment = "/* 用 Flexbox 做水平排列 */", Code = ".container {", Explanation = "Flexbox 是現代 CSS 排版的核心工具，比 float 更直覺好用。container 通常作為 flex 容器的 class 名稱。" },
                    new() { Code = "    display: flex;", Explanation = "display:flex 啟用彈性盒子排版。子元素預設會水平排列。這是目前做一維排版最推薦的方式。" },
                    new() { Code = "    justify-content: center;", Explanation = "justify-content 控制主軸（預設水平）的對齊方式。center 置中，space-between 平均分散，flex-start 靠左。" },
                    new() { Code = "    align-items: center;", Explanation = "align-items 控制交叉軸（預設垂直）的對齊方式。center 垂直置中，搭配 justify-content:center 可完美置中。" },
                    new() { Code = "}" },
                    new() { Comment = "/* 響應式設計：螢幕小於 768px 時 */", Code = "@media (max-width: 768px) {", Explanation = "@media 媒體查詢讓樣式根據螢幕大小變化。768px 是常見的平板斷點。行動優先設計用 min-width，桌面優先用 max-width。" },
                    new() { Code = "    .container { flex-direction: column; }", Explanation = "小螢幕時將水平排列改為垂直排列。flex-direction:column 讓 flex 子元素由上到下排列。" },
                    new() { Code = "}" },
                }
            },
            // ===== Vue 3 =====
            new() { Id = 16, Title = "Vue 3 快速入門", Difficulty = "intermediate", Description = "學習 Vue 3 Composition API 的核心概念",
                Lines = new List<CodeLine> {
                    new() { Comment = "// 從 vue 匯入必要的函式", Code = "import { createApp, ref, computed, watch } from 'vue';", Explanation = "Vue 3 的 Composition API 透過 import 匯入需要的功能。ref 管理響應式資料，computed 計算屬性，watch 監聽變化。" },
                    new() { Comment = "// 建立 Vue 應用程式", Code = "const app = createApp({", Explanation = "createApp 是 Vue 3 建立應用程式的入口。取代了 Vue 2 的 new Vue()。傳入一個元件選項物件。" },
                    new() { Comment = "// setup 函式是 Composition API 的核心", Code = "    setup() {", Explanation = "setup() 在元件建立時執行，取代了 data、methods、computed 等選項。所有邏輯集中在這裡，方便抽取共用邏輯。" },
                    new() { Comment = "// 用 ref() 建立響應式變數", Code = "        const count = ref(0);", Explanation = "ref() 將值包裝成響應式物件。在 JS 中要用 .value 存取，但在 template 中會自動解包。" },
                    new() { Comment = "// 用 ref() 建立響應式字串", Code = "        const name = ref('Mike');", Explanation = "ref 可以包裝任何型別：字串、數字、布林值、物件、陣列。當值改變時，畫面會自動更新。" },
                    new() { Comment = "// 用 computed 建立計算屬性", Code = "        const doubleCount = computed(() => count.value * 2);", Explanation = "computed 根據其他響應式資料自動計算。有快取機制，只在依賴改變時重新計算，比 methods 更有效率。" },
                    new() { Comment = "// 用 watch 監聽變數變化", Code = "        watch(count, (newVal, oldVal) => {", Explanation = "watch 監聽響應式資料的變化。回呼函式會收到新值和舊值。適合在資料變化時執行副作用（如 API 呼叫）。" },
                    new() { Code = "            console.log(`count 從 ${oldVal} 變為 ${newVal}`);", Explanation = "在 watch 回呼中可以做任何操作：發送請求、記錄日誌、更新其他狀態等。" },
                    new() { Code = "        });" },
                    new() { Comment = "// 定義方法", Code = "        const increment = () => count.value++;", Explanation = "在 setup 中定義的函式就是方法。修改 ref 要用 .value。這個函式可以在 template 中用 @click 綁定。" },
                    new() { Comment = "// 回傳要在 template 中使用的資料和方法", Code = "        return { count, name, doubleCount, increment };", Explanation = "setup 必須回傳 template 需要用到的所有變數和函式。沒有回傳的資料在 template 中無法使用。" },
                    new() { Code = "    }," },
                    new() { Comment = "// template 定義 HTML 結構", Code = "    template: `<div>", Explanation = "template 用反引號定義多行 HTML。{{ }} 雙大括號是插值語法，會自動更新。也可以用 .vue 單檔案元件。" },
                    new() { Code = "        <h1>{{ name }} 的計數器</h1>", Explanation = "{{ name }} 會自動解包 ref 的值並顯示。當 name 改變時，畫面會自動重新渲染。" },
                    new() { Code = "        <p v-if=\"count > 0\">目前計數：{{ count }}</p>", Explanation = "v-if 條件渲染：條件為 true 時才渲染元素。和 v-show 不同，v-if 會完全移除 DOM 元素。" },
                    new() { Code = "        <button @click=\"increment\">+1</button>", Explanation = "@click 是 v-on:click 的縮寫，綁定點擊事件。@ 是事件綁定的語法糖，常用事件還有 @input、@submit 等。" },
                    new() { Code = "    `" },
                    new() { Code = "});" },
                    new() { Comment = "// 掛載到 DOM 元素", Code = "app.mount('#app');", Explanation = "mount 將 Vue 應用掛載到指定的 DOM 元素上。#app 對應 HTML 中 id=\"app\" 的元素。" },
                }
            },
            // ===== React =====
            new() { Id = 17, Title = "React 快速入門", Difficulty = "intermediate", Description = "學習 React 函式元件與 Hooks",
                Lines = new List<CodeLine> {
                    new() { Comment = "// 從 react 匯入必要的 Hooks", Code = "import React, { useState, useEffect } from 'react';", Explanation = "React 用 Hooks 管理元件狀態和副作用。useState 管理狀態，useEffect 處理副作用（API 呼叫、計時器等）。" },
                    new() { Comment = "// 定義函式元件", Code = "function Counter() {", Explanation = "React 元件是回傳 JSX 的函式。函式名稱必須大寫開頭，這是 React 區分元件和 HTML 元素的方式。" },
                    new() { Comment = "// 用 useState 建立狀態變數", Code = "    const [count, setCount] = useState(0);", Explanation = "useState 回傳 [狀態值, 設定函式] 的陣列。用解構賦值取出。直接修改 count 無效，必須用 setCount 才會觸發重新渲染。" },
                    new() { Comment = "// 建立第二個狀態", Code = "    const [name, setName] = useState('Mike');", Explanation = "一個元件可以有多個 useState。每個 state 獨立管理。Hook 的呼叫順序必須固定，不能放在 if 或迴圈中。" },
                    new() { Comment = "// 定義點擊事件處理函式", Code = "    const handleClick = () => {", Explanation = "事件處理函式命名慣例以 handle 開頭。在 JSX 中用 onClick={handleClick} 綁定，不要加括號 ()，否則會立即執行。" },
                    new() { Code = "        setCount(count + 1);", Explanation = "setCount 更新狀態並觸發重新渲染。如果新狀態依賴舊狀態，建議用函式寫法：setCount(prev => prev + 1)。" },
                    new() { Code = "    };" },
                    new() { Comment = "// useEffect 處理副作用", Code = "    useEffect(() => {", Explanation = "useEffect 在元件渲染後執行。類似 class 元件的 componentDidMount + componentDidUpdate。第二個參數控制執行時機。" },
                    new() { Code = "        document.title = `計數：${count}`;", Explanation = "這裡在 count 改變時更新頁面標題。useEffect 適合做 DOM 操作、API 呼叫、訂閱等副作用。" },
                    new() { Code = "    }, [count]);", Explanation = "[count] 是依賴陣列。只有 count 改變時才重新執行。空陣列 [] 表示只執行一次，省略則每次渲染都執行。" },
                    new() { Comment = "// 回傳 JSX（React 的 HTML 語法）", Code = "    return (", Explanation = "JSX 是 JavaScript 的語法擴展，看起來像 HTML 但其實是 JS。多行 JSX 要用括號包起來，且必須有一個根元素。" },
                    new() { Code = "        <div>", Explanation = "JSX 只能有一個根元素。如果不想多包一層 div，可以用 <></> (Fragment)。" },
                    new() { Code = "            <h1>{name} 的計數器</h1>", Explanation = "JSX 中用 {} 嵌入 JavaScript 表達式。注意是 {} 不是 {{}}（那是 Vue 的語法）。" },
                    new() { Comment = "// 條件渲染：用 && 或三元運算子", Code = "            {count > 0 && <p>目前計數：{count}</p>}", Explanation = "React 的條件渲染用 JS 邏輯運算子。&& 前面為 true 才渲染後面。也可以用三元運算子 condition ? a : b。" },
                    new() { Code = "            <button onClick={handleClick}>+1</button>", Explanation = "JSX 的事件用 camelCase 命名（onClick 不是 onclick）。傳入函式參考，不是字串。" },
                    new() { Code = "        </div>" },
                    new() { Code = "    );" },
                    new() { Code = "}" },
                }
            },
            // ===== Angular =====
            new() { Id = 18, Title = "Angular 快速入門", Difficulty = "intermediate", Description = "學習 Angular 元件的基本結構",
                Lines = new List<CodeLine> {
                    new() { Comment = "// 匯入 Angular 核心裝飾器和介面", Code = "import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';", Explanation = "Angular 用 TypeScript 開發。Component 裝飾器定義元件，Input/Output 處理父子元件通訊，OnInit 是生命週期介面。" },
                    new() { Comment = "// @Component 裝飾器定義元件的中繼資料", Code = "@Component({", Explanation = "@Component 是 Angular 最重要的裝飾器。它告訴 Angular 這個 class 是一個元件，並設定模板、樣式和選擇器。" },
                    new() { Code = "    selector: 'app-counter',", Explanation = "selector 定義元件在 HTML 中的標籤名稱。用 <app-counter></app-counter> 來使用。慣例是 app- 前綴加功能名稱。" },
                    new() { Code = "    template: `", Explanation = "template 用反引號寫內聯模板。較大的模板建議用 templateUrl 指向獨立的 .html 檔案。" },
                    new() { Comment = "<!-- Angular 的插值語法 -->", Code = "        <h1>{{ title }}</h1>", Explanation = "{{ }} 雙大括號是 Angular 的插值綁定，會自動將元件屬性的值渲染到畫面上，並在值改變時自動更新。" },
                    new() { Comment = "<!-- *ngIf 條件渲染 -->", Code = "        <p *ngIf=\"count > 0\">計數：{{ count }}</p>", Explanation = "*ngIf 是結構型指令，條件為 true 才渲染元素。* 號是語法糖，Angular 會將它展開為 ng-template。" },
                    new() { Comment = "<!-- *ngFor 迴圈渲染 -->", Code = "        <li *ngFor=\"let item of items\">{{ item }}</li>", Explanation = "*ngFor 遍歷陣列渲染列表。let item of items 類似 for...of 迴圈。建議加上 trackBy 提升大列表的效能。" },
                    new() { Comment = "<!-- (click) 事件綁定 -->", Code = "        <button (click)=\"increment()\">+1</button>", Explanation = "(click) 是事件綁定語法，括號表示從模板到元件的資料流。對應的 [property] 是屬性綁定，方向相反。" },
                    new() { Comment = "<!-- [disabled] 屬性綁定 -->", Code = "        <button [disabled]=\"count >= 10\">最多10</button>", Explanation = "[disabled] 用方括號做屬性綁定，將元件的表達式結果綁定到 DOM 屬性。當 count >= 10 時按鈕會被停用。" },
                    new() { Code = "    `" },
                    new() { Code = "})" },
                    new() { Comment = "// 元件類別實作 OnInit 生命週期介面", Code = "export class CounterComponent implements OnInit {", Explanation = "implements OnInit 表示這個元件要實作 ngOnInit 方法。export 讓其他模組可以匯入使用這個元件。" },
                    new() { Comment = "// @Input 接收父元件傳入的資料", Code = "    @Input() title: string = '計數器';", Explanation = "@Input() 裝飾器讓父元件透過屬性綁定傳資料進來。例如 <app-counter [title]=\"'自訂標題'\">。" },
                    new() { Comment = "// @Output 發送事件給父元件", Code = "    @Output() countChanged = new EventEmitter<number>();", Explanation = "@Output() 搭配 EventEmitter 讓子元件發送事件給父元件。父元件用 (countChanged)=\"handler($event)\" 接收。" },
                    new() { Code = "    count = 0;", Explanation = "元件的屬性直接宣告即可，Angular 會自動追蹤變化並更新畫面。不需要像 React 用 setState。" },
                    new() { Code = "    items = ['蘋果', '香蕉', '橘子'];", Explanation = "陣列屬性搭配 *ngFor 在模板中渲染列表。修改陣列（push、splice 等）後畫面會自動更新。" },
                    new() { Comment = "// ngOnInit 在元件初始化時執行", Code = "    ngOnInit() {", Explanation = "ngOnInit 在元件的 @Input 屬性初始化完成後呼叫，適合放初始化邏輯和 API 呼叫。比 constructor 更適合做初始化。" },
                    new() { Code = "        console.log('元件已初始化');", Explanation = "在 ngOnInit 中可以存取 @Input 的值，而 constructor 中可能還沒準備好。" },
                    new() { Code = "    }" },
                    new() { Comment = "// 方法：增加計數並發送事件", Code = "    increment() {", Explanation = "元件方法直接定義在 class 中。模板中用 (click)=\"increment()\" 呼叫。" },
                    new() { Code = "        this.count++;", Explanation = "Angular 中用 this 存取元件屬性。Angular 的變更偵測會自動更新畫面，不需要手動通知。" },
                    new() { Code = "        this.countChanged.emit(this.count);", Explanation = "emit() 發送事件給父元件。傳入的值可以在父元件的事件處理器中用 $event 取得。" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            // ===== SQL =====
            new() { Id = 19, Title = "SQL 基礎查詢", Difficulty = "beginner", Description = "學習 SQL 的基本查詢語法",
                Lines = new List<CodeLine> {
                    new() { Comment = "-- 從 students 表格選取所有欄位", Code = "SELECT * FROM students;", Explanation = "SELECT * 選取所有欄位，FROM 指定表格。實務上建議明確列出需要的欄位，避免用 * 以提升效能和可讀性。" },
                    new() { Comment = "-- 只選取特定欄位", Code = "SELECT name, age, grade FROM students;", Explanation = "列出欄位名稱用逗號分隔。只取需要的欄位可以減少資料傳輸量，尤其在表格欄位很多時效果明顯。" },
                    new() { Comment = "-- 用 WHERE 篩選條件", Code = "SELECT * FROM students WHERE age >= 18;", Explanation = "WHERE 放在 FROM 之後，設定篩選條件。支援 =、!=、>、<、>=、<=、LIKE、IN、BETWEEN 等運算子。" },
                    new() { Comment = "-- 用 AND/OR 組合多個條件", Code = "SELECT * FROM students WHERE age >= 18 AND grade = 'A';", Explanation = "AND 要兩個條件都成立，OR 只要一個成立。多個條件時建議用括號明確優先順序，避免邏輯錯誤。" },
                    new() { Comment = "-- 用 ORDER BY 排序結果", Code = "SELECT * FROM students ORDER BY age DESC;", Explanation = "ORDER BY 排序結果。ASC 升冪（預設），DESC 降冪。可以多欄位排序：ORDER BY grade ASC, age DESC。" },
                    new() { Comment = "-- 用 LIMIT 限制回傳筆數", Code = "SELECT * FROM students ORDER BY grade LIMIT 10;", Explanation = "LIMIT 限制最多回傳幾筆。分頁常用 LIMIT 搭配 OFFSET：LIMIT 10 OFFSET 20 取第 21-30 筆。" },
                    new() { Comment = "-- 用 COUNT 計算筆數", Code = "SELECT COUNT(*) AS total FROM students;", Explanation = "COUNT(*) 計算總筆數，AS 設定欄位別名。也可以用 COUNT(column) 只計算該欄位非 NULL 的筆數。" },
                    new() { Comment = "-- 用 GROUP BY 分組統計", Code = "SELECT grade, COUNT(*) AS count FROM students GROUP BY grade;", Explanation = "GROUP BY 按指定欄位分組。SELECT 中非聚合函式的欄位都必須出現在 GROUP BY 中，否則會報錯。" },
                    new() { Comment = "-- 用 HAVING 篩選分組結果", Code = "SELECT grade, COUNT(*) AS count FROM students GROUP BY grade HAVING count > 5;", Explanation = "HAVING 是對 GROUP BY 分組後的結果做篩選。WHERE 篩選原始資料，HAVING 篩選聚合結果。兩者時機不同。" },
                    new() { Comment = "-- 用 AS 設定別名讓結果更易讀", Code = "SELECT name AS 姓名, age AS 年齡 FROM students;", Explanation = "AS 可以為欄位或表格設定別名。中文別名讓結果更易讀。表格別名在 JOIN 時特別有用，可以簡化語法。" },
                }
            },
            new() { Id = 20, Title = "SQL 進階：JOIN 與子查詢", Difficulty = "intermediate", Description = "學習多表格查詢與資料操作",
                Lines = new List<CodeLine> {
                    new() { Comment = "-- INNER JOIN：取兩表交集", Code = "SELECT s.name, c.course_name FROM students s INNER JOIN courses c ON s.id = c.student_id;", Explanation = "INNER JOIN 只回傳兩個表格都有匹配的資料。s 和 c 是表格別名，ON 指定關聯條件。這是最常用的 JOIN 類型。" },
                    new() { Comment = "-- LEFT JOIN：保留左表所有資料", Code = "SELECT s.name, c.course_name FROM students s LEFT JOIN courses c ON s.id = c.student_id;", Explanation = "LEFT JOIN 保留左表（students）所有資料，右表沒匹配的欄位填 NULL。適合找「有/沒有選課的學生」。" },
                    new() { Comment = "-- 多個 JOIN 條件", Code = "SELECT s.name, c.name, g.score FROM students s JOIN enrollments e ON s.id = e.student_id JOIN courses c ON e.course_id = c.id;", Explanation = "可以連續 JOIN 多個表格。每個 JOIN 都要有 ON 條件。複雜查詢時建議用表格別名讓語法更清楚。" },
                    new() { Comment = "-- INSERT INTO 新增資料", Code = "INSERT INTO students (name, age, grade) VALUES ('小明', 20, 'B');", Explanation = "INSERT INTO 指定表格和欄位，VALUES 提供對應的值。欄位和值的順序必須一致。字串用單引號包起來。" },
                    new() { Comment = "-- UPDATE 修改資料", Code = "UPDATE students SET grade = 'A', age = 21 WHERE name = '小明';", Explanation = "UPDATE SET 修改指定欄位。一定要加 WHERE 條件！忘記 WHERE 會更新整個表格的所有資料，這是非常常見的災難性錯誤。" },
                    new() { Comment = "-- DELETE 刪除資料", Code = "DELETE FROM students WHERE grade = 'F';", Explanation = "DELETE FROM 刪除符合條件的資料。和 UPDATE 一樣，一定要加 WHERE！沒有 WHERE 的 DELETE 會清空整個表格。" },
                    new() { Comment = "-- CREATE TABLE 建立新表格", Code = "CREATE TABLE courses (id INT PRIMARY KEY, name VARCHAR(100) NOT NULL, credits INT DEFAULT 3);", Explanation = "CREATE TABLE 定義表格結構。PRIMARY KEY 是主鍵（唯一識別），NOT NULL 不允許空值，DEFAULT 設預設值。" },
                    new() { Comment = "-- 子查詢：查詢中的查詢", Code = "SELECT * FROM students WHERE age > (SELECT AVG(age) FROM students);", Explanation = "子查詢放在括號中，先執行內層查詢再用結果做外層查詢。這裡找出年齡大於平均的學生。子查詢也可以用在 IN、EXISTS 中。" },
                    new() { Comment = "-- 建立索引加速查詢", Code = "CREATE INDEX idx_student_name ON students (name);", Explanation = "INDEX 像書的目錄，加速 WHERE 和 JOIN 的查詢。在常用的查詢條件欄位建立索引。但太多索引會拖慢寫入速度。" },
                }
            },
        };
    }
}

public class CodeLesson
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Difficulty { get; set; } = "beginner";
    public string Description { get; set; } = "";
    public List<CodeLine> Lines { get; set; } = new();
}

public class CodeLine
{
    public string? Comment { get; set; }
    public string Code { get; set; } = "";
    public string? Explanation { get; set; }
}
