using System.Text;
using AnroveBPO.Application;
using AnroveBPO.Domain.Models.ValueObjects;
using AnroveBPO.Infrastructure.Identity;
using AnroveBPO.Infrastructure.Identity.Jwt;
using AnroveBPO.Infrastructure.Postgres;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Exceptions;

namespace AnroveBPO.Web.Configuration;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors();
        services.AddControllers();
        services.AddSerilogLogging(configuration, "AnroveBPO.API");
        services.AddOpenApiSpec("AnroveBPO.API", "v1");
        services.AddApplication();
        services.AddPostgres(configuration);
        services.AddAuth(configuration);
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });
        return services;
    }

    private static IServiceCollection AddSerilogLogging(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        services.AddSerilog((sp, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(sp)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ServiceName", serviceName));

        return services;
    }

    private static IServiceCollection AddOpenApiSpec(this IServiceCollection services, string title, string version)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Version = version,
                Contact = new OpenApiContact
                {
                    Name = "Anrove",
                    Email = "andrey@vegele.ru"
                }
            });

            const string securitySchemeName = "Bearer";

            options.AddSecurityDefinition(securitySchemeName, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT токен без префикса Bearer"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference(securitySchemeName, hostDocument: document, externalResource: null),
                    new List<string>()
                }
            });
        });

        return services;
    }
    
    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        AuthOptions authOptions = configuration.GetSection(AuthOptions.SECTION_NAME).Get<AuthOptions>()
                                  ?? throw new InvalidOperationException("Missing auth configuration");

        services.AddIdentityServices(configuration);

        services
            .AddAuthentication(options =>
            {
                
                
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecretKey)),
                    RoleClaimType = System.Security.Claims.ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddAuthorization();

        return services;
    }

    
}
