using System.Linq.Expressions;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Application.Abstractions.Database.Repositories;

public interface ICustomersRepository
{
    Task<Result<Guid, Error>> AddAsync(
        Customer customer, 
        CancellationToken ct = default);

    Task<Result<bool, Error>> ExistsBy(
        Expression<Func<Customer, bool>> predicate,
        CancellationToken ct = default);

    Task<Result<Customer, Error>> GetBy(
        Expression<Func<Customer, bool>> predicate,
        CancellationToken ct = default);

    UnitResult<Error> Delete(Customer customer);
}
