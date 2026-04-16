using AnroveBPO.Application.Abstractions.Core;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemName;

public record UpdateItemNameCommand(Guid Id, string Name) : ICommand;