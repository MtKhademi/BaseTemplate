namespace IAMModule.Contract.Queries;

public record FeatureGetPaginatedQuery : IPagination, IQuery<PaginatedList<FeatureModel>>
{
    public string? FeatureName { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }


    private FeatureGetPaginatedQuery(string? featureName, int? currentPage = default!, int? pageSize = default!)
    {
        FeatureName = featureName;
        CurrentPage = currentPage ?? 1;
        PageSize = pageSize ?? 10;
    }

    public static FeatureGetPaginatedQuery Create(FeatureGetPaginatedRequest request)
    {
        return new FeatureGetPaginatedQuery(
            featureName: request.FeatureName,
            currentPage: request.CurrentPage,
            pageSize: request.PageSize
        );
    }

    internal class FeatureGetPaginatedQueryException : NotValidDataException<FeatureGetPaginatedQueryException>
    {
        public FeatureGetPaginatedQueryException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}