using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Database
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Chapter 22: SQL 進階 ─────────────────────────────────
        new() { Id=22, Category="database", Order=3, Level="intermediate", Icon="📊", Title="SQL 進階：JOIN、子查詢、CTE", Slug="sql-advanced", IsPublished=true, Content=@"
# SQL 進階：JOIN、子查詢、CTE

學會了基本 CRUD 之後，我們要學習如何**跨表查詢**，這才是 SQL 真正強大的地方。

> 想像你有一本「學生名冊」和一本「成績單」，JOIN 就是把兩本冊子攤開來，用學號把資料對在一起看。

---

## JOIN 的種類

### INNER JOIN — 交集

只回傳**兩邊都有對應資料**的列。

```sql
-- 查詢有選課的學生及其課程名稱
SELECT s.Name, c.CourseName, e.Score  -- 選取學生姓名、課程名稱、分數
FROM Students s                        -- 從學生表開始
INNER JOIN Enrollments e               -- 內連接選課表
    ON s.Id = e.StudentId              -- 用學生 ID 對應
INNER JOIN Courses c                   -- 再連接課程表
    ON e.CourseId = c.Id;              -- 用課程 ID 對應
```

### LEFT JOIN — 左邊全部保留

```sql
-- 查詢所有學生（包含沒選課的）
SELECT s.Name, c.CourseName            -- 選取姓名和課程
FROM Students s                        -- 左表：學生（全部保留）
LEFT JOIN Enrollments e                -- 左連接選課表
    ON s.Id = e.StudentId              -- 用學生 ID 對應
LEFT JOIN Courses c                    -- 再左連接課程表
    ON e.CourseId = c.Id;              -- 用課程 ID 對應
-- 沒選課的學生，CourseName 會顯示 NULL
```

### RIGHT JOIN — 右邊全部保留

```sql
-- 查詢所有課程（包含沒人選的）
SELECT s.Name, c.CourseName            -- 選取姓名和課程
FROM Students s                        -- 左表：學生
RIGHT JOIN Enrollments e               -- 右連接選課表
    ON s.Id = e.StudentId              -- 用學生 ID 對應
RIGHT JOIN Courses c                   -- 再右連接課程表
    ON e.CourseId = c.Id;              -- 用課程 ID 對應
-- 沒人選的課程，Name 會顯示 NULL
```

### FULL OUTER JOIN — 兩邊全部保留

```sql
-- 查詢所有學生和所有課程的配對
SELECT s.Name, c.CourseName            -- 選取姓名和課程
FROM Students s                        -- 左表：學生
FULL OUTER JOIN Enrollments e          -- 完全外連接選課表
    ON s.Id = e.StudentId              -- 用學生 ID 對應
FULL OUTER JOIN Courses c              -- 再完全外連接課程表
    ON e.CourseId = c.Id;              -- 用課程 ID 對應
-- 兩邊沒對應到的都會保留，缺少的部分顯示 NULL
```

---

## 子查詢（Subquery）

子查詢就是**查詢裡面再包一個查詢**，像是俄羅斯娃娃一樣。

```sql
-- 找出分數高於平均的學生
SELECT Name, Score                     -- 選取姓名和分數
FROM Students                          -- 從學生表查詢
WHERE Score > (                        -- 分數大於...
    SELECT AVG(Score) FROM Students    -- ...所有學生的平均分數
);
```

### 相關子查詢（Correlated Subquery）

每一列都會執行一次子查詢，效能較差但功能強大。

```sql
-- 找出每門課的最高分學生
SELECT s.Name, e.CourseId, e.Score     -- 選取姓名、課程 ID、分數
FROM Students s                        -- 從學生表
JOIN Enrollments e                     -- 連接選課表
    ON s.Id = e.StudentId              -- 用學生 ID 對應
WHERE e.Score = (                      -- 分數等於...
    SELECT MAX(e2.Score)               -- ...該課程的最高分
    FROM Enrollments e2                -- 從選課表
    WHERE e2.CourseId = e.CourseId     -- 條件：同一門課（這裡參考了外層查詢）
);
```

---

## CTE（Common Table Expression）

CTE 用 `WITH ... AS` 語法，把複雜查詢拆成**有名字的暫時結果集**，讓 SQL 更好讀。

```sql
-- 用 CTE 計算每個學生的平均分數，再篩選及格的
WITH StudentAvg AS (                   -- 定義一個叫 StudentAvg 的暫時結果集
    SELECT                             -- 查詢內容
        StudentId,                     -- 學生 ID
        AVG(Score) AS AvgScore         -- 計算平均分數
    FROM Enrollments                   -- 從選課表
    GROUP BY StudentId                 -- 依學生分組
)
SELECT s.Name, sa.AvgScore             -- 從 CTE 和學生表選取資料
FROM StudentAvg sa                     -- 使用剛才定義的 CTE
JOIN Students s                        -- 連接學生表
    ON sa.StudentId = s.Id             -- 用學生 ID 對應
WHERE sa.AvgScore >= 60                -- 只要平均及格的
ORDER BY sa.AvgScore DESC;             -- 依平均分數降序排列
```

---

## Window Functions（窗口函數）

窗口函數可以在**不改變原始列數**的情況下，計算排名、累計等聚合值。

```sql
-- 在每門課中計算學生排名
SELECT
    s.Name,                            -- 學生姓名
    c.CourseName,                      -- 課程名稱
    e.Score,                           -- 分數
    ROW_NUMBER() OVER (                -- 依每門課的分數排序，給予流水編號
        PARTITION BY e.CourseId        -- 依課程分組（每門課獨立排名）
        ORDER BY e.Score DESC          -- 依分數降序
    ) AS RowNum,
    RANK() OVER (                      -- RANK 遇到同分會跳號
        PARTITION BY e.CourseId        -- 同樣依課程分組
        ORDER BY e.Score DESC          -- 依分數降序
    ) AS ScoreRank,
    DENSE_RANK() OVER (                -- DENSE_RANK 遇到同分不跳號
        PARTITION BY e.CourseId        -- 依課程分組
        ORDER BY e.Score DESC          -- 依分數降序
    ) AS DenseRank
FROM Enrollments e                     -- 從選課表
JOIN Students s ON s.Id = e.StudentId  -- 連接學生表
JOIN Courses c ON c.Id = e.CourseId;   -- 連接課程表
```

> **ROW_NUMBER vs RANK vs DENSE_RANK**：假設分數是 100, 95, 95, 90
> - ROW_NUMBER: 1, 2, 3, 4（不管同分，流水號）
> - RANK: 1, 2, 2, 4（同分同名次，跳號）
> - DENSE_RANK: 1, 2, 2, 3（同分同名次，不跳號）

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：忘記 ON 條件，產生 Cartesian Join

```sql
-- ❌ 忘記 ON 條件 → 兩張表的每一列都會配對！
SELECT s.Name, c.CourseName            -- 選取姓名和課程
FROM Students s, Courses c;            -- 這會產生 M × N 列的結果！
-- 如果學生有 100 人，課程有 50 門 → 回傳 5000 列 😱
```

```sql
-- ✅ 正確寫法：用 JOIN + ON 指定關聯條件
SELECT s.Name, c.CourseName            -- 選取姓名和課程
FROM Students s                        -- 從學生表
JOIN Enrollments e ON s.Id = e.StudentId -- 透過選課表連接
JOIN Courses c ON e.CourseId = c.Id;     -- 再連接課程表
```

### ❌ 錯誤 2：NULL 在 JOIN 中的陷阱

```sql
-- ❌ NULL 不等於任何值（包括另一個 NULL）
-- 如果 DepartmentId 為 NULL 的學生不會出現在 INNER JOIN 結果中
SELECT s.Name, d.DeptName              -- 選取姓名和部門名稱
FROM Students s                        -- 從學生表
INNER JOIN Departments d               -- 內連接部門表
    ON s.DepartmentId = d.Id;          -- NULL != 任何值 → 這些列被排除
```

```sql
-- ✅ 如果要保留沒有部門的學生，用 LEFT JOIN
SELECT s.Name, ISNULL(d.DeptName, N'未分配') AS DeptName -- 用 ISNULL 處理 NULL
FROM Students s                        -- 從學生表
LEFT JOIN Departments d                -- 左連接保留所有學生
    ON s.DepartmentId = d.Id;          -- NULL 的學生也會保留
```

### ❌ 錯誤 3：在 WHERE 子句中對 LEFT JOIN 的右表篩選

```sql
-- ❌ 這等於把 LEFT JOIN 變成 INNER JOIN！
SELECT s.Name, c.CourseName            -- 選取姓名和課程
FROM Students s                        -- 從學生表
LEFT JOIN Courses c                    -- 左連接課程表
    ON s.CourseId = c.Id               -- 連接條件
WHERE c.IsActive = 1;                  -- 這會過濾掉 NULL（等於 INNER JOIN）
```

```sql
-- ✅ 條件放在 ON 裡面，才能保留左表所有列
SELECT s.Name, c.CourseName            -- 選取姓名和課程
FROM Students s                        -- 從學生表
LEFT JOIN Courses c                    -- 左連接課程表
    ON s.CourseId = c.Id               -- 連接條件
    AND c.IsActive = 1;               -- 篩選條件放在 ON 裡
```

---

## 💡 重點整理

| JOIN 類型 | 說明 |
|-----------|------|
| INNER JOIN | 只回傳兩邊都有的（交集）|
| LEFT JOIN | 左邊全部保留，右邊沒有的填 NULL |
| RIGHT JOIN | 右邊全部保留，左邊沒有的填 NULL |
| FULL OUTER JOIN | 兩邊全部保留 |
| CTE | 把子查詢取名字，提高可讀性 |
| Window Function | 不減少列數的聚合計算 |
" },

        // ── Chapter 23: EF Core 進階操作 ─────────────────────────────
        new() { Id=23, Category="database", Order=4, Level="intermediate", Icon="⚙️", Title="EF Core 進階操作", Slug="ef-core-advanced", IsPublished=true, Content=@"
# EF Core 進階操作

上一章學了 EF Core 基礎，現在要深入了解**Fluent API、載入策略、進階設定**。

---

## Fluent API vs Data Annotations

兩種設定 Entity 的方式，Fluent API 更強大、更靈活。

```csharp
// === Data Annotations 方式（用 Attribute）===
public class Student                       // 學生 Entity
{
    public int Id { get; set; }            // 主鍵（慣例自動識別）

    [Required]                             // 必填
    [MaxLength(100)]                       // 最大長度 100
    public string Name { get; set; } = """"; // 學生姓名

    [EmailAddress]                         // Email 格式驗證
    public string Email { get; set; } = """"; // 學生信箱
}
```

```csharp
// === Fluent API 方式（在 DbContext 中設定）===
protected override void OnModelCreating(ModelBuilder modelBuilder) // 覆寫模型建構方法
{
    modelBuilder.Entity<Student>(entity =>  // 設定 Student Entity
    {
        entity.HasKey(s => s.Id);           // 設定主鍵
        entity.Property(s => s.Name)        // 設定 Name 屬性
            .IsRequired()                   // 必填
            .HasMaxLength(100);             // 最大長度 100
        entity.HasIndex(s => s.Email)       // 在 Email 上建立索引
            .IsUnique();                    // 唯一索引
        entity.Property(s => s.Email)       // 設定 Email 屬性
            .HasMaxLength(200);             // 最大長度 200
    });
}
```

> **選哪個？** 簡單驗證用 Data Annotations，複雜關聯設定用 Fluent API。團隊中建議統一風格。

---

## Navigation Properties（導航屬性）

導航屬性讓你用 C# 物件的方式存取**關聯資料**。

```csharp
// 一對多關係：一個學生有多筆選課記錄
public class Student                       // 學生 Entity
{
    public int Id { get; set; }            // 主鍵
    public string Name { get; set; } = """"; // 姓名

    // 導航屬性：一個學生 → 多筆選課
    public ICollection<Enrollment> Enrollments { get; set; } // 選課集合
        = new List<Enrollment>();           // 初始化空集合
}

public class Enrollment                    // 選課 Entity
{
    public int Id { get; set; }            // 主鍵
    public int StudentId { get; set; }     // 外鍵：學生 ID
    public int CourseId { get; set; }      // 外鍵：課程 ID
    public int Score { get; set; }         // 分數

    // 導航屬性：多對一
    public Student Student { get; set; } = null!; // 所屬學生
    public Course Course { get; set; } = null!;   // 所屬課程
}
```

---

## 載入策略：Eager / Lazy / Explicit

> 想像你去圖書館借書：
> - **Eager Loading**：一次把主書和所有參考書都搬回來
> - **Lazy Loading**：先拿主書，需要參考書時再跑一趟圖書館
> - **Explicit Loading**：先拿主書，你明確說要參考書時才去拿

### Eager Loading（預先載入）— 推薦 ✅

```csharp
// 一次查詢就把關聯資料載入
var students = await db.Students           // 查詢學生
    .Include(s => s.Enrollments)           // 同時載入選課記錄
        .ThenInclude(e => e.Course)        // 再載入每筆選課的課程資料
    .Where(s => s.Score > 80)              // 篩選分數大於 80
    .ToListAsync();                        // 執行查詢並轉為 List
// 只產生 1 個 SQL 查詢（含 JOIN）
```

### Lazy Loading（延遲載入）— 小心使用 ⚠️

```csharp
// 需要安裝套件並設定
// dotnet add package Microsoft.EntityFrameworkCore.Proxies

// 在 DbContext 中啟用
optionsBuilder.UseLazyLoadingProxies();    // 啟用延遲載入代理

// 導航屬性必須加 virtual
public class Student                       // 學生 Entity
{
    public int Id { get; set; }            // 主鍵
    public virtual ICollection<Enrollment> Enrollments { get; set; } // 加 virtual！
        = new List<Enrollment>();           // 初始化
}

// 存取時自動查詢（但可能產生 N+1 問題！）
foreach (var s in students)                // 迴圈每個學生
{
    Console.WriteLine(s.Enrollments.Count); // 每次存取都會發一個 SQL 查詢！
}
```

### Explicit Loading（明確載入）

```csharp
var student = await db.Students            // 先查詢學生
    .FirstAsync(s => s.Id == 1);           // 取得 ID=1 的學生

await db.Entry(student)                    // 取得該 Entity 的追蹤資訊
    .Collection(s => s.Enrollments)        // 指定要載入的集合
    .LoadAsync();                          // 明確載入選課記錄
// 分兩次查詢，但你可以控制何時載入
```

---

## Shadow Properties（影子屬性）

影子屬性是**不出現在 Entity 類別中**，但存在於資料庫的欄位。

```csharp
// 在 Fluent API 中定義影子屬性
modelBuilder.Entity<Student>()             // 設定 Student Entity
    .Property<DateTime>(""CreatedAt"")     // 定義影子屬性 CreatedAt
    .HasDefaultValueSql(""GETDATE()"");    // 預設值為目前時間

modelBuilder.Entity<Student>()             // 設定 Student Entity
    .Property<DateTime>(""UpdatedAt"");    // 定義影子屬性 UpdatedAt

// 在 SaveChanges 中自動更新
public override int SaveChanges()          // 覆寫 SaveChanges
{
    foreach (var entry in ChangeTracker.Entries()) // 遍歷所有追蹤的 Entity
    {
        if (entry.State == EntityState.Modified)   // 如果是修改狀態
        {
            entry.Property(""UpdatedAt"")          // 設定 UpdatedAt 影子屬性
                .CurrentValue = DateTime.Now;      // 更新為目前時間
        }
    }
    return base.SaveChanges();             // 呼叫原始的 SaveChanges
}
```

---

## Value Conversions（值轉換）

把 C# 型別自動轉換成資料庫型別。

```csharp
// 把 Enum 存成字串
public enum StudentStatus                  // 學生狀態列舉
{
    Active,                                // 在學
    Graduated,                             // 畢業
    Suspended                              // 休學
}

modelBuilder.Entity<Student>()             // 設定 Student Entity
    .Property(s => s.Status)               // Status 屬性
    .HasConversion<string>();              // 存到資料庫時轉成字串
// 資料庫存 ""Active""，C# 讀取時自動轉回 Enum
```

---

## Global Query Filters（全域查詢篩選）

自動在每次查詢時加上篩選條件，適合做**軟刪除**。

```csharp
// 在 OnModelCreating 中設定
modelBuilder.Entity<Student>()             // 設定 Student Entity
    .HasQueryFilter(s => !s.IsDeleted);    // 自動過濾已刪除的資料

// 之後所有查詢都會自動加上 WHERE IsDeleted = 0
var students = await db.Students.ToListAsync(); // 只會回傳未刪除的學生

// 需要查看已刪除資料時，用 IgnoreQueryFilters
var allStudents = await db.Students        // 查詢學生
    .IgnoreQueryFilters()                  // 忽略全域篩選
    .ToListAsync();                        // 包含已刪除的學生
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：N+1 問題

```csharp
// ❌ 沒有用 Include，每次存取導航屬性都會查一次資料庫
var students = await db.Students.ToListAsync(); // 查詢 1：取得所有學生
foreach (var s in students)                     // 迴圈 N 個學生
{
    var count = s.Enrollments.Count;            // 查詢 2~N+1：每個學生各查一次！
}
// 如果有 100 個學生 → 101 次 SQL 查詢 😱
```

```csharp
// ✅ 用 Include 一次載入
var students = await db.Students               // 查詢學生
    .Include(s => s.Enrollments)               // 同時載入選課記錄
    .ToListAsync();                            // 只有 1 次 SQL 查詢 ✅
foreach (var s in students)                    // 迴圈 N 個學生
{
    var count = s.Enrollments.Count;           // 已經載入，不需要額外查詢
}
```

### ❌ 錯誤 2：Lazy Loading 效能陷阱

```csharp
// ❌ 開啟 Lazy Loading 後，在迴圈中存取導航屬性
var students = await db.Students.ToListAsync();   // 取得所有學生
var result = students.Select(s => new            // 投影
{
    s.Name,                                      // 學生姓名
    CourseCount = s.Enrollments.Count             // 這裡觸發 Lazy Loading！
}).ToList();                                     // N+1 問題再次出現
```

```csharp
// ✅ 用投影（Select）在資料庫端完成計算
var result = await db.Students                   // 查詢學生
    .Select(s => new                             // 在資料庫端投影
    {
        s.Name,                                  // 學生姓名
        CourseCount = s.Enrollments.Count         // 在 SQL 中計算 COUNT
    })
    .ToListAsync();                              // 一次查詢完成 ✅
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Fluent API | 在 DbContext 中用程式碼設定模型 |
| Eager Loading | 用 Include 一次載入關聯資料 |
| Lazy Loading | 存取時自動載入（有 N+1 風險）|
| Shadow Property | 不在 Entity 中但存在於 DB 的屬性 |
| Global Query Filter | 每次查詢自動套用的篩選條件 |
" },

        // ── Chapter 24: Dapper 微型 ORM ─────────────────────────────
        new() { Id=24, Category="database", Order=5, Level="intermediate", Icon="🏎️", Title="Dapper 微型 ORM", Slug="dapper-micro-orm", IsPublished=true, Content=@"
# Dapper 微型 ORM

## 什麼是 Dapper？

> 如果 EF Core 是**自排車**（自動幫你處理 SQL、追蹤變更、管理關聯），那 Dapper 就是**手排車**（你自己寫 SQL，它只幫你把結果對應到 C# 物件）。

Dapper 是由 Stack Overflow 團隊開發的**微型 ORM**，特點是：
- ✅ **極快**：效能接近原生 ADO.NET
- ✅ **輕量**：只有一個 NuGet 套件
- ✅ **簡單**：就是 IDbConnection 的擴充方法
- ⚠️ 需要自己寫 SQL

```bash
# 安裝 Dapper
dotnet add package Dapper                  # 安裝 Dapper 套件
dotnet add package Microsoft.Data.SqlClient # 安裝 SQL Server 連線套件
```

---

## 基本操作

### Query<T> — 查詢多筆

```csharp
using Dapper;                              // 引用 Dapper
using Microsoft.Data.SqlClient;            // 引用 SQL Server 連線

// 建立資料庫連線
using var conn = new SqlConnection(connectionString); // 建立連線物件

// 查詢所有學生
var students = await conn.QueryAsync<Student>( // 查詢並對應到 Student 類別
    ""SELECT Id, Name, Email, Score FROM Students"" // SQL 查詢語句
);

foreach (var s in students)                // 遍歷結果
{
    Console.WriteLine($""{s.Name}: {s.Score}""); // 輸出姓名和分數
}
```

### QueryFirstOrDefault — 查詢單筆

```csharp
// 查詢單一學生
var student = await conn.QueryFirstOrDefaultAsync<Student>( // 查詢第一筆或 null
    ""SELECT * FROM Students WHERE Id = @Id"", // 使用參數化查詢
    new { Id = 1 }                         // 傳入參數（匿名物件）
);

if (student is null)                       // 檢查是否找到
{
    Console.WriteLine(""找不到學生"");       // 沒找到的處理
}
```

### Execute — 新增 / 修改 / 刪除

```csharp
// 新增學生
var rowsAffected = await conn.ExecuteAsync( // 執行 SQL 並回傳影響列數
    @""INSERT INTO Students (Name, Email, Score) -- 新增語句
       VALUES (@Name, @Email, @Score)"",        // 使用參數
    new { Name = ""小賢"", Email = ""xian@test.com"", Score = 95 } // 參數值
);
Console.WriteLine($""新增了 {rowsAffected} 筆""); // 輸出影響列數

// 批次新增（傳入 List）
var newStudents = new List<Student>        // 建立多筆學生資料
{
    new() { Name = ""小明"", Email = ""ming@test.com"", Score = 88 }, // 學生 1
    new() { Name = ""小華"", Email = ""hua@test.com"", Score = 92 },  // 學生 2
};

var count = await conn.ExecuteAsync(       // 批次執行
    @""INSERT INTO Students (Name, Email, Score) -- 新增語句
       VALUES (@Name, @Email, @Score)"",        // 使用參數
    newStudents                            // 傳入整個 List，Dapper 會逐筆執行
);
```

---

## 參數化查詢（防 SQL Injection）

這是使用 Dapper（或任何 ORM）最重要的安全觀念。

```csharp
// ❌ 絕對不要這樣做！字串串接 → SQL Injection 風險
var name = ""'; DROP TABLE Students; --"";  // 惡意輸入
var sql = $""SELECT * FROM Students WHERE Name = '{name}'""; // 直接串接 😱
// 最終 SQL: SELECT * FROM Students WHERE Name = ''; DROP TABLE Students; --'

// ✅ 使用參數化查詢
var student = await conn.QueryFirstOrDefaultAsync<Student>( // 安全的查詢
    ""SELECT * FROM Students WHERE Name = @Name"", // 用 @Name 參數佔位
    new { Name = name }                    // Dapper 會安全地處理參數值
);
// Dapper 會自動將參數值進行轉義，防止 SQL Injection
```

---

## Multi-Mapping（多表對應）

當 JOIN 查詢回傳多張表的資料時，Dapper 可以自動對應到不同的 C# 物件。

```csharp
// JOIN 查詢：學生 + 選課 + 課程
var sql = @""
    SELECT s.Id, s.Name, s.Email,          -- 學生欄位
           e.Id, e.Score,                  -- 選課欄位
           c.Id, c.CourseName              -- 課程欄位
    FROM Students s
    INNER JOIN Enrollments e ON s.Id = e.StudentId  -- 連接選課表
    INNER JOIN Courses c ON e.CourseId = c.Id       -- 連接課程表
    WHERE s.Id = @StudentId"";              -- 篩選條件

var enrollments = await conn.QueryAsync<Student, Enrollment, Course, Enrollment>(
    sql,                                   // SQL 查詢
    (student, enrollment, course) =>       // 對應函式（三個物件合併）
    {
        enrollment.Student = student;      // 設定選課的學生
        enrollment.Course = course;        // 設定選課的課程
        return enrollment;                 // 回傳選課記錄
    },
    new { StudentId = 1 },                 // 參數
    splitOn: ""Id,Id""                     // 告訴 Dapper 在哪裡切分欄位
);
```

---

## Dapper vs EF Core：什麼時候用哪個？

| 場景 | 推薦 | 原因 |
|------|------|------|
| 快速開發 CRUD | EF Core | 自動產生 SQL，開發速度快 |
| 複雜報表查詢 | Dapper | 手寫 SQL 更靈活 |
| 效能敏感的 API | Dapper | 效能接近原生 ADO.NET |
| 需要 Change Tracking | EF Core | 自動追蹤變更 |
| 呼叫 Stored Procedure | Dapper | 語法更簡單 |
| 新手學習 | EF Core | 不需要會 SQL 也能開始 |

> 💡 **實務建議**：很多團隊會**兩個一起用**！EF Core 做一般 CRUD，Dapper 做複雜查詢和報表。

```csharp
// 在同一個專案中混用 EF Core 和 Dapper
public class StudentService                // 學生服務
{
    private readonly AppDbContext _db;      // EF Core 的 DbContext
    private readonly IDbConnection _conn;  // Dapper 用的連線

    // 簡單 CRUD 用 EF Core
    public async Task<Student?> GetById(int id) // 用 EF Core 查詢單筆
    {
        return await _db.Students          // 使用 DbContext
            .Include(s => s.Enrollments)   // 載入關聯資料
            .FirstOrDefaultAsync(s => s.Id == id); // 依 ID 查詢
    }

    // 複雜報表用 Dapper
    public async Task<IEnumerable<StudentReport>> GetReport() // 用 Dapper 查報表
    {
        return await _conn.QueryAsync<StudentReport>( // 使用 Dapper
            @""SELECT s.Name, COUNT(e.Id) AS CourseCount, -- 手寫複雜 SQL
                      AVG(e.Score) AS AvgScore
               FROM Students s
               LEFT JOIN Enrollments e ON s.Id = e.StudentId
               GROUP BY s.Name
               HAVING AVG(e.Score) > 60""  // 只要平均及格的
        );
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：SQL Injection — 字串串接

```csharp
// ❌ 絕對不要用字串串接組 SQL！
public async Task<Student?> Search(string keyword) // 搜尋學生
{
    var sql = ""SELECT * FROM Students WHERE Name LIKE '%"" + keyword + ""%'""; // 危險！
    return await conn.QueryFirstOrDefaultAsync<Student>(sql); // SQL Injection 風險
}
```

```csharp
// ✅ 用參數化查詢
public async Task<Student?> Search(string keyword) // 搜尋學生
{
    var sql = ""SELECT * FROM Students WHERE Name LIKE @Keyword""; // 參數化
    return await conn.QueryFirstOrDefaultAsync<Student>( // 安全查詢
        sql,
        new { Keyword = $""%{keyword}%"" } // Dapper 安全處理參數
    );
}
```

### ❌ 錯誤 2：忘記 Dispose 連線

```csharp
// ❌ 沒有 using，連線不會被釋放
var conn = new SqlConnection(connectionString); // 建立連線
var data = await conn.QueryAsync<Student>(sql);  // 查詢
// conn 永遠不會被關閉！連線池耗盡 → 系統當掉
```

```csharp
// ✅ 用 using 確保連線被釋放
using var conn = new SqlConnection(connectionString); // using 確保自動釋放
var data = await conn.QueryAsync<Student>(sql);       // 查詢完成後自動關閉連線
```

### ❌ 錯誤 3：Multi-Mapping 忘記設定 splitOn

```csharp
// ❌ 沒設定 splitOn，Dapper 不知道哪些欄位屬於哪個物件
var result = await conn.QueryAsync<Student, Course, Student>( // 多表對應
    ""SELECT s.*, c.* FROM Students s JOIN Courses c ON ..."", // 查詢
    (s, c) => { s.Course = c; return s; }  // 對應函式
    // 忘記 splitOn 參數 → 預設只用 ""Id"" 切分，可能對應錯誤
);
```

```csharp
// ✅ 明確指定 splitOn
var result = await conn.QueryAsync<Student, Course, Student>( // 多表對應
    ""SELECT s.Id, s.Name, c.Id, c.CourseName FROM Students s JOIN Courses c ON ..."",
    (s, c) => { s.Course = c; return s; }, // 對應函式
    splitOn: ""Id""                        // 告訴 Dapper 在第二個 Id 欄位切分
);
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Dapper | 微型 ORM，效能極佳 |
| Query<T> | 查詢多筆並對應到類別 |
| Execute | 執行新增/修改/刪除 |
| 參數化查詢 | 防 SQL Injection 的關鍵 |
| Multi-Mapping | JOIN 結果對應到多個物件 |
| splitOn | 告訴 Dapper 在哪裡切分欄位 |
" },

        // ── Chapter 25: 資料庫遷移 Migration ─────────────────────────
        new() { Id=25, Category="database", Order=6, Level="intermediate", Icon="📦", Title="資料庫遷移 Migration", Slug="database-migration", IsPublished=true, Content=@"
# 資料庫遷移 Migration

## 為什麼需要 Migration？

> 想像你在蓋房子，每次要改格局（加房間、拆牆）都需要一份**施工記錄**。Migration 就是資料庫的施工記錄——記錄每一次結構變更，讓你可以追蹤、重現、甚至回退。

---

## Code First vs Database First

| 方式 | 說明 | 適用場景 |
|------|------|---------|
| Code First | 先寫 C# Entity → 自動產生資料庫 | 新專案、敏捷開發 |
| Database First | 先有資料庫 → 用工具產生 C# Entity | 既有資料庫、DBA 設計 |

```csharp
// Code First：先定義 Entity
public class Student                       // 定義學生 Entity
{
    public int Id { get; set; }            // 主鍵
    public string Name { get; set; } = """"; // 姓名
    public string Email { get; set; } = """"; // 信箱
    public int Score { get; set; }         // 分數
}

// EF Core 會根據這個類別自動建立資料表
```

---

## Migration 基本指令

### 建立 Migration

```bash
# 建立新的 Migration（PMC，Package Manager Console）
Add-Migration InitialCreate                # 建立名為 InitialCreate 的遷移

# 或用 .NET CLI
dotnet ef migrations add InitialCreate     # 同上，使用命令列工具
```

### 套用 Migration

```bash
# 更新資料庫（套用所有未套用的 Migration）
Update-Database                            # PMC 指令

# 或用 .NET CLI
dotnet ef database update                  # 同上，使用命令列工具
```

### 查看 Migration 狀態

```bash
# 列出所有 Migration 及其套用狀態
dotnet ef migrations list                  # 顯示所有遷移及是否已套用
```

---

## Migration 檔案結構

每次 `Add-Migration` 會產生**三個檔案**：

```
Migrations/
├── 20240101120000_InitialCreate.cs         # 遷移內容（Up/Down 方法）
├── 20240101120000_InitialCreate.Designer.cs # 快照設計器（自動產生）
└── AppDbContextModelSnapshot.cs            # 模型快照（目前資料庫狀態）
```

```csharp
// 20240101120000_InitialCreate.cs — 遷移檔案範例
public partial class InitialCreate : Migration // 繼承 Migration 類別
{
    protected override void Up(MigrationBuilder migrationBuilder) // 升級方法
    {
        migrationBuilder.CreateTable(      // 建立資料表
            name: ""Students"",            // 表名
            columns: table => new          // 定義欄位
            {
                Id = table.Column<int>(nullable: false) // 主鍵欄位
                    .Annotation(""SqlServer:Identity"", ""1, 1""), // 自動遞增
                Name = table.Column<string>(maxLength: 100), // 姓名欄位
                Email = table.Column<string>(maxLength: 200), // 信箱欄位
                Score = table.Column<int>(nullable: false, defaultValue: 0) // 分數欄位
            },
            constraints: table =>          // 設定約束
            {
                table.PrimaryKey(""PK_Students"", x => x.Id); // 主鍵約束
            });

        migrationBuilder.CreateIndex(      // 建立索引
            name: ""IX_Students_Email"",   // 索引名稱
            table: ""Students"",           // 資料表
            column: ""Email"",             // 欄位
            unique: true);                 // 唯一索引
    }

    protected override void Down(MigrationBuilder migrationBuilder) // 降級方法（回退用）
    {
        migrationBuilder.DropTable(""Students""); // 刪除資料表
    }
}
```

---

## 在 Migration 中植入種子資料

```csharp
// 方法一：在 OnModelCreating 中用 HasData
protected override void OnModelCreating(ModelBuilder modelBuilder) // 模型建構
{
    modelBuilder.Entity<Student>().HasData( // 植入種子資料
        new Student { Id = 1, Name = ""小賢"", Email = ""xian@test.com"", Score = 95 }, // 學生 1
        new Student { Id = 2, Name = ""小明"", Email = ""ming@test.com"", Score = 88 }  // 學生 2
    );
}

// 方法二：在 Migration 的 Up 方法中直接寫 SQL
protected override void Up(MigrationBuilder migrationBuilder) // 升級方法
{
    migrationBuilder.Sql(                  // 執行原始 SQL
        @""INSERT INTO Students (Name, Email, Score) -- 插入種子資料
           VALUES (N'小賢', 'xian@test.com', 95),   -- 學生 1
                  (N'小明', 'ming@test.com', 88)""   -- 學生 2
    );
}
```

---

## 回退 Migration

```bash
# 回退到指定的 Migration
dotnet ef database update InitialCreate    # 回退到 InitialCreate（會執行 Down 方法）

# 回退到最初狀態（移除所有 Migration）
dotnet ef database update 0               # 回退到零（執行所有 Down 方法）

# 移除最後一個未套用的 Migration 檔案
dotnet ef migrations remove               # 刪除最後一個 Migration 檔案
```

---

## 實用技巧

### 產生 SQL 腳本（不直接執行）

```bash
# 產生 SQL 腳本，用於正式環境部署
dotnet ef migrations script               # 產生所有 Migration 的 SQL

# 指定範圍
dotnet ef migrations script FromMigration ToMigration # 產生指定範圍的 SQL

# 產生冪等腳本（可重複執行）
dotnet ef migrations script --idempotent   # 加上 IF NOT EXISTS 檢查
```

### 手動修改 Migration

```csharp
// 有時候自動產生的 Migration 不夠用，可以手動修改
protected override void Up(MigrationBuilder migrationBuilder) // 升級方法
{
    // 重新命名欄位（EF Core 預設會刪除再建立，資料會遺失！）
    migrationBuilder.RenameColumn(         // 手動改成重新命名
        name: ""StudentName"",             // 原始欄位名
        table: ""Students"",              // 資料表
        newName: ""Name"");               // 新欄位名
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：修改已發佈的 Migration

```bash
# ❌ 已經 Update-Database 之後，又修改 Migration 檔案的內容
# 這會導致 Migration 的雜湊值不一致，EF Core 會報錯
# ""The migration '20240101_Init' has already been applied to the database""
```

```bash
# ✅ 正確做法：建立一個新的 Migration 來修改
dotnet ef migrations add FixStudentTable   # 建立新的修正遷移
dotnet ef database update                  # 套用新的遷移
```

### ❌ 錯誤 2：沒有檢查自動產生的 SQL

```csharp
// ❌ 直接 Update-Database，沒有看過產生的 SQL
// 可能會不小心刪除欄位（EF Core 會把重新命名誤判為刪除+新增）
```

```bash
# ✅ 先產生 SQL 腳本檢查
dotnet ef migrations script --idempotent   # 先看 SQL 再決定是否執行
# 確認 SQL 沒問題後再 Update-Database
```

### ❌ 錯誤 3：在正式環境直接用 Update-Database

```bash
# ❌ 在正式環境直接執行 Update-Database
# 如果 Migration 有問題，可能會導致資料遺失！
```

```bash
# ✅ 正式環境應該用 SQL 腳本部署
dotnet ef migrations script --idempotent -o migrate.sql # 產生腳本
# 由 DBA 審核後，在正式環境執行 SQL 腳本
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Code First | 先寫 C# Entity，再產生資料庫 |
| Add-Migration | 建立新的遷移檔案 |
| Update-Database | 套用遷移到資料庫 |
| Up / Down | 升級和回退的方法 |
| HasData | 在模型中定義種子資料 |
| --idempotent | 產生可重複執行的 SQL 腳本 |
" },

        // ── Chapter 26: 資料庫效能優化 ────────────────────────────────
        new() { Id=26, Category="database", Order=7, Level="advanced", Icon="⚡", Title="資料庫效能優化", Slug="database-performance", IsPublished=true, Content=@"
# 資料庫效能優化

## 為什麼效能很重要？

> 想像你在一個巨大的圖書館找一本書：
> - **沒有索引**：從第一本書開始，一本一本翻，直到找到為止（全表掃描）
> - **有索引**：查目錄，直接走到正確的書架（索引查詢）
>
> 當資料量從 100 筆變成 100 萬筆，差別就是**一秒和一小時**的區別。

---

## 索引（Index）

### 叢集索引（Clustered Index）

每張表只能有**一個**叢集索引，決定資料的**實際儲存順序**。

```sql
-- 主鍵預設就是叢集索引
CREATE TABLE Students (
    Id INT PRIMARY KEY CLUSTERED,          -- 主鍵 = 叢集索引，資料按 Id 排序儲存
    Name NVARCHAR(100),                    -- 姓名
    Email NVARCHAR(200)                    -- 信箱
);
```

### 非叢集索引（Non-Clustered Index）

可以有**多個**，像書的目錄一樣指向實際資料。

```sql
-- 在 Email 欄位建立非叢集索引
CREATE NONCLUSTERED INDEX IX_Students_Email -- 建立非叢集索引
ON Students(Email);                        -- 在 Email 欄位上

-- 複合索引（多欄位）
CREATE INDEX IX_Students_Name_Score        -- 建立複合索引
ON Students(Name ASC, Score DESC);         -- 先按姓名升序，再按分數降序
-- 注意：複合索引的欄位順序很重要！
-- WHERE Name = 'xxx' → ✅ 會用到索引
-- WHERE Score > 90   → ❌ 不會用到索引（必須從最左邊的欄位開始匹配）
```

### 在 EF Core 中建立索引

```csharp
// Fluent API 建立索引
modelBuilder.Entity<Student>(entity =>     // 設定 Student Entity
{
    entity.HasIndex(s => s.Email)           // 在 Email 上建索引
        .IsUnique();                       // 唯一索引

    entity.HasIndex(s => new { s.Name, s.Score }) // 複合索引
        .HasDatabaseName(""IX_Students_Name_Score""); // 指定索引名稱
});
```

---

## 查詢執行計畫（Execution Plan）

```sql
-- 查看查詢的執行計畫
SET STATISTICS IO ON;                      -- 開啟 IO 統計
SET STATISTICS TIME ON;                    -- 開啟時間統計

-- 使用 EXPLAIN 查看執行計畫（MSSQL 用法）
SET SHOWPLAN_TEXT ON;                      -- 開啟文字格式的執行計畫
GO
SELECT * FROM Students WHERE Email = 'test@test.com'; -- 要分析的查詢
GO
SET SHOWPLAN_TEXT OFF;                     -- 關閉執行計畫顯示
```

```
-- 執行計畫結果範例
-- Index Seek（好！用到索引）→ 直接跳到對的位置
-- Table Scan（不好！全表掃描）→ 逐列搜尋，很慢
-- Index Scan（普通）→ 掃描整個索引，比 Table Scan 快一點
```

---

## N+1 問題與解決方案

```csharp
// ❌ N+1 問題：查 1 次取得清單 + N 次取得關聯資料
var students = await db.Students.ToListAsync(); // 查詢 1：取得所有學生
foreach (var student in students)               // 迴圈 N 次
{
    // 每次迴圈都產生一個 SQL 查詢
    var enrollments = await db.Enrollments      // 查詢 2~N+1
        .Where(e => e.StudentId == student.Id)  // 篩選該學生的選課
        .ToListAsync();                         // 執行查詢
}
// 100 個學生 = 101 次查詢 😱

// ✅ 解法 1：Eager Loading
var students = await db.Students               // 查詢學生
    .Include(s => s.Enrollments)               // 一次載入所有選課記錄
    .ToListAsync();                            // 只有 1 次查詢 ✅

// ✅ 解法 2：投影（最佳效能）
var result = await db.Students                 // 查詢學生
    .Select(s => new                           // 投影成需要的形狀
    {
        s.Name,                                // 只選需要的欄位
        EnrollmentCount = s.Enrollments.Count  // 在 SQL 端計算
    })
    .ToListAsync();                            // 一次查詢搞定 ✅

// ✅ 解法 3：分批載入
var studentIds = students.Select(s => s.Id).ToList(); // 先取得所有學生 ID
var allEnrollments = await db.Enrollments              // 一次查詢所有相關選課
    .Where(e => studentIds.Contains(e.StudentId))      // 用 IN 查詢
    .ToListAsync();                                    // 只有 2 次查詢 ✅
```

---

## 批次操作

```csharp
// ❌ 逐筆更新（慢）
foreach (var student in students)              // 遍歷每個學生
{
    student.Score += 5;                        // 加分
    await db.SaveChangesAsync();               // 每次都送一個 UPDATE 語句
}

// ✅ 批次更新（EF Core 7+ 支援 ExecuteUpdate）
await db.Students                              // 查詢學生
    .Where(s => s.Score < 60)                  // 篩選不及格的
    .ExecuteUpdateAsync(s =>                   // 批次更新
        s.SetProperty(x => x.Score, x => x.Score + 5) // 加 5 分
    );                                         // 只產生一個 UPDATE 語句 ✅

// ✅ 批次刪除（EF Core 7+）
await db.Students                              // 查詢學生
    .Where(s => s.IsDeleted)                   // 篩選已刪除的
    .ExecuteDeleteAsync();                     // 一個 DELETE 語句搞定 ✅
```

---

## Compiled Queries（編譯查詢）

```csharp
// 預先編譯常用查詢，避免每次都重新產生 SQL
private static readonly Func<AppDbContext, int, Task<Student?>> // 宣告編譯查詢
    GetStudentById = EF.CompileAsyncQuery(     // 編譯查詢
        (AppDbContext db, int id) =>           // 參數：DbContext 和 ID
            db.Students.FirstOrDefault(s => s.Id == id) // 查詢邏輯
    );

// 使用編譯查詢
var student = await GetStudentById(db, 1);     // 直接呼叫，不需要重新產生 SQL
```

---

## 快取策略

```csharp
// MemoryCache — 記憶體快取（單機）
using Microsoft.Extensions.Caching.Memory;     // 引用快取命名空間

public class StudentService                    // 學生服務
{
    private readonly IMemoryCache _cache;       // 注入快取服務
    private readonly AppDbContext _db;          // 注入 DbContext

    public async Task<Student?> GetById(int id) // 查詢學生（帶快取）
    {
        var cacheKey = $""student_{id}"";       // 快取鍵
        if (_cache.TryGetValue(cacheKey, out Student? student)) // 嘗試從快取取得
        {
            return student;                    // 快取命中，直接回傳
        }

        student = await _db.Students           // 快取未命中，查詢資料庫
            .FirstOrDefaultAsync(s => s.Id == id); // 依 ID 查詢

        if (student is not null)               // 如果找到學生
        {
            _cache.Set(cacheKey, student,       // 存入快取
                TimeSpan.FromMinutes(5));       // 5 分鐘後過期
        }
        return student;                        // 回傳結果
    }
}
```

> 💡 **Redis** 是分散式快取方案，適合多台伺服器共用快取的情境。概念類似 MemoryCache，但資料存在獨立的 Redis 伺服器上。

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：SELECT *（選取所有欄位）

```csharp
// ❌ 只需要姓名，卻把所有欄位都選出來
var students = await db.Students.ToListAsync(); // SELECT * FROM Students
var names = students.Select(s => s.Name);       // 在 C# 端才篩選欄位
// 如果 Student 有大型的 ProfileImage 欄位 → 浪費大量頻寬和記憶體
```

```csharp
// ✅ 只選取需要的欄位
var names = await db.Students                  // 查詢學生
    .Select(s => s.Name)                       // 只選 Name 欄位
    .ToListAsync();                            // SQL: SELECT Name FROM Students ✅
```

### ❌ 錯誤 2：缺少索引

```sql
-- ❌ 每次查詢都要全表掃描
SELECT * FROM Orders                           -- 查詢訂單
WHERE CustomerId = 12345                       -- 按客戶 ID 篩選
AND Status = 'Pending';                        -- 按狀態篩選
-- 如果沒有索引，100 萬筆資料每次都要掃描全部 😱
```

```sql
-- ✅ 建立適當的索引
CREATE INDEX IX_Orders_CustomerId_Status       -- 建立複合索引
ON Orders(CustomerId, Status);                 -- 按查詢條件建索引
-- 現在查詢只需要毫秒等級 ✅
```

### ❌ 錯誤 3：過度索引

```sql
-- ❌ 在每個欄位上都建索引
CREATE INDEX IX_1 ON Students(Name);           -- 索引 1
CREATE INDEX IX_2 ON Students(Email);          -- 索引 2
CREATE INDEX IX_3 ON Students(Score);          -- 索引 3
CREATE INDEX IX_4 ON Students(Name, Email);    -- 索引 4
CREATE INDEX IX_5 ON Students(Name, Score);    -- 索引 5
CREATE INDEX IX_6 ON Students(Email, Score);   -- 索引 6
-- 每次 INSERT/UPDATE/DELETE 都要更新所有索引 → 寫入效能暴降！
```

```sql
-- ✅ 只在常用查詢條件上建索引
-- 分析查詢模式後，只建必要的索引
CREATE INDEX IX_Students_Email ON Students(Email); -- 經常用 Email 查詢
CREATE INDEX IX_Students_Name_Score                -- 經常用姓名+分數排序
ON Students(Name, Score DESC);
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Clustered Index | 決定資料實際儲存順序，每表只有一個 |
| Non-Clustered Index | 像書的目錄，可以有多個 |
| N+1 問題 | 1 次查主表 + N 次查關聯表 |
| ExecuteUpdate | EF Core 7+ 批次更新 |
| Compiled Query | 預先編譯查詢，提高效能 |
| MemoryCache | 記憶體快取，減少資料庫查詢 |
" },

        // ── Chapter 27: 交易與並行控制 ────────────────────────────────
        new() { Id=27, Category="database", Order=8, Level="advanced", Icon="🔄", Title="交易與並行控制", Slug="transaction-concurrency", IsPublished=true, Content=@"
# 交易與並行控制

## 什麼是交易（Transaction）？

> 想像你在銀行轉帳：
> 1. 從 A 帳戶扣 1000 元
> 2. 在 B 帳戶加 1000 元
>
> 這兩步**必須同時成功或同時失敗**。如果扣了 A 的錢但 B 沒加到，錢就憑空消失了！
> 這就是交易（Transaction）要解決的問題。

---

## ACID 特性

| 特性 | 英文 | 說明 | 銀行比喻 |
|------|------|------|---------|
| 原子性 | Atomicity | 全部成功或全部失敗 | 轉帳要嘛成功，要嘛完全沒動 |
| 一致性 | Consistency | 交易前後資料都合法 | 總金額不會改變 |
| 隔離性 | Isolation | 交易之間互不干擾 | 你轉帳時別人看不到中間狀態 |
| 持久性 | Durability | 完成後永久保存 | 轉完帳重開機錢不會消失 |

---

## EF Core 中的交易

### 隱式交易（預設）

```csharp
// SaveChanges 自動包在交易裡
var student = new Student { Name = ""小賢"" }; // 建立學生
db.Students.Add(student);                      // 加入追蹤

var course = new Course { CourseName = ""C#"" }; // 建立課程
db.Courses.Add(course);                          // 加入追蹤

await db.SaveChangesAsync();                     // 一次送出 → 自動包在交易裡
// 如果任何一個 INSERT 失敗，全部都會回退（Rollback）
```

### 明確交易

```csharp
// 需要跨多次 SaveChanges 的交易
using var transaction = await db.Database    // 開始一個交易
    .BeginTransactionAsync();                // 取得交易物件

try
{
    // 步驟 1：從 A 帳戶扣款
    var accountA = await db.Accounts         // 查詢 A 帳戶
        .FirstAsync(a => a.Id == 1);         // 取得帳戶資料
    accountA.Balance -= 1000;                // 扣 1000 元
    await db.SaveChangesAsync();             // 儲存（但交易尚未提交）

    // 步驟 2：在 B 帳戶加款
    var accountB = await db.Accounts         // 查詢 B 帳戶
        .FirstAsync(a => a.Id == 2);         // 取得帳戶資料
    accountB.Balance += 1000;                // 加 1000 元
    await db.SaveChangesAsync();             // 儲存（但交易尚未提交）

    // 兩步都成功，提交交易
    await transaction.CommitAsync();         // 提交交易（真正寫入資料庫）
}
catch (Exception ex)                         // 任何步驟出錯
{
    await transaction.RollbackAsync();       // 回退所有變更
    Console.WriteLine($""交易失敗: {ex.Message}""); // 記錄錯誤
    throw;                                   // 重新拋出例外
}
```

---

## Dapper 中的交易

```csharp
using var conn = new SqlConnection(connectionString); // 建立連線
await conn.OpenAsync();                                // 開啟連線

using var transaction = conn.BeginTransaction();       // 開始交易

try
{
    // 步驟 1：扣款
    await conn.ExecuteAsync(                           // 執行 SQL
        ""UPDATE Accounts SET Balance = Balance - @Amount WHERE Id = @Id"", // 扣款語句
        new { Amount = 1000, Id = 1 },                 // 參數
        transaction                                    // 傳入交易物件
    );

    // 步驟 2：加款
    await conn.ExecuteAsync(                           // 執行 SQL
        ""UPDATE Accounts SET Balance = Balance + @Amount WHERE Id = @Id"", // 加款語句
        new { Amount = 1000, Id = 2 },                 // 參數
        transaction                                    // 傳入同一個交易物件
    );

    transaction.Commit();                              // 提交交易
}
catch
{
    transaction.Rollback();                            // 回退交易
    throw;                                             // 重新拋出
}
```

---

## 隔離等級（Isolation Level）

不同的隔離等級決定了交易之間的**可見性**。

| 隔離等級 | 髒讀 | 不可重複讀 | 幻讀 | 效能 |
|---------|------|-----------|------|------|
| Read Uncommitted | ✅ 可能 | ✅ 可能 | ✅ 可能 | 最快 |
| Read Committed（預設）| ❌ 防止 | ✅ 可能 | ✅ 可能 | 快 |
| Repeatable Read | ❌ 防止 | ❌ 防止 | ✅ 可能 | 中等 |
| Serializable | ❌ 防止 | ❌ 防止 | ❌ 防止 | 最慢 |

> **名詞解釋**：
> - **髒讀**：讀到別人還沒提交的資料（可能被回退）
> - **不可重複讀**：同一交易中，兩次讀取同一列得到不同結果
> - **幻讀**：同一交易中，兩次查詢得到不同數量的列

```csharp
// 在 EF Core 中設定隔離等級
using var transaction = await db.Database.BeginTransactionAsync(
    System.Data.IsolationLevel.Serializable // 最嚴格的隔離等級
);
// ⚠️ Serializable 最安全但最慢，只在真正需要時使用
```

```sql
-- 在 SQL 中設定隔離等級
SET TRANSACTION ISOLATION LEVEL READ COMMITTED; -- 設定隔離等級
BEGIN TRANSACTION;                               -- 開始交易
    SELECT * FROM Accounts WHERE Id = 1;         -- 查詢帳戶
    UPDATE Accounts SET Balance = Balance - 1000 WHERE Id = 1; -- 扣款
COMMIT TRANSACTION;                              -- 提交交易
```

---

## 樂觀並行控制 vs 悲觀並行控制

### 樂觀並行控制（Optimistic Concurrency）

> 假設衝突很少發生，先做再說，提交時再檢查是否有衝突。

```csharp
// 在 Entity 中加入 RowVersion（並行權杖）
public class Product                           // 產品 Entity
{
    public int Id { get; set; }                // 主鍵
    public string Name { get; set; } = """";   // 產品名稱
    public decimal Price { get; set; }         // 價格
    public int Stock { get; set; }             // 庫存

    [Timestamp]                                // 標記為並行權杖
    public byte[] RowVersion { get; set; } = null!; // 每次更新自動變更
}

// Fluent API 設定
modelBuilder.Entity<Product>()                 // 設定 Product Entity
    .Property(p => p.RowVersion)               // RowVersion 屬性
    .IsRowVersion();                           // 標記為列版本

// 使用時自動檢查並行衝突
try
{
    var product = await db.Products            // 查詢產品
        .FirstAsync(p => p.Id == 1);           // 取得 ID=1 的產品
    product.Stock -= 1;                        // 減少庫存
    await db.SaveChangesAsync();               // 儲存時會檢查 RowVersion
}
catch (DbUpdateConcurrencyException ex)        // 並行衝突例外
{
    // 有人在你之前修改了這筆資料！
    var entry = ex.Entries.Single();           // 取得衝突的 Entity
    var dbValues = await entry.GetDatabaseValuesAsync(); // 取得資料庫最新值
    // 決定要：1. 用資料庫的值 2. 用你的值 3. 合併
    entry.OriginalValues.SetValues(dbValues!); // 用資料庫最新值重試
    await db.SaveChangesAsync();               // 重新儲存
}
```

### 悲觀並行控制（Pessimistic Concurrency）

> 假設衝突經常發生，先鎖定資源再操作。

```sql
-- 在 SQL 中使用悲觀鎖定
BEGIN TRANSACTION;                             -- 開始交易

SELECT * FROM Products WITH (UPDLOCK, ROWLOCK) -- 鎖定這一列
WHERE Id = 1;                                  -- 其他交易無法修改這列

UPDATE Products SET Stock = Stock - 1          -- 更新庫存
WHERE Id = 1;                                  -- 更新指定產品

COMMIT TRANSACTION;                            -- 提交並釋放鎖定
```

```csharp
// 在 EF Core 中使用原始 SQL 實現悲觀鎖定
using var transaction = await db.Database      // 開始交易
    .BeginTransactionAsync();

var product = await db.Products                // 使用原始 SQL 查詢並鎖定
    .FromSqlRaw(""SELECT * FROM Products WITH (UPDLOCK) WHERE Id = {0}"", 1) // 加鎖
    .FirstAsync();                             // 取得產品

product.Stock -= 1;                            // 減少庫存
await db.SaveChangesAsync();                   // 儲存變更
await transaction.CommitAsync();               // 提交交易並釋放鎖
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：長時間持有交易

```csharp
// ❌ 在交易中做耗時操作
using var transaction = await db.Database.BeginTransactionAsync(); // 開始交易
var data = await db.Products.ToListAsync();   // 查詢（鎖定資源）

await SendEmailAsync(data);                   // ❌ 發送 Email（可能要好幾秒！）
await CallExternalApiAsync();                 // ❌ 呼叫外部 API（更慢！）

await db.SaveChangesAsync();                  // 儲存
await transaction.CommitAsync();              // 提交
// 在整段期間，其他交易都被擋住了！可能導致 Timeout
```

```csharp
// ✅ 交易中只做資料庫操作，其他的放在交易外面
var data = await db.Products.ToListAsync();   // 先查詢資料（交易外）

using var transaction = await db.Database.BeginTransactionAsync(); // 開始交易
// 只做必要的資料庫操作
product.Stock -= 1;                           // 更新資料
await db.SaveChangesAsync();                  // 儲存
await transaction.CommitAsync();              // 提交（快速完成）

// 交易結束後再做其他操作
await SendEmailAsync(data);                   // 發送 Email（交易外）
await CallExternalApiAsync();                 // 呼叫外部 API（交易外）
```

### ❌ 錯誤 2：死鎖（Deadlock）

```csharp
// ❌ 兩個交易互相等待對方釋放鎖定
// 交易 A：先鎖 Products → 再鎖 Orders
// 交易 B：先鎖 Orders → 再鎖 Products
// 結果：A 等 B 釋放 Orders，B 等 A 釋放 Products → 死鎖！
```

```csharp
// ✅ 所有交易都按照相同的順序存取資源
// 交易 A 和 B 都先鎖 Orders → 再鎖 Products
// 這樣就不會產生循環等待
```

### ❌ 錯誤 3：忘記 Rollback

```csharp
// ❌ 沒有 try-catch，異常時交易不會回退
var transaction = await db.Database.BeginTransactionAsync(); // 開始交易
await db.SaveChangesAsync();                  // 如果這裡拋出例外...
await transaction.CommitAsync();              // 這行不會執行
// 交易會一直掛著，佔用資源，直到連線超時
```

```csharp
// ✅ 用 using + try-catch 確保交易正確處理
using var transaction = await db.Database.BeginTransactionAsync(); // using 確保釋放
try
{
    await db.SaveChangesAsync();              // 儲存變更
    await transaction.CommitAsync();          // 提交交易
}
catch
{
    await transaction.RollbackAsync();        // 明確回退
    throw;                                    // 重新拋出例外
}
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| ACID | 原子性、一致性、隔離性、持久性 |
| SaveChanges | EF Core 自動包在隱式交易中 |
| BeginTransaction | 明確開始一個交易 |
| Isolation Level | 控制交易之間的可見性 |
| 樂觀並行 | 用 RowVersion 檢查衝突 |
| 悲觀並行 | 用 UPDLOCK 鎖定資源 |
| 死鎖 | 兩個交易互相等待 → 統一存取順序避免 |
" },

        // ── Chapter 28: MSSQL 特有功能 ───────────────────────────────
        new() { Id=28, Category="database", Order=9, Level="intermediate", Icon="🗄️", Title="MSSQL 特有功能", Slug="mssql-features", IsPublished=true, Content=@"
# MSSQL 特有功能

## SQL Server Management Studio（SSMS）

SSMS 是管理 SQL Server 的**圖形化工具**，就像 Visual Studio 之於 C#，SSMS 之於 SQL Server。

### 常用功能

- **物件總管**：瀏覽資料庫、資料表、索引等
- **查詢編輯器**：撰寫和執行 SQL
- **執行計畫**：圖形化顯示查詢效能
- **活動監視器**：監控伺服器狀態

```sql
-- 在 SSMS 中常用的查詢
-- 查看所有資料庫
SELECT name FROM sys.databases;            -- 列出所有資料庫名稱

-- 查看某個資料庫的所有資料表
SELECT TABLE_NAME                          -- 選取資料表名稱
FROM INFORMATION_SCHEMA.TABLES             -- 從資訊架構表
WHERE TABLE_TYPE = 'BASE TABLE';           -- 只要基本資料表（排除 View）

-- 查看資料表的欄位資訊
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE -- 選取欄位名、資料型別、是否可為空
FROM INFORMATION_SCHEMA.COLUMNS            -- 從欄位資訊表
WHERE TABLE_NAME = 'Students';             -- 指定資料表名稱
```

---

## Stored Procedures（預存程序）

預存程序是**預先編譯並儲存在資料庫中的 SQL 程式**。

> 想像一道菜的食譜：你把做菜步驟寫好存起來（CREATE PROCEDURE），之後只要說一聲「做這道菜」（EXEC），就會自動按步驟執行。

```sql
-- 建立預存程序：查詢學生成績報表
CREATE PROCEDURE sp_GetStudentReport       -- 建立名為 sp_GetStudentReport 的預存程序
    @MinScore INT = 60,                    -- 參數：最低分數（預設 60）
    @CourseName NVARCHAR(100) = NULL       -- 參數：課程名稱（可選）
AS
BEGIN
    SET NOCOUNT ON;                        -- 不回傳「受影響的列數」訊息

    SELECT                                 -- 查詢學生成績報表
        s.Name AS StudentName,             -- 學生姓名
        c.CourseName,                      -- 課程名稱
        e.Score,                           -- 分數
        CASE                               -- 用 CASE 判斷等級
            WHEN e.Score >= 90 THEN N'優秀' -- 90 分以上
            WHEN e.Score >= 80 THEN N'良好' -- 80~89 分
            WHEN e.Score >= 60 THEN N'及格' -- 60~79 分
            ELSE N'不及格'                  -- 60 分以下
        END AS Grade                       -- 等級欄位
    FROM Students s                        -- 從學生表
    JOIN Enrollments e ON s.Id = e.StudentId -- 連接選課表
    JOIN Courses c ON e.CourseId = c.Id       -- 連接課程表
    WHERE e.Score >= @MinScore             -- 篩選分數
        AND (@CourseName IS NULL OR c.CourseName = @CourseName) -- 可選的課程篩選
    ORDER BY e.Score DESC;                 -- 依分數降序排列
END;
```

```sql
-- 執行預存程序
EXEC sp_GetStudentReport;                  -- 使用預設參數
EXEC sp_GetStudentReport @MinScore = 80;   -- 只看 80 分以上
EXEC sp_GetStudentReport @MinScore = 70, @CourseName = N'C# 程式設計'; -- 指定兩個參數
```

```csharp
// 在 Dapper 中呼叫預存程序
var results = await conn.QueryAsync<StudentReport>( // 用 Dapper 查詢
    ""sp_GetStudentReport"",               // 預存程序名稱
    new { MinScore = 80 },                 // 傳入參數
    commandType: CommandType.StoredProcedure // 指定為預存程序
);

// 在 EF Core 中呼叫預存程序
var results = await db.Students            // 使用 EF Core
    .FromSqlRaw(""EXEC sp_GetStudentReport @p0, @p1"", 80, ""C#"") // 呼叫預存程序
    .ToListAsync();                        // 取得結果
```

---

## Views（檢視）

View 是**預先定義好的查詢**，用起來像一張虛擬的資料表。

```sql
-- 建立 View：學生成績摘要
CREATE VIEW vw_StudentSummary AS           -- 建立名為 vw_StudentSummary 的 View
SELECT                                     -- 查詢內容
    s.Id AS StudentId,                     -- 學生 ID
    s.Name AS StudentName,                 -- 學生姓名
    COUNT(e.Id) AS CourseCount,            -- 選課數量
    AVG(e.Score) AS AvgScore,              -- 平均分數
    MAX(e.Score) AS HighestScore           -- 最高分數
FROM Students s                            -- 從學生表
LEFT JOIN Enrollments e                    -- 左連接選課表
    ON s.Id = e.StudentId                  -- 連接條件
GROUP BY s.Id, s.Name;                     -- 依學生分組
```

```sql
-- 使用 View（就像查資料表一樣）
SELECT * FROM vw_StudentSummary            -- 查詢 View
WHERE AvgScore >= 80                       -- 篩選平均 80 分以上
ORDER BY AvgScore DESC;                    -- 依平均分數排序
```

---

## Functions（使用者定義函數）

```sql
-- 純量函數（回傳單一值）
CREATE FUNCTION fn_GetLetterGrade          -- 建立計算等級的函數
(
    @Score INT                             -- 參數：分數
)
RETURNS NVARCHAR(10)                       -- 回傳型別：字串
AS
BEGIN
    RETURN CASE                            -- 判斷等級
        WHEN @Score >= 90 THEN N'A'        -- 90 分以上
        WHEN @Score >= 80 THEN N'B'        -- 80~89 分
        WHEN @Score >= 70 THEN N'C'        -- 70~79 分
        WHEN @Score >= 60 THEN N'D'        -- 60~69 分
        ELSE N'F'                          -- 60 分以下
    END;
END;
```

```sql
-- 使用函數
SELECT Name, Score,                        -- 選取姓名和分數
    dbo.fn_GetLetterGrade(Score) AS Grade   -- 呼叫函數計算等級
FROM Students;                             -- 從學生表查詢
```

---

## Triggers（觸發器）

> ⚠️ 觸發器很強大但也很危險——它們會在特定操作時**自動觸發**，如果不小心可能造成意想不到的副作用。

```sql
-- 建立觸發器：記錄學生資料的修改歷史
CREATE TRIGGER trg_StudentAudit            -- 建立觸發器
ON Students                                -- 綁定在 Students 表上
AFTER UPDATE                               -- 在 UPDATE 之後觸發
AS
BEGIN
    SET NOCOUNT ON;                        -- 不回傳影響列數

    INSERT INTO StudentAuditLog            -- 寫入審計日誌表
    (
        StudentId,                         -- 學生 ID
        OldName,                           -- 修改前的姓名
        NewName,                           -- 修改後的姓名
        ChangedAt                          -- 修改時間
    )
    SELECT                                 -- 從 inserted 和 deleted 虛擬表取值
        i.Id,                              -- 學生 ID
        d.Name,                            -- deleted 表 = 修改前的值
        i.Name,                            -- inserted 表 = 修改後的值
        GETDATE()                          -- 目前時間
    FROM inserted i                        -- inserted = 新值
    JOIN deleted d ON i.Id = d.Id;         -- deleted = 舊值
END;
```

> 💡 **為什麼要小心觸發器？**
> - 觸發器是「隱形」的——看程式碼看不到它會執行
> - 可能造成連鎖觸發（觸發器 A 觸發觸發器 B）
> - 偵錯困難，效能影響不容易發現
> - **建議**：優先使用應用程式邏輯或 EF Core 的 SaveChanges 事件

---

## SQL Server Profiler 基礎

SQL Server Profiler 用來**監控資料庫的即時活動**。

### 常見用途

- 找出慢查詢（Slow Query）
- 監控哪些 SQL 被執行
- 偵錯效能問題

```sql
-- 替代方案：用 DMV（Dynamic Management Views）查詢效能資訊
-- 查看目前正在執行的查詢
SELECT                                     -- 查詢執行中的請求
    r.session_id,                          -- 連線 ID
    r.status,                              -- 狀態
    r.command,                             -- 命令類型
    t.text AS QueryText,                   -- SQL 文字
    r.total_elapsed_time / 1000 AS ElapsedSec -- 已執行秒數
FROM sys.dm_exec_requests r                -- 從執行請求 DMV
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) t -- 取得 SQL 文字
WHERE r.session_id > 50;                   -- 排除系統連線
```

```sql
-- 查看最耗資源的查詢（Top 10）
SELECT TOP 10                              -- 取前 10 名
    qs.total_elapsed_time / qs.execution_count AS AvgTime, -- 平均執行時間
    qs.execution_count,                    -- 執行次數
    SUBSTRING(t.text, 1, 200) AS QueryText -- SQL 文字（前 200 字）
FROM sys.dm_exec_query_stats qs            -- 從查詢統計 DMV
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) t -- 取得 SQL 文字
ORDER BY AvgTime DESC;                     -- 依平均時間降序
```

---

## 系統資料庫

| 資料庫 | 用途 |
|--------|------|
| master | 系統級設定、登入帳號、所有資料庫的目錄 |
| tempdb | 暫存資料、臨時表、排序暫存（重啟後清空）|
| model | 新資料庫的範本（新建 DB 會複製 model 的設定）|
| msdb | SQL Agent 排程工作、備份歷史 |

```sql
-- 查看 tempdb 使用狀況
SELECT                                     -- 查詢 tempdb 空間使用
    SUM(unallocated_extent_page_count) * 8 / 1024 AS FreeSpaceMB,  -- 可用空間
    SUM(internal_object_reserved_page_count) * 8 / 1024 AS InternalMB, -- 內部物件
    SUM(user_object_reserved_page_count) * 8 / 1024 AS UserMB         -- 使用者物件
FROM sys.dm_db_file_space_usage;           -- 從空間使用 DMV 查詢
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：使用 Cursor（游標）代替集合操作

```sql
-- ❌ 用游標逐列處理（非常慢）
DECLARE @Id INT, @Score INT                -- 宣告變數
DECLARE student_cursor CURSOR FOR          -- 宣告游標
    SELECT Id, Score FROM Students         -- 游標查詢

OPEN student_cursor                        -- 開啟游標
FETCH NEXT FROM student_cursor INTO @Id, @Score -- 取得第一列

WHILE @@FETCH_STATUS = 0                   -- 迴圈處理每一列
BEGIN
    UPDATE Students SET Score = @Score + 5 -- 逐列更新
    WHERE Id = @Id                         -- 一次只更新一列
    FETCH NEXT FROM student_cursor INTO @Id, @Score -- 取下一列
END

CLOSE student_cursor                       -- 關閉游標
DEALLOCATE student_cursor                  -- 釋放游標
-- 逐列處理 → 超級慢！像是一個一個搬磚頭
```

```sql
-- ✅ 用集合操作一次搞定
UPDATE Students                            -- 一個 UPDATE 語句
SET Score = Score + 5;                     -- 一次更新所有列
-- 集合操作 → 超級快！像是開怪手一次搬一堆
```

### ❌ 錯誤 2：觸發器連鎖導致效能問題

```sql
-- ❌ 觸發器 A 修改表 B → 表 B 上的觸發器又修改表 C → ...
-- 造成連鎖反應，難以追蹤和偵錯
-- 而且可能導致無限迴圈！

-- ✅ 盡量用應用程式邏輯取代觸發器
-- 在 EF Core 的 SaveChanges 中處理審計邏輯
```

### ❌ 錯誤 3：直接修改系統資料庫

```sql
-- ❌ 絕對不要手動修改 master、tempdb 的資料
-- USE master
-- DELETE FROM sys.objects ... ← 不要這樣做！

-- ✅ 只使用官方提供的系統預存程序和 DDL 語句
-- 例如用 ALTER DATABASE 修改設定
-- 用 sp_helpdb 查看資料庫資訊
EXEC sp_helpdb;                            -- 查看所有資料庫的詳細資訊
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Stored Procedure | 預先編譯的 SQL 程式，可重複呼叫 |
| View | 虛擬資料表，簡化複雜查詢 |
| Function | 回傳值的 SQL 函數 |
| Trigger | 自動觸發的程式（小心使用）|
| DMV | 動態管理檢視，監控效能 |
| 系統資料庫 | master、tempdb、model、msdb |
" },
    };
}
