using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Angular
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Chapter 800: Angular 入門 ────────────────────────────
        new() { Id=900, Category="frontend", Order=90, Level="beginner", Icon="🅰️", Title="Angular 入門：Google 的企業級框架", Slug="angular-intro", IsPublished=true, Content=@"# 🅰️ Angular 入門：Google 的企業級框架

## 📌 Angular 是什麼？

Angular 是由 **Google 維護**的**完整前端框架**（Full-Featured Framework），專為建構大型企業級單頁應用（SPA）設計。

> ⚠️ **重要觀念：Angular 不是原生 JavaScript！**
> Angular 使用 **TypeScript** 作為主要開發語言。TypeScript 是 JavaScript 的**超集**（Superset），
> 加入了**型別系統**和**進階功能**，最終會**編譯（Compile）成 JavaScript** 才能在瀏覽器執行。
>
> 這和直接寫原生 JS 有本質上的不同——你寫的是 TypeScript，瀏覽器跑的是編譯後的 JavaScript。

### TypeScript vs JavaScript 對比

```typescript
// ❌ 原生 JavaScript — 沒有型別檢查
function add(a, b) {
  return a + b;
}
add(""hello"", 5); // 不會報錯，但結果是 ""hello5""（字串拼接）

// ✅ TypeScript — 編譯時就會檢查型別
function add(a: number, b: number): number {
  return a + b;
}
add(""hello"", 5); // ❌ 編譯錯誤！不允許傳入字串
```

## 📌 Angular vs AngularJS 的區別

| 特性 | AngularJS (1.x) | Angular (2+) |
|------|-----------------|--------------|
| 語言 | JavaScript | **TypeScript** |
| 架構 | MVC | **元件化（Component-based）** |
| 效能 | 雙向綁定效能差 | **變更偵測優化** |
| 行動端 | 不支援 | **支援 PWA / Ionic** |
| 維護狀態 | 已停止維護 | **持續更新中** |

> 💡 **注意**：AngularJS 和 Angular 是完全不同的框架！Angular 2+ 是從頭重寫的。

## 📌 原生 JS vs Angular 開發方式對比

```html
<!-- ❌ 原生 JS：手動操作 DOM -->
<div id=""app"">
  <h1 id=""title""></h1>
  <button onclick=""changeTitle()"">點擊</button>
</div>
<script>
  let title = 'Hello';
  document.getElementById('title').textContent = title;
  function changeTitle() {
    title = '你好！';
    document.getElementById('title').textContent = title; // 手動更新 DOM
  }
</script>
```

```typescript
// ✅ Angular：宣告式綁定，自動同步
@Component({
  selector: 'app-root',
  template: `
    <h1>{{ title }}</h1>
    <button (click)=""changeTitle()"">點擊</button>
  `
})
export class AppComponent {
  title = 'Hello';

  changeTitle() {
    this.title = '你好！'; // 只要改資料，UI 自動更新
  }
}
```

> 🎯 **核心差異**：原生 JS 需要手動操作 DOM，Angular 透過資料綁定**自動同步** UI 和資料。

## 📌 安裝 Angular CLI 與建立專案

```bash
# 1. 確認 Node.js 已安裝（需要 18+ 版本）
node --version

# 2. 全域安裝 Angular CLI
npm install -g @angular/cli

# 3. 建立新專案（會詢問樣式格式和是否啟用 SSR）
ng new my-app

# 4. 進入專案目錄並啟動開發伺服器
cd my-app
ng serve --open
# 瀏覽器會自動開啟 http://localhost:4200
```

## 📌 專案結構解析

```
my-app/
├── src/
│   ├── app/                    # 👈 主要程式碼放這裡
│   │   ├── app.component.ts    # 根元件的 TypeScript（邏輯）
│   │   ├── app.component.html  # 根元件的模板（畫面）
│   │   ├── app.component.css   # 根元件的樣式
│   │   ├── app.component.spec.ts # 單元測試
│   │   ├── app.config.ts       # 應用設定（Standalone 模式）
│   │   └── app.routes.ts       # 路由設定
│   ├── index.html              # 主 HTML 檔（只有一個！SPA）
│   ├── main.ts                 # 應用程式進入點
│   └── styles.css              # 全域樣式
├── angular.json                # Angular CLI 設定
├── tsconfig.json               # TypeScript 編譯設定
└── package.json                # npm 套件管理
```

> 💡 **記住**：`.ts` 檔案是 TypeScript，Angular CLI 會自動編譯成 `.js` 檔案供瀏覽器執行。

## 📌 小結

- Angular 是 Google 維護的**完整框架**，不是函式庫
- 使用 **TypeScript** 開發，編譯後產生 JavaScript
- Angular (2+) 和 AngularJS (1.x) 是**完全不同**的框架
- 透過 Angular CLI 可以快速建立、開發、測試和部署應用
- 相比原生 JS，Angular 提供**結構化**的開發方式和**自動化**的 DOM 更新
" },

        // ── Chapter 801: Angular 基礎 ────────────────────────────
        new() { Id=901, Category="frontend", Order=91, Level="beginner", Icon="🧱", Title="Angular 基礎：模板語法、資料綁定與元件", Slug="angular-basics", IsPublished=true, Content=@"# 🧱 Angular 基礎：模板語法、資料綁定與元件

## 📌 元件（Component）= TypeScript + HTML + CSS

Angular 的核心概念是**元件**。每個元件由三個部分組成：

```typescript
// counter.component.ts — TypeScript 邏輯
import { Component } from '@angular/core';

@Component({
  selector: 'app-counter',        // HTML 標籤名稱
  templateUrl: './counter.component.html',  // 模板檔案
  styleUrls: ['./counter.component.css']    // 樣式檔案
})
export class CounterComponent {
  count = 0;                       // 元件的資料（屬性）

  increment() {                    // 元件的方法
    this.count++;
  }

  decrement() {
    this.count--;
  }
}
```

```html
<!-- counter.component.html — 模板 -->
<div class=""counter"">
  <h2>計數器</h2>
  <p>目前數值：{{ count }}</p>
  <button (click)=""increment()"">+1</button>
  <button (click)=""decrement()"">-1</button>
</div>
```

> 💡 **注意**：`@Component` 是一個**裝飾器（Decorator）**，這是 TypeScript 的功能，原生 JS 沒有。

## 📌 模板語法

### 插值（Interpolation）`{{ }}`

```html
<!-- 顯示元件屬性的值 -->
<h1>{{ title }}</h1>
<p>歡迎，{{ userName }}！你有 {{ messageCount }} 則訊息。</p>

<!-- 也可以放表達式 -->
<p>總價：{{ price * quantity }} 元</p>
<p>狀態：{{ isActive ? '啟用' : '停用' }}</p>
```

### 屬性綁定（Property Binding）`[property]`

```html
<!-- 將元件屬性綁定到 HTML 屬性 -->
<img [src]=""imageUrl"" [alt]=""imageDescription"">
<button [disabled]=""isLoading"">送出</button>
<div [class.active]=""isSelected"">項目</div>
<div [style.color]=""textColor"">彩色文字</div>
```

### 事件綁定（Event Binding）`(event)`

```html
<!-- 監聽 DOM 事件，觸發元件方法 -->
<button (click)=""onSubmit()"">送出</button>
<input (input)=""onInputChange($event)"">
<form (submit)=""onFormSubmit($event)"">
<div (mouseover)=""onHover()"" (mouseleave)=""onLeave()"">
```

### 雙向綁定（Two-way Binding）`[(ngModel)]`

```typescript
// 需要先匯入 FormsModule
import { FormsModule } from '@angular/forms';

@Component({
  // ...
  imports: [FormsModule]  // Standalone component 直接匯入
})
export class SearchComponent {
  searchTerm = '';
}
```

```html
<!-- 輸入框和資料自動雙向同步 -->
<input [(ngModel)]=""searchTerm"" placeholder=""搜尋..."">
<p>你正在搜尋：{{ searchTerm }}</p>
<!-- 當你打字時，searchTerm 自動更新；當程式改 searchTerm 時，輸入框也自動更新 -->
```

## 📌 結構指令

### `*ngIf` — 條件顯示

```html
<div *ngIf=""isLoggedIn"">
  歡迎回來，{{ userName }}！
</div>

<div *ngIf=""items.length > 0; else emptyTemplate"">
  共 {{ items.length }} 個項目
</div>
<ng-template #emptyTemplate>
  <p>目前沒有任何項目。</p>
</ng-template>
```

### `*ngFor` — 迴圈渲染

```html
<ul>
  <li *ngFor=""let item of items; let i = index; trackBy: trackById"">
    {{ i + 1 }}. {{ item.name }} - {{ item.price }} 元
  </li>
</ul>
```

```typescript
// 元件中的 trackBy 函式（效能優化）
trackById(index: number, item: any): number {
  return item.id;
}
```

## 📌 @Input() 和 @Output() 裝飾器

### @Input() — 父元件傳資料給子元件

```typescript
// child.component.ts
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-child',
  template: `<p>收到訊息：{{ message }}</p>`
})
export class ChildComponent {
  @Input() message = '';  // 從父元件接收資料
}
```

```html
<!-- parent.component.html -->
<app-child [message]=""'Hello from parent!'""></app-child>
<app-child [message]=""dynamicMessage""></app-child>
```

### @Output() — 子元件發送事件給父元件

```typescript
// child.component.ts
import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-child',
  template: `<button (click)=""sendMessage()"">通知父元件</button>`
})
export class ChildComponent {
  @Output() notify = new EventEmitter<string>();

  sendMessage() {
    this.notify.emit('子元件說 Hello！');
  }
}
```

```html
<!-- parent.component.html -->
<app-child (notify)=""onChildNotify($event)""></app-child>
```

```typescript
// parent.component.ts
onChildNotify(message: string) {
  console.log('收到子元件訊息：', message);
}
```

## 📌 完整範例：待辦清單

```typescript
// todo.component.ts
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

interface Todo {
  id: number;
  text: string;
  done: boolean;
}

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [FormsModule, CommonModule],
  template: `
    <h2>待辦清單</h2>
    <div>
      <input [(ngModel)]=""newTodo"" placeholder=""新增待辦..."" (keyup.enter)=""addTodo()"">
      <button (click)=""addTodo()"" [disabled]=""!newTodo.trim()"">新增</button>
    </div>
    <ul>
      <li *ngFor=""let todo of todos"" [class.done]=""todo.done"">
        <input type=""checkbox"" [(ngModel)]=""todo.done"">
        {{ todo.text }}
        <button (click)=""removeTodo(todo.id)"">刪除</button>
      </li>
    </ul>
    <p>完成 {{ completedCount }} / {{ todos.length }} 項</p>
  `
})
export class TodoComponent {
  newTodo = '';
  todos: Todo[] = [];
  private nextId = 1;

  get completedCount(): number {
    return this.todos.filter(t => t.done).length;
  }

  addTodo() {
    if (this.newTodo.trim()) {
      this.todos.push({ id: this.nextId++, text: this.newTodo.trim(), done: false });
      this.newTodo = '';
    }
  }

  removeTodo(id: number) {
    this.todos = this.todos.filter(t => t.id !== id);
  }
}
```

> 🎯 **注意**：上面的 `interface Todo` 是 TypeScript 語法，原生 JS 沒有介面（interface）的概念。
> TypeScript 的型別系統讓你在開發時就能發現錯誤，而不是在執行時才出問題。

## 📌 小結

- 元件 = TypeScript（邏輯）+ HTML（模板）+ CSS（樣式）
- 插值 `{{ }}` 顯示資料，`[屬性]` 綁定屬性，`(事件)` 監聽事件
- `[(ngModel)]` 實現雙向綁定
- `*ngIf` 和 `*ngFor` 控制模板結構
- `@Input()` 父傳子，`@Output()` 子傳父
" },

        // ── Chapter 802: Angular 服務與依賴注入 ────────────────────────────
        new() { Id=902, Category="frontend", Order=92, Level="intermediate", Icon="💉", Title="Angular 服務與依賴注入", Slug="angular-services", IsPublished=true, Content=@"# 💉 Angular 服務與依賴注入

## 📌 什麼是服務（Service）？

服務是一個**專注於特定功能**的 TypeScript 類別，用來處理：
- 呼叫 API 取得資料
- 商業邏輯運算
- 跨元件共享資料
- 日誌紀錄等

> 💡 **比喻**：元件是「店員」（負責和顧客互動），服務是「倉庫管理員」（負責管理資料）。
> 店員不需要知道倉庫怎麼運作，只要跟倉庫管理員要東西就好。

```typescript
// user.service.ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'  // 整個應用共享同一個實例（Singleton）
})
export class UserService {
  private users: User[] = [];

  getUsers(): User[] {
    return this.users;
  }

  addUser(user: User): void {
    this.users.push(user);
  }
}
```

## 📌 依賴注入（Dependency Injection, DI）

Angular 的 DI 和 **ASP.NET Core 的 DI 概念完全相同**！

### 對比 ASP.NET Core DI

```csharp
// ASP.NET Core — 在 Program.cs 註冊服務
builder.Services.AddScoped<IUserService, UserService>();

// Controller 透過建構子注入
public class UserController : Controller
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;  // 框架自動注入
    }
}
```

```typescript
// Angular — 在 @Injectable 裝飾器中註冊
@Injectable({
  providedIn: 'root'  // 相當於 ASP.NET Core 的 AddSingleton
})
export class UserService { /* ... */ }

// Component 透過建構子注入
@Component({ /* ... */ })
export class UserListComponent {
  constructor(private userService: UserService) {
    // Angular 框架自動注入，和 ASP.NET Core 一樣！
  }
}
```

> 🎯 **關鍵相似處**：
> - 兩者都是**框架自動管理**物件的建立和生命週期
> - 兩者都透過**建構子注入**
> - 兩者都支援不同的**生命週期**（Singleton / Scoped / Transient）

### Angular DI 的生命週期

```typescript
// Singleton（全應用共享，等同 ASP.NET Core AddSingleton）
@Injectable({ providedIn: 'root' })
export class GlobalService { }

// 元件層級（每個元件實例都有自己的一份，類似 AddScoped）
@Component({
  providers: [LocalService]  // 每次建立元件都會建立新的 LocalService
})
export class MyComponent {
  constructor(private localService: LocalService) { }
}
```

## 📌 HttpClient 發送 HTTP 請求

```typescript
// data.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

interface Product {
  id: number;
  name: string;
  price: number;
}

@Injectable({ providedIn: 'root' })
export class DataService {
  private apiUrl = 'https://api.example.com/products';

  constructor(private http: HttpClient) { }

  // GET — 取得所有產品
  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  // GET — 取得單一產品
  getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  // POST — 新增產品
  addProduct(product: Product): Observable<Product> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<Product>(this.apiUrl, product, { headers });
  }

  // PUT — 更新產品
  updateProduct(product: Product): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl}/${product.id}`, product);
  }

  // DELETE — 刪除產品
  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
```

## 📌 Observable 與 RxJS 基礎

Angular 的 HTTP 請求回傳的不是 Promise，而是 **Observable**。

```typescript
// Observable vs Promise 對比
// Promise（原生 JS / fetch）：
fetch('/api/products')
  .then(res => res.json())
  .then(data => console.log(data));

// Observable（Angular / RxJS）：
this.http.get<Product[]>('/api/products')
  .subscribe(data => console.log(data));
```

### 為什麼用 Observable？

```typescript
import { Observable, interval, Subject } from 'rxjs';
import { map, filter, takeUntil } from 'rxjs/operators';

// 1. 可以取消（Promise 不行！）
const subscription = this.dataService.getProducts().subscribe(data => {
  this.products = data;
});
subscription.unsubscribe(); // 取消請求

// 2. 可以用操作符轉換資料流
this.dataService.getProducts().pipe(
  map(products => products.filter(p => p.price > 100)),  // 過濾貴的產品
  map(products => products.sort((a, b) => a.price - b.price))  // 按價格排序
).subscribe(filtered => {
  this.expensiveProducts = filtered;
});

// 3. 可以處理連續的事件（如 WebSocket、使用者輸入）
// 這是 Promise 做不到的
interval(1000).pipe(
  map(n => `第 ${n} 秒`),
  takeUntil(this.destroy$)  // 元件銷毀時自動停止
).subscribe(msg => console.log(msg));
```

## 📌 完整範例：呼叫 REST API

```typescript
// product-list.component.ts
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-product-list',
  template: `
    <h2>產品列表</h2>
    <div *ngIf=""loading"">載入中...</div>
    <div *ngIf=""error"" class=""error"">{{ error }}</div>
    <ul *ngIf=""!loading && !error"">
      <li *ngFor=""let product of products"">
        {{ product.name }} — NT$ {{ product.price }}
      </li>
    </ul>
    <button (click)=""refresh()"">重新載入</button>
  `
})
export class ProductListComponent implements OnInit, OnDestroy {
  products: Product[] = [];
  loading = false;
  error = '';
  private destroy$ = new Subject<void>();

