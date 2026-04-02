using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Vue
{
    public static List<Chapter> GetChapters()
    {
        return new List<Chapter>
        {
            // ── Chapter 600: Vue.js 入門：什麼是前端框架？ ──
            new Chapter
            {
                Id = 1000,
                Title = "Vue.js 入門：什麼是前端框架？",
                Slug = "vue-intro",
                Category = "vue",
                Order = 600,
                Level = "beginner",
                Icon = "🟢",
                IsPublished = true,
                Content = @"# 🟢 Vue.js 入門：什麼是前端框架？

## 📌 重要觀念：框架不是 JavaScript 原生語法！

> **在開始學習 Vue 之前，最重要的一件事：**
>
> Vue.js、React、Angular 這些「前端框架」**都不是** JavaScript 的原生語法。
> 它們是**建立在 JavaScript 之上**的工具庫／框架，底層全部都是用 JavaScript 寫成的。
> 當你寫 `ref()`、`computed()` 時，這些不是瀏覽器認識的語法——是 Vue 團隊用 JS 封裝出來的函式。

---

## 📌 什麼是前端框架？為什麼需要框架？

想像你要蓋一棟房子：
- **原生 JavaScript (Vanilla JS)** = 你自己一磚一瓦慢慢蓋
- **前端框架** = 你用預製的模組化建材，快速搭建

### 沒有框架時的痛點

用原生 JS 寫一個簡單的待辦清單：

```html
<!-- 原生 JS 版本：手動操作 DOM -->
<div id=""app"">
  <input id=""todoInput"" type=""text"" />
  <button onclick=""addTodo()"">新增</button>
  <ul id=""todoList""></ul>
</div>

<script>
  // 原生 JS：你必須自己管理 DOM
  let todos = [];

  function addTodo() {
    const input = document.getElementById('todoInput');
    const text = input.value.trim();
    if (!text) return;

    todos.push(text);
    input.value = '';

    // 痛點：每次資料變化，你都要手動更新畫面
    renderTodos();
  }

  function renderTodos() {
    const list = document.getElementById('todoList');
    // 每次都要清空再重建整個列表
    list.innerHTML = '';
    todos.forEach((todo, index) => {
      const li = document.createElement('li');
      li.textContent = todo;
      const btn = document.createElement('button');
      btn.textContent = '刪除';
      btn.onclick = () => {
        todos.splice(index, 1);
        renderTodos(); // 又要重新渲染...
      };
      li.appendChild(btn);
      list.appendChild(li);
    });
  }
</script>
```

### 用 Vue 框架的同等功能

```vue
<!-- Vue 版本：宣告式、自動響應 -->
<template>
  <div>
    <input v-model=""newTodo"" @keyup.enter=""addTodo"" />
    <button @click=""addTodo"">新增</button>
    <ul>
      <li v-for=""(todo, index) in todos"" :key=""index"">
        {{ todo }}
        <button @click=""todos.splice(index, 1)"">刪除</button>
      </li>
    </ul>
  </div>
</template>

<script setup>
import { ref } from 'vue'

// Vue 的 ref() 不是 JS 原生語法！
// 它是 Vue 提供的「響應式包裝函式」
const newTodo = ref('')
const todos = ref([])

function addTodo() {
  if (!newTodo.value.trim()) return
  todos.value.push(newTodo.value.trim())
  newTodo.value = ''
  // 不需要手動更新 DOM！Vue 自動追蹤變化
}
</script>
```

**差異一目瞭然：**
| 比較項目 | 原生 JS | Vue 框架 |
|---------|---------|---------|
| DOM 操作 | 手動 `getElementById`、`createElement` | 自動響應式更新 |
| 資料綁定 | 自己寫 render 函式 | `v-model` 雙向綁定 |
| 事件處理 | `onclick` 字串 | `@click` 指令 |
| 程式碼量 | 多、容易出錯 | 少、宣告式 |
| 可維護性 | 難以擴展 | 元件化、易擴展 |

---

## 📌 三大前端框架比較：Vue vs React vs Angular

> **再次強調：這三個框架底層都是 JavaScript！**

```
JavaScript（語言本身）
  ├── Vue.js   → 漸進式框架，學習曲線平緩
  ├── React    → UI 函式庫，生態系豐富
  └── Angular  → 完整框架，企業級功能齊全
```

| 特點 | Vue 3 | React 18 | Angular 17 |
|------|-------|----------|------------|
| 定位 | 漸進式框架 | UI 函式庫 | 完整框架 |
| 語法 | SFC (`.vue` 檔) | JSX | TypeScript + 裝飾器 |
| 學習曲線 | ⭐ 平緩 | ⭐⭐ 中等 | ⭐⭐⭐ 陡峭 |
| 狀態管理 | Pinia | Redux / Zustand | RxJS / NgRx |
| 適合場景 | 中小型到大型 | 各種規模 | 大型企業應用 |

---

## 📌 Vue 的設計哲學：漸進式框架

Vue 最大的特色是「漸進式」(Progressive)：

```
Level 1: 只用 Vue 核心 → 響應式 + 元件
Level 2: 加上 Vue Router → 單頁應用 (SPA)
Level 3: 加上 Pinia → 狀態管理
Level 4: 加上 Vite + TypeScript → 完整工程化
Level 5: 加上 Nuxt → 伺服端渲染 (SSR)
```

你可以只在一個小區塊使用 Vue，也可以用它建構整個大型應用。不像 Angular 要求「全家桶」，Vue 讓你按需引入。

### 用 CDN 快速體驗 Vue（無需建置工具）

```html
<!DOCTYPE html>
<html>
<head>
  <!-- 直接引入 Vue，不需要 npm 或 Vite -->
  <script src=""https://unpkg.com/vue@3/dist/vue.global.js""></script>
</head>
<body>
  <div id=""app"">
    <h1>{{ message }}</h1>
    <button @click=""count++"">點了 {{ count }} 次</button>
  </div>

  <script>
    // 這裡的 createApp、ref 都是 Vue 提供的函式
    // 不是 JavaScript 原生就有的！
    const { createApp, ref } = Vue

    createApp({
      setup() {
        const message = ref('Hello Vue！')
        const count = ref(0)
        return { message, count }
      }
    }).mount('#app')
  </script>
</body>
</html>
```

---

## 💡 常見誤區

- ❌ 「Vue 是一種程式語言」→ Vue 是一個 **JavaScript 框架**
- ❌ 「學了 Vue 就不用學 JS」→ 你**必須**先掌握 JS 基礎
- ❌ 「`ref()` 是 JS 的函式」→ `ref()` 是 **Vue 提供**的響應式 API
- ❌ 「`.vue` 檔案瀏覽器看得懂」→ 需要經過 **Vite/Webpack 編譯**
- ✅ 「Vue 幫你封裝了複雜的 DOM 操作和狀態管理」

---

## 🧭 學習路線建議

```
1. 先學好 JavaScript 基礎（變數、函式、陣列、物件、Promise）
2. 了解 DOM 操作基本概念
3. 學習 Vue 3 基礎語法（下一章）
4. 學習元件化開發
5. 學習路由和狀態管理
6. 實作完整專案
```
"
            },

            // ── Chapter 601: Vue 3 基礎語法 ──
            new Chapter
            {
                Id = 1001,
                Title = "Vue 3 基礎語法：模板、資料綁定與事件",
                Slug = "vue-basics",
                Category = "vue",
                Order = 601,
                Level = "beginner",
                Icon = "📗",
                IsPublished = true,
                Content = @"# 📗 Vue 3 基礎語法：模板、資料綁定與事件

## 📌 Composition API vs Options API

Vue 3 提供了兩種寫法風格。本教程使用推薦的 **Composition API**。

> **記住：不管哪種 API，底層都是 JavaScript。Vue 只是幫你封裝了響應式系統。**

```javascript
// ❌ Options API（Vue 2 風格，仍支援但不推薦）
export default {
  data() {
    return { count: 0 }
  },
  methods: {
    increment() { this.count++ }
  }
}

// ✅ Composition API + <script setup>（Vue 3 推薦）
// 更接近原生 JavaScript 的寫法
import { ref } from 'vue'
const count = ref(0)
function increment() { count.value++ }
```

---

## 📌 模板語法：插值與指令

### 文字插值 `{{ }}`

```vue
<template>
  <!-- 雙大括號：將 JS 表達式渲染為文字 -->
  <p>你好，{{ name }}！</p>
  <p>1 + 1 = {{ 1 + 1 }}</p>
  <p>大寫：{{ name.toUpperCase() }}</p>
  <!-- ⚠️ 只能用「表達式」，不能用「語句」 -->
  <!-- ❌ {{ if (ok) { return 'yes' } }} -->
  <!-- ✅ {{ ok ? 'yes' : 'no' }} -->
</template>

<script setup>
import { ref } from 'vue'
// ref() 是 Vue 的 API，不是原生 JS！
// 它把普通值包裝成「響應式物件」
const name = ref('小明')
</script>
```

### 屬性綁定 `v-bind`

```vue
<template>
  <!-- v-bind 綁定 HTML 屬性 -->
  <img v-bind:src=""imageUrl"" v-bind:alt=""imageAlt"" />

  <!-- 簡寫：用冒號 : 代替 v-bind -->
  <img :src=""imageUrl"" :alt=""imageAlt"" />

  <!-- 動態 class 和 style -->
  <div :class=""{ active: isActive, 'text-danger': hasError }"">
    條件式 class
  </div>

  <div :style=""{ color: textColor, fontSize: fontSize + 'px' }"">
    動態樣式
  </div>
</template>

<script setup>
import { ref } from 'vue'

const imageUrl = ref('https://example.com/logo.png')
const imageAlt = ref('Logo')
const isActive = ref(true)
const hasError = ref(false)
const textColor = ref('blue')
const fontSize = ref(16)
</script>
```

### 事件處理 `v-on` / `@`

```vue
<template>
  <!-- v-on 監聽事件 -->
  <button v-on:click=""handleClick"">點我 (完整寫法)</button>

  <!-- 簡寫：用 @ 代替 v-on -->
  <button @click=""count++"">直接寫表達式：{{ count }}</button>
  <button @click=""handleClick"">呼叫函式</button>

  <!-- 帶參數 -->
  <button @click=""greet('小明')"">打招呼</button>

  <!-- 事件修飾符 -->
  <form @submit.prevent=""onSubmit"">
    <!-- .prevent = preventDefault() -->
    <input @keyup.enter=""onEnter"" placeholder=""按 Enter"" />
    <button type=""submit"">送出</button>
  </form>
</template>

<script setup>
import { ref } from 'vue'

const count = ref(0)

// 這些就是普通的 JavaScript 函式
function handleClick() {
  console.log('被點擊了！')
  count.value++
}

function greet(name) {
  alert(`你好，${name}！`)
}

function onSubmit() {
  console.log('表單送出')
}

function onEnter() {
  console.log('按了 Enter')
}
</script>
```

---

## 📌 響應式資料：ref() 和 reactive()

### ref() — 包裝任意值

```vue
<script setup>
import { ref } from 'vue'

// ref() 包裝基本型別
const count = ref(0)          // 數字
const name = ref('小明')       // 字串
const isReady = ref(false)     // 布林值
const items = ref([1, 2, 3])   // 陣列
const user = ref({ name: '小明', age: 25 }) // 物件

// ⚠️ 在 <script> 中存取要加 .value
console.log(count.value)   // 0
count.value++              // 修改值
console.log(count.value)   // 1

// ✅ 在 <template> 中不需要 .value（Vue 自動解包）
// <p>{{ count }}</p>  ← 直接用，不用 count.value
</script>
```

### reactive() — 包裝物件

```vue
<script setup>
import { reactive } from 'vue'

// reactive() 只能包裝物件或陣列
const state = reactive({
  count: 0,
  user: { name: '小明', age: 25 },
  todos: []
})

// 不需要 .value！直接存取
state.count++
state.user.name = '小華'
state.todos.push('學 Vue')

// ⚠️ 不能解構！會失去響應性
// ❌ const { count } = state  // count 不會是響應式的
// ✅ 用 toRefs 解構
import { toRefs } from 'vue'
const { count } = toRefs(state)  // 現在 count 是 ref
</script>
```

### ref vs reactive 怎麼選？

```
ref()      → 適合基本型別（數字、字串、布林）
reactive() → 適合物件、表單資料等複合型別
推薦：統一用 ref()，因為更一致、不容易出錯
```

---

## 📌 計算屬性 computed

```vue
<template>
  <div>
    <p>原價：${{ price }}</p>
    <p>折扣後：${{ discountedPrice }}</p>
    <p>訊息：{{ statusMessage }}</p>

    <input v-model=""firstName"" placeholder=""名"" />
    <input v-model=""lastName"" placeholder=""姓"" />
    <p>全名：{{ fullName }}</p>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const price = ref(1000)
const discount = ref(0.8)

// computed 會自動追蹤依賴的 ref
// 當 price 或 discount 改變時，自動重新計算
const discountedPrice = computed(() => {
  return Math.round(price.value * discount.value)
})

// computed 有快取！只有依賴變化時才重新計算
// 比起 methods，效能更好
const statusMessage = computed(() => {
  return price.value > 500 ? '高價商品' : '平價商品'
})

const firstName = ref('')
const lastName = ref('')

// 可讀可寫的 computed
const fullName = computed({
  get: () => `${lastName.value}${firstName.value}`,
  set: (val) => {
    lastName.value = val[0] || ''
    firstName.value = val.slice(1) || ''
  }
})
</script>
```

---

## 📌 條件渲染與列表渲染

```vue
<template>
  <!-- v-if / v-else-if / v-else：條件渲染 -->
  <div v-if=""score >= 90"">🏆 優秀！</div>
  <div v-else-if=""score >= 60"">✅ 及格</div>
  <div v-else>❌ 不及格</div>

  <!-- v-show：用 CSS display 切換（頻繁切換用這個） -->
  <div v-show=""isVisible"">我可以被顯示/隱藏</div>

  <!-- v-for：列表渲染 -->
  <ul>
    <li v-for=""(item, index) in items"" :key=""item.id"">
      {{ index + 1 }}. {{ item.name }} - ${{ item.price }}
    </li>
  </ul>

  <!-- v-for 搭配物件 -->
  <div v-for=""(value, key) in userInfo"" :key=""key"">
    {{ key }}: {{ value }}
  </div>
</template>

<script setup>
import { ref } from 'vue'

const score = ref(85)
const isVisible = ref(true)

const items = ref([
  { id: 1, name: '蘋果', price: 30 },
  { id: 2, name: '香蕉', price: 15 },
  { id: 3, name: '橘子', price: 25 }
])

const userInfo = ref({
  name: '小明',
  age: 25,
  city: '台北'
})
</script>
```

---

## 📌 完整範例：計數器 + 待辦清單

```vue
<template>
  <div class=""app"">
    <h1>Vue 3 練習</h1>

    <!-- 計數器 -->
    <section>
      <h2>計數器</h2>
      <p>目前數字：{{ count }}</p>
      <p>是否為偶數：{{ isEven ? '是' : '否' }}</p>
      <button @click=""count--"">-1</button>
      <button @click=""count++"">+1</button>
      <button @click=""count = 0"">歸零</button>
    </section>

    <!-- 待辦清單 -->
    <section>
      <h2>待辦清單 ({{ remainingCount }} 項未完成)</h2>
      <form @submit.prevent=""addTodo"">
        <input v-model=""newTodo"" placeholder=""輸入待辦事項..."" />
        <button type=""submit"" :disabled=""!newTodo.trim()"">新增</button>
      </form>

      <ul>
        <li v-for=""todo in todos"" :key=""todo.id""
            :class=""{ done: todo.completed }"">
          <input type=""checkbox"" v-model=""todo.completed"" />
          <span>{{ todo.text }}</span>
          <button @click=""removeTodo(todo.id)"">🗑️</button>
        </li>
      </ul>

      <p v-if=""todos.length === 0"">還沒有待辦事項 🎉</p>
    </section>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

// 計數器
const count = ref(0)
const isEven = computed(() => count.value % 2 === 0)

// 待辦清單
const newTodo = ref('')
const todos = ref([])
let nextId = 1

function addTodo() {
  if (!newTodo.value.trim()) return
  todos.value.push({
    id: nextId++,
    text: newTodo.value.trim(),
    completed: false
  })
  newTodo.value = ''
}

function removeTodo(id) {
  todos.value = todos.value.filter(t => t.id !== id)
}

const remainingCount = computed(() => {
  return todos.value.filter(t => !t.completed).length
})
</script>

<style scoped>
.done span {
  text-decoration: line-through;
  color: #999;
}
</style>
```

---

## 💡 小提醒

- `ref()` 在 JS 中要用 `.value`，在 template 中不用
- `computed` 有快取，`methods` 沒有——需要快取的用 computed
- `v-if` 是真正移除 DOM，`v-show` 只是 CSS 隱藏
- `v-for` 一定要加 `:key`，用唯一值（不要用 index）
- 這些語法（`ref`、`computed`、`v-model`）都是 Vue 封裝的 API，不是 JS 原生的！
"
            },

            // ── Chapter 602: Vue 元件系統 ──
            new Chapter
            {
                Id = 1002,
                Title = "Vue 元件系統：Props、Emit 與插槽",
                Slug = "vue-components",
                Category = "vue",
                Order = 602,
                Level = "intermediate",
                Icon = "🧩",
                IsPublished = true,
                Content = @"# 🧩 Vue 元件系統：Props、Emit 與插槽

## 📌 為什麼要拆元件？

> 元件就像樂高積木——把大型 UI 拆成可重複使用的小塊。

```
App.vue
├── Header.vue          (導航列)
├── Sidebar.vue         (側邊欄)
├── MainContent.vue     (主要內容)
│   ├── ArticleCard.vue (文章卡片 × N)
│   └── Pagination.vue  (分頁)
└── Footer.vue          (頁尾)
```

### 不拆元件 vs 拆元件

```vue
<!-- ❌ 不拆元件：全部寫在一個檔案，500+ 行 -->
<template>
  <div>
    <nav>...</nav>         <!-- 50 行 -->
    <aside>...</aside>     <!-- 80 行 -->
    <main>...</main>       <!-- 200 行 -->
    <footer>...</footer>   <!-- 30 行 -->
  </div>
</template>

<!-- ✅ 拆元件：每個檔案只負責一個功能 -->
<template>
  <div>
    <AppHeader />
    <AppSidebar />
    <MainContent />
    <AppFooter />
  </div>
</template>
```

---

## 📌 defineProps — 父傳子

父元件透過屬性 (props) 把資料傳給子元件。

### 子元件：UserCard.vue

```vue
<template>
  <div class=""card"">
    <h3>{{ name }}</h3>
    <p>年齡：{{ age }}</p>
    <p v-if=""email"">信箱：{{ email }}</p>
    <span :class=""['badge', levelClass]"">{{ level }}</span>
  </div>
</template>

<script setup>
import { computed } from 'vue'

// defineProps 是 Vue 的編譯器巨集 (compiler macro)
// 不需要 import！（這也不是 JS 原生語法）
const props = defineProps({
  name: {
    type: String,
    required: true
  },
  age: {
    type: Number,
    default: 0
  },
  email: {
    type: String,
    default: ''
  },
  level: {
    type: String,
    default: 'beginner',
    validator: (value) => ['beginner', 'intermediate', 'advanced'].includes(value)
  }
})

const levelClass = computed(() => {
  return {
    beginner: 'bg-green',
    intermediate: 'bg-blue',
    advanced: 'bg-red'
  }[props.level]
})
</script>
```

### 父元件使用

```vue
<template>
  <div>
    <!-- 靜態 props -->
    <UserCard name=""小明"" :age=""25"" email=""ming@example.com"" />

    <!-- 動態 props -->
    <UserCard
      v-for=""user in users""
      :key=""user.id""
      :name=""user.name""
      :age=""user.age""
      :level=""user.level""
    />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import UserCard from './UserCard.vue'

const users = ref([
  { id: 1, name: '小明', age: 25, level: 'beginner' },
  { id: 2, name: '小華', age: 30, level: 'intermediate' },
  { id: 3, name: '小美', age: 28, level: 'advanced' }
])
</script>
```

---

## 📌 defineEmits — 子傳父

子元件透過事件 (emit) 把資料傳回父元件。

### 子元件：SearchBox.vue

```vue
<template>
  <div class=""search-box"">
    <input
      :value=""modelValue""
      @input=""$emit('update:modelValue', $event.target.value)""
      placeholder=""搜尋...""
    />
    <button @click=""handleSearch"">搜尋</button>
    <button @click=""$emit('clear')"">清除</button>
  </div>
</template>

<script setup>
// defineEmits 也是 Vue 編譯器巨集
const emit = defineEmits(['update:modelValue', 'search', 'clear'])

const props = defineProps({
  modelValue: { type: String, default: '' }
})

function handleSearch() {
  // 觸發自訂事件，把搜尋關鍵字傳給父元件
  emit('search', props.modelValue)
}
</script>
```

### 父元件使用

```vue
<template>
  <div>
    <!-- v-model 是 :modelValue + @update:modelValue 的語法糖 -->
    <SearchBox
      v-model=""keyword""
      @search=""onSearch""
      @clear=""keyword = ''""
    />
    <p>目前關鍵字：{{ keyword }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import SearchBox from './SearchBox.vue'

const keyword = ref('')

function onSearch(query) {
  console.log('搜尋:', query)
  // 呼叫 API...
}
</script>
```

---

## 📌 Slot 插槽 — 內容分發

插槽讓父元件可以「塞內容」到子元件裡。

### 基本插槽

```vue
<!-- Card.vue（子元件） -->
<template>
  <div class=""card"">
    <div class=""card-header"">
      <slot name=""header"">預設標題</slot>
    </div>
    <div class=""card-body"">
      <slot>預設內容</slot>
    </div>
    <div class=""card-footer"">
      <slot name=""footer""></slot>
    </div>
  </div>
</template>

<!-- 父元件使用 -->
<template>
  <Card>
    <template #header>
      <h2>自訂標題</h2>
    </template>

    <!-- 預設插槽 -->
    <p>這是卡片的主要內容</p>

    <template #footer>
      <button>確認</button>
      <button>取消</button>
    </template>
  </Card>
</template>
```

### 作用域插槽 (Scoped Slots)

```vue
<!-- DataList.vue（子元件） -->
<template>
  <ul>
    <li v-for=""item in items"" :key=""item.id"">
      <!-- 把 item 傳給父元件的插槽 -->
      <slot :item=""item"" :index=""items.indexOf(item)"">
        {{ item.name }}
      </slot>
    </li>
  </ul>
</template>

<script setup>
defineProps({
  items: { type: Array, required: true }
})
</script>

<!-- 父元件使用 -->
<template>
  <DataList :items=""products"">
    <!-- 透過 v-slot 接收子元件傳來的資料 -->
    <template #default=""{ item, index }"">
      <span>{{ index + 1 }}. {{ item.name }} — ${{ item.price }}</span>
    </template>
  </DataList>
</template>
```

---

## 📌 動態元件 & keep-alive

```vue
<template>
  <div>
    <!-- 頁籤切換 -->
    <button
      v-for=""tab in tabs""
      :key=""tab""
      @click=""currentTab = tab""
      :class=""{ active: currentTab === tab }""
    >
      {{ tab }}
    </button>

    <!-- component :is 動態切換元件 -->
    <!-- keep-alive 保留元件狀態（不會被銷毀） -->
    <keep-alive>
      <component :is=""tabComponents[currentTab]"" />
    </keep-alive>
  </div>
</template>

<script setup>
import { ref, shallowRef } from 'vue'
import TabHome from './TabHome.vue'
import TabProfile from './TabProfile.vue'
import TabSettings from './TabSettings.vue'

const tabs = ['首頁', '個人檔案', '設定']
const currentTab = ref('首頁')

const tabComponents = {
  '首頁': TabHome,
  '個人檔案': TabProfile,
  '設定': TabSettings
}
</script>
```

---

## 📌 實作範例：可重用的表單元件

```vue
<!-- FormInput.vue -->
<template>
  <div class=""form-group"">
    <label :for=""id"">{{ label }}</label>
    <input
      :id=""id""
      :type=""type""
      :value=""modelValue""
      :placeholder=""placeholder""
      @input=""$emit('update:modelValue', $event.target.value)""
      :class=""{ error: errorMessage }""
    />
    <p v-if=""errorMessage"" class=""error-text"">{{ errorMessage }}</p>
  </div>
</template>

<script setup>
defineProps({
  modelValue: { type: String, default: '' },
  label: { type: String, required: true },
  type: { type: String, default: 'text' },
  placeholder: { type: String, default: '' },
  id: { type: String, default: () => `input-${Math.random().toString(36).slice(2)}` },
  errorMessage: { type: String, default: '' }
})

defineEmits(['update:modelValue'])
</script>

<!-- 使用 -->
<template>
  <form @submit.prevent=""submitForm"">
    <FormInput v-model=""form.name"" label=""姓名"" placeholder=""請輸入姓名"" />
    <FormInput v-model=""form.email"" label=""Email"" type=""email""
               :error-message=""errors.email"" />
    <FormInput v-model=""form.password"" label=""密碼"" type=""password"" />
    <button type=""submit"">送出</button>
  </form>
</template>
```

---

## 💡 常見陷阱

- ❌ 直接修改 props → 改用 emit 通知父元件
- ❌ 忘記加 `:key` → 動態列表一定要有唯一 key
- ❌ 把所有東西放同一個元件 → 超過 200 行就該考慮拆分
- ✅ `defineProps` 和 `defineEmits` 不需要 import（編譯器巨集）
"
            },

            // ── Chapter 603: Vue Router ──
            new Chapter
            {
                Id = 1003,
                Title = "Vue Router：單頁應用路由管理",
                Slug = "vue-router",
                Category = "vue",
                Order = 603,
                Level = "intermediate",
                Icon = "🧭",
                IsPublished = true,
                Content = @"# 🧭 Vue Router：單頁應用路由管理

## 📌 什麼是 SPA（單頁應用）？

> **傳統網站** = 每次點連結，瀏覽器重新載入整個頁面
> **SPA** = 只載入一次 HTML，之後「換頁」只是用 JavaScript 切換顯示的元件

```
傳統網站（Multi-Page Application）：
  點「關於」→ 瀏覽器送請求 → 伺服器回傳 about.html → 整頁重新載入

SPA（Single Page Application）：
  點「關於」→ JavaScript 攔截 → 切換顯示 About 元件 → 網址列更新
  ❌ 不重新載入頁面
  ✅ 使用者體驗更流暢
```

Vue Router 就是 Vue 官方提供的 SPA 路由管理工具。它**不是瀏覽器原生功能**，而是用 JavaScript 的 History API 封裝出來的。

---

## 📌 安裝與基本設定

```bash
# 用 npm 安裝（Vite 專案）
npm install vue-router@4
```

### 定義路由：router/index.js

```javascript
// router/index.js
import { createRouter, createWebHistory } from 'vue-router'

// 引入頁面元件
import Home from '@/views/Home.vue'
import About from '@/views/About.vue'
import NotFound from '@/views/NotFound.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    component: About
  },
  // 動態路由：用冒號 :id 定義參數
  {
    path: '/user/:id',
    name: 'UserProfile',
    component: () => import('@/views/UserProfile.vue'), // 懶載入
    props: true // 將路由參數作為 props 傳入
  },
  // 巢狀路由
  {
    path: '/dashboard',
    component: () => import('@/views/Dashboard.vue'),
    children: [
      { path: '', component: () => import('@/views/DashboardHome.vue') },
      { path: 'settings', component: () => import('@/views/DashboardSettings.vue') },
      { path: 'profile', component: () => import('@/views/DashboardProfile.vue') }
    ]
  },
  // 404 頁面（萬用路由，放最後）
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: NotFound
  }
]

const router = createRouter({
  // createWebHistory 用的是瀏覽器的 History API
  // 這是原生 JS 的 API，Vue Router 只是封裝它
  history: createWebHistory(),
  routes
})

export default router
```

### 掛載到 Vue App：main.js

```javascript
// main.js
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

const app = createApp(App)
app.use(router) // 安裝 Vue Router
app.mount('#app')
```

### App.vue 中使用

```vue
<template>
  <div id=""app"">
    <nav>
      <!-- router-link 取代 <a> 標籤 -->
      <router-link to=""/"">首頁</router-link>
      <router-link to=""/about"">關於</router-link>
      <router-link :to=""{ name: 'UserProfile', params: { id: 1 } }"">
        個人檔案
      </router-link>
    </nav>

    <!-- router-view 是路由元件的顯示區域 -->
    <router-view />
  </div>
</template>
```

---

## 📌 動態路由與取得參數

```vue
<!-- views/UserProfile.vue -->
<template>
  <div>
    <h1>使用者 #{{ userId }}</h1>
    <p>查詢參數 tab：{{ tab }}</p>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'

// useRoute() 取得目前路由資訊
const route = useRoute()
// useRouter() 取得路由器實例（可以用來導航）
const router = useRouter()

// 路由參數 /user/:id → route.params.id
const userId = computed(() => route.params.id)

// 查詢參數 /user/1?tab=posts → route.query.tab
const tab = computed(() => route.query.tab || 'info')

// 程式化導航
function goHome() {
  router.push('/')
}

function goToUser(id) {
  router.push({ name: 'UserProfile', params: { id } })
}

function goBack() {
  router.back() // 等同於瀏覽器上一頁
}
</script>
```

---

## 📌 導航守衛 (Navigation Guards)

導航守衛就像「門衛」，在路由切換前做檢查。

### 全域守衛

```javascript
// router/index.js
router.beforeEach((to, from, next) => {
  // to: 要去的路由
  // from: 來自的路由
  // next: 放行函式

  const isLoggedIn = !!localStorage.getItem('token')

  // 需要登入的頁面
  if (to.meta.requiresAuth && !isLoggedIn) {
    // 導向登入頁，並記住原本要去的頁面
    next({ path: '/login', query: { redirect: to.fullPath } })
  } else {
    next() // 放行
  }
})

// 路由定義加上 meta
const routes = [
  {
    path: '/dashboard',
    component: Dashboard,
    meta: { requiresAuth: true } // 需要登入
  },
  {
    path: '/login',
    component: Login,
    meta: { requiresAuth: false }
  }
]
```

### 元件內守衛

```vue
<script setup>
import { onBeforeRouteLeave } from 'vue-router'

// 離開頁面前確認
onBeforeRouteLeave((to, from) => {
  if (hasUnsavedChanges.value) {
    const answer = window.confirm('有未儲存的變更，確定離開嗎？')
    if (!answer) return false // 取消離開
  }
})
</script>
```

---

## 📌 路由懶載入 (Lazy Loading)

```javascript
// ❌ 全部打包在一起（首次載入慢）
import Home from './views/Home.vue'
import About from './views/About.vue'
import Dashboard from './views/Dashboard.vue'

// ✅ 懶載入：只在需要時才載入（動態 import 是原生 JS 語法！）
const routes = [
  { path: '/', component: () => import('./views/Home.vue') },
  { path: '/about', component: () => import('./views/About.vue') },
  {
    path: '/dashboard',
    // 還可以自訂 chunk 名稱
    component: () => import(/* webpackChunkName: ""dashboard"" */ './views/Dashboard.vue')
  }
]
```

### 路由載入動畫

```vue
<template>
  <router-view v-slot=""{ Component }"">
    <transition name=""fade"" mode=""out-in"">
      <Suspense>
        <component :is=""Component"" />
        <template #fallback>
          <div class=""loading"">載入中...</div>
        </template>
      </Suspense>
    </transition>
  </router-view>
</template>

<style>
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}
</style>
```

---

## 📌 完整範例：多頁面導航

```vue
<!-- App.vue -->
<template>
  <div>
    <nav class=""navbar"">
      <router-link
        v-for=""link in navLinks""
        :key=""link.path""
        :to=""link.path""
        active-class=""active""
      >
        {{ link.icon }} {{ link.label }}
      </router-link>
    </nav>

    <main>
      <router-view />
    </main>
  </div>
</template>

<script setup>
const navLinks = [
  { path: '/', label: '首頁', icon: '🏠' },
  { path: '/courses', label: '課程', icon: '📚' },
  { path: '/dashboard', label: '儀表板', icon: '📊' },
  { path: '/about', label: '關於', icon: 'ℹ️' }
]
</script>
```

---

## 💡 小提醒

- `router-link` 比 `<a>` 好，因為不會重新載入頁面
- 動態路由參數變化時，元件不會重新建立——用 `watch` 監聽 `route.params`
- 路由懶載入用的 `import()` 是 **ES Module 原生語法**（這個真的是 JS 的！）
- `useRoute` 和 `useRouter` 是 Vue Router 的 API，不是 JS 原生的
"
            },

            // ── Chapter 604: Pinia 狀態管理 ──
            new Chapter
            {
                Id = 1004,
                Title = "Pinia 狀態管理：跨元件資料共享",
                Slug = "vue-state",
                Category = "vue",
                Order = 604,
                Level = "intermediate",
                Icon = "🍍",
                IsPublished = true,
                Content = @"# 🍍 Pinia 狀態管理：跨元件資料共享

## 📌 為什麼需要狀態管理？

當你的應用越來越大，元件之間需要共享資料時：

```
問題場景：
  Header 需要知道「使用者是否登入」
  Sidebar 需要知道「購物車有幾件商品」
  ProductPage 需要修改「購物車內容」
  CartPage 需要顯示「購物車所有商品」

如果只靠 props / emit，傳遞鏈會非常複雜：
  App → Header (props)
  App → Sidebar (props)
  App → ProductPage → AddButton (emit 好幾層)
```

**狀態管理** = 把共用資料放到一個「全域倉庫」(store)，任何元件都能直接存取。

> **Pinia 底層也是 JavaScript！它用的是 Vue 的響應式系統。**

---

## 📌 Pinia vs Vuex

| 比較 | Pinia (推薦) | Vuex 4 |
|------|-------------|--------|
| API 風格 | Composition API 風格 | mutations 繁瑣 |
| TypeScript | 完美支援 | 支援但麻煩 |
| DevTools | 支援 | 支援 |
| 模組化 | 天生模組化 | 需要 modules |
| 學習曲線 | 簡單 | 較複雜 |
| 檔案大小 | ~1KB | ~10KB |

Pinia 是 Vue 官方推薦的狀態管理工具，已取代 Vuex。

---

## 📌 安裝與設定

```bash
npm install pinia
```

```javascript
// main.js
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'

const app = createApp(App)
app.use(createPinia()) // 安裝 Pinia
app.mount('#app')
```

---

## 📌 定義 Store

### 基本結構

```javascript
// stores/counter.js
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

// defineStore 的第一個參數是 store 的唯一 ID
export const useCounterStore = defineStore('counter', () => {
  // state — 用 ref() 定義狀態
  const count = ref(0)
  const name = ref('計數器')

  // getters — 用 computed() 定義衍生狀態
  const doubleCount = computed(() => count.value * 2)
  const isPositive = computed(() => count.value > 0)

  // actions — 用普通函式定義操作
  function increment() {
    count.value++
  }

  function decrement() {
    count.value--
  }

  function reset() {
    count.value = 0
  }

  // 非同步 action
  async function fetchCount() {
    const response = await fetch('/api/count')
    const data = await response.json()
    count.value = data.count
  }

  // 必須 return 所有要公開的狀態和方法
  return {
    count, name,
    doubleCount, isPositive,
    increment, decrement, reset, fetchCount
  }
})
```

---

## 📌 在元件中使用 Store

```vue
<template>
  <div>
    <h2>{{ counterStore.name }}</h2>
    <p>數量：{{ counterStore.count }}</p>
    <p>雙倍：{{ counterStore.doubleCount }}</p>

    <button @click=""counterStore.increment()"">+1</button>
    <button @click=""counterStore.decrement()"">-1</button>
    <button @click=""counterStore.reset()"">歸零</button>
  </div>
</template>

<script setup>
import { useCounterStore } from '@/stores/counter'

// 直接呼叫就能取得 store 實例
const counterStore = useCounterStore()

// ⚠️ 不能解構！會失去響應性
// ❌ const { count } = counterStore
// ✅ 用 storeToRefs 解構
import { storeToRefs } from 'pinia'
const { count, doubleCount } = storeToRefs(counterStore)
// actions 可以直接解構（函式不需要響應性）
const { increment, decrement } = counterStore
</script>
```

---

## 📌 完整範例：購物車狀態管理

### 定義購物車 Store

```javascript
// stores/cart.js
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCartStore = defineStore('cart', () => {
  // 購物車商品列表
  const items = ref([])

  // 商品總數
  const totalItems = computed(() => {
    return items.value.reduce((sum, item) => sum + item.quantity, 0)
  })

  // 總金額
  const totalPrice = computed(() => {
    return items.value.reduce((sum, item) => {
      return sum + item.price * item.quantity
    }, 0)
  })

  // 加入購物車
  function addItem(product) {
    const existing = items.value.find(item => item.id === product.id)
    if (existing) {
      existing.quantity++
    } else {
      items.value.push({ ...product, quantity: 1 })
    }
  }

  // 移除商品
  function removeItem(productId) {
    items.value = items.value.filter(item => item.id !== productId)
  }

  // 更新數量
  function updateQuantity(productId, quantity) {
    const item = items.value.find(item => item.id === productId)
    if (item) {
      item.quantity = Math.max(0, quantity)
      if (item.quantity === 0) removeItem(productId)
    }
  }

  // 清空購物車
  function clearCart() {
    items.value = []
  }

  return {
    items, totalItems, totalPrice,
    addItem, removeItem, updateQuantity, clearCart
  }
})
```

### 商品頁面使用

```vue
<!-- views/Products.vue -->
<template>
  <div>
    <h1>商品列表</h1>
    <div class=""product-grid"">
      <div v-for=""product in products"" :key=""product.id"" class=""product-card"">
        <h3>{{ product.name }}</h3>
        <p>${{ product.price }}</p>
        <button @click=""cartStore.addItem(product)"">
          加入購物車 🛒
        </button>
      </div>
    </div>

    <!-- 購物車小圖示 -->
    <div class=""cart-badge"">
      🛒 {{ cartStore.totalItems }} 件
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useCartStore } from '@/stores/cart'

const cartStore = useCartStore()

const products = ref([
  { id: 1, name: 'Vue 教學書', price: 580 },
  { id: 2, name: 'JavaScript 大全', price: 750 },
  { id: 3, name: 'TypeScript 入門', price: 420 },
  { id: 4, name: 'CSS 設計模式', price: 350 }
])
</script>
```

### 購物車頁面

```vue
<!-- views/Cart.vue -->
<template>
  <div>
    <h1>購物車</h1>

    <div v-if=""cartStore.items.length === 0"">
      <p>購物車是空的 😢</p>
    </div>

    <div v-else>
      <div v-for=""item in cartStore.items"" :key=""item.id"" class=""cart-item"">
        <span>{{ item.name }}</span>
        <span>${{ item.price }}</span>
        <div>
          <button @click=""cartStore.updateQuantity(item.id, item.quantity - 1)"">-</button>
          <span>{{ item.quantity }}</span>
          <button @click=""cartStore.updateQuantity(item.id, item.quantity + 1)"">+</button>
        </div>
        <span>小計：${{ item.price * item.quantity }}</span>
        <button @click=""cartStore.removeItem(item.id)"">🗑️</button>
      </div>

      <hr />
      <p>總計：${{ cartStore.totalPrice }}</p>
      <button @click=""cartStore.clearCart()"">清空購物車</button>
    </div>
  </div>
</template>

<script setup>
import { useCartStore } from '@/stores/cart'
const cartStore = useCartStore()
</script>
```

---

## 📌 持久化存儲

購物車資料重新整理後會消失，用 `localStorage` 持久化：

```javascript
// stores/cart.js（加入持久化）
import { defineStore } from 'pinia'
import { ref, computed, watch } from 'vue'

export const useCartStore = defineStore('cart', () => {
  // 從 localStorage 初始化
  const saved = localStorage.getItem('cart-items')
  const items = ref(saved ? JSON.parse(saved) : [])

  // 監聽變化，自動存到 localStorage
  // watch 是 Vue 的 API，底層用的是 JS 的 Proxy
  watch(items, (newItems) => {
    localStorage.setItem('cart-items', JSON.stringify(newItems))
  }, { deep: true })

  // ... 其他 getters 和 actions 同上

  return { items /* ... */ }
})
```

> **注意：** `localStorage` 是**瀏覽器原生 API**，不是 Vue 的東西。
> `watch` 則是 **Vue 的響應式 API**。分清楚哪些是框架、哪些是原生，很重要！

---

## 💡 小提醒

- 不是所有狀態都需要放 Store——只有「跨元件共享」的資料才需要
- 局部狀態（只有一個元件用）就用 `ref()` / `reactive()`
- Store 是單例的——整個 App 只有一個 counter store 實例
- Pinia 支援 Vue DevTools，可以在瀏覽器中查看/修改 Store 狀態
"
            },

            // ── Chapter 605: Vue 進階 ──
            new Chapter
            {
                Id = 1005,
                Title = "Vue 進階：組合式函式與效能優化",
                Slug = "vue-advanced",
                Category = "vue",
                Order = 605,
                Level = "advanced",
                Icon = "⚡",
                IsPublished = true,
                Content = @"# ⚡ Vue 進階：組合式函式與效能優化

## 📌 自訂 Composable (useXxx)

Composable 是 Vue 3 最強大的程式碼復用模式——把可重用的邏輯抽成函式。

> **Composable 的本質就是 JavaScript 函式！** 只是裡面使用了 Vue 的響應式 API。

### 範例：useMouse — 追蹤滑鼠位置

```javascript
// composables/useMouse.js
import { ref, onMounted, onUnmounted } from 'vue'

export function useMouse() {
  const x = ref(0)
  const y = ref(0)

  function update(event) {
    // event 是原生 DOM 事件（這是 JS 原生的！）
    x.value = event.clientX
    y.value = event.clientY
  }

  // onMounted / onUnmounted 是 Vue 的生命週期鉤子
  onMounted(() => window.addEventListener('mousemove', update))
  onUnmounted(() => window.removeEventListener('mousemove', update))

  return { x, y }
}
```

```vue
<!-- 在元件中使用 -->
<template>
  <p>滑鼠位置：{{ x }}, {{ y }}</p>
</template>

<script setup>
import { useMouse } from '@/composables/useMouse'
const { x, y } = useMouse()
</script>
```

### 範例：useFetch — 封裝 API 請求

```javascript
// composables/useFetch.js
import { ref, watchEffect } from 'vue'

export function useFetch(url) {
  const data = ref(null)
  const error = ref(null)
  const loading = ref(true)

  async function fetchData() {
    loading.value = true
    error.value = null
    try {
      // fetch 是瀏覽器原生 API，不是 Vue 的
      const response = await fetch(url.value || url)
      if (!response.ok) throw new Error(`HTTP ${response.status}`)
      data.value = await response.json()
    } catch (err) {
      error.value = err.message
    } finally {
      loading.value = false
    }
  }

  // watchEffect 自動追蹤響應式依賴
  watchEffect(() => {
    fetchData()
  })

  return { data, error, loading, refetch: fetchData }
}
```

```vue
<template>
  <div v-if=""loading"">載入中...</div>
  <div v-else-if=""error"">錯誤：{{ error }}</div>
  <div v-else>
    <pre>{{ data }}</pre>
  </div>
  <button @click=""refetch"">重新載入</button>
</template>

<script setup>
import { useFetch } from '@/composables/useFetch'
const { data, error, loading, refetch } = useFetch('https://api.example.com/data')
</script>
```

### 範例：useLocalStorage — 持久化響應式資料

```javascript
// composables/useLocalStorage.js
import { ref, watch } from 'vue'

export function useLocalStorage(key, defaultValue) {
  // localStorage 是瀏覽器原生 API
  const stored = localStorage.getItem(key)
  const data = ref(stored ? JSON.parse(stored) : defaultValue)

  // 當資料變化時自動存到 localStorage
  watch(data, (newValue) => {
    localStorage.setItem(key, JSON.stringify(newValue))
  }, { deep: true })

  return data
}
```

---

## 📌 watchEffect vs watch

```vue
<script setup>
import { ref, watch, watchEffect } from 'vue'

const count = ref(0)
const name = ref('小明')

// watchEffect：自動追蹤用到的所有響應式資料
// 立即執行一次，之後只要依賴變化就再執行
watchEffect(() => {
  console.log(`count = ${count.value}`)
  // 只要 count 變了就會觸發
  // 如果裡面也用了 name.value，name 變了也會觸發
})

// watch：明確指定要監聽的來源
// 預設不立即執行
watch(count, (newVal, oldVal) => {
  console.log(`count 從 ${oldVal} 變成 ${newVal}`)
})

// watch 多個來源
watch([count, name], ([newCount, newName], [oldCount, oldName]) => {
  console.log(`count: ${oldCount} → ${newCount}`)
  console.log(`name: ${oldName} → ${newName}`)
})

// watch 深層物件
const user = ref({ name: '小明', address: { city: '台北' } })
watch(
  () => user.value.address.city,
  (newCity) => {
    console.log(`搬到 ${newCity} 了`)
  }
)
</script>
```

| 比較 | `watch` | `watchEffect` |
|------|---------|---------------|
| 指定來源 | 明確指定 | 自動追蹤 |
| 立即執行 | 預設否 | 預設是 |
| 取得舊值 | ✅ `(new, old)` | ❌ |
| 適合場景 | 需要比較新舊值 | 副作用同步 |

---

## 📌 provide / inject — 依賴注入

跨越多層元件傳遞資料，不用一層層 props。

```vue
<!-- 祖先元件 -->
<script setup>
import { ref, provide } from 'vue'

const theme = ref('dark')
const toggleTheme = () => {
  theme.value = theme.value === 'dark' ? 'light' : 'dark'
}

// provide 提供資料給所有後代元件
provide('theme', theme)
provide('toggleTheme', toggleTheme)
</script>

<!-- 深層子元件（不管隔幾層都能用） -->
<script setup>
import { inject } from 'vue'

// inject 接收祖先 provide 的資料
const theme = inject('theme')           // ref('dark')
const toggleTheme = inject('toggleTheme') // function

// 可以設定預設值
const locale = inject('locale', 'zh-TW')
</script>
```

---

## 📌 虛擬 DOM 與 Diff 算法原理

> **這裡揭示框架的底層——全部都是 JavaScript！**

### 什麼是虛擬 DOM？

```javascript
// 真實 DOM（瀏覽器的東西，操作很慢）
const div = document.createElement('div')
div.textContent = 'Hello'
document.body.appendChild(div) // 觸發瀏覽器重排 (reflow)

// 虛擬 DOM（就是普通的 JavaScript 物件！）
const vnode = {
  tag: 'div',
  props: { class: 'greeting' },
  children: 'Hello'
}
// 這只是一個 JS 物件，操作它不會觸發瀏覽器重排
// Vue 在背後比較新舊 vnode，只更新有變化的部分
```

### Diff 算法簡化版

```javascript
// Vue 的 diff 算法（簡化概念）
function patch(oldVNode, newVNode) {
  // 1. 如果節點類型不同 → 直接替換
  if (oldVNode.tag !== newVNode.tag) {
    replaceNode(oldVNode, newVNode)
    return
  }

  // 2. 更新屬性（只改變化的部分）
  updateProps(oldVNode.props, newVNode.props)

  // 3. 比較子節點（這是最複雜的部分）
  diffChildren(oldVNode.children, newVNode.children)
}

// 這就是為什麼 v-for 需要 :key
// key 幫助 Vue 識別哪些節點可以復用，避免不必要的 DOM 操作
```

---

## 📌 效能優化技巧

### v-memo — 跳過不必要的更新

```vue
<template>
  <!-- v-memo 會記住渲染結果，只有依賴變化才重新渲染 -->
  <div v-for=""item in list"" :key=""item.id"" v-memo=""[item.name, item.selected]"">
    <!-- 只有 item.name 或 item.selected 變化時才重新渲染這個 div -->
    <p>{{ item.name }}</p>
    <p>{{ formatDate(item.updatedAt) }}</p>
  </div>
</template>
```

### shallowRef — 淺層響應式

```javascript
import { shallowRef, triggerRef } from 'vue'

// shallowRef 只追蹤 .value 本身的變化
// 不追蹤深層屬性（適合大型物件/陣列）
const bigList = shallowRef([])

// ❌ 這不會觸發更新（淺層追蹤）
bigList.value.push({ name: 'new' })

// ✅ 替換整個陣列才會觸發
bigList.value = [...bigList.value, { name: 'new' }]

// ✅ 或手動觸發更新
bigList.value.push({ name: 'new' })
triggerRef(bigList)
```

### computed 快取

```javascript
import { computed, ref } from 'vue'

const items = ref([/* 1000 個商品 */])

// ✅ computed 有快取：只在 items 變化時重新計算
const expensiveItems = computed(() => {
  console.log('重新計算！')
  return items.value
    .filter(item => item.price > 1000)
    .sort((a, b) => b.price - a.price)
})

// ❌ 不要用方法代替，每次渲染都會重新執行
function getExpensiveItems() {
  return items.value.filter(item => item.price > 1000)
}
```

---

## 📌 打包優化與 Tree Shaking

```javascript
// vite.config.js
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  build: {
    // 分割程式碼
    rollupOptions: {
      output: {
        manualChunks: {
          'vue-vendor': ['vue', 'vue-router', 'pinia'],
          'ui-lib': ['element-plus']
        }
      }
    },
    // 壓縮
    minify: 'terser',
    terserOptions: {
      compress: {
        drop_console: true,  // 移除 console.log
        drop_debugger: true  // 移除 debugger
      }
    }
  }
})
```

### Tree Shaking

```javascript
// ✅ 按需引入（Tree Shaking 友好）
import { ref, computed, watch } from 'vue'

// ❌ 全部引入（打包會包含 Vue 所有功能）
import * as Vue from 'vue'
```

---

## 💡 進階提醒

- Composable 命名慣例：以 `use` 開頭（`useMouse`、`useFetch`）
- `provide/inject` 適合「主題」「語系」等全域設定，不要濫用
- 虛擬 DOM 的存在就是因為直接操作 DOM 太慢——框架用 JS 物件做差異比較
- 效能優化遵循「先量測、再優化」——不要過早優化
"
            },

            // ── Chapter 606: Vue 測試與部署 ──
            new Chapter
            {
                Id = 1006,
                Title = "Vue 測試與部署：從開發到上線",
                Slug = "vue-testing",
                Category = "vue",
                Order = 606,
                Level = "advanced",
                Icon = "🧪",
                IsPublished = true,
                Content = @"# 🧪 Vue 測試與部署：從開發到上線

## 📌 為什麼要寫測試？

```
沒有測試的程式碼 = 每次改功能都在走鋼索 🎪
有測試的程式碼 = 有安全網保護你 🛡️

測試金字塔：
  🔺 E2E 測試（少量）     → 模擬真實使用者操作
  🔹 元件測試（適量）     → 測試元件行為
  🟩 單元測試（大量）     → 測試個別函式/邏輯
```

---

## 📌 Vitest 單元測試

Vitest 是 Vite 生態系的測試框架，速度快、設定簡單。

### 安裝

```bash
npm install -D vitest
```

```javascript
// vite.config.js 加入測試設定
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  test: {
    globals: true,
    environment: 'jsdom' // 模擬瀏覽器環境
  }
})
```

### 測試純函式

```javascript
// utils/calculator.js
export function add(a, b) { return a + b }
export function multiply(a, b) { return a * b }
export function discount(price, rate) {
  if (rate < 0 || rate > 1) throw new Error('折扣率需在 0-1 之間')
  return Math.round(price * (1 - rate))
}

// utils/calculator.test.js
import { describe, it, expect } from 'vitest'
import { add, multiply, discount } from './calculator'

describe('Calculator', () => {
  it('加法正確', () => {
    expect(add(1, 2)).toBe(3)
    expect(add(-1, 1)).toBe(0)
  })

  it('乘法正確', () => {
    expect(multiply(3, 4)).toBe(12)
  })

  it('折扣計算', () => {
    expect(discount(1000, 0.2)).toBe(800)  // 打八折
    expect(discount(1000, 0.15)).toBe(850) // 85折
  })

  it('折扣率超出範圍應拋錯', () => {
    expect(() => discount(1000, -0.1)).toThrow('折扣率需在 0-1 之間')
    expect(() => discount(1000, 1.5)).toThrow()
  })
})
```

### 測試 Composable

```javascript
// composables/useCounter.test.js
import { describe, it, expect } from 'vitest'
import { useCounter } from './useCounter'

describe('useCounter', () => {
  it('初始值為 0', () => {
    const { count } = useCounter()
    expect(count.value).toBe(0)
  })

  it('可以自訂初始值', () => {
    const { count } = useCounter(10)
    expect(count.value).toBe(10)
  })

  it('increment 增加 1', () => {
    const { count, increment } = useCounter()
    increment()
    expect(count.value).toBe(1)
  })

  it('decrement 減少 1', () => {
    const { count, decrement } = useCounter(5)
    decrement()
    expect(count.value).toBe(4)
  })
})
```

---

## 📌 Vue Test Utils 元件測試

```bash
npm install -D @vue/test-utils
```

### 測試元件

```javascript
// components/TodoList.test.js
import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import TodoList from './TodoList.vue'

describe('TodoList', () => {
  it('渲染正確', () => {
    const wrapper = mount(TodoList)
    expect(wrapper.find('h2').text()).toContain('待辦清單')
  })

  it('可以新增待辦事項', async () => {
    const wrapper = mount(TodoList)

    // 找到 input 並輸入文字
    const input = wrapper.find('input')
    await input.setValue('學習 Vue 測試')

    // 找到按鈕並點擊
    const button = wrapper.find('button[type=""submit""]')
    await button.trigger('click')

    // 檢查列表是否新增了項目
    const items = wrapper.findAll('li')
    expect(items.length).toBe(1)
    expect(items[0].text()).toContain('學習 Vue 測試')
  })

  it('可以刪除待辦事項', async () => {
    const wrapper = mount(TodoList)

    // 先新增一個項目
    await wrapper.find('input').setValue('要刪除的項目')
    await wrapper.find('button[type=""submit""]').trigger('click')

    // 確認有一個項目
    expect(wrapper.findAll('li').length).toBe(1)

    // 點擊刪除按鈕
    await wrapper.find('.delete-btn').trigger('click')

    // 確認已刪除
    expect(wrapper.findAll('li').length).toBe(0)
  })

  it('空白輸入不應新增', async () => {
    const wrapper = mount(TodoList)
    await wrapper.find('input').setValue('   ')
    await wrapper.find('button[type=""submit""]').trigger('click')
    expect(wrapper.findAll('li').length).toBe(0)
  })
})
```

### 測試 Props 和 Emit

```javascript
// components/UserCard.test.js
import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import UserCard from './UserCard.vue'

describe('UserCard', () => {
  it('正確顯示 props 資料', () => {
    const wrapper = mount(UserCard, {
      props: {
        name: '小明',
        age: 25,
        level: 'intermediate'
      }
    })
    expect(wrapper.text()).toContain('小明')
    expect(wrapper.text()).toContain('25')
  })

  it('點擊會觸發 emit 事件', async () => {
    const wrapper = mount(UserCard, {
      props: { name: '小明', age: 25 }
    })
    await wrapper.find('.edit-btn').trigger('click')

    // 檢查是否 emit 了 'edit' 事件
    expect(wrapper.emitted()).toHaveProperty('edit')
    expect(wrapper.emitted('edit')[0]).toEqual([{ name: '小明', age: 25 }])
  })
})
```

---

## 📌 E2E 測試 (Cypress / Playwright)

### Playwright 範例

```bash
npm install -D @playwright/test
```

```javascript
// e2e/todo.spec.js
import { test, expect } from '@playwright/test'

test.describe('待辦清單 App', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5173')
  })

  test('可以新增和完成待辦事項', async ({ page }) => {
    // 輸入待辦事項
    await page.fill('input[placeholder=""輸入待辦事項...""]', '買牛奶')
    await page.click('button:text(""新增"")')

    // 確認項目已新增
    await expect(page.locator('li')).toContainText('買牛奶')

    // 勾選完成
    await page.click('input[type=""checkbox""]')
    await expect(page.locator('li')).toHaveClass(/done/)
  })

  test('空白不能新增', async ({ page }) => {
    await page.fill('input', '   ')
    await page.click('button:text(""新增"")')
    await expect(page.locator('li')).toHaveCount(0)
  })
})
```

---

## 📌 Vite 打包與環境變數

### 環境變數

```bash
# .env（所有環境）
VITE_APP_TITLE=我的 Vue App

# .env.development（開發環境）
VITE_API_BASE_URL=http://localhost:5000/api

# .env.production（正式環境）
VITE_API_BASE_URL=https://api.example.com
```

```javascript
// 在程式碼中使用（要加 VITE_ 前綴才能在前端存取）
console.log(import.meta.env.VITE_APP_TITLE)
console.log(import.meta.env.VITE_API_BASE_URL)
console.log(import.meta.env.MODE) // 'development' 或 'production'
```

### 打包指令

```bash
# 開發伺服器
npm run dev

# 正式打包
npm run build

# 預覽打包結果
npm run preview
```

---

## 📌 部署方案

### Vercel 部署

```bash
# 安裝 Vercel CLI
npm install -g vercel

# 部署
vercel

# 或設定 vercel.json
```

```json
{
  ""buildCommand"": ""npm run build"",
  ""outputDirectory"": ""dist"",
  ""rewrites"": [
    { ""source"": ""/(.*)"", ""destination"": ""/index.html"" }
  ]
}
```

### Docker 部署

```dockerfile
# Dockerfile
# 建置階段
FROM node:20-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# 正式階段
FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD [""nginx"", ""-g"", ""daemon off;""]
```

```nginx
# nginx.conf — SPA 路由設定
server {
    listen 80;
    root /usr/share/nginx/html;
    index index.html;

    # SPA：所有路徑都導到 index.html
    location / {
        try_files $uri $uri/ /index.html;
    }

    # 靜態資源快取
    location ~* \.(js|css|png|jpg|svg|ico)$ {
        expires 1y;
        add_header Cache-Control ""public, immutable"";
    }
}
```

---

## 📌 CI/CD 流程

```yaml
# .github/workflows/deploy.yml
name: Deploy Vue App

on:
  push:
    branches: [main]

jobs:
  test-and-deploy:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: 20
          cache: npm

      - name: Install dependencies
        run: npm ci

      - name: Run unit tests
        run: npm run test:unit

      - name: Run E2E tests
        run: npx playwright install && npm run test:e2e

      - name: Build
        run: npm run build

      - name: Deploy to Vercel
        uses: amondnet/vercel-action@v25
        with:
          vercel-token: ${{ secrets.VERCEL_TOKEN }}
          vercel-org-id: ${{ secrets.VERCEL_ORG_ID }}
          vercel-project-id: ${{ secrets.VERCEL_PROJECT_ID }}
          vercel-args: '--prod'
```

---

## 💡 測試與部署小提醒

- 測試覆蓋率不必 100%，但關鍵邏輯一定要測
- E2E 測試不要太多（慢），單元測試多寫（快）
- 環境變數前綴 `VITE_` 才能在前端用
- SPA 部署要設定「所有路徑導到 index.html」
- Docker 部署用 multi-stage build 減小映像檔大小
"
            },

            // ── Chapter 607: Vue + ASP.NET Core 全端整合 ──
            new Chapter
            {
                Id = 1007,
                Title = "Vue + ASP.NET Core 全端整合",
                Slug = "vue-fullstack",
                Category = "vue",
                Order = 607,
                Level = "advanced",
                Icon = "🔗",
                IsPublished = true,
                Content = @"# 🔗 Vue + ASP.NET Core 全端整合

## 📌 前後端分離 vs 同源部署

### 前後端分離

```
Vue (前端)                    ASP.NET Core (後端)
localhost:5173          ←→    localhost:5000
Vercel / Netlify        ←→    Azure / Railway
  ↓                              ↓
  瀏覽器載入 HTML/JS            提供 API (JSON)
  呼叫後端 API                   處理商業邏輯
  渲染畫面                       存取資料庫
```

### 同源部署（Vue 打包後放進 .NET 專案）

```
ASP.NET Core
  wwwroot/
    ├── index.html      ← Vue 打包產出
    ├── assets/
    │   ├── index-xxx.js
    │   └── index-xxx.css
    └── favicon.ico
  Controllers/
    └── ApiController.cs  ← 後端 API
```

---

## 📌 Vue 呼叫 ASP.NET Core API

### 後端 API（C#）

```csharp
// Controllers/TodoController.cs
[ApiController]
[Route(""api/[controller]"")]
public class TodoController : ControllerBase
{
    private readonly AppDbContext _db;

    public TodoController(AppDbContext db) => _db = db;

    // GET api/todo
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var todos = await _db.Todos.OrderByDescending(t => t.CreatedAt).ToListAsync();
        return Ok(todos);
    }

    // POST api/todo
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoDto dto)
    {
        var todo = new Todo
        {
            Title = dto.Title,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = todo.Id }, todo);
    }

    // PUT api/todo/5
    [HttpPut(""{id}"")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoDto dto)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo == null) return NotFound();

        todo.Title = dto.Title ?? todo.Title;
        todo.IsCompleted = dto.IsCompleted ?? todo.IsCompleted;
        await _db.SaveChangesAsync();
        return Ok(todo);
    }

    // DELETE api/todo/5
    [HttpDelete(""{id}"")]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo == null) return NotFound();

        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
```

### 前端 API 封裝（Vue）

```javascript
// api/todo.js
const API_BASE = import.meta.env.VITE_API_BASE_URL || '/api'

// 這些全都是原生 JavaScript 的 fetch API！
// Vue 沒有自己的 HTTP 客戶端

export async function fetchTodos() {
  const response = await fetch(`${API_BASE}/todo`)
  if (!response.ok) throw new Error('取得待辦清單失敗')
  return response.json()
}

export async function createTodo(title) {
  const response = await fetch(`${API_BASE}/todo`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ title })
  })
  if (!response.ok) throw new Error('新增失敗')
  return response.json()
}

export async function updateTodo(id, data) {
  const response = await fetch(`${API_BASE}/todo/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data)
  })
  if (!response.ok) throw new Error('更新失敗')
  return response.json()
}

