import { GAME_WIDTH, GAME_HEIGHT, FONT } from '../config.js';

export default class GameOverScene extends Phaser.Scene {
    constructor() { super('GameOver'); }

    init(data) {
        this.players = data.players;
        this.winner = data.winner;
        this.reason = data.reason;
        this.round = data.round;
    }

    create() {
        this.add.rectangle(GAME_WIDTH/2, GAME_HEIGHT/2, GAME_WIDTH, GAME_HEIGHT, 0x0d1117);

        // Title
        const isWin = !!this.winner;
        this.add.text(GAME_WIDTH/2, 60, isWin ? '🏆 遊戲結束！' : '🎮 遊戲結束', {
            fontSize: '42px', fontFamily: FONT.family, color: '#f59e0b', fontStyle: 'bold',
        }).setOrigin(0.5);

        if (this.winner) {
            this.add.text(GAME_WIDTH/2, 120, `${this.winner.character.emoji} ${this.winner.name} 獲勝！`, {
                fontSize: '28px', fontFamily: FONT.family, color: '#22c55e', fontStyle: 'bold',
            }).setOrigin(0.5);

            const reasonText = this.reason === 'bankrupt' ? '其他玩家全部破產' : `${this.round} 回合後最有錢`;
            this.add.text(GAME_WIDTH/2, 160, reasonText, {
                fontSize: '16px', fontFamily: FONT.family, color: '#8b949e',
            }).setOrigin(0.5);
        }

        // Rankings
        const sorted = [...this.players].sort((a, b) => {
            if (a.bankrupt && !b.bankrupt) return 1;
            if (!a.bankrupt && b.bankrupt) return -1;
            return b.money - a.money;
        });

        this.add.text(GAME_WIDTH/2, 210, '📊 最終排名', {
            fontSize: '20px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5);

        sorted.forEach((p, i) => {
            const y = 255 + i * 80;
            const medal = ['🥇', '🥈', '🥉', '4️⃣'][i] || '';
            const bg = this.add.graphics();
            bg.fillStyle(i === 0 ? 0x1a3a2e : 0x161b22, 0.9);
            bg.fillRoundedRect(GAME_WIDTH/2 - 250, y, 500, 65, 10);
            if (i === 0) { bg.lineStyle(2, 0x22c55e); bg.strokeRoundedRect(GAME_WIDTH/2 - 250, y, 500, 65, 10); }

            this.add.text(GAME_WIDTH/2 - 230, y + 15, `${medal} ${p.character.emoji} ${p.name}`, {
                fontSize: '18px', fontFamily: FONT.family, color: p.bankrupt ? '#484f58' : '#e6edf3', fontStyle: 'bold',
            });

            const statsText = p.bankrupt ? '💀 破產' : `💰 $${p.money}  🏠 ${p.ownedProperties.length}地  ✅ ${p.stats.questionsCorrect}/${p.stats.questionsAnswered}題`;
            this.add.text(GAME_WIDTH/2 - 230, y + 40, statsText, {
                fontSize: '13px', fontFamily: FONT.family, color: '#8b949e',
            });
        });

        // Buttons
        const btnY = GAME_HEIGHT - 60;
        const replayBtn = this.add.graphics();
        replayBtn.fillStyle(0x238636); replayBtn.fillRoundedRect(GAME_WIDTH/2 - 230, btnY - 20, 200, 44, 10);
        this.add.text(GAME_WIDTH/2 - 130, btnY + 2, '🔄 再玩一次', {
            fontSize: '17px', fontFamily: FONT.family, color: '#fff', fontStyle: 'bold',
        }).setOrigin(0.5);
        this.add.zone(GAME_WIDTH/2 - 130, btnY + 2, 200, 44).setInteractive({ useHandCursor: true })
            .on('pointerdown', () => this.scene.start('CharacterSelect'));

        const homeBtn = this.add.graphics();
        homeBtn.fillStyle(0x21262d); homeBtn.fillRoundedRect(GAME_WIDTH/2 + 30, btnY - 20, 200, 44, 10);
        this.add.text(GAME_WIDTH/2 + 130, btnY + 2, '🏠 回首頁', {
            fontSize: '17px', fontFamily: FONT.family, color: '#8b949e', fontStyle: 'bold',
        }).setOrigin(0.5);
        this.add.zone(GAME_WIDTH/2 + 130, btnY + 2, 200, 44).setInteractive({ useHandCursor: true })
            .on('pointerdown', () => window.location.href = '/');
    }
}
