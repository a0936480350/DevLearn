using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_AIModel
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── AI 基礎概念 Chapter 530 ────────────────────────────
        new() { Id=530, Category="aimodel", Order=1, Level="beginner", Icon="🧠", Title="AI 基礎概念", Slug="ai-fundamentals", IsPublished=true, Content=@"
# AI 基礎概念

## 什麼是 AI / ML / DL？

> 💡 **比喻：教小孩學認字**
> 想像你在教一個小朋友認字：
> - **AI（人工智慧）**：小朋友「學會認字」這件事本身就是智慧的表現
> - **ML（機器學習）**：你拿很多字卡給小朋友看，讓他自己找出規律
> - **DL（深度學習）**：小朋友的大腦有很多層神經元在處理資訊，越深層越抽象
>
> 所以：AI 是目標，ML 是方法，DL 是更進階的方法。

### 三者的關係

```
AI（人工智慧）          // AI 是最大的概念，讓機器表現出智慧行為
├── ML（機器學習）      // ML 是 AI 的子集，讓機器從資料中學習
│   ├── DL（深度學習）  // DL 是 ML 的子集，使用多層神經網路
│   │   ├── CNN        // 卷積神經網路，擅長影像辨識
│   │   ├── RNN        // 遞歸神經網路，擅長序列資料
│   │   └── Transformer // 注意力機制，現代 LLM 的基礎
│   ├── 決策樹          // 像是一連串的 if-else 判斷
│   ├── SVM            // 支持向量機，找最佳分界線
│   └── KNN            // K 近鄰演算法，物以類聚
└── 專家系統            // 用人類定義的規則來做決策
```

### 用 C# 理解 AI 的概念

```csharp
// 傳統程式：人類寫規則
public string ClassifyEmail(string email) // 傳統方式：人類定義所有規則
{
    if (email.Contains(""免費""))  // 人類告訴電腦「看到免費就是垃圾信」
        return ""垃圾信"";        // 每個規則都要人寫，很累
    if (email.Contains(""中獎""))  // 而且規則可能不夠全面
        return ""垃圾信"";        // 騙子換個說法就破功了
    return ""正常信"";            // 傳統程式的極限就在這裡
}

// ML 的方式：讓機器從資料中學規則
public class SpamClassifier // ML 方式：機器自己學規則
{
    private MLModel _model; // 模型就像是機器的「大腦」

    public void Train(List<EmailData> data) // 餵資料給機器學習
    {
        _model = ML.Train(data); // 機器自己找出垃圾信的規律
    }                            // 不用人類一條一條寫規則

    public string Predict(string email) // 用學到的規律來預測
    {
        return _model.Predict(email);   // 機器根據學到的模式判斷
    }                                    // 即使遇到新的詐騙手法也能辨識
}
```

---

## 監督式 vs 非監督式 vs 強化學習

> 💡 **比喻：三種教學方式**
> - **監督式學習**：老師出題，每題都有標準答案（有答案的考卷）
> - **非監督式學習**：給你一堆東西，自己分類（沒人告訴你怎麼分）
> - **強化學習**：玩遊戲，做對加分做錯扣分（邊玩邊學）

### 監督式學習 (Supervised Learning)

```csharp
// 監督式學習：每筆資料都有標籤（答案）
var trainingData = new List<LabeledData> // 訓練資料，每筆都有正確答案
{
    new(""今天加班到很晚"", ""負面""),   // 告訴機器這句話是「負面」情緒
    new(""週末去海邊玩"", ""正面""),     // 告訴機器這句話是「正面」情緒
    new(""考試沒過好難過"", ""負面""),   // 有標籤的資料越多，學得越好
    new(""升職加薪了耶"", ""正面""),     // 就像老師批改的考卷越多，學生越進步
};

// 常見應用場景
// 1. 分類問題：垃圾郵件偵測、情緒分析、疾病診斷
// 2. 迴歸問題：房價預測、股價趨勢、溫度預報
```

### 非監督式學習 (Unsupervised Learning)

```csharp
// 非監督式學習：資料沒有標籤，讓機器自己找規律
var customers = new List<CustomerData> // 只有資料，沒有分類標籤
{
    new(Age: 25, Spending: 5000),  // 機器會自己發現這些人消費習慣相似
    new(Age: 30, Spending: 4500),  // 不需要人類事先定義「這是高消費群」
    new(Age: 55, Spending: 15000), // 機器自己會把相似的客戶歸在一起
    new(Age: 60, Spending: 18000), // 這就是「分群」(Clustering)
};

// 常見應用場景
// 1. 客戶分群：找出不同類型的消費者
// 2. 異常偵測：找出信用卡盜刷行為
// 3. 推薦系統：「買了這個的人也買了那個」
```

### 強化學習 (Reinforcement Learning)

```csharp
// 強化學習：透過獎勵和懲罰來學習最佳策略
public class GameAgent // 遊戲 AI 代理人
{
    public Action ChooseAction(State state) // 根據當前狀態選擇動作
    {
        // 一開始隨機嘗試（探索）
        // 慢慢學會哪些動作會得到獎勵（利用）
        // 最終找到最佳策略
        return ExploreOrExploit(state); // 探索 vs 利用的平衡
    }

    public void Learn(double reward) // 根據結果學習
    {
        // reward > 0：這個動作不錯，下次多做
        // reward < 0：這個動作不好，下次少做
        UpdateStrategy(reward); // 更新策略，越來越聰明
    }
}

// 常見應用場景
// 1. AlphaGo：學會下圍棋
// 2. 自動駕駛：學會開車
// 3. ChatGPT 的 RLHF：根據人類回饋學會更好的回答
```

---

## 訓練集 / 驗證集 / 測試集

> 💡 **比喻：考試準備**
> - **訓練集 (Training Set)**：課本和練習題（用來學習的）
> - **驗證集 (Validation Set)**：模擬考（邊學邊測，調整學習方法）
> - **測試集 (Test Set)**：正式考試（學完後才考，評估真正實力）

```csharp
// 資料分割的概念
var allData = LoadData(""dataset.csv""); // 載入所有資料

// 常見的分割比例：70% 訓練、15% 驗證、15% 測試
var trainSet = allData.Take(700);      // 70% 的資料拿來訓練模型
var validSet = allData.Skip(700).Take(150); // 15% 拿來調整超參數
var testSet  = allData.Skip(850);      // 15% 留到最後評估模型效果

// 為什麼要分三份？
// 訓練集：模型從這裡學習規律
// 驗證集：檢查模型有沒有「死記硬背」（過擬合）
// 測試集：最終成績單，模型從沒看過這些資料
```

### 資料分割的重要原則

```csharp
// ❌ 錯誤：用訓練資料來評估模型
model.Train(allData);               // 用所有資料訓練
var score = model.Evaluate(allData); // 用同樣資料評估，成績一定很好
// 這就像考試前先看到考卷答案，考 100 分也不代表你真的會

// ✅ 正確：分開訓練和測試資料
model.Train(trainSet);               // 只用訓練集學習
model.Tune(validSet);                // 用驗證集調整參數
var score = model.Evaluate(testSet); // 用從沒看過的資料評估
// 這才是模型真正的實力
```

---

## 過擬合與欠擬合

> 💡 **比喻：學生的學習狀態**
> - **過擬合 (Overfitting)**：死記硬背型學生，課本題目全對，但換個數字就不會
> - **欠擬合 (Underfitting)**：還沒學會的學生，連課本題目都答錯
> - **剛好 (Good Fit)**：真正理解的學生，新題目也能舉一反三

```csharp
// 用 C# 比喻過擬合
public class OverfitStudent // 過擬合的學生
{
    // 死記所有題目的答案
    private Dictionary<string, string> _memorized = new() // 把答案背下來
    {
        [""1+1=""] = ""2"",      // 背了這題
        [""2+3=""] = ""5"",      // 背了那題
        [""10+20=""] = ""30"",   // 只要是看過的題目都會
    };

    public string Answer(string question) // 回答問題
    {
        if (_memorized.ContainsKey(question)) // 如果是背過的題目
            return _memorized[question];      // 答對！
        return ""不知道"";                     // 新題目就完全不會
    }
}

// 用 C# 比喻剛好的學習
public class GoodFitStudent // 真正理解的學生
{
    public string Answer(string question) // 回答問題
    {
        var numbers = ParseNumbers(question); // 理解題目的結構
        return (numbers[0] + numbers[1]).ToString(); // 用學到的規則計算
        // 即使遇到新的數字組合也能算出答案
    }
}
```

### 如何避免過擬合？

```csharp
// 1. 增加訓練資料量
var moreData = CollectMoreData();   // 資料越多，模型越不容易死記硬背

// 2. 正則化 (Regularization)
var options = new TrainingOptions     // 訓練設定
{
    L2Regularization = 0.01f,         // 懲罰過於複雜的模型
};                                    // 就像告訴學生「不要想太複雜」

// 3. Dropout（隨機丟棄神經元）
var layer = new DropoutLayer(0.5);    // 訓練時隨機關閉 50% 的神經元
// 就像考試時隨機遮住一半的筆記，逼你真正理解

// 4. 早停法 (Early Stopping)
while (training)                       // 持續訓練
{
    if (validationLoss > previousLoss) // 如果驗證集的表現開始變差
        break;                         // 就停止訓練，避免過擬合
}
```

---

## 常見 AI 應用場景

```csharp
// AI 在各領域的應用
var aiApplications = new Dictionary<string, List<string>> // AI 應用場景大全
{
    [""自然語言處理 NLP""] = new()  // 處理人類語言的 AI
    {
        ""ChatGPT / Claude 對話"",  // 聊天機器人
        ""機器翻譯"",               // Google 翻譯
        ""情緒分析"",               // 分析評論是正面還是負面
        ""文件摘要"",               // 自動整理重點
    },
    [""電腦視覺 CV""] = new()      // 處理影像的 AI
    {
        ""人臉辨識"",               // 手機解鎖
        ""自動駕駛"",               // Tesla 的自駕系統
        ""醫療影像診斷"",           // AI 看 X 光片
        ""OCR 文字辨識"",           // 拍照轉文字
    },
    [""推薦系統""] = new()          // 猜你喜歡什麼
    {
        ""Netflix 推薦電影"",       // 根據你看過的推薦
        ""YouTube 推薦影片"",       // 越看越停不下來的原因
        ""電商推薦商品"",           // 「買了這個的人也買了...」
    },
    [""生成式 AI""] = new()         // 創造新內容
    {
        ""文字生成 (GPT, Claude)"", // 寫文章、寫程式
        ""圖片生成 (DALL-E, MJ)"",  // 用文字描述生成圖片
        ""音樂生成 (Suno)"",        // AI 作曲
        ""影片生成 (Sora)"",        // AI 拍電影
    },
};
```

---

## Python vs C# 在 AI 領域的角色

```csharp
// Python：AI 研究和訓練的首選語言
// 優勢：
// - 豐富的 AI 套件（TensorFlow, PyTorch, scikit-learn）
// - 社群資源最多，論文範例幾乎都用 Python
// - 語法簡潔，適合快速實驗

// C#：AI 應用部署和企業整合的好選擇
// 優勢：
// - ML.NET：微軟官方機器學習框架
// - Semantic Kernel：整合 LLM 的框架
// - ONNX Runtime：跑推論超快
// - 企業級 Web API 整合方便

// 用 C# 呼叫 AI 服務的範例
using var client = new HttpClient();              // 建立 HTTP 客戶端
client.DefaultRequestHeaders.Add(                 // 加入 API 金鑰
    ""Authorization"", ""Bearer YOUR_API_KEY"");  // 認證用的標頭

var request = new                                 // 建立請求內容
{
    model = ""claude-sonnet-4-20250514"",             // 指定使用的模型
    max_tokens = 1024,                            // 最多回傳 1024 個 token
    messages = new[]                              // 對話訊息陣列
    {
        new { role = ""user"", content = ""你好"" } // 使用者的訊息
    }
};

var json = JsonSerializer.Serialize(request);     // 把物件轉成 JSON 字串
var content = new StringContent(                  // 建立請求內容
    json, Encoding.UTF8, ""application/json"");   // 指定編碼和格式

var response = await client.PostAsync(            // 發送 POST 請求
    ""https://api.anthropic.com/v1/messages"",    // API 端點
    content);                                     // 請求內容

var result = await response.Content               // 讀取回應
    .ReadAsStringAsync();                         // 轉成字串
Console.WriteLine(result);                        // 印出 AI 的回答
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：以為 AI 就是萬能的

```csharp
// ❌ 初學者常見迷思
// 「AI 可以取代所有工程師」
// 「AI 什麼都能做，只是我不會用」

// ✅ 正確觀念
// AI 是工具，不是魔法
// AI 擅長：模式辨識、大量資料處理、重複性工作
// AI 不擅長：創造性思考、因果推理、理解常識
// 就像計算機很擅長算數，但它不會「理解」數學
```

### ❌ 錯誤 2：混淆 AI、ML、DL 的關係

```csharp
// ❌ 錯誤理解
// 「AI 和 ML 是不同的東西」
// 「深度學習比機器學習更好」

// ✅ 正確理解
// AI ⊃ ML ⊃ DL（包含關係）       // DL 是 ML 的子集，ML 是 AI 的子集
// 不是越深越好，要看問題類型       // 小資料量用深度學習反而會過擬合
// 有時候簡單的線性迴歸就夠用了     // 殺雞不需要用牛刀
```

### ❌ 錯誤 3：忽略資料品質

```csharp
// ❌ 只追求模型，不管資料
var dirtyData = new List<Data>         // 品質很差的資料
{
    new(null, ""正面""),                // 輸入是空的
    new(""很棒"", null),               // 標籤是空的
    new(""asdfjkl"", ""正面""),        // 輸入是亂碼
};
model.Train(dirtyData);                // 垃圾進，垃圾出 (GIGO)

// ✅ 先清理資料再訓練
var cleanData = dirtyData              // 資料清理流程
    .Where(d => d.Input != null)       // 過濾掉空值
    .Where(d => d.Label != null)       // 確保標籤存在
    .Where(d => IsValidText(d.Input))  // 檢查輸入合法性
    .ToList();                         // 乾淨的資料才能訓練好的模型
model.Train(cleanData);               // 資料品質決定模型品質
```
" },

        // ── 大型語言模型 LLM Chapter 531 ────────────────────────────
        new() { Id=531, Category="aimodel", Order=2, Level="intermediate", Icon="💬", Title="大型語言模型 LLM", Slug="large-language-model", IsPublished=true, Content=@"
# 大型語言模型 LLM

## Transformer 架構簡介

> 💡 **比喻：翻譯團隊的注意力機制**
> 想像一個專業翻譯團隊在翻譯一本書：
> - 每個翻譯員（Attention Head）負責注意不同的面向
> - 有人專注「文法結構」，有人專注「情感語氣」，有人專注「專有名詞」
> - 他們同時工作（平行處理），然後把結果合在一起
> - 這就是 **Multi-Head Attention（多頭注意力機制）** 的概念
>
> 之前的 RNN 像是一個翻譯員從頭到尾一個字一個字翻，很慢。
> Transformer 像是一群翻譯員同時看完整本書，然後各自負責不同面向，超快。

### Transformer 的核心組件

```
Transformer 架構（簡化版）
┌─────────────────────────────┐
│          Output             │  // 最終輸出的文字
├─────────────────────────────┤
│    Linear + Softmax         │  // 把向量轉換成機率分布
├─────────────────────────────┤
│  ┌───────────────────────┐  │
│  │   Decoder Block × N   │  │  // 解碼器，負責生成文字
│  │  ┌─────────────────┐  │  │
│  │  │ Masked Self-Attn │  │  │  // 只能看到前面的文字（防作弊）
│  │  │ Cross Attention  │  │  │  // 參考編碼器的結果
│  │  │ Feed Forward     │  │  │  // 全連接層，做進一步處理
│  │  └─────────────────┘  │  │
│  └───────────────────────┘  │
├─────────────────────────────┤
│  ┌───────────────────────┐  │
│  │   Encoder Block × N   │  │  // 編碼器，負責理解輸入
│  │  ┌─────────────────┐  │  │
│  │  │ Self-Attention   │  │  │  // 每個字都看其他所有字
│  │  │ Feed Forward     │  │  │  // 全連接層
│  │  └─────────────────┘  │  │
│  └───────────────────────┘  │
├─────────────────────────────┤
│   Positional Encoding       │  // 告訴模型每個字的位置
├─────────────────────────────┤
│   Input Embedding           │  // 把文字轉成向量
├─────────────────────────────┤
│          Input              │  // 輸入的文字
└─────────────────────────────┘
```

### Self-Attention 自注意力機制

```csharp
// Self-Attention 的直覺理解
// 句子：「小明把蘋果給了小華，因為他很餓」

// 問題：「他」指的是誰？
// Self-Attention 會計算「他」和每個字的關聯度

var attentionScores = new Dictionary<string, double> // 注意力分數
{
    [""小明""] = 0.15,    // 「他」跟「小明」有一些關聯
    [""蘋果""] = 0.02,    // 「他」跟「蘋果」關聯很低
    [""小華""] = 0.75,    // 「他」跟「小華」關聯最高（因為餓→收到蘋果的人）
    [""餓""]   = 0.08,    // 「他」跟「餓」有一點關聯
};
// 模型學會了「他」最可能指「小華」
// 這就是 Attention 的威力：理解上下文關係
```

### Q、K、V 的直覺解釋

```csharp
// Query（查詢）、Key（鍵）、Value（值）
// 比喻：圖書館找書

// Query = 你想找什麼？「我想找關於 C# 的書」
// Key = 每本書的標籤：「C# 入門」「Python 教學」「C# 進階」
// Value = 書的實際內容

// Attention(Q, K, V) = softmax(Q × K^T / √d) × V

var query = ""我想找 C# 的書"";          // 你的需求（Query）
var books = new Dictionary<string, string> // Key → Value 的對應
{
    [""C# 入門""] = ""第一章：變數..."",   // Key 匹配度高 → 取出 Value
    [""Python 教學""] = ""import 語法"",   // Key 匹配度低 → 忽略
    [""C# 進階""] = ""泛型與反射..."",     // Key 匹配度高 → 取出 Value
};
// 最終結果是根據匹配度加權混合所有 Value
```

---

## GPT vs Claude vs LLaMA 比較

```
模型          開發公司      特色                     開源？
──────────────────────────────────────────────────────────
GPT-4o       OpenAI       最廣泛使用的 LLM          否
Claude       Anthropic    注重安全性和誠實           否
LLaMA 3      Meta         開源社群最活躍             是
Gemini       Google       多模態能力強               否
Mistral      Mistral AI   歐洲開源模型，效能好       部分
```

```csharp
// 在 C# 中呼叫不同的 LLM API
// 雖然每家 API 格式不同，但概念相似

// OpenAI GPT
var openaiClient = new OpenAIClient(apiKey);     // 建立 OpenAI 客戶端
var gptResponse = await openaiClient              // 發送請求
    .GetChatCompletionsAsync(                     // 聊天補全 API
        ""gpt-4o"",                               // 指定模型名稱
        new ChatCompletionsOptions                // 設定選項
        {
            Messages = { new ChatMessage(         // 加入訊息
                ChatRole.User, ""你好"") }        // 使用者角色的訊息
        });

// Anthropic Claude
var claudeClient = new AnthropicClient(apiKey);  // 建立 Claude 客戶端
var claudeResponse = await claudeClient           // 發送請求
    .CreateMessageAsync(new()                     // 建立訊息 API
    {
        Model = ""claude-sonnet-4-20250514"",         // 指定模型名稱
        MaxTokens = 1024,                         // 最大回應 token 數
        Messages = new[] {                        // 訊息陣列
            new(""user"", ""你好"") }              // 使用者訊息
    });

// 選擇模型的考量
// 1. 任務類型：程式碼生成、文字分析、多語言翻譯
// 2. 成本：API 計價方式不同
// 3. 回應品質：不同模型各有擅長
// 4. 隱私需求：開源模型可以自己架設
```

---

## Token 與 Tokenization

> 💡 **比喻：樂高積木**
> 把文字想像成樂高，Token 就是一塊一塊的積木：
> - 常見的字就是一塊大積木（例如 ""the"" = 1 token）
> - 少見的字會被拆成小積木（例如 ""tokenization"" = ""token"" + ""ization""）
> - 中文通常一個字 = 1-2 個 token

```csharp
// Token 的概念
var text = ""Hello, how are you?""; // 這句英文大約 6 個 token

// 英文 Tokenization 示例
// ""Hello""  → token_1     // 常見字，一個 token
// "",""      → token_2     // 標點符號，一個 token
// "" how""   → token_3     // 含前面空格，一個 token
// "" are""   → token_4     // 含前面空格，一個 token
// "" you""   → token_5     // 含前面空格，一個 token
// ""?""      → token_6     // 標點符號，一個 token

// 中文 Tokenization 示例
var chineseText = ""今天天氣很好""; // 中文的 token 切法不同
// ""今天""   → token_1     // 兩個字可能合成一個 token
// ""天氣""   → token_2     // 常見詞彙一個 token
// ""很好""   → token_3     // 常見詞彙一個 token

// 為什麼 Token 很重要？
// 1. API 計費是按 token 數計算       // 用越多 token 越貴
// 2. 模型有 token 上限               // 超過上限就無法處理
// 3. 中文比英文花更多 token           // 同樣內容，中文成本稍高
// 4. 程式碼的 token 消耗量很大        // 因為有很多特殊符號

// 估算 token 數量的經驗法則
int EstimateTokens(string text)        // 粗略估算 token 數
{
    var englishWords = text.Split(' ').Length; // 英文大約 1 字 = 1.3 token
    var chineseChars = text.Count(            // 中文大約 1 字 = 1.5 token
        c => c >= 0x4E00 && c <= 0x9FFF);     // 偵測中文字元的 Unicode 範圍
    return (int)(englishWords * 1.3            // 英文的估算
        + chineseChars * 1.5);                 // 中文的估算
}
```

---

## Temperature, Top-P 參數意義

> 💡 **比喻：點餐的隨機程度**
> - **Temperature = 0**：每次都點一樣的（最愛的那道菜）
> - **Temperature = 0.7**：通常點喜歡的，偶爾嘗鮮
> - **Temperature = 1.5**：隨便亂點，可能點到奇怪的東西

```csharp
// Temperature 控制回答的「隨機性」
var stableRequest = new ChatRequest     // 穩定的回答設定
{
    Temperature = 0.0f,                  // 溫度 = 0，每次回答都一樣
    // 適合：寫程式、翻譯、事實性問答    // 需要精確答案的場景
};

var creativeRequest = new ChatRequest   // 有創意的回答設定
{
    Temperature = 0.9f,                  // 溫度 = 0.9，回答更多樣化
    // 適合：寫故事、腦力激盪、創意發想  // 需要多樣性的場景
};

// Top-P (Nucleus Sampling) 控制詞彙選擇的範圍
var request = new ChatRequest           // Top-P 設定
{
    TopP = 0.9f,                         // 只從機率最高的 90% 詞彙中選
    // TopP = 1.0 → 所有詞彙都有機會被選到
    // TopP = 0.1 → 只從最可能的少數詞彙中選
};

// 實際應用中的參數設定建議
var codeGeneration = new ChatRequest    // 程式碼生成
{
    Temperature = 0.0f,                  // 低溫度，確保程式碼正確
    TopP = 1.0f,                         // Top-P 保持預設
};

var creativeWriting = new ChatRequest   // 創意寫作
{
    Temperature = 0.8f,                  // 高溫度，增加創意
    TopP = 0.95f,                        // 稍微限制太離譜的選擇
};

var chatbot = new ChatRequest           // 一般聊天
{
    Temperature = 0.5f,                  // 中等溫度，平衡準確和自然
    TopP = 0.9f,                         // 適當範圍的詞彙選擇
};
```

---

## Context Window 限制

> 💡 **比喻：工作桌面的大小**
> Context Window 就像你的工作桌面：
> - 桌子越大，能同時看的文件越多
> - GPT-4o 的桌子能放 128K token（約一本小說）
> - Claude 的桌子能放 200K token（約兩本小說）
> - 但桌子再大，放太多東西找起來也慢

```csharp
// Context Window 的概念
var contextLimits = new Dictionary<string, int> // 各模型的上下文窗口
{
    [""GPT-4o""]         = 128_000,   // 128K tokens
    [""Claude 3.5""]     = 200_000,   // 200K tokens
    [""LLaMA 3""]        = 128_000,   // 128K tokens
    [""Gemini 1.5 Pro""] = 2_000_000, // 2M tokens（超大！）
};

// Context Window 包含什麼？
// Input tokens（你說的話） + Output tokens（AI 的回答）= 總 token 數
// 如果對話太長，超過 Context Window，就要截斷或摘要

// 管理 Context Window 的策略
public class ConversationManager // 對話管理器
{
    private List<Message> _messages = new();    // 所有訊息歷史
    private int _maxTokens = 128000;           // 模型的 token 上限
    private int _currentTokens = 0;            // 目前已使用的 token 數

    public void AddMessage(Message msg)         // 加入新訊息
    {
        _currentTokens += msg.TokenCount;       // 累計 token 數
        _messages.Add(msg);                     // 加入訊息列表

        while (_currentTokens > _maxTokens * 0.8) // 超過 80% 就開始清理
        {
            var oldest = _messages[1];           // 保留系統訊息，移除最舊的
            _currentTokens -= oldest.TokenCount; // 減少 token 計數
            _messages.RemoveAt(1);               // 移除最舊的使用者訊息
        }
    }
}
```

---

## Fine-tuning vs RAG vs Prompt Engineering

> 💡 **比喻：三種讓 AI 變聰明的方式**
> - **Prompt Engineering**：問對問題（不改變 AI，改變你問問題的方式）
> - **RAG**：開卷考試（給 AI 一本參考書讓它查）
> - **Fine-tuning**：送 AI 去補習班（修改 AI 本身的知識）

```csharp
// 1. Prompt Engineering（提示工程）
// 成本：最低 | 速度：最快 | 效果：中等
var prompt = @""你是一個專業的 C# 程式教師。  // 設定角色
請用繁體中文回答。                            // 設定語言
回答時請附上程式碼範例。                       // 設定格式
如果不確定，請說「我不確定」。"";              // 設定行為準則

// 2. RAG（檢索增強生成）
// 成本：中等 | 速度：中等 | 效果：高（特定領域）
// 不修改模型，而是給模型額外的參考資料
var context = SearchVectorDB(userQuestion);    // 從向量資料庫搜尋相關資料
var ragPrompt = $""根據以下資料回答：\n{context}\n問題：{userQuestion}"";
// 優點：資料可以即時更新，不需要重新訓練模型

// 3. Fine-tuning（微調）
// 成本：最高 | 速度：需要訓練時間 | 效果：高（風格和格式）
// 修改模型的權重，讓它學習新的知識或風格
var trainingData = new List<FineTuneExample>    // 準備微調資料
{
    new(""如何宣告變數？"",                      // 輸入
        ""在 C# 中使用 var 或型別名稱...""),      // 期望的輸出格式
};
await client.CreateFineTuneJob(trainingData);   // 送出微調訓練任務

// 如何選擇？
// ┌──────────────────┬─────────────┬────────┬──────────┐
// │ 方法             │ 成本        │ 難度   │ 適用場景 │
// ├──────────────────┼─────────────┼────────┼──────────┤
// │ Prompt Eng.      │ 幾乎免費    │ 低     │ 一般任務 │
// │ RAG              │ 中（需DB）  │ 中     │ 知識問答 │
// │ Fine-tuning      │ 高（需GPU） │ 高     │ 特殊風格 │
// └──────────────────┴─────────────┴────────┴──────────┘
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：Temperature 設太高導致程式碼出錯

```csharp
// ❌ 用高 Temperature 生成程式碼
var request = new ChatRequest             // 錯誤的設定
{
    Prompt = ""寫一個排序演算法"",          // 需要精確的程式碼
    Temperature = 1.5f,                    // 溫度太高，回答太隨機
};
// 結果：可能生成語法錯誤或邏輯錯誤的程式碼

// ✅ 程式碼生成應該用低 Temperature
var betterRequest = new ChatRequest       // 正確的設定
{
    Prompt = ""寫一個排序演算法"",          // 同樣的需求
    Temperature = 0.0f,                    // 低溫度確保準確性
};
// 結果：穩定、正確的程式碼
```

### ❌ 錯誤 2：把所有資料都塞進 Prompt

```csharp
// ❌ 嘗試把整個資料庫放進 prompt
var hugePrompt = $@""             // 超大的 prompt
以下是我們公司的所有資料：
{entireDatabase}                  // 可能有幾百萬筆資料
請回答：{question}"";             // 一定會超過 Context Window

// ✅ 應該用 RAG 只取相關的資料
var relevantDocs = await vectorDb  // 向量資料庫搜尋
    .SearchAsync(question, top: 5); // 只取最相關的 5 筆

var smartPrompt = $@""             // 精簡的 prompt
參考資料：{string.Join(""\n"", relevantDocs)} // 只放相關資料
請回答：{question}"";              // 不會超過 Context Window
```

### ❌ 錯誤 3：以為 Fine-tuning 能讓模型學新知識

```csharp
// ❌ 錯誤觀念：Fine-tuning 是教模型新知識
// 很多人以為 fine-tuning 可以讓 GPT 學會你的產品文件
// 實際上 fine-tuning 更適合調整「回答的風格和格式」

// ✅ 正確用法
// 學新知識 → 用 RAG（檢索增強生成）    // 把文件放向量資料庫
// 調整風格 → 用 Fine-tuning             // 例如讓模型用特定語氣回答
// 簡單指引 → 用 Prompt Engineering       // 在 prompt 中給明確指示
```
" },

        // ── RAG 檢索增強生成 Chapter 532 ────────────────────────────
        new() { Id=532, Category="aimodel", Order=3, Level="intermediate", Icon="📚", Title="RAG 檢索增強生成", Slug="rag-retrieval-augmented-generation", IsPublished=true, Content=@"
# RAG 檢索增強生成

## 為什麼需要 RAG？

> 💡 **比喻：開卷考試 vs 閉卷考試**
> - **沒有 RAG 的 LLM**：閉卷考試，只能靠訓練時記住的知識回答
>   - 問它最新的產品價格？不知道（訓練資料太舊）
>   - 問它公司內部的規定？不知道（沒學過）
>   - 問它昨天的新聞？不知道（知識有截止日期）
>
> - **有 RAG 的 LLM**：開卷考試，可以翻書查資料再回答
>   - 問任何問題，先從資料庫搜尋相關文件
>   - 把找到的資料放進 prompt 中
>   - LLM 根據這些資料生成回答

### RAG 解決了什麼問題？

```csharp
// 問題 1：知識截止日期
// LLM 的訓練資料有截止日期
var llmKnowledge = new LLMInfo          // LLM 的知識範圍
{
    TrainingCutoff = ""2024-04"",        // 訓練資料截止到 2024 年 4 月
    // 2024 年 5 月之後的事都不知道     // 包括新產品、新法規、新技術
};

// 問題 2：缺乏私有知識
// LLM 不知道你公司的內部資料
var question = ""我們公司的請假流程是什麼？""; // LLM 答不出來
// 因為訓練資料裡沒有你公司的員工手冊          // 這是私有資訊

// 問題 3：幻覺 (Hallucination)
// LLM 可能會「編造」看起來合理但錯誤的答案
// RAG 提供了參考資料，大幅降低幻覺的可能性

// RAG 的解法：把相關資料餵給 LLM
var ragAnswer = await AskWithRAG(question);    // 先搜尋再回答
// 1. 搜尋向量資料庫，找到「員工手冊.pdf」     // 找到相關文件
// 2. 取出相關段落放進 prompt                   // 提供參考資料
// 3. LLM 根據這些資料生成正確回答              // 有依據的回答
```

---

## Embedding 向量化概念

> 💡 **比喻：GPS 座標**
> 想像每個詞都有一個 GPS 座標：
> - 「貓」的座標是 (3.2, 1.5, 7.8, ...)
> - 「狗」的座標是 (3.1, 1.6, 7.9, ...)（很接近，因為都是寵物）
> - 「汽車」的座標是 (8.5, 4.2, 1.1, ...)（很遠，因為完全不同類別）
>
> 意思相近的詞，座標就很接近。這就是 Embedding。

```csharp
// Embedding 把文字轉成數字向量
// 向量的維度通常是 768 或 1536（很多數字組成一個座標）

var embeddingService = new EmbeddingService();   // 嵌入服務

// 把文字轉成向量
var vec1 = await embeddingService                // 文字 → 向量
    .EmbedAsync(""如何在 C# 中使用 LINQ？"");    // 關於 C# LINQ 的問題
// vec1 = [0.023, -0.156, 0.892, ...]            // 1536 維的向量

var vec2 = await embeddingService                // 文字 → 向量
    .EmbedAsync(""C# 的 LINQ 查詢語法"");         // 類似的問題
// vec2 = [0.025, -0.148, 0.885, ...]            // 跟 vec1 很接近！

var vec3 = await embeddingService                // 文字 → 向量
    .EmbedAsync(""今天晚餐吃什麼？"");             // 完全不同的問題
// vec3 = [0.754, 0.332, -0.102, ...]            // 跟 vec1 差很遠

// 計算相似度（餘弦相似度）
double CosineSimilarity(                          // 計算兩個向量的相似度
    float[] a, float[] b)                         // 輸入兩個向量
{
    var dotProduct = a.Zip(b,                     // 對應元素相乘
        (x, y) => x * y).Sum();                   // 然後加總
    var magnitudeA = Math.Sqrt(                   // 計算向量 A 的長度
        a.Sum(x => x * x));                       // 各元素平方和開根號
    var magnitudeB = Math.Sqrt(                   // 計算向量 B 的長度
        b.Sum(x => x * x));                       // 各元素平方和開根號
    return dotProduct /                            // 內積除以
        (magnitudeA * magnitudeB);                 // 兩個長度的乘積
}

// CosineSimilarity(vec1, vec2) ≈ 0.95  → 非常相似！
// CosineSimilarity(vec1, vec3) ≈ 0.12  → 完全不同
```

---

## 向量資料庫

> 💡 **比喻：智慧型圖書館**
> 傳統資料庫像是用書名或作者找書（精確搜尋）。
> 向量資料庫像是說「我想找關於貓咪健康的書」，
> 圖書館員會找出所有相關的書，不管標題有沒有「貓」這個字。

```csharp
// 常見的向量資料庫比較
// ┌──────────────┬────────────────┬───────────────┬──────────┐
// │ 向量資料庫   │ 特色           │ 部署方式      │ 適合場景 │
// ├──────────────┼────────────────┼───────────────┼──────────┤
// │ Pinecone     │ 全託管，簡單   │ 雲端 SaaS     │ 快速上手 │
// │ Qdrant       │ 開源，效能好   │ 自架或雲端    │ 進階使用 │
// │ ChromaDB     │ 超簡單，適合學 │ 本地嵌入式    │ 原型開發 │
// │ Weaviate     │ 功能豐富       │ 自架或雲端    │ 企業級   │
// │ pgvector     │ PostgreSQL 擴充│ 現有 PG 資料庫│ 已用 PG  │
// └──────────────┴────────────────┴───────────────┴──────────┘

// 使用 Qdrant 的 C# 範例
using Qdrant.Client;                              // 引用 Qdrant 套件
using Qdrant.Client.Grpc;                         // gRPC 通訊協定

var qdrantClient = new QdrantClient(""localhost""); // 連接到本地 Qdrant

// 建立 Collection（類似資料庫的 Table）
await qdrantClient.CreateCollectionAsync(          // 建立集合
    ""my_documents"",                               // 集合名稱
    new VectorParams                                // 向量參數設定
    {
        Size = 1536,                                // 向量維度（配合嵌入模型）
        Distance = Distance.Cosine                  // 使用餘弦相似度
    });

// 插入文件向量
await qdrantClient.UpsertAsync(                    // 新增或更新向量
    ""my_documents"",                               // 集合名稱
    new List<PointStruct>                           // 資料點列表
    {
        new()                                       // 一個文件的向量
        {
            Id = 1,                                 // 文件 ID
            Vectors = embeddingVector,              // 嵌入向量
            Payload =                               // 附加資料（原文）
            {
                [""text""] = ""C# 是一種強型別語言..."", // 原始文字
                [""source""] = ""csharp-guide.pdf""      // 來源檔案
            }
        }
    });

// 搜尋相似文件
var searchResult = await qdrantClient               // 執行相似度搜尋
    .SearchAsync(""my_documents"",                   // 在這個集合中搜尋
        queryVector,                                 // 查詢向量
        limit: 5);                                   // 取前 5 筆最相似的
```

---

## RAG Pipeline：切割 → 嵌入 → 檢索 → 生成

```
RAG 完整流程
┌─────────────────────────────────────────────────────────┐
│ 離線階段（準備資料）                                      │
│                                                          │
│  文件 ──→ 切割成 Chunk ──→ Embedding ──→ 存入向量 DB    │
│  📄          ✂️              🔢              💾          │
│                                                          │
├─────────────────────────────────────────────────────────┤
│ 線上階段（回答問題）                                      │
│                                                          │
│  使用者問題 ──→ Embedding ──→ 搜尋向量 DB ──→ 取回相關文件│
│  ❓              🔢              🔍              📋      │
│                                                          │
│  相關文件 + 問題 ──→ 組合 Prompt ──→ LLM 生成回答       │
│  📋    ❓              📝              🤖   💬          │
└─────────────────────────────────────────────────────────┘
```

```csharp
// 完整 RAG Pipeline 的 C# 實作概念
public class RagPipeline // RAG 管線
{
    private readonly IEmbeddingService _embedding; // 嵌入服務
    private readonly IVectorStore _vectorStore;     // 向量資料庫
    private readonly ILlmService _llm;              // LLM 服務

    // 步驟 1：切割文件成小段落
    public List<string> ChunkDocument(             // 切割文件的方法
        string document, int chunkSize = 500)      // 每段預設 500 字元
    {
        var chunks = new List<string>();            // 儲存切割後的段落
        for (int i = 0; i < document.Length;        // 從頭到尾遍歷文件
             i += chunkSize)                        // 每次跳一個 chunk 的大小
        {
            var chunk = document.Substring(         // 取出一段文字
                i, Math.Min(chunkSize,              // 取 chunkSize 或剩餘長度
                document.Length - i));               // 避免超出範圍
            chunks.Add(chunk);                      // 加入列表
        }
        return chunks;                              // 回傳所有段落
    }

    // 步驟 2：嵌入並存入向量資料庫
    public async Task IndexDocumentAsync(           // 索引文件的方法
        string document, string source)             // 文件內容和來源
    {
        var chunks = ChunkDocument(document);       // 先切割文件
        foreach (var chunk in chunks)               // 對每個段落
        {
            var vector = await _embedding           // 轉成向量
                .EmbedAsync(chunk);                  // 呼叫嵌入 API
            await _vectorStore.StoreAsync(           // 存入向量資料庫
                vector, chunk, source);              // 向量、原文、來源
        }
    }

    // 步驟 3：檢索相關文件
    public async Task<List<string>> RetrieveAsync(  // 檢索的方法
        string question, int topK = 5)              // 問題和取回數量
    {
        var queryVector = await _embedding           // 把問題轉成向量
            .EmbedAsync(question);                    // 呼叫嵌入 API
        var results = await _vectorStore              // 搜尋向量資料庫
            .SearchAsync(queryVector, topK);           // 取最相似的 topK 筆
        return results.Select(r => r.Text)            // 取出原文
            .ToList();                                 // 轉成列表
    }

    // 步驟 4：組合 Prompt 並生成回答
    public async Task<string> AskAsync(              // 完整的 RAG 問答
        string question)                              // 使用者的問題
    {
        var relevantDocs = await RetrieveAsync(       // 先檢索相關文件
            question);                                 // 用問題去搜尋
        var context = string.Join(""\n\n"",            // 把文件合併
            relevantDocs);                              // 用換行分隔

        var prompt = $@""根據以下參考資料回答問題。      // 組合 prompt
如果資料中沒有提到，請說「根據現有資料無法回答」。

參考資料：
{context}

問題：{question}

請用繁體中文回答："";                                   // 指定語言

        return await _llm.GenerateAsync(prompt);       // 呼叫 LLM 生成回答
    }
}
```

---

## Chunk 策略

> 💡 **比喻：切蛋糕**
> - **固定大小切割**：每隔 5 公分切一刀（簡單但可能切到裝飾）
> - **語意切割**：沿著蛋糕的分層線切（保留完整的口味層次）

```csharp
// 策略 1：固定大小切割 (Fixed-size Chunking)
public List<string> FixedSizeChunk(               // 固定大小切割
    string text, int size = 500, int overlap = 50) // 大小 500，重疊 50
{
    var chunks = new List<string>();                // 結果列表
    for (int i = 0; i < text.Length;                // 從頭開始
         i += size - overlap)                       // 每次前進 size - overlap
    {
        var end = Math.Min(i + size, text.Length);  // 計算結束位置
        chunks.Add(text.Substring(i, end - i));     // 取出一段
    }
    return chunks;                                  // 回傳所有段落
}
// 優點：簡單快速
// 缺點：可能在句子中間切斷，破壞語意

// 策略 2：語意切割 (Semantic Chunking)
public List<string> SemanticChunk(                 // 語意切割
    string text)                                    // 輸入文字
{
    var paragraphs = text.Split(                    // 先按段落分割
        new[] { ""\n\n"" },                          // 雙換行通常是段落分界
        StringSplitOptions.RemoveEmptyEntries);      // 移除空段落

    var chunks = new List<string>();                 // 結果列表
    var current = new StringBuilder();               // 目前的 chunk

    foreach (var para in paragraphs)                 // 遍歷每個段落
    {
        if (current.Length + para.Length > 800)       // 如果加了會太長
        {
            chunks.Add(current.ToString());           // 先存目前的 chunk
            current.Clear();                          // 清空重來
        }
        current.AppendLine(para);                     // 加入段落
    }

    if (current.Length > 0)                           // 處理最後剩餘的
        chunks.Add(current.ToString());               // 加入最後一段
    return chunks;                                    // 回傳所有段落
}
// 優點：保留完整語意
// 缺點：chunk 大小不一致

// 策略 3：遞迴切割 (Recursive Chunking)
// 先按大段落切 → 太長的按小段落切 → 還太長的按句子切
// 這是 LangChain 預設的策略
var separators = new[] { ""\n\n"", ""\n"", ""。"", ""，"", "" "" }; // 分隔符優先級
// 先嘗試用雙換行切，不夠再用單換行，再不夠用句號...
```

---

## C# 實作 RAG 範例 (Semantic Kernel)

```csharp
// 使用 Microsoft Semantic Kernel 實作 RAG
// 安裝套件：
// dotnet add package Microsoft.SemanticKernel
// dotnet add package Microsoft.SemanticKernel.Connectors.OpenAI

using Microsoft.SemanticKernel;                    // Semantic Kernel 核心
using Microsoft.SemanticKernel.Memory;             // 記憶功能
using Microsoft.SemanticKernel.Connectors.OpenAI;  // OpenAI 連接器

// 建立 Kernel
var builder = Kernel.CreateBuilder();               // 建立 Kernel 建構器
builder.AddOpenAIChatCompletion(                    // 加入 OpenAI 聊天模型
    ""gpt-4o"",                                     // 模型名稱
    ""your-api-key"");                               // API 金鑰

var kernel = builder.Build();                       // 建立 Kernel 實例

// 建立記憶儲存
var memoryBuilder = new MemoryBuilder();             // 記憶建構器
memoryBuilder.WithOpenAITextEmbeddingGeneration(     // 加入嵌入模型
    ""text-embedding-3-small"",                       // 嵌入模型名稱
    ""your-api-key"");                                // API 金鑰
memoryBuilder.WithMemoryStore(                       // 記憶儲存
    new VolatileMemoryStore());                       // 使用記憶體儲存（開發用）

var memory = memoryBuilder.Build();                   // 建立記憶實例

// 步驟 1：將文件存入記憶
var documents = new Dictionary<string, string>        // 準備文件
{
    [""doc1""] = ""C# 的 var 關鍵字用於隱式型別推斷..."",  // 第一份文件
    [""doc2""] = ""LINQ 提供了統一的資料查詢語法..."",     // 第二份文件
    [""doc3""] = ""async/await 是 C# 的非同步程式模式..."", // 第三份文件
};

foreach (var doc in documents)                         // 逐一處理文件
{
    await memory.SaveInformationAsync(                 // 存入記憶
        ""csharp-docs"",                                // 集合名稱
        doc.Value,                                      // 文件內容
        doc.Key);                                       // 文件 ID
}

// 步驟 2：搜尋相關文件
var question = ""如何使用非同步程式設計？"";              // 使用者的問題
var searchResults = memory.SearchAsync(                 // 搜尋記憶
    ""csharp-docs"",                                    // 集合名稱
    question,                                           // 搜尋查詢
    limit: 3);                                          // 取前 3 筆

var context = new StringBuilder();                      // 組合搜尋結果
await foreach (var result in searchResults)              // 非同步遍歷結果
{
    context.AppendLine(result.Metadata.Text);           // 加入原文
}

// 步驟 3：組合 Prompt 並生成回答
var ragPrompt = $@""你是一個 C# 程式教師。               // 設定角色
根據以下參考資料回答問題。

參考資料：
{context}

問題：{question}

請用繁體中文、初學者友善的方式回答："";                   // 設定風格

var result = await kernel.InvokePromptAsync(            // 呼叫 LLM
    ragPrompt);                                          // 傳入完整 prompt
Console.WriteLine(result);                               // 印出回答
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：Chunk 太大或太小

```csharp
// ❌ Chunk 太大（例如整份文件不切）
var bigChunk = entireDocument;          // 整份文件當一個 chunk
// 問題：搜尋時會取回太多不相關的內容
// LLM 的 Context Window 也可能放不下

// ❌ Chunk 太小（例如一句話一個）
var tinyChunks = document.Split('。');  // 每句話一個 chunk
// 問題：缺少上下文，LLM 看不懂前後關係

// ✅ 適當大小，保留重疊
var goodChunks = ChunkWithOverlap(      // 適當切割
    document,                            // 輸入文件
    chunkSize: 500,                      // 每段 500 字
    overlap: 50);                        // 重疊 50 字，保留上下文
```

### ❌ 錯誤 2：沒有處理搜尋結果為空的情況

```csharp
// ❌ 假設一定能找到相關文件
var docs = await SearchAsync(question);  // 搜尋
var prompt = $""根據 {docs} 回答"";       // 直接用，沒檢查
// 如果 docs 是空的，LLM 會亂回答（幻覺）

// ✅ 檢查搜尋結果
var docs = await SearchAsync(question);   // 搜尋
if (docs.Count == 0 ||                    // 沒有找到結果
    docs.All(d => d.Score < 0.5))          // 或相似度都太低
{
    return ""根據現有資料無法回答此問題。"";  // 誠實告知
}
```

### ❌ 錯誤 3：忽略 Embedding 模型的選擇

```csharp
// ❌ 索引和搜尋用不同的 Embedding 模型
// 索引時用 text-embedding-3-small
await IndexWithModel(""text-embedding-3-small"", docs);  // 用 A 模型建索引

// 搜尋時用 text-embedding-ada-002
var results = await SearchWithModel(                      // 用 B 模型搜尋
    ""text-embedding-ada-002"", question);                 // 向量維度不同！
// 結果：完全搜不到東西，因為向量空間不一致

// ✅ 索引和搜尋必須用相同的 Embedding 模型
const string MODEL = ""text-embedding-3-small"";          // 統一使用同一模型
await IndexWithModel(MODEL, docs);                         // 建索引
var results = await SearchWithModel(MODEL, question);      // 搜尋
// 確保向量空間一致，搜尋才有意義
```
" },

        // ── IPAS 資訊技術相關知識 Chapter 533 ────────────────────────────
        new() { Id=533, Category="aimodel", Order=4, Level="beginner", Icon="📋", Title="IPAS 資訊技術相關知識", Slug="ipas-information-technology", IsPublished=true, Content=@"
# IPAS 資訊技術相關知識

## IPAS 考試簡介與準備方向

> 💡 **比喻：資訊界的駕照**
> IPAS 就像是資訊領域的「駕照」：
> - 證明你有基本的資訊技術能力
> - 企業在招聘時會參考這張證照
> - 分為「初級」和「中級」兩種，就像普通駕照和職業駕照

```
IPAS 經濟部產業人才能力鑑定
┌──────────────────────────────────────────────┐
│ 資訊技術應用類                                 │
│                                               │
│  初級能力鑑定                                  │
│  ├── 資訊技術應用                              │
│  │   ├── 資訊安全概論                          │
│  │   ├── 軟體工程概論                          │
│  │   ├── 資料庫概論                            │
│  │   └── 網路概論                              │
│  │                                             │
│  中級能力鑑定                                   │
│  ├── 進階資訊技術應用                           │
│  │   ├── 資訊安全實務                           │
│  │   ├── 軟體開發與測試                         │
│  │   ├── 資料庫管理                             │
│  │   └── 網路管理                               │
└──────────────────────────────────────────────┘
```

```csharp
// IPAS 考試準備策略
var examPrep = new ExamStrategy                    // 考試準備策略
{
    ExamName = ""IPAS 資訊技術應用"",               // 考試名稱
    Duration = ""90 分鐘"",                          // 考試時間
    QuestionType = ""選擇題"",                       // 題型：全部選擇題
    PassingScore = 60,                               // 及格分數 60 分
    Tips = new List<string>                          // 準備建議
    {
        ""每個領域都要讀，不能只挑喜歡的"",           // 四個領域平均出題
        ""歷屆試題一定要做，至少做三年份"",           // 考古題非常重要
        ""概念理解比死背重要"",                       // 理解原理才能應變
        ""計算題要練熟（子網路遮罩、正規化）"",       // 計算題是拿分關鍵
    }
};
```

---

## 資訊安全基礎（CIA 三要素）

> 💡 **比喻：保管箱的三個特性**
> - **機密性 (Confidentiality)**：只有你有鑰匙能打開（防偷看）
> - **完整性 (Integrity)**：箱子裡的東西不能被偷偷換掉（防竄改）
> - **可用性 (Availability)**：你想用的時候隨時能打開（防故障）

```csharp
// CIA 三要素是資訊安全的核心
public class InformationSecurity // 資訊安全基礎
{
    // 機密性 (Confidentiality)：防止未授權存取
    public void ProtectConfidentiality()           // 保護機密性
    {
        // 加密：把資料變成看不懂的密文
        var encrypted = Encrypt(data, key);        // 加密資料
        // HTTPS：網路傳輸加密
        var secureUrl = ""https://example.com"";    // 使用 HTTPS 協定
        // 存取控制：只有授權人員能看
        if (!user.HasPermission(""read""))          // 檢查使用者權限
            throw new UnauthorizedException();      // 沒有權限就拒絕
    }

    // 完整性 (Integrity)：確保資料沒被竄改
    public void ProtectIntegrity()                  // 保護完整性
    {
        // 雜湊：計算資料的「指紋」
        var hash = SHA256.HashData(data);           // 計算雜湊值
        // 數位簽章：證明資料來源可信
        var signature = SignData(data, privateKey); // 用私鑰簽名
        // 版本控制：追蹤所有修改記錄
        git.Commit(""修改了設定檔"");                // 用 Git 追蹤變更
    }

    // 可用性 (Availability)：確保服務隨時可用
    public void ProtectAvailability()               // 保護可用性
    {
        // 備份：定期備份資料
        BackupDatabase(""daily"");                   // 每日備份
        // 負載均衡：分散流量
        var lb = new LoadBalancer(servers);          // 負載均衡器
        // 災難復原：有備援方案
        var drPlan = new DisasterRecoveryPlan();     // 災難復原計畫
    }
}
```

### 常見的資安威脅

```csharp
// IPAS 考試常考的資安威脅類型
var securityThreats = new Dictionary<string, string> // 資安威脅對照表
{
    // 社交工程攻擊（針對人的弱點）
    [""釣魚攻擊 Phishing""]    = ""假冒銀行寄信騙你輸入密碼"",     // 最常見的攻擊
    [""魚叉式釣魚 Spear""]     = ""針對特定人物的釣魚攻擊"",       // 更精準的攻擊
    [""社交工程 Social Eng.""]  = ""假裝是 IT 人員騙你給密碼"",     // 利用人性弱點

    // 惡意程式（針對電腦的攻擊）
    [""病毒 Virus""]           = ""附著在程式上，會自我複製"",      // 需要宿主程式
    [""蠕蟲 Worm""]            = ""不需宿主，自動透過網路傳播"",    // 不需要人操作
    [""木馬 Trojan""]          = ""偽裝成正常程式的惡意軟體"",      // 偽裝成有用程式
    [""勒索軟體 Ransomware""]  = ""加密你的檔案，要你付贖金"",     // 近年最猖獗

    // 網路攻擊
    [""DDoS""]                 = ""大量假流量癱瘓你的伺服器"",      // 分散式阻斷攻擊
    [""SQL Injection""]        = ""在輸入框注入 SQL 指令"",         // 注入攻擊
    [""XSS""]                  = ""在網頁注入惡意 JavaScript"",     // 跨站腳本攻擊
    [""MITM""]                 = ""中間人攻擊，竊聽你的通訊"",      // 中間人攻擊
};
```

### 加密基礎概念

```csharp
// 對稱加密 vs 非對稱加密
// 對稱加密：同一把鑰匙加密和解密
var key = GenerateKey();                            // 產生一把鑰匙
var encrypted = AES.Encrypt(data, key);             // 用鑰匙加密
var decrypted = AES.Decrypt(encrypted, key);        // 用同一把鑰匙解密
// 優點：速度快 | 缺點：鑰匙怎麼安全地給對方？

// 非對稱加密：公鑰加密，私鑰解密
var (publicKey, privateKey) = RSA.GenerateKeyPair(); // 產生一對鑰匙
var encrypted = RSA.Encrypt(data, publicKey);        // 用公鑰加密（公開）
var decrypted = RSA.Decrypt(encrypted, privateKey);  // 用私鑰解密（保密）
// 優點：不用交換私鑰 | 缺點：速度慢

// 雜湊 (Hash)：不可逆的「指紋」
var hash = SHA256.ComputeHash(password);             // 密碼雜湊
// 特性：同樣輸入一定得到同樣輸出                      // 確定性
// 特性：無法從 hash 反推原始資料                       // 不可逆性
// 特性：輸入差一點點，輸出差很多                       // 雪崩效應
```

---

## 軟體工程基礎（SDLC、敏捷開發）

> 💡 **比喻：蓋房子的不同方式**
> - **瀑布式開發**：先畫完所有設計圖 → 打地基 → 蓋結構 → 裝潢（一步一步來）
> - **敏捷開發**：先蓋一間小木屋能住 → 再加房間 → 再加車庫（邊蓋邊改）

### SDLC 軟體開發生命週期

```
瀑布式模型 (Waterfall)
┌──────────┐
│ 需求分析  │ → 了解客戶要什麼
├──────────┤
│ 系統設計  │ → 決定怎麼做
├──────────┤
│ 實作開發  │ → 寫程式
├──────────┤
│ 測試驗證  │ → 找 Bug
├──────────┤
│ 部署上線  │ → 給客戶用
├──────────┤
│ 維護運營  │ → 持續修 Bug
└──────────┘
每個階段完成才能進入下一階段
```

```csharp
// SDLC 各階段的工作
var sdlcPhases = new Dictionary<string, List<string>> // SDLC 各階段工作
{
    [""需求分析""] = new()                              // 第一階段
    {
        ""訪談客戶，了解需求"",                          // 知道客戶要什麼
        ""撰寫需求規格書 (SRS)"",                       // 正式文件化
        ""確認可行性（技術、成本、時間）"",               // 評估做不做得到
    },
    [""系統設計""] = new()                              // 第二階段
    {
        ""架構設計（前後端分離？微服務？）"",             // 決定大方向
        ""資料庫設計（ER 圖、正規化）"",                  // 設計資料結構
        ""UI/UX 設計（Wireframe、Mockup）"",             // 設計使用者介面
    },
    [""實作開發""] = new()                              // 第三階段
    {
        ""寫程式碼"",                                    // 動手寫
        ""程式碼審查 (Code Review)"",                    // 同事互相檢查
        ""版本控制 (Git)"",                               // 追蹤所有修改
    },
    [""測試驗證""] = new()                              // 第四階段
    {
        ""單元測試 (Unit Test)"",                         // 測試個別功能
        ""整合測試 (Integration Test)"",                  // 測試組合功能
        ""使用者驗收測試 (UAT)"",                         // 客戶確認OK
    },
};
```

### 敏捷開發 (Agile / Scrum)

```csharp
// Scrum 框架的核心概念
public class ScrumFramework // Scrum 敏捷開發框架
{
    // 三個角色
    public string ProductOwner = ""產品負責人"";    // 決定「做什麼」
    public string ScrumMaster  = ""Scrum 教練"";    // 確保流程順暢
    public string DevTeam      = ""開發團隊"";       // 負責「怎麼做」

    // Sprint：2-4 週的開發迭代
    public class Sprint                             // 一次衝刺
    {
        public int DurationWeeks = 2;               // 通常 2 週一個 Sprint
        public List<string> BacklogItems;            // 這次要完成的工作
        public string Goal;                          // 這次 Sprint 的目標
    }

    // 四個會議
    public void SprintPlanning() { }                // 計畫會議：決定做什麼
    public void DailyStandup() { }                  // 每日站會：15 分鐘同步進度
    public void SprintReview() { }                  // 檢視會議：展示成果給客戶
    public void SprintRetro() { }                   // 回顧會議：團隊自我改善
}

// 瀑布式 vs 敏捷的比較
// ┌─────────────┬──────────────┬──────────────────┐
// │ 特性        │ 瀑布式       │ 敏捷式           │
// ├─────────────┼──────────────┼──────────────────┤
// │ 需求變更    │ 困難且昂貴   │ 歡迎變更         │
// │ 交付頻率    │ 最後一次交付 │ 每 2-4 週交付    │
// │ 客戶參與    │ 開頭和結尾   │ 全程參與         │
// │ 文件量      │ 大量文件     │ 夠用就好         │
// │ 風險        │ 後期才發現   │ 早期就發現       │
// │ 適合情境    │ 需求明確     │ 需求常變動       │
// └─────────────┴──────────────┴──────────────────┘
```

---

## 資料庫基礎概念（正規化、ER 圖）

> 💡 **比喻：整理衣櫃**
> - **未正規化**：所有衣服丟在一個大箱子裡（找東西很慢，重複的很多）
> - **正規化**：分類放好（上衣一區、褲子一區、配件一區，用標籤連結）

### 正規化 (Normalization)

```csharp
// 未正規化的資料（很多重複）
// ┌────┬──────┬────────┬──────────┬────────────┐
// │學號│ 姓名 │ 課程1  │ 課程2    │ 老師       │
// ├────┼──────┼────────┼──────────┼────────────┤
// │001 │ 小明 │ C#入門 │ 資料庫   │ 王老師     │
// │002 │ 小華 │ C#入門 │ 網路概論 │ 王老師     │
// └────┴──────┴────────┴──────────┴────────────┘
// 問題：「C#入門」重複出現，「王老師」也重複
// 如果王老師改名，要改很多地方

// 第一正規化 (1NF)：消除重複群組
// 規則：每個欄位只能有一個值（原子值）
var firstNF = new List<Enrollment>                  // 一筆一筆記錄
{
    new(StudentId: ""001"", Name: ""小明"",          // 學生資訊
        Course: ""C#入門"", Teacher: ""王老師""),     // 一門課一筆
    new(StudentId: ""001"", Name: ""小明"",          // 同一個學生
        Course: ""資料庫"", Teacher: ""李老師""),     // 另一門課另一筆
};

// 第二正規化 (2NF)：消除部分相依
// 規則：非主鍵欄位必須完全依賴主鍵
// 把「學生」和「選課」分開

// 第三正規化 (3NF)：消除遞移相依
// 規則：非主鍵欄位不能依賴其他非主鍵欄位
// 把「課程」和「老師」也分開

// 正規化後的結構
public class Student                                // 學生資料表
{
    public string StudentId { get; set; }            // 主鍵：學號
    public string Name { get; set; }                 // 姓名
}

public class Course                                  // 課程資料表
{
    public string CourseId { get; set; }              // 主鍵：課程編號
    public string CourseName { get; set; }            // 課程名稱
    public string TeacherId { get; set; }             // 外鍵：老師編號
}

public class Enrollment                              // 選課資料表（關聯表）
{
    public string StudentId { get; set; }             // 外鍵：學號
    public string CourseId { get; set; }              // 外鍵：課程編號
}
// 優點：減少重複、方便維護、避免異常
```

### ER 圖 (Entity-Relationship Diagram)

```
ER 圖的基本元素
┌─────────────┐       ┌─────────────┐
│   學生       │       │   課程       │
│ (Entity)    │       │ (Entity)    │
├─────────────┤       ├─────────────┤
│ *學號 (PK)  │       │ *課程編號(PK)│
│  姓名       │       │  課程名稱    │
│  電話       │       │  學分數      │
└──────┬──────┘       └──────┬──────┘
       │   N               M │
       │    ┌──────────┐     │
       └────┤  選課     ├─────┘
            │(Relation) │
            ├──────────┤
            │ 成績      │
            └──────────┘

關係類型：
1:1  一對一（一個人有一個身分證）
1:N  一對多（一個老師教多門課）
M:N  多對多（多個學生選多門課）→ 需要關聯表
```

```csharp
// 用 Entity Framework 實現 ER 圖的關係
public class Teacher                                 // 老師實體
{
    public int Id { get; set; }                      // 主鍵
    public string Name { get; set; }                 // 姓名
    public List<Course> Courses { get; set; }        // 一對多：一個老師多門課
}

public class Student                                 // 學生實體
{
    public int Id { get; set; }                      // 主鍵
    public string Name { get; set; }                 // 姓名
    public List<Enrollment> Enrollments { get; set; }// 多對多用關聯表
}
```

---

## 網路基礎（TCP/IP、子網路）

> 💡 **比喻：寄信的過程**
> - **IP 位址**：收件人的地址（告訴郵差送去哪裡）
> - **Port**：收件人的房間號碼（同一棟大樓的不同房間）
> - **TCP**：掛號信（確保對方收到，收不到會重寄）
> - **UDP**：明信片（寄出去就不管了，快但不保證送達）

### TCP/IP 四層模型

```
TCP/IP 四層模型 vs OSI 七層模型
┌────────────┬───────────────┬──────────────────┐
│ TCP/IP     │ OSI           │ 常見協定          │
├────────────┼───────────────┼──────────────────┤
│ 應用層     │ 應用層        │ HTTP, HTTPS,     │
│            │ 表示層        │ FTP, SMTP,       │
│            │ 會議層        │ DNS, SSH         │
├────────────┼───────────────┼──────────────────┤
│ 傳輸層     │ 傳輸層        │ TCP, UDP         │
├────────────┼───────────────┼──────────────────┤
│ 網路層     │ 網路層        │ IP, ICMP, ARP    │
├────────────┼───────────────┼──────────────────┤
│ 網路介面層 │ 資料鏈結層    │ Ethernet, Wi-Fi  │
│            │ 實體層        │ 光纖, 雙絞線     │
└────────────┴───────────────┴──────────────────┘
```

```csharp
// 常用 Port 號碼（IPAS 必考！）
var commonPorts = new Dictionary<int, string>        // 常見 Port 對照表
{
    [20]  = ""FTP 資料傳輸"",                         // 檔案傳輸（資料）
    [21]  = ""FTP 控制"",                              // 檔案傳輸（控制）
    [22]  = ""SSH 安全遠端連線"",                       // 加密的遠端登入
    [23]  = ""Telnet 遠端連線（不安全）"",              // 明文傳輸，已淘汰
    [25]  = ""SMTP 寄信"",                              // 寄出電子郵件
    [53]  = ""DNS 網域名稱解析"",                       // 把網址轉成 IP
    [80]  = ""HTTP 網頁"",                              // 一般網頁
    [110] = ""POP3 收信"",                              // 收電子郵件
    [143] = ""IMAP 收信（進階）"",                      // 進階收信協定
    [443] = ""HTTPS 加密網頁"",                         // 加密的網頁
    [3306] = ""MySQL 資料庫"",                          // MySQL 預設 Port
    [3389] = ""RDP 遠端桌面"",                          // Windows 遠端桌面
};

// TCP vs UDP 的差異
var tcpVsUdp = new                                    // TCP 和 UDP 比較
{
    TCP = new                                          // TCP 傳輸控制協定
    {
        連線方式 = ""三次握手建立連線"",                 // SYN → SYN-ACK → ACK
        可靠性 = ""保證資料送達、順序正確"",             // 遺失會重傳
        速度 = ""較慢（因為要確認）"",                   // 可靠但慢
        適用 = ""網頁、電子郵件、檔案傳輸"",            // 需要完整資料的場景
    },
    UDP = new                                          // UDP 使用者資料報協定
    {
        連線方式 = ""不需建立連線"",                     // 直接發送
        可靠性 = ""不保證送達"",                         // 遺失就算了
        速度 = ""很快（不用等確認）"",                   // 快但不可靠
        適用 = ""視訊通話、線上遊戲、DNS"",             // 要求即時性的場景
    },
};
```

### 子網路遮罩 (Subnet Mask)

```csharp
// IP 位址的組成
// IPv4 位址：32 位元，分成 4 組（每組 8 位元）
// 例如：192.168.1.100

// 子網路遮罩：區分「網路部分」和「主機部分」
var ip         = ""192.168.1.100"";     // IP 位址
var subnetMask = ""255.255.255.0"";     // 子網路遮罩（/24）

// 二進位表示法
// IP:     11000000.10101000.00000001.01100100  // 192.168.1.100
// Mask:   11111111.11111111.11111111.00000000  // 255.255.255.0
// ──────────────────────────────────────────
// 網路:   11000000.10101000.00000001           // 192.168.1（前 24 位）
// 主機:                              01100100  // .100（後 8 位）

// 常見的子網路遮罩
var subnets = new Dictionary<string, string>         // 子網路遮罩對照表
{
    [""255.0.0.0""]     = ""/8  → 可用主機數約 1600 萬"",  // A 類網路
    [""255.255.0.0""]   = ""/16 → 可用主機數約 65000"",    // B 類網路
    [""255.255.255.0""] = ""/24 → 可用主機數 254"",        // C 類網路（最常見）
    [""255.255.255.128""]=""/25 → 可用主機數 126"",        // 切半
    [""255.255.255.192""]=""/26 → 可用主機數 62"",         // 切四份
};

// 計算可用主機數的公式
int CalcHosts(int prefixLength)                       // 計算可用主機數
{
    int hostBits = 32 - prefixLength;                 // 主機部分的位元數
    int totalHosts = (int)Math.Pow(2, hostBits);      // 2 的 n 次方
    return totalHosts - 2;                             // 減掉網路位址和廣播位址
}
// CalcHosts(24) = 2^8 - 2 = 254                      // /24 有 254 台主機
// CalcHosts(25) = 2^7 - 2 = 126                      // /25 有 126 台主機
```

---

## 考試技巧與常見題型分析

```csharp
// IPAS 考試常見題型
var examTopics = new Dictionary<string, List<string>> // 各領域重點題型
{
    [""資訊安全""] = new()                              // 資安相關題型
    {
        ""CIA 三要素的定義和應用場景"",                  // 必考基礎題
        ""對稱/非對稱加密的差異"",                       // 加密概念
        ""各種資安攻擊的辨識"",                          // 知道每種攻擊特徵
        ""防火牆和 IDS/IPS 的功能"",                     // 網路安全設備
    },
    [""軟體工程""] = new()                              // 軟工相關題型
    {
        ""SDLC 各階段的工作內容"",                       // 瀑布式模型
        ""敏捷開發 vs 瀑布式的比較"",                    // 方法論比較
        ""測試層級（單元/整合/系統/驗收）"",              // 測試概念
        ""UML 圖的類型和用途"",                          // 統一塑模語言
    },
    [""資料庫""] = new()                                // 資料庫相關題型
    {
        ""正規化（1NF, 2NF, 3NF）"",                     // 最常考的計算題
        ""SQL 基本語法 (SELECT, JOIN)"",                 // SQL 查詢
        ""ER 圖的關係類型"",                              // 一對一/一對多/多對多
        ""交易的 ACID 特性"",                              // 交易管理
    },
    [""網路概論""] = new()                               // 網路相關題型
    {
        ""TCP/IP 和 OSI 模型的對應"",                     // 網路層級
        ""常用 Port 號碼"",                               // 必背！
        ""子網路遮罩的計算"",                              // 計算題
        ""HTTP 狀態碼的意義"",                             // 200/404/500 等
    },
};

// HTTP 狀態碼（Web 開發必知）
var httpStatusCodes = new Dictionary<int, string>      // HTTP 狀態碼
{
    [200] = ""OK - 請求成功"",                           // 一切正常
    [201] = ""Created - 建立成功"",                      // POST 成功建立資源
    [301] = ""Moved Permanently - 永久重導向"",          // 網址搬家了
    [302] = ""Found - 暫時重導向"",                      // 暫時去別的地方
    [400] = ""Bad Request - 請求格式錯誤"",              // 你送的資料有問題
    [401] = ""Unauthorized - 未授權"",                   // 你還沒登入
    [403] = ""Forbidden - 禁止存取"",                    // 你沒有權限
    [404] = ""Not Found - 找不到"",                      // 網頁不存在
    [500] = ""Internal Server Error - 伺服器錯誤"",     // 伺服器壞了
    [503] = ""Service Unavailable - 服務不可用"",        // 伺服器太忙
};

// ACID 交易特性
var acidProperties = new Dictionary<string, string>    // 交易的四大特性
{
    [""Atomicity 原子性""]    = ""交易要嘛全部成功，要嘛全部失敗"",   // 不會只做一半
    [""Consistency 一致性""]  = ""交易前後資料必須保持一致"",         // 不會出現矛盾
    [""Isolation 隔離性""]    = ""多個交易同時進行不會互相影響"",     // 彼此獨立
    [""Durability 持久性""]   = ""交易完成後的結果會永久保存"",       // 不會突然消失
};
```

### 答題策略

```csharp
// 考試答題策略
public class ExamTips // 考試小技巧
{
    public void AnswerStrategy()                       // 答題策略
    {
        // 1. 先做有把握的題目
        var easyQuestions = questions                   // 先找簡單的題目
            .Where(q => q.Confidence > 0.8);           // 有八成把握的先做

        // 2. 刪去法
        // 四選一，通常可以先排除 2 個明顯錯的
        // 剩下 2 個猜也有 50% 機率                     // 比瞎猜 25% 好很多

        // 3. 關鍵字作答法
        // 題目中的關鍵字通常暗示答案
        // 「確保資料不被竄改」→ 完整性 (Integrity)     // 看到「竄改」想到完整性
        // 「防止未授權存取」→ 機密性 (Confidentiality) // 看到「未授權」想到機密性
        // 「系統隨時可用」→ 可用性 (Availability)      // 看到「隨時可用」想到可用性

        // 4. 時間管理
        var timePerQuestion = 90.0 / 50;               // 90 分鐘 50 題
        // 每題平均 1.8 分鐘                            // 不要在一題上花太久
        // 先做完全部，再回頭檢查不確定的                  // 確保每題都有作答
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：搞混 CIA 三要素

```csharp
// ❌ 常見混淆
// 題目：「某公司的網站被駭客入侵，資料被修改」
// 錯誤答案：這是違反「機密性」                       // 很多人選這個

// ✅ 正確答案：這是違反「完整性」                     // 資料被「修改」= 完整性
// 記憶口訣：
// 被偷看 → 機密性 (Confidentiality)                 // 不該看到的人看到了
// 被竄改 → 完整性 (Integrity)                       // 資料被改了
// 掛掉了 → 可用性 (Availability)                    // 服務不能用了
```

### ❌ 錯誤 2：正規化的順序搞不清楚

```csharp
// ❌ 以為 2NF 就是把所有欄位拆開
// 正規化是有順序的，必須先滿足前一級

// ✅ 正確的正規化步驟
// 1NF：消除重複群組（每格只有一個值）                // 先做這個
// 2NF：消除部分相依（非主鍵欄位完全依賴主鍵）        // 1NF 做完才做這個
// 3NF：消除遞移相依（非主鍵欄位不依賴其他非主鍵）    // 2NF 做完才做這個

// 口訣：「一格一值 → 完全依賴 → 不傳話」
// 1NF：一個格子只能放一個值（一格一值）
// 2NF：非主鍵欄位要完全依賴整個主鍵（完全依賴）
// 3NF：非主鍵欄位之間不能有相依關係（不傳話）
```

### ❌ 錯誤 3：子網路計算粗心

```csharp
// ❌ 忘記減 2
int wrongHosts = (int)Math.Pow(2, 8);      // 2^8 = 256
// 錯！要扣掉網路位址和廣播位址

// ✅ 正確計算
int correctHosts = (int)Math.Pow(2, 8) - 2; // 2^8 - 2 = 254
// 第一個 IP 是網路位址（不能用）              // 例如 192.168.1.0
// 最後一個 IP 是廣播位址（不能用）            // 例如 192.168.1.255
// 所以可用主機數 = 2^(32-prefix) - 2         // 記得一定要減 2！
```
" },
    };
}
