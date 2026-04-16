import { GAME_WIDTH, GAME_HEIGHT, COLORS, FONT } from '../config.js';
import { CHARACTERS } from '../data/characters.js';

export default class CharacterSelectScene extends Phaser.Scene {
    constructor() { super('CharacterSelect'); }

    create() {
        this.playerCount = 1; // default: 1 player vs AI
        this.selections = [null, null, null, null];
        this.activeSlot = 0;

        this.add.rectangle(GAME_WIDTH/2, GAME_HEIGHT/2, GAME_WIDTH, GAME_HEIGHT, 0x0d1117);

        this.add.text(GAME_WIDTH/2, 25, '選擇角色', {
            fontSize: '30px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
        }).setOrigin(0.5);

        this.add.text(GAME_WIDTH/2, 60, '遊戲模式', {
            fontSize: '16px', fontFamily: FONT.family, color: '#8b949e',
        }).setOrigin(0.5);

        // Player count buttons (1=vs AI, 2-4=local multiplayer)
        this.countBtnGraphics = [];
        this.countBtnTexts = [];
        const modes = [
            { n: 1, label: '🤖 vs AI' },
            { n: 2, label: '2 人' },
            { n: 3, label: '3 人' },
            { n: 4, label: '4 人' },
        ];
        for (const { n, label } of modes) {
            const x = GAME_WIDTH/2 + (n - 2.5) * 100;
            const bw = n === 1 ? 90 : 60;
            const g = this.add.graphics();
            const t = this.add.text(x, 90, label, {
                fontSize: '14px', fontFamily: FONT.family, color: '#e6edf3', fontStyle: 'bold',
            }).setOrigin(0.5);
            this.countBtnGraphics.push({ g, n, x, bw });
            this.countBtnTexts.push(t);
            const zone = this.add.zone(x, 90, bw, 40).setInteractive({ useHandCursor: true });
            zone.on('pointerdown', () => {
                this.playerCount = n;
                this.selections = [null, null, null, null];
                this.activeSlot = 0;
                this._refresh();
            });
        }

        // Dynamic layers
        this.slotLayer = this.add.container(0, 0);
        this.charLayer = this.add.container(0, 0);

        // Start button
        this.startBtnG = this.add.graphics();
        this.startBtnT = this.add.text(GAME_WIDTH/2, 430, '▶ 開始遊戲', {
            fontSize: '22px', fontFamily: FONT.family, color: '#fff', fontStyle: 'bold',
        }).setOrigin(0.5);
        this.add.zone(GAME_WIDTH/2, 430, 220, 50).setInteractive({ useHandCursor: true })
            .on('pointerdown', () => this._startGame());

        this._refresh();
    }

