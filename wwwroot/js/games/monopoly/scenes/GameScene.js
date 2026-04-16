import { GAME_WIDTH, GAME_HEIGHT, WORLD_WIDTH, WORLD_HEIGHT, COLORS, FONT } from '../config.js';
import { BOARD } from '../data/board-data.js';
import BoardManager from '../managers/BoardManager.js';
import GameStateManager from '../managers/GameStateManager.js';
import QuestionManager from '../managers/QuestionManager.js';
import CardManager from '../managers/CardManager.js';
import UIManager from '../managers/UIManager.js';
import EffectsManager from '../managers/EffectsManager.js';

export default class GameScene extends Phaser.Scene {
    constructor() { super('Game'); }
    init(data) { this.playerData = data.players; }

    async create() {
        this.gs = new GameStateManager(this.playerData);
        this.qm = new QuestionManager();
        this.cm = new CardManager(this.gs);
        this.board = new BoardManager(this);
        this.ui = new UIManager(this);
        this.fx = new EffectsManager(this);
        this.tokens = [];
        this.turnPhase = 'idle';

        // World camera
        this.cameras.main.setBounds(0, 0, WORLD_WIDTH, WORLD_HEIGHT);

        // Draw board
        this.board.drawBoard();

        // Create player tokens on square 0
        this.gs.players.forEach((p, i) => {
            const pos = this.board.getPosition(0);
            const ox = (i % 2) * 22 - 11;
            const oy = Math.floor(i / 2) * 22 - 11;
            const token = this.add.container(pos.x + ox, pos.y + oy).setDepth(20);
            const glow = this.add.graphics();
            glow.fillStyle(p.character.color, 0.25);
            glow.fillCircle(0, 0, 22);
            token.add(glow);
            const base = this.add.image(0, 0, 'token_base').setTint(p.character.color).setScale(1.2);
            const emoji = this.add.text(0, -1, p.character.emoji, { fontSize: '20px' }).setOrigin(0.5);
            token.add([base, emoji]);
            this.tokens[i] = token;
            this.fx.bounceIn(token, i * 200);
        });

        // ═══ Dice button — world-space, repositioned in update() ═══
        this._createDiceButton();

        // HUD
        this.ui.createHUD(this.gs.players, this.gs.currentPlayerIndex);
        this.ui.createTurnInfo(this.gs.currentPlayer, this.gs.roundNumber);
        this.ui.createLog(this.gs.log);

        // Init questions
        this.qm.init();

        // Camera: center on starting position (square 0)
        const startPos = this.board.getPosition(0);
        this.cameras.main.setScroll(
            startPos.x - GAME_WIDTH / 2,
            startPos.y - GAME_HEIGHT / 2 + 50
        );

        this.gs.addLog(`🎮 遊戲開始！${this.gs.currentPlayer.character.emoji} ${this.gs.currentPlayer.name} 先手`);
        this._refreshUI();

        if (this.gs.currentPlayer.isAI) {
            this.time.delayedCall(1000, () => this._aiTurn());
        }
    }

