using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedBugChallenges
{
    public static List<BugChallenge> GetChallenges()
    {
        return new List<BugChallenge>
        {
            // ===== 初級 (beginner) =====

            // 1. 缺少分號
            new BugChallenge
            {
                Title = "消失的分號",
                Difficulty = "beginner",
                Category = "語法錯誤",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 計算兩個數字的總和
public class Calculator
{
    public int Add(int a, int b)
    {
        int result = a + b  // 這裡少了什麼？
        return result;
    }
}",
                FixedCode = @"// 計算兩個數字的總和
public class Calculator
{
    public int Add(int a, int b)
    {
        int result = a + b;  // 加上分號
        return result;
    }
}",
                Explanation = "C# 每一行陳述式結尾都需要加上分號 (;)。`int result = a + b` 缺少了結尾的分號，編譯器會報錯。"
            },

            // 2. 錯誤的變數型別
            new BugChallenge
            {
                Title = "型別不對勁",
                Difficulty = "beginner",
                Category = "型別錯誤",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 計算平均分數
public class ScoreCalculator
{
    public double GetAverage(int[] scores)
    {
        int total = 0;
        foreach (var s in scores)
        {
            total += s;
        }
        // 為什麼平均值總是整數？
        int average = total / scores.Length;
        return average;
    }
}",
                FixedCode = @"// 計算平均分數
public class ScoreCalculator
{
    public double GetAverage(int[] scores)
    {
        int total = 0;
        foreach (var s in scores)
        {
            total += s;
        }
        // 使用 double 進行除法運算
        double average = (double)total / scores.Length;
        return average;
    }
}",
                Explanation = "整數除以整數的結果還是整數（會捨去小數）。要得到精確的平均值，需要將 total 轉型為 double，並用 double 變數接收結果。"
            },

            // 3. 差一錯誤 (Off-by-one)
            new BugChallenge
            {
                Title = "陣列越界之謎",
                Difficulty = "beginner",
                Category = "邏輯錯誤",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 印出陣列所有元素
public class ArrayPrinter
{
    public void PrintAll(string[] names)
    {
        // 為什麼會 IndexOutOfRangeException？
        for (int i = 0; i <= names.Length; i++)
        {
            Console.WriteLine(names[i]);
        }
    }
}",
                FixedCode = @"// 印出陣列所有元素
public class ArrayPrinter
{
    public void PrintAll(string[] names)
    {
        // 陣列索引從 0 開始，到 Length-1 結束
        for (int i = 0; i < names.Length; i++)
        {
            Console.WriteLine(names[i]);
        }
    }
}",
                Explanation = "陣列索引從 0 開始，最後一個有效索引是 Length - 1。使用 `<=` 會導致存取 names[names.Length]，造成 IndexOutOfRangeException。應改為 `<`。"
            },

            // 4. Null Reference
            new BugChallenge
            {
                Title = "空值陷阱",
                Difficulty = "beginner",
                Category = "Null 參考",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 取得使用者名稱的長度
public class UserService
{
    public int GetNameLength(string? name)
    {
        // 為什麼有時候會 NullReferenceException？
        return name.Length;
    }
}",
                FixedCode = @"// 取得使用者名稱的長度
public class UserService
{
    public int GetNameLength(string? name)
    {
        // 先檢查 null，避免 NullReferenceException
        if (name == null) return 0;
        return name.Length;
    }
}",
                Explanation = "當 name 為 null 時，呼叫 name.Length 會拋出 NullReferenceException。應先檢查 null 或使用 `name?.Length ?? 0`。"
            },

            // 5. 錯誤的比較運算子
            new BugChallenge
            {
                Title = "賦值還是比較？",
                Difficulty = "beginner",
                Category = "運算子錯誤",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 檢查是否為管理員
public class AuthChecker
{
    public bool IsAdmin(string role)
    {
        // 為什麼每個人都變成管理員了？
        if (role == ""admin"" | role == ""Admin"")
        {
            return true;
        }
        return false;
    }

    public bool IsActive(int status)
    {
        // 這裡的判斷正確嗎？
        if (status = 1)
        {
            return true;
        }
        return false;
    }
}",
                FixedCode = @"// 檢查是否為管理員
public class AuthChecker
{
    public bool IsAdmin(string role)
    {
        // 使用 || 短路邏輯或運算子
        if (role == ""admin"" || role == ""Admin"")
        {
            return true;
        }
        return false;
    }

    public bool IsActive(int status)
    {
        // 使用 == 比較運算子，而非 = 賦值運算子
        if (status == 1)
        {
            return true;
        }
        return false;
    }
}",
                Explanation = "兩個問題：(1) 使用 `|` 是位元 OR，雖然對 bool 可用但不會短路求值，應用 `||`。(2) `status = 1` 是賦值而非比較，應使用 `==`。在 C# 中 int 不能隱式轉 bool，所以會編譯錯誤。"
            },

            // ===== 中級 (intermediate) =====

            // 6. async/await 誤用
            new BugChallenge
            {
                Title = "非同步的陷阱",
                Difficulty = "intermediate",
                Category = "Async/Await",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 非同步取得使用者資料
public class UserRepository
{
    private readonly HttpClient _client = new();

    // 為什麼這個方法會造成死結 (deadlock)？
    public User GetUser(int id)
    {
        var result = GetUserAsync(id).Result;
        return result;
    }

    public async Task<User> GetUserAsync(int id)
    {
        var response = await _client.GetAsync($""/api/users/{id}"");
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(json);
    }
}",
                FixedCode = @"// 非同步取得使用者資料
public class UserRepository
{
    private readonly HttpClient _client = new();

    // 正確做法：呼叫端也要用 async/await
    public async Task<User> GetUserAsync(int id)
    {
        var response = await _client.GetAsync($""/api/users/{id}"");
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(json);
    }
}",
                Explanation = "使用 .Result 或 .Wait() 會同步等待非同步方法完成，在有 SynchronizationContext 的環境（如 ASP.NET、WPF）中會造成死結。正確做法是一路 async/await 到底，不要混用同步呼叫。"
            },

            // 7. LINQ 錯誤
            new BugChallenge
            {
                Title = "LINQ 查詢之惑",
                Difficulty = "intermediate",
                Category = "LINQ",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 找出所有及格學生的平均分數
public class StudentAnalyzer
{
    public double GetPassingAverage(List<Student> students)
    {
        // 為什麼沒有及格學生時會崩潰？
        var average = students
            .Where(s => s.Score >= 60)
            .Average(s => s.Score);

        return average;
    }

    public Student? FindTopStudent(List<Student> students)
    {
        // 為什麼找不到最高分的學生？
        var top = students
            .OrderBy(s => s.Score)
            .FirstOrDefault();

        return top;
    }
}",
                FixedCode = @"// 找出所有及格學生的平均分數
public class StudentAnalyzer
{
    public double GetPassingAverage(List<Student> students)
    {
        // 先檢查是否有及格學生，避免 InvalidOperationException
        var passing = students.Where(s => s.Score >= 60);
        if (!passing.Any()) return 0;
        return passing.Average(s => s.Score);
    }

    public Student? FindTopStudent(List<Student> students)
    {
        // 應使用 OrderByDescending 取最高分
        var top = students
            .OrderByDescending(s => s.Score)
            .FirstOrDefault();

        return top;
    }
}",
                Explanation = "兩個問題：(1) 對空序列呼叫 Average() 會拋出 InvalidOperationException，需先檢查是否有元素。(2) OrderBy 是升序排列，FirstOrDefault 會取到最低分，應改用 OrderByDescending。"
            },

            // 8. foreach 中修改集合
            new BugChallenge
            {
                Title = "迭代中的禁忌",
                Difficulty = "intermediate",
                Category = "集合操作",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 移除所有不及格的學生
public class ClassManager
{
    public void RemoveFailingStudents(List<Student> students)
    {
        // 為什麼會拋出 InvalidOperationException？
        foreach (var student in students)
        {
            if (student.Score < 60)
            {
                students.Remove(student);
            }
        }
    }
}",
                FixedCode = @"// 移除所有不及格的學生
public class ClassManager
{
    public void RemoveFailingStudents(List<Student> students)
    {
        // 使用 RemoveAll 安全地移除符合條件的元素
        students.RemoveAll(s => s.Score < 60);
    }
}",
                Explanation = "在 foreach 迴圈中修改正在迭代的集合會拋出 InvalidOperationException。可以使用 RemoveAll()、倒序 for 迴圈、或先用 ToList() 建立副本再操作。"
            },

            // 9. 變數作用域問題
            new BugChallenge
            {
                Title = "看不見的變數",
                Difficulty = "intermediate",
                Category = "變數作用域",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 找出最大值
public class MaxFinder
{
    public int FindMax(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            int max = numbers[0];
            if (numbers[i] > max)
            {
                max = numbers[i];
            }
        }
        // 編譯錯誤：max 不存在於此作用域
        return max;
    }
}",
                FixedCode = @"// 找出最大值
public class MaxFinder
{
    public int FindMax(int[] numbers)
    {
        // 將 max 宣告在迴圈外面
        int max = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] > max)
            {
                max = numbers[i];
            }
        }
        return max;
    }
}",
                Explanation = "變數 max 宣告在 for 迴圈的區塊內，離開迴圈後就無法存取。應將 max 宣告在迴圈之前。另外，每次迴圈都重設 max = numbers[0] 也會導致邏輯錯誤，永遠只比較最後一個元素和第一個元素。"
            },

            // 10. 字串比較問題
            new BugChallenge
            {
                Title = "字串比較的學問",
                Difficulty = "intermediate",
                Category = "字串處理",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 檢查檔案副檔名
public class FileValidator
{
    public bool IsImageFile(string fileName)
    {
        // 為什麼 ""Photo.JPG"" 不被認為是圖片？
        string ext = fileName.Substring(fileName.LastIndexOf('.'));
        if (ext == "".jpg"" || ext == "".png"" || ext == "".gif"")
        {
            return true;
        }
        return false;
    }

    public bool HasSameContent(string a, string b)
    {
        // 土耳其語系問題：為什麼 ""file"" 和 ""FILE"" 有時比較失敗？
        return a.ToLower() == b.ToLower();
    }
}",
                FixedCode = @"// 檢查檔案副檔名
public class FileValidator
{
    public bool IsImageFile(string fileName)
    {
        // 使用 StringComparison.OrdinalIgnoreCase 忽略大小寫
        string ext = Path.GetExtension(fileName);
        return ext.Equals("".jpg"", StringComparison.OrdinalIgnoreCase)
            || ext.Equals("".png"", StringComparison.OrdinalIgnoreCase)
            || ext.Equals("".gif"", StringComparison.OrdinalIgnoreCase);
    }

    public bool HasSameContent(string a, string b)
    {
        // 使用 OrdinalIgnoreCase 避免文化特定的大小寫轉換問題
        return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    }
}",
                Explanation = "字串比較應使用 StringComparison.OrdinalIgnoreCase 來忽略大小寫，而非手動 ToLower()。ToLower() 會受到文化設定影響（如土耳其語的 I 問題）。另外應使用 Path.GetExtension() 取得副檔名，更安全可靠。"
            },

            // ===== 高級 (advanced) =====

            // 11. 死結 (Deadlock)
            new BugChallenge
            {
                Title = "死結迷局",
                Difficulty = "advanced",
                Category = "多執行緒",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 銀行轉帳系統
public class BankAccount
{
    private readonly object _lock = new();
    public decimal Balance { get; private set; }
    public string Name { get; }

    public BankAccount(string name, decimal balance)
    {
        Name = name;
        Balance = balance;
    }

    // 為什麼兩人互相轉帳時會卡住？
    public static void Transfer(BankAccount from, BankAccount to, decimal amount)
    {
        lock (from._lock)
        {
            Thread.Sleep(100); // 模擬處理時間
            lock (to._lock)
            {
                if (from.Balance >= amount)
                {
                    from.Balance -= amount;
                    to.Balance += amount;
                }
            }
        }
    }
}",
                FixedCode = @"// 銀行轉帳系統
public class BankAccount
{
    private readonly object _lock = new();
    public decimal Balance { get; private set; }
    public string Name { get; }
    public int Id { get; }

    public BankAccount(int id, string name, decimal balance)
    {
        Id = id;
        Name = name;
        Balance = balance;
    }

    // 使用固定順序取得鎖，避免死結
    public static void Transfer(BankAccount from, BankAccount to, decimal amount)
    {
        // 永遠先鎖 Id 較小的帳戶，確保鎖的順序一致
        var first = from.Id < to.Id ? from : to;
        var second = from.Id < to.Id ? to : from;

        lock (first._lock)
        {
            lock (second._lock)
            {
                if (from.Balance >= amount)
                {
                    from.Balance -= amount;
                    to.Balance += amount;
                }
            }
        }
    }
}",
                Explanation = "死結發生在兩個執行緒以相反順序取得鎖時：執行緒 A 鎖了帳戶 1 等帳戶 2，執行緒 B 鎖了帳戶 2 等帳戶 1。解決方法是統一鎖的取得順序（如按 Id 排序），確保所有執行緒都以相同順序取得鎖。"
            },

            // 12. 競爭條件 (Race Condition)
            new BugChallenge
            {
                Title = "競爭危機",
                Difficulty = "advanced",
                Category = "多執行緒",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 計數器服務
public class CounterService
{
    private int _count = 0;

    // 為什麼多執行緒下計數不正確？
    public void Increment()
    {
        _count = _count + 1;
    }

    public int GetCount() => _count;

    // 單例模式也有問題嗎？
    private static CounterService? _instance;

    public static CounterService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CounterService();
            }
            return _instance;
        }
    }
}",
                FixedCode = @"// 計數器服務
