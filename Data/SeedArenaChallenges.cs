using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedArenaChallenges
{
    public static List<ArenaChallenge> GetChallenges()
    {
        return new List<ArenaChallenge>
        {
            new ArenaChallenge
            {
                Title = "最短的 FizzBuzz",
                Description = "用最少行數寫出 FizzBuzz！規則：印出 1 到 100，遇到 3 的倍數印 Fizz、5 的倍數印 Buzz、同時是 3 和 5 的倍數印 FizzBuzz。看誰的程式碼最精簡！可以使用任何語言，但要能正確執行。",
                Category = "code-golf",
                Difficulty = "beginner",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true
            },
            new ArenaChallenge
            {
                Title = "優雅的排序",
                Description = "不使用內建的 Sort() 或 OrderBy() 方法，自己實作一個排序演算法！可以是氣泡排序、快速排序、合併排序等。評比標準：程式碼的可讀性、效能、以及註解的清晰度。額外加分：實作多種排序並比較效能。",
                Category = "algorithm",
                Difficulty = "intermediate",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true
            },
            new ArenaChallenge
            {
                Title = "字串反轉大師",
                Description = "用最多種不同的方式來反轉一個字串！每種方式都要能正確處理中文、emoji 等 Unicode 字元。評比標準：方法的數量與創意程度。提示：可以考慮遞迴、迴圈、LINQ、Stack、Array.Reverse 等各種方式。",
                Category = "creativity",
                Difficulty = "beginner",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true
            },
            new ArenaChallenge
            {
                Title = "LINQ 魔法",
                Description = "用一行 LINQ 解決以下問題：給定一組學生資料（姓名、科目、分數），找出每個科目的最高分學生，並按照平均分數排序輸出。評比標準：LINQ 的優雅程度、可讀性、以及是否真的只用了一行。",
                Category = "linq",
                Difficulty = "advanced",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true
            },
            new ArenaChallenge
            {
                Title = "API 設計挑戰",
                Description = "設計一個最符合 RESTful 原則的 API Controller！主題：線上書店管理系統。需要包含：書籍的 CRUD、搜尋/篩選/分頁、適當的 HTTP 狀態碼、清晰的路由設計。評比標準：RESTful 程度、錯誤處理、程式碼結構。",
                Category = "api-design",
                Difficulty = "advanced",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true
            },
            new ArenaChallenge
            {
                Title = "最快的斐波那契數列",
                Description = "計算第 N 個斐波那契數（N 最大到 1000），比拼誰的演算法最快！可以使用遞迴、動態規劃、矩陣快速冪等任何方法。評比標準：正確性、執行速度、記憶體使用量。額外加分：處理大數（BigInteger）且不溢位。",
                Category = "algorithm",
                Difficulty = "intermediate",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true
            },
            new ArenaChallenge
            {
                Title = "設計一個簡易快取系統",
                Description = "實作一個具有 LRU（Least Recently Used）淘汰策略的快取系統！需要支援：Get(key)、Set(key, value, ttl)、Delete(key)、以及容量上限自動淘汰。評比標準：執行緒安全性、時間複雜度、API 設計的優雅程度。進階挑戰：支援過期自動清除。",
                Category = "system-design",
                Difficulty = "advanced",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true
            },
            new ArenaChallenge
            {
                Title = "手寫一個 Mini ORM",
                Description = "不使用 EF Core 或 Dapper，自己實作一個迷你 ORM！需要支援：透過泛型方法查詢資料表、自動將 DataReader 映射到物件、支援基本的 WHERE 條件篩選。評比標準：反射的運用、SQL 注入防護、程式碼的可擴展性。額外加分：支援 Insert 和 Update。",
                Category = "system-design",
                Difficulty = "advanced",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true
            }
        };
    }
}
