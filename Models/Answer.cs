using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        [Required]
        public string Text { get; set; }

        // Indicates whether this is the correct answer.
        public bool IsCorrect { get; set; }

        // Foreign key to Question.
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}