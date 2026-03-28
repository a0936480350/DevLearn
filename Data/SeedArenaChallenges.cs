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
            }
        };
    }
}
