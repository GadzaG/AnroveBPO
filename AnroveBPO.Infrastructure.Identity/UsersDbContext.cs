using AnroveBPO.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnroveBPO.Infrastructure.Identity;

public class UsersDbContext(DbContextOptions<UsersDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    /*protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
    }*/
}