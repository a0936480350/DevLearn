// Fetches questions from API and caches by difficulty
export default class QuestionManager {
    constructor() {
        this.cache = { 1: [], 2: [], 3: [] };
        this.loading = { 1: false, 2: false, 3: false };
    }

    async init() {
        await Promise.all([
            this._fetch(1),
            this._fetch(2),
            this._fetch(3),
        ]);
    }

    async _fetch(difficulty) {
        if (this.loading[difficulty]) return;
        this.loading[difficulty] = true;
        try {
            const res = await fetch(`/Monopoly/GetQuestions?difficulty=${difficulty}&count=15`);
            if (res.ok) {
                const data = await res.json();
                this.cache[difficulty].push(...data);
            }
        } catch (e) {
            console.warn('Question fetch failed:', e);
        }
        this.loading[difficulty] = false;
    }

    async getQuestion(difficulty) {
        const d = Math.min(Math.max(difficulty, 1), 3);
        if (this.cache[d].length === 0) {
            await this._fetch(d);
        }
        if (this.cache[d].length === 0) {
            // Fallback: try other difficulties
            for (const alt of [1, 2, 3]) {
                if (this.cache[alt].length > 0) return this.cache[alt].shift();
            }
            // Final fallback
            return this._fallbackQuestion();
        }
        // Refetch in background if running low
        if (this.cache[d].length < 3) this._fetch(d);
        return this.cache[d].shift();
    }

    async checkAnswer(questionId, answer) {
        try {
            const res = await fetch('/Monopoly/CheckAnswer', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ questionId, answer }),
            });
            if (res.ok) return await res.json();
        } catch (e) {
            console.warn('Answer check failed:', e);
        }
        return { correct: false, correctAnswer: 0 };
    }

    _fallbackQuestion() {
        const questions = [
            { id: -1, text: 'C# 中 string 是什麼型別？', options: ['值型別', '參考型別', '列舉型別', '結構型別'], correctAnswer: 1, difficulty: 1 },
            { id: -2, text: 'HTML 的 <a> 標籤用來做什麼？', options: ['插入圖片', '建立連結', '建立表格', '換行'], correctAnswer: 1, difficulty: 1 },
            { id: -3, text: 'SQL 中 SELECT DISTINCT 的作用？', options: ['選全部', '去重複', '排序', '分組'], correctAnswer: 1, difficulty: 1 },
            { id: -4, text: 'JavaScript 中 === 和 == 的差異？', options: ['沒差異', '=== 嚴格比較', '== 更嚴格', '=== 只比較數字'], correctAnswer: 1, difficulty: 1 },
            { id: -5, text: 'CSS 的 display: flex 用來做什麼？', options: ['隱藏元素', '彈性排版', '固定定位', '設定字體'], correctAnswer: 1, difficulty: 1 },
            { id: -6, text: 'C# 的 List<T> 屬於哪個命名空間？', options: ['System.IO', 'System.Collections.Generic', 'System.Linq', 'System.Text'], correctAnswer: 1, difficulty: 1 },
            { id: -7, text: 'SQL 的 WHERE 子句用來做什麼？', options: ['排序', '過濾資料', '分組', '合併表格'], correctAnswer: 1, difficulty: 1 },
            { id: -8, text: 'Git 中用來查看狀態的指令是？', options: ['git log', 'git status', 'git diff', 'git show'], correctAnswer: 1, difficulty: 1 },
            { id: -9, text: 'HTTP 狀態碼 404 代表什麼？', options: ['伺服器錯誤', '找不到資源', '禁止存取', '重新導向'], correctAnswer: 1, difficulty: 1 },
            { id: -10, text: 'async/await 是用來處理什麼的？', options: ['迴圈', '非同步操作', '錯誤處理', '型別轉換'], correctAnswer: 1, difficulty: 1 },
            { id: -11, text: 'CSS 的 position: absolute 相對於誰定位？', options: ['瀏覽器視窗', '最近的定位祖先', '父元素', 'body'], correctAnswer: 1, difficulty: 2 },
            { id: -12, text: 'LINQ 的 Where() 方法回傳什麼？', options: ['單一元素', 'IEnumerable<T>', 'bool', 'int'], correctAnswer: 1, difficulty: 2 },
        ];
        return questions[Math.floor(Math.random() * questions.length)];
    }
}