  constructor(private dataService: DataService) { }

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.loading = true;
    this.error = '';
    this.dataService.getProducts().pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next: (data) => {
        this.products = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = '載入失敗：' + err.message;
        this.loading = false;
      }
    });
  }

  refresh() {
    this.loadProducts();
  }

  ngOnDestroy() {
    this.destroy$.next();     // 發出銷毀信號
    this.destroy$.complete(); // 清理所有訂閱
  }
}
```

## 📌 小結

- 服務（Service）負責商業邏輯和資料存取，元件（Component）負責畫面
- Angular 的 DI 和 ASP.NET Core 的 DI **概念相同**：建構子注入、生命週期管理
- `@Injectable({ providedIn: 'root' })` 等同 ASP.NET Core 的 `AddSingleton`
- HttpClient 回傳 Observable，比 Promise 更強大（可取消、可組合、可處理串流）
- 務必在 `ngOnDestroy` 中取消訂閱，避免記憶體洩漏
" },

        // ── Chapter 803: Angular Router ────────────────────────────
        new() { Id=903, Category="frontend", Order=93, Level="intermediate", Icon="🧭", Title="Angular Router：導航與路由管理", Slug="angular-routing", IsPublished=true, Content=@"# 🧭 Angular Router：導航與路由管理

## 📌 路由模組設定

Angular Router 讓你在**單一頁面**中切換不同的「視圖」，實現 SPA（Single Page Application）。

```typescript
// app.routes.ts
import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { ProductListComponent } from './products/product-list.component';
import { ProductDetailComponent } from './products/product-detail.component';
import { NotFoundComponent } from './not-found/not-found.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },                    // 首頁
  { path: 'about', component: AboutComponent },              // /about
  { path: 'products', component: ProductListComponent },     // /products
  { path: 'products/:id', component: ProductDetailComponent }, // /products/123
  { path: '**', component: NotFoundComponent }               // 404 萬用路由（放最後）
];
```

```typescript
// app.config.ts — Standalone 模式設定
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes)]
};
```

## 📌 RouterLink 與 router-outlet

```html
<!-- app.component.html -->
<nav>
  <!-- routerLink 取代傳統的 <a href>，不會重新載入頁面 -->
  <a routerLink=""/"" routerLinkActive=""active"" [routerLinkActiveOptions]=""{exact: true}"">首頁</a>
  <a routerLink=""/about"" routerLinkActive=""active"">關於</a>
  <a routerLink=""/products"" routerLinkActive=""active"">產品</a>
