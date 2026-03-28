// ══════════════════════════════════════════════════════
//  想法分享系統 — 統一 Modal 風格
// ══════════════════════════════════════════════════════

// 確保 Modal HTML 存在（動態注入）
(function injectIdeaModal() {
    if (document.getElementById('ideaModal')) return;
    const div = document.createElement('div');
    div.innerHTML = `
    <div id="ideaModal" class="idea-modal" style="display:none;">
        <div class="idea-modal-inner">
            <div class="idea-modal-header">
                <h3 id="ideaModalTitle">💡 分享想法</h3>
                <button onclick="closeIdeaModal()" class="idea-modal-close">✕</button>
            </div>
            <div class="idea-modal-body">
                <div class="idea-form-group">
                    <label>暱稱</label>
                    <input type="text" id="ideaNickname" placeholder="留空 = 匿名學習者" maxlength="20" />
                </div>
                <div class="idea-form-group">
                    <label id="ideaContentLabel">你的想法</label>
                    <textarea id="ideaContent" rows="5" placeholder="分享你的學習心得、筆記、技術發現..." maxlength="1000"></textarea>
                    <div class="idea-char-count"><span id="ideaCharCount">0</span> / 1000</div>
                </div>
                <button id="ideaSubmitBtn" class="idea-submit-btn" onclick="submitIdea()">📤 發布</button>
            </div>
        </div>
    </div>
    <div id="ideaReplyModal" class="idea-modal" style="display:none;">
        <div class="idea-modal-inner" style="max-width:600px;">
            <div class="idea-modal-header">
                <h3>💬 回覆</h3>
                <button onclick="closeReplyModal()" class="idea-modal-close">✕</button>
            </div>
            <div class="idea-modal-body">
                <div id="replyOriginal" class="reply-original"></div>
                <div id="replyList" class="reply-list"></div>
                <div class="idea-form-group" style="margin-top:1rem;">
                    <input type="text" id="replyNickname" placeholder="暱稱（留空=匿名）" maxlength="20" />
                </div>
                <div class="idea-form-group">
                    <textarea id="replyContent" rows="3" placeholder="寫下你的回覆..." maxlength="500"></textarea>
                </div>
                <button class="idea-submit-btn" onclick="submitReply()">💬 回覆</button>
            </div>
        </div>
    </div>`;
    document.body.appendChild(div);

    // 字數計算
    document.getElementById('ideaContent')?.addEventListener('input', function() {
        document.getElementById('ideaCharCount').textContent = this.value.length;
    });
})();

// ── 開啟想法 Modal ──
let _ideaChapterId = null;
function openIdeaForm(chapterId) {
    _ideaChapterId = chapterId || null;
    document.getElementById('ideaModalTitle').textContent = chapterId ? '💡 分享這章的想法' : '💡 分享想法';
    document.getElementById('ideaContentLabel').textContent = chapterId ? '你對這章的心得、筆記或發現' : '分享你的學習心得或技術知識';
    document.getElementById('ideaNickname').value = localStorage.getItem('px_nickname') || '';
    document.getElementById('ideaContent').value = '';
    document.getElementById('ideaCharCount').textContent = '0';
    document.getElementById('ideaModal').style.display = 'flex';
}

function closeIdeaModal() {
    document.getElementById('ideaModal').style.display = 'none';
}

async function submitIdea() {
    const nickname = document.getElementById('ideaNickname').value.trim();
    const content = document.getElementById('ideaContent').value.trim();
    if (!content) { alert('請輸入內容'); return; }

    // 記住暱稱
    if (nickname) localStorage.setItem('px_nickname', nickname);

    const btn = document.getElementById('ideaSubmitBtn');
    btn.disabled = true; btn.textContent = '發布中...';

    try {
        const res = await fetch('/Idea/Post', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nickname: nickname || null, chapterId: _ideaChapterId, content })
        });
        const data = await res.json();
        if (data.success) {
            closeIdeaModal();
            location.reload();
        } else {
            alert(data.error || '發布失敗');
        }
    } catch(e) { alert('發布失敗：' + e.message); }
    finally { btn.disabled = false; btn.textContent = '📤 發布'; }
}

// ── 回覆 Modal ──
let _replyIdeaId = null;
async function openReplyModal(ideaId, authorName, content) {
    _replyIdeaId = ideaId;
    document.getElementById('replyOriginal').innerHTML = `
        <div class="reply-orig-author">👤 ${authorName}</div>
        <div class="reply-orig-content">${content.replace(/</g,'&lt;').replace(/\n/g,'<br>')}</div>
    `;
    document.getElementById('replyNickname').value = localStorage.getItem('px_nickname') || '';
    document.getElementById('replyContent').value = '';
    document.getElementById('replyList').innerHTML = '<div class="idea-loading">載入回覆...</div>';
    document.getElementById('ideaReplyModal').style.display = 'flex';

    // 載入回覆
    try {
        const res = await fetch('/Idea/GetReplies?ideaId=' + ideaId);
        const replies = await res.json();
        const list = document.getElementById('replyList');
        if (replies.length === 0) {
            list.innerHTML = '<div class="idea-empty" style="padding:.5rem;">還沒有回覆</div>';
        } else {
            list.innerHTML = replies.map(r => `
                <div class="reply-item">
                    <span class="reply-author">👤 ${r.nickname}</span>
                    <span class="reply-time">${r.timeAgo}</span>
                    <div class="reply-body">${r.content.replace(/</g,'&lt;').replace(/\n/g,'<br>')}</div>
                </div>
            `).join('');
        }
    } catch(e) { console.log(e); }
}

