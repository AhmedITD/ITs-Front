using RentARide.Application.Interfaces.Auth;
using RentARide.Application.Interfaces.Services;
using RentARide.Application.Services;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace RentARide.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IAmenityService, AmenityService>();
        TypeAdapterConfig.GlobalSettings.Scan(typeof(DependencyInjection).Assembly);

        // services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        return services;
    }
}