</nav>

<!-- router-outlet 是路由的「出口」，符合的元件會顯示在這裡 -->
<router-outlet></router-outlet>

<footer>版權所有 &copy; 2024</footer>
```

> 💡 `routerLinkActive=""active""` 會在目前路由符合時自動加上 `active` CSS 類別。

## 📌 路由參數與查詢參數

### 路由參數（Route Parameters）

```typescript
// product-detail.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-detail',
  template: `
    <h2>產品詳情</h2>
    <p>產品 ID：{{ productId }}</p>
  `
})
export class ProductDetailComponent implements OnInit {
  productId = '';

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    // 方法 1：快照（適合不會變的參數）
    this.productId = this.route.snapshot.paramMap.get('id') ?? '';

    // 方法 2：Observable（適合同一元件中參數會改變的情況）
    this.route.paramMap.subscribe(params => {
      this.productId = params.get('id') ?? '';
      // 重新載入資料...
    });
  }
}
```

### 查詢參數（Query Parameters）

```html
<!-- 傳遞查詢參數 /products?category=phone&sort=price -->
<a routerLink=""/products"" [queryParams]=""{category: 'phone', sort: 'price'}"">
  手機（按價格排序）
</a>
```

```typescript
// 讀取查詢參數
this.route.queryParamMap.subscribe(params => {
  const category = params.get('category');  // 'phone'
  const sort = params.get('sort');          // 'price'
});
```

## 📌 子路由（Child Routes）

```typescript
// 路由設定
export const routes: Routes = [
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      { path: '', component: AdminDashboardComponent },       // /admin
      { path: 'users', component: AdminUsersComponent },      // /admin/users
      { path: 'settings', component: AdminSettingsComponent } // /admin/settings
    ]
  }
];
```

```html
<!-- admin-layout.component.html -->
<div class=""admin-layout"">
  <aside>
    <a routerLink=""/admin"">儀表板</a>
    <a routerLink=""/admin/users"">使用者管理</a>
    <a routerLink=""/admin/settings"">系統設定</a>
  </aside>
  <main>
    <router-outlet></router-outlet>  <!-- 子路由的出口 -->
  </main>
