namespace IAMModule.Contract.Requests;

public record ModuleGetPaginatedRequest : IPagination
{
    public int? ModuleId { get; set; }
    public string? ModuleName { get; set; }
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }

    public ModuleGetPaginatedQuery ToQuery() => ModuleGetPaginatedQuery.Create(this);
}