    // ═══ Dice button (world-space container, repositioned in update) ═══
    _createDiceButton() {
        this.diceContainer = this.add.container(0, 0).setDepth(90);

        const panelBg = this.add.graphics();
        panelBg.fillStyle(0x0d1117, 0.9);
        panelBg.fillRoundedRect(-120, -40, 240, 80, 14);
        panelBg.lineStyle(1, 0x30363d);
        panelBg.strokeRoundedRect(-120, -40, 240, 80, 14);
        this.diceContainer.add(panelBg);

        this.diceSprite = this.add.image(-50, 0, 'dice_1').setScale(1);
        this.diceContainer.add(this.diceSprite);

        const btnBg = this.add.graphics();
        btnBg.fillStyle(0x238636);
        btnBg.fillRoundedRect(0, -20, 120, 40, 8);
        this.diceContainer.add(btnBg);

        this.rollBtnText = this.add.text(60, 0, '🎲 擲骰子', {
            fontSize: '15px', fontFamily: FONT.family, color: '#ffffff', fontStyle: 'bold',
        }).setOrigin(0.5);
        this.diceContainer.add(this.rollBtnText);

        // Interactive zone — in container-local coords, works with native Phaser input
        const zone = this.add.zone(60, 0, 130, 50).setInteractive({ useHandCursor: true });
        zone.on('pointerdown', () => this._rollDice());
        this.diceContainer.add(zone);

        this._startDicePulse();

        // Turn banner container
        this.turnContainer = this.add.container(0, 0).setDepth(90);
        const bannerBg = this.add.graphics();
        bannerBg.fillStyle(0x0d1117, 0.85);
        bannerBg.fillRoundedRect(-180, -17, 360, 34, 10);
        this.turnContainer.add(bannerBg);
        this.turnText = this.add.text(0, 0, '', {
            fontSize: '15px', fontFamily: FONT.family, color: '#58a6ff', fontStyle: 'bold',
        }).setOrigin(0.5);
        this.turnContainer.add(this.turnText);
    }

    // Reposition UI containers to follow camera every frame
    update() {
        const cam = this.cameras.main;
        // Dice: bottom center of viewport
        this.diceContainer.setPosition(
            cam.scrollX + GAME_WIDTH / 2,
            cam.scrollY + GAME_HEIGHT - 80
        );
        // Turn banner: top center
        this.turnContainer.setPosition(
            cam.scrollX + GAME_WIDTH / 2,
            cam.scrollY + 25
        );
    }

    _startDicePulse() {
        if (this._dicePulseTween) this._dicePulseTween.destroy();
        this._dicePulseTween = this.tweens.add({
            targets: this.rollBtnText, scaleX: 1.08, scaleY: 1.08,
            duration: 600, yoyo: true, repeat: -1, ease: 'Sine.easeInOut',
        });
    }

    _stopDicePulse() {
        if (this._dicePulseTween) { this._dicePulseTween.destroy(); this._dicePulseTween = null; }
        this.rollBtnText.setScale(1);
    }

    // ═══ Camera scroll (smooth) ═══
    _scrollCameraTo(pos, animate = true) {
        const tx = pos.x - GAME_WIDTH / 2;
        const ty = pos.y - GAME_HEIGHT / 2;
        if (animate) {
            this.tweens.add({
                targets: this.cameras.main, scrollX: tx, scrollY: ty,
                duration: 500, ease: 'Power2',
            });
        } else {
            this.cameras.main.setScroll(tx, ty);
        }
    }

    // ═══ Roll dice ═══
    async _rollDice() {
        if (this.turnPhase !== 'idle') return;
        this.turnPhase = 'rolling';
        this._stopDicePulse();

        const result = Phaser.Math.Between(1, 6);
        for (let i = 0; i < 12; i++) {
            await this._delay(50 + i * 8);
            this.diceSprite.setTexture(`dice_${Phaser.Math.Between(1, 6)}`);
        }
        this.diceSprite.setTexture(`dice_${result}`);
        this.fx.pulseObject(this.diceSprite, 1.4);

        this.gs.addLog(`${this.gs.currentPlayer.character.emoji} 擲出了 ${result}！`);
        this._refreshUI();
        await this._animateMove(this.gs.currentPlayerIndex, result);
        await this._handleLanding();
    }

    // ═══ Movement + camera follow ═══
    async _animateMove(playerIdx, steps) {
        this.turnPhase = 'moving';
        const player = this.gs.players[playerIdx];
        const token = this.tokens[playerIdx];
        const startPos = player.position;
        const ox = (playerIdx % 2) * 22 - 11;
        const oy = Math.floor(playerIdx / 2) * 22 - 11;

        for (let i = 1; i <= steps; i++) {
            const nextIdx = (startPos + i) % BOARD.length;
            const pos = this.board.getPosition(nextIdx);
            // Move camera AND token together
            this._scrollCameraTo(pos);
            await new Promise(resolve => {
                this.tweens.add({
                    targets: token, x: pos.x + ox, y: pos.y + oy,
                    duration: 250, ease: 'Power2', onComplete: resolve,
                });
            });
        }

        const result = this.gs.movePlayer(playerIdx, steps);
        this.board.highlightSquare(result.newPos);
        this._refreshUI();
    }

