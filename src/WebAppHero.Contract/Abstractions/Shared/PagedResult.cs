using Microsoft.EntityFrameworkCore;

namespace WebAppHero.Contract.Abstractions.Shared;

public class PagedResult<T>(List<T> items, int pageIndex, int pageSize, int totalCount)
{
    public const int UpperPageSize = 100;
    public const int DefaultPageSize = 10;
    public const int DefaultPageIndex = 1;

    public List<T> Items { get; } = items;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalCount { get; } = totalCount;

    public bool HasNextPage => PageIndex * PageSize < TotalCount;

    public bool HasPreviousPage => PageIndex > 1;

    public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize)
    {
        pageIndex = pageIndex <= 0 ? DefaultPageIndex : pageIndex;
        pageSize = pageSize <= 0 ? DefaultPageSize : Math.Min(pageSize, UpperPageSize);

        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new(items, pageIndex, pageSize, totalCount);
    }

    public static PagedResult<T> Create(List<T> items, int pageIndex, int pageSize, int totalCount)
    {
        return new(items, pageIndex, pageSize, totalCount);
    }
}
