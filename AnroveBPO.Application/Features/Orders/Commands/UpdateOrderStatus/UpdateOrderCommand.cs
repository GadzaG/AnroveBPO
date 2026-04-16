using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Domain.Models.Enums;

namespace AnroveBPO.Application.Features.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderCommand(
    Guid Id,
    OrderStatus Status,
    DateTime? ShipmentDate = null) : ICommand;
