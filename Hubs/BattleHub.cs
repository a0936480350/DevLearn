using Microsoft.AspNetCore.SignalR;
using DotNetLearning.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace DotNetLearning.Hubs;

public class BattleHub : Hub
{
    private readonly AppDbContext _db;

    // Static queues for matchmaking: difficulty -> list of waiting connections
    private static readonly ConcurrentDictionary<string, List<WaitingPlayer>> _queues = new();
    // Active battle rooms: roomId -> BattleRoom
    private static readonly ConcurrentDictionary<string, BattleRoom> _rooms = new();

    public BattleHub(AppDbContext db) { _db = db; }

    public async Task JoinQueue(string difficulty, string userId, string userName)
    {
        var queue = _queues.GetOrAdd(difficulty, _ => new List<WaitingPlayer>());

        WaitingPlayer? opponent = null;
        lock (queue)
        {
            // Remove any stale entries for this user
            queue.RemoveAll(p => p.UserId == userId);

            if (queue.Count > 0)
            {
                opponent = queue[0];
                queue.RemoveAt(0);
            }
        }

        if (opponent != null)
        {
            // Match found! Create battle room
            var roomId = Guid.NewGuid().ToString("N")[..8];
            var question = await GetRandomQuestion(difficulty);

            var room = new BattleRoom
            {
                RoomId = roomId,
                Difficulty = difficulty,
                Question = question,
                Player1 = opponent,
                Player2 = new WaitingPlayer { ConnectionId = Context.ConnectionId, UserId = userId, UserName = userName },
                StartedAt = DateTime.Now
            };
            _rooms[roomId] = room;

            await Groups.AddToGroupAsync(opponent.ConnectionId, roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.Group(roomId).SendAsync("BattleStart", new {
                roomId,
                question = new { question.Id, question.Title, question.Description, question.StarterCode, question.SampleInput, question.SampleOutput, question.TimeLimitSeconds },
                player1 = new { opponent.UserId, opponent.UserName },
                player2 = new { userId, userName },
                difficulty
            });
        }
        else
        {
            // Join queue
            lock (queue)
            {
                queue.Add(new WaitingPlayer { ConnectionId = Context.ConnectionId, UserId = userId, UserName = userName });
            }
            await Clients.Caller.SendAsync("Queued", new { difficulty, message = "等待對手中..." });

            // Broadcast online count
            await BroadcastOnlineCount(difficulty);
        }
    }

    public async Task StartAIBattle(string difficulty, string aiLevel, string userId, string userName)
    {
        var question = await GetRandomQuestion(difficulty);
        var roomId = "AI_" + Guid.NewGuid().ToString("N")[..8];

        // Calculate AI response time based on level
        int aiMinSeconds, aiMaxSeconds, aiAccuracy;
        switch (aiLevel)
        {
            case "easy":   aiMinSeconds = 60; aiMaxSeconds = 90; aiAccuracy = 65; break;
            case "medium": aiMinSeconds = 30; aiMaxSeconds = 55; aiAccuracy = 82; break;
            case "hard":   aiMinSeconds = 8;  aiMaxSeconds = 22; aiAccuracy = 96; break;
            default:       aiMinSeconds = 60; aiMaxSeconds = 90; aiAccuracy = 65; break;
        }

        string aiName = aiLevel switch { "easy" => "初等 AI", "medium" => "中等 AI", "hard" => "高等 AI", _ => "AI" };

        var room = new BattleRoom
        {
            RoomId = roomId,
            Difficulty = difficulty,
            Question = question,
            Player1 = new WaitingPlayer { ConnectionId = Context.ConnectionId, UserId = userId, UserName = userName },
            Player2 = new WaitingPlayer { ConnectionId = "AI", UserId = $"AI_{aiLevel.ToUpper()}", UserName = aiName },
            StartedAt = DateTime.Now,
            IsAIBattle = true,
            AILevel = aiLevel,
            AITimeSeconds = new Random().Next(aiMinSeconds, aiMaxSeconds + 1),
            AIAccuracy = aiAccuracy
        };
        _rooms[roomId] = room;

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        await Clients.Caller.SendAsync("BattleStart", new {
            roomId,
            question = new { question.Id, question.Title, question.Description, question.StarterCode, question.SampleInput, question.SampleOutput, question.TimeLimitSeconds },
            player1 = new { userId, userName },
            player2 = new { UserId = $"AI_{aiLevel.ToUpper()}", UserName = aiName },
            difficulty,
            isAI = true
        });
    }

    public async Task SubmitCode(string roomId, string code, string userId)
    {
        if (!_rooms.TryGetValue(roomId, out var room)) return;

        var elapsed = (int)(DateTime.Now - room.StartedAt).TotalSeconds;
        var accuracy = ValidateCode(code, room.Question);

        bool isPlayer1 = room.Player1.UserId == userId;

        if (isPlayer1 && !room.Player1Submitted)
        {
            room.Player1Code = code;
            room.Player1TimeSeconds = elapsed;
            room.Player1Accuracy = accuracy;
            room.Player1Submitted = true;
        }
        else if (!isPlayer1 && !room.Player2Submitted)
        {
            room.Player2Code = code;
            room.Player2TimeSeconds = elapsed;
            room.Player2Accuracy = accuracy;
            room.Player2Submitted = true;
        }

        // Notify opponent of submission
        await Clients.Group(roomId).SendAsync("PlayerSubmitted", new { userId, elapsed, accuracy });

        // Check if both submitted or AI battle
        bool bothDone = room.Player1Submitted && room.Player2Submitted;

        if (room.IsAIBattle && room.Player1Submitted && !room.Player2Submitted)
        {
            // AI submits after a short delay (1-3 seconds), capped to avoid timeout.
            // We await inline to keep the SignalR Hub context alive for broadcasting.
            int aiDelay = Math.Max(1000, Math.Min(room.AITimeSeconds * 100, 3000));
            await Task.Delay(aiDelay);
            room.Player2TimeSeconds = room.AITimeSeconds;
            room.Player2Accuracy = room.AIAccuracy;
            room.Player2Submitted = true;
            await FinalizeBattle(room);
        }
        else if (bothDone)
        {
            await FinalizeBattle(room);
        }
    }

    public async Task LeaveQueue(string difficulty, string userId)
    {
        if (_queues.TryGetValue(difficulty, out var queue))
        {
            lock (queue) { queue.RemoveAll(p => p.UserId == userId); }
        }
        await Clients.Caller.SendAsync("LeftQueue");
    }

    public async Task GetOnlineCount(string difficulty)
    {
        int count = _queues.TryGetValue(difficulty, out var q) ? q.Count : 0;
        await Clients.Caller.SendAsync("OnlineCount", new { difficulty, count });
    }

    private async Task FinalizeBattle(BattleRoom room)
    {
        // Calculate scores: accuracy * (1 + speed_bonus)
        double p1Score = room.Player1Accuracy * (1.0 + Math.Max(0, room.Question.TimeLimitSeconds - room.Player1TimeSeconds) / (double)room.Question.TimeLimitSeconds);
        double p2Score = room.Player2Accuracy * (1.0 + Math.Max(0, room.Question.TimeLimitSeconds - room.Player2TimeSeconds) / (double)room.Question.TimeLimitSeconds);

        string winnerId = p1Score >= p2Score ? room.Player1.UserId : room.Player2.UserId;
        string winnerName = winnerId == room.Player1.UserId ? room.Player1.UserName : room.Player2.UserName;

        // Save to DB
        var record = new BattleRecord
        {
            Player1Id = room.Player1.UserId,
            Player1Name = room.Player1.UserName,
            Player2Id = room.Player2.UserId,
            Player2Name = room.Player2.UserName,
            WinnerId = winnerId,
            Difficulty = room.Difficulty,
            QuestionId = room.Question.Id,
            QuestionTitle = room.Question.Title,
            Player1TimeSeconds = room.Player1TimeSeconds,
            Player2TimeSeconds = room.Player2TimeSeconds,
            Player1Accuracy = room.Player1Accuracy,
            Player2Accuracy = room.Player2Accuracy,
            IsAIMatch = room.IsAIBattle,
            AILevel = room.AILevel,
            StartedAt = room.StartedAt,
            EndedAt = DateTime.Now
        };
        _db.BattleRecords.Add(record);

        // Update stats for real players
        await UpdateStats(room.Player1.UserId, room.Player1.UserName, room.Difficulty, winnerId == room.Player1.UserId);
        if (!room.IsAIBattle)
            await UpdateStats(room.Player2.UserId, room.Player2.UserName, room.Difficulty, winnerId == room.Player2.UserId);

        await _db.SaveChangesAsync();

        // Notify result
        await Clients.Group(room.RoomId).SendAsync("BattleEnd", new {
            winnerId, winnerName,
            player1 = new { room.Player1.UserId, room.Player1.UserName, score = (int)p1Score, room.Player1TimeSeconds, room.Player1Accuracy },
            player2 = new { room.Player2.UserId, room.Player2.UserName, score = (int)p2Score, room.Player2TimeSeconds, room.Player2Accuracy },
            recordId = record.Id
        });

        _rooms.TryRemove(room.RoomId, out _);
    }

    private async Task UpdateStats(string userId, string userName, string difficulty, bool won)
    {
        var stat = await _db.BattleStats.FirstOrDefaultAsync(s => s.UserId == userId);
        if (stat == null)
        {
            stat = new BattleStat { UserId = userId, UserName = userName };
            _db.BattleStats.Add(stat);
        }
        stat.UserName = userName;
        stat.UpdatedAt = DateTime.Now;
        if (won) { if (difficulty == "beginner") stat.BeginnerWins++; else if (difficulty == "intermediate") stat.IntermediateWins++; else stat.AdvancedWins++; }
        else { if (difficulty == "beginner") stat.BeginnerLosses++; else if (difficulty == "intermediate") stat.IntermediateLosses++; else stat.AdvancedLosses++; }
    }

    private static int ValidateCode(string code, BattleQuestion question)
    {
        if (string.IsNullOrWhiteSpace(code)) return 0;
        var keywords = question.ValidationKeywords.Split(',', StringSplitOptions.RemoveEmptyEntries);
        if (keywords.Length == 0) return code.Length > 20 ? 70 : 30;
        int matched = keywords.Count(k => code.Contains(k.Trim(), StringComparison.OrdinalIgnoreCase));
        return (int)(matched * 100.0 / keywords.Length);
    }

    private async Task<BattleQuestion> GetRandomQuestion(string difficulty)
    {
        var questions = await _db.BattleQuestions.Where(q => q.Difficulty == difficulty).ToListAsync();
        if (!questions.Any()) return new BattleQuestion { Title = "Hello World", Description = "寫一個 Hello World 程式", StarterCode = "Console.Write(", ValidationKeywords = "Console.Write,Hello", TimeLimitSeconds = 60 };
        return questions[new Random().Next(questions.Count)];
    }

    private async Task BroadcastOnlineCount(string difficulty)
    {
        int count = _queues.TryGetValue(difficulty, out var q) ? q.Count : 0;
        await Clients.All.SendAsync("OnlineCount", new { difficulty, count });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        foreach (var queue in _queues.Values)
            lock (queue) { queue.RemoveAll(p => p.ConnectionId == Context.ConnectionId); }
        await base.OnDisconnectedAsync(exception);
    }
}

public class WaitingPlayer
{
    public string ConnectionId { get; set; } = "";
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
}

public class BattleRoom
{
    public string RoomId { get; set; } = "";
    public string Difficulty { get; set; } = "";
    public BattleQuestion Question { get; set; } = new();
    public WaitingPlayer Player1 { get; set; } = new();
    public WaitingPlayer Player2 { get; set; } = new();
    public DateTime StartedAt { get; set; }
    public bool IsAIBattle { get; set; }
    public string AILevel { get; set; } = "";
    public int AITimeSeconds { get; set; }
    public int AIAccuracy { get; set; }
    public string Player1Code { get; set; } = "";
    public string Player2Code { get; set; } = "";
    public int Player1TimeSeconds { get; set; }
    public int Player2TimeSeconds { get; set; }
    public int Player1Accuracy { get; set; }
    public int Player2Accuracy { get; set; }
    public bool Player1Submitted { get; set; }
    public bool Player2Submitted { get; set; }
}
