namespace DotNetLearning.Models;

// === 程式碼偵探 (Bug Finding) ===
public class BugChallenge
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string BuggyCode { get; set; } = "";
    public string FixedCode { get; set; } = "";
    public string Explanation { get; set; } = "";
    public string Language { get; set; } = "csharp";
    public string Difficulty { get; set; } = "beginner"; // beginner/intermediate/advanced
    public string Category { get; set; } = "";
    public int BugCount { get; set; } = 1; // 有幾個 bug
}

public class BugAttempt
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public int BugChallengeId { get; set; }
    public bool Solved { get; set; }
    public int TimeTakenSeconds { get; set; }
    public DateTime AttemptedAt { get; set; } = DateTime.Now;
}

// === 速度挑戰 (Speed Challenge) ===
public class SpeedRun
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "";
    public int CorrectCount { get; set; }
    public int TotalCount { get; set; }
    public int TimeLimitSeconds { get; set; } = 60;
    public int ScorePoints { get; set; }
    public string Category { get; set; } = "all";
    public DateTime PlayedAt { get; set; } = DateTime.Now;
}

// === 每日一題 (Daily Challenge) ===
public class DailyChallenge
{
    public int Id { get; set; }
    public DateTime ChallengeDate { get; set; }
    public int QuestionId { get; set; }
    public string Category { get; set; } = "";
}

public class DailyAttempt
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public int DailyChallengeId { get; set; }
    public bool IsCorrect { get; set; }
    public string UserAnswer { get; set; } = "";
    public int StreakDays { get; set; }
    public DateTime AnsweredAt { get; set; } = DateTime.Now;
}

// === 程式碼填字遊戲 (Code Fill-in) ===
public class CodePuzzle
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string FullCode { get; set; } = ""; // 完整程式碼
    public string BlankPositionsJson { get; set; } = "[]"; // 挖空位置 [{start,end,answer}]
    public string Language { get; set; } = "csharp";
    public string Difficulty { get; set; } = "beginner";
    public string Category { get; set; } = "";
    public string Hint { get; set; } = "";
}

public class PuzzleAttempt
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public int CodePuzzleId { get; set; }
    public int CorrectBlanks { get; set; }
    public int TotalBlanks { get; set; }
    public int TimeTakenSeconds { get; set; }
    public DateTime AttemptedAt { get; set; } = DateTime.Now;
}

// === 程式碼擂台 (Code Arena) ===
public class ArenaChallenge
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = ""; // 題目描述
    public string Category { get; set; } = "";
    public string Difficulty { get; set; } = "intermediate";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ArenaSubmission
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "";
    public int ArenaChallengeId { get; set; }
    public string Code { get; set; } = "";
    public string Language { get; set; } = "csharp";
    public string Explanation { get; set; } = ""; // 解題思路
    public int Upvotes { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.Now;
}

// === 學習夥伴配對 (Study Buddy) ===
public class StudyBuddy
{
    public int Id { get; set; }
    public string SessionId { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Level { get; set; } = "beginner"; // beginner/intermediate/advanced
    public string InterestsJson { get; set; } = "[]"; // ["csharp","aspnet","database"]
    public string Goal { get; set; } = ""; // 學習目標
    public string ContactInfo { get; set; } = ""; // LINE ID / Discord
    public bool IsLookingForBuddy { get; set; } = true;
    public DateTime RegisteredAt { get; set; } = DateTime.Now;
}

public class BuddyMatch
{
    public int Id { get; set; }
    public int BuddyAId { get; set; }
    public int BuddyBId { get; set; }
    public DateTime MatchedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
}