function closeReplyModal() {
    document.getElementById('ideaReplyModal').style.display = 'none';
}

async function submitReply() {
    const nickname = document.getElementById('replyNickname').value.trim();
    const content = document.getElementById('replyContent').value.trim();
    if (!content) { alert('請輸入回覆內容'); return; }
    if (nickname) localStorage.setItem('px_nickname', nickname);

    try {
        const res = await fetch('/Idea/PostReply', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ ideaId: _replyIdeaId, nickname: nickname || null, content })
        });
        const data = await res.json();
        if (data.success) {
            openReplyModal(_replyIdeaId,
                document.querySelector('.reply-orig-author')?.textContent?.replace('👤 ','') || '',
                document.querySelector('.reply-orig-content')?.textContent || ''
            );
            document.getElementById('replyContent').value = '';
        }
    } catch(e) { alert('回覆失敗'); }
}

// ── 通用：載入章節想法 ──
async function loadChapterIdeas(chapterId) {
    try {
        const res = await fetch('/Idea/ByChapter?chapterId=' + chapterId);
        const ideas = await res.json();
        const list = document.getElementById('ideaList');
        const count = document.getElementById('ideaCount');
        if (count) count.textContent = ideas.length;
        if (!list) return;
        if (ideas.length === 0) {
            list.innerHTML = '<div class="idea-empty">還沒有想法，成為第一個分享的人！</div>';
            return;
        }
        list.innerHTML = ideas.map(renderIdeaCard).join('');
    } catch(e) { console.log(e); }
}

// ── 通用：載入全站想法牆 ──
async function loadIdeaWall() {
    try {
        const res = await fetch('/Idea/Wall?take=30');
        const ideas = await res.json();
        const wall = document.getElementById('globalIdeaWall');
        if (!wall) return;
        if (ideas.length === 0) {
            wall.innerHTML = '<div class="idea-empty">還沒有人分享，成為第一個！</div>';
            return;
        }
        wall.innerHTML = ideas.map(i => renderIdeaCard(i, true)).join('');
    } catch(e) { console.log(e); }
}

function renderIdeaCard(i, showChapter) {
    const esc = s => (s||'').replace(/</g,'&lt;').replace(/\n/g,'<br>');
    const rc = i.replyCount || 0;
    return `<div class="idea-card">
        <div class="idea-header">
            <span class="idea-author">👤 ${esc(i.nickname)}</span>
            ${rc > 0 ? '<span class="idea-reply-count-badge">\u{1F4AC} ' + rc + ' \u5247\u56DE\u8986</span>' : ''}
            <span class="idea-time">${i.timeAgo}</span>
        </div>
        ${showChapter && i.chapterTitle ? '<div class="idea-chapter">📖 ' + esc(i.chapterTitle) + '</div>' : ''}
        <div class="idea-body">${esc(i.content)}</div>
        <div class="idea-actions">
            <button class="idea-like" onclick="likeIdea(${i.id}, this)">❤️ <span>${i.likes}</span></button>
            <button class="idea-reply-btn" onclick="openReplyModal(${i.id}, '${esc(i.nickname).replace(/'/g,"\\'")}', '${esc(i.content).replace(/'/g,"\\'")}')">💬 回覆</button>
            ${rc > 0 ? '<button class="idea-reply-btn idea-view-replies-btn" onclick="toggleInlineReplies(' + i.id + ', this)">\u{1F50D} \u67E5\u770B\u56DE\u8986</button>' : ''}
        </div>
        <div class="idea-inline-replies" id="replies-${i.id}" style="display:none;"></div>
    </div>`;
}

async function likeIdea(id, btn) {
    const res = await fetch('/Idea/Like', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ ideaId: id })
    });
    const data = await res.json();
    btn.querySelector('span').textContent = data.likes;
}

// 展開/收起回覆
async function toggleInlineReplies(ideaId, btn) {
    const container = document.getElementById('replies-' + ideaId);
    if (!container) return;
    if (container.style.display !== 'none') {
        container.style.display = 'none';
        btn.textContent = '\u{1F50D} \u67E5\u770B\u56DE\u8986';
        return;
    }
    container.style.display = 'block';
    btn.textContent = '\u{1F50D} \u6536\u8D77\u56DE\u8986';
    container.innerHTML = '<div class="idea-loading" style="padding:8px;font-size:13px;">載入回覆中...</div>';
    try {
        const res = await fetch('/Idea/GetReplies?ideaId=' + ideaId);
        const replies = await res.json();
        if (replies.length === 0) {
            container.innerHTML = '<div class="idea-empty" style="padding:8px;font-size:13px;">還沒有回覆</div>';
        } else {
            const esc = s => (s||'').replace(/</g,'&lt;').replace(/\n/g,'<br>');
            container.innerHTML = replies.map(r => `
                <div class="inline-reply-item">
                    <span class="reply-author">\u{1F464} ${esc(r.nickname)}</span>
                    <span class="reply-time">${r.timeAgo}</span>
                    <div class="reply-body">${esc(r.content)}</div>
                </div>
            `).join('');
        }
    } catch(e) {
        container.innerHTML = '<div class="idea-empty" style="padding:8px;font-size:13px;">載入失敗</div>';
    }
}

// 首頁用
function postGlobalIdea() { openIdeaForm(null); }
