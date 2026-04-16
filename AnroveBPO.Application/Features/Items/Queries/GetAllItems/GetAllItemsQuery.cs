using AnroveBPO.Application.Abstractions.Core;

namespace AnroveBPO.Application.Features.Items.Queries.GetAllItems;

public record GetAllItemsQuery(int Page = 1, int PageSize = 10) : IQuery;