using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Application.Abstractions.Database;

public interface ITransactionScope : IDisposable
{
    UnitResult<Error> Commit();

    UnitResult<Error> Rollback();
}