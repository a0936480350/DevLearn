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
}
