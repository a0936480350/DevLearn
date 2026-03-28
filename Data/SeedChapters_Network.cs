using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Network
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── 網路 Chapter 31 ─────────────────────────────────────
        new() { Id=31, Category="network", Order=2, Level="beginner", Icon="📬", Title="HTTP 協議深入理解", Slug="network-http", IsPublished=true, Content=@"
# HTTP 協議深入理解

## 什麼是 HTTP？

HTTP（HyperText Transfer Protocol）是瀏覽器和伺服器之間溝通的語言。

> 💡 **比喻：寄信**
> 想像你去郵局寄信：
> - **Request（請求）** = 你寫的信（告訴對方你要什麼）
> - **Response（回應）** = 對方的回信（把結果寄回來）
> - **HTTP Method** = 信封上的「目的」（查詢？修改？刪除？）
> - **Status Code** = 回信的「處理結果」（成功？找不到？伺服器爆炸？）

---

## HTTP 方法（Methods）

```
方法       用途                 比喻
───────────────────────────────────────
GET        取得資料             去圖書館借書
POST       新增資料             交一份新的申請表
PUT        完整更新資料         整份文件重寫
PATCH      部分更新資料         只改文件中的一段
DELETE     刪除資料             撕掉一張表單
```

### C# HttpClient 範例

```csharp
// 建立 HttpClient 實例
var client = new HttpClient();

// GET：取得使用者列表
var response = await client.GetAsync(""https://api.example.com/users"");
// 讀取回應內容
var body = await response.Content.ReadAsStringAsync();

// POST：新增一筆使用者資料
var newUser = new StringContent(
    // JSON 格式的使用者資料
    ""\""{  \""name\"": \""小明\"", \""age\"": 25 }\"" "",
    System.Text.Encoding.UTF8,
    // 指定內容類型為 JSON
    ""application/json""
);
// 送出 POST 請求
var postResponse = await client.PostAsync(""https://api.example.com/users"", newUser);

// PUT：完整更新使用者（需要傳入所有欄位）
var updatedUser = new StringContent(
    // 完整的使用者資料
    ""\""{  \""name\"": \""小明\"", \""age\"": 26, \""email\"": \""ming@example.com\"" }\"" "",
    System.Text.Encoding.UTF8,
    ""application/json""
);
// 送出 PUT 請求到指定的使用者 ID
await client.PutAsync(""https://api.example.com/users/1"", updatedUser);

// DELETE：刪除指定使用者
await client.DeleteAsync(""https://api.example.com/users/1"");
```

---

## HTTP 狀態碼（Status Codes）

### 2xx — 成功 ✅

```
狀態碼   意思             什麼時候會看到
──────────────────────────────────────────
200      OK               一般請求成功
201      Created          POST 新增成功
204      No Content       DELETE 成功，沒有回傳內容
```

### 3xx — 重新導向 🔄

```
狀態碼   意思             什麼時候會看到
──────────────────────────────────────────
301      永久搬家         舊網址永久轉到新網址
302      暫時搬家         暫時轉向（例如登入後跳轉）
304      沒有修改         瀏覽器可以用快取
```

### 4xx — 客戶端錯誤 ❌

```
狀態碼   意思             什麼時候會看到
──────────────────────────────────────────
400      Bad Request      參數格式錯誤
401      Unauthorized     沒有登入（未驗證身份）
403      Forbidden        有登入但沒有權限
404      Not Found        網址或資源不存在
405      Method Not Allowed  用了不支援的 HTTP 方法
429      Too Many Requests   請求太頻繁被限流
```

### 5xx — 伺服器錯誤 💥

```
狀態碼   意思             什麼時候會看到
──────────────────────────────────────────
500      Internal Server Error  程式碼出錯（Bug）
502      Bad Gateway      反向代理連不到後端
503      Service Unavailable  伺服器維護中
504      Gateway Timeout  後端回應太慢
```

---

## HTTP Headers（標頭）

Headers 就像信封上的附加資訊，告訴對方一些額外的細節。

### 常見 Request Headers

```http
GET /api/users HTTP/1.1
Host: api.example.com
Content-Type: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
Accept: application/json
Cache-Control: no-cache
User-Agent: MyApp/1.0
```

### 重點 Headers 說明

```
Header           用途                    比喻
──────────────────────────────────────────────────────
Content-Type     告訴對方資料格式         信封上寫「內含文件」
Authorization    身份驗證憑證             出示你的員工證
Cache-Control    快取策略                 這封信可以影印保存嗎？
Accept           期望的回應格式           我要中文版的回覆
```

### C# 設定 Headers

```csharp
// 建立 HttpClient 並設定預設標頭
var client = new HttpClient();
// 設定 Authorization 標頭（Bearer Token）
client.DefaultRequestHeaders.Authorization =
    new System.Net.Http.Headers.AuthenticationHeaderValue(""Bearer"", ""your-token-here"");
// 設定 Accept 標頭，告訴伺服器我們要 JSON 格式
client.DefaultRequestHeaders.Accept.Add(
    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(""application/json""));
```

---

## HTTPS 與 TLS

> 💡 **比喻：信封加鎖**
> - HTTP = 明信片（任何人都能看到內容）
> - HTTPS = 信封 + 掛號鎖（只有收件人能打開）

### TLS 握手過程（簡化版）

```
瀏覽器                           伺服器
  |                                 |
  |--- 1. 你好，我要安全連線 ------>|
  |                                 |
  |<-- 2. 這是我的證書（公鑰）-----|
  |                                 |
  |--- 3. 用公鑰加密對稱金鑰 ----->|
  |                                 |
  |<== 4. 之後都用對稱金鑰加密 ===>|
  |     （快速又安全）              |
```

### 為什麼需要 HTTPS？

```
沒有 HTTPS 的風險：
├── 竊聽（Eavesdropping）    → 駭客看到你的密碼
├── 篡改（Tampering）        → 中間人修改回應內容
└── 偽裝（Impersonation）    → 假的銀行網站騙你的資料
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：用 GET 傳送敏感資料

```csharp
// ❌ 錯誤：密碼出現在網址列，會被瀏覽器歷史紀錄和伺服器 Log 記錄
var url = ""https://api.example.com/login?username=admin&password=12345"";
await client.GetAsync(url);

// ✅ 正確：用 POST 把敏感資料放在 Body 中
var loginData = new StringContent(
    // 密碼放在請求主體中，不會出現在網址列
    ""\""{  \""username\"": \""admin\"", \""password\"": \""12345\"" }\"" "",
    System.Text.Encoding.UTF8,
    ""application/json""
);
// POST 請求會把資料放在 Body，而不是 URL
await client.PostAsync(""https://api.example.com/login"", loginData);
```

### ❌ 錯誤 2：忽略狀態碼，只看有沒有回應

```csharp
// ❌ 錯誤：沒有檢查狀態碼，可能拿到錯誤的回應還繼續處理
var response = await client.GetAsync(""https://api.example.com/users/999"");
var data = await response.Content.ReadAsStringAsync();
// 如果回傳 404，data 可能是錯誤訊息而不是使用者資料

// ✅ 正確：先檢查狀態碼
var response2 = await client.GetAsync(""https://api.example.com/users/999"");
if (response2.IsSuccessStatusCode) // 檢查是否為 2xx 成功
{
    // 只有成功時才處理資料
    var userData = await response2.Content.ReadAsStringAsync();
    Console.WriteLine(userData);
}
else
{
    // 失敗時印出狀態碼，方便除錯
    Console.WriteLine($""錯誤：{(int)response2.StatusCode} {response2.ReasonPhrase}"");
}
```

### ❌ 錯誤 3：沒有正確使用 Content-Type

```csharp
// ❌ 錯誤：送 JSON 但沒有指定 Content-Type，伺服器可能無法解析
var content = new StringContent(""\""{  \""name\"": \""test\"" }\"" "");
await client.PostAsync(""https://api.example.com/users"", content);

// ✅ 正確：明確指定 Content-Type 為 JSON
var correctContent = new StringContent(
    ""\""{  \""name\"": \""test\"" }\"" "",
    System.Text.Encoding.UTF8,
    // 告訴伺服器這是 JSON 格式的資料
    ""application/json""
);
await client.PostAsync(""https://api.example.com/users"", correctContent);
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| HTTP Method | 告訴伺服器你要做什麼（GET/POST/PUT/DELETE） |
| Status Code | 伺服器的處理結果（2xx 成功 / 4xx 你的錯 / 5xx 伺服器的錯） |
| Headers | 請求和回應的附加資訊（Content-Type、Authorization） |
| HTTPS | 加密版的 HTTP，保護傳輸安全 |
| TLS | HTTPS 底層的加密協議 |
" },

        // ── 網路 Chapter 32 ─────────────────────────────────────
        new() { Id=32, Category="network", Order=3, Level="intermediate", Icon="🔗", Title="DNS 與網域名稱系統", Slug="network-dns", IsPublished=true, Content=@"
# DNS 與網域名稱系統

## 什麼是 DNS？

DNS（Domain Name System）負責把**人類看得懂的網域名稱**轉換成**電腦看得懂的 IP 位址**。

> 💡 **比喻：電話簿**
> 你記得朋友的名字「小明」，但打電話需要號碼 0912-345-678。
> DNS 就像一本超大的電話簿：
> - 你說：「我要找 google.com」
> - DNS 回答：「他的號碼是 142.250.185.78」
> - 瀏覽器就用這個 IP 去連線

---

## DNS 解析流程

```
你的瀏覽器輸入 www.example.com
        |
        ↓
1. 瀏覽器快取 → 之前查過嗎？查過就直接用
        |（沒有）
        ↓
2. 作業系統快取 → OS 記得嗎？
        |（沒有）
        ↓
3. DNS Resolver（ISP 提供）→ 幫你去問
        |
        ↓
4. Root DNS → 「.com 要去問 .com 的 DNS」
        |
        ↓
5. TLD DNS（.com）→ 「example.com 在這個 NS」
        |
        ↓
6. Authoritative DNS → 「www.example.com = 93.184.216.34」
        |
        ↓
瀏覽器拿到 IP，開始連線！
```

---

## DNS 記錄類型

### 常見記錄一覽

```
記錄類型   用途                      範例
──────────────────────────────────────────────────────
A          網域 → IPv4 位址          example.com → 93.184.216.34
AAAA       網域 → IPv6 位址          example.com → 2606:2800:220:1::
CNAME      網域別名（指向另一個網域）  www.example.com → example.com
MX         郵件伺服器                example.com → mail.example.com
TXT        文字記錄（驗證用）          用於 SPF、DKIM、網域驗證
NS         指定 DNS 伺服器           example.com → ns1.provider.com
```

### 實際設定範例

```
; 這是 DNS Zone 設定檔範例
; A 記錄：把網域指向伺服器 IP
example.com.       A      93.184.216.34
; CNAME：www 子網域指向主網域
www.example.com.   CNAME  example.com.
; MX：郵件伺服器設定（數字越小優先級越高）
example.com.       MX     10 mail.example.com.
; TXT：SPF 記錄，指定哪些伺服器可以寄信
example.com.       TXT    ""v=spf1 include:_spf.google.com ~all""
; NS：指定管理此網域的 DNS 伺服器
example.com.       NS     ns1.cloudflare.com.
```

---

## DNS 傳播與 TTL

### TTL（Time To Live）

```
TTL = 快取保留時間（秒）

TTL=300   → 快取 5 分鐘（變更後 5 分鐘生效）
TTL=3600  → 快取 1 小時
TTL=86400 → 快取 1 天

建議：
├── 平常用較長 TTL（3600+）→ 減少查詢次數，加快速度
└── 要改 DNS 時先降低 TTL（300）→ 讓變更快速生效
```

### DNS 傳播

```
你修改了 DNS 記錄...

  時間線
  ├── 0 分鐘    → 你的 DNS 提供商已更新
  ├── 5 分鐘    → 部分 ISP 的 DNS 已更新
  ├── 1 小時    → 大部分地區已更新
  └── 24-48 小時 → 全球完全傳播完成

  為什麼要這麼久？
  因為全球各地的 DNS 伺服器都有快取，
  要等舊快取過期（TTL 到期）才會去查新記錄
```

---

## DNS 查詢工具

### nslookup

```bash
# 查詢 A 記錄（最基本的查詢）
nslookup example.com

# 指定查詢類型為 MX（郵件伺服器）
nslookup -type=MX example.com

# 使用 Google 的 DNS 伺服器（8.8.8.8）來查詢
nslookup example.com 8.8.8.8

# 查詢 CNAME 記錄
nslookup -type=CNAME www.example.com
```

### dig（Linux/macOS）

```bash
# 查詢 A 記錄（詳細輸出）
dig example.com

# 只顯示簡短結果
dig +short example.com

# 查詢 MX 記錄
dig MX example.com

# 追蹤完整的 DNS 解析過程（從 Root DNS 開始）
dig +trace example.com

# 查詢特定 DNS 伺服器
dig @8.8.8.8 example.com
```

---

## CDN 基礎概念

> 💡 **比喻：7-11 物流系統**
> 如果所有商品都從台北總倉出貨，花蓮的客人要等很久。
> 所以 7-11 在各地都有**物流中心（節點）**，把商品預先放好。
> CDN 也一樣：
> - **Origin Server（原始伺服器）** = 台北總倉
> - **CDN Edge Node（邊緣節點）** = 各地物流中心
> - **使用者** = 各地的客人（從最近的節點取貨）

### CDN 運作流程

```
使用者（高雄）
    |
    ↓ 請求 www.example.com/image.jpg
    |
CDN 邊緣節點（高雄）
    |
    ├── 有快取？→ 直接回傳（超快！）
    |
    └── 沒快取？→ 去 Origin Server 拿
                    ↓
              Origin Server（美國）
                    ↓
              拿到後存一份在高雄節點
                    ↓
              下次高雄的人要就直接給
```

### 常見 CDN 服務

```
服務           特色
───────────────────────────────────
Cloudflare     免費方案、自帶 DNS 和 DDoS 防護
AWS CloudFront 和 AWS 深度整合
Azure CDN      和 Azure 深度整合
Akamai         老牌 CDN，企業級
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：改了 DNS 但網站沒有馬上生效

```
問題：我改了 A 記錄指向新伺服器，但網站還是連到舊的？

原因：DNS 快取還沒過期（TTL 還沒到）

✅ 正確做法：
1. 修改 DNS 之前，先把 TTL 降低到 300（5 分鐘）
2. 等舊 TTL 過期後再修改 A 記錄
3. 確認生效後，把 TTL 調回 3600
4. 本機可以清除 DNS 快取來立即看到變更
```

```bash
# Windows 清除 DNS 快取
ipconfig /flushdns

# macOS 清除 DNS 快取
sudo dscacheutil -flushcache

# Linux 清除 DNS 快取（systemd-resolved）
sudo systemd-resolve --flush-caches
```

### ❌ 錯誤 2：CNAME 和 A 記錄搞混

```
問題：我想讓 www.example.com 指向 example.com

❌ 錯誤：設定 A 記錄指向另一個網域名稱
www.example.com  A  example.com  （A 記錄只能指向 IP！）

✅ 正確：用 CNAME 記錄指向另一個網域
www.example.com  CNAME  example.com.  （CNAME 才能指向網域）

或者：用 A 記錄指向 IP
www.example.com  A  93.184.216.34  （A 記錄指向 IP 也可以）
```

### ❌ 錯誤 3：沒有設定 MX 記錄導致收不到信

```
問題：買了網域 example.com 但收不到 info@example.com 的信

原因：沒有設定 MX 記錄，郵件系統不知道信要送到哪裡

✅ 正確做法：設定 MX 記錄指向郵件伺服器
example.com.  MX  10  mail.example.com.
example.com.  MX  20  backup-mail.example.com.
; 數字越小優先級越高，10 比 20 優先
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| DNS | 把網域名稱轉換成 IP 位址的系統 |
| A 記錄 | 網域 → IP 位址 |
| CNAME | 網域別名 → 另一個網域 |
| MX | 郵件伺服器指向 |
| TTL | DNS 快取保留時間（秒） |
| CDN | 內容分發網路，讓使用者從最近的節點取得資料 |
" },

        // ── 網路 Chapter 33 ─────────────────────────────────────
        new() { Id=33, Category="network", Order=4, Level="advanced", Icon="🔌", Title="WebSocket 與即時通訊", Slug="network-websocket", IsPublished=true, Content=@"
# WebSocket 與即時通訊

## 為什麼需要即時通訊？

傳統 HTTP 是「一問一答」模式：客戶端發請求，伺服器才回應。
但有些場景需要**伺服器主動推送**資料給客戶端：

```
需要即時通訊的場景：
├── 聊天室（LINE、Discord）
├── 股票即時報價
├── 多人線上遊戲
├── 通知系統
└── 協作編輯（Google Docs）
```

---

## 四種即時通訊方案比較

### 1. 短輪詢（Short Polling）

```
客戶端                     伺服器
  |--- 有新訊息嗎？ -------->|
  |<-- 沒有 -----------------|
  |（等 3 秒）                |
  |--- 有新訊息嗎？ -------->|
  |<-- 沒有 -----------------|
  |（等 3 秒）                |
  |--- 有新訊息嗎？ -------->|
  |<-- 有！這是新訊息 --------|

缺點：浪費資源，大部分請求都是空的
```

### 2. 長輪詢（Long Polling）

```
客戶端                     伺服器
  |--- 有新訊息嗎？ -------->|
  |    （伺服器先不回應...     |
  |     等到有新訊息才回）     |
  |<-- 有！這是新訊息 --------|
  |--- 有新訊息嗎？ -------->|  ← 馬上再問
  |    （繼續等...）           |

優點：減少空的回應
缺點：每次收到訊息都要重新建立連線
```

### 3. Server-Sent Events（SSE）

```
客戶端                     伺服器
  |--- 我要訂閱 ------------>|
  |<== 保持連線 ============>|
  |<-- 新訊息 1 -------------|
  |<-- 新訊息 2 -------------|
  |<-- 新訊息 3 -------------|

優點：伺服器可以持續推送
缺點：單向（只能伺服器 → 客戶端）
```

### 4. WebSocket（雙向即時通訊）

```
客戶端                     伺服器
  |--- HTTP 升級請求 -------->|
  |<-- 101 Switching ---------|
  |<=== WebSocket 連線 =====>|
  |<--> 雙向即時通訊 <------->|
  |--- 我說哈囉 ------------>|
  |<-- 伺服器說嗨 ------------|
  |<-- 伺服器推通知 ----------|

優點：雙向、低延遲、省頻寬
```

### 方案比較表

```
方案           方向     延遲    複雜度   適用場景
───────────────────────────────────────────────────
短輪詢         單向     高      低       簡單通知
長輪詢         單向     中      中       即時通知
SSE            單向     低      中       股票報價、新聞推播
WebSocket      雙向     最低    高       聊天室、遊戲
```

---

## WebSocket 握手過程

```http
# 客戶端發送升級請求（從 HTTP 升級到 WebSocket）
GET /chat HTTP/1.1
Host: example.com
Upgrade: websocket
Connection: Upgrade
Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==
Sec-WebSocket-Version: 13
```

```http
# 伺服器回應同意升級
HTTP/1.1 101 Switching Protocols
Upgrade: websocket
Connection: Upgrade
Sec-WebSocket-Accept: s3pPLMBiTxaQ9kYGzzhZRbK+xOo=
```

```
握手完成後：
├── 不再是 HTTP 協議了
├── 變成 WebSocket 協議（ws:// 或 wss://）
├── 連線保持開啟，雙方可以隨時傳資料
└── 資料以「Frame」為單位傳輸（比 HTTP 輕量很多）
```

---

## C# WebSocket 客戶端

```csharp
using System.Net.WebSockets;
using System.Text;

// 建立 WebSocket 客戶端
var ws = new ClientWebSocket();

// 連接到 WebSocket 伺服器
await ws.ConnectAsync(
    new Uri(""wss://echo.websocket.org""),
    CancellationToken.None
);
// 確認連線狀態
Console.WriteLine($""連線狀態：{ws.State}"");  // Open

// 傳送訊息
var message = ""你好，WebSocket！"";
// 把字串轉成 byte 陣列
var bytes = Encoding.UTF8.GetBytes(message);
// 送出訊息（Text 類型，EndOfMessage 表示這是完整的一筆）
await ws.SendAsync(
    new ArraySegment<byte>(bytes),
    WebSocketMessageType.Text,
    endOfMessage: true,
    CancellationToken.None
);

// 接收訊息
var buffer = new byte[1024];
// 等待伺服器回傳訊息
var result = await ws.ReceiveAsync(
    new ArraySegment<byte>(buffer),
    CancellationToken.None
);
// 把收到的 byte 轉回字串
var received = Encoding.UTF8.GetString(buffer, 0, result.Count);
Console.WriteLine($""收到：{received}"");

// 關閉連線（禮貌地告訴對方要斷線了）
await ws.CloseAsync(
    WebSocketCloseStatus.NormalClosure,
    ""再見"",
    CancellationToken.None
);
```

---

## SignalR — 更好用的即時通訊框架

> 💡 **比喻**
> WebSocket 像是自己接水管——你要處理連線、斷線、重連、序列化...
> SignalR 像是請水電師傅來裝——幫你處理好所有細節，你只要開水龍頭就好。

### SignalR Hub（伺服器端）

```csharp
using Microsoft.AspNetCore.SignalR;

// 定義一個聊天 Hub（類似 Controller）
public class ChatHub : Hub
{
    // 當客戶端呼叫 SendMessage 時觸發
    public async Task SendMessage(string user, string message)
    {
        // 廣播給所有連線的客戶端
        await Clients.All.SendAsync(""ReceiveMessage"", user, message);
    }

    // 加入特定群組（例如聊天室）
    public async Task JoinRoom(string roomName)
    {
        // 把目前的連線加入指定群組
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        // 通知群組內的其他人
        await Clients.Group(roomName).SendAsync(
            ""ReceiveMessage"", ""系統"", $""{Context.ConnectionId} 加入了 {roomName}""
        );
    }

    // 當客戶端斷線時觸發
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // 可以在這裡做清理工作
        Console.WriteLine($""使用者 {Context.ConnectionId} 已斷線"");
        await base.OnDisconnectedAsync(exception);
    }
}
```

### SignalR 客戶端（C#）

```csharp
using Microsoft.AspNetCore.SignalR.Client;

// 建立 SignalR 連線
var connection = new HubConnectionBuilder()
    // 指定 Hub 的 URL
    .WithUrl(""https://localhost:5001/chatHub"")
    // 啟用自動重連（斷線後自動嘗試重新連線）
    .WithAutomaticReconnect()
    .Build();

// 監聽伺服器推送的 ReceiveMessage 事件
connection.On<string, string>(""ReceiveMessage"", (user, message) =>
{
    // 收到訊息時顯示在 Console
    Console.WriteLine($""{user}: {message}"");
});

// 監聽重連事件
connection.Reconnecting += error =>
{
    Console.WriteLine(""正在重新連線..."");
    return Task.CompletedTask;
};

// 開始連線
await connection.StartAsync();
Console.WriteLine(""已連線到 SignalR Hub"");

// 發送訊息
await connection.InvokeAsync(""SendMessage"", ""小明"", ""大家好！"");
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：沒有處理 WebSocket 斷線重連

```csharp
// ❌ 錯誤：連線斷了就整個掛掉
var ws = new ClientWebSocket();
await ws.ConnectAsync(uri, CancellationToken.None);
// 如果網路中斷，下面的 ReceiveAsync 會拋出例外，程式就結束了
var result = await ws.ReceiveAsync(buffer, CancellationToken.None);

// ✅ 正確：加入斷線重連機制
async Task ConnectWithRetry(Uri uri)
{
    // 最多重試 5 次
    var maxRetries = 5;
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            var ws = new ClientWebSocket();
            // 嘗試連線
            await ws.ConnectAsync(uri, CancellationToken.None);
            Console.WriteLine(""連線成功！"");
            // 連線成功後開始接收訊息
            await ReceiveLoop(ws);
        }
        catch (Exception ex)
        {
            // 連線失敗或斷線，等待後重試
            Console.WriteLine($""連線失敗：{ex.Message}，{i + 1}/{maxRetries} 次重試"");
            // 指數退避：每次等待時間加倍
            await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i)));
        }
    }
}
```

### ❌ 錯誤 2：WebSocket 記憶體洩漏

```csharp
// ❌ 錯誤：每次接收都建立新的 byte 陣列，造成 GC 壓力
while (ws.State == WebSocketState.Open)
{
    // 每次迴圈都分配新的記憶體（浪費！）
    var buffer = new byte[4096];
    await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
}

// ✅ 正確：重複使用同一個 buffer
// 在迴圈外面就分配好 buffer
var buffer2 = new byte[4096];
while (ws.State == WebSocketState.Open)
{
    // 重複使用同一塊記憶體
    var result2 = await ws.ReceiveAsync(
        new ArraySegment<byte>(buffer2),
        CancellationToken.None
    );
    // 只處理實際收到的資料長度
    var msg = Encoding.UTF8.GetString(buffer2, 0, result2.Count);
}
```

### ❌ 錯誤 3：沒有正確關閉 WebSocket 連線

```csharp
// ❌ 錯誤：直接斷線，沒有通知對方
ws.Dispose();  // 粗暴地斷開，對方不知道發生什麼事

// ✅ 正確：先發送 Close 訊框，再關閉
// 禮貌地告訴對方要關閉連線
await ws.CloseAsync(
    WebSocketCloseStatus.NormalClosure,
    ""正常關閉"",
    CancellationToken.None
);
// 關閉後再釋放資源
ws.Dispose();
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Short Polling | 定時發請求問有沒有新資料（浪費資源） |
| Long Polling | 請求掛著等有新資料才回應 |
| SSE | 伺服器單向推送（適合通知） |
| WebSocket | 雙向即時通訊（適合聊天、遊戲） |
| SignalR | ASP.NET Core 的即時通訊框架，簡化 WebSocket 使用 |
| 重連機制 | WebSocket 必須處理斷線重連，建議用指數退避 |
" },
    };
}
