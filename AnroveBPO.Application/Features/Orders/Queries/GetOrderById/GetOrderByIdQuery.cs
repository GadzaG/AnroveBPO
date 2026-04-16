using AnroveBPO.Application.Abstractions.Core;

namespace AnroveBPO.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IQuery;