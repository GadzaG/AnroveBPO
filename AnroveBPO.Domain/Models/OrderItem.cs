using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Domain.Models;

public sealed class OrderItem : Entity<Guid>
{
    private OrderItem()
    {}

    private OrderItem(
        Guid id,
        Guid orderId,
        Guid itemId,
        uint itemsCount,
        decimal itemPrice) : base(id)
    {
        OrderId = orderId;
        ItemId = itemId;
        ItemsCount = itemsCount;
        ItemPrice = itemPrice;
    }
    
    public Guid OrderId { get; private set; }
    
    public Guid ItemId { get; private set; }
    
    public uint ItemsCount { get; private set; }
    
    public decimal ItemPrice { get; private set; }

    public static Result<OrderItem, Error> Create(
        Guid orderId,
        Guid itemId,
        uint itemsCount,
        decimal itemPrice)
    {
        var id = Guid.NewGuid();
        return new OrderItem(id, orderId,  itemId, itemsCount,  itemPrice);
    }
}