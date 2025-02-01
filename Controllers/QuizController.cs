using System.Collections.Generic;
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
    }

    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
    }

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes { get; set; }
    }
}