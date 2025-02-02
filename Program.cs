using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizApp.Data;

public class Program
{
    public static void Main(string[] args)
    {
        // Create builder for configuring the application
        var builder = WebApplication.CreateBuilder(args);

        // Load connection string from appsettings.json
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // Configure EF Core with SQL Server database
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Configure cookie-based authentication
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Set login route
                options.AccessDeniedPath = "/Account/AccessDenied"; // Set access denied route
            });

        // Enable support for MVC controllers and views
        builder.Services.AddControllersWithViews();

        // Build the application
        var app = builder.Build();

        // Configure middleware pipeline based on environment
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // Enable authentication and authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Map default routes
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Quiz}/{action=Index}/{id?}");

        // Run the application
        app.Run();
    }
}