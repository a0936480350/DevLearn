using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_HTML
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── 1600: HTML 簡介 ──
        new() { Id=1600, Category="html", Order=1, Level="beginner", Icon="📄", Title="HTML 簡介與文件結構", Slug="html-intro", IsPublished=true, Content=@"
# HTML 簡介與文件結構

## HTML 是什麼？

> **比喻：HTML 就像房子的骨架** 🏗️
>
> 鋼筋決定了牆壁在哪、門在哪、窗戶在哪。
> HTML 決定了網頁上「這裡是標題」「那裡是圖片」「這邊是連結」。

HTML = HyperText Markup Language（超文本標記語言）

---

## 標籤（Tag）的基本概念

```html
<h1>Hello World</h1>
│  │            │  │
│  │            │  └── 結束標籤的斜線
│  │            └───── 結束標籤
│  └────────────────── 內容（Content）
└───────────────────── 開始標籤
```

```html
<!-- 配對標籤（有開有關） -->
<p>這是一個段落</p>
<div>這是一個容器</div>

<!-- 自閉合標籤（沒有內容） -->
<br>              <!-- 換行 -->
<hr>              <!-- 水平線 -->
<img src=""pic.jpg"" alt=""圖片"">  <!-- 圖片 -->
<input type=""text"">             <!-- 輸入框 -->
```

---

## 完整的 HTML5 文件結構

```html
<!DOCTYPE html>                    <!-- ← 宣告這是 HTML5 文件 -->
<html lang=""zh-Hant"">              <!-- ← 根元素，lang 設定語言 -->

<head>                             <!-- ← 頭部：放「看不見」的設定 -->
    <meta charset=""UTF-8"">         <!-- ← 字元編碼（支援中文） -->
    <meta name=""viewport""          <!-- ← 手機適配 -->
          content=""width=device-width, initial-scale=1.0"">
    <title>我的網頁</title>         <!-- ← 瀏覽器分頁標題 -->
    <link rel=""stylesheet""         <!-- ← 引入 CSS -->
          href=""style.css"">
</head>

<body>                             <!-- ← 身體：放「看得見」的內容 -->
    <h1>歡迎</h1>                  <!-- ← 主標題 -->
    <p>這是第一個網頁。</p>         <!-- ← 段落 -->
    <script src=""app.js""></script>  <!-- ← 引入 JS（放 body 最後） -->
</body>

</html>
```

逐行解析：
```
<!DOCTYPE html>          -- 告訴瀏覽器用 HTML5 模式解析（不寫會進入怪異模式）
<html lang=""zh-Hant"">    -- 根元素，lang 幫助搜尋引擎和螢幕閱讀器識別語言
<meta charset=""UTF-8"">   -- 字元編碼設為 UTF-8，才能正確顯示中文、日文等
<meta name=""viewport"">   -- 讓手機瀏覽器用裝置寬度顯示（不設會縮很小）
<title>              -- 顯示在瀏覽器分頁上的文字（也影響 SEO）
<link rel=""stylesheet"">  -- 引入外部 CSS 檔案
<script src=""app.js"">    -- 引入外部 JavaScript，放 body 最後避免阻塞載入
```

---

## 標籤的屬性

```html
<a href=""https://google.com""     <!-- href 是屬性 -->
   target=""_blank""               <!-- target 也是屬性 -->
   class=""link""                  <!-- class 用來套 CSS -->
   id=""main-link"">               <!-- id 是唯一識別碼 -->
   前往 Google
</a>
```

常見全域屬性：

| 屬性 | 用途 | 範例 |
|------|------|------|
| `id` | 唯一識別碼 | `id=""header""` |
| `class` | CSS 類別（可多個） | `class=""btn primary""` |
| `style` | 行內 CSS | `style=""color: red""` |
| `title` | 提示文字（hover 顯示） | `title=""點我""` |
| `data-*` | 自訂資料屬性 | `data-id=""123""` |
| `hidden` | 隱藏元素 | `hidden` |

---

## 註解

```html
<!-- 這是註解，不會顯示在頁面上 -->
<!--
    多行註解
    也可以這樣寫
-->

<!-- TODO: 之後要加上 footer -->
```
" },

        // ── 1601: 文字標籤 ──
        new() { Id=1601, Category="html", Order=2, Level="beginner", Icon="📝", Title="文字相關標籤", Slug="html-text-tags", IsPublished=true, Content=@"
# 文字相關標籤

## 標題（h1 ~ h6）

```html
<h1>主標題（最大）</h1>     <!-- ← 一頁通常只有一個 h1 -->
<h2>副標題</h2>             <!-- ← 章節標題 -->
<h3>小節標題</h3>           <!-- ← 子章節 -->
<h4>更小的標題</h4>
<h5>很少用到</h5>
<h6>最小的標題</h6>
```

> ⚠️ 不要為了字大而用 h1！標題層級要有語意。
> ```html
> <!-- ❌ 壞：用 h1 只是因為想要大字 -->
> <h1>注意事項</h1>
>
> <!-- ✅ 好：用 CSS 控制大小 -->
> <p class=""large-text"">注意事項</p>
> ```

---

## 段落與文字

```html
<!-- 段落 -->
<p>這是一個段落。段落之間會自動有間距。</p>
<p>這是第二個段落。</p>

<!-- 換行（不建立新段落） -->
<p>第一行<br>第二行</p>

<!-- 水平線 -->
<hr>

<!-- 粗體 / 斜體 / 底線 -->
<strong>重要文字</strong>     <!-- ← 語意上「重要」，通常顯示為粗體 -->
<b>粗體文字</b>              <!-- ← 只是視覺上粗體，沒有語意 -->
<em>強調文字</em>            <!-- ← 語意上「強調」，通常顯示為斜體 -->
<i>斜體文字</i>              <!-- ← 只是視覺上斜體 -->
<u>底線文字</u>              <!-- ← 底線（少用，容易被誤認為連結） -->
<s>刪除線</s>                <!-- ← 表示已不正確的資訊 -->
<del>被刪除的</del>          <!-- ← 語意上表示「刪除」 -->
<ins>新插入的</ins>          <!-- ← 語意上表示「新增」 -->

<!-- 上標 / 下標 -->
<p>水的化學式是 H<sub>2</sub>O</p>
<p>面積 = r<sup>2</sup> × π</p>

<!-- 預格式化文字（保留空格和換行） -->
<pre>
    function hello() {
        console.log(""Hello"");
    }
</pre>

<!-- 程式碼 -->
<p>請使用 <code>console.log()</code> 來除錯。</p>

<!-- 引用 -->
<blockquote>
    <p>學而不思則罔，思而不學則殆。</p>
    <cite>— 孔子</cite>
</blockquote>

<!-- 行內引用 -->
<p>孔子說過 <q>學而時習之</q>。</p>
```

---

## 清單

```html
<!-- 無序列表（項目符號） -->
<ul>
    <li>蘋果</li>
    <li>香蕉</li>
    <li>橘子</li>
</ul>

<!-- 有序列表（編號） -->
<ol>
    <li>第一步：開啟檔案</li>
    <li>第二步：編輯內容</li>
    <li>第三步：儲存</li>
</ol>

<!-- 自訂起始編號 -->
<ol start=""5"" type=""A"">
    <li>E 項目</li>
    <li>F 項目</li>
</ol>

<!-- 巢狀清單 -->
<ul>
    <li>前端
        <ul>
            <li>HTML</li>
            <li>CSS</li>
            <li>JavaScript</li>
        </ul>
    </li>
    <li>後端
        <ul>
            <li>C#</li>
            <li>Python</li>
        </ul>
    </li>
</ul>

<!-- 定義列表（術語 + 解釋） -->
<dl>
    <dt>HTML</dt>
    <dd>超文本標記語言，用來建立網頁結構。</dd>
    <dt>CSS</dt>
    <dd>階層式樣式表，用來美化網頁。</dd>
</dl>
```

---

## 特殊字元

```html
<!-- 用實體名稱表示特殊字元 -->
&lt;     <!-- < -->
&gt;     <!-- > -->
&amp;    <!-- & -->
&quot;   <!-- "" -->
&apos;   <!-- ' -->
&nbsp;   <!-- 不換行空格 -->
&copy;   <!-- © -->
&reg;    <!-- ® -->
&trade;  <!-- ™ -->
&mdash;  <!-- — -->
```
" },

        // ── 1602: 連結與圖片 ──
        new() { Id=1602, Category="html", Order=3, Level="beginner", Icon="🔗", Title="連結與圖片", Slug="html-links-images", IsPublished=true, Content=@"
# 連結與圖片

## 超連結（`<a>`）

```html
<!-- 基本連結 -->
<a href=""https://google.com"">前往 Google</a>

<!-- 新分頁開啟 -->
<a href=""https://google.com"" target=""_blank"" rel=""noopener noreferrer"">
    在新分頁開啟 Google
</a>
```

逐行解析：
```
href=""https://google.com""    -- 連結目標 URL
target=""_blank""              -- 在新分頁開啟（不加就是同分頁）
rel=""noopener noreferrer""    -- 安全性：防止新頁面存取原頁面的 window 物件
```

### 連結類型

```html
<!-- 外部連結 -->
<a href=""https://example.com"">外部網站</a>

<!-- 相對連結 -->
<a href=""/about"">關於我們</a>          <!-- 從根目錄開始 -->
<a href=""./contact.html"">聯絡我們</a>  <!-- 同一層 -->
<a href=""../index.html"">回首頁</a>     <!-- 上一層 -->

<!-- 頁內錨點 -->
<a href=""#section2"">跳到第二段</a>
<h2 id=""section2"">第二段</h2>          <!-- 目標元素 -->

<!-- Email 連結 -->
<a href=""mailto:hello@example.com"">寄信給我</a>

<!-- 電話連結（手機可直接撥號） -->
<a href=""tel:+886912345678"">打電話</a>

<!-- 下載連結 -->
<a href=""/files/doc.pdf"" download>下載 PDF</a>
<a href=""/files/doc.pdf"" download=""自訂檔名.pdf"">下載</a>
```

---

## 圖片（`<img>`）

```html
<img src=""photo.jpg""              <!-- ← 圖片來源 -->
     alt=""一隻可愛的貓""           <!-- ← 替代文字（很重要！） -->
     width=""400""                  <!-- ← 寬度（像素） -->
     height=""300""                 <!-- ← 高度（像素） -->
     loading=""lazy""               <!-- ← 懶載入（進入畫面才載入） -->
>
```

### alt 屬性的重要性

```html
<!-- ✅ 好的 alt -->
<img src=""chart.png"" alt=""2024年營收成長圖表，Q4 成長 15%"">
<img src=""logo.png"" alt=""DevLearn Logo"">

<!-- ✅ 裝飾性圖片用空 alt -->
<img src=""divider.png"" alt="""">

<!-- ❌ 壞的 alt -->
<img src=""chart.png"" alt=""圖片"">          <!-- 太模糊 -->
<img src=""chart.png"">                      <!-- 沒有 alt！ -->
```

> alt 的用途：
> 1. 螢幕閱讀器會唸出 alt（無障礙）
> 2. 圖片載入失敗時顯示 alt
> 3. 搜尋引擎用 alt 理解圖片（SEO）

---

## 響應式圖片

```html
<!-- srcset — 根據裝置解析度選擇圖片 -->
<img src=""photo-400.jpg""
     srcset=""photo-400.jpg 400w,
             photo-800.jpg 800w,
             photo-1200.jpg 1200w""
     sizes=""(max-width: 600px) 400px,
            (max-width: 1000px) 800px,
            1200px""
     alt=""風景照"">

<!-- picture — 根據條件選擇不同圖片 -->
<picture>
    <source media=""(max-width: 600px)"" srcset=""photo-mobile.jpg"">
    <source media=""(max-width: 1000px)"" srcset=""photo-tablet.jpg"">
    <img src=""photo-desktop.jpg"" alt=""風景照"">  <!-- 後備 -->
</picture>

<!-- WebP 格式（更小、更快）+ 後備 -->
<picture>
    <source type=""image/webp"" srcset=""photo.webp"">
    <source type=""image/jpeg"" srcset=""photo.jpg"">
    <img src=""photo.jpg"" alt=""風景照"">
</picture>
```

---

## 圖片與連結結合

```html
<!-- 可點擊的圖片連結 -->
<a href=""/products/1"">
    <img src=""product.jpg"" alt=""iPhone 15"">
</a>

<!-- Figure — 圖片 + 標題 -->
<figure>
    <img src=""chart.png"" alt=""銷售圖表"">
    <figcaption>圖 1：2024 年第四季銷售數據</figcaption>
</figure>
```
" },

        // ── 1603: 表格 ──
        new() { Id=1603, Category="html", Order=4, Level="beginner", Icon="📊", Title="表格（Table）", Slug="html-tables", IsPublished=true, Content=@"
# 表格（Table）

## 基本表格結構

```html
<table>
    <thead>                        <!-- ← 表頭區 -->
        <tr>                       <!-- ← table row（一列） -->
            <th>姓名</th>          <!-- ← table header（表頭儲存格） -->
            <th>年齡</th>
            <th>城市</th>
        </tr>
    </thead>
    <tbody>                        <!-- ← 表身區 -->
        <tr>
            <td>小明</td>          <!-- ← table data（資料儲存格） -->
            <td>20</td>
            <td>台北</td>
        </tr>
        <tr>
            <td>小華</td>
            <td>22</td>
            <td>高雄</td>
        </tr>
    </tbody>
    <tfoot>                        <!-- ← 表尾區 -->
        <tr>
            <td colspan=""3"">共 2 筆</td>  <!-- ← 跨 3 欄 -->
        </tr>
    </tfoot>
</table>
```

---

## 合併儲存格

```html
<!-- colspan — 水平合併（跨欄） -->
<tr>
    <td colspan=""2"">我佔兩欄</td>  <!-- ← 這格佔 2 欄的寬度 -->
    <td>正常一格</td>
</tr>

<!-- rowspan — 垂直合併（跨列） -->
<tr>
    <td rowspan=""2"">我佔兩列</td>  <!-- ← 這格佔 2 列的高度 -->
    <td>A1</td>
</tr>
<tr>
    <!-- 這列不用再寫第一格（被上面的 rowspan 佔了） -->
    <td>A2</td>
</tr>
```

---

## 實用範例

```html
<!-- 課表 -->
<table>
    <thead>
        <tr>
            <th>時間</th>
            <th>週一</th>
            <th>週二</th>
            <th>週三</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>09:00</td>
            <td>數學</td>
            <td rowspan=""2"">英文（連堂）</td>
            <td>物理</td>
        </tr>
        <tr>
            <td>10:00</td>
            <td>國文</td>
            <td>化學</td>
        </tr>
    </tbody>
</table>
```

---

## 表格無障礙

```html
<!-- caption 描述表格內容 -->
<table>
    <caption>2024 年第四季銷售報表</caption>
    <thead>
        <tr>
            <th scope=""col"">月份</th>      <!-- scope 說明這是「欄標題」 -->
            <th scope=""col"">營收</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope=""row"">10 月</th>     <!-- scope 說明這是「列標題」 -->
            <td>$120,000</td>
        </tr>
    </tbody>
</table>
```

> 💡 **現代 CSS 排版建議**：簡單資料展示用 `<table>`，頁面版面配置用 CSS Flexbox/Grid，不要用表格排版。
" },

        // ── 1604: 表單 ──
        new() { Id=1604, Category="html", Order=5, Level="beginner", Icon="📋", Title="表單（Form）基礎", Slug="html-forms", IsPublished=true, Content=@"
# 表單（Form）基礎

## 表單結構

```html
<form action=""/api/register""    <!-- ← 送出的目標 URL -->
      method=""POST""             <!-- ← HTTP 方法 -->
      id=""registerForm"">

    <!-- 文字輸入 -->
    <label for=""username"">使用者名稱</label>
    <input type=""text"" id=""username"" name=""username""
           placeholder=""請輸入使用者名稱""
           required                  <!-- ← 必填 -->
           minlength=""3""            <!-- ← 最少 3 字 -->
           maxlength=""20"">          <!-- ← 最多 20 字 -->

    <!-- Email -->
    <label for=""email"">電子信箱</label>
    <input type=""email"" id=""email"" name=""email""
           placeholder=""example@mail.com""
           required>

    <!-- 密碼 -->
    <label for=""password"">密碼</label>
    <input type=""password"" id=""password"" name=""password""
           minlength=""8"" required>

    <!-- 送出按鈕 -->
    <button type=""submit"">註冊</button>
</form>
```

---

## 常用 input type

```html
<!-- 文字類 -->
<input type=""text"">          <!-- 一般文字 -->
<input type=""email"">         <!-- Email（會驗證格式） -->
<input type=""password"">      <!-- 密碼（隱藏輸入） -->
<input type=""tel"">           <!-- 電話（手機會彈數字鍵盤） -->
<input type=""url"">           <!-- 網址 -->
<input type=""search"">        <!-- 搜尋框 -->
<input type=""number""         <!-- 數字 -->
       min=""0"" max=""100"" step=""1"">
<input type=""range""          <!-- 滑桿 -->
       min=""0"" max=""100"" value=""50"">

<!-- 日期時間類 -->
<input type=""date"">          <!-- 日期選擇器 -->
<input type=""time"">          <!-- 時間選擇器 -->
<input type=""datetime-local""> <!-- 日期+時間 -->
<input type=""month"">         <!-- 年月 -->
<input type=""week"">          <!-- 週 -->

<!-- 選擇類 -->
<input type=""checkbox"">      <!-- 勾選框（可多選） -->
<input type=""radio"">         <!-- 單選按鈕 -->
<input type=""file"">          <!-- 檔案上傳 -->
<input type=""color"">         <!-- 顏色選擇器 -->

<!-- 隱藏 / 按鈕 -->
<input type=""hidden"" name=""token"" value=""abc123"">  <!-- 隱藏欄位 -->
<input type=""submit"" value=""送出"">
<input type=""reset"" value=""重設"">
```

---

## 選擇控制項

```html
<!-- 下拉選單 -->
<label for=""city"">城市</label>
<select id=""city"" name=""city"">
    <option value="""">請選擇</option>
    <option value=""taipei"">台北</option>
    <option value=""kaohsiung"" selected>高雄</option>  <!-- 預選 -->
    <optgroup label=""東部"">
        <option value=""hualien"">花蓮</option>
        <option value=""taitung"">台東</option>
    </optgroup>
</select>

<!-- 勾選框 -->
<fieldset>
    <legend>興趣（可多選）</legend>
    <label><input type=""checkbox"" name=""hobby"" value=""sports""> 運動</label>
    <label><input type=""checkbox"" name=""hobby"" value=""music""> 音樂</label>
    <label><input type=""checkbox"" name=""hobby"" value=""reading""> 閱讀</label>
</fieldset>

<!-- 單選按鈕 -->
<fieldset>
    <legend>性別</legend>
    <label><input type=""radio"" name=""gender"" value=""male""> 男</label>
    <label><input type=""radio"" name=""gender"" value=""female""> 女</label>
    <label><input type=""radio"" name=""gender"" value=""other""> 其他</label>
</fieldset>

<!-- 多行文字 -->
<label for=""bio"">自我介紹</label>
<textarea id=""bio"" name=""bio""
          rows=""4"" cols=""50""
          placeholder=""請簡短介紹自己...""></textarea>
```

---

## 表單驗證屬性

```html
<input required>                  <!-- 必填 -->
<input minlength=""3"" maxlength=""20"">  <!-- 字數限制 -->
<input min=""1"" max=""100"">        <!-- 數字範圍 -->
<input pattern=""[0-9]{3}-[0-9]{4}""> <!-- 正則驗證 -->
<input type=""email"">              <!-- 自動驗證 Email 格式 -->
```

---

## 檔案上傳

```html
<!-- 單一檔案 -->
<input type=""file"" name=""avatar"" accept=""image/*"">

<!-- 多檔案 -->
<input type=""file"" name=""photos"" multiple accept="".jpg,.png,.webp"">

<!-- 搭配 form 的 enctype -->
<form method=""POST"" enctype=""multipart/form-data"">
    <input type=""file"" name=""document"">
    <button type=""submit"">上傳</button>
</form>
```
" },

        // ── 1605: 表單進階 ──
        new() { Id=1605, Category="html", Order=6, Level="beginner", Icon="✅", Title="表單進階與驗證", Slug="html-forms-advanced", IsPublished=true, Content=@"
# 表單進階與驗證

## datalist — 自動完成建議

```html
<label for=""browser"">瀏覽器</label>
<input list=""browsers"" id=""browser"" name=""browser"">
<datalist id=""browsers"">          <!-- ← 和 input 的 list 對應 -->
    <option value=""Chrome"">
    <option value=""Firefox"">
    <option value=""Safari"">
    <option value=""Edge"">
</datalist>
<!-- 使用者輸入時會顯示建議清單 -->
```

---

## output — 顯示計算結果

```html
<form oninput=""result.value = parseInt(a.value) + parseInt(b.value)"">
    <input type=""number"" id=""a"" value=""0""> +
    <input type=""number"" id=""b"" value=""0""> =
    <output name=""result"" for=""a b"">0</output>
</form>
```

---

## 自訂驗證訊息

```html
<form id=""myForm"">
    <input type=""text"" id=""phone"" name=""phone""
           pattern=""09\d{8}""
           title=""請輸入 09 開頭的 10 位手機號碼""
           required>
    <button type=""submit"">送出</button>
</form>

<script>
    // JavaScript 自訂驗證
    let phone = document.getElementById(""phone"");
    phone.addEventListener(""input"", () => {
        if (phone.validity.patternMismatch) {
            phone.setCustomValidity(""手機號碼格式不正確，請輸入 09 開頭的 10 位數字"");
        } else {
            phone.setCustomValidity("""");   // 清除錯誤訊息
        }
    });
</script>
```

---

## 無障礙表單

```html
<!-- ✅ 用 label + for 關聯 -->
<label for=""email"">Email</label>
<input type=""email"" id=""email"" name=""email""
       aria-describedby=""email-help"">
<small id=""email-help"">我們不會分享你的 Email</small>

<!-- ✅ 用 fieldset + legend 分組 -->
<fieldset>
    <legend>聯絡方式</legend>
    <label for=""phone"">電話</label>
    <input type=""tel"" id=""phone"">
    <label for=""addr"">地址</label>
    <input type=""text"" id=""addr"">
</fieldset>

<!-- ✅ 錯誤提示 -->
<input type=""email"" id=""email""
       aria-invalid=""true""
       aria-errormessage=""email-error"">
<span id=""email-error"" role=""alert"">
    請輸入有效的 Email 地址
</span>
```

---

## 表單安全注意事項

```html
<!-- 1. 用 POST 傳送敏感資料（不要用 GET） -->
<form method=""POST"">              <!-- ✅ 資料在 body 裡 -->
<form method=""GET"">               <!-- ❌ 資料在 URL 上，會被看到 -->

<!-- 2. CSRF 防護 -->
<form method=""POST"">
    <input type=""hidden"" name=""__RequestVerificationToken"" value=""..."">
</form>

<!-- 3. autocomplete 控制 -->
<input type=""password"" autocomplete=""new-password"">  <!-- 新密碼 -->
<input type=""password"" autocomplete=""current-password""> <!-- 現有密碼 -->
<input type=""text"" autocomplete=""off"">  <!-- 關閉自動填入 -->
```
" },

        // ── 1606: 語意標籤 ──
        new() { Id=1606, Category="html", Order=7, Level="beginner", Icon="🏷️", Title="語意標籤（Semantic HTML）", Slug="html-semantic", IsPublished=true, Content=@"
# 語意標籤（Semantic HTML）

## 為什麼要用語意標籤？

> **比喻：語意標籤就像幫房間貼上門牌** 🏷️
>
> `<div>` 就像一個沒標籤的箱子——你不知道裡面裝什麼。
> `<nav>` 就像標了「導覽列」的箱子——一目了然。

### 好處：
1. **無障礙**：螢幕閱讀器能理解頁面結構
2. **SEO**：搜尋引擎更懂你的內容
3. **可維護**：程式碼更容易閱讀

---

## 語意標籤 vs div

```html
<!-- ❌ 壞：全部用 div，看不出結構 -->
<div class=""header"">
    <div class=""nav"">...</div>
</div>
<div class=""main"">
    <div class=""article"">...</div>
    <div class=""sidebar"">...</div>
</div>
<div class=""footer"">...</div>

<!-- ✅ 好：用語意標籤，結構清楚 -->
<header>
    <nav>...</nav>
</header>
<main>
    <article>...</article>
    <aside>...</aside>
</main>
<footer>...</footer>
```

---

## 頁面結構標籤

```html
<body>
    <header>                       <!-- ← 頁首：Logo、導覽列 -->
        <nav>                      <!-- ← 導覽列 -->
            <ul>
                <li><a href=""/"">首頁</a></li>
                <li><a href=""/about"">關於</a></li>
            </ul>
        </nav>
    </header>

    <main>                         <!-- ← 主要內容（一頁只有一個） -->
        <article>                  <!-- ← 獨立內容（文章、貼文） -->
            <header>
                <h1>文章標題</h1>
                <time datetime=""2024-01-15"">2024/01/15</time>
            </header>
            <section>              <!-- ← 區段（主題段落） -->
                <h2>第一段</h2>
                <p>...</p>
            </section>
            <section>
                <h2>第二段</h2>
                <p>...</p>
            </section>
            <footer>
                <p>作者：小明</p>
            </footer>
        </article>

        <aside>                    <!-- ← 側邊欄（相關但非主要的內容） -->
            <h3>相關文章</h3>
            <ul>
                <li><a href=""#"">另一篇文章</a></li>
            </ul>
        </aside>
    </main>

    <footer>                       <!-- ← 頁尾：版權、聯絡資訊 -->
        <p>&copy; 2024 DevLearn</p>
    </footer>
</body>
```

---

## 各標籤的用途

| 標籤 | 用途 | 範例 |
|------|------|------|
| `<header>` | 頁首或區段的頭部 | Logo、導覽 |
| `<nav>` | 導覽連結區 | 主選單、麵包屑 |
| `<main>` | 頁面主要內容（唯一） | 文章、產品列表 |
| `<article>` | 獨立可分享的內容 | 部落格文章、新聞 |
| `<section>` | 主題相關的區段 | 章節、功能區塊 |
| `<aside>` | 附帶內容 | 側邊欄、廣告 |
| `<footer>` | 頁尾或區段的尾部 | 版權、聯絡方式 |
| `<figure>` | 圖表 + 標題 | 圖片、程式碼區塊 |
| `<time>` | 時間 | 發布日期 |
| `<mark>` | 標記文字 | 搜尋結果高亮 |
| `<details>` | 可展開的內容 | FAQ |
| `<summary>` | details 的標題 | 點擊展開 |

---

## details 和 summary

```html
<!-- 原生可展開/收合（不需要 JS！） -->
<details>
    <summary>點擊查看答案</summary>
    <p>答案是 42。</p>
</details>

<details open>                  <!-- ← open 屬性 = 預設展開 -->
    <summary>常見問題</summary>
    <p>Q: 如何註冊？</p>
    <p>A: 點擊右上角的註冊按鈕。</p>
</details>
```
" },

        // ── 1607: 多媒體 ──
        new() { Id=1607, Category="html", Order=8, Level="beginner", Icon="🎬", Title="多媒體（Video / Audio）", Slug="html-multimedia", IsPublished=true, Content=@"
# 多媒體（Video / Audio）

## 影片（`<video>`）

```html
<video src=""movie.mp4""
       width=""640"" height=""360""
       controls                   <!-- ← 顯示控制列 -->
       autoplay                   <!-- ← 自動播放（通常需要 muted） -->
       muted                      <!-- ← 靜音（autoplay 需要） -->
       loop                       <!-- ← 循環播放 -->
       poster=""thumbnail.jpg""     <!-- ← 載入前顯示的預覽圖 -->
       preload=""metadata"">        <!-- ← 預載入：none/metadata/auto -->
    你的瀏覽器不支援影片播放。     <!-- ← 後備文字 -->
</video>

<!-- 多格式後備 -->
<video controls width=""640"">
    <source src=""movie.webm"" type=""video/webm"">  <!-- 優先用 WebM -->
    <source src=""movie.mp4"" type=""video/mp4"">    <!-- 後備用 MP4 -->
    <p>你的瀏覽器不支援 HTML5 影片。</p>
</video>
```

---

## 音訊（`<audio>`）

```html
<audio src=""song.mp3"" controls>
    你的瀏覽器不支援音訊播放。
</audio>

<!-- 多格式 -->
<audio controls>
    <source src=""song.ogg"" type=""audio/ogg"">
    <source src=""song.mp3"" type=""audio/mpeg"">
</audio>
```

---

## 嵌入外部內容（`<iframe>`）

```html
<!-- YouTube 影片 -->
<iframe width=""560"" height=""315""
        src=""https://www.youtube.com/embed/VIDEO_ID""
        title=""影片標題""
        frameborder=""0""
        allow=""accelerometer; autoplay; clipboard-write;
               encrypted-media; gyroscope; picture-in-picture""
        allowfullscreen
        loading=""lazy"">
</iframe>

<!-- Google Maps -->
<iframe src=""https://www.google.com/maps/embed?...""
        width=""600"" height=""450""
        style=""border:0""
        allowfullscreen
        loading=""lazy"">
</iframe>
```

> ⚠️ iframe 安全性注意：
> ```html
> <!-- sandbox 限制 iframe 的權限 -->
> <iframe src=""..."" sandbox=""allow-scripts allow-same-origin""></iframe>
> ```

---

## Canvas（畫布）

```html
<canvas id=""myCanvas"" width=""400"" height=""300"">
    你的瀏覽器不支援 Canvas
</canvas>

<script>
    let canvas = document.getElementById(""myCanvas"");
    let ctx = canvas.getContext(""2d"");

    // 畫一個紅色矩形
    ctx.fillStyle = ""red"";
    ctx.fillRect(10, 10, 100, 50);

    // 畫文字
    ctx.font = ""20px Arial"";
    ctx.fillStyle = ""black"";
    ctx.fillText(""Hello Canvas"", 10, 100);
</script>
```

---

## SVG（向量圖）

```html
<!-- 內嵌 SVG -->
<svg width=""100"" height=""100"">
    <circle cx=""50"" cy=""50"" r=""40""
            stroke=""#333"" stroke-width=""2""
            fill=""#61DAFB"" />
</svg>

<!-- 用 img 載入 SVG -->
<img src=""icon.svg"" alt=""圖示"" width=""24"" height=""24"">

<!-- SVG 的優點 -->
<!-- 1. 縮放不模糊（向量圖） -->
<!-- 2. 可用 CSS 改顏色 -->
<!-- 3. 檔案通常比 PNG 小 -->
```
" },

        // ── 1608: Meta 與 SEO ──
        new() { Id=1608, Category="html", Order=9, Level="intermediate", Icon="🔎", Title="Meta 標籤與 SEO 基礎", Slug="html-meta-seo", IsPublished=true, Content=@"
# Meta 標籤與 SEO 基礎

## Meta 標籤

```html
<head>
    <!-- 基本 Meta -->
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">

    <!-- SEO Meta -->
    <title>DevLearn — 免費學習程式設計</title>   <!-- ← 最重要的 SEO 元素 -->
    <meta name=""description""
          content=""DevLearn 提供免費的 C#、SQL、JavaScript 程式設計教學，適合初學者到進階開發者。"">
    <meta name=""keywords"" content=""程式設計, C#, SQL, JavaScript, 教學"">  <!-- Google 已不用 -->
    <meta name=""author"" content=""DevLearn"">

    <!-- 搜尋引擎指令 -->
    <meta name=""robots"" content=""index, follow"">     <!-- 允許索引和追蹤連結 -->
    <meta name=""robots"" content=""noindex, nofollow"">  <!-- 禁止索引 -->
    <link rel=""canonical"" href=""https://devlearn.com/chapter/sql-intro"">  <!-- 正規 URL -->
</head>
```

---

## Open Graph（社群分享）

```html
<!-- Facebook / LINE / Discord 分享時顯示的資訊 -->
<meta property=""og:type"" content=""website"">
<meta property=""og:title"" content=""SQL 基礎教學"">
<meta property=""og:description"" content=""從零開始學 SQL，逐行解析每個指令"">
<meta property=""og:image"" content=""https://devlearn.com/images/sql-og.jpg"">
<meta property=""og:url"" content=""https://devlearn.com/chapter/sql-intro"">
<meta property=""og:site_name"" content=""DevLearn"">
<meta property=""og:locale"" content=""zh_TW"">
```

---

## Twitter Card

```html
<meta name=""twitter:card"" content=""summary_large_image"">
<meta name=""twitter:title"" content=""SQL 基礎教學"">
<meta name=""twitter:description"" content=""從零開始學 SQL"">
<meta name=""twitter:image"" content=""https://devlearn.com/images/sql-og.jpg"">
```

---

## 結構化資料（JSON-LD）

```html
<!-- 幫助 Google 理解你的內容 -->
<script type=""application/ld+json"">
{
    ""@context"": ""https://schema.org"",
    ""@type"": ""Course"",
    ""name"": ""SQL 基礎教學"",
    ""description"": ""從零開始學 SQL"",
    ""provider"": {
        ""@type"": ""Organization"",
        ""name"": ""DevLearn""
    }
}
</script>
```

---

## Favicon

```html
<!-- 瀏覽器分頁小圖示 -->
<link rel=""icon"" type=""image/x-icon"" href=""/favicon.ico"">
<link rel=""icon"" type=""image/png"" sizes=""32x32"" href=""/favicon-32x32.png"">
<link rel=""apple-touch-icon"" sizes=""180x180"" href=""/apple-touch-icon.png"">
```

---

## SEO 最佳實踐

| 項目 | 建議 |
|------|------|
| `<title>` | 50~60 字元，包含關鍵字 |
| `<meta description>` | 120~160 字元，吸引人點擊 |
| `<h1>` | 每頁一個，包含主要關鍵字 |
| `<img alt>` | 描述圖片內容 |
| URL | 簡短、有意義、用連字號分隔 |
| HTTPS | 必要（Google 排名因素） |
| 行動友善 | 必要（Google 手機優先索引） |
| 載入速度 | 越快越好（Core Web Vitals） |
" },

        // ── 1609: 無障礙 ──
        new() { Id=1609, Category="html", Order=10, Level="intermediate", Icon="♿", Title="無障礙網頁（Accessibility）", Slug="html-accessibility", IsPublished=true, Content=@"
# 無障礙網頁（Accessibility / a11y）

## 為什麼無障礙很重要？

> **比喻：無障礙就像建築物的無障礙坡道** ♿
>
> 不只輪椅使用者需要——推嬰兒車、搬重物的人也受益。
> 無障礙網頁不只幫助視障者——也幫助所有人。

---

## ARIA 屬性

```html
<!-- role — 定義元素的角色 -->
<div role=""alert"">操作成功！</div>          <!-- 螢幕閱讀器會立刻朗讀 -->
<div role=""navigation"">...</div>           <!-- 等同 <nav> -->
<div role=""button"" tabindex=""0"">點我</div> <!-- 讓 div 被當作按鈕 -->

<!-- aria-label — 給沒有文字的元素加標籤 -->
<button aria-label=""關閉"">✕</button>       <!-- 螢幕閱讀器會唸「關閉」 -->
<input aria-label=""搜尋"">                   <!-- 沒有 label 時用 aria-label -->

<!-- aria-hidden — 隱藏裝飾性元素 -->
<span aria-hidden=""true"">🎉</span>          <!-- 螢幕閱讀器會忽略 -->

<!-- aria-live — 動態更新的區域 -->
<div aria-live=""polite"">                    <!-- 內容改變時通知使用者 -->
    目前有 5 個通知
</div>

<!-- aria-expanded — 展開/收合狀態 -->
<button aria-expanded=""false""               <!-- 告知選單是收合的 -->
        aria-controls=""menu"">
    選單
</button>
<ul id=""menu"" hidden>...</ul>
```

---

## 鍵盤操作

```html
<!-- tabindex 控制 Tab 鍵順序 -->
<input tabindex=""1"">              <!-- 第一個被 Tab 到 -->
<input tabindex=""2"">              <!-- 第二個 -->
<div tabindex=""0"">可聚焦的 div</div>  <!-- 0 = 照 DOM 順序 -->
<div tabindex=""-1"">程式才能聚焦</div> <!-- -1 = Tab 跳過，JS 可聚焦 -->
```

```javascript
// 自訂元素支援 Enter 和 Space 觸發
document.querySelector('[role=""button""]').addEventListener('keydown', (e) => {
    if (e.key === 'Enter' || e.key === ' ') {
        e.preventDefault();
        // 執行按鈕動作
    }
});
```

---

## 無障礙清單

| 項目 | 做法 |
|------|------|
| 圖片 | 所有 `<img>` 都要有 `alt` |
| 表單 | 每個 input 都要有 `<label>` |
| 色彩 | 對比度至少 4.5:1 |
| 鍵盤 | 所有功能可用鍵盤操作 |
| 語意 | 使用正確的語意標籤 |
| 動態 | 用 `aria-live` 通知更新 |
| 焦點 | 焦點指示器清楚可見 |
| 結構 | 標題層級正確（h1→h2→h3） |

---

## 常見錯誤

```html
<!-- ❌ 用 div 當按鈕 -->
<div onclick=""doSomething()"">點我</div>

<!-- ✅ 用真正的按鈕 -->
<button onclick=""doSomething()"">點我</button>

<!-- ❌ 用顏色作為唯一指示 -->
<span style=""color: red"">錯誤</span>

<!-- ✅ 除了顏色，加上文字或圖示 -->
<span style=""color: red"">❌ 錯誤：請輸入有效的 Email</span>

<!-- ❌ 自動播放有聲音的影片 -->
<video autoplay>

<!-- ✅ 自動播放要靜音 -->
<video autoplay muted>
```
" },

        // ── 1610: HTML5 API ──
        new() { Id=1610, Category="html", Order=11, Level="intermediate", Icon="🔌", Title="HTML5 新功能與 API", Slug="html5-apis", IsPublished=true, Content=@"
# HTML5 新功能與 API

## dialog — 原生對話框

```html
<!-- 不需要 JavaScript 函式庫！ -->
<dialog id=""myDialog"">
    <h2>確認刪除？</h2>
    <p>此操作無法復原。</p>
    <form method=""dialog"">         <!-- ← method=""dialog"" 自動關閉 -->
        <button value=""cancel"">取消</button>
        <button value=""confirm"">確認</button>
    </form>
</dialog>

<button onclick=""myDialog.showModal()"">刪除</button>

<script>
    myDialog.addEventListener(""close"", () => {
        console.log(myDialog.returnValue);  // ""cancel"" 或 ""confirm""
    });
</script>
```

---

## progress 和 meter

```html
<!-- 進度條 -->
<label for=""download"">下載進度：</label>
<progress id=""download"" value=""70"" max=""100"">70%</progress>

<!-- 不確定進度（loading） -->
<progress>載入中...</progress>

<!-- 度量表（非進度） -->
<label for=""disk"">磁碟使用量：</label>
<meter id=""disk"" min=""0"" max=""100""
       low=""25"" high=""75"" optimum=""50""
       value=""80"">80%</meter>
<!-- 80% > high(75%) → 顯示警告色 -->
```

---

## contenteditable — 可編輯內容

```html
<div contenteditable=""true""
     style=""border: 1px solid #ccc; padding: 10px;"">
    點擊這裡可以直接編輯文字！
</div>

<script>
    let editor = document.querySelector('[contenteditable]');
    editor.addEventListener('input', () => {
        console.log('內容：', editor.innerHTML);
    });
</script>
```

---

## Drag and Drop

```html
<div id=""drag-item"" draggable=""true""
     style=""width:100px; height:100px; background:coral;"">
    拖我
</div>

<div id=""drop-zone""
     style=""width:200px; height:200px; border:2px dashed #ccc;"">
    放到這裡
</div>

<script>
    let item = document.getElementById('drag-item');
    let zone = document.getElementById('drop-zone');

    item.addEventListener('dragstart', (e) => {
        e.dataTransfer.setData('text/plain', 'hello');
    });

    zone.addEventListener('dragover', (e) => {
        e.preventDefault();              // ← 必須阻止預設行為
    });

    zone.addEventListener('drop', (e) => {
        e.preventDefault();
        let data = e.dataTransfer.getData('text/plain');
        zone.textContent = `收到: ${data}`;
    });
</script>
```

---

## template — 模板

```html
<!-- template 裡的內容不會顯示也不會執行 -->
<template id=""card-template"">
    <div class=""card"">
        <h3 class=""card-title""></h3>
        <p class=""card-body""></p>
    </div>
</template>

<div id=""container""></div>

<script>
    let template = document.getElementById('card-template');
    let container = document.getElementById('container');

    // 複製模板並填入資料
    let clone = template.content.cloneNode(true);
    clone.querySelector('.card-title').textContent = '標題';
    clone.querySelector('.card-body').textContent = '內容';
    container.appendChild(clone);
</script>
```

---

## Web Components（自訂元素）

```html
<script>
    class MyCard extends HTMLElement {
        connectedCallback() {
            this.innerHTML = `
                <div style=""border:1px solid #ccc; padding:16px; border-radius:8px;"">
                    <h3>${this.getAttribute('title')}</h3>
                    <slot></slot>
                </div>
            `;
        }
    }
    customElements.define('my-card', MyCard);
</script>

<!-- 使用自訂元素 -->
<my-card title=""Hello"">
    <p>這是卡片內容</p>
</my-card>
```
" },

        // ── 1611: CSS 基礎 ──
        new() { Id=1611, Category="html", Order=12, Level="beginner", Icon="🎨", Title="CSS 基礎選擇器與屬性", Slug="html-css-basics", IsPublished=true, Content=@"
# CSS 基礎選擇器與屬性

## 三種引入 CSS 的方式

```html
<!-- 1. 外部樣式（推薦） -->
<link rel=""stylesheet"" href=""style.css"">

<!-- 2. 內部樣式 -->
<style>
    h1 { color: blue; }
</style>

<!-- 3. 行內樣式（盡量避免） -->
<p style=""color: red; font-size: 16px;"">紅色文字</p>
```

---

## 選擇器

```css
/* 元素選擇器 */
p { color: blue; }              /* 所有 <p> */

/* Class 選擇器（可重複使用） */
.highlight { background: yellow; }  /* class=""highlight"" */

/* ID 選擇器（唯一） */
#header { font-size: 24px; }    /* id=""header"" */

/* 後代選擇器 */
nav a { color: white; }         /* nav 裡面的所有 a */

/* 子選擇器（只選直接子元素） */
ul > li { list-style: none; }   /* ul 的直接子 li */

/* 相鄰兄弟選擇器 */
h1 + p { font-size: 18px; }    /* h1 後面的第一個 p */

/* 屬性選擇器 */
input[type=""text""] { border: 1px solid #ccc; }
a[href^=""https""] { color: green; }   /* href 開頭是 https */

/* 偽類 */
a:hover { color: red; }        /* 滑鼠移上去 */
li:first-child { font-weight: bold; }  /* 第一個 li */
li:nth-child(2n) { background: #f0f0f0; }  /* 偶數行 */
input:focus { border-color: blue; }   /* 聚焦時 */

/* 偽元素 */
p::first-line { font-weight: bold; }  /* 第一行 */
p::before { content: '▶ '; }         /* 前面加文字 */
p::after { content: ' ◀'; }          /* 後面加文字 */
```

---

## 常用屬性

```css
/* 文字 */
color: #333;                    /* 文字顏色 */
font-size: 16px;                /* 字體大小 */
font-weight: bold;              /* 粗細：normal, bold, 100~900 */
font-family: 'Noto Sans TC', sans-serif;  /* 字體 */
text-align: center;             /* 對齊：left, center, right */
line-height: 1.6;               /* 行高（1.6 倍字體大小） */
text-decoration: none;          /* 去除底線 */
text-transform: uppercase;      /* 全大寫 */

/* 背景 */
background-color: #f5f5f5;
background-image: url('bg.jpg');
background-size: cover;         /* 填滿 */
background-position: center;

/* 邊框 */
border: 1px solid #ccc;
border-radius: 8px;             /* 圓角 */

/* 大小 */
width: 100%;
max-width: 1200px;
height: auto;
min-height: 100vh;              /* 至少佔滿畫面高度 */

/* 間距 */
padding: 16px;                  /* 內距 */
margin: 0 auto;                 /* 外距（0 上下，auto 左右居中） */
```

---

## Box Model

```
┌─────────── margin ──────────┐
│  ┌──────── border ────────┐  │
│  │  ┌──── padding ─────┐  │  │
│  │  │                   │  │  │
│  │  │    content        │  │  │
│  │  │                   │  │  │
│  │  └───────────────────┘  │  │
│  └─────────────────────────┘  │
└───────────────────────────────┘
```

```css
/* 預設：width = content 寬度 */
/* border-box：width = content + padding + border */
* {
    box-sizing: border-box;     /* ← 推薦全域設定 */
}
```

---

## 優先順序（Specificity）

```
!important > 行內樣式 > ID > Class > 元素
     ∞         1000    100    10      1
```

```css
p { color: blue; }              /* 1 */
.text { color: green; }         /* 10 */
#title { color: red; }          /* 100 */
p.text { color: orange; }       /* 11 (1+10) */
```
" },

        // ── 1612: Flexbox ──
        new() { Id=1612, Category="html", Order=13, Level="beginner", Icon="📐", Title="CSS Flexbox 排版", Slug="html-flexbox", IsPublished=true, Content=@"
# CSS Flexbox 排版

## 什麼是 Flexbox？

> **比喻：Flexbox 就像把物品排在架子上** 📐
>
> 你決定架子是橫的還是直的、物品怎麼對齊、間距怎麼分配。

---

## 啟用 Flexbox

```css
.container {
    display: flex;              /* ← 啟用 Flex 佈局 */
}
```

```html
<div class=""container"">
    <div class=""item"">1</div>
    <div class=""item"">2</div>
    <div class=""item"">3</div>
</div>
```

---

## 容器屬性（父元素）

```css
.container {
    display: flex;

    /* 主軸方向 */
    flex-direction: row;            /* → 水平（預設） */
    flex-direction: row-reverse;    /* ← 水平反轉 */
    flex-direction: column;         /* ↓ 垂直 */
    flex-direction: column-reverse; /* ↑ 垂直反轉 */

    /* 換行 */
    flex-wrap: nowrap;              /* 不換行（預設） */
    flex-wrap: wrap;                /* 換行 */

    /* 主軸對齊 */
    justify-content: flex-start;    /* 靠左（預設） */
    justify-content: flex-end;      /* 靠右 */
    justify-content: center;        /* 置中 */
    justify-content: space-between; /* 兩端對齊，中間平均分配 */
    justify-content: space-around;  /* 每個元素兩側等距 */
    justify-content: space-evenly;  /* 所有間距完全相等 */

    /* 交叉軸對齊 */
    align-items: stretch;           /* 拉伸填滿（預設） */
    align-items: flex-start;        /* 頂部對齊 */
    align-items: flex-end;          /* 底部對齊 */
    align-items: center;            /* 垂直置中 */

    /* 間距 */
    gap: 16px;                      /* 元素之間的間距 */
    gap: 16px 24px;                 /* 列距 欄距 */
}
```

---

## 子元素屬性

```css
.item {
    /* 伸展比例 */
    flex-grow: 1;               /* 佔剩餘空間的比例（0=不伸展） */

    /* 收縮比例 */
    flex-shrink: 0;             /* 空間不足時不縮小 */

    /* 基本大小 */
    flex-basis: 200px;          /* 預設大小（替代 width） */

    /* 簡寫 */
    flex: 1;                    /* = flex-grow:1 flex-shrink:1 flex-basis:0% */
    flex: 0 0 200px;            /* 不伸展、不收縮、固定 200px */

    /* 個別對齊 */
    align-self: center;         /* 覆蓋容器的 align-items */

    /* 排序 */
    order: -1;                  /* 排最前面（預設 0） */
}
```

---

## 常見佈局

```css
/* 水平垂直置中 */
.center {
    display: flex;
    justify-content: center;    /* 水平置中 */
    align-items: center;        /* 垂直置中 */
    height: 100vh;
}

/* 等寬三欄 */
.three-cols {
    display: flex;
    gap: 16px;
}
.three-cols > div {
    flex: 1;                    /* 三個都 flex:1 = 等分 */
}

/* 側邊欄 + 主內容 */
.layout {
    display: flex;
}
.sidebar {
    flex: 0 0 250px;            /* 固定 250px，不伸縮 */
}
.main {
    flex: 1;                    /* 佔剩餘空間 */
}

/* 底部固定 footer */
body {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
}
main {
    flex: 1;                    /* 主內容撐滿 → footer 自然在底部 */
}
```

---

## Navbar 實例

```css
.navbar {
    display: flex;
    justify-content: space-between;  /* Logo 靠左，選單靠右 */
    align-items: center;
    padding: 0 24px;
    height: 60px;
    background: #1a1a2e;
}

.nav-links {
    display: flex;
    gap: 24px;
    list-style: none;
}
```
" },

        // ── 1613: CSS Grid ──
        new() { Id=1613, Category="html", Order=14, Level="intermediate", Icon="🔲", Title="CSS Grid 格線排版", Slug="html-css-grid", IsPublished=true, Content=@"
# CSS Grid 格線排版

## Flexbox vs Grid

| 比較 | Flexbox | Grid |
|------|---------|------|
| 維度 | 一維（列或行） | 二維（列和行同時） |
| 適合 | 導覽列、卡片排列 | 整頁佈局、複雜格線 |
| 方向 | 主軸 + 交叉軸 | 列 + 欄 |

---

## 基本 Grid

```css
.grid-container {
    display: grid;
    grid-template-columns: 200px 200px 200px;  /* 3 欄，各 200px */
    grid-template-rows: 100px 100px;           /* 2 列，各 100px */
    gap: 16px;                                 /* 間距 */
}
```

---

## fr 單位

```css
.grid {
    display: grid;
    /* fr = fraction（比例） */
    grid-template-columns: 1fr 2fr 1fr;  /* 1:2:1 比例 */
    /* 第一欄 25%，第二欄 50%，第三欄 25% */

    /* 混合使用 */
    grid-template-columns: 250px 1fr;    /* 固定 250px + 剩餘空間 */
}
```

---

## repeat 和 auto-fill

```css
.grid {
    /* repeat(次數, 大小) */
    grid-template-columns: repeat(3, 1fr);        /* 3 等分 */
    grid-template-columns: repeat(3, 200px);      /* 3 個 200px */

    /* 自動填滿 — 響應式神器 */
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
    /* 自動計算能放幾欄，每欄最小 250px，最大 1fr */
    /* 畫面寬 → 自動變多欄；畫面窄 → 自動變少欄 */
}
```

---

## 指定子元素位置

```css
.header {
    grid-column: 1 / -1;         /* 從第 1 條線到最後（跨整列） */
}

.sidebar {
    grid-column: 1 / 2;         /* 第 1 欄 */
    grid-row: 2 / 4;            /* 第 2~3 列 */
}

.main {
    grid-column: 2 / 4;         /* 第 2~3 欄 */
}

/* 用 span 更直覺 */
.wide {
    grid-column: span 2;        /* 橫跨 2 欄 */
    grid-row: span 3;           /* 橫跨 3 列 */
}
```

---

## Grid Template Areas

```css
.page {
    display: grid;
    grid-template-areas:
        ""header  header  header""
        ""sidebar main    main""
        ""footer  footer  footer"";
    grid-template-columns: 250px 1fr 1fr;
    grid-template-rows: 60px 1fr 40px;
    min-height: 100vh;
}

.header  { grid-area: header; }
.sidebar { grid-area: sidebar; }
.main    { grid-area: main; }
.footer  { grid-area: footer; }
```

---

## 對齊

```css
.grid {
    /* 整體內容對齊 */
    justify-content: center;     /* 水平置中 */
    align-content: center;       /* 垂直置中 */

    /* 格子內容對齊 */
    justify-items: center;       /* 每個格子水平置中 */
    align-items: center;         /* 每個格子垂直置中 */
    place-items: center;         /* 上面兩個的簡寫 */
}

/* 個別格子 */
.item {
    justify-self: end;           /* 這格靠右 */
    align-self: start;           /* 這格靠上 */
}
```

---

## 響應式佈局實例

```css
/* 卡片自動排列 */
.card-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 24px;
    padding: 24px;
}

/* 聖杯佈局（Holy Grail） */
.holy-grail {
    display: grid;
    grid-template:
        ""header header header"" 60px
        ""nav    main   aside""  1fr
        ""footer footer footer"" 40px
        / 200px  1fr    200px;
    min-height: 100vh;
}

/* 手機版：改成單欄 */
@media (max-width: 768px) {
    .holy-grail {
        grid-template:
            ""header"" 60px
            ""nav""    auto
            ""main""   1fr
            ""aside""  auto
            ""footer"" 40px
            / 1fr;
    }
}
```
" },

        // ── 1614: 響應式設計 ──
        new() { Id=1614, Category="html", Order=15, Level="intermediate", Icon="📱", Title="響應式設計（RWD）", Slug="html-responsive", IsPublished=true, Content=@"
# 響應式設計（Responsive Web Design）

## 什麼是 RWD？

> **比喻：RWD 就像水** 💧
>
> 水倒進杯子就是杯子的形狀，倒進碗就是碗的形狀。
> 響應式網頁會自動適應不同大小的螢幕。

---

## Viewport Meta 標籤

```html
<!-- 必要！沒有這行，手機會用桌面寬度顯示 -->
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
```

---

## Media Query（媒體查詢）

```css
/* 桌面優先（Desktop First） */
.container { width: 1200px; }

@media (max-width: 1024px) {    /* 平板以下 */
    .container { width: 100%; padding: 0 16px; }
}

@media (max-width: 768px) {     /* 手機以下 */
    .container { padding: 0 8px; }
    .sidebar { display: none; }  /* 手機隱藏側邊欄 */
}

/* 手機優先（Mobile First）— 推薦 */
.container { width: 100%; }

@media (min-width: 768px) {     /* 平板以上 */
    .container { max-width: 720px; margin: 0 auto; }
}

@media (min-width: 1024px) {    /* 桌面以上 */
    .container { max-width: 1200px; }
}
```

---

## 常用斷點

| 裝置 | 寬度 | 斷點 |
|------|------|------|
| 手機 | < 768px | `max-width: 767px` |
| 平板 | 768px ~ 1023px | `min-width: 768px` |
| 桌面 | 1024px ~ 1439px | `min-width: 1024px` |
| 大螢幕 | >= 1440px | `min-width: 1440px` |

---

## 響應式圖片

```css
/* 圖片不超過容器 */
img {
    max-width: 100%;
    height: auto;
}
```

```html
<!-- 不同裝置用不同圖片 -->
<picture>
    <source media=""(max-width: 768px)"" srcset=""small.jpg"">
    <source media=""(max-width: 1024px)"" srcset=""medium.jpg"">
    <img src=""large.jpg"" alt=""響應式圖片"">
</picture>
```

---

## 響應式文字

```css
/* 用 clamp() 自動縮放字體 */
h1 {
    font-size: clamp(24px, 5vw, 48px);
    /* 最小 24px，理想 5vw，最大 48px */
}

/* 用 vw 單位 */
.hero-text {
    font-size: 4vw;              /* 畫面寬度的 4% */
}
```

---

## 響應式工具

```css
/* 響應式間距 */
.section {
    padding: clamp(16px, 4vw, 64px);
}

/* 容器查詢（Container Query）— 新功能 */
.card-container {
    container-type: inline-size;
}

@container (min-width: 400px) {
    .card { display: flex; }
}

/* 響應式隱藏 */
.desktop-only { display: none; }
@media (min-width: 1024px) {
    .desktop-only { display: block; }
    .mobile-only { display: none; }
}
```

---

## 完整響應式模板

```css
/* 全域重設 */
* { box-sizing: border-box; margin: 0; padding: 0; }
body { font-family: 'Noto Sans TC', sans-serif; }
img { max-width: 100%; height: auto; }

/* 容器 */
.container {
    width: 100%;
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 16px;
}

/* 卡片格線 */
.cards {
    display: grid;
    grid-template-columns: 1fr;              /* 手機：1 欄 */
    gap: 16px;
}

@media (min-width: 768px) {
    .cards {
        grid-template-columns: repeat(2, 1fr); /* 平板：2 欄 */
    }
}

@media (min-width: 1024px) {
    .cards {
        grid-template-columns: repeat(3, 1fr); /* 桌面：3 欄 */
        gap: 24px;
    }
}

/* Navbar 響應式 */
.nav-links { display: none; }               /* 手機隱藏 */
.hamburger { display: block; }              /* 手機顯示漢堡按鈕 */

@media (min-width: 768px) {
    .nav-links { display: flex; }            /* 平板以上顯示 */
    .hamburger { display: none; }            /* 隱藏漢堡按鈕 */
}
```
" },
    };
}
