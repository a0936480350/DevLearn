using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;
using DotNetLearning.Services;

namespace DotNetLearning.Controllers;

/// <summary>
/// ECPay 金流 — 目前主力：老師 Premium 月費訂閱。
/// 流程：
///   /Payment/SubscribeTeacher  (GET)  建立訂單 → render 自動 POST 到 ECPay 的頁面
///   /Payment/EcpayReturn       (POST) 綠界伺服器 → 我方伺服器（唯一可信的付款結果通知）
///   /Payment/EcpayOrderResult  (POST) 使用者 browser 被 redirect 回來的 landing 頁
///   /Payment/MySubscription    (GET)  使用者查自己訂閱狀態
/// </summary>
public class PaymentController : Controller
{
    private readonly AppDbContext _db;
    private readonly EcpayService _ecpay;
    private readonly ILogger<PaymentController> _logger;
    private const int TeacherMonthlyAmount = 299; // NTD / 月

    public PaymentController(AppDbContext db, EcpayService ecpay, ILogger<PaymentController> logger)
    {
        _db = db;
        _ecpay = ecpay;
        _logger = logger;
    }

    private string? GetAnonId()
        => Request.Cookies["DotNetLearner"] ?? HttpContext.Session.GetString("SessionId");

    // ════════════════════════════════════════════════
    //  1. 建立老師月費訂單 → 導去 ECPay
    // ════════════════════════════════════════════════
    [HttpGet]
    public async Task<IActionResult> SubscribeTeacher()
    {
        var anonId = GetAnonId();
        if (string.IsNullOrEmpty(anonId))
        {
            TempData["Error"] = "請先登入才能訂閱";
            return RedirectToAction("Login", "Account");
        }

        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == anonId);
        if (user == null || !user.IsRegistered)
        {
            TempData["Error"] = "訂閱功能需要註冊會員";
            return RedirectToAction("Register", "Account");
        }

        // 產訂單
        var orderId = EcpayService.GenerateMerchantTradeNo("TCH"); // Teacher
        var payment = new Payment
        {
            OrderId = orderId,
            UserAnonymousId = anonId,
            ProductType = "teacher_monthly",
            ProductName = "DevLearn 老師 Premium 月費",
            Amount = TeacherMonthlyAmount,
            Status = "pending",
        };
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        // 組 ECPay 參數
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var parameters = _ecpay.BuildCheckoutParams(
            merchantTradeNo: orderId,
            totalAmount: TeacherMonthlyAmount,
            tradeDesc: "DevLearn Teacher Premium Monthly",
            itemName: "DevLearn 老師 Premium 月費 x 1",
            returnUrl: $"{baseUrl}/Payment/EcpayReturn",
            clientBackUrl: $"{baseUrl}/Payment/MySubscription",
            orderResultUrl: $"{baseUrl}/Payment/EcpayOrderResult",
            customField1: anonId
        );

