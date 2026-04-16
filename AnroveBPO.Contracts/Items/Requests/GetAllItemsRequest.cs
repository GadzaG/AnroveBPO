namespace AnroveBPO.Contracts.Items.Requests;

public record GetAllItemsRequest(
    int Page = 1,
    int PageSize = 10);
