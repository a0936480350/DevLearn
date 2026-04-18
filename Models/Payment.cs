namespace DotNetLearning.Models;

/// <summary>
/// 付款訂單（目前只做「老師 Premium 月費訂閱」，未來可擴充到其他商品）
/// </summary>
public class Payment
{
    public int Id { get; set; }

    // ─── 訂單識別 ─────────────────────────
    public string OrderId { get; set; } = "";        // 我們自己生的訂單號，送給 ECPay 當 MerchantTradeNo
    public string? EcpayTradeNo { get; set; }         // ECPay 回傳的交易號
    public string UserAnonymousId { get; set; } = ""; // 付款人 SiteUser.AnonymousId

    // ─── 商品 ─────────────────────────────
    public string ProductType { get; set; } = "teacher_monthly";  // teacher_monthly / teacher_yearly / ...
    public string ProductName { get; set; } = "老師 Premium 月費";
    public int Amount { get; set; }                  // 台幣整數，不帶小數

    // ─── 金流狀態 ─────────────────────────
    public string Status { get; set; } = "pending";  // pending / paid / failed / refunded / expired
    public string PaymentMethod { get; set; } = "";  // Credit / WebATM / ATM / CVS / BARCODE
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
    public string RawCallbackPayload { get; set; } = ""; // ECPay Notify 回來的完整 form data（除錯用）

    // ─── 開通效期（for subscription products）───
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveUntil { get; set; }
}

/// <summary>
/// 老師的訂閱狀態（冗餘快取，避免每次查 Payment 表）
/// </summary>
public class TeacherSubscription
{
    public int Id { get; set; }
    public string UserAnonymousId { get; set; } = ""; // SiteUser.AnonymousId
    public string Tier { get; set; } = "free";        // free / monthly / yearly
    public DateTime? ActiveUntil { get; set; }         // null 代表沒付過 / 已過期時還是保留日期
    public int TotalPaidAmount { get; set; }           // 累積付了多少（未來做 VIP 分級用）
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive => Tier != "free" && ActiveUntil.HasValue && ActiveUntil.Value > DateTime.UtcNow;
}