export async function deleteTodo(id) {
  const response = await fetch(`${API_BASE}/todo/${id}`, {
    method: 'DELETE'
  })
  if (!response.ok) throw new Error('刪除失敗')
}
```

### Vue 元件使用 API

```vue
<!-- views/TodoApp.vue -->
<template>
  <div class=""todo-app"">
    <h1>Vue + .NET 待辦應用</h1>

    <!-- 新增 -->
    <form @submit.prevent=""handleAdd"">
      <input v-model=""newTitle"" placeholder=""新增待辦..."" />
      <button type=""submit"" :disabled=""loading"">
        {{ loading ? '處理中...' : '新增' }}
      </button>
    </form>

    <!-- 載入狀態 -->
    <div v-if=""loading && !todos.length"">載入中...</div>
    <div v-if=""error"" class=""error"">{{ error }}</div>

    <!-- 列表 -->
    <ul>
      <li v-for=""todo in todos"" :key=""todo.id""
          :class=""{ completed: todo.isCompleted }"">
        <input
          type=""checkbox""
          :checked=""todo.isCompleted""
          @change=""toggleTodo(todo)""
        />
        <span>{{ todo.title }}</span>
        <button @click=""handleDelete(todo.id)"">🗑️</button>
      </li>
    </ul>

    <p>共 {{ todos.length }} 項，{{ remainingCount }} 項未完成</p>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { fetchTodos, createTodo, updateTodo, deleteTodo } from '@/api/todo'

