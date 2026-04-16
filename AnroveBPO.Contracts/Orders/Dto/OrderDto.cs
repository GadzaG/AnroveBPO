namespace AnroveBPO.Contracts.Orders.Dto;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    DateTime OrderDate,
    DateTime? ShipmentDate,
    uint OrderNumber,
    string Status);
