using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webOdev.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // BU �K�S� ZATEN VARDI
    options.SignIn.RequireConfirmedAccount = true;

    // ---- BAS�T ��FRELERE �Z�N VERMEK ���N BU SATIRLARI EKLEY�N ----
    options.Password.RequireDigit = false; // Say� zorunlulu�u yok
    options.Password.RequireLowercase = false; // K���k harf zorunlulu�u yok
    options.Password.RequireUppercase = false; // B�y�k harf zorunlulu�u yok
    options.Password.RequireNonAlphanumeric = false; // Sembol zorunlulu�u yok
    options.Password.RequiredLength = 3; // En az 3 karakter (bizim "sau" 3 karakter)
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ----- VER�TABANINI OTOMAT�K G�NCELLEME KODU BA�LANGICI -----
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // 'Update-Database' komutunu otomatik �al��t�r:
    dbContext.Database.Migrate();
}
// ----- VER�TABANI OTOMAT�K G�NCELLEME KODU B�T��� ----


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // 1. Rolleri olu�tur (Admin, Uye)
    string[] roller = { "Admin", "Uye" };
    foreach (var rol in roller)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }

    // 2. Admin kullan�c�s�n� g�venli bir �ekilde olu�tur
    var adminEmail = "ogrencinumarasi@sakarya.edu.tr";
    var adminSifre = "sau";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    // E�er kullan�c� yoksa, olu�tur VE rol�n� ata
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true // E-posta onay� gerektirmeden giri� yaps�n
        };

        // �nce kullan�c�y� olu�tur
        var createResult = await userManager.CreateAsync(adminUser, adminSifre);

        // SADECE kullan�c� ba�ar�yla olu�turulduysa rol�n� ata
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
