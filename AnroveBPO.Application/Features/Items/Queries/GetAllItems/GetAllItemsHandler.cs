using AnroveBPO.Application.Abstractions;
using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Contracts.Items.Dto;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Application.Features.Items.Queries.GetAllItems;

public sealed class GetAllItemsHandler(
    IReadDbContext readDbContext,
    ILogger<GetAllItemsHandler> logger) : IQueryHandlerWithResult<PagedResult<ItemDto>, GetAllItemsQuery>
{
    private const int MaxPageSize = 100;

    public async Task<Result<PagedResult<ItemDto>, Error>> Handle(GetAllItemsQuery query, CancellationToken ct = default)
    {
        if (query.Page < 1)
            return Error.Validation("pagination.page.invalid", "Page must be greater than or equal to 1", "page");

        if (query.PageSize < 1)
            return Error.Validation("pagination.page_size.invalid", "PageSize must be greater than or equal to 1", "pageSize");

        int pageSize = Math.Min(query.PageSize, MaxPageSize);
        int skip = (query.Page - 1) * pageSize;

        try
        {
            IQueryable<ItemDto> queryableItems = readDbContext.ItemsQueryable
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Select(x => new ItemDto(
                    x.Id,
                    x.Name,
                    x.Price,
                    x.Category,
                    x.Code.Value));

            int totalCount = await queryableItems.CountAsync(ct);
            List<ItemDto> pagedItems = await queryableItems
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<ItemDto>(pagedItems, query.Page, pageSize, totalCount);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("GetAllItems request was cancelled");
            return GeneralErrors.OperationCancelled();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting items page {Page}, pageSize {PageSize}", query.Page, query.PageSize);
            return GeneralErrors.DatabaseError("error while getting items");
        }
    }
}
