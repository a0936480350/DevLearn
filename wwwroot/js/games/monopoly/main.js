// Cache-busting version
const V = '20260411j';

const { PHASER_CONFIG } = await import(`./config.js?v=${V}`);
const { default: BootScene } = await import(`./scenes/BootScene.js?v=${V}`);
const { default: MenuScene } = await import(`./scenes/MenuScene.js?v=${V}`);
const { default: CharacterSelectScene } = await import(`./scenes/CharacterSelectScene.js?v=${V}`);
const { default: GameScene } = await import(`./scenes/GameScene.js?v=${V}`);
const { default: GameOverScene } = await import(`./scenes/GameOverScene.js?v=${V}`);

PHASER_CONFIG.scene = [BootScene, MenuScene, CharacterSelectScene, GameScene, GameOverScene];

const game = new Phaser.Game(PHASER_CONFIG);
