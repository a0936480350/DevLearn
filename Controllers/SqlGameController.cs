using Microsoft.AspNetCore.Mvc;

namespace DotNetLearning.Controllers;

public class SqlGameController : Controller
{
    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult GetChallenges()
    {
        return Json(GetAllChallenges());
    }

    private List<SqlChallenge> GetAllChallenges()
    {
        return new List<SqlChallenge>
        {
            // Level 1: Basic SELECT
            new() { Id=1, Level=1, Title="查詢所有員工", Description="查出 employees 表中所有員工的資料",
                TableName="employees",
                Columns=new[]{"id","name","age","department","salary"},
                Data=new List<object[]>{
                    new object[]{1,"Alice",28,"Engineering",75000},
                    new object[]{2,"Bob",35,"Marketing",65000},
                    new object[]{3,"Charlie",42,"Engineering",90000},
                    new object[]{4,"Diana",31,"HR",60000},
                    new object[]{5,"Eve",26,"Engineering",72000},
                    new object[]{6,"Frank",38,"Marketing",68000},
                    new object[]{7,"Grace",29,"HR",62000},
                    new object[]{8,"Hank",45,"Engineering",95000},
                },
                ExpectedQuery="SELECT * FROM employees",
                ExpectedColumns=new[]{"id","name","age","department","salary"},
                ExpectedRows=8,
                Hint="用 SELECT * FROM 表名 查詢所有欄位"
            },
            // Level 1: WHERE
            new() { Id=2, Level=1, Title="篩選工程部門", Description="查出 Engineering 部門的所有員工",
                TableName="employees",
                Columns=new[]{"id","name","age","department","salary"},
                Data=new List<object[]>{
                    new object[]{1,"Alice",28,"Engineering",75000},
                    new object[]{2,"Bob",35,"Marketing",65000},
                    new object[]{3,"Charlie",42,"Engineering",90000},
                    new object[]{4,"Diana",31,"HR",60000},
                    new object[]{5,"Eve",26,"Engineering",72000},
                    new object[]{6,"Frank",38,"Marketing",68000},
                    new object[]{7,"Grace",29,"HR",62000},
                    new object[]{8,"Hank",45,"Engineering",95000},
                },
                ExpectedQuery="SELECT * FROM employees WHERE department = 'Engineering'",
                ExpectedColumns=new[]{"id","name","age","department","salary"},
                ExpectedRows=4,
                Hint="用 WHERE department = 'Engineering' 篩選"
            },
            // Level 1: SELECT specific columns
            new() { Id=3, Level=1, Title="只看名字和薪水", Description="只查出員工的 name 和 salary 欄位",
                TableName="employees",
                Columns=new[]{"id","name","age","department","salary"},
                Data=new List<object[]>{
                    new object[]{1,"Alice",28,"Engineering",75000},
                    new object[]{2,"Bob",35,"Marketing",65000},
                    new object[]{3,"Charlie",42,"Engineering",90000},
                    new object[]{4,"Diana",31,"HR",60000},
                    new object[]{5,"Eve",26,"Engineering",72000},
                    new object[]{6,"Frank",38,"Marketing",68000},
                    new object[]{7,"Grace",29,"HR",62000},
                    new object[]{8,"Hank",45,"Engineering",95000},
                },
                ExpectedQuery="SELECT name, salary FROM employees",
                ExpectedColumns=new[]{"name","salary"},
                ExpectedRows=8,
                Hint="在 SELECT 後面列出你要的欄位名稱"
            },
            // Level 2: WHERE with comparison
            new() { Id=4, Level=2, Title="高薪員工", Description="查出薪水大於 70000 的員工",
                TableName="employees",
                Columns=new[]{"id","name","age","department","salary"},
                Data=new List<object[]>{
                    new object[]{1,"Alice",28,"Engineering",75000},
                    new object[]{2,"Bob",35,"Marketing",65000},
                    new object[]{3,"Charlie",42,"Engineering",90000},
                    new object[]{4,"Diana",31,"HR",60000},
                    new object[]{5,"Eve",26,"Engineering",72000},
                    new object[]{6,"Frank",38,"Marketing",68000},
                    new object[]{7,"Grace",29,"HR",62000},
                    new object[]{8,"Hank",45,"Engineering",95000},
                },
                ExpectedQuery="SELECT * FROM employees WHERE salary > 70000",
                ExpectedColumns=new[]{"id","name","age","department","salary"},
                ExpectedRows=4,
                Hint="用 WHERE salary > 70000 篩選"
            },
            // Level 2: ORDER BY
            new() { Id=5, Level=2, Title="按薪水排序", Description="查出所有員工，按 salary 從高到低排序",
                TableName="employees",
                Columns=new[]{"id","name","age","department","salary"},
                Data=new List<object[]>{
                    new object[]{1,"Alice",28,"Engineering",75000},
                    new object[]{2,"Bob",35,"Marketing",65000},
                    new object[]{3,"Charlie",42,"Engineering",90000},
                    new object[]{4,"Diana",31,"HR",60000},
                    new object[]{5,"Eve",26,"Engineering",72000},
                    new object[]{6,"Frank",38,"Marketing",68000},
                    new object[]{7,"Grace",29,"HR",62000},
                    new object[]{8,"Hank",45,"Engineering",95000},
                },
                ExpectedQuery="SELECT * FROM employees ORDER BY salary DESC",
                ExpectedColumns=new[]{"id","name","age","department","salary"},
                ExpectedRows=8,
                Hint="用 ORDER BY salary DESC 降序排列"
            },
            // Level 2: COUNT
            new() { Id=6, Level=2, Title="統計人數", Description="計算每個部門有多少人",
                TableName="employees",
                Columns=new[]{"id","name","age","department","salary"},
                Data=new List<object[]>{
                    new object[]{1,"Alice",28,"Engineering",75000},
                    new object[]{2,"Bob",35,"Marketing",65000},
                    new object[]{3,"Charlie",42,"Engineering",90000},
                    new object[]{4,"Diana",31,"HR",60000},
                    new object[]{5,"Eve",26,"Engineering",72000},
                    new object[]{6,"Frank",38,"Marketing",68000},
                    new object[]{7,"Grace",29,"HR",62000},
                    new object[]{8,"Hank",45,"Engineering",95000},
                },
                ExpectedQuery="SELECT department, COUNT(*) FROM employees GROUP BY department",
                ExpectedColumns=new[]{"department","count"},
                ExpectedRows=3,
                Hint="用 GROUP BY department 搭配 COUNT(*)"
            },
            // Level 3: JOIN (two tables)
            new() { Id=7, Level=3, Title="訂單與客戶", Description="查出每筆訂單對應的客戶名稱（JOIN orders 和 customers 表）",
                TableName="orders",
                Columns=new[]{"order_id","customer_id","product","amount"},
                Data=new List<object[]>{
                    new object[]{101,1,"Laptop",1200},
                    new object[]{102,2,"Phone",800},
                    new object[]{103,1,"Tablet",500},
                    new object[]{104,3,"Monitor",350},
                    new object[]{105,2,"Keyboard",80},
                },
                ExtraTableName="customers",
                ExtraColumns=new[]{"id","name","city"},
                ExtraData=new List<object[]>{
                    new object[]{1,"小明","台北"},
                    new object[]{2,"小華","台中"},
                    new object[]{3,"小美","高雄"},
                },
                ExpectedQuery="SELECT orders.order_id, customers.name, orders.product, orders.amount FROM orders JOIN customers ON orders.customer_id = customers.id",
                ExpectedColumns=new[]{"order_id","name","product","amount"},
                ExpectedRows=5,
                Hint="用 JOIN ... ON orders.customer_id = customers.id 連接兩張表"
            },
            // Level 3: Subquery
            new() { Id=8, Level=3, Title="高於平均薪水", Description="找出薪水高於平均薪水的員工",
                TableName="employees",
                Columns=new[]{"id","name","age","department","salary"},
                Data=new List<object[]>{
                    new object[]{1,"Alice",28,"Engineering",75000},
                    new object[]{2,"Bob",35,"Marketing",65000},
                    new object[]{3,"Charlie",42,"Engineering",90000},
                    new object[]{4,"Diana",31,"HR",60000},
                    new object[]{5,"Eve",26,"Engineering",72000},
                    new object[]{6,"Frank",38,"Marketing",68000},
                    new object[]{7,"Grace",29,"HR",62000},
                    new object[]{8,"Hank",45,"Engineering",95000},
                },
                ExpectedQuery="SELECT * FROM employees WHERE salary > (SELECT AVG(salary) FROM employees)",
                ExpectedColumns=new[]{"id","name","age","department","salary"},
                ExpectedRows=4,
                Hint="用子查詢 (SELECT AVG(salary) FROM employees) 計算平均值"
            },
        };
    }
}

public class SqlChallenge
{
    public int Id { get; set; }
    public int Level { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string TableName { get; set; } = "";
    public string[] Columns { get; set; } = Array.Empty<string>();
    public List<object[]> Data { get; set; } = new();
    public string? ExtraTableName { get; set; }
    public string[]? ExtraColumns { get; set; }
    public List<object[]>? ExtraData { get; set; }
    public string ExpectedQuery { get; set; } = "";
    public string[] ExpectedColumns { get; set; } = Array.Empty<string>();
    public int ExpectedRows { get; set; }
    public string Hint { get; set; } = "";
}
