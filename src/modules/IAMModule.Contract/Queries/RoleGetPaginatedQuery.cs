namespace IAMModule.Contract.Queries;

public record RoleGetPaginatedQuery : IPagination, IQuery<PaginatedList<RoleModel>>
{
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }


    private RoleGetPaginatedQuery(int? currentPage = default!, int? pageSize = default!)
    {
        CurrentPage = currentPage ?? 1;
        PageSize = pageSize ?? 10;
    }

    public static RoleGetPaginatedQuery Create(RoleGetPaginatedRequest request)
    {
        return new RoleGetPaginatedQuery(
            currentPage: request.CurrentPage,
            pageSize: request.PageSize
        );
    }
}