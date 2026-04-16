namespace AnroveBPO.Contracts.Items.Dto;

public record ItemDto(
    Guid Id,
    string Name,
    decimal Price,
    string Category,
    string Code);