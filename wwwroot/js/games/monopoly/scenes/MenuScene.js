import { GAME_WIDTH, GAME_HEIGHT, COLORS, FONT } from '../config.js';

export default class MenuScene extends Phaser.Scene {
    constructor() { super('Menu'); }

    create() {
        // Background gradient effect
        const bg = this.add.graphics();
        bg.fillGradientStyle(0x0d1117, 0x0d1117, 0x1a1a2e, 0x1a1a2e);
        bg.fillRect(0, 0, GAME_WIDTH, GAME_HEIGHT);

        // Floating particles
        for (let i = 0; i < 30; i++) {
            const x = Phaser.Math.Between(0, GAME_WIDTH);
            const y = Phaser.Math.Between(0, GAME_HEIGHT);
            const dot = this.add.circle(x, y, Phaser.Math.Between(1, 3), 0x58a6ff, 0.3);
            this.tweens.add({
                targets: dot, y: y - 100, alpha: 0, duration: Phaser.Math.Between(3000, 6000),
                repeat: -1, yoyo: true,
            });
        }

        // Title
        this.add.text(GAME_WIDTH / 2, 200, '🎲 程式碼大富翁', {
            fontSize: '52px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5);

        // Subtitle
        this.add.text(GAME_WIDTH / 2, 270, 'Code Monopoly — Phaser.js Edition', {
            fontSize: '20px', fontFamily: FONT.family, color: '#8b949e',
        }).setOrigin(0.5);

        // Animated tagline
        const tag = this.add.text(GAME_WIDTH / 2, 330, '答題買地．蓋房害人．稱霸程式世界！', {
            fontSize: '18px', fontFamily: FONT.family, color: '#f59e0b',
        }).setOrigin(0.5);
        this.tweens.add({ targets: tag, alpha: 0.5, duration: 1500, yoyo: true, repeat: -1 });

        // Start button
        const btnW = 260, btnH = 60;
        const btn = this.add.graphics();
        btn.fillStyle(0x238636);
        btn.fillRoundedRect(GAME_WIDTH/2 - btnW/2, 420, btnW, btnH, 12);
        btn.lineStyle(2, 0x2ea043);
        btn.strokeRoundedRect(GAME_WIDTH/2 - btnW/2, 420, btnW, btnH, 12);

        const btnText = this.add.text(GAME_WIDTH / 2, 450, '🚀 開始遊戲', {
            fontSize: '24px', fontFamily: FONT.family, color: '#ffffff', fontStyle: 'bold',
        }).setOrigin(0.5);

        const btnZone = this.add.zone(GAME_WIDTH/2, 450, btnW, btnH).setInteractive({ useHandCursor: true });
        btnZone.on('pointerover', () => { btn.clear(); btn.fillStyle(0x2ea043); btn.fillRoundedRect(GAME_WIDTH/2-btnW/2, 420, btnW, btnH, 12); });
        btnZone.on('pointerout', () => { btn.clear(); btn.fillStyle(0x238636); btn.fillRoundedRect(GAME_WIDTH/2-btnW/2, 420, btnW, btnH, 12); btn.lineStyle(2,0x2ea043); btn.strokeRoundedRect(GAME_WIDTH/2-btnW/2, 420, btnW, btnH, 12); });
        btnZone.on('pointerdown', () => this.scene.start('CharacterSelect'));

        // Features
        const features = [
            '🎯 2-4 人本地多人對戰',
            '🏠 買地蓋房，越蓋題目越難',
            '🃏 命運卡 + 機會卡陷害對手',
            '💰 答對賺錢，答錯罰款',
        ];
        features.forEach((f, i) => {
            this.add.text(GAME_WIDTH / 2, 530 + i * 35, f, {
                fontSize: '16px', fontFamily: FONT.family, color: '#8b949e',
            }).setOrigin(0.5);
        });

        // Version
        this.add.text(GAME_WIDTH / 2, GAME_HEIGHT - 30, 'DevLearn Code Monopoly v2.0 — Powered by Phaser 3', {
            fontSize: '12px', fontFamily: FONT.family, color: '#484f58',
        }).setOrigin(0.5);
    }
}
