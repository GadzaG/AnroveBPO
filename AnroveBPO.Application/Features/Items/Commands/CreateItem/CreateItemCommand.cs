using AnroveBPO.Application.Abstractions.Core;

namespace AnroveBPO.Application.Features.Items.Commands.CreateItem;

public record CreateItemCommand(
    string Code,
    string Name,
    decimal Price,
    string Category) : ICommand;