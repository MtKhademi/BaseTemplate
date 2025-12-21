namespace Test.Infrastructure.Requestes;

public record PaginatedListTest<TEntity>(
    int? CurrentPage,
    int? SizeOfPage,
    long? TotalPages,
    long? TotalCount,
    IEnumerable<TEntity> Items,
    bool HasPreviousPage,
    bool HasNextPage
) where TEntity : class;