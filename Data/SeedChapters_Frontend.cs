using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Frontend
{
    public static List<Chapter> GetChapters()
    {
        return new List<Chapter>
        {
            // ── Chapter 500: HTML 基礎結構與語意標籤 ──
            new Chapter
            {
                Id = 500,
                Title = "HTML 基礎結構與語意標籤",
                Slug = "frontend-html-basics",
                Category = "frontend",
                Order = 1,
                Level = "beginner",
                Icon = "📄",
                IsPublished = true,
                Content = @"# 📄 HTML 基礎結構與語意標籤

## 📌 什麼是 HTML？

> **比喻：HTML 就像房子的骨架結構** 🏗️
>
> 想像你要蓋一棟房子，首先需要的是骨架——鋼筋、梁柱、牆壁的位置。
> HTML 就是網頁的骨架，它決定了「這裡放標題」「那裡放段落」「這邊放圖片」。
> 沒有 HTML，網頁就像一堆散落的建材，無法成形。

HTML（HyperText Markup Language）是網頁的基礎標記語言，每個網頁都是由 HTML 構成的。

---

## 📌 HTML5 文件基本結構

每個 HTML 文件都有固定的結構，就像每棟房子都有地基、牆壁和屋頂。

```html
<!DOCTYPE html> <!-- 告訴瀏覽器這是 HTML5 文件 -->
<html lang=""zh-Hant""> <!-- 根元素，設定語言為繁體中文 -->
<head> <!-- 頭部區塊，放置網頁的設定資訊（不會顯示在畫面上） -->
    <meta charset=""UTF-8""> <!-- 設定字元編碼為 UTF-8，支援中文 -->
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""> <!-- 設定手機版面自適應 -->
    <title>我的第一個網頁</title> <!-- 設定瀏覽器分頁上顯示的標題 -->
    <link rel=""stylesheet"" href=""style.css""> <!-- 引入外部 CSS 樣式檔 -->
</head>
<body> <!-- 身體區塊，放置所有可見的網頁內容 -->
    <h1>歡迎來到我的網站</h1> <!-- 主標題，一個頁面通常只有一個 h1 -->
    <p>這是一個段落。</p> <!-- 段落標籤 -->
    <script src=""app.js""></script> <!-- 引入外部 JavaScript 檔案，放在 body 結尾處 -->
</body>
</html>
```

### 重要觀念

```html
<!-- head 裡常見的 meta 標籤 -->
<meta name=""description"" content=""這是網頁描述""> <!-- 搜尋引擎會顯示這段描述 -->
<meta name=""keywords"" content=""HTML, CSS, 教學""> <!-- 設定搜尋關鍵字（SEO 用途已降低） -->
<meta name=""author"" content=""你的名字""> <!-- 設定網頁作者 -->
<meta http-equiv=""X-UA-Compatible"" content=""IE=edge""> <!-- 告訴 IE 用最新渲染引擎 -->
```

---

## 📌 語意標籤（Semantic Tags）

語意標籤讓瀏覽器和搜尋引擎能「理解」網頁結構，就像幫房間貼上門牌一樣。

```html
<!-- 完整的語意標籤範例 -->
<body> <!-- 整個網頁的可見內容 -->

    <header> <!-- 頁首區塊：通常放 Logo 和主導覽列 -->
        <h1>我的部落格</h1> <!-- 網站主標題 -->
        <nav> <!-- 導覽區塊：放置導覽連結 -->
            <ul> <!-- 無序列表，用來組織導覽項目 -->
                <li><a href=""/"">首頁</a></li> <!-- 列表項目 + 超連結 -->
                <li><a href=""/about"">關於我</a></li> <!-- 導覽到「關於我」頁面 -->
                <li><a href=""/contact"">聯絡我</a></li> <!-- 導覽到「聯絡我」頁面 -->
            </ul>
        </nav>
    </header>

    <main> <!-- 主要內容區塊：一個頁面只能有一個 main -->
        <section> <!-- 區段：將相關內容分組 -->
            <h2>最新文章</h2> <!-- 區段標題 -->
            <article> <!-- 文章區塊：獨立的內容單元 -->
                <h3>學習 HTML 的第一天</h3> <!-- 文章標題 -->
                <time datetime=""2024-01-15"">2024 年 1 月 15 日</time> <!-- 時間標籤，datetime 給機器讀 -->
                <p>今天開始學習 HTML...</p> <!-- 文章內容 -->
            </article>
            <article> <!-- 另一篇獨立文章 -->
                <h3>CSS 入門心得</h3> <!-- 第二篇文章標題 -->
                <p>CSS 真的很有趣...</p> <!-- 第二篇文章內容 -->
            </article>
        </section>

        <aside> <!-- 側邊欄：放置次要的輔助內容 -->
            <h3>熱門標籤</h3> <!-- 側邊欄標題 -->
            <ul> <!-- 標籤列表 -->
                <li>HTML</li> <!-- 標籤項目 -->
                <li>CSS</li> <!-- 標籤項目 -->
            </ul>
        </aside>
    </main>

    <footer> <!-- 頁尾區塊：放置版權、聯絡資訊 -->
        <p>&copy; 2024 我的部落格</p> <!-- 版權符號 + 文字 -->
    </footer>

</body>
```

---

## 📌 表單元素（Form Elements）

表單是使用者與網站互動的橋樑，就像房子裡的門鈴、信箱和對講機。

```html
<form action=""/api/register"" method=""POST""> <!-- 表單標籤，設定送出的目標和方法 -->

    <!-- 文字輸入 -->
    <label for=""username"">使用者名稱：</label> <!-- label 綁定到 input，點標籤也能聚焦 -->
    <input type=""text"" id=""username"" name=""username"" placeholder=""請輸入帳號"" required> <!-- 文字輸入框，required 表示必填 -->

    <!-- 電子信箱 -->
    <label for=""email"">電子信箱：</label> <!-- 信箱欄位的標籤 -->
    <input type=""email"" id=""email"" name=""email"" placeholder=""example@mail.com""> <!-- email 類型會自動驗證格式 -->

    <!-- 密碼 -->
    <label for=""password"">密碼：</label> <!-- 密碼欄位的標籤 -->
    <input type=""password"" id=""password"" name=""password"" minlength=""8""> <!-- password 類型會隱藏輸入內容 -->

    <!-- 數字 -->
    <label for=""age"">年齡：</label> <!-- 年齡欄位的標籤 -->
    <input type=""number"" id=""age"" name=""age"" min=""1"" max=""120""> <!-- number 類型有上下箭頭，可限制範圍 -->

    <!-- 日期 -->
    <label for=""birthday"">生日：</label> <!-- 生日欄位的標籤 -->
    <input type=""date"" id=""birthday"" name=""birthday""> <!-- date 類型會顯示日期選擇器 -->

    <!-- 下拉選單 -->
    <label for=""city"">城市：</label> <!-- 下拉選單的標籤 -->
    <select id=""city"" name=""city""> <!-- 下拉選單標籤 -->
        <option value="""">請選擇城市</option> <!-- 預設選項，value 為空 -->
        <option value=""taipei"">台北</option> <!-- 選項：台北 -->
        <option value=""taichung"">台中</option> <!-- 選項：台中 -->
        <option value=""kaohsiung"">高雄</option> <!-- 選項：高雄 -->
    </select>

    <!-- 多行文字 -->
    <label for=""bio"">自我介紹：</label> <!-- 多行文字的標籤 -->
    <textarea id=""bio"" name=""bio"" rows=""4"" cols=""50"" placeholder=""請簡述自己...""></textarea> <!-- 多行文字框，rows 設行數 -->

    <!-- 核取方塊 -->
    <label> <!-- label 包住 input 也能達到綁定效果 -->
        <input type=""checkbox"" name=""agree"" required> <!-- 核取方塊，required 表示必須勾選 -->
        我同意服務條款
    </label>

    <!-- 單選按鈕 -->
    <fieldset> <!-- 用 fieldset 將相關的單選按鈕分組 -->
        <legend>性別：</legend> <!-- legend 是 fieldset 的標題 -->
        <label><input type=""radio"" name=""gender"" value=""male""> 男</label> <!-- 同一組 radio 用相同 name -->
        <label><input type=""radio"" name=""gender"" value=""female""> 女</label> <!-- 同 name 的 radio 只能選一個 -->
        <label><input type=""radio"" name=""gender"" value=""other""> 其他</label> <!-- 第三個選項 -->
    </fieldset>

    <button type=""submit"">送出表單</button> <!-- 送出按鈕，觸發 form 的 action -->
    <button type=""reset"">清除</button> <!-- 重設按鈕，清除所有填寫內容 -->

</form>
```

---

## 📌 表格（Table）

表格適合呈現結構化資料，就像 Excel 試算表。

```html
<table border=""1""> <!-- 表格標籤，border 設定邊框（實務中用 CSS） -->
    <caption>學生成績表</caption> <!-- 表格標題，會顯示在表格上方 -->
    <thead> <!-- 表頭區塊：定義欄位標題 -->
        <tr> <!-- 表格列（table row） -->
            <th>姓名</th> <!-- 表頭儲存格（會粗體置中） -->
            <th>國文</th> <!-- 第二個欄位標題 -->
            <th>數學</th> <!-- 第三個欄位標題 -->
            <th>英文</th> <!-- 第四個欄位標題 -->
        </tr>
    </thead>
    <tbody> <!-- 表格主體：放置資料列 -->
        <tr> <!-- 第一列資料 -->
            <td>小明</td> <!-- 一般儲存格（table data） -->
            <td>85</td> <!-- 小明的國文成績 -->
            <td>92</td> <!-- 小明的數學成績 -->
            <td>78</td> <!-- 小明的英文成績 -->
        </tr>
        <tr> <!-- 第二列資料 -->
            <td>小華</td> <!-- 第二位學生 -->
            <td>90</td> <!-- 小華的國文成績 -->
            <td>88</td> <!-- 小華的數學成績 -->
            <td>95</td> <!-- 小華的英文成績 -->
        </tr>
    </tbody>
    <tfoot> <!-- 表格頁尾：通常放合計或平均 -->
        <tr> <!-- 頁尾列 -->
            <td>平均</td> <!-- 標示這列是平均 -->
            <td>87.5</td> <!-- 國文平均 -->
            <td>90</td> <!-- 數學平均 -->
            <td>86.5</td> <!-- 英文平均 -->
        </tr>
    </tfoot>
</table>
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：忘記關閉標籤

```html
<!-- 錯誤：<p> 沒有關閉，會導致後面的排版全部亂掉 -->
<p>這是第一段
<p>這是第二段</p> <!-- 瀏覽器會猜測結構，結果不可預期 -->

<!-- 正確：每個開啟標籤都要有對應的關閉標籤 -->
<p>這是第一段</p> <!-- 正確關閉段落標籤 -->
<p>這是第二段</p> <!-- 正確關閉段落標籤 -->
```

### ❌ 錯誤 2：label 的 for 和 input 的 id 不對應

```html
<!-- 錯誤：for=""name"" 但 input 的 id=""username""，點標籤不會聚焦到輸入框 -->
<label for=""name"">姓名：</label> <!-- for 寫的是 name -->
<input type=""text"" id=""username""> <!-- id 卻是 username，兩者不匹配 -->

<!-- 正確：for 和 id 必須完全一致 -->
<label for=""username"">姓名：</label> <!-- for 改成 username -->
<input type=""text"" id=""username""> <!-- 現在 for 和 id 一致了 -->
```

### ❌ 錯誤 3：在 main 外面放主要內容

```html
<!-- 錯誤：主要內容放在 main 外面，不符合語意結構 -->
<body> <!-- body 裡應該用語意標籤組織內容 -->
    <div>這是我的文章內容</div> <!-- 用 div 沒有語意，搜尋引擎不知道這是什麼 -->
</body>

<!-- 正確：用語意標籤包裹主要內容 -->
<body> <!-- body 裡正確使用語意標籤 -->
    <main> <!-- main 告訴瀏覽器這是主要內容區 -->
        <article> <!-- article 表示這是一篇獨立的文章 -->
            <h1>這是我的文章內容</h1> <!-- 用標題標籤而非純文字 -->
            <p>文章正文...</p> <!-- 段落用 p 標籤 -->
        </article>
    </main>
</body>
```

---

## 📌 給 .NET 開發者的提醒

如果你在 ASP.NET Core 的 Razor 頁面中使用 HTML：

```html
<!-- 在 Razor 頁面中，HTML 和 C# 可以混合使用 -->
<h1>@Model.Title</h1> <!-- @Model 是 Razor 語法，會被伺服器端取代為實際值 -->
<p>歡迎，@User.Identity.Name</p> <!-- @User 取得目前登入使用者的名稱 -->

<!-- 表單會搭配 Tag Helper 使用 -->
<form asp-action=""Create"" asp-controller=""Product""> <!-- asp-action 指定 Controller 的方法 -->
    <input asp-for=""Name"" /> <!-- asp-for 自動綁定 Model 的屬性 -->
    <span asp-validation-for=""Name""></span> <!-- 自動顯示驗證錯誤訊息 -->
    <button type=""submit"">建立</button> <!-- 送出按鈕 -->
</form>
```
"
            },

            // ── Chapter 501: CSS 樣式與排版 ──
            new Chapter
            {
                Id = 501,
                Title = "CSS 樣式與排版",
                Slug = "frontend-css-styling",
                Category = "frontend",
                Order = 2,
                Level = "beginner",
                Icon = "🎨",
                IsPublished = true,
                Content = @"# 🎨 CSS 樣式與排版

## 📌 什麼是 CSS？

> **比喻：CSS 就像房子的裝潢設計** 🎨
>
> 如果 HTML 是房子的骨架（鋼筋水泥），那 CSS 就是室內設計師。
> CSS 決定了牆壁要什麼顏色、家具怎麼擺放、窗簾用什麼材質。
> 同一棟房子（HTML），換一套裝潢（CSS）就能變成完全不同的風格。

CSS（Cascading Style Sheets）用來控制 HTML 元素的外觀和排版方式。

---

## 📌 CSS 選擇器（Selectors）

選擇器決定了「你要裝潢哪個房間」。

```css
/* 元素選擇器：選中所有 p 標籤 */
p {
    color: #333; /* 設定文字顏色為深灰色 */
    line-height: 1.6; /* 設定行高為字體大小的 1.6 倍 */
}

/* Class 選擇器：用 . 開頭，可重複使用在多個元素上 */
.highlight {
    background-color: yellow; /* 設定背景顏色為黃色 */
    padding: 4px 8px; /* 設定內距：上下 4px，左右 8px */
}

/* ID 選擇器：用 # 開頭，一個頁面中每個 ID 只能出現一次 */
#main-title {
    font-size: 2rem; /* 設定字體大小為 2rem（相對於根元素） */
    font-weight: bold; /* 設定字體粗細為粗體 */
}

/* 後代選擇器：選中 nav 裡面所有的 a 標籤 */
nav a {
    text-decoration: none; /* 移除底線 */
    color: #0066cc; /* 設定連結顏色為藍色 */
}

/* 子元素選擇器：只選中 ul 的「直接」子元素 li */
ul > li {
    list-style: disc; /* 設定列表樣式為實心圓點 */
    margin-bottom: 8px; /* 設定下方外距為 8px */
}

/* 相鄰兄弟選擇器：選中緊跟在 h2 後面的第一個 p */
h2 + p {
    font-size: 1.1rem; /* 第一段比其他段落稍大 */
    color: #555; /* 設定為較淺的灰色 */
}

/* 偽類選擇器：滑鼠懸停時的樣式 */
a:hover {
    color: #ff6600; /* 懸停時連結變成橘色 */
    text-decoration: underline; /* 懸停時顯示底線 */
}

/* 偽類選擇器：聚焦時的樣式（鍵盤 Tab 或點擊輸入框） */
input:focus {
    border-color: #0066cc; /* 聚焦時邊框變藍色 */
    outline: 2px solid rgba(0, 102, 204, 0.3); /* 加上半透明的外框 */
}

/* 偽元素選擇器：在元素「之前」插入內容 */
.required::before {
    content: ""* ""; /* 在必填欄位前面加上星號 */
    color: red; /* 星號顯示為紅色 */
}

/* 屬性選擇器：選中所有 type=""email"" 的 input */
input[type=""email""] {
    width: 100%; /* 寬度佔滿父容器 */
    padding: 8px; /* 內距 8px */
}

/* nth-child 選擇器：選中奇數列（表格斑馬紋） */
tr:nth-child(odd) {
    background-color: #f9f9f9; /* 奇數列背景設為淺灰色 */
}
```

---

## 📌 Box Model（盒模型）

> 每個 HTML 元素都是一個「盒子」，就像每個房間都有牆壁、地板、傢俱的空間配置。

```css
/* Box Model 完整示範 */
.card {
    /* Content：內容區域，放文字和圖片的空間 */
    width: 300px; /* 設定內容寬度為 300px */
    height: auto; /* 高度自動根據內容調整 */

    /* Padding：內距，內容和邊框之間的空間（像房間裡牆壁到傢俱的距離） */
    padding: 20px; /* 四個方向都是 20px */
    padding-top: 10px; /* 也可以單獨設定某一邊的內距 */

    /* Border：邊框，盒子的外框（像房間的牆壁） */
    border: 1px solid #ddd; /* 1px 寬、實線、淺灰色邊框 */
    border-radius: 8px; /* 設定圓角半徑為 8px */

    /* Margin：外距，盒子和其他盒子之間的空間（像房間和房間之間的走廊） */
    margin: 16px; /* 四個方向都是 16px */
    margin-bottom: 24px; /* 下方外距設大一點 */

    /* box-sizing 很重要！ */
    box-sizing: border-box; /* 讓 width 包含 padding 和 border，計算更直覺 */
}

/* 建議全域設定 box-sizing */
*, *::before, *::after {
    box-sizing: border-box; /* 所有元素都用 border-box，避免計算寬度時出錯 */
}
```

---

## 📌 Flexbox 完整指南

> Flexbox 就像一條彈性的置物架，可以自動調整裡面物品的排列方式。

```css
/* === 容器屬性（Container Properties） === */

.flex-container {
    display: flex; /* 啟用 Flexbox 排版模式 */
    flex-direction: row; /* 主軸方向：row（水平）| column（垂直） */
    flex-wrap: wrap; /* 允許子元素換行（預設 nowrap 不換行） */
    justify-content: space-between; /* 主軸對齊：均勻分布，頭尾貼邊 */
    align-items: center; /* 交叉軸對齊：垂直置中 */
    gap: 16px; /* 子元素之間的間距 */
}

/* justify-content 常用值 */
.demo-justify {
    justify-content: flex-start; /* 靠主軸起點（預設值） */
    justify-content: flex-end; /* 靠主軸終點 */
    justify-content: center; /* 主軸置中 */
    justify-content: space-between; /* 均勻分布，第一和最後一個貼邊 */
    justify-content: space-around; /* 均勻分布，兩側有半個間距 */
    justify-content: space-evenly; /* 完全均勻分布 */
}

/* align-items 常用值 */
.demo-align {
    align-items: stretch; /* 拉伸填滿交叉軸（預設值） */
    align-items: flex-start; /* 靠交叉軸起點（頂部） */
    align-items: flex-end; /* 靠交叉軸終點（底部） */
    align-items: center; /* 交叉軸置中 */
    align-items: baseline; /* 以文字基線對齊 */
}

/* === 子元素屬性（Item Properties） === */

.flex-item {
    flex-grow: 1; /* 允許元素放大，數字越大佔越多空間 */
    flex-shrink: 0; /* 不允許元素縮小（0 表示不縮小） */
    flex-basis: 200px; /* 元素的初始大小 */
    flex: 1 0 200px; /* 簡寫：grow shrink basis */
    align-self: flex-end; /* 覆蓋容器的 align-items，單獨設定對齊 */
    order: 2; /* 改變排列順序，數字越小越前面（預設 0） */
}

/* 實用範例：導覽列 */
.navbar {
    display: flex; /* 啟用 Flex 排版 */
    justify-content: space-between; /* Logo 和選單分散在兩端 */
    align-items: center; /* 垂直置中 */
    padding: 0 24px; /* 左右內距 24px */
    background-color: #1a1a2e; /* 深色背景 */
    height: 60px; /* 固定高度 60px */
}

/* 實用範例：卡片網格 */
.card-grid {
    display: flex; /* 啟用 Flex 排版 */
    flex-wrap: wrap; /* 允許卡片換行 */
    gap: 20px; /* 卡片之間的間距 */
}

.card-grid .card {
    flex: 1 1 300px; /* 可放大、可縮小、基礎寬度 300px */
    max-width: 400px; /* 限制最大寬度 */
}

/* 實用範例：垂直水平完美置中 */
.center-content {
    display: flex; /* 啟用 Flex 排版 */
    justify-content: center; /* 水平置中 */
    align-items: center; /* 垂直置中 */
    min-height: 100vh; /* 最小高度佔滿整個視窗 */
}
```

---

## 📌 CSS Grid 基礎

```css
/* Grid 容器設定 */
.grid-container {
    display: grid; /* 啟用 Grid 排版模式 */
    grid-template-columns: repeat(3, 1fr); /* 建立 3 欄，每欄等寬（1fr = 1 份） */
    grid-template-rows: auto; /* 列高自動根據內容調整 */
    gap: 20px; /* 網格之間的間距 */
    padding: 20px; /* 容器內距 */
}

/* 讓某個子元素跨多欄 */
.full-width {
    grid-column: 1 / -1; /* 從第 1 條線到最後一條線，佔滿所有欄 */
}

/* 讓某個子元素佔兩欄 */
.span-two {
    grid-column: span 2; /* 跨越 2 個欄位 */
}

/* 經典聖杯佈局 */
.holy-grail {
    display: grid; /* 啟用 Grid */
    grid-template-areas: /* 用命名區域定義版面 */
        ""header header header"" /* 第一列：header 橫跨全部 */
        ""nav    main   aside"" /* 第二列：左側導覽、中間內容、右側邊欄 */
        ""footer footer footer""; /* 第三列：footer 橫跨全部 */
    grid-template-columns: 200px 1fr 200px; /* 三欄：左 200px、中間彈性、右 200px */
    grid-template-rows: 60px 1fr 40px; /* 三列：頂 60px、中間彈性、底 40px */
    min-height: 100vh; /* 最小高度佔滿視窗 */
}

/* 指定子元素到對應區域 */
.holy-grail header { grid-area: header; } /* header 放到 header 區域 */
.holy-grail nav { grid-area: nav; } /* nav 放到 nav 區域 */
.holy-grail main { grid-area: main; } /* main 放到 main 區域 */
.holy-grail aside { grid-area: aside; } /* aside 放到 aside 區域 */
.holy-grail footer { grid-area: footer; } /* footer 放到 footer 區域 */
```

---

## 📌 響應式設計（Responsive Design）

```css
/* 手機優先（Mobile First）：先寫手機樣式，再用 media query 擴展 */
.container {
    width: 100%; /* 手機上佔滿寬度 */
    padding: 0 16px; /* 左右留 16px 的呼吸空間 */
}

/* 平板以上（768px 以上）*/
@media (min-width: 768px) { /* 當螢幕寬度 >= 768px 時套用 */
    .container {
        max-width: 720px; /* 限制最大寬度 */
        margin: 0 auto; /* 水平置中 */
    }
    .grid-container {
        grid-template-columns: repeat(2, 1fr); /* 平板上顯示 2 欄 */
    }
}

/* 桌機以上（1024px 以上）*/
@media (min-width: 1024px) { /* 當螢幕寬度 >= 1024px 時套用 */
    .container {
        max-width: 960px; /* 桌機最大寬度 960px */
    }
    .grid-container {
        grid-template-columns: repeat(3, 1fr); /* 桌機上顯示 3 欄 */
    }
}

/* rem 和 em 的差別 */
html {
    font-size: 16px; /* 根元素字體大小（1rem = 16px） */
}

.title {
    font-size: 2rem; /* 2 × 16px = 32px，基於根元素計算 */
    margin-bottom: 1rem; /* 1 × 16px = 16px */
}

.subtitle {
    font-size: 1.5em; /* 1.5 × 父元素字體大小，基於父元素計算 */
}
```

---

## 📌 CSS 變數（Custom Properties）

```css
/* 在 :root 定義全域變數（像是設計系統的調色盤） */
:root {
    --primary-color: #0066cc; /* 主色：藍色 */
    --secondary-color: #ff6600; /* 輔助色：橘色 */
    --text-color: #333333; /* 文字顏色：深灰 */
    --bg-color: #ffffff; /* 背景顏色：白色 */
    --spacing-sm: 8px; /* 小間距 */
    --spacing-md: 16px; /* 中間距 */
    --spacing-lg: 24px; /* 大間距 */
    --border-radius: 8px; /* 統一圓角大小 */
    --font-family: 'Noto Sans TC', sans-serif; /* 設定字體為思源黑體 */
}

/* 使用 var() 引用變數 */
.button {
    background-color: var(--primary-color); /* 使用主色作為按鈕背景 */
    color: white; /* 按鈕文字為白色 */
    padding: var(--spacing-sm) var(--spacing-md); /* 使用間距變數 */
    border: none; /* 移除邊框 */
    border-radius: var(--border-radius); /* 使用統一圓角 */
    font-family: var(--font-family); /* 使用統一字體 */
    cursor: pointer; /* 滑鼠游標變成手指 */
}

/* 深色模式：只需要修改變數值 */
@media (prefers-color-scheme: dark) { /* 偵測系統深色模式 */
    :root {
        --text-color: #e0e0e0; /* 深色模式：文字改為淺灰 */
        --bg-color: #1a1a1a; /* 深色模式：背景改為深色 */
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：忘記設定 box-sizing 導致寬度計算錯誤

```css
/* 錯誤：width 300px + padding 20px×2 + border 1px×2 = 實際佔 342px */
.card {
    width: 300px; /* 以為寬度是 300px */
    padding: 20px; /* 但 padding 會額外增加寬度 */
    border: 1px solid #ccc; /* border 也會額外增加寬度 */
}

/* 正確：加上 box-sizing 讓 width 包含 padding 和 border */
.card {
    width: 300px; /* 設定寬度為 300px */
    padding: 20px; /* padding 包含在 300px 裡面 */
    border: 1px solid #ccc; /* border 也包含在 300px 裡面 */
    box-sizing: border-box; /* 重點！讓 300px 是最終寬度 */
}
```

### ❌ 錯誤 2：margin 塌陷（Margin Collapse）

```css
/* 錯誤：以為上下兩個元素的間距是 40px + 30px = 70px */
.box-a {
    margin-bottom: 40px; /* 下方外距 40px */
}
.box-b {
    margin-top: 30px; /* 上方外距 30px */
}
/* 實際間距只有 40px（取較大值），這就是 margin 塌陷 */

/* 解法：只在一個方向設定 margin */
.box-a {
    margin-bottom: 40px; /* 統一只用 margin-bottom */
}
.box-b {
    margin-top: 0; /* 不設定 margin-top，避免塌陷困擾 */
}
```

### ❌ 錯誤 3：Flexbox 子元素不換行導致擠壓

```css
/* 錯誤：沒有設定 flex-wrap，子元素擠在一行被壓扁 */
.container {
    display: flex; /* 啟用 Flex 但預設不換行 */
}
.item {
    width: 300px; /* 設定了寬度，但會被壓縮 */
}

/* 正確：設定 flex-wrap 允許換行 */
.container {
    display: flex; /* 啟用 Flex 排版 */
    flex-wrap: wrap; /* 重點！允許子元素換行 */
    gap: 16px; /* 設定間距 */
}
.item {
    flex: 1 1 300px; /* 可放大、可縮小、基礎寬度 300px */
}
```
"
            },

            // ── Chapter 502: JavaScript 核心概念 ──
            new Chapter
            {
                Id = 502,
                Title = "JavaScript 核心概念",
                Slug = "frontend-javascript-core",
                Category = "frontend",
                Order = 3,
                Level = "intermediate",
                Icon = "⚡",
                IsPublished = true,
                Content = @"# ⚡ JavaScript 核心概念

## 📌 什麼是 JavaScript？

> **比喻：JavaScript 是房子的水電和自動化系統** 💡
>
> HTML 是骨架，CSS 是裝潢，而 JavaScript 就是讓房子「活起來」的系統。
> 它控制電燈開關（事件處理）、自動門（互動效果）、中央空調（狀態管理）。
> 沒有 JavaScript，網頁就像一棟沒有水電的樣品屋——好看但不能住。

---

## 📌 變數宣告：var / let / const

```javascript
// var：函式作用域，會被提升（hoisting），現代開發中盡量避免使用
var oldWay = ""我是 var""; // 用 var 宣告的變數，作用範圍是整個函式

// let：區塊作用域，可重新賦值，適合會變動的值
let counter = 0; // 用 let 宣告計數器，之後可以改變它的值
counter = 1; // 合法：let 允許重新賦值

// const：區塊作用域，不可重新賦值，適合常數和物件參考
const PI = 3.14159; // 用 const 宣告常數，之後不能改變
// PI = 3; // 錯誤！const 不允許重新賦值

// 但注意！const 的物件內容可以修改
const user = { name: ""小明"" }; // const 鎖住的是「參考」，不是「內容」
user.name = ""小華""; // 合法：修改物件的屬性，不是修改參考
// user = {}; // 錯誤！不能把 user 指向新的物件

// var 的提升陷阱
console.log(hoisted); // 不會報錯，但值是 undefined（被提升了）
var hoisted = ""hello""; // var 宣告會被移到作用域頂端，但賦值不會

// let 不會被提升（嚴格來說有，但在暫時性死區 TDZ）
// console.log(notHoisted); // 報錯！ReferenceError
let notHoisted = ""world""; // let 在宣告前不能使用
```

---

## 📌 資料型別與型別轉換陷阱

```javascript
// JavaScript 有 7 種原始型別
let str = ""文字""; // string：字串
let num = 42; // number：數字（整數和浮點數都是 number）
let bool = true; // boolean：布林值（true 或 false）
let nothing = null; // null：刻意設定的「空值」
let notDefined = undefined; // undefined：尚未賦值的變數
let bigNum = 9007199254740991n; // bigint：超大整數
let sym = Symbol(""id""); // symbol：唯一識別符

// 型別轉換陷阱（JavaScript 最讓人崩潰的部分）
console.log(""5"" + 3); // ""53""：字串 + 數字 → 字串串接（不是加法！）
console.log(""5"" - 3); // 2：字串 - 數字 → 自動轉成數字做減法
console.log(""5"" * ""3""); // 15：兩個字串相乘 → 自動轉成數字
console.log(true + 1); // 2：true 被轉成 1，加上 1 等於 2
console.log(false + ""hello""); // ""falsehello""：false 轉成字串再串接
console.log(null + 1); // 1：null 被轉成 0，加上 1 等於 1
console.log(undefined + 1); // NaN：undefined 無法轉成有效數字

// == vs ===（建議永遠用 ===）
console.log(0 == false); // true：== 會做型別轉換後比較
console.log(0 === false); // false：=== 不做型別轉換，型別不同直接 false
console.log("""" == false); // true：空字串和 false 在 == 下相等
console.log("""" === false); // false：嚴格比較，型別不同
console.log(null == undefined); // true：這是 == 的特例
console.log(null === undefined); // false：嚴格比較，型別不同
```

---

## 📌 函式（Functions）

```javascript
// 函式宣告（Function Declaration）：會被提升，可以在宣告前呼叫
function greet(name) { // 宣告一個名為 greet 的函式，接受 name 參數
    return `你好，${name}！`; // 用模板字串回傳問候語
}

// 函式表達式（Function Expression）：不會被提升
const add = function(a, b) { // 將匿名函式賦值給 add 變數
    return a + b; // 回傳兩數相加的結果
};

// 箭頭函式（Arrow Function）：更簡潔的語法
const multiply = (a, b) => { // 箭頭函式用 => 取代 function 關鍵字
    return a * b; // 回傳兩數相乘的結果
};

// 箭頭函式的簡寫：只有一行時可以省略大括號和 return
const double = x => x * 2; // 單一參數可省略括號，單一表達式可省略 return

// 預設參數
const createUser = (name, role = ""member"") => { // role 預設為 ""member""
    return { name, role }; // 簡寫語法：屬性名稱和變數名稱相同時可省略
};

console.log(createUser(""小明"")); // { name: ""小明"", role: ""member"" }（使用預設值）
console.log(createUser(""小華"", ""admin"")); // { name: ""小華"", role: ""admin"" }（覆蓋預設值）

// 解構參數（在處理 API 回應時很常用）
const displayUser = ({ name, age, email = ""未提供"" }) => { // 直接解構物件參數
    console.log(`姓名：${name}，年齡：${age}，信箱：${email}`); // 使用解構出的變數
};

displayUser({ name: ""小明"", age: 25 }); // 信箱會用預設值「未提供」
```

---

## 📌 閉包（Closure）與作用域

```javascript
// 閉包：內層函式可以存取外層函式的變數，即使外層函式已經執行完畢
function createCounter() { // 外層函式：建立計數器
    let count = 0; // 這個變數被「封閉」在閉包裡
    return {
        increment: () => ++count, // 內層函式：可以存取外層的 count
        decrement: () => --count, // 內層函式：也可以存取同一個 count
        getCount: () => count // 內層函式：讀取 count 的值
    };
}

const counter = createCounter(); // 建立一個計數器實例
console.log(counter.increment()); // 1：count 從 0 變成 1
console.log(counter.increment()); // 2：count 從 1 變成 2
console.log(counter.getCount()); // 2：讀取目前的 count 值
// console.log(count); // 錯誤！外部無法直接存取 count，實現了封裝

// 閉包的經典陷阱：迴圈中的 var
for (var i = 0; i < 3; i++) { // var 是函式作用域，迴圈結束後 i = 3
    setTimeout(() => console.log(i), 100); // 全部印出 3，因為共用同一個 i
}

// 解法：用 let 取代 var
for (let j = 0; j < 3; j++) { // let 是區塊作用域，每次迴圈有自己的 j
    setTimeout(() => console.log(j), 100); // 正確印出 0, 1, 2
}
```

---

## 📌 Promise 與 async/await

```javascript
// Promise：代表一個非同步操作的最終結果
const fetchData = (url) => { // 建立一個回傳 Promise 的函式
    return new Promise((resolve, reject) => { // Promise 接受 resolve 和 reject
        setTimeout(() => { // 模擬網路請求的延遲
            if (url) {
                resolve({ data: ""取得成功"" }); // 成功時呼叫 resolve
            } else {
                reject(new Error(""URL 不能為空"")); // 失敗時呼叫 reject
            }
        }, 1000); // 延遲 1 秒
    });
};

// 用 .then() / .catch() 處理 Promise
fetchData(""/api/users"") // 呼叫函式，取得 Promise
    .then(result => console.log(result)) // 成功時執行：印出結果
    .catch(error => console.error(error)) // 失敗時執行：印出錯誤
    .finally(() => console.log(""請求結束"")); // 不論成敗都會執行

// async/await：更直覺的非同步寫法（語法糖）
async function loadUserData() { // async 標記這是一個非同步函式
    try { // 用 try/catch 取代 .then()/.catch()
        const response = await fetch(""/api/users""); // await 等待 Promise 完成
        if (!response.ok) { // 檢查 HTTP 狀態碼
            throw new Error(`HTTP 錯誤：${response.status}`); // 手動拋出錯誤
        }
        const data = await response.json(); // 等待 JSON 解析完成
        console.log(data); // 印出解析後的資料
        return data; // 回傳資料（自動包裝成 Promise）
    } catch (error) { // 捕捉任何錯誤（網路錯誤或程式錯誤）
        console.error(""載入失敗："", error.message); // 印出錯誤訊息
    }
}

// 平行執行多個 Promise
async function loadDashboard() { // 同時載入多個 API 資料
    try {
        const [users, products, orders] = await Promise.all([ // 平行送出三個請求
            fetch(""/api/users"").then(r => r.json()), // 第一個請求：取得使用者
            fetch(""/api/products"").then(r => r.json()), // 第二個請求：取得產品
            fetch(""/api/orders"").then(r => r.json()) // 第三個請求：取得訂單
        ]); // Promise.all 等待全部完成
        console.log(""全部載入完成"", { users, products, orders }); // 印出所有結果
    } catch (error) {
        console.error(""其中一個請求失敗："", error); // 任一失敗就會進入 catch
    }
}
```

---

## 📌 事件處理

```javascript
// 取得 DOM 元素
const button = document.querySelector(""#myButton""); // 用 CSS 選擇器取得按鈕

// 基本事件監聽
button.addEventListener(""click"", function(event) { // 監聽點擊事件
    console.log(""按鈕被點擊了！""); // 印出訊息
    console.log(""事件目標："", event.target); // event.target 是觸發事件的元素
});

// 事件冒泡（Event Bubbling）：事件從子元素向上傳遞到父元素
document.querySelector("".parent"").addEventListener(""click"", () => { // 父元素監聽
    console.log(""父元素被點擊""); // 點擊子元素時，這裡也會觸發
});

document.querySelector("".child"").addEventListener(""click"", (e) => { // 子元素監聽
    console.log(""子元素被點擊""); // 先觸發子元素的事件
    e.stopPropagation(); // 阻止事件繼續向上冒泡到父元素
});

// 事件委託（Event Delegation）：在父元素上統一處理子元素的事件
document.querySelector(""#todoList"").addEventListener(""click"", (e) => { // 在列表上監聽
    if (e.target.classList.contains(""delete-btn"")) { // 判斷點擊的是否為刪除按鈕
        const item = e.target.closest(""li""); // 找到最近的 li 祖先元素
        item.remove(); // 移除該列表項目
    }
});

// 常用事件類型
const input = document.querySelector(""#searchInput""); // 取得搜尋輸入框
input.addEventListener(""input"", (e) => { // input 事件：每次輸入都會觸發
    console.log(""目前輸入："", e.target.value); // 取得輸入框的值
});

input.addEventListener(""keydown"", (e) => { // keydown 事件：按下鍵盤時觸發
    if (e.key === ""Enter"") { // 判斷是否按下 Enter 鍵
        console.log(""送出搜尋："", e.target.value); // 執行搜尋
    }
});
```

---

## 📌 DOM 操作

```javascript
// 選取元素
const title = document.querySelector(""h1""); // 選取第一個 h1 元素
const items = document.querySelectorAll("".item""); // 選取所有 class=""item"" 的元素
const main = document.getElementById(""main""); // 用 ID 選取元素

// 修改內容
title.textContent = ""新標題""; // 修改純文字內容（安全，不解析 HTML）
title.innerHTML = ""<em>斜體標題</em>""; // 修改 HTML 內容（注意 XSS 風險）

// 修改樣式
title.style.color = ""blue""; // 直接修改行內樣式
title.style.fontSize = ""2rem""; // CSS 屬性名用駝峰命名

// 操作 CSS class
title.classList.add(""active""); // 新增 class
title.classList.remove(""hidden""); // 移除 class
title.classList.toggle(""dark-mode""); // 切換 class（有就移除，沒有就新增）
title.classList.contains(""active""); // 檢查是否有某個 class，回傳 boolean

// 建立和插入元素
const newCard = document.createElement(""div""); // 建立一個新的 div 元素
newCard.className = ""card""; // 設定 class 名稱
newCard.textContent = ""新卡片""; // 設定文字內容

const container = document.querySelector("".container""); // 取得容器元素
container.appendChild(newCard); // 將新元素加到容器的最後面
container.prepend(newCard); // 將新元素加到容器的最前面
container.insertBefore(newCard, container.firstChild); // 插入到第一個子元素前面

// 移除元素
const oldItem = document.querySelector("".old-item""); // 選取要刪除的元素
oldItem.remove(); // 直接移除元素

// 操作屬性
const link = document.querySelector(""a""); // 取得超連結元素
link.setAttribute(""href"", ""https://example.com""); // 設定 href 屬性
link.getAttribute(""href""); // 取得 href 屬性的值
link.removeAttribute(""target""); // 移除 target 屬性

// data 屬性（自訂資料）
const card = document.querySelector("".card""); // 取得卡片元素
card.dataset.id = ""123""; // 設定 data-id=""123""
console.log(card.dataset.id); // 讀取 data-id 的值：""123""
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：用 == 比較導致意外結果

```javascript
// 錯誤：== 會做型別轉換，結果不如預期
if (userInput == 0) { // 如果 userInput 是空字串 """"，這也會是 true！
    console.log(""輸入為零""); // 空字串被轉成 0，意外觸發這段
}

// 正確：用 === 嚴格比較
if (userInput === 0) { // === 不做型別轉換，只有真正的數字 0 才會是 true
    console.log(""輸入為零""); // 現在只有數字 0 會觸發
}
```

### ❌ 錯誤 2：async/await 忘記用 try/catch

```javascript
// 錯誤：沒有錯誤處理，網路失敗時整個程式會崩潰
async function loadData() { // 非同步函式
    const response = await fetch(""/api/data""); // 如果網路失敗，會拋出未處理的錯誤
    const data = await response.json(); // 如果回應不是 JSON，也會拋出錯誤
    return data; // 這行可能永遠不會執行到
}

// 正確：用 try/catch 包裹非同步操作
async function loadData() { // 非同步函式
    try { // 用 try 包裹可能出錯的程式碼
        const response = await fetch(""/api/data""); // 等待回應
        if (!response.ok) throw new Error(""HTTP 錯誤""); // 檢查 HTTP 狀態
        const data = await response.json(); // 解析 JSON
        return data; // 回傳資料
    } catch (error) { // 捕捉所有錯誤
        console.error(""載入失敗："", error); // 印出錯誤訊息
        return null; // 回傳預設值，避免後續程式出錯
    }
}
```

### ❌ 錯誤 3：在迴圈中用 var 導致閉包問題

```javascript
// 錯誤：var 沒有區塊作用域，所有計時器共用同一個 i
for (var i = 0; i < 5; i++) { // var 宣告的 i 在迴圈外也存在
    setTimeout(() => { // 箭頭函式捕捉的是「同一個」i
        console.log(i); // 全部印出 5，因為迴圈結束後 i 是 5
    }, i * 100); // 即使延遲不同，印出的都是 5
}

// 正確：用 let 取代 var
for (let i = 0; i < 5; i++) { // let 在每次迴圈建立新的區塊作用域
    setTimeout(() => { // 每個箭頭函式捕捉到自己的 i
        console.log(i); // 正確印出 0, 1, 2, 3, 4
    }, i * 100); // 每隔 100ms 依序印出
}
```
"
            },

            // ── Chapter 503: 前後端整合：Fetch API 與 AJAX ──
            new Chapter
            {
                Id = 503,
                Title = "前後端整合：Fetch API 與 AJAX",
                Slug = "frontend-fetch-api-ajax",
                Category = "frontend",
                Order = 4,
                Level = "intermediate",
                Icon = "🔗",
                IsPublished = true,
                Content = @"# 🔗 前後端整合：Fetch API 與 AJAX

## 📌 什麼是前後端通訊？

> **比喻：前後端通訊就像餐廳的服務生和廚房** 🍽️
>
> 前端是外場的服務生，負責接待客人、記錄點餐（使用者介面）。
> 後端是廚房的廚師，負責備料、烹飪、出餐（資料處理和儲存）。
> AJAX / Fetch API 就是服務生和廚房之間的「傳菜系統」——
> 服務生把點單送進廚房（Request），廚房做好菜後傳出來（Response）。
> 整個過程中客人不需要離開座位（頁面不需要重新整理）。

---

## 📌 XMLHttpRequest vs Fetch API

```javascript
// === 舊方法：XMLHttpRequest（了解即可，現在已少用） ===
const xhr = new XMLHttpRequest(); // 建立一個 XMLHttpRequest 物件
xhr.open(""GET"", ""/api/users"", true); // 設定方法、網址和是否非同步
xhr.onreadystatechange = function() { // 監聽狀態變化
    if (xhr.readyState === 4 && xhr.status === 200) { // 狀態 4 表示完成，200 表示成功
        const data = JSON.parse(xhr.responseText); // 手動解析 JSON 字串
        console.log(data); // 印出資料
    }
};
xhr.send(); // 送出請求

// === 新方法：Fetch API（推薦使用） ===
fetch(""/api/users"") // 送出 GET 請求，回傳 Promise
    .then(response => response.json()) // 將回應解析為 JSON（也是 Promise）
    .then(data => console.log(data)) // 取得解析後的資料
    .catch(error => console.error(""錯誤："", error)); // 捕捉任何錯誤

// === 使用 async/await（最推薦的寫法） ===
async function getUsers() { // 宣告非同步函式
    try {
        const response = await fetch(""/api/users""); // 等待回應
        if (!response.ok) { // 檢查 HTTP 狀態（fetch 不會自動拋出 HTTP 錯誤）
            throw new Error(`伺服器錯誤：${response.status}`); // 手動拋出錯誤
        }
        const data = await response.json(); // 等待 JSON 解析完成
        return data; // 回傳資料
    } catch (error) {
        console.error(""取得使用者失敗："", error); // 印出錯誤訊息
        throw error; // 重新拋出讓呼叫者處理
    }
}
```

---

## 📌 GET / POST / PUT / DELETE 請求

```javascript
// === GET 請求：取得資料（像服務生去廚房拿現成的菜） ===
async function getProducts(category) { // 依分類取得產品列表
    const url = new URL(""/api/products"", window.location.origin); // 建立 URL 物件
    url.searchParams.append(""category"", category); // 加入查詢參數 ?category=xxx
    url.searchParams.append(""page"", ""1""); // 加入分頁參數 ?page=1

    const response = await fetch(url); // 送出 GET 請求（預設就是 GET）
    return await response.json(); // 解析並回傳 JSON 資料
}

// === POST 請求：建立新資料（像客人點一道新菜） ===
async function createProduct(product) { // 建立新產品
    const response = await fetch(""/api/products"", { // 送出 POST 請求
        method: ""POST"", // 設定 HTTP 方法為 POST
        headers: { // 設定請求標頭
            ""Content-Type"": ""application/json"", // 告訴伺服器送的是 JSON
            ""Authorization"": `Bearer ${getToken()}` // 附上驗證 Token
        },
        body: JSON.stringify(product) // 將 JavaScript 物件轉成 JSON 字串
    });

    if (!response.ok) { // 檢查回應狀態
        const error = await response.json(); // 解析錯誤訊息
        throw new Error(error.message || ""建立失敗""); // 拋出錯誤
    }

    return await response.json(); // 回傳新建立的產品資料
}

// 呼叫範例
const newProduct = await createProduct({ // 傳入產品資料物件
    name: ""筆記型電腦"", // 產品名稱
    price: 35000, // 產品價格
    category: ""electronics"" // 產品分類
});

// === PUT 請求：更新整筆資料（像要求廚房重做一道菜） ===
async function updateProduct(id, product) { // 更新指定 ID 的產品
    const response = await fetch(`/api/products/${id}`, { // 送出 PUT 請求到指定 ID
        method: ""PUT"", // 設定 HTTP 方法為 PUT
        headers: {
            ""Content-Type"": ""application/json"" // 內容類型為 JSON
        },
        body: JSON.stringify(product) // 將更新資料轉成 JSON
    });
    return await response.json(); // 回傳更新後的資料
}

// === DELETE 請求：刪除資料（像退掉一道菜） ===
async function deleteProduct(id) { // 刪除指定 ID 的產品
    const response = await fetch(`/api/products/${id}`, { // 送出 DELETE 請求
        method: ""DELETE"", // 設定 HTTP 方法為 DELETE
        headers: {
            ""Authorization"": `Bearer ${getToken()}` // 刪除需要驗證權限
        }
    });

    if (!response.ok) { // 檢查是否成功
        throw new Error(""刪除失敗""); // 失敗時拋出錯誤
    }

    return response.status === 204; // 204 No Content 表示刪除成功
}
```

---

## 📌 JSON 序列化與反序列化

```javascript
// JavaScript 物件 → JSON 字串（序列化）
const user = { // 建立一個 JavaScript 物件
    name: ""小明"", // 姓名屬性
    age: 25, // 年齡屬性
    hobbies: [""coding"", ""reading""] // 興趣陣列
};

const jsonString = JSON.stringify(user); // 將物件轉成 JSON 字串
console.log(jsonString); // 印出：{""name"":""小明"",""age"":25,""hobbies"":[""coding"",""reading""]}

// 美化輸出（第三個參數是縮排空格數）
const prettyJson = JSON.stringify(user, null, 2); // 用 2 個空格縮排
console.log(prettyJson); // 印出格式化的 JSON（方便閱讀）

// JSON 字串 → JavaScript 物件（反序列化）
const jsonData = '{""name"":""小華"",""age"":30}'; // 一段 JSON 字串
const parsed = JSON.parse(jsonData); // 將 JSON 字串解析為物件
console.log(parsed.name); // 印出：小華

// 搭配 C# 後端的對應
// C# 端：public class User { public string Name { get; set; } public int Age { get; set; } }
// JavaScript 注意：C# 的 PascalCase 會被 System.Text.Json 轉成 camelCase
const userFromApi = await fetch(""/api/users/1"").then(r => r.json()); // 從 API 取得使用者
console.log(userFromApi.name); // camelCase：name（不是 Name）
console.log(userFromApi.age); // camelCase：age（不是 Age）
```

---

## 📌 與 ASP.NET Core API 搭配

```javascript
// 完整的 CRUD 服務類別
class ProductService { // 封裝所有產品相關的 API 呼叫
    constructor(baseUrl = ""/api/products"") { // 建構子設定基礎 URL
        this.baseUrl = baseUrl; // 儲存基礎 URL
    }

    async getAll() { // 取得所有產品
        const response = await fetch(this.baseUrl); // 送出 GET 請求
        if (!response.ok) throw new Error(""取得產品列表失敗""); // 錯誤處理
        return await response.json(); // 回傳 JSON 資料
    }

    async getById(id) { // 依 ID 取得單一產品
        const response = await fetch(`${this.baseUrl}/${id}`); // 送出 GET 請求到指定 ID
        if (response.status === 404) return null; // 找不到就回傳 null
        if (!response.ok) throw new Error(""取得產品失敗""); // 其他錯誤
        return await response.json(); // 回傳產品資料
    }

    async create(product) { // 建立新產品
        const response = await fetch(this.baseUrl, { // POST 到基礎 URL
            method: ""POST"", // HTTP 方法
            headers: { ""Content-Type"": ""application/json"" }, // 設定內容類型
            body: JSON.stringify(product) // 將產品物件轉成 JSON
        });
        if (!response.ok) { // 檢查回應
            const errors = await response.json(); // 解析伺服器的驗證錯誤
            throw new Error(JSON.stringify(errors)); // 將錯誤訊息拋出
        }
        return await response.json(); // 回傳建立的產品
    }

    async update(id, product) { // 更新產品
        const response = await fetch(`${this.baseUrl}/${id}`, { // PUT 到指定 ID
            method: ""PUT"", // HTTP 方法
            headers: { ""Content-Type"": ""application/json"" }, // 設定內容類型
            body: JSON.stringify(product) // 將更新資料轉成 JSON
        });
        if (!response.ok) throw new Error(""更新失敗""); // 錯誤處理
        return await response.json(); // 回傳更新後的產品
    }

    async delete(id) { // 刪除產品
        const response = await fetch(`${this.baseUrl}/${id}`, { // DELETE 指定 ID
            method: ""DELETE"" // HTTP 方法
        });
        return response.ok; // 回傳是否成功
    }
}

// 使用範例
const service = new ProductService(); // 建立服務實例
const products = await service.getAll(); // 取得所有產品
```

---

## 📌 CORS 跨域問題與解法

```javascript
// 什麼是 CORS？
// 當前端（http://localhost:3000）向不同來源的後端（http://localhost:5000）
// 發送請求時，瀏覽器會因為「同源政策」而阻擋。

// 錯誤訊息通常長這樣：
// Access to fetch at 'http://localhost:5000/api/data' from origin
// 'http://localhost:3000' has been blocked by CORS policy

// 解法在 ASP.NET Core 後端設定（不是前端能處理的！）
// 在 Program.cs 中：
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(""AllowFrontend"", policy =>
//     {
//         policy.WithOrigins(""http://localhost:3000"")  // 允許的來源
//               .AllowAnyHeader()                       // 允許任何標頭
//               .AllowAnyMethod()                       // 允許任何 HTTP 方法
//               .AllowCredentials();                    // 允許攜帶 Cookie
//     });
// });
// app.UseCors(""AllowFrontend"");

// 前端攜帶 Cookie 的設定
const response = await fetch(""http://localhost:5000/api/data"", { // 跨域請求
    method: ""GET"", // HTTP 方法
    credentials: ""include"" // 重點！告訴瀏覽器要攜帶 Cookie
});
```

---

## 📌 FormData 上傳檔案

```javascript
// 使用 FormData 上傳檔案
async function uploadFile(file) { // 接受一個 File 物件
    const formData = new FormData(); // 建立 FormData 物件
    formData.append(""file"", file); // 加入檔案，key 為 ""file""
    formData.append(""description"", ""產品圖片""); // 也可以加入其他欄位

    const response = await fetch(""/api/upload"", { // 送出 POST 請求
        method: ""POST"", // HTTP 方法
        body: formData // 直接傳 FormData，不需要設定 Content-Type（瀏覽器會自動設定）
        // 注意：不要手動設定 Content-Type，否則 boundary 會不正確！
    });

    if (!response.ok) throw new Error(""上傳失敗""); // 錯誤處理
    return await response.json(); // 回傳上傳結果
}

// 搭配 HTML input 使用
const fileInput = document.querySelector(""#fileInput""); // 取得檔案輸入元素
fileInput.addEventListener(""change"", async (e) => { // 監聽檔案選擇事件
    const file = e.target.files[0]; // 取得選擇的第一個檔案
    if (!file) return; // 如果沒選擇檔案就跳過

    if (file.size > 5 * 1024 * 1024) { // 檢查檔案大小（5MB 上限）
        alert(""檔案大小不能超過 5MB""); // 顯示錯誤提示
        return; // 中止上傳
    }

    try {
        const result = await uploadFile(file); // 呼叫上傳函式
        console.log(""上傳成功："", result); // 印出結果
    } catch (error) {
        console.error(""上傳錯誤："", error); // 印出錯誤
    }
});

// 多檔案上傳
async function uploadMultipleFiles(files) { // 接受 FileList 或 File 陣列
    const formData = new FormData(); // 建立 FormData
    for (const file of files) { // 遍歷所有檔案
        formData.append(""files"", file); // 用相同的 key 加入多個檔案
    }

    const response = await fetch(""/api/upload/multiple"", { // 送出到多檔上傳端點
        method: ""POST"", // HTTP 方法
        body: formData // 送出 FormData
    });
    return await response.json(); // 回傳結果
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：fetch 的 HTTP 錯誤不會自動拋出例外

```javascript
// 錯誤：以為 fetch 失敗會自動進入 catch
try {
    const response = await fetch(""/api/not-found""); // 即使 404，fetch 也不會拋出錯誤！
    const data = await response.json(); // 試圖解析 404 頁面會出錯
    console.log(data); // 可能得到意外結果
} catch (error) {
    console.error(error); // 只有網路斷線才會進入這裡
}

// 正確：手動檢查 response.ok
try {
    const response = await fetch(""/api/not-found""); // 送出請求
    if (!response.ok) { // 手動檢查 HTTP 狀態碼是否在 200-299 之間
        throw new Error(`HTTP 錯誤 ${response.status}: ${response.statusText}`); // 手動拋出
    }
    const data = await response.json(); // 只有成功才解析
    console.log(data); // 使用資料
} catch (error) {
    console.error(""請求失敗："", error.message); // 現在能正確捕捉 HTTP 錯誤
}
```

### ❌ 錯誤 2：POST 時忘記設定 Content-Type

```javascript
// 錯誤：沒有設定 Content-Type，伺服器不知道收到的是 JSON
const response = await fetch(""/api/products"", { // 送出 POST 請求
    method: ""POST"", // HTTP 方法
    body: JSON.stringify({ name: ""手機"" }) // 有轉 JSON 字串
    // 但沒有設定 Content-Type！伺服器會當成純文字處理
});

// 正確：明確設定 Content-Type 為 application/json
const response = await fetch(""/api/products"", { // 送出 POST 請求
    method: ""POST"", // HTTP 方法
    headers: {
        ""Content-Type"": ""application/json"" // 告訴伺服器這是 JSON 格式
    },
    body: JSON.stringify({ name: ""手機"" }) // 將物件轉成 JSON 字串
});
```

### ❌ 錯誤 3：上傳檔案時手動設定 Content-Type

```javascript
// 錯誤：手動設定 Content-Type 會破壞 FormData 的 boundary
const formData = new FormData(); // 建立 FormData
formData.append(""file"", file); // 加入檔案
const response = await fetch(""/api/upload"", { // 送出請求
    method: ""POST"", // HTTP 方法
    headers: {
        ""Content-Type"": ""multipart/form-data"" // 錯誤！手動設定會缺少 boundary
    },
    body: formData // FormData 內容
});

// 正確：讓瀏覽器自動設定 Content-Type（包含正確的 boundary）
const formData = new FormData(); // 建立 FormData
formData.append(""file"", file); // 加入檔案
const response = await fetch(""/api/upload"", { // 送出請求
    method: ""POST"", // HTTP 方法
    // 不要設定 Content-Type！瀏覽器會自動加上正確的 multipart/form-data 和 boundary
    body: formData // 直接傳 FormData
});
```
"
            },

            // ── Chapter 504: 前端工具鏈與框架概覽 ──
            new Chapter
            {
                Id = 504,
                Title = "前端工具鏈與框架概覽",
                Slug = "frontend-tools-frameworks",
                Category = "frontend",
                Order = 5,
                Level = "intermediate",
                Icon = "📦",
                IsPublished = true,
                Content = @"# 📦 前端工具鏈與框架概覽

## 📌 什麼是前端工具鏈？

> **比喻：前端工具鏈就像工廠的生產線** 🏭
>
> 想像你要大量生產家具（網頁應用程式），不可能全部手工打造。
> 你需要一條生產線：原料進貨系統（npm）、加工機台（Bundler）、
> 品質檢測站（Linter/TypeScript）、包裝出貨（Build）。
> 前端工具鏈就是這整條生產線，讓開發更有效率。

---

## 📌 npm / Node.js 基礎

```bash
# Node.js 是 JavaScript 的執行環境，npm 是套件管理工具
# 就像 C# 的 NuGet，npm 讓你下載和管理第三方套件

# 初始化專案（建立 package.json）
npm init -y # -y 表示全部使用預設值，快速建立

# 安裝套件
npm install axios # 安裝 axios 套件（HTTP 請求工具），加到 dependencies
npm install -D typescript # 安裝 TypeScript，加到 devDependencies（-D 是開發時依賴）

# package.json 的重要欄位
# {
#   ""name"": ""my-app"",           // 專案名稱
#   ""version"": ""1.0.0"",         // 版本號
#   ""scripts"": {                 // 自訂指令（像是 Makefile）
#     ""dev"": ""vite"",            // npm run dev → 啟動開發伺服器
#     ""build"": ""vite build"",    // npm run build → 建置正式版
#     ""preview"": ""vite preview"" // npm run preview → 預覽建置結果
#   },
#   ""dependencies"": {           // 正式環境需要的套件
#     ""axios"": ""^1.6.0""         // ^ 表示允許自動升級次要版本
#   },
#   ""devDependencies"": {        // 只有開發時需要的套件
#     ""typescript"": ""^5.3.0""    // TypeScript 編譯器
#   }
# }

# 常用 npm 指令
npm install # 根據 package.json 安裝所有依賴（別人拉專案後第一件事）
npm update # 更新所有套件到最新允許的版本
npm run dev # 執行 scripts 裡定義的 dev 指令
npm list --depth=0 # 列出目前安裝的所有頂層套件
```

---

## 📌 Bundler 概念（Webpack vs Vite）

```javascript
// Bundler 的作用：把多個 JavaScript 檔案打包成少數幾個檔案
// 就像把一堆零件組裝成成品，方便運送（部署）

// === Webpack（老牌，功能完整，設定複雜） ===
// webpack.config.js 範例
module.exports = { // 匯出 Webpack 設定物件
    entry: ""./src/index.js"", // 進入點：從哪個檔案開始打包
    output: { // 輸出設定
        filename: ""bundle.js"", // 打包後的檔案名稱
        path: __dirname + ""/dist"" // 輸出到 dist 資料夾
    },
    module: { // 模組處理規則
        rules: [ // 定義不同檔案類型的處理方式
            {
                test: /\.css$/, // 正則表達式：匹配 .css 檔案
                use: [""style-loader"", ""css-loader""] // 用這兩個 loader 處理 CSS
            },
            {
                test: /\.js$/, // 匹配 .js 檔案
                exclude: /node_modules/, // 排除 node_modules 資料夾
                use: ""babel-loader"" // 用 Babel 轉譯新語法為舊瀏覽器能懂的
            }
        ]
    }
};

// === Vite（新一代，快速，設定簡單，推薦新專案使用） ===
// vite.config.js 範例
import { defineConfig } from ""vite""; // 引入 Vite 的設定函式

export default defineConfig({ // 匯出 Vite 設定
    server: { // 開發伺服器設定
        port: 3000, // 開發伺服器使用 3000 埠
        proxy: { // 代理設定（解決開發時的 CORS 問題）
            ""/api"": { // 所有 /api 開頭的請求
                target: ""http://localhost:5000"", // 轉發到後端伺服器
                changeOrigin: true // 修改請求的 origin 標頭
            }
        }
    },
    build: { // 建置設定
        outDir: ""../wwwroot"" // 輸出到 ASP.NET Core 的 wwwroot 資料夾
    }
});

// Webpack vs Vite 比較：
// | 特性         | Webpack           | Vite              |
// |-------------|-------------------|-------------------|
// | 開發啟動速度  | 慢（全部打包）      | 快（按需載入）      |
// | 設定複雜度    | 高                | 低                |
// | 生態系       | 成熟豐富          | 快速成長           |
// | 推薦用於     | 大型遺留專案       | 新專案             |
```

---

## 📌 TypeScript 基礎（給 C# 開發者）

```typescript
// TypeScript 是 JavaScript 的超集，加入了型別系統
// 對 C# 開發者來說，TypeScript 會感覺非常親切！

// === 基本型別（像 C# 的 int, string, bool） ===
let name: string = ""小明""; // 字串型別（對應 C# 的 string）
let age: number = 25; // 數字型別（對應 C# 的 int/double）
let isActive: boolean = true; // 布林型別（對應 C# 的 bool）
let items: string[] = [""a"", ""b""]; // 字串陣列（對應 C# 的 string[]）
let tuple: [string, number] = [""小明"", 25]; // 元組（對應 C# 的 ValueTuple）

// === 介面（像 C# 的 interface） ===
interface User { // 定義 User 介面（像 C# 的 interface 或 class）
    id: number; // 必要屬性：ID
    name: string; // 必要屬性：名稱
    email?: string; // 可選屬性：信箱（? 表示可省略，像 C# 的 string?）
    readonly createdAt: Date; // 唯讀屬性（像 C# 的 { get; }）
}

// 使用介面
const user: User = { // 建立符合 User 介面的物件
    id: 1, // ID
    name: ""小明"", // 名稱
    createdAt: new Date() // 建立日期
}; // email 是可選的，所以可以省略

// === 泛型（像 C# 的 Generic） ===
interface ApiResponse<T> { // 定義泛型介面（像 C# 的 ApiResponse<T>）
    data: T; // 泛型資料（對應 C# 的 T Data { get; set; }）
    success: boolean; // 是否成功
    message: string; // 訊息
}

// 使用泛型
async function fetchApi<T>(url: string): Promise<ApiResponse<T>> { // 泛型函式
    const response = await fetch(url); // 送出請求
    return await response.json() as ApiResponse<T>; // 斷言回傳型別
}

// 呼叫時指定型別
const result = await fetchApi<User[]>(""/api/users""); // T 是 User[]
console.log(result.data); // data 的型別是 User[]，IDE 會提供自動完成

// === Enum（像 C# 的 enum） ===
enum OrderStatus { // 定義訂單狀態列舉
    Pending = ""pending"", // 待處理
    Processing = ""processing"", // 處理中
    Completed = ""completed"", // 已完成
    Cancelled = ""cancelled"" // 已取消
}

// 使用 enum
const status: OrderStatus = OrderStatus.Pending; // 指定狀態為待處理

// === 型別別名和聯合型別 ===
type ID = string | number; // 聯合型別：ID 可以是字串或數字
type Theme = ""light"" | ""dark"" | ""system""; // 字面量型別：只能是這三個值之一

function setTheme(theme: Theme): void { // 參數只接受指定的值
    document.documentElement.dataset.theme = theme; // 設定 HTML 的 data-theme 屬性
}

setTheme(""dark""); // 合法
// setTheme(""blue""); // 編譯錯誤！""blue"" 不在 Theme 型別中
```

---

## 📌 前端框架比較

```javascript
// === React（Meta 開發，最大生態系） ===
// 特色：虛擬 DOM、JSX 語法、函式組件 + Hooks
// 適合：大型專案、豐富的第三方套件需求

// React 組件範例
function ProductCard({ product }) { // 函式組件，接收 props
    const [count, setCount] = useState(0); // useState Hook 管理狀態

    return ( // JSX 語法：在 JavaScript 中寫 HTML
        <div className=""card""> {/* className 取代 HTML 的 class */}
            <h3>{product.name}</h3> {/* 用大括號插入 JavaScript 表達式 */}
            <p>數量：{count}</p> {/* 顯示狀態值 */}
            <button onClick={() => setCount(count + 1)}> {/* 事件處理 */}
                增加
            </button>
        </div>
    );
}

// === Vue（尤雨溪開發，漸進式框架） ===
// 特色：模板語法、響應式系統、Single File Component
// 適合：中小型專案、快速上手

// Vue 組件範例（Composition API）
// <template>
//   <div class=""card"">
//     <h3>{{ product.name }}</h3>        <!-- 模板語法：用雙大括號插值 -->
//     <p>數量：{{ count }}</p>            <!-- 顯示響應式資料 -->
//     <button @click=""count++"">增加</button>  <!-- @click 是 v-on:click 的簡寫 -->
//   </div>
// </template>
// <script setup>
// import { ref } from 'vue';            // 引入 ref 建立響應式資料
// const count = ref(0);                  // ref(0) 建立一個響應式的數字
// const product = defineProps(['product']); // 定義從父組件接收的 props
// </script>

// === Angular（Google 開發，企業級框架） ===
// 特色：完整框架（路由、表單、HTTP 全包）、TypeScript 優先、DI 系統
// 適合：大型企業專案、熟悉 C# 的開發者（概念相似）

// === Blazor（Microsoft 開發，用 C# 寫前端） ===
// 特色：使用 C# 和 Razor 語法、可選 WebAssembly 或 Server 模式
// 適合：純 .NET 團隊、不想學 JavaScript 的 C# 開發者

// Blazor 組件範例
// @page ""/counter""
// <h3>計數器</h3>
// <p>目前數量：@count</p>                  <!-- Razor 語法：@ 開頭插入 C# -->
// <button @onclick=""Increment"">增加</button>  <!-- @onclick 綁定 C# 方法 -->
// @code {
//     private int count = 0;                // C# 的私有欄位
//     private void Increment() => count++;  // C# 的方法
// }

// 框架選擇建議：
// | 情境                        | 推薦框架      |
// |----------------------------|-------------|
// | 大型專案 + 大量第三方套件      | React       |
// | 中小型專案 + 快速開發         | Vue         |
// | 企業級 + 完整架構            | Angular     |
// | 純 .NET 團隊               | Blazor      |
```

---

## 📌 CSS 框架

```html
<!-- === Bootstrap（最老牌，元件豐富） === -->
<!-- 用 class 就能快速建立響應式版面 -->
<div class=""container""> <!-- container 自動置中和限制寬度 -->
    <div class=""row""> <!-- row 建立一列 -->
        <div class=""col-md-6""> <!-- col-md-6 在中等螢幕佔一半寬度 -->
            <div class=""card""> <!-- card 預設的卡片元件 -->
                <div class=""card-body""> <!-- 卡片內容區 -->
                    <h5 class=""card-title"">標題</h5> <!-- 卡片標題 -->
                    <p class=""card-text"">內容</p> <!-- 卡片內文 -->
                    <button class=""btn btn-primary"">按鈕</button> <!-- 主色按鈕 -->
                </div>
            </div>
        </div>
    </div>
</div>

<!-- === Tailwind CSS（Utility-First，近年最流行） === -->
<!-- 用小型工具 class 組合出任何設計 -->
<div class=""max-w-sm mx-auto""> <!-- max-w-sm 最大寬度、mx-auto 水平置中 -->
    <div class=""bg-white rounded-lg shadow-md p-6""> <!-- 白背景、圓角、陰影、內距 -->
        <h5 class=""text-xl font-bold text-gray-800"">標題</h5> <!-- 文字大小、粗體、深灰色 -->
        <p class=""text-gray-600 mt-2"">內容</p> <!-- 較淺灰色、上方間距 -->
        <button class=""mt-4 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600""> <!-- 藍色按鈕、hover 變深 -->
            按鈕
        </button>
    </div>
</div>
```

```css
/* Bootstrap vs Tailwind 比較 */
/* | 特性         | Bootstrap          | Tailwind CSS        | */
/* |-------------|--------------------|--------------------|  */
/* | 學習曲線     | 低（預設元件多）      | 中（需記 class 名）  | */
/* | 客製化       | 有限（覆蓋樣式）      | 極高（從零組合）      | */
/* | 檔案大小     | 較大（整包引入）      | 極小（只打包用到的）   | */
/* | 設計一致性   | 高（統一風格）        | 取決於開發者         | */
/* | 適合         | 快速原型、後台管理    | 客製化設計、現代專案   | */
```

---

## 📌 SPA vs MVC SSR：何時該用哪個？

```javascript
// SPA（Single Page Application）：整個應用是一個 HTML 頁面
// - 前後端完全分離
// - 使用 React/Vue/Angular 等框架
// - 頁面切換不重新載入（Client-Side Routing）
// - 適合：互動性強的應用（管理後台、社群平台、即時通訊）

// MVC SSR（Server-Side Rendering）：伺服器產生完整 HTML
// - ASP.NET Core MVC + Razor Views
// - 每次換頁向伺服器請求新的 HTML
// - SEO 友好（搜尋引擎直接讀到完整內容）
// - 適合：內容網站、部落格、電商展示頁

// 混合方案：
// 1. ASP.NET Core MVC + 局部 JavaScript（最簡單）
//    - 大部分用 Razor 渲染，需要互動的地方加 JS

// 2. ASP.NET Core + Blazor Server（純 C# 方案）
//    - 用 SignalR 即時同步 UI，不需要寫 JavaScript

// 3. ASP.NET Core API + SPA 前端（最大彈性）
//    - 後端只提供 API，前端用 React/Vue 開發
//    - 適合前後端分開部署的場景

// 選擇建議：
// | 需求                    | 推薦方案                    |
// |------------------------|---------------------------|
// | SEO 重要 + 少互動       | MVC SSR（Razor Pages）     |
// | 高互動 + 即時更新        | SPA（React/Vue）           |
// | 純 .NET 團隊            | Blazor Server/WASM         |
// | 兩者都要                | Next.js / Nuxt.js（SSR+SPA）|
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：npm install 後忘記把 node_modules 加入 .gitignore

```bash
# 錯誤：把 node_modules 推上 Git，倉庫暴增數百 MB
git add . # 這會把 node_modules 裡的數萬個檔案全部加入
git commit -m ""加入專案"" # 推上去後別人 clone 要等很久

# 正確：在專案根目錄建立 .gitignore
# .gitignore 內容：
# node_modules/ # 忽略所有下載的套件（別人用 npm install 重新安裝）
# dist/ # 忽略建置輸出（CI/CD 會重新建置）
# .env # 忽略環境變數檔案（包含密鑰等敏感資訊）
```

### ❌ 錯誤 2：TypeScript 的型別斷言濫用

```typescript
// 錯誤：用 as any 跳過所有型別檢查，失去 TypeScript 的意義
const data = response.data as any; // 任何型別都通過，完全沒有型別保護
console.log(data.nonExistentProp.value); // 執行時才會爆炸

// 正確：定義正確的型別
interface Product { // 定義產品介面
    id: number; // ID 是數字
    name: string; // 名稱是字串
    price: number; // 價格是數字
}

const data = response.data as Product; // 斷言為具體型別
console.log(data.name); // IDE 會提供自動完成，打錯字會有警告
// console.log(data.nonExistentProp); // 編譯錯誤！Property 不存在
```

### ❌ 錯誤 3：SPA 沒有處理 404 路由的後端設定

```javascript
// 錯誤情境：SPA 部署到 IIS 或 Nginx 後，重新整理頁面得到 404

// 原因：SPA 的路由（例如 /products/123）是前端處理的
// 但重新整理時，瀏覽器會向伺服器請求 /products/123
// 伺服器找不到這個實際檔案，就回傳 404

// 解法：在 ASP.NET Core 中設定 Fallback
// Program.cs 中：
// app.UseStaticFiles();   // 先嘗試靜態檔案
// app.MapFallbackToFile(""index.html"");  // 找不到就回傳 index.html

// 或在 IIS 的 web.config 中：
// <rewrite>
//   <rules>
//     <rule name=""SPA"" stopProcessing=""true"">
//       <match url="".*"" />
//       <conditions>
//         <add input=""{REQUEST_FILENAME}"" matchType=""IsFile"" negate=""true"" />
//       </conditions>
//       <action type=""Rewrite"" url=""/"" />
//     </rule>
//   </rules>
// </rewrite>
```
"
            }
        };
    }
}
