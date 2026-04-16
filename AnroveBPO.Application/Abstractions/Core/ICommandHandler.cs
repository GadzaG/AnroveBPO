using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Application.Abstractions.Core;

public interface ICommandHandler<TResponse, in TCommand>
    where TCommand : ICommand
{
    Task<Result<TResponse, Error>> Handle(TCommand command, CancellationToken ct = default);
}

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<UnitResult<Error>> Handle(TCommand command, CancellationToken ct = default);
}
