

using AnroveBPO.Application.Abstractions.Core;

namespace AnroveBPO.Application.Features.Items.Commands.DeleteItem;

public record DeleteItemCommand(Guid Id) : ICommand; 