</div>
```

## 📌 路由守衛（Guards）

```typescript
// auth.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';

// CanActivate — 進入路由前檢查（最常用）
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true;  // 允許進入
  }

  // 未登入 → 導向登入頁，並記住原本要去的頁面
  return router.createUrlTree(['/login'], {
    queryParams: { returnUrl: state.url }
  });
};

// 在路由設定中使用守衛
export const routes: Routes = [
  { path: 'admin', component: AdminComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent }
];
```

## 📌 懶載入模組（Lazy Loading）

```typescript
// 懶載入 — 只有進入該路由時才下載對應的程式碼
export const routes: Routes = [
  {
    path: 'admin',
    loadComponent: () => import('./admin/admin.component')
      .then(m => m.AdminComponent)
  },
  {
    path: 'reports',
    loadChildren: () => import('./reports/reports.routes')
      .then(m => m.REPORT_ROUTES)  // 載入整個子路由模組
  }
];
```

> 💡 **效能優化**：懶載入讓首頁只下載必要的程式碼，其他頁面的程式碼**按需載入**。
> 這對大型應用特別重要——使用者不需要一次下載所有程式碼。

## 📌 程式化導航

```typescript
import { Router } from '@angular/router';

@Component({ /* ... */ })
export class MyComponent {
  constructor(private router: Router) { }

  goToProduct(id: number) {
    // 程式碼中觸發導航
    this.router.navigate(['/products', id]);
  }

  goToSearch(term: string) {
    this.router.navigate(['/search'], {
      queryParams: { q: term }
    });
  }
}
```

## 📌 小結

- `Routes` 陣列定義路徑和元件的對應關係
- `<router-outlet>` 是路由元件的顯示區域
- 路由參數 `:id` 用於動態路徑，查詢參數用於篩選
- 子路由實現嵌套的頁面佈局
- 路由守衛控制存取權限（如登入驗證）
- 懶載入大幅提升首頁載入速度
" },

        // ── Chapter 804: Angular 表單 ────────────────────────────
        new() { Id=904, Category="frontend", Order=94, Level="intermediate", Icon="📝", Title="Angular 表單：Template-driven vs Reactive Forms", Slug="angular-forms", IsPublished=true, Content=@"# 📝 Angular 表單：Template-driven vs Reactive Forms

## 📌 兩種表單策略

Angular 提供兩種建立表單的方式：

| 特性 | Template-driven | Reactive Forms |
|------|----------------|----------------|
| 定義位置 | HTML 模板中 | TypeScript 類別中 |
| 資料模型 | 隱式（由模板建立） | 顯式（FormGroup / FormControl） |
| 驗證 | 用模板指令 | 用 TypeScript 函式 |
| 適合場景 | 簡單表單 | **複雜表單（推薦）** |
| 可測試性 | 較差 | **優秀** |

## 📌 模板驅動表單（Template-driven）

```typescript
// 需要匯入 FormsModule
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface UserForm {
  name: string;
  email: string;
  age: number;
}

@Component({
  selector: 'app-simple-form',
  standalone: true,
  imports: [FormsModule],
  template: `
    <form #myForm=""ngForm"" (ngSubmit)=""onSubmit(myForm)"">
      <div>
        <label>姓名：</label>
        <input name=""name"" [(ngModel)]=""user.name"" required minlength=""2"" #nameInput=""ngModel"">
        <div *ngIf=""nameInput.invalid && nameInput.touched"" class=""error"">
          <span *ngIf=""nameInput.errors?.['required']"">姓名必填</span>
          <span *ngIf=""nameInput.errors?.['minlength']"">至少 2 個字</span>
        </div>
      </div>

      <div>
        <label>Email：</label>
        <input name=""email"" [(ngModel)]=""user.email"" required email #emailInput=""ngModel"">
        <div *ngIf=""emailInput.invalid && emailInput.touched"" class=""error"">
          Email 格式不正確
        </div>
      </div>

      <button type=""submit"" [disabled]=""myForm.invalid"">送出</button>
    </form>
  `
})
export class SimpleFormComponent {
  user: UserForm = { name: '', email: '', age: 0 };

