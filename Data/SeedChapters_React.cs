using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_React
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Chapter 800: React 入門 ─────────────────────────────────
        new() { Id=800, Category="frontend", Order=20, Level="beginner", Icon="⚛️", Title="React 入門：元件化思維與 JSX", Slug="react-intro", IsPublished=true, Content=@"
# ⚛️ React 入門：元件化思維與 JSX

## 📌 React 是什麼？

React 是由 **Facebook（Meta）** 開發並維護的開源 **UI 函式庫（Library）**，用於建構使用者介面。

> ⚠️ **重點觀念：React 不是原生 JavaScript！**
> React 是建立在 JavaScript **之上**的函式庫，它使用一種叫 **JSX** 的語法糖。
> JSX 看起來像 HTML，但實際上會被編譯（transpile）成 `React.createElement()` 呼叫。
> 你寫的 React 程式碼**不能直接在瀏覽器中執行**，必須經過 Babel 或其他編譯工具轉換。

---

## 🔄 JSX 是語法糖，底層是 React.createElement()

```jsx
// 你寫的 JSX：
const element = <h1 className=""title"">Hello React</h1>;

// Babel 編譯後的原生 JS：
const element = React.createElement(
  'h1',                          // 標籤名稱
  { className: 'title' },       // 屬性（props）
  'Hello React'                  // 子元素（children）
);
```

> 💡 **為什麼要知道這個？**
> 理解 JSX 底層是 `React.createElement()`，能幫助你除錯、理解 React 的運作機制。
> JSX 不是魔法——它只是讓你用更直覺的語法來寫 UI。

---

## 🆚 原生 JS DOM 操作 vs React 宣告式寫法

### 原生 JavaScript（命令式）

```javascript
// 原生 JS：一步步告訴瀏覽器「怎麼做」
const container = document.getElementById('app');
const h1 = document.createElement('h1');     // 建立元素
h1.textContent = 'Hello World';               // 設定文字
h1.className = 'title';                       // 設定 class
container.appendChild(h1);                     // 加到 DOM

// 更新時要手動操作 DOM
function updateTitle(newText) {
  h1.textContent = newText;  // 直接操作 DOM 節點
}
```

### React（宣告式）

```jsx
// React：告訴 React「你要什麼」，React 幫你處理 DOM
function App() {
  const [title, setTitle] = useState('Hello World');

  return (
    <div>
      <h1 className=""title"">{title}</h1>
      <button onClick={() => setTitle('Hello React')}>
        更新標題
      </button>
    </div>
  );
}
```

> 📝 **差異總結**：
> - 原生 JS：你要自己操作 DOM（命令式）
> - React：你描述 UI 應該長什麼樣子，React 幫你更新 DOM（宣告式）

---

## 🧠 虛擬 DOM（Virtual DOM）概念

React 不直接操作真實 DOM，而是維護一個**虛擬 DOM**（JavaScript 物件樹）。

```
更新流程：
1. 狀態改變 → React 產生新的虛擬 DOM
2. React 比較新舊虛擬 DOM（Diffing）
3. 只更新真正改變的部分到真實 DOM（Reconciliation）
```

```jsx
// 虛擬 DOM 本質上是 JS 物件
const virtualElement = {
  type: 'h1',
  props: {
    className: 'title',
    children: 'Hello React'
  }
};
// React 用這種物件來描述 UI，比直接操作 DOM 更高效
```

> 💡 **為什麼虛擬 DOM 比較快？**
> 直接操作真實 DOM 很慢（瀏覽器要重新計算佈局、重繪）。
> 虛擬 DOM 在記憶體中比較差異，只把「最小改動」套用到真實 DOM。

---

## 🛠️ 建立 React 專案

### 方法一：Vite（推薦）

```bash
# 使用 Vite 建立 React + TypeScript 專案
npm create vite@latest my-react-app -- --template react-ts

cd my-react-app
npm install
npm run dev    # 啟動開發伺服器
```

### 方法二：Create React App（較舊，不推薦新專案使用）

```bash
npx create-react-app my-app --template typescript
cd my-app
npm start
```

> ⚡ **Vite vs CRA**：Vite 啟動速度快非常多（使用原生 ES Module），是目前社群推薦的方式。

---

## 🧩 第一個 React 元件

```jsx
// App.jsx — React 元件就是一個回傳 JSX 的函式
function App() {
  const name = '小明';
  const currentTime = new Date().toLocaleTimeString();

  return (
    <div>
      <h1>你好，{name}！</h1>       {/* 用大括號嵌入 JS 表達式 */}
      <p>現在時間：{currentTime}</p>
      <Greeting message=""歡迎來到 React 的世界"" />
    </div>
  );
}

// Greeting.jsx — 自訂元件（元件名稱必須大寫開頭）
function Greeting({ message }) {
  return <p style={{ color: 'blue', fontSize: '18px' }}>{message}</p>;
}

export default App;
```

> ⚠️ **JSX 注意事項**：
> - `class` 要寫成 `className`（因為 `class` 是 JS 保留字）
> - `for` 要寫成 `htmlFor`
> - JSX 必須有**一個根元素**（可用 `<>...</>` Fragment）
> - 所有標籤必須關閉：`<img />` 而非 `<img>`

---

## 📂 React 專案結構

```
my-react-app/
├── public/            # 靜態檔案
│   └── index.html     # 唯一的 HTML 檔案（SPA）
├── src/
│   ├── main.jsx       # 進入點，掛載 React App
│   ├── App.jsx        # 根元件
│   ├── App.css        # 樣式
│   └── components/    # 自訂元件資料夾
├── package.json
└── vite.config.js     # Vite 設定
```

> 💡 React 是 **SPA（Single Page Application）**，整個應用只有一個 HTML 檔案，所有畫面切換都由 JavaScript 完成。

---

## ✅ 本章重點

| 觀念 | 說明 |
|------|------|
| React 不是原生 JS | 需要編譯（Babel/SWC），JSX → React.createElement() |
| 宣告式 vs 命令式 | React 描述「要什麼」，原生 JS 描述「怎麼做」 |
| 虛擬 DOM | 記憶體中比較差異，最小化真實 DOM 操作 |
| JSX | 語法糖，看起來像 HTML，實際是 JS |
| 元件 | 函式回傳 JSX，名稱必須大寫開頭 |
" },

        // ── Chapter 801: React 基礎 ─────────────────────────────────
        new() { Id=801, Category="frontend", Order=21, Level="beginner", Icon="⚛️", Title="React 基礎：useState、Props 與事件處理", Slug="react-basics", IsPublished=true, Content=@"
# ⚛️ React 基礎：useState、Props 與事件處理

## 📌 函式元件（Function Components）

React 元件就是一個 **JavaScript 函式**，回傳 JSX 來描述 UI。

> ⚠️ 再次強調：React 元件不是原生 HTML，JSX 會被編譯成 `React.createElement()` 呼叫。

```jsx
// 最簡單的函式元件
function Welcome() {
  return <h1>歡迎使用 React！</h1>;
}

// 箭頭函式寫法（也很常見）
const Welcome = () => <h1>歡迎使用 React！</h1>;

// 使用元件（像 HTML 標籤一樣）
function App() {
  return (
    <div>
      <Welcome />       {/* 使用自訂元件 */}
      <Welcome />       {/* 可以重複使用 */}
    </div>
  );
}
```

---

## 🔄 useState Hook — 狀態管理

`useState` 讓元件擁有「記憶」——能記住並更新資料。

```jsx
import { useState } from 'react';

function Counter() {
  // useState 回傳 [目前的值, 更新函式]
  const [count, setCount] = useState(0);  // 初始值 = 0

  return (
    <div>
      <p>目前計數：{count}</p>
      <button onClick={() => setCount(count + 1)}>+1</button>
      <button onClick={() => setCount(count - 1)}>-1</button>
      <button onClick={() => setCount(0)}>歸零</button>
    </div>
  );
}
```

> 💡 **為什麼不能直接 `count = count + 1`？**
> React 只在呼叫 `setCount` 時才知道要重新渲染。
> 直接修改變數不會觸發 UI 更新——這是 React 與原生 JS 最大的差異之一。

### 物件與陣列狀態

```jsx
function UserForm() {
  const [user, setUser] = useState({ name: '', age: 0 });

  // ⚠️ 必須展開原物件，React 靠「新參考」判斷是否更新
  const updateName = (e) => {
    setUser({ ...user, name: e.target.value });  // 展開運算子
  };

  return (
    <input value={user.name} onChange={updateName} />
  );
}
```

---

## 📦 Props — 父元件傳資料給子元件

Props 是元件之間溝通的方式，像函式的參數一樣。

```jsx
// 子元件：接收 props
function UserCard({ name, age, isActive }) {
  return (
    <div className={`card ${isActive ? 'active' : ''}`}>
      <h2>{name}</h2>
      <p>年齡：{age}</p>
      <span>{isActive ? '🟢 上線' : '🔴 離線'}</span>
    </div>
  );
}

// 父元件：傳遞 props
function App() {
  return (
    <div>
      <UserCard name=""小明"" age={25} isActive={true} />
      <UserCard name=""小華"" age={30} isActive={false} />
    </div>
  );
}
```

> ⚠️ **Props 是唯讀的！** 子元件不能修改收到的 props。
> 如果需要改變資料，應該由父元件透過回呼函式處理。

---

## 🖱️ 事件處理

React 事件用 **camelCase** 命名（`onClick` 而非 `onclick`），傳入的是函式而非字串。

```jsx
function EventDemo() {
  // 點擊事件
  const handleClick = () => {
    alert('按鈕被點擊了！');
  };

  // 輸入變更事件
  const [text, setText] = useState('');
  const handleChange = (e) => {
    setText(e.target.value);    // e.target.value 取得輸入值
  };

  // 表單送出事件
  const handleSubmit = (e) => {
    e.preventDefault();          // 阻止表單預設送出行為
    console.log('提交的文字：', text);
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        value={text}
        onChange={handleChange}
        placeholder=""輸入文字...""
      />
      <button type=""submit"">送出</button>
      <button type=""button"" onClick={handleClick}>點我</button>
    </form>
  );
}
```

> 💡 **React 事件 vs 原生 JS 事件**：
> - 原生：`element.addEventListener('click', handler)`
> - React：`<button onClick={handler}>`（宣告式，更直覺）
> - React 使用 **合成事件（SyntheticEvent）**，跨瀏覽器一致。

---

## 🔀 條件渲染

```jsx
function StatusMessage({ isLoggedIn, username }) {
  // 方法一：三元運算子
  return (
    <div>
      {isLoggedIn ? (
        <p>歡迎回來，{username}！</p>
      ) : (
        <p>請先登入</p>
      )}

      {/* 方法二：&& 短路求值 */}
      {isLoggedIn && <button>登出</button>}
    </div>
  );
}
```

---

## 📋 列表渲染

```jsx
function TodoList() {
  const [todos, setTodos] = useState([
    { id: 1, text: '學習 React', done: false },
    { id: 2, text: '寫元件', done: true },
    { id: 3, text: '部署應用', done: false },
  ]);

  const toggleTodo = (id) => {
    setTodos(todos.map(todo =>
      todo.id === id ? { ...todo, done: !todo.done } : todo
    ));
  };

  return (
    <ul>
      {todos.map(todo => (
        <li
          key={todo.id}                    // ⚠️ key 是必要的！
          onClick={() => toggleTodo(todo.id)}
          style={{ textDecoration: todo.done ? 'line-through' : 'none' }}
        >
          {todo.text}
        </li>
      ))}
    </ul>
  );
}
```

> ⚠️ **key 的重要性**：
> React 用 `key` 來追蹤列表中的每個項目。沒有 `key` 或用 `index` 當 `key` 可能導致效能問題和 bug。

---

## 🛠️ 完整範例：計數器 + 待辦清單

```jsx
import { useState } from 'react';

function App() {
  const [count, setCount] = useState(0);
  const [input, setInput] = useState('');
  const [todos, setTodos] = useState([]);

  const addTodo = () => {
    if (input.trim() === '') return;
    setTodos([...todos, { id: Date.now(), text: input, done: false }]);
    setInput('');                    // 清空輸入
    setCount(count + 1);            // 計數器加 1
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1>已新增 {count} 個待辦</h1>

      <input
        value={input}
        onChange={(e) => setInput(e.target.value)}
        onKeyDown={(e) => e.key === 'Enter' && addTodo()}
        placeholder=""輸入待辦事項...""
      />
      <button onClick={addTodo}>新增</button>

      <ul>
        {todos.map(todo => (
          <li key={todo.id}>{todo.text}</li>
        ))}
      </ul>
    </div>
  );
}
```

---

## ✅ 本章重點

| 觀念 | 說明 |
|------|------|
| 函式元件 | 回傳 JSX 的函式，大寫開頭 |
| useState | 狀態管理 Hook，更新觸發重新渲染 |
| Props | 父傳子的唯讀資料 |
| 事件處理 | camelCase、傳入函式、合成事件 |
| 條件渲染 | 三元運算子、&& 短路 |
| 列表渲染 | .map() + key |
" },

        // ── Chapter 802: React Hooks 深入 ─────────────────────────────
        new() { Id=802, Category="frontend", Order=22, Level="intermediate", Icon="🪝", Title="React Hooks 深入：useEffect、useRef、useContext", Slug="react-hooks", IsPublished=true, Content=@"
# 🪝 React Hooks 深入：useEffect、useRef、useContext

## 📌 useEffect — 副作用管理

`useEffect` 讓你在元件渲染後執行「副作用」操作，例如 API 呼叫、設定定時器、訂閱事件等。

> ⚠️ **什麼是副作用？**
> 任何不是「根據 props/state 算出 UI」的行為都是副作用。
> 原生 JS 直接在全域寫就好，但 React 需要 `useEffect` 來確保時機正確。

```jsx
import { useState, useEffect } from 'react';

function UserProfile({ userId }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  // useEffect(副作用函式, 依賴陣列)
  useEffect(() => {
    setLoading(true);

    // 呼叫 API（副作用）
    fetch(`https://api.example.com/users/${userId}`)
      .then(res => res.json())
      .then(data => {
        setUser(data);
        setLoading(false);
      });

    // 清理函式（元件卸載或依賴改變時執行）
    return () => {
      console.log('清理上一次的副作用');
    };
  }, [userId]);  // 只在 userId 改變時重新執行

  if (loading) return <p>載入中...</p>;
  return <h1>{user?.name}</h1>;
}
```

### useEffect 依賴陣列規則

```jsx
// 1. 無依賴陣列 → 每次渲染都執行（通常不建議）
useEffect(() => { /* 每次渲染後都執行 */ });

