// Page loader
window.addEventListener('load', function() {
    var loader = document.getElementById('pageLoader');
    if (loader) {
        loader.classList.add('hidden');
        setTimeout(function() { loader.remove(); }, 300);
    }
});

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

// Scroll reveal animation
document.addEventListener('DOMContentLoaded', function() {
    var reveals = document.querySelectorAll('.scroll-reveal, .category-card, section > div');

    function checkReveal() {
        reveals.forEach(function(el) {
            var top = el.getBoundingClientRect().top;
            if (top < window.innerHeight - 50) {
                el.classList.add('visible');
            }
        });
    }

    window.addEventListener('scroll', checkReveal);
    checkReveal(); // Initial check
});

// Toast notification
function showToast(message, type) {
    type = type || 'success';
    var colors = { success: '#00b894', error: '#ef4444', info: '#6366f1', warning: '#f59e0b' };
    var icons = { success: '✅', error: '❌', info: 'ℹ️', warning: '⚠️' };

    var toast = document.createElement('div');
    toast.style.cssText = 'position:fixed;top:80px;right:20px;z-index:99999;background:rgba(15,17,23,0.95);border:1px solid ' + colors[type] + ';border-radius:12px;padding:14px 20px;color:#e2e8f0;font-size:14px;max-width:350px;animation:toastSlideIn 0.3s ease-out;backdrop-filter:blur(10px);display:flex;align-items:center;gap:10px;';
    toast.innerHTML = '<span style="font-size:20px;">' + icons[type] + '</span><span>' + message + '</span>';
    document.body.appendChild(toast);

    setTimeout(function() {
        toast.style.animation = 'toastSlideOut 0.3s ease-in forwards';
        setTimeout(function() { toast.remove(); }, 300);
    }, 3000);
}

// Show toast on page load for returning users
if (document.referrer && document.referrer.includes(location.hostname)) {
    // User navigated within the site - subtle transition
} else if (location.pathname === '/') {
    // First visit or direct URL
    setTimeout(function() {
        showToast('歡迎來到 DevLearn！試試左下角的手勢控制 🎥', 'info');
    }, 2000);
}
