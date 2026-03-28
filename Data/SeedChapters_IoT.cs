using DotNetLearning.Models;

namespace DotNetLearning.Data;

public static class SeedChapters_IoT
{
    public static List<Chapter> GetChapters() => new()
    {
        // ── IoT Chapter 600 ────────────────────────────
        new() { Id=600, Category="iot", Order=1, Level="beginner", Icon="🍓", Title="Raspberry Pi 入門與環境建置", Slug="raspberry-pi-setup", IsPublished=true, Content=@"
# Raspberry Pi 入門與環境建置

## 什麼是 Raspberry Pi？

> 💡 **比喻：一台信用卡大小的電腦**
> 想像你把一台桌上型電腦壓縮到跟信用卡一樣大——
> 它有 CPU、記憶體、USB、HDMI、WiFi，什麼都有，
> 但只要一千多塊台幣就能買到。
> Raspberry Pi 就是這樣的「迷你電腦」，
> 可以跑 Linux、裝 .NET、當伺服器，甚至做 POS 收銀機！

### Raspberry Pi 的核心概念

```
Raspberry Pi 就像一台「迷你伺服器」：

傳統伺服器                    Raspberry Pi
─────────────────────────────────────────────
大小：一個機櫃               大小：一張信用卡
價格：數萬元                 價格：約 1,000～3,000 元
電力：數百瓦                 電力：5～15 瓦
用途：企業級服務             用途：IoT、POS、學習、原型開發

雖然效能不如正式伺服器，但用來做 POS 系統綽綽有餘！
```

---

## 型號比較（Pi 4, Pi 5, Pi Zero）

```
型號              CPU           RAM        價格(USD)   適合用途
──────────────────────────────────────────────────────────────────
Pi Zero 2 W      四核 1GHz     512MB      $15         簡單感測器、LED 控制
Pi 4 Model B     四核 1.5GHz   2/4/8GB    $35~$75     POS 系統、Web 伺服器
Pi 5             四核 2.4GHz   4/8GB      $60~$80     高效能應用、多媒體

💡 做 POS 系統建議至少用 Pi 4（4GB RAM）
   Pi 5 效能更好，但 Pi 4 已經夠用了
```

### 用 C# 來理解型號差異

```csharp
// 定義 Raspberry Pi 型號的列舉 // 列出所有常用型號
public enum PiModel // Pi 型號列舉
{
    PiZero2W,   // Pi Zero 2 W：最小最便宜 // 適合簡單 IoT
    Pi4B,       // Pi 4 Model B：主流選擇 // 適合 POS 系統
    Pi5         // Pi 5：最新最強 // 適合高效能需求
}

// 定義 Raspberry Pi 規格的類別 // 描述硬體資訊
public class PiSpec // Pi 規格類別
{
    public PiModel Model { get; set; }  // 型號 // 哪一款 Pi
    public string Cpu { get; set; } = """"; // CPU 規格 // 處理器資訊
    public int RamMb { get; set; }      // 記憶體（MB）// RAM 大小
    public decimal PriceUsd { get; set; } // 價格（美元）// 售價
    public bool HasWifi { get; set; }   // 是否有 WiFi // 無線網路支援
    public bool HasBluetooth { get; set; } // 是否有藍牙 // 藍牙支援
}

// 取得各型號規格的方法 // 回傳規格清單
public static List<PiSpec> GetAllSpecs() // 取得所有 Pi 規格
{
    return new List<PiSpec> // 建立規格清單
    {
        new() // Pi Zero 2 W 的規格
        {
            Model = PiModel.PiZero2W, // 設定型號為 Zero 2 W
            Cpu = ""Quad-core 1GHz ARM Cortex-A53"", // 四核心 1GHz 處理器
            RamMb = 512, // 512MB 記憶體
            PriceUsd = 15m, // 售價 15 美元
            HasWifi = true, // 有 WiFi
            HasBluetooth = true // 有藍牙
        },
        new() // Pi 4 Model B 的規格
        {
            Model = PiModel.Pi4B, // 設定型號為 Pi 4B
            Cpu = ""Quad-core 1.5GHz ARM Cortex-A72"", // 四核心 1.5GHz 處理器
            RamMb = 4096, // 4GB 記憶體（建議版本）
            PriceUsd = 55m, // 售價 55 美元
            HasWifi = true, // 有 WiFi
            HasBluetooth = true // 有藍牙
        },
        new() // Pi 5 的規格
        {
            Model = PiModel.Pi5, // 設定型號為 Pi 5
            Cpu = ""Quad-core 2.4GHz ARM Cortex-A76"", // 四核心 2.4GHz 處理器
            RamMb = 8192, // 8GB 記憶體（建議版本）
            PriceUsd = 80m, // 售價 80 美元
            HasWifi = true, // 有 WiFi
            HasBluetooth = true // 有藍牙
        }
    }; // 回傳完整清單
}
```

---

## 安裝 Raspberry Pi OS

> 💡 **比喻：幫電腦裝 Windows，只不過這次裝的是 Linux**
> Raspberry Pi OS 就是 Pi 專用的作業系統，
> 基於 Debian Linux，操作方式跟 Ubuntu 很像。
> 用 Raspberry Pi Imager 燒錄到 SD 卡，插上去就能開機。

### 安裝步驟

```bash
# 步驟 1：下載 Raspberry Pi Imager // 到官網下載燒錄工具
# https://www.raspberrypi.com/software/ // 官方下載頁面

# 步驟 2：選擇作業系統 // 選 Raspberry Pi OS (64-bit)
# Raspberry Pi OS (64-bit) with Desktop // 有桌面環境的版本

# 步驟 3：選擇目標 SD 卡 // 至少 16GB 的 microSD 卡
# 建議使用 32GB 以上的高速 SD 卡 // Class 10 或 U1 以上

# 步驟 4：進階設定（齒輪圖示）// 設定主機名稱和帳號
# 設定主機名稱 // 例如：pos-terminal-01
# 設定使用者帳號和密碼 // 建議不要用預設的 pi / raspberry
# 啟用 SSH // 這樣才能遠端連線
# 設定 WiFi // 輸入你的 WiFi SSID 和密碼

# 步驟 5：燒錄 // 點擊「Write」開始燒錄
# 等待完成後插入 Pi 的 SD 卡槽 // 接上電源就能開機
```

### 用 C# 模擬安裝流程

```csharp
// 定義 Pi 安裝設定的類別 // 描述安裝時需要的參數
public class PiSetupConfig // Pi 安裝設定
{
    public string Hostname { get; set; } = ""pos-terminal-01""; // 主機名稱 // 設備識別名稱
    public string Username { get; set; } = ""admin"";   // 使用者帳號 // 登入用的帳號
    public string Password { get; set; } = """";         // 密碼 // 登入用的密碼
    public bool EnableSsh { get; set; } = true;         // 啟用 SSH // 遠端連線必須開啟
    public string WifiSsid { get; set; } = """";         // WiFi 名稱 // 無線網路 SSID
    public string WifiPassword { get; set; } = """";     // WiFi 密碼 // 無線網路密碼
    public string OsVersion { get; set; } = ""Raspberry Pi OS 64-bit""; // 作業系統版本 // 選擇 64 位元版
    public int SdCardSizeGb { get; set; } = 32;         // SD 卡大小 // 至少 16GB
}

// 驗證安裝設定是否正確 // 檢查必填欄位
public static List<string> ValidateConfig(PiSetupConfig config) // 驗證設定方法
{
    var errors = new List<string>(); // 建立錯誤清單 // 收集所有錯誤訊息

    if (string.IsNullOrEmpty(config.Hostname)) // 檢查主機名稱是否為空
        errors.Add(""主機名稱不可為空""); // 新增錯誤訊息

    if (string.IsNullOrEmpty(config.Password)) // 檢查密碼是否為空
        errors.Add(""密碼不可為空""); // 安全性要求必須設定密碼

    if (config.Password == ""raspberry"") // 檢查是否使用預設密碼
        errors.Add(""請勿使用預設密碼 raspberry""); // 預設密碼不安全

    if (config.SdCardSizeGb < 16) // 檢查 SD 卡大小
        errors.Add(""SD 卡至少需要 16GB""); // 空間不足會導致安裝失敗

    if (!config.EnableSsh) // 檢查 SSH 是否啟用
        errors.Add(""建議啟用 SSH 以便遠端管理""); // SSH 是遠端管理的關鍵

    return errors; // 回傳錯誤清單 // 空清單表示設定正確
}
```

---

## SSH 遠端連線設定

> 💡 **比喻：遠端遙控你的 Pi**
> SSH 就像是一條「隱形的線」，讓你用自己的電腦
> 遠端操控 Raspberry Pi，不需要額外接螢幕和鍵盤。
> 就像用遙控器操作電視一樣方便。

### SSH 連線指令

```bash
# 用 SSH 連線到 Pi // 輸入你設定的帳號和 IP
ssh admin@192.168.1.100 // 連線到 Pi 的 IP 位址

# 第一次連線會詢問是否信任 // 輸入 yes
# Are you sure you want to continue connecting? // 確認連線
# 輸入 yes 後再輸入密碼 // 就可以登入了

# 查看系統資訊 // 確認 Pi 正常運作
uname -a // 顯示 Linux 核心版本
cat /proc/cpuinfo // 顯示 CPU 資訊
free -h // 顯示記憶體使用情況
df -h // 顯示硬碟（SD 卡）使用情況

# 設定免密碼登入（SSH Key）// 更安全更方便
ssh-keygen -t ed25519 // 產生 SSH 金鑰對
ssh-copy-id admin@192.168.1.100 // 把公鑰複製到 Pi 上
```

### 用 C# 模擬 SSH 連線管理

```csharp
// 定義 SSH 連線資訊的類別 // 儲存連線所需的參數
public class SshConnection // SSH 連線類別
{
    public string Host { get; set; } = """"; // Pi 的 IP 位址 // 例如 192.168.1.100
    public int Port { get; set; } = 22;    // SSH 預設埠號 // 通常不需要改
    public string Username { get; set; } = """"; // 登入帳號 // 你設定的使用者名稱
    public bool UseKeyAuth { get; set; } = false; // 是否用金鑰認證 // 比密碼更安全
    public string KeyPath { get; set; } = """";   // 金鑰檔案路徑 // .ssh/id_ed25519
}

// 定義裝置管理器的類別 // 管理多台 Pi 的連線
public class DeviceManager // 裝置管理器
{
    private readonly List<SshConnection> _devices = new(); // 裝置清單 // 儲存所有 Pi 連線

    // 新增裝置的方法 // 把新的 Pi 加入管理
    public void AddDevice(SshConnection device) // 新增裝置方法
    {
        if (string.IsNullOrEmpty(device.Host)) // 檢查 IP 是否為空
            throw new ArgumentException(""IP 位址不可為空""); // 丟出例外

        _devices.Add(device); // 加入裝置清單 // 成功新增
        Console.WriteLine($""已新增裝置：{device.Host}""); // 顯示成功訊息
    }

    // 列出所有裝置的方法 // 顯示管理中的所有 Pi
    public void ListDevices() // 列出裝置方法
    {
        Console.WriteLine(""=== 管理中的 Raspberry Pi 裝置 ===""); // 顯示標題
        foreach (var device in _devices) // 逐一列出裝置
        {
            var authType = device.UseKeyAuth ? ""金鑰"" : ""密碼""; // 判斷認證方式
            Console.WriteLine($""  {device.Host}:{device.Port} ({authType}認證)""); // 顯示裝置資訊
        }
    }
}
```

---

## 安裝 .NET Runtime on ARM

> 💡 **比喻：讓 Pi 學會說 C# 語言**
> Raspberry Pi 預設只懂 Linux 指令，
> 安裝 .NET Runtime 後，它就能執行你寫的 C# 程式了。
> 就像在外國人的電腦上裝翻譯軟體，讓他能讀你寫的文件。

### 安裝 .NET 8 Runtime

```bash
# 方法 1：使用微軟官方安裝腳本 // 最簡單的方式
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0 --runtime aspnetcore // 安裝 ASP.NET Core Runtime 8.0

# 設定環境變數 // 讓系統找到 dotnet 指令
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc // 設定 .NET 安裝路徑
echo 'export PATH=$PATH:$DOTNET_ROOT' >> ~/.bashrc // 把 dotnet 加入 PATH
source ~/.bashrc // 重新載入設定 // 立即生效

# 驗證安裝 // 確認 .NET 已正確安裝
dotnet --info // 顯示 .NET 版本和環境資訊

# 方法 2：使用 apt 套件管理器 // 適合 Debian/Ubuntu 系列
wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb // 下載微軟套件庫設定
sudo dpkg -i packages-microsoft-prod.deb // 安裝套件庫設定
sudo apt-get update // 更新套件清單
sudo apt-get install -y aspnetcore-runtime-8.0 // 安裝 ASP.NET Core Runtime
```

### 用 C# 描述安裝流程

```csharp
// 定義 .NET 安裝設定的類別 // 描述安裝參數
public class DotNetInstallConfig // .NET 安裝設定
{
    public string Channel { get; set; } = ""8.0"";     // .NET 版本通道 // 目前建議用 8.0 LTS
    public string RuntimeType { get; set; } = ""aspnetcore""; // Runtime 類型 // Web 應用需要 aspnetcore
    public string Architecture { get; set; } = ""arm64"";     // CPU 架構 // Pi 4/5 用 arm64
    public string InstallDir { get; set; } = ""~/.dotnet"";   // 安裝目錄 // 預設安裝在家目錄
}

// 檢查 .NET 安裝狀態的方法 // 驗證環境是否正確
public static void CheckDotNetStatus() // 檢查安裝狀態
{
    var requiredEnvVars = new Dictionary<string, string> // 必要的環境變數 // 需要檢查的設定
    {
        [""DOTNET_ROOT""] = ""~/.dotnet"",  // .NET 安裝路徑 // 必須正確設定
        [""PATH""] = ""包含 ~/.dotnet""      // 系統路徑 // 必須包含 .NET 路徑
    }; // 環境變數字典結束

    foreach (var envVar in requiredEnvVars) // 逐一檢查環境變數
    {
        Console.WriteLine($""檢查 {envVar.Key}: {envVar.Value}""); // 顯示檢查項目
    }
}
```

---

## 安裝 Node.js + Chromium（for Kiosk Mode）

> 💡 **比喻：裝好「舞台」和「演員」**
> Node.js 是後台的工作人員（Print Agent 會用到），
> Chromium 是前台的大螢幕（用來顯示 POS 收銀畫面）。
> 兩個都裝好，你的 Pi 就變成一台完整的 POS 收銀機。

### 安裝指令

```bash
# 安裝 Node.js 20 LTS // 使用 NodeSource 套件庫
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash - // 加入 NodeSource 套件庫
sudo apt-get install -y nodejs // 安裝 Node.js 20 LTS

# 驗證 Node.js 安裝 // 確認版本號
node --version // 應該顯示 v20.x.x
npm --version  // 應該顯示 10.x.x

# 安裝 Chromium 瀏覽器 // Pi OS 通常已預裝
sudo apt-get install -y chromium-browser // 安裝 Chromium 瀏覽器

# 安裝必要的字型 // 中文字型支援
sudo apt-get install -y fonts-noto-cjk // 安裝 Noto CJK 中文字型

# 測試 Kiosk Mode // 全螢幕開啟 Chromium
chromium-browser --kiosk --noerrdialogs --disable-infobars http://localhost:5000 // 全螢幕開啟指定網址
```

### 用 C# 描述 Kiosk 設定

```csharp
// 定義 Kiosk 模式設定的類別 // 全螢幕顯示設定
public class KioskConfig // Kiosk 設定類別
{
    public string Browser { get; set; } = ""chromium-browser""; // 瀏覽器路徑 // 使用 Chromium
    public string TargetUrl { get; set; } = ""http://localhost:5000""; // 目標網址 // POS 系統的首頁
    public bool FullScreen { get; set; } = true;   // 全螢幕模式 // Kiosk 必須全螢幕
    public bool HideCursor { get; set; } = false;  // 隱藏游標 // 觸控螢幕可以隱藏
    public bool DisableErrorDialogs { get; set; } = true; // 停用錯誤對話框 // 避免干擾操作
}

// 產生 Kiosk 啟動指令的方法 // 組合 Chromium 參數
public static string BuildKioskCommand(KioskConfig config) // 建立 Kiosk 指令
{
    var args = new List<string> // 建立參數清單 // 收集所有啟動參數
    {
        config.Browser // 瀏覽器執行檔 // chromium-browser
    }; // 初始化參數清單

    if (config.FullScreen) // 如果啟用全螢幕
        args.Add(""--kiosk""); // 加入 kiosk 參數 // 全螢幕模式

    if (config.DisableErrorDialogs) // 如果停用錯誤對話框
    {
        args.Add(""--noerrdialogs""); // 不顯示錯誤對話框 // 避免彈出視窗
        args.Add(""--disable-infobars""); // 停用資訊列 // 隱藏上方提示
    }

    args.Add(config.TargetUrl); // 加入目標網址 // 最後一個參數是網址

    return string.Join("" "", args); // 組合成完整指令 // 用空格串接
}
```

---

## GPIO 基礎概念（控制 LED、按鈕）

> 💡 **比喻：Pi 的「觸手」**
> GPIO（General Purpose Input/Output）就像 Pi 的觸手，
> 可以伸出去控制 LED 燈、讀取按鈕狀態、驅動馬達。
> 在 POS 系統中，GPIO 可以用來控制錢箱（Cash Drawer）的開關。

### GPIO 腳位說明

```
Raspberry Pi GPIO 腳位圖（簡化版）：

        3.3V  [1]  [2]  5V        // 電源腳位
   GPIO  2  [3]  [4]  5V        // I2C SDA / 電源
   GPIO  3  [5]  [6]  GND       // I2C SCL / 接地
   GPIO  4  [7]  [8]  GPIO 14   // 通用 / UART TX
         GND  [9]  [10] GPIO 15  // 接地 / UART RX
   GPIO 17 [11] [12] GPIO 18   // 通用 / PWM
   GPIO 27 [13] [14] GND       // 通用 / 接地
   GPIO 22 [15] [16] GPIO 23   // 通用 / 通用
        3.3V [17] [18] GPIO 24  // 電源 / 通用
   GPIO 10 [19] [20] GND       // SPI MOSI / 接地

💡 做 POS 系統常用的 GPIO 應用：
   - GPIO 控制繼電器 → 打開錢箱
   - GPIO 讀取按鈕 → 緊急關閉鍵
   - GPIO 控制 LED → 狀態指示燈
```

### 用 C# 控制 GPIO

```csharp
// 引用 GPIO 套件 // 需要安裝 System.Device.Gpio NuGet 套件
// dotnet add package System.Device.Gpio // 安裝 GPIO 控制套件

// 定義 GPIO 控制器的類別 // 封裝 GPIO 操作
public class PiGpioController // GPIO 控制器類別
{
    private const int LedPin = 17;    // LED 連接的 GPIO 腳位 // 使用 GPIO 17
    private const int ButtonPin = 27; // 按鈕連接的 GPIO 腳位 // 使用 GPIO 27
    private const int DrawerPin = 22; // 錢箱連接的 GPIO 腳位 // 使用 GPIO 22

    // 初始化 GPIO 腳位的方法 // 設定腳位模式
    public static void InitializeGpio() // 初始化方法
    {
        Console.WriteLine(""初始化 GPIO 腳位...""); // 顯示初始化訊息
        Console.WriteLine($""  LED 腳位：GPIO {LedPin}（輸出）""); // 設定 LED 為輸出
        Console.WriteLine($""  按鈕腳位：GPIO {ButtonPin}（輸入）""); // 設定按鈕為輸入
        Console.WriteLine($""  錢箱腳位：GPIO {DrawerPin}（輸出）""); // 設定錢箱為輸出
    }

    // 控制 LED 的方法 // 開啟或關閉 LED
    public static void SetLed(bool on) // 設定 LED 狀態
    {
        var state = on ? ""HIGH（亮）"" : ""LOW（暗）""; // 判斷 LED 狀態 // HIGH=亮, LOW=暗
        Console.WriteLine($""設定 GPIO {LedPin} = {state}""); // 顯示設定結果
    }

    // 開啟錢箱的方法 // 送出脈衝信號打開錢箱
    public static void OpenCashDrawer() // 開啟錢箱方法
    {
        Console.WriteLine($""送出脈衝到 GPIO {DrawerPin}...""); // 顯示脈衝訊息
        Console.WriteLine(""錢箱已開啟！""); // 顯示開啟成功
        // 實際上是送出一個短暫的 HIGH 信號 // 大約 200ms 的脈衝
    }
}
```

---

## 比喻總結：Raspberry Pi 是你的迷你伺服器

```
你的 POS 系統架構：

  ┌─────────────────────────────────────────────┐
  │            Raspberry Pi（迷你伺服器）          │
  │                                             │
  │   ┌──────────┐  ┌──────────┐  ┌──────────┐ │
  │   │ .NET     │  │ Node.js  │  │ Chromium │ │
  │   │ Runtime  │  │ Print    │  │ Kiosk    │ │
  │   │ (後端)   │  │ Agent    │  │ (前端)   │ │
  │   └──────────┘  └──────────┘  └──────────┘ │
  │         │              │             │      │
  │   ┌─────┴──────────────┴─────────────┴──┐  │
  │   │           Linux (Raspberry Pi OS)    │  │
  │   └─────────────────────────────────────┘  │
  │         │              │             │      │
  │       GPIO          USB           HDMI     │
  │         │              │             │      │
  └─────────┼──────────────┼─────────────┼──────┘
            │              │             │
       ┌────┴────┐   ┌────┴────┐   ┌───┴────┐
       │ 錢箱   │   │ 印表機 │   │ 觸控   │
       │ LED    │   │ 掃描器 │   │ 螢幕   │
       └────────┘   └────────┘   └────────┘
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：在 Pi 上安裝 x64 版本的 .NET

```csharp
// ❌ 錯誤：下載 x64 版本的 .NET // Pi 的 CPU 是 ARM 架構
// 很多人直接從官網下載 x64 的 .NET SDK // 但 Pi 用的是 ARM 處理器
var config = new DotNetInstallConfig // 錯誤的安裝設定
{
    Architecture = ""x64"", // ❌ 錯！Pi 不是 x86/x64 架構 // 這是桌機用的
    Channel = ""8.0"" // .NET 版本沒問題 // 但架構錯了
}; // 這樣安裝會無法執行

// ✅ 正確：使用 ARM64 架構 // Pi 4/5 使用 64 位元 ARM 處理器
var correctConfig = new DotNetInstallConfig // 正確的安裝設定
{
    Architecture = ""arm64"", // ✅ 正確！Pi 4/5 用 arm64 // ARM 64 位元架構
    Channel = ""8.0"" // .NET 8.0 LTS // 長期支援版本
}; // 這樣才能正確安裝
```

### ❌ 錯誤 2：直接用 root 帳號操作所有事情

```bash
# ❌ 錯誤：全程使用 root // 這是非常不安全的做法
sudo su  // 切換成 root 帳號
# 然後用 root 做所有事情 // 任何錯誤都可能毀掉整個系統

# ✅ 正確：只在需要時使用 sudo // 最小權限原則
sudo apt-get install nodejs  // 只在安裝軟體時用 sudo
dotnet run  // 執行程式不需要 sudo // 用一般使用者即可
```

### ❌ 錯誤 3：忘記設定環境變數就關掉終端機

```bash
# ❌ 錯誤：只在終端機裡設定環境變數 // 關掉終端機就消失了
export DOTNET_ROOT=$HOME/.dotnet  // 這只是暫時的 // 關掉就沒了

# ✅ 正確：寫入 .bashrc 永久保存 // 每次開機都會自動載入
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc  // 寫入設定檔 // 永久生效
echo 'export PATH=$PATH:$DOTNET_ROOT' >> ~/.bashrc    // PATH 也要設定 // 才能找到 dotnet 指令
source ~/.bashrc  // 立即套用設定 // 不需要重新開機
```
" },

        // ── IoT Chapter 601 ────────────────────────────
        new() { Id=601, Category="iot", Order=2, Level="intermediate", Icon="🖥️", Title="Web-Based POS 系統開發", Slug="web-pos-system", IsPublished=true, Content=@"
# Web-Based POS 系統開發

## POS 系統架構設計

> 💡 **比喻：數位化的收銀台**
> 傳統的收銀台有：收銀機、商品標籤、計算機、收據紙。
> Web-Based POS 就是把這些全部搬到瀏覽器裡：
> - 收銀機 → Web 畫面上的購物車
> - 商品標籤 → 資料庫裡的商品資料
> - 計算機 → JavaScript 自動計算
> - 收據紙 → 透過 Print Agent 列印

### 系統架構圖

```
Web-Based POS 系統架構：

  使用者（店員）
      │
      ▼
  ┌──────────────┐
  │  Chromium     │   ← 前端（HTML/CSS/JS）
  │  Kiosk Mode   │   ← 觸控螢幕操作
  └──────┬───────┘
         │ HTTP / WebSocket
         ▼
  ┌──────────────┐
  │  ASP.NET     │   ← 後端 API
  │  Core API    │   ← 商品/訂單/庫存管理
  └──────┬───────┘
         │
    ┌────┴────┐
    ▼         ▼
 ┌──────┐ ┌──────────┐
 │ SQLite│ │ Print    │
 │ 資料庫│ │ Agent    │
 └──────┘ └──────────┘
```

---

## 前端：HTML/CSS/JS 收銀畫面

> 💡 **比喻：店員面前的收銀螢幕**
> 收銀畫面就是店員每天面對的「工作台」。
> 左邊是商品按鈕（快速點選），右邊是購物車（已選商品）。
> 設計時要注意：按鈕要夠大（觸控螢幕用手指按）、字要清楚。

### POS 前端 HTML 結構

```html
<!-- POS 收銀畫面的 HTML 結構 -->
<!-- 主要分成左右兩個區域 -->
<!DOCTYPE html>
<html lang=""zh-TW""> <!-- 設定語言為繁體中文 -->
<head>
    <meta charset=""UTF-8""> <!-- 字元編碼 UTF-8 -->
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""> <!-- 響應式設計 -->
    <title>POS 收銀系統</title> <!-- 頁面標題 -->
    <style>
        /* POS 系統的基本樣式 */
        * { margin: 0; padding: 0; box-sizing: border-box; } /* 重設邊距 */
        body { font-family: 'Noto Sans TC', sans-serif; } /* 使用中文字型 */

        .pos-container { /* POS 主容器 */
            display: grid; /* 使用 Grid 排版 */
            grid-template-columns: 1fr 400px; /* 左寬右窄 */
            height: 100vh; /* 滿版高度 */
        }
        .product-grid { /* 商品區域 */
            display: grid; /* 商品用 Grid 排列 */
            grid-template-columns: repeat(4, 1fr); /* 每排 4 個 */
            gap: 10px; /* 間距 10px */
            padding: 20px; /* 內距 20px */
            overflow-y: auto; /* 超出時可捲動 */
        }
        .product-btn { /* 商品按鈕 */
            height: 100px; /* 按鈕高度 100px */
            font-size: 18px; /* 字體大小 18px */
            border: 2px solid #ddd; /* 邊框樣式 */
            border-radius: 8px; /* 圓角 */
            cursor: pointer; /* 滑鼠游標 */
            display: flex; /* Flex 排版 */
            flex-direction: column; /* 垂直排列 */
            align-items: center; /* 水平置中 */
            justify-content: center; /* 垂直置中 */
        }
        .cart-panel { /* 購物車面板 */
            background: #f5f5f5; /* 淺灰背景 */
            padding: 20px; /* 內距 */
            display: flex; /* Flex 排版 */
            flex-direction: column; /* 垂直排列 */
        }
    </style>
</head>
<body>
    <div class=""pos-container""> <!-- POS 主容器開始 -->
        <div class=""product-grid"" id=""productGrid""> <!-- 商品區域 -->
            <!-- 商品按鈕會由 JavaScript 動態產生 -->
        </div>
        <div class=""cart-panel""> <!-- 購物車面板 -->
            <h2>購物車</h2> <!-- 購物車標題 -->
            <div id=""cartItems"" style=""flex:1;overflow-y:auto;""> <!-- 購物車商品清單 -->
            </div>
            <div id=""cartTotal"" style=""font-size:24px;font-weight:bold;padding:10px 0;""> <!-- 總計金額 -->
                總計：$0
            </div>
            <button id=""checkoutBtn"" style=""height:60px;font-size:20px;background:#4CAF50;color:white;border:none;border-radius:8px;""> <!-- 結帳按鈕 -->
                結帳
            </button>
        </div>
    </div>
</body>
</html>
```

---

## 商品管理（CRUD + 條碼掃描）

### 商品資料模型

```csharp
// 定義商品資料的類別 // POS 系統最核心的資料
public class Product // 商品類別
{
    public int Id { get; set; }           // 商品編號 // 自動遞增的主鍵
    public string Name { get; set; } = """"; // 商品名稱 // 例如「美式咖啡」
    public string Barcode { get; set; } = """"; // 條碼 // EAN-13 或自訂條碼
    public decimal Price { get; set; }    // 售價 // 含稅價格
    public int Stock { get; set; }        // 庫存數量 // 即時庫存
    public string Category { get; set; } = """"; // 商品分類 // 例如「飲料」、「餐點」
    public bool IsActive { get; set; } = true; // 是否上架 // 可暫時下架
    public string ImageUrl { get; set; } = """"; // 商品圖片網址 // 顯示在 POS 按鈕上
    public DateTime CreatedAt { get; set; } = DateTime.Now; // 建立時間 // 自動記錄
}

// 商品管理服務的類別 // 處理商品的新增、查詢、修改、刪除
public class ProductService // 商品服務類別
{
    private readonly List<Product> _products = new(); // 商品清單 // 模擬資料庫

    // 新增商品的方法 // Create 操作
    public Product AddProduct(string name, string barcode, decimal price, int stock) // 新增商品
    {
        var product = new Product // 建立商品物件
        {
            Id = _products.Count + 1, // 自動設定編號 // 簡單的遞增 ID
            Name = name,      // 設定名稱 // 商品顯示名稱
            Barcode = barcode, // 設定條碼 // 用於掃描器讀取
            Price = price,    // 設定價格 // 銷售單價
            Stock = stock     // 設定庫存 // 初始庫存數量
        }; // 商品物件建立完成

        _products.Add(product); // 加入清單 // 儲存到資料庫
        return product; // 回傳新建的商品 // 含自動產生的 ID
    }

    // 用條碼查詢商品的方法 // 掃描器掃到條碼後用這個查
    public Product? FindByBarcode(string barcode) // 條碼查詢方法
    {
        return _products.FirstOrDefault(p => p.Barcode == barcode); // 找到第一個符合的商品 // 找不到回傳 null
    }

    // 更新庫存的方法 // 賣出商品後扣庫存
    public bool UpdateStock(int productId, int quantitySold) // 更新庫存方法
    {
        var product = _products.FirstOrDefault(p => p.Id == productId); // 找到商品 // 用 ID 查詢
        if (product == null) return false; // 商品不存在 // 回傳失敗

        if (product.Stock < quantitySold) return false; // 庫存不足 // 回傳失敗

        product.Stock -= quantitySold; // 扣除庫存 // 減去賣出數量
        return true; // 回傳成功 // 庫存更新完成
    }
}
```

---

## 購物車邏輯（加入/移除/數量/小計/總計）

> 💡 **比喻：超市推車**
> 購物車就像實體的超市推車：
> 可以放入商品、拿出商品、改數量。
> 最後結帳時算總金額。

### 購物車 JavaScript 實作

```javascript
// POS 購物車類別 // 管理所有購物車操作
class PosCart { // 購物車類別定義
    constructor() { // 建構函式 // 初始化購物車
        this.items = []; // 購物車商品陣列 // 空陣列開始
    }

    // 加入商品的方法 // 點擊商品按鈕時呼叫
    addItem(product) { // 加入商品方法
        const existing = this.items.find(i => i.productId === product.id); // 檢查商品是否已在購物車 // 用商品 ID 比對
        if (existing) { // 如果已存在
            existing.quantity += 1; // 數量加 1 // 同商品累加
            existing.subtotal = existing.quantity * existing.price; // 重新計算小計 // 數量 x 單價
        } else { // 如果是新商品
            this.items.push({ // 加入新商品到陣列
                productId: product.id, // 商品 ID // 用於識別
                name: product.name,    // 商品名稱 // 顯示用
                price: product.price,  // 單價 // 用於計算
                quantity: 1,           // 初始數量為 1 // 第一次加入
                subtotal: product.price // 小計等於單價 // 數量為 1 所以等於單價
            }); // 新商品加入完成
        }
        this.render(); // 重新渲染畫面 // 更新顯示
    }

    // 移除商品的方法 // 從購物車移除指定商品
    removeItem(productId) { // 移除商品方法
        this.items = this.items.filter(i => i.productId !== productId); // 過濾掉指定商品 // 保留其他商品
        this.render(); // 重新渲染畫面 // 更新顯示
    }

    // 修改數量的方法 // 加減按鈕用
    updateQuantity(productId, delta) { // 修改數量方法
        const item = this.items.find(i => i.productId === productId); // 找到指定商品 // 用 ID 查找
        if (!item) return; // 找不到就跳過 // 防禦性程式設計

        item.quantity += delta; // 加減數量 // delta 可以是 +1 或 -1
        if (item.quantity <= 0) { // 如果數量變成 0 或負數
            this.removeItem(productId); // 移除該商品 // 數量為 0 就不要了
            return; // 結束方法 // 已經移除了
        }
        item.subtotal = item.quantity * item.price; // 重新計算小計 // 數量 x 單價
        this.render(); // 重新渲染畫面 // 更新顯示
    }

    // 計算總金額的方法 // 所有商品小計加總
    getTotal() { // 取得總計方法
        return this.items.reduce((sum, item) => sum + item.subtotal, 0); // 加總所有小計 // reduce 累加
    }

    // 清空購物車的方法 // 結帳完成後呼叫
    clear() { // 清空方法
        this.items = []; // 清空陣列 // 重設為空
        this.render(); // 重新渲染畫面 // 更新顯示
    }

    // 渲染購物車畫面的方法 // 更新 HTML 顯示
    render() { // 渲染方法
        const cartEl = document.getElementById('cartItems'); // 取得購物車容器 // DOM 元素
        const totalEl = document.getElementById('cartTotal'); // 取得總計容器 // DOM 元素

        cartEl.innerHTML = this.items.map(item => // 產生每個商品的 HTML // 用 map 轉換
            `<div style=""display:flex;justify-content:space-between;padding:8px;border-bottom:1px solid #ddd;"">
                <span>${item.name}</span>
                <span>${item.quantity} x $${item.price} = $${item.subtotal}</span>
            </div>` // 商品名稱、數量、小計 // 格式化顯示
        ).join(''); // 組合所有 HTML // 串接字串

        totalEl.textContent = `總計：$${this.getTotal()}`; // 更新總計顯示 // 呼叫 getTotal()
    }
}

// 初始化購物車 // 頁面載入時建立
const cart = new PosCart(); // 建立購物車實例 // 全域變數
```

---

## 付款流程（現金/信用卡/行動支付）

```csharp
// 定義付款方式的列舉 // POS 支援的付款方式
public enum PaymentMethod // 付款方式列舉
{
    Cash,       // 現金 // 最傳統的付款方式
    CreditCard, // 信用卡 // 刷卡付款
    LinePay,    // LINE Pay // 行動支付
    JkoPay,     // 街口支付 // 行動支付
    EasyCard    // 悠遊卡 // 電子票證
}

// 定義訂單的類別 // 一筆完整的交易記錄
public class Order // 訂單類別
{
    public int Id { get; set; }          // 訂單編號 // 自動遞增
    public DateTime OrderTime { get; set; } = DateTime.Now; // 下單時間 // 自動記錄
    public List<OrderItem> Items { get; set; } = new(); // 訂單明細 // 商品清單
    public decimal TotalAmount { get; set; } // 總金額 // 所有商品加總
    public PaymentMethod Payment { get; set; } // 付款方式 // 現金/信用卡/行動支付
    public decimal ReceivedAmount { get; set; } // 收到金額 // 現金付款時使用
    public decimal ChangeAmount { get; set; }  // 找零金額 // 收到 - 總額
    public string Status { get; set; } = ""completed""; // 訂單狀態 // 完成/取消/退貨
}

// 定義訂單明細的類別 // 單一商品在訂單中的記錄
public class OrderItem // 訂單明細類別
{
    public int ProductId { get; set; }  // 商品編號 // 對應商品資料
    public string ProductName { get; set; } = """"; // 商品名稱 // 冗餘儲存避免關聯查詢
    public decimal UnitPrice { get; set; } // 單價 // 購買當下的價格
    public int Quantity { get; set; }   // 數量 // 購買數量
    public decimal Subtotal { get; set; } // 小計 // 單價 x 數量
}

// 結帳服務的類別 // 處理付款流程
public class CheckoutService // 結帳服務
{
    // 處理現金付款的方法 // 計算找零
    public Order ProcessCashPayment(List<OrderItem> items, decimal receivedAmount) // 現金結帳方法
    {
        var total = items.Sum(i => i.Subtotal); // 計算總金額 // 加總所有小計

        if (receivedAmount < total) // 檢查收到金額是否足夠
            throw new InvalidOperationException( // 金額不足丟出例外
                $""金額不足！應收 {total}，實收 {receivedAmount}""); // 顯示差額

        var order = new Order // 建立訂單物件
        {
            Items = items,           // 設定訂單明細 // 所有商品
            TotalAmount = total,     // 設定總金額 // 加總結果
            Payment = PaymentMethod.Cash, // 設定付款方式為現金
            ReceivedAmount = receivedAmount, // 設定收到金額
            ChangeAmount = receivedAmount - total // 計算找零 // 收到 - 總額
        }; // 訂單建立完成

        Console.WriteLine($""交易完成！總額：{total}，收到：{receivedAmount}，找零：{order.ChangeAmount}""); // 顯示交易結果
        return order; // 回傳訂單 // 包含完整交易資訊
    }
}
```

---

## 收據格式設計

```csharp
// 定義收據產生器的類別 // 產生文字格式的收據
public class ReceiptGenerator // 收據產生器
{
    private const int ReceiptWidth = 32; // 收據寬度（字元數）// 熱感應印表機通常 32 或 48 字元

    // 產生收據的方法 // 回傳格式化的收據字串
    public static string GenerateReceipt(Order order, string storeName) // 產生收據方法
    {
        var lines = new List<string>(); // 建立行清單 // 收據的每一行

        lines.Add(CenterText(storeName)); // 店名置中 // 收據最上方
        lines.Add(CenterText(""統一編號：12345678"")); // 統編置中 // 公司稅號
        lines.Add(new string('-', ReceiptWidth)); // 分隔線 // 用虛線分隔
        lines.Add($""日期：{order.OrderTime:yyyy/MM/dd HH:mm}""); // 交易日期時間 // 格式化日期
        lines.Add($""單號：{order.Id:D6}""); // 訂單編號 // 6 位數補零
        lines.Add(new string('-', ReceiptWidth)); // 分隔線

        foreach (var item in order.Items) // 逐一列出商品
        {
            lines.Add($""{item.ProductName}""); // 商品名稱 // 獨立一行
            lines.Add($""  {item.Quantity} x ${item.UnitPrice} = ${item.Subtotal}""); // 數量 x 單價 = 小計
        }

        lines.Add(new string('=', ReceiptWidth)); // 雙線分隔 // 用等號
        lines.Add($""總計：${order.TotalAmount}""); // 總金額 // 所有商品加總
        lines.Add($""付款方式：{order.Payment}""); // 付款方式 // 現金/信用卡等
        if (order.Payment == PaymentMethod.Cash) // 如果是現金付款
        {
            lines.Add($""收到：${order.ReceivedAmount}""); // 收到金額
            lines.Add($""找零：${order.ChangeAmount}""); // 找零金額
        }
        lines.Add(new string('-', ReceiptWidth)); // 分隔線
        lines.Add(CenterText(""謝謝光臨！"")); // 感謝語置中

        return string.Join(""\n"", lines); // 組合所有行 // 用換行符號連接
    }

    // 文字置中的輔助方法 // 在收據寬度內置中顯示
    private static string CenterText(string text) // 置中方法
    {
        var padding = (ReceiptWidth - text.Length) / 2; // 計算左邊空白數 // 讓文字置中
        return text.PadLeft(padding + text.Length); // 左邊補空白 // 實現置中效果
    }
}
```

---

## Raspberry Pi Kiosk Mode（全螢幕 Web 模式）

```bash
# 設定 Pi 開機自動進入 Kiosk Mode // 自動啟動 POS 畫面
# 編輯自動啟動設定檔 // 開機後自動執行

# 步驟 1：建立自動啟動腳本 // 放在 home 目錄
cat << 'SCRIPT' > ~/start-pos.sh  # 建立啟動腳本
#!/bin/bash  # 使用 bash 執行
# 等待桌面環境準備好 // 避免太快啟動
sleep 5  # 等待 5 秒

# 停用螢幕保護程式 // POS 不需要螢幕保護
xset s off  # 關閉螢幕保護
xset -dpms  # 關閉電源管理
xset s noblank  # 不要黑屏

# 啟動 Chromium Kiosk Mode // 全螢幕開啟 POS 系統
chromium-browser \
  --kiosk \  # 全螢幕模式
  --noerrdialogs \  # 不顯示錯誤對話框
  --disable-infobars \  # 隱藏資訊列
  --disable-translate \  # 停用翻譯功能
  --no-first-run \  # 跳過首次執行提示
  --start-fullscreen \  # 全螢幕啟動
  http://localhost:5000  # POS 系統網址
SCRIPT
chmod +x ~/start-pos.sh  # 設定執行權限

# 步驟 2：設定開機自動執行 // 加入 autostart
mkdir -p ~/.config/autostart  # 建立 autostart 目錄
cat << 'DESKTOP' > ~/.config/autostart/pos-kiosk.desktop  # 建立 desktop 檔案
[Desktop Entry]  # Desktop 檔案格式
Type=Application  # 類型為應用程式
Name=POS Kiosk  # 名稱
Exec=/home/admin/start-pos.sh  # 執行腳本路徑
DESKTOP
```

---

## 觸控螢幕操作優化

```css
/* 觸控螢幕優化的 CSS 樣式 */
/* 讓按鈕更適合手指操作 */

/* 防止長按選取文字 */ /* 觸控螢幕常見問題 */
* {
    -webkit-user-select: none; /* Safari/Chrome 防選取 */
    user-select: none; /* 標準屬性 防選取 */
    -webkit-touch-callout: none; /* iOS 防長按選單 */
    -webkit-tap-highlight-color: transparent; /* 移除點擊高亮 */
}

/* 按鈕最小尺寸 */ /* 手指操作至少 44px */
.touch-btn {
    min-width: 44px; /* 最小寬度 44px */
    min-height: 44px; /* 最小高度 44px */
    padding: 12px 16px; /* 內距加大 */
    font-size: 16px; /* 字體不要太小 */
    touch-action: manipulation; /* 優化觸控行為 */
}

/* 快速點擊回饋 */ /* 讓店員知道按到了 */
.touch-btn:active {
    transform: scale(0.95); /* 按下時縮小 */
    opacity: 0.8; /* 按下時半透明 */
    transition: all 0.1s; /* 過渡動畫 0.1 秒 */
}

/* 數字鍵盤樣式 */ /* 輸入金額用 */
.numpad {
    display: grid; /* Grid 排版 */
    grid-template-columns: repeat(3, 1fr); /* 3 欄 */
    gap: 8px; /* 間距 8px */
    padding: 16px; /* 內距 16px */
}
.numpad button {
    height: 64px; /* 按鈕高度 64px */
    font-size: 24px; /* 字體大小 24px */
    border-radius: 8px; /* 圓角 8px */
}
```

---

## 離線模式（Service Worker）

> 💡 **比喻：停電時的備用發電機**
> 如果網路斷了怎麼辦？Service Worker 就像備用發電機，
> 讓 POS 系統在沒有網路時也能繼續運作。
> 等網路恢復後，再把離線期間的交易資料同步回伺服器。

```javascript
// Service Worker 註冊 // 在主頁面載入時執行
if ('serviceWorker' in navigator) { // 檢查瀏覽器是否支援 Service Worker
    navigator.serviceWorker.register('/sw.js') // 註冊 Service Worker 檔案
        .then(reg => console.log('SW 註冊成功', reg.scope)) // 註冊成功的回呼 // 顯示作用範圍
        .catch(err => console.log('SW 註冊失敗', err)); // 註冊失敗的回呼 // 顯示錯誤
}

// sw.js - Service Worker 檔案 // 處理離線快取
const CACHE_NAME = 'pos-cache-v1'; // 快取名稱 // 版本號用於更新
const URLS_TO_CACHE = [ // 需要快取的檔案清單
    '/',                // 首頁 // POS 主畫面
    '/css/pos.css',     // CSS 樣式 // 畫面樣式
    '/js/pos.js',       // JavaScript // 購物車邏輯
    '/js/cart.js',      // 購物車 JS // 購物車類別
    '/api/products',    // 商品資料 API // 商品清單
]; // 快取清單結束

// 安裝事件 // Service Worker 第一次安裝時執行
self.addEventListener('install', event => { // 監聽安裝事件
    event.waitUntil( // 等待快取完成
        caches.open(CACHE_NAME) // 開啟快取空間
            .then(cache => cache.addAll(URLS_TO_CACHE)) // 快取所有指定檔案
    ); // waitUntil 結束
}); // install 事件結束

// 攔截請求事件 // 決定從快取還是網路取得資料
self.addEventListener('fetch', event => { // 監聽網路請求
    event.respondWith( // 回應請求
        caches.match(event.request) // 先從快取找
            .then(response => { // 快取查詢結果
                if (response) return response; // 有快取就用快取 // 離線也能用
                return fetch(event.request); // 沒快取就從網路取 // 正常連線時
            }) // 查詢結束
    ); // respondWith 結束
}); // fetch 事件結束
```

### 離線交易暫存

```javascript
// 離線交易暫存管理 // 網路斷線時暫存交易
class OfflineStore { // 離線暫存類別
    constructor() { // 建構函式
        this.dbName = 'pos-offline-db'; // IndexedDB 名稱 // 本地資料庫
    }

    // 儲存離線交易 // 存到 IndexedDB
    async saveOfflineOrder(order) { // 儲存離線訂單方法
        const orders = JSON.parse(localStorage.getItem('offlineOrders') || '[]'); // 讀取現有離線訂單 // 從 localStorage
        orders.push({ ...order, offlineAt: new Date().toISOString() }); // 加入新訂單和時間戳
        localStorage.setItem('offlineOrders', JSON.stringify(orders)); // 儲存回 localStorage
        console.log(`離線訂單已暫存，共 ${orders.length} 筆待同步`); // 顯示暫存數量
    }

    // 同步離線交易 // 網路恢復時呼叫
    async syncOfflineOrders() { // 同步方法
        const orders = JSON.parse(localStorage.getItem('offlineOrders') || '[]'); // 讀取離線訂單
        if (orders.length === 0) return; // 沒有離線訂單就跳過

        console.log(`開始同步 ${orders.length} 筆離線訂單...`); // 顯示同步開始
        for (const order of orders) { // 逐筆同步
            try { // 嘗試同步
                await fetch('/api/orders', { // 送出 API 請求
                    method: 'POST', // POST 方法
                    headers: { 'Content-Type': 'application/json' }, // JSON 格式
                    body: JSON.stringify(order) // 訂單資料
                }); // 請求結束
                console.log(`訂單同步成功：${order.id}`); // 同步成功
            } catch (err) { // 同步失敗
                console.error(`訂單同步失敗：${order.id}`, err); // 顯示錯誤
                return; // 停止同步 // 等下次再試
            }
        }
        localStorage.removeItem('offlineOrders'); // 清除已同步的離線訂單
        console.log('所有離線訂單同步完成！'); // 顯示同步完成
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：購物車用浮點數計算金額

```javascript
// ❌ 錯誤：直接用浮點數計算 // 會產生精度問題
let total = 0; // 初始化總計
total += 19.9; // 加一杯咖啡 // 19.9 元
total += 35.5; // 加一份三明治 // 35.5 元
console.log(total); // 55.400000000000006 ← 浮點數精度問題！

// ✅ 正確：用整數計算（分為單位）// 最後再除以 100
let totalCents = 0; // 用「分」為單位 // 避免浮點數問題
totalCents += 1990; // 19.9 元 = 1990 分
totalCents += 3550; // 35.5 元 = 3550 分
console.log(totalCents / 100); // 55.4 ← 正確！

// 或使用 toFixed // 簡單但不夠精確
console.log(parseFloat((19.9 + 35.5).toFixed(2))); // 55.4
```

### ❌ 錯誤 2：Service Worker 沒有處理更新

```javascript
// ❌ 錯誤：永遠只用快取，不更新 // 商品價格改了也不知道
self.addEventListener('fetch', event => { // 監聽請求
    event.respondWith(caches.match(event.request)); // 只看快取 // 永遠不會更新
}); // 這樣修改商品價格後 POS 還是顯示舊價格

// ✅ 正確：Cache-first + 背景更新 // 先用快取再更新
self.addEventListener('fetch', event => { // 監聯請求
    event.respondWith( // 回應請求
        caches.match(event.request).then(cached => { // 先查快取
            const fetchPromise = fetch(event.request).then(response => { // 同時從網路取
                caches.open(CACHE_NAME).then(cache => { // 開啟快取空間
                    cache.put(event.request, response.clone()); // 更新快取 // 下次就用新的
                }); // 快取更新完成
                return response; // 回傳網路結果
            }); // 網路請求結束
            return cached || fetchPromise; // 有快取用快取，沒有就等網路 // 離線容錯
        }) // 快取查詢結束
    ); // respondWith 結束
}); // fetch 事件結束
```

### ❌ 錯誤 3：觸控按鈕太小，店員按不到

```css
/* ❌ 錯誤：按鈕太小 */ /* 手指按不準 */
.product-btn {
    width: 30px; /* 太小了！ */ /* 手指至少需要 44px */
    height: 30px; /* 太小了！ */ /* 觸控螢幕操作困難 */
    font-size: 10px; /* 字太小看不清 */ /* 店員要瞇眼睛 */
}

/* ✅ 正確：按鈕至少 44x44px */ /* Apple HIG 觸控最小尺寸建議 */
.product-btn {
    min-width: 80px; /* 足夠的寬度 */ /* 手指輕鬆點擊 */
    min-height: 60px; /* 足夠的高度 */ /* 不會按錯 */
    font-size: 16px; /* 清楚的字體 */ /* 店員一目了然 */
    padding: 12px; /* 充足的內距 */ /* 點擊區域更大 */
}
```
" },

        // ── IoT Chapter 602 ────────────────────────────
        new() { Id=602, Category="iot", Order=3, Level="intermediate", Icon="🖨️", Title="Print Agent 與硬體整合", Slug="print-agent-hardware", IsPublished=true, Content=@"
# Print Agent 與硬體整合

## 什麼是 Print Agent？

> 💡 **比喻：Web 和印表機之間的翻譯官**
> 瀏覽器（Web）不能直接跟印表機溝通，
> 就像你不能直接用中文跟一個只懂日文的人對話。
> Print Agent 就是「翻譯官」——
> Web 說「請幫我印這張收據」，Print Agent 翻譯成印表機懂的指令。

### 為什麼需要 Print Agent？

```
瀏覽器的限制：
─────────────────────────────────────────────
❌ 瀏覽器不能直接控制 USB 印表機
❌ 瀏覽器不能送 ESC/POS 指令
❌ 瀏覽器不能開錢箱
❌ 瀏覽器不能控制客顯螢幕

解決方案：Print Agent（本機代理程式）
─────────────────────────────────────────────
✅ Print Agent 是一個跑在本機的小程式
✅ 它監聽 WebSocket 連線
✅ Web 透過 WebSocket 傳送列印指令
✅ Print Agent 收到後轉給印表機

資料流程：
  Web (JS) → WebSocket → Print Agent → USB → 印表機
              ↑
         這就是 Print Agent 扮演「翻譯官」的地方
```

---

## 熱感應印表機原理（ESC/POS 指令）

> 💡 **比喻：印表機的「摩斯密碼」**
> ESC/POS 是 Epson 發明的印表機指令集，
> 就像摩斯密碼一樣，每個指令代表不同動作。
> 例如 `\x1b\x40` 就是「初始化」，
> `\x1d\x56\x00` 就是「切紙」。

### 常用 ESC/POS 指令

```
指令                     功能             十六進位
──────────────────────────────────────────────────
ESC @                    初始化印表機     1B 40
LF                       換行             0A
ESC a 1                  置中對齊         1B 61 01
ESC a 0                  靠左對齊         1B 61 00
ESC E 1                  粗體開           1B 45 01
ESC E 0                  粗體關           1B 45 00
GS V 0                   全切紙           1D 56 00
GS V 1                   半切紙           1D 56 01
ESC p 0 100 100          開錢箱           1B 70 00 64 64
GS ! 0x11                放大字體 2x2     1D 21 11
```

### 用 C# 定義 ESC/POS 指令

```csharp
// 定義 ESC/POS 指令的靜態類別 // 封裝常用印表機指令
public static class EscPos // ESC/POS 指令類別
{
    // 初始化印表機 // 重設所有設定
    public static readonly byte[] Init = new byte[] { 0x1B, 0x40 }; // ESC @ 指令 // 印表機重設

    // 換行 // 印一行空白
    public static readonly byte[] LineFeed = new byte[] { 0x0A }; // LF 指令 // 換行

    // 置中對齊 // 文字置中
    public static readonly byte[] AlignCenter = new byte[] { 0x1B, 0x61, 0x01 }; // ESC a 1 // 置中

    // 靠左對齊 // 文字靠左（預設）
    public static readonly byte[] AlignLeft = new byte[] { 0x1B, 0x61, 0x00 }; // ESC a 0 // 靠左

    // 靠右對齊 // 文字靠右
    public static readonly byte[] AlignRight = new byte[] { 0x1B, 0x61, 0x02 }; // ESC a 2 // 靠右

    // 粗體開 // 加粗文字
    public static readonly byte[] BoldOn = new byte[] { 0x1B, 0x45, 0x01 }; // ESC E 1 // 粗體

    // 粗體關 // 取消加粗
    public static readonly byte[] BoldOff = new byte[] { 0x1B, 0x45, 0x00 }; // ESC E 0 // 取消粗體

    // 放大字體（2倍寬2倍高）// 用於店名或總金額
    public static readonly byte[] DoubleSize = new byte[] { 0x1D, 0x21, 0x11 }; // GS ! 0x11 // 2x2 放大

    // 恢復正常字體 // 取消放大
    public static readonly byte[] NormalSize = new byte[] { 0x1D, 0x21, 0x00 }; // GS ! 0x00 // 正常大小

    // 切紙（全切）// 切斷收據紙
    public static readonly byte[] CutPaper = new byte[] { 0x1D, 0x56, 0x00 }; // GS V 0 // 全切

    // 開錢箱 // 送電子信號打開錢箱
    public static readonly byte[] OpenDrawer = new byte[] { 0x1B, 0x70, 0x00, 0x64, 0x64 }; // ESC p // 開錢箱

    // 組合指令的輔助方法 // 把多個 byte 陣列串在一起
    public static byte[] Combine(params byte[][] commands) // 組合指令方法
    {
        var result = new List<byte>(); // 建立 byte 清單 // 收集所有位元組
        foreach (var cmd in commands) // 逐一加入指令
        {
            result.AddRange(cmd); // 加入指令的位元組 // 串接
        }
        return result.ToArray(); // 轉換為陣列回傳 // 完整的指令序列
    }
}
```

---

## Node.js Print Agent 實作

> 💡 **比喻：一個隨時待命的翻譯官**
> Print Agent 就是一個 Node.js 程式，
> 它啟動後會打開一個 WebSocket 伺服器，
> 等著 Web 端送來列印指令，
> 收到後就翻譯成 ESC/POS 指令送給印表機。

### Node.js Print Agent 程式碼

```javascript
// Node.js Print Agent 主程式 // 監聽 WebSocket 並控制印表機
const WebSocket = require('ws'); // 引入 WebSocket 套件 // 用於與 Web 端通訊
const { SerialPort } = require('serialport'); // 引入串列埠套件 // USB 印表機用
const escpos = require('escpos'); // 引入 ESC/POS 套件 // 印表機指令

// Print Agent 設定 // 連接埠和印表機設定
const CONFIG = { // 設定物件
    wsPort: 9100,        // WebSocket 監聽埠 // Web 端連線用
    printerPath: '/dev/usb/lp0', // 印表機裝置路徑 // Linux USB 印表機
    encoding: 'Big5',    // 字元編碼 // 中文用 Big5 或 UTF-8
}; // 設定結束

// 建立 WebSocket 伺服器 // 等待 Web 端連線
const wss = new WebSocket.Server({ port: CONFIG.wsPort }); // 監聽指定埠號
console.log(`Print Agent 啟動，監聽 ws://localhost:${CONFIG.wsPort}`); // 顯示啟動訊息

// 處理 WebSocket 連線 // 每個連線代表一個 Web 端
wss.on('connection', (ws) => { // 監聽連線事件
    console.log('Web 端已連線'); // 顯示連線成功

    // 處理收到的訊息 // Web 端送來的列印指令
    ws.on('message', (message) => { // 監聽訊息事件
        try { // 嘗試處理訊息
            const data = JSON.parse(message); // 解析 JSON 訊息 // 取得列印資料
            console.log('收到列印指令:', data.type); // 顯示指令類型

            switch (data.type) { // 根據指令類型處理
                case 'print-receipt': // 列印收據指令
                    printReceipt(data.payload); // 呼叫列印收據方法
                    ws.send(JSON.stringify({ status: 'ok', message: '收據列印成功' })); // 回傳成功
                    break; // 結束 case
                case 'open-drawer': // 開錢箱指令
                    openCashDrawer(); // 呼叫開錢箱方法
                    ws.send(JSON.stringify({ status: 'ok', message: '錢箱已開啟' })); // 回傳成功
                    break; // 結束 case
                case 'test-print': // 測試列印指令
                    testPrint(); // 呼叫測試列印方法
                    ws.send(JSON.stringify({ status: 'ok', message: '測試列印成功' })); // 回傳成功
                    break; // 結束 case
                default: // 未知指令
                    ws.send(JSON.stringify({ status: 'error', message: '未知指令' })); // 回傳錯誤
            }
        } catch (err) { // 處理錯誤
            console.error('處理指令失敗:', err); // 顯示錯誤訊息
            ws.send(JSON.stringify({ status: 'error', message: err.message })); // 回傳錯誤
        }
    }); // message 事件結束

    ws.on('close', () => console.log('Web 端已斷線')); // 斷線事件 // 顯示斷線訊息
}); // connection 事件結束

// 列印收據的方法 // 核心列印功能
function printReceipt(receipt) { // 列印收據函式
    console.log('開始列印收據...'); // 顯示開始列印
    console.log('店名:', receipt.storeName); // 顯示店名
    console.log('商品數:', receipt.items.length); // 顯示商品數量
    console.log('總金額:', receipt.total); // 顯示總金額
    // 實際上這裡會用 escpos 套件送 ESC/POS 指令到印表機 // 這裡簡化為 console.log
}

// 開錢箱的方法 // 送 ESC/POS 開錢箱指令
function openCashDrawer() { // 開錢箱函式
    console.log('開啟錢箱...'); // 顯示開啟訊息
    // 送 ESC p 0 100 100 指令到印表機 // 透過印表機的 RJ-11 接口控制錢箱
}

// 測試列印的方法 // 確認印表機是否正常
function testPrint() { // 測試列印函式
    console.log('執行測試列印...'); // 顯示測試訊息
    // 印出一張測試頁 // 包含日期時間和虛線
}
```

---

## JS 透過 WebSocket 呼叫 Print Agent

```javascript
// Web 端的 Print Agent 連線管理 // 負責與 Print Agent 通訊
class PrintClient { // 列印客戶端類別
    constructor(url = 'ws://localhost:9100') { // 建構函式 // 預設連線到本機
        this.url = url; // WebSocket URL // Print Agent 的位址
        this.ws = null; // WebSocket 實例 // 初始為 null
        this.connected = false; // 連線狀態 // 初始未連線
        this.reconnectInterval = 3000; // 重連間隔（毫秒）// 斷線後 3 秒重連
    }

    // 建立連線的方法 // 連到 Print Agent
    connect() { // 連線方法
        this.ws = new WebSocket(this.url); // 建立 WebSocket 連線

        this.ws.onopen = () => { // 連線成功事件
            this.connected = true; // 設定狀態為已連線
            console.log('已連線到 Print Agent'); // 顯示連線成功
            document.getElementById('printerStatus').textContent = '🟢 印表機已連線'; // 更新狀態顯示
        }; // onopen 結束

        this.ws.onclose = () => { // 斷線事件
            this.connected = false; // 設定狀態為未連線
            console.log('與 Print Agent 斷線，準備重連...'); // 顯示斷線訊息
            document.getElementById('printerStatus').textContent = '🔴 印表機已斷線'; // 更新狀態顯示
            setTimeout(() => this.connect(), this.reconnectInterval); // 排程重連 // 3 秒後重試
        }; // onclose 結束

        this.ws.onmessage = (event) => { // 收到訊息事件
            const response = JSON.parse(event.data); // 解析回應 // JSON 格式
            console.log('Print Agent 回應:', response); // 顯示回應內容
        }; // onmessage 結束

        this.ws.onerror = (err) => { // 錯誤事件
            console.error('WebSocket 錯誤:', err); // 顯示錯誤
        }; // onerror 結束
    }

    // 列印收據的方法 // 送列印指令到 Print Agent
    printReceipt(order) { // 列印收據方法
        if (!this.connected) { // 檢查是否已連線
            console.error('未連線到 Print Agent'); // 未連線的錯誤
            return false; // 回傳失敗
        }

        const message = { // 建立訊息物件
            type: 'print-receipt', // 指令類型：列印收據
            payload: { // 列印資料
                storeName: '我的小店', // 店名 // 會印在收據最上方
                items: order.items.map(i => ({ // 商品清單 // 轉換格式
                    name: i.name,      // 商品名稱
                    qty: i.quantity,   // 數量
                    price: i.price,   // 單價
                    subtotal: i.subtotal // 小計
                })), // 商品清單結束
                total: order.total, // 總金額
                payment: order.payment, // 付款方式
                time: new Date().toLocaleString('zh-TW') // 交易時間 // 格式化為中文日期
            } // payload 結束
        }; // 訊息物件結束

        this.ws.send(JSON.stringify(message)); // 送出訊息 // 轉為 JSON 字串
        return true; // 回傳成功
    }

    // 開錢箱的方法 // 送開錢箱指令
    openDrawer() { // 開錢箱方法
        if (!this.connected) return false; // 檢查連線
        this.ws.send(JSON.stringify({ type: 'open-drawer' })); // 送開錢箱指令
        return true; // 回傳成功
    }
}

// 初始化列印客戶端 // 頁面載入時自動連線
const printer = new PrintClient(); // 建立列印客戶端實例
printer.connect(); // 開始連線到 Print Agent
```

---

## 收據模板設計（店名、商品、金額、條碼）

```csharp
// 定義進階收據模板的類別 // 支援更多格式化選項
public class AdvancedReceiptTemplate // 進階收據模板
{
    // 產生 ESC/POS 收據指令的方法 // 回傳 byte 陣列
    public static byte[] BuildReceiptBytes(Order order, string storeName) // 建立收據方法
    {
        var commands = new List<byte[]>(); // 建立指令清單 // 收集所有 ESC/POS 指令

        // === 收據頭部 === // 店名和基本資訊
        commands.Add(EscPos.Init);         // 初始化印表機 // 重設所有設定
        commands.Add(EscPos.AlignCenter);  // 置中對齊 // 店名要置中
        commands.Add(EscPos.DoubleSize);   // 放大字體 // 店名要大一點
        commands.Add(TextToBytes(storeName)); // 印出店名 // 轉換為 byte 陣列
        commands.Add(EscPos.LineFeed);     // 換行

        commands.Add(EscPos.NormalSize);   // 恢復正常字體 // 其他內容用正常大小
        commands.Add(TextToBytes(""統一編號：12345678"")); // 印出統編
        commands.Add(EscPos.LineFeed);     // 換行
        commands.Add(TextToBytes(""TEL: 02-1234-5678"")); // 印出電話
        commands.Add(EscPos.LineFeed);     // 換行
        commands.Add(TextToBytes(""================================"")); // 分隔線 // 32 字元
        commands.Add(EscPos.LineFeed);     // 換行

        // === 交易資訊 === // 日期、單號
        commands.Add(EscPos.AlignLeft);    // 靠左對齊 // 交易資訊靠左
        commands.Add(TextToBytes($""日期：{order.OrderTime:yyyy/MM/dd HH:mm}"")); // 交易日期
        commands.Add(EscPos.LineFeed);     // 換行
        commands.Add(TextToBytes($""單號：{order.Id:D8}"")); // 訂單編號 // 8 位補零
        commands.Add(EscPos.LineFeed);     // 換行
        commands.Add(TextToBytes(""--------------------------------"")); // 分隔線
        commands.Add(EscPos.LineFeed);     // 換行

        // === 商品明細 === // 逐筆列出
        foreach (var item in order.Items) // 逐一列出商品
        {
            commands.Add(TextToBytes(item.ProductName)); // 商品名稱 // 獨立一行
            commands.Add(EscPos.LineFeed); // 換行
            var detail = $""  {item.Quantity} x ${item.UnitPrice,-8} ${item.Subtotal,8:F0}""; // 格式化明細
            commands.Add(TextToBytes(detail)); // 數量 x 單價 = 小計
            commands.Add(EscPos.LineFeed); // 換行
        }

        // === 總計 === // 金額加總
        commands.Add(TextToBytes(""================================"")); // 雙線分隔
        commands.Add(EscPos.LineFeed);     // 換行
        commands.Add(EscPos.BoldOn);       // 粗體開 // 總計要醒目
        commands.Add(EscPos.DoubleSize);   // 放大字體 // 總計要大
        commands.Add(TextToBytes($""總計 ${order.TotalAmount:F0}"")); // 總金額
        commands.Add(EscPos.LineFeed);     // 換行
        commands.Add(EscPos.NormalSize);   // 恢復正常 // 其他內容正常大小
        commands.Add(EscPos.BoldOff);      // 粗體關

        // === 付款資訊 === // 付款方式和找零
        commands.Add(TextToBytes($""付款：{order.Payment}"")); // 付款方式
        commands.Add(EscPos.LineFeed);     // 換行

        // === 尾部 === // 感謝語
        commands.Add(TextToBytes(""--------------------------------"")); // 分隔線
        commands.Add(EscPos.LineFeed);     // 換行
        commands.Add(EscPos.AlignCenter);  // 置中對齊
        commands.Add(TextToBytes(""謝謝光臨，歡迎再來！"")); // 感謝語
        commands.Add(EscPos.LineFeed);     // 換行
        commands.Add(EscPos.LineFeed);     // 多換幾行 // 留白方便撕紙
        commands.Add(EscPos.LineFeed);     // 換行
        commands.Add(EscPos.CutPaper);     // 切紙 // 自動切斷收據

        return EscPos.Combine(commands.ToArray()); // 組合所有指令 // 回傳完整的 byte 陣列
    }

    // 文字轉 byte 陣列的輔助方法 // 使用 UTF-8 編碼
    private static byte[] TextToBytes(string text) // 文字轉 bytes
    {
        return System.Text.Encoding.UTF8.GetBytes(text); // UTF-8 編碼 // 支援中文字
    }
}
```

---

## 錢箱控制（Cash Drawer）

> 💡 **比喻：用電子信號開保險箱**
> 錢箱（Cash Drawer）通常用 RJ-11 線連接到印表機，
> 印表機收到「開錢箱」的 ESC/POS 指令後，
> 會透過 RJ-11 接口送出電子脈衝打開錢箱的電磁鎖。
> 就像用遙控器開車門一樣。

```csharp
// 定義錢箱控制器的類別 // 管理錢箱的開關
public class CashDrawerController // 錢箱控制器
{
    private bool _isOpen = false; // 錢箱狀態 // 初始為關閉
    private DateTime? _lastOpenTime; // 上次開啟時間 // 記錄操作時間

    // 開啟錢箱的方法 // 送 ESC/POS 指令
    public void Open() // 開啟錢箱方法
    {
        if (_isOpen) // 如果已經開著
        {
            Console.WriteLine(""錢箱已經是開啟狀態""); // 顯示提示訊息
            return; // 不需要重複開啟
        }

        // 送 ESC p 0 100 100 到印表機 // 透過印表機控制錢箱
        Console.WriteLine(""送出開錢箱指令：ESC p 0 100 100""); // 顯示指令
        _isOpen = true; // 更新狀態為開啟
        _lastOpenTime = DateTime.Now; // 記錄開啟時間
        Console.WriteLine($""錢箱已開啟 - {_lastOpenTime:HH:mm:ss}""); // 顯示開啟時間
    }

    // 錢箱關閉的回呼 // 通常由感測器觸發
    public void OnDrawerClosed() // 錢箱關閉回呼
    {
        _isOpen = false; // 更新狀態為關閉
        Console.WriteLine(""錢箱已關閉""); // 顯示關閉訊息
    }
}
```

---

## 客顯（Customer Display）

> 💡 **比喻：面對客人的小螢幕**
> 客顯（Customer Display）就是面對客人的那個小螢幕，
> 顯示商品名稱、價格、總金額，讓客人知道自己要付多少錢。
> 有些是 VFD（螢光顯示器），有些是小型 LCD。

```csharp
// 定義客顯控制器的類別 // 控制客戶面對的顯示器
public class CustomerDisplayController // 客顯控制器
{
    private const int MaxLineLength = 20; // 每行最多顯示字元數 // VFD 通常是 20 字元

    // 顯示歡迎訊息的方法 // 待機時顯示
    public void ShowWelcome() // 顯示歡迎訊息
    {
        var line1 = CenterText(""歡迎光臨"", MaxLineLength); // 第一行：歡迎光臨 // 置中顯示
        var line2 = CenterText(""Welcome"", MaxLineLength);  // 第二行：Welcome // 英文歡迎
        Console.WriteLine($""客顯：{line1}""); // 送到客顯第一行
        Console.WriteLine($""客顯：{line2}""); // 送到客顯第二行
    }

    // 顯示商品資訊的方法 // 掃描商品時顯示
    public void ShowItem(string name, decimal price) // 顯示商品方法
    {
        var line1 = name.Length > MaxLineLength // 檢查名稱是否超過顯示寬度
            ? name[..MaxLineLength]  // 太長就截斷 // 只顯示前 20 字
            : name; // 沒超過就完整顯示
        var line2 = $""NT$ {price:F0}"".PadLeft(MaxLineLength); // 價格靠右顯示 // 格式化為整數
        Console.WriteLine($""客顯：{line1}""); // 送到客顯第一行
        Console.WriteLine($""客顯：{line2}""); // 送到客顯第二行
    }

    // 顯示總金額的方法 // 結帳時顯示
    public void ShowTotal(decimal total) // 顯示總計方法
    {
        var line1 = CenterText(""總  計"", MaxLineLength); // 第一行：總計 // 置中
        var line2 = $""NT$ {total:F0}"".PadLeft(MaxLineLength); // 第二行：金額 // 靠右
        Console.WriteLine($""客顯：{line1}""); // 送到客顯第一行
        Console.WriteLine($""客顯：{line2}""); // 送到客顯第二行
    }

    // 文字置中的輔助方法 // 在指定寬度內置中
    private static string CenterText(string text, int width) // 置中方法
    {
        var padding = (width - text.Length) / 2; // 計算左邊空白 // 置中計算
        return text.PadLeft(padding + text.Length).PadRight(width); // 左右補空白 // 填滿寬度
    }
}
```

---

## 條碼掃描器整合（USB HID → Keyboard Input）

> 💡 **比喻：掃描器就是一個「超快速打字員」**
> USB 條碼掃描器的運作原理其實很簡單：
> 它掃到條碼後，會「假裝自己是鍵盤」，
> 把條碼數字一個一個「打」出來，最後按 Enter。
> 所以 Web 端只要監聽鍵盤輸入就能收到條碼了！

### 條碼掃描器的 JavaScript 監聽

```javascript
// 條碼掃描器監聽器 // 自動偵測快速鍵盤輸入（＝掃描器）
class BarcodeScanner { // 掃描器類別
    constructor(onScan) { // 建構函式 // onScan 是掃到條碼後的回呼
        this.buffer = ''; // 輸入緩衝區 // 暫存掃描到的字元
        this.lastKeyTime = 0; // 上次按鍵時間 // 用於判斷是否為掃描器
        this.threshold = 50; // 按鍵間隔閾值（毫秒）// 人打字不可能這麼快
        this.onScan = onScan; // 掃描完成的回呼函式
        this.minLength = 4; // 最短條碼長度 // 避免誤判
        this.init(); // 初始化監聽 // 開始監聽鍵盤事件
    }

    // 初始化鍵盤監聽 // 綁定 keydown 事件
    init() { // 初始化方法
        document.addEventListener('keydown', (e) => { // 監聽鍵盤按下事件
            const now = Date.now(); // 取得當前時間 // 毫秒級時間戳

            if (e.key === 'Enter') { // 如果按了 Enter // 掃描器讀完會送 Enter
                if (this.buffer.length >= this.minLength) { // 如果緩衝區有足夠長度
                    this.onScan(this.buffer); // 觸發回呼 // 傳入條碼字串
                }
                this.buffer = ''; // 清空緩衝區 // 準備下次掃描
                return; // 結束處理
            }

            if (now - this.lastKeyTime > this.threshold * 3) { // 如果距離上次按鍵太久
                this.buffer = ''; // 清空緩衝區 // 這不是掃描器的輸入
            }

            if (e.key.length === 1) { // 如果是可打印字元 // 排除 Shift、Ctrl 等
                this.buffer += e.key; // 加入緩衝區 // 累積條碼字串
            }

            this.lastKeyTime = now; // 更新上次按鍵時間
        }); // keydown 事件結束
    }
}

// 使用方式 // 建立掃描器並處理掃到的條碼
const scanner = new BarcodeScanner(async (barcode) => { // 建立掃描器實例
    console.log('掃描到條碼:', barcode); // 顯示掃到的條碼
    const response = await fetch(`/api/products/barcode/${barcode}`); // 用條碼查詢商品 // 呼叫後端 API
    if (response.ok) { // 如果查詢成功
        const product = await response.json(); // 解析商品資料 // JSON 格式
        cart.addItem(product); // 自動加入購物車 // 掃一下就加入
        console.log(`已加入：${product.name} $${product.price}`); // 顯示加入的商品
    } else { // 查詢失敗
        alert(`找不到條碼 ${barcode} 對應的商品`); // 顯示找不到的提示
    }
}); // 掃描器建立完成
```

---

## C# Print Agent 替代方案

> 💡 **比喻：換一個說 C# 的翻譯官**
> 如果你比較熟 C#，也可以用 C# 寫 Print Agent。
> 功能完全一樣，只是語言不同。
> 就像有些翻譯官說英文，有些說日文，
> 但都能把你的話翻譯給印表機聽。

```csharp
// C# Print Agent 使用 WebSocket // ASP.NET Core 最小 API
// 需要 NuGet：dotnet add package System.IO.Ports // 串列埠套件

// 定義 C# Print Agent 的類別 // 替代 Node.js 方案
public class CSharpPrintAgent // C# Print Agent 類別
{
    private const string PrinterPort = ""/dev/usb/lp0""; // 印表機裝置路徑 // Linux USB 印表機

    // 啟動 Print Agent 的方法 // 建立 WebSocket 伺服器
    public static void StartAgent() // 啟動方法
    {
        var builder = WebApplication.CreateBuilder(); // 建立 Web 應用程式建構器
        var app = builder.Build(); // 建構應用程式

        app.UseWebSockets(); // 啟用 WebSocket 支援 // 必須加這行

        // 設定 WebSocket 端點 // 監聽 /ws 路徑
        app.Map(""/ws"", async (HttpContext context) => // WebSocket 端點
        {
            if (!context.WebSockets.IsWebSocketRequest) // 檢查是否為 WebSocket 請求
            {
                context.Response.StatusCode = 400; // 不是 WebSocket 就回 400 // 錯誤的請求
                return; // 結束處理
            }

            var ws = await context.WebSockets.AcceptWebSocketAsync(); // 接受 WebSocket 連線
            Console.WriteLine(""Web 端已透過 WebSocket 連線""); // 顯示連線訊息

            var buffer = new byte[1024 * 4]; // 接收緩衝區 // 4KB
            while (ws.State == System.Net.WebSockets.WebSocketState.Open) // 持續接收訊息
            {
                var result = await ws.ReceiveAsync(buffer, CancellationToken.None); // 接收訊息
                if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close) // 如果是關閉訊息
                    break; // 結束迴圈

                var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count); // 解碼訊息
                Console.WriteLine($""收到指令：{message}""); // 顯示收到的指令

                // 處理列印指令 // 根據指令類型執行對應操作
                var response = ""{\""status\"":\""ok\""}""; // 回應訊息 // JSON 格式
                var responseBytes = System.Text.Encoding.UTF8.GetBytes(response); // 編碼回應
                await ws.SendAsync(responseBytes, // 送出回應
                    System.Net.WebSockets.WebSocketMessageType.Text, // 文字類型
                    true, CancellationToken.None); // 送出完成
            }
        }); // WebSocket 端點結束

        app.Run(""http://0.0.0.0:9100""); // 啟動伺服器 // 監聽所有介面的 9100 埠
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：Print Agent 沒有處理斷線重連

```javascript
// ❌ 錯誤：WebSocket 斷線後就不管了 // 印表機離線就再也印不了
const ws = new WebSocket('ws://localhost:9100'); // 建立連線 // 只建一次
ws.onclose = () => console.log('斷線了'); // 只印訊息 // 沒有重連邏輯
// 如果 Print Agent 重啟或網路中斷，就再也連不上了 // 店員只能重開整個系統

// ✅ 正確：斷線後自動重連 // 確保列印服務持續可用
function connectPrintAgent() { // 封裝連線函式 // 可重複呼叫
    const ws = new WebSocket('ws://localhost:9100'); // 建立連線
    ws.onopen = () => console.log('已連線到 Print Agent'); // 連線成功
    ws.onclose = () => { // 斷線事件
        console.log('Print Agent 斷線，3 秒後重連...'); // 顯示重連訊息
        setTimeout(connectPrintAgent, 3000); // 3 秒後重新連線 // 遞迴呼叫
    }; // 斷線事件結束
    return ws; // 回傳 WebSocket 實例
}
```

### ❌ 錯誤 2：ESC/POS 中文編碼錯誤

```csharp
// ❌ 錯誤：用 ASCII 編碼送中文 // 中文會變成亂碼
var bytes = System.Text.Encoding.ASCII.GetBytes(""美式咖啡""); // ASCII 不支援中文 // 會變成 ????
// 印出來的收據全是問號或亂碼 // 因為 ASCII 只有英文字母

// ✅ 正確：依照印表機支援的編碼 // 通常是 Big5 或 UTF-8
// 如果印表機支援 UTF-8 // 先送 UTF-8 模式指令
var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(""美式咖啡""); // UTF-8 編碼 // 支援中文

// 如果印表機只支援 Big5 // 要用 Big5 編碼
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); // 註冊 Big5 編碼支援
var big5Bytes = System.Text.Encoding.GetEncoding(""big5"").GetBytes(""美式咖啡""); // Big5 編碼 // 繁體中文
```

### ❌ 錯誤 3：條碼掃描器把輸入當成普通鍵盤

```javascript
// ❌ 錯誤：沒有區分掃描器和鍵盤輸入 // 店員打字也會被當成條碼
document.addEventListener('keydown', (e) => { // 監聽所有鍵盤輸入
    buffer += e.key; // 全部加入緩衝區 // 不管是掃描器還是鍵盤
    if (e.key === 'Enter') { // 按 Enter 就查詢
        searchProduct(buffer); // 當成條碼查詢 // 如果店員按 Enter 也會觸發
        buffer = ''; // 清空
    }
}); // 這樣店員在搜尋框打字再按 Enter 也會被當成掃描

// ✅ 正確：用時間間隔區分 // 掃描器的按鍵速度遠比人快
document.addEventListener('keydown', (e) => { // 監聽鍵盤輸入
    const now = Date.now(); // 取得當前時間
    if (now - lastKeyTime > 150) buffer = ''; // 間隔超過 150ms 就清空 // 人打字速度較慢
    if (e.key === 'Enter' && buffer.length >= 4) { // 按 Enter 且長度足夠
        searchProduct(buffer); // 才當成條碼處理
        buffer = ''; // 清空
    } else if (e.key.length === 1) { // 可打印字元
        buffer += e.key; // 加入緩衝區
    }
    lastKeyTime = now; // 更新時間
}); // 這樣可以正確區分掃描器和人工輸入
```
" },

        // ── IoT Chapter 603 ────────────────────────────
        new() { Id=603, Category="iot", Order=4, Level="advanced", Icon="🚀", Title="CI/CD 自動部署到 POS 設備", Slug="cicd-pos-deployment", IsPublished=true, Content=@"
# CI/CD 自動部署到 POS 設備

## GitHub Actions CI/CD Pipeline

> 💡 **比喻：自動化的工廠生產線**
> CI/CD 就像一條自動化工廠生產線：
> 你把原料（程式碼）放上去，
> 機器會自動檢查品質（Test）、組裝（Build）、包裝（Package）、
> 然後送到客戶手上（Deploy）。
> 全程不需要人工介入，又快又可靠。

### GitHub Actions 工作流程

```yaml
# .github/workflows/pos-deploy.yml // CI/CD 工作流程設定檔
name: POS System CI/CD # 工作流程名稱 // 在 GitHub 上顯示的名稱

on: # 觸發條件 // 什麼時候執行這個流程
  push: # 推送程式碼時觸發
    branches: [ main ] # 只在 main 分支觸發 // 避免每個分支都部署
  pull_request: # PR 時觸發 // 用於檢查程式碼品質
    branches: [ main ] # 只在 main 分支的 PR

env: # 環境變數 // 全域設定
  DOTNET_VERSION: '8.0.x' # .NET 版本 // 使用 8.0 LTS
  NODE_VERSION: '20' # Node.js 版本 // LTS 版本

jobs: # 工作定義 // 包含所有要執行的步驟
  # === 階段 1：建置和測試 === // Build + Test
  build-and-test: # 工作名稱
    runs-on: ubuntu-latest # 執行環境 // 使用最新 Ubuntu
    steps: # 步驟清單
      - uses: actions/checkout@v4 # 取出程式碼 // 從 Git 倉庫
      - name: Setup .NET # 安裝 .NET SDK
        uses: actions/setup-dotnet@v4 # 使用官方 Action
        with: # 設定參數
          dotnet-version: ${{ env.DOTNET_VERSION }} # 使用指定版本

      - name: Restore dependencies # 還原 NuGet 套件
        run: dotnet restore # 下載所有相依套件

      - name: Build # 建置專案
        run: dotnet build --no-restore --configuration Release # Release 模式建置

      - name: Test # 執行測試
        run: dotnet test --no-build --configuration Release --verbosity normal # 執行所有測試

      - name: Publish # 發佈應用程式
        run: dotnet publish -c Release -r linux-arm64 --self-contained -o ./publish # 發佈為 ARM64 Linux

      - name: Upload artifact # 上傳建置結果
        uses: actions/upload-artifact@v4 # 使用上傳 Action
        with: # 設定參數
          name: pos-app # 產出物名稱
          path: ./publish # 要上傳的路徑

  # === 階段 2：部署 === // Deploy to Pi
  deploy: # 部署工作
    needs: build-and-test # 依賴建置工作 // 建置成功才部署
    runs-on: ubuntu-latest # 執行環境
    if: github.ref == 'refs/heads/main' # 只在 main 分支部署 // PR 不部署
    steps: # 步驟清單
      - name: Download artifact # 下載建置結果
        uses: actions/download-artifact@v4 # 使用下載 Action
        with: # 設定參數
          name: pos-app # 產出物名稱
          path: ./publish # 下載到的路徑

      - name: Deploy to Raspberry Pi # 部署到 Pi
        uses: appleboy/scp-action@v0.1.7 # 使用 SCP Action // 透過 SSH 複製檔案
        with: # 設定參數
          host: ${{ secrets.PI_HOST }} # Pi 的 IP // 從 GitHub Secrets 取得
          username: ${{ secrets.PI_USER }} # SSH 帳號 // 從 Secrets 取得
          key: ${{ secrets.PI_SSH_KEY }} # SSH 私鑰 // 從 Secrets 取得
          source: './publish/*' # 來源檔案 // 建置結果
          target: '/opt/pos-app' # 目標路徑 // Pi 上的安裝目錄

      - name: Restart POS Service # 重啟 POS 服務
        uses: appleboy/ssh-action@v1.0.3 # 使用 SSH Action // 遠端執行指令
        with: # 設定參數
          host: ${{ secrets.PI_HOST }} # Pi 的 IP
          username: ${{ secrets.PI_USER }} # SSH 帳號
          key: ${{ secrets.PI_SSH_KEY }} # SSH 私鑰
          script: | # 要執行的腳本
            sudo systemctl restart pos-app # 重啟 POS 服務 // 使用 systemd
            sudo systemctl status pos-app # 檢查服務狀態 // 確認正常運作
```

### 用 C# 描述 CI/CD Pipeline

```csharp
// 定義 CI/CD Pipeline 階段的列舉 // 每個階段代表一個步驟
public enum PipelineStage // Pipeline 階段列舉
{
    Checkout,   // 取出程式碼 // 從 Git 倉庫下載
    Restore,    // 還原套件 // NuGet restore
    Build,      // 建置專案 // 編譯程式碼
    Test,       // 執行測試 // 跑單元測試
    Publish,    // 發佈應用 // 打包成可部署的檔案
    Deploy,     // 部署到設備 // SCP 到 Pi
    Restart     // 重啟服務 // 讓新版本生效
}

// 定義 Pipeline 執行結果的類別 // 記錄每個階段的結果
public class PipelineResult // Pipeline 結果類別
{
    public PipelineStage Stage { get; set; }  // 階段名稱 // 哪個步驟
    public bool Success { get; set; }         // 是否成功 // true/false
    public string Message { get; set; } = """"; // 訊息 // 成功或錯誤描述
    public TimeSpan Duration { get; set; }    // 執行時間 // 花了多久
}

// 定義 Pipeline 執行器的類別 // 模擬 CI/CD 流程
public class PipelineRunner // Pipeline 執行器
{
    private readonly List<PipelineResult> _results = new(); // 結果清單 // 記錄所有階段

    // 執行完整 Pipeline 的方法 // 依序執行每個階段
    public async Task<bool> RunPipeline() // 執行 Pipeline 方法
    {
        var stages = Enum.GetValues<PipelineStage>(); // 取得所有階段 // 列舉值

        foreach (var stage in stages) // 逐一執行每個階段
        {
            Console.WriteLine($""▶ 執行階段：{stage}...""); // 顯示目前階段
            var startTime = DateTime.Now; // 記錄開始時間

            var success = await ExecuteStage(stage); // 執行該階段 // 回傳是否成功

            var result = new PipelineResult // 建立結果物件
            {
                Stage = stage, // 設定階段
                Success = success, // 設定結果
                Duration = DateTime.Now - startTime, // 計算執行時間
                Message = success ? ""完成"" : ""失敗"" // 設定訊息
            }; // 結果物件建立完成

            _results.Add(result); // 加入結果清單
            Console.WriteLine($""  {(success ? ""✅"" : ""❌"")} {stage}: {result.Duration.TotalSeconds:F1}s""); // 顯示結果

            if (!success) // 如果失敗
            {
                Console.WriteLine($""❌ Pipeline 在 {stage} 階段失敗！""); // 顯示失敗訊息
                return false; // 回傳失敗 // 停止後續階段
            }
        }

        Console.WriteLine(""✅ Pipeline 完成！所有階段成功""); // 全部完成
        return true; // 回傳成功
    }

    // 執行單一階段的方法 // 模擬各階段操作
    private async Task<bool> ExecuteStage(PipelineStage stage) // 執行階段方法
    {
        await Task.Delay(100); // 模擬執行時間 // 實際上會花更久
        return true; // 回傳成功 // 模擬用
    }
}
```

---

## 自動 Build → Test → Deploy 到 Server

```csharp
// 定義部署設定的類別 // 描述部署到 Pi 的參數
public class DeploymentConfig // 部署設定類別
{
    public string ServerHost { get; set; } = """";    // 伺服器 IP // Pi 的位址
    public int ServerPort { get; set; } = 22;        // SSH 埠號 // 預設 22
    public string Username { get; set; } = """";      // SSH 帳號 // 登入用
    public string SshKeyPath { get; set; } = """";    // SSH 金鑰路徑 // 免密碼登入用
    public string RemotePath { get; set; } = ""/opt/pos-app""; // 遠端安裝路徑 // Pi 上的目錄
    public string ServiceName { get; set; } = ""pos-app""; // 服務名稱 // systemd 服務名
    public string BuildConfig { get; set; } = ""Release""; // 建置組態 // Release 模式
    public string RuntimeId { get; set; } = ""linux-arm64""; // 目標平台 // ARM64 Linux
}

// 定義自動部署器的類別 // 執行自動化部署
public class AutoDeployer // 自動部署器
{
    private readonly DeploymentConfig _config; // 部署設定 // 建構時傳入

    public AutoDeployer(DeploymentConfig config) // 建構函式 // 接收設定
    {
        _config = config; // 儲存設定 // 後續步驟會用到
    }

    // 執行完整部署流程的方法 // Build → Test → Deploy
    public void Deploy() // 部署方法
    {
        Console.WriteLine(""=== 開始自動部署流程 ===""); // 顯示開始

        // 步驟 1：建置 // dotnet publish
        Console.WriteLine(""[1/4] 建置應用程式...""); // 顯示步驟
        Console.WriteLine($""  dotnet publish -c {_config.BuildConfig} -r {_config.RuntimeId}""); // 顯示指令

        // 步驟 2：測試 // dotnet test
        Console.WriteLine(""[2/4] 執行測試...""); // 顯示步驟
        Console.WriteLine(""  dotnet test --configuration Release""); // 顯示指令

        // 步驟 3：部署 // SCP 檔案到 Pi
        Console.WriteLine(""[3/4] 部署到 Raspberry Pi...""); // 顯示步驟
        Console.WriteLine($""  scp -r ./publish/* {_config.Username}@{_config.ServerHost}:{_config.RemotePath}""); // SCP 指令

        // 步驟 4：重啟 // 重啟 systemd 服務
        Console.WriteLine(""[4/4] 重啟 POS 服務...""); // 顯示步驟
        Console.WriteLine($""  ssh {_config.Username}@{_config.ServerHost} 'sudo systemctl restart {_config.ServiceName}'""); // SSH 重啟

        Console.WriteLine(""=== 部署完成！===""); // 顯示完成
    }
}
```

---

## Raspberry Pi 自動更新機制（Pull 或 Webhook）

> 💡 **比喻：快遞到府 vs. 自己去取件**
> Pull 模式就像你每天去郵局問「有我的包裹嗎？」（定時檢查）。
> Webhook 模式就像快遞員直接送到你家門口（伺服器主動通知）。
> 兩種都可以，但 Webhook 更即時。

```csharp
// 定義更新機制的列舉 // 兩種更新方式
public enum UpdateMode // 更新模式列舉
{
    Pull,    // 拉取模式 // Pi 定時去問有沒有新版
    Webhook  // 推送模式 // 伺服器有新版主動通知 Pi
}

// 定義 Pull 模式更新器的類別 // 定時檢查更新
public class PullUpdater // Pull 更新器
{
    private readonly string _updateUrl; // 更新檢查 URL // 伺服器的版本檢查端點
    private readonly TimeSpan _checkInterval; // 檢查間隔 // 多久檢查一次

    public PullUpdater(string updateUrl, TimeSpan interval) // 建構函式
    {
        _updateUrl = updateUrl; // 設定更新 URL
        _checkInterval = interval; // 設定檢查間隔
    }

    // 開始定時檢查的方法 // 背景執行
    public async Task StartChecking() // 開始檢查方法
    {
        while (true) // 無限迴圈 // 持續檢查
        {
            Console.WriteLine($""[{DateTime.Now:HH:mm:ss}] 檢查更新...""); // 顯示檢查訊息

            try // 嘗試檢查更新
            {
                var currentVersion = GetCurrentVersion(); // 取得目前版本 // 本地版本號
                Console.WriteLine($""  目前版本：{currentVersion}""); // 顯示目前版本

                // 模擬呼叫 API 檢查最新版本 // 實際上要用 HttpClient
                var latestVersion = ""1.0.1""; // 模擬最新版本 // 從伺服器取得
                Console.WriteLine($""  最新版本：{latestVersion}""); // 顯示最新版本

                if (currentVersion != latestVersion) // 如果版本不同
                {
                    Console.WriteLine(""  🔄 發現新版本，開始更新...""); // 開始更新
                    await DownloadAndApplyUpdate(latestVersion); // 下載並套用更新
                }
                else // 版本相同
                {
                    Console.WriteLine(""  ✅ 已是最新版本""); // 不需要更新
                }
            }
            catch (Exception ex) // 檢查失敗
            {
                Console.WriteLine($""  ❌ 檢查更新失敗：{ex.Message}""); // 顯示錯誤
            }

            await Task.Delay(_checkInterval); // 等待下次檢查 // 按照設定的間隔
        }
    }

    private string GetCurrentVersion() => ""1.0.0""; // 取得目前版本 // 簡化示範

    private async Task DownloadAndApplyUpdate(string version) // 下載並套用更新方法
    {
        Console.WriteLine($""  下載版本 {version}...""); // 顯示下載訊息
        await Task.Delay(100); // 模擬下載 // 實際上要下載檔案
        Console.WriteLine(""  更新完成，重啟服務...""); // 顯示更新完成
    }
}
```

---

## Docker 容器化 POS 應用

```dockerfile
# POS 系統的 Dockerfile // 多階段建置
# === 階段 1：Build === // 用 SDK Image 編譯
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# 設定工作目錄 // 所有操作在此目錄下
WORKDIR /src
# 複製專案檔 // 利用 Docker 層快取
COPY *.csproj ./
# 還原 NuGet 套件 // 這層變動少可以快取
RUN dotnet restore
# 複製所有原始碼 // 這層每次 build 都會變
COPY . .
# 發佈為 Release 模式 // 輸出到 /app/publish
RUN dotnet publish -c Release -o /app/publish

# === 階段 2：Runtime === // 用精簡的 Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim-arm64 AS runtime
# 設定工作目錄
WORKDIR /app
# 從 build 階段複製發佈結果 // 只要成品不要原始碼
COPY --from=build /app/publish .
# 設定時區 // 台灣時區
ENV TZ=Asia/Taipei
# 對外開放的埠號 // POS 系統的 Web 埠
EXPOSE 5000
# 啟動指令 // 執行 POS 應用程式
ENTRYPOINT [""dotnet"", ""PosApp.dll""]
```

### Docker Compose 整合

```yaml
# docker-compose.yml // 整合所有 POS 服務
version: '3.8' # Compose 版本 // 使用 3.8

services: # 服務定義 // 所有容器
  pos-web: # POS Web 應用 // 主要的 POS 系統
    build: . # 從當前目錄建置 // 使用 Dockerfile
    ports: # 埠號對應
      - ""5000:5000"" # 主機 5000 → 容器 5000
    environment: # 環境變數
      - ASPNETCORE_URLS=http://+:5000 # 監聽所有介面的 5000 埠
      - ConnectionStrings__DefaultConnection=Data Source=/data/pos.db # SQLite 路徑
    volumes: # 掛載資料卷
      - pos-data:/data # 資料庫持久化 // 重啟不會遺失資料
    restart: always # 自動重啟 // 異常時自動恢復

  print-agent: # Print Agent 服務 // Node.js 列印代理
    build: ./print-agent # 從 print-agent 目錄建置
    ports: # 埠號對應
      - ""9100:9100"" # WebSocket 埠
    devices: # 裝置對應 // 讓容器存取 USB 印表機
      - /dev/usb/lp0:/dev/usb/lp0 # USB 印表機裝置
    privileged: true # 特權模式 // 存取硬體需要
    restart: always # 自動重啟

volumes: # 資料卷定義
  pos-data: # POS 資料卷 // 儲存 SQLite 資料庫
```

---

## 多台 POS 設備管理（Fleet Management）

> 💡 **比喻：管理一群遙控汽車**
> 如果你的店有 5 台 POS 機，每台都要更新、監控、維護。
> Fleet Management 就像一個「遙控器」，
> 可以同時控制所有 POS 機，一次更新全部。

```csharp
// 定義 POS 設備的類別 // 描述單一 POS 機的資訊
public class PosDevice // POS 設備類別
{
    public string DeviceId { get; set; } = """";   // 設備 ID // 唯一識別碼
    public string Hostname { get; set; } = """";   // 主機名稱 // 例如 pos-01
    public string IpAddress { get; set; } = """";  // IP 位址 // 網路位址
    public string Version { get; set; } = """";    // 應用版本 // 目前的版本號
    public string Status { get; set; } = ""offline""; // 狀態 // online/offline/updating
    public DateTime LastSeen { get; set; }        // 最後上線時間 // 心跳檢測
    public string Location { get; set; } = """";   // 位置 // 例如「一樓櫃台」
}

// 定義設備管理器的類別 // 管理所有 POS 設備
public class FleetManager // 設備管理器
{
    private readonly List<PosDevice> _devices = new(); // 設備清單 // 所有 POS 機

    // 註冊新設備的方法 // POS 機上線時呼叫
    public void RegisterDevice(PosDevice device) // 註冊設備方法
    {
        _devices.Add(device); // 加入設備清單 // 新設備加入管理
        Console.WriteLine($""設備已註冊：{device.Hostname} ({device.IpAddress})""); // 顯示註冊訊息
    }

    // 取得所有設備狀態的方法 // 儀表板用
    public void PrintDashboard() // 顯示儀表板方法
    {
        Console.WriteLine(""╔════════════════════════════════════════════════════╗""); // 表格上框
        Console.WriteLine(""║          POS 設備管理儀表板                        ║""); // 標題
        Console.WriteLine(""╠════════════════════════════════════════════════════╣""); // 分隔線
        Console.WriteLine(""║ 名稱       IP              版本     狀態   位置   ║""); // 欄位標題
        Console.WriteLine(""╠════════════════════════════════════════════════════╣""); // 分隔線

        foreach (var d in _devices) // 逐一列出設備
        {
            var statusIcon = d.Status switch // 根據狀態顯示圖示
            {
                ""online"" => ""🟢"",   // 上線 // 綠色圓點
                ""offline"" => ""🔴"",  // 離線 // 紅色圓點
                ""updating"" => ""🟡"", // 更新中 // 黃色圓點
                _ => ""⚪""             // 未知 // 白色圓點
            }; // switch 結束
            Console.WriteLine($""║ {d.Hostname,-10} {d.IpAddress,-15} {d.Version,-8} {statusIcon,-4} {d.Location,-6} ║""); // 設備資訊
        }

        Console.WriteLine(""╚════════════════════════════════════════════════════╝""); // 表格下框
    }

    // 批次更新所有設備的方法 // 一次更新全部 POS 機
    public async Task UpdateAll(string newVersion) // 批次更新方法
    {
        Console.WriteLine($""開始批次更新到 v{newVersion}...""); // 顯示更新開始

        foreach (var device in _devices) // 逐一更新設備
        {
            device.Status = ""updating""; // 設定狀態為更新中
            Console.WriteLine($""  更新 {device.Hostname}...""); // 顯示更新進度

            await Task.Delay(200); // 模擬更新過程 // 實際上是 SCP + 重啟

            device.Version = newVersion; // 更新版本號
            device.Status = ""online""; // 恢復上線狀態
            Console.WriteLine($""  ✅ {device.Hostname} 更新完成""); // 顯示完成
        }

        Console.WriteLine($""全部 {_devices.Count} 台設備更新完成！""); // 顯示全部完成
    }
}
```

---

## 監控與遠端維護（SSH Tunnel、VPN）

```csharp
// 定義監控設定的類別 // 描述監控和遠端維護的設定
public class MonitoringConfig // 監控設定類別
{
    public int HealthCheckIntervalSeconds { get; set; } = 30; // 健康檢查間隔 // 30 秒一次
    public string SshTunnelHost { get; set; } = """"; // SSH Tunnel 伺服器 // 跳板機
    public int SshTunnelPort { get; set; } = 22;    // SSH Tunnel 埠號
    public string VpnServer { get; set; } = """";    // VPN 伺服器 // 安全連線用
    public bool EnableAlerts { get; set; } = true;  // 是否啟用警報 // 異常時通知
}

// 定義健康檢查結果的類別 // 單次檢查的結果
public class HealthCheckResult // 健康檢查結果
{
    public string DeviceId { get; set; } = """";  // 設備 ID // 哪台機器
    public DateTime CheckTime { get; set; }     // 檢查時間 // 什麼時候檢查的
    public bool IsHealthy { get; set; }         // 是否健康 // 正常/異常
    public double CpuUsage { get; set; }        // CPU 使用率 // 百分比
    public double MemoryUsage { get; set; }     // 記憶體使用率 // 百分比
    public double DiskUsage { get; set; }       // 硬碟使用率 // 百分比
    public bool PrinterOnline { get; set; }     // 印表機是否在線 // 列印服務狀態
    public bool NetworkOnline { get; set; }     // 網路是否在線 // 連線狀態
}

// 定義設備監控器的類別 // 持續監控所有 POS 設備
public class DeviceMonitor // 設備監控器
{
    private readonly MonitoringConfig _config; // 監控設定 // 建構時傳入

    public DeviceMonitor(MonitoringConfig config) // 建構函式
    {
        _config = config; // 儲存設定
    }

    // 執行健康檢查的方法 // 檢查單一設備
    public HealthCheckResult CheckDevice(PosDevice device) // 檢查設備方法
    {
        var result = new HealthCheckResult // 建立檢查結果
        {
            DeviceId = device.DeviceId, // 設備 ID
            CheckTime = DateTime.Now,   // 檢查時間
            CpuUsage = 35.2,           // 模擬 CPU 使用率 // 實際要用 SSH 查詢
            MemoryUsage = 62.8,        // 模擬記憶體使用率
            DiskUsage = 45.0,          // 模擬硬碟使用率
            PrinterOnline = true,      // 印表機在線
            NetworkOnline = true       // 網路在線
        }; // 結果建立完成

        result.IsHealthy = result.CpuUsage < 90 // CPU 低於 90%
            && result.MemoryUsage < 90  // 記憶體低於 90%
            && result.DiskUsage < 90    // 硬碟低於 90%
            && result.PrinterOnline     // 印表機在線
            && result.NetworkOnline;    // 網路在線

        if (!result.IsHealthy && _config.EnableAlerts) // 如果不健康且啟用警報
        {
            Console.WriteLine($""⚠️ 警報：{device.Hostname} 狀態異常！""); // 顯示警報
            Console.WriteLine($""  CPU: {result.CpuUsage}% | RAM: {result.MemoryUsage}% | Disk: {result.DiskUsage}%""); // 詳細數據
        }

        return result; // 回傳檢查結果
    }
}
```

---

## 資料同步（離線→上線自動同步）

> 💡 **比喻：出差回來整理報帳**
> POS 設備離線期間（網路斷了）的交易就像出差時的花費，
> 先用小本子記下來，等回到公司再整理報帳。
> 資料同步就是把「小本子上的記錄」匯入公司的系統。

```csharp
// 定義同步項目的類別 // 需要同步的單筆資料
public class SyncItem // 同步項目
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // 唯一 ID // UUID 格式
    public string Type { get; set; } = """";       // 資料類型 // order, product, inventory
    public string JsonData { get; set; } = """";   // JSON 資料 // 序列化的資料內容
    public DateTime CreatedAt { get; set; } = DateTime.Now; // 建立時間 // 離線時記錄
    public bool IsSynced { get; set; } = false;  // 是否已同步 // 同步後設為 true
    public int RetryCount { get; set; } = 0;     // 重試次數 // 同步失敗累計
}

// 定義資料同步管理器的類別 // 處理離線→上線的資料同步
public class SyncManager // 同步管理器
{
    private readonly List<SyncItem> _pendingItems = new(); // 待同步清單 // 離線期間累積的資料
    private readonly string _serverUrl; // 伺服器 URL // 總部伺服器的位址

    public SyncManager(string serverUrl) // 建構函式 // 接收伺服器位址
    {
        _serverUrl = serverUrl; // 儲存伺服器 URL
    }

    // 加入待同步項目的方法 // 離線時呼叫
    public void AddPending(string type, string jsonData) // 加入待同步方法
    {
        var item = new SyncItem // 建立同步項目
        {
            Type = type,       // 設定資料類型
            JsonData = jsonData // 設定 JSON 資料
        }; // 項目建立完成

        _pendingItems.Add(item); // 加入待同步清單
        Console.WriteLine($""新增待同步項目：{type}（共 {_pendingItems.Count} 筆待同步）""); // 顯示訊息
    }

    // 執行同步的方法 // 網路恢復時呼叫
    public async Task SyncAll() // 同步全部方法
    {
        var unsyncedItems = _pendingItems // 篩選未同步的項目
            .Where(i => !i.IsSynced) // 只處理未同步的
            .OrderBy(i => i.CreatedAt) // 按建立時間排序 // 先進先出
            .ToList(); // 轉為清單

        if (unsyncedItems.Count == 0) // 沒有待同步的
        {
            Console.WriteLine(""沒有待同步的項目""); // 顯示訊息
            return; // 結束
        }

        Console.WriteLine($""開始同步 {unsyncedItems.Count} 筆資料...""); // 顯示開始

        var successCount = 0; // 成功計數 // 統計用
        var failCount = 0;    // 失敗計數 // 統計用

        foreach (var item in unsyncedItems) // 逐筆同步
        {
            try // 嘗試同步
            {
                Console.WriteLine($""  同步 {item.Type} ({item.Id[..8]}...)""); // 顯示同步項目
                await Task.Delay(50); // 模擬 API 呼叫 // 實際要用 HttpClient POST

                item.IsSynced = true; // 標記為已同步
                successCount++; // 成功計數加 1
            }
            catch (Exception ex) // 同步失敗
            {
                item.RetryCount++; // 重試次數加 1
                failCount++; // 失敗計數加 1
                Console.WriteLine($""  ❌ 同步失敗（第 {item.RetryCount} 次）：{ex.Message}""); // 顯示錯誤

                if (item.RetryCount >= 5) // 重試超過 5 次
                {
                    Console.WriteLine($""  ⚠️ 項目 {item.Id[..8]} 已超過重試上限，標記為需人工處理""); // 需要人工介入
                }
            }
        }

        Console.WriteLine($""同步完成！成功 {successCount} 筆，失敗 {failCount} 筆""); // 顯示結果
    }
}
```

---

## 安全考量（HTTPS、防火牆、VPN）

```csharp
// 定義安全設定的類別 // POS 系統的安全相關設定
public class SecurityConfig // 安全設定類別
{
    public bool EnableHttps { get; set; } = true;   // 啟用 HTTPS // 加密傳輸
    public bool EnableFirewall { get; set; } = true; // 啟用防火牆 // 限制網路存取
    public bool EnableVpn { get; set; } = false;    // 啟用 VPN // 遠端管理用
    public int SessionTimeoutMinutes { get; set; } = 30; // 工作階段逾時 // 閒置自動登出
    public List<string> AllowedPorts { get; set; } = new() // 允許的埠號 // 防火牆規則
    {
        ""22"",   // SSH // 遠端管理
        ""443"",  // HTTPS // Web 加密連線
        ""5000"", // POS Web // 應用程式埠
        ""9100""  // Print Agent // 列印服務埠
    }; // 允許的埠號清單
}

// 定義安全檢查器的類別 // 檢查 POS 系統的安全狀態
public class SecurityAuditor // 安全檢查器
{
    // 執行安全稽核的方法 // 檢查所有安全設定
    public List<string> Audit(SecurityConfig config) // 安全稽核方法
    {
        var findings = new List<string>(); // 發現清單 // 收集所有問題

        if (!config.EnableHttps) // 如果沒啟用 HTTPS
            findings.Add(""⚠️ 高風險：未啟用 HTTPS，交易資料可能被攔截""); // 加入警告

        if (!config.EnableFirewall) // 如果沒啟用防火牆
            findings.Add(""⚠️ 高風險：未啟用防火牆，POS 設備暴露在網路中""); // 加入警告

        if (config.AllowedPorts.Contains(""80"")) // 如果允許 HTTP（不安全）
            findings.Add(""⚠️ 中風險：開放了 HTTP port 80，建議只用 HTTPS""); // 加入警告

        if (config.SessionTimeoutMinutes > 60) // 如果工作階段逾時太長
            findings.Add(""⚠️ 低風險：工作階段逾時設定過長，建議 30 分鐘內""); // 加入警告

        if (findings.Count == 0) // 沒有發現問題
            findings.Add(""✅ 所有安全設定都正常""); // 加入通過訊息

        return findings; // 回傳稽核結果
    }
}
```

---

## 實際部署案例分析

```
POS 系統實際部署案例：小型咖啡廳

📍 店家規模：
   - 1 間店面，2 台 POS 設備
   - 1 台熱感應印表機
   - 1 個錢箱
   - 觸控螢幕

🔧 硬體配置：
   ┌─────────────────────────────────────────────┐
   │ POS 機 1（前台）                              │
   │ ├── Raspberry Pi 4 (4GB)                     │
   │ ├── 10 吋觸控螢幕                             │
   │ ├── 熱感應印表機（USB）                       │
   │ ├── 錢箱（RJ-11 接印表機）                   │
   │ └── 條碼掃描器（USB）                         │
   │                                              │
   │ POS 機 2（外帶區）                            │
   │ ├── Raspberry Pi 4 (4GB)                     │
   │ ├── 7 吋觸控螢幕                              │
   │ └── 條碼掃描器（USB）                         │
   │                                              │
   │ 伺服器（後台管理）                            │
   │ ├── 雲端 VPS 或本地 NAS                      │
   │ ├── 資料庫（PostgreSQL）                     │
   │ └── 管理後台（報表、庫存）                   │
   └─────────────────────────────────────────────┘

💰 成本估算：
   Pi 4 (4GB) x2     = NT$ 4,000
   觸控螢幕 x2        = NT$ 6,000
   熱感應印表機 x1     = NT$ 3,000
   錢箱 x1            = NT$ 2,000
   條碼掃描器 x2       = NT$ 2,000
   SD 卡 x2           = NT$ 600
   ─────────────────────────────
   總計               ≈ NT$ 17,600

   vs. 市售 POS 系統  ≈ NT$ 50,000+（還不含月租費）
   省下超過 60% 的成本！
```

### 用 C# 模擬部署規劃

```csharp
// 定義部署規劃的類別 // 描述完整的部署方案
public class DeploymentPlan // 部署規劃類別
{
    public string StoreName { get; set; } = """"; // 店家名稱 // 部署的對象
    public int PosCount { get; set; }          // POS 數量 // 幾台收銀機
    public List<PosDevice> Devices { get; set; } = new(); // 設備清單 // 所有 POS 機
    public decimal EstimatedCost { get; set; } // 預估成本 // 硬體+軟體

    // 產生部署報告的方法 // 給店家看的評估報告
    public void GenerateReport() // 產生報告方法
    {
        Console.WriteLine(""╔══════════════════════════════════════╗""); // 報告框線
        Console.WriteLine($""║  POS 系統部署規劃報告              ║""); // 報告標題
        Console.WriteLine($""║  店家：{StoreName,-28}║""); // 店家名稱
        Console.WriteLine(""╠══════════════════════════════════════╣""); // 分隔線
        Console.WriteLine($""║  POS 設備數量：{PosCount} 台         ║""); // POS 數量
        Console.WriteLine($""║  預估總成本：NT$ {EstimatedCost:N0}  ║""); // 預估成本

        foreach (var device in Devices) // 逐一列出設備
        {
            Console.WriteLine($""║  {device.Hostname}: {device.Location,-20}║""); // 設備名稱和位置
        }

        Console.WriteLine(""╚══════════════════════════════════════╝""); // 報告底框
    }
}
```

---

## 🤔 我這樣寫為什麼會錯？

### ❌ 錯誤 1：把密碼和金鑰寫死在程式碼裡

```csharp
// ❌ 錯誤：密碼和金鑰寫死在原始碼 // 推上 GitHub 全世界都看得到
var config = new DeploymentConfig // 部署設定
{
    ServerHost = ""192.168.1.100"", // Pi 的 IP
    Username = ""admin"",           // ❌ 帳號寫在程式碼裡
    SshKeyPath = ""/home/user/.ssh/id_rsa"" // ❌ 金鑰路徑寫在程式碼裡
}; // 這些資訊推上 Git 就洩漏了

// ✅ 正確：使用環境變數或 Secrets // 永遠不把敏感資訊放在程式碼裡
var secureConfig = new DeploymentConfig // 安全的部署設定
{
    ServerHost = Environment.GetEnvironmentVariable(""PI_HOST"") ?? """", // 從環境變數讀取 // 不會進版控
    Username = Environment.GetEnvironmentVariable(""PI_USER"") ?? """",  // 從環境變數讀取
    SshKeyPath = Environment.GetEnvironmentVariable(""PI_SSH_KEY"") ?? """" // 從環境變數讀取
}; // 敏感資訊不會出現在 Git 歷史中
```

### ❌ 錯誤 2：Docker 容器直接用 root 執行

```dockerfile
# ❌ 錯誤：用 root 身份執行應用程式 // 安全隱患
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
# 沒有切換使用者 // 預設就是 root // 如果應用程式被入侵，攻擊者就有 root 權限
ENTRYPOINT [""dotnet"", ""PosApp.dll""]

# ✅ 正確：建立專用使用者 // 最小權限原則
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
# 建立非 root 使用者 // 專用的應用程式帳號
RUN adduser --disabled-password --gecos '' appuser
# 切換到非 root 使用者 // 降低安全風險
USER appuser
ENTRYPOINT [""dotnet"", ""PosApp.dll""]
```

### ❌ 錯誤 3：同步時沒有處理衝突

```csharp
// ❌ 錯誤：同步時直接覆蓋 // 離線期間的修改會被蓋掉
public async Task SyncNaive(string jsonData) // 天真的同步方法
{
    // 直接 POST 到伺服器 // 不管伺服器上的資料有沒有被別人改過
    Console.WriteLine(""直接覆蓋伺服器資料...""); // 暴力覆蓋 // 可能遺失其他 POS 機的交易
}

// ✅ 正確：使用時間戳和衝突偵測 // 確保不會遺失資料
public async Task SyncWithConflictCheck(SyncItem item) // 有衝突檢查的同步方法
{
    Console.WriteLine($""檢查衝突：{item.Type} ({item.Id[..8]})""); // 顯示檢查訊息

    // 1. 先查詢伺服器上的最新版本 // 比較時間戳
    Console.WriteLine(""  查詢伺服器版本...""); // 取得伺服器資料

    // 2. 如果伺服器版本比本地舊，直接同步 // 本地是最新的
    // 3. 如果伺服器版本比本地新，需要合併 // 有人在我離線時改了資料
    // 4. 對於訂單資料，通常用「追加」而非「覆蓋」// 新訂單直接加入
    Console.WriteLine(""  使用追加模式同步訂單資料""); // 訂單只增不改 // 最安全的方式

    item.IsSynced = true; // 標記為已同步
    Console.WriteLine(""  ✅ 同步完成（無衝突）""); // 顯示完成
}
```
" }
    };
}
