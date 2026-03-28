using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Server
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Server Chapter 520 ────────────────────────────
        new() { Id=520, Category="server", Order=1, Level="beginner", Icon="🖥️", Title="Web Server 基礎概念", Slug="web-server-basics", IsPublished=true, Content=@"
# Web Server 基礎概念

## 什麼是 Web Server？

> 💡 **比喻：餐廳的櫃台接待**
> 想像一間餐廳：
> - **客人**（瀏覽器）走進餐廳
> - **櫃台接待**（Web Server）迎接客人
> - 客人說：「我要看菜單」（HTTP Request）
> - 接待員把菜單拿給客人（HTTP Response）
> - 如果客人點了一道需要現做的菜（動態內容），接待員會把訂單送到**廚房**（應用程式）
> - 如果客人只要一瓶水（靜態檔案），接待員直接從冰箱拿
>
> Web Server 就是這個**櫃台接待**——負責接收請求、分派工作、回傳結果。

### Web Server 的核心工作

```
客人（瀏覽器）        櫃台接待（Web Server）        廚房（應用程式）
    |                       |                           |
    |--- 我要看首頁 ------->|                           |
    |                       |--- 這是動態頁面 --------->|
    |                       |<-- 做好了，給你 HTML ------|
    |<-- 這是你要的首頁 ----|                           |
    |                       |                           |
    |--- 我要 logo.png ---->|                           |
    |<-- 直接給你圖片 ------|  （靜態檔案不用問廚房）     |
```

---

## Kestrel vs IIS vs Nginx

### 三大 Web Server 比較

```
// 三種常見 Web Server 的角色比較
// Kestrel：ASP.NET Core 內建的輕量級 Server
// IIS：Windows 專用的老牌 Server
// Nginx：跨平台的高效能 Server

特性              Kestrel          IIS              Nginx
─────────────────────────────────────────────────────────────
平台              跨平台           僅 Windows        跨平台          // 部署環境的限制
效能              高               中等             非常高          // 處理請求的速度
內建於 .NET       是               否               否              // 是否開箱即用
反向代理          不建議直接暴露    可以             最常用          // 面對外網的能力
靜態檔案處理      普通             好               非常好          // 處理圖片CSS等
SSL/TLS          支援             支援             最佳            // 加密連線能力
```

> 💡 **比喻：接待員的等級**
> - **Kestrel** 像是餐廳的**實習接待員**——做事很快，但經驗不足，不適合獨自面對大量客人
> - **IIS** 像是**資深接待員**——經驗豐富，但只在特定餐廳（Windows）工作
> - **Nginx** 像是**五星級飯店的大堂經理**——處理大量客人游刃有餘，還能協調多間餐廳

### 最佳實踐架構

```
// 生產環境推薦架構
// Nginx 在前面負責接待（反向代理）
// Kestrel 在後面負責處理（應用程式）

外部請求 → Nginx（反向代理）→ Kestrel（ASP.NET Core 應用）

// 這樣的好處：
// 1. Nginx 擅長處理靜態檔案和 SSL
// 2. Kestrel 專心處理業務邏輯
// 3. Nginx 提供額外的安全防護
```

---

## 靜態檔案 vs 動態內容

### 靜態檔案

```csharp
// Program.cs 中啟用靜態檔案服務
var builder = WebApplication.CreateBuilder(args); // 建立應用程式建構器
var app = builder.Build();                         // 建構應用程式

app.UseStaticFiles(); // 啟用 wwwroot 資料夾中的靜態檔案服務

// 靜態檔案的存放位置：
// wwwroot/
//   ├── css/          // 樣式表檔案
//   │   └── site.css  // 網站主要樣式
//   ├── js/           // JavaScript 檔案
//   │   └── app.js    // 前端邏輯
//   ├── images/       // 圖片檔案
//   │   └── logo.png  // 網站 Logo
//   └── favicon.ico   // 網站圖示
```

### 動態內容

```csharp
// 動態內容由 Controller 或 Minimal API 產生
// 每次請求都可能回傳不同的結果

app.MapGet(""/api/time"", () =>        // 定義一個 API 端點
{
    return DateTime.Now.ToString();     // 每次呼叫回傳當下時間（動態）
});

app.MapGet(""/api/users/{id}"", (int id) =>  // 根據使用者 ID 查詢
{
    return $""使用者 {id} 的資料"";            // 根據參數回傳不同結果
});
```

---

## Port、Binding、Listen

### 什麼是 Port？

> 💡 **比喻：大樓的房間號碼**
> IP 位址像是一棟大樓的地址，Port 就是大樓裡的**房間號碼**。
> - 80 號房 → HTTP 接待室
> - 443 號房 → HTTPS 加密接待室
> - 5000 號房 → 你的 ASP.NET Core 應用
> - 一棟大樓（一台電腦）可以有 65535 個房間

### 設定 Port 和 Binding

```csharp
// Program.cs 中設定監聽的 Port
var builder = WebApplication.CreateBuilder(args); // 建立建構器

// 方法一：透過程式碼設定
builder.WebHost.UseUrls(
    ""http://localhost:5000"",   // 監聽 HTTP 5000 Port
    ""https://localhost:5001""   // 監聽 HTTPS 5001 Port
);

// 方法二：透過命令列參數
// dotnet run --urls ""http://localhost:8080""    // 指定單一 Port
// dotnet run --urls ""http://*:80""              // 監聽所有網路介面的 80 Port
```

```json
// launchSettings.json 中的設定
{
  ""profiles"": {                          // 啟動設定檔
    ""MyApp"": {                           // 應用程式名稱
      ""commandName"": ""Project"",        // 使用專案啟動
      ""applicationUrl"": ""https://localhost:5001;http://localhost:5000"",  // 監聽的網址
      ""environmentVariables"": {          // 環境變數
        ""ASPNETCORE_ENVIRONMENT"": ""Development""  // 開發環境
      }
    }
  }
}
```

---

## HTTP Request/Response 完整流程

### 一個請求的完整旅程

```
// 當你在瀏覽器輸入 https://myapp.com/products 時發生了什麼？

步驟 1：DNS 解析                        // 把網址轉成 IP 位址
  myapp.com → 123.45.67.89             // 就像查電話簿

步驟 2：TCP 連線                        // 建立通訊通道
  三次握手 (SYN → SYN-ACK → ACK)       // 就像打電話先確認對方在

步驟 3：TLS 握手（HTTPS）               // 建立加密通道
  交換憑證、協商加密方式                  // 就像雙方約定暗號

步驟 4：發送 HTTP Request               // 送出請求
  GET /products HTTP/1.1                // 我要看商品列表
  Host: myapp.com                       // 目標主機
  Accept: text/html                     // 我想要 HTML 格式

步驟 5：Server 處理請求                  // Web Server 接收並處理
  Nginx → Kestrel → Controller → DB    // 經過層層處理

步驟 6：回傳 HTTP Response              // 送回結果
  HTTP/1.1 200 OK                       // 狀態碼 200 表示成功
  Content-Type: text/html               // 內容是 HTML
  <html>...</html>                      // 實際的網頁內容

步驟 7：瀏覽器渲染                       // 把 HTML 變成你看到的畫面
```

### 常見 HTTP 狀態碼

```
// 狀態碼就像櫃台接待的回應
200 OK              // 沒問題，這是你要的東西
301 Moved           // 搬家了，去新地址找
304 Not Modified    // 跟上次一樣，用你的快取就好
400 Bad Request     // 你的要求我看不懂
401 Unauthorized    // 你還沒登入，請先登入
403 Forbidden       // 你沒有權限看這個
404 Not Found       // 找不到你要的東西
500 Server Error    // 我們這邊出問題了，不是你的錯
502 Bad Gateway     // 後面的廚房（應用程式）沒回應
503 Unavailable     // 我們暫時休息中
```

---

## appsettings.json 環境設定

```json
// appsettings.json - 基本設定（所有環境共用）
{
  ""Logging"": {                              // 日誌設定區塊
    ""LogLevel"": {                           // 日誌等級設定
      ""Default"": ""Information"",           // 預設記錄 Information 以上
      ""Microsoft.AspNetCore"": ""Warning""   // ASP.NET Core 只記錄 Warning 以上
    }
  },
  ""AllowedHosts"": ""*"",                    // 允許所有主機名稱存取
  ""ConnectionStrings"": {                    // 資料庫連線字串
    ""DefaultConnection"": ""Server=localhost;Database=MyDb;Trusted_Connection=true""  // 本機 SQL Server
  },
  ""AppSettings"": {                          // 自訂應用程式設定
    ""SiteName"": ""我的網站"",               // 網站名稱
    ""MaxUploadSize"": 10485760              // 最大上傳大小（10MB）
  }
}
```

```json
// appsettings.Development.json - 開發環境專用（會覆蓋基本設定）
{
  ""Logging"": {                              // 開發環境的日誌設定
    ""LogLevel"": {                           // 日誌等級
      ""Default"": ""Debug""                  // 開發時記錄更詳細的 Debug 等級
    }
  },
  ""AppSettings"": {                          // 開發環境的應用設定
    ""SiteName"": ""我的網站（開發版）""       // 開發版網站名稱
  }
}
```

```csharp
// 在程式中讀取設定
var builder = WebApplication.CreateBuilder(args); // 建構器會自動載入設定檔

// 讀取設定值
var siteName = builder.Configuration[""AppSettings:SiteName""];        // 用冒號分隔階層
var maxSize = builder.Configuration.GetValue<int>(""AppSettings:MaxUploadSize""); // 轉型讀取

// 用強型別讀取設定（推薦做法）
builder.Services.Configure<AppSettings>(                              // 綁定設定到類別
    builder.Configuration.GetSection(""AppSettings"")                  // 指定設定區塊
);

// 在 Controller 或 Service 中注入使用
public class HomeController : Controller                              // 控制器
{
    private readonly AppSettings _settings;                           // 設定欄位

    public HomeController(IOptions<AppSettings> options)              // 透過 DI 注入
    {
        _settings = options.Value;                                    // 取得設定值
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤一：在生產環境直接暴露 Kestrel

```csharp
// 錯誤：讓 Kestrel 直接面對外部網路
builder.WebHost.UseUrls(""http://*:80"");  // 直接監聽 80 Port 對外服務

// 問題：Kestrel 缺乏完整的安全防護
// 沒有速率限制、沒有請求過濾、沒有 DDoS 防護

// ✅ 正確做法：前面加上 Nginx 反向代理
// Nginx 負責對外，Kestrel 只監聽 localhost
builder.WebHost.UseUrls(""http://localhost:5000""); // 只監聽本機，由 Nginx 轉發
```

### ❌ 錯誤二：把敏感資訊寫在 appsettings.json

```json
// 錯誤：把密碼和金鑰直接寫在設定檔中
{
  ""ConnectionStrings"": {
    ""Default"": ""Server=db;Password=MyP@ssw0rd123;""  // 密碼明文寫在設定檔！
  },
  ""ApiKeys"": {
    ""Stripe"": ""sk_live_abc123""                       // API 金鑰直接暴露！
  }
}
// 這些設定檔可能被提交到 Git，所有人都看得到

// ✅ 正確做法：使用環境變數或 User Secrets
// 開發環境用 dotnet user-secrets set ""ApiKeys:Stripe"" ""sk_live_abc123""
// 生產環境用環境變數 export ConnectionStrings__Default=""Server=db;...""
```

### ❌ 錯誤三：不理解環境設定的覆蓋順序

```csharp
// 錯誤：以為 appsettings.json 的值不會被覆蓋
// 實際的載入順序（後面的會覆蓋前面的）：
// 1. appsettings.json                    // 基礎設定（最先載入）
// 2. appsettings.{Environment}.json      // 環境設定（覆蓋基礎）
// 3. User Secrets（開發環境）             // 開發密鑰（覆蓋環境）
// 4. 環境變數                            // 系統變數（覆蓋密鑰）
// 5. 命令列參數                          // 最後載入（最高優先）

// ✅ 正確理解：後載入的設定會覆蓋先載入的
// 所以生產環境的環境變數會覆蓋 appsettings.json 中的同名設定
```
" },

        // ── Server Chapter 521 ────────────────────────────
        new() { Id=521, Category="server", Order=2, Level="intermediate", Icon="🔄", Title="反向代理與負載平衡", Slug="reverse-proxy-load-balancing", IsPublished=true, Content=@"
# 反向代理與負載平衡

## 正向代理 vs 反向代理

> 💡 **比喻：秘書代接電話**
> - **正向代理**（Forward Proxy）：你請秘書**幫你打電話**給別人
>   - 對方不知道是你打的，只知道是秘書打的
>   - 例如：VPN、公司內部代理伺服器
>
> - **反向代理**（Reverse Proxy）：公司請秘書**幫所有員工接電話**
>   - 外面的人打電話進來，都先經過秘書
>   - 秘書決定轉給哪位員工處理
>   - 例如：Nginx、HAProxy
>
> 關鍵差別：正向代理**代替客戶端**，反向代理**代替伺服器**。

### 圖解差異

```
// 正向代理（幫客戶端出去）
客戶端 A ─┐
客戶端 B ─┼─→ [正向代理] ─→ 網際網路 ─→ 伺服器   // 代理幫客戶端存取外部
客戶端 C ─┘                                        // 伺服器看不到真正的客戶端

// 反向代理（幫伺服器進來）
客戶端 ─→ 網際網路 ─→ [反向代理] ─┬→ 伺服器 A     // 代理幫伺服器接收請求
                                   ├→ 伺服器 B     // 客戶端看不到真正的伺服器
                                   └→ 伺服器 C     // 代理決定分配給哪台
```

---

## Nginx 設定反向代理到 Kestrel

### 安裝 Nginx

```bash
# Ubuntu/Debian 安裝 Nginx
sudo apt update                    # 更新套件清單
sudo apt install nginx -y          # 安裝 Nginx，-y 自動確認

# 啟動並設為開機自動啟動
sudo systemctl start nginx         # 立即啟動 Nginx
sudo systemctl enable nginx        # 設定開機自動啟動

# 檢查 Nginx 狀態
sudo systemctl status nginx        # 查看是否正常運行
# 應該看到 active (running)

# 測試設定檔語法是否正確
sudo nginx -t                      # 檢查設定檔有沒有語法錯誤
```

### 基本反向代理設定

```nginx
# /etc/nginx/sites-available/myapp
# 這個設定檔告訴 Nginx 如何轉發請求給 Kestrel

server {                                          # 定義一個虛擬主機
    listen 80;                                    # 監聽 HTTP 80 Port
    server_name myapp.com www.myapp.com;          # 回應這些網域名稱的請求

    location / {                                  # 所有路徑的請求
        proxy_pass http://localhost:5000;          # 轉發到 Kestrel（本機 5000 Port）
        proxy_http_version 1.1;                   # 使用 HTTP 1.1 協定
        proxy_set_header Upgrade $http_upgrade;   # 支援 WebSocket 升級
        proxy_set_header Connection keep-alive;   # 保持連線不斷開
        proxy_set_header Host $host;              # 傳遞原始的主機名稱
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;   # 傳遞客戶端真實 IP
        proxy_set_header X-Forwarded-Proto $scheme;                    # 傳遞原始協定（http/https）
        proxy_cache_bypass $http_upgrade;         # WebSocket 請求不走快取
    }

    location /static/ {                           # 靜態檔案路徑
        alias /var/www/myapp/wwwroot/;            # 直接由 Nginx 提供靜態檔案
        expires 30d;                              # 快取 30 天
        add_header Cache-Control ""public"";      # 設定為公開快取
    }
}
```

```bash
# 啟用站台設定
sudo ln -s /etc/nginx/sites-available/myapp /etc/nginx/sites-enabled/   # 建立符號連結啟用站台
sudo nginx -t                  # 測試設定檔語法
sudo systemctl reload nginx    # 重新載入設定（不中斷服務）
```

### ASP.NET Core 配合反向代理

```csharp
// Program.cs 中設定轉發標頭
var builder = WebApplication.CreateBuilder(args);   // 建立建構器

// 設定轉發標頭中介軟體（重要！否則拿不到真實 IP）
builder.Services.Configure<ForwardedHeadersOptions>(options =>  // 設定轉發標頭選項
{
    options.ForwardedHeaders =                       // 指定要處理的標頭
        ForwardedHeaders.XForwardedFor |             // 處理客戶端 IP
        ForwardedHeaders.XForwardedProto;            // 處理原始協定
});

var app = builder.Build();                           // 建構應用程式

app.UseForwardedHeaders();    // 啟用轉發標頭（必須放在其他中介軟體之前）
app.UseHttpsRedirection();    // HTTP 重導向到 HTTPS
app.UseStaticFiles();         // 提供靜態檔案
app.UseAuthorization();       // 授權中介軟體
app.MapControllers();         // 對應 Controller 路由
app.Run();                    // 啟動應用程式
```

---

## 負載平衡演算法

> 💡 **比喻：餐廳帶位**
> 想像你是餐廳經理，要決定把客人帶到哪一桌（伺服器）：
> - **Round Robin**：輪流帶位，1號桌 → 2號桌 → 3號桌 → 回到1號桌
> - **Least Connections**：看哪桌客人最少就帶到那桌
> - **IP Hash**：看客人的臉（IP），同一張臉永遠去同一桌

### Nginx 負載平衡設定

```nginx
# /etc/nginx/nginx.conf
# 負載平衡設定範例

# 方法一：Round Robin（預設）—— 輪流分配
upstream myapp_servers {                           # 定義後端伺服器群組
    server 192.168.1.10:5000;                      # 後端伺服器 1
    server 192.168.1.11:5000;                      # 後端伺服器 2
    server 192.168.1.12:5000;                      # 後端伺服器 3
    # 請求會依序分配：1 → 2 → 3 → 1 → 2 → 3...   # 公平輪流
}

# 方法二：Least Connections —— 最少連線優先
upstream myapp_least {                             # 另一個伺服器群組
    least_conn;                                    # 啟用最少連線演算法
    server 192.168.1.10:5000;                      # 連線少的優先接收新請求
    server 192.168.1.11:5000;                      # 適合請求處理時間不均的情況
    server 192.168.1.12:5000;                      # 動態平衡負載
}

# 方法三：IP Hash —— 同 IP 固定到同一台
upstream myapp_sticky {                            # 黏性 Session 群組
    ip_hash;                                       # 根據客戶端 IP 做 Hash
    server 192.168.1.10:5000;                      # 同一個 IP 永遠去同一台
    server 192.168.1.11:5000;                      # 適合需要 Session 的應用
    server 192.168.1.12:5000;                      # 缺點：負載可能不均勻
}

# 方法四：加權 Round Robin —— 效能好的多分一點
upstream myapp_weighted {                          # 加權群組
    server 192.168.1.10:5000 weight=3;             # 這台效能好，分配 3 份
    server 192.168.1.11:5000 weight=2;             # 這台普通，分配 2 份
    server 192.168.1.12:5000 weight=1;             # 這台較弱，分配 1 份
}

server {                                           # 虛擬主機設定
    listen 80;                                     # 監聽 80 Port
    server_name myapp.com;                         # 網域名稱

    location / {                                   # 所有請求
        proxy_pass http://myapp_servers;            # 轉發到伺服器群組
        proxy_set_header Host $host;               # 傳遞主機名稱
        proxy_set_header X-Real-IP $remote_addr;   # 傳遞真實 IP
    }
}
```

---

## SSL/TLS 終止（SSL Termination）

> 💡 **比喻：門口保安檢查**
> SSL 終止就像大樓門口的保安：
> - 訪客進來時要**出示證件**（加密連線）
> - 保安**驗證完**後，讓訪客進入大樓（解密）
> - 大樓內部就不需要再出示證件了（內部走明文）
> - 這樣每間辦公室（後端伺服器）就不用各自安排保安

```nginx
# Nginx 處理 SSL/TLS 終止
server {                                              # HTTPS 虛擬主機
    listen 443 ssl;                                   # 監聽 443 Port 並啟用 SSL
    server_name myapp.com;                            # 網域名稱

    ssl_certificate /etc/ssl/certs/myapp.crt;         # SSL 憑證檔案路徑
    ssl_certificate_key /etc/ssl/private/myapp.key;   # SSL 私鑰檔案路徑

    ssl_protocols TLSv1.2 TLSv1.3;                   # 只允許安全的 TLS 版本
    ssl_ciphers HIGH:!aNULL:!MD5;                     # 使用高強度加密套件
    ssl_prefer_server_ciphers on;                     # 優先使用伺服器的加密套件

    # Nginx 負責解密，轉發給 Kestrel 時用 HTTP（明文）
    location / {                                      # 所有請求
        proxy_pass http://localhost:5000;              # 內部用 HTTP 就好（已在同一台機器）
        proxy_set_header X-Forwarded-Proto https;     # 告訴應用程式原始是 HTTPS
    }
}

# HTTP 自動重導向到 HTTPS
server {                                              # HTTP 虛擬主機
    listen 80;                                        # 監聽 80 Port
    server_name myapp.com;                            # 網域名稱
    return 301 https://$host$request_uri;             # 永久重導向到 HTTPS
}
```

```bash
# 使用 Let's Encrypt 免費取得 SSL 憑證
sudo apt install certbot python3-certbot-nginx -y     # 安裝 Certbot 和 Nginx 外掛
sudo certbot --nginx -d myapp.com -d www.myapp.com    # 自動設定 SSL 憑證
# Certbot 會自動修改 Nginx 設定檔                       # 並設定自動續約
sudo certbot renew --dry-run                          # 測試自動續約是否正常
```

---

## Health Check

### Nginx 被動健康檢查

```nginx
# Nginx 開源版的被動健康檢查
upstream myapp_backend {                                    # 後端伺服器群組
    server 192.168.1.10:5000 max_fails=3 fail_timeout=30s; # 失敗 3 次後暫停 30 秒
    server 192.168.1.11:5000 max_fails=3 fail_timeout=30s; # 同樣的失敗門檻設定
    server 192.168.1.12:5000 backup;                       # 備援伺服器，平時不使用
}
# max_fails=3：連續失敗 3 次就標記為不健康                    // 容錯次數
# fail_timeout=30s：30 秒後再試試看是否恢復                   // 恢復等待時間
# backup：只有其他伺服器都掛了才啟用                          // 最後防線
```

### ASP.NET Core Health Check 端點

```csharp
// Program.cs 中設定 Health Check
var builder = WebApplication.CreateBuilder(args);   // 建構器

// 加入 Health Check 服務
builder.Services.AddHealthChecks()                  // 註冊健康檢查
    .AddSqlServer(                                  // 檢查 SQL Server 連線
        builder.Configuration.GetConnectionString(""DefaultConnection""),  // 連線字串
        name: ""database"",                         // 檢查項目名稱
        timeout: TimeSpan.FromSeconds(3))           // 逾時時間 3 秒
    .AddRedis(                                      // 檢查 Redis 連線
        ""localhost:6379"",                         // Redis 連線位址
        name: ""redis"",                            // 檢查項目名稱
        timeout: TimeSpan.FromSeconds(3))           // 逾時時間 3 秒
    .AddUrlGroup(                                   // 檢查外部 API
        new Uri(""https://api.example.com/health""), // 外部 API 的健康端點
        name: ""external-api"",                     // 檢查項目名稱
        timeout: TimeSpan.FromSeconds(5));          // 逾時時間 5 秒

var app = builder.Build();                          // 建構應用程式
app.MapHealthChecks(""/health"");                   // 對應到 /health 路徑
app.Run();                                          // 啟動
```

---

## nginx.conf 完整範例

```nginx
# /etc/nginx/nginx.conf 完整設定範例

user www-data;                                 # Nginx 執行的使用者身分
worker_processes auto;                         # 工作程序數量（auto 會自動偵測 CPU 核心數）
pid /run/nginx.pid;                            # PID 檔案位置
events {
    worker_connections 1024;                   # 每個工作程序的最大連線數
    multi_accept on;                           # 允許一次接受多個新連線
}

http {                                         # HTTP 區塊設定
    # 基本設定
    sendfile on;                               # 啟用高效檔案傳輸
    tcp_nopush on;                             # 最佳化封包傳送
    tcp_nodelay on;                            # 減少延遲
    keepalive_timeout 65;                      # Keep-Alive 逾時時間 65 秒
    types_hash_max_size 2048;                  # MIME 類型 Hash 表大小
    server_tokens off;                         # 隱藏 Nginx 版本號（安全考量）

    # MIME 類型
    include /etc/nginx/mime.types;             # 載入 MIME 類型對應表
    default_type application/octet-stream;     # 預設 MIME 類型

    # 日誌設定
    access_log /var/log/nginx/access.log;      # 存取日誌路徑
    error_log /var/log/nginx/error.log;        # 錯誤日誌路徑

    # Gzip 壓縮
    gzip on;                                   # 啟用 Gzip 壓縮
    gzip_vary on;                              # 加入 Vary 標頭
    gzip_proxied any;                          # 對所有代理請求壓縮
    gzip_comp_level 6;                         # 壓縮等級（1-9，6 是好的平衡點）
    gzip_types text/plain text/css application/json application/javascript;  # 壓縮的類型

    # 速率限制
    limit_req_zone $binary_remote_addr zone=api:10m rate=10r/s;  # API 限流：每秒 10 個請求

    # 後端伺服器群組
    upstream dotnet_app {                      # 定義後端應用群組
        least_conn;                            # 使用最少連線演算法
        server 127.0.0.1:5000;                 # 本機的 ASP.NET Core 應用
        server 127.0.0.1:5001 backup;          # 備援實例
    }

    server {                                   # 虛擬主機設定
        listen 443 ssl http2;                  # 監聽 HTTPS 並啟用 HTTP/2
        server_name myapp.com;                 # 網域名稱

        ssl_certificate /etc/ssl/certs/myapp.crt;         # SSL 憑證
        ssl_certificate_key /etc/ssl/private/myapp.key;   # SSL 私鑰
        ssl_protocols TLSv1.2 TLSv1.3;                    # 安全的 TLS 版本
        ssl_ciphers HIGH:!aNULL:!MD5;                     # 高強度加密

        # 靜態檔案
        location /static/ {                    # 靜態檔案路徑
            alias /var/www/myapp/wwwroot/;     # 對應到實際目錄
            expires 7d;                        # 快取 7 天
            access_log off;                    # 靜態檔案不記錄存取日誌
        }

        # API 路由（有速率限制）
        location /api/ {                       # API 路徑
            limit_req zone=api burst=20 nodelay;  # 套用速率限制，允許突發 20 個
            proxy_pass http://dotnet_app;       # 轉發到後端群組
            proxy_http_version 1.1;            # HTTP 1.1
            proxy_set_header Upgrade $http_upgrade;         # WebSocket 支援
            proxy_set_header Connection keep-alive;         # 保持連線
            proxy_set_header Host $host;                    # 主機名稱
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;  # 真實 IP
            proxy_set_header X-Forwarded-Proto $scheme;     # 原始協定
        }

        # 其他所有請求
        location / {                           # 預設路徑
            proxy_pass http://dotnet_app;       # 轉發到後端
            proxy_http_version 1.1;            # HTTP 1.1
            proxy_set_header Host $host;       # 主機名稱
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;  # 真實 IP
            proxy_set_header X-Forwarded-Proto $scheme;  # 原始協定
        }
    }

    # HTTP 重導向到 HTTPS
    server {                                   # HTTP 虛擬主機
        listen 80;                             # 監聽 80 Port
        server_name myapp.com;                 # 網域名稱
        return 301 https://$host$request_uri;  # 301 永久重導向
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤一：反向代理後拿不到客戶端真實 IP

```csharp
// 錯誤：以為 HttpContext.Connection.RemoteIpAddress 是客戶端的 IP
var ip = HttpContext.Connection.RemoteIpAddress;  // 拿到的是 127.0.0.1（Nginx 的 IP）！

// 原因：經過反向代理後，Kestrel 看到的是 Nginx 的 IP
// 真正的客戶端 IP 被放在 X-Forwarded-For 標頭中

// ✅ 正確做法：啟用 ForwardedHeaders 中介軟體
// 在 Program.cs 中加入：
app.UseForwardedHeaders();  // 必須放在 UseAuthorization 之前
// 這樣 RemoteIpAddress 就會自動讀取 X-Forwarded-For
```

### ❌ 錯誤二：負載平衡時 Session 遺失

```csharp
// 錯誤：使用 In-Memory Session，負載平衡時 Session 會遺失
builder.Services.AddDistributedMemoryCache();   // 記憶體快取（只存在單一伺服器）
builder.Services.AddSession();                  // 啟用 Session

// 問題：使用者第一次請求到 Server A 登入
// 第二次請求被分到 Server B，Session 不在 B 上面，被登出！

// ✅ 正確做法：使用分散式 Session（Redis）
builder.Services.AddStackExchangeRedisCache(options =>  // 改用 Redis
{
    options.Configuration = ""localhost:6379"";           // Redis 連線位址
});
builder.Services.AddSession();  // Session 資料存在 Redis，所有伺服器都能讀取
```

### ❌ 錯誤三：SSL 設定不安全

```nginx
# 錯誤：允許舊版不安全的 SSL/TLS 協定
ssl_protocols SSLv3 TLSv1 TLSv1.1 TLSv1.2 TLSv1.3;  # SSLv3 和 TLSv1 有已知漏洞！

# 問題：SSLv3 有 POODLE 攻擊漏洞
# TLSv1.0 和 TLSv1.1 已被主流瀏覽器棄用

# ✅ 正確做法：只允許 TLSv1.2 和 TLSv1.3
ssl_protocols TLSv1.2 TLSv1.3;                        # 只用安全的版本
ssl_ciphers ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256;  # 強加密套件
ssl_prefer_server_ciphers on;                          # 優先使用伺服器的加密設定
```
" },

        // ── Server Chapter 522 ────────────────────────────
        new() { Id=522, Category="server", Order=3, Level="intermediate", Icon="👥", Title="多人連線與 Session 管理", Slug="multi-connection-session", IsPublished=true, Content=@"
# 多人連線與 Session 管理

## 同步 vs 非同步處理

> 💡 **比喻：點餐方式**
> - **同步處理**：你在櫃台點餐，站在那裡等做好才離開。後面的人都在排隊等你。
> - **非同步處理**：你在櫃台點餐，拿到號碼牌就去坐下。櫃台可以繼續服務下一位。
>
> 非同步不是「更快」，而是「不浪費等待時間」。
> 做菜（I/O 操作）的時間沒有變短，但櫃台（執行緒）可以服務更多人。

### 同步 vs 非同步程式碼

```csharp
// ❌ 同步寫法：執行緒在等待資料庫回應時被卡住
public IActionResult GetUsers()                    // 同步方法，沒有 async
{
    var users = _db.Users.ToList();                // 等待資料庫回應（執行緒被佔用）
    return Ok(users);                              // 資料庫回應後才執行
}
// 問題：如果有 100 個請求同時來，需要 100 個執行緒
// 執行緒是有限的資源，用完就會出現 503 錯誤

// ✅ 非同步寫法：執行緒在等待時可以去處理其他請求
public async Task<IActionResult> GetUsers()        // 非同步方法，回傳 Task
{
    var users = await _db.Users.ToListAsync();     // await 等待時釋放執行緒
    return Ok(users);                              // 資料庫回應後繼續
}
// 好處：100 個請求可能只需要 10 幾個執行緒就能處理
// 因為大部分時間都在等 I/O，執行緒可以重複利用
```

### 效能差異圖解

```
// 同步模式：4 個執行緒處理 4 個請求
Thread 1: [===處理===][等待DB..........][===完成===]         // 執行緒被卡住
Thread 2: [===處理===][等待DB..........][===完成===]         // 無法服務新請求
Thread 3: [===處理===][等待DB..........][===完成===]         // 資源浪費
Thread 4: [===處理===][等待DB..........][===完成===]         // 全部都在等
Request 5: 排隊中...等不到執行緒...超時...503 錯誤           // 第 5 個請求失敗

// 非同步模式：2 個執行緒處理 4+ 個請求
Thread 1: [處理1][等1→處理3][等3→處理5][完成1][完成3][完成5]  // 一個執行緒輪流處理
Thread 2: [處理2][等2→處理4][完成2][完成4]                   // 充分利用等待時間
// 少量執行緒就能處理大量請求！
```

---

## Connection Pool 概念

> 💡 **比喻：共用腳踏車**
> - 沒有 Connection Pool：每次出門都**買一台新腳踏車**，用完丟掉（太浪費！）
> - 有 Connection Pool：社區有一批**公共腳踏車**，需要時借，用完歸還
>
> 建立資料庫連線很花時間（像買腳踏車），Connection Pool 讓你重複使用已建立的連線。

### 資料庫 Connection Pool

```csharp
// 連線字串中設定 Connection Pool 參數
var connectionString = new StringBuilder()               // 建立連線字串
    .Append(""Server=localhost;"")                       // 資料庫伺服器位址
    .Append(""Database=MyDb;"")                          // 資料庫名稱
    .Append(""Min Pool Size=5;"")                        // 最少保持 5 個連線
    .Append(""Max Pool Size=100;"")                      // 最多建立 100 個連線
    .Append(""Connection Timeout=30;"")                  // 等待連線的逾時時間（秒）
    .Append(""Connection Lifetime=300;"")                // 連線存活最長時間（秒）
    .ToString();                                         // 轉成字串

// 在 Program.cs 中註冊 DbContext
builder.Services.AddDbContext<AppDbContext>(options =>    // 註冊資料庫上下文
    options.UseSqlServer(connectionString)                // 使用 SQL Server
);
// EF Core 會自動管理 Connection Pool
// 取得連線 → 使用 → 歸還，不需要手動管理
```

### HttpClient Connection Pool

```csharp
// ❌ 錯誤做法：每次都 new HttpClient
public async Task<string> CallApi()                       // 呼叫外部 API
{
    using var client = new HttpClient();                   // 每次 new 會建立新連線
    return await client.GetStringAsync(""https://api.example.com"");  // 用完就丟
}
// 問題：頻繁建立/關閉連線會耗盡 Socket（Socket Exhaustion）

// ✅ 正確做法：使用 IHttpClientFactory
// 在 Program.cs 中註冊
builder.Services.AddHttpClient(""ExternalApi"", client =>  // 註冊具名 HttpClient
{
    client.BaseAddress = new Uri(""https://api.example.com"");  // 基底位址
    client.Timeout = TimeSpan.FromSeconds(30);                  // 逾時設定
    client.DefaultRequestHeaders.Add(""Accept"", ""application/json""); // 預設標頭
});

// 在 Service 中使用
public class MyService                                     // 服務類別
{
    private readonly IHttpClientFactory _factory;           // HttpClient 工廠

    public MyService(IHttpClientFactory factory)            // 透過 DI 注入
    {
        _factory = factory;                                 // 儲存工廠實例
    }

    public async Task<string> CallApi()                    // 呼叫 API
    {
        var client = _factory.CreateClient(""ExternalApi""); // 從工廠取得 HttpClient
        return await client.GetStringAsync(""/data"");       // 連線會被自動管理和重用
    }
}
```

---

## Session vs Token 狀態管理

> 💡 **比喻：身分識別方式**
> - **Session**：像是遊樂園的**手環**——入園時套在手上，工作人員看到手環就知道你買過票。但手環資訊存在遊樂園的系統裡。
> - **Token (JWT)**：像是**護照**——你自己帶著，上面寫著你的身分資訊。任何人都能讀取，但偽造不了（有防偽鋼印）。

### Session 方式

```csharp
// Program.cs 設定 Session
builder.Services.AddDistributedMemoryCache();              // 記憶體快取（開發用）
builder.Services.AddSession(options =>                     // 設定 Session 選項
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);        // 閒置 30 分鐘過期
    options.Cookie.HttpOnly = true;                        // Cookie 只能透過 HTTP 存取
    options.Cookie.IsEssential = true;                     // 標記為必要 Cookie
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // 只在 HTTPS 傳送
});

var app = builder.Build();                                 // 建構應用程式
app.UseSession();                                          // 啟用 Session 中介軟體

// 在 Controller 中使用 Session
public class CartController : Controller                   // 購物車控制器
{
    public IActionResult AddItem(int productId)            // 加入商品
    {
        var cart = HttpContext.Session.GetString(""Cart""); // 從 Session 取得購物車
        // 處理購物車邏輯...                                // 業務邏輯
        HttpContext.Session.SetString(""Cart"", updatedCart); // 更新 Session
        return Ok();                                       // 回傳成功
    }
}
```

### JWT Token 方式

```csharp
// Program.cs 設定 JWT 驗證
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  // 設定 JWT 驗證
    .AddJwtBearer(options =>                               // JWT Bearer 選項
    {
        options.TokenValidationParameters = new TokenValidationParameters   // 驗證參數
        {
            ValidateIssuer = true,                         // 驗證發行者
            ValidateAudience = true,                       // 驗證對象
            ValidateLifetime = true,                       // 驗證有效期限
            ValidateIssuerSigningKey = true,               // 驗證簽名金鑰
            ValidIssuer = ""myapp.com"",                   // 合法的發行者
            ValidAudience = ""myapp.com"",                 // 合法的對象
            IssuerSigningKey = new SymmetricSecurityKey(    // 簽名金鑰
                Encoding.UTF8.GetBytes(""YourSuperSecretKey123!""))  // 金鑰字串（生產環境要用環境變數）
        };
    });

// 產生 JWT Token
public string GenerateToken(User user)                     // 產生 Token 方法
{
    var claims = new[]                                     // 宣告（Token 中的資訊）
    {
        new Claim(ClaimTypes.Name, user.Username),         // 使用者名稱
        new Claim(ClaimTypes.Role, user.Role),             // 使用者角色
        new Claim(""UserId"", user.Id.ToString())          // 使用者 ID
    };

    var key = new SymmetricSecurityKey(                     // 建立簽名金鑰
        Encoding.UTF8.GetBytes(""YourSuperSecretKey123!"") // 金鑰字串
    );
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);  // 簽名憑證

    var token = new JwtSecurityToken(                       // 建立 JWT Token
        issuer: ""myapp.com"",                             // 發行者
        audience: ""myapp.com"",                           // 對象
        claims: claims,                                    // 宣告內容
        expires: DateTime.Now.AddHours(1),                 // 1 小時後過期
        signingCredentials: creds                          // 簽名憑證
    );

    return new JwtSecurityTokenHandler().WriteToken(token); // 序列化為字串
}
```

---

## WebSocket 長連線

> 💡 **比喻：打電話 vs 傳簡訊**
> - **HTTP**：像傳簡訊——每次都要重新撥號、說完就掛斷
> - **WebSocket**：像打電話——接通後保持連線，雙方可以隨時說話
>
> 適合需要即時更新的場景：聊天室、股票報價、線上遊戲、通知推播。

### SignalR（ASP.NET Core 的 WebSocket 框架）

```csharp
// ChatHub.cs - 定義 SignalR Hub
using Microsoft.AspNetCore.SignalR;                        // SignalR 命名空間

public class ChatHub : Hub                                 // 繼承 Hub 基底類別
{
    // 當客戶端連線時觸發
    public override async Task OnConnectedAsync()          // 連線事件
    {
        var username = Context.User?.Identity?.Name;       // 取得使用者名稱
        await Clients.All.SendAsync(                       // 通知所有人
            ""UserJoined"", $""{username} 加入了聊天室""   // 發送加入訊息
        );
        await base.OnConnectedAsync();                     // 呼叫基底方法
    }

    // 當客戶端發送訊息時觸發
    public async Task SendMessage(string message)          // 接收客戶端的訊息
    {
        var username = Context.User?.Identity?.Name;       // 取得發送者名稱
        await Clients.All.SendAsync(                       // 廣播給所有連線的客戶端
            ""ReceiveMessage"", username, message          // 傳送使用者名稱和訊息內容
        );
    }

    // 發送訊息給特定群組
    public async Task SendToGroup(string group, string msg)  // 群組訊息
    {
        await Clients.Group(group).SendAsync(              // 只發給特定群組
            ""ReceiveMessage"", Context.User?.Identity?.Name, msg  // 訊息內容
        );
    }

    // 加入群組
    public async Task JoinGroup(string groupName)          // 加入群組方法
    {
        await Groups.AddToGroupAsync(                      // 將連線加入群組
            Context.ConnectionId, groupName                // 使用連線 ID 和群組名稱
        );
        await Clients.Group(groupName).SendAsync(          // 通知群組成員
            ""UserJoined"", $""{Context.User?.Identity?.Name} 加入了 {groupName}""
        );
    }

    // 當客戶端斷線時觸發
    public override async Task OnDisconnectedAsync(Exception? ex)  // 斷線事件
    {
        await Clients.All.SendAsync(                       // 通知所有人
            ""UserLeft"", $""{Context.User?.Identity?.Name} 離開了""  // 離開訊息
        );
        await base.OnDisconnectedAsync(ex);                // 呼叫基底方法
    }
}

// Program.cs 中註冊 SignalR
var builder = WebApplication.CreateBuilder(args);          // 建構器
builder.Services.AddSignalR();                             // 註冊 SignalR 服務

var app = builder.Build();                                 // 建構應用程式
app.MapHub<ChatHub>(""/chatHub"");                         // 將 Hub 對應到 /chatHub 路徑
app.Run();                                                 // 啟動
```

---

## 限流（Rate Limiting）與防護

> 💡 **比喻：夜店的門口管制**
> - 限流就像夜店門口的保鏢：
>   - 「一次只能進 100 人」（並發限制）
>   - 「每個人每小時只能進出 3 次」（速率限制）
>   - 「VIP 不受限制」（白名單）
> - 目的是防止惡意攻擊者（DDoS）或是爬蟲把你的服務搞垮

### ASP.NET Core 內建限流

```csharp
// Program.cs 設定限流
using System.Threading.RateLimiting;                       // 限流命名空間

var builder = WebApplication.CreateBuilder(args);          // 建構器

builder.Services.AddRateLimiter(options =>                  // 新增限流服務
{
    // 全域固定視窗限流
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(  // 全域限流器
        context => RateLimitPartition.GetFixedWindowLimiter(                     // 固定視窗演算法
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? ""unknown"",  // 用 IP 分區
            factory: _ => new FixedWindowRateLimiterOptions                      // 限流選項
            {
                PermitLimit = 100,                         // 每個視窗允許 100 個請求
                Window = TimeSpan.FromMinutes(1),          // 視窗大小：1 分鐘
                QueueLimit = 10,                           // 排隊等待的請求數
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst  // 先進先出
            }
        )
    );

    // 針對特定端點的限流策略
    options.AddFixedWindowLimiter(""ApiPolicy"", opt =>     // 建立具名策略
    {
        opt.PermitLimit = 30;                              // 每個視窗允許 30 個請求
        opt.Window = TimeSpan.FromMinutes(1);              // 視窗：1 分鐘
        opt.QueueLimit = 5;                                // 排隊數量
    });

    // 被限流時的回應
    options.OnRejected = async (context, token) =>         // 被拒絕時的處理
    {
        context.HttpContext.Response.StatusCode = 429;      // 回傳 429 Too Many Requests
        await context.HttpContext.Response.WriteAsync(      // 寫入回應內容
            ""請求太頻繁，請稍後再試。"", cancellationToken: token  // 中文錯誤訊息
        );
    };
});

var app = builder.Build();                                 // 建構應用程式
app.UseRateLimiter();                                      // 啟用限流中介軟體

// 在 Controller 上套用特定策略
app.MapGet(""/api/data"", () => ""OK"")                    // API 端點
    .RequireRateLimiting(""ApiPolicy"");                   // 套用 ApiPolicy 限流策略
```

---

## SignalR Scale-Out（Redis Backplane）

> 💡 **比喻：連鎖餐廳的廣播系統**
> 如果你的聊天室跑在多台伺服器上：
> - 使用者 A 連到 Server 1，使用者 B 連到 Server 2
> - A 發訊息給 B，但 Server 1 不知道 B 在 Server 2 上
> - **Redis Backplane** 就像連鎖餐廳的**內部廣播系統**
> - Server 1 把訊息發到 Redis，Redis 廣播給所有 Server
> - Server 2 收到後轉發給使用者 B

### 設定 Redis Backplane

```csharp
// 安裝 NuGet 套件
// dotnet add package Microsoft.AspNetCore.SignalR.StackExchangeRedis  // 安裝 Redis 套件

// Program.cs 設定 SignalR + Redis
var builder = WebApplication.CreateBuilder(args);          // 建構器

builder.Services.AddSignalR()                              // 註冊 SignalR
    .AddStackExchangeRedis(                                // 加入 Redis Backplane
        ""localhost:6379"",                                // Redis 連線位址
        options =>                                         // Redis 選項
        {
            options.Configuration.ChannelPrefix =          // 頻道前綴
                RedisChannel.Literal(""MyApp"");           // 避免不同應用衝突
        }
    );

var app = builder.Build();                                 // 建構應用程式
app.MapHub<ChatHub>(""/chatHub"");                         // 對應 Hub 路徑
app.Run();                                                 // 啟動
```

```
// Redis Backplane 運作流程
使用者 A ──→ Server 1 ──┐
                         ├──→ Redis（訊息中繼站）──→ 廣播給所有 Server
使用者 B ──→ Server 2 ──┘                          │
                                                    ├──→ Server 1 ──→ 使用者 A（收到）
                                                    └──→ Server 2 ──→ 使用者 B（收到）
```

```bash
# 安裝 Redis（Ubuntu）
sudo apt update                                    # 更新套件清單
sudo apt install redis-server -y                   # 安裝 Redis

# 設定 Redis
sudo nano /etc/redis/redis.conf                    # 編輯設定檔
# 將 bind 127.0.0.1 改為 bind 0.0.0.0             # 如果需要外部存取
# 設定 requirepass YourRedisPassword               # 設定密碼保護

sudo systemctl restart redis                       # 重啟 Redis 套用設定
sudo systemctl enable redis                        # 設定開機自動啟動

# 測試 Redis 連線
redis-cli ping                                     # 應該回傳 PONG
redis-cli info clients                             # 查看連線數量
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤一：在非同步方法中使用同步呼叫

```csharp
// 錯誤：在 async 方法裡使用 .Result 或 .Wait()
public async Task<IActionResult> GetData()                 // 非同步方法
{
    var result = _httpClient.GetStringAsync(""https://api.example.com"").Result;  // 用 .Result 同步等待！
    return Ok(result);
}
// 問題：.Result 會阻塞執行緒，可能造成死結（Deadlock）
// 在 ASP.NET Core 中，這會導致應用程式凍結

// ✅ 正確做法：全程使用 await
public async Task<IActionResult> GetData()                 // 非同步方法
{
    var result = await _httpClient.GetStringAsync(""https://api.example.com"");  // 用 await 非同步等待
    return Ok(result);                                     // 不會阻塞執行緒
}
```

### ❌ 錯誤二：每次請求都 new HttpClient

```csharp
// 錯誤：在 Controller 中直接 new HttpClient
public class MyController : Controller
{
    public async Task<IActionResult> CallApi()
    {
        using var client = new HttpClient();               // 每次都建立新的 HttpClient！
        var result = await client.GetStringAsync(""https://api.example.com"");
        return Ok(result);
    }
}
// 問題：HttpClient 的 Dispose 不會立即釋放 Socket
// 大量請求會導致 Socket Exhaustion 錯誤

// ✅ 正確做法：使用 IHttpClientFactory
public class MyController : Controller
{
    private readonly IHttpClientFactory _factory;          // 注入工廠

    public MyController(IHttpClientFactory factory)        // 建構函式注入
    {
        _factory = factory;                                // 儲存參考
    }

    public async Task<IActionResult> CallApi()
    {
        var client = _factory.CreateClient();              // 從工廠取得（連線會被重用）
        var result = await client.GetStringAsync(""https://api.example.com"");
        return Ok(result);
    }
}
```

### ❌ 錯誤三：JWT 金鑰太短或硬編碼

```csharp
// 錯誤：使用太短的金鑰
var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(""123"")                        // 金鑰太短！至少要 256 位元（32 字元）
);

// 錯誤：金鑰直接寫在程式碼中
var secretKey = ""MySecretKeyHardcodedInSourceCode123!"";  // 程式碼可能被推到 GitHub！

// ✅ 正確做法：從環境變數或 User Secrets 讀取
var secretKey = builder.Configuration[""Jwt:SecretKey""];  // 從設定檔讀取
// 金鑰長度至少 32 字元以上
// 生產環境存在環境變數中，不要提交到版本控制
```
" },

        // ── Server Chapter 523 ────────────────────────────
        new() { Id=523, Category="server", Order=4, Level="advanced", Icon="📊", Title="伺服器監控與維運", Slug="server-monitoring-ops", IsPublished=true, Content=@"
# 伺服器監控與維運

## 日誌集中管理（Serilog + Seq/ELK）

> 💡 **比喻：醫院的病歷系統**
> - 沒有集中日誌：每個醫生手寫病歷，放在自己抽屜裡。要查病史？去每個診間翻！
> - 有集中日誌：所有病歷電子化，存在中央系統。任何醫生都能查詢任何病人的完整病史。
>
> 伺服器的日誌就像病歷——你需要一個中央系統來統一收集、查詢、分析。

### 安裝 Serilog

```bash
# 安裝 Serilog 相關 NuGet 套件
dotnet add package Serilog.AspNetCore                      # Serilog ASP.NET Core 整合
dotnet add package Serilog.Sinks.Console                   # 輸出到終端機
dotnet add package Serilog.Sinks.File                      # 輸出到檔案
dotnet add package Serilog.Sinks.Seq                       # 輸出到 Seq 日誌平台
dotnet add package Serilog.Enrichers.Environment           # 加入環境資訊
dotnet add package Serilog.Enrichers.Thread                # 加入執行緒資訊
```

### 設定 Serilog

```csharp
// Program.cs 設定 Serilog
using Serilog;                                             // 引用 Serilog 命名空間

// 設定 Serilog Logger
Log.Logger = new LoggerConfiguration()                     // 建立 Logger 設定
    .MinimumLevel.Information()                            // 最低記錄等級為 Information
    .MinimumLevel.Override(""Microsoft"", Serilog.Events.LogEventLevel.Warning)  // Microsoft 的只記錄 Warning
    .MinimumLevel.Override(""System"", Serilog.Events.LogEventLevel.Warning)     // System 的只記錄 Warning
    .Enrich.FromLogContext()                               // 加入 Log 上下文資訊
    .Enrich.WithEnvironmentName()                          // 加入環境名稱（Development/Production）
    .Enrich.WithMachineName()                              // 加入機器名稱
    .Enrich.WithThreadId()                                 // 加入執行緒 ID
    .WriteTo.Console(                                      // 輸出到終端機
        outputTemplate: ""[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}""  // 格式範本
    )
    .WriteTo.File(                                         // 輸出到檔案
        path: ""logs/app-.log"",                           // 檔案路徑（自動加日期）
        rollingInterval: RollingInterval.Day,              // 每天產生新檔案
        retainedFileCountLimit: 30,                        // 保留最近 30 天的日誌
        fileSizeLimitBytes: 10_000_000,                    // 每個檔案最大 10MB
        rollOnFileSizeLimit: true                          // 超過大小就新建檔案
    )
    .WriteTo.Seq(""http://localhost:5341"")                // 輸出到 Seq 日誌平台
    .CreateLogger();                                       // 建立 Logger

try                                                        // 包在 try-catch 中保護啟動過程
{
    Log.Information(""應用程式啟動中..."");                  // 記錄啟動訊息

    var builder = WebApplication.CreateBuilder(args);       // 建構器
    builder.Host.UseSerilog();                              // 用 Serilog 取代內建日誌

    var app = builder.Build();                              // 建構應用程式

    app.UseSerilogRequestLogging(options =>                 // 記錄每個 HTTP 請求
    {
        options.MessageTemplate =                          // 自訂訊息範本
            ""{RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms"";
    });

    app.Run();                                             // 啟動應用程式
}
catch (Exception ex)                                       // 捕捉啟動時的錯誤
{
    Log.Fatal(ex, ""應用程式啟動失敗"");                    // 記錄致命錯誤
}
finally                                                    // 無論成功失敗都執行
{
    Log.CloseAndFlush();                                   // 確保所有日誌都寫入完畢
}
```

### 結構化日誌的威力

```csharp
// 在 Controller 或 Service 中使用結構化日誌
public class OrderService                                  // 訂單服務
{
    private readonly ILogger<OrderService> _logger;        // 注入 Logger

    public OrderService(ILogger<OrderService> logger)      // 建構函式
    {
        _logger = logger;                                  // 儲存 Logger
    }

    public async Task<Order> CreateOrder(int userId, decimal amount)  // 建立訂單
    {
        // 結構化日誌：用 {@} 記錄物件，用 {} 記錄純值
        _logger.LogInformation(                            // 記錄訂單建立資訊
            ""建立訂單：使用者 {UserId}，金額 {Amount:C}，時間 {OrderTime}"",  // 訊息範本
            userId, amount, DateTime.UtcNow                // 參數值（會被結構化儲存）
        );

        try                                                // 嘗試建立訂單
        {
            var order = new Order { UserId = userId, Amount = amount };  // 建立訂單物件
            // 儲存訂單...                                  // 資料庫操作
            _logger.LogInformation(""訂單 {OrderId} 建立成功"", order.Id);  // 記錄成功
            return order;                                  // 回傳訂單
        }
        catch (Exception ex)                               // 捕捉錯誤
        {
            _logger.LogError(ex,                           // 記錄錯誤（包含例外物件）
                ""訂單建立失敗：使用者 {UserId}，金額 {Amount}"",  // 錯誤訊息
                userId, amount                             // 參數值
            );
            throw;                                         // 重新拋出例外
        }
    }
}
```

### 安裝 Seq 日誌平台

```bash
# 使用 Docker 安裝 Seq
docker run -d \                                            # 背景執行容器
    --name seq \                                           # 容器名稱
    -e ACCEPT_EULA=Y \                                     # 接受使用者授權合約
    -p 5341:80 \                                           # 對應 Port（本機 5341 到容器 80）
    datalust/seq:latest                                    # 使用最新版 Seq 映像

# 開啟瀏覽器到 http://localhost:5341 就可以查看日誌儀表板
# Seq 提供強大的查詢語法：
# UserId = 123                                             # 查詢特定使用者
# @Level = 'Error'                                        # 查詢所有錯誤
# Amount > 1000                                           # 查詢大金額訂單
# RequestPath like '/api/%'                               # 查詢 API 路徑
```

---

## Health Check Endpoint (/health)

> 💡 **比喻：定期健康檢查**
> Health Check 就像員工每年的健康檢查：
> - 量血壓（檢查資料庫連線）
> - 驗血（檢查記憶體使用量）
> - 心電圖（檢查外部 API 回應時間）
> 如果任何一項不正常，就要發出警報。

### 完整 Health Check 設定

```csharp
// 自訂 Health Check 類別
using Microsoft.Extensions.Diagnostics.HealthChecks;       // 健康檢查命名空間

public class DiskSpaceHealthCheck : IHealthCheck           // 實作健康檢查介面
{
    public Task<HealthCheckResult> CheckHealthAsync(       // 檢查方法
        HealthCheckContext context,                        // 檢查上下文
        CancellationToken cancellationToken = default)     // 取消令牌
    {
        var drive = new DriveInfo(""C"");                   // 取得 C 槽資訊
        var freeSpacePercent = (double)drive.AvailableFreeSpace / drive.TotalSize * 100;  // 計算剩餘空間百分比

        if (freeSpacePercent < 5)                          // 剩餘空間小於 5%
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(  // 不健康
                $""磁碟空間嚴重不足：剩餘 {freeSpacePercent:F1}%""  // 錯誤訊息
            ));
        }
        if (freeSpacePercent < 15)                         // 剩餘空間小於 15%
        {
            return Task.FromResult(HealthCheckResult.Degraded(   // 效能降低
                $""磁碟空間偏低：剩餘 {freeSpacePercent:F1}%""     // 警告訊息
            ));
        }

        return Task.FromResult(HealthCheckResult.Healthy(  // 健康
            $""磁碟空間正常：剩餘 {freeSpacePercent:F1}%""  // 正常訊息
        ));
    }
}

// Program.cs 註冊 Health Check
var builder = WebApplication.CreateBuilder(args);          // 建構器

builder.Services.AddHealthChecks()                         // 註冊健康檢查
    .AddSqlServer(                                         // SQL Server 檢查
        builder.Configuration.GetConnectionString(""DefaultConnection""),  // 連線字串
        name: ""sqlserver"",                               // 檢查名稱
        failureStatus: HealthStatus.Unhealthy,             // 失敗時的狀態
        tags: new[] { ""db"", ""critical"" })              // 標籤分類
    .AddRedis(                                             // Redis 檢查
        ""localhost:6379"",                                // 連線位址
        name: ""redis"",                                   // 檢查名稱
        tags: new[] { ""cache"" })                         // 標籤
    .AddCheck<DiskSpaceHealthCheck>(                        // 自訂磁碟檢查
        ""disk-space"",                                    // 檢查名稱
        tags: new[] { ""infrastructure"" });                // 標籤

var app = builder.Build();                                 // 建構

// 基本健康端點
app.MapHealthChecks(""/health"");                          // 對應到 /health

// 詳細健康端點（含每個檢查項目的結果）
app.MapHealthChecks(""/health/detail"", new HealthCheckOptions  // 詳細端點
{
    ResponseWriter = async (context, report) =>            // 自訂回應格式
    {
        context.Response.ContentType = ""application/json"";  // JSON 格式
        var result = new                                   // 建立回應物件
        {
            status = report.Status.ToString(),             // 整體狀態
            checks = report.Entries.Select(e => new        // 各項目狀態
            {
                name = e.Key,                              // 項目名稱
                status = e.Value.Status.ToString(),        // 項目狀態
                description = e.Value.Description,         // 描述
                duration = e.Value.Duration.TotalMilliseconds  // 檢查花費時間
            })
        };
        await context.Response.WriteAsJsonAsync(result);   // 寫入 JSON 回應
    }
});

app.Run();                                                 // 啟動
```

---

## APM（Application Performance Monitoring）

> 💡 **比喻：汽車的儀表板**
> APM 就像汽車的儀表板：
> - 時速表 → 回應時間
> - 轉速表 → CPU 使用率
> - 油量表 → 記憶體使用量
> - 引擎警示燈 → 錯誤率
>
> 沒有 APM，就像開車沒有儀表板——出問題時完全不知道原因。

### 使用 OpenTelemetry

```csharp
// 安裝 NuGet 套件
// dotnet add package OpenTelemetry.Extensions.Hosting           // 主機整合
// dotnet add package OpenTelemetry.Instrumentation.AspNetCore   // ASP.NET Core 監控
// dotnet add package OpenTelemetry.Instrumentation.Http         // HTTP 請求監控
// dotnet add package OpenTelemetry.Instrumentation.SqlClient    // SQL 監控
// dotnet add package OpenTelemetry.Exporter.Prometheus.AspNetCore  // Prometheus 匯出

// Program.cs 設定 OpenTelemetry
using OpenTelemetry.Metrics;                               // 指標命名空間
using OpenTelemetry.Trace;                                 // 追蹤命名空間

var builder = WebApplication.CreateBuilder(args);          // 建構器

// 設定追蹤（Tracing）
builder.Services.AddOpenTelemetry()                        // 加入 OpenTelemetry
    .WithTracing(tracing =>                                // 設定追蹤
    {
        tracing
            .AddAspNetCoreInstrumentation()                // 監控 ASP.NET Core 請求
            .AddHttpClientInstrumentation()                // 監控 HttpClient 呼叫
            .AddSqlClientInstrumentation(options =>         // 監控 SQL 查詢
            {
                options.SetDbStatementForText = true;      // 記錄 SQL 語句文字
            })
            .AddConsoleExporter();                         // 輸出到終端機（開發用）
    })
    .WithMetrics(metrics =>                                // 設定指標
    {
        metrics
            .AddAspNetCoreInstrumentation()                // ASP.NET Core 指標
            .AddHttpClientInstrumentation()                // HTTP 客戶端指標
            .AddRuntimeInstrumentation()                   // .NET Runtime 指標
            .AddProcessInstrumentation()                   // 程序指標
            .AddPrometheusExporter();                      // 匯出到 Prometheus
    });

var app = builder.Build();                                 // 建構

app.MapPrometheusScrapingEndpoint(""/metrics"");           // Prometheus 抓取端點

app.Run();                                                 // 啟動
```

### 自訂指標

```csharp
// 建立自訂指標來追蹤業務數據
using System.Diagnostics.Metrics;                          // 指標命名空間

public class OrderMetrics                                  // 訂單指標類別
{
    private readonly Counter<long> _ordersCreated;         // 訂單建立計數器
    private readonly Histogram<double> _orderAmount;       // 訂單金額直方圖
    private readonly UpDownCounter<int> _activeOrders;     // 活躍訂單數量

    public OrderMetrics(IMeterFactory meterFactory)        // 透過 DI 注入
    {
        var meter = meterFactory.Create(""MyApp.Orders""); // 建立指標計量器

        _ordersCreated = meter.CreateCounter<long>(         // 計數器：只增不減
            ""orders.created"",                            // 指標名稱
            unit: ""orders"",                              // 單位
            description: ""建立的訂單總數""                 // 描述
        );

        _orderAmount = meter.CreateHistogram<double>(      // 直方圖：記錄數值分布
            ""orders.amount"",                             // 指標名稱
            unit: ""TWD"",                                 // 單位（新台幣）
            description: ""訂單金額分布""                   // 描述
        );

        _activeOrders = meter.CreateUpDownCounter<int>(    // 上下計數器：可增可減
            ""orders.active"",                             // 指標名稱
            description: ""目前活躍的訂單數""               // 描述
        );
    }

    public void OrderCreated(decimal amount, string region)  // 記錄訂單建立
    {
        _ordersCreated.Add(1, new(""region"", region));     // 計數加 1，附帶地區標籤
        _orderAmount.Record((double)amount);                // 記錄金額
        _activeOrders.Add(1);                               // 活躍訂單加 1
    }

    public void OrderCompleted()                           // 記錄訂單完成
    {
        _activeOrders.Add(-1);                             // 活躍訂單減 1
    }
}
```

---

## 記憶體洩漏排查

> 💡 **比喻：水龍頭沒關好**
> 記憶體洩漏就像水龍頭沒關好：
> - 水一滴一滴地流（記憶體一點一點地增加）
> - 短時間看不出問題（應用剛啟動很正常）
> - 時間一長水桶就滿了（記憶體用完就崩潰 OOM）
> 排查就是找到哪個水龍頭沒關好。

### 使用 dotnet 診斷工具

```bash
# 安裝 .NET 診斷工具
dotnet tool install -g dotnet-counters                     # 即時效能計數器
dotnet tool install -g dotnet-dump                         # 記憶體傾印分析
dotnet tool install -g dotnet-trace                        # 效能追蹤
dotnet tool install -g dotnet-gcdump                       # GC 堆積傾印

# 使用 dotnet-counters 即時監控
dotnet-counters monitor -p <PID>                           # 監控指定程序的計數器
# 會顯示：
# CPU 使用率、記憶體使用量、GC 次數、執行緒數量
# Exception 數量、HTTP 請求速率等

# 監控特定計數器
dotnet-counters monitor -p <PID> \                         # 指定程序 ID
    --counters System.Runtime,Microsoft.AspNetCore.Hosting  # 指定要監控的計數器類別

# 收集記憶體傾印
dotnet-dump collect -p <PID>                               # 收集記憶體快照
# 會產生一個 .dmp 檔案

# 分析記憶體傾印
dotnet-dump analyze <dump-file>                            # 開啟分析互動介面
# 常用分析命令：
# > dumpheap -stat                                        # 查看堆積統計（哪個類型佔最多記憶體）
# > dumpheap -type System.String                          # 查看所有字串物件
# > gcroot <address>                                      # 查看物件被誰參考（為什麼無法回收）

# 使用 dotnet-trace 追蹤效能
dotnet-trace collect -p <PID> \                            # 收集效能追蹤資料
    --duration 00:00:30                                    # 追蹤 30 秒
# 產生的 .nettrace 檔案可以用 Visual Studio 或 PerfView 開啟
```

### 常見記憶體洩漏原因

```csharp
// ❌ 洩漏原因一：事件處理器沒有取消訂閱
public class LeakyService                                  // 有洩漏的服務
{
    public LeakyService(EventBus bus)                       // 建構函式
    {
        bus.OnOrderCreated += HandleOrder;                  // 訂閱事件，但從沒取消！
    }
    // 即使 LeakyService 不再使用，EventBus 仍然持有它的參考
    // 垃圾回收器無法回收它

    private void HandleOrder(Order order) { }              // 事件處理方法
}

// ✅ 正確做法：實作 IDisposable 來取消訂閱
public class FixedService : IDisposable                    // 實作 IDisposable
{
    private readonly EventBus _bus;                         // 儲存事件匯流排參考
    public FixedService(EventBus bus)                       // 建構函式
    {
        _bus = bus;                                         // 儲存參考
        _bus.OnOrderCreated += HandleOrder;                 // 訂閱事件
    }

    public void Dispose()                                  // 釋放資源
    {
        _bus.OnOrderCreated -= HandleOrder;                 // 取消訂閱事件！
    }

    private void HandleOrder(Order order) { }              // 事件處理方法
}

// ❌ 洩漏原因二：靜態集合不斷增長
public static class Cache                                  // 靜態快取
{
    private static readonly Dictionary<string, object>     // 永遠不會被清除的字典
        _items = new();                                    // 只增不減

    public static void Add(string key, object value)       // 只有新增
    {
        _items[key] = value;                               // 加進去就永遠在那裡
    }
    // 沒有清除機制，記憶體會不斷增長
}

// ✅ 正確做法：使用 MemoryCache（有過期機制）
builder.Services.AddMemoryCache();                         // 註冊記憶體快取服務

public class MyService                                     // 服務類別
{
    private readonly IMemoryCache _cache;                  // 注入快取
    public MyService(IMemoryCache cache) => _cache = cache;  // 建構函式

    public void CacheData(string key, object value)        // 快取資料
    {
        _cache.Set(key, value, TimeSpan.FromMinutes(30));  // 設定 30 分鐘過期
    }
}
```

---

## 自動重啟與 Process Manager

> 💡 **比喻：值班護士**
> Process Manager 就像醫院的值班護士：
> - 定時巡房（監控程序狀態）
> - 病人有異狀就按鈴（程序崩潰就重啟）
> - 換班時交接（應用程式更新時平滑切換）
> - 記錄巡房日誌（記錄重啟紀錄）

### 使用 systemd（Linux）

```bash
# 建立 systemd 服務設定檔
sudo nano /etc/systemd/system/myapp.service                # 編輯服務設定檔
```

```ini
# /etc/systemd/system/myapp.service
[Unit]
Description=My ASP.NET Core App                            # 服務描述
After=network.target                                       # 在網路啟動後才啟動

[Service]
WorkingDirectory=/var/www/myapp                             # 應用程式工作目錄
ExecStart=/usr/bin/dotnet /var/www/myapp/MyApp.dll          # 啟動命令
Restart=always                                             # 永遠自動重啟
RestartSec=10                                              # 重啟前等待 10 秒
SyslogIdentifier=myapp                                     # 系統日誌識別名稱
User=www-data                                              # 以 www-data 使用者身分執行
Group=www-data                                             # 使用者群組
Environment=ASPNETCORE_ENVIRONMENT=Production               # 環境設定
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false            # 關閉遙測訊息
LimitNOFILE=65536                                          # 最大開啟檔案數
TimeoutStopSec=30                                          # 停止超時時間

[Install]
WantedBy=multi-user.target                                 # 多使用者模式啟動
```

```bash
# 管理 systemd 服務
sudo systemctl daemon-reload                               # 重新載入 systemd 設定
sudo systemctl enable myapp                                # 設定開機自動啟動
sudo systemctl start myapp                                 # 啟動服務
sudo systemctl status myapp                                # 查看服務狀態
sudo systemctl stop myapp                                  # 停止服務
sudo systemctl restart myapp                               # 重啟服務

# 查看服務日誌
sudo journalctl -u myapp -f                                # 即時查看日誌（-f 持續追蹤）
sudo journalctl -u myapp --since ""1 hour ago""            # 查看最近一小時的日誌
sudo journalctl -u myapp --since today -p err              # 查看今天的錯誤日誌
```

### 部署腳本

```bash
#!/bin/bash
# deploy.sh - 自動部署腳本

APP_NAME=""myapp""                                          # 應用程式名稱
DEPLOY_DIR=""/var/www/$APP_NAME""                           # 部署目錄
PUBLISH_DIR=""./publish""                                   # 發佈輸出目錄
BACKUP_DIR=""/var/www/backups/$APP_NAME""                   # 備份目錄

echo ""開始部署 $APP_NAME...""                              # 顯示部署開始

# 步驟 1：建置發佈
echo ""步驟 1：建置發佈版本""                                # 提示訊息
dotnet publish -c Release -o $PUBLISH_DIR                   # 以 Release 模式發佈

# 步驟 2：備份目前版本
echo ""步驟 2：備份目前版本""                                # 提示訊息
TIMESTAMP=$(date +%Y%m%d_%H%M%S)                           # 產生時間戳記
mkdir -p $BACKUP_DIR                                        # 建立備份目錄
cp -r $DEPLOY_DIR $BACKUP_DIR/$TIMESTAMP                   # 複製目前版本到備份

# 步驟 3：停止服務
echo ""步驟 3：停止服務""                                    # 提示訊息
sudo systemctl stop $APP_NAME                               # 停止服務

# 步驟 4：部署新版本
echo ""步驟 4：部署新版本""                                  # 提示訊息
rm -rf $DEPLOY_DIR/*                                        # 清除舊檔案
cp -r $PUBLISH_DIR/* $DEPLOY_DIR/                          # 複製新版本

# 步驟 5：重啟服務
echo ""步驟 5：重啟服務""                                    # 提示訊息
sudo systemctl start $APP_NAME                              # 啟動服務

# 步驟 6：檢查健康狀態
echo ""步驟 6：檢查健康狀態""                                # 提示訊息
sleep 5                                                     # 等待 5 秒讓應用程式啟動
HEALTH=$(curl -s -o /dev/null -w ""%{http_code}"" http://localhost:5000/health)  # 呼叫健康端點
if [ ""$HEALTH"" == ""200"" ]; then                         # 如果回傳 200
    echo ""部署成功！健康檢查通過。""                        # 成功訊息
else                                                        # 否則
    echo ""部署可能有問題，健康檢查回傳: $HEALTH""          # 警告訊息
    echo ""正在回滾到前一版本...""                           # 回滾提示
    sudo systemctl stop $APP_NAME                           # 停止服務
    rm -rf $DEPLOY_DIR/*                                    # 清除失敗版本
    cp -r $BACKUP_DIR/$TIMESTAMP/* $DEPLOY_DIR/            # 還原備份
    sudo systemctl start $APP_NAME                          # 重啟
    echo ""回滾完成。""                                     # 回滾完成
fi
```

---

## 備份策略

> 💡 **比喻：保險箱策略**
> 備份就像存放重要文件：
> - **完整備份**：把所有文件影印一份放進保險箱（慢但完整）
> - **差異備份**：只影印跟上次完整備份不同的文件（中等速度）
> - **增量備份**：只影印今天新增或修改的文件（快但還原麻煩）
>
> 3-2-1 法則：至少 **3** 份備份，存在 **2** 種不同媒體，**1** 份放在異地。

### 資料庫備份腳本

```bash
#!/bin/bash
# db-backup.sh - 資料庫備份腳本

DB_HOST=""localhost""                                       # 資料庫主機
DB_NAME=""MyAppDb""                                        # 資料庫名稱
BACKUP_DIR=""/var/backups/database""                        # 備份目錄
RETENTION_DAYS=30                                           # 保留天數
DATE=$(date +%Y%m%d_%H%M%S)                                # 時間戳記

echo ""開始備份資料庫 $DB_NAME...""                         # 顯示開始

# 建立備份目錄
mkdir -p $BACKUP_DIR                                        # 確保目錄存在

# PostgreSQL 備份
pg_dump -h $DB_HOST \                                       # 指定主機
    -U postgres \                                           # 使用者名稱
    -d $DB_NAME \                                           # 資料庫名稱
    -F c \                                                  # 自訂格式（可壓縮）
    -f $BACKUP_DIR/${DB_NAME}_${DATE}.backup                # 輸出檔案路徑

# 壓縮備份檔
gzip $BACKUP_DIR/${DB_NAME}_${DATE}.backup                  # 壓縮節省空間

# 計算備份檔大小
SIZE=$(du -sh $BACKUP_DIR/${DB_NAME}_${DATE}.backup.gz | cut -f1)  # 取得檔案大小
echo ""備份完成！檔案大小: $SIZE""                           # 顯示大小

# 清除過期備份
find $BACKUP_DIR -name ""*.backup.gz"" -mtime +$RETENTION_DAYS -delete  # 刪除超過保留天數的備份
echo ""已清除 $RETENTION_DAYS 天前的備份。""                 # 顯示清除訊息

# 驗證備份完整性
pg_restore --list $BACKUP_DIR/${DB_NAME}_${DATE}.backup.gz > /dev/null 2>&1  # 測試備份是否可還原
if [ $? -eq 0 ]; then                                      # 如果成功
    echo ""備份驗證通過。""                                  # 驗證成功
else                                                        # 如果失敗
    echo ""警告：備份驗證失敗！""                             # 驗證失敗警告
fi
```

### 排程自動備份

```bash
# 使用 crontab 設定定時備份
crontab -e                                                  # 編輯排程任務

# 加入以下排程
# 每天凌晨 2 點執行資料庫備份
0 2 * * * /opt/scripts/db-backup.sh >> /var/log/backup.log 2>&1  # 每日備份，日誌輸出到檔案

# 每週日凌晨 3 點執行完整檔案備份
0 3 * * 0 /opt/scripts/full-backup.sh >> /var/log/backup.log 2>&1  # 每週完整備份

# 每 6 小時執行增量備份
0 */6 * * * /opt/scripts/incremental-backup.sh >> /var/log/backup.log 2>&1  # 每 6 小時增量備份
```

### 應用程式檔案備份

```bash
#!/bin/bash
# file-backup.sh - 應用程式檔案備份腳本

APP_DIR=""/var/www/myapp""                                  # 應用程式目錄
UPLOAD_DIR=""/var/www/myapp/uploads""                       # 使用者上傳目錄
BACKUP_DIR=""/var/backups/files""                           # 備份目錄
DATE=$(date +%Y%m%d)                                        # 日期

mkdir -p $BACKUP_DIR                                        # 建立備份目錄

# 備份應用程式設定檔
tar -czf $BACKUP_DIR/config_${DATE}.tar.gz \               # 壓縮打包
    $APP_DIR/appsettings.Production.json \                  # 生產環境設定
    /etc/nginx/sites-available/ \                           # Nginx 站台設定
    /etc/systemd/system/myapp.service                       # systemd 服務設定

# 備份使用者上傳的檔案
tar -czf $BACKUP_DIR/uploads_${DATE}.tar.gz \              # 壓縮使用者上傳檔案
    $UPLOAD_DIR                                             # 上傳目錄

echo ""檔案備份完成：$(date)""                               # 顯示完成時間

# 同步到遠端備份（異地備份）
rsync -avz $BACKUP_DIR/ \                                   # 增量同步到遠端
    backup-user@remote-server:/backups/myapp/               # 遠端備份伺服器

echo ""異地備份同步完成""                                    # 同步完成訊息
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤一：只用 Console.WriteLine 當日誌

```csharp
// 錯誤：用 Console.WriteLine 記錄日誌
Console.WriteLine($""Order created: {orderId}"");          // 沒有時間戳記
Console.WriteLine($""Error: {ex.Message}"");               // 沒有日誌等級
// 問題：
// 1. 沒有時間戳記，不知道什麼時候發生的
// 2. 沒有日誌等級，無法過濾重要的錯誤
// 3. 應用程式重啟後日誌就消失了
// 4. 無法結構化查詢

// ✅ 正確做法：使用 ILogger
_logger.LogInformation(""訂單 {OrderId} 建立成功"", orderId);  // 結構化日誌
_logger.LogError(ex, ""訂單處理失敗 {OrderId}"", orderId);     // 包含例外和上下文
// 自動包含時間、等級、分類，可以輸出到多個目的地
```

### ❌ 錯誤二：Health Check 沒有設定超時

```csharp
// 錯誤：Health Check 沒有設定超時時間
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: ""database"");   // 沒有設 timeout！

// 問題：如果資料庫回應很慢，Health Check 可能要等很久
// 負載平衡器可能以為這台伺服器掛了

// ✅ 正確做法：設定合理的超時時間
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString,
        name: ""database"",
        timeout: TimeSpan.FromSeconds(3)                   // 3 秒沒回應就算失敗
    );
```

### ❌ 錯誤三：沒有備份驗證就安心

```bash
# 錯誤：備份了但從沒測試過還原
pg_dump -d MyDb -f backup.sql                              # 備份做了
echo ""備份完成，可以安心了""                                # 就這樣？

# 問題：
# 1. 備份檔案可能損壞
# 2. 備份可能漏了重要的資料表
# 3. 還原程序可能有問題
# 4. 你從來沒練習過還原步驟

# ✅ 正確做法：定期測試還原
# 每月至少做一次還原演練
pg_restore -d TestDb backup.sql                            # 還原到測試資料庫
psql -d TestDb -c ""SELECT COUNT(*) FROM orders""          # 驗證資料筆數
# 比較還原的資料量是否與預期一致
echo ""還原測試完成，驗證資料正確性""
```
" },
    };
}