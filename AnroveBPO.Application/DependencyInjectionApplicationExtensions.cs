using AnroveBPO.Application.Abstractions.Core;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AnroveBPO.Application;

public static class DependencyInjectionApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjectionApplicationExtensions).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjectionApplicationExtensions).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryHandlerWithResult<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.AddValidatorsFromAssembly(typeof(DependencyInjectionApplicationExtensions).Assembly);

        return services;
    }
}