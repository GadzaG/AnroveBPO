using AnroveBPO.Infrastructure.Identity.Jwt;
using AnroveBPO.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Infrastructure.Identity;

public static class DependencyInjectionIdentityExtensions
{
    private const string DATABASE = "Database";
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        AuthOptions authOptions = ReadAuthOptions(configuration);

        services.AddSingleton(authOptions);
        services.AddSingleton<JwtTokenProvider>();
        

        services.AddDbContextPool<UsersDbContext>((sp, options) =>
        {
            string? connectionString = configuration.GetConnectionString(DATABASE);
            IHostEnvironment hostEnvironment = sp.GetRequiredService<IHostEnvironment>();
            ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            options.UseNpgsql(connectionString);

            if (hostEnvironment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            options.UseLoggerFactory(loggerFactory);
        });

        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<UsersDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
        });

        return services;
    }
    private static AuthOptions ReadAuthOptions(IConfiguration configuration)
    {
        string section = AuthOptions.SECTION_NAME;

        string secretKey = configuration[$"{section}:{nameof(AuthOptions.SecretKey)}"] ?? string.Empty;
        string issuer = configuration[$"{section}:{nameof(AuthOptions.Issuer)}"] ?? string.Empty;
        string audience = configuration[$"{section}:{nameof(AuthOptions.Audience)}"] ?? "All";
        string tokenLifetimeRaw = configuration[$"{section}:{nameof(AuthOptions.TokenLifeTimeInMinutes)}"] ?? "5";

        _ = int.TryParse(tokenLifetimeRaw, out int tokenLifetimeInMinutes);

        if (tokenLifetimeInMinutes <= 0)
        {
            tokenLifetimeInMinutes = 5;
        }

        if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(issuer))
        {
            throw new InvalidOperationException("Missing auth configuration");
        }

        return new AuthOptions
        {
            SecretKey = secretKey,
            Issuer = issuer,
            Audience = audience,
            TokenLifeTimeInMinutes = tokenLifetimeInMinutes,
        };
    }
}