// 2. 空陣列 → 只在掛載時執行一次（相當於 componentDidMount）
useEffect(() => { /* 只執行一次 */ }, []);

// 3. 有依賴 → 依賴改變時才執行
useEffect(() => { /* userId 或 token 改變時執行 */ }, [userId, token]);
```

---

## 🔗 useRef — DOM 參照與持久值

`useRef` 有兩個用途：**取得 DOM 元素參照** 和 **儲存不觸發重新渲染的值**。

```jsx
import { useRef, useEffect } from 'react';

function AutoFocusInput() {
  const inputRef = useRef(null);     // 建立 ref

  useEffect(() => {
    inputRef.current.focus();         // 直接操作 DOM（像原生 JS）
  }, []);

  return <input ref={inputRef} placeholder=""自動聚焦"" />;
}
```

### useRef 儲存不重新渲染的值

```jsx
function Timer() {
  const [seconds, setSeconds] = useState(0);
  const intervalRef = useRef(null);    // 不會觸發重新渲染

  const start = () => {
    intervalRef.current = setInterval(() => {
      setSeconds(prev => prev + 1);
    }, 1000);
  };

  const stop = () => {
    clearInterval(intervalRef.current);  // 用 ref 存定時器 ID
  };

  return (
    <div>
      <p>{seconds} 秒</p>
      <button onClick={start}>開始</button>
      <button onClick={stop}>停止</button>
    </div>
  );
}
```

> 💡 **useState vs useRef**：
> - `useState`：值改變 → 觸發重新渲染
> - `useRef`：值改變 → **不觸發**重新渲染（適合存定時器 ID、前一次值等）

---

## 🌐 useContext — 跨元件狀態共享

不用層層傳 props，直接跨元件共享資料。

```jsx
import { createContext, useContext, useState } from 'react';

