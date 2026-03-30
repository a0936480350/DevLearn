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
        'nav-checkin': '簽到', 'nav-daily': '每日', 'nav-games': '遊戲',
        'nav-tools': '工具', 'nav-teacher': '找老師', 'nav-login': '登入',
        'hero-badge': '完整 .NET 學習路徑',
        'hero-title': '從零開始，\n完整學會 .NET',
        'hero-desc': 'C# / ASP.NET Core / 資料庫 / 設計模式 / Docker / AI\n每章附詳細說明、程式碼範例、常見錯誤分析與隨機測驗',
        'hero-start': '開始學習', 'hero-challenge': '隨機挑戰', 'hero-scores': '成績總覽',
        'stat-chapters': '章節內容', 'stat-categories': '主題分類', 'stat-free': '完全免費',
        'section-features': '平台功能導覽',
        'section-features-desc': '一站式學習 + 互動 + 媒合，所有功能免費使用',
        'section-core': '核心學習路線',
        'section-advanced': '進階擴充模組',
        'dev-title': '嗨！我是 Mike，歡迎來到 DevLearn',
        'dev-signature': '—— Mike（邱瀚賢）｜DevLearn 開發者',
    },
    en: {
        'nav-checkin': 'Check-in', 'nav-daily': 'Daily', 'nav-games': 'Games',
        'nav-tools': 'Tools', 'nav-teacher': 'Teachers', 'nav-login': 'Login',
        'hero-badge': 'Complete .NET Learning Path',
        'hero-title': 'From Zero to\nMaster .NET',
        'hero-desc': 'C# / ASP.NET Core / Database / Design Patterns / Docker / AI\nDetailed tutorials, code examples, common mistakes & quizzes',
        'hero-start': 'Start Learning', 'hero-challenge': 'Random Challenge', 'hero-scores': 'Scores',
        'stat-chapters': 'Chapters', 'stat-categories': 'Categories', 'stat-free': 'Totally Free',
        'section-features': 'Platform Features',
        'section-features-desc': 'All-in-one learning + interaction + matching, completely free',
        'section-core': 'Core Learning Path',
        'section-advanced': 'Advanced Modules',
        'dev-title': "Hi! I'm Mike, Welcome to DevLearn",
        'dev-signature': '—— Mike (Chiu Han-Hsien) | DevLearn Developer',
    },
    ja: {
        'nav-checkin': 'チェックイン', 'nav-daily': '毎日', 'nav-games': 'ゲーム',
        'nav-tools': 'ツール', 'nav-teacher': '先生を探す', 'nav-login': 'ログイン',
        'hero-badge': '.NET 完全学習パス',
        'hero-title': 'ゼロから\n.NET を完全マスター',
        'hero-desc': 'C# / ASP.NET Core / データベース / デザインパターン / Docker / AI\n詳細解説、コード例、よくある間違い分析とクイズ付き',
        'hero-start': '学習開始', 'hero-challenge': 'ランダム挑戦', 'hero-scores': '成績一覧',
        'stat-chapters': '章', 'stat-categories': 'カテゴリ', 'stat-free': '完全無料',
        'section-features': 'プラットフォーム機能',
        'section-features-desc': 'オールインワン学習 + 交流 + マッチング、すべて無料',
        'section-core': 'コア学習パス',
        'section-advanced': '上級モジュール',
        'dev-title': 'こんにちは！Mike です、DevLearn へようこそ',
        'dev-signature': '—— Mike（邱瀚賢）｜DevLearn 開発者',
    }
};

function changeLang(lang) {
    localStorage.setItem('lang', lang);
    var t = translations[lang];
    if (!t) return;

    // Update all elements with data-i18n attribute
    document.querySelectorAll('[data-i18n]').forEach(function(el) {
        var key = el.getAttribute('data-i18n');
        if (t[key]) {
            if (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA') {
                el.placeholder = t[key];
            } else {
                el.textContent = t[key];
            }
        }
    });

    // Update select
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
