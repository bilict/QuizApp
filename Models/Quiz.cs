using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }

        [Required]
        public string Title { get; set; }

        // One-to-many relationship with Questions.
        public ICollection<Question> Questions { get; set; }
    }
}