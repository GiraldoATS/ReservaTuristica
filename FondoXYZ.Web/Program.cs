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

#region Connection String

var connectionString =
    builder.Configuration
        .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

#endregion

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(connectionString));

builder.Services
    .AddDatabaseDeveloperPageExceptionFilter();

#endregion

#region Email Settings

builder.Services.Configure<EmailSettings>(
    builder.Configuration
        .GetSection("EmailSettings"));

#endregion

#region Identity

builder.Services.AddIdentity<
    ApplicationUser,
    IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount =
            false;

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

#endregion

#region Cookies

builder.Services.ConfigureApplicationCookie(
    options =>
    {
        options.LoginPath =
            "/Account/Login";

        options.AccessDeniedPath =
            "/Account/AccessDenied";

        options.Cookie.Name =
            "FondoXYZAuth";

        options.Cookie.HttpOnly =
            true;

        options.Cookie.IsEssential =
            true;

        options.ExpireTimeSpan =
            TimeSpan.FromHours(8);

        options.SlidingExpiration =
            true;

        options.Events.OnRedirectToAccessDenied =
            context =>
            {
                context.Response.Redirect(
                    "/Account/AccessDenied");

                return Task.CompletedTask;
            };
    });

#endregion

#region MVC

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

#endregion

#region Repositories

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

#endregion

#region Services

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

#endregion

#region Build

var app = builder.Build();

#endregion

#region Middleware

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

#endregion

#region Authentication

app.UseAuthentication();

app.UseAuthorization();

#endregion

#region Routes

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();

#endregion

#region Run

app.Run();

#endregion