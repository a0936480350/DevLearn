// ── 進度 Badge ─────────────────────────────────────────
(async () => {
    try {
        const res = await fetch('/Home/ProgressSummary');
        const data = await res.json();
        const badge = document.getElementById('totalProgress');
        if (badge) badge.textContent = `${data.done} / ${data.total} 完成`;
    } catch {}
})();

// ── 全局搜尋 ───────────────────────────────────────────
const searchInput = document.getElementById('globalSearch');
const searchResults = document.getElementById('searchResults');

if (searchInput) {
    let timer;
    searchInput.addEventListener('input', () => {
        clearTimeout(timer);
        const q = searchInput.value.trim();
        if (!q) { searchResults.innerHTML = ''; return; }
        timer = setTimeout(async () => {
            const res = await fetch(`/Home/Search?q=${encodeURIComponent(q)}`);
            const items = await res.json();
            searchResults.innerHTML = items.length
                ? items.map(i => `<div class="search-item" onclick="location.href='/Home/Chapter/${i.slug}'">
                    <span>${i.icon}</span><span>${i.title}</span>
                    <span style="margin-left:auto;font-size:.75rem;color:#8B949E">${i.category}</span>
                  </div>`).join('')
                : '<div class="search-item" style="color:#8B949E">找不到結果</div>';
        }, 300);
    });
    document.addEventListener('click', e => {
        if (!searchInput.contains(e.target)) searchResults.innerHTML = '';
    });
}

// ── 手機版 Navbar Toggle ────────────────────────────────
function toggleMobileNav() {
    document.getElementById('navRight')?.classList.toggle('open');
}

// ── 手機版側邊欄 Drawer ─────────────────────────────────
function toggleSidebar() {
    const sidebar = document.querySelector('.sidebar');
    const overlay = document.querySelector('.sidebar-overlay');
    if (sidebar) sidebar.classList.toggle('open');
    if (overlay) overlay.classList.toggle('open');
}

// 點 overlay 關閉 sidebar
document.addEventListener('click', e => {
    if (e.target.classList.contains('sidebar-overlay')) {
        toggleSidebar();
    }
});

// ── 側邊欄 ─────────────────────────────────────────────
function toggleGroup(btn) {
    btn.parentElement.classList.toggle('open');
}

const sideSearch = document.getElementById('sidebarSearch');
if (sideSearch) {
    sideSearch.addEventListener('input', () => {
        const q = sideSearch.value.toLowerCase();
        document.querySelectorAll('.nav-item').forEach(item => {
            const title = item.querySelector('.nav-title')?.textContent.toLowerCase() ?? '';
            const match = !q || title.includes(q);
            item.style.display = match ? '' : 'none';
            if (match && q) item.closest('.nav-group')?.classList.add('open');
        });
    });
}

// ── 進度標記 ───────────────────────────────────────────
async function markComplete(chapterId) {
    const sessionId = typeof SESSION_ID !== 'undefined' ? SESSION_ID : '';
    await fetch('/Home/MarkComplete', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ sessionId, chapterId })
    });
    const btn = document.getElementById('markDoneBtn');
    if (btn) { btn.textContent = '✅ 已完成'; btn.classList.add('is-done'); }
    const navItem = document.querySelector(`.nav-item.active`);
    if (navItem) { navItem.classList.add('done'); const c = navItem.querySelector('.nav-check'); if (c) c.textContent = '✅'; }
}

// Theme toggle
function toggleTheme() {
    var html = document.documentElement;
    var btn = document.getElementById('themeToggle');
    if (html.getAttribute('data-theme') === 'light') {
        html.removeAttribute('data-theme');
        btn.textContent = '🌙';
        localStorage.setItem('theme', 'dark');
    } else {
        html.setAttribute('data-theme', 'light');
        btn.textContent = '☀️';
        localStorage.setItem('theme', 'light');
    }
}

// Load saved theme
(function() {
    var saved = localStorage.getItem('theme');
    if (saved === 'light') {
        document.documentElement.setAttribute('data-theme', 'light');
        var btn = document.getElementById('themeToggle');
        if (btn) btn.textContent = '☀️';
    }
})();

