using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Controllers;

public class MessageController : Controller
{
    private readonly AppDbContext _db;
    public MessageController(AppDbContext db) => _db = db;
    private string GetAnonId() => HttpContext.Session.GetString("SessionId") ?? "";

    // 我的所有對話列表（按老師分組）
    [HttpGet]
    public async Task<IActionResult> Conversations()
    {
        var myId = GetAnonId();
        var messages = await _db.PrivateMessages
            .Where(m => m.SenderId == myId || m.ReceiverId == myId)
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();

        // Group by the other person
        var conversations = messages
            .GroupBy(m => m.SenderId == myId ? m.ReceiverId : m.SenderId)
            .Select(g => new {
                OtherId = g.Key,
                OtherName = g.First().SenderId == myId ? g.First().ReceiverName : g.First().SenderName,
                TeacherId = g.First().TeacherId,
                LastMessage = g.First().Message,
                LastTime = g.First().SentAt,
                UnreadCount = g.Count(m => m.ReceiverId == myId && !m.IsRead)
            }).ToList();

        return Json(conversations);
    }

    // 取得與某人的訊息
    [HttpGet]
    public async Task<IActionResult> GetMessages(string otherId, int take = 50)
    {
        var myId = GetAnonId();
        var messages = await _db.PrivateMessages
            .Where(m => (m.SenderId == myId && m.ReceiverId == otherId) ||
                       (m.SenderId == otherId && m.ReceiverId == myId))
            .OrderByDescending(m => m.SentAt)
            .Take(take)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        // Mark as read
        var unread = messages.Where(m => m.ReceiverId == myId && !m.IsRead).ToList();
        unread.ForEach(m => m.IsRead = true);
        if (unread.Any()) await _db.SaveChangesAsync();

        return Json(messages.Select(m => new {
            m.Id, m.SenderId, m.SenderName, m.ReceiverId, m.Message, m.SentAt,
            isMine = m.SenderId == myId
        }));
    }

    // 發送私訊
    [HttpPost]
    public async Task<IActionResult> Send([FromBody] SendMsgReq req)
    {
        var myId = GetAnonId();
        var me = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == myId);
        if (me == null || !me.IsRegistered)
            return Ok(new { success = false, message = "請先註冊才能發送私訊" });

        var receiver = await _db.SiteUsers.FirstOrDefaultAsync(u => u.AnonymousId == req.ReceiverId);

        var msg = new PrivateMessage
        {
            SenderId = myId,
            SenderName = me.Nickname,
            ReceiverId = req.ReceiverId,
            ReceiverName = receiver?.Nickname ?? "未知",
            TeacherId = req.TeacherId,
            Message = req.Message
        };
        _db.PrivateMessages.Add(msg);
        await _db.SaveChangesAsync();

        return Ok(new { success = true, id = msg.Id });
    }

    // 未讀訊息數
    [HttpGet]
    public async Task<IActionResult> UnreadCount()
    {
        var myId = GetAnonId();
        var count = await _db.PrivateMessages.CountAsync(m => m.ReceiverId == myId && !m.IsRead);
        return Json(new { count });
    }
}

public record SendMsgReq(string ReceiverId, int? TeacherId, string Message);
