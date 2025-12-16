namespace IAMModule.Contract.Queries;

public record PermissionGetPaginatedQuery : IPagination, IQuery<PaginatedList<PermissionModel>>
{
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }


    private PermissionGetPaginatedQuery(int? currentPage = default!, int? pageSize = default!)
    {
        CurrentPage = currentPage ?? 1;
        PageSize = pageSize ?? 10;
    }

    public static PermissionGetPaginatedQuery Create(PermissionGetPaginatedRequest request)
    {
        return new PermissionGetPaginatedQuery(
            currentPage: request.CurrentPage,
            pageSize: request.PageSize
        );
    }
}