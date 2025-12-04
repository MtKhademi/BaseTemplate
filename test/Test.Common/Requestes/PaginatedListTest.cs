namespace Test.Infrastructure.Requestes;

public record PaginatedListTest<TEntity>(
    int? CurrentPage,
    int? SizeOfPage,
    long? TotalPages,
    long? TotalItems,
    IEnumerable<TEntity> Data,
    bool HasPreviousPage,
    bool HasNextPage
) where TEntity : class;