const todos = ref([])
const newTitle = ref('')
const loading = ref(false)
const error = ref('')

const remainingCount = computed(() =>
  todos.value.filter(t => !t.isCompleted).length
)

// 載入待辦清單
onMounted(async () => {
  loading.value = true
  try {
    todos.value = await fetchTodos()
  } catch (err) {
    error.value = err.message
  } finally {
    loading.value = false
  }
})

async function handleAdd() {
  if (!newTitle.value.trim()) return
  loading.value = true
  try {
    const todo = await createTodo(newTitle.value.trim())
    todos.value.unshift(todo)
    newTitle.value = ''
  } catch (err) {
    error.value = err.message
  } finally {
    loading.value = false
  }
}

async function toggleTodo(todo) {
  try {
    const updated = await updateTodo(todo.id, {
      isCompleted: !todo.isCompleted
    })
    const index = todos.value.findIndex(t => t.id === todo.id)
    if (index !== -1) todos.value[index] = updated
  } catch (err) {
    error.value = err.message
  }
}

async function handleDelete(id) {
  try {
    await deleteTodo(id)
    todos.value = todos.value.filter(t => t.id !== id)
  } catch (err) {
    error.value = err.message
  }
}
</script>
```

---

## 📌 CORS 設定

前後端分離時，瀏覽器會阻擋不同來源的請求（CORS 政策）。

```csharp
// Program.cs — ASP.NET Core CORS 設定
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(""VueDev"", policy =>
    {
        policy.WithOrigins(""http://localhost:5173"") // Vue 開發伺服器
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // 如果要帶 Cookie
    });
});

