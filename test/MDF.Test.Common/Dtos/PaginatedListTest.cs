namespace MDF.Test.Common.Dtos;

public class PaginatedListTest<TEntity> where TEntity : class
{
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
    public long? TotalPages { get; set; }
    public long? TotalItems { get; set; }
    public IEnumerable<TEntity> Data { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

}
