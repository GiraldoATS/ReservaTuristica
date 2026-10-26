using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Infrastructure.Identity;
using FondoXYZ.Infrastructure.Notifications;
using FondoXYZ.Infrastructure.Persistence;
using FondoXYZ.Infrastructure.Persistence.Queries;
using FondoXYZ.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FondoXYZ.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPersistence(services, configuration);
        AddIdentity(services);
        AddNotifications(services, configuration);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ISedeRepository, SedeRepository>();
        services.AddScoped<IAlojamientoRepository, AlojamientoRepository>();
        services.AddScoped<ITemporadaRepository, TemporadaRepository>();
        services.AddScoped<ITarifaRepository, TarifaRepository>();
        services.AddScoped<IReservaRepository, ReservaRepository>();

        services.AddScoped<IDisponibilidadQuery, DisponibilidadQuery>();
        services.AddScoped<ICalculoTarifaQuery, CalculoTarifaQuery>();
        services.AddScoped<IDashboardQuery, DashboardQuery>();
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        services.AddScoped<IIdentityService, IdentityService>();
    }

    private static void AddNotifications(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

        services.AddScoped<SmtpEmailSender>();
        services.AddScoped<INotificacionService, EmailNotificacionService>();
    }
}
