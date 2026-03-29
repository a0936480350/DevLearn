namespace DotNetLearning.Data;
using DotNetLearning.Models;

public static class SeedQuestions_IoT
{
    public static List<Question> GetQuestions()
    {
        return new List<Question>
        {
            // ── Chapter 600: Raspberry Pi 入門與環境建置 ──
            new() { Id=2000, ChapterId=600, Type="multiple", Difficulty=1, QuestionText="Raspberry Pi 最常用的作業系統是？", OptionsJson=@"[""A. Windows 11"",""B. macOS"",""C. Raspberry Pi OS (Linux)"",""D. Android""]", CorrectAnswer="C", Explanation="Raspberry Pi OS 是基於 Debian 的官方 Linux 系統，最適合 Pi 使用。" },
            new() { Id=2001, ChapterId=600, Type="truefalse", Difficulty=1, QuestionText="Raspberry Pi 可以安裝 .NET Runtime 來執行 C# 程式。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="正確", Explanation=".NET 支援 ARM 架構，可以在 Raspberry Pi 上執行 .NET 應用程式。" },
            new() { Id=2002, ChapterId=600, Type="fillin", Difficulty=1, QuestionText="透過 SSH 連線到 Pi 的指令是 ssh ___@<IP位址>", OptionsJson="[]", CorrectAnswer="pi", Explanation="預設使用者名稱為 pi，可透過 SSH 遠端連線操作 Pi。" },

            // ── Chapter 601: Web-Based POS 系統開發 ──
            new() { Id=2003, ChapterId=601, Type="multiple", Difficulty=2, QuestionText="Web-Based POS 系統相較於傳統 POS 的最大優勢是？", OptionsJson=@"[""A. 不需要網路"",""B. 跨平台且易於更新"",""C. 速度一定比較快"",""D. 不需要伺服器""]", CorrectAnswer="B", Explanation="Web-Based POS 透過瀏覽器運行，不受作業系統限制，更新只需部署伺服器端。" },
            new() { Id=2004, ChapterId=601, Type="fillin", Difficulty=2, QuestionText="ASP.NET Core POS 系統中，處理交易邏輯通常放在 ___ 層", OptionsJson="[]", CorrectAnswer="Service", Explanation="Service 層負責商業邏輯，Controller 只負責接收請求和回傳結果。" },
            new() { Id=2005, ChapterId=601, Type="truefalse", Difficulty=2, QuestionText="POS 系統必須在有網路的環境下才能完全運作，不需要離線機制。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="錯誤", Explanation="好的 POS 系統應該有離線模式，在斷網時仍可記帳，恢復網路後再同步。" },

            // ── Chapter 602: Print Agent 與硬體整合 ──
            new() { Id=2006, ChapterId=602, Type="multiple", Difficulty=2, QuestionText="Print Agent 在 POS 系統中的角色是？", OptionsJson=@"[""A. 管理資料庫"",""B. 處理付款"",""C. 橋接 Web 應用與本地印表機"",""D. 監控網路""]", CorrectAnswer="C", Explanation="Print Agent 作為中間層，讓 Web 應用可以控制本地連接的收據印表機。" },
            new() { Id=2007, ChapterId=602, Type="fillin", Difficulty=2, QuestionText="熱感應印表機常用的通訊協定是 ___ 指令集", OptionsJson="[]", CorrectAnswer="ESC/POS", Explanation="ESC/POS 是 Epson 開發的收據印表機標準指令集，大多數熱感應印表機都支援。" },
            new() { Id=2008, ChapterId=602, Type="truefalse", Difficulty=3, QuestionText="Print Agent 可以直接在瀏覽器的 JavaScript 中實作，不需要額外的本地程式。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="錯誤", Explanation="瀏覽器基於安全限制無法直接存取本地硬體，需要本地 Agent 程式作為橋接。" },

            // ── Chapter 603: CI/CD 自動部署到 POS 設備 ──
            new() { Id=2009, ChapterId=603, Type="multiple", Difficulty=3, QuestionText="在 CI/CD 中，CD 代表什麼？", OptionsJson=@"[""A. Code Development"",""B. Continuous Delivery / Deployment"",""C. Central Database"",""D. Cloud Distribution""]", CorrectAnswer="B", Explanation="CD 是 Continuous Delivery（持續交付）或 Continuous Deployment（持續部署）。" },
            new() { Id=2010, ChapterId=603, Type="fillin", Difficulty=2, QuestionText="GitHub Actions 的設定檔放在 .github/___ 目錄下", OptionsJson="[]", CorrectAnswer="workflows", Explanation="GitHub Actions 的 YAML 設定檔放在 .github/workflows/ 目錄。" },
            new() { Id=2011, ChapterId=603, Type="truefalse", Difficulty=3, QuestionText="部署到 Raspberry Pi 時，可以使用 SCP 或 rsync 來傳輸檔案。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="正確", Explanation="SCP 和 rsync 都是透過 SSH 傳輸檔案的常用方式，適合部署到遠端 Pi。" },

            // ── Chapter 604: Web Kiosk 模式完整指南 ──
            new() { Id=2012, ChapterId=604, Type="multiple", Difficulty=1, QuestionText="Kiosk 模式的主要用途是？", OptionsJson=@"[""A. 讓使用者自由上網"",""B. 限制裝置只顯示特定應用程式"",""C. 加速網路速度"",""D. 備份資料""]", CorrectAnswer="B", Explanation="Kiosk 模式鎖定裝置只能執行指定的 Web 應用，防止使用者操作其他功能。" },
            new() { Id=2013, ChapterId=604, Type="fillin", Difficulty=2, QuestionText="Chromium 瀏覽器啟動 Kiosk 模式的參數是 --___", OptionsJson="[]", CorrectAnswer="kiosk", Explanation="使用 chromium-browser --kiosk <URL> 可以全螢幕鎖定顯示特定網頁。" },
            new() { Id=2014, ChapterId=604, Type="truefalse", Difficulty=1, QuestionText="Kiosk 模式下使用者仍然可以按 Alt+F4 關閉視窗。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="錯誤", Explanation="正確設定的 Kiosk 模式會禁用快捷鍵，防止使用者離開指定應用程式。" },

            // ── Chapter 605: IoT 通訊協定與感測器 ──
            new() { Id=2015, ChapterId=605, Type="multiple", Difficulty=2, QuestionText="MQTT 協定最適合以下哪種場景？", OptionsJson=@"[""A. 大檔案傳輸"",""B. 低頻寬 IoT 裝置訊息傳遞"",""C. 視訊串流"",""D. 資料庫查詢""]", CorrectAnswer="B", Explanation="MQTT 是輕量級的發布/訂閱協定，專為低頻寬、不穩定網路的 IoT 場景設計。" },
            new() { Id=2016, ChapterId=605, Type="fillin", Difficulty=2, QuestionText="MQTT 的訊息傳遞模式是發布/___", OptionsJson="[]", CorrectAnswer="訂閱", Explanation="MQTT 使用 Publish/Subscribe（發布/訂閱）模式，透過 Broker 中轉訊息。" },
            new() { Id=2017, ChapterId=605, Type="truefalse", Difficulty=2, QuestionText="I2C 和 SPI 都是常見的感測器通訊介面。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="正確", Explanation="I2C 使用兩條線（SDA、SCL），SPI 使用四條線，都常用於連接感測器。" },

            // ── Chapter 606: 金流與支付整合 ──
            new() { Id=2018, ChapterId=606, Type="multiple", Difficulty=2, QuestionText="PCI DSS 是什麼？", OptionsJson=@"[""A. 程式語言"",""B. 支付卡產業資料安全標準"",""C. 資料庫系統"",""D. 網路協定""]", CorrectAnswer="B", Explanation="PCI DSS（Payment Card Industry Data Security Standard）是處理信用卡資料時必須遵守的安全標準。" },
            new() { Id=2019, ChapterId=606, Type="fillin", Difficulty=2, QuestionText="第三方金流串接時，伺服器接收付款結果的回呼端點叫做 ___", OptionsJson="[]", CorrectAnswer="Webhook", Explanation="Webhook 是金流服務在付款完成後，主動通知你的伺服器的機制。" },
            new() { Id=2020, ChapterId=606, Type="truefalse", Difficulty=3, QuestionText="在前端 JavaScript 中直接處理信用卡卡號是安全的做法。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="錯誤", Explanation="信用卡資料應透過金流商提供的安全表單處理，不應在自己的前端程式碼中直接處理。" },

            // ── Chapter 607: 完整 POS 系統架構設計 ──
            new() { Id=2021, ChapterId=607, Type="multiple", Difficulty=3, QuestionText="POS 系統採用 Event Sourcing 的好處是？", OptionsJson=@"[""A. 減少程式碼量"",""B. 完整記錄所有交易變更歷程"",""C. 加快資料庫查詢"",""D. 減少網路流量""]", CorrectAnswer="B", Explanation="Event Sourcing 記錄每一筆狀態變更事件，可完整追溯交易歷程，適合金融場景。" },
            new() { Id=2022, ChapterId=607, Type="fillin", Difficulty=3, QuestionText="微服務架構中，各服務之間通訊常用 ___ Queue 來解耦", OptionsJson="[]", CorrectAnswer="Message", Explanation="Message Queue（訊息佇列）如 RabbitMQ、Redis 可以讓服務之間非同步通訊、降低耦合。" },
            new() { Id=2023, ChapterId=607, Type="truefalse", Difficulty=2, QuestionText="POS 系統的前後端應該部署在同一台機器上以提高效能。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="錯誤", Explanation="前後端分離部署更利於擴展、維護和負載均衡，不一定要在同一台機器。" },

            // ── Chapter 608: Raspberry Pi 進階維運 ──
            new() { Id=2024, ChapterId=608, Type="multiple", Difficulty=2, QuestionText="監控 Raspberry Pi CPU 溫度的指令是？", OptionsJson=@"[""A. top"",""B. vcgencmd measure_temp"",""C. df -h"",""D. free -m""]", CorrectAnswer="B", Explanation="vcgencmd measure_temp 可以讀取 Pi 的 CPU 溫度，用於監控散熱狀況。" },
            new() { Id=2025, ChapterId=608, Type="fillin", Difficulty=2, QuestionText="Linux 中設定定時任務的工具叫 ___", OptionsJson="[]", CorrectAnswer="crontab", Explanation="crontab 用來設定排程任務，例如定時重啟服務或清理日誌。" },
            new() { Id=2026, ChapterId=608, Type="truefalse", Difficulty=3, QuestionText="Raspberry Pi 突然斷電不會對 SD 卡造成任何影響。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="錯誤", Explanation="突然斷電可能導致 SD 卡檔案系統損壞，應使用 UPS 或唯讀檔案系統來保護。" },

            // ── Chapter 609: Raspberry Pi 硬體組裝與螢幕串接 ──
            new() { Id=2027, ChapterId=609, Type="multiple", Difficulty=1, QuestionText="Raspberry Pi 的 GPIO 是什麼？", OptionsJson=@"[""A. 圖形處理器"",""B. 通用輸入輸出腳位"",""C. 網路接口"",""D. 記憶體模組""]", CorrectAnswer="B", Explanation="GPIO（General Purpose Input/Output）是通用輸入輸出腳位，可以控制 LED、按鈕、感測器等。" },
            new() { Id=2028, ChapterId=609, Type="fillin", Difficulty=1, QuestionText="Raspberry Pi 官方觸控螢幕使用 ___ 介面連接", OptionsJson="[]", CorrectAnswer="DSI", Explanation="DSI（Display Serial Interface）是 Pi 官方觸控螢幕使用的排線介面。" },
            new() { Id=2029, ChapterId=609, Type="truefalse", Difficulty=1, QuestionText="所有 HDMI 螢幕都可以直接連接 Raspberry Pi 4 使用。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="錯誤", Explanation="Pi 4 使用 micro HDMI 接口，需要 micro HDMI 轉 HDMI 轉接線才能連接標準 HDMI 螢幕。" },

            // ── Chapter 610: 從零組裝一台 POS 機實戰 ──
            new() { Id=2030, ChapterId=610, Type="multiple", Difficulty=2, QuestionText="組裝 POS 機時，最重要的散熱措施是？", OptionsJson=@"[""A. 不需要散熱"",""B. 加裝散熱片和風扇"",""C. 放在冰箱裡"",""D. 降低螢幕亮度""]", CorrectAnswer="B", Explanation="Pi 長時間運行會過熱降頻，加裝散熱片和風扇是最基本的散熱措施。" },
            new() { Id=2031, ChapterId=610, Type="fillin", Difficulty=2, QuestionText="POS 機的收據印表機通常使用 ___ 接口連接", OptionsJson="[]", CorrectAnswer="USB", Explanation="大部分收據印表機支援 USB 連接，也有部分支援藍牙或網路連接。" },
            new() { Id=2032, ChapterId=610, Type="truefalse", Difficulty=2, QuestionText="組裝 POS 機時，選用 SSD 比 SD 卡更穩定可靠。", OptionsJson=@"[""正確"",""錯誤""]", CorrectAnswer="正確", Explanation="SD 卡寫入壽命有限且容易損壞，SSD 透過 USB 連接更適合長期運行的 POS 場景。" },
        };
    }
}
