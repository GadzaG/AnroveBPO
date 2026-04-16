using AnroveBPO.Application.Abstractions.Core;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemPrice;

public record UpdateItemPriceCommand(Guid Id, decimal Price) : ICommand;
