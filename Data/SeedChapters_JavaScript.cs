using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_JavaScript
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── 1500: JavaScript 簡介 ──
        new() { Id=1500, Category="javascript", Order=1, Level="beginner", Icon="🟨", Title="JavaScript 簡介與執行環境", Slug="js-intro", IsPublished=true, Content=@"
# JavaScript 簡介與執行環境

## JavaScript 是什麼？

> **比喻：如果 HTML 是房子的骨架，CSS 是裝潢，那 JavaScript 就是電力系統** ⚡
>
> 沒有 JavaScript，網頁就像一棟沒有電的房子——
> 燈不會亮、門不會自動開、電梯不會動。
> JavaScript 讓網頁「活」起來。

---

## JavaScript 可以做什麼？

```
前端（瀏覽器）：
├── 動態更新頁面內容（不用重新載入）
├── 表單驗證（即時檢查格式）
├── 動畫效果
├── 與後端 API 通訊
└── 遊戲、圖表、互動功能

後端（Node.js）：
├── 建立 Web 伺服器
├── 存取資料庫
├── 檔案操作
└── 即時通訊（WebSocket）
```

---

## 在瀏覽器中執行 JavaScript

### 方式 1：HTML 內嵌

```html
<!DOCTYPE html>
<html>
<body>
    <h1 id=""title"">Hello</h1>

    <script>
        // ← 這裡就是 JavaScript 程式碼
        // 按 F12 打開瀏覽器開發者工具 → Console 可以看到輸出
        console.log(""Hello JavaScript!"");  // ← 在 Console 印出文字
    </script>
</body>
</html>
```

### 方式 2：外部檔案（推薦）

```html
<!-- HTML 檔案 -->
<script src=""app.js""></script>  <!-- ← 引入外部 JS 檔案 -->
```

```javascript
// app.js
console.log(""我是外部的 JS 檔案"");
```

### 方式 3：瀏覽器 Console

```
按 F12 → 點 Console 分頁 → 直接輸入 JavaScript 程式碼
> 1 + 1
< 2
> ""Hello"".toUpperCase()
< ""HELLO""
```

---

## 第一個程式

```javascript
// console.log() — 在 Console 印出資訊
console.log(""Hello, World!"");      // ← 印出字串
console.log(42);                    // ← 印出數字
console.log(true);                  // ← 印出布林值
console.log(1 + 2);                 // ← 印出計算結果：3

// alert() — 彈出對話框（會暫停程式）
alert(""歡迎光臨！"");                // ← 瀏覽器彈窗

// prompt() — 請使用者輸入
let name = prompt(""你叫什麼名字？""); // ← 輸入框
console.log(""你好，"" + name);        // ← 用輸入的值
```

---

## 程式碼註解

```javascript
// 單行註解：這行不會被執行

/*
   多行註解：
   這裡面的內容
   都不會被執行
*/

console.log(""這行會執行"");
// console.log(""這行不會執行"");  ← 被註解掉了
```

---

## 嚴格模式

```javascript
""use strict"";                      // ← 放在檔案最上面
                                    // 開啟嚴格模式，幫你抓更多錯誤

x = 5;  // ❌ 嚴格模式下會報錯：x 沒有被宣告
let x = 5;  // ✅ 正確：先宣告再使用
```

> 💡 現代 JavaScript（ES6 模組）預設就是嚴格模式。
" },

        // ── 1501: 變數與型態 ──
        new() { Id=1501, Category="javascript", Order=2, Level="beginner", Icon="📦", Title="變數宣告與資料型態", Slug="js-variables", IsPublished=true, Content=@"
# 變數宣告與資料型態

## 三種宣告方式

```javascript
var name = ""小明"";    // ← 舊式宣告（不推薦，有作用域問題）
let age = 20;         // ← 現代宣告（可以改值）
const PI = 3.14;      // ← 常數宣告（不能改值）
```

逐行解析：
```
var name = ""小明"";   // var 是 ES5 的宣告方式，作用域是「函數」
                      // 問題：在 if/for 裡宣告的 var 會洩漏到外面
let age = 20;         // let 是 ES6 的宣告方式，作用域是「區塊 {}」
                      // ✅ 推薦使用 let
const PI = 3.14;      // const 也是 ES6，宣告後不能重新賦值
                      // ✅ 不會改的值用 const
```

### var 的問題

```javascript
// var 的作用域是「函數」，不是「區塊」
if (true) {
    var x = 10;       // ← 用 var 宣告
}
console.log(x);       // ← 10（洩漏到 if 外面了！）

if (true) {
    let y = 10;       // ← 用 let 宣告
}
console.log(y);       // ← ❌ ReferenceError（let 限制在 {} 內）
```

---

## 七種基本型態

### 1. Number（數字）

```javascript
let integer = 42;           // ← 整數
let float = 3.14;           // ← 浮點數（JS 不區分整數和小數）
let negative = -10;         // ← 負數
let infinity = Infinity;    // ← 無限大
let notANumber = NaN;       // ← Not a Number（非數字）

// 特殊值
console.log(0.1 + 0.2);         // ← 0.30000000000000004（浮點數精度問題！）
console.log(10 / 0);            // ← Infinity
console.log(""hello"" * 2);       // ← NaN（字串不能乘）
console.log(typeof 42);         // ← ""number""
```

### 2. String（字串）

```javascript
let single = '單引號';          // ← 單引號
let double = ""雙引號"";          // ← 雙引號
let backtick = `模板字串`;      // ← 反引號（ES6 模板字串）

// 模板字串可以嵌入變數
let name = ""小明"";
let greeting = `你好，${name}！今年 ${20 + 1} 歲。`;
// → ""你好，小明！今年 21 歲。""

// 字串方法
console.log(""Hello"".length);       // ← 5
console.log(""Hello"".toUpperCase()); // ← ""HELLO""
console.log(""Hello"".includes(""ell"")); // ← true
console.log(""Hello"".slice(1, 3));  // ← ""el""（從位置 1 到 3）
```

### 3. Boolean（布林）

```javascript
let isActive = true;
let isDeleted = false;

// 假值（Falsy）— 會被判斷為 false
console.log(Boolean(0));         // false
console.log(Boolean(""""));        // false（空字串）
console.log(Boolean(null));      // false
console.log(Boolean(undefined)); // false
console.log(Boolean(NaN));       // false

// 其他都是真值（Truthy）
console.log(Boolean(1));         // true
console.log(Boolean(""hello""));   // true
console.log(Boolean([]));        // true（空陣列也是 true！）
```

### 4-7. 其他型態

```javascript
let empty = null;               // ← null：刻意設為「空」
let notDefined = undefined;     // ← undefined：還沒給值
let id = Symbol(""id"");          // ← Symbol：唯一識別碼（ES6）
let big = 9007199254740991n;    // ← BigInt：超大整數（ES2020）

console.log(typeof null);       // ← ""object""（這是 JS 的歷史 bug！）
console.log(typeof undefined);  // ← ""undefined""
```

---

## 型態轉換

```javascript
// 字串 → 數字
Number(""42"")         // ← 42
parseInt(""42.5"")     // ← 42（取整數）
parseFloat(""42.5"")   // ← 42.5
+""42""                 // ← 42（一元加號轉換）

// 數字 → 字串
String(42)           // ← ""42""
(42).toString()      // ← ""42""
42 + """"              // ← ""42""（加空字串）

// ⚠️ 隱式轉換陷阱
console.log(""5"" + 3);    // ← ""53""（字串串接！）
console.log(""5"" - 3);    // ← 2（減法會轉數字）
console.log(""5"" == 5);   // ← true（== 會隱式轉換）
console.log(""5"" === 5);  // ← false（=== 嚴格比較，不轉換）
```

> 💡 **永遠用 `===` 而不是 `==`**，避免隱式轉換的坑。
" },

        // ── 1502: 運算子與流程控制 ──
        new() { Id=1502, Category="javascript", Order=3, Level="beginner", Icon="🔀", Title="運算子與流程控制", Slug="js-operators-control", IsPublished=true, Content=@"
# 運算子與流程控制

## 算術運算子

```javascript
let a = 10, b = 3;

console.log(a + b);    // ← 13（加）
console.log(a - b);    // ← 7（減）
console.log(a * b);    // ← 30（乘）
console.log(a / b);    // ← 3.3333...（除）
console.log(a % b);    // ← 1（取餘數：10 ÷ 3 = 3 餘 1）
console.log(a ** b);   // ← 1000（次方：10³）

// 遞增 / 遞減
let x = 5;
x++;                   // ← x 變成 6（等同 x = x + 1）
x--;                   // ← x 變回 5
```

---

## 比較運算子

```javascript
console.log(5 == ""5"");    // ← true（寬鬆比較，會轉型）
console.log(5 === ""5"");   // ← false（嚴格比較，型態不同）
console.log(5 !== ""5"");   // ← true（嚴格不等於）

console.log(5 > 3);      // ← true
console.log(5 >= 5);     // ← true
console.log(5 < 3);      // ← false
```

---

## 邏輯運算子

```javascript
// && AND — 兩個都 true 才 true
console.log(true && true);    // true
console.log(true && false);   // false

// || OR — 其中一個 true 就 true
console.log(false || true);   // true
console.log(false || false);  // false

// ! NOT — 反轉
console.log(!true);           // false
console.log(!0);              // true（0 是 falsy）

// 短路求值
let user = null;
let name = user && user.name;     // ← user 是 null → 直接回傳 null，不會報錯
let fallback = name || ""訪客"";    // ← name 是 falsy → 回傳 ""訪客""

// ?? 空值合併（ES2020）
let value = null ?? ""預設值"";     // ← ""預設值""（只在 null/undefined 時觸發）
let zero = 0 ?? ""預設值"";         // ← 0（0 不是 null/undefined）
// 對比 ||：
let zeroOr = 0 || ""預設值"";       // ← ""預設值""（因為 0 是 falsy）
```

---

## if / else

```javascript
let score = 85;

if (score >= 90) {                // ← 條件 1
    console.log(""優秀"");
} else if (score >= 60) {         // ← 條件 2
    console.log(""及格"");
} else {                          // ← 以上都不符
    console.log(""不及格"");
}

// 三元運算子（簡化的 if-else）
let result = score >= 60 ? ""及格"" : ""不及格"";
// 等同於 if (score >= 60) result = ""及格""; else result = ""不及格"";
```

---

## switch

```javascript
let day = ""Monday"";

switch (day) {
    case ""Monday"":
    case ""Tuesday"":
    case ""Wednesday"":
    case ""Thursday"":
    case ""Friday"":
        console.log(""工作日"");
        break;                    // ← break 跳出，不加會繼續往下執行
    case ""Saturday"":
    case ""Sunday"":
        console.log(""週末"");
        break;
    default:
        console.log(""未知"");
}
```

---

## for 迴圈

```javascript
// 基本 for
for (let i = 0; i < 5; i++) {    // ← i 從 0 到 4
    console.log(i);               // 輸出：0, 1, 2, 3, 4
}

// for...of（遍歷值）— 用於陣列
let fruits = [""蘋果"", ""香蕉"", ""橘子""];
for (let fruit of fruits) {       // ← fruit 是每個元素的值
    console.log(fruit);           // 蘋果, 香蕉, 橘子
}

// for...in（遍歷鍵）— 用於物件
let person = { name: ""小明"", age: 20 };
for (let key in person) {         // ← key 是屬性名稱
    console.log(`${key}: ${person[key]}`);
    // name: 小明, age: 20
}
```

---

## while 迴圈

```javascript
// while — 條件成立就執行
let count = 0;
while (count < 3) {               // ← 先檢查條件
    console.log(count);           // 0, 1, 2
    count++;
}

// do...while — 至少執行一次
let num = 10;
do {
    console.log(num);             // 10（至少執行一次）
    num++;
} while (num < 3);               // ← 條件不符，但已經執行過了
```

---

## break 和 continue

```javascript
// break — 跳出迴圈
for (let i = 0; i < 10; i++) {
    if (i === 5) break;           // ← i 到 5 就停
    console.log(i);               // 0, 1, 2, 3, 4
}

// continue — 跳過本次，繼續下一次
for (let i = 0; i < 5; i++) {
    if (i === 2) continue;        // ← 跳過 i=2
    console.log(i);               // 0, 1, 3, 4
}
```
" },

        // ── 1503: 函式 ──
        new() { Id=1503, Category="javascript", Order=4, Level="beginner", Icon="🔧", Title="函式（Function）", Slug="js-functions", IsPublished=true, Content=@"
# 函式（Function）

## 什麼是函式？

> **比喻：函式就像一台果汁機** 🍹
>
> 放入水果（參數）→ 果汁機運作（執行程式）→ 產出果汁（回傳值）。
> 你可以重複使用這台果汁機，不用每次都重新造一台。

---

## 函式宣告

```javascript
// 方式 1：函式宣告（Function Declaration）
function greet(name) {            // ← 函式名稱 + 參數
    return `你好，${name}！`;      // ← return 回傳結果
}
console.log(greet(""小明""));       // ← ""你好，小明！""

// 方式 2：函式表達式（Function Expression）
const greet2 = function(name) {
    return `你好，${name}！`;
};

// 方式 3：箭頭函式（Arrow Function，ES6）
const greet3 = (name) => {
    return `你好，${name}！`;
};

// 箭頭函式簡寫（只有一行 return 時）
const greet4 = (name) => `你好，${name}！`;
// 只有一個參數時可以省略括號
const greet5 = name => `你好，${name}！`;
```

---

## 參數

```javascript
// 預設參數（ES6）
function greet(name = ""訪客"") {   // ← 沒傳的話用預設值
    return `你好，${name}！`;
}
console.log(greet());             // ← ""你好，訪客！""
console.log(greet(""小明""));       // ← ""你好，小明！""

// 剩餘參數（Rest Parameters）
function sum(...numbers) {        // ← ...numbers 收集所有參數成陣列
    let total = 0;
    for (let n of numbers) {
        total += n;
    }
    return total;
}
console.log(sum(1, 2, 3));        // ← 6
console.log(sum(1, 2, 3, 4, 5)); // ← 15

// 解構參數
function createUser({ name, age, email = """" }) {
    console.log(`${name}, ${age}歲`);
}
createUser({ name: ""小明"", age: 20 });
```

---

## 回傳值

```javascript
// 單一回傳值
function add(a, b) {
    return a + b;                 // ← 回傳結果後，函式立刻結束
    console.log(""這行不會執行"");   // ← return 後的程式碼不會執行
}

// 回傳物件
function createUser(name, age) {
    return { name, age };         // ← ES6 簡寫：{ name: name, age: age }
}
let user = createUser(""小明"", 20);
console.log(user.name);          // ← ""小明""

// 沒有 return → 回傳 undefined
function doSomething() {
    console.log(""做了一些事"");
    // 沒有 return
}
let result = doSomething();       // ← undefined
```

---

## 作用域（Scope）

```javascript
let global = ""我是全域變數"";      // ← 全域：到處都能用

function outer() {
    let outerVar = ""我是外層變數""; // ← 函式作用域

    function inner() {
        let innerVar = ""我是內層變數"";
        console.log(global);     // ✅ 可以存取全域
        console.log(outerVar);   // ✅ 可以存取外層
        console.log(innerVar);   // ✅ 可以存取自己的
    }

    inner();
    console.log(innerVar);       // ❌ Error：innerVar 在這裡不存在
}
```

---

## 閉包（Closure）

```javascript
function createCounter() {
    let count = 0;                // ← 這個變數被「關」在閉包裡

    return {
        increment: () => ++count, // ← 可以存取外層的 count
        getCount: () => count     // ← 可以讀取 count
    };
}

const counter = createCounter();
console.log(counter.increment()); // ← 1
console.log(counter.increment()); // ← 2
console.log(counter.getCount());  // ← 2
// 外部無法直接存取 count，只能透過回傳的方法
```

---

## 高階函式

```javascript
// 函式可以作為參數傳遞
function doOperation(a, b, operation) {
    return operation(a, b);       // ← 呼叫傳入的函式
}

const add = (a, b) => a + b;
const multiply = (a, b) => a * b;

console.log(doOperation(5, 3, add));       // ← 8
console.log(doOperation(5, 3, multiply));  // ← 15

// 函式可以作為回傳值
function multiplier(factor) {
    return (num) => num * factor;  // ← 回傳一個新函式
}
const double = multiplier(2);
const triple = multiplier(3);
console.log(double(5));           // ← 10
console.log(triple(5));           // ← 15
```
" },

        // ── 1504: 陣列 ──
        new() { Id=1504, Category="javascript", Order=5, Level="beginner", Icon="📋", Title="陣列（Array）基礎與方法", Slug="js-arrays", IsPublished=true, Content=@"
# 陣列（Array）基礎與方法

## 建立陣列

```javascript
// 字面值（最常用）
let fruits = [""蘋果"", ""香蕉"", ""橘子""];

// 存取元素（索引從 0 開始）
console.log(fruits[0]);          // ← ""蘋果""（第一個）
console.log(fruits[2]);          // ← ""橘子""（第三個）
console.log(fruits.length);      // ← 3（陣列長度）

// 修改元素
fruits[1] = ""芒果"";             // ← 把""香蕉""改成""芒果""
```

---

## 增刪方法

```javascript
let arr = [1, 2, 3];

// 尾部操作
arr.push(4);            // ← [1, 2, 3, 4]（尾部新增）
arr.pop();              // ← [1, 2, 3]（尾部移除，回傳 4）

// 頭部操作
arr.unshift(0);         // ← [0, 1, 2, 3]（頭部新增）
arr.shift();            // ← [1, 2, 3]（頭部移除，回傳 0）

// splice — 萬用刀（新增/刪除/取代）
let colors = [""紅"", ""橙"", ""黃"", ""綠"", ""藍""];

// splice(起始位置, 刪除幾個, ...要插入的元素)
colors.splice(2, 1);                // ← 從位置 2 刪 1 個：[""紅"",""橙"",""綠"",""藍""]
colors.splice(1, 0, ""粉"");         // ← 在位置 1 插入：[""紅"",""粉"",""橙"",""綠"",""藍""]
colors.splice(1, 1, ""紫"", ""白"");   // ← 位置 1 刪 1 個再插入 2 個
```

---

## 搜尋方法

```javascript
let nums = [10, 20, 30, 20, 40];

nums.indexOf(20);        // ← 1（第一個 20 的位置）
nums.lastIndexOf(20);    // ← 3（最後一個 20 的位置）
nums.includes(30);       // ← true（有沒有 30）

// find — 找到第一個符合條件的元素
let users = [
    { name: ""小明"", age: 20 },
    { name: ""小華"", age: 25 },
    { name: ""小美"", age: 20 }
];
let found = users.find(u => u.age === 25);
console.log(found);      // ← { name: ""小華"", age: 25 }

// findIndex — 找到第一個符合條件的索引
let idx = users.findIndex(u => u.name === ""小美"");
console.log(idx);        // ← 2
```

---

## 遍歷方法

### forEach

```javascript
let fruits = [""蘋果"", ""香蕉"", ""橘子""];
fruits.forEach((fruit, index) => {   // ← 對每個元素執行
    console.log(`${index}: ${fruit}`);
});
// 0: 蘋果
// 1: 香蕉
// 2: 橘子
```

### map — 轉換

```javascript
let nums = [1, 2, 3, 4, 5];
let doubled = nums.map(n => n * 2);  // ← 每個元素 ×2，回傳新陣列
console.log(doubled);    // ← [2, 4, 6, 8, 10]
console.log(nums);       // ← [1, 2, 3, 4, 5]（原陣列不變！）
```

### filter — 過濾

```javascript
let nums = [1, 2, 3, 4, 5, 6];
let evens = nums.filter(n => n % 2 === 0);  // ← 保留偶數
console.log(evens);      // ← [2, 4, 6]
```

### reduce — 累計

```javascript
let nums = [1, 2, 3, 4, 5];
let sum = nums.reduce((acc, curr) => {
    // acc = 累計器（上一次的結果）
    // curr = 當前元素
    return acc + curr;
}, 0);                   // ← 0 是初始值
console.log(sum);        // ← 15

// 執行過程：
// acc=0, curr=1 → 0+1=1
// acc=1, curr=2 → 1+2=3
// acc=3, curr=3 → 3+3=6
// acc=6, curr=4 → 6+4=10
// acc=10, curr=5 → 10+5=15
```

---

## 排序

```javascript
// sort 預設按字串排序（注意！）
let nums = [10, 2, 30, 4, 5];
nums.sort();             // ← [10, 2, 30, 4, 5] → 字串排序！
console.log(nums);       // ← [10, 2, 30, 4, 5]（10 < 2？因為 ""1"" < ""2""）

// ✅ 正確的數字排序
nums.sort((a, b) => a - b);  // ← 升冪：[2, 4, 5, 10, 30]
nums.sort((a, b) => b - a);  // ← 降冪：[30, 10, 5, 4, 2]
```

---

## 其他實用方法

```javascript
// 展開運算子（Spread）
let arr1 = [1, 2, 3];
let arr2 = [4, 5, 6];
let combined = [...arr1, ...arr2];  // ← [1, 2, 3, 4, 5, 6]
let copy = [...arr1];               // ← 淺拷貝

// 解構賦值
let [first, second, ...rest] = [1, 2, 3, 4, 5];
console.log(first);    // ← 1
console.log(second);   // ← 2
console.log(rest);     // ← [3, 4, 5]

// flat — 攤平巢狀陣列
let nested = [1, [2, [3, [4]]]];
console.log(nested.flat());      // ← [1, 2, [3, [4]]]（攤一層）
console.log(nested.flat(Infinity)); // ← [1, 2, 3, 4]（全部攤平）

// 鏈式呼叫
let result = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    .filter(n => n % 2 === 0)    // ← 過濾偶數：[2, 4, 6, 8, 10]
    .map(n => n * 10)            // ← 乘 10：[20, 40, 60, 80, 100]
    .reduce((acc, n) => acc + n, 0);  // ← 加總：300
```
" },

        // ── 1505: 物件 ──
        new() { Id=1505, Category="javascript", Order=6, Level="beginner", Icon="🏠", Title="物件（Object）", Slug="js-objects", IsPublished=true, Content=@"
# 物件（Object）

## 什麼是物件？

> **比喻：物件就像一張個人名片** 📇
>
> 名片上有：姓名、電話、公司、職位...
> 物件就是用「屬性名：值」的方式組織資料。

---

## 建立物件

```javascript
// 物件字面值
let person = {
    name: ""小明"",              // ← 屬性名: 值
    age: 20,
    email: ""ming@test.com"",
    isStudent: true,
    hobbies: [""籃球"", ""吉他""],  // ← 值可以是陣列
    address: {                  // ← 值可以是另一個物件
        city: ""台北"",
        district: ""信義區""
    }
};
```

---

## 存取屬性

```javascript
// 點記法
console.log(person.name);       // ← ""小明""
console.log(person.address.city); // ← ""台北""

// 括號記法（屬性名是變數或含特殊字元時用）
console.log(person[""name""]);     // ← ""小明""

let key = ""age"";
console.log(person[key]);       // ← 20（用變數當 key）

// 修改
person.age = 21;
person[""email""] = ""new@test.com"";

// 新增
person.phone = ""0912345678"";

// 刪除
delete person.phone;
```

---

## 物件方法

```javascript
let calculator = {
    value: 0,

    // 方法（ES6 簡寫）
    add(n) {
        this.value += n;         // ← this 指向這個物件自己
        return this;             // ← 回傳自己，可以鏈式呼叫
    },
    subtract(n) {
        this.value -= n;
        return this;
    },
    getResult() {
        return this.value;
    }
};

let result = calculator.add(10).subtract(3).add(5).getResult();
console.log(result);            // ← 12
```

---

## 解構賦值

```javascript
let { name, age, email = ""無"" } = person;
// 等同於：
// let name = person.name;
// let age = person.age;
// let email = person.email ?? ""無"";

console.log(name);              // ← ""小明""

// 重新命名
let { name: userName, age: userAge } = person;
console.log(userName);          // ← ""小明""

// 巢狀解構
let { address: { city } } = person;
console.log(city);              // ← ""台北""
```

---

## 展開運算子

```javascript
// 複製物件（淺拷貝）
let copy = { ...person };

// 合併物件
let defaults = { theme: ""dark"", lang: ""zh"" };
let userPrefs = { lang: ""en"" };
let settings = { ...defaults, ...userPrefs };
// ← { theme: ""dark"", lang: ""en"" }（後面的覆蓋前面的）
```

---

## 常用靜態方法

```javascript
let obj = { a: 1, b: 2, c: 3 };

// Object.keys — 取所有 key
Object.keys(obj);              // ← [""a"", ""b"", ""c""]

// Object.values — 取所有 value
Object.values(obj);            // ← [1, 2, 3]

// Object.entries — 取 [key, value] 配對
Object.entries(obj);           // ← [[""a"",1], [""b"",2], [""c"",3]]

// 搭配 for...of
for (let [key, value] of Object.entries(obj)) {
    console.log(`${key}: ${value}`);
}

// Object.assign — 合併（會修改第一個參數）
let target = { a: 1 };
Object.assign(target, { b: 2 }, { c: 3 });
console.log(target);           // ← { a: 1, b: 2, c: 3 }

// Object.freeze — 凍結（不能改）
let frozen = Object.freeze({ x: 1, y: 2 });
frozen.x = 99;                 // ← 靜默失敗（嚴格模式下會報錯）
console.log(frozen.x);         // ← 1（沒有改到）
```

---

## Optional Chaining（?.）

```javascript
let user = {
    name: ""小明"",
    address: null
};

// 沒有 ?. 會報錯
// console.log(user.address.city);  // ❌ TypeError!

// 有 ?. 安全存取
console.log(user.address?.city);    // ← undefined（不報錯）
console.log(user.profile?.avatar);  // ← undefined

// 搭配方法呼叫
user.greet?.();                     // ← 如果 greet 方法存在才呼叫
```
" },

        // ── 1506: DOM 操作 ──
        new() { Id=1506, Category="javascript", Order=7, Level="beginner", Icon="🌳", Title="DOM 操作", Slug="js-dom", IsPublished=true, Content=@"
# DOM 操作

## 什麼是 DOM？

> **比喻：DOM 就像一棵樹** 🌳
>
> HTML 文件被瀏覽器解析成一棵樹狀結構，
> 每個標籤都是樹上的一個節點。
> JavaScript 透過 DOM 操作這棵樹，改變網頁的內容和外觀。

```
document
└── html
    ├── head
    │   └── title
    └── body
        ├── h1
        ├── p
        └── div
            ├── span
            └── a
```

---

## 選取元素

```javascript
// 用 ID 選取（回傳一個元素）
let title = document.getElementById(""title"");

// 用 CSS 選擇器選取（回傳第一個符合的）
let btn = document.querySelector("".btn-primary"");
let nav = document.querySelector(""nav > ul > li:first-child"");

// 用 CSS 選擇器選取所有符合的（回傳 NodeList）
let items = document.querySelectorAll("".item"");
// NodeList 可以用 forEach
items.forEach(item => console.log(item.textContent));

// 其他選取方式
let inputs = document.getElementsByClassName(""form-input"");  // HTMLCollection
let paragraphs = document.getElementsByTagName(""p"");          // HTMLCollection
```

---

## 修改內容

```javascript
let el = document.querySelector(""#title"");

// textContent — 純文字
el.textContent = ""新標題"";            // ← 設定文字（安全，不解析 HTML）

// innerHTML — HTML 內容
el.innerHTML = ""<strong>粗體標題</strong>""; // ← 設定 HTML（⚠️ 小心 XSS）

// ⚠️ XSS 風險！
let userInput = ""<img src=x onerror=alert('hack')>"";
el.innerHTML = userInput;              // ❌ 危險！會執行惡意程式碼
el.textContent = userInput;            // ✅ 安全！只顯示純文字
```

---

## 修改樣式

```javascript
let box = document.querySelector("".box"");

// 直接修改 style（行內樣式）
box.style.backgroundColor = ""#ff0000"";  // ← 注意：CSS 的 background-color
box.style.fontSize = ""20px"";            //         變成 JS 的 fontSize
box.style.display = ""none"";             // ← 隱藏

// 用 classList 操作 CSS class（推薦）
box.classList.add(""active"");             // ← 新增 class
box.classList.remove(""active"");          // ← 移除 class
box.classList.toggle(""active"");          // ← 有就移除，沒有就新增
box.classList.contains(""active"");        // ← 有沒有這個 class：true/false
```

---

## 修改屬性

```javascript
let link = document.querySelector(""a"");

// getAttribute / setAttribute
link.getAttribute(""href"");              // ← 讀取 href 屬性
link.setAttribute(""href"", ""https://example.com"");  // ← 設定
link.removeAttribute(""target"");         // ← 移除

// data-* 自訂屬性
// <div data-user-id=""123"" data-role=""admin"">
let div = document.querySelector(""div"");
console.log(div.dataset.userId);       // ← ""123""
console.log(div.dataset.role);         // ← ""admin""
div.dataset.status = ""active"";         // ← 新增 data-status=""active""
```

---

## 建立與刪除元素

```javascript
// 建立新元素
let newDiv = document.createElement(""div"");      // ← 建立 <div>
newDiv.textContent = ""我是新元素"";
newDiv.classList.add(""card"");

// 插入到頁面
let container = document.querySelector(""#container"");
container.appendChild(newDiv);                    // ← 加到最後面
container.prepend(newDiv);                        // ← 加到最前面
container.insertBefore(newDiv, container.firstChild); // ← 指定位置前

// 刪除元素
let old = document.querySelector("".old-item"");
old.remove();                                     // ← 直接移除自己
// 或
old.parentNode.removeChild(old);                  // ← 透過父元素移除
```

---

## 實用範例

```javascript
// 動態建立清單
function renderList(items, containerId) {
    let container = document.getElementById(containerId);
    container.innerHTML = """";                     // 清空容器

    items.forEach(item => {
        let li = document.createElement(""li"");
        li.textContent = item;
        li.classList.add(""list-item"");
        container.appendChild(li);
    });
}

renderList([""React"", ""Vue"", ""Angular""], ""framework-list"");
```
" },

        // ── 1507: 事件處理 ──
        new() { Id=1507, Category="javascript", Order=8, Level="beginner", Icon="🖱️", Title="事件處理（Event）", Slug="js-events", IsPublished=true, Content=@"
# 事件處理（Event）

## 什麼是事件？

> **比喻：事件就像門鈴** 🔔
>
> 有人按門鈴（事件發生）→ 你去開門（事件處理函數）。
> 使用者的每個動作（點擊、輸入、滾動）都是一個事件。

---

## 綁定事件

```javascript
let btn = document.querySelector(""#myBtn"");

// 方式 1：addEventListener（推薦）
btn.addEventListener(""click"", function() {
    console.log(""按鈕被點擊了！"");
});

// 方式 2：addEventListener + 箭頭函式
btn.addEventListener(""click"", () => {
    console.log(""按鈕被點擊了！"");
});

// 方式 3：用具名函式（方便移除）
function handleClick() {
    console.log(""按鈕被點擊了！"");
}
btn.addEventListener(""click"", handleClick);
btn.removeEventListener(""click"", handleClick);  // ← 移除事件
```

---

## 事件物件（Event Object）

```javascript
btn.addEventListener(""click"", function(event) {  // ← event 是事件物件
    console.log(event.type);          // ← ""click""（事件類型）
    console.log(event.target);        // ← 被點擊的元素
    console.log(event.clientX);       // ← 滑鼠 X 座標
    console.log(event.clientY);       // ← 滑鼠 Y 座標
    event.preventDefault();           // ← 阻止預設行為（如表單送出）
    event.stopPropagation();          // ← 阻止事件冒泡
});
```

---

## 常見事件類型

### 滑鼠事件

```javascript
el.addEventListener(""click"", handler);        // 單擊
el.addEventListener(""dblclick"", handler);      // 雙擊
el.addEventListener(""mouseenter"", handler);    // 滑鼠移入
el.addEventListener(""mouseleave"", handler);    // 滑鼠移出
el.addEventListener(""mousemove"", handler);     // 滑鼠移動
```

### 鍵盤事件

```javascript
document.addEventListener(""keydown"", (e) => {
    console.log(e.key);              // ← 按了什麼鍵（""Enter""、""a""、""Escape""）
    console.log(e.code);             // ← 鍵碼（""KeyA""、""ArrowUp""）
    console.log(e.ctrlKey);          // ← 是否按住 Ctrl
    console.log(e.shiftKey);         // ← 是否按住 Shift
});
```

### 表單事件

```javascript
let input = document.querySelector(""input"");
let form = document.querySelector(""form"");

input.addEventListener(""input"", (e) => {      // ← 每次輸入都觸發
    console.log(e.target.value);
});

input.addEventListener(""change"", (e) => {     // ← 失去焦點時觸發
    console.log(e.target.value);
});

form.addEventListener(""submit"", (e) => {
    e.preventDefault();                         // ← 阻止表單送出（自己處理）
    let formData = new FormData(form);
    console.log(formData.get(""username""));
});
```

---

## 事件委派（Event Delegation）

```javascript
// ❌ 壞：對每個 li 都綁事件
document.querySelectorAll(""li"").forEach(li => {
    li.addEventListener(""click"", () => { /* ... */ });
});
// 問題：如果動態新增 li，新的 li 不會有事件

// ✅ 好：在父元素上用事件委派
document.querySelector(""ul"").addEventListener(""click"", (e) => {
    if (e.target.tagName === ""LI"") {     // ← 檢查是不是 li 被點
        console.log(e.target.textContent);
    }
});
// 優點：動態新增的 li 也能觸發！
```

逐行解析：
```
ul.addEventListener(""click"", ...)   -- 事件綁在父元素 ul 上
e.target                            -- 實際被點擊的元素（可能是 li、span 等）
e.target.tagName === ""LI""           -- 確認是 li 被點才處理
e.currentTarget                     -- 綁定事件的元素（ul）
```

---

## 防抖與節流

```javascript
// 防抖（Debounce）— 停止操作後才執行
function debounce(fn, delay) {
    let timer;
    return function(...args) {
        clearTimeout(timer);
        timer = setTimeout(() => fn.apply(this, args), delay);
    };
}

// 使用：搜尋框輸入 300ms 後才發送請求
let searchInput = document.querySelector(""#search"");
searchInput.addEventListener(""input"", debounce((e) => {
    fetch(`/api/search?q=${e.target.value}`);
}, 300));

// 節流（Throttle）— 固定間隔執行一次
function throttle(fn, limit) {
    let inThrottle;
    return function(...args) {
        if (!inThrottle) {
            fn.apply(this, args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    };
}

// 使用：滾動事件每 100ms 最多觸發一次
window.addEventListener(""scroll"", throttle(() => {
    console.log(""滾動中..."");
}, 100));
```
" },

        // ── 1508: Promise 與 async/await ──
        new() { Id=1508, Category="javascript", Order=9, Level="intermediate", Icon="⏳", Title="Promise 與 async/await", Slug="js-async", IsPublished=true, Content=@"
# Promise 與 async/await

## 同步 vs 非同步

> **比喻：** 🍳
> - **同步**：煎完蛋 → 才能烤吐司 → 才能倒咖啡（一件接一件）
> - **非同步**：同時煎蛋 + 烤吐司 + 泡咖啡（不用等前一件做完）

JavaScript 是單執行緒，但透過非同步機制避免「卡住」。

---

## 回呼地獄（Callback Hell）

```javascript
// 舊方式：巢狀回呼
getUser(1, function(user) {
    getOrders(user.id, function(orders) {
        getOrderDetails(orders[0].id, function(details) {
            console.log(details);
            // 越來越深...這就是回呼地獄 🔥
        });
    });
});
```

---

## Promise — 解決回呼地獄

```javascript
// Promise 有三種狀態：
// pending（等待中）→ fulfilled（成功）或 rejected（失敗）

// 建立 Promise
function fetchUser(id) {
    return new Promise((resolve, reject) => {  // ← 建立 Promise
        setTimeout(() => {
            if (id > 0) {
                resolve({ id, name: ""小明"" });   // ← 成功，傳出資料
            } else {
                reject(new Error(""ID 無效""));     // ← 失敗，傳出錯誤
            }
        }, 1000);
    });
}

// 使用 Promise
fetchUser(1)
    .then(user => {                            // ← 成功時執行
        console.log(user.name);
        return fetchOrders(user.id);           // ← 回傳新的 Promise
    })
    .then(orders => {                          // ← 鏈式呼叫
        console.log(orders);
    })
    .catch(error => {                          // ← 失敗時執行（任何一步失敗都會到這）
        console.error(""錯誤："", error.message);
    })
    .finally(() => {                           // ← 不管成功失敗都執行
        console.log(""完成"");
    });
```

---

## async / await — Promise 的語法糖

```javascript
// async 函式永遠回傳 Promise
async function getUser(id) {
    // await 等待 Promise 完成，取得結果
    let response = await fetch(`/api/users/${id}`);  // ← 等待 HTTP 請求完成
    let user = await response.json();                 // ← 等待 JSON 解析完成
    return user;                                      // ← 自動包成 Promise
}

// 呼叫
async function main() {
    try {
        let user = await getUser(1);          // ← 等待結果
        console.log(user.name);

        let orders = await getOrders(user.id); // ← 繼續下一步
        console.log(orders);
    } catch (error) {                          // ← 錯誤處理
        console.error(""出錯了："", error.message);
    }
}

main();
```

### 對比

```javascript
// Promise 鏈
fetchUser(1)
    .then(user => fetchOrders(user.id))
    .then(orders => console.log(orders))
    .catch(err => console.error(err));

// async/await（更像同步程式碼，更好讀）
async function main() {
    let user = await fetchUser(1);
    let orders = await fetchOrders(user.id);
    console.log(orders);
}
```

---

## 並行執行

```javascript
// ❌ 串行（等 A 完才跑 B）— 慢
async function slow() {
    let userA = await fetchUser(1);    // 等 1 秒
    let userB = await fetchUser(2);    // 再等 1 秒
    // 總共 2 秒
}

// ✅ 並行（A 和 B 同時跑）— 快
async function fast() {
    let [userA, userB] = await Promise.all([
        fetchUser(1),                  // 同時開始
        fetchUser(2)                   // 同時開始
    ]);
    // 總共 1 秒
}

// Promise.allSettled — 全部完成（不管成功失敗）
let results = await Promise.allSettled([
    fetchUser(1),     // 成功
    fetchUser(-1),    // 失敗
]);
// results[0] = { status: ""fulfilled"", value: {...} }
// results[1] = { status: ""rejected"", reason: Error }

// Promise.race — 最快的那個
let fastest = await Promise.race([
    fetchFromServer1(),
    fetchFromServer2()
]);
// 回傳最先完成的結果
```

---

## 實用模式

```javascript
// 帶超時的請求
async function fetchWithTimeout(url, ms) {
    const controller = new AbortController();
    const timeout = setTimeout(() => controller.abort(), ms);

    try {
        let response = await fetch(url, { signal: controller.signal });
        return await response.json();
    } finally {
        clearTimeout(timeout);
    }
}

// 重試機制
async function fetchWithRetry(url, retries = 3) {
    for (let i = 0; i < retries; i++) {
        try {
            return await fetch(url).then(r => r.json());
        } catch (err) {
            if (i === retries - 1) throw err;
            await new Promise(r => setTimeout(r, 1000 * (i + 1)));
        }
    }
}
```
" },

        // ── 1509: Fetch API ──
        new() { Id=1509, Category="javascript", Order=10, Level="intermediate", Icon="🌐", Title="Fetch API 與 HTTP 請求", Slug="js-fetch", IsPublished=true, Content=@"
# Fetch API 與 HTTP 請求

## Fetch — 瀏覽器內建的 HTTP 客戶端

```javascript
// 最簡單的 GET 請求
let response = await fetch(""https://api.example.com/users"");
let data = await response.json();   // ← 解析 JSON 回應
console.log(data);
```

---

## GET 請求

```javascript
async function getUsers() {
    try {
        let response = await fetch(""/api/users"");

        // 檢查 HTTP 狀態
        if (!response.ok) {                    // ← ok = 200-299
            throw new Error(`HTTP ${response.status}`);
        }

        let users = await response.json();     // ← 解析 JSON
        return users;
    } catch (error) {
        console.error(""取得使用者失敗："", error);
    }
}

// 帶查詢參數
let params = new URLSearchParams({
    page: 1,
    limit: 10,
    search: ""小明""
});
let response = await fetch(`/api/users?${params}`);
// URL: /api/users?page=1&limit=10&search=小明
```

---

## POST 請求

```javascript
async function createUser(userData) {
    let response = await fetch(""/api/users"", {
        method: ""POST"",                          // ← HTTP 方法
        headers: {
            ""Content-Type"": ""application/json"",   // ← 告訴伺服器是 JSON
        },
        body: JSON.stringify(userData),            // ← 把物件轉成 JSON 字串
    });

    if (!response.ok) {
        let error = await response.json();
        throw new Error(error.message);
    }

    return await response.json();
}

// 使用
let newUser = await createUser({
    name: ""小明"",
    age: 20,
    email: ""ming@test.com""
});
```

---

## PUT / DELETE

```javascript
// PUT — 更新資料
async function updateUser(id, data) {
    return await fetch(`/api/users/${id}`, {
        method: ""PUT"",
        headers: { ""Content-Type"": ""application/json"" },
        body: JSON.stringify(data)
    });
}

// DELETE — 刪除資料
async function deleteUser(id) {
    return await fetch(`/api/users/${id}`, {
        method: ""DELETE""
    });
}
```

---

## 上傳檔案

```javascript
async function uploadFile(file) {
    let formData = new FormData();         // ← 用 FormData 包裝
    formData.append(""file"", file);         // ← 加入檔案
    formData.append(""name"", ""my-file"");    // ← 加入其他欄位

    let response = await fetch(""/api/upload"", {
        method: ""POST"",
        body: formData                      // ← 不需要設 Content-Type
        // 瀏覽器會自動設定 multipart/form-data
    });

    return await response.json();
}

// 搭配 <input type=""file"">
let fileInput = document.querySelector(""#fileInput"");
fileInput.addEventListener(""change"", async (e) => {
    let file = e.target.files[0];           // ← 取得選中的檔案
    await uploadFile(file);
});
```

---

## 錯誤處理模式

```javascript
// 封裝成通用 API 函式
async function apiRequest(url, options = {}) {
    let defaultOptions = {
        headers: { ""Content-Type"": ""application/json"" },
        ...options
    };

    if (options.body && typeof options.body === ""object"") {
        defaultOptions.body = JSON.stringify(options.body);
    }

    let response = await fetch(url, defaultOptions);

    if (!response.ok) {
        let errorBody;
        try {
            errorBody = await response.json();
        } catch {
            errorBody = { message: response.statusText };
        }
        throw {
            status: response.status,
            message: errorBody.message || ""未知錯誤"",
            data: errorBody
        };
    }

    // 204 No Content
    if (response.status === 204) return null;

    return await response.json();
}

// 使用
try {
    let users = await apiRequest(""/api/users"");
    let newUser = await apiRequest(""/api/users"", {
        method: ""POST"",
        body: { name: ""小明"", age: 20 }
    });
} catch (err) {
    if (err.status === 401) {
        // 導向登入頁
    } else if (err.status === 404) {
        // 顯示找不到
    } else {
        // 通用錯誤處理
        alert(err.message);
    }
}
```
" },

        // ── 1510: ES6+ 新特性 ──
        new() { Id=1510, Category="javascript", Order=11, Level="intermediate", Icon="✨", Title="ES6+ 新特性總覽", Slug="js-es6-features", IsPublished=true, Content=@"
# ES6+ 新特性總覽

## 解構賦值（Destructuring）

```javascript
// 陣列解構
let [a, b, c] = [1, 2, 3];
let [first, ...rest] = [1, 2, 3, 4, 5];  // rest = [2,3,4,5]

// 交換變數
let x = 1, y = 2;
[x, y] = [y, x];    // ← x=2, y=1（不需要 temp 變數！）

// 物件解構
let { name, age } = { name: ""小明"", age: 20 };

// 函式參數解構
function greet({ name, age = 18 }) {
    console.log(`${name}, ${age}歲`);
}
greet({ name: ""小明"", age: 20 });
```

---

## 模板字串（Template Literals）

```javascript
let name = ""小明"";
let age = 20;

// 舊寫法
let msg1 = ""你好，"" + name + ""！你今年 "" + age + "" 歲。"";

// 新寫法（反引號 + ${}）
let msg2 = `你好，${name}！你今年 ${age} 歲。`;

// 多行字串
let html = `
    <div class=""card"">
        <h2>${name}</h2>
        <p>年齡：${age}</p>
    </div>
`;

// 可以放運算式
let msg3 = `${age >= 18 ? ""成年"" : ""未成年""}`;
```

---

## 展開與剩餘（Spread / Rest）

```javascript
// Spread（展開）
let arr1 = [1, 2, 3];
let arr2 = [...arr1, 4, 5];     // [1, 2, 3, 4, 5]

let obj1 = { a: 1, b: 2 };
let obj2 = { ...obj1, c: 3 };   // { a: 1, b: 2, c: 3 }

// Rest（收集）
function sum(...nums) {           // nums = [1, 2, 3, 4, 5]
    return nums.reduce((a, b) => a + b, 0);
}
sum(1, 2, 3, 4, 5);             // 15
```

---

## Map 與 Set

```javascript
// Map — 任何值都可以當 key
let map = new Map();
map.set(""name"", ""小明"");
map.set(42, ""數字 key"");
map.set(true, ""布林 key"");

console.log(map.get(""name""));   // ""小明""
console.log(map.size);          // 3
console.log(map.has(42));       // true
map.delete(true);

// 遍歷
for (let [key, value] of map) {
    console.log(`${key}: ${value}`);
}

// Set — 不重複的集合
let set = new Set([1, 2, 2, 3, 3, 3]);
console.log(set);               // Set {1, 2, 3}
console.log(set.size);          // 3

// 陣列去重複
let arr = [1, 1, 2, 2, 3];
let unique = [...new Set(arr)]; // [1, 2, 3]
```

---

## 類別（Class）

```javascript
class Animal {
    // 建構子
    constructor(name, sound) {
        this.name = name;       // ← 實例屬性
        this.sound = sound;
    }

    // 方法
    speak() {
        return `${this.name} 說 ${this.sound}`;
    }

    // 靜態方法（用 Animal.create() 呼叫）
    static create(name, sound) {
        return new Animal(name, sound);
    }
}

// 繼承
class Dog extends Animal {
    constructor(name) {
        super(name, ""汪汪"");    // ← 呼叫父類建構子
    }

    // 覆寫方法
    speak() {
        return `🐕 ${super.speak()}！`;
    }

    // 新方法
    fetch(item) {
        return `${this.name} 撿回了 ${item}`;
    }
}

let dog = new Dog(""旺財"");
console.log(dog.speak());      // ""🐕 旺財 說 汪汪！""
console.log(dog.fetch(""球"")); // ""旺財 撿回了 球""
```

---

## 模組（Modules）

```javascript
// math.js — 匯出
export function add(a, b) { return a + b; }
export function subtract(a, b) { return a - b; }
export default class Calculator { /* ... */ }

// app.js — 匯入
import Calculator, { add, subtract } from './math.js';
// default export 不用大括號
// named export 用大括號

import * as math from './math.js';  // 匯入全部
math.add(1, 2);
```

```html
<!-- HTML 中使用模組 -->
<script type=""module"" src=""app.js""></script>
```

---

## 其他實用特性

```javascript
// Optional Chaining（?.）
let city = user?.address?.city;    // undefined（不報錯）

// Nullish Coalescing（??）
let name = user?.name ?? ""訪客"";  // null/undefined 才用預設值

// 邏輯賦值
x ||= 10;   // x 是 falsy 就設為 10
x ??= 10;   // x 是 null/undefined 就設為 10
x &&= 10;   // x 是 truthy 就設為 10

// Array.at()（負數索引）
let arr = [1, 2, 3, 4, 5];
arr.at(-1);  // 5（最後一個）
arr.at(-2);  // 4（倒數第二個）

// structuredClone（深拷貝）
let original = { a: 1, b: { c: 2 } };
let deep = structuredClone(original);
deep.b.c = 99;
console.log(original.b.c);  // 2（原始不受影響）
```
" },

        // ── 1511: 錯誤處理 ──
        new() { Id=1511, Category="javascript", Order=12, Level="intermediate", Icon="🛡️", Title="錯誤處理（Error Handling）", Slug="js-error-handling", IsPublished=true, Content=@"
# 錯誤處理（Error Handling）

## try / catch / finally

```javascript
try {
    // 可能出錯的程式碼
    let data = JSON.parse(""不是 JSON"");    // ← 會拋出 SyntaxError
} catch (error) {
    // 錯誤發生時執行
    console.error(""類型："", error.name);     // ← ""SyntaxError""
    console.error(""訊息："", error.message);  // ← ""Unexpected token...""
    console.error(""堆疊："", error.stack);    // ← 詳細的錯誤位置
} finally {
    // 不管有沒有錯都會執行
    console.log(""結束"");
}
```

---

## 常見錯誤類型

```javascript
// ReferenceError — 使用未宣告的變數
console.log(x);           // ReferenceError: x is not defined

// TypeError — 對錯誤的型態做操作
null.name;                // TypeError: Cannot read property 'name' of null
(123).toUpperCase();      // TypeError: 123.toUpperCase is not a function

// SyntaxError — 語法錯誤
JSON.parse(""{ bad }"");   // SyntaxError

// RangeError — 超出範圍
new Array(-1);            // RangeError: Invalid array length
```

---

## 自訂錯誤

```javascript
// 自訂 Error 類別
class ValidationError extends Error {
    constructor(field, message) {
        super(message);             // ← 呼叫父類
        this.name = ""ValidationError"";
        this.field = field;         // ← 自訂屬性
    }
}

// 拋出自訂錯誤
function validateAge(age) {
    if (typeof age !== ""number"") {
        throw new ValidationError(""age"", ""年齡必須是數字"");
    }
    if (age < 0 || age > 150) {
        throw new ValidationError(""age"", ""年齡必須在 0~150 之間"");
    }
    return true;
}

// 捕捉
try {
    validateAge(""abc"");
} catch (error) {
    if (error instanceof ValidationError) {
        console.log(`欄位 ${error.field} 錯誤：${error.message}`);
    } else {
        throw error;                // ← 不認識的錯誤重新拋出
    }
}
```

---

## async 函式的錯誤處理

```javascript
// 方式 1：try/catch
async function fetchUser(id) {
    try {
        let response = await fetch(`/api/users/${id}`);
        if (!response.ok) throw new Error(`HTTP ${response.status}`);
        return await response.json();
    } catch (error) {
        console.error(""取得使用者失敗："", error);
        return null;               // ← 回傳預設值
    }
}

// 方式 2：.catch()
let user = await fetchUser(1).catch(err => {
    console.error(err);
    return null;
});
```

---

## 全域錯誤處理

```javascript
// 捕捉未處理的錯誤
window.addEventListener(""error"", (event) => {
    console.error(""未捕捉的錯誤："", event.error);
    // 可以發送到錯誤追蹤服務
});

// 捕捉未處理的 Promise 拒絕
window.addEventListener(""unhandledrejection"", (event) => {
    console.error(""未處理的 Promise 拒絕："", event.reason);
    event.preventDefault();        // ← 防止出現在 Console
});
```
" },

        // ── 1512: LocalStorage 與 JSON ──
        new() { Id=1512, Category="javascript", Order=13, Level="beginner", Icon="💾", Title="LocalStorage 與 JSON", Slug="js-localstorage", IsPublished=true, Content=@"
# LocalStorage 與 JSON

## LocalStorage — 瀏覽器端的儲存空間

> **比喻：LocalStorage 就像瀏覽器裡的小筆記本** 📓
>
> 關掉瀏覽器再打開，資料還在。
> 但它只能存**字串**，要存物件需要先轉成 JSON。

---

## 基本操作

```javascript
// 存入
localStorage.setItem(""username"", ""小明"");

// 讀取
let name = localStorage.getItem(""username"");  // ← ""小明""

// 刪除
localStorage.removeItem(""username"");

// 清空全部
localStorage.clear();

// 檢查有幾筆
console.log(localStorage.length);
```

---

## 存物件（需要 JSON）

```javascript
// ❌ 直接存物件會變成 ""[object Object]""
localStorage.setItem(""user"", { name: ""小明"" });
localStorage.getItem(""user"");  // ← ""[object Object]""（壞了）

// ✅ 用 JSON.stringify / JSON.parse
let user = { name: ""小明"", age: 20, hobbies: [""籃球""] };

// 存入：物件 → JSON 字串
localStorage.setItem(""user"", JSON.stringify(user));

// 讀取：JSON 字串 → 物件
let saved = JSON.parse(localStorage.getItem(""user""));
console.log(saved.name);     // ← ""小明""
console.log(saved.hobbies);  // ← [""籃球""]
```

---

## 安全讀取

```javascript
// 封裝安全的讀寫函式
function saveToStorage(key, value) {
    localStorage.setItem(key, JSON.stringify(value));
}

function loadFromStorage(key, defaultValue = null) {
    try {
        let item = localStorage.getItem(key);
        return item ? JSON.parse(item) : defaultValue;
    } catch {
        return defaultValue;     // ← JSON 解析失敗時回傳預設值
    }
}

// 使用
saveToStorage(""settings"", { theme: ""dark"", lang: ""zh"" });
let settings = loadFromStorage(""settings"", { theme: ""light"" });
```

---

## SessionStorage

```javascript
// 用法和 localStorage 完全一樣，但關掉分頁就消失
sessionStorage.setItem(""token"", ""abc123"");
sessionStorage.getItem(""token"");
```

| 比較 | localStorage | sessionStorage |
|------|-------------|---------------|
| 生命週期 | 永久（手動清除） | 關閉分頁就消失 |
| 容量 | 約 5~10 MB | 約 5~10 MB |
| 共享範圍 | 同源的所有分頁 | 只限當前分頁 |

---

## 實用範例

```javascript
// 記住使用者的佈景主題
function setTheme(theme) {
    document.body.className = theme;
    saveToStorage(""theme"", theme);
}

// 頁面載入時套用
let savedTheme = loadFromStorage(""theme"", ""light"");
setTheme(savedTheme);

// 記住購物車
function addToCart(product) {
    let cart = loadFromStorage(""cart"", []);
    cart.push(product);
    saveToStorage(""cart"", cart);
}
```
" },

        // ── 1513: 模組化 ──
        new() { Id=1513, Category="javascript", Order=14, Level="intermediate", Icon="📦", Title="模組化（Import / Export）", Slug="js-modules", IsPublished=true, Content=@"
# 模組化（Import / Export）

## 為什麼要模組化？

> **比喻：模組就像樂高積木** 🧱
>
> 把程式碼拆成小塊，每塊負責一件事。
> 需要時組裝起來，不需要時拆掉。

---

## ES Modules 基本語法

### Named Export（具名匯出）

```javascript
// utils.js
export function add(a, b) {
    return a + b;
}

export function subtract(a, b) {
    return a - b;
}

export const PI = 3.14159;
```

```javascript
// app.js
import { add, subtract, PI } from './utils.js';

console.log(add(1, 2));         // 3
console.log(PI);                // 3.14159

// 重新命名
import { add as sum } from './utils.js';
console.log(sum(1, 2));         // 3

// 匯入全部
import * as utils from './utils.js';
console.log(utils.add(1, 2));   // 3
```

### Default Export（預設匯出）

```javascript
// Calculator.js
export default class Calculator {
    add(a, b) { return a + b; }
    subtract(a, b) { return a - b; }
}
```

```javascript
// app.js — default export 不用大括號，名字隨便取
import Calculator from './Calculator.js';
import Calc from './Calculator.js';  // 也可以

let calc = new Calculator();
console.log(calc.add(1, 2));    // 3
```

### 混合使用

```javascript
// api.js
export default class ApiClient { /* ... */ }
export function formatUrl(path) { /* ... */ }
export const BASE_URL = ""/api"";
```

```javascript
import ApiClient, { formatUrl, BASE_URL } from './api.js';
```

---

## 動態匯入

```javascript
// 需要時才載入（節省初始載入時間）
async function loadChart() {
    let { Chart } = await import('./chart.js');   // ← 動態匯入
    let chart = new Chart(""#canvas"");
    chart.render(data);
}

// 條件匯入
if (needsAdmin) {
    let { AdminPanel } = await import('./admin.js');
}
```

---

## 在 HTML 中使用

```html
<!-- type=""module"" 才能使用 import/export -->
<script type=""module"" src=""app.js""></script>

<!-- 模組的特性 -->
<!-- 1. 自動使用嚴格模式 -->
<!-- 2. 有自己的作用域（不會汙染全域） -->
<!-- 3. 只會執行一次（多次 import 不會重複執行） -->
```

---

## 模組組織最佳實踐

```
project/
├── index.html
├── js/
│   ├── app.js              ← 進入點
│   ├── api/
│   │   ├── client.js       ← API 請求封裝
│   │   └── endpoints.js    ← API 路徑常數
│   ├── components/
│   │   ├── Header.js       ← UI 元件
│   │   └── Card.js
│   ├── utils/
│   │   ├── format.js       ← 格式化工具
│   │   └── validate.js     ← 驗證工具
│   └── constants.js        ← 全域常數
```

```javascript
// 用 index.js 統一匯出（barrel export）
// utils/index.js
export { formatDate, formatCurrency } from './format.js';
export { validateEmail, validatePhone } from './validate.js';

// 使用時
import { formatDate, validateEmail } from './utils/index.js';
```
" },

        // ── 1514: this 與原型鏈 ──
        new() { Id=1514, Category="javascript", Order=15, Level="intermediate", Icon="🔍", Title="this 關鍵字與原型鏈", Slug="js-this-prototype", IsPublished=true, Content=@"
# this 關鍵字與原型鏈

## this 是什麼？

> **this 指向「誰在呼叫這個函式」。**
> 它不是在定義時決定的，而是在**呼叫時**才決定。

---

## this 的四種綁定規則

```javascript
// 1. 預設綁定 — 全域呼叫
function sayHi() {
    console.log(this);
}
sayHi();                // ← 瀏覽器中 this = window（嚴格模式下 undefined）

// 2. 隱式綁定 — 被物件呼叫
let user = {
    name: ""小明"",
    greet() {
        console.log(this.name);   // ← this = user（呼叫者）
    }
};
user.greet();           // ← ""小明""

// 3. 顯式綁定 — call / apply / bind
function greet(greeting) {
    console.log(`${greeting}, ${this.name}`);
}
let person = { name: ""小華"" };

greet.call(person, ""你好"");     // ← ""你好, 小華""（this = person）
greet.apply(person, [""哈囉""]);  // ← ""哈囉, 小華""（參數用陣列）

let bound = greet.bind(person);  // ← 回傳新函式，this 永遠是 person
bound(""嗨"");                     // ← ""嗨, 小華""

// 4. new 綁定 — 建構函式
function User(name) {
    this.name = name;             // ← this = 新建立的物件
}
let u = new User(""小美"");
console.log(u.name);             // ← ""小美""
```

---

## 箭頭函式的 this

```javascript
// ⚠️ 箭頭函式沒有自己的 this，它繼承外層的 this

let user = {
    name: ""小明"",
    // ❌ 箭頭函式：this 不是 user
    greetArrow: () => {
        console.log(this.name);   // ← this = 外層（可能是 window）
    },
    // ✅ 一般方法：this 是 user
    greetNormal() {
        console.log(this.name);   // ← this = user
    },
    // ✅ 箭頭函式在方法「裡面」很好用
    delayGreet() {
        setTimeout(() => {
            console.log(this.name); // ← this 繼承外層的 this = user ✅
        }, 1000);
    }
};
```

---

## 原型鏈（Prototype Chain）

```javascript
// 每個物件都有一個隱藏的 __proto__ 屬性
let animal = {
    eat() { console.log(""吃東西""); }
};

let dog = {
    bark() { console.log(""汪汪""); }
};

// 設定 dog 的原型為 animal
Object.setPrototypeOf(dog, animal);

dog.bark();     // ← ""汪汪""（自己有）
dog.eat();      // ← ""吃東西""（從原型找到的）

// 原型鏈：dog → animal → Object.prototype → null
```

---

## Class 的原型本質

```javascript
class Animal {
    constructor(name) {
        this.name = name;        // ← 實例屬性
    }
    eat() {                      // ← 原型上的方法
        console.log(`${this.name} 在吃東西`);
    }
}

class Dog extends Animal {
    bark() {
        console.log(`${this.name} 汪汪`);
    }
}

let dog = new Dog(""旺財"");

// 原型鏈：
// dog → Dog.prototype → Animal.prototype → Object.prototype → null

console.log(dog instanceof Dog);      // true
console.log(dog instanceof Animal);   // true
console.log(dog instanceof Object);   // true
```

> 💡 JavaScript 的 class 只是原型繼承的語法糖，底層還是原型鏈。
" },

        // ── 1515: 正則表達式 ──
        new() { Id=1515, Category="javascript", Order=16, Level="intermediate", Icon="🔣", Title="正則表達式（RegExp）", Slug="js-regex", IsPublished=true, Content=@"
# 正則表達式（RegExp）

## 什麼是正則表達式？

> **比喻：正則就像一個模式匹配器** 🔣
>
> 你給它一個「模式」（pattern），它幫你從文字中找出所有符合的部分。

---

## 建立正則

```javascript
// 方式 1：字面值（常用）
let pattern = /hello/;         // ← 匹配 ""hello""

// 方式 2：建構函式（動態建立時用）
let pattern2 = new RegExp(""hello"");

// 加旗標
let pattern3 = /hello/gi;     // g=全域, i=不分大小寫
```

---

## 常用方法

```javascript
let regex = /小[明華美]/g;

// test — 測試是否匹配
regex.test(""小明你好"");        // ← true

// match — 找出所有匹配
""小明和小華"".match(/小[明華美]/g);  // ← [""小明"", ""小華""]

// replace — 取代
""小明好棒"".replace(/小明/, ""小華"");  // ← ""小華好棒""

// search — 找位置
""Hello World"".search(/World/);      // ← 6

// split — 分割
""a,b;c|d"".split(/[,;|]/);          // ← [""a"", ""b"", ""c"", ""d""]
```

---

## 常用模式

```javascript
// 字元類
/[abc]/        // a 或 b 或 c
/[a-z]/        // a 到 z
/[0-9]/        // 0 到 9
/[^abc]/       // 不是 a, b, c

// 特殊字元
/\d/           // 數字（等同 [0-9]）
/\D/           // 非數字
/\w/           // 字母數字底線（等同 [a-zA-Z0-9_]）
/\W/           // 非字母數字
/\s/           // 空白字元
/\S/           // 非空白字元
/./            // 任意字元（換行除外）

// 量詞
/a*/           // a 出現 0 次或多次
/a+/           // a 出現 1 次或多次
/a?/           // a 出現 0 次或 1 次
/a{3}/         // a 剛好 3 次
/a{2,5}/       // a 2~5 次
/a{3,}/        // a 至少 3 次

// 錨點
/^hello/       // 開頭是 hello
/world$/       // 結尾是 world
/\bhello\b/    // 完整單詞 hello
```

---

## 實用驗證範例

```javascript
// Email 驗證（簡易版）
function isValidEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

// 手機號碼（台灣）
function isValidPhone(phone) {
    return /^09\d{8}$/.test(phone);
}

// 密碼強度（至少 8 字元，包含大小寫和數字）
function isStrongPassword(pwd) {
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/.test(pwd);
}

// 取代敏感字
function censorBadWords(text) {
    return text.replace(/壞詞|髒話/g, ""***"");
}

// 提取數字
""價格是 NT$1,200 元"".match(/\d+/g);  // [""1"", ""200""]
""價格是 NT$1,200 元"".match(/[\d,]+/g); // [""1,200""]
```

---

## 分組與捕獲

```javascript
// 捕獲組 ()
let match = ""2024-01-15"".match(/(\d{4})-(\d{2})-(\d{2})/);
console.log(match[0]);     // ""2024-01-15""（完整匹配）
console.log(match[1]);     // ""2024""（第 1 組）
console.log(match[2]);     // ""01""（第 2 組）
console.log(match[3]);     // ""15""（第 3 組）

// 命名捕獲組
let match2 = ""2024-01-15"".match(/(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})/);
console.log(match2.groups.year);   // ""2024""
console.log(match2.groups.month);  // ""01""

// 在 replace 中使用捕獲組
""2024-01-15"".replace(/(\d{4})-(\d{2})-(\d{2})/, ""$3/$2/$1"");
// → ""15/01/2024""
```
" },

        // ── 1516: 閉包與進階函式 ──
        new() { Id=1516, Category="javascript", Order=17, Level="advanced", Icon="🔐", Title="閉包與進階函式模式", Slug="js-closures-advanced", IsPublished=true, Content=@"
# 閉包與進階函式模式

## 閉包（Closure）深入

```javascript
// 閉包 = 函式 + 它能存取的外層變數
function createMultiplier(factor) {
    // factor 被「關」在閉包裡
    return function(number) {
        return number * factor;    // ← 可以存取外層的 factor
    };
}

let double = createMultiplier(2);  // factor = 2
let triple = createMultiplier(3);  // factor = 3

console.log(double(5));    // 10
console.log(triple(5));    // 15
// 即使 createMultiplier 已經執行完畢，factor 還活在閉包裡
```

---

## 閉包的實用場景

### 私有變數

```javascript
function createBankAccount(initialBalance) {
    let balance = initialBalance;    // ← 私有變數

    return {
        deposit(amount) {
            if (amount > 0) balance += amount;
            return balance;
        },
        withdraw(amount) {
            if (amount > 0 && amount <= balance) balance -= amount;
            return balance;
        },
        getBalance() {
            return balance;
        }
    };
}

let account = createBankAccount(1000);
console.log(account.getBalance());   // 1000
account.deposit(500);
console.log(account.getBalance());   // 1500
// account.balance ← undefined（無法直接存取！）
```

### 記憶化（Memoization）

```javascript
function memoize(fn) {
    let cache = {};                   // ← 快取結果

    return function(...args) {
        let key = JSON.stringify(args);
        if (cache[key] !== undefined) {
            console.log(""從快取取得"");
            return cache[key];
        }
        let result = fn(...args);
        cache[key] = result;
        return result;
    };
}

// 使用
let expensiveCalc = memoize((n) => {
    console.log(""計算中..."");
    return n * n * n;
});

expensiveCalc(5);   // ""計算中..."" → 125
expensiveCalc(5);   // ""從快取取得"" → 125（不重算！）
```

---

## 柯里化（Currying）

```javascript
// 把多參數函式轉成一系列單參數函式
function curry(fn) {
    return function curried(...args) {
        if (args.length >= fn.length) {
            return fn(...args);
        }
        return (...moreArgs) => curried(...args, ...moreArgs);
    };
}

// 使用
function add(a, b, c) {
    return a + b + c;
}

let curriedAdd = curry(add);
console.log(curriedAdd(1)(2)(3));     // 6
console.log(curriedAdd(1, 2)(3));     // 6
console.log(curriedAdd(1)(2, 3));     // 6

// 實用：建立特化版本
let addTax = curriedAdd(1.08);        // 固定第一個參數
let addTaxAndShipping = addTax(50);   // 固定第二個參數
```

---

## 組合（Composition）

```javascript
// 把多個函式串起來
function compose(...fns) {
    return (value) => fns.reduceRight((acc, fn) => fn(acc), value);
}

function pipe(...fns) {
    return (value) => fns.reduce((acc, fn) => fn(acc), value);
}

// 使用
let trim = s => s.trim();
let lower = s => s.toLowerCase();
let addExclamation = s => s + ""!"";

// compose 從右到左：addExclamation(lower(trim(""  Hello  "")))
let process1 = compose(addExclamation, lower, trim);
console.log(process1(""  Hello  ""));  // ""hello!""

// pipe 從左到右（更直觀）
let process2 = pipe(trim, lower, addExclamation);
console.log(process2(""  Hello  ""));  // ""hello!""
```

---

## IIFE（立即執行函式）

```javascript
// 定義後立刻執行，常用來建立獨立作用域
(function() {
    let secret = ""私有變數"";
    console.log(secret);     // ← 可以存取
})();

console.log(secret);         // ← ❌ Error（外面存取不到）

// 現代寫法：用區塊 + let/const 取代
{
    let secret = ""私有變數"";
    console.log(secret);
}
```
" },

        // ── 1517: 解構、展開、迭代器 ──
        new() { Id=1517, Category="javascript", Order=18, Level="intermediate", Icon="🔄", Title="迭代器與產生器", Slug="js-iterators-generators", IsPublished=true, Content=@"
# 迭代器與產生器

## 迭代器（Iterator）

```javascript
// 可迭代物件都有 Symbol.iterator 方法
let arr = [10, 20, 30];
let iterator = arr[Symbol.iterator]();

console.log(iterator.next());  // { value: 10, done: false }
console.log(iterator.next());  // { value: 20, done: false }
console.log(iterator.next());  // { value: 30, done: false }
console.log(iterator.next());  // { value: undefined, done: true }

// for...of 背後就是在用迭代器
for (let item of arr) {
    console.log(item);         // 10, 20, 30
}
```

---

## 自訂可迭代物件

```javascript
let range = {
    from: 1,
    to: 5,

    // 實作 Symbol.iterator
    [Symbol.iterator]() {
        let current = this.from;
        let last = this.to;
        return {
            next() {
                return current <= last
                    ? { value: current++, done: false }
                    : { done: true };
            }
        };
    }
};

for (let num of range) {
    console.log(num);          // 1, 2, 3, 4, 5
}

// 也可以用展開運算子
let nums = [...range];         // [1, 2, 3, 4, 5]
```

---

## 產生器（Generator）

```javascript
// function* 定義產生器函式
function* numberGenerator() {
    yield 1;                   // ← 暫停，回傳 1
    yield 2;                   // ← 暫停，回傳 2
    yield 3;                   // ← 暫停，回傳 3
}

let gen = numberGenerator();
console.log(gen.next());      // { value: 1, done: false }
console.log(gen.next());      // { value: 2, done: false }
console.log(gen.next());      // { value: 3, done: false }
console.log(gen.next());      // { value: undefined, done: true }

// 可以用 for...of
for (let n of numberGenerator()) {
    console.log(n);            // 1, 2, 3
}
```

---

## 產生器的實用場景

### 無限序列

```javascript
function* fibonacci() {
    let a = 0, b = 1;
    while (true) {             // ← 無限迴圈也沒問題！
        yield a;               // ← 每次 next() 才執行到這裡
        [a, b] = [b, a + b];
    }
}

let fib = fibonacci();
console.log(fib.next().value); // 0
console.log(fib.next().value); // 1
console.log(fib.next().value); // 1
console.log(fib.next().value); // 2
console.log(fib.next().value); // 3

// 取前 10 個
function take(gen, n) {
    let result = [];
    for (let i = 0; i < n; i++) {
        result.push(gen.next().value);
    }
    return result;
}
console.log(take(fibonacci(), 10));
// [0, 1, 1, 2, 3, 5, 8, 13, 21, 34]
```

### ID 產生器

```javascript
function* idGenerator(prefix = ""id"") {
    let id = 1;
    while (true) {
        yield `${prefix}_${id++}`;
    }
}

let userIds = idGenerator(""user"");
console.log(userIds.next().value);  // ""user_1""
console.log(userIds.next().value);  // ""user_2""
```

---

## async 產生器

```javascript
// 非同步產生器
async function* fetchPages(url) {
    let page = 1;
    while (true) {
        let response = await fetch(`${url}?page=${page}`);
        let data = await response.json();
        if (data.length === 0) return;    // ← 沒資料就停
        yield data;
        page++;
    }
}

// 使用 for await...of
async function getAllUsers() {
    let allUsers = [];
    for await (let page of fetchPages(""/api/users"")) {
        allUsers.push(...page);
    }
    return allUsers;
}
```
" },

        // ── 1518: Web API ──
        new() { Id=1518, Category="javascript", Order=19, Level="intermediate", Icon="🔌", Title="常用 Web API", Slug="js-web-apis", IsPublished=true, Content=@"
# 常用 Web API

## setTimeout / setInterval

```javascript
// setTimeout — 延遲執行一次
let timerId = setTimeout(() => {
    console.log(""3 秒後執行"");
}, 3000);                        // ← 3000 毫秒 = 3 秒

clearTimeout(timerId);           // ← 取消

// setInterval — 每隔 N 毫秒重複執行
let intervalId = setInterval(() => {
    console.log(""每 2 秒執行一次"");
}, 2000);

clearInterval(intervalId);       // ← 停止
```

---

## URL 與 URLSearchParams

```javascript
// 解析 URL
let url = new URL(""https://example.com/path?name=小明&age=20#section"");
console.log(url.hostname);      // ""example.com""
console.log(url.pathname);      // ""/path""
console.log(url.hash);          // ""#section""
console.log(url.searchParams.get(""name""));  // ""小明""

// 建構查詢參數
let params = new URLSearchParams({ page: 1, limit: 10 });
params.append(""sort"", ""name"");
console.log(params.toString());  // ""page=1&limit=10&sort=name""
```

---

## History API

```javascript
// 不重新載入頁面的情況下改變 URL
history.pushState({ page: 2 }, """", ""/page/2"");   // ← 新增歷史記錄
history.replaceState({ page: 3 }, """", ""/page/3""); // ← 取代當前記錄

// 監聽上一頁/下一頁
window.addEventListener(""popstate"", (event) => {
    console.log(""導航到："", event.state);
});
```

---

## Clipboard API

```javascript
// 複製到剪貼簿
async function copyText(text) {
    await navigator.clipboard.writeText(text);
    console.log(""已複製！"");
}

// 讀取剪貼簿
async function pasteText() {
    let text = await navigator.clipboard.readText();
    console.log(""剪貼簿內容："", text);
}
```

---

## Intersection Observer

```javascript
// 偵測元素是否進入畫面（懶載入、無限滾動）
let observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {        // ← 元素進入畫面
            entry.target.classList.add(""visible"");
            observer.unobserve(entry.target); // ← 觸發後停止觀察
        }
    });
}, {
    threshold: 0.1                         // ← 出現 10% 就觸發
});

// 觀察所有 .lazy 元素
document.querySelectorAll("".lazy"").forEach(el => {
    observer.observe(el);
});
```

---

## Web Storage 事件

```javascript
// 監聽其他分頁的 localStorage 變化
window.addEventListener(""storage"", (event) => {
    console.log(""Key:"", event.key);
    console.log(""舊值:"", event.oldValue);
    console.log(""新值:"", event.newValue);
});
// 注意：只有「其他」分頁的變化會觸發，自己的不會
```

---

## 通知 API

```javascript
// 請求通知權限
async function requestNotification() {
    let permission = await Notification.requestPermission();
    if (permission === ""granted"") {
        new Notification(""DevLearn"", {
            body: ""你有新的學習任務！"",
            icon: ""/images/logo.png""
        });
    }
}
```

---

## 地理定位

```javascript
// 取得使用者位置
navigator.geolocation.getCurrentPosition(
    (position) => {
        console.log(""緯度:"", position.coords.latitude);
        console.log(""經度:"", position.coords.longitude);
    },
    (error) => {
        console.error(""定位失敗:"", error.message);
    }
);
```
" },

        // ── 1519: 實戰模式 ──
        new() { Id=1519, Category="javascript", Order=20, Level="advanced", Icon="🏆", Title="JavaScript 實戰模式", Slug="js-practical-patterns", IsPublished=true, Content=@"
# JavaScript 實戰模式

## 觀察者模式（Event Emitter）

```javascript
class EventEmitter {
    constructor() {
        this.events = {};
    }

    on(event, callback) {
        if (!this.events[event]) this.events[event] = [];
        this.events[event].push(callback);
        return this;
    }

    off(event, callback) {
        if (this.events[event]) {
            this.events[event] = this.events[event]
                .filter(cb => cb !== callback);
        }
        return this;
    }

    emit(event, ...args) {
        if (this.events[event]) {
            this.events[event].forEach(cb => cb(...args));
        }
        return this;
    }
}

// 使用
let bus = new EventEmitter();
bus.on(""userLogin"", (user) => console.log(`${user.name} 登入了`));
bus.on(""userLogin"", (user) => updateUI(user));
bus.emit(""userLogin"", { name: ""小明"" });
```

---

## 狀態機（State Machine）

```javascript
class StateMachine {
    constructor(initialState, transitions) {
        this.state = initialState;
        this.transitions = transitions;
    }

    transition(action) {
        let key = `${this.state}_${action}`;
        let nextState = this.transitions[key];
        if (nextState) {
            console.log(`${this.state} → ${nextState}`);
            this.state = nextState;
            return true;
        }
        console.warn(`無效轉換：${this.state} + ${action}`);
        return false;
    }
}

// 訂單狀態機
let orderMachine = new StateMachine(""pending"", {
    ""pending_pay"": ""paid"",
    ""paid_ship"": ""shipped"",
    ""shipped_deliver"": ""delivered"",
    ""pending_cancel"": ""cancelled"",
    ""paid_refund"": ""refunded""
});

orderMachine.transition(""pay"");      // pending → paid
orderMachine.transition(""ship"");     // paid → shipped
orderMachine.transition(""deliver"");  // shipped → delivered
orderMachine.transition(""refund"");   // 無效轉換
```

---

## 防抖與節流（完整版）

```javascript
// 防抖：連續觸發只執行最後一次
function debounce(fn, delay, immediate = false) {
    let timer;
    return function(...args) {
        let callNow = immediate && !timer;
        clearTimeout(timer);
        timer = setTimeout(() => {
            timer = null;
            if (!immediate) fn.apply(this, args);
        }, delay);
        if (callNow) fn.apply(this, args);
    };
}

// 節流：固定間隔執行一次
function throttle(fn, limit) {
    let lastCall = 0;
    return function(...args) {
        let now = Date.now();
        if (now - lastCall >= limit) {
            lastCall = now;
            fn.apply(this, args);
        }
    };
}
```

---

## 深拷貝

```javascript
// 簡單版（不支持函式、Date、RegExp 等）
function deepClone(obj) {
    return JSON.parse(JSON.stringify(obj));
}

// 現代版（推薦）
let clone = structuredClone(originalObj);

// 完整版（支持所有類型）
function deepCloneComplete(obj, seen = new WeakMap()) {
    if (obj === null || typeof obj !== ""object"") return obj;
    if (seen.has(obj)) return seen.get(obj);

    if (obj instanceof Date) return new Date(obj);
    if (obj instanceof RegExp) return new RegExp(obj);

    let clone = Array.isArray(obj) ? [] : {};
    seen.set(obj, clone);

    for (let key of Object.keys(obj)) {
        clone[key] = deepCloneComplete(obj[key], seen);
    }
    return clone;
}
```

---

## 請求佇列

```javascript
// 控制並行請求數量
class RequestQueue {
    constructor(maxConcurrent = 3) {
        this.max = maxConcurrent;
        this.running = 0;
        this.queue = [];
    }

    async add(requestFn) {
        return new Promise((resolve, reject) => {
            this.queue.push(async () => {
                try {
                    resolve(await requestFn());
                } catch (err) {
                    reject(err);
                } finally {
                    this.running--;
                    this.processNext();
                }
            });
            this.processNext();
        });
    }

    processNext() {
        while (this.running < this.max && this.queue.length > 0) {
            this.running++;
            this.queue.shift()();
        }
    }
}

// 使用：最多同時 3 個請求
let queue = new RequestQueue(3);
let urls = [""/api/1"", ""/api/2"", ""/api/3"", ""/api/4"", ""/api/5""];

let results = await Promise.all(
    urls.map(url => queue.add(() => fetch(url).then(r => r.json())))
);
```
" },
    };
}
