import { BOARD } from '../data/board-data.js';

export default class GameStateManager {
    constructor(players) {
        this.players = players.map(p => ({
            ...p,
            position: 0,
            money: 3000,
            ownedProperties: [],
            inJail: false,
            skipTurns: 0,
            bankrupt: false,
            isAI: p.isAI || false,
            stats: { questionsAnswered: 0, questionsCorrect: 0, propertiesBought: 0, rentPaid: 0, rentCollected: 0 },
        }));
        this.properties = {};
        BOARD.forEach(sq => {
            if (sq.type === 'property') {
                this.properties[sq.id] = { ownerId: null, houses: 0 };
            }
        });
        this.currentPlayerIndex = 0;
        this.roundNumber = 1;
        this.maxRounds = 20;
        this.log = [];
    }

    get currentPlayer() {
        return this.players[this.currentPlayerIndex];
    }

    get activePlayers() {
        return this.players.filter(p => !p.bankrupt);
    }

    addLog(msg) {
        this.log.unshift(msg);
        if (this.log.length > 20) this.log.pop();
    }

    nextTurn() {
        do {
            this.currentPlayerIndex = (this.currentPlayerIndex + 1) % this.players.length;
            if (this.currentPlayerIndex === 0) this.roundNumber++;
        } while (this.currentPlayer.bankrupt);

        // Handle skip turns (jail etc)
        if (this.currentPlayer.skipTurns > 0) {
            this.currentPlayer.skipTurns--;
            this.addLog(`${this.currentPlayer.character.emoji} ${this.currentPlayer.name} 跳過本回合`);
            return { skipped: true };
        }
        return { skipped: false };
    }

    movePlayer(playerId, steps) {
        const player = this.players[playerId];
        const oldPos = player.position;
        const newPos = (oldPos + steps) % BOARD.length;
        // Check if passed GO
        const passedGo = (oldPos + steps) >= BOARD.length;
        if (passedGo) {
            player.money += 200;
            this.addLog(`${player.character.emoji} 經過起點，領取 $200！`);
        }
        player.position = newPos;
        return { oldPos, newPos, passedGo, square: BOARD[newPos] };
    }

    getSquareOwner(squareId) {
        const prop = this.properties[squareId];
        if (!prop || prop.ownerId === null) return null;
        return this.players[prop.ownerId];
    }

    buyProperty(playerId, squareId) {
        const sq = BOARD[squareId];
        const player = this.players[playerId];
        if (!sq || sq.type !== 'property') return false;
        if (this.properties[squareId].ownerId !== null) return false;
        if (player.money < sq.price) return false;
        player.money -= sq.price;
        player.ownedProperties.push(squareId);
        this.properties[squareId].ownerId = playerId;
        player.stats.propertiesBought++;
        this.addLog(`${player.character.emoji} 購買了 ${sq.title}！(-$${sq.price})`);
        return true;
    }

    payRent(payerId, squareId) {
        const prop = this.properties[squareId];
        if (!prop || prop.ownerId === null || prop.ownerId === payerId) return 0;
        const sq = BOARD[squareId];
        const rent = sq.rent[prop.houses] || sq.rent[0];
        const payer = this.players[payerId];
        const owner = this.players[prop.ownerId];
        const actual = Math.min(payer.money, rent);
        payer.money -= actual;
        owner.money += actual;
        payer.stats.rentPaid += actual;
        owner.stats.rentCollected += actual;
        this.addLog(`${payer.character.emoji} 付租金 $${actual} 給 ${owner.character.emoji}！`);
        if (payer.money <= 0) this._bankrupt(payerId);
        return actual;
    }

    canBuildHouse(playerId, squareId) {
        const prop = this.properties[squareId];
        if (!prop || prop.ownerId !== playerId || prop.houses >= 3) return false;
        const sq = BOARD[squareId];
        if (!sq.group) return false;
        // Must own all properties in the group
        const groupSquares = BOARD.filter(s => s.group === sq.group);
        const ownsAll = groupSquares.every(s => this.properties[s.id]?.ownerId === playerId);
        const cost = this.getHouseCost(squareId);
        return ownsAll && this.players[playerId].money >= cost;
    }

    getHouseCost(squareId) {
        const sq = BOARD[squareId];
        // Base cost scales with property price
        return Math.round(sq.price * 0.4);
    }

    buildHouse(playerId, squareId) {
        if (!this.canBuildHouse(playerId, squareId)) return false;
        const cost = this.getHouseCost(squareId);
        this.players[playerId].money -= cost;
        this.properties[squareId].houses++;
        const sq = BOARD[squareId];
        this.addLog(`${this.players[playerId].character.emoji} 在 ${sq.title} 蓋了房子！(Lv${this.properties[squareId].houses})`);
        return true;
    }

    getQuestionDifficulty(squareId) {
        const prop = this.properties[squareId];
        if (!prop) return 1;
        return Math.min(prop.houses + 1, 3);
    }

    applyMoney(playerId, amount) {
        const player = this.players[playerId];
        player.money += amount;
        if (player.money < 0) player.money = 0;
        if (player.money <= 0) this._bankrupt(playerId);
    }

    _bankrupt(playerId) {
        const player = this.players[playerId];
        player.bankrupt = true;
        // Release all properties
        player.ownedProperties.forEach(sid => {
            this.properties[sid].ownerId = null;
            this.properties[sid].houses = 0;
        });
        player.ownedProperties = [];
        this.addLog(`💀 ${player.character.emoji} ${player.name} 破產出局！`);
    }

    demolishHouse(squareId) {
        const prop = this.properties[squareId];
        if (prop && prop.houses > 0) {
            prop.houses--;
            return true;
        }
        return false;
    }

    checkGameOver() {
        const alive = this.activePlayers;
        if (alive.length <= 1) return { over: true, winner: alive[0] || null, reason: 'bankrupt' };
        if (this.roundNumber > this.maxRounds) {
            const richest = alive.reduce((a, b) => a.money > b.money ? a : b);
            return { over: true, winner: richest, reason: 'rounds' };
        }
        return { over: false };
    }

    getRandomOpponent(playerId) {
        const opponents = this.activePlayers.filter(p => p.id !== playerId);
        return opponents.length > 0 ? opponents[Math.floor(Math.random() * opponents.length)] : null;
    }
}
