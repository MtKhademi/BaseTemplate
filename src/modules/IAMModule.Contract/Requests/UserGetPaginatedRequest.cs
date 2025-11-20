namespace IAMModule.Contract.Requests;

public record UserGetPaginatedRequest : IPagination
{
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }

    public UserGetPaginatedQuery ToQuery() => UserGetPaginatedQuery.Create(this);
}