  onSubmit(form: any) {
    if (form.valid) {
      console.log('表單資料：', this.user);
    }
  }
}
```

## 📌 響應式表單（Reactive Forms）—— 推薦

```typescript
// register.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  template: `
    <form [formGroup]=""registerForm"" (ngSubmit)=""onSubmit()"">
      <div>
        <label>使用者名稱：</label>
        <input formControlName=""username"">
        <div *ngIf=""f['username'].invalid && f['username'].touched"" class=""error"">
          <span *ngIf=""f['username'].errors?.['required']"">必填</span>
          <span *ngIf=""f['username'].errors?.['minlength']"">至少 3 個字</span>
        </div>
      </div>

      <div>
        <label>Email：</label>
        <input formControlName=""email"">
      </div>

      <div formGroupName=""password"">
        <div>
          <label>密碼：</label>
          <input type=""password"" formControlName=""pwd"">
        </div>
        <div>
          <label>確認密碼：</label>
          <input type=""password"" formControlName=""confirmPwd"">
        </div>
        <div *ngIf=""registerForm.get('password')?.errors?.['passwordMismatch']"" class=""error"">
          兩次密碼不一致
        </div>
      </div>

      <div>
        <label>興趣：</label>
        <div formArrayName=""hobbies"">
          <div *ngFor=""let hobby of hobbies.controls; let i = index"">
            <input [formControlName]=""i"">
            <button type=""button"" (click)=""removeHobby(i)"">✕</button>
          </div>
        </div>
        <button type=""button"" (click)=""addHobby()"">+ 新增興趣</button>
      </div>

      <button type=""submit"" [disabled]=""registerForm.invalid"">註冊</button>
    </form>

    <pre>表單值：{{ registerForm.value | json }}</pre>
    <pre>表單狀態：{{ registerForm.status }}</pre>
  `
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: this.fb.group({
        pwd: ['', [Validators.required, Validators.minLength(8)]],
        confirmPwd: ['', Validators.required]
      }, { validators: this.passwordMatchValidator }),
      hobbies: this.fb.array([])  // 動態陣列
    });
  }

  // 便利 getter
  get f() { return this.registerForm.controls; }
  get hobbies() { return this.registerForm.get('hobbies') as FormArray; }

  // 自訂驗證器：密碼比對
  passwordMatchValidator(group: FormGroup): { [key: string]: boolean } | null {
    const pwd = group.get('pwd')?.value;
    const confirmPwd = group.get('confirmPwd')?.value;
    return pwd === confirmPwd ? null : { passwordMismatch: true };
  }

  addHobby() {
    this.hobbies.push(this.fb.control(''));
  }

  removeHobby(index: number) {
    this.hobbies.removeAt(index);
  }

  onSubmit() {
    if (this.registerForm.valid) {
      console.log('送出表單：', this.registerForm.value);
    } else {
      // 標記所有欄位為 touched，觸發錯誤顯示
      this.registerForm.markAllAsTouched();
    }
  }
}
```

## 📌 FormControl、FormGroup、FormArray

```typescript
import { FormControl, FormGroup, FormArray } from '@angular/forms';

// FormControl — 單一表單欄位
const name = new FormControl('預設值');
console.log(name.value);     // '預設值'
console.log(name.valid);     // true
console.log(name.touched);   // false

// FormGroup — 一組表單欄位
const form = new FormGroup({
  firstName: new FormControl(''),
  lastName: new FormControl('')
});
console.log(form.value);  // { firstName: '', lastName: '' }

// FormArray — 動態數量的表單欄位（如多個電話號碼）
const phones = new FormArray([
  new FormControl('0912-345-678'),
  new FormControl('02-1234-5678')
]);
phones.push(new FormControl('新號碼'));  // 動態新增
phones.removeAt(0);                      // 動態移除
```

## 📌 自訂驗證器

```typescript
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

// 同步驗證器 — 檢查是否包含禁止字詞
export function forbiddenNameValidator(forbidden: RegExp): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const isForbidden = forbidden.test(control.value);
    return isForbidden ? { forbiddenName: { value: control.value } } : null;
  };
}

// 非同步驗證器 — 檢查使用者名稱是否已被使用
export function uniqueUsernameValidator(
  userService: UserService
): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    return userService.checkUsername(control.value).pipe(
      map(isTaken => isTaken ? { usernameTaken: true } : null)
    );
  };
}

// 使用自訂驗證器
this.fb.group({
  username: ['', [Validators.required, forbiddenNameValidator(/admin/i)]],
});
```

## 📌 小結

