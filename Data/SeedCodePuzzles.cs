using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedCodePuzzles
{
    public static List<CodePuzzle> GetPuzzles()
    {
        return new List<CodePuzzle>
        {
            // ===== 初級 (Beginner) =====
            new CodePuzzle
            {
                Title = "變數宣告基礎",
                FullCode = "int age = 25;\nstring name = \"小明\";\nbool isStudent = true;\nConsole.WriteLine(name);",
                BlankPositionsJson = "[{\"start\":0,\"end\":3,\"answer\":\"int\"},{\"start\":16,\"end\":22,\"answer\":\"string\"},{\"start\":39,\"end\":43,\"answer\":\"bool\"}]",
                Language = "csharp",
                Difficulty = "beginner",
                Category = "變數型別",
                Hint = "C# 中常見的基本型別有 int（整數）、string（字串）、bool（布林值）"
            },
            new CodePuzzle
            {
                Title = "主控台輸出",
                FullCode = "string greeting = \"你好世界\";\nConsole.WriteLine(greeting);\nConsole.Write(\"不換行輸出\");",
                BlankPositionsJson = "[{\"start\":30,\"end\":47,\"answer\":\"Console.WriteLine\"},{\"start\":60,\"end\":73,\"answer\":\"Console.Write\"}]",
                Language = "csharp",
                Difficulty = "beginner",
                Category = "基本語法",
                Hint = "Console.WriteLine 會換行，Console.Write 不會換行"
            },
            new CodePuzzle
            {
                Title = "條件判斷",
                FullCode = "int score = 85;\nif (score >= 60)\n{\n    Console.WriteLine(\"及格\");\n}\nelse\n{\n    Console.WriteLine(\"不及格\");\n}",
                BlankPositionsJson = "[{\"start\":17,\"end\":19,\"answer\":\"if\"},{\"start\":35,\"end\":37,\"answer\":\">=\"},{\"start\":66,\"end\":70,\"answer\":\"else\"}]",
                Language = "csharp",
                Difficulty = "beginner",
                Category = "流程控制",
                Hint = "if/else 用於條件判斷，>= 表示大於等於"
            },
            new CodePuzzle
            {
                Title = "方法回傳值",
                FullCode = "static int Add(int a, int b)\n{\n    return a + b;\n}\nint result = Add(3, 5);",
                BlankPositionsJson = "[{\"start\":7,\"end\":10,\"answer\":\"int\"},{\"start\":35,\"end\":41,\"answer\":\"return\"},{\"start\":55,\"end\":58,\"answer\":\"int\"}]",
                Language = "csharp",
                Difficulty = "beginner",
                Category = "方法定義",
                Hint = "方法需要指定回傳型別，使用 return 關鍵字回傳值"
            },
            new CodePuzzle
            {
                Title = "迴圈基礎",
                FullCode = "for (int i = 0; i < 10; i++)\n{\n    Console.WriteLine(i);\n}\nwhile (true)\n{\n    break;\n}",
                BlankPositionsJson = "[{\"start\":0,\"end\":3,\"answer\":\"for\"},{\"start\":16,\"end\":17,\"answer\":\"<\"},{\"start\":62,\"end\":67,\"answer\":\"while\"},{\"start\":80,\"end\":85,\"answer\":\"break\"}]",
                Language = "csharp",
                Difficulty = "beginner",
                Category = "迴圈",
                Hint = "for 迴圈用於已知次數的迴圈，while 迴圈用於條件式迴圈，break 跳出迴圈"
            },

            // ===== 中級 (Intermediate) =====
            new CodePuzzle
            {
                Title = "LINQ 查詢方法",
                FullCode = "var numbers = new List<int> { 1, 2, 3, 4, 5 };\nvar evens = numbers.Where(n => n % 2 == 0);\nvar doubled = evens.Select(n => n * 2);\nvar sorted = doubled.OrderBy(n => n);",
                BlankPositionsJson = "[{\"start\":69,\"end\":74,\"answer\":\"Where\"},{\"start\":98,\"end\":104,\"answer\":\"Select\"},{\"start\":123,\"end\":130,\"answer\":\"OrderBy\"}]",
                Language = "csharp",
                Difficulty = "intermediate",
                Category = "LINQ",
                Hint = "LINQ 三大常用方法：Where（篩選）、Select（轉換）、OrderBy（排序）"
            },
            new CodePuzzle
            {
                Title = "非同步程式設計",
                FullCode = "async Task<string> GetDataAsync()\n{\n    var client = new HttpClient();\n    string result = await client.GetStringAsync(\"https://api.example.com\");\n    return result;\n}",
                BlankPositionsJson = "[{\"start\":0,\"end\":5,\"answer\":\"async\"},{\"start\":6,\"end\":18,\"answer\":\"Task<string>\"},{\"start\":92,\"end\":97,\"answer\":\"await\"}]",
                Language = "csharp",
                Difficulty = "intermediate",
                Category = "非同步",
                Hint = "async 標記非同步方法，Task<T> 為回傳型別，await 等待非同步操作完成"
            },
            new CodePuzzle
            {
                Title = "泛型集合",
                FullCode = "List<string> names = new List<string>();\nnames.Add(\"小明\");\nDictionary<string, int> ages = new Dictionary<string, int>();\nages.Add(\"小明\", 25);",
                BlankPositionsJson = "[{\"start\":0,\"end\":12,\"answer\":\"List<string>\"},{\"start\":60,\"end\":82,\"answer\":\"Dictionary<string, int>\"}]",
                Language = "csharp",
                Difficulty = "intermediate",
                Category = "泛型",
                Hint = "List<T> 是泛型列表，Dictionary<TKey, TValue> 是泛型字典"
            },
            new CodePuzzle
            {
                Title = "介面與繼承",
                FullCode = "interface IAnimal\n{\n    string Name { get; set; }\n    void Speak();\n}\nclass Dog : IAnimal\n{\n    public string Name { get; set; } = \"\";\n    public void Speak() => Console.WriteLine(\"汪汪\");\n}",
                BlankPositionsJson = "[{\"start\":0,\"end\":9,\"answer\":\"interface\"},{\"start\":69,\"end\":74,\"answer\":\"class\"},{\"start\":81,\"end\":88,\"answer\":\"IAnimal\"}]",
                Language = "csharp",
                Difficulty = "intermediate",
                Category = "物件導向",
                Hint = "interface 定義介面，class 定義類別，使用 : 實作介面"
            },
            new CodePuzzle
            {
                Title = "例外處理",
                FullCode = "try\n{\n    int result = int.Parse(\"abc\");\n}\ncatch (FormatException ex)\n{\n    Console.WriteLine(ex.Message);\n}\nfinally\n{\n    Console.WriteLine(\"結束\");\n}",
                BlankPositionsJson = "[{\"start\":0,\"end\":3,\"answer\":\"try\"},{\"start\":39,\"end\":44,\"answer\":\"catch\"},{\"start\":46,\"end\":61,\"answer\":\"FormatException\"},{\"start\":95,\"end\":102,\"answer\":\"finally\"}]",
                Language = "csharp",
                Difficulty = "intermediate",
                Category = "例外處理",
                Hint = "try 包裹可能出錯的程式碼，catch 捕獲特定例外，finally 無論如何都會執行"
            },

            // ===== 高級 (Advanced) =====
            new CodePuzzle
            {
                Title = "依賴注入註冊",
                FullCode = "builder.Services.AddScoped<IUserService, UserService>();\nbuilder.Services.AddSingleton<ICacheService, CacheService>();\nbuilder.Services.AddTransient<IEmailService, EmailService>();",
                BlankPositionsJson = "[{\"start\":18,\"end\":26,\"answer\":\"AddScoped\"},{\"start\":75,\"end\":87,\"answer\":\"AddSingleton\"},{\"start\":136,\"end\":148,\"answer\":\"AddTransient\"}]",
                Language = "csharp",
                Difficulty = "advanced",
                Category = "依賴注入",
                Hint = "AddScoped（每次請求一個實例）、AddSingleton（全域唯一實例）、AddTransient（每次注入新實例）"
            },
            new CodePuzzle
            {
                Title = "中介軟體管線",
                FullCode = "app.UseHttpsRedirection();\napp.UseStaticFiles();\napp.UseRouting();\napp.UseAuthentication();\napp.UseAuthorization();\napp.MapControllers();",
                BlankPositionsJson = "[{\"start\":4,\"end\":24,\"answer\":\"UseHttpsRedirection\"},{\"start\":33,\"end\":47,\"answer\":\"UseStaticFiles\"},{\"start\":56,\"end\":76,\"answer\":\"UseAuthentication\"},{\"start\":84,\"end\":100,\"answer\":\"UseAuthorization\"}]",
                Language = "csharp",
                Difficulty = "advanced",
                Category = "中介軟體",
                Hint = "ASP.NET Core 中介軟體的順序很重要：先驗證身份（Authentication），再授權（Authorization）"
            },
            new CodePuzzle
            {
                Title = "EF Core 關聯查詢",
                FullCode = "var orders = await _context.Orders\n    .Include(o => o.Customer)\n    .ThenInclude(c => c.Address)\n    .Where(o => o.Total > 1000)\n    .OrderByDescending(o => o.OrderDate)\n    .ToListAsync();",
                BlankPositionsJson = "[{\"start\":39,\"end\":46,\"answer\":\"Include\"},{\"start\":68,\"end\":79,\"answer\":\"ThenInclude\"},{\"start\":131,\"end\":149,\"answer\":\"OrderByDescending\"},{\"start\":170,\"end\":181,\"answer\":\"ToListAsync\"}]",
                Language = "csharp",
                Difficulty = "advanced",
                Category = "EF Core",
                Hint = "Include 載入關聯資料，ThenInclude 載入巢狀關聯，OrderByDescending 降冪排序"
            },
            new CodePuzzle
            {
                Title = "自訂中介軟體",
                FullCode = "public class LoggingMiddleware\n{\n    private readonly RequestDelegate _next;\n    public LoggingMiddleware(RequestDelegate next)\n    {\n        _next = next;\n    }\n    public async Task InvokeAsync(HttpContext context)\n    {\n        Console.WriteLine($\"Request: {context.Request.Path}\");\n        await _next(context);\n    }\n}",
                BlankPositionsJson = "[{\"start\":42,\"end\":57,\"answer\":\"RequestDelegate\"},{\"start\":149,\"end\":160,\"answer\":\"InvokeAsync\"},{\"start\":161,\"end\":172,\"answer\":\"HttpContext\"},{\"start\":238,\"end\":243,\"answer\":\"_next\"}]",
                Language = "csharp",
                Difficulty = "advanced",
                Category = "中介軟體",
                Hint = "自訂中介軟體需要 RequestDelegate、InvokeAsync 方法和 HttpContext 參數"
            },
            new CodePuzzle
            {
                Title = "模式比對與記錄型別",
                FullCode = "record Product(string Name, decimal Price);\n\nstring GetCategory(Product p) => p.Price switch\n{\n    < 100 => \"便宜\",\n    >= 100 and < 500 => \"中等\",\n    >= 500 => \"昂貴\",\n    _ => \"未知\"\n};",
                BlankPositionsJson = "[{\"start\":0,\"end\":6,\"answer\":\"record\"},{\"start\":92,\"end\":98,\"answer\":\"switch\"},{\"start\":138,\"end\":141,\"answer\":\"and\"},{\"start\":168,\"end\":169,\"answer\":\"_\"}]",
                Language = "csharp",
                Difficulty = "advanced",
                Category = "模式比對",
                Hint = "record 定義不可變記錄型別，switch 表達式搭配模式比對，_ 為預設情況"
            },
        };
    }
}
