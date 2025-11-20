namespace IAMModule.Contract.Queries;

public record UserGetPaginatedQuery : IPagination, IQuery<PaginatedList<ApplicationUserModel>>
{
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }


    private UserGetPaginatedQuery(int? currentPage = default!, int? pageSize = default!)
    {
        CurrentPage = currentPage ?? 1;
        PageSize = pageSize ?? 10;
    }

    public static UserGetPaginatedQuery Create(UserGetPaginatedRequest request)
    {
        return new UserGetPaginatedQuery(
            currentPage: request.CurrentPage,
            pageSize: request.PageSize
        );
    }
}