- Template-driven 適合簡單表單，Reactive Forms 適合**複雜表單**
- FormControl = 單一欄位、FormGroup = 欄位群組、FormArray = 動態欄位
- Reactive Forms 在 TypeScript 中定義，**可測試性高**
- 驗證器分為**同步**和**非同步**，可自訂驗證邏輯
- `markAllAsTouched()` 可以在送出時觸發所有驗證訊息顯示
" },

        // ── Chapter 805: Angular 進階 ────────────────────────────
        new() { Id=905, Category="frontend", Order=95, Level="advanced", Icon="⚡", Title="Angular 進階：RxJS、效能優化與 Signals", Slug="angular-advanced", IsPublished=true, Content=@"# ⚡ Angular 進階：RxJS、效能優化與 Signals

## 📌 RxJS 操作符

RxJS（Reactive Extensions for JavaScript）是 Angular 處理非同步資料流的核心工具。

### 常用操作符

```typescript
import { of, from, interval, combineLatest, forkJoin } from 'rxjs';
import { map, filter, switchMap, debounceTime, distinctUntilChanged,
         catchError, retry, tap, take } from 'rxjs/operators';

// map — 轉換資料（類似 Array.map）
of(1, 2, 3).pipe(
  map(x => x * 10)
).subscribe(v => console.log(v));  // 10, 20, 30

// filter — 過濾資料（類似 Array.filter）
of(1, 2, 3, 4, 5).pipe(
  filter(x => x % 2 === 0)
).subscribe(v => console.log(v));  // 2, 4

// switchMap — 切換到新的 Observable（最常用！取消前一個請求）
// 使用情境：搜尋框輸入時，只保留最新的搜尋結果
this.searchControl.valueChanges.pipe(
  debounceTime(300),            // 等使用者停止輸入 300ms
  distinctUntilChanged(),       // 值沒變就不發請求
  switchMap(term =>             // 切換到新的搜尋請求（自動取消舊的）
    this.searchService.search(term).pipe(
      catchError(() => of([]))  // 錯誤時回傳空陣列
    )
  )
).subscribe(results => {
  this.searchResults = results;
});

// combineLatest — 組合多個 Observable 的最新值
combineLatest([
  this.route.paramMap,           // 路由參數
  this.filterService.filters$    // 篩選條件
]).pipe(
  switchMap(([params, filters]) => {
    const id = params.get('categoryId');
    return this.productService.getProducts(id, filters);
  })
).subscribe(products => {
  this.products = products;
});

// forkJoin — 等所有 Observable 都完成（類似 Promise.all）
forkJoin({
  user: this.userService.getUser(userId),
  orders: this.orderService.getOrders(userId),
  preferences: this.prefService.getPreferences(userId)
}).subscribe(({ user, orders, preferences }) => {
  // 三個請求都完成後才執行
  this.user = user;
  this.orders = orders;
  this.preferences = preferences;
});
```

## 📌 Angular Signals（新版響應式 API）

Signals 是 Angular 16+ 引入的新響應式系統，比傳統的 Zone.js 更精確、更高效。

```typescript
import { Component, signal, computed, effect } from '@angular/core';

@Component({
  selector: 'app-counter',
  template: `
    <h2>計數器：{{ count() }}</h2>
    <p>雙倍值：{{ doubleCount() }}</p>
    <button (click)=""increment()"">+1</button>
    <button (click)=""reset()"">重置</button>
  `
})
export class CounterComponent {
  // signal — 可讀可寫的響應式值
  count = signal(0);

  // computed — 根據其他 signal 計算的衍生值（自動追蹤依賴）
  doubleCount = computed(() => this.count() * 2);

  // effect — 當依賴的 signal 變化時自動執行副作用
  logEffect = effect(() => {
    console.log(`計數器變為：${this.count()}`);
  });

  increment() {
    this.count.update(v => v + 1);  // 基於目前值更新
    // 也可以用 this.count.set(10); 直接設定
  }

  reset() {
    this.count.set(0);
  }
}
```

> 💡 **Signal vs Observable**：
> - Signal 是**同步**的，適合管理 UI 狀態
> - Observable 是**非同步**的，適合處理事件流和 HTTP 請求
> - 兩者可以互相轉換：`toSignal()` 和 `toObservable()`

## 📌 變更偵測策略 OnPush

```typescript
import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'app-product-card',
  changeDetection: ChangeDetectionStrategy.OnPush,  // 👈 啟用 OnPush
  template: `
    <div class=""card"">
      <h3>{{ product.name }}</h3>
      <p>NT$ {{ product.price }}</p>
    </div>
  `
})
export class ProductCardComponent {
  @Input() product!: Product;
  // OnPush 策略下，只有當 @Input() 的「參考」改變時才會重新渲染
  // 如果只是修改物件的屬性（mutation），Angular 不會偵測到變化
  // 所以搭配 OnPush 時，要用**不可變資料**（Immutable Data）
}
```

### 預設 vs OnPush 對比

```
預設策略（Default）：
  任何事件 → 檢查所有元件 → 效能差（大型應用會卡頓）

OnPush 策略：
  只在以下情況重新渲染：
  1. @Input() 的參考改變
  2. 元件內的事件觸發
  3. Observable 發出新值（配合 async pipe）
  4. Signal 值改變
  → 效能好！只更新需要更新的元件
```

## 📌 虛擬滾動（Virtual Scrolling）

```typescript
// 需要安裝 @angular/cdk
// npm install @angular/cdk

import { ScrollingModule } from '@angular/cdk/scrolling';

@Component({
  selector: 'app-big-list',
  standalone: true,
  imports: [ScrollingModule],
  template: `
    <!-- 即使有 10,000 筆資料，DOM 上只會渲染可見的幾十筆 -->
    <cdk-virtual-scroll-viewport itemSize=""50"" class=""viewport"">
      <div *cdkVirtualFor=""let item of items"" class=""item"">
        {{ item.name }}
      </div>
    </cdk-virtual-scroll-viewport>
  `,
  styles: [`
    .viewport { height: 400px; }
    .item { height: 50px; }
  `]
})
export class BigListComponent {
  items = Array.from({ length: 10000 }, (_, i) => ({ name: `項目 ${i + 1}` }));
}
```

## 📌 Standalone Components（無模組化）

Angular 14+ 引入了 Standalone Components，不再需要 NgModule：

```typescript
// 傳統模組方式（舊）
@NgModule({
  declarations: [AppComponent, HeaderComponent, FooterComponent],
  imports: [BrowserModule, FormsModule, HttpClientModule],
  bootstrap: [AppComponent]
})
export class AppModule { }

// Standalone 方式（新，推薦）
@Component({
  selector: 'app-root',
  standalone: true,               // 👈 宣告為獨立元件
  imports: [HeaderComponent, FooterComponent, RouterOutlet],  // 直接匯入依賴
  template: `
    <app-header />
    <router-outlet />
    <app-footer />
  `
})
export class AppComponent { }
```

## 📌 Zone.js 與 JavaScript 事件循環

Angular 傳統上使用 **Zone.js** 來偵測狀態變化。它的原理是**攔截所有非同步操作**：

```
JavaScript 事件循環（Event Loop）
┌──────────────────────────────────────┐
│  Call Stack（呼叫堆疊）              │
│  ↓ 執行同步程式碼                     │
├──────────────────────────────────────┤
│  Web APIs                            │
│  ↓ setTimeout、HTTP 請求、DOM 事件    │
├──────────────────────────────────────┤
│  Task Queue（任務佇列）              │
│  ↓ 非同步回呼排隊等待                 │
├──────────────────────────────────────┤
│  Zone.js 在這裡攔截！                │
│  ↓ 每當非同步操作完成，Zone.js 通知   │
│    Angular 執行變更偵測               │
└──────────────────────────────────────┘
```

```typescript
// Zone.js 攔截的操作包括：
// - setTimeout / setInterval
// - Promise.then
// - addEventListener（DOM 事件）
// - XMLHttpRequest / fetch
// - WebSocket

// 這就是為什麼你改一個變數，UI 就自動更新的原因！
// Zone.js 知道「有非同步操作完成了」→ 觸發變更偵測 → 更新 DOM

// Angular 18+ 支援 Zoneless 模式（不再依賴 Zone.js）
// 配合 Signals 使用，效能更好
```

## 📌 小結

- RxJS 操作符（`switchMap`、`combineLatest`、`forkJoin`）是處理複雜非同步流的利器
- Signals 是新一代響應式 API，比 Zone.js 更精確
- OnPush 變更偵測策略大幅提升效能
- 虛擬滾動解決大量資料的渲染效能問題
- Standalone Components 簡化了模組管理
- Zone.js 透過攔截 JS 事件循環來偵測變化（底層機制）
" },

        // ── Chapter 806: Angular 測試與部署 ────────────────────────────
        new() { Id=906, Category="frontend", Order=96, Level="advanced", Icon="🧪", Title="Angular 測試與部署", Slug="angular-testing", IsPublished=true, Content=@"# 🧪 Angular 測試與部署

## 📌 Jasmine + Karma 單元測試

Angular CLI 內建 **Jasmine**（測試框架）和 **Karma**（測試執行器）。

```typescript
// calculator.service.ts
@Injectable({ providedIn: 'root' })
export class CalculatorService {
  add(a: number, b: number): number { return a + b; }
  divide(a: number, b: number): number {
    if (b === 0) throw new Error('不能除以零');
    return a / b;
  }
}
```

```typescript
// calculator.service.spec.ts — 測試檔案（.spec.ts）
import { TestBed } from '@angular/core/testing';
import { CalculatorService } from './calculator.service';

describe('CalculatorService', () => {
  let service: CalculatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});  // 設定測試模組
    service = TestBed.inject(CalculatorService);  // 取得服務實例
  });

  it('應該被建立', () => {
    expect(service).toBeTruthy();
  });

  it('1 + 2 應該等於 3', () => {
    expect(service.add(1, 2)).toBe(3);
  });

  it('10 / 2 應該等於 5', () => {
    expect(service.divide(10, 2)).toBe(5);
  });

  it('除以零應該拋出錯誤', () => {
    expect(() => service.divide(10, 0)).toThrowError('不能除以零');
  });
});
```

```bash
# 執行所有測試
ng test

# 執行測試並產生覆蓋率報告
ng test --code-coverage
```

## 📌 TestBed 元件測試

```typescript
// greeting.component.ts
@Component({
  selector: 'app-greeting',
  template: `
    <h1>{{ greeting }}</h1>
    <button (click)=""changeName('Angular')"">打招呼</button>
  `
})
export class GreetingComponent {
  @Input() name = 'World';
  greeting = '';

  ngOnInit() { this.greeting = `Hello, ${this.name}!`; }

  changeName(newName: string) {
    this.name = newName;
    this.greeting = `Hello, ${this.name}!`;
  }
}
```

```typescript
// greeting.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GreetingComponent } from './greeting.component';

describe('GreetingComponent', () => {
  let component: GreetingComponent;
  let fixture: ComponentFixture<GreetingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GreetingComponent]  // Standalone component
    }).compileComponents();

    fixture = TestBed.createComponent(GreetingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();  // 觸發初始化（ngOnInit）
  });

  it('應該顯示預設招呼語', () => {
    const h1 = fixture.nativeElement.querySelector('h1');
    expect(h1.textContent).toContain('Hello, World!');
  });

  it('應該根據 @Input 顯示不同名字', () => {
    component.name = 'Angular';
    component.ngOnInit();
    fixture.detectChanges();  // 觸發變更偵測
    const h1 = fixture.nativeElement.querySelector('h1');
    expect(h1.textContent).toContain('Hello, Angular!');
  });

  it('按下按鈕後應該改變招呼語', () => {
    const button = fixture.nativeElement.querySelector('button');
    button.click();
    fixture.detectChanges();
    const h1 = fixture.nativeElement.querySelector('h1');
    expect(h1.textContent).toContain('Hello, Angular!');
  });
});
```

## 📌 HttpClientTestingModule Mock API

```typescript
// data.service.spec.ts
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DataService } from './data.service';

describe('DataService', () => {
  let service: DataService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],  // 使用測試用的 HttpClient
      providers: [DataService]
    });
    service = TestBed.inject(DataService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();  // 確認沒有未處理的請求
  });

  it('GET /products 應回傳產品列表', () => {
    const mockProducts = [
      { id: 1, name: '筆電', price: 30000 },
      { id: 2, name: '手機', price: 15000 }
    ];

    service.getProducts().subscribe(products => {
      expect(products.length).toBe(2);
      expect(products[0].name).toBe('筆電');
    });

    // 攔截 HTTP 請求，回傳假資料
    const req = httpMock.expectOne('https://api.example.com/products');
    expect(req.request.method).toBe('GET');
    req.flush(mockProducts);  // 回傳假資料
  });

  it('應處理 HTTP 錯誤', () => {
    service.getProducts().subscribe({
      next: () => fail('應該要失敗'),
      error: (err) => {
        expect(err.status).toBe(500);
      }
    });

    const req = httpMock.expectOne('https://api.example.com/products');
    req.flush('Server Error', { status: 500, statusText: 'Internal Server Error' });
  });
});
```

## 📌 E2E 測試

```typescript
// 使用 Cypress（目前推薦）
// cypress/e2e/app.cy.ts

describe('首頁', () => {
  beforeEach(() => {
    cy.visit('/');
  });

  it('應該顯示歡迎訊息', () => {
    cy.get('h1').should('contain', '歡迎使用');
  });

  it('導航到產品頁', () => {
    cy.get('a[routerLink=""/products""]').click();
    cy.url().should('include', '/products');
    cy.get('.product-card').should('have.length.at.least', 1);
  });

  it('搜尋功能', () => {
    cy.get('input[placeholder=""搜尋...""]').type('Angular');
    cy.get('.search-results').should('be.visible');
    cy.get('.search-result-item').should('have.length.at.least', 1);
  });
});
```

## 📌 Angular CLI 打包優化

```bash
# 生產環境打包（自動開啟 AOT 編譯、Tree Shaking、程式碼壓縮）
ng build --configuration=production

# 分析打包大小
ng build --stats-json
npx webpack-bundle-analyzer dist/my-app/stats.json
```

### angular.json 打包設定

```json
{
  ""configurations"": {
    ""production"": {
      ""budgets"": [
        {
          ""type"": ""initial"",
          ""maximumWarning"": ""500kb"",
          ""maximumError"": ""1mb""
        }
      ],
      ""outputHashing"": ""all"",
      ""optimization"": true,
      ""sourceMap"": false
    }
  }
}
```

## 📌 部署到雲端

```bash
# 部署到 Firebase Hosting
npm install -g firebase-tools
firebase init hosting
ng build --configuration=production
firebase deploy

# 部署到 Azure Static Web Apps
# 在 GitHub Actions 中自動部署
ng build --configuration=production --output-path=dist

# 部署到 Nginx
# 將 dist/ 複製到 Nginx 的靜態資源目錄
# 重要：SPA 需要設定 URL 重寫
```

```nginx
# Nginx 設定（SPA URL 重寫）
server {
    listen 80;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;  # 所有路由都導向 index.html
    }
}
```

## 📌 小結

- `.spec.ts` 檔案是測試檔案，Angular CLI 內建 Jasmine + Karma
- TestBed 是 Angular 的測試工具，用來建立元件和服務的測試環境
- HttpClientTestingModule 可以 Mock HTTP 請求，不需要真正的後端
- E2E 測試推薦使用 Cypress
- `ng build --configuration=production` 自動執行 AOT、Tree Shaking、壓縮
- SPA 部署需要設定 URL 重寫（所有路由指向 index.html）
" },

        // ── Chapter 807: Angular + ASP.NET Core 全端整合 ────────────────────────────
        new() { Id=907, Category="frontend", Order=97, Level="advanced", Icon="🔗", Title="Angular + ASP.NET Core 全端整合", Slug="angular-fullstack", IsPublished=true, Content=@"# 🔗 Angular + ASP.NET Core 全端整合

## 📌 前後端分離架構

```
┌─────────────────────────────────────────────────┐
│                    使用者瀏覽器                    │
│  ┌────────────────────────────────────────────┐  │
│  │         Angular SPA (TypeScript)           │  │
│  │  • 處理 UI 互動                             │  │
│  │  • 路由管理                                 │  │
│  │  • 狀態管理                                 │  │
│  │  • 透過 HttpClient 呼叫 API                │  │
│  └──────────────┬─────────────────────────────┘  │
│                 │ HTTP 請求 (JSON)               │
└─────────────────┼───────────────────────────────┘
                  │
┌─────────────────┼───────────────────────────────┐
│  ASP.NET Core   │ Web API                       │
│  ┌──────────────▼─────────────────────────────┐  │
│  │         Controllers / Minimal API          │  │
│  │  • 處理 HTTP 請求                           │  │
│  │  • 商業邏輯                                 │  │
│  │  • 資料驗證                                 │  │
│  │  • 存取資料庫                               │  │
│  └────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────┘
```

## 📌 HttpClient 呼叫 .NET API

### Angular 端：建立 API Service

```typescript
// api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  // 根據環境切換 API 網址
  private baseUrl = environment.apiUrl;  // 例如 https://localhost:5001/api

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}/products`);
  }

  getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/products/${id}`);
  }

  createProduct(product: Omit<Product, 'id'>): Observable<Product> {
    return this.http.post<Product>(`${this.baseUrl}/products`, product);
  }

  updateProduct(id: number, product: Product): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/products/${id}`, product);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/products/${id}`);
  }
}
```

