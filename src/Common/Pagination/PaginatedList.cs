namespace Infrastructure.Pagination;

public interface IPaginated
{
    long TotalItems { get; }
    long CountOfAllLogsBaseFilter { get; }
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
        TotalItems = totalItems;
        CountOfAllLogsBaseFilter = countOfAllLogsBaseFilter;
    }

    public PaginatedList(int currentPage, int pageSize, long totalItems = 0, long countOfAllLogsBaseFilter = 0)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalItems = totalItems;
        CountOfAllLogsBaseFilter = countOfAllLogsBaseFilter;
    }

    public long TotalItems { get; set; } = 0;
    public long CountOfAllLogsBaseFilter { get; set; } = 0;
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 50;

    public long TotalPages =>
        (PageSize <= 0 || TotalItems <= 0) ? 0 : (TotalItems + PageSize - 1) / PageSize;
}

public class PaginatedList<TEntity> : PaginatedList where TEntity : class
{
    public PaginatedList(
        IReadOnlyCollection<TEntity> data,
        int currentPage = 1,
        int pageSize = 50,
        long totalItems = 0,
        long countOfAllLogsBaseFilter = 0) : base(currentPage, pageSize, totalItems, countOfAllLogsBaseFilter)
    {
        Data = data;
    }

    public IReadOnlyCollection<TEntity> Data { get; }

    
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}