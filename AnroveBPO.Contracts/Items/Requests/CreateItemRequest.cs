namespace AnroveBPO.Contracts.Items.Requests;

public record CreateItemRequest(
    string Code,
    string Name,
    decimal Price,
    string Category);
