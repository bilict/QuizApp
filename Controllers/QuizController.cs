using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Quiz>>> GetQuizzes()
        {
            var quizzes = await _context.Quizzes.Include(q => q.Questions).ToListAsync();
            return Ok(quizzes);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Quiz>> CreateQuiz([FromBody] Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuiz), new { id = quiz.Id }, quiz);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Quiz>> GetQuiz(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == id);

            return quiz != null ? Ok(quiz) : NotFound();
        }

        [HttpPost("{id}/submit")]
        [Authorize]
        public async Task<IActionResult> SubmitQuiz(int id, [FromBody] List<int> answers)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null) return NotFound();

            int score = quiz.Questions
                .Zip(answers, (q, a) => q.CorrectOptionIndex == a)
                .Count(correct => correct);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.UserScores.Add(new UserScore
            {
                UserId = userId,
                QuizId = id,
                Score = score
            });

            await _context.SaveChangesAsync();
            return Ok(new { Score = score, Total = quiz.Questions.Count });
        }
    }

    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public int CorrectOptionIndex { get; set; } // Added this to compare answers
    }

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<UserScore> UserScores { get; set; }  // Ensure this is added

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }

    public class UserScore
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
    }
}
