namespace IAMModule.AccessControl.Features.PermissionGetPaginated;

internal class PermissionGetPaginatedHandler(IPermissionRepository repository) :
    IQueryHandler<PermissionGetPaginatedQuery, PaginatedList<PermissionModel>>
{
    public async Task<PaginatedList<PermissionModel>> Handle(PermissionGetPaginatedQuery query, CancellationToken cancellationToken)
    {
        var paginatedPermissions = await repository
            .QueryNoTracking()
            .ToPaginatedListAsync(query, cancellationToken);

        return paginatedPermissions.ToPaginatedList(Permission => Permission.ToModel());
    }
}