        ViewBag.Params = parameters;
        ViewBag.PaymentUrl = _ecpay.PaymentUrl;
        ViewBag.IsSandbox = _ecpay.IsSandbox;
        return View("Redirect");
    }

    // ════════════════════════════════════════════════
    //  2. ECPay 伺服器 → 我方伺服器（Notify 回呼）
    //  成功回 "1|OK"，失敗回 "0|ErrorMsg"
    // ════════════════════════════════════════════════
    [HttpPost]
    public async Task<IActionResult> EcpayReturn()
    {
        var form = Request.Form.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());

        // 1. 驗簽
        if (!_ecpay.VerifyCallback(form))
        {
            _logger.LogWarning("[ECPay] Invalid callback signature. Raw={Raw}", string.Join("&", form.Select(k => $"{k.Key}={k.Value}")));
            return Content("0|InvalidSignature");
        }

        // 2. 找訂單
        var orderId = form.GetValueOrDefault("MerchantTradeNo") ?? "";
        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        if (payment == null)
        {
            _logger.LogError("[ECPay] Order not found: {OrderId}", orderId);
            return Content("0|OrderNotFound");
        }

        // 3. Idempotent：已處理過就直接回 OK
        if (payment.Status == "paid")
            return Content("1|OK");

        // 4. 解析付款結果
        var rtnCode = form.GetValueOrDefault("RtnCode") ?? "";
        payment.RawCallbackPayload = string.Join("&", form.Select(k => $"{k.Key}={k.Value}"));
        payment.EcpayTradeNo = form.GetValueOrDefault("TradeNo");
        payment.PaymentMethod = form.GetValueOrDefault("PaymentType") ?? "";

        if (rtnCode == "1") // 1 = 付款成功
        {
            payment.Status = "paid";
            payment.PaidAt = DateTime.UtcNow;
            payment.EffectiveFrom = DateTime.UtcNow;
            payment.EffectiveUntil = payment.ProductType switch
            {
                "teacher_monthly" => DateTime.UtcNow.AddMonths(1),
                "teacher_yearly" => DateTime.UtcNow.AddYears(1),
                _ => DateTime.UtcNow.AddMonths(1),
            };

            // 更新 TeacherSubscription 快取
            await UpdateSubscriptionAsync(payment);

            _logger.LogInformation("[ECPay] Payment success: {OrderId} User={User} Amount={Amount}",
                orderId, payment.UserAnonymousId, payment.Amount);
        }
        else
        {
            payment.Status = "failed";
            _logger.LogWarning("[ECPay] Payment failed: {OrderId} RtnCode={RtnCode} RtnMsg={RtnMsg}",
                orderId, rtnCode, form.GetValueOrDefault("RtnMsg"));
        }

        await _db.SaveChangesAsync();
        // ECPay 規定：成功處理回 "1|OK"
        return Content("1|OK");
    }

    private async Task UpdateSubscriptionAsync(Payment payment)
    {
        var sub = await _db.TeacherSubscriptions
            .FirstOrDefaultAsync(s => s.UserAnonymousId == payment.UserAnonymousId);

        var tier = payment.ProductType.Replace("teacher_", ""); // monthly / yearly

        if (sub == null)
        {
            sub = new TeacherSubscription
            {
                UserAnonymousId = payment.UserAnonymousId,
                Tier = tier,
                ActiveUntil = payment.EffectiveUntil,
                TotalPaidAmount = payment.Amount,
                UpdatedAt = DateTime.UtcNow,
            };
            _db.TeacherSubscriptions.Add(sub);
        }
        else
        {
            // 如果還沒過期，從當前有效期開始接；過期了就從現在開始
            var startFrom = (sub.ActiveUntil.HasValue && sub.ActiveUntil.Value > DateTime.UtcNow)
                ? sub.ActiveUntil.Value
                : DateTime.UtcNow;
            sub.ActiveUntil = payment.ProductType switch
            {
                "teacher_monthly" => startFrom.AddMonths(1),
                "teacher_yearly" => startFrom.AddYears(1),
                _ => startFrom.AddMonths(1),
            };
            sub.Tier = tier;
            sub.TotalPaidAmount += payment.Amount;
            sub.UpdatedAt = DateTime.UtcNow;
        }
    }

    // ════════════════════════════════════════════════
    //  3. 使用者 browser 被導回的 landing 頁
    // ════════════════════════════════════════════════
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> EcpayOrderResult()
    {
        var form = Request.Form.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());
        var orderId = form.GetValueOrDefault("MerchantTradeNo") ?? "";
        var rtnCode = form.GetValueOrDefault("RtnCode") ?? "";

        ViewBag.OrderId = orderId;
        ViewBag.Success = rtnCode == "1";
        ViewBag.Amount = form.GetValueOrDefault("TradeAmt");
        ViewBag.Message = form.GetValueOrDefault("RtnMsg");

        // 注意：不能以這裡看到的結果為最終判斷（可被竄改），真正的狀態以 EcpayReturn 為準
        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        ViewBag.DbStatus = payment?.Status ?? "unknown";

        return View();
    }

    // ════════════════════════════════════════════════
    //  4. 我的訂閱
    // ════════════════════════════════════════════════
    [HttpGet]
    public async Task<IActionResult> MySubscription()
    {
        var anonId = GetAnonId();
        if (string.IsNullOrEmpty(anonId))
            return RedirectToAction("Login", "Account");

        var sub = await _db.TeacherSubscriptions
            .FirstOrDefaultAsync(s => s.UserAnonymousId == anonId);

        var history = await _db.Payments
            .Where(p => p.UserAnonymousId == anonId)
            .OrderByDescending(p => p.CreatedAt)
            .Take(20)
            .ToListAsync();

        ViewBag.Subscription = sub;
        ViewBag.History = history;
        ViewBag.MonthlyPrice = TeacherMonthlyAmount;
        ViewBag.IsSandbox = _ecpay.IsSandbox;
        return View();
    }
}
