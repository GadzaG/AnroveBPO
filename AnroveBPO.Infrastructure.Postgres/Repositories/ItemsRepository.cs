using System.Linq.Expressions;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Infrastructure.Postgres.Repositories;


public sealed class ItemsRepository(
    AndoveBPODbContext context,
    ILogger<ItemsRepository>  logger) : IItemsRepository
{
    public async Task<Result<Guid, Error>> AddAsync(Item item, CancellationToken ct = default)
    {
        try
        {
            await context.Items.AddAsync(item, ct);
            return item.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding item");
            return GeneralErrors.DatabaseError("Error while adding item");
        }
    }

    public async Task<Result<bool, Error>> ExistsBy(Expression<Func<Item, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            bool exists = await context.Items
                .AsNoTracking()
                .AnyAsync(predicate, ct);

            return exists;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while checking existing item by predicate");
            return GeneralErrors.DatabaseError("Error while checking existing item by predicate");
        }
    }

    public async Task<Result<Item, Error>> GetBy(Expression<Func<Item, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            Item? item = await context.Items.FirstOrDefaultAsync(predicate, ct);
            return item == null ? GeneralErrors.NotFound(null, predicate.Name?.ToString()) : item;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in getting item");
            return GeneralErrors.DatabaseError("Error while getting item");
        }
    }

    public UnitResult<Error> Delete(Item item)
    {
        try
        {
            context.Items.Remove(item);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting item");
            return GeneralErrors.DatabaseError("Error while deleting item");
        }
    }
}
