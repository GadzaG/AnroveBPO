using System.Linq.Expressions;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Application.Abstractions.Database.Repositories;

public interface IOrderItemsRepository
{
    Task<Result<Guid, Error>> AddAsync(OrderItem orderItem, CancellationToken ct = default);

    Task<Result<bool, Error>> ExistsBy(
        Expression<Func<OrderItem, bool>> predicate,
        CancellationToken ct = default);

    Task<Result<OrderItem, Error>> GetBy(
        Expression<Func<OrderItem, bool>> predicate,
        CancellationToken ct = default);

    UnitResult<Error> Delete(OrderItem orderItem);
}