public class CounterService
{
    private int _count = 0;

    // 使用 Interlocked 確保原子操作
    public void Increment()
    {
        Interlocked.Increment(ref _count);
    }

    public int GetCount() => Volatile.Read(ref _count);

    // 使用 Lazy<T> 確保執行緒安全的單例
    private static readonly Lazy<CounterService> _instance
        = new(() => new CounterService());

    public static CounterService Instance => _instance.Value;
}",
                Explanation = "競爭條件：`_count = _count + 1` 不是原子操作（讀取、加一、寫回三步驟），多執行緒同時執行會遺失更新。應使用 Interlocked.Increment。單例模式的 check-then-act 也非執行緒安全，可能建立多個實例，應使用 Lazy<T>。"
            },

            // 13. 事件造成的記憶體洩漏
            new BugChallenge
            {
                Title = "記憶體黑洞",
                Difficulty = "advanced",
                Category = "記憶體管理",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 事件訂閱系統
public class EventPublisher
{
    public event EventHandler? DataChanged;

    public void UpdateData()
    {
        DataChanged?.Invoke(this, EventArgs.Empty);
    }
}

public class DataDisplay
{
    private readonly EventPublisher _publisher;

    public DataDisplay(EventPublisher publisher)
    {
        _publisher = publisher;
        // 訂閱事件但從未取消訂閱
        _publisher.DataChanged += OnDataChanged;
    }

    private void OnDataChanged(object? sender, EventArgs e)
    {
        Console.WriteLine(""資料已更新"");
    }

    // 為什麼 DataDisplay 無法被 GC 回收？
}",
                FixedCode = @"// 事件訂閱系統
public class EventPublisher
{
    public event EventHandler? DataChanged;

    public void UpdateData()
    {
        DataChanged?.Invoke(this, EventArgs.Empty);
    }
}

public class DataDisplay : IDisposable
{
    private readonly EventPublisher _publisher;
    private bool _disposed = false;

    public DataDisplay(EventPublisher publisher)
    {
        _publisher = publisher;
        _publisher.DataChanged += OnDataChanged;
    }

    private void OnDataChanged(object? sender, EventArgs e)
    {
        Console.WriteLine(""資料已更新"");
    }

    // 實作 IDisposable 以取消訂閱事件
    public void Dispose()
    {
        if (!_disposed)
        {
            _publisher.DataChanged -= OnDataChanged;
            _disposed = true;
        }
    }
}",
                Explanation = "事件訂閱會建立從發佈者到訂閱者的強參考。如果不取消訂閱，即使訂閱者已不再使用，GC 也無法回收它，造成記憶體洩漏。應實作 IDisposable 介面，在 Dispose 方法中取消事件訂閱。"
            },

            // 14. 泛型約束錯誤
            new BugChallenge
            {
                Title = "泛型的束縛",
                Difficulty = "advanced",
                Category = "泛型",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 泛型排序比較器
public class GenericComparer<T>
{
    // 為什麼編譯器說不能用 < 和 > ？
    public T FindMin(T[] items)
    {
        T min = items[0];
        for (int i = 1; i < items.Length; i++)
        {
            if (items[i] < min)
            {
                min = items[i];
            }
        }
        return min;
    }

    // 為什麼不能 new T() ？
    public T CreateDefault()
    {
        return new T();
    }
}",
                FixedCode = @"// 泛型排序比較器
public class GenericComparer<T> where T : IComparable<T>, new()
{
    // 使用 IComparable<T>.CompareTo 進行比較
    public T FindMin(T[] items)
    {
        T min = items[0];
        for (int i = 1; i < items.Length; i++)
        {
            if (items[i].CompareTo(min) < 0)
            {
                min = items[i];
            }
        }
        return min;
    }

    // 加上 new() 約束才能建立實例
    public T CreateDefault()
    {
        return new T();
    }
}",
                Explanation = "泛型型別 T 預設沒有任何能力：不能用 < > 比較、不能 new。需要加上泛型約束：`where T : IComparable<T>` 才能用 CompareTo 比較大小，`where T : new()` 才能建立新實例。"
            },

            // 15. SQL Injection 漏洞
            new BugChallenge
            {
                Title = "SQL 注入危機",
                Difficulty = "advanced",
                Category = "安全性",
                BugCount = 1,
                Language = "csharp",
                BuggyCode = @"// 使用者搜尋功能
public class UserSearchService
{
    private readonly string _connectionString;

    public UserSearchService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // 這段程式碼有什麼安全漏洞？
    public List<User> SearchUsers(string keyword)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        // 直接拼接使用者輸入到 SQL 字串中
        var sql = $""SELECT * FROM Users WHERE Name LIKE '%{keyword}%'"";
        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        var users = new List<User>();
        while (reader.Read())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }
        return users;
    }
}",
                FixedCode = @"// 使用者搜尋功能
public class UserSearchService
{
    private readonly string _connectionString;

    public UserSearchService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // 使用參數化查詢防止 SQL Injection
    public List<User> SearchUsers(string keyword)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        // 使用 @parameter 參數化查詢
        var sql = ""SELECT * FROM Users WHERE Name LIKE @keyword"";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue(""@keyword"", $""%{keyword}%"");
        using var reader = cmd.ExecuteReader();

        var users = new List<User>();
        while (reader.Read())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }
        return users;
    }
}",
                Explanation = "直接將使用者輸入拼接到 SQL 字串中會造成 SQL Injection 漏洞。攻擊者可以輸入 `'; DROP TABLE Users; --` 來刪除整個資料表。正確做法是使用參數化查詢（Parameterized Query），讓資料庫引擎自動處理特殊字元的跳脫。"
            },
        };
    }
}
