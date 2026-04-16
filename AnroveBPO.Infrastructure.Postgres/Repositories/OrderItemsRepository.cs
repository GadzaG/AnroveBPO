using System.Linq.Expressions;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Infrastructure.Postgres.Repositories;

public sealed class OrderItemsRepository(
    AndoveBPODbContext context,
    ILogger<OrderItemsRepository> logger) : IOrderItemsRepository
{
    public async Task<Result<Guid, Error>> AddAsync(OrderItem orderItem, CancellationToken ct = default)
    {
        try
        {
            await context.OrderItems.AddAsync(orderItem, ct);
            return orderItem.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding order item");
            return GeneralErrors.DatabaseError("Error while adding order item");
        }
    }

    public async Task<Result<bool, Error>> ExistsBy(Expression<Func<OrderItem, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            bool exists = await context.OrderItems
                .AsNoTracking()
                .AnyAsync(predicate, ct);

            return exists;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while checking existing order item by predicate");
            return GeneralErrors.DatabaseError("Error while checking existing order item by predicate");
        }
    }

    public async Task<Result<OrderItem, Error>> GetBy(Expression<Func<OrderItem, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            OrderItem? orderItem = await context.OrderItems.FirstOrDefaultAsync(predicate, ct);
            return orderItem == null ? GeneralErrors.NotFound(null, predicate.Name?.ToString()) : orderItem;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting order item");
            return GeneralErrors.DatabaseError("Error while getting order item");
        }
    }

    public UnitResult<Error> Delete(OrderItem orderItem)
    {
        try
        {
            context.OrderItems.Remove(orderItem);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting order item");
            return GeneralErrors.DatabaseError("Error while deleting order item");
        }
    }
}
