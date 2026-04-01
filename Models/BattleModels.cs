using System.ComponentModel.DataAnnotations;

public class BattleQuestion
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string StarterCode { get; set; } = "";
    public string SampleInput { get; set; } = "";
    public string SampleOutput { get; set; } = "";
    public string ValidationKeywords { get; set; } = ""; // comma-separated required keywords
    public string Difficulty { get; set; } = "beginner"; // beginner, intermediate, advanced
    public int TimeLimitSeconds { get; set; } = 120;
    public string Tags { get; set; } = "";
}

public class BattleRecord
{
    public int Id { get; set; }
    public string Player1Id { get; set; } = "";
    public string Player1Name { get; set; } = "";
    public string Player2Id { get; set; } = ""; // "AI_EASY", "AI_MEDIUM", "AI_HARD" for AI
    public string Player2Name { get; set; } = "";
    public string WinnerId { get; set; } = "";
    public string Difficulty { get; set; } = "beginner";
    public int QuestionId { get; set; }
    public string QuestionTitle { get; set; } = "";
    public int Player1TimeSeconds { get; set; }
    public int Player2TimeSeconds { get; set; }
    public int Player1Accuracy { get; set; } // 0-100
    public int Player2Accuracy { get; set; }
    public bool IsAIMatch { get; set; }
    public string AILevel { get; set; } = "";
    public DateTime StartedAt { get; set; } = DateTime.Now;
    public DateTime? EndedAt { get; set; }
}

public class BattleStat
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public int BeginnerWins { get; set; }
    public int IntermediateWins { get; set; }
    public int AdvancedWins { get; set; }
    public int BeginnerLosses { get; set; }
    public int IntermediateLosses { get; set; }
    public int AdvancedLosses { get; set; }
    public int TotalWins => BeginnerWins + IntermediateWins + AdvancedWins;
    public int TotalLosses => BeginnerLosses + IntermediateLosses + AdvancedLosses;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
