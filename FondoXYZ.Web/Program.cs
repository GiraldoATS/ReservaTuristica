using FondoXYZ.Data;
using FondoXYZ.Entities;
using FondoXYZ.Entities.Configurations;
using FondoXYZ.Repositories.Implementations;
using FondoXYZ.Repositories.Interfaces;
using FondoXYZ.Services.Implementations;
using FondoXYZ.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================
// CONNECTION STRING
// =========================

var connectionString =
    builder.Configuration
        .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

// =========================
// DB CONTEXT
// =========================

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(connectionString));

builder.Services
    .AddDatabaseDeveloperPageExceptionFilter();

// =========================
// EMAIL SETTINGS
// =========================

builder.Services.Configure<EmailSettings>(
    builder.Configuration
        .GetSection("EmailSettings"));

// =========================
// IDENTITY
// =========================

builder.Services.AddIdentity<
    ApplicationUser,
    IdentityRole>(options =>
    {
        // =========================
        // SIGN IN
        // =========================

        options.SignIn.RequireConfirmedAccount =
            false;

        // =========================
        // PASSWORD
        // =========================

        options.Password.RequireDigit =
            false;

        options.Password.RequireUppercase =
            false;

        options.Password.RequireLowercase =
            false;

        options.Password.RequireNonAlphanumeric =
            false;

        options.Password.RequiredLength =
            6;
    })
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// =========================
// COOKIE LOGIN
// =========================

builder.Services.ConfigureApplicationCookie(
    options =>
    {
        // =========================
        // RUTAS
        // =========================

        options.LoginPath =
            "/Account/Login";

        options.AccessDeniedPath =
            "/Account/AccessDenied";

        // =========================
        // COOKIE
        // =========================

        options.Cookie.Name =
            "FondoXYZAuth";

        options.Cookie.HttpOnly =
            true;

        options.Cookie.IsEssential =
            true;

        // =========================
        // TIEMPO
        // =========================

        options.ExpireTimeSpan =
            TimeSpan.FromHours(8);

        options.SlidingExpiration =
            true;

        // =========================
        // REDIRECCION
        // =========================

        options.Events.OnRedirectToAccessDenied =
            context =>
            {
                context.Response.Redirect(
                    "/Account/AccessDenied");

                return Task.CompletedTask;
            };
    });

// =========================
// MVC + GLOBAL AUTH
// =========================

builder.Services
    .AddControllersWithViews(options =>
    {
        var policy =
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

        options.Filters.Add(
            new AuthorizeFilter(policy));
    });

builder.Services.AddRazorPages();

// =========================
// REPOSITORIES
// =========================

builder.Services.AddScoped<
    IReservaRepository,
    ReservaRepository>();

builder.Services.AddScoped<
    ITarifaRepository,
    TarifaRepository>();

builder.Services.AddScoped<
    ITemporadaRepository,
    TemporadaRepository>();

builder.Services.AddScoped<
    IAlojamientoRepository,
    AlojamientoRepository>();

builder.Services.AddScoped<
    IDashboardRepository,
    DashboardRepository>();

builder.Services.AddScoped<
    ISedeRepository,
    SedeRepository>();

// =========================
// SERVICES
// =========================

builder.Services.AddScoped<
    IReservaService,
    ReservaService>();

builder.Services.AddScoped<
    ITarifaService,
    TarifaService>();

builder.Services.AddScoped<
    ITemporadaService,
    TemporadaService>();

builder.Services.AddScoped<
    IAlojamientoService,
    AlojamientoService>();

builder.Services.AddScoped<
    IDashboardService,
    DashboardService>();

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    IEmailService,
    EmailService>();

builder.Services.AddScoped<
    ISedeService,
    SedeService>();

// =========================
// BUILD
// =========================

var app = builder.Build();

// =========================
// MIDDLEWARE
// =========================

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler(
        "/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// =========================
// AUTHENTICATION
// =========================

app.UseAuthentication();

app.UseAuthorization();

// =========================
// ROUTES
// =========================

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();

// =========================
// RUN
// =========================

app.Run();