namespace AnroveBPO.Application.Abstractions;

public class PagedResult<T>(
    IEnumerable<T> items,
    int pageNumber,
    int pageSize,
    int totalCount)
{
    public int PageNumber { get; set; } = pageNumber;

    public int PageSize { get; set; } = pageSize;

    public int TotalPages { get; set; } = (int)Math.Ceiling(totalCount / (double)pageSize);

    public int TotalCount { get; set; } = totalCount;

    public IEnumerable<T> Items { get; set; } = items;
}