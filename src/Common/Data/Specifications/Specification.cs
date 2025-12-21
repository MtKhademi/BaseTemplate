namespace Infrastructure.Data.Specifications;

/// <summary>
/// Base specification class for building queries with filtering, sorting, and paging
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public abstract class Specification<T> where T : class
{
    public IQueryable<T> Query { get; protected set; } = null!;

    protected Specification()
    {
        // Initialize with empty query in constructor
    }

    protected void InitializeQuery(IQueryable<T> query)
    {
        Query = query;
    }
}