### ASP.NET Core 端：API Controller

```csharp
// ProductsController.cs
[ApiController]
[Route(""api/[controller]"")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        return await _db.Products.ToListAsync();
    }

    [HttpGet(""{id}"")]
    public async Task<ActionResult<Product>> Get(int id)
    {
        var product = await _db.Products.FindAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }
}
```

## 📌 CORS 設定

前後端分離時，Angular（`localhost:4200`）和 .NET API（`localhost:5001`）在不同的 origin，
瀏覽器會阻擋跨域請求。需要在 .NET 端設定 CORS：

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 註冊 CORS 政策
builder.Services.AddCors(options =>
{
    options.AddPolicy(""AllowAngular"", policy =>
    {
        policy.WithOrigins(""http://localhost:4200"")  // Angular 開發伺服器
              .AllowAnyHeader()                       // 允許任何請求標頭
              .AllowAnyMethod()                       // 允許 GET, POST, PUT, DELETE
              .AllowCredentials();                    // 允許傳送 Cookie
    });
});

var app = builder.Build();

// 啟用 CORS（要放在 UseRouting 之後、UseAuthorization 之前）
app.UseCors(""AllowAngular"");
```

> ⚠️ **生產環境**請勿使用 `AllowAnyOrigin()`，應該限定特定的前端網域。

## 📌 JWT Interceptor 攔截器

Angular 的 HTTP Interceptor 可以在**每個請求**自動加上 JWT Token：

```typescript
// auth.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  if (token) {
    // 複製請求並加上 Authorization 標頭
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(authReq);
  }

  return next(req);
};
```

```typescript
// app.config.ts — 註冊攔截器
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([authInterceptor])),
    provideRouter(routes)
  ]
};
```

### Auth Service

```typescript
// auth.service.ts
@Injectable({ providedIn: 'root' })
export class AuthService {
  private tokenKey = 'jwt_token';

