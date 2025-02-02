using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Utilities;
using System.Threading.Tasks; 

namespace QuizApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            // Check if the username already exists.
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                ModelState.AddModelError("", "Username already exists.");
                return View();
            }

            // Validate strong password (minimum 8 chars, one uppercase, one lowercase, one digit, one special char)
            if (!PasswordValidator.IsValid(password))
            {
                ModelState.AddModelError("", "The password does not meet the strength requirements.");
                return View();
            }

            var user = new User
            {
                Username = username,
                // Store hash (for simplicity using a helper. In production use a robust hashing mechanism like ASP.NET Identity’s PasswordHasher)
                PasswordHash = PasswordValidator.HashPassword(password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null ||
                !PasswordValidator.VerifyPassword(password, user.PasswordHash))
            {
                // Wrong credentials: send to Access Denied view.
                return RedirectToAction("AccessDenied");
            }

            // Prevent unauthorized admin access:
            // Only allow admin login for users whose role is "Admin" if additional admin validation is done.
            // (For now, user supplies credentials and role is stored in DB.)
            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Quiz");
        }

        // GET: /Account/AccessDenied
        public IActionResult AccessDenied()
        {
            ViewBag.Message = "Access denied; please enter the correct username and/or password";
            return View();
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
