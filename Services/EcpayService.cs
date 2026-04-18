using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DotNetLearning.Services;

/// <summary>
/// 綠界 ECPay 全功能金流（AioCheckOut v5）簽章 + 參數產生器。
/// 沙箱模式：用內建的公開測試 MerchantID / HashKey / HashIV。
/// 正式模式：從環境變數讀 ECPAY_MERCHANT_ID / ECPAY_HASH_KEY / ECPAY_HASH_IV。
/// </summary>
public class EcpayService
{
    public string MerchantId { get; }
    public string HashKey { get; }
    public string HashIV { get; }
    public string PaymentUrl { get; }
    public string QueryUrl { get; }
    public bool IsSandbox { get; }

    public EcpayService()
    {
        var envMerchant = Environment.GetEnvironmentVariable("ECPAY_MERCHANT_ID");
        var envKey = Environment.GetEnvironmentVariable("ECPAY_HASH_KEY");
        var envIv = Environment.GetEnvironmentVariable("ECPAY_HASH_IV");

        if (!string.IsNullOrWhiteSpace(envMerchant) &&
            !string.IsNullOrWhiteSpace(envKey) &&
            !string.IsNullOrWhiteSpace(envIv))
        {
            // 正式環境
            MerchantId = envMerchant;
            HashKey = envKey;
            HashIV = envIv;
            PaymentUrl = "https://payment.ecpay.com.tw/Cashier/AioCheckOut/V5";
            QueryUrl = "https://payment.ecpay.com.tw/Cashier/QueryTradeInfo/V5";
            IsSandbox = false;
        }
        else
        {
            // 沙箱測試環境（ECPay 官方公開的測試商店）
            MerchantId = "2000132";
            HashKey = "5294y06JbISpM5x9";
            HashIV = "v77hoKGq4kWxNNIS";
            PaymentUrl = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";
            QueryUrl = "https://payment-stage.ecpay.com.tw/Cashier/QueryTradeInfo/V5";
            IsSandbox = true;
        }
    }

    /// <summary>
    /// 建立付款參數（含 CheckMacValue），View 直接把這些欄位 POST 到 PaymentUrl。
    /// </summary>
    public Dictionary<string, string> BuildCheckoutParams(
        string merchantTradeNo,
        int totalAmount,
        string tradeDesc,
        string itemName,
        string returnUrl,
        string clientBackUrl,
        string orderResultUrl,
        string? customField1 = null)
    {
        var parameters = new Dictionary<string, string>
        {
            ["MerchantID"] = MerchantId,
            ["MerchantTradeNo"] = merchantTradeNo,                                   // 唯一訂單號（英數，最長 20）
            ["MerchantTradeDate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),   // 注意要台灣時區
            ["PaymentType"] = "aio",
            ["TotalAmount"] = totalAmount.ToString(),
            ["TradeDesc"] = tradeDesc,
            ["ItemName"] = itemName,
            ["ReturnURL"] = returnUrl,           // ECPay 伺服器 → 我們伺服器（重要！這條是唯一可信的付款結果）
            ["ClientBackURL"] = clientBackUrl,   // 使用者按「返回商店」回的網址
            ["OrderResultURL"] = orderResultUrl, // 付完款 browser 被 redirect 的網址
            ["ChoosePayment"] = "ALL",           // ALL / Credit / WebATM / ATM / CVS / BARCODE
            ["EncryptType"] = "1",               // 1 = SHA256
        };
        if (!string.IsNullOrWhiteSpace(customField1))
            parameters["CustomField1"] = customField1;

        parameters["CheckMacValue"] = GenerateCheckMacValue(parameters);
        return parameters;
    }

    /// <summary>
    /// 驗證 ECPay 回來的 callback 簽章是否正確。
    /// </summary>
    public bool VerifyCallback(IDictionary<string, string> callbackData)
    {
        if (!callbackData.TryGetValue("CheckMacValue", out var receivedMac))
            return false;

        var forHash = new Dictionary<string, string>(callbackData);
        forHash.Remove("CheckMacValue");

        var calculatedMac = GenerateCheckMacValue(forHash);
        return string.Equals(receivedMac, calculatedMac, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 產生 CheckMacValue（SHA256）。
    /// 演算法：
    ///   1. 參數依名稱字母 A→Z 排序
    ///   2. 用 & 串接成 "Key1=Value1&Key2=Value2..."
    ///   3. 前後加上 HashKey 和 HashIV：HashKey=xxx&...&HashIV=yyy
    ///   4. URL encode 成小寫
    ///   5. 對整個字串做 SHA256 → 轉大寫 hex
    /// </summary>
    private string GenerateCheckMacValue(IDictionary<string, string> parameters)
    {
        var sorted = parameters
            .Where(kv => kv.Key != "CheckMacValue")
            .OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase)
            .Select(kv => $"{kv.Key}={kv.Value}");

        var joined = $"HashKey={HashKey}&{string.Join("&", sorted)}&HashIV={HashIV}";
        // ECPay 的 URL encode 規則：特殊字元按 .NET UrlEncode，然後把某些字元還原
        var encoded = HttpUtility.UrlEncode(joined, Encoding.UTF8)?.ToLowerInvariant() ?? "";
        // ECPay 官方指定要還原這些字元
        encoded = encoded
            .Replace("%2d", "-").Replace("%5f", "_").Replace("%2e", ".")
            .Replace("%21", "!").Replace("%2a", "*").Replace("%28", "(")
            .Replace("%29", ")").Replace("%20", "+");

        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(encoded));
        var hex = new StringBuilder(hash.Length * 2);
        foreach (var b in hash) hex.Append(b.ToString("X2"));
        return hex.ToString();
    }

    /// <summary>
    /// 產生唯一訂單號（英數 20 碼以內）
    /// 格式：DL + yyMMddHHmm + 8 位 random hex = 共 20 碼
    /// </summary>
    public static string GenerateMerchantTradeNo(string prefix = "DL")
    {
        var ts = DateTime.Now.ToString("yyMMddHHmm");
        var rand = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpperInvariant();
        return $"{prefix}{ts}{rand}"; // 長度 = 2 + 10 + 8 = 20
    }
}
