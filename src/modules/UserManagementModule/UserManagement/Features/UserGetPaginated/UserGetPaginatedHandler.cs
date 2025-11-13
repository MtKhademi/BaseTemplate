using Common.Pagination;
using UserManagementModule.Contract.Queries;

namespace UserManagementModule.UserManagement.Features.UserGetPaginated;

internal class UserGetPaginatedHandler(
    UserManager<ApplicationUser> userManager) : IQueryHandler<UserGetPaginatedQuery, PaginatedList<ApplicationUserModel>>
{
    public async Task<PaginatedList<ApplicationUserModel>> Handle(UserGetPaginatedQuery query, CancellationToken cancellationToken)
    {
        var paginatedUsers = await userManager
            .Users.ToPaginatedListAsync(query, cancellationToken);

        return paginatedUsers.ToPaginatedList(user => user.ToModel());
    }
}
