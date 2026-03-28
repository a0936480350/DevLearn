namespace DotNetLearning.Models;

public class Question
{
    public int Id { get; set; }
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; } = null!;
    public string QuestionText { get; set; } = "";
    public string Type { get; set; } = "multiple"; // multiple, truefalse, code
    public string OptionsJson { get; set; } = "[]"; // JSON array of options
    public string CorrectAnswer { get; set; } = "";
    public string Explanation { get; set; } = "";
    public int Difficulty { get; set; } = 1; // 1=easy, 2=medium, 3=hard
}