var app = builder.Build();
app.UseCors(""VueDev""); // 套用 CORS 政策
```

### Vite 開發代理（免設 CORS）

```javascript
// vite.config.js
export default defineConfig({
  server: {
    proxy: {
      // 開發時 /api/* 的請求轉發到 .NET 後端
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true
      }
    }
  }
})
```

---

## 📌 JWT 認證整合

### 後端發 Token

```csharp
// Controllers/AuthController.cs
[ApiController]
[Route(""api/auth"")]
public class AuthController : ControllerBase
{
    [HttpPost(""login"")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        // 驗證帳號密碼...
        var token = GenerateJwtToken(user);
        return Ok(new { token, user = new { user.Name, user.Email } });
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(""your-secret-key-at-least-32-chars""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: ""DotNetLearning"",
            audience: ""VueApp"",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

### 前端存取 Token

```javascript
// api/auth.js
export async function login(email, password) {
  const res = await fetch('/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  })
  if (!res.ok) throw new Error('登入失敗')
  const data = await res.json()

  // 存到 localStorage（原生瀏覽器 API）
  localStorage.setItem('token', data.token)
  localStorage.setItem('user', JSON.stringify(data.user))

  return data
}

// 帶 Token 的 API 請求
export async function authFetch(url, options = {}) {
  const token = localStorage.getItem('token')
  return fetch(url, {
    ...options,
    headers: {
      ...options.headers,
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}` // JWT 放在 Header
    }
  })
}
```

---

## 📌 SignalR 即時通訊整合

### 後端 Hub

```csharp
// Hubs/ChatHub.cs
public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        // 廣播訊息給所有連線的客戶端
        await Clients.All.SendAsync(""ReceiveMessage"", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync(""UserJoined"", Context.ConnectionId);
        await base.OnConnectedAsync();
    }
}
```

### Vue 前端連接 SignalR

```bash
npm install @microsoft/signalr
```

```javascript
// composables/useSignalR.js
import { ref, onMounted, onUnmounted } from 'vue'
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

