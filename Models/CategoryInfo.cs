namespace DotNetLearning.Models;

/// <summary>
/// 章節分類的顯示資訊（名稱、圖示、主色、描述）。
/// 單一真相來源 — 新增分類時**只要改這裡**，Index 首頁卡片、Chapter 側欄、
/// 未來任何用 Category 的地方都自動跟上。
///
/// 不在表內的 Category 值仍會渲染（safety net），用 Slug 當顯示名稱，
/// 避免側欄 / 首頁漏掉新章節。
/// </summary>
public record CategoryInfo(string Key, string Name, string Icon, string Color, string Desc)
{
    /// <summary>是否屬於「核心路線」三大主軸（首頁首屏顯示）。</summary>
    public bool IsCore { get; init; }
}

public static class CategoryRegistry
{
    /// <summary>
    /// 依「顯示順序」排列的完整清單。Index / Chapter 兩邊都消費這份。
    /// </summary>
    public static readonly IReadOnlyList<CategoryInfo> All = new List<CategoryInfo>
    {
        // ── 核心路線（有順序的三大主軸）──
        new("csharp",   "C# 基礎",          "🔷", "#9B59B6", "語言根基：變數、OOP、LINQ、非同步") { IsCore = true },
        new("aspnet",   "ASP.NET Core MVC", "🌐", "#3498DB", "Web 開發：路由、Controller、DI、API") { IsCore = true },
        new("database", "資料庫 & ORM",      "🗄️", "#E74C3C", "資料層：SQL、EF Core、Dapper、優化") { IsCore = true },

        // ── 前後端基礎 ──
        new("frontend",   "前端基礎",        "🎨", "#E91E63", "HTML / CSS / JavaScript 與前後端整合"),
        new("html",       "HTML & CSS",       "📄", "#E44D26", "網頁基礎：標籤、表單、語意化、Flexbox、Grid、RWD"),
        new("javascript", "JavaScript",       "🟨", "#F7DF1E", "前端核心語言：變數、函式、DOM、事件、Promise、ES6+"),
        new("sql",        "SQL 資料庫查詢",    "📘", "#336791", "純 SQL 教學：SELECT、JOIN、子查詢、索引、交易、Window Functions"),

        // ── 前端框架 ──
        new("vue",      "Vue.js",  "💚", "#42B883", "漸進式框架：Composition API、Router、Pinia"),
        new("react",    "React",   "⚛️", "#61DAFB", "元件化 UI：Hooks、JSX、Redux、Next.js"),
        new("angular",  "Angular", "🅰️", "#DD0031", "企業級框架：TypeScript、RxJS、DI、Forms"),

        // ── 工具 & 基礎建設 ──
        new("git",           "Git 版本控制",       "📝", "#F57C00", "版本管理、分支、GitHub 協作"),
        new("server",        "架 Server 概念",     "🖥️", "#00897B", "Nginx、反向代理、負載平衡、監控"),
        new("docker",        "Docker & 部署",      "🐳", "#1ABC9C", "容器化：Docker、CI/CD、雲端部署"),
        new("network",       "網路 TCP/IP",        "🌍", "#27AE60", "通訊基礎：OSI、HTTP、DNS、WebSocket"),
        new("security",      "資安",               "🔒", "#F39C12", "安全開發：JWT、加密、OWASP Top 10"),
        new("patterns",      "設計模式 & SOLID",   "🏛️", "#E67E22", "架構思維：SOLID、GoF、Clean Architecture"),
        new("infrastructure","快取 & 日誌 & 效能", "⚡", "#9B59B6", "系統調校：Caching、Serilog、Profiling"),

        // ── 架構 & 深入概念 ──
        new("microservices",    "微服務架構",  "🏗️", "#7C3AED", "DDD、API Gateway、Docker、Saga、K8s"),
        new("redis",            "Redis 快取",   "🔴", "#DC382D", "快取策略、分散式架構、Pub/Sub、效能優化"),
        new("concept-backend",  "後端概念深入", "🧠", "#6366F1", "DI 為什麼不 new？SOLID、async 真相、LINQ 陷阱、設計模式實戰"),
        new("concept-arch",     "架構概念深入", "🏗️", "#14B8A6", "REST 哲學、認證對比、快取策略、微服務 vs 單體、系統設計"),
        new("concept-frontend", "前端概念深入", "🎯", "#F43F5E", "Event Loop、閉包記憶體、Virtual DOM、SSR/CSR、效能優化"),

        // ── AI 相關 ──
        new("ai",         "AI 應用",           "🤖", "#FF6B6B", "AI 輔助：Prompt、MCP、Claude Code"),
        new("aimodel",    "AI 模型與語言",     "🧠", "#7C4DFF", "LLM、RAG、Embedding、IPAS 知識"),
        new("claudecode", "Claude Code 工具",  "🔌", "#00BCD4", "不用 CMD 做專案、No-Code 流程"),
        new("ipas-ai",    "iPAS AI 應用規劃師", "🎓", "#FF6F00", "經濟部 iPAS 證照：AI 基礎、機器學習、生成式 AI、企業導入策略｜508 題考古題"),

        // ── 實戰 ──
        new("project", "專案實戰",        "🛒", "#4CAF50", "電商、部落格、API 微服務完整開發"),
        new("iot",     "IoT & POS 系統",  "🍓", "#FF5722", "Raspberry Pi、POS 開發、硬體整合"),
    };

    /// <summary>依 Key 快速查找。不在表中的 Key → 回傳「fallback」CategoryInfo（保底顯示用）。</summary>
    public static CategoryInfo Get(string key) =>
        _byKey.TryGetValue(key, out var info) ? info : Fallback(key);

    /// <summary>是否已在註冊表中（給 view 判斷「要顯示原生名稱還是 fallback」用）。</summary>
    public static bool IsRegistered(string key) => _byKey.ContainsKey(key);

    /// <summary>核心三大（首頁首屏）。</summary>
    public static IEnumerable<CategoryInfo> Core => All.Where(c => c.IsCore);

    /// <summary>除核心外的進階擴充。</summary>
    public static IEnumerable<CategoryInfo> Advanced => All.Where(c => !c.IsCore);

    /// <summary>
    /// 給定 DB 裡實際出現的 Category 清單，按照 registry 順序排出「該顯示的分類」。
    /// 若 DB 冒出 registry 沒寫的新分類，會 append 在尾端，用 fallback 名稱，**絕不漏掉**。
    /// </summary>
    public static IReadOnlyList<CategoryInfo> OrderedForRender(IEnumerable<string> dbCategoryKeys)
    {
        var present = new HashSet<string>(dbCategoryKeys);
        var result = new List<CategoryInfo>();
        // 先放 registry 內、且 DB 有的，保持順序
        foreach (var info in All)
            if (present.Contains(info.Key)) result.Add(info);
        // DB 有、但 registry 沒寫到的 → fallback（新分類自動顯示，不必改 code）
        foreach (var key in present)
            if (!_byKey.ContainsKey(key)) result.Add(Fallback(key));
        return result;
    }

    private static readonly Dictionary<string, CategoryInfo> _byKey =
        All.ToDictionary(c => c.Key, c => c);

    private static CategoryInfo Fallback(string key) =>
        new(key, key, "📁", "#6B7280", "");
}
