namespace AnroveBPO.Contracts.Orders.Requests;

public record GetOrdersRequest(
    int Page = 1,
    int PageSize = 10);
