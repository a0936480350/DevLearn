using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_IoT2
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── IoT2 Chapter 604 ────────────────────────────
        new() { Id=604, Category="iot", Order=5, Level="beginner", Icon="🌐", Title="Web Kiosk 模式完整指南", Slug="web-kiosk-mode-guide", IsPublished=true, Content=@"
# Web Kiosk 模式完整指南

## 什麼是 Kiosk 模式？

> 💡 **比喻：把瀏覽器變成專用電視**
> 你家的電視只能看電視節目，不能拿來上網、玩遊戲。
> Kiosk 模式就是把一台電腦「鎖」成只能顯示一個網頁，
> 就像把一台電腦變成一台專用電視——只顯示你要的畫面，用戶無法跳出。

### 常見 Kiosk 應用場景

```
Kiosk 模式常見場景：
🏪 POS 收銀系統 → 只顯示收銀介面
🏥 醫院叫號系統 → 只顯示叫號畫面
🍔 速食店點餐機 → 只顯示點餐頁面
🏨 飯店 Check-in → 只顯示自助入住
🚉 車站資訊看板 → 只顯示時刻表
📊 工廠看板系統 → 只顯示即時產量
```

---

## Chromium Kiosk Mode 基本設定

### 安裝 Chromium

```bash
# 更新套件庫 // 確保取得最新版本
sudo apt update // 執行套件更新

# 安裝 Chromium 瀏覽器 // Kiosk 模式的核心
sudo apt install -y chromium-browser // 安裝 Chromium

# 安裝無頭顯示套件 // 虛擬螢幕支援
sudo apt install -y xdotool unclutter // 安裝輔助工具

# 確認安裝成功 // 檢查版本號
chromium-browser --version // 顯示 Chromium 版本
```

### 基本 Kiosk 啟動指令

```bash
# 啟動全螢幕 Kiosk 模式 // 最基本的 Kiosk 指令
chromium-browser \
  --kiosk \                    // 啟用 Kiosk 模式
  --noerrdialogs \             // 不顯示錯誤對話框
  --disable-infobars \         // 不顯示資訊列
  --no-first-run \             // 跳過首次執行設定
  --disable-translate \        // 停用翻譯提示
  --disable-features=TranslateUI \  // 停用翻譯 UI
  --check-for-update-interval=31536000 \  // 停用自動更新檢查
  --incognito \                // 無痕模式（不留紀錄）
  'http://localhost:5000'      // 指定要顯示的網頁
```

---

## 自動開機啟動 Kiosk

### 方法一：使用 autostart 設定

```bash
# 建立 autostart 目錄 // 存放自動啟動設定
mkdir -p /home/pi/.config/autostart // 建立目錄

# 建立 autostart 桌面檔案 // 自動啟動設定
cat > /home/pi/.config/autostart/kiosk.desktop << 'EOF'  // 寫入設定檔
[Desktop Entry]                // 桌面項目設定區塊
Type=Application               // 類型為應用程式
Name=Kiosk                     // 名稱為 Kiosk
Exec=/home/pi/kiosk.sh         // 執行啟動腳本
X-GNOME-Autostart-enabled=true // 啟用自動啟動
EOF
```

### 方法二：使用 systemd service

```bash
# 建立 systemd service 檔案 // 系統服務設定
sudo cat > /etc/systemd/system/kiosk.service << 'EOF'  // 寫入 service 檔
[Unit]                               // 單元設定區塊
Description=Chromium Kiosk Mode      // 服務描述
Wants=graphical.target               // 需要圖形介面
After=graphical.target               // 在圖形介面後啟動

[Service]                            // 服務設定區塊
Environment=DISPLAY=:0               // 設定顯示器環境變數
Environment=XAUTHORITY=/home/pi/.Xauthority  // 設定 X 授權
Type=simple                          // 服務類型為簡單
ExecStart=/home/pi/kiosk.sh          // 啟動腳本路徑
Restart=on-failure                   // 失敗時自動重啟
RestartSec=5                         // 重啟間隔 5 秒
User=pi                              // 以 pi 用戶執行

[Install]                            // 安裝設定區塊
WantedBy=graphical.target            // 隨圖形介面啟動
EOF

# 啟用並啟動服務 // 讓服務開機自動執行
sudo systemctl enable kiosk.service  // 啟用開機自動啟動
sudo systemctl start kiosk.service   // 立即啟動服務
sudo systemctl status kiosk.service  // 檢查服務狀態
```

---

## 防止用戶跳出（禁用快捷鍵）

### 禁用常見快捷鍵

```bash
# 建立禁用快捷鍵腳本 // 防止用戶跳出 Kiosk
cat > /home/pi/disable_shortcuts.sh << 'EOF'  // 寫入腳本
#!/bin/bash                          // 指定 Bash 直譯器

# 禁用 Alt+F4 // 防止關閉視窗
xdotool key --clearmodifiers alt+F4  // 清除 Alt+F4 綁定

# 禁用 Ctrl+Alt+Delete // 防止開啟工作管理員
xdg-settings set default-web-browser chromium-browser.desktop  // 固定瀏覽器

# 使用 xbindkeys 攔截快捷鍵 // 進階快捷鍵管理
cat > /home/pi/.xbindkeysrc << 'KEYS'  // 寫入快捷鍵設定
""echo '已禁用'""                    // 快捷鍵動作（無效化）
    Alt + F4                         // 攔截 Alt+F4

""echo '已禁用'""                    // 快捷鍵動作（無效化）
    Control + w                      // 攔截 Ctrl+W

""echo '已禁用'""                    // 快捷鍵動作（無效化）
    Alt + Tab                        // 攔截 Alt+Tab
KEYS

# 啟動 xbindkeys // 套用快捷鍵攔截
xbindkeys                           // 執行快捷鍵攔截
EOF

chmod +x /home/pi/disable_shortcuts.sh  // 設定腳本可執行
```

### 隱藏滑鼠游標

```bash
# 安裝 unclutter // 自動隱藏閒置游標
sudo apt install -y unclutter        // 安裝 unclutter

# 啟動時隱藏游標 // 5 秒無操作就隱藏
unclutter -idle 5 -root &           // 背景執行隱藏游標
```

---

## 觸控螢幕校準

### 安裝校準工具

```bash
# 安裝觸控校準工具 // 讓觸控點對準螢幕位置
sudo apt install -y xinput-calibrator  // 安裝 xinput-calibrator

# 執行校準程序 // 跟著螢幕點四個角落
xinput_calibrator                    // 啟動校準精靈

# 查看觸控裝置列表 // 找出觸控螢幕裝置 ID
xinput list                          // 列出所有輸入裝置
```

### 使用 C# 處理觸控事件

```csharp
// 觸控螢幕事件處理服務 // 處理 Kiosk 的觸控輸入
public class TouchScreenService // 觸控螢幕服務類別
{
    private readonly ILogger<TouchScreenService> _logger; // 日誌記錄器

    // 建構函式注入日誌服務 // 依賴注入模式
    public TouchScreenService(ILogger<TouchScreenService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 處理觸控座標 // 將觸控位置轉換為畫面座標
    public (int X, int Y) CalibrateTouch( // 校準觸控座標方法
        int rawX, int rawY, // 原始觸控座標
        int screenWidth, int screenHeight) // 螢幕解析度
    {
        // 校準比例計算 // 將原始座標映射到螢幕
        double scaleX = (double)screenWidth / 4096; // X 軸縮放比例
        double scaleY = (double)screenHeight / 4096; // Y 軸縮放比例

        int calibratedX = (int)(rawX * scaleX); // 計算校準後 X
        int calibratedY = (int)(rawY * scaleY); // 計算校準後 Y

        _logger.LogDebug(""觸控校準：({RawX},{RawY}) → ({CalX},{CalY})"", // 記錄校準結果
            rawX, rawY, calibratedX, calibratedY); // 傳入參數

        return (calibratedX, calibratedY); // 回傳校準後座標
    }
}
```

---

## 螢幕旋轉設定（直式/橫式）

### 旋轉螢幕

```bash
# 查看目前螢幕資訊 // 確認螢幕名稱
xrandr                               // 顯示螢幕資訊

# 旋轉螢幕 90 度（直式） // 適合點餐機、叫號機
xrandr --output HDMI-1 --rotate left  // 逆時針旋轉 90 度

# 旋轉螢幕 180 度（倒掛） // 適合天花板螢幕
xrandr --output HDMI-1 --rotate inverted  // 旋轉 180 度

# 恢復橫式 // 標準方向
xrandr --output HDMI-1 --rotate normal  // 恢復正常方向

# 永久設定旋轉 // 寫入啟動設定
echo 'xrandr --output HDMI-1 --rotate left' >> /home/pi/.bashrc  // 開機自動旋轉
```

### 旋轉觸控座標同步

```bash
# 旋轉觸控座標對應 // 螢幕旋轉後觸控也要跟著轉
TOUCH_ID=$(xinput list --id-only ""touchscreen"")  // 取得觸控裝置 ID

# 設定觸控旋轉矩陣（左轉 90 度） // 數學矩陣轉換
xinput set-prop $TOUCH_ID 'Coordinate Transformation Matrix' \
  0 -1 1 \    // 旋轉矩陣第一列
  1  0 0 \    // 旋轉矩陣第二列
  0  0 1      // 旋轉矩陣第三列
```

---

## 自動重新整理（避免記憶體洩漏）

### JavaScript 自動重新整理

```csharp
// Kiosk 頁面自動重新整理服務 // 防止長時間執行記憶體洩漏
public class KioskRefreshService // 自動重新整理服務類別
{
    // 產生自動重新整理的 JavaScript // 注入到 Kiosk 頁面
    public string GenerateAutoRefreshScript( // 產生 JS 腳本方法
        int intervalMinutes = 30) // 預設每 30 分鐘刷新
    {
        // 回傳 JavaScript 程式碼 // 定時重新整理頁面
        return $@""
            <script>
                // 設定自動重新整理計時器 // 避免記憶體洩漏
                setInterval(function() {{ // 每隔指定時間執行
                    // 檢查是否閒置 // 有人操作就不刷新
                    if (Date.now() - lastInteraction > {intervalMinutes * 60 * 1000}) {{ // 判斷閒置時間
                        location.reload(); // 重新整理頁面
                    }}
                }}, {intervalMinutes * 60 * 1000}); // 設定間隔毫秒數

                // 記錄最後互動時間 // 追蹤用戶操作
                var lastInteraction = Date.now(); // 初始化互動時間
                document.addEventListener('click', function() {{ // 監聽點擊事件
                    lastInteraction = Date.now(); // 更新互動時間
                }});
                document.addEventListener('touchstart', function() {{ // 監聽觸控事件
                    lastInteraction = Date.now(); // 更新互動時間
                }});
            </script>""; // 結束 JavaScript 區塊
    }
}
```

### 記憶體監控自動重啟

```bash
# 記憶體監控腳本 // 記憶體不足時自動重啟 Chromium
cat > /home/pi/monitor_memory.sh << 'EOF'  // 建立監控腳本
#!/bin/bash                          // 指定直譯器

while true; do                       // 無限迴圈監控
    # 取得可用記憶體（MB） // 計算剩餘記憶體
    FREE_MEM=$(free -m | awk '/^Mem:/{print $7}')  // 取得可用 MB

    # 如果可用記憶體低於 100MB // 觸發重啟條件
    if [ ""$FREE_MEM"" -lt 100 ]; then  // 判斷記憶體門檻
        echo ""$(date): 記憶體不足 ${FREE_MEM}MB，重啟 Chromium""  // 記錄日誌
        pkill -f chromium              // 殺掉 Chromium 程序
        sleep 3                        // 等待 3 秒
        /home/pi/kiosk.sh &            // 重新啟動 Kiosk
    fi

    sleep 60                           // 每 60 秒檢查一次
done
EOF

chmod +x /home/pi/monitor_memory.sh  // 設定可執行權限
```

---

## 多螢幕設定（主螢幕+客顯）

### 雙螢幕配置

```bash
# 查看所有螢幕 // 確認有幾個輸出
xrandr --listmonitors                // 列出所有螢幕

# 設定雙螢幕（延伸模式） // 主螢幕右邊接客顯
xrandr --output HDMI-1 --auto --primary \  // 主螢幕設定
       --output HDMI-2 --auto --right-of HDMI-1  // 客顯在右邊

# 設定雙螢幕（鏡像模式） // 兩個螢幕顯示一樣
xrandr --output HDMI-1 --auto --primary \  // 主螢幕設定
       --output HDMI-2 --auto --same-as HDMI-1  // 客顯鏡像
```

### C# 雙螢幕 Kiosk 管理

```csharp
// 雙螢幕 Kiosk 管理器 // 管理主螢幕和客顯
public class DualScreenKiosk // 雙螢幕管理類別
{
    // 螢幕配置模型 // 記錄每個螢幕的設定
    public class ScreenConfig // 螢幕設定類別
    {
        public string OutputName { get; set; } = """"; // 螢幕輸出名稱（如 HDMI-1）
        public int Width { get; set; } // 螢幕寬度像素
        public int Height { get; set; } // 螢幕高度像素
        public string Role { get; set; } = ""main""; // 角色：main 或 customer
        public string Url { get; set; } = """"; // 要顯示的網址
    }

    // 產生雙螢幕啟動腳本 // 自動開兩個 Chromium 視窗
    public string GenerateLaunchScript( // 產生啟動腳本方法
        ScreenConfig mainScreen, // 主螢幕設定
        ScreenConfig customerScreen) // 客顯設定
    {
        // 組合 bash 啟動腳本 // 分別在兩個螢幕開 Chromium
        return $@""#!/bin/bash
# 主螢幕 Kiosk // 操作員使用的畫面
chromium-browser --kiosk --window-position=0,0 \
  --window-size={mainScreen.Width},{mainScreen.Height} \
  '{mainScreen.Url}' &

# 客顯螢幕 // 給客人看的畫面
chromium-browser --kiosk --window-position={mainScreen.Width},0 \
  --window-size={customerScreen.Width},{customerScreen.Height} \
  '{customerScreen.Url}' &
""; // 結束腳本字串
    }
}
```

---

## 完整的 kiosk.sh 啟動腳本

```bash
#!/bin/bash                              // 指定 Bash 直譯器
# kiosk.sh - 完整的 Kiosk 啟動腳本 // 一鍵啟動所有 Kiosk 元件

# === 基本設定 === // 可依需求修改
KIOSK_URL=""http://localhost:5000""      // Kiosk 要顯示的網址
SCREEN_ROTATION=""normal""               // 螢幕旋轉：normal/left/right/inverted
REFRESH_INTERVAL=1800                    // 自動重新整理間隔（秒）
LOG_FILE=""/home/pi/kiosk.log""          // 日誌檔案路徑

# === 記錄啟動時間 === // 方便除錯
echo ""$(date): Kiosk 啟動中..."" >> $LOG_FILE  // 寫入啟動日誌

# === 等待桌面環境就緒 === // 確保 X11 已啟動
sleep 5                                  // 等待 5 秒讓桌面就緒

# === 螢幕旋轉 === // 依設定旋轉螢幕
xrandr --output HDMI-1 --rotate $SCREEN_ROTATION  // 執行螢幕旋轉

# === 隱藏游標 === // 無操作 3 秒後隱藏
unclutter -idle 3 -root &               // 背景隱藏游標

# === 停用螢幕保護 === // 防止螢幕變黑
xset s off                               // 關閉螢幕保護
xset -dpms                              // 關閉電源管理
xset s noblank                           // 關閉螢幕空白

# === 禁用快捷鍵 === // 防止用戶跳出
xbindkeys                               // 啟動快捷鍵攔截

# === 清除 Chromium 快取 === // 避免快取問題
rm -rf /home/pi/.config/chromium/Default/Cache/*  // 清除快取檔案

# === 啟動 Chromium Kiosk === // 主要 Kiosk 程序
chromium-browser \
  --kiosk \                              // 全螢幕 Kiosk 模式
  --noerrdialogs \                       // 隱藏錯誤對話框
  --disable-infobars \                   // 隱藏資訊列
  --no-first-run \                       // 跳過首次執行
  --disable-translate \                  // 停用翻譯
  --disable-features=TranslateUI \       // 停用翻譯介面
  --disable-session-crashed-bubble \     // 停用當機氣泡
  --disable-component-update \           // 停用元件更新
  --check-for-update-interval=31536000 \ // 極長更新間隔
  --incognito \                          // 無痕模式
  --disk-cache-size=0 \                  // 不使用磁碟快取
  ""$KIOSK_URL"" &                       // 開啟目標網址

# === 記錄啟動完成 === // 確認 Kiosk 已啟動
echo ""$(date): Kiosk 啟動完成"" >> $LOG_FILE  // 寫入完成日誌
```

---

## 錯誤自動重啟腳本

```bash
#!/bin/bash                              // 指定直譯器
# watchdog.sh - 監控並自動重啟 // Kiosk 的看門狗

KIOSK_URL=""http://localhost:5000""      // Kiosk 網址
CHECK_INTERVAL=30                        // 每 30 秒檢查一次
LOG_FILE=""/home/pi/watchdog.log""       // 看門狗日誌

while true; do                           // 無限迴圈監控
    # 檢查 Chromium 是否在執行 // 確認程序存活
    if ! pgrep -x ""chromium"" > /dev/null; then  // 如果 Chromium 沒在跑
        echo ""$(date): Chromium 已停止，重新啟動..."" >> $LOG_FILE  // 記錄異常
        /home/pi/kiosk.sh &             // 重新啟動 Kiosk
    fi

    # 檢查網頁是否能連線 // 確認 Web Server 正常
    HTTP_CODE=$(curl -s -o /dev/null -w ""%{http_code}"" $KIOSK_URL)  // 取得 HTTP 狀態碼
    if [ ""$HTTP_CODE"" != ""200"" ]; then  // 如果不是 200 OK
        echo ""$(date): 網頁回應 $HTTP_CODE，等待恢復..."" >> $LOG_FILE  // 記錄異常
    fi

    sleep $CHECK_INTERVAL                // 等待下次檢查
done
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：直接執行 chromium --kiosk 卻看到白畫面

```bash
# 錯誤寫法 // 沒等 Web Server 就啟動 Kiosk
chromium-browser --kiosk http://localhost:5000  // 直接啟動（Web Server 還沒準備好）
```

### ✅ 正確：先等 Web Server 就緒再啟動

```bash
# 正確寫法 // 確認 Web Server 已啟動
while ! curl -s http://localhost:5000 > /dev/null; do  // 迴圈等待 Server 就緒
    echo ""等待 Web Server 啟動...""   // 顯示等待訊息
    sleep 2                            // 每 2 秒重試
done
chromium-browser --kiosk http://localhost:5000  // Server 就緒後再啟動 Kiosk
```

### ❌ 錯誤：螢幕旋轉後觸控位置偏移

```bash
# 錯誤寫法 // 只旋轉螢幕沒旋轉觸控
xrandr --output HDMI-1 --rotate left   // 旋轉螢幕
# 觸控座標還是原本的方向 ← 點哪裡都不對！ // 忘記同步觸控矩陣
```

### ✅ 正確：螢幕和觸控一起旋轉

```bash
# 正確寫法 // 螢幕和觸控都要旋轉
xrandr --output HDMI-1 --rotate left   // 旋轉螢幕
xinput set-prop 'touchscreen' 'Coordinate Transformation Matrix' \  // 同步旋轉觸控
  0 -1 1 1 0 0 0 0 1                  // 左轉 90 度的矩陣
```

### ❌ 錯誤：Kiosk 記憶體越用越多

```csharp
// 錯誤寫法 // 沒有自動重新整理機制
public class KioskPage // Kiosk 頁面類別
{
    // 頁面永遠不刷新 // 長時間運行記憶體會洩漏
    public void Render() // 渲染頁面方法
    {
        // 只渲染一次就不管了 // 記憶體只增不減
    }
}
```

### ✅ 正確：加入定時重新整理和記憶體監控

```csharp
// 正確寫法 // 定期重新整理 + 監控記憶體
public class SmartKioskPage // 智慧 Kiosk 頁面類別
{
    private readonly Timer _refreshTimer; // 重新整理計時器

    public SmartKioskPage() // 建構函式
    {
        // 每 30 分鐘自動重新整理 // 避免記憶體洩漏
        _refreshTimer = new Timer(RefreshPage, null, // 建立計時器
            TimeSpan.FromMinutes(30), // 30 分鐘後第一次觸發
            TimeSpan.FromMinutes(30)); // 之後每 30 分鐘觸發
    }

    private void RefreshPage(object? state) // 重新整理回呼方法
    {
        // 檢查是否有用戶正在操作 // 避免打斷操作
        if (IsUserIdle()) // 如果用戶閒置
        {
            ForceRefresh(); // 執行頁面重新整理
        }
    }

    private bool IsUserIdle() => true; // 判斷用戶是否閒置
    private void ForceRefresh() { } // 強制重新整理
}
```
" },

        // ── IoT2 Chapter 605 ────────────────────────────
        new() { Id=605, Category="iot", Order=6, Level="intermediate", Icon="📡", Title="IoT 通訊協定與感測器", Slug="iot-protocols-sensors", IsPublished=true, Content=@"
# IoT 通訊協定與感測器

## GPIO 深入理解

> 💡 **比喻：Raspberry Pi 的神經系統**
> 人的神經系統從大腦延伸到手指、腳趾，感覺冷熱、控制肌肉。
> GPIO 就是 Raspberry Pi 的神經系統——那 40 根針腳就是它的「手指」，
> 可以感測外界訊號（輸入），也可以控制外部裝置（輸出）。

### GPIO 針腳圖

```
Raspberry Pi GPIO 針腳配置（40 Pin）：
┌─────────────────────────────────────┐
│  3V3 (1)  (2)  5V                   │  // 第 1-2 腳：電源
│  GPIO2 (3)  (4)  5V                 │  // 第 3 腳：I2C SDA
│  GPIO3 (5)  (6)  GND               │  // 第 5 腳：I2C SCL
│  GPIO4 (7)  (8)  GPIO14            │  // 第 8 腳：UART TXD
│  GND (9)  (10) GPIO15              │  // 第 10 腳：UART RXD
│  GPIO17(11) (12) GPIO18            │  // 第 12 腳：PWM
│  GPIO27(13) (14) GND               │  // 第 13 腳：通用 GPIO
│  GPIO22(15) (16) GPIO23            │  // 第 15-16 腳：通用 GPIO
│  3V3 (17) (18) GPIO24              │  // 第 17 腳：3.3V 電源
│  GPIO10(19) (20) GND               │  // 第 19 腳：SPI MOSI
│  GPIO9 (21) (22) GPIO25            │  // 第 21 腳：SPI MISO
│  GPIO11(23) (24) GPIO8             │  // 第 23 腳：SPI SCLK
│  GND (25) (26) GPIO7               │  // 第 24 腳：SPI CE0
│  GPIO0 (27) (28) GPIO1             │  // 第 27-28 腳：I2C EEPROM
│  GPIO5 (29) (30) GND               │  // 第 29 腳：通用 GPIO
│  GPIO6 (31) (32) GPIO12            │  // 第 31 腳：通用 GPIO
│  GPIO13(33) (34) GND               │  // 第 33 腳：PWM1
│  GPIO19(35) (36) GPIO16            │  // 第 35 腳：PCM FS
│  GPIO26(37) (38) GPIO20            │  // 第 37 腳：通用 GPIO
│  GND (39) (40) GPIO21              │  // 第 39 腳：接地
└─────────────────────────────────────┘
```

---

## 用 C# 讀取 GPIO

### 安裝 NuGet 套件

```bash
# 安裝 GPIO 套件 // .NET IoT 官方函式庫
dotnet add package System.Device.Gpio   // 加入 GPIO 支援
dotnet add package Iot.Device.Bindings  // 加入感測器綁定
```

### GPIO 基本控制

```csharp
// 引用 GPIO 命名空間 // .NET IoT 核心
using System.Device.Gpio; // GPIO 控制類別

// GPIO 控制服務 // 管理所有 GPIO 針腳
public class GpioService : IDisposable // GPIO 服務（實作 IDisposable）
{
    private readonly GpioController _controller; // GPIO 控制器
    private readonly ILogger<GpioService> _logger; // 日誌記錄器

    // LED 針腳定義 // 定義各裝置的針腳號碼
    private const int LED_PIN = 17; // LED 接在 GPIO17
    private const int BUTTON_PIN = 27; // 按鈕接在 GPIO27
    private const int BUZZER_PIN = 22; // 蜂鳴器接在 GPIO22

    // 建構函式 // 初始化 GPIO 控制器
    public GpioService(ILogger<GpioService> logger) // 注入日誌服務
    {
        _logger = logger; // 儲存日誌記錄器
        _controller = new GpioController(); // 建立 GPIO 控制器

        // 設定針腳模式 // 輸出控制 LED，輸入讀取按鈕
        _controller.OpenPin(LED_PIN, PinMode.Output); // LED 設為輸出
        _controller.OpenPin(BUTTON_PIN, PinMode.InputPullUp); // 按鈕設為輸入（上拉）
        _controller.OpenPin(BUZZER_PIN, PinMode.Output); // 蜂鳴器設為輸出

        _logger.LogInformation(""GPIO 初始化完成""); // 記錄初始化成功
    }

    // 控制 LED // 開啟或關閉 LED
    public void SetLed(bool on) // 設定 LED 狀態方法
    {
        var value = on ? PinValue.High : PinValue.Low; // 轉換布林為電位
        _controller.Write(LED_PIN, value); // 寫入 GPIO 電位
        _logger.LogDebug(""LED 狀態：{State}"", on ? ""開"" : ""關""); // 記錄狀態
    }

    // 讀取按鈕 // 偵測按鈕是否被按下
    public bool IsButtonPressed() // 讀取按鈕狀態方法
    {
        var value = _controller.Read(BUTTON_PIN); // 讀取 GPIO 電位
        return value == PinValue.Low; // 上拉電路按下時為 Low
    }

    // 蜂鳴器嗶聲 // 發出提示音
    public async Task BeepAsync(int durationMs = 200) // 蜂鳴器方法
    {
        _controller.Write(BUZZER_PIN, PinValue.High); // 開啟蜂鳴器
        await Task.Delay(durationMs); // 等待指定毫秒數
        _controller.Write(BUZZER_PIN, PinValue.Low); // 關閉蜂鳴器
    }

    // 釋放資源 // 清理 GPIO 資源
    public void Dispose() // 實作 IDisposable
    {
        _controller.ClosePin(LED_PIN); // 關閉 LED 針腳
        _controller.ClosePin(BUTTON_PIN); // 關閉按鈕針腳
        _controller.ClosePin(BUZZER_PIN); // 關閉蜂鳴器針腳
        _controller.Dispose(); // 釋放控制器
    }
}
```

---

## I2C 通訊（溫濕度感測器 DHT22）

### 讀取 DHT22 感測器

```csharp
// 引用 IoT 感測器綁定 // DHT22 溫濕度感測器
using Iot.Device.DHTxx; // DHT 系列感測器
using System.Device.I2c; // I2C 通訊
using UnitsNet; // 物理單位換算

// 溫濕度監控服務 // 定期讀取環境溫濕度
public class TemperatureService // 溫濕度服務類別
{
    private readonly ILogger<TemperatureService> _logger; // 日誌記錄器
    private const int DHT_PIN = 4; // DHT22 資料線接 GPIO4

    // 建構函式 // 注入日誌服務
    public TemperatureService(ILogger<TemperatureService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 讀取溫濕度 // 回傳溫度和濕度數值
    public (double Temperature, double Humidity)? ReadSensor() // 讀取感測器方法
    {
        try // 嘗試讀取感測器
        {
            using var dht = new Dht22(DHT_PIN); // 建立 DHT22 實例

            var temp = dht.Temperature; // 讀取溫度
            var humidity = dht.Humidity; // 讀取濕度

            if (temp.Equals(default(Temperature)) || // 檢查溫度是否有效
                humidity.Equals(default(RelativeHumidity))) // 檢查濕度是否有效
            {
                _logger.LogWarning(""感測器讀取失敗""); // 記錄讀取失敗
                return null; // 回傳 null 表示失敗
            }

            var result = (temp.DegreesCelsius, humidity.Percent); // 組合結果
            _logger.LogInformation( // 記錄讀取結果
                ""溫度：{Temp:F1}°C，濕度：{Hum:F1}%"", // 格式化訊息
                result.DegreesCelsius, result.Percent); // 傳入參數

            return result; // 回傳溫濕度結果
        }
        catch (Exception ex) // 捕捉例外
        {
            _logger.LogError(ex, ""DHT22 讀取錯誤""); // 記錄錯誤
            return null; // 回傳 null
        }
    }
}
```

---

## SPI 通訊（RFID 讀卡器 RC522）

### RFID 讀卡服務

```csharp
// RFID 讀卡服務 // 使用 SPI 通訊讀取 RFID 卡片
public class RfidService // RFID 服務類別
{
    private readonly ILogger<RfidService> _logger; // 日誌記錄器

    // RFID 事件 // 當卡片被偵測到時觸發
    public event Action<string>? OnCardDetected; // 卡片偵測事件

    // 建構函式 // 注入日誌
    public RfidService(ILogger<RfidService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 啟動 RFID 監聽 // 持續偵測卡片
    public async Task StartListeningAsync( // 開始監聽方法
        CancellationToken ct) // 取消令牌
    {
        _logger.LogInformation(""RFID 讀卡器已啟動""); // 記錄啟動

        while (!ct.IsCancellationRequested) // 持續監聽直到取消
        {
            try // 嘗試讀取卡片
            {
                var cardId = await ReadCardAsync(); // 讀取卡片 ID
                if (cardId != null) // 如果有偵測到卡片
                {
                    _logger.LogInformation(""偵測到卡片：{CardId}"", cardId); // 記錄卡片
                    OnCardDetected?.Invoke(cardId); // 觸發卡片偵測事件
                }
            }
            catch (Exception ex) // 捕捉例外
            {
                _logger.LogError(ex, ""RFID 讀取錯誤""); // 記錄錯誤
            }

            await Task.Delay(500, ct); // 每 0.5 秒偵測一次
        }
    }

    // 讀取卡片 ID // 透過 SPI 通訊取得 UID
    private Task<string?> ReadCardAsync() // 讀取卡片非同步方法
    {
        // 實際實作需要 SPI 通訊 // 這裡是模擬框架
        // 使用 System.Device.Spi 進行通訊 // SPI 匯流排
        return Task.FromResult<string?>(null); // 回傳讀取結果
    }
}
```

---

## UART 通訊（條碼掃描模組）

### 串口條碼讀取

```csharp
// 引用串口通訊命名空間 // UART 序列埠
using System.IO.Ports; // 串口通訊類別

// 條碼掃描服務 // 透過 UART 讀取條碼
public class BarcodeService : IDisposable // 條碼服務（可釋放）
{
    private readonly SerialPort _serialPort; // 串口物件
    private readonly ILogger<BarcodeService> _logger; // 日誌記錄器

    // 條碼掃描事件 // 掃到條碼時觸發
    public event Action<string>? OnBarcodeScanned; // 條碼掃描事件

    // 建構函式 // 初始化串口設定
    public BarcodeService(ILogger<BarcodeService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器

        _serialPort = new SerialPort // 建立串口物件
        {
            PortName = ""/dev/ttyUSB0"", // Linux 串口路徑
            BaudRate = 9600, // 鮑率 9600
            DataBits = 8, // 資料位元 8
            Parity = Parity.None, // 無同位元檢查
            StopBits = StopBits.One, // 停止位元 1
            ReadTimeout = 1000 // 讀取超時 1 秒
        };

        // 註冊資料接收事件 // 當串口收到資料時觸發
        _serialPort.DataReceived += OnDataReceived; // 綁定接收事件
    }

    // 開啟串口 // 開始接收條碼資料
    public void Open() // 開啟串口方法
    {
        _serialPort.Open(); // 開啟串口連線
        _logger.LogInformation(""條碼掃描器已連線：{Port}"", // 記錄連線
            _serialPort.PortName); // 顯示埠名
    }

    // 串口資料接收處理 // 解析條碼字串
    private void OnDataReceived(object sender, // 發送者參數
        SerialDataReceivedEventArgs e) // 事件參數
    {
        var barcode = _serialPort.ReadLine().Trim(); // 讀取一行並去空白
        if (!string.IsNullOrEmpty(barcode)) // 如果條碼不為空
        {
            _logger.LogInformation(""掃描到條碼：{Barcode}"", barcode); // 記錄條碼
            OnBarcodeScanned?.Invoke(barcode); // 觸發掃描事件
        }
    }

    // 釋放資源 // 關閉串口
    public void Dispose() // 實作 IDisposable
    {
        if (_serialPort.IsOpen) // 如果串口是開的
            _serialPort.Close(); // 關閉串口
        _serialPort.Dispose(); // 釋放串口資源
    }
}
```

---

## MQTT 訊息佇列

> 💡 **比喻：物聯網的 LINE 群組**
> MQTT 就像一個 LINE 群組——你發一則訊息（Publish），
> 所有加入群組的人（Subscribe）都會收到。
> 不用知道對方是誰，只要在同一個「主題」（Topic）下就能通訊。

### MQTT 客戶端

```csharp
// 引用 MQTT 套件 // 安裝 MQTTnet NuGet
// dotnet add package MQTTnet // 加入 MQTT 支援

// MQTT 通訊服務 // 物聯網訊息發布與訂閱
public class MqttService // MQTT 服務類別
{
    private readonly ILogger<MqttService> _logger; // 日誌記錄器
    private readonly string _brokerHost; // MQTT Broker 位址
    private readonly int _brokerPort; // MQTT Broker 埠號

    // MQTT 主題定義 // 依功能分類的主題
    public static class Topics // 主題常數類別
    {
        public const string Temperature = ""pos/sensors/temperature""; // 溫度主題
        public const string Humidity = ""pos/sensors/humidity""; // 濕度主題
        public const string Barcode = ""pos/scanner/barcode""; // 條碼主題
        public const string Receipt = ""pos/printer/receipt""; // 收據主題
        public const string Alert = ""pos/system/alert""; // 系統警報主題
    }

    // 建構函式 // 設定 Broker 連線資訊
    public MqttService( // 建構 MQTT 服務
        ILogger<MqttService> logger, // 注入日誌
        string brokerHost = ""localhost"", // 預設本地 Broker
        int brokerPort = 1883) // 預設 MQTT 埠
    {
        _logger = logger; // 儲存日誌記錄器
        _brokerHost = brokerHost; // 儲存 Broker 位址
        _brokerPort = brokerPort; // 儲存 Broker 埠號
    }

    // 發布訊息 // 將資料發送到指定主題
    public async Task PublishAsync( // 發布訊息方法
        string topic, string message) // 主題和訊息內容
    {
        _logger.LogInformation( // 記錄發布動作
            ""發布到 {Topic}: {Message}"", // 格式化訊息
            topic, message); // 傳入參數

        // 實際發布邏輯 // 透過 MQTTnet 發送
        await Task.CompletedTask; // 非同步發布完成
    }

    // 訂閱主題 // 接收指定主題的訊息
    public async Task SubscribeAsync( // 訂閱主題方法
        string topic, // 要訂閱的主題
        Action<string> onMessage) // 收到訊息的回呼
    {
        _logger.LogInformation(""訂閱主題：{Topic}"", topic); // 記錄訂閱

        // 實際訂閱邏輯 // 透過 MQTTnet 訂閱
        await Task.CompletedTask; // 非同步訂閱完成
    }
}
```

---

## WebSocket vs MQTT 比較

```
┌──────────────┬─────────────────┬─────────────────┐
│     比較項目  │   WebSocket     │     MQTT        │
├──────────────┼─────────────────┼─────────────────┤
│ 通訊模式     │ 點對點          │ 發布/訂閱       │
│ 協定         │ TCP（ws://）    │ TCP（mqtt://）  │
│ 適用場景     │ 即時聊天、遊戲  │ IoT 感測器      │
│ 訊息大小     │ 較大            │ 極小（2 bytes） │
│ 連線品質     │ 需要穩定網路    │ 支援離線佇列    │
│ 複雜度       │ 較簡單          │ 需要 Broker     │
│ 瀏覽器支援   │ 原生支援        │ 需要轉接        │
│ 耗電量       │ 較高            │ 極低            │
└──────────────┴─────────────────┴─────────────────┘
```

---

## 用 Python 讀取感測器（搭配 .NET API）

### Python 感測器腳本

```python
# sensor_reader.py // Python 感測器讀取腳本
import json          # JSON 序列化 // 轉換資料格式
import time          # 時間模組 // 控制讀取間隔
import requests      # HTTP 請求 // 發送到 .NET API

API_URL = ""http://localhost:5000/api/sensors""  # .NET API 網址 // 感測器端點

def read_and_send():  # 讀取並發送函式 // 主要邏輯
    # 模擬讀取感測器 // 實際需連接硬體
    data = {  # 感測器資料字典 // 包含溫濕度
        ""temperature"": 25.5,  # 溫度（攝氏） // 來自 DHT22
        ""humidity"": 65.0,     # 濕度（百分比） // 來自 DHT22
        ""timestamp"": time.time()  # 時間戳記 // Unix 時間
    }

    try:  # 嘗試發送資料 // 可能網路斷線
        response = requests.post(  # 發送 POST 請求 // 到 .NET API
            API_URL,  # API 網址 // 感測器端點
            json=data,  # 以 JSON 傳送 // 自動序列化
            timeout=5  # 超時 5 秒 // 避免卡住
        )
        print(f""已發送：{response.status_code}"")  # 印出狀態碼 // 確認成功
    except Exception as e:  # 捕捉例外 // 網路錯誤
        print(f""發送失敗：{e}"")  # 印出錯誤 // 方便除錯

while True:  # 無限迴圈 // 持續讀取
    read_and_send()  # 執行讀取並發送 // 主要函式
    time.sleep(5)  # 等待 5 秒 // 控制讀取頻率
```

### .NET API 接收端

```csharp
// 感測器資料模型 // 接收 Python 傳來的資料
public class SensorData // 感測器資料類別
{
    public double Temperature { get; set; } // 溫度
    public double Humidity { get; set; } // 濕度
    public double Timestamp { get; set; } // 時間戳記
}

// 感測器 API 控制器 // 接收並儲存感測器資料
[ApiController] // 標記為 API 控制器
[Route(""api/[controller]"")] // 路由設定
public class SensorsController : ControllerBase // 感測器控制器
{
    private readonly ILogger<SensorsController> _logger; // 日誌記錄器

    // 建構函式 // 注入日誌
    public SensorsController(ILogger<SensorsController> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 接收感測器資料 // POST api/sensors
    [HttpPost] // HTTP POST 方法
    public IActionResult PostSensorData( // 接收資料方法
        [FromBody] SensorData data) // 從請求本體讀取
    {
        _logger.LogInformation( // 記錄接收到的資料
            ""收到感測器資料：溫度={Temp}°C, 濕度={Hum}%"", // 格式化訊息
            data.Temperature, data.Humidity); // 傳入參數

        // TODO: 儲存到資料庫 // 之後實作
        return Ok(new { status = ""received"" }); // 回傳成功
    }
}
```

---

## 感測器資料視覺化

### SignalR 即時推送

```csharp
// 感測器 SignalR Hub // 即時推送感測器資料到前端
public class SensorHub : Hub // 繼承 SignalR Hub
{
    // 推送溫度更新 // 前端即時收到溫度
    public async Task SendTemperature( // 發送溫度方法
        double temperature) // 溫度數值
    {
        await Clients.All.SendAsync( // 發送給所有連線的客戶端
            ""ReceiveTemperature"", temperature); // 觸發前端事件
    }

    // 推送濕度更新 // 前端即時收到濕度
    public async Task SendHumidity( // 發送濕度方法
        double humidity) // 濕度數值
    {
        await Clients.All.SendAsync( // 發送給所有客戶端
            ""ReceiveHumidity"", humidity); // 觸發前端事件
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：GPIO 用完沒有釋放

```csharp
// 錯誤寫法 // 沒有 Dispose 會導致針腳被鎖住
public void BlinkLed() // 閃爍 LED 方法
{
    var controller = new GpioController(); // 每次都建新的控制器
    controller.OpenPin(17, PinMode.Output); // 開啟 GPIO17
    controller.Write(17, PinValue.High); // 點亮 LED
    // 沒有 Close 也沒有 Dispose ← 針腳被鎖住！ // 忘記釋放資源
}
```

### ✅ 正確：使用 using 確保釋放

```csharp
// 正確寫法 // 使用 using 自動釋放
public void BlinkLed() // 閃爍 LED 方法
{
    using var controller = new GpioController(); // using 確保釋放
    controller.OpenPin(17, PinMode.Output); // 開啟 GPIO17
    controller.Write(17, PinValue.High); // 點亮 LED
    Thread.Sleep(1000); // 亮 1 秒
    controller.Write(17, PinValue.Low); // 關閉 LED
} // 離開 using 範圍自動 Dispose // 自動釋放資源
```

### ❌ 錯誤：MQTT 沒有處理斷線重連

```csharp
// 錯誤寫法 // 斷線後就再也收不到訊息
public class BadMqttService // 錯誤的 MQTT 服務
{
    public void Connect() // 連線方法
    {
        // 連線一次就不管了 // 斷線就完蛋
        // 沒有重連邏輯 // 網路不穩就掛掉
    }
}
```

### ✅ 正確：加入自動重連機制

```csharp
// 正確寫法 // 斷線自動重連
public class ResilientMqttService // 有韌性的 MQTT 服務
{
    private int _retryCount = 0; // 重試計數器
    private const int MAX_RETRY = 10; // 最大重試次數

    public async Task ConnectWithRetryAsync() // 帶重試的連線方法
    {
        while (_retryCount < MAX_RETRY) // 在重試上限內迴圈
        {
            try // 嘗試連線
            {
                await ConnectAsync(); // 執行連線
                _retryCount = 0; // 成功後重置計數器
                break; // 跳出迴圈
            }
            catch // 連線失敗
            {
                _retryCount++; // 增加重試計數
                var delay = Math.Min(1000 * _retryCount, 30000); // 指數退避延遲
                await Task.Delay(delay); // 等待後重試
            }
        }
    }

    private Task ConnectAsync() => Task.CompletedTask; // 實際連線邏輯
}
```
" },

        // ── IoT2 Chapter 606 ────────────────────────────
        new() { Id=606, Category="iot", Order=7, Level="intermediate", Icon="💳", Title="金流與支付整合", Slug="payment-integration", IsPublished=true, Content=@"
# 金流與支付整合

## 台灣常見金流平台

> 💡 **比喻：收銀台和銀行之間的橋梁**
> 你的網站就像一家商店，客人要付錢時不會直接把鈔票塞進電腦螢幕。
> 金流平台就是那座「橋梁」——幫你把客人的錢從他的銀行帳戶，
> 安全地搬到你的銀行帳戶，中間還幫你處理信用卡、超商代碼等等。

### 金流平台比較

```
台灣主流金流平台比較：
┌──────────────┬──────────┬──────────┬──────────────┐
│   平台名稱   │  手續費  │  撥款週期 │   適合對象   │
├──────────────┼──────────┼──────────┼──────────────┤
│ 綠界 ECPay   │ 2.75%    │ 月結     │ 中小企業     │
│ 藍新 NewebPay│ 2.6%     │ 月結     │ 中大型企業   │
│ LINE Pay     │ 3%       │ 月結     │ 行動支付     │
│ 街口支付     │ 2%       │ 月結     │ 小店家       │
│ Apple Pay    │ 需搭配   │ 依金流   │ iOS 用戶     │
│ PayPal       │ 3.4%     │ 即時     │ 跨國交易     │
└──────────────┴──────────┴──────────┴──────────────┘
```

---

## 金流串接架構

### 標準金流流程

```
金流串接完整流程：
                    ┌─────────┐
                    │  客 人   │
                    └────┬────┘
                         │ ① 按下結帳按鈕
                    ┌────▼────┐
                    │ 你的網站 │
                    └────┬────┘
                         │ ② 建立訂單 + 產生付款參數
                    ┌────▼────┐
                    │ 金流平台 │  (ECPay / 藍新)
                    └────┬────┘
                         │ ③ 顯示付款頁面（信用卡/ATM）
                    ┌────▼────┐
                    │ 銀 行   │
                    └────┬────┘
                         │ ④ 扣款成功
                    ┌────▼────┐
                    │ 金流平台 │
                    └────┬────┘
                         │ ⑤ 通知你的網站（回呼 URL）
                    ┌────▼────┐
                    │ 你的網站 │  更新訂單狀態
                    └─────────┘
```

---

## ECPay SDK 整合

### 設定金流參數

```csharp
// ECPay 設定模型 // 綠界金流設定
public class ECPaySettings // 金流設定類別
{
    public string MerchantID { get; set; } = """"; // 特店編號
    public string HashKey { get; set; } = """"; // 加密金鑰
    public string HashIV { get; set; } = """"; // 加密向量
    public string PaymentApiUrl { get; set; } = """"; // 付款 API 網址
    public string ReturnUrl { get; set; } = """"; // 付款結果回傳網址
    public string NotifyUrl { get; set; } = """"; // 付款通知回呼網址
}

// appsettings.json 設定範例 // 在設定檔中加入金流設定
// ""ECPay"": {
//     ""MerchantID"": ""3002607"",        // 測試特店編號
//     ""HashKey"": ""pwFHCqoQZGmho4w6"",  // 測試金鑰
//     ""HashIV"": ""EkRm7iFT261dpevs"",   // 測試向量
//     ""PaymentApiUrl"": ""https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5"" // 測試 API
// }
```

### 建立付款請求

```csharp
// ECPay 金流服務 // 處理綠界金流串接
public class ECPayService // 綠界金流服務類別
{
    private readonly ECPaySettings _settings; // 金流設定
    private readonly ILogger<ECPayService> _logger; // 日誌記錄器

    // 建構函式 // 注入設定和日誌
    public ECPayService( // 建構金流服務
        IOptions<ECPaySettings> settings, // 注入設定
        ILogger<ECPayService> logger) // 注入日誌
    {
        _settings = settings.Value; // 取得設定值
        _logger = logger; // 儲存日誌記錄器
    }

    // 建立付款訂單 // 產生送往 ECPay 的表單參數
    public Dictionary<string, string> CreatePayment( // 建立付款方法
        string orderId, // 訂單編號
        int amount, // 金額
        string description) // 商品描述
    {
        // 組合付款參數 // ECPay 要求的欄位
        var parameters = new Dictionary<string, string> // 建立參數字典
        {
            [""MerchantID""] = _settings.MerchantID, // 特店編號
            [""MerchantTradeNo""] = orderId, // 特店交易編號
            [""MerchantTradeDate""] = DateTime.Now.ToString(""yyyy/MM/dd HH:mm:ss""), // 交易時間
            [""PaymentType""] = ""aio"", // 付款類型（全方位）
            [""TotalAmount""] = amount.ToString(), // 交易金額
            [""TradeDesc""] = description, // 交易描述
            [""ItemName""] = description, // 商品名稱
            [""ReturnURL""] = _settings.NotifyUrl, // 付款結果通知網址
            [""OrderResultURL""] = _settings.ReturnUrl, // 付款完成導回網址
            [""ChoosePayment""] = ""ALL"", // 付款方式（全部）
            [""EncryptType""] = ""1"" // 加密類型（SHA256）
        };

        // 計算檢查碼 // 防止參數被竄改
        var checkMacValue = GenerateCheckMacValue(parameters); // 產生檢查碼
        parameters[""CheckMacValue""] = checkMacValue; // 加入檢查碼

        _logger.LogInformation(""建立 ECPay 付款：{OrderId}, ${Amount}"", // 記錄付款
            orderId, amount); // 傳入參數

        return parameters; // 回傳付款參數
    }

    // 產生檢查碼 // SHA256 雜湊驗證
    private string GenerateCheckMacValue( // 產生 CheckMacValue 方法
        Dictionary<string, string> parameters) // 付款參數
    {
        // 依照 ECPay 規則排序 // 參數名稱字母排序
        var sorted = parameters.OrderBy(p => p.Key); // 排序參數

        // 組合字串 // HashKey + 參數 + HashIV
        var raw = $""HashKey={_settings.HashKey}&""; // 開頭加 HashKey
        raw += string.Join(""&"", sorted.Select(p => $""{p.Key}={p.Value}"")); // 串接所有參數
        raw += $""&HashIV={_settings.HashIV}""; // 結尾加 HashIV

        // URL Encode 後轉小寫 // ECPay 規定
        raw = System.Net.WebUtility.UrlEncode(raw)?.ToLower() ?? """"; // 編碼並轉小寫

        // SHA256 雜湊 // 產生最終檢查碼
        using var sha256 = System.Security.Cryptography.SHA256.Create(); // 建立 SHA256
        var bytes = sha256.ComputeHash( // 計算雜湊
            System.Text.Encoding.UTF8.GetBytes(raw)); // UTF-8 編碼
        return BitConverter.ToString(bytes) // 轉為十六進位字串
            .Replace(""-"", """").ToUpper(); // 去除破折號並轉大寫
    }

    // 驗證回呼 // 確認付款通知來自 ECPay
    public bool VerifyCallback( // 驗證回呼方法
        Dictionary<string, string> formData) // 表單資料
    {
        if (!formData.ContainsKey(""CheckMacValue"")) // 檢查是否有檢查碼
            return false; // 沒有檢查碼就是偽造的

        var receivedMac = formData[""CheckMacValue""]; // 取得收到的檢查碼
        var paramsWithoutMac = formData // 排除 CheckMacValue
            .Where(p => p.Key != ""CheckMacValue"") // 過濾掉檢查碼
            .ToDictionary(p => p.Key, p => p.Value); // 重建字典

        var expectedMac = GenerateCheckMacValue(paramsWithoutMac); // 重新計算檢查碼

        return receivedMac == expectedMac; // 比對是否一致
    }
}
```

---

## 信用卡刷卡機串接

### 串口通訊刷卡

```csharp
// 刷卡機串口通訊服務 // 透過 RS232 與刷卡機通訊
public class CardReaderService : IDisposable // 刷卡機服務
{
    private readonly SerialPort _port; // 串口物件
    private readonly ILogger<CardReaderService> _logger; // 日誌記錄器

    // 建構函式 // 初始化刷卡機連線
    public CardReaderService( // 建構刷卡機服務
        ILogger<CardReaderService> logger) // 注入日誌
    {
        _logger = logger; // 儲存日誌記錄器
        _port = new SerialPort // 建立串口
        {
            PortName = ""/dev/ttyUSB1"", // 刷卡機串口
            BaudRate = 115200, // 鮑率 115200
            DataBits = 8, // 資料位元
            Parity = Parity.None, // 無同位元
            StopBits = StopBits.One // 停止位元
        };
    }

    // 發送刷卡請求 // 告訴刷卡機要刷多少錢
    public async Task<CardTransactionResult> ProcessPaymentAsync( // 刷卡方法
        decimal amount) // 刷卡金額
    {
        try // 嘗試刷卡
        {
            _port.Open(); // 開啟串口

            // 組合刷卡指令 // 依刷卡機協定格式化
            var command = FormatCommand(amount); // 格式化刷卡指令
            _port.Write(command, 0, command.Length); // 發送指令

            _logger.LogInformation(""發送刷卡請求：${Amount}"", amount); // 記錄金額

            // 等待刷卡機回應 // 最多等 60 秒
            var response = await WaitForResponseAsync( // 等待回應
                TimeSpan.FromSeconds(60)); // 超時時間

            return ParseResponse(response); // 解析回應結果
        }
        catch (Exception ex) // 捕捉例外
        {
            _logger.LogError(ex, ""刷卡處理失敗""); // 記錄錯誤
            return new CardTransactionResult // 回傳失敗結果
            {
                Success = false, // 標記失敗
                ErrorMessage = ex.Message // 錯誤訊息
            };
        }
        finally // 最終處理
        {
            if (_port.IsOpen) _port.Close(); // 確保關閉串口
        }
    }

    private byte[] FormatCommand(decimal amount) => // 格式化指令方法
        System.Text.Encoding.ASCII.GetBytes( // 轉為位元組陣列
            $""SALE:{amount:F2}\r\n""); // 刷卡指令格式

    private Task<byte[]> WaitForResponseAsync( // 等待回應方法
        TimeSpan timeout) => // 超時時間
        Task.FromResult(Array.Empty<byte>()); // 回傳空陣列（待實作）

    private CardTransactionResult ParseResponse( // 解析回應方法
        byte[] response) => // 回應位元組
        new() { Success = true }; // 回傳結果（待實作）

    public void Dispose() => _port.Dispose(); // 釋放串口資源
}

// 刷卡交易結果 // 記錄刷卡結果
public class CardTransactionResult // 交易結果類別
{
    public bool Success { get; set; } // 是否成功
    public string? AuthCode { get; set; } // 授權碼
    public string? CardNumber { get; set; } // 卡號末四碼
    public string? ErrorMessage { get; set; } // 錯誤訊息
}
```

---

## QR Code 行動支付

### 產生 QR Code

```csharp
// QR Code 支付服務 // 產生行動支付用的 QR Code
public class QrPaymentService // QR 支付服務類別
{
    private readonly ILogger<QrPaymentService> _logger; // 日誌記錄器

    // 建構函式 // 注入日誌
    public QrPaymentService( // 建構 QR 支付服務
        ILogger<QrPaymentService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 產生 LINE Pay QR Code 網址 // 客人掃碼付款
    public string GenerateLinePayUrl( // 產生 LINE Pay 網址方法
        string orderId, // 訂單編號
        int amount) // 金額
    {
        // LINE Pay API 付款請求 // 取得付款網址
        var paymentUrl = $""https://sandbox-api-pay.line.me/v3/payments/request""; // API 端點

        _logger.LogInformation( // 記錄產生 QR Code
            ""產生 LINE Pay QR：訂單 {OrderId}, ${Amount}"", // 格式化訊息
            orderId, amount); // 傳入參數

        return paymentUrl; // 回傳付款網址
    }
}
```

---

## 電子發票串接

### 電子發票服務

```csharp
// 電子發票服務 // 串接財政部電子發票 API
public class InvoiceService // 電子發票服務類別
{
    private readonly HttpClient _httpClient; // HTTP 客戶端
    private readonly ILogger<InvoiceService> _logger; // 日誌記錄器

    // 發票資料模型 // 開立發票所需資訊
    public class InvoiceRequest // 發票請求類別
    {
        public string BuyerIdentifier { get; set; } = """"; // 買方統編（空白為個人）
        public string BuyerName { get; set; } = """"; // 買方名稱
        public List<InvoiceItem> Items { get; set; } = new(); // 商品明細
        public int TotalAmount { get; set; } // 總金額
        public string CarrierType { get; set; } = """"; // 載具類型
        public string CarrierNum { get; set; } = """"; // 載具號碼
    }

    // 發票商品明細 // 每一筆商品資訊
    public class InvoiceItem // 發票商品類別
    {
        public string Name { get; set; } = """"; // 商品名稱
        public int Quantity { get; set; } // 數量
        public decimal UnitPrice { get; set; } // 單價
        public decimal Amount { get; set; } // 小計
    }

    // 建構函式 // 注入 HttpClient
    public InvoiceService( // 建構發票服務
        HttpClient httpClient, // 注入 HTTP 客戶端
        ILogger<InvoiceService> logger) // 注入日誌
    {
        _httpClient = httpClient; // 儲存 HTTP 客戶端
        _logger = logger; // 儲存日誌記錄器
    }

    // 開立電子發票 // 呼叫財政部 API
    public async Task<string?> IssueInvoiceAsync( // 開立發票方法
        InvoiceRequest request) // 發票請求
    {
        _logger.LogInformation( // 記錄開立發票
            ""開立電子發票：{Amount} 元"", request.TotalAmount); // 傳入金額

        // 呼叫財政部 API // 取得發票號碼
        // 實際實作需依照財政部規格 // 這裡是框架示範
        var invoiceNumber = $""AB-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}""; // 模擬發票號碼

        return invoiceNumber; // 回傳發票號碼
    }
}
```

---

## 退款流程處理

```csharp
// 退款服務 // 處理各種退款情境
public class RefundService // 退款服務類別
{
    private readonly ECPayService _ecpay; // 綠界金流服務
    private readonly ILogger<RefundService> _logger; // 日誌記錄器

    // 建構函式 // 注入金流服務
    public RefundService( // 建構退款服務
        ECPayService ecpay, // 注入綠界服務
        ILogger<RefundService> logger) // 注入日誌
    {
        _ecpay = ecpay; // 儲存金流服務
        _logger = logger; // 儲存日誌記錄器
    }

    // 處理退款 // 依付款方式執行退款
    public async Task<RefundResult> ProcessRefundAsync( // 退款方法
        string orderId, // 訂單編號
        decimal amount, // 退款金額
        string reason) // 退款原因
    {
        _logger.LogInformation( // 記錄退款請求
            ""處理退款：訂單 {OrderId}, ${Amount}, 原因：{Reason}"", // 格式化訊息
            orderId, amount, reason); // 傳入參數

        // 驗證退款金額 // 不能超過原始金額
        if (amount <= 0) // 金額必須大於零
        {
            return new RefundResult // 回傳失敗
            {
                Success = false, // 標記失敗
                Message = ""退款金額必須大於零"" // 錯誤訊息
            };
        }

        // 呼叫金流平台退款 API // 實際執行退款
        await Task.CompletedTask; // 非同步退款（待實作）

        return new RefundResult // 回傳成功
        {
            Success = true, // 標記成功
            Message = ""退款處理中"", // 成功訊息
            RefundId = Guid.NewGuid().ToString() // 退款編號
        };
    }
}

// 退款結果 // 退款處理回傳
public class RefundResult // 退款結果類別
{
    public bool Success { get; set; } // 是否成功
    public string Message { get; set; } = """"; // 結果訊息
    public string? RefundId { get; set; } // 退款編號
}
```

---

## 金流安全

### 防重複付款

```csharp
// 冪等性付款服務 // 防止重複扣款
public class IdempotentPaymentService // 冪等付款服務類別
{
    private readonly IDistributedCache _cache; // 分散式快取
    private readonly ILogger<IdempotentPaymentService> _logger; // 日誌記錄器

    // 建構函式 // 注入快取和日誌
    public IdempotentPaymentService( // 建構冪等付款服務
        IDistributedCache cache, // 注入分散式快取
        ILogger<IdempotentPaymentService> logger) // 注入日誌
    {
        _cache = cache; // 儲存快取
        _logger = logger; // 儲存日誌記錄器
    }

    // 處理付款（防重複） // 同一筆訂單只會扣一次
    public async Task<PaymentResult> ProcessPaymentAsync( // 防重複付款方法
        string idempotencyKey, // 冪等鍵（通常用訂單編號）
        Func<Task<PaymentResult>> paymentAction) // 實際付款動作
    {
        // 檢查是否已處理過 // 從快取查詢
        var cached = await _cache.GetStringAsync(idempotencyKey); // 查詢快取
        if (cached != null) // 如果已經處理過
        {
            _logger.LogWarning(""偵測到重複付款：{Key}"", idempotencyKey); // 記錄重複
            return System.Text.Json.JsonSerializer // 反序列化快取結果
                .Deserialize<PaymentResult>(cached)!; // 回傳之前的結果
        }

        // 執行付款 // 第一次處理
        var result = await paymentAction(); // 執行實際付款動作

        // 快取結果（24小時） // 防止重複
        await _cache.SetStringAsync(idempotencyKey, // 儲存到快取
            System.Text.Json.JsonSerializer.Serialize(result), // 序列化結果
            new DistributedCacheEntryOptions // 快取選項
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) // 24 小時後過期
            });

        return result; // 回傳付款結果
    }
}

// 付款結果 // 付款處理回傳
public class PaymentResult // 付款結果類別
{
    public bool Success { get; set; } // 是否成功
    public string? TransactionId { get; set; } // 交易編號
    public string? Message { get; set; } // 結果訊息
}
```

---

## 對帳系統設計

```csharp
// 對帳服務 // 每日自動對帳
public class ReconciliationService // 對帳服務類別
{
    private readonly ILogger<ReconciliationService> _logger; // 日誌記錄器

    // 建構函式 // 注入日誌
    public ReconciliationService( // 建構對帳服務
        ILogger<ReconciliationService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 每日對帳 // 比對系統訂單和金流平台紀錄
    public async Task<ReconciliationReport> DailyReconcileAsync( // 每日對帳方法
        DateTime date) // 對帳日期
    {
        _logger.LogInformation(""開始對帳：{Date:yyyy-MM-dd}"", date); // 記錄開始

        var report = new ReconciliationReport // 建立對帳報告
        {
            Date = date, // 對帳日期
            TotalOrders = 0, // 系統訂單數
            TotalPayments = 0, // 金流平台筆數
            MatchedCount = 0, // 對帳成功筆數
            MismatchedCount = 0 // 對帳失敗筆數
        };

        // 實際對帳邏輯 // 比對系統和金流資料
        await Task.CompletedTask; // 非同步對帳（待實作）

        return report; // 回傳對帳報告
    }
}

// 對帳報告 // 對帳結果統計
public class ReconciliationReport // 對帳報告類別
{
    public DateTime Date { get; set; } // 對帳日期
    public int TotalOrders { get; set; } // 系統訂單總數
    public int TotalPayments { get; set; } // 金流筆數
    public int MatchedCount { get; set; } // 成功比對筆數
    public int MismatchedCount { get; set; } // 失敗比對筆數
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：金流 HashKey 寫在程式碼裡

```csharp
// 錯誤寫法 // 把機密金鑰寫死在程式碼裡
public class BadPaymentService // 錯誤的金流服務
{
    private const string HashKey = ""pwFHCqoQZGmho4w6""; // 金鑰寫死在程式碼 ← 超危險！
    private const string HashIV = ""EkRm7iFT261dpevs""; // 向量也寫死 ← 會被 Git 追蹤到！
}
```

### ✅ 正確：使用環境變數或 Secret Manager

```csharp
// 正確寫法 // 金鑰放在安全的地方
public class SecurePaymentService // 安全的金流服務
{
    private readonly string _hashKey; // 金鑰（不寫死）

    public SecurePaymentService( // 建構函式
        IConfiguration config) // 注入設定
    {
        // 從環境變數或 User Secrets 讀取 // 不會出現在原始碼
        _hashKey = config[""ECPay:HashKey""] // 從設定讀取金鑰
            ?? throw new InvalidOperationException( // 找不到就拋例外
                ""ECPay:HashKey 未設定""); // 錯誤訊息
    }
}
```

### ❌ 錯誤：沒有驗證回呼來源

```csharp
// 錯誤寫法 // 任何人都能偽造付款成功通知
[HttpPost(""payment/callback"")] // 付款回呼端點
public IActionResult PaymentCallback( // 回呼方法
    [FromForm] Dictionary<string, string> data) // 表單資料
{
    // 直接信任回呼資料 ← 沒有驗證！ // 任何人都能偽造
    var orderId = data[""MerchantTradeNo""]; // 直接取訂單編號
    UpdateOrderStatus(orderId, ""paid""); // 直接更新為已付款
    return Ok(); // 回傳成功
}

private void UpdateOrderStatus(string id, string s) { } // 更新訂單狀態
```

### ✅ 正確：驗證 CheckMacValue

```csharp
// 正確寫法 // 驗證回呼是否來自金流平台
[HttpPost(""payment/callback"")] // 付款回呼端點
public IActionResult PaymentCallback( // 回呼方法
    [FromForm] Dictionary<string, string> data) // 表單資料
{
    // 先驗證 CheckMacValue // 確認是 ECPay 發的
    if (!_ecpayService.VerifyCallback(data)) // 驗證檢查碼
    {
        _logger.LogWarning(""收到偽造的付款回呼！""); // 記錄可疑行為
        return BadRequest(""驗證失敗""); // 拒絕偽造請求
    }

    // 驗證通過才更新 // 確保資料真實性
    var orderId = data[""MerchantTradeNo""]; // 取得訂單編號
    UpdateOrderStatus(orderId, ""paid""); // 安全地更新狀態
    return Ok(""1|OK""); // ECPay 要求回傳此格式
}

private ECPayService _ecpayService = null!; // 金流服務
private ILogger _logger2 = null!; // 日誌記錄器
private void UpdateOrderStatus(string id, string s) { } // 更新訂單
```
" },

        // ── IoT2 Chapter 607 ────────────────────────────
        new() { Id=607, Category="iot", Order=8, Level="advanced", Icon="🏪", Title="完整 POS 系統架構設計", Slug="pos-system-architecture", IsPublished=true, Content=@"
# 完整 POS 系統架構設計

## 系統架構圖

### 整體架構

```
POS 系統完整架構：

┌─────────────────────────────────────────────────────────┐
│                      ☁️ 雲端層                          │
│  ┌───────────┐  ┌───────────┐  ┌───────────┐           │
│  │ 中央 API  │  │  資料庫   │  │  報表系統  │           │
│  │  Server   │  │ (PostgreSQL)│ │ Dashboard │           │
│  └─────┬─────┘  └─────┬─────┘  └─────┬─────┘           │
│        └───────────────┴───────────────┘                 │
│                        │ HTTPS API                       │
└────────────────────────┼────────────────────────────────┘
                         │
         ┌───────────────┼───────────────┐
         │               │               │
    ┌────▼────┐     ┌────▼────┐    ┌────▼────┐
    │ 門市 A  │     │ 門市 B  │    │ 門市 C  │
    │  POS    │     │  POS    │    │  POS    │
    └────┬────┘     └────┬────┘    └────┬────┘
         │               │               │
    ┌────▼────────────────▼───────────────▼────┐
    │              邊緣 POS 層                  │
    │  ┌─────┐ ┌──────┐ ┌──────┐ ┌─────────┐  │
    │  │ 收銀 │ │ 出單 │ │ 刷卡 │ │ 條碼掃描 │  │
    │  │ 介面 │ │  機  │ │  機  │ │    器    │  │
    │  └─────┘ └──────┘ └──────┘ └─────────┘  │
    └──────────────────────────────────────────┘
```

---

## 資料庫設計

### 核心 Entity 設計

```csharp
// ===== 商品相關 Entity ===== // 商品管理核心

// 商品分類 // 商品的大類別（如：飲料、食品）
public class ProductCategory // 商品分類類別
{
    public int Id { get; set; } // 分類編號
    public string Name { get; set; } = """"; // 分類名稱
    public string? Description { get; set; } // 分類描述
    public int SortOrder { get; set; } // 排序順序
    public bool IsActive { get; set; } = true; // 是否啟用
    public ICollection<Product> Products { get; set; } = new List<Product>(); // 分類下的商品
}

// 商品 // 可銷售的品項
public class Product // 商品類別
{
    public int Id { get; set; } // 商品編號
    public string Barcode { get; set; } = """"; // 商品條碼
    public string Name { get; set; } = """"; // 商品名稱
    public string? Description { get; set; } // 商品描述
    public decimal Price { get; set; } // 售價
    public decimal Cost { get; set; } // 成本
    public int CategoryId { get; set; } // 分類外鍵
    public ProductCategory Category { get; set; } = null!; // 分類導航屬性
    public string? ImageUrl { get; set; } // 商品圖片網址
    public bool IsActive { get; set; } = true; // 是否上架
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 建立時間
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // 訂單明細
    public ICollection<InventoryRecord> InventoryRecords { get; set; } = new List<InventoryRecord>(); // 庫存紀錄
}
```

### 訂單 Entity

```csharp
// ===== 訂單相關 Entity ===== // 銷售核心

// 訂單 // 一筆銷售交易
public class Order // 訂單類別
{
    public int Id { get; set; } // 訂單編號
    public string OrderNumber { get; set; } = """"; // 訂單流水號
    public DateTime OrderDate { get; set; } = DateTime.UtcNow; // 訂單日期時間
    public decimal SubTotal { get; set; } // 小計（未稅）
    public decimal TaxAmount { get; set; } // 稅額
    public decimal DiscountAmount { get; set; } // 折扣金額
    public decimal TotalAmount { get; set; } // 應付金額
    public decimal PaidAmount { get; set; } // 實付金額
    public decimal ChangeAmount { get; set; } // 找零金額
    public string PaymentMethod { get; set; } = ""cash""; // 付款方式
    public string Status { get; set; } = ""completed""; // 訂單狀態
    public int? MemberId { get; set; } // 會員外鍵（可為空）
    public Member? Member { get; set; } // 會員導航屬性
    public int StoreId { get; set; } // 門市外鍵
    public Store Store { get; set; } = null!; // 門市導航屬性
    public string CashierId { get; set; } = """"; // 收銀員編號
    public string? InvoiceNumber { get; set; } // 電子發票號碼
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>(); // 訂單明細
}

// 訂單明細 // 訂單中的每一筆商品
public class OrderItem // 訂單明細類別
{
    public int Id { get; set; } // 明細編號
    public int OrderId { get; set; } // 訂單外鍵
    public Order Order { get; set; } = null!; // 訂單導航屬性
    public int ProductId { get; set; } // 商品外鍵
    public Product Product { get; set; } = null!; // 商品導航屬性
    public string ProductName { get; set; } = """"; // 商品名稱（快照）
    public decimal UnitPrice { get; set; } // 單價（快照）
    public int Quantity { get; set; } // 數量
    public decimal Discount { get; set; } // 折扣
    public decimal LineTotal { get; set; } // 小計
}
```

### 庫存與會員 Entity

```csharp
// ===== 庫存相關 Entity ===== // 庫存管理核心

// 庫存紀錄 // 進貨/銷貨/盤點/調撥
public class InventoryRecord // 庫存紀錄類別
{
    public int Id { get; set; } // 紀錄編號
    public int ProductId { get; set; } // 商品外鍵
    public Product Product { get; set; } = null!; // 商品導航屬性
    public int StoreId { get; set; } // 門市外鍵
    public Store Store { get; set; } = null!; // 門市導航屬性
    public string Type { get; set; } = """"; // 類型：in/out/adjust/transfer
    public int Quantity { get; set; } // 數量（正數進貨，負數銷貨）
    public int BalanceAfter { get; set; } // 異動後餘額
    public string? Note { get; set; } // 備註
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 建立時間
    public string OperatorId { get; set; } = """"; // 操作人員
}

// ===== 會員相關 Entity ===== // 會員管理核心

// 會員 // 註冊的客人
public class Member // 會員類別
{
    public int Id { get; set; } // 會員編號
    public string MemberNumber { get; set; } = """"; // 會員卡號
    public string Name { get; set; } = """"; // 姓名
    public string? Phone { get; set; } // 手機號碼
    public string? Email { get; set; } // 電子信箱
    public int Points { get; set; } // 累積點數
    public string Level { get; set; } = ""normal""; // 會員等級
    public DateTime JoinDate { get; set; } = DateTime.UtcNow; // 入會日期
    public DateTime? LastVisitDate { get; set; } // 最後消費日期
    public bool IsActive { get; set; } = true; // 是否有效
    public ICollection<Order> Orders { get; set; } = new List<Order>(); // 消費紀錄
    public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>(); // 擁有的優惠券
}

// 優惠券 // 會員優惠
public class Coupon // 優惠券類別
{
    public int Id { get; set; } // 優惠券編號
    public string Code { get; set; } = """"; // 優惠碼
    public string Name { get; set; } = """"; // 優惠券名稱
    public string Type { get; set; } = ""percent""; // 類型：percent/fixed
    public decimal Value { get; set; } // 折扣值（百分比或金額）
    public decimal? MinimumAmount { get; set; } // 最低消費門檻
    public DateTime StartDate { get; set; } // 開始日期
    public DateTime EndDate { get; set; } // 結束日期
    public bool IsUsed { get; set; } // 是否已使用
    public int? MemberId { get; set; } // 所屬會員
    public Member? Member { get; set; } // 會員導航屬性
}
```

### 門市與權限 Entity

```csharp
// ===== 門市相關 Entity ===== // 多店管理

// 門市 // 實體店面
public class Store // 門市類別
{
    public int Id { get; set; } // 門市編號
    public string StoreCode { get; set; } = """"; // 門市代碼
    public string Name { get; set; } = """"; // 門市名稱
    public string? Address { get; set; } // 門市地址
    public string? Phone { get; set; } // 門市電話
    public bool IsActive { get; set; } = true; // 是否營業
    public ICollection<Order> Orders { get; set; } = new List<Order>(); // 門市訂單
    public ICollection<StoreEmployee> Employees { get; set; } = new List<StoreEmployee>(); // 門市員工
}

// 門市員工 // 在門市工作的人員
public class StoreEmployee // 門市員工類別
{
    public int Id { get; set; } // 員工編號
    public string EmployeeCode { get; set; } = """"; // 員工代碼
    public string Name { get; set; } = """"; // 員工姓名
    public string Role { get; set; } = ""cashier""; // 角色：manager/cashier/inventory
    public int StoreId { get; set; } // 門市外鍵
    public Store Store { get; set; } = null!; // 門市導航屬性
    public bool IsActive { get; set; } = true; // 是否在職
}
```

---

## 多店管理架構

### 中央 Server + 邊緣 POS

```csharp
// 中央同步服務 // 管理雲端和邊緣 POS 的資料同步
public class CentralSyncService // 中央同步服務類別
{
    private readonly HttpClient _httpClient; // HTTP 客戶端
    private readonly ILogger<CentralSyncService> _logger; // 日誌記錄器

    // 建構函式 // 注入相依服務
    public CentralSyncService( // 建構同步服務
        HttpClient httpClient, // 注入 HTTP 客戶端
        ILogger<CentralSyncService> logger) // 注入日誌
    {
        _httpClient = httpClient; // 儲存 HTTP 客戶端
        _logger = logger; // 儲存日誌記錄器
    }

    // 上傳訂單到中央 // 邊緣 POS 將訂單同步到雲端
    public async Task<bool> SyncOrdersAsync( // 同步訂單方法
        List<Order> orders) // 要同步的訂單
    {
        try // 嘗試同步
        {
            var json = System.Text.Json.JsonSerializer.Serialize(orders); // 序列化訂單
            var content = new StringContent(json, // 建立請求內容
                System.Text.Encoding.UTF8, ""application/json""); // 設定 JSON 格式

            var response = await _httpClient.PostAsync( // 發送 POST 請求
                ""api/sync/orders"", content); // 到同步端點

            response.EnsureSuccessStatusCode(); // 確保成功

            _logger.LogInformation( // 記錄同步成功
                ""成功同步 {Count} 筆訂單"", orders.Count); // 傳入數量

            return true; // 回傳成功
        }
        catch (Exception ex) // 捕捉例外
        {
            _logger.LogError(ex, ""訂單同步失敗""); // 記錄錯誤
            return false; // 回傳失敗
        }
    }

    // 從中央下載最新商品 // 同步商品資料到邊緣 POS
    public async Task<List<Product>> DownloadProductsAsync() // 下載商品方法
    {
        try // 嘗試下載
        {
            var response = await _httpClient.GetAsync(""api/sync/products""); // GET 商品
            response.EnsureSuccessStatusCode(); // 確保成功

            var json = await response.Content.ReadAsStringAsync(); // 讀取回應
            var products = System.Text.Json.JsonSerializer // 反序列化
                .Deserialize<List<Product>>(json) ?? new(); // 轉為商品列表

            _logger.LogInformation( // 記錄下載成功
                ""下載了 {Count} 筆商品"", products.Count); // 傳入數量

            return products; // 回傳商品列表
        }
        catch (Exception ex) // 捕捉例外
        {
            _logger.LogError(ex, ""商品下載失敗""); // 記錄錯誤
            return new List<Product>(); // 回傳空列表
        }
    }
}
```

---

## 離線模式與資料同步

```csharp
// 離線佇列管理 // 斷網時暫存操作，恢復後同步
public class OfflineQueueService // 離線佇列服務類別
{
    private readonly Queue<OfflineAction> _queue = new(); // 離線操作佇列
    private readonly ILogger<OfflineQueueService> _logger; // 日誌記錄器
    private bool _isOnline = true; // 網路連線狀態

    // 離線操作資料結構 // 記錄離線時的操作
    public class OfflineAction // 離線操作類別
    {
        public string ActionType { get; set; } = """"; // 操作類型
        public string Data { get; set; } = """"; // 序列化的資料
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // 操作時間
        public int RetryCount { get; set; } // 重試次數
    }

    // 建構函式 // 注入日誌
    public OfflineQueueService( // 建構離線佇列服務
        ILogger<OfflineQueueService> logger) // 注入日誌
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 加入離線佇列 // 斷網時把操作暫存起來
    public void Enqueue(string actionType, string data) // 加入佇列方法
    {
        _queue.Enqueue(new OfflineAction // 建立離線操作並加入
        {
            ActionType = actionType, // 設定操作類型
            Data = data, // 設定資料
            Timestamp = DateTime.UtcNow // 設定時間
        });

        _logger.LogInformation( // 記錄加入佇列
            ""離線操作已佇列：{Type}，目前佇列數：{Count}"", // 格式化訊息
            actionType, _queue.Count); // 傳入參數
    }

    // 同步離線佇列 // 恢復網路後逐一處理
    public async Task FlushQueueAsync( // 清空佇列方法
        Func<OfflineAction, Task<bool>> processor) // 處理每個操作的委派
    {
        _logger.LogInformation( // 記錄開始同步
            ""開始同步離線佇列，共 {Count} 筆"", _queue.Count); // 顯示數量

        while (_queue.Count > 0) // 逐一處理佇列
        {
            var action = _queue.Peek(); // 查看佇列前端
            var success = await processor(action); // 處理操作

            if (success) // 如果成功
            {
                _queue.Dequeue(); // 從佇列移除
            }
            else // 如果失敗
            {
                action.RetryCount++; // 增加重試次數
                if (action.RetryCount > 5) // 超過重試上限
                {
                    _queue.Dequeue(); // 放棄此操作
                    _logger.LogError( // 記錄放棄
                        ""離線操作重試失敗已放棄：{Type}"", // 格式化訊息
                        action.ActionType); // 傳入類型
                }
                break; // 停止處理（可能又斷線了）
            }
        }
    }
}
```

---

## 報表系統

### 銷售報表服務

```csharp
// 報表服務 // 產生各種銷售報表
public class ReportService // 報表服務類別
{
    private readonly ILogger<ReportService> _logger; // 日誌記錄器

    // 日報資料模型 // 每日銷售摘要
    public class DailyReport // 日報類別
    {
        public DateTime Date { get; set; } // 報表日期
        public int TotalTransactions { get; set; } // 交易筆數
        public decimal TotalRevenue { get; set; } // 營業額
        public decimal TotalCost { get; set; } // 總成本
        public decimal GrossProfit { get; set; } // 毛利
        public decimal AverageOrderValue { get; set; } // 客單價
        public Dictionary<string, decimal> PaymentBreakdown { get; set; } = new(); // 付款方式分布
        public List<TopSellingItem> TopItems { get; set; } = new(); // 暢銷商品
    }

    // 暢銷品項 // 銷售排行
    public class TopSellingItem // 暢銷品項類別
    {
        public string ProductName { get; set; } = """"; // 商品名稱
        public int QuantitySold { get; set; } // 銷售數量
        public decimal Revenue { get; set; } // 銷售金額
    }

    // 建構函式 // 注入日誌
    public ReportService(ILogger<ReportService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 產生日報 // 統計當日銷售數據
    public DailyReport GenerateDailyReport( // 產生日報方法
        DateTime date, // 報表日期
        List<Order> orders) // 當日訂單
    {
        var report = new DailyReport // 建立日報
        {
            Date = date, // 設定日期
            TotalTransactions = orders.Count, // 計算交易筆數
            TotalRevenue = orders.Sum(o => o.TotalAmount), // 計算總營收
            AverageOrderValue = orders.Count > 0 // 計算客單價
                ? orders.Average(o => o.TotalAmount) : 0 // 有訂單才計算平均
        };

        // 付款方式分布 // 各種付款方式的金額統計
        report.PaymentBreakdown = orders // 依付款方式分組
            .GroupBy(o => o.PaymentMethod) // 分組
            .ToDictionary( // 轉為字典
                g => g.Key, // 鍵為付款方式
                g => g.Sum(o => o.TotalAmount)); // 值為金額合計

        _logger.LogInformation( // 記錄報表產生
            ""日報 {Date:MM/dd}：{Count} 筆, ${Revenue}"", // 格式化訊息
            date, report.TotalTransactions, report.TotalRevenue); // 傳入參數

        return report; // 回傳日報
    }
}
```

---

## 權限管理

```csharp
// POS 權限管理 // 依角色控制功能存取
public class PosAuthorizationService // POS 授權服務類別
{
    // 權限定義 // 各角色可執行的操作
    private readonly Dictionary<string, HashSet<string>> _rolePermissions = new() // 角色權限字典
    {
        [""manager""] = new HashSet<string> // 店長權限
        {
            ""sale"", ""refund"", ""void"", // 銷售、退款、作廢
            ""report"", ""inventory"", // 報表、庫存
            ""member"", ""settings"", ""discount"" // 會員、設定、折扣
        },
        [""cashier""] = new HashSet<string> // 收銀員權限
        {
            ""sale"", ""member"" // 只能銷售和查會員
        },
        [""inventory""] = new HashSet<string> // 倉管權限
        {
            ""inventory"", ""report"" // 庫存和報表
        }
    };

    // 檢查權限 // 確認員工是否有權執行操作
    public bool HasPermission( // 檢查權限方法
        string role, string action) // 角色和操作
    {
        if (!_rolePermissions.ContainsKey(role)) // 如果角色不存在
            return false; // 回傳無權限

        return _rolePermissions[role].Contains(action); // 檢查是否包含該操作
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：訂單金額在前端計算

```csharp
// 錯誤寫法 // 讓前端算金額，後端直接存
[HttpPost(""api/orders"")] // 建立訂單 API
public IActionResult CreateOrder( // 建立訂單方法
    [FromBody] Order order) // 直接接收前端送來的訂單（含金額）
{
    // 直接存入資料庫 ← 前端送什麼就存什麼！ // 客人可以竄改金額
    _db.Orders.Add(order); // 直接儲存
    _db.SaveChanges(); // 存入資料庫
    return Ok(); // 回傳成功
}

private dynamic _db = null!; // 資料庫（示意）
```

### ✅ 正確：後端重新計算金額

```csharp
// 正確寫法 // 後端根據商品 ID 和數量重新計算
[HttpPost(""api/orders"")] // 建立訂單 API
public IActionResult CreateOrder( // 建立訂單方法
    [FromBody] CreateOrderRequest request) // 只接收商品 ID 和數量
{
    var order = new Order(); // 建立新訂單
    decimal total = 0; // 初始化總金額

    foreach (var item in request.Items) // 逐筆處理商品
    {
        var product = _db2.Products.Find(item.ProductId); // 從資料庫查商品
        if (product == null) continue; // 商品不存在就跳過

        var lineTotal = product.Price * item.Quantity; // 後端計算小計
        total += lineTotal; // 累加總金額

        order.Items.Add(new OrderItem // 加入訂單明細
        {
            ProductId = product.Id, // 商品 ID
            ProductName = product.Name, // 商品名稱（快照）
            UnitPrice = product.Price, // 單價（從資料庫取）
            Quantity = item.Quantity, // 數量
            LineTotal = lineTotal // 小計
        });
    }

    order.TotalAmount = total; // 後端計算的總金額
    _db2.Orders.Add(order); // 儲存訂單
    _db2.SaveChanges(); // 存入資料庫
    return Ok(order); // 回傳訂單
}

// 前端只需送這些 // 不包含金額
public class CreateOrderRequest // 建立訂單請求類別
{
    public List<OrderItemRequest> Items { get; set; } = new(); // 商品列表
}

public class OrderItemRequest // 訂單商品請求
{
    public int ProductId { get; set; } // 商品 ID
    public int Quantity { get; set; } // 數量
}

private dynamic _db2 = null!; // 資料庫（示意）
```

### ❌ 錯誤：離線時拒絕所有操作

```csharp
// 錯誤寫法 // 斷網就不能用 ← POS 怎麼能停擺！
public class BadPosService // 錯誤的 POS 服務
{
    public bool ProcessSale() // 處理銷售方法
    {
        if (!IsOnline()) // 如果沒有網路
            throw new Exception(""無法連線，請稍後再試""); // 直接拒絕
        return true; // 有網路才處理
    }

    private bool IsOnline() => false; // 檢查網路狀態
}
```

### ✅ 正確：離線模式繼續服務

```csharp
// 正確寫法 // 斷網也能繼續收銀
public class ResilientPosService // 有韌性的 POS 服務
{
    private readonly OfflineQueueService _offlineQueue; // 離線佇列

    public ResilientPosService( // 建構函式
        OfflineQueueService offlineQueue) // 注入離線佇列
    {
        _offlineQueue = offlineQueue; // 儲存離線佇列
    }

    public bool ProcessSale(Order order) // 處理銷售方法
    {
        // 先存到本地資料庫 // 確保資料不會遺失
        SaveToLocalDb(order); // 存入本地 SQLite

        // 嘗試同步到雲端 // 失敗就加入離線佇列
        if (!TrySyncToCloud(order)) // 如果同步失敗
        {
            _offlineQueue.Enqueue(""order"", // 加入離線佇列
                System.Text.Json.JsonSerializer.Serialize(order)); // 序列化訂單
        }

        return true; // 不管有沒有網路都回傳成功
    }

    private void SaveToLocalDb(Order o) { } // 存到本地資料庫
    private bool TrySyncToCloud(Order o) => false; // 嘗試同步到雲端
}
```
" },

        // ── IoT2 Chapter 608 ────────────────────────────
        new() { Id=608, Category="iot", Order=9, Level="advanced", Icon="🔧", Title="Raspberry Pi 進階維運", Slug="raspberry-pi-advanced-ops", IsPublished=true, Content=@"
# Raspberry Pi 進階維運

## 遠端管理多台 Pi（Ansible）

### Ansible 安裝與設定

```bash
# 在管理機上安裝 Ansible // 從管理機統一控制所有 Pi
sudo apt install -y ansible  // 安裝 Ansible

# 建立主機清單檔案 // 列出所有要管理的 Pi
cat > /etc/ansible/hosts << 'EOF'  // 寫入主機清單
[pos_machines]                     // POS 機器群組
pos-store-a ansible_host=192.168.1.101 ansible_user=pi  // 門市 A 的 POS
pos-store-b ansible_host=192.168.1.102 ansible_user=pi  // 門市 B 的 POS
pos-store-c ansible_host=192.168.1.103 ansible_user=pi  // 門市 C 的 POS

[kiosk_machines]                   // Kiosk 機器群組
kiosk-lobby ansible_host=192.168.1.201 ansible_user=pi  // 大廳 Kiosk
kiosk-entrance ansible_host=192.168.1.202 ansible_user=pi  // 入口 Kiosk

[all:vars]                         // 所有機器的共用變數
ansible_python_interpreter=/usr/bin/python3  // 指定 Python 路徑
EOF
```

### Ansible Playbook 範例

```yaml
# deploy_pos.yml // POS 部署 Playbook
---
- name: 部署 POS 應用程式    # Playbook 名稱 // 描述用途
  hosts: pos_machines          # 目標主機群組 // 所有 POS 機器
  become: yes                  # 使用 sudo // 需要管理員權限

  tasks:                       # 任務列表 // 依序執行
    - name: 更新系統套件       # 任務名稱 // 確保系統最新
      apt:                     # 使用 apt 模組 // 套件管理
        update_cache: yes      # 更新套件庫 // apt update
        upgrade: yes           # 升級套件 // apt upgrade

    - name: 安裝 .NET Runtime  # 任務名稱 // 安裝執行環境
      shell: |                 # 使用 shell 模組 // 執行指令
        curl -sSL https://dot.net/v1/dotnet-install.sh | bash  # 安裝腳本 // 官方安裝

    - name: 部署 POS 應用      # 任務名稱 // 複製應用程式
      copy:                    # 使用 copy 模組 // 檔案複製
        src: ./publish/         # 來源路徑 // 本地編譯結果
        dest: /opt/pos-app/    # 目標路徑 // Pi 上的安裝位置

    - name: 重啟 POS 服務      # 任務名稱 // 套用新版本
      systemd:                 # 使用 systemd 模組 // 服務管理
        name: pos-app          # 服務名稱 // POS 應用服務
        state: restarted       # 狀態 // 重啟服務
```

### 批量執行指令

```bash
# 對所有 POS 機器執行指令 // 一次管理多台
ansible pos_machines -m shell -a ""uptime""  // 查看所有機器運行時間

# 批量重啟 Kiosk 服務 // 一次重啟所有 Kiosk
ansible kiosk_machines -m systemd -a ""name=kiosk state=restarted""  // 重啟服務

# 執行 Playbook // 部署更新到所有 POS
ansible-playbook deploy_pos.yml  // 執行部署腳本
```

---

## SSH Tunnel 穿越 NAT

> 💡 **比喻：挖隧道回家**
> 你的 Pi 在門市的區域網路裡，就像住在一個封閉社區裡的人。
> SSH Tunnel 就是從社區裡「挖一條隧道」通到外面的伺服器，
> 這樣你在任何地方都能透過這條隧道連回門市的 Pi。

### 反向 SSH Tunnel

```bash
# 在 Pi 上建立反向隧道 // 從門市 Pi 連到雲端伺服器
ssh -fN \                              // 背景執行（不開 shell）
  -R 2222:localhost:22 \               // 反向轉發：雲端的 2222 → Pi 的 22
  -o ServerAliveInterval=60 \          // 每 60 秒發送心跳
  -o ServerAliveCountMax=3 \           // 心跳失敗 3 次就斷線
  -o ExitOnForwardFailure=yes \        // 轉發失敗就退出
  tunnel-user@cloud-server.com         // 雲端伺服器帳號

# 從雲端伺服器連回 Pi // 透過隧道遠端管理
ssh -p 2222 pi@localhost               // 連到本地 2222 就是連到 Pi
```

### 自動重連腳本

```bash
#!/bin/bash                              // 指定直譯器
# autossh_tunnel.sh // 自動重連 SSH 隧道

REMOTE_USER=""tunnel""                   // 雲端伺服器帳號
REMOTE_HOST=""cloud-server.com""         // 雲端伺服器位址
REMOTE_PORT=2222                         // 雲端轉發埠號
LOCAL_PORT=22                            // 本地 SSH 埠號
LOG_FILE=""/var/log/ssh-tunnel.log""     // 隧道日誌檔

while true; do                           // 無限迴圈
    echo ""$(date): 建立 SSH 隧道..."" >> $LOG_FILE  // 記錄嘗試連線

    # 建立隧道 // 斷線會自動跳到下一輪迴圈
    ssh -N \                             // 不開 shell
      -R ${REMOTE_PORT}:localhost:${LOCAL_PORT} \  // 反向轉發
      -o ServerAliveInterval=60 \        // 心跳間隔
      -o ServerAliveCountMax=3 \         // 心跳重試
      -o ExitOnForwardFailure=yes \      // 失敗就退出
      -o StrictHostKeyChecking=no \      // 不檢查主機金鑰
      ${REMOTE_USER}@${REMOTE_HOST}      // 遠端伺服器

    echo ""$(date): SSH 隧道斷開，10秒後重連..."" >> $LOG_FILE  // 記錄斷線
    sleep 10                             // 等 10 秒再重連
done
```

### systemd service 設定

```bash
# SSH Tunnel 自動啟動服務 // 開機自動建立隧道
sudo cat > /etc/systemd/system/ssh-tunnel.service << 'EOF'  // 寫入 service 檔
[Unit]                                   // 單元設定
Description=SSH Reverse Tunnel           // 服務描述
After=network-online.target              // 網路就緒後啟動
Wants=network-online.target              // 需要網路

[Service]                                // 服務設定
ExecStart=/home/pi/autossh_tunnel.sh     // 啟動腳本
Restart=always                           // 總是重啟
RestartSec=10                            // 重啟間隔
User=pi                                  // 執行用戶

[Install]                                // 安裝設定
WantedBy=multi-user.target               // 多用戶模式啟動
EOF

sudo systemctl enable ssh-tunnel.service  // 啟用自動啟動
sudo systemctl start ssh-tunnel.service   // 立即啟動
```

---

## 自動更新機制

### Webhook 觸發更新

```csharp
// 自動更新控制器 // 接收 Webhook 觸發部署
[ApiController] // 標記為 API 控制器
[Route(""api/[controller]"")] // 路由設定
public class DeployController : ControllerBase // 部署控制器
{
    private readonly ILogger<DeployController> _logger; // 日誌記錄器

    // 建構函式 // 注入日誌
    public DeployController( // 建構部署控制器
        ILogger<DeployController> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 接收部署 Webhook // GitHub Actions 完成後呼叫
    [HttpPost(""webhook"")] // POST api/deploy/webhook
    public IActionResult TriggerDeploy( // 觸發部署方法
        [FromHeader(Name = ""X-Deploy-Token"")] string? token) // 部署令牌
    {
        // 驗證令牌 // 防止未授權的部署
        if (token != Environment.GetEnvironmentVariable(""DEPLOY_TOKEN"")) // 比對令牌
        {
            _logger.LogWarning(""收到未授權的部署請求""); // 記錄可疑行為
            return Unauthorized(); // 拒絕未授權請求
        }

        // 執行部署腳本 // 背景更新應用程式
        _logger.LogInformation(""開始執行部署...""); // 記錄開始部署

        Task.Run(() => ExecuteDeployScript()); // 背景執行部署

        return Ok(new { message = ""部署已觸發"" }); // 回傳成功
    }

    // 執行部署腳本 // 實際的更新步驟
    private void ExecuteDeployScript() // 部署腳本方法
    {
        try // 嘗試部署
        {
            var process = new System.Diagnostics.Process(); // 建立程序
            process.StartInfo.FileName = ""/home/pi/deploy.sh""; // 部署腳本路徑
            process.StartInfo.UseShellExecute = false; // 不使用 shell
            process.Start(); // 啟動部署
            process.WaitForExit(); // 等待完成

            _logger.LogInformation(""部署完成，退出碼：{Code}"", // 記錄結果
                process.ExitCode); // 傳入退出碼
        }
        catch (Exception ex) // 捕捉例外
        {
            _logger.LogError(ex, ""部署失敗""); // 記錄錯誤
        }
    }
}
```

---

## 系統監控

### C# 系統監控服務

```csharp
// 系統監控服務 // 監控 Pi 的 CPU、記憶體、溫度
public class SystemMonitorService // 系統監控服務類別
{
    private readonly ILogger<SystemMonitorService> _logger; // 日誌記錄器

    // 系統狀態模型 // 記錄各項系統指標
    public class SystemStatus // 系統狀態類別
    {
        public double CpuUsagePercent { get; set; } // CPU 使用率
        public double MemoryUsagePercent { get; set; } // 記憶體使用率
        public double TemperatureCelsius { get; set; } // CPU 溫度
        public double DiskUsagePercent { get; set; } // 磁碟使用率
        public TimeSpan Uptime { get; set; } // 系統運行時間
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // 記錄時間
    }

    // 建構函式 // 注入日誌
    public SystemMonitorService( // 建構監控服務
        ILogger<SystemMonitorService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 取得系統狀態 // 讀取各項指標
    public SystemStatus GetStatus() // 取得狀態方法
    {
        var status = new SystemStatus // 建立狀態物件
        {
            CpuUsagePercent = GetCpuUsage(), // 讀取 CPU 使用率
            MemoryUsagePercent = GetMemoryUsage(), // 讀取記憶體使用率
            TemperatureCelsius = GetCpuTemperature(), // 讀取 CPU 溫度
            DiskUsagePercent = GetDiskUsage(), // 讀取磁碟使用率
            Uptime = GetUptime() // 讀取運行時間
        };

        // 溫度過高警告 // 超過 70°C 就記錄警告
        if (status.TemperatureCelsius > 70) // 溫度門檻
        {
            _logger.LogWarning(""CPU 溫度過高：{Temp}°C"", // 記錄警告
                status.TemperatureCelsius); // 傳入溫度
        }

        return status; // 回傳狀態
    }

    // 讀取 CPU 溫度 // 從系統檔案取得
    private double GetCpuTemperature() // CPU 溫度方法
    {
        try // 嘗試讀取
        {
            var temp = System.IO.File.ReadAllText( // 讀取溫度檔案
                ""/sys/class/thermal/thermal_zone0/temp""); // Linux 溫度檔路徑
            return double.Parse(temp.Trim()) / 1000.0; // 轉換為攝氏度
        }
        catch // 讀取失敗
        {
            return -1; // 回傳 -1 表示無法讀取
        }
    }

    // 讀取記憶體使用率 // 從 /proc/meminfo 取得
    private double GetMemoryUsage() // 記憶體使用率方法
    {
        try // 嘗試讀取
        {
            var info = System.IO.File.ReadAllLines(""/proc/meminfo""); // 讀取記憶體資訊
            var total = ParseMemInfo(info, ""MemTotal""); // 取得總記憶體
            var available = ParseMemInfo(info, ""MemAvailable""); // 取得可用記憶體
            return total > 0 ? (1.0 - (double)available / total) * 100 : 0; // 計算使用率
        }
        catch // 讀取失敗
        {
            return -1; // 回傳 -1
        }
    }

    private long ParseMemInfo(string[] lines, string key) => // 解析記憶體資訊
        long.TryParse(lines // 從行列中尋找
            .FirstOrDefault(l => l.StartsWith(key))? // 找到指定鍵
            .Split(':').LastOrDefault()? // 取值部分
            .Trim().Split(' ').FirstOrDefault(), out var val) // 取數字
            ? val : 0; // 回傳解析結果

    private double GetCpuUsage() => 0; // CPU 使用率（待實作）
    private double GetDiskUsage() => 0; // 磁碟使用率（待實作）
    private TimeSpan GetUptime() => TimeSpan.Zero; // 運行時間（待實作）
}
```

---

## Log 集中管理

### Serilog 設定

```csharp
// Serilog 集中日誌設定 // 將 Pi 的日誌傳到中央 Server
// Program.cs 中的設定 // 應用程式啟動時設定

// 安裝 NuGet 套件 // Serilog 相關套件
// dotnet add package Serilog.AspNetCore // ASP.NET Core 整合
// dotnet add package Serilog.Sinks.Http // HTTP 傳送
// dotnet add package Serilog.Sinks.File // 本地檔案備份

// 設定 Serilog // 同時寫入本地和遠端
public static class SerilogConfig // Serilog 設定類別
{
    public static void Configure( // 設定方法
        string machineName, // 機器名稱
        string centralLogUrl) // 中央日誌伺服器網址
    {
        Log.Logger = new LoggerConfiguration() // 建立日誌設定
            .MinimumLevel.Information() // 最低記錄等級
            .Enrich.WithProperty(""MachineName"", machineName) // 加入機器名稱
            .Enrich.WithProperty(""AppName"", ""POS"") // 加入應用名稱
            .WriteTo.Console() // 輸出到主控台
            .WriteTo.File( // 寫入本地檔案
                ""/var/log/pos-app/log-.txt"", // 本地日誌路徑
                rollingInterval: RollingInterval.Day, // 每日輪替
                retainedFileCountLimit: 7) // 保留 7 天
            .WriteTo.Http(centralLogUrl) // 傳送到中央伺服器
            .CreateLogger(); // 建立 Logger
    }
}
```

---

## SD 卡防損壞（Read-Only 檔案系統）

```bash
# 設定 Read-Only 根檔案系統 // 防止突然斷電導致 SD 卡損壞

# 安裝 overlayroot // 覆蓋式唯讀檔案系統
sudo apt install -y overlayroot        // 安裝 overlayroot

# 編輯 overlayroot 設定 // 啟用唯讀模式
sudo cat > /etc/overlayroot.local.conf << 'EOF'  // 寫入設定
overlayroot=""tmpfs""                  // 使用 tmpfs 作為覆蓋層
EOF

# 掛載 tmpfs 給需要寫入的目錄 // 日誌和暫存檔案
echo ""tmpfs /var/log tmpfs defaults,noatime,size=50M 0 0"" | \  // 日誌目錄
  sudo tee -a /etc/fstab              // 加入開機掛載

echo ""tmpfs /tmp tmpfs defaults,noatime,size=100M 0 0"" | \  // 暫存目錄
  sudo tee -a /etc/fstab              // 加入開機掛載

# 需要永久寫入時暫時解除唯讀 // 更新程式時使用
sudo overlayroot-chroot               // 進入可寫入模式
# 在這裡做需要永久保存的修改 // 例如更新應用程式
exit                                   // 退出可寫入模式
```

---

## 備份與還原

### SD 卡映像檔備份

```bash
# 備份整張 SD 卡 // 建立完整映像檔
sudo dd if=/dev/mmcblk0 \              // 來源：SD 卡裝置
  of=/backup/pos-image-$(date +%Y%m%d).img \  // 目標：映像檔（含日期）
  bs=4M \                              // 區塊大小 4MB
  status=progress                      // 顯示進度

# 壓縮映像檔 // 節省儲存空間
sudo gzip /backup/pos-image-$(date +%Y%m%d).img  // 壓縮映像檔

# 還原映像檔 // 將備份寫回 SD 卡
sudo dd if=/backup/pos-image-20240101.img.gz \  // 來源：壓縮映像檔
  of=/dev/sdX \                        // 目標：新的 SD 卡
  bs=4M \                              // 區塊大小 4MB
  status=progress                      // 顯示進度
```

### 自動備份腳本

```bash
#!/bin/bash                              // 指定直譯器
# auto_backup.sh // 每週自動備份 POS 資料

BACKUP_DIR=""/backup""                   // 備份目錄
DB_FILE=""/opt/pos-app/pos.db""          // SQLite 資料庫
REMOTE_SERVER=""backup@cloud-server.com"" // 遠端備份伺服器
LOG_FILE=""/var/log/backup.log""         // 備份日誌

echo ""$(date): 開始備份..."" >> $LOG_FILE  // 記錄開始

# 備份 SQLite 資料庫 // 使用 .backup 指令避免鎖定
sqlite3 $DB_FILE "".backup '$BACKUP_DIR/pos-db-$(date +%Y%m%d).db'""  // 備份資料庫

# 備份設定檔 // 應用程式設定
cp /opt/pos-app/appsettings.json $BACKUP_DIR/  // 複製設定檔

# 壓縮備份 // 節省空間和傳輸時間
tar czf $BACKUP_DIR/backup-$(date +%Y%m%d).tar.gz \  // 建立壓縮檔
  $BACKUP_DIR/pos-db-*.db \            // 包含資料庫
  $BACKUP_DIR/appsettings.json         // 包含設定檔

# 上傳到遠端 // 異地備份
scp $BACKUP_DIR/backup-$(date +%Y%m%d).tar.gz \  // 安全複製
  $REMOTE_SERVER:/backup/              // 到遠端伺服器

# 清理 7 天前的本地備份 // 避免塞滿磁碟
find $BACKUP_DIR -name ""backup-*.tar.gz"" -mtime +7 -delete  // 刪除舊備份

echo ""$(date): 備份完成"" >> $LOG_FILE  // 記錄完成
```

---

## UPS 不斷電設計

### UPS 監控服務

```csharp
// UPS 不斷電監控服務 // 偵測停電並安全關機
public class UpsMonitorService // UPS 監控服務類別
{
    private readonly ILogger<UpsMonitorService> _logger; // 日誌記錄器

    // UPS 狀態模型 // 記錄 UPS 當前狀態
    public class UpsStatus // UPS 狀態類別
    {
        public bool IsOnBattery { get; set; } // 是否在使用電池
        public int BatteryPercent { get; set; } // 電池電量百分比
        public int EstimatedMinutes { get; set; } // 預估剩餘時間（分鐘）
        public string PowerSource { get; set; } = ""AC""; // 電源來源
    }

    // 建構函式 // 注入日誌
    public UpsMonitorService( // 建構 UPS 監控
        ILogger<UpsMonitorService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 監控 UPS 狀態 // 持續檢查電源狀態
    public async Task MonitorAsync( // 監控方法
        CancellationToken ct) // 取消令牌
    {
        while (!ct.IsCancellationRequested) // 持續監控
        {
            var status = await GetUpsStatusAsync(); // 取得 UPS 狀態

            if (status.IsOnBattery) // 如果使用電池（停電了）
            {
                _logger.LogWarning( // 記錄停電警告
                    ""停電中！電池 {Percent}%，預估剩餘 {Min} 分鐘"", // 格式化
                    status.BatteryPercent, status.EstimatedMinutes); // 傳入參數

                if (status.BatteryPercent < 20) // 電量低於 20%
                {
                    _logger.LogCritical(""電量不足，執行安全關機！""); // 緊急記錄
                    await SafeShutdownAsync(); // 安全關機
                }
            }

            await Task.Delay(10000, ct); // 每 10 秒檢查一次
        }
    }

    // 安全關機 // 先儲存資料再關機
    private async Task SafeShutdownAsync() // 安全關機方法
    {
        _logger.LogInformation(""開始安全關機流程...""); // 記錄開始

        // 1. 儲存未完成的交易 // 防止資料遺失
        // 2. 同步離線佇列 // 盡量同步到雲端
        // 3. 關閉應用程式 // 優雅停止
        // 4. 執行系統關機 // 最後一步

        await Task.Delay(1000); // 等待儲存完成

        var process = new System.Diagnostics.Process(); // 建立程序
        process.StartInfo.FileName = ""sudo""; // 使用 sudo
        process.StartInfo.Arguments = ""shutdown -h now""; // 立即關機
        process.Start(); // 執行關機
    }

    private Task<UpsStatus> GetUpsStatusAsync() => // 取得 UPS 狀態
        Task.FromResult(new UpsStatus()); // 回傳狀態（待實作）
}
```

---

## 故障排除 SOP

```csharp
// 故障排除服務 // 自動診斷常見問題
public class TroubleshootService // 故障排除服務類別
{
    private readonly ILogger<TroubleshootService> _logger; // 日誌記錄器

    // 診斷結果 // 記錄檢查項目和狀態
    public class DiagnosticResult // 診斷結果類別
    {
        public string CheckName { get; set; } = """"; // 檢查項目名稱
        public bool Passed { get; set; } // 是否通過
        public string Message { get; set; } = """"; // 結果訊息
        public string? Suggestion { get; set; } // 建議修復方式
    }

    // 建構函式 // 注入日誌
    public TroubleshootService( // 建構故障排除服務
        ILogger<TroubleshootService> logger) // 注入 Logger
    {
        _logger = logger; // 儲存日誌記錄器
    }

    // 執行完整診斷 // 檢查所有常見問題
    public async Task<List<DiagnosticResult>> RunDiagnosticsAsync() // 執行診斷方法
    {
        var results = new List<DiagnosticResult>(); // 建立結果列表

        // 1. 檢查網路連線 // 確認能上網
        results.Add(await CheckNetworkAsync()); // 加入網路檢查結果

        // 2. 檢查磁碟空間 // 確認還有空間
        results.Add(CheckDiskSpace()); // 加入磁碟檢查結果

        // 3. 檢查 CPU 溫度 // 確認沒過熱
        results.Add(CheckCpuTemperature()); // 加入溫度檢查結果

        // 4. 檢查服務狀態 // 確認應用程式在跑
        results.Add(CheckServiceStatus()); // 加入服務檢查結果

        // 5. 檢查資料庫連線 // 確認 DB 正常
        results.Add(await CheckDatabaseAsync()); // 加入資料庫檢查結果

        // 記錄診斷結果 // 方便事後追蹤
        foreach (var r in results) // 逐一記錄
        {
            if (r.Passed) // 如果通過
                _logger.LogInformation(""✓ {Check}: {Msg}"", r.CheckName, r.Message); // 記錄成功
            else // 如果失敗
                _logger.LogWarning(""✗ {Check}: {Msg}"", r.CheckName, r.Message); // 記錄警告
        }

        return results; // 回傳所有結果
    }

    // 檢查網路 // ping 外部主機
    private async Task<DiagnosticResult> CheckNetworkAsync() // 網路檢查方法
    {
        try // 嘗試連線
        {
            using var client = new HttpClient(); // 建立 HTTP 客戶端
            client.Timeout = TimeSpan.FromSeconds(5); // 超時 5 秒
            await client.GetAsync(""https://www.google.com""); // 嘗試連線

            return new DiagnosticResult // 回傳成功
            {
                CheckName = ""網路連線"", // 檢查名稱
                Passed = true, // 通過
                Message = ""網路連線正常"" // 成功訊息
            };
        }
        catch // 連線失敗
        {
            return new DiagnosticResult // 回傳失敗
            {
                CheckName = ""網路連線"", // 檢查名稱
                Passed = false, // 未通過
                Message = ""無法連線到網路"", // 失敗訊息
                Suggestion = ""檢查網路線或 Wi-Fi 設定"" // 建議
            };
        }
    }

    private DiagnosticResult CheckDiskSpace() => new() // 磁碟檢查
    {
        CheckName = ""磁碟空間"", Passed = true, Message = ""磁碟空間充足"" // 預設結果
    };

    private DiagnosticResult CheckCpuTemperature() => new() // 溫度檢查
    {
        CheckName = ""CPU 溫度"", Passed = true, Message = ""溫度正常"" // 預設結果
    };

    private DiagnosticResult CheckServiceStatus() => new() // 服務檢查
    {
        CheckName = ""應用服務"", Passed = true, Message = ""服務運行中"" // 預設結果
    };

    private Task<DiagnosticResult> CheckDatabaseAsync() => // 資料庫檢查
        Task.FromResult(new DiagnosticResult // 回傳結果
        {
            CheckName = ""資料庫"", Passed = true, Message = ""資料庫連線正常"" // 預設結果
        });
}
```

---

## 批量佈署（Pi Imager + cloud-init）

### cloud-init 設定

```yaml
# user-data // cloud-init 自動設定檔
#cloud-config                          # cloud-init 標記 // 必須放在第一行

hostname: pos-store-001                # 設定主機名稱 // 每台 Pi 不同

users:                                 # 建立用戶 // 設定管理帳號
  - name: pi                           # 用戶名稱 // 預設帳號
    groups: sudo                       # 群組 // 加入 sudo 群組
    shell: /bin/bash                   # Shell // 使用 Bash
    sudo: ALL=(ALL) NOPASSWD:ALL       # sudo 權限 // 免密碼

packages:                              # 安裝套件 // 自動安裝需要的軟體
  - chromium-browser                   # Chromium 瀏覽器 // Kiosk 用
  - unclutter                          # 隱藏游標 // Kiosk 用
  - sqlite3                            # SQLite // 本地資料庫

runcmd:                                # 執行指令 // 首次開機執行
  - curl -sSL https://dot.net/v1/dotnet-install.sh | bash  # 安裝 .NET // 執行環境
  - mkdir -p /opt/pos-app              # 建立應用目錄 // POS 安裝路徑
  - systemctl enable kiosk.service     # 啟用 Kiosk // 開機自動啟動
```

### 批量燒錄腳本

```bash
#!/bin/bash                              // 指定直譯器
# batch_flash.sh // 批量燒錄 SD 卡

IMAGE_FILE=""pos-base-image.img""        // 基礎映像檔
STORE_LIST=(""store-001"" ""store-002"" ""store-003"")  // 門市列表

echo ""批量燒錄開始""                    // 顯示開始訊息

for STORE in ""${STORE_LIST[@]}""; do    // 逐一處理每間門市
    echo ""準備 ${STORE} 的 SD 卡...""   // 顯示進度
    echo ""請插入 SD 卡後按 Enter""      // 提示插入 SD 卡
    read                                 // 等待使用者按 Enter

    # 偵測新插入的 SD 卡 // 通常是 /dev/sdX
    DEVICE=$(lsblk -dpno NAME | tail -1)  // 取得最後插入的裝置

    # 燒錄映像檔 // 寫入 SD 卡
    echo ""燒錄到 ${DEVICE}...""         // 顯示燒錄目標
    sudo dd if=$IMAGE_FILE of=$DEVICE bs=4M status=progress  // 執行燒錄

    # 修改 hostname // 讓每台 Pi 有不同的名稱
    sudo mount ${DEVICE}2 /mnt          // 掛載 SD 卡分區
    echo ""$STORE"" | sudo tee /mnt/etc/hostname  // 寫入主機名稱
    sudo umount /mnt                    // 卸載分區

    echo ""${STORE} 燒錄完成！""         // 顯示完成
done

echo ""全部燒錄完成！""                  // 顯示全部完成
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤：SD 卡直接斷電不關機

```bash
# 錯誤做法 // 直接拔電源
# 直接拔掉 Pi 的電源線 ← SD 卡可能損壞！ // 正在寫入的資料會毀損
# 尤其是正在寫入資料庫的時候 // 資料庫可能整個壞掉
```

### ✅ 正確：使用 UPS + 安全關機

```bash
# 正確做法 // 透過 UPS 偵測停電後安全關機
# 1. 安裝 UPS // 不斷電系統
# 2. 偵測到停電 → 儲存資料 // 先存檔
# 3. 執行 sudo shutdown -h now // 安全關機
sudo shutdown -h now                   // 正確的關機指令
```

### ❌ 錯誤：所有 Pi 用同一個 SSH 金鑰

```bash
# 錯誤做法 // 一把鑰匙開所有門
# 把同一個 SSH 私鑰複製到所有 Pi ← 一台被駭全部中招！ // 嚴重安全漏洞
ssh-copy-id -i ~/.ssh/shared_key pi@all-machines  // 共用金鑰很危險
```

### ✅ 正確：每台 Pi 獨立金鑰 + 集中管理

```bash
# 正確做法 // 每台 Pi 有自己的金鑰
for host in store-a store-b store-c; do  // 逐台處理
    ssh-keygen -t ed25519 \              // 產生 ED25519 金鑰
      -f ~/.ssh/key_${host} \            // 每台一個獨立金鑰
      -N """"                            // 無密碼（搭配 ssh-agent）
    ssh-copy-id -i ~/.ssh/key_${host}.pub pi@${host}  // 部署公鑰到 Pi
done
```

### ❌ 錯誤：日誌只存在本地 SD 卡

```csharp
// 錯誤寫法 // 日誌只寫到本地 SD 卡
public class BadLogging // 錯誤的日誌設定
{
    public void Configure() // 設定方法
    {
        // 只寫到本地檔案 ← SD 卡壞了就看不到了！ // 也會加速 SD 卡損壞
        Log.Logger = new LoggerConfiguration() // 建立設定
            .WriteTo.File(""/var/log/app.log"") // 只寫本地
            .CreateLogger(); // 建立 Logger
    }
}
```

### ✅ 正確：日誌同時寫到本地和遠端

```csharp
// 正確寫法 // 本地 + 遠端雙重保險
public class GoodLogging // 正確的日誌設定
{
    public void Configure() // 設定方法
    {
        Log.Logger = new LoggerConfiguration() // 建立設定
            .WriteTo.File(""/var/log/app.log"", // 本地備份
                rollingInterval: RollingInterval.Day, // 每日輪替
                retainedFileCountLimit: 3) // 只保留 3 天（省 SD 卡空間）
            .WriteTo.Http(""https://log-server.com/api/logs"") // 傳到中央伺服器
            .CreateLogger(); // 建立 Logger
    }
}
```
" }
    };
}