// 1. 建立 Context
const ThemeContext = createContext();

// 2. Provider 包在外層
function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');
  const toggleTheme = () =>
    setTheme(prev => prev === 'light' ? 'dark' : 'light');

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

// 3. 任何子元件都可以用 useContext 取得
function Header() {
  const { theme, toggleTheme } = useContext(ThemeContext);
  return (
    <header style={{ background: theme === 'dark' ? '#333' : '#fff' }}>
      <h1>目前主題：{theme}</h1>
      <button onClick={toggleTheme}>切換主題</button>
    </header>
  );
}

// 4. 使用
function App() {
  return (
    <ThemeProvider>
      <Header />          {/* Header 不需要接收 props */}
      <MainContent />
    </ThemeProvider>
  );
}
```

---

## ⚡ useMemo 和 useCallback — 效能優化

```jsx
import { useMemo, useCallback, useState } from 'react';

function ExpensiveComponent({ items, onItemClick }) {
  // useMemo：快取計算結果，依賴不變就不重算
  const sortedItems = useMemo(() => {
    console.log('排序中...');            // 只在 items 改變時執行
    return [...items].sort((a, b) => a.name.localeCompare(b.name));
  }, [items]);

  return (
    <ul>
      {sortedItems.map(item => (
        <li key={item.id} onClick={() => onItemClick(item.id)}>
          {item.name}
        </li>
      ))}
    </ul>
  );
}

function App() {
  const [items] = useState([{ id: 1, name: 'Banana' }, { id: 2, name: 'Apple' }]);

  // useCallback：快取函式參照，避免子元件不必要的重新渲染
  const handleClick = useCallback((id) => {
    console.log('點擊了項目', id);
  }, []);  // 空依賴 → 函式不會改變

  return <ExpensiveComponent items={items} onItemClick={handleClick} />;
}
```

---

## 🧩 自訂 Hook（Custom Hooks）

把重複邏輯抽成可重用的 Hook，名稱必須以 `use` 開頭。

```jsx
// useLocalStorage.js — 自訂 Hook
function useLocalStorage(key, initialValue) {
  const [value, setValue] = useState(() => {
    const saved = localStorage.getItem(key);
    return saved ? JSON.parse(saved) : initialValue;
  });

  useEffect(() => {
    localStorage.setItem(key, JSON.stringify(value));
  }, [key, value]);

  return [value, setValue];   // 和 useState 一樣的介面
}

// 使用自訂 Hook
function Settings() {
  const [darkMode, setDarkMode] = useLocalStorage('darkMode', false);

  return (
    <label>
      <input
        type=""checkbox""
        checked={darkMode}
        onChange={(e) => setDarkMode(e.target.checked)}
      />
      深色模式
    </label>
  );
}
```

---

## ⚠️ 常見陷阱

### 閉包陷阱（Stale Closure）

```jsx
function StaleClosureDemo() {
  const [count, setCount] = useState(0);

  useEffect(() => {
    const timer = setInterval(() => {
      // ❌ 錯誤：count 永遠是 0（閉包捕獲了初始值）
      // setCount(count + 1);

      // ✅ 正確：用函式型更新，取得最新值
      setCount(prev => prev + 1);
    }, 1000);

    return () => clearInterval(timer);
  }, []);  // 空依賴 → effect 只建立一次

  return <p>{count}</p>;
}
```

### 無限迴圈

```jsx
// ❌ 錯誤：每次渲染都建立新物件 → 觸發 useEffect → 又設定 state → 無限迴圈
useEffect(() => {
  setData({ ...data, loaded: true });
}, [data]);  // data 每次都是新物件！

// ✅ 正確：精確指定依賴，或使用函式型更新
useEffect(() => {
  setData(prev => ({ ...prev, loaded: true }));
}, []);  // 只執行一次
```

---

## ✅ 本章重點

| Hook | 用途 | 常見場景 |
|------|------|----------|
| useEffect | 副作用管理 | API 呼叫、定時器、訂閱 |
| useRef | DOM 參照 / 持久值 | 聚焦 input、存定時器 ID |
| useContext | 跨元件共享狀態 | 主題、語言、使用者資訊 |
| useMemo | 快取計算結果 | 大量資料排序 / 過濾 |
| useCallback | 快取函式參照 | 傳給子元件的回呼 |
| 自訂 Hook | 重用邏輯 | useLocalStorage、useFetch |
" },

        // ── Chapter 803: React Router ─────────────────────────────────
        new() { Id=803, Category="frontend", Order=23, Level="intermediate", Icon="🗺️", Title="React Router：客戶端路由管理", Slug="react-router", IsPublished=true, Content=@"
# 🗺️ React Router：客戶端路由管理

## 📌 SPA 路由概念

傳統網頁每切換頁面都要跟伺服器要新的 HTML。SPA（Single Page Application）只有一個 HTML，透過 JavaScript 切換畫面。

> ⚠️ **React 本身沒有路由功能！**
> React Router 是獨立的第三方套件，不是 React 內建的。
> 它透過 JavaScript 監聽 URL 變化，決定顯示哪個元件。

```bash
# 安裝 React Router v6
npm install react-router-dom
```

---

## 🛠️ 基本設定（React Router v6）

```jsx
// main.jsx — 在進入點設定路由
import { BrowserRouter } from 'react-router-dom';
import App from './App';

