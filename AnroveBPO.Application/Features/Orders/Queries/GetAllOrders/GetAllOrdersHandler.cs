using AnroveBPO.Application.Abstractions;
using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Contracts.Orders.Dto;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Application.Features.Orders.Queries.GetAllOrders;



public sealed class GetAllOrdersHandler() : IQueryHandlerWithResult<PagedResult<OrderDto>, GetAllOrdersQuery>
{
    public async Task<Result<PagedResult<OrderDto>, Error>> Handle(GetAllOrdersQuery query, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}