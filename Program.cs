using DbContexts;
using SAT242516081.Data;
using ITHelpDesk.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Providers;
using SAT242516081.Components;
using System.Globalization;
using UnitOfWorks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- EKLENEN KISIM 1: Controller Servisi ---
builder.Services.AddControllers();
// ------------------------------------------

builder.Services.AddCascadingAuthenticationState();

// Identity DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User", "Admin"));
});

// MyDbModel DbContext
builder.Services.AddDbContext<MyDbModel_DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// UnitOfWork & Provider
builder.Services.AddScoped<IMyDbModel_UnitOfWork, MyDbModel_UnitOfWork<MyDbModel_DbContext>>();
builder.Services.AddScoped<IMyDbModel_Provider, MyDbModel_Provider>();

// Services
builder.Services.AddScoped<IDbLogger, DbLogger>();
builder.Services.AddScoped<IFileLogger, FileLogger>();
builder.Services.AddScoped<IReportService, ReportService>();

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

// Localization middleware
var supportedCultures = new[] { new CultureInfo("tr"), new CultureInfo("en") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

// --- EKLENEN KISIM 2: Controller Rotalarý ---
app.MapControllers();
// -------------------------------------------

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Rolleri otomatik oluþturma kodu (önceki adýmda eklemiþtik, aynen kalýyor)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "User", "Technician" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();