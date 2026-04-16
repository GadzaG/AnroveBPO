using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Application.Abstractions.Database;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, Error>> BeginTransactionAsync(
        CancellationToken ct = default);

    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken ct = default);
}