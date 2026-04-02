using Microsoft.AspNetCore.Mvc;

namespace DotNetLearning.Controllers;

public class MonopolyController : Controller
{
    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult GetBoard()
    {
        var board = GetGameBoard();
        return Json(board);
    }

    private List<BoardSquare> GetGameBoard()
    {
        return new List<BoardSquare>
        {
            new() { Id=0, Type="start", Title="\U0001f3c1 \u8d77\u9ede", Description="\u64f2\u9ab0\u5b50\u958b\u59cb\u5192\u96aa\uff01" },
            new() { Id=1, Type="question", Title="\U0001f537 C# \u57fa\u790e", Question="Console.WriteLine() \u7684\u7528\u9014\u662f\u4ec0\u9ebc\uff1f", Options=new[]{ "\u5728\u87a2\u5e55\u4e0a\u5370\u51fa\u6587\u5b57", "\u5ba3\u544a\u8b8a\u6578", "\u5efa\u7acb\u8ff4\u5708", "\u532f\u5165\u5957\u4ef6" }, Answer=0, Coins=10 },
            new() { Id=2, Type="question", Title="\U0001f537 \u8b8a\u6578", Question="\u4ee5\u4e0b\u54ea\u500b\u662f\u6b63\u78ba\u7684 C# \u6574\u6578\u5ba3\u544a\uff1f", Options=new[]{ "int x = 5;", "x = int 5;", "var: x = 5;", "integer x = 5;" }, Answer=0, Coins=10 },
            new() { Id=3, Type="bonus", Title="\U0001f4b0 \u91d1\u5e63\u5bf6\u7bb1", Description="\u606d\u559c\uff01\u7372\u5f97 20 \u91d1\u5e63\uff01", Coins=20 },
            new() { Id=4, Type="question", Title="\U0001f537 \u5b57\u4e32", Question="C# \u5b57\u4e32\u63d2\u503c\u7684\u6b63\u78ba\u8a9e\u6cd5\u662f\uff1f", Options=new[]{ "$\"Hello {name}\"", "\"Hello\" + name", "f\"Hello {name}\"", "\"Hello #{name}\"" }, Answer=0, Coins=15 },
            new() { Id=5, Type="question", Title="\U0001f7e1 JS \u57fa\u790e", Question="JavaScript \u4e2d === \u548c == \u7684\u5dee\u5225\u662f\uff1f", Options=new[]{ "=== \u6bd4\u8f03\u503c\u548c\u578b\u5225\uff0c== \u53ea\u6bd4\u8f03\u503c", "\u6c92\u6709\u5dee\u5225", "=== \u662f\u8ce6\u503c\uff0c== \u662f\u6bd4\u8f03", "=== \u53ea\u80fd\u6bd4\u8f03\u6578\u5b57" }, Answer=0, Coins=15 },
            new() { Id=6, Type="trap", Title="\U0001f480 \u9677\u9631\uff01", Description="\u8e29\u5230\u5730\u96f7\uff01\u6263 1 \u689d\u547d", HpChange=-1 },
            new() { Id=7, Type="question", Title="\U0001f537 if/else", Question="if (x > 10) \u4e2d\u7684 > \u662f\u4ec0\u9ebc\u904b\u7b97\u5b50\uff1f", Options=new[]{ "\u6bd4\u8f03\u904b\u7b97\u5b50", "\u6307\u6d3e\u904b\u7b97\u5b50", "\u908f\u8f2f\u904b\u7b97\u5b50", "\u4f4d\u5143\u904b\u7b97\u5b50" }, Answer=0, Coins=10 },
            new() { Id=8, Type="question", Title="\U0001f7e1 HTML", Question="HTML \u4e2d\u54ea\u500b\u6a19\u7c64\u7528\u4f86\u5efa\u7acb\u8d85\u9023\u7d50\uff1f", Options=new[]{ "<a>", "<link>", "<href>", "<url>" }, Answer=0, Coins=10 },
            new() { Id=9, Type="heal", Title="\u2764\ufe0f \u56de\u8840\u7ad9", Description="\u6062\u5fa9 1 \u689d\u547d\uff01", HpChange=1 },
            new() { Id=10, Type="question", Title="\U0001f537 \u8ff4\u5708", Question="for (int i = 0; i < 5; i++) \u6703\u57f7\u884c\u5e7e\u6b21\uff1f", Options=new[]{ "5 \u6b21", "4 \u6b21", "6 \u6b21", "\u7121\u9650\u6b21" }, Answer=0, Coins=15 },
            new() { Id=11, Type="question", Title="\U0001f7e1 CSS", Question="CSS \u4e2d display: flex \u7684\u7528\u9014\u662f\uff1f", Options=new[]{ "\u5f48\u6027\u4f48\u5c40\u5bb9\u5668", "\u96b1\u85cf\u5143\u7d20", "\u56fa\u5b9a\u5b9a\u4f4d", "\u6539\u8b8a\u5b57\u578b" }, Answer=0, Coins=15 },
            new() { Id=12, Type="skip", Title="\u23e9 \u50b3\u9001\u9580", Description="\u5411\u524d\u8df3 2 \u683c\uff01", SkipAmount=2 },
            new() { Id=13, Type="question", Title="\U0001f537 \u9663\u5217", Question="C# \u9663\u5217\u7684\u7d22\u5f15\u5f9e\u5e7e\u958b\u59cb\uff1f", Options=new[]{ "0", "1", "-1", "\u4efb\u610f" }, Answer=0, Coins=10 },
            new() { Id=14, Type="question", Title="\U0001f7e2 SQL", Question="SQL \u4e2d\u7528\u54ea\u500b\u95dc\u9375\u5b57\u7be9\u9078\u8cc7\u6599\uff1f", Options=new[]{ "WHERE", "FILTER", "SELECT", "FIND" }, Answer=0, Coins=15 },
            new() { Id=15, Type="bonus", Title="\U0001f48e \u5927\u734e\uff01", Description="\u7372\u5f97 30 \u91d1\u5e63\uff01", Coins=30 },
            new() { Id=16, Type="question", Title="\U0001f537 OOP", Question="class \u548c object \u7684\u95dc\u4fc2\u662f\uff1f", Options=new[]{ "class \u662f\u85cd\u5716\uff0cobject \u662f\u5be6\u4f8b", "\u5b8c\u5168\u76f8\u540c", "class \u662f\u8b8a\u6578\uff0cobject \u662f\u65b9\u6cd5", "\u6c92\u6709\u95dc\u4fc2" }, Answer=0, Coins=20 },
            new() { Id=17, Type="question", Title="\U0001f7e1 JS DOM", Question="document.getElementById() \u56de\u50b3\u4ec0\u9ebc\uff1f", Options=new[]{ "\u4e00\u500b DOM \u5143\u7d20", "\u4e00\u500b\u5b57\u4e32", "\u4e00\u500b\u6578\u5b57", "\u4e00\u500b\u9663\u5217" }, Answer=0, Coins=15 },
            new() { Id=18, Type="trap", Title="\U0001f573\ufe0f \u6389\u5751\uff01", Description="\u9000\u56de 2 \u683c\uff01", SkipAmount=-2 },
            new() { Id=19, Type="question", Title="\U0001f537 LINQ", Question="numbers.Where(n => n > 5) \u7684\u4f5c\u7528\u662f\uff1f", Options=new[]{ "\u7be9\u9078\u5927\u65bc 5 \u7684\u5143\u7d20", "\u6392\u5e8f", "\u8a08\u7b97\u7e3d\u548c", "\u53d6\u7b2c\u4e00\u500b" }, Answer=0, Coins=20 },
            new() { Id=20, Type="question", Title="\U0001f7e2 SQL JOIN", Question="INNER JOIN \u56de\u50b3\u4ec0\u9ebc\uff1f", Options=new[]{ "\u5169\u5f35\u8868\u90fd\u6709\u5339\u914d\u7684\u8cc7\u6599", "\u5de6\u8868\u6240\u6709\u8cc7\u6599", "\u53f3\u8868\u6240\u6709\u8cc7\u6599", "\u6240\u6709\u8cc7\u6599" }, Answer=0, Coins=20 },
            new() { Id=21, Type="heal", Title="\U0001f34e \u88dc\u8840\u860b\u679c", Description="\u6062\u5fa9 1 \u689d\u547d\uff01", HpChange=1 },
            new() { Id=22, Type="question", Title="\U0001f537 async", Question="await \u95dc\u9375\u5b57\u7684\u7528\u9014\u662f\uff1f", Options=new[]{ "\u7b49\u5f85\u975e\u540c\u6b65\u64cd\u4f5c\u5b8c\u6210", "\u5efa\u7acb\u65b0\u57f7\u884c\u7dd2", "\u505c\u6b62\u7a0b\u5f0f", "\u5ba3\u544a\u8b8a\u6578" }, Answer=0, Coins=25 },
            new() { Id=23, Type="question", Title="\U0001f7e1 React", Question="React \u7528\u4ec0\u9ebc\u7ba1\u7406\u5143\u4ef6\u72c0\u614b\uff1f", Options=new[]{ "useState", "setState", "this.state", "createState" }, Answer=0, Coins=20 },
            new() { Id=24, Type="finish", Title="\U0001f3c6 \u7d42\u9ede", Description="\u606d\u559c\u901a\u95dc\uff01" },
        };
    }
}

public class BoardSquare
{
    public int Id { get; set; }
    public string Type { get; set; } = "";
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string? Question { get; set; }
    public string[]? Options { get; set; }
    public int Answer { get; set; }
    public int Coins { get; set; }
    public int HpChange { get; set; }
    public int SkipAmount { get; set; }
}