createRoot(document.getElementById('root')).render(
  <BrowserRouter>      {/* 包在最外層 */}
    <App />
  </BrowserRouter>
);
```

```jsx
// App.jsx — 定義路由表
import { Routes, Route, Link, Outlet } from 'react-router-dom';
import Home from './pages/Home';
import About from './pages/About';
import NotFound from './pages/NotFound';

function App() {
  return (
    <div>
      {/* 導航列 */}
      <nav>
        <Link to=""/"">首頁</Link>          {/* Link 取代 <a> 標籤 */}
        <Link to=""/about"">關於</Link>
      </nav>

      {/* 路由出口 */}
      <Routes>
        <Route path=""/"" element={<Home />} />
        <Route path=""/about"" element={<About />} />
        <Route path=""*"" element={<NotFound />} />   {/* 404 */}
      </Routes>
    </div>
  );
}
```

> 💡 **Link vs <a> 標籤**：
> - `<a href>`：會刷新整個頁面（重新載入）
> - `<Link to>`：只切換元件，不刷新頁面（SPA 體驗）

---

## 🔗 動態路由與參數

```jsx
import { useParams, useSearchParams } from 'react-router-dom';

// 路由定義
<Route path=""/users/:userId"" element={<UserDetail />} />

// 取得路由參數
function UserDetail() {
  const { userId } = useParams();     // 取得 :userId 的值

  // 取得查詢參數 ?tab=posts&page=2
  const [searchParams, setSearchParams] = useSearchParams();
  const tab = searchParams.get('tab') || 'profile';

  return (
    <div>
      <h1>使用者 #{userId}</h1>
      <div>
        <button onClick={() => setSearchParams({ tab: 'profile' })}>
          個人資料
        </button>
        <button onClick={() => setSearchParams({ tab: 'posts' })}>
          貼文
        </button>
      </div>
      {tab === 'profile' ? <Profile /> : <Posts />}
    </div>
  );
}
```

---

## 🏗️ 巢狀路由（Nested Routes）

```jsx
// App.jsx
function App() {
  return (
    <Routes>
      <Route path=""/"" element={<Layout />}>        {/* 共用版面 */}
        <Route index element={<Home />} />          {/* 預設子路由 */}
        <Route path=""dashboard"" element={<Dashboard />}>
          <Route index element={<Overview />} />
          <Route path=""settings"" element={<Settings />} />
          <Route path=""analytics"" element={<Analytics />} />
        </Route>
        <Route path=""*"" element={<NotFound />} />
      </Route>
    </Routes>
  );
}

// Layout.jsx — 共用版面配置
function Layout() {
  return (
    <div>
      <Header />
      <main>
        <Outlet />     {/* 子路由的內容渲染在這裡 */}
      </main>
      <Footer />
    </div>
  );
}

// Dashboard.jsx — 巢狀路由的父元件
function Dashboard() {
  return (
    <div>
      <h1>儀表板</h1>
      <nav>
        <Link to=""/dashboard"">概覽</Link>
        <Link to=""/dashboard/settings"">設定</Link>
        <Link to=""/dashboard/analytics"">分析</Link>
      </nav>
      <Outlet />       {/* 子路由渲染處 */}
    </div>
  );
}
```

---

## 🔒 路由保護（Protected Routes）

```jsx
import { Navigate, useLocation } from 'react-router-dom';

// 自訂保護元件
function ProtectedRoute({ children }) {
  const { user } = useAuth();                // 取得登入狀態（自訂 Hook）
  const location = useLocation();

  if (!user) {
    // 未登入 → 導向登入頁，並記住原本要去的路徑
    return <Navigate to=""/login"" state={{ from: location }} replace />;
  }

  return children;     // 已登入 → 正常顯示
}

// 使用方式
<Routes>
  <Route path=""/login"" element={<Login />} />
  <Route path=""/dashboard"" element={
    <ProtectedRoute>
      <Dashboard />
    </ProtectedRoute>
  } />
</Routes>
```

---

## 🧭 程式導航（Programmatic Navigation）

```jsx
import { useNavigate } from 'react-router-dom';

function LoginForm() {
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    const success = await login(username, password);

    if (success) {
      navigate('/dashboard');             // 導向儀表板
      // navigate(-1);                    // 回上一頁
      // navigate('/dashboard', { replace: true }); // 取代歷史紀錄
    }
  };

  return <form onSubmit={handleLogin}>...</form>;
}
```

---

## 🛠️ 完整範例：多頁面應用

```jsx
// App.jsx
import { Routes, Route, Link, Navigate } from 'react-router-dom';
import { useState } from 'react';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  return (
    <div>
      <nav style={{ display: 'flex', gap: '16px', padding: '16px' }}>
        <Link to=""/"">首頁</Link>
        <Link to=""/products"">商品</Link>
        <Link to=""/profile"">個人資料</Link>
        <button onClick={() => setIsLoggedIn(!isLoggedIn)}>
          {isLoggedIn ? '登出' : '登入'}
        </button>
      </nav>

      <Routes>
        <Route path=""/"" element={<h1>首頁</h1>} />
        <Route path=""/products"" element={<ProductList />} />
        <Route path=""/products/:id"" element={<ProductDetail />} />
        <Route path=""/profile"" element={
          isLoggedIn ? <Profile /> : <Navigate to=""/"" />
        } />
      </Routes>
    </div>
  );
}
```

---

## ✅ 本章重點

| 觀念 | 說明 |
|------|------|
| BrowserRouter | 包在最外層，啟用路由功能 |
| Routes / Route | 定義路徑與元件的對應 |
| Link | SPA 導航，不刷新頁面 |
| useParams | 取得動態路由參數（:id） |
| Outlet | 巢狀路由的渲染出口 |
| Navigate | 程式化重導向 |
| Protected Route | 路由保護模式 |
" },

        // ── Chapter 804: React 狀態管理 ─────────────────────────────────
        new() { Id=804, Category="frontend", Order=24, Level="intermediate", Icon="🏪", Title="React 狀態管理：Redux Toolkit 與 Zustand", Slug="react-state", IsPublished=true, Content=@"
# 🏪 React 狀態管理：Redux Toolkit 與 Zustand

## 📌 為什麼需要全域狀態管理？

當應用變大，元件之間共享資料變得複雜。用 props 層層傳遞（prop drilling）會讓程式碼難以維護。

```
問題示意：
App → Layout → Sidebar → UserMenu → Avatar
                                      ↑
        使用者資料要從 App 傳 4 層才到 Avatar
```

> 💡 **全域狀態管理**讓任何元件都能直接存取共享資料，不用層層傳遞。

---

## 🔧 Redux Toolkit（RTK）

Redux Toolkit 是 Redux 的官方推薦工具包，大幅簡化了 Redux 的樣板程式碼。

```bash
npm install @reduxjs/toolkit react-redux
```

### createSlice — 定義狀態和操作

```jsx
// store/cartSlice.js
import { createSlice } from '@reduxjs/toolkit';

const cartSlice = createSlice({
  name: 'cart',
  initialState: {
    items: [],
    totalAmount: 0,
  },
  reducers: {
    // 每個 reducer 自動產生對應的 action
    addItem: (state, action) => {
      const existing = state.items.find(i => i.id === action.payload.id);
      if (existing) {
        existing.quantity += 1;       // RTK 用 Immer，可以直接修改
      } else {
        state.items.push({ ...action.payload, quantity: 1 });
      }
      state.totalAmount = state.items.reduce(
        (sum, item) => sum + item.price * item.quantity, 0
      );
    },
    removeItem: (state, action) => {
      state.items = state.items.filter(i => i.id !== action.payload);
      state.totalAmount = state.items.reduce(
        (sum, item) => sum + item.price * item.quantity, 0
      );
    },
    clearCart: (state) => {
      state.items = [];
      state.totalAmount = 0;
    },
  },
});

