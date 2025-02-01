[ApiController]
[Route("api/quizzes")]
public class QuizController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QuizController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateQuiz([FromBody] Quiz quiz)
    {
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
        return Ok(quiz);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetQuizzes()
    {
        var quizzes = await _context.Quizzes.Include(q => q.Questions).ToListAsync();
        return Ok(quizzes);
    }

    [HttpPost("{id}/submit")]
    [Authorize]
    public async Task<IActionResult> SubmitQuiz(int id, [FromBody] List<int> answers)
    {
        // Calculate score logic here...
        return Ok(new { Score = calculatedScore });
    }
}