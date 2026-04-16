namespace AnroveBPO.Contracts.Orders.Requests;

public record CreateOrderRequest(
    Guid CustomerId,
    List<CreateOrderItemRequest> Items);

public record CreateOrderItemRequest(
    Guid ItemId,
    uint Count);