// Multi-language
var translations = {
    zh: {
        // Navbar
        'nav-search': '搜尋章節...',
        'nav-checkin': '簽到',
        'nav-daily': '每日',
        'nav-games': '遊戲',
        'nav-tools': '工具',
        'nav-teacher': '找老師',
        'nav-login': '登入',
        'nav-progress': '完成',
        'nav-home': '首頁',
        'nav-my': '我的',
        'nav-detective': '程式碼偵探',
        'nav-speed': '速度挑戰',
        'nav-puzzle': '填字遊戲',
        'nav-arena': '程式碼擂台',
        'nav-analytics': '學習分析',
        'nav-flashcard': '暗記卡片',
        'nav-snippet': '程式碼收藏',
        'nav-qna': '問答區',
        'nav-buddy': '學習夥伴',
        'nav-leaderboard': '排行榜',

        // Hero
        'hero-badge': '完整 .NET 學習路徑',
        'hero-title-1': '從零開始，',
        'hero-title-2': '完整學會 .NET',
        'hero-desc': 'C# / ASP.NET Core / 資料庫 / 設計模式 / Docker / AI\n每章附詳細說明、程式碼範例、常見錯誤分析與隨機測驗',
        'hero-start': '開始學習',
        'hero-challenge': '隨機挑戰',
        'hero-scores': '成績總覽',
        'stat-chapters': '���節內容',
        'stat-categories': '主題分類',
        'stat-free': '完全免費',

        // Features section
        'section-features': '平台功能導覽',
        'section-features-desc': '一站式學習 + 互動 + 媒合，所有功能免費使用',
        'feat-chapters': '學習章節',
        'feat-chapters-desc': '93 章完整教學，含程式碼範例與錯誤分析',
        'feat-games': '互動遊戲',
        'feat-games-desc': '速度挑戰、偵探、填字、每日一題、擂台',
        'feat-teacher': '找老師',
        'feat-teacher-desc': '瀏覽老師、預約課程、一對一教學',
        'feat-checkin': '每日簽到',
        'feat-checkin-desc': '連續簽到拿積分，解鎖 14 種成就',
        'feat-analytics': '學習分析',
        'feat-analytics-desc': '熱力圖、弱點分析、學習時間統計',
        'feat-community': '社群互動',
        'feat-community-desc': '問答區、想法分享、即時聊天室、私訊',
        'feat-tools': '學習工具',
        'feat-tools-desc': '暗記卡片、程式碼收藏、筆記分享',
        'feat-leaderboard': '排行榜',
        'feat-leaderboard-desc': '積分排名、等級徽章、學習競賽',

        // Promo sections
        'promo-path-title': '從零開始的完整學習路徑',
        'promo-path-desc': '82 個章節涵蓋 C#、ASP.NET Core、資料庫、Docker、AI 等完整技術棧。每個章節都有程式碼範例、常見錯誤分析和隨機測驗。',
        'promo-path-link': '開始學習 →',
        'promo-teacher-title': '找到最適合你的老師',
        'promo-teacher-desc': '專業老師一對一教學，根據你的程度量身打造課程。支援線上即時聊天、預約管理、學習追蹤。',
        'promo-teacher-link': '瀏覽老師 →',
        'promo-game-title': '邊玩邊學，不再枯燥',
        'promo-game-desc': '速度挑戰、程式碼偵探、填字遊戲、每日一題...用遊戲化方式學程式，積分排行榜讓你越學越上癮。',
        'promo-game-link': '開始挑戰 →',

        // Sections
        'section-core': '核心學習路線',
        'section-core-desc': '按順序學習這三個主題，打好 .NET 開發基礎',
        'section-advanced': '進階擴充模組',
        'section-advanced-desc': '核心路線學完後，依需求選擇深入的方向',
        'section-challenge': '隨機挑戰',
        'section-challenge-desc': '不用看課程也能測實力！隨機抽題，看看你的程度',
        'section-ideas': '知識分享牆',
        'section-ideas-desc': '看看大家的學習心得、筆記和想法',
        'section-ideas-btn': '分享你的想法',
        'section-teacher-posts': '老師免費分享',
        'section-teacher-posts-desc': '來自平台老師的最新免費文章、影片和資源',
        'challenge-quick': '快速挑戰',
        'challenge-quick-time': '3 分鐘',
        'challenge-standard': '標準測驗',
        'challenge-standard-time': '8 分鐘',
        'challenge-full': '完整挑戰',
        'challenge-full-time': '15 分鐘',

        // Developer section
        'dev-title': '嗨！我是 Mike，歡迎來到 DevLearn',
        'dev-signature': '—— Mike（邱瀚賢）｜DevLearn 開發者',

        // Footer
        'footer-title': 'DevLearn · .NET 學習平台',
        'footer-home': '首頁',
        'footer-leaderboard': '排行榜',
        'footer-scores': '成績',

        // Chapter page
        'ch-sidebar': '目錄',
        'ch-back': '← 回首頁',
        'ch-share-idea': '分享想法',
        'ch-practice': '練習',
        'ch-share': '分享',
        'ch-quiz': '測驗',
        'ch-complete': '標記完成',
        'ch-completed': '已完成',
        'ch-prev': '上一章',
        'ch-next': '下一章',
        'ch-search': '搜尋...',
        'ch-ideas-title': '大家的想法',
        'ch-sidebar-toggle': '目錄',

        // Login/Register
        'login-title': '登入',
        'login-subtitle': '輸入暱稱和 Email 登入你的帳號',
        'login-nickname': '暱稱',
        'login-email': 'Email',
        'login-btn': '登入',
        'login-no-account': '還沒帳號？',
        'login-register': '註冊',
        'login-home': '回首頁',
        'register-title': '註冊帳號',
        'register-subtitle': '升級為正式會員，保存你的學習紀錄',
        'register-btn': '註冊',
        'register-has-account': '已有帳號？',
        'register-login': '登入',

        // Games
        'game-checkin': '每日簽到',
        'game-checkin-title': '每日簽到 & 成就',
        'game-checkin-desc': '每天簽到累積積分，解鎖學習成就',
        'game-checkin-btn': '簽到',
        'game-checkin-done': '已簽到',
        'game-daily': '每日一題',
        'game-speed': '速度挑戰',
        'game-speed-start': '開始挑戰！',
        'game-detective': '程式碼偵探',
        'game-puzzle': '程式碼填字遊戲',
        'game-arena': '程式碼擂台',
        'game-arena-desc': '和其他學習者一起比拼程式能力',
        'game-start': '開始挑戰',
        'game-submit': '提交答案',

        // Teacher
        'teacher-title': '找老師',
        'teacher-subtitle': '尋找最適合你的 .NET 專業老師',
        'teacher-search': '搜尋老師名稱、專長...',
        'teacher-search-btn': '搜尋',
        'teacher-all': '全部',
        'teacher-book': '預約體驗課',
        'teacher-contact': '聯絡老師',
        'teacher-apply': '申請成為老師',

        // Profile
        'profile-title': '個人檔案',
        'profile-home': '回首頁',
        'profile-section-personal': '個人中心',
        'profile-info': '個人資料',
        'profile-ideas': '我的想法',
        'profile-qna': '我的問答',
        'profile-chat': '我的聊天',
        'profile-quiz': '我的測驗',
        'profile-logout': '登出',

        // Chat
        'chat-title': '聊天室',
        'chat-header': '即時聊天室',
        'chat-public': '公開聊天',
        'chat-private': '私訊',
        'chat-support': '客服',
        'chat-send': '傳送',
        'chat-placeholder': '輸入訊息...',
        'chat-nickname-label': '設定你的暱稱',
        'chat-avatar-label': '選擇頭像',
        'chat-join': '加入聊天',
        'chat-pm-placeholder': '輸入私訊...',
        'chat-support-new': '新問題',
        'chat-support-submit': '提交回報',
        'chat-support-tickets': '我的工單',

        // Common
        'btn-save': '儲存',
        'btn-cancel': '取消',
        'btn-delete': '刪除',
        'btn-edit': '編輯',
        'btn-submit': '提交',
        'btn-close': '關閉',
        'loading': '載入中...',
        'no-data': '暫無資料',

        // Gesture / Voice
        'gesture-title': '手勢控制',
        'voice-title': '語音控制',

        // Analytics / Leaderboard / Scores
        'analytics-title': '學習分析',
        'analytics-desc': '追蹤你的學習歷程，找出弱點，持續進步',
        'leaderboard-title': '學習排行榜',
        'leaderboard-desc': '與其他學習者一較高下',
        'scores-title': '我的學習成績',
        'scores-desc': '追蹤你的測驗紀錄與學習進度',

        // QnA
        'qna-title': '問答討論',
        'qna-desc': '有問題就問，大家一起討論解決',
        'qna-ask': '提問',

        // Buddy
        'buddy-title': '學習夥伴配對',
        'buddy-desc': '找到和你志同道合的學習夥伴，一起成長！',

        // Flashcard / Snippet
        'flashcard-title': '暗記卡片',
        'flashcard-desc': '用間隔重複法高效記憶重要觀念',
        'flashcard-manage': '📋 管理',
        'flashcard-review': '🔄 複習',
        'flashcard-add': '＋ 新增卡片',
        'flashcard-total': '卡片總數',
        'flashcard-due': '待複習',
        'flashcard-mastery': '掌握率',
        'flashcard-empty': '還沒有卡片，點右上角新增第一張暗記卡片吧！',
        'flashcard-flip-hint': '點擊翻面',
        'flashcard-forgot': '忘了 ❌',
        'flashcard-remembered': '記得 ✅',
        'flashcard-done': '太棒了！所有待複習的卡片都完成了！',
        'flashcard-back-manage': '返回管理',
        'flashcard-create-title': '🧠 新增暗記卡片',
        'flashcard-front-label': '正面（問題）',
        'flashcard-front-ph': '例：什麼是 SOLID 原則中的 S？',
        'flashcard-back-label': '背面（答案）',
        'flashcard-back-ph': '例：Single Responsibility Principle — 單一職責原則',
        'flashcard-category-label': '分類（選填）',
        'flashcard-category-ph': '例：設計模式、C# 語法、ASP.NET',
        'flashcard-no-due': '目前沒有需要複習的卡片！',
        'flashcard-fill-required': '請填入正面和背面內容',
        'flashcard-confirm-delete': '確定要刪除這張卡片嗎？',
        'flashcard-review-times': '複習',
        'flashcard-review-unit': '次',
        'snippet-title': '程式碼收藏',
        'snippet-desc': '收藏實用的程式碼片段，隨時查閱',
        'snippet-add': '＋ 新增片段',
        'snippet-empty': '還沒有收藏，在章節頁面點 📌 收藏程式碼',
        'snippet-create-title': '📌 新增程式碼片段',
        'snippet-title-label': '標題',
        'snippet-title-ph': '例：LINQ GroupBy 範例',
        'snippet-lang-label': '程式語言',
        'snippet-code-label': '程式碼',
        'snippet-code-ph': '貼上你的程式碼...',
        'snippet-note-label': '備註（選填）',
        'snippet-note-ph': '任何筆記或說明...',
        'snippet-fill-required': '請填入標題和程式碼',
        'snippet-confirm-delete': '確定要刪除這個片段嗎？',
        'snippet-count': '個片段',
        'snippet-delete': '🗑 刪除',

        // Account extras
        'login-ph-nickname': '輸入你的暱稱',
        'login-ph-email': '輸入您的 Email（已隱藏保護）',
        'login-email-hint': '💡 Email 作為您的身份驗證，已隱藏保護',
        'register-ph-email': '輸入您的 Email',
        'register-email-hint': '💡 Email 不會公開顯示，僅用於登入驗證',
        'profile-registered': '已註冊會員',
        'profile-anonymous': '匿名用戶',
        'profile-total-score': '總分數',
        'profile-quizzes-taken': '測驗次數',
        'profile-chapters-done': '完成章節',
        'profile-account-info': '帳號資訊',
        'profile-registered-badge': '已註冊 ✅',
        'profile-anon-id': '匿名 ID',
        'profile-created-at': '建立時間',
        'profile-last-active': '最後活動',
        'profile-edit-title': '✏️ 編輯個人資料',
        'profile-upload-avatar': '📷 上傳/更換頭像',
        'profile-nickname-label': '✏️ 暱稱',
        'profile-email-label': '📧 Email',
        'profile-save-btn': '💾 儲存修改',
        'profile-upgrade-title': '🚀 註冊升級',
        'profile-upgrade-desc': '註冊後可以管理你的想法、問答、聊天紀錄和測驗歷史！',
        'profile-upgrade-btn': '📝 立即註冊',
        'profile-fav-teachers': '❤️ 我的收藏',
        'profile-bookings': '📅 我的預約',
        'profile-confirm-logout': '確定要登出嗎？登出後將使用新的匿名身份',
        'profile-sidebar-upgrade': '註冊升級',

        // Error page
        'error404-title': '404 - 找不到頁面',
        'error404-msg': '哎呀！這個頁面去冒險了',
        'error404-desc': '你找的頁面可能已經搬家、改名，或正在學習 .NET 的路上迷路了',
        'error404-home': '回到首頁 🏠',

        // Layout extras
        'support-category-bug': '網站問題回報',
        'support-category-tech': '技術問題',
        'support-category-payment': '付款問題',
        'support-category-teacher': '老師相關',
        'support-category-booking': '預約問題',
        'support-category-feature': '功能建議',
        'support-category-other': '其他',
        'loading-badge': '⏳ 載入中',
        'theme-toggle': '切換主題',
        'menu-label': '選單',
    },
    en: {
        // Navbar
        'nav-search': 'Search chapters...',
        'nav-checkin': 'Check-in',
        'nav-daily': 'Daily',
        'nav-games': 'Games',
        'nav-tools': 'Tools',
        'nav-teacher': 'Teachers',
        'nav-login': 'Login',
        'nav-progress': 'Done',
        'nav-home': 'Home',
        'nav-my': 'Me',
        'nav-detective': 'Code Detective',
        'nav-speed': 'Speed Challenge',
        'nav-puzzle': 'Code Puzzle',
        'nav-arena': 'Code Arena',
        'nav-analytics': 'Analytics',
        'nav-flashcard': 'Flashcards',
        'nav-snippet': 'Code Snippets',
        'nav-qna': 'Q&A',
        'nav-buddy': 'Study Buddy',
        'nav-leaderboard': 'Leaderboard',

        // Hero
        'hero-badge': 'Complete .NET Learning Path',
        'hero-title-1': 'From Zero,',
        'hero-title-2': 'Master .NET',
        'hero-desc': 'C# / ASP.NET Core / Database / Design Patterns / Docker / AI\nDetailed tutorials, code examples, common mistakes & quizzes',
        'hero-start': 'Start Learning',
        'hero-challenge': 'Random Challenge',
        'hero-scores': 'Scores',
        'stat-chapters': 'Chapters',
        'stat-categories': 'Categories',
        'stat-free': 'Totally Free',

        // Features
        'section-features': 'Platform Features',
        'section-features-desc': 'All-in-one learning + interaction + matching, completely free',
        'feat-chapters': 'Chapters',
        'feat-chapters-desc': '93 full tutorials with code examples & error analysis',
        'feat-games': 'Games',
        'feat-games-desc': 'Speed challenge, detective, puzzle, daily, arena',
        'feat-teacher': 'Teachers',
        'feat-teacher-desc': 'Browse teachers, book lessons, 1-on-1 teaching',
        'feat-checkin': 'Daily Check-in',
        'feat-checkin-desc': 'Earn points with streaks, unlock 14 achievements',
        'feat-analytics': 'Analytics',
        'feat-analytics-desc': 'Heatmap, weakness analysis, study time tracking',
        'feat-community': 'Community',
        'feat-community-desc': 'Q&A, idea sharing, live chat, private messages',
        'feat-tools': 'Learning Tools',
        'feat-tools-desc': 'Flashcards, code snippets, note sharing',
        'feat-leaderboard': 'Leaderboard',
        'feat-leaderboard-desc': 'Score ranking, level badges, learning competition',

        // Promo sections
        'promo-path-title': 'Complete Learning Path From Zero',
        'promo-path-desc': '82 chapters covering C#, ASP.NET Core, Database, Docker, AI and more. Each chapter includes code examples, common mistake analysis and quizzes.',
        'promo-path-link': 'Start Learning \u2192',
        'promo-teacher-title': 'Find Your Perfect Teacher',
        'promo-teacher-desc': 'Professional 1-on-1 teaching, customized to your level. Live chat, booking management, learning tracking.',
        'promo-teacher-link': 'Browse Teachers \u2192',
        'promo-game-title': 'Learn by Playing, No More Boredom',
        'promo-game-desc': 'Speed challenge, code detective, crossword puzzle, daily challenge... Gamified learning with leaderboard competition.',
        'promo-game-link': 'Start Challenge \u2192',

        // Sections
        'section-core': 'Core Learning Path',
        'section-core-desc': 'Learn these three topics in order to build a solid .NET foundation',
        'section-advanced': 'Advanced Modules',
        'section-advanced-desc': 'After the core path, choose topics to go deeper',
        'section-challenge': 'Random Challenge',
        'section-challenge-desc': 'Test your skills without lessons! Random questions to check your level',
        'section-ideas': 'Knowledge Wall',
        'section-ideas-desc': "See everyone's learning notes, insights and ideas",
        'section-ideas-btn': 'Share Your Idea',
        'section-teacher-posts': 'Teacher Sharing',
        'section-teacher-posts-desc': 'Latest free articles, videos and resources from platform teachers',
        'challenge-quick': 'Quick Challenge',
        'challenge-quick-time': '3 min',
        'challenge-standard': 'Standard Quiz',
        'challenge-standard-time': '8 min',
        'challenge-full': 'Full Challenge',
        'challenge-full-time': '15 min',

        // Developer
        'dev-title': "Hi! I'm Mike, Welcome to DevLearn",
        'dev-signature': '\u2014\u2014 Mike (Chiu Han-Hsien) | DevLearn Developer',

        // Footer
        'footer-title': 'DevLearn \u00B7 .NET Learning Platform',
        'footer-home': 'Home',
        'footer-leaderboard': 'Leaderboard',
        'footer-scores': 'Scores',

        // Chapter
        'ch-sidebar': 'Contents',
        'ch-back': '\u2190 Home',
        'ch-share-idea': 'Share Idea',
        'ch-practice': 'Practice',
        'ch-share': 'Share',
        'ch-quiz': 'Quiz',
        'ch-complete': 'Mark Complete',
        'ch-completed': 'Completed',
        'ch-prev': 'Previous',
        'ch-next': 'Next',
        'ch-search': 'Search...',
        'ch-ideas-title': "Everyone's Ideas",
        'ch-sidebar-toggle': 'Contents',

        // Login/Register
        'login-title': 'Login',
        'login-subtitle': 'Enter your nickname and email to login',
        'login-nickname': 'Nickname',
        'login-email': 'Email',
        'login-btn': 'Login',
        'login-no-account': "Don't have an account?",
        'login-register': 'Register',
        'login-home': 'Home',
        'register-title': 'Register',
        'register-subtitle': 'Create your learning account',
        'register-btn': 'Register',
        'register-has-account': 'Already have an account?',
        'register-login': 'Login',

        // Games
        'game-checkin': 'Daily Check-in',
        'game-checkin-title': 'Daily Check-in & Achievements',
        'game-checkin-desc': 'Check in daily to earn points and unlock achievements',
        'game-checkin-btn': 'Check In',
        'game-checkin-done': 'Checked In',
        'game-daily': 'Daily Challenge',
        'game-speed': 'Speed Challenge',
        'game-speed-start': 'Start Challenge!',
        'game-detective': 'Code Detective',
        'game-puzzle': 'Code Crossword Puzzle',
        'game-arena': 'Code Arena',
        'game-arena-desc': 'Compete with other learners in coding skills',
        'game-start': 'Start Challenge',
        'game-submit': 'Submit Answer',

        // Teacher
        'teacher-title': 'Find Teachers',
        'teacher-subtitle': 'Find the best .NET teachers for you',
        'teacher-search': 'Search teacher name, skills...',
        'teacher-search-btn': 'Search',
        'teacher-all': 'All',
        'teacher-book': 'Book Trial Lesson',
        'teacher-contact': 'Contact Teacher',
        'teacher-apply': 'Apply as Teacher',

        // Profile
        'profile-title': 'Profile',
        'profile-home': 'Home',
        'profile-section-personal': 'Personal',
        'profile-info': 'Profile',
        'profile-ideas': 'My Ideas',
        'profile-qna': 'My Q&A',
        'profile-chat': 'My Chat',
        'profile-quiz': 'My Quizzes',
        'profile-logout': 'Logout',

        // Chat
        'chat-title': 'Chat',
        'chat-header': 'Live Chat',
        'chat-public': 'Public Chat',
        'chat-private': 'Private',
        'chat-support': 'Support',
        'chat-send': 'Send',
        'chat-placeholder': 'Type a message...',
        'chat-nickname-label': 'Set your nickname',
        'chat-avatar-label': 'Choose avatar',
        'chat-join': 'Join Chat',
        'chat-pm-placeholder': 'Type a private message...',
        'chat-support-new': 'New Issue',
        'chat-support-submit': 'Submit Report',
        'chat-support-tickets': 'My Tickets',

        // Common
        'btn-save': 'Save',
        'btn-cancel': 'Cancel',
        'btn-delete': 'Delete',
        'btn-edit': 'Edit',
        'btn-submit': 'Submit',
        'btn-close': 'Close',
        'loading': 'Loading...',
        'no-data': 'No data',

        // Gesture / Voice
        'gesture-title': 'Gesture Control',
        'voice-title': 'Voice Control',

        // Analytics / Leaderboard / Scores
        'analytics-title': 'Learning Analytics',
        'analytics-desc': 'Track your learning journey, find weaknesses, keep improving',
        'leaderboard-title': 'Leaderboard',
        'leaderboard-desc': 'Compete with other learners',
        'scores-title': 'My Scores',
        'scores-desc': 'Track your quiz records and learning progress',

        // QnA
        'qna-title': 'Q&A Discussion',
        'qna-desc': 'Ask questions and discuss with everyone',
        'qna-ask': 'Ask',

        // Buddy
        'buddy-title': 'Study Buddy Matching',
        'buddy-desc': 'Find like-minded study partners and grow together!',

        // Flashcard / Snippet
        'flashcard-title': 'Flashcards',
        'flashcard-desc': 'Efficiently memorize key concepts with spaced repetition',
        'flashcard-manage': '📋 Manage',
        'flashcard-review': '🔄 Review',
        'flashcard-add': '＋ New Card',
        'flashcard-total': 'Total Cards',
        'flashcard-due': 'Due Review',
        'flashcard-mastery': 'Mastery',
        'flashcard-empty': 'No cards yet. Click the top-right button to add your first flashcard!',
        'flashcard-flip-hint': 'Click to flip',
        'flashcard-forgot': 'Forgot ❌',
        'flashcard-remembered': 'Got it ✅',
        'flashcard-done': 'Awesome! All review cards are done!',
        'flashcard-back-manage': 'Back to Manage',
        'flashcard-create-title': '🧠 New Flashcard',
        'flashcard-front-label': 'Front (Question)',
        'flashcard-front-ph': 'e.g. What is the S in SOLID?',
        'flashcard-back-label': 'Back (Answer)',
        'flashcard-back-ph': 'e.g. Single Responsibility Principle',
        'flashcard-category-label': 'Category (optional)',
        'flashcard-category-ph': 'e.g. Design Patterns, C# Syntax, ASP.NET',
        'flashcard-no-due': 'No cards due for review!',
        'flashcard-fill-required': 'Please fill in both front and back',
        'flashcard-confirm-delete': 'Are you sure you want to delete this card?',
        'flashcard-review-times': 'Reviewed',
        'flashcard-review-unit': 'times',
        'snippet-title': 'Code Snippets',
        'snippet-desc': 'Save useful code snippets for quick reference',
        'snippet-add': '＋ New Snippet',
        'snippet-empty': 'No snippets yet. Click 📌 on chapter pages to save code',
        'snippet-create-title': '📌 New Code Snippet',
        'snippet-title-label': 'Title',
        'snippet-title-ph': 'e.g. LINQ GroupBy Example',
        'snippet-lang-label': 'Language',
        'snippet-code-label': 'Code',
        'snippet-code-ph': 'Paste your code here...',
        'snippet-note-label': 'Notes (optional)',
        'snippet-note-ph': 'Any notes or description...',
        'snippet-fill-required': 'Please fill in title and code',
        'snippet-confirm-delete': 'Are you sure you want to delete this snippet?',
        'snippet-count': 'snippets',
        'snippet-delete': '🗑 Delete',

        // Account extras
        'login-ph-nickname': 'Enter your nickname',
        'login-ph-email': 'Enter your email (hidden for security)',
        'login-email-hint': '💡 Email is used for verification, kept hidden',
        'register-ph-email': 'Enter your email',
        'register-email-hint': '💡 Email is private, used only for login verification',
        'profile-registered': 'Registered Member',
        'profile-anonymous': 'Anonymous User',
        'profile-total-score': 'Total Score',
        'profile-quizzes-taken': 'Quizzes Taken',
        'profile-chapters-done': 'Chapters Done',
        'profile-account-info': 'Account Info',
        'profile-registered-badge': 'Registered ✅',
        'profile-anon-id': 'Anonymous ID',
        'profile-created-at': 'Created',
        'profile-last-active': 'Last Active',
        'profile-edit-title': '✏️ Edit Profile',
        'profile-upload-avatar': '📷 Upload/Change Avatar',
        'profile-nickname-label': '✏️ Nickname',
        'profile-email-label': '📧 Email',
        'profile-save-btn': '💾 Save Changes',
        'profile-upgrade-title': '🚀 Upgrade Account',
        'profile-upgrade-desc': 'Register to manage your ideas, Q&A, chat history and quiz records!',
        'profile-upgrade-btn': '📝 Register Now',
        'profile-fav-teachers': '❤️ My Favorites',
        'profile-bookings': '📅 My Bookings',
        'profile-confirm-logout': 'Are you sure you want to log out? You will get a new anonymous identity',
        'profile-sidebar-upgrade': 'Upgrade',

        // Error page
        'error404-title': '404 - Page Not Found',
        'error404-msg': 'Oops! This page went on an adventure',
        'error404-desc': 'The page you are looking for may have moved, been renamed, or got lost on the .NET learning path',
        'error404-home': 'Back to Home 🏠',

        // Layout extras
        'support-category-bug': 'Bug Report',
        'support-category-tech': 'Technical Issue',
        'support-category-payment': 'Payment Issue',
        'support-category-teacher': 'Teacher Related',
        'support-category-booking': 'Booking Issue',
        'support-category-feature': 'Feature Request',
        'support-category-other': 'Other',
        'loading-badge': '⏳ Loading',
        'theme-toggle': 'Toggle Theme',
        'menu-label': 'Menu',
    },
    ja: {
        // Navbar
        'nav-search': '章を検索...',
        'nav-checkin': 'チェックイン',
        'nav-daily': '毎日',
        'nav-games': 'ゲーム',
        'nav-tools': 'ツール',
        'nav-teacher': '先生を探す',
        'nav-login': 'ログイン',
        'nav-progress': '完了',
        'nav-home': 'ホーム',
        'nav-my': 'マイ',
        'nav-detective': 'コード探偵',
        'nav-speed': 'スピードチャレンジ',
        'nav-puzzle': 'コードパズル',
        'nav-arena': 'コードアリーナ',
        'nav-analytics': '学習分析',
        'nav-flashcard': '暗記カード',
        'nav-snippet': 'コードスニペット',
        'nav-qna': 'Q&A',
        'nav-buddy': '学習パートナー',
        'nav-leaderboard': 'ランキング',

        // Hero
        'hero-badge': '.NET 完全学習パス',
        'hero-title-1': 'ゼロから、',
        'hero-title-2': '.NET を完全マスター',
        'hero-desc': 'C# / ASP.NET Core / データベース / デザインパターン / Docker / AI\n詳細解説、コード例、よくある���違い分析とクイズ付き',
        'hero-start': '学習開始',
        'hero-challenge': 'ランダム挑戦',
        'hero-scores': '成績一覧',
        'stat-chapters': '章',
        'stat-categories': 'カテゴリ',
        'stat-free': '完全無料',

        // Features
        'section-features': 'プラットフォーム機能',
        'section-features-desc': 'オールインワン学習＋交流＋マッチング、すべて無料',
        'feat-chapters': '学習章',
        'feat-chapters-desc': 'コード例とエラー分析付きの完全チュートリアル',
        'feat-games': 'ゲーム',
        'feat-games-desc': 'スピードチ���レンジ、探偵、パズル、毎���、アリーナ',
        'feat-teacher': '先生',
        'feat-teacher-desc': '先生を探す、レッスン予約、マンツーマン指導',
        'feat-checkin': '毎日チェックイン',
        'feat-checkin-desc': '連続チェックインでポイント獲得、14種の実績解除',
        'feat-analytics': '学習分析',
        'feat-analytics-desc': 'ヒートマップ、弱点分析、学習時間統計',
        'feat-community': 'コミュニティ',
        'feat-community-desc': 'Q&A、アイデア共有、ライブチャット、DM',
        'feat-tools': '学習ツール',
        'feat-tools-desc': '暗記カー���、コードスニペット、ノート共有',
        'feat-leaderboard': 'ランキング',
        'feat-leaderboard-desc': 'スコアランキング、レベルバッジ、学習競争',

        // Promo sections
        'promo-path-title': 'ゼロからの完全学習パス',
        'promo-path-desc': '82章でC#、ASP.NET Core、デー���ベース、Docker、AIなどをカバー。各章にコード例、よくある間違い分析、クイズ付き。',
        'promo-path-link': '学習開始 \u2192',
        'promo-teacher-title': 'あ��たに最適な先生を見つけよう',
        'promo-teacher-desc': 'プロによる��ンツーマン指導。レベルに合わせたカスタムレッスン。チャット、予約管理、学習追跡対応。',
        'promo-teacher-link': '先生を探す \u2192',
        'promo-game-title': '遊びながら学ぶ、退��しない',
        'promo-game-desc': 'スピードチャレンジ、コード探偵、パズル、毎日の挑戦...ゲーム化学習でランキング競争。',
        'promo-game-link': '挑戦開始 \u2192',

        // Sections
        'section-core': 'コア学習パス',
        'section-core-desc': 'この3つのテーマを順番に学んで.NETの基礎を固めよう',
        'section-advanced': '上級モジュール',
        'section-advanced-desc': 'コアパス修了後、必要に応じて深掘り',
        'section-challenge': 'ランダ��挑戦',
        'section-challenge-desc': 'レッスンなしで実力テスト！ランダム出題であなたのレベルをチェック',
        'section-ideas': 'ナレッジウォール',
        'section-ideas-desc': 'みんなの学習メモ、気づき、アイデアを見よう',
        'section-ideas-btn': 'アイデアを共有',
        'section-teacher-posts': '先生の無料シェア',
        'section-teacher-posts-desc': 'プラットフォーム講師による最新の無料記事・動画・リソース',
        'challenge-quick': 'クイックチャレンジ',
        'challenge-quick-time': '3分',
        'challenge-standard': '標準クイズ',
        'challenge-standard-time': '8分',
        'challenge-full': '���ルチャレンジ',
        'challenge-full-time': '15分',

        // Developer
        'dev-title': 'こんにちは！Mike です、DevLearn へようこそ',
        'dev-signature': '\u2014\u2014 Mike（邱瀚賢）| DevLearn 開発者',

        // Footer
        'footer-title': 'DevLearn \u00B7 .NET 学習��ラットフォーム',
        'footer-home': 'ホーム',
        'footer-leaderboard': 'ランキング',
        'footer-scores': '成績',

        // Chapter
        'ch-sidebar': '目次',
        'ch-back': '\u2190 ホーム',
        'ch-share-idea': 'アイデア共有',
        'ch-practice': '練習',
        'ch-share': '共有',
        'ch-quiz': 'クイズ',
        'ch-complete': '完了にする',
        'ch-completed': '完了済み',
        'ch-prev': '前の章',
        'ch-next': '次の章',
        'ch-search': '検索...',
        'ch-ideas-title': 'みんなのアイデア',
        'ch-sidebar-toggle': '目次',

        // Login/Register
        'login-title': 'ログイン',
        'login-subtitle': 'ニックネームとメールでログイン',
        'login-nickname': 'ニックネーム',
        'login-email': 'メール',
        'login-btn': 'ログイン',
        'login-no-account': 'アカウントがない？',
        'login-register': '登録',
        'login-home': 'ホーム',
        'register-title': 'アカウント登録',
        'register-subtitle': '学習アカウントを作成',
        'register-btn': '登録',
        'register-has-account': 'すでに���カウントがある？',
        'register-login': 'ログイン',

        // Games
        'game-checkin': '毎日チェックイン',
        'game-checkin-title': '毎日チェックイン & 実績',
        'game-checkin-desc': '毎日チェックインでポイント獲得、実績解除',
        'game-checkin-btn': 'チェックイン',
        'game-checkin-done': 'チェックイン済み',
        'game-daily': '毎日の���戦',
        'game-speed': 'スピードチャレンジ',
        'game-speed-start': '挑戦開始！',
        'game-detective': 'コード探偵',
        'game-puzzle': 'コードクロスワードパズル',
        'game-arena': 'コードアリーナ',
        'game-arena-desc': '他の学習者とプログラミングスキルを競おう',
        'game-start': '挑戦開始',
        'game-submit': '回答を提出',

        // Teacher
        'teacher-title': '先生を探す',
        'teacher-subtitle': 'あなたに最適な.NET先生を見��けよう',
        'teacher-search': '先生の名前、スキルを検索...',
        'teacher-search-btn': '検索',
        'teacher-all': 'すべて',
        'teacher-book': '体験レッスンを予約',
        'teacher-contact': '先生に連絡',
        'teacher-apply': '先生に応募',

        // Profile
        'profile-title': 'プロフィール',
        'profile-home': 'ホーム',
        'profile-section-personal': '個人センター',
        'profile-info': 'プロフィール',
        'profile-ideas': '私のアイデア',
        'profile-qna': '私のQ&A',
        'profile-chat': '私のチャット',
        'profile-quiz': '私のクイズ',
        'profile-logout': 'ログアウト',

        // Chat
        'chat-title': 'チャット',
        'chat-header': 'ライブチャット',
        'chat-public': '公���チャット',
        'chat-private': 'DM',
        'chat-support': 'サポート',
        'chat-send': '送信',
        'chat-placeholder': 'メッセージを入力...',
        'chat-nickname-label': 'ニックネームを設定',
        'chat-avatar-label': '���バターを選択',
        'chat-join': 'チャットに参加',
        'chat-pm-placeholder': 'DMを入力...',
        'chat-support-new': '新しい問題',
        'chat-support-submit': 'レポート提出',
        'chat-support-tickets': '私のチケット',

        // Common
        'btn-save': '保存',
        'btn-cancel': 'キャンセル',
        'btn-delete': '削除',
        'btn-edit': '編集',
        'btn-submit': '提出',
        'btn-close': '閉じる',
        'loading': '読み込み中...',
        'no-data': 'データなし',

        // Gesture / Voice
        'gesture-title': 'ジェスチャー制御',
        'voice-title': '音声制御',

        // Analytics / Leaderboard / Scores
        'analytics-title': '学習分析',
        'analytics-desc': '学習履歴を追跡し、弱点を見つけ、継続的に改善',
        'leaderboard-title': 'ランキング',
        'leaderboard-desc': '他の学習者と競争しよう',
        'scores-title': '私の成績',
        'scores-desc': 'クイズの記録と学習進捗を追跡',

        // QnA
        'qna-title': 'Q&Aディスカッショ���',
        'qna-desc': '質問して、みんなで議論して解決しよう',
        'qna-ask': '質問する',

        // Buddy
        'buddy-title': '学習パートナーマッチング',
        'buddy-desc': '志を同じくする学習パートナーを見つけて、一緒に成長しよう！',

        // Flashcard / Snippet
        'flashcard-title': '暗記カード',
        'flashcard-desc': '間隔反復法で重要な概念を効率的に記憶',
        'flashcard-manage': '📋 管理',
        'flashcard-review': '🔄 復習',
        'flashcard-add': '＋ 新規カード',
        'flashcard-total': 'カード総数',
        'flashcard-due': '復習待ち',
        'flashcard-mastery': '習得率',
        'flashcard-empty': 'まだカードがありません。右上のボタンから最初のカードを追加しましょう！',
        'flashcard-flip-hint': 'クリックで裏返す',
        'flashcard-forgot': '忘れた ❌',
        'flashcard-remembered': '覚えた ✅',
        'flashcard-done': 'すごい！復習待ちのカードをすべて完了しました！',
        'flashcard-back-manage': '管理に戻る',
        'flashcard-create-title': '🧠 新規暗記カード',
        'flashcard-front-label': '表面（質問）',
        'flashcard-front-ph': '例：SOLIDのSとは？',
        'flashcard-back-label': '裏面（答え）',
        'flashcard-back-ph': '例：Single Responsibility Principle — 単一責任の原則',
        'flashcard-category-label': 'カテゴリ（任意）',
        'flashcard-category-ph': '例：デザインパターン、C#構文、ASP.NET',
        'flashcard-no-due': '復習が必要なカードはありません！',
        'flashcard-fill-required': '表面と裏面の内容を入力してください',
        'flashcard-confirm-delete': 'このカードを削除してもよろしいですか？',
        'flashcard-review-times': '復習',
        'flashcard-review-unit': '回',
        'snippet-title': 'コードスニペット',
        'snippet-desc': '便利なコードスニペットを保存していつでも参照',
        'snippet-add': '＋ 新規スニペット',
        'snippet-empty': 'まだ保存がありません。章のページで📌をクリックしてコードを保存',
        'snippet-create-title': '📌 新規コードスニペット',
        'snippet-title-label': 'タイトル',
        'snippet-title-ph': '例：LINQ GroupBy の例',
        'snippet-lang-label': 'プログラミング言語',
        'snippet-code-label': 'コード',
        'snippet-code-ph': 'コードを貼り付け...',
        'snippet-note-label': 'メモ（任意）',
        'snippet-note-ph': 'メモや説明...',
        'snippet-fill-required': 'タイトルとコードを入力してください',
        'snippet-confirm-delete': 'このスニペットを削除してもよろしいですか？',
        'snippet-count': '個のスニペット',
        'snippet-delete': '🗑 削除',

        // Account extras
        'login-ph-nickname': 'ニックネームを入力',
        'login-ph-email': 'メールアドレスを入力（非表示で保護）',
        'login-email-hint': '💡 メールは本人確認用、非表示で保護されます',
        'register-ph-email': 'メールアドレスを入力',
        'register-email-hint': '💡 メールは公開されず、ログイン認証にのみ使用',
        'profile-registered': '登録済み会員',
        'profile-anonymous': '匿名ユーザー',
        'profile-total-score': '合計スコア',
        'profile-quizzes-taken': 'クイズ回数',
        'profile-chapters-done': '完了した章',
        'profile-account-info': 'アカウント情報',
        'profile-registered-badge': '登録済み ✅',
        'profile-anon-id': '匿名ID',
        'profile-created-at': '作成日時',
        'profile-last-active': '最終アクティブ',
        'profile-edit-title': '✏️ プロフィール編集',
        'profile-upload-avatar': '📷 アバターをアップロード/変更',
        'profile-nickname-label': '✏️ ニックネーム',
        'profile-email-label': '📧 メール',
        'profile-save-btn': '💾 変更を保存',
        'profile-upgrade-title': '🚀 アカウントアップグレード',
        'profile-upgrade-desc': '登録するとアイデア、Q&A、チャット履歴、クイズ記録を管理できます！',
        'profile-upgrade-btn': '📝 今すぐ登録',
        'profile-fav-teachers': '❤️ お気に入り',
        'profile-bookings': '📅 予約一覧',
        'profile-confirm-logout': 'ログアウトしますか？新しい匿名IDが割り当てられます',
        'profile-sidebar-upgrade': 'アップグレード',

        // Error page
        'error404-title': '404 - ページが見つかりません',
        'error404-msg': 'おっと！このページは冒険に出かけました',
        'error404-desc': 'お探しのページは移動、名前変更、または.NET学習の道で迷子になった可能性があります',
        'error404-home': 'ホームに戻る 🏠',

        // Layout extras
        'support-category-bug': 'バグ報告',
        'support-category-tech': '技術的な問題',
        'support-category-payment': '支払い問題',
        'support-category-teacher': '先生関連',
        'support-category-booking': '予約問題',
        'support-category-feature': '機能リクエスト',
        'support-category-other': 'その他',
        'loading-badge': '⏳ 読み込み中',
        'theme-toggle': 'テーマ切替',
        'menu-label': 'メニュー',
    }
};

// Helper: get translated string for JS usage
function t(key) {
    var lang = localStorage.getItem('lang') || 'zh';
    var dict = translations[lang];
    return (dict && dict[key]) ? dict[key] : (translations.zh[key] || key);
}

function changeLang(lang) {
    localStorage.setItem('lang', lang);
    var tr = translations[lang];
    if (!tr) return;

    document.querySelectorAll('[data-i18n]').forEach(function(el) {
        var key = el.getAttribute('data-i18n');
        if (tr[key]) {
            if (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA') {
                el.placeholder = tr[key];
            } else if (el.tagName === 'OPTION') {
                el.textContent = tr[key];
            } else {
                el.textContent = tr[key];
            }
        }
    });

    // Also handle data-i18n-placeholder
    document.querySelectorAll('[data-i18n-placeholder]').forEach(function(el) {
        var key = el.getAttribute('data-i18n-placeholder');
        if (tr[key]) el.placeholder = tr[key];
    });

    var sel = document.getElementById('langSelect');
    if (sel) sel.value = lang;
}

// Load saved language
(function() {
    var saved = localStorage.getItem('lang');
    if (saved && saved !== 'zh') {
        changeLang(saved);
    }
})();
