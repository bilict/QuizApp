using System.Collections.Generic;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ICollection<Question> Questions { get; set; }
}

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<string> Options { get; set; }
    public int CorrectOptionIndex { get; set; }
}

public class UserScore
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int QuizId { get; set; }
    public int Score { get; set; }
}