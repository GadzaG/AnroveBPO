using AnroveBPO.Application.Abstractions.Core;

namespace AnroveBPO.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId,
    IReadOnlyCollection<(Guid ItemId, uint Count)> Items) : ICommand;
