
namespace UserManagementModule.Contract.Requests;

public record UserGetPaginatedRequest : IPagination, IQuery<PaginatedList<ApplicationUserModel>>
{
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }

    public UserGetPaginatedQuery ToQuery() => UserGetPaginatedQuery.Create(this);
}
