using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        // Store the hashed password.
        [Required]
        public string PasswordHash { get; set; }

        // "User" or "Admin" (default is "User")
        public string Role { get; set; } = "User";
    }
}