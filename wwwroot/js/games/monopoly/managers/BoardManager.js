import { WORLD_WIDTH, WORLD_HEIGHT, SQUARE_W, SQUARE_H, CORNER_SIZE, COLORS, GROUP_COLORS, FONT } from '../config.js';
import { BOARD } from '../data/board-data.js';

const TOTAL = BOARD.length; // 36

export default class BoardManager {
    constructor(scene) {
        this.scene = scene;
        this.squares = [];
        this.positions = this._calcPositions();
    }

    _calcPositions() {
        const pos = [];
        // Board center
        const cx = WORLD_WIDTH / 2;
        const cy = WORLD_HEIGHT / 2;

        // Layout: bottom 10, right 8, top 10, left 8 = 36
        // Each side is a line of cards
        const gapX = SQUARE_W + 8;
        const gapY = SQUARE_H + 8;

        // Board dimensions
        const bottomCount = 10, rightCount = 8, topCount = 10, leftCount = 8;
        const boardW = bottomCount * gapX;
        const boardH = 2 * gapY + rightCount * gapY; // top + bottom rows + sides

        const startX = cx - boardW / 2 + gapX / 2;
        const startY = cy + boardH / 2 - gapY / 2;

        let idx = 0;
        // Bottom row: left to right
        for (let i = 0; i < bottomCount; i++) {
            pos[idx++] = { x: startX + i * gapX, y: startY, rotation: 0 };
        }
        // Right column: bottom to top
        const rightX = startX + (bottomCount - 1) * gapX;
        for (let i = 1; i <= rightCount; i++) {
            pos[idx++] = { x: rightX, y: startY - i * gapY, rotation: 0 };
        }
        // Top row: right to left
        const topY = startY - (rightCount) * gapY;
        for (let i = 1; i <= topCount; i++) {
            pos[idx++] = { x: rightX - i * gapX, y: topY, rotation: 0 };
        }
        // Left column: top to bottom
        const leftX = startX;
        for (let i = 1; i <= leftCount; i++) {
            pos[idx++] = { x: leftX, y: topY + i * gapY, rotation: 0 };
        }

        return pos;
    }

    drawBoard() {
        const scene = this.scene;

        // World background — dark with subtle grid
        const worldBg = scene.add.graphics();
        worldBg.fillStyle(0x080c14);
        worldBg.fillRect(0, 0, WORLD_WIDTH, WORLD_HEIGHT);

        // Subtle grid pattern
        worldBg.lineStyle(1, 0x111827, 0.3);
        for (let x = 0; x < WORLD_WIDTH; x += 80) worldBg.lineBetween(x, 0, x, WORLD_HEIGHT);
        for (let y = 0; y < WORLD_HEIGHT; y += 80) worldBg.lineBetween(0, y, WORLD_WIDTH, y);

        // Board area glow
        const bounds = this._getBoardBounds();
        const glow = scene.add.graphics();
        glow.fillStyle(0x0a1628, 0.6);
        glow.fillRoundedRect(bounds.x - 40, bounds.y - 40, bounds.w + 80, bounds.h + 80, 24);

        // Board path line (connecting squares)
        const pathGfx = scene.add.graphics();
        pathGfx.lineStyle(3, 0x1a2332, 0.8);
        pathGfx.beginPath();
        this.positions.forEach((p, i) => {
            if (i === 0) pathGfx.moveTo(p.x, p.y);
            else pathGfx.lineTo(p.x, p.y);
        });
        pathGfx.lineTo(this.positions[0].x, this.positions[0].y);
        pathGfx.strokePath();

        // Draw each square as a card
        BOARD.forEach((sq, i) => {
            const p = this.positions[i];
            if (!p) return;
            const container = scene.add.container(p.x, p.y);

            // Card shadow
            const shadow = scene.add.graphics();
            shadow.fillStyle(0x000000, 0.3);
            shadow.fillRoundedRect(-SQUARE_W/2 + 4, -SQUARE_H/2 + 4, SQUARE_W, SQUARE_H, 12);
            container.add(shadow);

            // Card background
            const bg = scene.add.graphics();
            const groupInfo = sq.group ? GROUP_COLORS[sq.group] : null;
            const fillColor = groupInfo ? groupInfo.fill : this._getTypeColor(sq.type);
            bg.fillStyle(fillColor, 0.95);
            bg.fillRoundedRect(-SQUARE_W/2, -SQUARE_H/2, SQUARE_W, SQUARE_H, 12);
            bg.lineStyle(2, groupInfo ? groupInfo.glow : 0x30363d, 0.6);
            bg.strokeRoundedRect(-SQUARE_W/2, -SQUARE_H/2, SQUARE_W, SQUARE_H, 12);
            container.add(bg);

            // Color strip on top for property groups
            if (groupInfo) {
                const strip = scene.add.graphics();
                strip.fillStyle(groupInfo.strip, 1);
                strip.fillRoundedRect(-SQUARE_W/2, -SQUARE_H/2, SQUARE_W, 18, { tl: 12, tr: 12, bl: 0, br: 0 });
                container.add(strip);
            }

            // Icon (large)
            container.add(scene.add.text(0, -25, sq.icon, { fontSize: '32px' }).setOrigin(0.5));

            // Title
            container.add(scene.add.text(0, 18, sq.title, {
                fontSize: '12px', fontFamily: FONT.family, color: '#e6edf3',
                fontStyle: 'bold', align: 'center',
                wordWrap: { width: SQUARE_W - 16 },
            }).setOrigin(0.5));

            // Price for properties
            if (sq.type === 'property') {
                const priceTag = scene.add.graphics();
                priceTag.fillStyle(0x000000, 0.4);
                priceTag.fillRoundedRect(-30, SQUARE_H/2 - 28, 60, 20, 6);
                container.add(priceTag);
                container.add(scene.add.text(0, SQUARE_H/2 - 18, `$${sq.price}`, {
                    fontSize: '11px', fontFamily: FONT.family, color: '#f59e0b', fontStyle: 'bold',
                }).setOrigin(0.5));
            }

            // Type label for special squares
            if (sq.type !== 'property') {
                const typeLabel = this._getTypeLabel(sq.type);
                if (typeLabel) {
                    container.add(scene.add.text(0, 45, typeLabel, {
                        fontSize: '10px', fontFamily: FONT.family, color: '#8b949e',
                    }).setOrigin(0.5));
                }
            }

            // Square index (small, for debug/reference)
            container.add(scene.add.text(-SQUARE_W/2 + 8, -SQUARE_H/2 + 4, `${i}`, {
                fontSize: '9px', fontFamily: FONT.family, color: '#4a5568',
            }));

            // Ownership indicator
            const ownerIndicator = scene.add.graphics();
            container.add(ownerIndicator);

            // House container
            const houseContainer = scene.add.container(0, -SQUARE_H/2 + 28);
            container.add(houseContainer);

            this.squares[i] = { x: p.x, y: p.y, sq, container, ownerIndicator, houseContainer, bg };
        });

        // Center of board — title
        const cx = WORLD_WIDTH / 2;
        const cy = WORLD_HEIGHT / 2;
        scene.add.text(cx, cy - 30, '🎲', { fontSize: '60px' }).setOrigin(0.5).setAlpha(0.15);
        scene.add.text(cx, cy + 30, '程式碼大富翁', {
            fontSize: '28px', fontFamily: FONT.family, color: '#1a2332', fontStyle: 'bold',
        }).setOrigin(0.5);
    }

