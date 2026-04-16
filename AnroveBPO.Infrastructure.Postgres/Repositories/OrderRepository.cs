using System.Linq.Expressions;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Infrastructure.Postgres.Repositories;

public sealed class OrderRepository(
    AndoveBPODbContext context,
    ILogger<OrderRepository> logger) : IOrderRepository
{
    public async Task<Result<Guid, Error>> AddAsync(Order order, CancellationToken ct = default)
    {
        try
        {
            await context.Orders.AddAsync(order, ct);
            return order.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding order");
            return GeneralErrors.DatabaseError("Error while adding order");
        }
    }

    public async Task<Result<long, Error>> CountBy(Expression<Func<Order, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            long count = await context.Orders
                .AsNoTracking()
                .LongCountAsync(predicate, ct);

            return count;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while counting orders by predicate");
            return GeneralErrors.DatabaseError("Error while counting orders by predicate");
        }
    }

    public async Task<Result<bool, Error>> ExistsBy(Expression<Func<Order, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            bool exists = await context.Orders
                .AsNoTracking()
                .AnyAsync(predicate, ct);

            return exists;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while checking existing order by predicate");
            return GeneralErrors.DatabaseError("Error while checking existing order by predicate");
        }
    }

    public async Task<Result<Order, Error>> GetBy(Expression<Func<Order, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            Order? order = await context.Orders.FirstOrDefaultAsync(predicate, ct);
            return order == null ? GeneralErrors.NotFound(null, predicate.Name?.ToString()) : order;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting order");
            return GeneralErrors.DatabaseError("Error while getting order");
        }
    }

    public UnitResult<Error> Delete(Order order)
    {
        try
        {
            context.Orders.Remove(order);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting order");
            return GeneralErrors.DatabaseError("Error while deleting order");
        }
    }
}
