// Cache-busting version
const V = '20260411j';

async function importWithRetry(url, retries = 2, delayMs = 800) {
    let lastErr;
    for (let i = 0; i <= retries; i++) {
        try {
            return await import(url);
        } catch (e) {
            lastErr = e;
            if (i < retries) await new Promise(r => setTimeout(r, delayMs * (i + 1)));
        }
    }
    throw lastErr;
}

try {
    const { PHASER_CONFIG } = await importWithRetry(`./config.js?v=${V}`);
    const { default: BootScene } = await importWithRetry(`./scenes/BootScene.js?v=${V}`);
    const { default: MenuScene } = await importWithRetry(`./scenes/MenuScene.js?v=${V}`);
    const { default: CharacterSelectScene } = await importWithRetry(`./scenes/CharacterSelectScene.js?v=${V}`);
    const { default: GameScene } = await importWithRetry(`./scenes/GameScene.js?v=${V}`);
    const { default: GameOverScene } = await importWithRetry(`./scenes/GameOverScene.js?v=${V}`);

    PHASER_CONFIG.scene = [BootScene, MenuScene, CharacterSelectScene, GameScene, GameOverScene];

    const game = new Phaser.Game(PHASER_CONFIG);
} catch (e) {
    console.warn('[Monopoly] failed to load game modules', e);
    const root = document.body;
    if (root) {
        const tip = document.createElement('div');
        tip.style.cssText = 'padding:24px;text-align:center;color:#fff;background:rgba(0,0,0,0.7);border-radius:12px;margin:40px auto;max-width:480px;font-family:sans-serif;';
        tip.innerHTML = '遊戲資源載入失敗，請重新整理頁面再試一次。<br><button onclick="location.reload()" style="margin-top:12px;padding:8px 24px;border:none;border-radius:20px;background:#00b894;color:#fff;cursor:pointer;">重新整理</button>';
        root.appendChild(tip);
    }
}