    _getTypeColor(type) {
        const map = {
            start: 0x1a4731, chance: 0x1a2a4a, community: 0x2d1a4a,
            tax: 0x4a1a1a, 'jail-visit': 0x2a2a2a, 'free-parking': 0x1a3a1a,
        };
        return map[type] || 0x161b22;
    }

    _getTypeLabel(type) {
        const map = {
            start: '經過領$200', chance: '抽命運卡', community: '開寶箱',
            tax: '繳稅', 'jail-visit': '安全格', 'free-parking': '安全格',
        };
        return map[type] || null;
    }

    _getBoardBounds() {
        let minX = Infinity, minY = Infinity, maxX = -Infinity, maxY = -Infinity;
        this.positions.forEach(p => {
            minX = Math.min(minX, p.x - SQUARE_W/2);
            minY = Math.min(minY, p.y - SQUARE_H/2);
            maxX = Math.max(maxX, p.x + SQUARE_W/2);
            maxY = Math.max(maxY, p.y + SQUARE_H/2);
        });
        return { x: minX, y: minY, w: maxX - minX, h: maxY - minY };
    }

    getPosition(index) {
        return this.positions[index % TOTAL];
    }

    updateOwnership(squareIdx, ownerColor) {
        const sq = this.squares[squareIdx];
        if (!sq) return;
        sq.ownerIndicator.clear();
        if (ownerColor !== null) {
            sq.ownerIndicator.fillStyle(ownerColor, 0.8);
            sq.ownerIndicator.fillRoundedRect(-SQUARE_W/2, SQUARE_H/2 - 10, SQUARE_W, 10, { tl: 0, tr: 0, bl: 12, br: 12 });
        }
    }

    updateHouses(squareIdx, houseCount) {
        const sq = this.squares[squareIdx];
        if (!sq) return;
        sq.houseContainer.removeAll(true);
        for (let i = 0; i < houseCount; i++) {
            const key = `house${Math.min(houseCount, 3)}`;
            const h = this.scene.add.image((i - (houseCount-1)/2) * 18, 0, key).setScale(1);
            sq.houseContainer.add(h);
        }
    }

    highlightSquare(index) {
        const sq = this.squares[index];
        if (!sq) return;
        this.scene.tweens.add({
            targets: sq.container, scaleX: 1.12, scaleY: 1.12,
            duration: 250, yoyo: true, ease: 'Back.easeOut',
        });
    }
}
