using System.Linq.Expressions;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Infrastructure.Postgres.Repositories;

public sealed class CustomersRepository(
    AndoveBPODbContext context,
    ILogger<CustomersRepository> logger) : ICustomersRepository
{
    public async Task<Result<Guid, Error>> AddAsync(Customer customer, CancellationToken ct = default)
    {
        try
        {
            await context.Customers.AddAsync(customer, ct);
            return customer.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding customer");
            return GeneralErrors.DatabaseError("Error while adding customer");
        }
    }

    public async Task<Result<bool, Error>> ExistsBy(Expression<Func<Customer, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            bool exists = await context.Customers
                .AsNoTracking()
                .AnyAsync(predicate, ct);

            return exists;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while checking existing customer by predicate");
            return GeneralErrors.DatabaseError("Error while checking existing customer by predicate");
        }
    }

    public async Task<Result<Customer, Error>> GetBy(Expression<Func<Customer, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            Customer? customer = await context.Customers.FirstOrDefaultAsync(predicate, ct);
            return customer == null ? GeneralErrors.NotFound(null, predicate.Name?.ToString()) : customer;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting customer");
            return GeneralErrors.DatabaseError("Error while getting customer");
        }
    }

    public UnitResult<Error> Delete(Customer customer)
    {
        try
        {
            context.Customers.Remove(customer);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting customer");
            return GeneralErrors.DatabaseError("Error while deleting customer");
        }
    }
}
