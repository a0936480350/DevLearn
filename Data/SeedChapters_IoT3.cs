using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_IoT3
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── IoT3 Chapter 609 ────────────────────────────
        new() { Id=609, Category="iot", Order=10, Level="beginner", Icon="🔌", Title="Raspberry Pi 硬體組裝與螢幕串接", Slug="raspberry-pi-hardware-assembly-screen", IsPublished=true, Content=@"
# Raspberry Pi 硬體組裝與螢幕串接

## 為什麼要學硬體組裝？

> 💡 **比喻：學煮菜之前，要先認識廚具**
> 你不會連鍋子、瓦斯爐都不認識就開始炒菜吧？
> 同樣地，寫 IoT 程式之前，你得先把硬體組裝起來。
> 這一章就是帶你從「開箱」到「畫面亮起來」的完整流程。

---

## 開箱清單（買什麼）

> 💡 **比喻：煮一道菜需要的食材清單**
> 少了任何一樣，這道菜就煮不成。Pi 也是一樣，每個配件都有它的用途。

```
必買清單：
┌─────────────────────────────────────────────────────┐
│ 1. Raspberry Pi 4/5 本體（推薦 4GB RAM 以上）        │
│    → 這是「大腦」，所有運算都靠它                      │
│                                                     │
│ 2. 電源供應器（USB-C 5V 3A）                         │
│    → 這是「心臟」，供電不足會當機                      │
│                                                     │
│ 3. MicroSD 卡（32GB 以上，推薦 SanDisk）              │
│    → 這是「硬碟」，裝系統和資料                        │
│                                                     │
│ 4. 散熱片 + 風扇（或被動散熱外殼）                     │
│    → 這是「冷氣」，Pi 跑久了會很燙                     │
│                                                     │
│ 5. 外殼                                              │
│    → 這是「衣服」，保護電路板不被碰到                   │
│                                                     │
│ 6. HDMI 線（Micro HDMI → HDMI）                      │
│    → 這是「眼睛的神經」，把畫面送到螢幕                 │
│                                                     │
│ ⚠️ 注意：Pi 4/5 用的是 Micro HDMI，不是一般的 HDMI！  │
│    很多人第一次買錯線，記得確認接頭大小。                │
└─────────────────────────────────────────────────────┘
```

---

## 螢幕類型比較表

> 💡 **比喻：買電視也有分液晶、OLED、投影機，螢幕也有不同類型**

| 類型 | 接法 | 優點 | 缺點 | 適合場景 | 價格 |
|------|------|------|------|---------|------|
| HDMI 螢幕 | HDMI 線直接插 | 最簡單、解析度高 | 佔空間 | POS 主螢幕 | NT$2000-5000 |
| 官方 7 吋觸控螢幕 | DSI 排線 | 觸控、官方支援 | 只有 7 吋 | POS 觸控 | NT$2500 |
| GPIO 3.5/5 吋小螢幕 | GPIO Pin + SPI | 小巧 | 解析度低、需驅動 | 客顯、狀態顯示 | NT$400-800 |
| USB 螢幕 | USB 線 | 即插即用 | 需驅動 | 副螢幕 | NT$1500-3000 |

### 怎麼選？

```
你要做什麼？
│
├─ POS 收銀主螢幕 → 7 吋觸控螢幕（DSI）或 HDMI 觸控螢幕
│
├─ 客人看的顯示器 → GPIO 小螢幕 或 HDMI 第二螢幕
│
├─ 工廠看板 → HDMI 大螢幕（接電視也行）
│
└─ 自己開發測試 → 任何 HDMI 螢幕（家裡的電腦螢幕就行）
```

---

## HDMI 螢幕接法（最簡單）

> 💡 **比喻：跟接電視一樣簡單，HDMI 線兩頭一插就好**

### 步驟

```
步驟 1：插 HDMI 線
┌──────────────┐          ┌──────────────┐
│  Raspberry Pi │          │   HDMI 螢幕   │
│               │          │              │
│ [Micro HDMI]──────────────[HDMI]        │
│               │  HDMI線  │              │
└──────────────┘          └──────────────┘

步驟 2：螢幕接電源（接插頭）

步驟 3：Pi 接電源 → 開機 → 畫面出現！

步驟 4（選配）：觸控螢幕加 USB 線
┌──────────────┐          ┌──────────────┐
│  Raspberry Pi │          │  觸控 HDMI 螢幕│
│               │          │              │
│ [Micro HDMI]──────────────[HDMI]        │
│ [USB-A]───────────────────[USB]         │
│               │  觸控訊號 │              │
└──────────────┘          └──────────────┘
```

### config.txt 設定解析度

```bash
# 編輯 Pi 的設定檔 // 調整螢幕輸出設定
sudo nano /boot/config.txt // 開啟設定檔

# 常用解析度設定 // 根據螢幕選擇
hdmi_group=2 // 設定為 DMT 模式（電腦螢幕用）
hdmi_mode=82 // 設定為 1920x1080 60Hz

# 如果螢幕沒畫面 // 強制輸出 HDMI 訊號
hdmi_force_hotplug=1 // 即使沒偵測到螢幕也強制輸出

# 常用 hdmi_mode 對照 // 選擇適合的解析度
# hdmi_mode=4  → 640x480   // 小螢幕用
# hdmi_mode=16 → 1024x768  // 7 吋螢幕用
# hdmi_mode=82 → 1920x1080 // Full HD
# hdmi_mode=85 → 1280x720  // HD

# 儲存後重新開機 // 讓設定生效
sudo reboot // 重新啟動 Pi
```

---

## 官方 7 吋觸控螢幕接法（DSI 排線）

> 💡 **比喻：這就像把兩台機器用「臍帶」連起來**
> DSI 排線就是 Pi 和螢幕之間的臍帶，傳送畫面訊號。

### 接線圖

```
Pi 本體（背面）
┌──────────────────────┐
│                      │
│  ┌────────────┐      │
│  │ DSI 接口    │      │  ← 扁平排線插這裡
│  └────────────┘      │
│                      │
│  ┌────────────────┐  │
│  │ GPIO 40Pin     │  │  ← 電源線接這裡（5V + GND）
│  │ ○○○○○○○○○○○○○ │  │
│  │ ○○○○○○○○○○○○○ │  │
│  └────────────────┘  │
│                      │
└──────────────────────┘
          ↕ DSI 排線（15pin 扁平排線）
┌──────────────────────┐
│    觸控螢幕控制板     │
│                      │
│  ┌────────────┐      │
│  │ DSI 接口    │      │  ← 排線另一端插這裡
│  └────────────┘      │
│                      │
│  [5V] [GND]          │  ← 跳線從 Pi 的 GPIO 接過來
│                      │
│  ┌────────────────┐  │
│  │                │  │
│  │   7 吋觸控面板  │  │
│  │                │  │
│  └────────────────┘  │
└──────────────────────┘
```

### 詳細步驟

```bash
# 步驟 1：關閉 Pi 電源 // 安全第一！帶電操作可能燒壞
# ⚠️ 一定要先拔電源線！

# 步驟 2：打開 DSI 接口的固定夾 // 輕輕往上撥開
# Pi 和螢幕板上各有一個 DSI 接口
# 固定夾是黑色的小蓋子，往上輕輕掀開

# 步驟 3：插入排線 // 注意金手指方向！
# ⚠️ 金手指（金色接點）朝向接口的接點方向
# Pi 端：金手指朝向 USB 接口那一側
# 螢幕端：金手指朝向螢幕面板那一側
# 如果插反了，螢幕不會亮，但不會燒壞

# 步驟 4：壓下固定夾 // 把排線固定住
# 輕輕按下黑色固定夾，聽到「喀」一聲

# 步驟 5：接電源跳線 // 從 Pi 的 GPIO 供電給螢幕
# 用杜邦線（母對母）連接：
# Pi GPIO Pin 2 (5V)  → 螢幕板的 5V  // 供電（紅線）
# Pi GPIO Pin 6 (GND) → 螢幕板的 GND // 接地（黑線）
```

### GPIO 電源接線對照

```
Pi GPIO 接腳（左上角往下數）：

Pin 1 (3.3V)  ●  ● Pin 2 (5V)   ← 接到螢幕 5V（紅線）
Pin 3 (SDA)   ●  ● Pin 4 (5V)
Pin 5 (SCL)   ●  ● Pin 6 (GND)  ← 接到螢幕 GND（黑線）
Pin 7 (GPIO4) ●  ● Pin 8
...            ...
```

```bash
# 步驟 6：開機 // 系統會自動偵測 DSI 螢幕
sudo reboot // 重新開機

# 步驟 7：如果畫面上下顛倒 // 加一行設定即可
sudo nano /boot/config.txt // 開啟設定檔
# 在最後面加上 // 旋轉螢幕 180 度
display_rotate=2 // 0=正常 1=90度 2=180度 3=270度
sudo reboot // 重新開機讓設定生效

# 確認觸控功能 // 用手指點螢幕看有沒有反應
# 如果觸控沒反應，檢查排線是否插好
```

---

## GPIO 小螢幕接法（SPI）

> 💡 **比喻：這是最麻煩的接法，像拼拼圖一樣，每條線都要對準位置**
> 但好處是螢幕很小、很便宜，適合做「客顯」或狀態顯示。

### Pin 對應表

```
GPIO 小螢幕接線圖：

Raspberry Pi GPIO          3.5 吋 SPI 螢幕
┌────────────────┐         ┌──────────────┐
│                │         │              │
│ Pin 1  (3.3V) ─────────── VCC           │  ← 電源 3.3V
│                │         │              │
│ Pin 6  (GND)  ─────────── GND           │  ← 接地
│                │         │              │
│ Pin 19 (MOSI) ─────────── SDA/DIN       │  ← 資料輸出
│                │         │              │
│ Pin 23 (SCLK) ─────────── SCL/CLK       │  ← 時脈訊號
│                │         │              │
│ Pin 24 (CE0)  ─────────── CS            │  ← 晶片選擇
│                │         │              │
│ Pin 22 (GPIO25)─────────── DC/RS         │  ← 資料/命令切換
│                │         │              │
│ Pin 18 (GPIO24)─────────── RST           │  ← 重置
│                │         │              │
│ Pin 12 (GPIO18)─────────── BL            │  ← 背光控制
│                │         │              │
└────────────────┘         └──────────────┘

⚠️ 重要：VCC 接 3.3V，不是 5V！接 5V 會燒壞螢幕！
```

### 安裝驅動

```bash
# 下載 LCD 驅動程式 // 從 GitHub 取得驅動
git clone https://github.com/goodtft/LCD-show.git // 克隆驅動庫
cd LCD-show // 進入驅動目錄

# 安裝 3.5 吋螢幕驅動 // 會自動修改系統設定並重開機
sudo ./LCD35-show // 安裝 3.5 吋驅動

# 如果要切回 HDMI 輸出 // 改回一般螢幕
sudo ./LCD-hdmi // 切換回 HDMI

# 安裝 5 吋螢幕驅動 // 另一種尺寸的指令
sudo ./LCD5-show // 安裝 5 吋驅動

# 確認螢幕解析度 // 查看目前顯示設定
fbset // 顯示 framebuffer 設定
```

---

## POS 完整硬體接線圖

> 💡 **比喻：這就像組裝一台電腦，每個週邊設備都要插到對的孔**

```
                    ┌──────────────────────────┐
                    │     Raspberry Pi 4       │
                    │                          │
[觸控螢幕] ←─HDMI──│  HDMI 0      USB-A 上左 ──│──→ [熱感應印表機 USB]
                    │  HDMI 1      USB-A 上右 ──│──→ [條碼掃描器 USB]
[電源 5V3A] ──USB-C─│  USB-C       USB-A 下左 ──│──→ [觸控螢幕 USB（觸控訊號）]
                    │              USB-A 下右 ──│──→ [備用 USB 裝置]
[網路線] ──RJ45─────│  LAN         GPIO 40Pin──│──→ [擴充裝置]
                    │              WiFi 天線  ──│──→ [備用無線網路]
                    │  MicroSD 卡（底部）        │
                    └──────────────────────────┘
                           │
                           │ 區域網路 / 網際網路
                           ↓
                    ┌──────────────┐
                    │  雲端 Server  │
                    │  (Railway)   │
                    └──────────────┘

錢箱的接法（特別注意）：
[熱感應印表機] ──RJ11 線── [錢箱]
↑ 錢箱不是直接接 Pi，而是透過印表機的 RJ11 孔控制！
  印表機收到開錢箱指令後，會送電訊號給錢箱彈開。
```

---

## 散熱處理

> 💡 **比喻：Pi 就像一個小暖爐，不散熱的話會過熱關機**
> POS 機要 24 小時運作，散熱特別重要！

```
散熱方案比較：

方案 1：貼散熱片（最便宜，NT$50）
┌──────┐
│ Pi   │
│ [■■] │ ← 小銅片/鋁片貼在 CPU 和 RAM 上
└──────┘
✅ 便宜、安靜
❌ 散熱效果有限，高負載時可能不夠

方案 2：風扇外殼（NT$200-400）
┌──────┐
│ 風扇 │ ← 小風扇主動吹風
│ Pi   │
└──────┘
✅ 散熱效果好
❌ 有風扇噪音，風扇壽命有限

方案 3：鋁合金散熱外殼（NT$300-600）
┌══════════┐
║          ║ ← 整個外殼就是散熱片
║   Pi     ║    透過金屬接觸導熱
║          ║
└══════════┘
✅ 散熱最好、完全無噪音、外觀好看
❌ 價格較高、比較重

🏪 POS 機建議：方案 3（鋁合金外殼）
   原因：24 小時運作、無噪音、散熱足夠
```

### config.txt 溫度監控設定

```bash
# 編輯設定檔 // 加入溫度控制
sudo nano /boot/config.txt // 開啟設定檔

# 溫度到 70°C 時降頻 // 保護 CPU 不過熱
temp_soft_limit=70 // 設定軟體溫度上限

# 溫度到 85°C 時強制關機 // 避免硬體損壞
# （Pi 預設就有這個保護，不用額外設定）

# 查看目前 CPU 溫度 // 隨時監控
vcgencmd measure_temp // 顯示 CPU 溫度

# 持續監控溫度 // 每秒刷新一次
watch -n 1 vcgencmd measure_temp // 即時溫度監控

# 查看 CPU 時脈 // 確認有沒有降頻
vcgencmd measure_clock arm // 顯示 CPU 頻率
```

---

## SD 卡燒錄

> 💡 **比喻：SD 卡就像 Pi 的硬碟，燒錄就像在新電腦裝 Windows**

### 步驟

```bash
# 步驟 1：下載 Raspberry Pi Imager // 官方燒錄工具
# 到 https://www.raspberrypi.com/software/ 下載
# 支援 Windows / macOS / Linux

# 步驟 2：插入 SD 卡到電腦 // 用讀卡機

# 步驟 3：開啟 Raspberry Pi Imager // 選擇三個東西
# (1) 選擇 OS → Raspberry Pi OS Lite (64-bit)  // 無桌面版，適合 Server
# (2) 選擇儲存裝置 → 你的 SD 卡               // 確認不要選錯碟
# (3) 按齒輪⚙️進入進階設定                     // 最重要的步驟！

# 步驟 4：進階設定（⚠️ 一定要設定！） // 不設定的話開機後很難連線
# ☑️ 設定主機名稱 → pos-pi             // 方便在網路上找到
# ☑️ 啟用 SSH → 使用密碼登入            // 遠端連線必備
# ☑️ 設定使用者帳號密碼                  // 帳號: pi  密碼: 你自己設
# ☑️ 設定 WiFi → 輸入 WiFi 名稱和密碼   // 讓 Pi 連上網路
# ☑️ 設定語系 → Asia/Taipei            // 時區設定

# 步驟 5：按「寫入」按鈕 // 開始燒錄（大約 5-10 分鐘）
# 燒錄完成後安全退出 SD 卡

# 步驟 6：把 SD 卡插入 Pi // 金手指朝上，輕輕推入底部插槽
# 然後接上電源 → 開機！

# 步驟 7：從電腦用 SSH 連線 // 確認 Pi 已經開機成功
ssh pi@pos-pi.local // 用主機名稱連線
# 或者用 IP 位址連線 // 如果 .local 找不到
ssh pi@192.168.1.xxx // 用實際 IP 連線
```

---

## 🤔 我這樣寫為什麼會錯？

### 常見錯誤 1：排線插反了

```
❌ 錯誤：DSI 排線金手指方向插反
┌──────────┐
│ DSI 接口  │
│ [排線反插] │ ← 金手指朝上（錯誤！）
└──────────┘

✅ 正確：金手指朝下，面向接口的金屬接點
┌──────────┐
│ DSI 接口  │
│ [排線正插] │ ← 金手指朝下對準接點
└──────────┘

💡 記憶法：金對金（金手指對金屬接點）
```

### 常見錯誤 2：電源不足

```bash
# ❌ 錯誤：用手機充電器（5V 1A） // 電流不夠
# 症狀：Pi 開機後隨機當機、螢幕閃爍、USB 裝置斷線
# 螢幕右上角出現閃電⚡符號 = 電力不足警告

# ✅ 正確：使用 5V 3A 的 USB-C 電源 // 官方推薦規格
# Pi 4 建議 5V 3A（15W）
# Pi 5 建議 5V 5A（25W）
# 接了觸控螢幕 + USB 裝置後，耗電更大！

# 檢查電壓是否正常 // 確認供電穩定
vcgencmd get_throttled // 查看節流狀態
# 回傳 0x0 表示正常 // 任何其他值表示曾經供電不足
```

### 常見錯誤 3：SD 卡太慢

```bash
# ❌ 錯誤：用老舊的 Class 4 SD 卡 // 讀寫速度太慢
# 症狀：開機要 3 分鐘、程式載入超慢、系統卡頓

# ✅ 正確：使用 Class 10 / U3 / A2 等級的 SD 卡
# 推薦品牌：SanDisk Extreme / Samsung EVO
# 容量至少 32GB

# 測試 SD 卡速度 // 確認讀寫效能
sudo dd if=/dev/zero of=/tmp/test bs=4M count=100 oflag=dsync // 測試寫入速度
# 好的卡應該有 40MB/s 以上 // 差的卡可能只有 5MB/s
```

### 常見錯誤 4：GPIO 接錯 Pin 導致短路

```bash
# ❌ 錯誤：5V 和 GND 接反，或 5V 接到 3.3V 裝置 // 可能燒壞零件！
# ⚠️ GPIO 沒有保護電路，接錯就是直接燒！

# ✅ 正確做法：
# 1. 接線前先斷電 // 一定要拔電源
# 2. 對照 GPIO 腳位圖 // 數清楚 Pin 編號
# 3. 先用三用電表量測 // 確認電壓正確
# 4. VCC 通常接紅線 // 統一顏色方便辨識
# 5. GND 通常接黑線 // 接地永遠用黑線

# 查看 GPIO 腳位配置 // 確認目前狀態
pinout // 顯示 Pi 的 GPIO 腳位圖（需要先安裝 gpiozero）
```

### 常見錯誤 5：沒有接地線

```bash
# ❌ 錯誤：只接 VCC（電源）沒接 GND（接地） // 電路不通
# 症狀：裝置完全沒反應，或訊號不穩定

# ✅ 正確：VCC 和 GND 一定要成對 // 就像水管要有進水口和出水口
# 電流從 VCC 流出，經過裝置，再從 GND 流回來
# 沒有 GND 就像水管沒有出水口，水流不動

# 記憶法：有電（VCC）就要有地（GND） // 永遠成對出現
```

---

## 實用小工具

```bash
# 查看 Pi 硬體資訊 // 確認你的 Pi 型號
cat /proc/cpuinfo // 顯示 CPU 資訊

# 查看記憶體使用量 // 確認有多少 RAM
free -h // 顯示記憶體使用狀況

# 查看儲存空間 // 確認 SD 卡剩餘空間
df -h // 顯示磁碟使用狀況

# 查看 USB 裝置 // 確認印表機、掃描器有被偵測到
lsusb // 列出所有 USB 裝置

# 查看 GPIO 狀態 // 確認腳位設定
raspi-gpio get // 顯示所有 GPIO 狀態

# 更新系統 // 保持最新狀態
sudo apt update && sudo apt upgrade -y // 更新所有套件
```
" },

        // ── IoT3 Chapter 610 ────────────────────────────
        new() { Id=610, Category="iot", Order=11, Level="intermediate", Icon="🏗️", Title="從零組裝一台 POS 機實戰", Slug="build-pos-machine-from-scratch", IsPublished=true, Content=@"
# 從零組裝一台 POS 機實戰

## 為什麼要自己組？

> 💡 **比喻：自己組電腦 vs 買品牌電腦**
> 品牌 POS 機動輒 NT$20,000-50,000，但其實裡面的零件跟 Pi 差不多。
> 自己組一台只要 NT$9,000 左右，還能完全客製化！
> 就像自己組電腦一樣，省錢又有成就感。

---

## 購買清單

> 💡 **比喻：去超市買菜前要先列清單，免得漏買**

```
┌─────────────────────────────────────────────────────────┐
│                  POS 機完整購買清單                       │
├──────────────────────┬──────────┬────────────────────────┤
│ 品項                  │ 預估價格  │ 備註                   │
├──────────────────────┼──────────┼────────────────────────┤
│ Raspberry Pi 4 4GB   │ NT$2,000 │ 建議買 4GB 版本        │
│ 7 吋觸控螢幕（官方）   │ NT$2,500 │ 含 DSI 排線           │
│ 熱感應印表機（58mm）   │ NT$1,500 │ USB 介面，ESC/POS 協議 │
│ 條碼掃描器            │ NT$800   │ USB 介面，即插即用     │
│ 錢箱                 │ NT$1,200 │ RJ11 介面，接印表機    │
│ USB-C 電源 5V 3A     │ NT$300   │ 官方認證最好           │
│ MicroSD 卡 32GB      │ NT$200   │ SanDisk Class 10      │
│ 鋁合金散熱外殼        │ NT$500   │ 含散熱柱              │
├──────────────────────┼──────────┼────────────────────────┤
│ 總計                  │ NT$9,000 │ 比傳統 POS 便宜一半！  │
└──────────────────────┴──────────┴────────────────────────┘

🛒 台灣購買管道：
   蝦皮購物 → 搜尋「Raspberry Pi 4」「58mm 熱感應印表機」
   露天拍賣 → 搜尋「樹莓派」「POS 印表機」
   台灣樹莓派官方代理 → https://www.raspberrypi.com.tw/
   光華商場 → 實體店面可以現場看
```

---

## 步驟一：組裝硬體

> 💡 **比喻：就像組樂高，照著說明書一步一步來**

### 1.1 安裝散熱外殼

```
步驟圖解：

(1) 拆開鋁合金外殼 // 通常分上下兩片
┌═══════════════┐
│   上蓋（有散熱柱）│
└═══════════════┘
┌═══════════════┐
│   下蓋（有螺絲孔）│
└═══════════════┘

(2) 把 Pi 放進下蓋 // 對準螺絲孔和接口位置
┌═══════════════┐
│ ┌───────────┐ │
│ │ Pi 本體    │ │
│ └───────────┘ │
└═══════════════┘

(3) 散熱柱對準 CPU 和 RAM // 塗一點散熱膏效果更好
(4) 蓋上上蓋，鎖螺絲 // 不要鎖太緊

完成！
┌═══════════════════════════┐
║                           ║
║  [HDMI][HDMI] [USB][USB]  ║  ← 所有接口都露出來
║  [USB-C][LAN] [USB][USB]  ║
║                           ║
└═══════════════════════════┘
```

### 1.2 接上觸控螢幕

```
(1) DSI 排線連接 Pi 和螢幕板 // 參考上一章的接法
(2) 跳線供電：Pin 2 (5V) → 螢幕 5V // 紅線
              Pin 6 (GND) → 螢幕 GND // 黑線
(3) 用螢幕支架固定螢幕 // 調整到適合的角度

組裝後的樣子（側面圖）：

    ┌─────────────┐
    │  7 吋螢幕    │  ← 面向客人/店員
    │  （觸控）     │
    │             │
    └──┬──────┬──┘
       │      │        ← 螢幕支架
    ┌──┴──────┴──┐
    ║  Pi + 外殼  ║     ← 放在螢幕後面或下面
    ║            ║
    └════════════┘
```

### 1.3 接上所有 USB 裝置

```
Pi USB 接口分配：

USB-A 上左 → 熱感應印表機（USB-B 線）
USB-A 上右 → 條碼掃描器（USB 線）
USB-A 下左 → 觸控螢幕 USB（觸控訊號）
USB-A 下右 → 備用（可接鍵盤 debug）

印表機 RJ11 → 錢箱 RJ11（錢箱線）
```

---

## 步驟二：系統安裝

### 2.1 燒錄系統

```bash
# 使用 Raspberry Pi Imager 燒錄 // 選擇 Raspberry Pi OS Lite 64-bit
# 進階設定：
# - 主機名稱: pos-pi // 方便識別
# - SSH: 啟用 // 遠端管理
# - WiFi: 設定好 // 連網
# - 使用者: pi // 預設帳號
# - 時區: Asia/Taipei // 台灣時區

# 燒好後插入 Pi，接電源開機 // 等待 1-2 分鐘
# 從電腦 SSH 連線進去 // 開始設定
ssh pi@pos-pi.local // 連線到 Pi
```

### 2.2 基本系統設定

```bash
# 更新系統 // 第一件事永遠是更新
sudo apt update // 更新套件清單
sudo apt upgrade -y // 升級所有套件

# 設定固定 IP（方便管理） // 不用每次找 IP
sudo nmcli con mod ""Wired connection 1"" \
  ipv4.addresses 192.168.1.100/24 \
  ipv4.gateway 192.168.1.1 \
  ipv4.dns ""8.8.8.8,8.8.4.4"" \
  ipv4.method manual // 設定固定 IP 為 192.168.1.100

# 重新啟動網路 // 讓設定生效
sudo nmcli con up ""Wired connection 1"" // 套用網路設定

# 確認 IP // 檢查是否正確
ip addr show eth0 // 顯示網路介面資訊
```

### 2.3 安裝 Node.js

```bash
# 安裝 Node.js 20 LTS // Print Agent 需要用到
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash - // 加入 Node.js 套件庫
sudo apt install -y nodejs // 安裝 Node.js

# 確認版本 // 應該顯示 v20.x.x
node --version // 檢查 Node.js 版本
npm --version // 檢查 npm 版本
```

### 2.4 安裝 .NET Runtime

```bash
# 安裝 .NET 8 Runtime // 如果要跑 .NET 應用
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0 --runtime aspnetcore // 安裝 ASP.NET Core Runtime

# 加入環境變數 // 讓系統找得到 dotnet
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc // 設定 .NET 路徑
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc // 加入 PATH
source ~/.bashrc // 重新載入設定

# 確認安裝 // 應該顯示 8.0.x
dotnet --info // 顯示 .NET 資訊
```

### 2.5 安裝 Chromium（Kiosk 用）

```bash
# 安裝桌面環境（最小安裝） // Kiosk 模式需要圖形介面
sudo apt install -y --no-install-recommends xserver-xorg x11-xserver-utils xinit // 安裝 X11 基本元件
sudo apt install -y --no-install-recommends chromium-browser // 安裝 Chromium 瀏覽器
sudo apt install -y --no-install-recommends openbox // 安裝輕量視窗管理器

# 安裝觸控支援 // 讓觸控螢幕可以用
sudo apt install -y xdotool unclutter // 安裝觸控輔助工具
```

---

## 步驟三：設定 Kiosk 模式

> 💡 **比喻：把 Pi 變成一台「專用機」，開機就只顯示 POS 畫面**

```bash
# 建立 Kiosk 啟動腳本 // 開機自動執行
cat > /home/pi/kiosk.sh << 'KIOSK_EOF' // 寫入啟動腳本
#!/bin/bash                              // 指定用 bash 執行
# POS Kiosk 啟動腳本                     // 腳本說明

# 隱藏滑鼠游標                           // 觸控螢幕不需要游標
unclutter -idle 0.5 -root &              // 0.5 秒不動就隱藏游標

# 停用螢幕保護                           // POS 需要螢幕常亮
xset s off                               // 關閉螢幕保護
xset -dpms                               // 關閉電源管理
xset s noblank                           // 不要黑屏

# 啟動 Chromium Kiosk                    // 全螢幕顯示 POS
chromium-browser \
  --kiosk \                              // Kiosk 全螢幕模式
  --noerrdialogs \                       // 不顯示錯誤
  --disable-infobars \                   // 不顯示資訊列
  --no-first-run \                       // 跳過首次設定
  --disable-translate \                  // 不要翻譯提示
  --disable-features=TranslateUI \       // 停用翻譯 UI
  --check-for-update-interval=31536000 \ // 不檢查更新
  --incognito \                          // 無痕模式
  --disable-pinch \                      // 停用手指縮放
  --overscroll-history-navigation=0 \    // 停用滑動換頁
  'http://localhost:5000'                // POS 網址
KIOSK_EOF

# 設定腳本可執行 // 給予執行權限
chmod +x /home/pi/kiosk.sh // 加上執行權限

# 設定自動登入並啟動 Kiosk // 開機自動進入
cat > /home/pi/.xinitrc << 'XINIT_EOF' // 寫入 X11 啟動設定
exec openbox-session &                   // 啟動視窗管理器
/home/pi/kiosk.sh                        // 執行 Kiosk 腳本
XINIT_EOF

# 設定自動登入後啟動 X // .bashrc 最後加上
echo '[[ -z \$DISPLAY && \$XDG_VTNR -eq 1 ]] && startx -- -nocursor' >> /home/pi/.bashrc // 自動啟動圖形介面

# 設定自動登入 // 不用打密碼
sudo raspi-config nonint do_boot_behaviour B2 // 設定自動登入到 Console
```

---

## 步驟四：安裝 Print Agent

```bash
# 建立 Print Agent 目錄 // 存放印表機代理程式
mkdir -p /home/pi/print-agent // 建立目錄
cd /home/pi/print-agent // 進入目錄

# 初始化 Node.js 專案 // 建立 package.json
npm init -y // 快速初始化

# 安裝必要套件 // 印表機和 WebSocket 相關
npm install escpos escpos-usb ws express // 安裝 ESC/POS 和通訊套件

# 建立設定檔 // 設定印表機連線參數
cat > config.json << 'CONFIG_EOF' // 寫入設定
{
  ""printer"": {
    ""vendorId"": ""0x0416"",
    ""productId"": ""0x5011""
  },
  ""server"": {
    ""port"": 3001
  }
}
CONFIG_EOF

# 確認印表機 USB 資訊 // 找到 vendorId 和 productId
lsusb // 列出 USB 裝置
# 會看到類似：Bus 001 Device 004: ID 0416:5011 Printer
#                                    ^^^^ ^^^^
#                                    這兩個就是 vendorId 和 productId
```

---

## 步驟五：連接印表機 + 測試列印

```bash
# 給 USB 印表機權限 // 讓 Node.js 可以存取
sudo usermod -a -G lp pi // 把 pi 加入 lp 群組

# 建立 udev 規則 // 讓印表機不用 sudo 也能用
sudo cat > /etc/udev/rules.d/99-printer.rules << 'UDEV_EOF' // 寫入權限規則
SUBSYSTEM==""usb"", ATTR{idVendor}==""0416"", ATTR{idProduct}==""5011"", MODE=""0666"" // 設定 USB 印表機權限
UDEV_EOF

# 重新載入 udev 規則 // 讓規則生效
sudo udevadm control --reload-rules // 重新載入
sudo udevadm trigger // 觸發規則

# 測試直接印列 // 不用程式，直接送文字
echo ""POS 印表機測試成功！"" > /dev/usb/lp0 // 直接送文字到印表機
# 如果印表機有吐出紙張，表示連接成功！

# 用 Python 測試（更可靠） // 快速測試用
python3 -c ""
import usb.core  # 匯入 USB 模組
dev = usb.core.find(idVendor=0x0416, idProduct=0x5011)  # 找到印表機
if dev:  # 如果找到
    print('印表機已偵測到！')  # 顯示成功訊息
else:  # 如果沒找到
    print('找不到印表機，請檢查 USB 連線')  # 顯示錯誤訊息
"" // 執行 Python 測試
```

---

## 步驟六：連接條碼掃描器 + 測試掃描

```bash
# 條碼掃描器通常是 USB HID 裝置 // 就像鍵盤一樣
# 插上 USB 就能用，不需要安裝驅動

# 確認掃描器被偵測到 // 檢查 USB 裝置清單
lsusb // 列出 USB 裝置
# 應該看到類似：Barcode Scanner 或 HID Device

# 測試掃描 // 開一個文字編輯器，掃描條碼
# 掃描器會像鍵盤一樣輸入條碼數字 + Enter
cat // 啟動簡單的文字輸入
# 然後拿條碼掃描器對著商品掃描
# 螢幕上會出現條碼數字，例如：4710088430014
# 按 Ctrl+C 結束

# 查看掃描器的輸入裝置 // 進階偵錯用
cat /proc/bus/input/devices // 列出所有輸入裝置
# 找到 Barcode 或 Scanner 相關的項目

# 測試掃描器的事件 // 確認按鍵訊號
sudo cat /dev/input/event0 // 讀取輸入事件（按 Ctrl+C 結束）
```

---

## 步驟七：連接錢箱 + 測試開啟

```bash
# 錢箱是透過印表機控制的 // 錢箱線接印表機的 RJ11 孔
# 印表機收到特定指令後，會送電訊號給錢箱

# 錢箱接線：
# 熱感應印表機背面有 RJ11 孔（像電話線插孔）
# 用 RJ11 線連接印表機和錢箱

# 測試開錢箱（ESC/POS 指令） // 送開錢箱指令給印表機
python3 -c ""
# ESC/POS 開錢箱指令
import struct  # 匯入結構模組
cmd = b'\x1b\x70\x00\x19\xfa'  # ESC p 0 25 250 開錢箱指令
with open('/dev/usb/lp0', 'wb') as f:  # 開啟印表機裝置
    f.write(cmd)  # 送出開錢箱指令
print('錢箱已開啟！')  # 顯示成功訊息
"" // 執行開錢箱指令

# 如果錢箱沒反應 // 偵錯步驟
# 1. 檢查 RJ11 線有沒有插好 // 兩端都要「喀」一聲
# 2. 試試另一組指令 // 不同印表機指令可能不同
python3 -c ""
cmd = b'\x1b\x70\x01\x19\xfa'  # 試試 Pin 1 而不是 Pin 0
with open('/dev/usb/lp0', 'wb') as f:  # 開啟印表機裝置
    f.write(cmd)  # 送出替代指令
"" // 嘗試替代指令
```

---

## 步驟八：部署 POS Web App

```bash
# 建立 POS 網站目錄 // 存放前端程式
mkdir -p /home/pi/pos-app // 建立目錄

# 方法 1：直接用 git clone // 從你的 Git 倉庫部署
cd /home/pi/pos-app // 進入目錄
git clone https://github.com/your-repo/pos-frontend.git . // 克隆前端專案

# 方法 2：用 scp 從電腦傳輸 // 如果沒有 Git 倉庫
# 在你的電腦上執行：
scp -r ./dist/* pi@pos-pi.local:/home/pi/pos-app/ // 傳輸打包好的檔案

# 安裝 serve 來提供靜態檔案 // 簡單的 HTTP 伺服器
sudo npm install -g serve // 全域安裝 serve

# 用 systemd 管理 POS 服務 // 讓它開機自動啟動
sudo cat > /etc/systemd/system/pos-web.service << 'SERVICE_EOF' // 寫入服務設定
[Unit]                                    // 單元設定
Description=POS Web Application           // 服務描述
After=network.target                      // 在網路啟動後才執行

[Service]                                 // 服務設定
Type=simple                               // 簡單服務類型
User=pi                                   // 用 pi 使用者執行
WorkingDirectory=/home/pi/pos-app         // 工作目錄
ExecStart=/usr/bin/serve -s -l 5000       // 啟動指令，監聽 5000 埠
Restart=always                            // 崩潰時自動重啟
RestartSec=5                              // 5 秒後重啟

[Install]                                 // 安裝設定
WantedBy=multi-user.target                // 多使用者模式啟動
SERVICE_EOF

# 啟動服務 // 讓 POS 開始運作
sudo systemctl daemon-reload // 重新載入 systemd 設定
sudo systemctl enable pos-web // 設定開機自動啟動
sudo systemctl start pos-web // 立即啟動服務
sudo systemctl status pos-web // 確認服務狀態
```

---

## 步驟九：測試完整流程

> 💡 **比喻：餐廳開幕前的試營運，每個環節都要跑一遍**

```
完整 POS 操作流程測試：

步驟 1：開機
  Pi 接電 → 自動進入 Kiosk → 顯示 POS 畫面
  ✅ 確認：螢幕顯示 POS 首頁

步驟 2：掃描商品條碼
  拿條碼掃描器 → 對準商品條碼 → 「嗶」一聲
  ✅ 確認：商品出現在購物車列表

步驟 3：加入購物車
  可以連續掃描多個商品
  ✅ 確認：數量和小計正確

步驟 4：結帳
  按「結帳」按鈕 → 選擇付款方式 → 確認
  ✅ 確認：金額計算正確

步驟 5：列印收據
  結帳後自動列印 → 熱感應印表機出紙
  ✅ 確認：收據內容正確（品項、數量、金額、時間）

步驟 6：開錢箱
  列印收據的同時 → 錢箱自動彈開
  ✅ 確認：錢箱有彈開

步驟 7：完成交易
  關錢箱 → 回到 POS 首頁 → 等待下一位客人
  ✅ 確認：畫面回到首頁
```

```bash
# 自動化測試腳本 // 一次測試所有硬體
cat > /home/pi/test-hardware.sh << 'TEST_EOF' // 寫入測試腳本
#!/bin/bash                                     // 指定 bash
echo ""=== POS 硬體測試開始 ===""                 // 顯示開始訊息

echo ""1. 測試螢幕...""                           // 測試螢幕
if [ -n ""\$DISPLAY"" ]; then                     // 檢查是否有顯示器
  echo ""   ✅ 螢幕正常""                         // 螢幕正常
else                                             // 沒有顯示器
  echo ""   ❌ 沒有偵測到螢幕""                    // 顯示錯誤
fi                                               // 結束判斷

echo ""2. 測試印表機...""                          // 測試印表機
if [ -e /dev/usb/lp0 ]; then                     // 檢查印表機裝置
  echo ""   ✅ 印表機已連接""                      // 印表機正常
  echo ""測試列印"" > /dev/usb/lp0                 // 送出測試文字
else                                             // 沒有印表機
  echo ""   ❌ 找不到印表機""                      // 顯示錯誤
fi                                               // 結束判斷

echo ""3. 測試條碼掃描器...""                      // 測試掃描器
if lsusb | grep -qi ""scanner\|barcode\|HID""; then // 搜尋 USB 裝置
  echo ""   ✅ 條碼掃描器已連接""                   // 掃描器正常
else                                              // 沒有掃描器
  echo ""   ⚠️ 未偵測到（可能顯示為 HID 裝置）""    // 顯示警告
fi                                                // 結束判斷

echo ""4. 測試網路...""                             // 測試網路
if ping -c 1 8.8.8.8 > /dev/null 2>&1; then      // 嘗試 ping Google
  echo ""   ✅ 網路連線正常""                       // 網路正常
else                                              // 沒有網路
  echo ""   ❌ 無法連線到網路""                     // 顯示錯誤
fi                                                // 結束判斷

echo ""5. 系統資訊...""                             // 顯示系統資訊
echo ""   CPU 溫度: \$(vcgencmd measure_temp)""     // 顯示 CPU 溫度
echo ""   記憶體: \$(free -h | awk '/Mem/{print \$3\""/\""\$2}')""  // 顯示記憶體
echo ""   SD 卡: \$(df -h / | awk 'NR==2{print \$3\""/\""\$2}')""  // 顯示儲存空間

echo ""=== 測試完成 ===""                           // 顯示完成訊息
TEST_EOF

# 設定腳本可執行 // 加上權限
chmod +x /home/pi/test-hardware.sh // 設定執行權限

# 執行測試 // 看結果
./test-hardware.sh // 執行硬體測試
```

---

## 步驟十：設定遠端管理

> 💡 **比喻：就算人不在店裡，也能用手機遠端管理 POS 機**

### SSH 遠端連線

```bash
# 確認 SSH 已啟用 // 應該在燒錄時就設定好了
sudo systemctl status ssh // 檢查 SSH 服務狀態

# 從你的電腦連線 // 在電腦的終端機執行
ssh pi@pos-pi.local // 用主機名稱連線
# 或
ssh pi@192.168.1.100 // 用固定 IP 連線

# 設定 SSH Key 免密碼登入 // 更安全也更方便
# 在你的電腦上執行：
ssh-keygen -t ed25519 // 產生金鑰對
ssh-copy-id pi@pos-pi.local // 把公鑰複製到 Pi
# 之後連線就不用再輸入密碼了
```

### 遠端看螢幕（VNC）

```bash
# 安裝 VNC Server // 遠端桌面
sudo apt install -y realvnc-vnc-server // 安裝 VNC

# 啟用 VNC // 設定開機自動啟動
sudo raspi-config nonint do_vnc 0 // 啟用 VNC Server

# 在電腦上用 VNC Viewer 連線 // 下載 RealVNC Viewer
# 輸入 pos-pi.local 或 192.168.1.100 // 就能看到 Pi 的螢幕畫面
```

### 自動更新腳本

```bash
# 建立自動更新腳本 // 從 Git 拉最新版本並重啟
cat > /home/pi/update-pos.sh << 'UPDATE_EOF' // 寫入更新腳本
#!/bin/bash                                    // 指定 bash
echo ""開始更新 POS 系統...""                    // 顯示更新訊息
cd /home/pi/pos-app                            // 進入 POS 目錄
git pull origin main                           // 拉取最新程式碼
npm install                                    // 安裝新的依賴
sudo systemctl restart pos-web                 // 重啟 POS 服務
echo ""更新完成！""                              // 顯示完成訊息
UPDATE_EOF

chmod +x /home/pi/update-pos.sh // 設定執行權限

# 設定每天凌晨 3 點自動更新 // 用 crontab
(crontab -l 2>/dev/null; echo ""0 3 * * * /home/pi/update-pos.sh >> /home/pi/update.log 2>&1"") | crontab - // 加入排程
```

---

## 🤔 我這樣寫為什麼會錯？

### 常見錯誤 1：開機黑畫面

```bash
# ❌ 錯誤：開機後螢幕全黑，什麼都沒有 // 最常見的問題
# 可能原因：
# 1. SD 卡沒燒好 // 重新燒錄一次
# 2. 電源不足 // 換 5V 3A 的電源
# 3. HDMI 線沒插好 // 重新插拔
# 4. 螢幕沒開 // 確認螢幕有通電

# ✅ 偵錯步驟：
# 看 Pi 上的 LED 燈 // 紅燈和綠燈的意義
# 紅燈亮 = 有供電 // 如果紅燈不亮，電源有問題
# 綠燈閃爍 = 在讀取 SD 卡 // 如果綠燈不亮，SD 卡有問題
# 綠燈連續閃 4 下 = 找不到系統 // SD 卡燒錄失敗，重來
# 綠燈連續閃 7 下 = kernel 錯誤 // 系統檔案損壞
```

### 常見錯誤 2：Kiosk 模式跳出來了

```bash
# ❌ 錯誤：客人不小心按到鍵盤快捷鍵，跳出 Kiosk // 尷尬
# 例如按了 Alt+F4、Ctrl+W、F11

# ✅ 解決方案：停用所有快捷鍵 // 在 Chromium 啟動參數加上
chromium-browser \
  --kiosk \                    // Kiosk 模式
  --disable-pinch \            // 停用手指縮放
  --overscroll-history-navigation=0 \  // 停用滑動換頁
  --ash-no-nudges \            // 停用提示訊息
  --disable-features=Translate,GlobalMediaControls \ // 停用翻譯和媒體控制
  'http://localhost:5000' // 開啟 POS 頁面

# 如果真的跳出來了 // 重啟 Kiosk
sudo systemctl restart kiosk // 重啟 Kiosk 服務
```

### 常見錯誤 3：印表機列印亂碼

```bash
# ❌ 錯誤：印出來的中文是亂碼 // 編碼問題
# 原因：印表機預設用 ASCII，不支援 UTF-8 中文

# ✅ 正確做法：設定印表機編碼 // 不同印表機設定不同
# 方法 1：使用 Big5 編碼 // 台灣繁體中文
python3 -c ""
text = '測試中文列印'  # 測試文字
encoded = text.encode('big5')  # 轉成 Big5 編碼
with open('/dev/usb/lp0', 'wb') as f:  # 開啟印表機
    f.write(b'\x1c\x26')  # 開啟中文模式指令
    f.write(encoded)  # 送出編碼後的文字
    f.write(b'\n\n\n')  # 空幾行
    f.write(b'\x1d\x56\x00')  # 切紙指令
"" // 用 Big5 編碼列印中文

# 方法 2：使用圖片列印 // 最可靠的方式
# 把文字渲染成圖片，再送給印表機
# 這樣任何字型、任何語言都能印
```

### 常見錯誤 4：錢箱不會開

```bash
# ❌ 錯誤：送了指令但錢箱沒反應 // 幾個常見原因
# 1. RJ11 線沒插好 // 重新插拔，要聽到「喀」一聲
# 2. 印表機不支援開錢箱 // 確認印表機型號有 RJ11 孔
# 3. 指令不對 // 不同印表機指令不同

# ✅ 偵錯步驟：
# 嘗試不同的開錢箱指令 // 常見的 ESC/POS 開錢箱指令
python3 -c ""
commands = [  # 常見的開錢箱指令列表
    b'\x1b\x70\x00\x19\xfa',  # 指令 1：Pin 0
    b'\x1b\x70\x01\x19\xfa',  # 指令 2：Pin 1
    b'\x10\x14\x01\x00\x05',  # 指令 3：DLE DC4
    b'\x1b\x70\x00\x32\xff',  # 指令 4：較長脈衝
]
for i, cmd in enumerate(commands):  # 逐一嘗試每個指令
    print(f'嘗試指令 {i+1}...')  # 顯示目前嘗試的指令
    with open('/dev/usb/lp0', 'wb') as f:  # 開啟印表機
        f.write(cmd)  # 送出指令
    import time; time.sleep(1)  # 等 1 秒看反應
"" // 逐一測試開錢箱指令
```

---

## 完成！你的 POS 機組裝成功了！

```
🎉 恭喜！你現在擁有一台自己組的 POS 機！

成本：約 NT$9,000（比商用 POS 便宜 50% 以上）
功能：觸控操作、條碼掃描、收據列印、錢箱控制
管理：SSH 遠端管理、自動更新、雲端同步

         ┌─────────────┐
         │  觸控螢幕     │
         │  POS 系統     │
         └──┬───────┬──┘
            │       │
    ┌───────┴───────┴───────┐
    ║    Raspberry Pi 4     ║
    ║    (你的 POS 大腦)     ║
    └═══════════════════════┘
      │      │       │
  [印表機] [掃描器] [錢箱]
      │
  [收據紙]  ← 「嗶～您的收據，謝謝光臨！」
```
" },
    };
}
