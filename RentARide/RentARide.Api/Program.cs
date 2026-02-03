using System.Reflection;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentARide.Application;
using RentARide.Application.Common;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Auth;
using RentARide.Application.Interfaces.Services;
using RentARide.Application.Interfaces.Sinks;
using RentARide.Application.Validators;
using RentARide.Domain.Enums;
using RentARide.Infrastructure.Constants;
using RentARide.Infrastructure.Data;
using RentARide.Infrastructure.Data.Interceptors;
using RentARide.Infrastructure.Jobs;
using RentARide.Infrastructure.Services;
using RentARide.Infrastructure.Services.Auth;
using RentARide.Infrastructure.Services.Sinks;
using RentARide.Api.Json;
using RentARide.Application.DTOs.Responses.Common;
using RentARide.Application.Validators.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableUtcDateTimeConverter());
    })
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

// Register all validators from the assembly containing 'Startup' or 'Program'
// builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// Enable automatic validation
// builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "RentARide API", Version = "v1" });
    options.AddServer(new Microsoft.OpenApi.Models.OpenApiServer { Url = "http://localhost:5022", Description = "HTTP (Development)" }); //
    options.AddServer(new Microsoft.OpenApi.Models.OpenApiServer { Url = "https://localhost:7253", Description = "HTTPS (Development)" }); //
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your JWT token."
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
            Array.Empty<string>()
        }
    });
});

//Cache Services
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IVehicleTypeCacheService, VehicleTypeCacheService>();

//External Services
builder.Services.AddHttpClient();
builder.Services.AddScoped<IPublicHolidayService, PublicHolidayService>();
builder.Services.AddScoped<IQiCardService, QiCardService>();

//Audit Log Services
builder.Services.AddScoped<AuditLogInterceptor>();
builder.Services.AddScoped<IAuditLogSink, EfCoreAuditLogSink>();

//Database Context
builder.Services.AddDbContext<RentARideDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(sp.GetRequiredService<AuditLogInterceptor>());
});
builder.Services.AddScoped<IRentARideDbContext, RentARideDbContext>();

builder.Services.AddApplicationServices(); //Services

//Http Context Accessor (HttpContext)
builder.Services.AddHttpContextAccessor();

//Auth Services
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<ISuperQiUserResolver, SuperQiUserResolver>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

//Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(UserRole.Admin.ToStringValue(), policy =>
        policy.RequireClaim(ClaimTypes.Role, UserRole.Admin.ToStringValue()));
});

//App Authtection (JWT)
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
        throw new InvalidOperationException("JwtSettings:Secret is not set in configuration.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});

//Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// CORS: allow all origins
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//HangrFire Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(connectionString!));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<OverdueRentalsJob>();

// --------------------------------------------------------------------------------------------------------
var app = builder.Build();
// --------------------------------------------------------------------------------------------------------

// Seed admin users if they don't exist
using (var scope = app.Services.CreateScope())
{
    await AdminUserSeeder.SeedAsync(scope.ServiceProvider);
}

// CORS first: add headers to every response and handle OPTIONS preflight so tunnels (Pinggy, zrok, etc.) don't block cross-origin requests
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
    context.Response.Headers.Append("Access-Control-Allow-Headers", "*");
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = StatusCodes.Status204NoContent;
        return;
    }
    await next();
});
app.UseCors();

// UseSwagger
app.UseSwagger();

// UseSwaggerUI
app.UseSwaggerUI(o =>
{
    o.DisplayRequestDuration();
    // Use same origin so "Try it out" and frontend calls don't fail
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "RentARide API v1");
});

// Only redirect to HTTPS in production; in Development use the URL you're actually running (http or https)
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

//...
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(_ => { });

app.MapControllers();

//Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

//Hangfire Jobs
RecurringJob.AddOrUpdate<OverdueRentalsJob>(
    "overdue-rentals",
    job => job.ExecuteAsync(CancellationToken.None),
    Cron.Hourly);

//--------------------------------------------------------------------------------------------------------
app.Run();
