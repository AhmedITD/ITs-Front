using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Library.Api.MiddleWares;
using Library.Infrastructure.Constants;
using Library.Application;
using Library.Application.Interfaces;
using Library.Infrastructure.Data;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Application.Validators;//
using FluentValidation.AspNetCore;//
using FluentValidation;
using Library.Application.Common;
using Library.Application.DTOs;
using Library.Application.Interfaces.Auth;
using Library.Application.Interfaces.Services;
using Library.Application.Interfaces.Sinks;
using Library.Domain.Enums;
using Library.Infrastructure.Data.Interceptors;
using Library.Infrastructure.Services;
using Library.Infrastructure.Services.Auth;
using Library.Infrastructure.Services.Sinks;
using Microsoft.AspNetCore.Authentication; //
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

// using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()        
    // Custom the FlueValidation error to the api standard response
    .ConfigureApiBehaviorOptions(options => 
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(ms => ms.Value?.Errors?.Count > 0)
                .ToDictionary(
                    ms => ms.Key,
                    ms => ms.Value!.Errors.Select(e => e.ErrorMessage ?? "").ToArray());
            var response = ApiResponse<object>.ErrorResponse("Validation failed.", errors);
            return new BadRequestObjectResult(response);
        };
    });
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your JWT token.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();

builder.Services.AddScoped<AuditLogInterceptor>();
builder.Services.AddScoped<IAuditLogSink, EfCoreAuditLogSink>();
builder.Services.AddDbContext<LibraryDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).AddInterceptors(sp.GetRequiredService<AuditLogInterceptor>());
});

builder.Services.AddScoped<ILibraryDbContext, LibraryDbContext>();

// Application services (Auth/Books/Loans)
builder.Services.AddApplicationServices();

builder.Services.AddHttpContextAccessor(); // Required by CurrentUser service
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<ICurrentUser , CurrentUser>();
// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddValidatorsFromAssemblyContaining<AuthValidator>();
// builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();
// builder.Services.AddValidatorsFromAssemblyContaining<CreateLoanRequestValidator>();

//[Authrize("Admin")]
builder.Services.AddAuthorization(options =>
{
    // Tokens are issued with ClaimTypes.Role (see JwtTokenGenerator)
    options.AddPolicy(UserRole.Admin.ToStringValue(), policy => policy.RequireClaim(ClaimTypes.Role, UserRole.Admin.ToStringValue()));
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = AuthConstants.JwtScheme;
        options.DefaultChallengeScheme = AuthConstants.JwtScheme;
        options.DefaultScheme = AuthConstants.JwtScheme;
    })
    .AddJwtBearer(AuthConstants.JwtScheme, options =>
    {
        var secret = builder.Configuration["JwtSettings:Secret"];
        if (string.IsNullOrEmpty(secret))
            throw new InvalidOperationException("JwtSettings:Secret is not set in configuration. Add it to appsettings.json or set the environment variable.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret))
        };
    });

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(o => o.DisplayRequestDuration());

// Only redirect to HTTPS when in Development (HTTPS port typically configured via launchSettings).
// When running the .exe in Production on HTTP only, skipping this avoids "Failed to determine the https port" and redirect issues.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<RequestTimingMiddleware>();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();