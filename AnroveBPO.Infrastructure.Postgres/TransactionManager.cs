using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Infrastructure.Postgres;

public class TransactionManager(
    AndoveBPODbContext dbContext,
    ILogger<TransactionManager> logger,
    ILoggerFactory loggerFactory)
    : ITransactionManager
{
    public async Task<Result<ITransactionScope, Error>> BeginTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(ct);
            ILogger<TransactionScope> transactionScopeLogger = loggerFactory.CreateLogger<TransactionScope>();
            TransactionScope transactionScope = new(transaction.GetDbTransaction(), transactionScopeLogger);

            return transactionScope;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to begin transaction");
            return Error.Failure("database", "Failed to begin transaction");
        }
    }

    public async Task<UnitResult<Error>> SaveChangesAsync(CancellationToken ct = default)
    {
        try
        {
            await dbContext.SaveChangesAsync(ct);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save changes");
            return Error.Failure("database", "Failed to save changes");
        }
    }
}