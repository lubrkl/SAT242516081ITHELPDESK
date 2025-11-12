using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAT242516081ITHELPDESK.Components;
using SAT242516081ITHELPDESK.Components.Account;
using SAT242516081ITHELPDESK.Data;
using SAT242516081ITHELPDESK.Models.DbContexts; // Düzeltme: Yeni DbContext konumu
using SAT242516081ITHELPDESK.Models.Providers;
using SAT242516081ITHELPDESK.Models.UnitOfWorks;
using SAT242516081ITHELPDESK.Models.MyDbModels; // Düzeltme: Modellerin yeni konumu

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// --- YÖNERGEDEKÝ 4 SERVÝSÝN KAYDI ---

// 1. DbContext (ve 2. MyDbModel)
// DÜZELTME: ApplicationDbContext -> MyDbModel_DbContext olarak deðiþtirildi
builder.Services.AddDbContext<MyDbModel_DbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 3. Provider (HelpdeskService) 
builder.Services.AddScoped<MyDbModel_Provider>();

// 4. UnitOfWork
builder.Services.AddScoped<MyDbModel_UnitOfWork>();

// --- Kalan Identity ayarlarý ---
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    // DÜZELTME: ApplicationDbContext -> MyDbModel_DbContext olarak deðiþtirildi
    .AddEntityFrameworkStores<MyDbModel_DbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();


// -----------------------------------------------------------------
// YENÝ EKLENEN VERÝ DOLDURMA (SEEDING) KODU
// -----------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    // DbContext'imizin yeni adýný ve konumunu kullanýyoruz
    var context = scope.ServiceProvider.GetRequiredService<SAT242516081ITHELPDESK.Models.DbContexts.MyDbModel_DbContext>();

    // Eðer veritabanýnda 'Durumlar' tablosunda HÝÇ veri yoksa:
    if (!context.Durumlar.Any())
    {
        context.Durumlar.Add(new SAT242516081ITHELPDESK.Models.Durum { Ad = "Açýk" });
        context.Durumlar.Add(new SAT242516081ITHELPDESK.Models.Durum { Ad = "Teknisyen Bekleniyor" });
        context.Durumlar.Add(new SAT242516081ITHELPDESK.Models.Durum { Ad = "Çözüldü" });
        context.Durumlar.Add(new SAT242516081ITHELPDESK.Models.Durum { Ad = "Kapatýldý" });
    }

    // Eðer veritabanýnda 'Kategoriler' tablosunda HÝÇ veri yoksa:
    if (!context.Kategoriler.Any())
    {
        context.Kategoriler.Add(new SAT242516081ITHELPDESK.Models.Kategori { Ad = "Donaným Sorunu" });
        context.Kategoriler.Add(new SAT242516081ITHELPDESK.Models.Kategori { Ad = "Yazýlým Sorunu" });
        context.Kategoriler.Add(new SAT242516081ITHELPDESK.Models.Kategori { Ad = "Að Sorunu" });
        context.Kategoriler.Add(new SAT242516081ITHELPDESK.Models.Kategori { Ad = "Diðer" });
    }

    // Deðiþiklikleri veritabanýna kaydet
    context.SaveChanges();
}
// -----------------------------------------------------------------


app.Run(); // Bu satýr en sonda kalmalý