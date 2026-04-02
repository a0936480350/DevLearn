using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using DotNetLearning.Data;
using DotNetLearning.Models;

namespace DotNetLearning.Hubs;

public class ChatHub : Hub
{
    private readonly AppDbContext _db;
    private static int _onlineCount = 0;

    public ChatHub(AppDbContext db) { _db = db; }

    public async Task SendMessage(string nickname, string message, string emoji, int? replyToId = null)
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

        if (replyToId.HasValue)
        {
            var replyTo = await _db.ChatMessages.FindAsync(replyToId.Value);
            if (replyTo != null)
            {
                chatMsg.ReplyToId = replyTo.Id;
                chatMsg.ReplyToNickname = replyTo.Nickname;
                chatMsg.ReplyToPreview = replyTo.Message.Length > 30
                    ? replyTo.Message.Substring(0, 30) + "..."
                    : replyTo.Message;
            }
        }

        await _db.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveMessage",
            chatMsg.Id, chatMsg.Nickname, chatMsg.Message, chatMsg.AvatarEmoji,
            chatMsg.SentAt.ToString("HH:mm"),
            chatMsg.ReplyToId, chatMsg.ReplyToNickname, chatMsg.ReplyToPreview);
    }

    public async Task GetRecentMessages()
    {
        var messages = await _db.ChatMessages
            .OrderByDescending(m => m.SentAt)
            .Take(50)
            .OrderBy(m => m.SentAt)
            .Select(m => new { m.Id, m.Nickname, m.Message, m.AvatarEmoji, m.SentAt, m.ReplyToId, m.ReplyToNickname, m.ReplyToPreview })
            .ToListAsync();

        var msgIds = messages.Select(m => m.Id).ToList();
        var reactions = await _db.ChatReactions
            .Where(r => msgIds.Contains(r.ChatMessageId))
            .GroupBy(r => new { r.ChatMessageId, r.Emoji })
            .Select(g => new { g.Key.ChatMessageId, g.Key.Emoji, Count = g.Count(), Users = g.Select(x => x.Nickname).ToList() })
            .ToListAsync();

        var reactionMap = reactions
            .GroupBy(r => r.ChatMessageId)
            .ToDictionary(g => g.Key, g => g.Select(r => new { r.Emoji, r.Count, r.Users }).ToList());

        var result = messages.Select(m => new {
            m.Id, m.Nickname, m.Message, avatarEmoji = m.AvatarEmoji,
            time = m.SentAt.ToString("HH:mm"),
            m.ReplyToId, m.ReplyToNickname, m.ReplyToPreview,
            reactions = reactionMap.ContainsKey(m.Id) ? reactionMap[m.Id] : null
        }).ToList();

        await Clients.Caller.SendAsync("LoadHistory", result);
    }

    public async Task AddReaction(int messageId, string emoji, string nickname, string sessionId)
    {
        var existing = await _db.ChatReactions
            .FirstOrDefaultAsync(r => r.ChatMessageId == messageId && r.SessionId == sessionId && r.Emoji == emoji);

        if (existing != null)
        {
            _db.ChatReactions.Remove(existing);
        }
        else
        {
            _db.ChatReactions.Add(new ChatReaction
            {
                ChatMessageId = messageId,
                SessionId = sessionId,
                Nickname = nickname,
                Emoji = emoji,
                ReactedAt = DateTime.Now
            });
        }
        await _db.SaveChangesAsync();

        // Broadcast updated reactions for this message
        var updated = await _db.ChatReactions
            .Where(r => r.ChatMessageId == messageId)
            .GroupBy(r => r.Emoji)
            .Select(g => new { Emoji = g.Key, Count = g.Count(), Users = g.Select(x => x.Nickname).ToList() })
            .ToListAsync();

        await Clients.All.SendAsync("UpdateReactions", messageId, updated);
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