  constructor(private http: HttpClient, private router: Router) { }

  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post<{ token: string }>('/api/auth/login', credentials).pipe(
      tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
      })
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    // 檢查 token 是否過期（解析 JWT payload）
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp > Date.now() / 1000;
  }
}
```

## 📌 SignalR 即時通訊整合

SignalR 讓 Angular 和 .NET 之間建立**雙向即時通訊**：

### ASP.NET Core Hub

```csharp
// ChatHub.cs
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        // 廣播訊息給所有連線的客戶端
        await Clients.All.SendAsync(""ReceiveMessage"", user, message);
    }

    public async Task JoinRoom(string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        await Clients.Group(room).SendAsync(""ReceiveMessage"", ""系統"", $""{Context.ConnectionId} 加入了 {room}"");
    }
}
```

### Angular 端：SignalR Client

```typescript
// chat.service.ts
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

export interface ChatMessage {
  user: string;
  message: string;
  timestamp: Date;
}

@Injectable({ providedIn: 'root' })
export class ChatService {
  private hubConnection!: signalR.HubConnection;
  private messagesSubject = new BehaviorSubject<ChatMessage[]>([]);
  messages$ = this.messagesSubject.asObservable();

  connect(token: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/chatHub', {
        accessTokenFactory: () => token  // JWT 認證
      })
      .withAutomaticReconnect()  // 斷線自動重連
      .build();

    // 監聽伺服器發送的訊息
    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      const current = this.messagesSubject.value;
      this.messagesSubject.next([...current, {
        user, message, timestamp: new Date()
      }]);
    });

    // 啟動連線
    this.hubConnection.start()
      .then(() => console.log('SignalR 已連線'))
      .catch(err => console.error('SignalR 連線失敗', err));
  }

  sendMessage(user: string, message: string) {
    this.hubConnection.invoke('SendMessage', user, message);
  }

  joinRoom(room: string) {
    this.hubConnection.invoke('JoinRoom', room);
  }

  disconnect() {
    this.hubConnection.stop();
  }
}
```

```typescript
// chat.component.ts
@Component({
  selector: 'app-chat',
  template: `
    <div class=""chat-room"">
      <div class=""messages"">
        <div *ngFor=""let msg of messages$ | async"" class=""message"">
          <strong>{{ msg.user }}：</strong>{{ msg.message }}
          <small>{{ msg.timestamp | date:'HH:mm:ss' }}</small>
        </div>
      </div>
      <div class=""input-area"">
        <input [(ngModel)]=""newMessage"" (keyup.enter)=""send()"" placeholder=""輸入訊息..."">
        <button (click)=""send()"">送出</button>
      </div>
    </div>
  `
})
export class ChatComponent implements OnInit, OnDestroy {
  messages$ = this.chatService.messages$;
  newMessage = '';

  constructor(
    private chatService: ChatService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    const token = this.authService.getToken() ?? '';
    this.chatService.connect(token);
  }

  send() {
    if (this.newMessage.trim()) {
      this.chatService.sendMessage('我', this.newMessage);
      this.newMessage = '';
    }
  }

  ngOnDestroy() {
    this.chatService.disconnect();
  }
}
```

## 📌 Environment 設定

```typescript
// environments/environment.ts（開發環境）
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};

// environments/environment.prod.ts（生產環境）
export const environment = {
  production: true,
  apiUrl: 'https://myapp.azurewebsites.net/api'
};
```

## 📌 小結

- Angular + ASP.NET Core 是企業級全端組合
- 前後端透過 **REST API**（JSON）溝通
- CORS 設定是前後端分離的必要步驟
- JWT Interceptor 自動在每個請求加上認證 Token
- SignalR 提供**雙向即時通訊**（聊天室、通知推播）
- 使用 environment 檔案管理不同環境的 API 位址
- TypeScript 讓前後端的資料結構可以**保持一致**（前端 interface 對應後端 DTO）
" }
    };
}
