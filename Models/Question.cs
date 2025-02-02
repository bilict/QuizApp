using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        [Required]
        public string Text { get; set; }

        // Navigation property: a question has many answers.
        public ICollection<Answer> Answers { get; set; }

        // Foreign key to Quiz.
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
    }
}