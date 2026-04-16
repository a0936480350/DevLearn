import { COLORS, GROUP_COLORS, SQUARE_W } from '../config.js';
const SQUARE_SIZE = SQUARE_W; // backward compat

export default class BootScene extends Phaser.Scene {
    constructor() { super('Boot'); }

    create() {
        // Generate board square textures per group
        for (const [name, info] of Object.entries(GROUP_COLORS)) {
            this._genSquare(`sq_${name}`, info.fill || info);
        }
        this._genSquare('sq_default', COLORS.bgLight);
        this._genSquare('sq_start', 0x2d6a4f);
        this._genSquare('sq_chance', 0x1e3a5f);
        this._genSquare('sq_community', 0x5c2d91);
        this._genSquare('sq_tax', 0x7c2d12);
        this._genSquare('sq_jail', 0x4a4a4a);
        this._genSquare('sq_parking', 0x2d4a2d);

        // Dice faces
        for (let i = 1; i <= 6; i++) {
            this._genDice(i);
        }

        // House textures
        this._genHouse('house1', 0x22c55e, 1);
        this._genHouse('house2', 0xf59e0b, 2);
        this._genHouse('house3', 0xef4444, 3);

        // Player token base
        this._genToken();

        // Particle
        const pg = this.add.graphics();
        pg.fillStyle(0xf59e0b);
        pg.fillCircle(4, 4, 4);
        pg.generateTexture('particle_coin', 8, 8);
        pg.destroy();

        this.scene.start('Menu');
    }

    _genSquare(key, color) {
        const g = this.add.graphics();
        g.fillStyle(color, 0.9);
        g.fillRoundedRect(0, 0, SQUARE_SIZE, SQUARE_SIZE, 6);
        g.lineStyle(1, COLORS.border, 0.8);
        g.strokeRoundedRect(0, 0, SQUARE_SIZE, SQUARE_SIZE, 6);
        g.generateTexture(key, SQUARE_SIZE, SQUARE_SIZE);
        g.destroy();
    }

    _genDice(num) {
        const s = 60;
        const g = this.add.graphics();
        g.fillStyle(0xffffff);
        g.fillRoundedRect(0, 0, s, s, 8);
        g.lineStyle(2, 0x333333);
        g.strokeRoundedRect(0, 0, s, s, 8);
        // Draw dots
        const dots = {
            1: [[30,30]],
            2: [[18,18],[42,42]],
            3: [[18,18],[30,30],[42,42]],
            4: [[18,18],[42,18],[18,42],[42,42]],
            5: [[18,18],[42,18],[30,30],[18,42],[42,42]],
            6: [[18,15],[42,15],[18,30],[42,30],[18,45],[42,45]],
        };
        g.fillStyle(0x1a1a2e);
        for (const [x, y] of dots[num]) {
            g.fillCircle(x, y, 5);
        }
        g.generateTexture(`dice_${num}`, s, s);
        g.destroy();
    }

    _genHouse(key, color, level) {
        const g = this.add.graphics();
        const w = 12 * level, h = 10 * level;
        g.fillStyle(color);
        // Simple house shape
        g.fillRect(2, h/2, w-4, h/2);
        g.fillTriangle(0, h/2, w/2, 0, w, h/2);
        g.generateTexture(key, w, h);
        g.destroy();
    }

    _genToken() {
        const g = this.add.graphics();
        g.fillStyle(0xffffff);
        g.fillCircle(16, 16, 14);
        g.lineStyle(2, 0x000000, 0.3);
        g.strokeCircle(16, 16, 14);
        g.generateTexture('token_base', 32, 32);
        g.destroy();
    }
}
