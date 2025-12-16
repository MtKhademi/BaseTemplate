namespace IAMModule.Contract.Requests;

public record FeatureGetPaginatedRequest : IPagination
{
    public string? FeatureName { get; set; }
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }

    public FeatureGetPaginatedQuery ToQuery() => FeatureGetPaginatedQuery.Create(this);
}
