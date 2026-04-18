using Microsoft.EntityFrameworkCore;
using DotNetLearning.Models;

namespace DotNetLearning.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Chapter> Chapters => Set<Chapter>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Progress> Progresses => Set<Progress>();
    public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<Idea> Ideas => Set<Idea>();
    public DbSet<Reply> Replies => Set<Reply>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<StudyLog> StudyLogs => Set<StudyLog>();
    public DbSet<CodeSnippet> CodeSnippets => Set<CodeSnippet>();
    public DbSet<Flashcard> Flashcards => Set<Flashcard>();
    public DbSet<QnA> QnAs => Set<QnA>();
    public DbSet<QnAAnswer> QnAAnswers => Set<QnAAnswer>();
    public DbSet<SiteUser> SiteUsers => Set<SiteUser>();

    // 遊戲功能
    public DbSet<BugChallenge> BugChallenges => Set<BugChallenge>();
    public DbSet<BugAttempt> BugAttempts => Set<BugAttempt>();
    public DbSet<SpeedRun> SpeedRuns => Set<SpeedRun>();
    public DbSet<DailyChallenge> DailyChallenges => Set<DailyChallenge>();
    public DbSet<DailyAttempt> DailyAttempts => Set<DailyAttempt>();
    public DbSet<CodePuzzle> CodePuzzles => Set<CodePuzzle>();
    public DbSet<PuzzleAttempt> PuzzleAttempts => Set<PuzzleAttempt>();
    public DbSet<ArenaChallenge> ArenaChallenges => Set<ArenaChallenge>();
    public DbSet<ArenaSubmission> ArenaSubmissions => Set<ArenaSubmission>();
    public DbSet<StudyBuddy> StudyBuddies => Set<StudyBuddy>();
    public DbSet<BuddyMatch> BuddyMatches => Set<BuddyMatch>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<ChatReaction> ChatReactions => Set<ChatReaction>();

    // 老師配對系統
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<TeacherSlot> TeacherSlots => Set<TeacherSlot>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<TeacherPost> TeacherPosts => Set<TeacherPost>();
    public DbSet<TeacherPostComment> TeacherPostComments => Set<TeacherPostComment>();
    public DbSet<FavoriteTeacher> FavoriteTeachers => Set<FavoriteTeacher>();
    public DbSet<PrivateMessage> PrivateMessages => Set<PrivateMessage>();

    // 客服工單
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

    // 錯誤日誌 + AI 工作紀錄
    public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>();
    public DbSet<AIWorkLog> AIWorkLogs => Set<AIWorkLog>();

    // 對戰系統
    public DbSet<BattleQuestion> BattleQuestions { get; set; }
    public DbSet<BattleRecord> BattleRecords { get; set; }
    public DbSet<BattleStat> BattleStats { get; set; }

    // 公告欄
    public DbSet<Announcement> Announcements => Set<Announcement>();

    // Claude 任務佇列
    public DbSet<ClaudeTask> ClaudeTasks => Set<ClaudeTask>();

    // 金流（ECPay 老師 Premium 訂閱）
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<TeacherSubscription> TeacherSubscriptions => Set<TeacherSubscription>();

    // 檔案分享（筆記、PPT、PDF）
    public DbSet<SharedFile> SharedFiles => Set<SharedFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Allow explicit Id values for seeded data (disable SERIAL auto-generation)
        modelBuilder.Entity<Chapter>().Property(c => c.Id).ValueGeneratedNever();
        modelBuilder.Entity<Question>().Property(q => q.Id).ValueGeneratedNever();
    }
}
