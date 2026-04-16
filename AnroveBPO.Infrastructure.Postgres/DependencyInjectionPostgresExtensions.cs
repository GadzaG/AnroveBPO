using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Infrastructure.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Infrastructure.Postgres;

public static class DependencyInjectionPostgresExtensions
{
    
    private const string DATABASE = "Database";

    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ICustomersRepository, CustomersRepository>();
        services.AddScoped<IItemsRepository, ItemsRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();
        services.AddDbContextPool<AndoveBPODbContext>((sp, options) =>
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
        
        
        services.AddDbContextPool<IReadDbContext, AndoveBPODbContext>((sp, options) =>
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

        services.AddScoped<ITransactionManager, TransactionManager>();

        return services;
    }
}
