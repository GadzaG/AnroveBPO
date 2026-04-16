using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Domain.Models.Enums;

namespace AnroveBPO.Application.Features.Orders.Queries.GetAllOrders;



public record GetAllOrdersQuery(
    int Page = 1,
    int PageSize = 10,
    Guid? CustomerId = null,
    OrderStatus? OrderStatus = null ) : IQuery;