export const { addItem, removeItem, clearCart } = cartSlice.actions;
export default cartSlice.reducer;
```

> ⚠️ **RTK 使用 Immer**：看起來像直接修改 state（`state.items.push(...)`），
> 但底層會產生新的不可變物件。這不是原生 JS 行為，是 RTK 的魔法。

### configureStore — 建立 Store

```jsx
// store/index.js
import { configureStore } from '@reduxjs/toolkit';
import cartReducer from './cartSlice';

export const store = configureStore({
  reducer: {
    cart: cartReducer,      // 註冊 slice
  },
});
```

### 在元件中使用

```jsx
// main.jsx — 用 Provider 包裝
import { Provider } from 'react-redux';
import { store } from './store';

createRoot(document.getElementById('root')).render(
  <Provider store={store}>
    <App />
  </Provider>
);
```

```jsx
// components/Cart.jsx — 讀取和操作 store
import { useSelector, useDispatch } from 'react-redux';
import { addItem, removeItem, clearCart } from '../store/cartSlice';

function Cart() {
  const { items, totalAmount } = useSelector(state => state.cart);
  const dispatch = useDispatch();

  return (
    <div>
      <h2>購物車（{items.length} 件商品）</h2>
      {items.map(item => (
        <div key={item.id}>
          <span>{item.name} x{item.quantity} — ${item.price * item.quantity}</span>
          <button onClick={() => dispatch(removeItem(item.id))}>移除</button>
        </div>
      ))}
      <p>總計：${totalAmount}</p>
      <button onClick={() => dispatch(clearCart())}>清空購物車</button>
    </div>
  );
}
```

---

## 🌊 RTK Query — 資料取得

RTK Query 自動處理快取、載入狀態、錯誤處理。

```jsx
// store/api.js
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const productApi = createApi({
  reducerPath: 'productApi',
  baseQuery: fetchBaseQuery({ baseUrl: 'https://api.example.com' }),
  endpoints: (builder) => ({
    getProducts: builder.query({
      query: () => '/products',
    }),
    getProductById: builder.query({
      query: (id) => `/products/${id}`,
    }),
  }),
});

export const { useGetProductsQuery, useGetProductByIdQuery } = productApi;
```

```jsx
// 使用自動產生的 Hook
function ProductList() {
  const { data: products, isLoading, error } = useGetProductsQuery();

  if (isLoading) return <p>載入中...</p>;
  if (error) return <p>錯誤：{error.message}</p>;

  return (
    <ul>
      {products.map(p => <li key={p.id}>{p.name} - ${p.price}</li>)}
    </ul>
  );
}
```

---

## 🐻 Zustand — 更輕量的替代方案

Zustand 比 Redux 更簡潔，適合中小型專案。

```bash
npm install zustand
```

```jsx
// store/useCartStore.js
import { create } from 'zustand';

const useCartStore = create((set, get) => ({
  items: [],
  totalAmount: 0,

  addItem: (product) => set((state) => {
    const existing = state.items.find(i => i.id === product.id);
    const newItems = existing
      ? state.items.map(i =>
          i.id === product.id ? { ...i, quantity: i.quantity + 1 } : i
        )
      : [...state.items, { ...product, quantity: 1 }];

    return {
      items: newItems,
      totalAmount: newItems.reduce((s, i) => s + i.price * i.quantity, 0),
    };
  }),

  removeItem: (id) => set((state) => {
    const newItems = state.items.filter(i => i.id !== id);
    return {
      items: newItems,
      totalAmount: newItems.reduce((s, i) => s + i.price * i.quantity, 0),
    };
  }),

  clearCart: () => set({ items: [], totalAmount: 0 }),
}));

export default useCartStore;
```

```jsx
// 使用 — 不需要 Provider！
function Cart() {
  const { items, totalAmount, removeItem, clearCart } = useCartStore();

  return (
    <div>
      <h2>購物車</h2>
      {items.map(item => (
        <div key={item.id}>
          {item.name} x{item.quantity}
          <button onClick={() => removeItem(item.id)}>移除</button>
        </div>
      ))}
      <p>總計：${totalAmount}</p>
      <button onClick={clearCart}>清空</button>
    </div>
  );
}
```

---

## 🆚 比較 Context vs Redux vs Zustand

| 特性 | Context | Redux Toolkit | Zustand |
|------|---------|---------------|---------|
| 設定複雜度 | 低 | 中 | 低 |
| 套件大小 | 0（內建） | ~11KB | ~1KB |
| 效能 | 一般（全部重新渲染） | 好（精確訂閱） | 好（精確訂閱） |
| 適合場景 | 主題、語言切換 | 大型應用、複雜邏輯 | 中小型應用 |
| 開發者工具 | 無 | Redux DevTools | Redux DevTools |
| 非同步處理 | 自己處理 | RTK Query / Thunk | 直接在 store 寫 |

> 💡 **選擇建議**：
> - 簡單共享（主題、語言）→ Context
> - 中小專案 → Zustand
> - 大型企業專案 → Redux Toolkit

---

## ✅ 本章重點

| 工具 | 核心概念 |
|------|----------|
| Redux Toolkit | createSlice + configureStore + Immer |
| RTK Query | 自動快取 + 載入狀態 + Hook |
| Zustand | create() 一個函式搞定 |
| 選擇依據 | 專案規模和團隊偏好 |
" },

        // ── Chapter 805: React 進階 ─────────────────────────────────
        new() { Id=805, Category="frontend", Order=25, Level="advanced", Icon="🚀", Title="React 進階：效能優化與設計模式", Slug="react-advanced", IsPublished=true, Content=@"
# 🚀 React 進階：效能優化與設計模式

## 📌 React.memo — 避免不必要的重新渲染

`React.memo` 是一個高階元件（HOC），當 props 沒變時跳過重新渲染。

```jsx
import { memo, useState } from 'react';

// 用 React.memo 包裝，props 相同就不重新渲染
const ExpensiveList = memo(function ExpensiveList({ items, onSelect }) {
  console.log('ExpensiveList 渲染了');  // 觀察渲染次數
  return (
    <ul>
      {items.map(item => (
        <li key={item.id} onClick={() => onSelect(item.id)}>
          {item.name}
        </li>
      ))}
    </ul>
  );
});

function App() {
  const [count, setCount] = useState(0);
  const [items] = useState([
    { id: 1, name: 'React' },
    { id: 2, name: 'Vue' },
  ]);

  // ⚠️ 如果不用 useCallback，每次渲染都建立新函式
  //    → React.memo 失效（因為 props 不同）
  const handleSelect = useCallback((id) => {
    console.log('選擇了', id);
  }, []);

  return (
    <div>
      <button onClick={() => setCount(c => c + 1)}>
        計數：{count}
      </button>
      {/* count 改變時，ExpensiveList 不會重新渲染 */}
      <ExpensiveList items={items} onSelect={handleSelect} />
    </div>
  );
}
```

---

## 🧠 虛擬 DOM Diff 算法（底層 JS 機制）

React 的效能關鍵在於它的 **Reconciliation（調和）** 算法。

```
React Diff 規則：
1. 不同類型的元素 → 整棵子樹重建
2. 同類型的元素 → 只更新改變的屬性
3. 列表元素 → 透過 key 比對（O(n) 而非 O(n³)）
```

```jsx
// 底層 JavaScript：React 如何比較虛擬 DOM
// 舊的虛擬 DOM
const oldTree = {
  type: 'div',
  props: { className: 'container' },
  children: [
    { type: 'h1', props: {}, children: ['Hello'] },
    { type: 'p',  props: {}, children: ['World'] },
  ]
};

