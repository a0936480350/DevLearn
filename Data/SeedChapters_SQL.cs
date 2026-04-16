using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_SQL
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── 1400: SQL 簡介與環境設置 ──
        new() { Id=1400, Category="sql", Order=1, Level="beginner", Icon="📘", Title="SQL 簡介與環境設置", Slug="sql-intro", IsPublished=true, Content=@"
# SQL 簡介與環境設置

## 什麼是 SQL？

> **比喻：SQL 就像圖書館的查詢系統** 📚
>
> 想像你去一間超大圖書館，裡面有幾百萬本書。
> 你不可能自己一本一本翻找——你需要一個查詢系統。
> SQL 就是這個查詢系統，讓你用簡單的指令找到你要的資料。

SQL（Structured Query Language）是操作**關聯式資料庫**的標準語言。

```
你（使用者）  →  SQL 指令  →  資料庫  →  回傳結果
```

---

## 關聯式資料庫是什麼？

資料庫把資料存在**表格（Table）**裡，就像 Excel 的工作表。

```
┌─────────────────────────────────────┐
│            Students 表               │
├────┬──────────┬─────┬───────────────┤
│ Id │ Name     │ Age │ Email         │
├────┼──────────┼─────┼───────────────┤
│  1 │ 小明     │  20 │ ming@test.com │
│  2 │ 小華     │  22 │ hua@test.com  │
│  3 │ 小美     │  21 │ mei@test.com  │
└────┴──────────┴─────┴───────────────┘
```

- **行（Row）** = 一筆資料（一個學生）
- **欄（Column）** = 一個欄位（姓名、年齡...）
- **主鍵（Primary Key）** = 每筆資料的唯一識別碼（Id）

---

## 常見的資料庫系統

| 資料庫        | 特點                     | 常見用途        |
|--------------|--------------------------|----------------|
| PostgreSQL   | 功能強大、免費開源        | Web 應用、企業   |
| SQL Server   | 微軟出品、與 .NET 整合好  | 企業、政府       |
| MySQL        | 輕量、廣泛使用           | WordPress、小型  |
| SQLite       | 檔案型、零設定           | 手機 App、嵌入式 |

---

## SQL 的四大類指令

```sql
-- 1. DDL（Data Definition Language）— 定義結構
CREATE TABLE ...     -- 建立表格
ALTER TABLE ...      -- 修改表格結構
DROP TABLE ...       -- 刪除表格

-- 2. DML（Data Manipulation Language）— 操作資料
SELECT ...           -- 查詢資料
INSERT INTO ...      -- 新增資料
UPDATE ...           -- 修改資料
DELETE FROM ...      -- 刪除資料

-- 3. DCL（Data Control Language）— 權限控制
GRANT ...            -- 授予權限
REVOKE ...           -- 撤銷權限

-- 4. TCL（Transaction Control Language）— 交易控制
BEGIN TRANSACTION    -- 開始交易
COMMIT               -- 確認交易
ROLLBACK             -- 取消交易
```

---

## 本系列教學環境

本系列使用 **PostgreSQL** 語法為主，大部分語法通用於各資料庫。

> 💡 **提醒：** SQL 關鍵字不分大小寫，但**慣例用大寫**以便閱讀。
> `SELECT` 和 `select` 效果相同，但 `SELECT` 更清楚。

下一章我們開始寫第一個查詢！
" },

        // ── 1401: SELECT 基礎查詢 ──
        new() { Id=1401, Category="sql", Order=2, Level="beginner", Icon="🔍", Title="SELECT 基礎查詢", Slug="sql-select-basics", IsPublished=true, Content=@"
# SELECT 基礎查詢

## SELECT — 查詢的起點

> **比喻：SELECT 就像在餐廳點菜** 🍽️
>
> 你告訴服務員「我要看菜單上的主菜和甜點」——
> SELECT 就是告訴資料庫「我要看哪些欄位的資料」。

---

## 查詢所有欄位

```sql
SELECT *             -- ← * 代表「所有欄位」
FROM Students;       -- ← 從 Students 這張表查
```

逐行解析：
```
SELECT *         -- 「我要全部欄位」——Id, Name, Age, Email 都給我
FROM Students;   -- 「從 Students 表裡面找」——指定資料來源
```

結果：
```
 Id | Name | Age | Email
----+------+-----+---------------
  1 | 小明 |  20 | ming@test.com
  2 | 小華 |  22 | hua@test.com
  3 | 小美 |  21 | mei@test.com
```

---

## 查詢特定欄位

```sql
SELECT Name, Age     -- ← 只要名字和年齡
FROM Students;       -- ← 從 Students 表查
```

逐行解析：
```
SELECT Name, Age  -- 「我只要 Name 跟 Age 這兩欄」——其他欄位不要
FROM Students;    -- 「來源是 Students」
```

結果只有兩欄：
```
 Name | Age
------+-----
 小明 |  20
 小華 |  22
 小美 |  21
```

> 💡 **實務建議：** 盡量不用 `SELECT *`，只選需要的欄位，效能更好。

---

## 欄位別名（AS）

```sql
SELECT Name AS 姓名,      -- ← 把 Name 顯示為「姓名」
       Age AS 年齡,        -- ← 把 Age 顯示為「年齡」
       Email AS 電子信箱   -- ← 把 Email 顯示為「電子信箱」
FROM Students;
```

逐行解析：
```
SELECT Name AS 姓名    -- Name 欄位改名叫「姓名」，只影響顯示，不影響資料庫
       Age AS 年齡      -- AS 是取別名的關鍵字
       Email AS 電子信箱 -- 別名可以用中文，讓報表更易讀
FROM Students;
```

---

## 去除重複（DISTINCT）

```sql
SELECT DISTINCT Age   -- ← 只取不重複的年齡
FROM Students;        -- ← 從 Students 表查
```

假設有 5 個學生，其中 2 人都是 20 歲：
```
-- 沒有 DISTINCT → 20 會出現兩次
-- 有 DISTINCT   → 20 只出現一次

 Age
-----
  20
  21
  22
```

---

## 計算欄位

```sql
SELECT Name,                    -- ← 學生姓名
       Age,                     -- ← 現在年齡
       Age + 1 AS 明年年齡,     -- ← 用運算產生新欄位
       Age * 2 AS 雙倍年齡      -- ← 可以做任何數學運算
FROM Students;
```

> SQL 可以直接在 SELECT 裡做運算，結果會變成一個臨時欄位。

---

## 字串串接

```sql
-- PostgreSQL 用 || 串接字串
SELECT Name || ' (' || Age || '歲)' AS 簡介
FROM Students;
-- 結果：小明 (20歲)、小華 (22歲)...

-- SQL Server 用 + 串接
SELECT Name + ' (' + CAST(Age AS VARCHAR) + '歲)' AS 簡介
FROM Students;
```

---

## 小結

| 語法 | 用途 |
|------|------|
| `SELECT *` | 查全部欄位 |
| `SELECT 欄位1, 欄位2` | 查指定欄位 |
| `AS 別名` | 欄位重新命名 |
| `DISTINCT` | 去重複 |
| 運算式 | 產生計算欄位 |
" },

        // ── 1402: WHERE 條件篩選 ──
        new() { Id=1402, Category="sql", Order=3, Level="beginner", Icon="🎯", Title="WHERE 條件篩選", Slug="sql-where", IsPublished=true, Content=@"
# WHERE 條件篩選

## WHERE — 過濾你要的資料

> **比喻：WHERE 就像篩金子的篩子** ⛏️
>
> 你從河裡撈了一堆沙子和石頭（所有資料），
> WHERE 就是那個篩子，只讓符合條件的金子（資料）通過。

---

## 基本比較運算子

```sql
SELECT * FROM Students
WHERE Age = 20;          -- ← 年齡「等於」20 的學生
```

逐行解析：
```
SELECT * FROM Students   -- 從 Students 表取所有欄位
WHERE Age = 20;          -- 但只要 Age 等於 20 的那些列
                         -- 注意：SQL 用 = 而不是 ==
```

### 所有比較運算子

```sql
WHERE Age = 20      -- 等於
WHERE Age != 20     -- 不等於（也可以寫 <>）
WHERE Age > 20      -- 大於
WHERE Age >= 20     -- 大於等於
WHERE Age < 20      -- 小於
WHERE Age <= 20     -- 小於等於
```

---

## AND、OR、NOT — 組合條件

```sql
-- AND：兩個條件都要成立
SELECT * FROM Students
WHERE Age >= 20          -- ← 條件 1：年齡 >= 20
  AND Email LIKE '%@gmail%';  -- ← 條件 2：Email 包含 @gmail
```

逐行解析：
```
WHERE Age >= 20              -- 第一個篩選條件
  AND Email LIKE '%@gmail%'  -- AND = 而且，兩個都要成立才會被選中
                             -- 只有「年齡>=20 且 用 gmail」的學生才會出現
```

```sql
-- OR：其中一個條件成立就行
SELECT * FROM Students
WHERE Age = 20           -- ← 20 歲
   OR Age = 22;          -- ← 或 22 歲
-- 20 歲或 22 歲的學生都會被選中
```

```sql
-- NOT：反轉條件
SELECT * FROM Students
WHERE NOT Age = 20;      -- ← 不是 20 歲的（等同 Age != 20）
```

---

## BETWEEN — 範圍查詢

```sql
SELECT * FROM Students
WHERE Age BETWEEN 20 AND 22;  -- ← 20 到 22 歲之間（包含頭尾）
```

逐行解析：
```
WHERE Age BETWEEN 20 AND 22   -- 等同於 Age >= 20 AND Age <= 22
                               -- BETWEEN 是「包含邊界」的
                               -- 20, 21, 22 歲都符合
```

---

## IN — 列表查詢

```sql
SELECT * FROM Students
WHERE Age IN (20, 21, 25);    -- ← 年齡是 20 或 21 或 25 的
```

逐行解析：
```
WHERE Age IN (20, 21, 25)     -- 等同於 Age=20 OR Age=21 OR Age=25
                               -- IN 比寫一堆 OR 更簡潔
                               -- 括號裡可以放很多值
```

---

## LIKE — 模糊比對

```sql
-- % 代表「任意字元，任意個數」
SELECT * FROM Students
WHERE Name LIKE '小%';        -- ← 名字以「小」開頭的
-- 小明 ✓, 小華 ✓, 大明 ✗

SELECT * FROM Students
WHERE Email LIKE '%@gmail.com';  -- ← Email 結尾是 @gmail.com

-- _ 代表「任意一個字元」
SELECT * FROM Students
WHERE Name LIKE '小_';        -- ← 「小」後面剛好一個字
-- 小明 ✓, 小華 ✓, 小美美 ✗
```

---

## IS NULL / IS NOT NULL

```sql
-- 檢查空值（NULL）
SELECT * FROM Students
WHERE Email IS NULL;           -- ← 沒有填 Email 的學生

SELECT * FROM Students
WHERE Email IS NOT NULL;       -- ← 有填 Email 的學生
```

> ⚠️ **重要：** NULL 不能用 `= NULL`，必須用 `IS NULL`！
> ```sql
> WHERE Email = NULL    -- ❌ 永遠不會回傳結果！
> WHERE Email IS NULL   -- ✅ 正確寫法
> ```

---

## 條件優先順序

```sql
-- 容易搞混的情況
SELECT * FROM Students
WHERE Age = 20 OR Age = 22
  AND Email LIKE '%@gmail%';

-- ⚠️ AND 優先於 OR！上面等同於：
-- Age = 20 OR (Age = 22 AND Email LIKE '%@gmail%')
-- 不是 (Age = 20 OR Age = 22) AND Email LIKE '%@gmail%'

-- ✅ 用括號明確表達意圖
SELECT * FROM Students
WHERE (Age = 20 OR Age = 22)
  AND Email LIKE '%@gmail%';
```

---

## 小結

| 語法 | 用途 | 範例 |
|------|------|------|
| `=, !=, >, <` | 比較 | `Age > 20` |
| `AND, OR, NOT` | 邏輯組合 | `Age > 20 AND Age < 30` |
| `BETWEEN` | 範圍 | `Age BETWEEN 20 AND 30` |
| `IN` | 列表 | `Age IN (20, 21, 22)` |
| `LIKE` | 模糊比對 | `Name LIKE '小%'` |
| `IS NULL` | 空值檢查 | `Email IS NULL` |
" },

        // ── 1403: ORDER BY 與 LIMIT ──
        new() { Id=1403, Category="sql", Order=4, Level="beginner", Icon="📊", Title="ORDER BY 排序與 LIMIT 分頁", Slug="sql-orderby-limit", IsPublished=true, Content=@"
# ORDER BY 排序與 LIMIT 分頁

## ORDER BY — 排序結果

> **比喻：ORDER BY 就像整理書架** 📚
>
> 你從圖書館找到了一堆書（查詢結果），
> 但它們亂七八糟的——ORDER BY 幫你按作者、書名或出版日期排好。

---

## 升冪排序（預設）

```sql
SELECT Name, Age
FROM Students
ORDER BY Age;            -- ← 按年齡從小到大排
```

逐行解析：
```
SELECT Name, Age         -- 要看名字和年齡
FROM Students            -- 從學生表
ORDER BY Age;            -- 按 Age 排序，預設是 ASC（升冪 = 小到大）
```

結果：
```
 Name | Age
------+-----
 小明 |  20
 小美 |  21
 小華 |  22
```

---

## 降冪排序（DESC）

```sql
SELECT Name, Age
FROM Students
ORDER BY Age DESC;       -- ← DESC = Descending = 從大到小
```

```
 Name | Age
------+-----
 小華 |  22    ← 最大的在最上面
 小美 |  21
 小明 |  20
```

---

## 多欄排序

```sql
SELECT Name, Age, Score
FROM Students
ORDER BY Age ASC,        -- ← 先按年齡升冪
         Score DESC;     -- ← 年齡相同時，按分數降冪
```

逐行解析：
```
ORDER BY Age ASC         -- 第一排序條件：年齡小到大
         Score DESC      -- 第二排序條件：同齡時分數高到低
                         -- 就像先按姓氏排，再按名字排
```

---

## LIMIT — 只取前 N 筆

```sql
-- PostgreSQL / MySQL
SELECT * FROM Students
ORDER BY Age
LIMIT 5;                 -- ← 只取前 5 筆

-- SQL Server
SELECT TOP 5 * FROM Students
ORDER BY Age;            -- ← SQL Server 用 TOP 而不是 LIMIT
```

逐行解析：
```
SELECT * FROM Students   -- 查所有學生
ORDER BY Age             -- 按年齡排序
LIMIT 5;                 -- 但只回傳前 5 筆
                         -- 常用於「排行榜前 10 名」等場景
```

---

## OFFSET — 跳過前 N 筆（分頁）

```sql
-- 第 1 頁（每頁 10 筆）
SELECT * FROM Students
ORDER BY Id
LIMIT 10 OFFSET 0;      -- ← 跳過 0 筆，取 10 筆

-- 第 2 頁
SELECT * FROM Students
ORDER BY Id
LIMIT 10 OFFSET 10;     -- ← 跳過前 10 筆，取接下來 10 筆

-- 第 3 頁
SELECT * FROM Students
ORDER BY Id
LIMIT 10 OFFSET 20;     -- ← 跳過前 20 筆
```

逐行解析：
```
LIMIT 10 OFFSET 10      -- LIMIT = 一次取幾筆（每頁筆數）
                         -- OFFSET = 跳過幾筆（前面的頁數 × 每頁筆數）
                         -- 公式：OFFSET = (頁碼 - 1) × 每頁筆數
```

---

## 實用範例

```sql
-- 找出年紀最大的 3 位學生
SELECT Name, Age
FROM Students
ORDER BY Age DESC        -- 年齡由大到小
LIMIT 3;                 -- 只取前 3 筆

-- 找出分數最高的學生（只要第 1 名）
SELECT Name, Score
FROM Students
ORDER BY Score DESC
LIMIT 1;                 -- 只取冠軍

-- 找出第 2~4 名（跳過第 1 名）
SELECT Name, Score
FROM Students
ORDER BY Score DESC
LIMIT 3 OFFSET 1;       -- 跳過 1 筆，取 3 筆
```

---

## 小結

| 語法 | 用途 |
|------|------|
| `ORDER BY col` | 升冪排序（小到大） |
| `ORDER BY col DESC` | 降冪排序（大到小） |
| `ORDER BY col1, col2` | 多欄排序 |
| `LIMIT n` | 只取前 n 筆 |
| `OFFSET n` | 跳過前 n 筆 |
| `LIMIT + OFFSET` | 分頁查詢 |
" },

        // ── 1404: INSERT、UPDATE、DELETE ──
        new() { Id=1404, Category="sql", Order=5, Level="beginner", Icon="✏️", Title="INSERT、UPDATE、DELETE 資料操作", Slug="sql-insert-update-delete", IsPublished=true, Content=@"
# INSERT、UPDATE、DELETE 資料操作

## 三大操作一覽

> **比喻：** 📝
> - INSERT = 在筆記本上「新增一頁」
> - UPDATE = 把某一頁的內容「修改」
> - DELETE = 把某一頁「撕掉」

---

## INSERT — 新增資料

### 新增一筆

```sql
INSERT INTO Students (Name, Age, Email)      -- ← 指定要填的欄位
VALUES ('小明', 20, 'ming@test.com');         -- ← 對應的值
```

逐行解析：
```
INSERT INTO Students     -- 要新增資料到 Students 表
(Name, Age, Email)       -- 指定要填哪些欄位（Id 通常自動產生）
VALUES ('小明', 20, 'ming@test.com')  -- 填入對應的值
                         -- 字串用單引號 '...'
                         -- 數字直接寫
```

### 新增多筆

```sql
INSERT INTO Students (Name, Age, Email)
VALUES
    ('小明', 20, 'ming@test.com'),    -- ← 第 1 筆
    ('小華', 22, 'hua@test.com'),     -- ← 第 2 筆
    ('小美', 21, 'mei@test.com');     -- ← 第 3 筆（最後用分號）
```

### 用 SELECT 結果新增

```sql
-- 把查詢結果直接插入另一個表
INSERT INTO GoodStudents (Name, Age)
SELECT Name, Age                      -- ← 從查詢結果取值
FROM Students
WHERE Score >= 90;                    -- ← 只取 90 分以上的
```

---

## UPDATE — 修改資料

```sql
UPDATE Students               -- ← 要修改 Students 表
SET Age = 21,                 -- ← 把 Age 改成 21
    Email = 'new@test.com'    -- ← 同時改 Email
WHERE Id = 1;                 -- ← 只改 Id=1 的那筆
```

逐行解析：
```
UPDATE Students           -- 指定要修改的表
SET Age = 21              -- SET 後面接「欄位 = 新值」
    Email = 'new@test.com' -- 可以同時改多個欄位，用逗號分隔
WHERE Id = 1              -- ⚠️ 一定要加 WHERE！不然會改到全部資料！
```

### 用運算式更新

```sql
-- 所有學生年齡 +1
UPDATE Students
SET Age = Age + 1;           -- ← 沒有 WHERE = 全部都改

-- 分數低於 60 的加 10 分（補救）
UPDATE Students
SET Score = Score + 10
WHERE Score < 60;
```

> ⚠️ **超重要警告：** UPDATE 忘了加 WHERE 會改掉**全部資料**！

---

## DELETE — 刪除資料

```sql
DELETE FROM Students          -- ← 從 Students 表刪除
WHERE Id = 3;                 -- ← 只刪 Id=3 的那筆
```

逐行解析：
```
DELETE FROM Students     -- 指定要從哪個表刪除
WHERE Id = 3;            -- ⚠️ 一定要加 WHERE！不然會刪光全部！
```

### 刪除所有資料

```sql
-- 方法 1：DELETE（可以 rollback，較慢）
DELETE FROM Students;         -- 刪掉所有資料，但表還在

-- 方法 2：TRUNCATE（不可 rollback，較快）
TRUNCATE TABLE Students;      -- 清空表格，更快速
```

---

## 安全操作守則 🛡️

```sql
-- ✅ 好習慣：先用 SELECT 確認要改/刪的資料
SELECT * FROM Students WHERE Id = 3;   -- 先看看是哪筆

-- 確認沒問題後再改
UPDATE Students SET Age = 25 WHERE Id = 3;

-- ✅ 好習慣：用 Transaction 包起來
BEGIN;
    DELETE FROM Students WHERE Score < 30;
    -- 看看結果對不對...
    SELECT COUNT(*) FROM Students;
COMMIT;   -- 確認沒問題就 COMMIT
-- 或
ROLLBACK; -- 後悔了就 ROLLBACK 復原
```

---

## RETURNING（PostgreSQL 專用）

```sql
-- 新增後直接回傳新資料
INSERT INTO Students (Name, Age)
VALUES ('小新', 19)
RETURNING Id, Name;          -- ← 回傳自動產生的 Id
-- 結果：Id=4, Name=小新

-- 刪除後確認刪了什麼
DELETE FROM Students
WHERE Score < 30
RETURNING *;                 -- ← 回傳被刪的資料
```

---

## 小結

| 語法 | 用途 | 危險度 |
|------|------|--------|
| `INSERT INTO ... VALUES` | 新增資料 | 🟢 低 |
| `UPDATE ... SET ... WHERE` | 修改資料 | 🟡 中（忘 WHERE 改全部） |
| `DELETE FROM ... WHERE` | 刪除資料 | 🔴 高（忘 WHERE 刪光） |
| `TRUNCATE` | 清空表格 | 🔴 高（不可復原） |
" },

        // ── 1405: 資料型態與約束 ──
        new() { Id=1405, Category="sql", Order=6, Level="beginner", Icon="🏗️", Title="CREATE TABLE 與資料型態", Slug="sql-create-table", IsPublished=true, Content=@"
# CREATE TABLE 與資料型態

## CREATE TABLE — 建立你的第一張表

> **比喻：CREATE TABLE 就像設計 Excel 表頭** 📋
>
> 在填資料之前，你得先決定有哪些欄位、每欄放什麼類型的資料。
> CREATE TABLE 就是在做這件事。

---

## 基本語法

```sql
CREATE TABLE Students (          -- ← 建立 Students 表
    Id SERIAL PRIMARY KEY,       -- ← 自動遞增的主鍵
    Name VARCHAR(50) NOT NULL,   -- ← 最多 50 字的字串，不可為空
    Age INT,                     -- ← 整數
    Email VARCHAR(100) UNIQUE,   -- ← 最多 100 字，不可重複
    Score DECIMAL(5,2),          -- ← 總 5 位數，小數 2 位
    IsActive BOOLEAN DEFAULT true, -- ← 布林值，預設 true
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP  -- ← 自動記錄建立時間
);
```

逐行解析：
```
Id SERIAL PRIMARY KEY        -- SERIAL = 自動遞增整數（1, 2, 3...）
                             -- PRIMARY KEY = 主鍵，每筆唯一
Name VARCHAR(50) NOT NULL    -- VARCHAR(50) = 可變長度字串，最多 50 字
                             -- NOT NULL = 這欄一定要有值，不能留空
Email VARCHAR(100) UNIQUE    -- UNIQUE = 不能重複（兩人不能同 email）
Score DECIMAL(5,2)           -- DECIMAL(5,2) = 最多 5 位數，其中 2 位小數
                             -- 例如 999.99（可以），10000.0（不行，超過 5 位）
IsActive BOOLEAN DEFAULT true -- DEFAULT = 沒填的話自動用這個值
CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP -- 自動填入當前時間
```

---

## 常用資料型態

### 數字類型

```sql
-- 整數
SMALLINT          -- -32,768 ~ 32,767（2 bytes）
INT / INTEGER     -- -21 億 ~ 21 億（4 bytes）← 最常用
BIGINT            -- 超大整數（8 bytes）
SERIAL            -- 自動遞增 INT（PostgreSQL）
BIGSERIAL         -- 自動遞增 BIGINT

-- 小數
DECIMAL(10,2)     -- 精確小數（金額用這個！）
NUMERIC(10,2)     -- 同 DECIMAL
REAL              -- 浮點數（有誤差）
DOUBLE PRECISION  -- 雙精度浮點數
```

### 字串類型

```sql
CHAR(10)          -- 固定長度 10 字元（不夠補空白）
VARCHAR(100)      -- 可變長度，最多 100 字元 ← 最常用
TEXT              -- 無長度限制的字串（適合存文章）
```

### 日期時間

```sql
DATE              -- 日期（2024-01-15）
TIME              -- 時間（14:30:00）
TIMESTAMP         -- 日期 + 時間（2024-01-15 14:30:00）
INTERVAL          -- 時間間隔（2 hours, 3 days）
```

### 其他

```sql
BOOLEAN           -- true / false
UUID              -- 全球唯一識別碼
JSON / JSONB      -- JSON 格式資料（PostgreSQL）
BYTEA             -- 二進位資料（檔案、圖片）
```

---

## 約束（Constraints）

```sql
CREATE TABLE Orders (
    Id SERIAL PRIMARY KEY,             -- 主鍵約束
    OrderNo VARCHAR(20) NOT NULL UNIQUE, -- 非空 + 唯一
    CustomerId INT NOT NULL,           -- 外鍵欄位
    Amount DECIMAL(10,2) CHECK (Amount > 0),  -- 檢查約束
    Status VARCHAR(20) DEFAULT 'pending',     -- 預設值
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    -- 外鍵約束（獨立宣告）
    CONSTRAINT fk_customer
        FOREIGN KEY (CustomerId)
        REFERENCES Customers(Id)
        ON DELETE CASCADE              -- 客戶被刪時，訂單也刪
);
```

### 約束一覽

| 約束 | 用途 | 範例 |
|------|------|------|
| `PRIMARY KEY` | 主鍵，唯一且非空 | `Id SERIAL PRIMARY KEY` |
| `NOT NULL` | 不可為空 | `Name VARCHAR(50) NOT NULL` |
| `UNIQUE` | 不可重複 | `Email VARCHAR(100) UNIQUE` |
| `DEFAULT` | 預設值 | `Status DEFAULT 'active'` |
| `CHECK` | 條件檢查 | `CHECK (Age >= 0)` |
| `FOREIGN KEY` | 外鍵，參照別的表 | `REFERENCES Customers(Id)` |

---

## ALTER TABLE — 修改表結構

```sql
-- 新增欄位
ALTER TABLE Students
ADD COLUMN Phone VARCHAR(20);         -- ← 加一欄電話

-- 刪除欄位
ALTER TABLE Students
DROP COLUMN Phone;                    -- ← 移除電話欄位

-- 修改欄位型態
ALTER TABLE Students
ALTER COLUMN Name TYPE VARCHAR(100);  -- ← 把名字長度改成 100

-- 新增約束
ALTER TABLE Students
ADD CONSTRAINT uq_email UNIQUE (Email);

-- 重新命名欄位
ALTER TABLE Students
RENAME COLUMN Name TO FullName;
```

---

## DROP TABLE — 刪除表

```sql
DROP TABLE Students;                  -- ⚠️ 整張表消失！

DROP TABLE IF EXISTS Students;        -- 如果存在才刪（避免錯誤）
```

> ⚠️ DROP TABLE 是不可逆的！請搭配備份使用。
" },

        // ── 1406: 聚合函數 ──
        new() { Id=1406, Category="sql", Order=7, Level="beginner", Icon="📈", Title="聚合函數 COUNT、SUM、AVG", Slug="sql-aggregate-functions", IsPublished=true, Content=@"
# 聚合函數 COUNT、SUM、AVG

## 什麼是聚合函數？

> **比喻：聚合函數就像統計老師** 👨‍🏫
>
> 班上 30 個學生的成績單攤開在桌上，
> 統計老師幫你算「有幾個人」、「平均幾分」、「最高幾分」——
> 這就是聚合函數在做的事。

聚合函數會把**多筆資料算成一個結果**。

---

## COUNT — 計算筆數

```sql
-- 計算學生總人數
SELECT COUNT(*) AS 學生人數     -- ← COUNT(*) 計算所有列
FROM Students;
-- 結果：30
```

```sql
-- 計算有填 Email 的學生數
SELECT COUNT(Email) AS 有Email人數  -- ← COUNT(欄位) 不計 NULL
FROM Students;
-- 如果有 5 人沒填 Email → 結果：25

-- 計算有幾種不同的年齡
SELECT COUNT(DISTINCT Age) AS 年齡種類數
FROM Students;
-- 如果有 20, 21, 22 三種 → 結果：3
```

逐行解析：
```
COUNT(*)           -- 計算所有列數（包含 NULL）
COUNT(Email)       -- 計算 Email 不是 NULL 的列數
COUNT(DISTINCT Age) -- 先去重複，再計算有幾種
```

---

## SUM — 加總

```sql
-- 全班分數總和
SELECT SUM(Score) AS 分數總和
FROM Students;

-- 及格學生的分數總和
SELECT SUM(Score) AS 及格分數總和
FROM Students
WHERE Score >= 60;
```

> ⚠️ SUM 只能用在**數字欄位**。用在字串上會出錯。

---

## AVG — 平均

```sql
-- 全班平均分數
SELECT AVG(Score) AS 平均分         -- ← 自動計算：SUM / COUNT
FROM Students;

-- 四捨五入到小數第 2 位
SELECT ROUND(AVG(Score), 2) AS 平均分
FROM Students;
```

---

## MAX / MIN — 最大最小值

```sql
-- 最高分和最低分
SELECT MAX(Score) AS 最高分,        -- ← 全班最高分
       MIN(Score) AS 最低分         -- ← 全班最低分
FROM Students;

-- 也可以用在日期
SELECT MAX(CreatedAt) AS 最新註冊時間,
       MIN(CreatedAt) AS 最早註冊時間
FROM Students;

-- 也可以用在字串（按字母順序）
SELECT MAX(Name) AS 最後名字,       -- 字母排序最後的
       MIN(Name) AS 最前名字        -- 字母排序最前的
FROM Students;
```

---

## 組合使用

```sql
-- 一次看所有統計資料
SELECT
    COUNT(*) AS 總人數,
    SUM(Score) AS 分數總和,
    ROUND(AVG(Score), 2) AS 平均分,
    MAX(Score) AS 最高分,
    MIN(Score) AS 最低分,
    MAX(Score) - MIN(Score) AS 分數差距   -- ← 可以做運算
FROM Students;
```

結果：
```
 總人數 | 分數總和 | 平均分 | 最高分 | 最低分 | 分數差距
-------+---------+-------+-------+-------+---------
    30 |    2250 | 75.00 |    98 |    32 |      66
```

---

## 搭配 WHERE 使用

```sql
-- 統計各個條件下的數據
SELECT COUNT(*) AS 及格人數
FROM Students
WHERE Score >= 60;

SELECT AVG(Score) AS 女生平均分
FROM Students
WHERE Gender = '女';

SELECT MAX(Age) AS 最大年齡
FROM Students
WHERE Department = '資工系';
```

---

## 小結

| 函數 | 用途 | NULL 處理 |
|------|------|----------|
| `COUNT(*)` | 計算總列數 | 包含 NULL |
| `COUNT(col)` | 計算非空列數 | 忽略 NULL |
| `SUM(col)` | 加總 | 忽略 NULL |
| `AVG(col)` | 平均 | 忽略 NULL |
| `MAX(col)` | 最大值 | 忽略 NULL |
| `MIN(col)` | 最小值 | 忽略 NULL |

> 💡 下一章學 GROUP BY，讓聚合函數發揮真正威力！
" },

        // ── 1407: GROUP BY 與 HAVING ──
        new() { Id=1407, Category="sql", Order=8, Level="beginner", Icon="📦", Title="GROUP BY 與 HAVING 分組統計", Slug="sql-group-by", IsPublished=true, Content=@"
# GROUP BY 與 HAVING 分組統計

## GROUP BY — 分組統計

> **比喻：GROUP BY 就像把學生按班級分組** 📦
>
> 全校 300 個學生，你想知道「每班平均分數」——
> 先把學生按班級分組，再對每組算平均。
> 這就是 GROUP BY 的概念。

---

## 基本用法

```sql
-- 每個科系有幾個學生？
SELECT Department,            -- ← 分組的欄位
       COUNT(*) AS 學生人數    -- ← 每組的統計
FROM Students
GROUP BY Department;          -- ← 按 Department 分組
```

逐行解析：
```
SELECT Department        -- 顯示科系名稱
       COUNT(*) AS 學生人數  -- 每組有幾筆 = 每系幾人
FROM Students            -- 從學生表
GROUP BY Department      -- 把相同 Department 的資料歸為一組
                         -- 結果是：每組一列
```

結果：
```
 Department | 學生人數
-----------+---------
 資工系     |      15
 電機系     |      12
 企管系     |      20
```

---

## 搭配聚合函數

```sql
-- 每個科系的平均分數、最高分、最低分
SELECT
    Department AS 科系,
    COUNT(*) AS 人數,
    ROUND(AVG(Score), 1) AS 平均分,
    MAX(Score) AS 最高分,
    MIN(Score) AS 最低分
FROM Students
GROUP BY Department
ORDER BY 平均分 DESC;        -- ← 按平均分排序（高到低）
```

---

## ⚠️ GROUP BY 的重要規則

```sql
-- ❌ 錯誤：SELECT 裡的非聚合欄位，必須出現在 GROUP BY 裡
SELECT Department, Name, COUNT(*)
FROM Students
GROUP BY Department;
-- 錯誤！Name 不在 GROUP BY 裡，資料庫不知道要顯示哪個 Name

-- ✅ 正確：
SELECT Department, COUNT(*)
FROM Students
GROUP BY Department;

-- ✅ 或者把 Name 也加入 GROUP BY
SELECT Department, Name, COUNT(*)
FROM Students
GROUP BY Department, Name;   -- 變成「每系 + 每人」一組
```

---

## 多欄分組

```sql
-- 按科系和性別分組
SELECT
    Department AS 科系,
    Gender AS 性別,
    COUNT(*) AS 人數,
    AVG(Score) AS 平均分
FROM Students
GROUP BY Department, Gender  -- ← 兩個欄位一起分組
ORDER BY Department, Gender;
```

結果：
```
 科系   | 性別 | 人數 | 平均分
-------+------+------+-------
 資工系 | 男   |   10 | 78.5
 資工系 | 女   |    5 | 82.3
 電機系 | 男   |    8 | 75.0
 電機系 | 女   |    4 | 80.1
```

---

## HAVING — 過濾分組結果

> WHERE 是在分組**前**過濾（過濾個別資料列）
> HAVING 是在分組**後**過濾（過濾分組結果）

```sql
-- 只顯示人數超過 10 的科系
SELECT Department, COUNT(*) AS 人數
FROM Students
GROUP BY Department
HAVING COUNT(*) > 10;        -- ← 只要人數 > 10 的組
```

逐行解析：
```
GROUP BY Department      -- 先分組
HAVING COUNT(*) > 10     -- 分完組之後，過濾掉人數 <= 10 的組
                         -- HAVING 裡只能用聚合函數或 GROUP BY 的欄位
```

---

## WHERE vs HAVING

```sql
-- 找出「分數 >= 60 的學生中」每系平均超過 80 的科系
SELECT Department, AVG(Score) AS 平均分
FROM Students
WHERE Score >= 60            -- ① 先過濾：只看及格的學生
GROUP BY Department          -- ② 再分組：按科系
HAVING AVG(Score) > 80       -- ③ 最後過濾組：只要平均 > 80
ORDER BY 平均分 DESC;        -- ④ 排序結果
```

### SQL 執行順序

```
1. FROM      — 從哪個表
2. WHERE     — 過濾個別列
3. GROUP BY  — 分組
4. HAVING    — 過濾分組
5. SELECT    — 選欄位
6. ORDER BY  — 排序
7. LIMIT     — 取前 N 筆
```

> 💡 這個順序很重要！它解釋了為什麼 WHERE 不能用聚合函數——因為 WHERE 在 GROUP BY 之前執行，那時候還沒分組呢。

---

## 實用範例

```sql
-- 找出購買次數最多的前 5 個客戶
SELECT CustomerId, COUNT(*) AS 購買次數, SUM(Amount) AS 消費總額
FROM Orders
GROUP BY CustomerId
ORDER BY 購買次數 DESC
LIMIT 5;

-- 找出每月營業額超過 10 萬的月份
SELECT
    DATE_TRUNC('month', OrderDate) AS 月份,
    SUM(Amount) AS 月營業額
FROM Orders
GROUP BY DATE_TRUNC('month', OrderDate)
HAVING SUM(Amount) > 100000
ORDER BY 月份;
```
" },

        // ── 1408: INNER JOIN ──
        new() { Id=1408, Category="sql", Order=9, Level="intermediate", Icon="🔗", Title="INNER JOIN 內連接", Slug="sql-inner-join", IsPublished=true, Content=@"
# INNER JOIN 內連接

## 為什麼需要 JOIN？

> **比喻：JOIN 就像合併兩份名冊** 📋📋
>
> 你手上有一本「學生名冊」和一本「成績單」，
> 學號是它們的共同欄位——你用學號把兩本冊子的資料對在一起看，
> 這就是 JOIN。

### 兩張表的情境

```
Students 表                    Courses 表
┌────┬──────┬─────┐           ┌────┬──────────┐
│ Id │ Name │ Age │           │ Id │ Name     │
├────┼──────┼─────┤           ├────┼──────────┤
│  1 │ 小明 │  20 │           │ 10 │ 數學     │
│  2 │ 小華 │  22 │           │ 11 │ 英文     │
│  3 │ 小美 │  21 │           │ 12 │ 物理     │
└────┴──────┴─────┘           └────┴──────────┘

Enrollments 表（選課記錄）
┌────┬───────────┬──────────┬───────┐
│ Id │ StudentId │ CourseId │ Score │
├────┼───────────┼──────────┼───────┤
│  1 │     1     │    10    │   85  │
│  2 │     1     │    11    │   92  │
│  3 │     2     │    10    │   78  │
└────┴───────────┴──────────┴───────┘
```

注意：小美（Id=3）沒有選課記錄。

---

## INNER JOIN 語法

```sql
SELECT
    s.Name AS 學生,           -- ← s 是 Students 的別名
    c.Name AS 課程,           -- ← c 是 Courses 的別名
    e.Score AS 分數            -- ← e 是 Enrollments 的別名
FROM Students s               -- ← 主表，取別名 s
INNER JOIN Enrollments e      -- ← 連接選課表
    ON s.Id = e.StudentId     -- ← 連接條件：學生 Id 對應
INNER JOIN Courses c          -- ← 再連接課程表
    ON e.CourseId = c.Id;     -- ← 連接條件：課程 Id 對應
```

逐行解析：
```
FROM Students s           -- 起點：Students 表，別名 s
INNER JOIN Enrollments e  -- 連接 Enrollments 表，別名 e
    ON s.Id = e.StudentId -- 連接條件：Students.Id = Enrollments.StudentId
                          -- 只有能配對的資料才會出現
INNER JOIN Courses c      -- 再連接 Courses 表
    ON e.CourseId = c.Id  -- 連接條件：Enrollments.CourseId = Courses.Id
```

結果：
```
 學生 | 課程 | 分數
------+------+------
 小明 | 數學 |   85
 小明 | 英文 |   92
 小華 | 數學 |   78
```

> 注意：小美不在結果中，因為她沒有選課記錄（INNER JOIN 只保留能配對的）。

---

## INNER JOIN 的視覺化

```
Students        Enrollments
┌────┐          ┌────┐
│ 小明│─────────▶│ 記錄│  ✅ 配對成功 → 出現在結果
│ 小華│─────────▶│ 記錄│  ✅ 配對成功 → 出現在結果
│ 小美│     ✗    │    │  ❌ 沒配對 → 不出現
└────┘          └────┘

INNER JOIN = 交集（兩邊都有才算）
```

---

## 表別名的重要性

```sql
-- ❌ 不用別名，很長很痛苦
SELECT Students.Name, Courses.Name, Enrollments.Score
FROM Students
INNER JOIN Enrollments ON Students.Id = Enrollments.StudentId
INNER JOIN Courses ON Enrollments.CourseId = Courses.Id;

-- ✅ 用別名，簡潔清楚
SELECT s.Name, c.Name, e.Score
FROM Students s
INNER JOIN Enrollments e ON s.Id = e.StudentId
INNER JOIN Courses c ON e.CourseId = c.Id;
```

---

## 搭配 WHERE 使用

```sql
-- 查詢小明的所有課程和分數
SELECT s.Name, c.Name AS 課程, e.Score
FROM Students s
INNER JOIN Enrollments e ON s.Id = e.StudentId
INNER JOIN Courses c ON e.CourseId = c.Id
WHERE s.Name = '小明';        -- ← JOIN 後再用 WHERE 過濾

-- 查詢數學科分數 >= 80 的學生
SELECT s.Name, e.Score
FROM Students s
INNER JOIN Enrollments e ON s.Id = e.StudentId
INNER JOIN Courses c ON e.CourseId = c.Id
WHERE c.Name = '數學'
  AND e.Score >= 80;
```

---

## 搭配 GROUP BY 使用

```sql
-- 每個學生的選課數和平均分
SELECT
    s.Name AS 學生,
    COUNT(*) AS 選課數,
    ROUND(AVG(e.Score), 1) AS 平均分
FROM Students s
INNER JOIN Enrollments e ON s.Id = e.StudentId
GROUP BY s.Name
ORDER BY 平均分 DESC;
```

---

## 自連接（Self Join）

```sql
-- 員工表：每個員工有一個 ManagerId 指向同一張表
SELECT
    emp.Name AS 員工,
    mgr.Name AS 主管
FROM Employees emp
INNER JOIN Employees mgr     -- ← 同一張表連接自己！
    ON emp.ManagerId = mgr.Id;
```

> 💡 自連接常用於階層結構（員工→主管、留言→回覆等）。
" },

        // ── 1409: LEFT/RIGHT/FULL JOIN ──
        new() { Id=1409, Category="sql", Order=10, Level="intermediate", Icon="↔️", Title="LEFT / RIGHT / FULL JOIN", Slug="sql-outer-joins", IsPublished=true, Content=@"
# LEFT / RIGHT / FULL JOIN

## 為什麼需要 OUTER JOIN？

> INNER JOIN 只回傳兩邊都有配對的資料。
> 但如果你想看到**所有學生（包含沒選課的）**，就需要 OUTER JOIN。

---

## LEFT JOIN

LEFT JOIN = 左邊的表全部保留，右邊沒配對的填 NULL。

```sql
SELECT
    s.Name AS 學生,
    c.Name AS 課程,
    e.Score AS 分數
FROM Students s                    -- ← 左表：全部保留
LEFT JOIN Enrollments e            -- ← 右表：有配對的才連
    ON s.Id = e.StudentId
LEFT JOIN Courses c
    ON e.CourseId = c.Id;
```

結果：
```
 學生 | 課程 | 分數
------+------+------
 小明 | 數學 |   85
 小明 | 英文 |   92
 小華 | 數學 |   78
 小美 | NULL | NULL    ← 小美沒有選課，但還是出現了！
```

```
Students        Enrollments
┌────┐          ┌────┐
│ 小明│─────────▶│ 記錄│  ✅ 有配對
│ 小華│─────────▶│ 記錄│  ✅ 有配對
│ 小美│──── ✗    │    │  ⚠️ 沒配對，但左表保留，右邊填 NULL
└────┘          └────┘
```

---

## 用 LEFT JOIN 找出「沒有」的資料

```sql
-- 找出沒有選任何課的學生
SELECT s.Name
FROM Students s
LEFT JOIN Enrollments e ON s.Id = e.StudentId
WHERE e.Id IS NULL;          -- ← 右表是 NULL = 沒有配對 = 沒選課
```

逐行解析：
```
LEFT JOIN Enrollments e      -- 左連接，保留所有學生
    ON s.Id = e.StudentId    -- 嘗試配對
WHERE e.Id IS NULL           -- 配對失敗的（e.Id 是 NULL）
                             -- = 在 Enrollments 裡找不到這個學生
                             -- = 這個學生沒有選課
```

> 💡 這是 LEFT JOIN 最常見的用法之一：找出「缺少關聯」的資料。

---

## RIGHT JOIN

RIGHT JOIN = 右邊的表全部保留，左邊沒配對的填 NULL。

```sql
-- 查詢所有課程（包含沒人選的）
SELECT s.Name AS 學生, c.Name AS 課程
FROM Students s
RIGHT JOIN Enrollments e ON s.Id = e.StudentId
RIGHT JOIN Courses c ON e.CourseId = c.Id;
```

```
 學生 | 課程
------+------
 小明 | 數學
 小華 | 數學
 小明 | 英文
 NULL | 物理    ← 物理沒人選，但還是出現了
```

> 💡 實務上 RIGHT JOIN 很少用。你可以把表的順序反過來，用 LEFT JOIN 代替：
> ```sql
> -- 這兩個完全等價：
> A RIGHT JOIN B ON ...  =  B LEFT JOIN A ON ...
> ```

---

## FULL OUTER JOIN

FULL JOIN = 兩邊都全部保留，沒配對的填 NULL。

```sql
SELECT s.Name AS 學生, c.Name AS 課程
FROM Students s
FULL OUTER JOIN Enrollments e ON s.Id = e.StudentId
FULL OUTER JOIN Courses c ON e.CourseId = c.Id;
```

```
 學生 | 課程
------+------
 小明 | 數學
 小明 | 英文
 小華 | 數學
 小美 | NULL    ← 小美沒選課
 NULL | 物理    ← 物理沒人選
```

---

## JOIN 比較總覽

```
INNER JOIN          LEFT JOIN           RIGHT JOIN          FULL JOIN
┌──┬──┐            ┌──┬──┐             ┌──┬──┐            ┌──┬──┐
│██│██│            │██│██│             │██│██│            │██│██│
│  │██│ ← 交集    │██│██│ ← 左全部   │  │██│ ← 右全部  │██│██│ ← 全部
│██│  │            │██│  │             │██│██│            │██│  │
└──┴──┘            └──┴──┘             └──┴──┘            └──┴──┘
```

| JOIN 類型 | 左表沒配對 | 右表沒配對 |
|-----------|-----------|-----------|
| INNER | ❌ 不顯示 | ❌ 不顯示 |
| LEFT | ✅ 顯示（右填 NULL） | ❌ 不顯示 |
| RIGHT | ❌ 不顯示 | ✅ 顯示（左填 NULL） |
| FULL | ✅ 顯示 | ✅ 顯示 |

---

## 實用範例

```sql
-- 每個客戶的訂單數（包含沒下過單的客戶）
SELECT
    c.Name AS 客戶,
    COUNT(o.Id) AS 訂單數      -- ← 用 COUNT(o.Id) 而不是 COUNT(*)
FROM Customers c                    -- COUNT(o.Id) 遇到 NULL 不計算
LEFT JOIN Orders o ON c.Id = o.CustomerId
GROUP BY c.Name
ORDER BY 訂單數 DESC;

-- 找出沒有訂單的客戶
SELECT c.Name
FROM Customers c
LEFT JOIN Orders o ON c.Id = o.CustomerId
WHERE o.Id IS NULL;
```
" },

        // ── 1410: 子查詢 ──
        new() { Id=1410, Category="sql", Order=11, Level="intermediate", Icon="🪆", Title="子查詢 Subquery", Slug="sql-subquery", IsPublished=true, Content=@"
# 子查詢 Subquery

## 什麼是子查詢？

> **比喻：子查詢就像俄羅斯娃娃** 🪆
>
> 打開一個查詢，裡面還有一個查詢——
> 內層的查詢先執行，結果交給外層使用。

---

## WHERE 裡的子查詢

### 回傳單一值

```sql
-- 找出分數高於平均的學生
SELECT Name, Score
FROM Students
WHERE Score > (                    -- ← 子查詢用括號包起來
    SELECT AVG(Score)              -- ← 這個先執行，算出平均分
    FROM Students                  -- ← 假設平均是 75
);                                 -- ← 外層變成 WHERE Score > 75
```

逐行解析：
```
WHERE Score > (                 -- 外層條件：分數要大於某個值
    SELECT AVG(Score)           -- 子查詢：先算出全班平均分（假設 75）
    FROM Students               -- 子查詢執行完畢，回傳 75
)                               -- 整句變成 WHERE Score > 75
```

### 回傳一組值（IN）

```sql
-- 找出有選「數學」的學生
SELECT Name
FROM Students
WHERE Id IN (                      -- ← IN 搭配回傳多值的子查詢
    SELECT StudentId
    FROM Enrollments
    WHERE CourseId = (
        SELECT Id FROM Courses WHERE Name = '數學'
    )
);
```

逐行解析：
```
-- 最內層先執行：找到數學的 CourseId（假設是 10）
SELECT Id FROM Courses WHERE Name = '數學'  -- → 10

-- 中間層執行：找出選了 CourseId=10 的學生 Id
SELECT StudentId FROM Enrollments WHERE CourseId = 10  -- → [1, 2]

-- 最外層執行：找出 Id 在 [1, 2] 裡的學生
SELECT Name FROM Students WHERE Id IN (1, 2)  -- → 小明, 小華
```

---

## EXISTS — 檢查是否存在

```sql
-- 找出「有選過課」的學生
SELECT s.Name
FROM Students s
WHERE EXISTS (                     -- ← 只要子查詢有結果就通過
    SELECT 1                       -- ← SELECT 什麼不重要
    FROM Enrollments e
    WHERE e.StudentId = s.Id       -- ← 注意：用到了外層的 s.Id
);
```

逐行解析：
```
WHERE EXISTS (...)              -- 子查詢有回傳任何列 → true
    SELECT 1                    -- 只需要知道「有沒有」，不需要實際資料
    FROM Enrollments e
    WHERE e.StudentId = s.Id    -- 關聯子查詢：每個學生都會執行一次
                                -- 如果這個學生有選課記錄 → EXISTS = true
```

```sql
-- 找出「沒選過課」的學生
SELECT s.Name
FROM Students s
WHERE NOT EXISTS (
    SELECT 1
    FROM Enrollments e
    WHERE e.StudentId = s.Id
);
```

---

## SELECT 裡的子查詢

```sql
-- 每個學生的分數 vs 全班平均
SELECT
    Name,
    Score,
    (SELECT AVG(Score) FROM Students) AS 全班平均,
    Score - (SELECT AVG(Score) FROM Students) AS 差距
FROM Students;
```

結果：
```
 Name | Score | 全班平均 | 差距
------+-------+---------+------
 小明 |    85 |   75.0  | 10.0
 小華 |    78 |   75.0  |  3.0
 小美 |    62 |   75.0  | -13.0
```

---

## FROM 裡的子查詢（衍生表）

```sql
-- 先算出每個科系的平均分，再找出平均 > 80 的
SELECT *
FROM (
    SELECT Department, AVG(Score) AS avg_score
    FROM Students
    GROUP BY Department
) AS dept_stats                    -- ← 子查詢結果當作一張臨時表
WHERE avg_score > 80;              -- ← 外層再過濾
```

> 💡 FROM 裡的子查詢必須給**別名**（AS dept_stats）。

---

## 子查詢 vs JOIN

```sql
-- 子查詢寫法
SELECT Name FROM Students
WHERE Id IN (SELECT StudentId FROM Enrollments WHERE CourseId = 10);

-- JOIN 寫法（通常效能更好）
SELECT DISTINCT s.Name
FROM Students s
INNER JOIN Enrollments e ON s.Id = e.StudentId
WHERE e.CourseId = 10;
```

| 比較 | 子查詢 | JOIN |
|------|--------|------|
| 可讀性 | 直覺，由內到外 | 需要理解連接邏輯 |
| 效能 | 簡單場景 OK | 大量資料通常更快 |
| 適用場景 | 比較值、EXISTS | 需要多表欄位 |

> 💡 能用 JOIN 就用 JOIN，效能通常更好。EXISTS 在檢查「有沒有」時效能很棒。
" },

        // ── 1411: CTE 通用表達式 ──
        new() { Id=1411, Category="sql", Order=12, Level="intermediate", Icon="🧱", Title="CTE 通用表達式", Slug="sql-cte", IsPublished=true, Content=@"
# CTE 通用表達式（Common Table Expression）

## 什麼是 CTE？

> **比喻：CTE 就像先做好便當盒再組裝** 🍱
>
> 與其把所有食材（子查詢）塞在一個鍋子裡煮，
> 不如先分別準備好每道菜（CTE），最後再組裝。

CTE 讓你用 `WITH` 先定義臨時結果集，然後在主查詢中引用。

---

## 基本語法

```sql
WITH honor_students AS (           -- ← 定義 CTE，取名 honor_students
    SELECT Name, Score
    FROM Students
    WHERE Score >= 90
)
SELECT *                           -- ← 主查詢，引用上面定義的 CTE
FROM honor_students
ORDER BY Score DESC;
```

逐行解析：
```
WITH honor_students AS (        -- WITH 開頭，定義一個叫 honor_students 的臨時表
    SELECT Name, Score          -- CTE 的內容：90 分以上的學生
    FROM Students
    WHERE Score >= 90
)                               -- CTE 定義結束
SELECT * FROM honor_students    -- 主查詢可以像用一般表一樣使用 CTE
ORDER BY Score DESC;
```

---

## 多個 CTE

```sql
WITH
-- CTE 1：每個科系的統計
dept_stats AS (
    SELECT
        Department,
        COUNT(*) AS cnt,
        AVG(Score) AS avg_score
    FROM Students
    GROUP BY Department
),
-- CTE 2：全校平均
school_avg AS (
    SELECT AVG(Score) AS overall_avg
    FROM Students
)
-- 主查詢：比較各系與全校平均
SELECT
    d.Department,
    d.cnt AS 人數,
    ROUND(d.avg_score, 1) AS 系平均,
    ROUND(s.overall_avg, 1) AS 校平均,
    CASE
        WHEN d.avg_score > s.overall_avg THEN '高於平均'
        ELSE '低於平均'
    END AS 評等
FROM dept_stats d
CROSS JOIN school_avg s          -- ← CROSS JOIN 讓每列都能看到全校平均
ORDER BY d.avg_score DESC;
```

---

## CTE vs 子查詢

```sql
-- 子查詢版本（巢狀，難讀）
SELECT * FROM (
    SELECT Department, AVG(Score) AS avg_score
    FROM Students
    GROUP BY Department
) AS dept_stats
WHERE avg_score > (
    SELECT AVG(Score) FROM Students
);

-- CTE 版本（清楚，好讀）
WITH dept_stats AS (
    SELECT Department, AVG(Score) AS avg_score
    FROM Students
    GROUP BY Department
),
school_avg AS (
    SELECT AVG(Score) AS val FROM Students
)
SELECT d.*
FROM dept_stats d, school_avg s
WHERE d.avg_score > s.val;
```

---

## 遞迴 CTE

遞迴 CTE 可以處理**階層結構**（組織圖、分類樹等）。

```sql
-- 員工組織圖
WITH RECURSIVE org_chart AS (
    -- 起始條件：找到最高主管（沒有 ManagerId）
    SELECT Id, Name, ManagerId, 1 AS Level
    FROM Employees
    WHERE ManagerId IS NULL

    UNION ALL

    -- 遞迴條件：找每個人的下屬
    SELECT e.Id, e.Name, e.ManagerId, oc.Level + 1
    FROM Employees e
    INNER JOIN org_chart oc ON e.ManagerId = oc.Id
)
SELECT
    REPEAT('  ', Level - 1) || Name AS 組織圖,  -- 縮排顯示
    Level AS 層級
FROM org_chart
ORDER BY Level, Name;
```

結果：
```
 組織圖          | 層級
-----------------+------
 張總經理         |    1
   李副總         |    2
   王副總         |    2
     陳經理       |    3
     林經理       |    3
       小明       |    4
```

---

## CTE 的優點

| 特點 | 說明 |
|------|------|
| 可讀性高 | 每個 CTE 有名字，邏輯分段清楚 |
| 可重用 | 同一個 CTE 可以被主查詢引用多次 |
| 遞迴能力 | 可以處理樹狀、階層結構 |
| 偵錯容易 | 可以單獨執行每個 CTE 檢查結果 |
" },

        // ── 1412: UNION 集合運算 ──
        new() { Id=1412, Category="sql", Order=13, Level="intermediate", Icon="🔀", Title="UNION 與集合運算", Slug="sql-union", IsPublished=true, Content=@"
# UNION 與集合運算

## UNION — 合併查詢結果

> **比喻：UNION 就像把兩份名單合併** 📋+📋
>
> A 班的學生名單 + B 班的學生名單 = 全校名單。
> UNION 就是把兩個 SELECT 的結果合併成一個。

---

## UNION（去重複）

```sql
-- 合併兩班的學生（去除重複）
SELECT Name, Age FROM ClassA
UNION                              -- ← 合併並去重複
SELECT Name, Age FROM ClassB;
```

逐行解析：
```
SELECT Name, Age FROM ClassA    -- 第一個查詢的結果
UNION                           -- 合併兩個結果，自動去除重複的列
SELECT Name, Age FROM ClassB    -- 第二個查詢的結果
                                -- 欄位數量和型態必須一致！
```

---

## UNION ALL（保留重複）

```sql
-- 合併兩班的學生（保留重複）
SELECT Name, Age FROM ClassA
UNION ALL                          -- ← 合併但保留重複
SELECT Name, Age FROM ClassB;
```

> 💡 UNION ALL 比 UNION 快，因為不需要去重複。
> 如果你確定沒有重複，或不在乎重複，用 UNION ALL。

---

## UNION 的規則

```sql
-- ✅ 正確：欄位數量和型態一致
SELECT Name, Age FROM Students
UNION
SELECT Name, Age FROM Teachers;

-- ❌ 錯誤：欄位數量不同
SELECT Name, Age FROM Students
UNION
SELECT Name FROM Teachers;         -- 少一個欄位！

-- ❌ 錯誤：型態不相容
SELECT Name, Age FROM Students
UNION
SELECT Name, Email FROM Teachers;  -- Age 是數字，Email 是字串
```

---

## INTERSECT — 交集

```sql
-- 同時選了數學和英文的學生
SELECT StudentId FROM Enrollments WHERE CourseId = 10
INTERSECT                          -- ← 只保留兩邊都有的
SELECT StudentId FROM Enrollments WHERE CourseId = 11;
```

---

## EXCEPT — 差集

```sql
-- 選了數學但「沒選」英文的學生
SELECT StudentId FROM Enrollments WHERE CourseId = 10
EXCEPT                             -- ← 從第一組去掉第二組有的
SELECT StudentId FROM Enrollments WHERE CourseId = 11;
```

---

## 實用範例

```sql
-- 合併搜尋結果（標題或內容含關鍵字）
SELECT Id, Title, '標題符合' AS 來源
FROM Articles
WHERE Title LIKE '%SQL%'
UNION
SELECT Id, Title, '內容符合' AS 來源
FROM Articles
WHERE Content LIKE '%SQL%';

-- 統計各類別的訂單數和退貨數
SELECT '訂單' AS 類型, Category, COUNT(*) AS 數量
FROM Orders
GROUP BY Category
UNION ALL
SELECT '退貨' AS 類型, Category, COUNT(*) AS 數量
FROM Returns
GROUP BY Category
ORDER BY Category, 類型;
```

---

## 集合運算總覽

| 運算 | 效果 | 重複 |
|------|------|------|
| `UNION` | 合併（去重） | 自動去除 |
| `UNION ALL` | 合併（保留） | 保留重複 |
| `INTERSECT` | 交集 | 兩邊都有 |
| `EXCEPT` | 差集 | 第一組有、第二組沒有 |
" },

        // ── 1413: INDEX 索引 ──
        new() { Id=1413, Category="sql", Order=14, Level="intermediate", Icon="📑", Title="INDEX 索引", Slug="sql-index", IsPublished=true, Content=@"
# INDEX 索引

## 什麼是索引？

> **比喻：索引就像書本的目錄** 📑
>
> 一本 500 頁的書，你要找「第三章」——
> 沒有目錄 → 一頁一頁翻（Full Table Scan）
> 有目錄 → 直接翻到第 87 頁（Index Scan）

索引是資料庫用來**加速查詢**的資料結構。

---

## 建立索引

```sql
-- 在 Students 的 Email 欄位建立索引
CREATE INDEX idx_students_email    -- ← 索引名稱（慣例：idx_表_欄位）
ON Students (Email);               -- ← 在哪個表的哪個欄位

-- 在 Orders 表建立複合索引
CREATE INDEX idx_orders_customer_date
ON Orders (CustomerId, OrderDate); -- ← 多欄位組合索引
```

逐行解析：
```
CREATE INDEX idx_students_email    -- 建立索引，取名 idx_students_email
ON Students (Email)                -- 在 Students 表的 Email 欄位上
                                   -- 之後 WHERE Email = '...' 會變快
```

---

## 索引的效果

```sql
-- 沒有索引：掃描全表（100 萬筆逐筆比對）
SELECT * FROM Students WHERE Email = 'test@gmail.com';
-- 可能需要 500ms

-- 建立索引後：直接定位
CREATE INDEX idx_students_email ON Students (Email);
SELECT * FROM Students WHERE Email = 'test@gmail.com';
-- 只需要 1ms
```

---

## 索引類型

### B-Tree 索引（預設）

```sql
-- 適合：等值查詢、範圍查詢、排序
CREATE INDEX idx_age ON Students (Age);

-- 這些查詢都能用到 B-Tree 索引：
WHERE Age = 20          -- 等值
WHERE Age > 18          -- 範圍
WHERE Age BETWEEN 20 AND 25  -- 範圍
ORDER BY Age            -- 排序
```

### 唯一索引

```sql
-- 確保值不重複（自動作為約束）
CREATE UNIQUE INDEX idx_email_unique
ON Students (Email);

-- UNIQUE 約束其實就是建立唯一索引
ALTER TABLE Students ADD CONSTRAINT uq_email UNIQUE (Email);
```

### 部分索引（Partial Index）

```sql
-- 只對「有效」的資料建索引
CREATE INDEX idx_active_students
ON Students (Name)
WHERE IsActive = true;     -- ← 只索引 IsActive=true 的列

-- 適合：大部分查詢都只找 IsActive=true 的資料
-- 索引更小、更快
```

### 複合索引

```sql
CREATE INDEX idx_dept_score
ON Students (Department, Score);

-- ✅ 這些查詢能用到：
WHERE Department = '資工系'                    -- 左前綴匹配
WHERE Department = '資工系' AND Score > 80     -- 兩欄都用到
ORDER BY Department, Score                     -- 排序

-- ❌ 這個用不到：
WHERE Score > 80                               -- 沒有用到最左邊的欄位
```

> 💡 **最左前綴原則**：複合索引 (A, B, C)，只有從 A 開始的查詢才能用到。

---

## 何時該建索引？

### ✅ 應該建索引

```sql
-- 1. WHERE 經常查詢的欄位
WHERE Email = '...'          -- 頻繁查詢 → 建索引

-- 2. JOIN 的連接欄位
ON Orders.CustomerId = Customers.Id  -- 外鍵欄位 → 建索引

-- 3. ORDER BY 的排序欄位
ORDER BY CreatedAt DESC      -- 頻繁排序 → 建索引

-- 4. UNIQUE 約束的欄位
-- 自動有索引
```

### ❌ 不應該建索引

```sql
-- 1. 很少查詢的欄位
-- 2. 經常大量 INSERT/UPDATE 的表（索引會拖慢寫入）
-- 3. 值很少變化的欄位（如 Gender 只有男/女，索引效果差）
-- 4. 資料量很小的表（直接全表掃描就很快了）
```

---

## 查看和管理索引

```sql
-- 查看表的所有索引（PostgreSQL）
SELECT indexname, indexdef
FROM pg_indexes
WHERE tablename = 'students';

-- 刪除索引
DROP INDEX idx_students_email;

-- 查看查詢是否有用到索引
EXPLAIN ANALYZE
SELECT * FROM Students WHERE Email = 'test@gmail.com';
-- 結果會顯示 Index Scan 或 Seq Scan（全表掃描）
```

---

## 小結

| 概念 | 說明 |
|------|------|
| 索引 | 加速查詢的資料結構 |
| B-Tree | 預設索引，適合等值/範圍/排序 |
| 唯一索引 | 確保不重複 |
| 複合索引 | 多欄組合，注意最左前綴 |
| 部分索引 | 只索引部分資料 |
| 代價 | 加速讀取，但拖慢寫入 |
" },

        // ── 1414: VIEW 檢視表 ──
        new() { Id=1414, Category="sql", Order=15, Level="intermediate", Icon="👁️", Title="VIEW 檢視表", Slug="sql-view", IsPublished=true, Content=@"
# VIEW 檢視表

## 什麼是 VIEW？

> **比喻：VIEW 就像一個存好的「書籤查詢」** 🔖
>
> 你常常執行同一段複雜的 SQL 查詢——
> VIEW 讓你把它存起來，取個名字，以後直接用名字查。

VIEW 是一個**虛擬表**，它不存資料，只存查詢語句。

---

## 建立 VIEW

```sql
-- 建立一個「學生成績總覽」的 View
CREATE VIEW student_scores AS     -- ← 定義 View 名稱
SELECT
    s.Name AS 學生,
    s.Department AS 科系,
    c.Name AS 課程,
    e.Score AS 分數
FROM Students s
INNER JOIN Enrollments e ON s.Id = e.StudentId
INNER JOIN Courses c ON e.CourseId = c.Id;
```

### 使用 VIEW

```sql
-- 用起來就像一般的表！
SELECT * FROM student_scores;

-- 可以加 WHERE
SELECT * FROM student_scores
WHERE 科系 = '資工系' AND 分數 >= 80;

-- 可以做聚合
SELECT 科系, AVG(分數) AS 平均分
FROM student_scores
GROUP BY 科系;
```

---

## VIEW 的好處

### 1. 簡化複雜查詢

```sql
-- 沒有 View：每次都要寫一大串 JOIN
SELECT s.Name, c.Name, e.Score
FROM Students s
INNER JOIN Enrollments e ON s.Id = e.StudentId
INNER JOIN Courses c ON e.CourseId = c.Id
WHERE s.Department = '資工系';

-- 有 View：一行搞定
SELECT * FROM student_scores WHERE 科系 = '資工系';
```

### 2. 權限控制

```sql
-- 給業務部門看的 View（隱藏敏感欄位）
CREATE VIEW public_students AS
SELECT Name, Department, Score     -- ← 不包含 Email、Phone
FROM Students;

-- 業務部門只能存取這個 View，看不到完整資料
GRANT SELECT ON public_students TO sales_role;
```

### 3. 資料抽象

```sql
-- 即使底層表結構改了，View 可以保持介面不變
-- 應用程式不需要跟著改
```

---

## 修改和刪除 VIEW

```sql
-- 修改 View（CREATE OR REPLACE）
CREATE OR REPLACE VIEW student_scores AS
SELECT
    s.Name AS 學生,
    s.Department AS 科系,
    c.Name AS 課程,
    e.Score AS 分數,
    CASE WHEN e.Score >= 60 THEN '及格' ELSE '不及格' END AS 狀態
FROM Students s
INNER JOIN Enrollments e ON s.Id = e.StudentId
INNER JOIN Courses c ON e.CourseId = c.Id;

-- 刪除 View
DROP VIEW student_scores;
DROP VIEW IF EXISTS student_scores;  -- 安全寫法
```

---

## Materialized View（實體化檢視）

普通 View 每次查詢都重新執行 SQL。
Materialized View 會**快取結果**，查詢更快。

```sql
-- 建立 Materialized View
CREATE MATERIALIZED VIEW mv_dept_stats AS
SELECT
    Department,
    COUNT(*) AS student_count,
    AVG(Score) AS avg_score
FROM Students
GROUP BY Department;

-- 查詢（直接讀快取，不重新計算）
SELECT * FROM mv_dept_stats;

-- 手動刷新（資料有變時需要刷新）
REFRESH MATERIALIZED VIEW mv_dept_stats;
```

| 比較 | VIEW | MATERIALIZED VIEW |
|------|------|-------------------|
| 存資料 | ❌ 只存查詢 | ✅ 存結果 |
| 速度 | 每次重算 | 快取，很快 |
| 即時性 | 永遠最新 | 需手動刷新 |
| 適用場景 | 簡化查詢 | 報表、儀表板 |
" },

        // ── 1415: Stored Procedure 預存程序 ──
        new() { Id=1415, Category="sql", Order=16, Level="intermediate", Icon="⚙️", Title="Stored Procedure 預存程序", Slug="sql-stored-procedure", IsPublished=true, Content=@"
# Stored Procedure 預存程序

## 什麼是 Stored Procedure？

> **比喻：SP 就像自動販賣機** 🎰
>
> 你投入硬幣（參數），按一個按鈕（呼叫），
> 機器內部執行一連串步驟，最後吐出飲料（結果）。

Stored Procedure（SP）是存在資料庫裡的**可重複執行的程式**。

---

## PostgreSQL 函數語法

PostgreSQL 使用 `CREATE FUNCTION` 而非 `CREATE PROCEDURE`（兩者都支持，但函數更常用）。

```sql
-- 建立一個函數：根據科系查詢學生
CREATE OR REPLACE FUNCTION get_students_by_dept(
    dept_name VARCHAR                    -- ← 輸入參數
)
RETURNS TABLE (                          -- ← 回傳表格類型
    student_name VARCHAR,
    student_age INT,
    student_score DECIMAL
)
LANGUAGE plpgsql                         -- ← 使用 PL/pgSQL 語言
AS $$
BEGIN
    RETURN QUERY                         -- ← 回傳查詢結果
    SELECT Name, Age, Score
    FROM Students
    WHERE Department = dept_name;
END;
$$;
```

### 呼叫函數

```sql
SELECT * FROM get_students_by_dept('資工系');
```

---

## 帶邏輯的函數

```sql
-- 註冊新學生（帶驗證邏輯）
CREATE OR REPLACE FUNCTION register_student(
    p_name VARCHAR,
    p_age INT,
    p_email VARCHAR
)
RETURNS INT                              -- 回傳新學生的 Id
LANGUAGE plpgsql
AS $$
DECLARE
    new_id INT;                          -- 宣告變數
BEGIN
    -- 驗證年齡
    IF p_age < 16 OR p_age > 60 THEN
        RAISE EXCEPTION '年齡必須在 16~60 之間';  -- 拋出錯誤
    END IF;

    -- 檢查 Email 是否已存在
    IF EXISTS (SELECT 1 FROM Students WHERE Email = p_email) THEN
        RAISE EXCEPTION 'Email 已被使用：%', p_email;
    END IF;

    -- 新增學生
    INSERT INTO Students (Name, Age, Email)
    VALUES (p_name, p_age, p_email)
    RETURNING Id INTO new_id;            -- 取得新 Id

    RETURN new_id;
END;
$$;
```

### 呼叫

```sql
SELECT register_student('小新', 19, 'new@test.com');
-- 成功 → 回傳 Id
-- 失敗 → 拋出錯誤訊息
```

---

## 變數與流程控制

```sql
CREATE OR REPLACE FUNCTION evaluate_student(p_id INT)
RETURNS VARCHAR
LANGUAGE plpgsql
AS $$
DECLARE
    v_score DECIMAL;                     -- 宣告變數
    v_result VARCHAR;
BEGIN
    -- 取得分數
    SELECT Score INTO v_score            -- 把查詢結果存入變數
    FROM Students
    WHERE Id = p_id;

    -- 判斷等級
    IF v_score >= 90 THEN
        v_result := '優秀';
    ELSIF v_score >= 80 THEN
        v_result := '良好';
    ELSIF v_score >= 60 THEN
        v_result := '及格';
    ELSE
        v_result := '不及格';
    END IF;

    RETURN v_result;
END;
$$;
```

---

## SP / Function 的優缺點

### ✅ 優點
- **效能好**：預先編譯，減少網路往返
- **安全性**：可以透過函數控制存取權限
- **重用性**：寫一次，到處呼叫
- **一致性**：商業邏輯集中在資料庫

### ❌ 缺點
- **難版控**：不像應用程式碼容易用 Git 管理
- **難除錯**：PL/pgSQL 的除錯工具不如程式語言
- **可移植性差**：每個資料庫的語法都不同
- **耦合**：商業邏輯綁在資料庫，換資料庫很痛苦

> 💡 **現代做法**：簡單邏輯放 SP，複雜商業邏輯放應用程式碼。
" },

        // ── 1416: Trigger 觸發器 ──
        new() { Id=1416, Category="sql", Order=17, Level="intermediate", Icon="⚡", Title="Trigger 觸發器", Slug="sql-trigger", IsPublished=true, Content=@"
# Trigger 觸發器

## 什麼是 Trigger？

> **比喻：Trigger 就像自動感應門** 🚪
>
> 有人走近（事件發生）→ 門自動打開（觸發動作）。
> 當資料被新增/修改/刪除時，Trigger 會自動執行你預設的程式。

---

## 建立 Trigger（PostgreSQL）

```sql
-- Step 1：建立 Trigger 函數
CREATE OR REPLACE FUNCTION update_timestamp()
RETURNS TRIGGER                          -- ← 回傳類型必須是 TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
    NEW.UpdatedAt = CURRENT_TIMESTAMP;   -- ← NEW 代表即將寫入的新資料
    RETURN NEW;                          -- ← 回傳修改後的資料
END;
$$;

-- Step 2：建立 Trigger，綁定到表
CREATE TRIGGER trg_students_updated       -- ← Trigger 名稱
    BEFORE UPDATE                         -- ← 在 UPDATE 之前觸發
    ON Students                           -- ← 綁定到 Students 表
    FOR EACH ROW                          -- ← 每一列都觸發
    EXECUTE FUNCTION update_timestamp();  -- ← 執行的函數
```

逐行解析：
```
BEFORE UPDATE              -- 時機：在 UPDATE 實際執行之前
ON Students                -- 觸發表：Students
FOR EACH ROW               -- 範圍：影響到的每一列都觸發一次
EXECUTE FUNCTION update_timestamp()  -- 執行的動作
```

---

## Trigger 的時機

| 時機 | 說明 |
|------|------|
| `BEFORE INSERT` | 新增前（可修改 NEW 的值） |
| `AFTER INSERT` | 新增後（適合寫日誌） |
| `BEFORE UPDATE` | 修改前（可修改 NEW 的值） |
| `AFTER UPDATE` | 修改後（適合同步、通知） |
| `BEFORE DELETE` | 刪除前（可取消刪除） |
| `AFTER DELETE` | 刪除後（適合清理關聯資料） |

---

## NEW 和 OLD

```sql
CREATE OR REPLACE FUNCTION log_score_change()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
    -- NEW = 更新後的新值
    -- OLD = 更新前的舊值
    INSERT INTO ScoreLog (StudentId, OldScore, NewScore, ChangedAt)
    VALUES (
        OLD.Id,                          -- ← 學生 Id（新舊都一樣）
        OLD.Score,                       -- ← 舊分數
        NEW.Score,                       -- ← 新分數
        CURRENT_TIMESTAMP
    );
    RETURN NEW;
END;
$$;

CREATE TRIGGER trg_score_change
    AFTER UPDATE OF Score                -- ← 只在 Score 欄位被改時觸發
    ON Students
    FOR EACH ROW
    WHEN (OLD.Score IS DISTINCT FROM NEW.Score)  -- ← 值真的有變才觸發
    EXECUTE FUNCTION log_score_change();
```

---

## 實用範例：軟刪除

```sql
-- 不真的刪除，而是標記為已刪除
CREATE OR REPLACE FUNCTION soft_delete()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
    -- 攔截 DELETE，改成 UPDATE
    UPDATE Students
    SET IsDeleted = true, DeletedAt = CURRENT_TIMESTAMP
    WHERE Id = OLD.Id;

    RETURN NULL;                         -- ← 回傳 NULL = 取消原本的 DELETE
END;
$$;

CREATE TRIGGER trg_soft_delete
    BEFORE DELETE ON Students
    FOR EACH ROW
    EXECUTE FUNCTION soft_delete();
```

---

## 管理 Trigger

```sql
-- 暫時停用 Trigger
ALTER TABLE Students DISABLE TRIGGER trg_students_updated;

-- 重新啟用
ALTER TABLE Students ENABLE TRIGGER trg_students_updated;

-- 刪除 Trigger
DROP TRIGGER trg_students_updated ON Students;

-- 查看表的所有 Trigger
SELECT trigger_name, event_manipulation, action_timing
FROM information_schema.triggers
WHERE event_object_table = 'students';
```

---

## ⚠️ Trigger 注意事項

1. **效能影響**：每次操作都會觸發，大量寫入時會變慢
2. **難除錯**：隱式執行，出問題不容易發現
3. **避免無限迴圈**：Trigger A 更新表 → 觸發 Trigger B → 又更新 → 觸發 Trigger A...
4. **現代替代方案**：在應用程式層處理（EF Core Interceptor、Middleware 等）
" },

        // ── 1417: Transaction 交易控制 ──
        new() { Id=1417, Category="sql", Order=18, Level="intermediate", Icon="🔒", Title="Transaction 交易控制", Slug="sql-transaction", IsPublished=true, Content=@"
# Transaction 交易控制

## 什麼是 Transaction？

> **比喻：Transaction 就像 ATM 轉帳** 🏧
>
> 你從 A 帳戶轉 1000 元到 B 帳戶：
> 1. A 帳戶 -1000
> 2. B 帳戶 +1000
>
> 如果步驟 1 成功但步驟 2 失敗，錢就消失了！
> Transaction 確保「全部成功」或「全部失敗」。

---

## ACID 四大特性

| 特性 | 英文 | 說明 |
|------|------|------|
| 原子性 | Atomicity | 全部成功或全部失敗，不會只做一半 |
| 一致性 | Consistency | 交易前後，資料保持一致（餘額不會變負） |
| 隔離性 | Isolation | 多個交易同時進行不會互相干擾 |
| 持久性 | Durability | 一旦 COMMIT，資料永久保存 |

---

## 基本語法

```sql
BEGIN;                               -- ← 開始交易

    -- 步驟 1：A 扣款
    UPDATE Accounts
    SET Balance = Balance - 1000
    WHERE Id = 1;

    -- 步驟 2：B 入款
    UPDATE Accounts
    SET Balance = Balance + 1000
    WHERE Id = 2;

COMMIT;                              -- ← 確認，全部生效
```

逐行解析：
```
BEGIN;                  -- 開始一個交易
                        -- 從此開始的所有操作都是「暫定的」

UPDATE ... (A 扣款)     -- 暫時從 A 扣 1000
UPDATE ... (B 入款)     -- 暫時給 B 加 1000

COMMIT;                 -- 全部沒問題 → 正式寫入資料庫
                        -- 如果中間任何一步出錯 → 用 ROLLBACK
```

---

## ROLLBACK — 復原

```sql
BEGIN;

    UPDATE Accounts SET Balance = Balance - 1000 WHERE Id = 1;
    UPDATE Accounts SET Balance = Balance + 1000 WHERE Id = 2;

    -- 發現轉錯人了！
    ROLLBACK;                        -- ← 取消全部，回到 BEGIN 之前的狀態
```

---

## SAVEPOINT — 存檔點

```sql
BEGIN;

    INSERT INTO Orders (CustomerId, Amount) VALUES (1, 500);
    SAVEPOINT sp1;                   -- ← 設一個存檔點

    INSERT INTO OrderItems (OrderId, Product) VALUES (1, 'A');
    INSERT INTO OrderItems (OrderId, Product) VALUES (1, 'B');

    -- 發現商品 B 有問題
    ROLLBACK TO sp1;                 -- ← 回到 sp1，只取消 OrderItems
                                     -- Orders 的 INSERT 還在！

    -- 重新插入正確的資料
    INSERT INTO OrderItems (OrderId, Product) VALUES (1, 'C');

COMMIT;                              -- ← 最終：Orders + 商品 A 和 C
```

---

## 隔離等級（Isolation Level）

| 等級 | 說明 | 問題 |
|------|------|------|
| READ UNCOMMITTED | 可讀其他交易未提交的資料 | 髒讀 |
| READ COMMITTED | 只讀已提交的資料（PostgreSQL 預設） | 不可重複讀 |
| REPEATABLE READ | 同一交易中多次讀取結果相同 | 幻讀 |
| SERIALIZABLE | 完全隔離，像排隊一個一個來 | 效能最差 |

```sql
-- 設定隔離等級
BEGIN;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

    SELECT Balance FROM Accounts WHERE Id = 1;
    -- 在 SERIALIZABLE 下，其他交易無法修改這筆資料
    UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1;

COMMIT;
```

---

## 實用範例：安全轉帳

```sql
CREATE OR REPLACE FUNCTION transfer_money(
    from_id INT,
    to_id INT,
    amount DECIMAL
)
RETURNS VOID
LANGUAGE plpgsql
AS $$
DECLARE
    from_balance DECIMAL;
BEGIN
    -- 檢查餘額
    SELECT Balance INTO from_balance
    FROM Accounts WHERE Id = from_id
    FOR UPDATE;                      -- ← 鎖定這列，防止同時被改

    IF from_balance < amount THEN
        RAISE EXCEPTION '餘額不足！目前餘額：%', from_balance;
    END IF;

    -- 扣款
    UPDATE Accounts SET Balance = Balance - amount WHERE Id = from_id;
    -- 入款
    UPDATE Accounts SET Balance = Balance + amount WHERE Id = to_id;
    -- 記錄
    INSERT INTO TransferLog (FromId, ToId, Amount, TransferAt)
    VALUES (from_id, to_id, amount, CURRENT_TIMESTAMP);
END;
$$;
```

---

## 死鎖（Deadlock）

```
交易 A：鎖住 Row 1，等待 Row 2
交易 B：鎖住 Row 2，等待 Row 1
→ 互相等待，永遠不會結束 = 死鎖！
```

```sql
-- 預防死鎖：統一鎖定順序
-- ✅ 好：永遠先鎖 Id 小的
BEGIN;
    SELECT * FROM Accounts WHERE Id = 1 FOR UPDATE;
    SELECT * FROM Accounts WHERE Id = 2 FOR UPDATE;
COMMIT;

-- ❌ 壞：A 先鎖 1 再鎖 2，B 先鎖 2 再鎖 1 → 死鎖
```
" },

        // ── 1418: Window Functions ──
        new() { Id=1418, Category="sql", Order=19, Level="advanced", Icon="🪟", Title="Window Functions 視窗函數", Slug="sql-window-functions", IsPublished=true, Content=@"
# Window Functions 視窗函數

## 什麼是 Window Function？

> **比喻：Window Function 就像站在教室門口看全班** 🪟
>
> GROUP BY 把全班打散到各個小房間，你只能看到自己那間的統計。
> Window Function 讓你站在門口，既能看到自己的分數，也能看到全班的統計。

Window Function 在**不縮減列數**的情況下做統計計算。

---

## GROUP BY vs Window Function

```sql
-- GROUP BY：每組一列（原始資料被壓縮）
SELECT Department, AVG(Score)
FROM Students
GROUP BY Department;
-- 結果只有 3 列（3 個科系）

-- Window Function：保留每一列，加上統計欄位
SELECT
    Name,
    Department,
    Score,
    AVG(Score) OVER (PARTITION BY Department) AS 系平均
FROM Students;
-- 結果還是 30 列（每個學生一列），但多了系平均欄位
```

---

## OVER() 基本語法

```sql
SELECT
    Name,
    Score,
    AVG(Score) OVER () AS 全班平均,       -- ← OVER() 空的 = 全體
    Score - AVG(Score) OVER () AS 差距    -- ← 每人與平均的差距
FROM Students;
```

結果：
```
 Name | Score | 全班平均 | 差距
------+-------+---------+------
 小明 |    85 |   75.0  | 10.0
 小華 |    78 |   75.0  |  3.0
 小美 |    62 |   75.0  | -13.0
```

---

## PARTITION BY — 分組窗口

```sql
SELECT
    Name,
    Department,
    Score,
    AVG(Score) OVER (PARTITION BY Department) AS 系平均,
    MAX(Score) OVER (PARTITION BY Department) AS 系最高分,
    Score - AVG(Score) OVER (PARTITION BY Department) AS 與系平均差距
FROM Students;
```

```
PARTITION BY Department  -- 按科系分窗口
                         -- 每個學生都能看到「自己科系」的統計
                         -- 但不會把資料壓縮成一列
```

---

## ROW_NUMBER、RANK、DENSE_RANK

```sql
SELECT
    Name,
    Score,
    ROW_NUMBER() OVER (ORDER BY Score DESC) AS 序號,
    RANK()       OVER (ORDER BY Score DESC) AS 排名,
    DENSE_RANK() OVER (ORDER BY Score DESC) AS 密集排名
FROM Students;
```

結果（假設有同分情況）：
```
 Name | Score | 序號 | 排名 | 密集排名
------+-------+------+------+---------
 小明 |    95 |    1 |    1 |       1
 小華 |    90 |    2 |    2 |       2
 小美 |    90 |    3 |    2 |       2   ← RANK 同分同名次
 小剛 |    85 |    4 |    4 |       3   ← RANK 跳 4，DENSE_RANK 接 3
```

| 函數 | 同分處理 | 下一名 |
|------|---------|--------|
| ROW_NUMBER | 不同分（1,2,3,4） | 接續 |
| RANK | 同分（1,2,2,4） | 跳號 |
| DENSE_RANK | 同分（1,2,2,3） | 接續 |

---

## 分組排名

```sql
-- 每個科系中的排名
SELECT
    Name,
    Department,
    Score,
    RANK() OVER (
        PARTITION BY Department      -- ← 每個科系各自排
        ORDER BY Score DESC          -- ← 分數高到低
    ) AS 系內排名
FROM Students;
```

```sql
-- 只取每科系前 3 名
WITH ranked AS (
    SELECT *, RANK() OVER (PARTITION BY Department ORDER BY Score DESC) AS rk
    FROM Students
)
SELECT * FROM ranked WHERE rk <= 3;
```

---

## LAG / LEAD — 前後列

```sql
SELECT
    Name,
    Score,
    LAG(Score, 1) OVER (ORDER BY Score DESC) AS 前一名分數,
    LEAD(Score, 1) OVER (ORDER BY Score DESC) AS 後一名分數,
    Score - LAG(Score, 1) OVER (ORDER BY Score DESC) AS 與前一名差距
FROM Students;
```

```
LAG(Score, 1)   -- 往前看 1 列的 Score（上一名的分數）
LEAD(Score, 1)  -- 往後看 1 列的 Score（下一名的分數）
```

---

## SUM / AVG 的窗口累計

```sql
-- 累計營業額
SELECT
    OrderDate,
    Amount,
    SUM(Amount) OVER (ORDER BY OrderDate) AS 累計營業額,
    AVG(Amount) OVER (ORDER BY OrderDate) AS 滾動平均
FROM Orders;
```

```sql
-- 移動平均（最近 7 天）
SELECT
    OrderDate,
    Amount,
    AVG(Amount) OVER (
        ORDER BY OrderDate
        ROWS BETWEEN 6 PRECEDING AND CURRENT ROW  -- ← 往前 6 列到現在
    ) AS 七日移動平均
FROM Orders;
```
" },

        // ── 1419: SQL 效能調校 ──
        new() { Id=1419, Category="sql", Order=20, Level="advanced", Icon="🏎️", Title="SQL 效能調校", Slug="sql-performance-tuning", IsPublished=true, Content=@"
# SQL 效能調校

## 為什麼要調校？

> **比喻：效能調校就像交通規劃** 🚗
>
> 一條路（查詢）如果有 100 萬台車（資料），
> 沒有紅綠燈和分流（索引和優化），就會塞車（查詢很慢）。

---

## EXPLAIN ANALYZE — 查詢計畫分析

```sql
-- 查看查詢的執行計畫
EXPLAIN ANALYZE
SELECT * FROM Students WHERE Email = 'test@gmail.com';
```

```
-- 沒有索引時：
Seq Scan on students            ← 全表掃描！
  Filter: (email = 'test@gmail.com')
  Rows Removed by Filter: 99999
  Planning Time: 0.1 ms
  Execution Time: 150.5 ms      ← 很慢

-- 有索引後：
Index Scan using idx_email      ← 用索引！
  Index Cond: (email = 'test@gmail.com')
  Planning Time: 0.1 ms
  Execution Time: 0.05 ms       ← 超快
```

### 關鍵字解讀

| 術語 | 意思 | 好壞 |
|------|------|------|
| Seq Scan | 全表掃描 | ❌ 慢 |
| Index Scan | 使用索引 | ✅ 快 |
| Bitmap Index Scan | 點陣圖索引 | ✅ 快 |
| Nested Loop | 巢狀迴圈 JOIN | 小表 OK |
| Hash Join | 雜湊 JOIN | 大表 OK |
| Sort | 排序 | 注意記憶體 |

---

## 常見效能陷阱

### 1. SELECT * 的問題

```sql
-- ❌ 壞：取全部欄位（包含大型 TEXT 欄位）
SELECT * FROM Articles WHERE CategoryId = 5;

-- ✅ 好：只取需要的欄位
SELECT Id, Title, CreatedAt FROM Articles WHERE CategoryId = 5;
```

### 2. 在索引欄位上做運算

```sql
-- ❌ 壞：函數包住欄位 → 索引失效
SELECT * FROM Students WHERE UPPER(Name) = 'MIKE';
SELECT * FROM Orders WHERE YEAR(OrderDate) = 2024;

-- ✅ 好：避免在欄位上做運算
SELECT * FROM Students WHERE Name = 'Mike';  -- 用大小寫不敏感的比對
SELECT * FROM Orders
WHERE OrderDate >= '2024-01-01' AND OrderDate < '2025-01-01';
```

### 3. LIKE 開頭用萬用字元

```sql
-- ❌ 壞：開頭用 % → 索引失效
SELECT * FROM Students WHERE Name LIKE '%明';

-- ✅ 好：開頭用確定值
SELECT * FROM Students WHERE Name LIKE '小%';
```

### 4. OR 可能導致索引失效

```sql
-- ❌ 可能不用索引
SELECT * FROM Students WHERE Age = 20 OR Email = 'test@gmail.com';

-- ✅ 改用 UNION
SELECT * FROM Students WHERE Age = 20
UNION
SELECT * FROM Students WHERE Email = 'test@gmail.com';
```

---

## N+1 查詢問題

```sql
-- ❌ N+1 問題（應用程式層常見）
-- 1. 先查所有訂單（1 次查詢）
SELECT * FROM Orders;
-- 2. 對每筆訂單查客戶名稱（N 次查詢）
SELECT Name FROM Customers WHERE Id = 1;
SELECT Name FROM Customers WHERE Id = 2;
-- ... 重複 N 次

-- ✅ 用 JOIN 一次搞定
SELECT o.*, c.Name AS CustomerName
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.Id;
```

---

## 分頁優化

```sql
-- ❌ 大 OFFSET 很慢（掃描 + 丟棄前 100000 筆）
SELECT * FROM Orders
ORDER BY Id
LIMIT 10 OFFSET 100000;

-- ✅ 用游標分頁（Keyset Pagination）
SELECT * FROM Orders
WHERE Id > 100000              -- ← 直接從上一頁最後的 Id 開始
ORDER BY Id
LIMIT 10;
```

---

## 其他優化技巧

```sql
-- 1. 用 EXISTS 取代 IN（大子查詢時）
-- ❌
SELECT * FROM Students WHERE Id IN (SELECT StudentId FROM Enrollments);
-- ✅
SELECT * FROM Students s WHERE EXISTS (
    SELECT 1 FROM Enrollments e WHERE e.StudentId = s.Id
);

-- 2. 批次操作取代逐筆操作
-- ❌ 一筆一筆插入
INSERT INTO Logs VALUES (...);
INSERT INTO Logs VALUES (...);
-- ✅ 批次插入
INSERT INTO Logs VALUES (...), (...), (...), ...;

-- 3. 適當使用 Materialized View
CREATE MATERIALIZED VIEW mv_report AS
SELECT ... -- 複雜的報表查詢
;
REFRESH MATERIALIZED VIEW mv_report;  -- 定時刷新
```

---

## 效能檢查清單

| # | 檢查項目 | 做法 |
|---|---------|------|
| 1 | 是否有用到索引？ | EXPLAIN ANALYZE |
| 2 | SELECT * ？ | 改成只選需要的欄位 |
| 3 | WHERE 欄位有運算？ | 移除運算，改寫條件 |
| 4 | N+1 查詢？ | 改用 JOIN |
| 5 | 大 OFFSET？ | 改用 Keyset Pagination |
| 6 | 適合的索引？ | 檢查 WHERE、JOIN、ORDER BY 的欄位 |
" },

        // ── 1420: CASE WHEN 條件表達式 ──
        new() { Id=1420, Category="sql", Order=21, Level="intermediate", Icon="🔀", Title="CASE WHEN 條件表達式", Slug="sql-case-when", IsPublished=true, Content=@"
# CASE WHEN 條件表達式

## 什麼是 CASE WHEN？

> **比喻：CASE WHEN 就像 if-else** 🔀
>
> 在 C# 裡你用 if-else 判斷邏輯，
> 在 SQL 裡就用 CASE WHEN 做同樣的事。

---

## 基本語法

```sql
SELECT
    Name,
    Score,
    CASE
        WHEN Score >= 90 THEN '優秀'       -- ← 條件 1
        WHEN Score >= 80 THEN '良好'       -- ← 條件 2
        WHEN Score >= 60 THEN '及格'       -- ← 條件 3
        ELSE '不及格'                       -- ← 以上都不符
    END AS 等級                             -- ← END 結束，AS 取名
FROM Students;
```

逐行解析：
```
CASE                          -- 開始條件判斷
    WHEN Score >= 90 THEN '優秀'  -- 如果 Score >= 90，回傳 '優秀'
    WHEN Score >= 80 THEN '良好'  -- 否則如果 >= 80，回傳 '良好'
    WHEN Score >= 60 THEN '及格'  -- 否則如果 >= 60，回傳 '及格'
    ELSE '不及格'                  -- 以上都不符合，回傳 '不及格'
END AS 等級                    -- END 結束 CASE，AS 給欄位命名
```

結果：
```
 Name | Score | 等級
------+-------+------
 小明 |    85 | 良好
 小華 |    92 | 優秀
 小美 |    58 | 不及格
```

---

## 在 WHERE 中使用

```sql
-- 根據條件過濾
SELECT * FROM Orders
WHERE
    CASE
        WHEN Status = 'vip' THEN Amount > 0
        ELSE Amount > 100
    END;
```

---

## 在 ORDER BY 中使用

```sql
-- 自訂排序順序
SELECT Name, Status
FROM Students
ORDER BY
    CASE Status
        WHEN 'active' THEN 1           -- ← active 排第一
        WHEN 'pending' THEN 2          -- ← pending 排第二
        WHEN 'inactive' THEN 3         -- ← inactive 排第三
        ELSE 4
    END;
```

---

## 搭配聚合函數 — 條件統計

```sql
-- 統計每個等級的人數
SELECT
    COUNT(CASE WHEN Score >= 90 THEN 1 END) AS 優秀人數,
    COUNT(CASE WHEN Score >= 60 AND Score < 90 THEN 1 END) AS 及格人數,
    COUNT(CASE WHEN Score < 60 THEN 1 END) AS 不及格人數
FROM Students;
```

結果：
```
 優秀人數 | 及格人數 | 不及格人數
---------+---------+-----------
       5 |      20 |         5
```

---

## 行轉列（Pivot）

```sql
-- 每個科系各等級的人數
SELECT
    Department,
    COUNT(CASE WHEN Score >= 90 THEN 1 END) AS 優秀,
    COUNT(CASE WHEN Score >= 60 AND Score < 90 THEN 1 END) AS 及格,
    COUNT(CASE WHEN Score < 60 THEN 1 END) AS 不及格
FROM Students
GROUP BY Department;
```

結果：
```
 Department | 優秀 | 及格 | 不及格
-----------+------+------+---------
 資工系     |    3 |    8 |       2
 電機系     |    2 |    6 |       1
```

---

## COALESCE — 處理 NULL

```sql
-- COALESCE 回傳第一個非 NULL 的值
SELECT
    Name,
    COALESCE(Phone, Email, '無聯絡方式') AS 聯絡方式
FROM Students;
-- Phone 有值 → 用 Phone
-- Phone 是 NULL，Email 有值 → 用 Email
-- 兩個都 NULL → 用 '無聯絡方式'
```

---

## NULLIF

```sql
-- NULLIF(a, b) = 如果 a = b 就回傳 NULL，否則回傳 a
-- 常用來避免除以零
SELECT
    Department,
    Total,
    Passed,
    Total / NULLIF(Passed, 0) AS 比率   -- ← Passed=0 時變 NULL 而不是錯誤
FROM DeptStats;
```

---

## 小結

| 語法 | 用途 |
|------|------|
| `CASE WHEN ... THEN ... END` | SQL 的 if-else |
| 搭配 `COUNT/SUM` | 條件統計 |
| 搭配 `ORDER BY` | 自訂排序 |
| `COALESCE(a, b, c)` | 第一個非 NULL |
| `NULLIF(a, b)` | a=b 時回傳 NULL |
" },
    };
}
