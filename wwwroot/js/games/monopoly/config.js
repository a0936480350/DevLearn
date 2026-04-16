// ===== Phaser Config & Constants =====
export const GAME_WIDTH = 1200;
export const GAME_HEIGHT = 800;

// World is larger than viewport — camera follows player
export const WORLD_WIDTH = 2400;
export const WORLD_HEIGHT = 1800;

export const COLORS = {
    bg: 0x0d1117,
    bgLight: 0x161b22,
    border: 0x30363d,
    text: 0xe6edf3,
    textDim: 0x8b949e,
    accent: 0x58a6ff,
    gold: 0xf59e0b,
    red: 0xef4444,
    green: 0x22c55e,
    purple: 0xa855f7,
};

export const SQUARE_W = 130;
export const SQUARE_H = 160;
export const CORNER_SIZE = 170;

export const FONT = {
    family: '"Noto Sans TC", "JetBrains Mono", sans-serif',
};

export const GROUP_COLORS = {
    brown:     { fill: 0x8B4513, strip: 0xA0522D, glow: 0xD2691E },
    lightBlue: { fill: 0x1a6985, strip: 0x87CEEB, glow: 0x5BC0DE },
    pink:      { fill: 0x8B3A62, strip: 0xDB7093, glow: 0xFF69B4 },
    orange:    { fill: 0x8B5E00, strip: 0xFF8C00, glow: 0xFFA500 },
    green:     { fill: 0x1B5E20, strip: 0x2E8B57, glow: 0x4CAF50 },
    darkBlue:  { fill: 0x0D2137, strip: 0x1E3A5F, glow: 0x2196F3 },
};

export const PHASER_CONFIG = {
    type: Phaser.AUTO,
    parent: 'game-container',
    width: GAME_WIDTH,
    height: GAME_HEIGHT,
    backgroundColor: '#080c14',
    scale: {
        mode: Phaser.Scale.FIT,
        autoCenter: Phaser.Scale.CENTER_BOTH,
    },
    scene: [],
};