// 新的虛擬 DOM
const newTree = {
  type: 'div',
  props: { className: 'container active' },  // className 改了
  children: [
    { type: 'h1', props: {}, children: ['Hello'] },    // 沒變
    { type: 'p',  props: {}, children: ['React!'] },   // 文字改了
  ]
};

// React 只會：
// 1. 更新 div 的 className
// 2. 更新 p 的文字內容
// 不會重建整個 DOM 樹！
```

> ⚠️ **這就是 React 不是原生 JS 的核心原因**：
> 原生 JS 操作 DOM 你需要自己追蹤哪些部分改了。
> React 透過虛擬 DOM + Diff 算法自動幫你做最小化更新。

---

## ⏳ Suspense 與 lazy loading

```jsx
import { Suspense, lazy } from 'react';

// lazy 動態載入（程式碼分割）
const AdminPanel = lazy(() => import('./pages/AdminPanel'));
const UserDashboard = lazy(() => import('./pages/UserDashboard'));

function App() {
  return (
    <Suspense fallback={<div>載入中...</div>}>
      <Routes>
        <Route path=""/admin"" element={<AdminPanel />} />
        <Route path=""/dashboard"" element={<UserDashboard />} />
      </Routes>
    </Suspense>
  );
}
```

> 💡 **為什麼要 lazy loading？**
> 使用者進入首頁時不需要載入 AdminPanel 的程式碼。
> lazy loading 只在實際訪問時才下載，減少初始載入時間。

---

## 🛡️ Error Boundaries — 錯誤邊界

Error Boundary 是目前唯一需要使用 **class component** 的場景。

```jsx
import { Component } from 'react';

class ErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };     // 更新 state 顯示錯誤 UI
  }

  componentDidCatch(error, errorInfo) {
    console.error('元件錯誤：', error, errorInfo);
    // 可以送到錯誤追蹤服務（Sentry 等）
  }

  render() {
    if (this.state.hasError) {
      return (
        <div style={{ padding: '20px', background: '#ffe0e0' }}>
          <h2>發生錯誤</h2>
          <p>{this.state.error?.message}</p>
          <button onClick={() => this.setState({ hasError: false })}>
            重試
          </button>
        </div>
      );
    }
    return this.props.children;
  }
}

// 使用
function App() {
  return (
    <ErrorBoundary>
      <Dashboard />     {/* 如果 Dashboard 出錯，顯示錯誤 UI */}
    </ErrorBoundary>
  );
}
```

---

## 🎨 常見設計模式

### HOC（Higher-Order Component）

```jsx
// withAuth HOC：包裝元件加上認證檢查
function withAuth(WrappedComponent) {
  return function AuthenticatedComponent(props) {
    const { user } = useAuth();

    if (!user) return <Navigate to=""/login"" />;
    return <WrappedComponent {...props} user={user} />;
  };
}

// 使用
const ProtectedDashboard = withAuth(Dashboard);
```

### Compound Components 模式

```jsx
// 像 HTML 的 <select> + <option> 一樣的元件 API
function Tabs({ children, defaultTab }) {
  const [activeTab, setActiveTab] = useState(defaultTab);

  return (
    <TabsContext.Provider value={{ activeTab, setActiveTab }}>
      <div className=""tabs"">{children}</div>
    </TabsContext.Provider>
  );
}

Tabs.Tab = function Tab({ id, children }) {
  const { activeTab, setActiveTab } = useContext(TabsContext);
  return (
    <button
      className={activeTab === id ? 'active' : ''}
      onClick={() => setActiveTab(id)}
    >
      {children}
    </button>
  );
};

Tabs.Panel = function Panel({ id, children }) {
  const { activeTab } = useContext(TabsContext);
  return activeTab === id ? <div>{children}</div> : null;
};

// 使用 — 直覺的 API
<Tabs defaultTab=""home"">
  <Tabs.Tab id=""home"">首頁</Tabs.Tab>
  <Tabs.Tab id=""settings"">設定</Tabs.Tab>
  <Tabs.Panel id=""home"">首頁內容</Tabs.Panel>
  <Tabs.Panel id=""settings"">設定內容</Tabs.Panel>
</Tabs>
```

---

## 🌐 React Server Components（RSC）概念

React Server Components 是 React 的最新發展方向（Next.js 13+ App Router 使用）。

```
傳統 React（Client Components）：
  伺服器 → 送 JS 到瀏覽器 → 瀏覽器執行 JS → 渲染畫面

React Server Components：
  伺服器 → 伺服器上執行元件 → 送 HTML 到瀏覽器 → 更快的首屏
```

```jsx
// Server Component（預設）— 在伺服器執行
async function ProductPage({ id }) {
  const product = await db.products.findById(id);  // 直接查資料庫
  return <ProductDetail product={product} />;
}

// Client Component — 需要互動時使用
'use client';  // 必須加這行標記
function AddToCartButton({ productId }) {
  const [added, setAdded] = useState(false);
  return (
    <button onClick={() => setAdded(true)}>
      {added ? '已加入' : '加入購物車'}
    </button>
  );
}
```

---

## ✅ 本章重點

| 技巧 | 說明 |
|------|------|
| React.memo | 避免不必要的子元件重新渲染 |
| 虛擬 DOM Diff | React 自動最小化 DOM 操作的核心機制 |
| Suspense + lazy | 程式碼分割，按需載入 |
| Error Boundary | 攔截子元件錯誤，顯示錯誤 UI |
| HOC / Compound | 常見的元件重用模式 |
| RSC | 伺服器端渲染元件，減少客戶端 JS |
" },

        // ── Chapter 806: React 測試與部署 ─────────────────────────────
        new() { Id=806, Category="frontend", Order=26, Level="advanced", Icon="🧪", Title="React 測試與部署", Slug="react-testing", IsPublished=true, Content=@"
# 🧪 React 測試與部署

## 📌 為什麼要測試？

> 「沒有測試的程式碼就是遺留程式碼」——Michael Feathers

測試確保：
- 功能正常運作
- 重構不破壞現有功能
- 團隊協作有信心
- 部署前抓到 bug

---

## 🧰 Jest + React Testing Library

Jest 是測試框架，React Testing Library 專注於**使用者行為**的測試方式。

```bash
# Vite 專案安裝
npm install -D vitest @testing-library/react @testing-library/jest-dom jsdom
```

### 基本元件測試

```jsx
// Counter.jsx
import { useState } from 'react';

export function Counter() {
  const [count, setCount] = useState(0);
  return (
    <div>
      <p data-testid=""count"">計數：{count}</p>
      <button onClick={() => setCount(c => c + 1)}>+1</button>
      <button onClick={() => setCount(0)}>歸零</button>
    </div>
  );
}
```

```jsx
// Counter.test.jsx
import { render, screen, fireEvent } from '@testing-library/react';
import { Counter } from './Counter';

