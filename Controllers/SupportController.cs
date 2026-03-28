using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class SupportController : Controller
{
    private readonly AppDbContext _db;
    public SupportController(AppDbContext db) => _db = db;
    private string GetAnonId() => HttpContext.Session.GetString("SessionId") ?? "";

    // 提交工單
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitTicketReq req)
    {
        var userId = GetAnonId();
        var user = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == userId);

        var ticket = new SupportTicket
        {
            UserId = userId,
            UserName = req.UserName ?? user?.Nickname ?? "訪客",
            UserEmail = req.UserEmail ?? user?.Email ?? "",
            Category = req.Category ?? "其他",
            Content = req.Content ?? ""
        };
        _db.SupportTickets.Add(ticket);
        await _db.SaveChangesAsync();

        // 「網站問題回報」自動建 ErrorLog，觸發 AI 自動修復排程
        if (ticket.Category == "網站問題回報")
        {
            _db.ErrorLogs.Add(new ErrorLog
            {
                PageUrl = "/Support/Ticket#" + ticket.Id,
                ErrorMessage = $"[用戶回報] {ticket.UserName}: {ticket.Content}",
                StackTrace = "",
                Source = "user-report",
                UserId = userId,
                UserAgent = Request.Headers.UserAgent.ToString(),
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? ""
            });
            await _db.SaveChangesAsync();
        }

        return Ok(new { success = true, id = ticket.Id });
    }

    // 我的工單
    [HttpGet]
    public async Task<IActionResult> MyTickets()
    {
        var userId = GetAnonId();
        var tickets = await _db.SupportTickets
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .Take(20)
            .ToListAsync();
        return Json(tickets);
    }

    // 未讀回覆數
    [HttpGet]
    public async Task<IActionResult> UnreadReplies()
    {
        var userId = GetAnonId();
        var count = await _db.SupportTickets.CountAsync(t =>
            t.UserId == userId && t.AdminReply != "" && t.Status == "resolved" && !t.IsReplyRead);
        return Json(new { count });
    }

    // 標記工單回覆為已讀
    [HttpPost]
    public async Task<IActionResult> MarkReplyRead([FromBody] MarkReadReq req)
    {
        var userId = GetAnonId();
        var ticket = await _db.SupportTickets.FirstOrDefaultAsync(t => t.Id == req.Id && t.UserId == userId);
        if (ticket == null) return NotFound();
        ticket.IsReplyRead = true;
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }
}

public record SubmitTicketReq(string? UserName, string? UserEmail, string? Category, string Content);
public record MarkReadReq(int Id);