export function useSignalR(hubUrl) {
  const connection = ref(null)
  const isConnected = ref(false)
  const messages = ref([])

  onMounted(async () => {
    // SignalR 客戶端也是 JavaScript 寫的！
    connection.value = new HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build()

    // 監聽伺服端推送的訊息
    connection.value.on('ReceiveMessage', (user, message) => {
      messages.value.push({ user, message, time: new Date() })
    })

    connection.value.on('UserJoined', (userId) => {
      console.log(`使用者 ${userId} 加入了`)
    })

    try {
      await connection.value.start()
      isConnected.value = true
      console.log('SignalR 已連接')
    } catch (err) {
      console.error('SignalR 連接失敗:', err)
    }
  })

  onUnmounted(() => {
    connection.value?.stop()
  })

  // 傳送訊息
  async function sendMessage(user, message) {
    if (!connection.value) return
    await connection.value.invoke('SendMessage', user, message)
  }

  return { isConnected, messages, sendMessage }
}
```

```vue
<!-- views/Chat.vue -->
<template>
  <div class=""chat"">
    <div class=""status"">
      {{ isConnected ? '🟢 已連接' : '🔴 連接中...' }}
    </div>

    <div class=""messages"">
      <div v-for=""(msg, i) in messages"" :key=""i"" class=""message"">
        <strong>{{ msg.user }}</strong>: {{ msg.message }}
        <small>{{ msg.time.toLocaleTimeString() }}</small>
      </div>
    </div>

    <form @submit.prevent=""handleSend"">
      <input v-model=""inputMessage"" placeholder=""輸入訊息..."" />
      <button type=""submit"" :disabled=""!isConnected"">送出</button>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useSignalR } from '@/composables/useSignalR'

