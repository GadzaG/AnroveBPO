using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AnroveBPO.Infrastructure.Postgres;


public class AndoveBPODbContext(DbContextOptions<AndoveBPODbContext> options)
    : DbContext(options), IReadDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<Item> Items => Set<Item>();

    public IQueryable<Item> ItemsQueryable => Items.AsNoTracking();

    public IQueryable<Order> OrdersQueryable => Orders.AsNoTracking();
    
    public IQueryable<OrderItem> OrderItemsQueryable => OrderItems.AsNoTracking();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AndoveBPODbContext).Assembly);
    }
}
