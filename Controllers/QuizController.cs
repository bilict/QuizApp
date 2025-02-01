using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase // Now resolves ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QuizController(ApplicationDbContext context)
    {
        _context = context;
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

        return quiz ?? (ActionResult<Quiz>)NotFound();
    }
}