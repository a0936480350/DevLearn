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
                    new() { Comment = "// 引入 System 命名空間，這樣才能使用 Console", Code = "using System;" },
                    new() { Comment = "// 定義一個類別，C# 的程式碼都要放在類別裡面", Code = "class Program {" },
                    new() { Comment = "// Main 是程式的進入點，程式從這裡開始執行", Code = "    static void Main() {" },
                    new() { Comment = "// 用 Console.WriteLine 在螢幕上印出文字", Code = "        Console.WriteLine(\"Hello World!\");" },
                    new() { Comment = "// 關閉 Main 方法的大括號", Code = "    }" },
                    new() { Comment = "// 關閉 Program 類別的大括號", Code = "}" },
                }
            },
            new() { Id = 2, Title = "變數與型別", Difficulty = "beginner", Description = "宣告變數並使用不同資料型別",
                Lines = new List<CodeLine> {
                    new() { Comment = "// 引入必要的命名空間", Code = "using System;" },
                    new() { Comment = "// 建立程式的主類別", Code = "class Program {" },
                    new() { Comment = "// 程式進入點", Code = "    static void Main() {" },
                    new() { Comment = "// 用 int 宣告整數變數，存放年齡", Code = "        int age = 25;" },
                    new() { Comment = "// 用 string 宣告字串變數，存放名字", Code = "        string name = \"Mike\";" },
                    new() { Comment = "// 用 double 宣告浮點數，存放身高", Code = "        double height = 175.5;" },
                    new() { Comment = "// 用 bool 宣告布林值，true 或 false", Code = "        bool isStudent = true;" },
                    new() { Comment = "// 用字串插值 $\"\" 組合變數輸出", Code = "        Console.WriteLine($\"姓名：{name}，年齡：{age}\");" },
                    new() { Comment = "// 輸出身高和學生狀態", Code = "        Console.WriteLine($\"身高：{height}，是學生：{isStudent}\");" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 3, Title = "if / else 條件判斷", Difficulty = "beginner", Description = "學習條件分支邏輯",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;" },
                    new() { Code = "class Program {" },
                    new() { Code = "    static void Main() {" },
                    new() { Comment = "// 宣告一個分數變數", Code = "        int score = 85;" },
                    new() { Comment = "// if 判斷：分數 >= 90 是 A", Code = "        if (score >= 90) {" },
                    new() { Code = "            Console.WriteLine(\"等級：A\");" },
                    new() { Comment = "// else if：分數 >= 80 是 B", Code = "        } else if (score >= 80) {" },
                    new() { Code = "            Console.WriteLine(\"等級：B\");" },
                    new() { Comment = "// else if：分數 >= 70 是 C", Code = "        } else if (score >= 70) {" },
                    new() { Code = "            Console.WriteLine(\"等級：C\");" },
                    new() { Comment = "// 其他情況都是 F", Code = "        } else {" },
                    new() { Code = "            Console.WriteLine(\"等級：F\");" },
                    new() { Code = "        }" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 4, Title = "for 迴圈", Difficulty = "beginner", Description = "用迴圈重複執行程式碼",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;" },
                    new() { Code = "class Program {" },
                    new() { Code = "    static void Main() {" },
                    new() { Comment = "// for 迴圈：i 從 1 到 5，每次 +1", Code = "        for (int i = 1; i <= 5; i++) {" },
                    new() { Comment = "// 印出目前的 i 值", Code = "            Console.WriteLine($\"第 {i} 次\");" },
                    new() { Code = "        }" },
                    new() { Comment = "// 計算 1+2+3+...+10 的總和", Code = "        int sum = 0;" },
                    new() { Comment = "// 用迴圈累加", Code = "        for (int i = 1; i <= 10; i++) {" },
                    new() { Code = "            sum += i;" },
                    new() { Code = "        }" },
                    new() { Comment = "// 輸出結果", Code = "        Console.WriteLine($\"1到10的總和：{sum}\");" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 5, Title = "陣列與 foreach", Difficulty = "intermediate", Description = "使用陣列存放多筆資料",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;" },
                    new() { Code = "class Program {" },
                    new() { Code = "    static void Main() {" },
                    new() { Comment = "// 宣告字串陣列，存放水果名稱", Code = "        string[] fruits = { \"蘋果\", \"香蕉\", \"橘子\", \"芒果\" };" },
                    new() { Comment = "// 用 foreach 遍歷每個水果", Code = "        foreach (string fruit in fruits) {" },
                    new() { Code = "            Console.WriteLine(fruit);" },
                    new() { Code = "        }" },
                    new() { Comment = "// 宣告整數陣列", Code = "        int[] numbers = { 3, 7, 1, 9, 4 };" },
                    new() { Comment = "// 用 Array.Sort 排序", Code = "        Array.Sort(numbers);" },
                    new() { Comment = "// 輸出排序後的結果", Code = "        Console.WriteLine(string.Join(\", \", numbers));" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 6, Title = "方法（函式）", Difficulty = "intermediate", Description = "把程式碼包成可重複使用的方法",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;" },
                    new() { Code = "class Program {" },
                    new() { Comment = "// 定義一個計算加法的方法，接收兩個 int 參數", Code = "    static int Add(int a, int b) {" },
                    new() { Comment = "// return 回傳計算結果", Code = "        return a + b;" },
                    new() { Code = "    }" },
                    new() { Comment = "// 定義一個打招呼方法，接收名字參數", Code = "    static void Greet(string name) {" },
                    new() { Code = "        Console.WriteLine($\"你好，{name}！\");" },
                    new() { Code = "    }" },
                    new() { Code = "    static void Main() {" },
                    new() { Comment = "// 呼叫 Add 方法並存放結果", Code = "        int result = Add(3, 5);" },
                    new() { Code = "        Console.WriteLine($\"3 + 5 = {result}\");" },
                    new() { Comment = "// 呼叫 Greet 方法", Code = "        Greet(\"Mike\");" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 7, Title = "類別與物件", Difficulty = "intermediate", Description = "OOP 物件導向基礎",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;" },
                    new() { Comment = "// 定義一個 Dog 類別", Code = "class Dog {" },
                    new() { Comment = "// 屬性：狗的名字", Code = "    public string Name { get; set; }" },
                    new() { Comment = "// 屬性：狗的年齡", Code = "    public int Age { get; set; }" },
                    new() { Comment = "// 方法：狗叫", Code = "    public void Bark() {" },
                    new() { Code = "        Console.WriteLine($\"{Name} 說：汪汪！\");" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                    new() { Code = "class Program {" },
                    new() { Code = "    static void Main() {" },
                    new() { Comment = "// 用 new 建立 Dog 物件", Code = "        Dog myDog = new Dog();" },
                    new() { Comment = "// 設定屬性值", Code = "        myDog.Name = \"小白\";" },
                    new() { Code = "        myDog.Age = 3;" },
                    new() { Comment = "// 呼叫方法", Code = "        myDog.Bark();" },
                    new() { Code = "        Console.WriteLine($\"{myDog.Name} 今年 {myDog.Age} 歲\");" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
                }
            },
            new() { Id = 8, Title = "LINQ 查詢", Difficulty = "advanced", Description = "用 LINQ 優雅地處理資料集合",
                Lines = new List<CodeLine> {
                    new() { Code = "using System;" },
                    new() { Comment = "// 引入 LINQ 命名空間", Code = "using System.Linq;" },
                    new() { Code = "class Program {" },
                    new() { Code = "    static void Main() {" },
                    new() { Comment = "// 建立一個數字清單", Code = "        int[] numbers = { 1, 5, 3, 8, 2, 9, 4, 7, 6 };" },
                    new() { Comment = "// Where：篩選大於 5 的數字", Code = "        var big = numbers.Where(n => n > 5);" },
                    new() { Code = "        Console.WriteLine(\"大於5：\" + string.Join(\", \", big));" },
                    new() { Comment = "// OrderBy：排序", Code = "        var sorted = numbers.OrderBy(n => n);" },
                    new() { Code = "        Console.WriteLine(\"排序：\" + string.Join(\", \", sorted));" },
                    new() { Comment = "// Select：轉換每個元素（乘以 2）", Code = "        var doubled = numbers.Select(n => n * 2);" },
                    new() { Code = "        Console.WriteLine(\"乘2：\" + string.Join(\", \", doubled));" },
                    new() { Comment = "// 聚合：Sum, Average, Max, Min", Code = "        Console.WriteLine($\"總和={numbers.Sum()}, 平均={numbers.Average():F1}\");" },
                    new() { Code = "    }" },
                    new() { Code = "}" },
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
}