describe('Counter 元件', () => {
  test('初始值為 0', () => {
    render(<Counter />);
    expect(screen.getByTestId('count')).toHaveTextContent('計數：0');
  });

  test('點擊 +1 後計數增加', () => {
    render(<Counter />);
    const button = screen.getByText('+1');
    fireEvent.click(button);
    expect(screen.getByTestId('count')).toHaveTextContent('計數：1');
  });

  test('點擊歸零後回到 0', () => {
    render(<Counter />);
    fireEvent.click(screen.getByText('+1'));
    fireEvent.click(screen.getByText('+1'));
    fireEvent.click(screen.getByText('歸零'));
    expect(screen.getByTestId('count')).toHaveTextContent('計數：0');
  });
});
```

> 💡 **React Testing Library 的理念**：
> 不測試實作細節（state 值、元件內部邏輯），而是測試**使用者看到什麼、做了什麼**。
> 這和原生 JS 的 DOM 測試不同——React 元件的測試模擬的是使用者互動。

---

## 🌐 Mock API 與非同步測試

```jsx
// UserProfile.jsx
export function UserProfile({ userId }) {
  const [user, setUser] = useState(null);

  useEffect(() => {
    fetch(`/api/users/${userId}`)
      .then(res => res.json())
      .then(setUser);
  }, [userId]);

  if (!user) return <p>載入中...</p>;
  return <h1>{user.name}</h1>;
}
```

```jsx
// UserProfile.test.jsx
import { render, screen, waitFor } from '@testing-library/react';
import { UserProfile } from './UserProfile';

// Mock fetch
beforeEach(() => {
  global.fetch = vi.fn(() =>
    Promise.resolve({
      json: () => Promise.resolve({ name: '小明', email: 'ming@test.com' }),
    })
  );
});

afterEach(() => {
  vi.restoreAllMocks();
});

test('載入使用者資料後顯示名稱', async () => {
  render(<UserProfile userId={1} />);

  // 等待非同步操作完成
  expect(screen.getByText('載入中...')).toBeInTheDocument();

  await waitFor(() => {
    expect(screen.getByText('小明')).toBeInTheDocument();
  });

  // 驗證 fetch 被正確呼叫
  expect(global.fetch).toHaveBeenCalledWith('/api/users/1');
});
```

---

## 🎭 E2E 測試（Playwright）

E2E 測試模擬真實使用者操作，在真實瀏覽器中執行。

```bash
npm install -D @playwright/test
npx playwright install
```

```javascript
// e2e/todo.spec.js
import { test, expect } from '@playwright/test';

test.describe('待辦應用', () => {
  test('可以新增和完成待辦事項', async ({ page }) => {
    await page.goto('http://localhost:5173');

    // 新增待辦
    await page.fill('input[placeholder=""輸入待辦事項...""]', '學 React');
    await page.click('button:text(""新增"")');

    // 確認待辦出現
    await expect(page.locator('li')).toContainText('學 React');

    // 點擊完成
    await page.click('li:text(""學 React"")');
    await expect(page.locator('li')).toHaveCSS(
      'text-decoration', 'line-through'
    );
  });

  test('空白待辦不可新增', async ({ page }) => {
    await page.goto('http://localhost:5173');
    await page.click('button:text(""新增"")');
    await expect(page.locator('li')).toHaveCount(0);
  });
});
```

---

## 📦 打包優化

```javascript
// vite.config.js — 打包設定
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  build: {
    rollupOptions: {
      output: {
        // 手動分割程式碼
        manualChunks: {
          vendor: ['react', 'react-dom'],
          router: ['react-router-dom'],
          redux: ['@reduxjs/toolkit', 'react-redux'],
        },
      },
    },
    // 壓縮設定
    minify: 'terser',
    sourcemap: false,     // 生產環境關閉 sourcemap
  },
});
```

```bash
# 分析打包大小
npm install -D rollup-plugin-visualizer
# 執行後會產生 stats.html，視覺化各套件佔比
```

---

## 🚀 部署到 Vercel / Netlify

### Vercel（推薦用於 React/Next.js）

```bash
# 安裝 Vercel CLI
npm install -g vercel

# 部署
vercel

# 或連結 GitHub 自動部署：
# 1. 在 vercel.com 匯入 GitHub 專案
# 2. 設定 Build Command: npm run build
# 3. 設定 Output Directory: dist
# 4. 每次 push 到 main 自動部署
```

### Netlify

```bash
# 安裝 Netlify CLI
npm install -g netlify-cli

