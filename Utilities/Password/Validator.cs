using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace QuizApp.Utilities
{
    public static class PasswordValidator
    {
        // Validate: minimum 8 characters, at least one uppercase, one lowercase, one digit and one special character.
        public static bool IsValid(string password)
        {
            if (password == null || password.Length < 8)
                return false;
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
            return regex.IsMatch(password);
        }

        // Simple hash example using SHA256. For production, consider a salted hash with unique salts.
        public static string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}