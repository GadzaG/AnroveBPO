using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Application.Abstractions.Core;

public interface IQueryHandler<TResponse, in TQuery>
    where TQuery : IQuery
{
    Task<TResponse> Handle(TQuery query, CancellationToken ct = default);
}

public interface IQueryHandlerWithResult<TResponse, in TQuery>
    where TQuery : IQuery
{
    Task<Result<TResponse, Error>> Handle(TQuery query, CancellationToken ct = default);
}
