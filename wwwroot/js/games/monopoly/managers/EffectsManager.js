import { FONT } from '../config.js';

export default class EffectsManager {
    constructor(scene) {
        this.scene = scene;
    }

    floatText(x, y, text, color = '#f59e0b', size = '18px') {
        const t = this.scene.add.text(x, y, text, {
            fontSize: size, fontFamily: FONT.family, color, fontStyle: 'bold',
        }).setOrigin(0.5).setDepth(100);
        this.scene.tweens.add({
            targets: t, y: y - 60, alpha: 0, duration: 1200,
            ease: 'Power2', onComplete: () => t.destroy(),
        });
    }

    coinBurst(x, y, count = 8) {
        for (let i = 0; i < count; i++) {
            const coin = this.scene.add.image(x, y, 'particle_coin').setDepth(100);
            this.scene.tweens.add({
                targets: coin,
                x: x + Phaser.Math.Between(-80, 80),
                y: y + Phaser.Math.Between(-80, 20),
                alpha: 0, scale: Phaser.Math.FloatBetween(0.5, 1.5),
                duration: 800, ease: 'Power2',
                onComplete: () => coin.destroy(),
            });
        }
    }

    shakeCamera(intensity = 0.005, duration = 200) {
        this.scene.cameras.main.shake(duration, intensity);
    }

    flashScreen(color = 0xef4444, alpha = 0.3, duration = 300) {
        const flash = this.scene.add.rectangle(
            this.scene.cameras.main.centerX, this.scene.cameras.main.centerY,
            this.scene.cameras.main.width, this.scene.cameras.main.height,
            color, alpha
        ).setDepth(99);
        this.scene.tweens.add({
            targets: flash, alpha: 0, duration,
            onComplete: () => flash.destroy(),
        });
    }

    pulseObject(obj, scale = 1.2, duration = 300) {
        this.scene.tweens.add({
            targets: obj, scaleX: scale, scaleY: scale,
            duration: duration / 2, yoyo: true, ease: 'Back.easeOut',
        });
    }

    bounceIn(obj, delay = 0) {
        obj.setScale(0);
        this.scene.tweens.add({
            targets: obj, scaleX: 1, scaleY: 1,
            duration: 400, ease: 'Back.easeOut', delay,
        });
    }
}
