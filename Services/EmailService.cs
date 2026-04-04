using System.Net;
using System.Net.Mail;

namespace DotNetLearning.Services;

public class EmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _user;
    private readonly string _pass;
    private readonly string _from;
    private readonly bool _configured;

    public EmailService()
    {
        _host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "";
        _port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var p) ? p : 587;
        _user = Environment.GetEnvironmentVariable("SMTP_USER") ?? "";
        _pass = Environment.GetEnvironmentVariable("SMTP_PASS") ?? "";
        _from = Environment.GetEnvironmentVariable("SMTP_FROM") ?? "noreply@devlearn.dev";
        _configured = !string.IsNullOrEmpty(_host) && !string.IsNullOrEmpty(_user);
    }

    public async Task<bool> SendVerificationEmailAsync(string toEmail, string token, string baseUrl)
    {
        if (!_configured) { Console.WriteLine("[Email] SMTP not configured, skip sending."); return false; }
        var link = $"{baseUrl}/Account/VerifyEmail?token={token}";
        var subject = "DevLearn - 驗證你的 Email";
        var body = $@"
<div style='font-family:sans-serif;max-width:500px;margin:0 auto;padding:20px;'>
    <h2 style='color:#00b894;'>DevLearn Email 驗證</h2>
    <p>感謝你註冊 DevLearn！請點擊下方按鈕驗證你的 Email：</p>
    <a href='{link}' style='display:inline-block;background:#3b82f6;color:#fff;padding:12px 24px;border-radius:8px;text-decoration:none;font-weight:bold;margin:16px 0;'>驗證 Email</a>
    <p style='color:#999;font-size:13px;'>此連結將在 1 分鐘內失效。如果不是你本人操作，請忽略此信。</p>
    <p style='color:#999;font-size:12px;'>連結：{link}</p>
</div>";
        return await SendAsync(toEmail, subject, body);
    }

    public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string token, string baseUrl)
    {
        if (!_configured) { Console.WriteLine("[Email] SMTP not configured, skip sending."); return false; }
        var link = $"{baseUrl}/Account/ResetPassword?token={token}";
        var subject = "DevLearn - 重設密碼";
        var body = $@"
<div style='font-family:sans-serif;max-width:500px;margin:0 auto;padding:20px;'>
    <h2 style='color:#f59e0b;'>DevLearn 密碼重設</h2>
    <p>你收到此信是因為有人請求重設密碼。如果是你本人，請點擊下方按鈕：</p>
    <a href='{link}' style='display:inline-block;background:#ef4444;color:#fff;padding:12px 24px;border-radius:8px;text-decoration:none;font-weight:bold;margin:16px 0;'>重設密碼</a>
    <p style='color:#999;font-size:13px;'>此連結將在 1 分鐘內失效。如果不是你操作，請忽略此信。</p>
</div>";
        return await SendAsync(toEmail, subject, body);
    }

    private async Task<bool> SendAsync(string to, string subject, string htmlBody)
    {
        try
        {
            using var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_user, _pass),
                EnableSsl = true
            };
            var msg = new MailMessage(_from, to, subject, htmlBody) { IsBodyHtml = true };
            await client.SendMailAsync(msg);
            Console.WriteLine($"[Email] Sent to {to}: {subject}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Email] Failed to send to {to}: {ex.Message}");
            return false;
        }
    }
}
