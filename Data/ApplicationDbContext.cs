using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enforce unique Username using a unique index.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Seed a Quiz with 10 anatomy questions.
            modelBuilder.Entity<Quiz>().HasData(
                new Quiz 
                { 
                    QuizId = 1, 
                    Title = "Anatomy Quiz" 
                }
            );

            // Seed Questions with explicit PK and FK values.
            modelBuilder.Entity<Question>().HasData(
                new Question { QuestionId = 1, QuizId = 1, Text = "What is the largest organ in the human body?" },
                new Question { QuestionId = 2, QuizId = 1, Text = "How many bones are in the adult human skeleton?" },
                new Question { QuestionId = 3, QuizId = 1, Text = "Which part of the brain is responsible for balance?" },
                new Question { QuestionId = 4, QuizId = 1, Text = "What is the main function of the respiratory system?" },
                new Question { QuestionId = 5, QuizId = 1, Text = "Which blood cells are responsible for fighting infection?" },
                new Question { QuestionId = 6, QuizId = 1, Text = "What is the name of the muscle that separates the chest from the abdomen?" },
                new Question { QuestionId = 7, QuizId = 1, Text = "What vessel carries oxygenated blood from the heart to the body?" },
                new Question { QuestionId = 8, QuizId = 1, Text = "Which organ produces insulin?" },
                new Question { QuestionId = 9, QuizId = 1, Text = "What is the smallest bone in the human body?" },
                new Question { QuestionId = 10, QuizId = 1, Text = "What is the function of the kidneys?" }
            );

            // Seed Answers with explicit keys and corresponding QuestionId for the FK.
            modelBuilder.Entity<Answer>().HasData(
                new Answer { AnswerId = 1, QuestionId = 1, Text = "Skin", IsCorrect = true },
                new Answer { AnswerId = 2, QuestionId = 1, Text = "Liver", IsCorrect = false },
                new Answer { AnswerId = 3, QuestionId = 2, Text = "206", IsCorrect = true },
                new Answer { AnswerId = 4, QuestionId = 2, Text = "305", IsCorrect = false },
                new Answer { AnswerId = 5, QuestionId = 3, Text = "Cerebellum", IsCorrect = true },
                new Answer { AnswerId = 6, QuestionId = 3, Text = "Medulla", IsCorrect = false },
                new Answer { AnswerId = 7, QuestionId = 4, Text = "Gas exchange", IsCorrect = true },
                new Answer { AnswerId = 8, QuestionId = 5, Text = "White blood cells", IsCorrect = true },
                new Answer { AnswerId = 9, QuestionId = 6, Text = "Diaphragm", IsCorrect = true },
                new Answer { AnswerId = 10, QuestionId = 7, Text = "Aorta", IsCorrect = true },
                new Answer { AnswerId = 11, QuestionId = 8, Text = "Pancreas", IsCorrect = true },
                new Answer { AnswerId = 12, QuestionId = 9, Text = "Stapes", IsCorrect = true },
                new Answer { AnswerId = 13, QuestionId = 10, Text = "Filter waste", IsCorrect = true }
            );
        }
    }
}

