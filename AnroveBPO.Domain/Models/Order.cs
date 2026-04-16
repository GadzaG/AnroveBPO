using AnroveBPO.Domain.Models.Enums;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Domain.Models;

public sealed class Order : Entity<Guid>
{
    public Guid CustomerId { get; private set; }
    
    public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
    
    public DateTime? ShipmentDate { get; private set; } = null;
    
    public uint OrderNumber { get; private set; }

    public OrderStatus OrderStatus { get; private set; } = OrderStatus.CREATED;

    private Order(Guid id, Guid customerId, uint orderNumber) : base(id)
    {
        CustomerId = customerId;
        OrderNumber = orderNumber;
    }

    public UnitResult<Error> SetInProgress(DateTime shipmentDate)
    {
        if (OrderStatus != OrderStatus.CREATED)
        {
            return Error.Validation(
                "order.status.transition.invalid",
                "Нельзя перевести заказ в статус IN_PROGRESS из текущего статуса",
                "orderStatus");
        }

        ShipmentDate = shipmentDate;
        OrderStatus = OrderStatus.IN_PROGRESS;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> SetCompleted()
    {
        if (OrderStatus != OrderStatus.IN_PROGRESS)
        {
            return Error.Validation(
                "order.status.transition.invalid",
                "Нельзя закрыть заказ, который не в статусе IN_PROGRESS",
                "orderStatus");
        }

        OrderStatus = OrderStatus.COMPLETED;
        return UnitResult.Success<Error>();
    }
    
    public static Result<Order> Create(
        Guid customerId,
        uint orderNumber)
    {
        var id = Guid.NewGuid();
        return new Order(id, customerId, orderNumber);
    }
}
