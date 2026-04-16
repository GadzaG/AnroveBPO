using AnroveBPO.Domain.Models;

namespace AnroveBPO.Application.Abstractions.Database;

public interface IReadDbContext
{
    IQueryable<Item> ItemsQueryable { get; }

    IQueryable<Order> OrdersQueryable { get; }
    
    IQueryable<OrderItem> OrderItemsQueryable { get; }
}
