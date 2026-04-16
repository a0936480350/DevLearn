import { GAME_WIDTH, GAME_HEIGHT, COLORS, FONT } from '../config.js';

export default class UIManager {
    constructor(scene) {
        this.scene = scene;
        this.overlay = null;
        this.modal = null;
    }

    // Helper: make all children of a container fixed to camera
    _fixToCamera(container) {
        container.setScrollFactor(0);
        container.each(child => { if (child.setScrollFactor) child.setScrollFactor(0); });
        return container;
    }

    // ───── Player HUD (right side panel) ─────
    createHUD(players, currentIdx) {
        if (this.hudContainer) this.hudContainer.destroy();
        this.hudContainer = this.scene.add.container(0, 0).setDepth(80).setScrollFactor(0);
        const panelX = GAME_WIDTH - 175;
        const panelW = 165;

        // Title
        this.hudContainer.add(this.scene.add.text(panelX + panelW/2, 15, '🎮 玩家', {
            fontSize: '16px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5));

        players.forEach((p, i) => {
            const y = 45 + i * 105;
            const isActive = i === currentIdx;
            const isBankrupt = p.bankrupt;

            const bg = this.scene.add.graphics();
            bg.fillStyle(isActive ? 0x1a3a5c : 0x161b22, isBankrupt ? 0.4 : 0.9);
            bg.fillRoundedRect(panelX, y, panelW, 95, 8);
            if (isActive) { bg.lineStyle(2, 0x58a6ff); bg.strokeRoundedRect(panelX, y, panelW, 95, 8); }
            this.hudContainer.add(bg);

            // Emoji + name
            this.hudContainer.add(this.scene.add.text(panelX + 10, y + 8, p.character.emoji, { fontSize: '22px' }));
            this.hudContainer.add(this.scene.add.text(panelX + 38, y + 10, p.name, {
                fontSize: '12px', fontFamily: FONT.family, color: isBankrupt ? '#484f58' : '#e6edf3', fontStyle: 'bold',
            }));

            // Money
            this.hudContainer.add(this.scene.add.text(panelX + 10, y + 38, `💰 $${p.money}`, {
                fontSize: '14px', fontFamily: FONT.family, color: isBankrupt ? '#484f58' : '#f59e0b', fontStyle: 'bold',
            }));

            // Properties count
            this.hudContainer.add(this.scene.add.text(panelX + 10, y + 58, `🏠 ${p.ownedProperties.length} 地`, {
                fontSize: '12px', fontFamily: FONT.family, color: '#8b949e',
            }));

            // Position
            this.hudContainer.add(this.scene.add.text(panelX + 80, y + 58, `📍 ${p.position}格`, {
                fontSize: '12px', fontFamily: FONT.family, color: '#8b949e',
            }));

            if (isBankrupt) {
                this.hudContainer.add(this.scene.add.text(panelX + panelW/2, y + 80, '💀 破產', {
                    fontSize: '11px', fontFamily: FONT.family, color: '#ef4444',
                }).setOrigin(0.5));
            }
        });
    }

    // ───── Round / Turn info ─────
    createTurnInfo(player, round) {
        if (this.turnInfo) this.turnInfo.destroy();
        this.turnInfo = this.scene.add.container(0, 0).setDepth(80).setScrollFactor(0);
        const x = GAME_WIDTH - 175, w = 165;
        const y = GAME_HEIGHT - 80;

        const bg = this.scene.add.graphics();
        bg.fillStyle(0x161b22, 0.9);
        bg.fillRoundedRect(x, y, w, 65, 8);
        this.turnInfo.add(bg);
        this.turnInfo.add(this.scene.add.text(x + w/2, y + 15, `第 ${round} 回合`, {
            fontSize: '13px', fontFamily: FONT.family, color: '#8b949e',
        }).setOrigin(0.5));
        this.turnInfo.add(this.scene.add.text(x + w/2, y + 40, `${player.character.emoji} ${player.name} 的回合`, {
            fontSize: '14px', fontFamily: FONT.family, color: '#58a6ff', fontStyle: 'bold',
        }).setOrigin(0.5));
    }

    // ───── Log panel (bottom left) ─────
    createLog(logs) {
        if (this.logContainer) this.logContainer.destroy();
        this.logContainer = this.scene.add.container(0, 0).setDepth(80).setScrollFactor(0);
        const x = 15, y = GAME_HEIGHT - 90, w = 300, h = 80;
        const bg = this.scene.add.graphics();
        bg.fillStyle(0x0d1117, 0.85);
        bg.fillRoundedRect(x, y, w, h, 8);
        this.logContainer.add(bg);
        const recent = logs.slice(0, 4);
        recent.forEach((msg, i) => {
            this.logContainer.add(this.scene.add.text(x + 8, y + 8 + i * 17, msg, {
                fontSize: '11px', fontFamily: FONT.family, color: i === 0 ? '#e6edf3' : '#6e7681',
                wordWrap: { width: w - 16 },
            }));
        });
    }

    // ───── Modal: Overlay + Content ─────
    // Position overlay and modals in WORLD space at camera center
    // so interactive zones work correctly with Phaser's input system
    _getScreenCenter() {
        const cam = this.scene.cameras.main;
        return {
            x: cam.scrollX + GAME_WIDTH / 2,
            y: cam.scrollY + GAME_HEIGHT / 2,
        };
    }

    _showOverlay() {
        const c = this._getScreenCenter();
        this.overlay = this.scene.add.rectangle(c.x, c.y, GAME_WIDTH * 2, GAME_HEIGHT * 2, 0x000000, 0.6)
            .setDepth(95).setInteractive();
    }

    _hideModal() {
        if (this.overlay) { this.overlay.destroy(); this.overlay = null; }
        if (this.modal) { this.modal.destroy(); this.modal = null; }
    }

    // ───── Question Modal ─────
    showQuestion(question, difficulty, timeLimit, onAnswer) {
        this._showOverlay();
        const w = 500, h = 380;
        const { x: cx, y: cy } = this._getScreenCenter();
        this.modal = this.scene.add.container(cx, cy).setDepth(96);

        // BG
        const bg = this.scene.add.graphics();
        bg.fillStyle(0x161b22); bg.fillRoundedRect(-w/2, -h/2, w, h, 16);
        bg.lineStyle(2, 0x58a6ff); bg.strokeRoundedRect(-w/2, -h/2, w, h, 16);
        this.modal.add(bg);

        // Difficulty badge
        const diffText = ['', '🟢 入門', '🟡 中級', '🔴 進階'][difficulty] || '🟢 入門';
        this.modal.add(this.scene.add.text(0, -h/2 + 25, diffText, {
            fontSize: '14px', fontFamily: FONT.family, color: '#8b949e',
        }).setOrigin(0.5));

        // Question text
        this.modal.add(this.scene.add.text(0, -h/2 + 70, question.text, {
            fontSize: '17px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
            wordWrap: { width: w - 60 }, align: 'center',
        }).setOrigin(0.5));

        // Timer bar
        const timerBg = this.scene.add.graphics();
        timerBg.fillStyle(0x21262d); timerBg.fillRoundedRect(-w/2 + 30, -h/2 + 110, w - 60, 8, 4);
        this.modal.add(timerBg);
        const timerFill = this.scene.add.graphics();
        timerFill.fillStyle(0x58a6ff); timerFill.fillRoundedRect(-w/2 + 30, -h/2 + 110, w - 60, 8, 4);
        this.modal.add(timerFill);

        const timerText = this.scene.add.text(w/2 - 40, -h/2 + 107, `${timeLimit}s`, {
            fontSize: '12px', fontFamily: FONT.family, color: '#f59e0b',
        }).setOrigin(0.5);
        this.modal.add(timerText);

        // Timer countdown
        let timeLeft = timeLimit;
        const timerEvent = this.scene.time.addEvent({
            delay: 1000, repeat: timeLimit - 1,
            callback: () => {
                timeLeft--;
                timerText.setText(`${timeLeft}s`);
                const pct = timeLeft / timeLimit;
                timerFill.clear();
                timerFill.fillStyle(pct < 0.3 ? 0xef4444 : 0x58a6ff);
                timerFill.fillRoundedRect(-w/2 + 30, -h/2 + 110, (w - 60) * pct, 8, 4);
                if (timeLeft <= 0) {
                    answered = true;
                    this._hideModal();
                    onAnswer(-1); // timeout
                }
            },
        });

        // Options
        let answered = false;
        const labels = ['A', 'B', 'C', 'D'];
        const options = question.options || [];
        options.forEach((opt, i) => {
            const oy = -h/2 + 145 + i * 52;
            const optBg = this.scene.add.graphics();
            optBg.fillStyle(0x21262d); optBg.fillRoundedRect(-w/2 + 30, oy, w - 60, 44, 8);
            this.modal.add(optBg);
            const label = this.scene.add.text(-w/2 + 55, oy + 22, `${labels[i]}. ${opt}`, {
                fontSize: '15px', fontFamily: FONT.family, color: '#c9d1d9',
            }).setOrigin(0, 0.5);
            this.modal.add(label);
            const zone = this.scene.add.zone(0, oy + 22, w - 60, 44).setInteractive({ useHandCursor: true });
            this.modal.add(zone);
            zone.on('pointerover', () => { if (!answered) { optBg.clear(); optBg.fillStyle(0x30363d); optBg.fillRoundedRect(-w/2+30, oy, w-60, 44, 8); } });
            zone.on('pointerout', () => { if (!answered) { optBg.clear(); optBg.fillStyle(0x21262d); optBg.fillRoundedRect(-w/2+30, oy, w-60, 44, 8); } });
            zone.on('pointerdown', () => {
                if (answered) return;
                answered = true;
                timerEvent.destroy();
                this._hideModal();
                onAnswer(i);
            });
        });
    }

    // ───── Buy Property Modal ─────
    showBuyPrompt(square, price, onDecision) {
        this._showOverlay();
        const w = 400, h = 220;
        const { x: cx, y: cy } = this._getScreenCenter();
        this.modal = this.scene.add.container(cx, cy).setDepth(96);

        const bg = this.scene.add.graphics();
        bg.fillStyle(0x161b22); bg.fillRoundedRect(-w/2, -h/2, w, h, 16);
        bg.lineStyle(2, 0x22c55e); bg.strokeRoundedRect(-w/2, -h/2, w, h, 16);
        this.modal.add(bg);

        this.modal.add(this.scene.add.text(0, -h/2 + 30, '🏠 購買土地？', {
            fontSize: '22px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5));
        this.modal.add(this.scene.add.text(0, -h/2 + 65, `${square.icon} ${square.title}`, {
            fontSize: '18px', fontFamily: FONT.family, color: '#c9d1d9',
        }).setOrigin(0.5));
        this.modal.add(this.scene.add.text(0, -h/2 + 95, `💰 價格：$${price}`, {
            fontSize: '16px', fontFamily: FONT.family, color: '#f59e0b',
        }).setOrigin(0.5));

        // Buy button
        const buyBtn = this.scene.add.graphics();
        buyBtn.fillStyle(0x238636); buyBtn.fillRoundedRect(-w/2 + 30, h/2 - 60, w/2 - 45, 42, 8);
        this.modal.add(buyBtn);
        this.modal.add(this.scene.add.text(-w/4 + 7, h/2 - 39, '✅ 購買', {
            fontSize: '16px', fontFamily: FONT.family, color: '#fff', fontStyle: 'bold',
        }).setOrigin(0.5));
        const buyZone = this.scene.add.zone(-w/4 + 7, h/2 - 39, w/2 - 45, 42).setInteractive({ useHandCursor: true });
        this.modal.add(buyZone);
        buyZone.on('pointerdown', () => { this._hideModal(); onDecision(true); });

        // Skip button
        const skipBtn = this.scene.add.graphics();
        skipBtn.fillStyle(0x21262d); skipBtn.fillRoundedRect(15, h/2 - 60, w/2 - 45, 42, 8);
        this.modal.add(skipBtn);
        this.modal.add(this.scene.add.text(w/4 + 7, h/2 - 39, '⏭ 跳過', {
            fontSize: '16px', fontFamily: FONT.family, color: '#8b949e', fontStyle: 'bold',
        }).setOrigin(0.5));
        const skipZone = this.scene.add.zone(w/4 + 7, h/2 - 39, w/2 - 45, 42).setInteractive({ useHandCursor: true });
        this.modal.add(skipZone);
        skipZone.on('pointerdown', () => { this._hideModal(); onDecision(false); });
    }

    // ───── Rent Payment Modal ─────
    showRentNotice(payer, owner, rent, onDone) {
        this._showOverlay();
        const w = 380, h = 180;
        this.modal = this.scene.add.container(this._getScreenCenter().x, this._getScreenCenter().y).setDepth(96);
        const bg = this.scene.add.graphics();
        bg.fillStyle(0x161b22); bg.fillRoundedRect(-w/2, -h/2, w, h, 16);
        bg.lineStyle(2, 0xef4444); bg.strokeRoundedRect(-w/2, -h/2, w, h, 16);
        this.modal.add(bg);
        this.modal.add(this.scene.add.text(0, -h/2 + 30, '💸 繳納租金！', {
            fontSize: '22px', fontFamily: FONT.family, color: '#ef4444', fontStyle: 'bold',
        }).setOrigin(0.5));
        this.modal.add(this.scene.add.text(0, -h/2 + 70, `向 ${owner.character.emoji} ${owner.name} 支付 $${rent}`, {
            fontSize: '16px', fontFamily: FONT.family, color: '#e6edf3',
        }).setOrigin(0.5));

        const okBtn = this.scene.add.graphics();
        okBtn.fillStyle(0x21262d); okBtn.fillRoundedRect(-60, h/2 - 55, 120, 40, 8);
        this.modal.add(okBtn);
        this.modal.add(this.scene.add.text(0, h/2 - 35, '確定', {
            fontSize: '16px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5));
        const zone = this.scene.add.zone(0, h/2 - 35, 120, 40).setInteractive({ useHandCursor: true });
        this.modal.add(zone);
        zone.on('pointerdown', () => { this._hideModal(); onDone(); });
    }

    // ───── Card Modal ─────
    showCard(card, onDone) {
        this._showOverlay();
        const w = 380, h = 220;
        this.modal = this.scene.add.container(this._getScreenCenter().x, this._getScreenCenter().y).setDepth(96);
        const bg = this.scene.add.graphics();
        const isChance = card.id < 100;
        bg.fillStyle(0x161b22); bg.fillRoundedRect(-w/2, -h/2, w, h, 16);
        bg.lineStyle(2, isChance ? 0x3b82f6 : 0xa855f7);
        bg.strokeRoundedRect(-w/2, -h/2, w, h, 16);
        this.modal.add(bg);

        this.modal.add(this.scene.add.text(0, -h/2 + 25, isChance ? '🃏 命運卡' : '📦 機會寶箱', {
            fontSize: '20px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5));

        this.modal.add(this.scene.add.text(0, -h/2 + 70, card.icon, { fontSize: '40px' }).setOrigin(0.5));

        this.modal.add(this.scene.add.text(0, -h/2 + 120, card.text, {
            fontSize: '16px', fontFamily: FONT.family, color: '#c9d1d9',
            wordWrap: { width: w - 60 }, align: 'center',
        }).setOrigin(0.5));

        const okBtn = this.scene.add.graphics();
        okBtn.fillStyle(0x21262d); okBtn.fillRoundedRect(-60, h/2 - 55, 120, 40, 8);
        this.modal.add(okBtn);
        this.modal.add(this.scene.add.text(0, h/2 - 35, '繼續', {
            fontSize: '16px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5));
        const zone = this.scene.add.zone(0, h/2 - 35, 120, 40).setInteractive({ useHandCursor: true });
        this.modal.add(zone);
        zone.on('pointerdown', () => { this._hideModal(); onDone(); });
    }

    // ───── Build House prompt ─────
    showBuildPrompt(square, cost, onDecision) {
        this._showOverlay();
        const w = 380, h = 200;
        this.modal = this.scene.add.container(this._getScreenCenter().x, this._getScreenCenter().y).setDepth(96);
        const bg = this.scene.add.graphics();
        bg.fillStyle(0x161b22); bg.fillRoundedRect(-w/2, -h/2, w, h, 16);
        bg.lineStyle(2, 0x22c55e); bg.strokeRoundedRect(-w/2, -h/2, w, h, 16);
        this.modal.add(bg);
        this.modal.add(this.scene.add.text(0, -h/2 + 25, '🏗️ 蓋房子？', {
            fontSize: '20px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5));
        this.modal.add(this.scene.add.text(0, -h/2 + 60, `${square.icon} ${square.title}  費用 $${cost}`, {
            fontSize: '15px', fontFamily: FONT.family, color: '#f59e0b',
        }).setOrigin(0.5));
        this.modal.add(this.scene.add.text(0, -h/2 + 85, '蓋房子會增加租金和題目難度！', {
            fontSize: '13px', fontFamily: FONT.family, color: '#8b949e',
        }).setOrigin(0.5));

        // Buttons
        const yesBtn = this.scene.add.graphics();
        yesBtn.fillStyle(0x238636); yesBtn.fillRoundedRect(-w/2+30, h/2-55, w/2-45, 40, 8);
        this.modal.add(yesBtn);
        this.modal.add(this.scene.add.text(-w/4+7, h/2-35, '🏠 蓋！', {
            fontSize: '15px', fontFamily: FONT.family, color: '#fff', fontStyle: 'bold',
        }).setOrigin(0.5));
        const yesZone = this.scene.add.zone(-w/4+7, h/2-35, w/2-45, 40).setInteractive({ useHandCursor: true });
        this.modal.add(yesZone);
        yesZone.on('pointerdown', () => { this._hideModal(); onDecision(true); });

        const noBtn = this.scene.add.graphics();
        noBtn.fillStyle(0x21262d); noBtn.fillRoundedRect(15, h/2-55, w/2-45, 40, 8);
        this.modal.add(noBtn);
        this.modal.add(this.scene.add.text(w/4+7, h/2-35, '⏭ 跳過', {
            fontSize: '15px', fontFamily: FONT.family, color: '#8b949e', fontStyle: 'bold',
        }).setOrigin(0.5));
        const noZone = this.scene.add.zone(w/4+7, h/2-35, w/2-45, 40).setInteractive({ useHandCursor: true });
        this.modal.add(noZone);
        noZone.on('pointerdown', () => { this._hideModal(); onDecision(false); });
    }

    // ───── Answer Result ─────
    showAnswerResult(correct, reward, onDone) {
        this._showOverlay();
        const w = 340, h = 160;
        this.modal = this.scene.add.container(this._getScreenCenter().x, this._getScreenCenter().y).setDepth(96);
        const bg = this.scene.add.graphics();
        bg.fillStyle(0x161b22); bg.fillRoundedRect(-w/2, -h/2, w, h, 16);
        bg.lineStyle(2, correct ? 0x22c55e : 0xef4444);
        bg.strokeRoundedRect(-w/2, -h/2, w, h, 16);
        this.modal.add(bg);
        this.modal.add(this.scene.add.text(0, -h/2 + 35, correct ? '✅ 答對了！' : '❌ 答錯了！', {
            fontSize: '24px', fontFamily: FONT.family, color: correct ? '#22c55e' : '#ef4444', fontStyle: 'bold',
        }).setOrigin(0.5));
        const msg = correct ? `獲得 $${reward}！` : `罰款 $${Math.abs(reward)}！`;
        this.modal.add(this.scene.add.text(0, -h/2 + 75, msg, {
            fontSize: '16px', fontFamily: FONT.family, color: '#e6edf3',
        }).setOrigin(0.5));
        const zone = this.scene.add.zone(0, h/2 - 30, 120, 40).setInteractive({ useHandCursor: true });
        this.modal.add(zone);
        const btn = this.scene.add.graphics();
        btn.fillStyle(0x21262d); btn.fillRoundedRect(-60, h/2-50, 120, 40, 8);
        this.modal.add(btn);
        this.modal.add(this.scene.add.text(0, h/2-30, '繼續', {
            fontSize: '15px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5));
        zone.on('pointerdown', () => { this._hideModal(); onDone(); });
    }
}