    _refresh() {
        // Count buttons
        this.countBtnGraphics.forEach(({ g, n, x, bw }) => {
            g.clear();
            const active = n === this.playerCount;
            g.fillStyle(active ? 0x238636 : 0x161b22);
            g.fillRoundedRect(x - bw/2, 105, bw, 40, 8);
            if (active) { g.lineStyle(2, 0x2ea043); g.strokeRoundedRect(x - bw/2, 105, bw, 40, 8); }
        });

        // Player slots
        this.slotLayer.removeAll(true);
        const slotY = 140;
        const humanSlots = this.playerCount === 1 ? 1 : this.playerCount;
        for (let i = 0; i < humanSlots; i++) {
            const x = GAME_WIDTH / 2 + (i - (humanSlots - 1) / 2) * 160;
            const isActive = i === this.activeSlot;
            const sel = this.selections[i];
            const ch = sel ? CHARACTERS.find(c => c.id === sel) : null;

            const g = this.add.graphics();
            g.fillStyle(isActive ? 0x1a3a5c : 0x161b22);
            g.fillRoundedRect(x - 60, slotY, 120, 110, 10);
            g.lineStyle(2, isActive ? 0x58a6ff : 0x30363d);
            g.strokeRoundedRect(x - 60, slotY, 120, 110, 10);
            this.slotLayer.add(g);

            this.slotLayer.add(this.add.text(x, slotY + 15, `P${i + 1}`, {
                fontSize: '14px', fontFamily: FONT.family, color: isActive ? '#58a6ff' : '#8b949e', fontStyle: 'bold',
            }).setOrigin(0.5));

            if (ch) {
                this.slotLayer.add(this.add.text(x, slotY + 55, ch.emoji, { fontSize: '36px' }).setOrigin(0.5));
                this.slotLayer.add(this.add.text(x, slotY + 95, ch.name, {
                    fontSize: '13px', fontFamily: FONT.family, color: '#e6edf3',
                }).setOrigin(0.5));
            } else {
                this.slotLayer.add(this.add.text(x, slotY + 60, '?', {
                    fontSize: '40px', fontFamily: FONT.family, color: '#30363d', fontStyle: 'bold',
                }).setOrigin(0.5));
            }

            const zone = this.add.zone(x, slotY + 65, 120, 130).setInteractive({ useHandCursor: true });
            zone.on('pointerdown', () => { this.activeSlot = i; this._refresh(); });
            this.slotLayer.add(zone);
        }

        // Character grid
        this.charLayer.removeAll(true);
        const gridY = 295;
        const taken = this.selections.filter(s => s !== null);
        CHARACTERS.forEach((ch, idx) => {
            const col = idx % 6;
            const x = GAME_WIDTH / 2 + (col - 2.5) * 110;
            const y = gridY;
            const isTaken = taken.includes(ch.id) && this.selections[this.activeSlot] !== ch.id;
            const isSelected = this.selections[this.activeSlot] === ch.id;

            const g = this.add.graphics();
            g.fillStyle(isSelected ? 0x1a3a5c : isTaken ? 0x0d1117 : 0x161b22, isTaken ? 0.4 : 1);
            g.fillRoundedRect(x - 45, y, 90, 100, 8);
            if (isSelected) { g.lineStyle(2, 0x58a6ff); g.strokeRoundedRect(x - 45, y, 90, 100, 8); }
            this.charLayer.add(g);

            this.charLayer.add(this.add.text(x, y + 30, ch.emoji, {
                fontSize: '32px',
            }).setOrigin(0.5).setAlpha(isTaken ? 0.3 : 1));

            this.charLayer.add(this.add.text(x, y + 70, ch.name, {
                fontSize: '12px', fontFamily: FONT.family, color: isTaken ? '#484f58' : '#e6edf3',
            }).setOrigin(0.5));

            if (!isTaken) {
                const zone = this.add.zone(x, y + 50, 90, 100).setInteractive({ useHandCursor: true });
                zone.on('pointerdown', () => {
                    this.selections[this.activeSlot] = ch.id;
                    for (let i = this.activeSlot + 1; i < this.playerCount; i++) {
                        if (!this.selections[i]) { this.activeSlot = i; break; }
                    }
                    this._refresh();
                });
                this.charLayer.add(zone);
            }
        });

        // VS AI hint
        if (this.playerCount === 1) {
            this.slotLayer.add(this.add.text(GAME_WIDTH/2, slotY + 145, '🤖 你將對戰 AI 電腦', {
                fontSize: '14px', fontFamily: FONT.family, color: '#f59e0b',
            }).setOrigin(0.5));
        }

        // Start button
        const ready = this.selections.slice(0, humanSlots).every(s => s !== null);
        this.startBtnG.clear();
        this.startBtnG.fillStyle(ready ? 0x238636 : 0x21262d);
        this.startBtnG.fillRoundedRect(GAME_WIDTH / 2 - 110, 405, 220, 50, 10);
        this.startBtnT.setColor(ready ? '#ffffff' : '#484f58');
    }

    _startGame() {
        const humanSlots = this.playerCount === 1 ? 1 : this.playerCount;
        const ready = this.selections.slice(0, humanSlots).every(s => s !== null);
        if (!ready) return;
        const players = [];
        const usedIds = [];

        // Human players
        for (let i = 0; i < humanSlots; i++) {
            const ch = CHARACTERS.find(c => c.id === this.selections[i]);
            usedIds.push(ch.id);
            players.push({
                id: i, name: `P${i + 1} ${ch.name}`, character: ch,
                position: 0, money: 3000, ownedProperties: [],
                inJail: false, skipTurns: 0, isAI: false,
            });
        }

        // AI player (for 1-player mode)
        if (this.playerCount === 1) {
            const aiChar = CHARACTERS.find(c => !usedIds.includes(c.id)) || CHARACTERS[1];
            players.push({
                id: 1, name: `🤖 AI ${aiChar.name}`, character: aiChar,
                position: 0, money: 3000, ownedProperties: [],
                inJail: false, skipTurns: 0, isAI: true,
            });
        }

        this.scene.start('Game', { players });
    }
}
