namespace AnroveBPO.Contracts.Orders.Requests;

public record UpdateOrderRequest(
    string Status,
    DateTime? ShipmentDate = null);
