using System.Linq.Expressions;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Application.Abstractions.Database.Repositories;

public interface IItemsRepository
{
    Task<Result<Guid, Error>> AddAsync(Item item, CancellationToken ct = default);

    Task<Result<bool, Error>> ExistsBy(
        Expression<Func<Item, bool>> predicate,
        CancellationToken ct = default);
    
    Task<Result<Item, Error>> GetBy(
        Expression<Func<Item, bool>> predicate,
        CancellationToken ct = default);
    
    UnitResult<Error> Delete(Item item);
}