    // ═══ Handle landing ═══
    async _handleLanding() {
        this.turnPhase = 'landed';
        const player = this.gs.currentPlayer;
        const sq = BOARD[player.position];
        const prop = this.gs.properties[sq.id];

        switch (sq.type) {
            case 'property':
                if (prop.ownerId === null) await this._handleQuestion(sq);
                else if (prop.ownerId !== player.id) await this._handleRent(sq);
                else await this._handleBuild(sq);
                break;
            case 'chance': await this._handleCard('chance'); break;
            case 'community': await this._handleCard('community'); break;
            case 'tax':
                this.gs.applyMoney(player.id, -sq.price);
                this.gs.addLog(`${player.character.emoji} 繳稅 $${sq.price}！`);
                this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, `-$${sq.price}`, '#ef4444');
                this.fx.shakeCamera();
                this._refreshUI();
                break;
            default: break;
        }

        const result = this.gs.checkGameOver();
        if (result.over) {
            this.scene.start('GameOver', { players: this.gs.players, winner: result.winner, reason: result.reason, round: this.gs.roundNumber });
            return;
        }
        await this._endTurn();
    }

    async _handleQuestion(sq) {
        const difficulty = this.gs.getQuestionDifficulty(sq.id);
        const question = await this.qm.getQuestion(difficulty);
        if (!question) return;
        const timeLimit = [0, 15, 12, 10][difficulty] || 15;

        return new Promise(resolve => {
            this.ui.showQuestion(question, difficulty, timeLimit, async (answerIdx) => {
                const player = this.gs.currentPlayer;
                let correct = false;
                if (question.id > 0) {
                    const r = await this.qm.checkAnswer(question.id, answerIdx);
                    correct = r.correct;
                } else {
                    correct = answerIdx === question.correctAnswer;
                }
                const reward = correct ? [0, 50, 100, 150][difficulty] || 50 : -100;
                this.gs.applyMoney(player.id, reward);
                player.stats.questionsAnswered++;
                if (correct) player.stats.questionsCorrect++;
                if (correct) {
                    this.fx.coinBurst(this.tokens[player.id].x, this.tokens[player.id].y);
                    this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, `+$${reward}`, '#22c55e');
                } else {
                    this.fx.shakeCamera();
                    this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, `-$${Math.abs(reward)}`, '#ef4444');
                }
                this.ui.showAnswerResult(correct, reward, () => {
                    if (correct && this.gs.properties[sq.id].ownerId === null && player.money >= sq.price) {
                        this.ui.showBuyPrompt(sq, sq.price, (buy) => {
                            if (buy) {
                                this.gs.buyProperty(player.id, sq.id);
                                this.board.updateOwnership(sq.id, player.character.color);
                                this.fx.coinBurst(this.tokens[player.id].x, this.tokens[player.id].y, 5);
                            }
                            this._refreshUI(); resolve();
                        });
                    } else { this._refreshUI(); resolve(); }
                });
            });
        });
    }

    async _handleRent(sq) {
        const owner = this.gs.getSquareOwner(sq.id);
        const prop = this.gs.properties[sq.id];
        const rent = sq.rent[prop.houses] || sq.rent[0];
        return new Promise(resolve => {
            this.ui.showRentNotice(this.gs.currentPlayer, owner, rent, () => {
                this.gs.payRent(this.gs.currentPlayer.id, sq.id);
                this.fx.floatText(this.tokens[this.gs.currentPlayerIndex].x, this.tokens[this.gs.currentPlayerIndex].y, `-$${rent}`, '#ef4444');
                this.fx.shakeCamera(0.003);
                this._refreshUI(); resolve();
            });
        });
    }

    async _handleBuild(sq) {
        const player = this.gs.currentPlayer;
        if (!this.gs.canBuildHouse(player.id, sq.id)) return;
        const cost = this.gs.getHouseCost(sq.id);
        return new Promise(resolve => {
            this.ui.showBuildPrompt(sq, cost, (build) => {
                if (build) {
                    this.gs.buildHouse(player.id, sq.id);
                    this.board.updateHouses(sq.id, this.gs.properties[sq.id].houses);
                    this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, '🏠', '#22c55e', '24px');
                }
                this._refreshUI(); resolve();
            });
        });
    }

    async _handleCard(type) {
        const card = type === 'chance' ? this.cm.drawChance() : this.cm.drawCommunity();
        return new Promise(resolve => {
            this.ui.showCard(card, () => {
                const r = this.cm.applyCard(card, this.gs.currentPlayer.id);
                if (r.moveChange !== null) {
                    const pos = this.board.getPosition(this.gs.currentPlayer.position);
                    const idx = this.gs.currentPlayerIndex;
                    const ox = (idx % 2) * 22 - 11, oy = Math.floor(idx / 2) * 22 - 11;
                    this.tweens.add({ targets: this.tokens[idx], x: pos.x+ox, y: pos.y+oy, duration: 500, ease: 'Power2' });
                    this._scrollCameraTo(pos);
                }
                if (r.moneyChange > 0) this.fx.coinBurst(this.tokens[this.gs.currentPlayerIndex].x, this.tokens[this.gs.currentPlayerIndex].y);
                if (r.moneyChange < 0) this.fx.shakeCamera(0.003);
                this._refreshUI(); resolve();
            });
        });
    }

    // ═══ End turn ═══
    async _endTurn() {
        this.turnPhase = 'endTurn';
        await this._delay(400);
        let result;
        do {
            result = this.gs.nextTurn();
            this._refreshUI();
            if (result.skipped) await this._delay(800);
        } while (result.skipped);

        // Pan camera to next player
        const nextPos = this.board.getPosition(this.gs.currentPlayer.position);
        this._scrollCameraTo(nextPos);

        this.turnPhase = 'idle';
        if (this.gs.currentPlayer.isAI) {
            this.time.delayedCall(800, () => this._aiTurn());
        } else {
            this._startDicePulse();
        }
    }

    // ═══ Refresh HUD ═══
    _refreshUI() {
        this.ui.createHUD(this.gs.players, this.gs.currentPlayerIndex);
        this.ui.createTurnInfo(this.gs.currentPlayer, this.gs.roundNumber);
        this.ui.createLog(this.gs.log);

        // Turn banner
        this.turnText.setText(`${this.gs.currentPlayer.character.emoji} ${this.gs.currentPlayer.name} — 第 ${this.gs.roundNumber} 回合`);

        // Update board ownership/houses
        BOARD.forEach(sq => {
            if (sq.type === 'property') {
                const prop = this.gs.properties[sq.id];
                const owner = prop.ownerId !== null ? this.gs.players[prop.ownerId] : null;
                this.board.updateOwnership(sq.id, owner ? owner.character.color : null);
                this.board.updateHouses(sq.id, prop.houses);
            }
        });
    }

    // ═══ AI Turn ═══
    async _aiTurn() {
        if (this.turnPhase !== 'idle') return;
        this.turnPhase = 'rolling';
        this._stopDicePulse();
        await this._delay(600);

        const result = Phaser.Math.Between(1, 6);
        for (let i = 0; i < 8; i++) {
            await this._delay(50);
            this.diceSprite.setTexture(`dice_${Phaser.Math.Between(1, 6)}`);
        }
        this.diceSprite.setTexture(`dice_${result}`);
        this.fx.pulseObject(this.diceSprite, 1.4);
        this.gs.addLog(`${this.gs.currentPlayer.character.emoji} 擲出了 ${result}！`);
        this._refreshUI();

        await this._animateMove(this.gs.currentPlayerIndex, result);
        await this._aiLand();
    }

    async _aiLand() {
        this.turnPhase = 'landed';
        const player = this.gs.currentPlayer;
        const sq = BOARD[player.position];
        const prop = this.gs.properties[sq.id];

        switch (sq.type) {
            case 'property':
                if (prop.ownerId === null) {
                    await this._delay(500);
                    const correct = Math.random() < 0.6;
                    const diff = this.gs.getQuestionDifficulty(sq.id);
                    const reward = correct ? [0,50,100,150][diff]||50 : -100;
                    this.gs.applyMoney(player.id, reward);
                    player.stats.questionsAnswered++;
                    if (correct) { player.stats.questionsCorrect++;
                        this.fx.coinBurst(this.tokens[player.id].x, this.tokens[player.id].y);
                        this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, `+$${reward}`, '#22c55e');
                        this.gs.addLog(`${player.character.emoji} 答對！+$${reward}`);
                        if (player.money >= sq.price) { await this._delay(400);
                            this.gs.buyProperty(player.id, sq.id);
                            this.board.updateOwnership(sq.id, player.character.color);
                            this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, '🏠', '#22c55e', '20px');
                        }
                    } else {
                        this.fx.shakeCamera(0.003);
                        this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, `-$100`, '#ef4444');
                        this.gs.addLog(`${player.character.emoji} 答錯！-$100`);
                    }
                } else if (prop.ownerId !== player.id) {
                    const rent = sq.rent[prop.houses]||sq.rent[0];
                    this.gs.payRent(player.id, sq.id);
                    this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, `-$${rent}`, '#ef4444');
                    this.fx.shakeCamera(0.003);
                } else if (this.gs.canBuildHouse(player.id, sq.id)) {
                    await this._delay(300);
                    this.gs.buildHouse(player.id, sq.id);
                    this.board.updateHouses(sq.id, this.gs.properties[sq.id].houses);
                    this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, '🏗️', '#22c55e', '20px');
                }
                break;
            case 'chance': { const c = this.cm.drawChance(); const r = this.cm.applyCard(c, player.id);
                this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, c.icon, '#58a6ff', '24px');
                if (r.moveChange !== null) { const p2 = this.board.getPosition(player.position); const idx = this.gs.currentPlayerIndex;
                    this.tweens.add({targets:this.tokens[idx], x:p2.x+(idx%2)*22-11, y:p2.y+Math.floor(idx/2)*22-11, duration:500, ease:'Power2'});
                    this._scrollCameraTo(p2); }
                await this._delay(500); break; }
            case 'community': { const c = this.cm.drawCommunity(); const r = this.cm.applyCard(c, player.id);
                this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, c.icon, '#a855f7', '24px');
                if (r.moveChange !== null) { const p2 = this.board.getPosition(player.position); const idx = this.gs.currentPlayerIndex;
                    this.tweens.add({targets:this.tokens[idx], x:p2.x+(idx%2)*22-11, y:p2.y+Math.floor(idx/2)*22-11, duration:500, ease:'Power2'});
                    this._scrollCameraTo(p2); }
                await this._delay(500); break; }
            case 'tax':
                this.gs.applyMoney(player.id, -sq.price);
                this.gs.addLog(`${player.character.emoji} 繳稅 $${sq.price}！`);
                this.fx.floatText(this.tokens[player.id].x, this.tokens[player.id].y, `-$${sq.price}`, '#ef4444');
                break;
            default: break;
        }
        this._refreshUI();
        const gr = this.gs.checkGameOver();
        if (gr.over) { this.scene.start('GameOver', {players:this.gs.players,winner:gr.winner,reason:gr.reason,round:this.gs.roundNumber}); return; }
        await this._endTurn();
    }

    _delay(ms) { return new Promise(resolve => this.time.delayedCall(ms, resolve)); }
}
