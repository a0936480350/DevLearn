// ── Quiz System ─────────────────────────────────────────
let quizQuestions = [];

async function openQuiz() {
    document.getElementById('quizModal').classList.remove('hidden');
    document.getElementById('modalOverlay').classList.remove('hidden');
    document.getElementById('quizContent').innerHTML = '<div class="quiz-loading">⏳ 載入題目中...</div>';

    // 改用新的 ChapterQuiz API（回傳 JSON）
    const res = await fetch(`/Quiz/ChapterQuiz?chapterId=${CHAPTER_ID}&count=5`);
    const data = await res.json();
    if (!data.questions || data.questions.length === 0) {
        document.getElementById('quizContent').innerHTML = '<div class="quiz-loading">此章節暫無測驗題目</div>';
        return;
    }
    quizQuestions = data.questions;
    renderQuiz();
}

function renderQuiz() {
    const container = document.getElementById('quizContent');
    container.innerHTML = `
        <div id="quiz-questions">
            ${quizQuestions.map((q, i) => `
                <div class="quiz-question">
                    <div class="q-num">Q${i + 1}</div>
                    <div class="q-text">${q.questionText}</div>
                    <div class="q-options">
                        ${q.type === 'fillin' ? `
                            <input type="text" name="q_${q.id}" class="quiz-input" placeholder="輸入答案..." />
                        ` : (q.options || []).map(opt => `
                            <label class="q-option">
                                <input type="radio" name="q_${q.id}" value="${opt.replace(/"/g,'&quot;')}" />
                                <span>${opt}</span>
                            </label>
                        `).join('')}
                    </div>
                </div>
            `).join('')}
        </div>
        <div class="quiz-actions">
            <button class="btn-retry" onclick="closeQuiz()">取消</button>
            <button class="btn-submit" onclick="submitQuiz()">📝 提交答案</button>
        </div>`;
}

async function submitQuiz() {
    const answers = {};
    quizQuestions.forEach(q => {
        if (q.type === 'fillin') {
            const input = document.querySelector(`input[name="q_${q.id}"]`);
            answers[q.id.toString()] = input ? input.value : '';
        } else {
            const checked = document.querySelector(`input[name="q_${q.id}"]:checked`);
            if (checked) answers[q.id.toString()] = checked.value;
        }
    });

    const res = await fetch('/Quiz/SubmitChapterQuiz', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ chapterId: CHAPTER_ID, answers })
    });
    const data = await res.json();
    renderResult(data);

    // 加積分
    try {
        const scoreRes = await fetch('/Leaderboard/AddScore', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ correctCount: data.score, totalCount: data.total })
        });
        const scoreData = await scoreRes.json();
        if (scoreData.earnedPoints > 0) {
            showScoreToast(scoreData.earnedPoints, scoreData.totalScore, scoreData.badgeLevel, scoreData.isPerfect);
        }
    } catch (e) { console.log('Score update failed:', e); }
}

function renderResult(data) {
    const pct = data.percentage;
    const passed = data.passed;
    const color = pct >= 80 ? '#7EE787' : pct >= 60 ? '#E3B341' : '#F78166';
    const msg = passed
        ? (pct >= 80 ? '🎉 太棒了！下一章已解鎖！' : '👍 通過！下一章已解鎖！')
        : '📚 未通過（需 60% 以上），再看一次章節內容吧！';

    document.getElementById('quizContent').innerHTML = `
        <div class="quiz-score">
            <div class="score-circle" style="border-color:${color}">
                <div class="score-num" style="color:${color}">${pct}%</div>
                <div class="score-label">${data.score}/${data.total}</div>
            </div>
            <div style="font-size:.9rem;color:#8B949E;margin-top:.5rem">${msg}</div>
            ${passed ? '<div style="margin-top:.5rem"><span style="background:rgba(126,231,135,.15);color:#7EE787;padding:4px 12px;border-radius:6px;font-size:.8rem;font-weight:700;">✅ PASSED</span></div>' : ''}
        </div>
        ${data.details.map((r, i) => `
            <div class="q-result">
                <div class="q-text">
                    <span class="${r.isCorrect ? 'correct-tag' : 'wrong-tag'}">${r.isCorrect ? '✅' : '❌'}</span>
                    Q${i+1}: ${r.questionText}
                </div>
                <div style="font-size:.82rem;margin-bottom:.3rem;">
                    你的答案：<strong style="color:${r.isCorrect ? '#7EE787' : '#F78166'}">${r.yourAnswer || '（未作答）'}</strong>
                    ${!r.isCorrect ? `&nbsp;&nbsp;正確答案：<strong style="color:#7EE787">${r.correctAnswer}</strong>` : ''}
                </div>
                <div class="explanation">💡 ${r.explanation}</div>
            </div>
        `).join('')}
        <div class="quiz-actions">
            <button class="btn-retry" onclick="openQuiz()">🔄 再試一次</button>
            ${passed ? '<button class="btn-submit" onclick="location.reload()">✅ 繼續學習</button>' : '<button class="btn-submit" onclick="closeQuiz()">關閉</button>'}
        </div>`;
}

function closeQuiz() {
    document.getElementById('quizModal').classList.add('hidden');
    document.getElementById('modalOverlay').classList.add('hidden');
}

function showScoreToast(points, total, badge, isPerfect) {
    const toast = document.getElementById('scoreToast');
    if (!toast) return;
    const badgeIcons = { newbie:'🌱', beginner:'🌿', intermediate:'💎', advanced:'⚡', expert:'🔥', master:'👑' };
    const icon = badgeIcons[badge] || '🌱';
    const text = isPerfect
        ? `🏆 滿分！+${points} 分（含完美獎勵）· 總分 ${total} ${icon}`
        : `+${points} 分 · 總分 ${total} ${icon}`;
    toast.querySelector('.toast-text').textContent = text;
    toast.classList.remove('hidden');
    setTimeout(() => toast.classList.add('hidden'), 4000);
}
