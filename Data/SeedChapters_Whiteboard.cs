using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_Whiteboard
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── Whiteboard Chapter 710 ────────────────────────────
        new() { Id=710, Category="project", Order=21, Level="intermediate", Icon="🧩", Title="面試白板題：陣列與字串", Slug="whiteboard-array-string", IsPublished=true, Content=@"
# 面試白板題：陣列與字串

## 為什麼要練白板題？

> 💡 **比喻：考駕照的路考**
> 你可能很會開車，但路考時還是要練固定的考試路線。
> 白板題也一樣——它不代表你的全部能力，
> 但它是面試這場遊戲的規則，練過就不怕。

---

## 題目一：Two Sum（兩數之和）

### 題目說明
給定一個整數陣列和一個目標值，找出陣列中兩個數字相加等於目標值的索引。

### 暴力解法 O(n²)

```csharp
// 暴力解法：雙重迴圈檢查所有配對 // Brute force: check all pairs
public int[] TwoSumBruteForce(int[] nums, int target) // 兩數之和暴力解
{
    for (int i = 0; i < nums.Length; i++) // 外層迴圈走訪每個元素
    {
        for (int j = i + 1; j < nums.Length; j++) // 內層迴圈檢查後面的元素
        {
            if (nums[i] + nums[j] == target) // 如果兩數相加等於目標值
            {
                return new int[] { i, j }; // 回傳兩個索引
            }
        }
    }
    return Array.Empty<int>(); // 找不到就回傳空陣列
}
```

### 優化解法 O(n) — 使用 Dictionary

```csharp
// 優化解法：用 Dictionary 記錄已看過的數字 // Optimized: use Dictionary
public int[] TwoSumOptimized(int[] nums, int target) // 兩數之和優化解
{
    var map = new Dictionary<int, int>(); // 建立字典存放「值 → 索引」
    for (int i = 0; i < nums.Length; i++) // 走訪陣列一次
    {
        int complement = target - nums[i]; // 計算需要的互補數字
        if (map.ContainsKey(complement)) // 如果字典中有互補數字
        {
            return new int[] { map[complement], i }; // 回傳兩個索引
        }
        map[nums[i]] = i; // 把當前數字和索引存入字典
    }
    return Array.Empty<int>(); // 找不到就回傳空陣列
}
```

### 時間複雜度分析

```
暴力解：O(n²) — 兩層迴圈，每個元素都要跟其他元素比較
優化解：O(n)  — 只走訪一次陣列，Dictionary 查詢是 O(1)
空間複雜度：O(n) — Dictionary 最多存 n 個元素
```

---

## 題目二：Reverse String（反轉字串）

### 暴力解法 — 新建字串

```csharp
// 暴力解法：從後面往前建立新字串 // Brute force: build new string backwards
public string ReverseStringSimple(string s) // 反轉字串簡單版
{
    char[] result = new char[s.Length]; // 建立同長度的字元陣列
    for (int i = 0; i < s.Length; i++) // 走訪每個字元
    {
        result[i] = s[s.Length - 1 - i]; // 從後面取字元放到前面
    }
    return new string(result); // 轉換回字串並回傳
}
```

### 優化解法 — 雙指標原地反轉

```csharp
// 優化解法：雙指標從兩端往中間交換 // Optimized: two pointers swap in-place
public void ReverseStringInPlace(char[] s) // 原地反轉字元陣列
{
    int left = 0; // 左指標從開頭開始
    int right = s.Length - 1; // 右指標從結尾開始
    while (left < right) // 當左指標還沒超過右指標
    {
        char temp = s[left]; // 暫存左邊的字元
        s[left] = s[right]; // 把右邊的字元放到左邊
        s[right] = temp; // 把暫存的字元放到右邊
        left++; // 左指標往右移
        right--; // 右指標往左移
    }
}
```

### 時間複雜度分析

```
兩種解法時間都是 O(n)
但原地反轉的空間複雜度是 O(1)，不需要額外空間！
```

---

## 題目三：Valid Parentheses（有效括號）

### 題目說明
給定一個只包含 '('、')'、'{'、'}'、'['、']' 的字串，判斷括號是否有效配對。

### 使用 Stack 解法

```csharp
// Stack 解法：遇到左括號推入，遇到右括號彈出比對 // Stack solution
public bool IsValidParentheses(string s) // 檢查括號是否有效
{
    var stack = new Stack<char>(); // 建立字元堆疊
    var pairs = new Dictionary<char, char> // 定義括號配對關係
    {
        { ')', '(' }, // 右括號對應左括號
        { '}', '{' }, // 右大括號對應左大括號
        { ']', '[' }  // 右中括號對應左中括號
    };

    foreach (char c in s) // 走訪字串中的每個字元
    {
        if (pairs.ContainsKey(c)) // 如果是右括號
        {
            if (stack.Count == 0 || stack.Pop() != pairs[c]) // 堆疊空或不配對
            {
                return false; // 回傳無效
            }
        }
        else // 如果是左括號
        {
            stack.Push(c); // 推入堆疊
        }
    }
    return stack.Count == 0; // 堆疊清空代表全部配對成功
}
```

### 時間複雜度分析

```
時間複雜度：O(n) — 走訪字串一次
空間複雜度：O(n) — 最差情況堆疊存放所有字元
```

---

## 題目四：Palindrome Check（迴文檢查）

### 暴力解法 — 反轉比較

```csharp
// 暴力解法：反轉字串後比較 // Brute force: reverse and compare
public bool IsPalindromeBrute(string s) // 迴文檢查暴力版
{
    string cleaned = new string( // 清理字串：只保留英數字並轉小寫
        s.Where(char.IsLetterOrDigit) // 篩選字母和數字
         .Select(char.ToLower) // 全部轉小寫
         .ToArray()); // 轉成字元陣列
    string reversed = new string(cleaned.Reverse().ToArray()); // 反轉字串
    return cleaned == reversed; // 比較是否相同
}
```

### 優化解法 — 雙指標

```csharp
// 優化解法：雙指標從兩端往中間比較 // Optimized: two pointers
public bool IsPalindromeOptimized(string s) // 迴文檢查優化版
{
    int left = 0; // 左指標
    int right = s.Length - 1; // 右指標
    while (left < right) // 當兩指標還沒交會
    {
        while (left < right && !char.IsLetterOrDigit(s[left])) // 跳過非英數字元
            left++; // 左指標右移
        while (left < right && !char.IsLetterOrDigit(s[right])) // 跳過非英數字元
            right--; // 右指標左移
        if (char.ToLower(s[left]) != char.ToLower(s[right])) // 比較兩端字元
            return false; // 不同就不是迴文
        left++; // 左指標右移
        right--; // 右指標左移
    }
    return true; // 全部比完都相同，是迴文
}
```

### 時間複雜度分析

```
暴力解：O(n) 時間，O(n) 空間（建立新字串）
優化解：O(n) 時間，O(1) 空間（不需要額外空間）
```

---

## 🤔 我這樣寫為什麼會錯？

### 錯誤一：Two Sum 忘記檢查同一個元素

```csharp
// ❌ 錯誤：同一個元素用了兩次 // Wrong: using same element twice
var map = new Dictionary<int, int>(); // 建立字典
for (int i = 0; i < nums.Length; i++) // 先把所有數字加入字典
    map[nums[i]] = i; // 存入字典
for (int i = 0; i < nums.Length; i++) // 再走訪一次
{
    int complement = target - nums[i]; // 計算互補值
    if (map.ContainsKey(complement)) // 有找到
        return new[] { i, map[complement] }; // ❌ 可能 i == map[complement]
}

// ✅ 正確：邊走邊查，確保不會用到同一個元素 // Correct: check while iterating
var map2 = new Dictionary<int, int>(); // 建立字典
for (int i = 0; i < nums.Length; i++) // 走訪一次
{
    int complement = target - nums[i]; // 計算互補值
    if (map2.ContainsKey(complement)) // 如果之前已經存過互補值
        return new[] { map2[complement], i }; // 一定是不同元素
    map2[nums[i]] = i; // 存入當前元素
}
```

### 錯誤二：括號檢查忘記處理空堆疊

```csharp
// ❌ 錯誤：沒檢查堆疊是否為空就 Pop // Wrong: Pop without checking
if (stack.Pop() != pairs[c]) // ❌ 堆疊可能是空的，會拋出例外

// ✅ 正確：先檢查堆疊再 Pop // Correct: check before Pop
if (stack.Count == 0 || stack.Pop() != pairs[c]) // 先確認堆疊不為空
```

---

## 面試小提示

```
1. 先釐清題目：問清楚邊界條件（空陣列？重複元素？）
2. 先說暴力解：讓面試官知道你會做，再優化
3. 分析複雜度：每個解法都要能說出時間和空間複雜度
4. 寫程式碼前先說思路：不要埋頭就寫
5. 測試：寫完後用範例 input 走一遍程式碼
```
" },

        // ── Whiteboard Chapter 711 ────────────────────────────
        new() { Id=711, Category="project", Order=22, Level="intermediate", Icon="🌳", Title="面試白板題：鏈結串列與樹", Slug="whiteboard-linkedlist-tree", IsPublished=true, Content=@"
# 面試白板題：鏈結串列與樹

## 資料結構基礎

> 💡 **比喻：鏈結串列像火車車廂**
> 每節車廂（節點）裝著貨物（資料），
> 並用連結器（指標）連接下一節車廂。
> 你只能從車頭開始，一節一節往後走。

### 基本節點定義

```csharp
// 鏈結串列節點定義 // Linked list node definition
public class ListNode // 鏈結串列節點類別
{
    public int Val { get; set; } // 節點的值
    public ListNode? Next { get; set; } // 指向下一個節點的指標

    public ListNode(int val, ListNode? next = null) // 建構子
    {
        Val = val; // 設定節點值
        Next = next; // 設定下一個節點
    }
}

// 二元樹節點定義 // Binary tree node definition
public class TreeNode // 二元樹節點類別
{
    public int Val { get; set; } // 節點的值
    public TreeNode? Left { get; set; } // 左子節點
    public TreeNode? Right { get; set; } // 右子節點

    public TreeNode(int val) // 建構子
    {
        Val = val; // 設定節點值
    }
}
```

---

## 題目一：Reverse Linked List（反轉鏈結串列）

### 迭代解法

```csharp
// 迭代解法：三個指標逐步反轉 // Iterative: reverse with three pointers
public ListNode? ReverseListIterative(ListNode? head) // 反轉鏈結串列迭代版
{
    ListNode? prev = null; // 前一個節點，初始為 null
    ListNode? current = head; // 當前節點，從頭開始
    while (current != null) // 當還有節點要處理
    {
        ListNode? next = current.Next; // 暫存下一個節點
        current.Next = prev; // 把當前節點的 Next 指向前一個
        prev = current; // 前一個節點移到當前位置
        current = next; // 當前節點移到下一個位置
    }
    return prev; // prev 就是新的頭節點
}
```

### 遞迴解法

```csharp
// 遞迴解法：從尾巴開始反轉 // Recursive: reverse from tail
public ListNode? ReverseListRecursive(ListNode? head) // 反轉鏈結串列遞迴版
{
    if (head == null || head.Next == null) // 基本情況：空或只有一個節點
        return head; // 直接回傳
    ListNode? newHead = ReverseListRecursive(head.Next); // 遞迴反轉後面的部分
    head.Next.Next = head; // 讓下一個節點指回自己
    head.Next = null; // 斷開原本的連結
    return newHead; // 回傳新的頭節點
}
```

### 圖解反轉過程

```
原始：1 → 2 → 3 → null

步驟一：prev=null, curr=1
        null ← 1    2 → 3 → null

步驟二：prev=1, curr=2
        null ← 1 ← 2    3 → null

步驟三：prev=2, curr=3
        null ← 1 ← 2 ← 3

結果：3 → 2 → 1 → null
```

---

## 題目二：Merge Two Sorted Lists（合併排序鏈結串列）

### 迭代解法

```csharp
// 迭代解法：用虛擬頭節點簡化邏輯 // Iterative: use dummy head
public ListNode? MergeTwoListsIterative(ListNode? l1, ListNode? l2) // 合併兩個排序鏈結串列
{
    var dummy = new ListNode(0); // 建立虛擬頭節點
    var current = dummy; // 用 current 追蹤串列尾端
    while (l1 != null && l2 != null) // 當兩個串列都還有節點
    {
        if (l1.Val <= l2.Val) // 如果 l1 的值比較小
        {
            current.Next = l1; // 接上 l1 的節點
            l1 = l1.Next; // l1 往前移
        }
        else // 如果 l2 的值比較小
        {
            current.Next = l2; // 接上 l2 的節點
            l2 = l2.Next; // l2 往前移
        }
        current = current.Next; // current 往前移
    }
    current.Next = l1 ?? l2; // 把剩餘的串列接上
    return dummy.Next; // 回傳虛擬頭節點的下一個（真正的頭）
}
```

### 遞迴解法

```csharp
// 遞迴解法：每次取較小的節點 // Recursive: pick smaller node each time
public ListNode? MergeTwoListsRecursive(ListNode? l1, ListNode? l2) // 遞迴合併
{
    if (l1 == null) return l2; // l1 用完，回傳 l2 剩餘部分
    if (l2 == null) return l1; // l2 用完，回傳 l1 剩餘部分
    if (l1.Val <= l2.Val) // 如果 l1 的值比較小
    {
        l1.Next = MergeTwoListsRecursive(l1.Next, l2); // 遞迴合併 l1 的下一個和 l2
        return l1; // 回傳 l1 作為當前節點
    }
    else // 如果 l2 的值比較小
    {
        l2.Next = MergeTwoListsRecursive(l1, l2.Next); // 遞迴合併 l1 和 l2 的下一個
        return l2; // 回傳 l2 作為當前節點
    }
}
```

---

## 題目三：Binary Tree Traversal（二元樹走訪）

### 三種走訪順序

```csharp
// 前序走訪：根 → 左 → 右 // Preorder: Root → Left → Right
public List<int> PreorderTraversal(TreeNode? root) // 前序走訪
{
    var result = new List<int>(); // 建立結果清單
    if (root == null) return result; // 空節點直接回傳
    result.Add(root.Val); // 先加入根節點的值
    result.AddRange(PreorderTraversal(root.Left)); // 遞迴走訪左子樹
    result.AddRange(PreorderTraversal(root.Right)); // 遞迴走訪右子樹
    return result; // 回傳結果
}

// 中序走訪：左 → 根 → 右 // Inorder: Left → Root → Right
public List<int> InorderTraversal(TreeNode? root) // 中序走訪
{
    var result = new List<int>(); // 建立結果清單
    if (root == null) return result; // 空節點直接回傳
    result.AddRange(InorderTraversal(root.Left)); // 遞迴走訪左子樹
    result.Add(root.Val); // 加入根節點的值
    result.AddRange(InorderTraversal(root.Right)); // 遞迴走訪右子樹
    return result; // 回傳結果
}

// 後序走訪：左 → 右 → 根 // Postorder: Left → Right → Root
public List<int> PostorderTraversal(TreeNode? root) // 後序走訪
{
    var result = new List<int>(); // 建立結果清單
    if (root == null) return result; // 空節點直接回傳
    result.AddRange(PostorderTraversal(root.Left)); // 遞迴走訪左子樹
    result.AddRange(PostorderTraversal(root.Right)); // 遞迴走訪右子樹
    result.Add(root.Val); // 加入根節點的值
    return result; // 回傳結果
}
```

### 迭代版前序走訪（使用 Stack）

```csharp
// 迭代版前序走訪：用 Stack 模擬遞迴 // Iterative preorder with Stack
public List<int> PreorderIterative(TreeNode? root) // 迭代前序走訪
{
    var result = new List<int>(); // 建立結果清單
    if (root == null) return result; // 空樹直接回傳
    var stack = new Stack<TreeNode>(); // 建立堆疊
    stack.Push(root); // 根節點入堆疊
    while (stack.Count > 0) // 當堆疊不為空
    {
        var node = stack.Pop(); // 彈出頂端節點
        result.Add(node.Val); // 加入結果
        if (node.Right != null) stack.Push(node.Right); // 先推右子節點（後處理）
        if (node.Left != null) stack.Push(node.Left); // 再推左子節點（先處理）
    }
    return result; // 回傳結果
}
```

---

## 題目四：Maximum Depth of Binary Tree（最大深度）

### 遞迴解法

```csharp
// 遞迴解法：取左右子樹深度的較大值加一 // Recursive: max of left/right + 1
public int MaxDepth(TreeNode? root) // 計算二元樹最大深度
{
    if (root == null) return 0; // 空節點深度為 0
    int leftDepth = MaxDepth(root.Left); // 遞迴計算左子樹深度
    int rightDepth = MaxDepth(root.Right); // 遞迴計算右子樹深度
    return Math.Max(leftDepth, rightDepth) + 1; // 取較大值加上自己
}
```

### 迭代解法（BFS 層序走訪）

```csharp
// 迭代解法：BFS 一層一層走，數有幾層 // Iterative: BFS level by level
public int MaxDepthBFS(TreeNode? root) // BFS 計算最大深度
{
    if (root == null) return 0; // 空樹深度為 0
    var queue = new Queue<TreeNode>(); // 建立佇列
    queue.Enqueue(root); // 根節點入佇列
    int depth = 0; // 深度計數器
    while (queue.Count > 0) // 當佇列不為空
    {
        int levelSize = queue.Count; // 當前層的節點數
        for (int i = 0; i < levelSize; i++) // 處理這一層所有節點
        {
            var node = queue.Dequeue(); // 取出節點
            if (node.Left != null) queue.Enqueue(node.Left); // 左子節點入佇列
            if (node.Right != null) queue.Enqueue(node.Right); // 右子節點入佇列
        }
        depth++; // 這一層處理完，深度加一
    }
    return depth; // 回傳總深度
}
```

---

## 🤔 我這樣寫為什麼會錯？

### 錯誤一：反轉鏈結串列忘記暫存 Next

```csharp
// ❌ 錯誤：直接改 Next 導致斷鏈 // Wrong: lost reference to next
current.Next = prev; // 改了 Next 之後...
current = current.Next; // ❌ 這時 current.Next 已經是 prev 了！

// ✅ 正確：先暫存再改 // Correct: save next first
ListNode? next = current.Next; // 先把下一個存起來
current.Next = prev; // 再反轉指標
current = next; // 用暫存的 next 移動
```

### 錯誤二：遞迴沒有基本情況

```csharp
// ❌ 錯誤：無限遞迴 // Wrong: infinite recursion
public int MaxDepth(TreeNode root) // 忘記檢查 null
{
    return Math.Max(MaxDepth(root.Left), MaxDepth(root.Right)) + 1; // ❌ NullReferenceException
}

// ✅ 正確：加上基本情況 // Correct: add base case
public int MaxDepth(TreeNode? root) // 參數可為 null
{
    if (root == null) return 0; // 基本情況：空節點回傳 0
    return Math.Max(MaxDepth(root.Left), MaxDepth(root.Right)) + 1; // 遞迴計算
}
```

### 錯誤三：合併串列忘記處理剩餘節點

```csharp
// ❌ 錯誤：迴圈結束後沒接上剩餘的串列 // Wrong: forgot remaining nodes
while (l1 != null && l2 != null) { /* ... */ } // 迴圈結束後
return dummy.Next; // ❌ 較長的串列後面的節點不見了！

// ✅ 正確：接上剩餘的串列 // Correct: append remaining
while (l1 != null && l2 != null) { /* ... */ } // 迴圈結束後
current.Next = l1 ?? l2; // 把還有剩的串列接上去
return dummy.Next; // 回傳完整結果
```

---

## 複雜度總整理

```
題目                    時間    空間    關鍵技巧
─────────────────────────────────────────────
反轉鏈結串列（迭代）    O(n)   O(1)   三指標法
反轉鏈結串列（遞迴）    O(n)   O(n)   呼叫堆疊空間
合併排序串列（迭代）    O(n+m) O(1)   虛擬頭節點
合併排序串列（遞迴）    O(n+m) O(n+m) 呼叫堆疊
二元樹走訪              O(n)   O(n)   遞迴 / Stack / Queue
最大深度                O(n)   O(h)   h = 樹的高度
```
" },

        // ── Whiteboard Chapter 712 ────────────────────────────
        new() { Id=712, Category="project", Order=23, Level="advanced", Icon="⚡", Title="面試白板題：動態規劃與排序", Slug="whiteboard-dp-sorting", IsPublished=true, Content=@"
# 面試白板題：動態規劃與排序

## 什麼是動態規劃？

> 💡 **比喻：聰明的備忘錄**
> 假設你在爬樓梯，每次可以爬 1 或 2 階。
> 如果有人問你「爬到第 10 階有幾種方法？」——
> 你可以把「爬到第 1 階」「爬到第 2 階」的答案記下來，
> 後面的答案就是前面兩個相加，不用重複計算。
> 這就是動態規劃的核心：**記住已經算過的結果**。

---

## 題目一：Climbing Stairs（爬樓梯）

### 暴力遞迴 O(2^n) — 超時！

```csharp
// 暴力遞迴：每次選擇爬 1 或 2 階 // Brute force: exponential time
public int ClimbStairsBrute(int n) // 爬樓梯暴力解
{
    if (n <= 2) return n; // 1 階有 1 種，2 階有 2 種
    return ClimbStairsBrute(n - 1) + ClimbStairsBrute(n - 2); // 遞迴計算
    // ❌ 會重複計算非常多次，指數級時間複雜度 // 會超時！
}
```

### 記憶化遞迴 O(n)

```csharp
// 記憶化遞迴：用字典記住算過的結果 // Memoization: cache computed results
public int ClimbStairsMemo(int n, Dictionary<int, int>? memo = null) // 記憶化版本
{
    memo ??= new Dictionary<int, int>(); // 初始化備忘錄
    if (n <= 2) return n; // 基本情況
    if (memo.ContainsKey(n)) return memo[n]; // 如果算過了直接回傳
    memo[n] = ClimbStairsMemo(n - 1, memo) + ClimbStairsMemo(n - 2, memo); // 計算並記住
    return memo[n]; // 回傳結果
}
```

### DP 表格解法 O(n)

```csharp
// DP 表格：從小問題往上計算 // Bottom-up DP table
public int ClimbStairsDP(int n) // 爬樓梯 DP 解法
{
    if (n <= 2) return n; // 基本情況
    int[] dp = new int[n + 1]; // 建立 DP 表格
    dp[1] = 1; // 1 階有 1 種方法
    dp[2] = 2; // 2 階有 2 種方法
    for (int i = 3; i <= n; i++) // 從第 3 階開始算
    {
        dp[i] = dp[i - 1] + dp[i - 2]; // 等於前兩階的方法數相加
    }
    return dp[n]; // 回傳第 n 階的方法數
}
```

### 空間優化 O(1)

```csharp
// 空間優化：只需要前兩個數字 // Space optimized: only need two variables
public int ClimbStairsOptimal(int n) // 最佳化爬樓梯
{
    if (n <= 2) return n; // 基本情況
    int prev2 = 1; // 前兩階的方法數
    int prev1 = 2; // 前一階的方法數
    for (int i = 3; i <= n; i++) // 從第 3 階開始
    {
        int current = prev1 + prev2; // 當前階 = 前一階 + 前兩階
        prev2 = prev1; // 更新前兩階
        prev1 = current; // 更新前一階
    }
    return prev1; // 回傳結果
}
```

---

## 題目二：Coin Change（零錢問題）

### 題目說明
給定不同面額的硬幣和一個總金額，計算湊出總金額所需的最少硬幣數。

### 暴力遞迴

```csharp
// 暴力遞迴：嘗試每種硬幣 // Brute force: try every coin
public int CoinChangeBrute(int[] coins, int amount) // 零錢問題暴力解
{
    if (amount == 0) return 0; // 金額為 0 不需要硬幣
    if (amount < 0) return -1; // 金額為負代表這條路不通
    int minCoins = int.MaxValue; // 初始化最小硬幣數
    foreach (int coin in coins) // 嘗試每種硬幣
    {
        int result = CoinChangeBrute(coins, amount - coin); // 遞迴計算剩餘金額
        if (result >= 0 && result + 1 < minCoins) // 如果這條路可行且更少
        {
            minCoins = result + 1; // 更新最小硬幣數
        }
    }
    return minCoins == int.MaxValue ? -1 : minCoins; // 回傳結果
}
```

### DP 表格解法

```csharp
// DP 解法：建表從金額 0 算到目標金額 // DP: build table from 0 to amount
public int CoinChangeDP(int[] coins, int amount) // 零錢問題 DP 解法
{
    int[] dp = new int[amount + 1]; // 建立 DP 表格
    Array.Fill(dp, amount + 1); // 初始化為不可能的大數
    dp[0] = 0; // 金額 0 需要 0 個硬幣
    for (int i = 1; i <= amount; i++) // 從金額 1 算到目標金額
    {
        foreach (int coin in coins) // 嘗試每種硬幣
        {
            if (coin <= i) // 如果這個硬幣面額不超過當前金額
            {
                dp[i] = Math.Min(dp[i], dp[i - coin] + 1); // 取較少的硬幣數
            }
        }
    }
    return dp[amount] > amount ? -1 : dp[amount]; // 回傳結果
}
```

### DP 推導過程圖解

```
硬幣 = [1, 3, 5]，目標金額 = 7

dp[0] = 0  （0 元需要 0 個硬幣）
dp[1] = 1  （1 元 → 用 1 個「1 元」）
dp[2] = 2  （2 元 → 用 2 個「1 元」）
dp[3] = 1  （3 元 → 用 1 個「3 元」）
dp[4] = 2  （4 元 → 用 1+3）
dp[5] = 1  （5 元 → 用 1 個「5 元」）
dp[6] = 2  （6 元 → 用 1+5 或 3+3）
dp[7] = 3  （7 元 → 用 1+1+5 或 1+3+3）

答案：dp[7] = 3
```

---

## 題目三：Quick Sort vs Merge Sort 實作

### Quick Sort（快速排序）

```csharp
// 快速排序：選基準值，分成大小兩堆 // Quick Sort: pivot partitioning
public void QuickSort(int[] arr, int low, int high) // 快速排序主方法
{
    if (low < high) // 還有元素要排
    {
        int pivotIndex = Partition(arr, low, high); // 取得基準值的正確位置
        QuickSort(arr, low, pivotIndex - 1); // 遞迴排左半邊
        QuickSort(arr, pivotIndex + 1, high); // 遞迴排右半邊
    }
}

private int Partition(int[] arr, int low, int high) // 分割：把小的放左邊大的放右邊
{
    int pivot = arr[high]; // 選最後一個元素作為基準值
    int i = low - 1; // i 指向小於基準值區域的邊界
    for (int j = low; j < high; j++) // 走訪所有元素
    {
        if (arr[j] < pivot) // 如果當前元素小於基準值
        {
            i++; // 擴大小於區域
            (arr[i], arr[j]) = (arr[j], arr[i]); // 交換位置
        }
    }
    (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]); // 基準值放到正確位置
    return i + 1; // 回傳基準值的索引
}
```

### Merge Sort（合併排序）

```csharp
// 合併排序：分成兩半再合併 // Merge Sort: divide and merge
public int[] MergeSort(int[] arr) // 合併排序主方法
{
    if (arr.Length <= 1) return arr; // 只有一個元素不用排
    int mid = arr.Length / 2; // 找到中間點
    int[] left = MergeSort(arr[..mid]); // 遞迴排左半邊
    int[] right = MergeSort(arr[mid..]); // 遞迴排右半邊
    return Merge(left, right); // 合併兩個已排序的陣列
}

private int[] Merge(int[] left, int[] right) // 合併兩個已排序陣列
{
    int[] result = new int[left.Length + right.Length]; // 建立結果陣列
    int i = 0, j = 0, k = 0; // 三個索引指標
    while (i < left.Length && j < right.Length) // 當兩邊都還有元素
    {
        if (left[i] <= right[j]) // 左邊的比較小
            result[k++] = left[i++]; // 放入左邊的元素
        else // 右邊的比較小
            result[k++] = right[j++]; // 放入右邊的元素
    }
    while (i < left.Length) result[k++] = left[i++]; // 放入左邊剩餘的元素
    while (j < right.Length) result[k++] = right[j++]; // 放入右邊剩餘的元素
    return result; // 回傳合併後的陣列
}
```

### 比較表

```
特性            Quick Sort          Merge Sort
──────────────────────────────────────────────
平均時間        O(n log n)          O(n log n)
最差時間        O(n²)               O(n log n)
空間複雜度      O(log n)            O(n)
穩定排序        ❌ 不穩定            ✅ 穩定
原地排序        ✅ 是                ❌ 否
適合場景        一般排序              需要穩定排序時
```

---

## 題目四：Binary Search 變體

### 基本二分搜尋

```csharp
// 基本二分搜尋：在已排序陣列中找目標值 // Basic binary search
public int BinarySearch(int[] nums, int target) // 二分搜尋
{
    int left = 0; // 左邊界
    int right = nums.Length - 1; // 右邊界
    while (left <= right) // 當搜尋範圍還有效
    {
        int mid = left + (right - left) / 2; // 計算中間位置（避免溢位）
        if (nums[mid] == target) return mid; // 找到了
        if (nums[mid] < target) left = mid + 1; // 目標在右半邊
        else right = mid - 1; // 目標在左半邊
    }
    return -1; // 找不到回傳 -1
}
```

### 變體一：找第一個出現的位置

```csharp
// 變體：找目標值第一次出現的位置 // Variant: find first occurrence
public int FindFirst(int[] nums, int target) // 找第一個出現的位置
{
    int left = 0; // 左邊界
    int right = nums.Length - 1; // 右邊界
    int result = -1; // 預設找不到
    while (left <= right) // 當搜尋範圍還有效
    {
        int mid = left + (right - left) / 2; // 計算中間位置
        if (nums[mid] == target) // 找到目標值
        {
            result = mid; // 記錄位置
            right = mid - 1; // 繼續往左找更早出現的
        }
        else if (nums[mid] < target) left = mid + 1; // 目標在右半邊
        else right = mid - 1; // 目標在左半邊
    }
    return result; // 回傳第一次出現的位置
}
```

### 變體二：找插入位置

```csharp
// 變體：找目標值應該插入的位置 // Variant: find insert position
public int SearchInsertPosition(int[] nums, int target) // 找插入位置
{
    int left = 0; // 左邊界
    int right = nums.Length - 1; // 右邊界
    while (left <= right) // 當搜尋範圍還有效
    {
        int mid = left + (right - left) / 2; // 計算中間位置
        if (nums[mid] == target) return mid; // 找到直接回傳
        if (nums[mid] < target) left = mid + 1; // 目標在右半邊
        else right = mid - 1; // 目標在左半邊
    }
    return left; // left 就是應該插入的位置
}
```

---

## 🤔 我這樣寫為什麼會錯？

### 錯誤一：DP 忘記初始化

```csharp
// ❌ 錯誤：dp 陣列沒有初始化 // Wrong: dp not initialized
int[] dp = new int[amount + 1]; // 預設都是 0
dp[0] = 0; // 設定起點
// ❌ dp[1] 到 dp[amount] 都是 0，Math.Min 永遠選 0

// ✅ 正確：初始化為不可能的大數 // Correct: initialize to impossible value
int[] dp2 = new int[amount + 1]; // 建立 DP 表格
Array.Fill(dp2, amount + 1); // 填入大數代表「還沒算」
dp2[0] = 0; // 金額 0 需要 0 個硬幣
```

### 錯誤二：二分搜尋中間值溢位

```csharp
// ❌ 錯誤：可能溢位 // Wrong: potential overflow
int mid = (left + right) / 2; // ❌ left + right 可能超過 int.MaxValue

// ✅ 正確：避免溢位的寫法 // Correct: overflow-safe
int mid2 = left + (right - left) / 2; // 永遠不會溢位
```

### 錯誤三：Quick Sort 基準值選擇不當

```csharp
// ❌ 風險：總是選第一個或最後一個元素 // Risky: always pick first/last
int pivot = arr[low]; // 如果陣列已排序，會退化成 O(n²)

// ✅ 更好：隨機選擇或三數取中 // Better: random or median-of-three
int randomIndex = new Random().Next(low, high + 1); // 隨機選一個
(arr[randomIndex], arr[high]) = (arr[high], arr[randomIndex]); // 交換到最後
int pivot2 = arr[high]; // 再用最後一個當基準值
```

---

## DP 解題框架

```
解 DP 題目的五步驟：

1. 定義狀態：dp[i] 代表什麼？
   → 爬樓梯：dp[i] = 爬到第 i 階的方法數
   → 零錢：dp[i] = 湊出金額 i 的最少硬幣數

2. 定義轉移方程：dp[i] 怎麼從前面的值算出來？
   → 爬樓梯：dp[i] = dp[i-1] + dp[i-2]
   → 零錢：dp[i] = min(dp[i], dp[i-coin] + 1)

3. 初始化：dp[0] 或 dp[1] 的值是什麼？
   → 爬樓梯：dp[1] = 1, dp[2] = 2
   → 零錢：dp[0] = 0

4. 計算順序：從小到大填表格

5. 回傳結果：dp[n] 就是答案
```
" },
    };
}
