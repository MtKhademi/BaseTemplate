namespace IAMModule.AccessControl.Features.ModuleGetPaginated;

internal class ModuleGetPaginatedHandler(IModuleRepository repository) :
    IQueryHandler<ModuleGetPaginatedQuery, PaginatedList<ModuleModel>>
{
    public async Task<PaginatedList<ModuleModel>> Handle(ModuleGetPaginatedQuery query, CancellationToken cancellationToken)
    {
        var paginatedPermissions = await repository
            .QueryNoTracking()
            .FilterById(query.ModuleId)
            .FilterByName(query.ModuleName)
            .ToPaginatedListAsync(query, cancellationToken);

        return paginatedPermissions.ToPaginatedList(Permission => Permission.ToModel());
    }
}
