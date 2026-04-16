using AnroveBPO.Infrastructure.Identity;
using AnroveBPO.Infrastructure.Identity.Models;
using AnroveBPO.Web.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Web.Configuration;

public static class AppExtensions
{
    public static IApplicationBuilder Configure(this WebApplication app)
    {
        ApplyMigrationsAndSeedDefaultUser(app).GetAwaiter().GetResult();

        app.UseCors(builder =>
        {
            builder.AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

        app.UseExceptionMiddleware();
        // app.UseSerilogRequestLogging();

        app.UseSwagger(options =>
        {
            options.RouteTemplate = "openapi/{documentName}.json";
        });
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "AnroveBPO V1");
            options.RoutePrefix = "swagger";
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    private static async Task ApplyMigrationsAndSeedDefaultUser(WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        UsersDbContext usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        await usersDbContext.Database.MigrateAsync();

        UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        RoleManager<IdentityRole<Guid>> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        ILogger logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("StartupSeed");

        bool hasAnyUsers = await userManager.Users.AnyAsync();
        if (hasAnyUsers)
            return;

        const string adminUserName = "admin";
        const string adminPassword = "admin";
        const string adminEmail = "admin@local";

        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {
            IdentityResult createRoleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin));
            if (!createRoleResult.Succeeded)
            {
                string roleErrors = string.Join("; ", createRoleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create '{Roles.Admin}' role: {roleErrors}");
            }
        }

        User adminUser = new()
        {
            UserName = adminUserName,
            Email = adminEmail,
            EmailConfirmed = true
        };

        IdentityResult createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (!createUserResult.Succeeded)
        {
            string userErrors = string.Join("; ", createUserResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create default admin user: {userErrors}");
        }

        IdentityResult addRoleResult = await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        if (!addRoleResult.Succeeded)
        {
            string roleErrors = string.Join("; ", addRoleResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to add admin role to default user: {roleErrors}");
        }

        logger.LogInformation("Default admin user '{UserName}' was created.", adminUserName);
    }
}
