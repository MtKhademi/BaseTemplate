namespace IAMModule.AccessControl.Features.FeatureGetPaginated;

internal class FeatureGetPaginatedHandler(IFeatureRepository repository) :
    IQueryHandler<FeatureGetPaginatedQuery, PaginatedList<FeatureModel>>
{
    public async Task<PaginatedList<FeatureModel>> Handle(FeatureGetPaginatedQuery query, CancellationToken cancellationToken)
    {
        var paginatedFeatures = await repository
            .QueryNoTracking()
            .ToPaginatedListAsync(query, cancellationToken);

        return paginatedFeatures.ToPaginatedList(Feature => Feature.ToModel());
    }
}
