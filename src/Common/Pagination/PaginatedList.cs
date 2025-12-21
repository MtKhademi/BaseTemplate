namespace Infrastructure.Pagination;

public interface IPaginated
{
    long TotalCount { get; }
    long TotalCountBaseFilter { get; }
    int CurrentPage { get; }
    int PageSize { get; }
    long TotalPages { get; }
}

public class PaginatedList : IPaginated
{
    public PaginatedList() { }

    public PaginatedList(IPagination pagination, long totalItems = 0, long countOfAllLogsBaseFilter = 0)
    {
        CurrentPage = pagination.CurrentPage;
        PageSize = pagination.PageSize;
        TotalCount = totalItems;
        TotalCountBaseFilter = countOfAllLogsBaseFilter;
    }

    public PaginatedList(int currentPage, int pageSize, long totalItems = 0, long countOfAllLogsBaseFilter = 0)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalItems;
        TotalCountBaseFilter = countOfAllLogsBaseFilter;
    }

    public long TotalCount { get; set; } = 0;
    public long TotalCountBaseFilter { get; set; } = 0;
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 50;

    public long TotalPages =>
        (PageSize <= 0 || TotalCount <= 0) ? 0 : (TotalCount + PageSize - 1) / PageSize;
}

public class PaginatedList<TEntity> : PaginatedList where TEntity : class
{
    public PaginatedList(
        IReadOnlyCollection<TEntity> data,
        int currentPage = 1,
        int pageSize = 50,
        long totalCount = 0,
        long totalCountBaseFilter = 0) : base(currentPage, pageSize, totalCount, totalCountBaseFilter)
    {
        Items = data;
    }

    public IReadOnlyCollection<TEntity> Items { get; }


    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}