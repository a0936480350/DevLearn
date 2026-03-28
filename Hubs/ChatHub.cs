using Microsoft.AspNetCore.SignalR;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Hubs;

public class ChatHub : Hub
{
    private readonly AppDbContext _db;
    private static int _onlineCount = 0;

    public ChatHub(AppDbContext db) { _db = db; }

    public async Task SendMessage(string nickname, string message, string emoji)
    {
        if (string.IsNullOrWhiteSpace(message) || message.Length > 500) return;

        var chatMsg = new ChatMessage
        {
            SessionId = Context.ConnectionId,
            Nickname = string.IsNullOrWhiteSpace(nickname) ? "\u533F\u540D" : nickname.Trim(),
            Message = message.Trim(),
            AvatarEmoji = string.IsNullOrWhiteSpace(emoji) ? "\U0001F600" : emoji
        };
        _db.ChatMessages.Add(chatMsg);
        await _db.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveMessage", chatMsg.Nickname, chatMsg.Message, chatMsg.AvatarEmoji, chatMsg.SentAt.ToString("HH:mm"));
    }

    public async Task GetRecentMessages()
    {
        var messages = _db.ChatMessages
            .OrderByDescending(m => m.SentAt)
            .Take(50)
            .OrderBy(m => m.SentAt)
            .Select(m => new { m.Nickname, m.Message, m.AvatarEmoji, Time = m.SentAt.ToString("HH:mm") })
            .ToList();

        await Clients.Caller.SendAsync("LoadHistory", messages);
    }

    public override async Task OnConnectedAsync()
    {
        Interlocked.Increment(ref _onlineCount);
        await Clients.All.SendAsync("UserCount", _onlineCount);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        Interlocked.Decrement(ref _onlineCount);
        await Clients.All.SendAsync("UserCount", _onlineCount);
        await base.OnDisconnectedAsync(ex);
    }
}
