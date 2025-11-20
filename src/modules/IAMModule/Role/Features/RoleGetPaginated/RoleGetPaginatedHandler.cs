namespace IAMModule.Role.Features.RoleGetPaginated;

internal class RoleGetPaginatedHandler(RoleManager<ApplicationRole> roleManager)
    : IQueryHandler<RoleGetPaginatedQuery, PaginatedList<RoleModel>>
{
    public async Task<PaginatedList<RoleModel>> Handle(RoleGetPaginatedQuery query, CancellationToken cancellationToken)
    {
        var rolePaginated = await roleManager
            .Roles.ToPaginatedListAsync(query, cancellationToken);

        return rolePaginated.ToPaginatedList(role => role.ToRoleModel());
    }
}
