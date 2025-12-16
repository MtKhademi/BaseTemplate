namespace IAMModule.Contract.Requests;

public record PermissionGetPaginatedRequest : IPagination
{
    public int? ModuleId { get; set; }
    public int? FeatureId { get; set; }
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }

    public PermissionGetPaginatedQuery ToQuery() => PermissionGetPaginatedQuery.Create(this);
}
