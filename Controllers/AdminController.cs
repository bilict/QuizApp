using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard view for admin users.
        public async Task<IActionResult> Dashboard()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}