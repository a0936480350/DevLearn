import { CHANCE_CARDS, COMMUNITY_CARDS } from '../data/cards.js';

export default class CardManager {
    constructor(gameState) {
        this.gameState = gameState;
        this.chanceDeck = this._shuffle([...CHANCE_CARDS]);
        this.communityDeck = this._shuffle([...COMMUNITY_CARDS]);
    }

    drawChance() {
        if (this.chanceDeck.length === 0) this.chanceDeck = this._shuffle([...CHANCE_CARDS]);
        return this.chanceDeck.pop();
    }

    drawCommunity() {
        if (this.communityDeck.length === 0) this.communityDeck = this._shuffle([...COMMUNITY_CARDS]);
        return this.communityDeck.pop();
    }

    applyCard(card, playerId) {
        const gs = this.gameState;
        const player = gs.players[playerId];
        const result = { message: card.text, icon: card.icon, moneyChange: 0, moveChange: null, targetPlayer: null };

        switch (card.action) {
            case 'money':
                gs.applyMoney(playerId, card.value);
                result.moneyChange = card.value;
                break;
            case 'moveTo':
                const oldPos = player.position;
                player.position = card.value;
                if (card.value < oldPos) { player.money += 200; result.moneyChange = 200; } // passed GO
                result.moveChange = card.value;
                break;
            case 'move':
                const newPos = (player.position + card.value + 36) % 36;
                player.position = newPos;
                result.moveChange = newPos;
                break;
            case 'steal': {
                const victim = gs.getRandomOpponent(playerId);
                if (victim) {
                    const stolen = Math.min(victim.money, card.value);
                    victim.money -= stolen;
                    player.money += stolen;
                    result.moneyChange = stolen;
                    result.targetPlayer = victim;
                    gs.addLog(`${player.character.emoji} 偷了 ${victim.character.emoji} $${stolen}！`);
                }
                break;
            }
            case 'demolish': {
                const victim = gs.getRandomOpponent(playerId);
                if (victim && victim.ownedProperties.length > 0) {
                    const propId = victim.ownedProperties[Math.floor(Math.random() * victim.ownedProperties.length)];
                    gs.demolishHouse(propId);
                    result.targetPlayer = victim;
                    gs.addLog(`${player.character.emoji} 拆了 ${victim.character.emoji} 的房子！`);
                }
                break;
            }
            case 'freeHouse': {
                const buildable = player.ownedProperties.find(sid => gs.canBuildHouse(playerId, sid) || gs.properties[sid]?.houses < 3);
                if (buildable !== undefined) {
                    gs.properties[buildable].houses = Math.min((gs.properties[buildable]?.houses || 0) + 1, 3);
                    gs.addLog(`${player.character.emoji} 免費蓋了一棟房！`);
                }
                break;
            }
            case 'collectAll': {
                const opponents = gs.activePlayers.filter(p => p.id !== playerId);
                let total = 0;
                opponents.forEach(opp => {
                    const pay = Math.min(opp.money, card.value);
                    opp.money -= pay;
                    total += pay;
                });
                player.money += total;
                result.moneyChange = total;
                gs.addLog(`${player.character.emoji} 向所有對手收取共 $${total}！`);
                break;
            }
            case 'moneyAll': {
                gs.players.filter(p => !p.bankrupt).forEach(p => p.money += card.value);
                gs.addLog(`所有玩家各獲得 $${card.value}！`);
                break;
            }
            case 'pushBack': {
                const victim = gs.getRandomOpponent(playerId);
                if (victim) {
                    victim.position = (victim.position - card.value + 36) % 36;
                    result.targetPlayer = victim;
                    gs.addLog(`${player.character.emoji} 把 ${victim.character.emoji} 推回了 ${card.value} 格！`);
                }
                break;
            }
            case 'jail':
                player.skipTurns = card.value;
                gs.addLog(`${player.character.emoji} 被關進監獄 ${card.value} 回合！`);
                break;
        }

        gs.addLog(`${card.icon} ${card.text}`);
        return result;
    }

    _shuffle(arr) {
        for (let i = arr.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [arr[i], arr[j]] = [arr[j], arr[i]];
        }
        return arr;
    }
}