const { isConnected, messages, sendMessage } = useSignalR('/chathub')
const inputMessage = ref('')
const userName = ref('匿名使用者')

async function handleSend() {
  if (!inputMessage.value.trim()) return
  await sendMessage(userName.value, inputMessage.value.trim())
  inputMessage.value = ''
}
</script>
```

---

## 📌 總結：Vue 全端開發架構

```
┌─────────────────────────────────────────┐
│               Vue.js 前端                │
│  ┌─────────┐ ┌──────────┐ ┌──────────┐  │
│  │ 元件系統 │ │ Vue Router│ │  Pinia   │  │
│  └─────────┘ └──────────┘ └──────────┘  │
│           ↕ fetch / axios ↕              │
│           ↕ SignalR (WebSocket) ↕        │
├─────────────────────────────────────────┤
│           ASP.NET Core 後端              │
│  ┌─────────┐ ┌──────────┐ ┌──────────┐  │
│  │Controller│ │  SignalR  │ │ 中介軟體 │  │
│  │ (API)    │ │  (Hub)   │ │ (Auth)   │  │
│  └─────────┘ └──────────┘ └──────────┘  │
│           ↕ Entity Framework ↕           │
│  ┌──────────────────────────────────┐   │
│  │      資料庫 (SQL/PostgreSQL)      │   │
│  └──────────────────────────────────┘   │
└─────────────────────────────────────────┘
```

### 核心重點回顧

1. **Vue 是 JavaScript 框架**——不是語言、不是原生語法
2. **`ref()`、`computed()`、`v-model`** 都是 Vue 封裝的 API
3. **`fetch()`、`localStorage`、`WebSocket`** 是瀏覽器原生 API
4. 前後端溝通用 **REST API** (HTTP) 或 **SignalR** (WebSocket)
5. CORS 是瀏覽器的安全機制，開發時可用 Vite Proxy 繞過
6. JWT Token 存前端，每次 API 請求帶在 Header 裡

---

## 💡 全端整合小提醒

- 開發時用 Vite Proxy 比設 CORS 更方便
- JWT 不要存敏感資料（它只是 Base64 編碼，不是加密）
- SignalR 連線斷掉會自動重連（`withAutomaticReconnect()`）
- 正式部署時用反向代理（Nginx）把前後端放在同一個 domain，免去 CORS 問題
- 記住：**學好 JavaScript 基礎是一切的根本！** 框架會變，語言不會
"
            }
        };
    }
}
