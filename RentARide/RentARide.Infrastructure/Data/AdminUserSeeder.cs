using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Auth;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;

namespace RentARide.Infrastructure.Data;

public static class AdminUserSeeder
{
    private const string DefaultAdminPassword = "Admin@123";

    private static readonly (string Email, string FirstName, string LastName)[] AdminUsers =
    {
        ("admin1@rentaride.com", "Admin", "One"),
        ("admin2@rentaride.com", "Admin", "Two"),
        ("admin3@rentaride.com", "Admin", "Three"),
        ("admin4@rentaride.com", "Admin", "Four"),
        ("admin5@rentaride.com", "Admin", "Five")
    };

    public static async Task SeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IRentARideDbContext>();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        foreach (var (email, firstName, lastName) in AdminUsers)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            var exists = await dbContext.Users
                .AnyAsync(u => u.Email == normalizedEmail, cancellationToken);
            if (exists)
                continue;

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = normalizedEmail,
                PasswordHash = passwordService.HashPassword(DefaultAdminPassword),
                Role = UserRole.Admin
            };

            dbContext.Users.Add(user);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
