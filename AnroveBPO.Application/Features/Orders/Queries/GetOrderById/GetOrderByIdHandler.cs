using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AnroveBPO.Application.Features.Orders.Queries.GetOrderById;



public record FullOrderDto();

public sealed class GetOrderByIdHandler(IReadDbContext readDbContext) : IQueryHandlerWithResult<FullOrderDto,GetOrderByIdQuery>
{
    public async Task<Result<FullOrderDto, Error>> Handle(GetOrderByIdQuery query, CancellationToken ct = default)
    {
        var order = await readDbContext.OrdersQueryable.FirstOrDefaultAsync(o => o.Id == query.Id, ct);
        if (order == null)
        {
            return GeneralErrors.NotFound(query.Id, "Order not found");
        }
        throw new  NotImplementedException();
    }
}