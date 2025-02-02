using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Data;
using Microsoft.AspNetCore.Components.Authorization; 


var builder = WebApplication.CreateBuilder(args);

// Dodajte ovisnosti
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

//Integracija s backendom
builder.Services.AddScoped(sp => {
    var client = new HttpClient { 
        BaseAddress = new Uri(builder.Configuration["BaseApiUrl"]) 
    };
    return client;
});

builder.Services.AddAuthorizationCore(options => {
    options.AddPolicy("AdminOnly", policy => 
        policy.RequireRole("Admin"));
});

app.Run();