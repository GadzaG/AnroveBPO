using AnroveBPO.Application.Abstractions.Core;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemCode;

public record UpdateItemCodeCommand(Guid Id, string Code) : ICommand;
