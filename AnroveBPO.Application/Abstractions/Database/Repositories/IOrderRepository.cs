using System.Linq.Expressions;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Application.Abstractions.Database.Repositories;

public interface IOrderRepository
{
    Task<Result<Guid, Error>> AddAsync(Order order, CancellationToken ct = default);

    Task<Result<long, Error>> CountBy(
        Expression<Func<Order, bool>> predicate,
        CancellationToken ct = default);

    Task<Result<bool, Error>> ExistsBy(
        Expression<Func<Order, bool>> predicate,
        CancellationToken ct = default);

    Task<Result<Order, Error>> GetBy(
        Expression<Func<Order, bool>> predicate,
        CancellationToken ct = default);

    UnitResult<Error> Delete(Order order);
}
