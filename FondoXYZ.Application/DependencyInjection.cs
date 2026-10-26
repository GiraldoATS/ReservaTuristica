using FondoXYZ.Application.Ports.In;
using FondoXYZ.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FondoXYZ.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);

        services.AddScoped<ISedeService, SedeService>();
        services.AddScoped<IAlojamientoService, AlojamientoService>();
        services.AddScoped<ITemporadaService, TemporadaService>();
        services.AddScoped<ITarifaService, TarifaService>();
        services.AddScoped<IReservaService, ReservaService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
