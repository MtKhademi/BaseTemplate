using System.Linq.Expressions;

namespace Infrastructure.Pagination;

public static class PaginatedListExtensions
{
    public static async Task<PaginatedList<TEntity>> ToPaginatedListAsync<TEntity>(
        this IQueryable<TEntity> source,
        int currentPage,
        int pageSize,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TEntity>(items, currentPage, pageSize, count);
    }

    public static async Task<PaginatedList<TEntity>> ToPaginatedListAsync<TEntity>(
        this IQueryable<TEntity> source,
        IPagination pagination,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        int currentPage = pagination.CurrentPage;
        int pageSize = pagination.PageSize;
        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TEntity>(items, currentPage, pageSize, count);
    }

    public static async Task<PaginatedList<TModel>> ToPaginatedListAsync<TModel, TEntity>(
        this IQueryable<TEntity> source,
        IPagination pagination,
        Expression<Func<TEntity, TModel>> selector,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TModel : class
    {
        int currentPage = pagination.CurrentPage;
        int pageSize = pagination.PageSize;
        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TModel>(items, currentPage, pageSize, count);
    }

    public static PaginatedList<TEntity> ToPaginatedList<TEntity>(
        this IEnumerable<TEntity> source,
        int currentPage = 1,
        int pageSize = 50,
        long? totalItems = null)
        where TEntity : class
    {
        var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        var count = totalItems ?? source.LongCount();
        return new PaginatedList<TEntity>(items, currentPage, pageSize, count);
    }

    public static PaginatedList<TEntity> ToPaginatedList<TEntity>(
        this IEnumerable<TEntity> source,
        IPagination? pagination)
        where TEntity : class
    {
        int currentPage = pagination?.CurrentPage ?? 1;
        int pageSize = pagination?.PageSize ?? 50;
        var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        var count = source.LongCount();
        return new PaginatedList<TEntity>(items, currentPage, pageSize, count);
    }

    public static PaginatedList<TResponse> ToPaginatedList<TResponse, TModel>(
        this PaginatedList<TModel> source,
        Func<TModel, TResponse> mapFunc)
        where TModel : class
        where TResponse : class
    {
        var mapped = source.Data.Select(mapFunc).ToList();
        return new PaginatedList<TResponse>(
            mapped,
            source.CurrentPage,
            source.PageSize,
            source.TotalItems,
            source.CountOfAllLogsBaseFilter
        );
    }

    public static IPagination InitializePagination(this IPagination pagination)
    {
        if (pagination.CurrentPage < 1) pagination.CurrentPage = 1;
        if (pagination.PageSize < 1) pagination.PageSize = 50;
        return pagination;
    }

    public static IQueryable<TResult> SetPagination<TResult>(
        this IQueryable<TResult> source,
        int currentPage = 1,
        int pageSize = 20)
    {
        return source.Skip((currentPage - 1) * pageSize).Take(pageSize);
    }

    public static IQueryable<TResult> SetPagination<TResult>(
        this IQueryable<TResult> source,
        IPagination pagination)
    {
        pagination.InitializePagination();
        return source.Skip((pagination.CurrentPage - 1) * pagination.PageSize).Take(pagination.PageSize);
    }
}