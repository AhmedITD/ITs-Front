using Library.Application.Interfaces.Auth;
using Library.Application.Interfaces.Services;
using Library.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
    {
        Services.AddScoped<IAuthService, AuthService>();
        Services.AddScoped<IBookService, BookService>();
        Services.AddScoped<ILoanService, LoanService>();
        return Services;
    }
}