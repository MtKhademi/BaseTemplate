
namespace IAMModule.Contract.Requests;

public record RoleGetPaginatedRequest : IPagination
{
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }

    public RoleGetPaginatedQuery ToQuery() => RoleGetPaginatedQuery.Create(this);
}