# 建置並部署
npm run build
netlify deploy --prod --dir=dist
```

### SPA 路由設定（重要！）

```
# 在 public/ 目錄建立 _redirects 檔案（Netlify）
/*    /index.html   200

# 或 vercel.json（Vercel）
{
  ""rewrites"": [
    { ""source"": ""/(.*)"", ""destination"": ""/index.html"" }
  ]
}
```

> ⚠️ **SPA 路由注意**：React Router 的路由是前端處理的，
> 直接訪問 `/dashboard` 時伺服器找不到檔案會回 404。
> 上面的設定把所有路徑導向 index.html，讓 React Router 處理。

---

## ✅ 本章重點

| 測試類型 | 工具 | 範圍 |
|---------|------|------|
| 單元測試 | Vitest/Jest | 個別函式/Hook |
| 元件測試 | React Testing Library | 單一元件行為 |
| E2E 測試 | Playwright/Cypress | 完整使用者流程 |

| 部署平台 | 特點 |
|---------|------|
| Vercel | 最佳 Next.js 支援，自動 CI/CD |
| Netlify | 簡單快速，適合靜態站 |
| SPA 設定 | 所有路徑導向 index.html |
" },

        // ── Chapter 807: React + ASP.NET Core 全端整合 ─────────────────
        new() { Id=807, Category="frontend", Order=27, Level="advanced", Icon="🔗", Title="React + ASP.NET Core 全端整合", Slug="react-fullstack", IsPublished=true, Content=@"
# 🔗 React + ASP.NET Core 全端整合

## 📌 前後端分離架構

```
前後端分離架構：

  React (前端)              ASP.NET Core (後端)
  ┌──────────────┐         ┌──────────────────┐
  │  瀏覽器 SPA    │ ←JSON→ │  Web API          │
  │  localhost:5173│         │  localhost:5000   │
  │  JSX → UI     │         │  Controller/      │
  │  React Router │         │  Minimal API      │
  │  Zustand/Redux│         │  Entity Framework │
  └──────────────┘         └──────────────────┘
```

> ⚠️ **React 前端和 .NET 後端是完全獨立的應用**，
> 透過 HTTP API（JSON 格式）溝通。這就是為什麼 React 不是原生 JS——
> 它需要編譯打包，而 .NET 負責資料處理和商業邏輯。

---

## 🌐 Fetch / Axios 呼叫 .NET API

### 使用 Fetch（原生）

```jsx
// hooks/useApi.js
import { useState, useEffect } from 'react';

const API_BASE = import.meta.env.VITE_API_URL || 'https://localhost:5001';

export function useFetch(endpoint) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const controller = new AbortController();  // 用於取消請求

    fetch(`${API_BASE}${endpoint}`, {
      signal: controller.signal,
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
      },
    })
      .then(res => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json();
      })
      .then(setData)
      .catch(err => {
        if (err.name !== 'AbortError') setError(err);
      })
      .finally(() => setLoading(false));

    return () => controller.abort();   // 元件卸載時取消請求
  }, [endpoint]);

  return { data, loading, error };
}

// 使用
function ChapterList() {
  const { data: chapters, loading, error } = useFetch('/api/chapters');

  if (loading) return <p>載入中...</p>;
  if (error) return <p>錯誤：{error.message}</p>;

  return (
    <ul>
      {chapters.map(ch => <li key={ch.id}>{ch.title}</li>)}
    </ul>
  );
}
```

### 使用 Axios（更方便）

```bash
npm install axios
```

```jsx
// services/api.js
import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'https://localhost:5001',
  timeout: 10000,
});

// 請求攔截器：自動附加 Token
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// 回應攔截器：統一錯誤處理
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';   // Token 過期，導向登入
    }
    return Promise.reject(error);
  }
);

export default api;
```

```jsx
// 使用 axios 實例
import api from '../services/api';

async function createChapter(data) {
  const response = await api.post('/api/chapters', data);
  return response.data;
}
```

---

## 🔒 CORS 設定（ASP.NET Core 端）

```csharp
// Program.cs — 設定 CORS
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(""ReactApp"", policy =>
    {
        policy.WithOrigins(""http://localhost:5173"",      // Vite 開發伺服器
                           ""https://your-app.vercel.app"")  // 部署網址
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();     // 如果要帶 Cookie
    });
});

var app = builder.Build();
app.UseCors(""ReactApp"");             // 在 UseRouting 之後
```

> ⚠️ **什麼是 CORS？**
> 瀏覽器安全策略：前端（localhost:5173）不能直接呼叫不同網域的 API（localhost:5001）。
> CORS 設定告訴瀏覽器「這個前端是被允許的」。

---

## 🔑 JWT 認證流程

```
JWT 認證流程：
1. 使用者輸入帳密 → React 送 POST /api/auth/login
2. .NET 驗證成功 → 回傳 JWT Token
3. React 存到 localStorage
4. 之後每次 API 請求都帶上 Authorization: Bearer {token}
5. .NET 驗證 Token 是否有效
```

### React 端

```jsx
// context/AuthContext.jsx
import { createContext, useContext, useState, useEffect } from 'react';
import api from '../services/api';

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  // 啟動時檢查 Token
  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      api.get('/api/auth/me')
        .then(res => setUser(res.data))
        .catch(() => localStorage.removeItem('token'))
        .finally(() => setLoading(false));
    } else {
      setLoading(false);
    }
  }, []);

  const login = async (username, password) => {
    const res = await api.post('/api/auth/login', { username, password });
    const { token, user } = res.data;
    localStorage.setItem('token', token);
    setUser(user);
    return user;
  };

  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);
```

### ASP.NET Core 端

```csharp
// AuthController.cs
[ApiController]
[Route(""api/auth"")]
public class AuthController : ControllerBase
{
    [HttpPost(""login"")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        // 驗證帳密（簡化範例）
        var user = _db.Users.FirstOrDefault(
            u => u.Username == dto.Username);

        if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
            return Unauthorized(""帳號或密碼錯誤"");

        // 產生 JWT Token
        var token = GenerateJwtToken(user);
        return Ok(new { token, user = new { user.Id, user.Username } });
    }

    [HttpGet(""me"")]
    [Authorize]     // 需要有效 Token
    public IActionResult GetMe()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = _db.Users.Find(int.Parse(userId));
        return Ok(new { user.Id, user.Username });
    }
}
```

---

## ⚡ SignalR 即時通訊

SignalR 讓 React 和 .NET 之間建立 **WebSocket** 連線，實現即時推播。

### ASP.NET Core 端

```csharp
// Hubs/ChatHub.cs
public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        // 廣播給所有連線的客戶端
        await Clients.All.SendAsync(""ReceiveMessage"", user, message);
    }

    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }
}

// Program.cs
builder.Services.AddSignalR();
app.MapHub<ChatHub>(""/chatHub"");
```

### React 端

```bash
npm install @microsoft/signalr
```

```jsx
// hooks/useSignalR.js
import { useEffect, useRef, useState } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export function useSignalR() {
  const [messages, setMessages] = useState([]);
  const connectionRef = useRef(null);

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/chatHub', {
        accessTokenFactory: () => localStorage.getItem('token'),
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    // 監聽伺服器推播
    connection.on('ReceiveMessage', (user, message) => {
      setMessages(prev => [...prev, { user, message, time: new Date() }]);
    });

    connection.start()
      .then(() => console.log('SignalR 已連線'))
      .catch(err => console.error('SignalR 連線失敗', err));

    connectionRef.current = connection;

    return () => {
      connection.stop();
    };
  }, []);

  const sendMessage = async (user, message) => {
    await connectionRef.current?.invoke('SendMessage', user, message);
  };

  return { messages, sendMessage };
}
```

```jsx
// components/Chat.jsx
function Chat() {
  const { messages, sendMessage } = useSignalR();
  const [input, setInput] = useState('');
  const { user } = useAuth();

  const handleSend = () => {
    if (input.trim()) {
      sendMessage(user.username, input);
      setInput('');
    }
  };

  return (
    <div>
      <div style={{ height: '400px', overflowY: 'auto' }}>
        {messages.map((msg, i) => (
          <div key={i}>
            <strong>{msg.user}</strong>: {msg.message}
            <small>{msg.time.toLocaleTimeString()}</small>
          </div>
        ))}
      </div>
      <input
        value={input}
        onChange={(e) => setInput(e.target.value)}
        onKeyDown={(e) => e.key === 'Enter' && handleSend()}
      />
      <button onClick={handleSend}>送出</button>
    </div>
  );
}
```

---

## 🛠️ 完整範例：React + .NET 待辦應用結構

```
專案結構：
├── backend/                        # ASP.NET Core
│   ├── Controllers/
│   │   ├── AuthController.cs       # 認證 API
│   │   └── TodoController.cs       # CRUD API
│   ├── Models/
│   │   └── Todo.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   └── Program.cs                  # CORS + SignalR + JWT 設定
│
├── frontend/                       # React (Vite)
│   ├── src/
│   │   ├── components/
│   │   │   ├── TodoList.jsx
│   │   │   └── Chat.jsx
│   │   ├── context/
│   │   │   └── AuthContext.jsx
│   │   ├── hooks/
│   │   │   ├── useApi.js
│   │   │   └── useSignalR.js
│   │   ├── services/
│   │   │   └── api.js              # Axios 實例
│   │   ├── pages/
│   │   │   ├── Login.jsx
│   │   │   └── Dashboard.jsx
│   │   ├── App.jsx                 # 路由設定
│   │   └── main.jsx                # 進入點
│   └── package.json
```

---

## ✅ 本章重點

| 主題 | 技術 |
|------|------|
| API 呼叫 | Fetch / Axios + 攔截器 |
| CORS | .NET 端 AddCors 設定 |
| JWT 認證 | login → 存 Token → 每次請求帶上 |
| 即時通訊 | SignalR Hub + @microsoft/signalr |
| 專案結構 | frontend/ + backend/ 分離 |

> 💡 **React + ASP.NET Core 是業界常見的全端組合**，
> React 負責 UI 和使用者互動，.NET 負責 API、資料庫和商業邏輯。
> 兩者透過 JSON API 溝通，各自獨立開發和部署。
" },
    };
}
