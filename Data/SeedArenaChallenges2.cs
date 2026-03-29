using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedArenaChallenges2
{
    public static List<ArenaChallenge> GetChallenges()
    {
        return new List<ArenaChallenge>
        {
            new ArenaChallenge
            {
                Id = 100,
                Title = "最快的斐波那契",
                Description = "用最高效方式計算第 N 個斐波那契數！\n\n要求：\n1. 實作一個方法 long Fibonacci(int n)，回傳第 n 個斐波那契數（n 從 0 開始）\n2. 必須能處理 n = 90 以內的數值\n3. 時間複雜度必須優於 O(2^n) 的遞迴暴力解\n\n評比標準：\n- 效能：用 Stopwatch 量測，越快越好\n- 正確性：Fibonacci(0)=0, Fibonacci(1)=1, Fibonacci(10)=55, Fibonacci(50)=12586269025\n- 程式碼可讀性與註解\n\n提示：可以考慮迭代法、矩陣快速冪、或記憶化遞迴。\n額外加分：實作多種方法並比較效能差異！",
                Category = "algorithm",
                Difficulty = "intermediate",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(90),
                IsActive = true
            },
            new ArenaChallenge
            {
                Id = 101,
                Title = "手寫 Mini ORM",
                Description = "實作一個簡易的物件關聯映射（ORM）！\n\n要求：\n1. 建立一個 MiniOrm 類別，支援泛型操作\n2. 實作 Query<T>(string sql) 方法，將 DataReader 自動映射到物件\n3. 實作 Insert<T>(T entity) 方法，自動產生 INSERT SQL\n4. 使用 Reflection 讀取屬性名稱與型別\n5. 支援 [Column(\"欄位名\")] Attribute 自訂映射\n\n評比標準：\n- 功能完整度：Query 與 Insert 都要能正確運作\n- 型別支援：至少支援 int、string、DateTime、bool、decimal\n- 錯誤處理：欄位不存在或型別不符時的處理\n- 程式碼品質與設計模式\n\n額外加分：支援 Update、Delete 或 Where 條件產生！",
                Category = "architecture",
                Difficulty = "advanced",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(90),
                IsActive = true
            },
            new ArenaChallenge
            {
                Id = 102,
                Title = "設計快取系統",
                Description = "實作一個 LRU（Least Recently Used）Cache！\n\n要求：\n1. 實作 LruCache<TKey, TValue> 類別\n2. 建構子接收 capacity 參數，指定快取容量上限\n3. 實作 Get(TKey key) 方法：取得值，不存在回傳 default\n4. 實作 Put(TKey key, TValue value) 方法：新增或更新值\n5. 當容量超過上限時，自動移除最久未使用的項目\n6. Get 和 Put 的時間複雜度都必須是 O(1)\n\n評比標準：\n- 正確性：LRU 淘汰邏輯正確\n- 效能：O(1) 的 Get/Put（提示：Dictionary + LinkedList）\n- 執行緒安全：加分項目\n- 單元測試覆蓋率\n\n額外加分：加入過期時間（TTL）支援！",
                Category = "data-structure",
                Difficulty = "intermediate",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(90),
                IsActive = true
            },
            new ArenaChallenge
            {
                Id = 103,
                Title = "字串壓縮演算法",
                Description = "實作 Run-Length Encoding（RLE）字串壓縮與解壓縮！\n\n要求：\n1. 實作 string Encode(string input) 方法：壓縮字串\n   - 例如：\"AAABBBCCDA\" → \"3A3B2C1D1A\"\n2. 實作 string Decode(string encoded) 方法：解壓縮字串\n   - 例如：\"3A3B2C1D1A\" → \"AAABBBCCDA\"\n3. 處理邊界情況：空字串、單一字元、全部不重複\n4. 如果壓縮後比原字串還長，回傳原字串\n\n評比標準：\n- 正確性：Encode 後 Decode 必須還原\n- 邊界處理：特殊字元、數字字元、Unicode\n- 效能：使用 StringBuilder 避免不必要的字串配置\n- 程式碼簡潔度\n\n額外加分：支援數字超過 9 的連續字元（如 12A 表示 12 個 A）！",
                Category = "algorithm",
                Difficulty = "beginner",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(90),
                IsActive = true
            },
            new ArenaChallenge
            {
                Id = 104,
                Title = "並行下載器",
                Description = "用 async/await 實作多檔案同時下載器！\n\n要求：\n1. 實作 ParallelDownloader 類別\n2. 實作 async Task<DownloadResult[]> DownloadAll(string[] urls, int maxConcurrency) 方法\n3. 使用 SemaphoreSlim 限制同時下載數量\n4. 每個下載結果包含：URL、檔案大小、耗時、是否成功、錯誤訊息\n5. 支援下載進度回報（透過 IProgress<T> 或事件）\n6. 任一檔案失敗不影響其他檔案下載\n\n評比標準：\n- 並行控制：SemaphoreSlim 正確限制並行數\n- 錯誤處理：逾時、網路錯誤、404 等狀況\n- 進度回報：即時回報各檔案下載進度\n- 取消支援：CancellationToken 支援\n\n額外加分：實作斷點續傳功能！",
                Category = "async",
                Difficulty = "advanced",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(90),
                IsActive = true
            }
        };
    }
}
