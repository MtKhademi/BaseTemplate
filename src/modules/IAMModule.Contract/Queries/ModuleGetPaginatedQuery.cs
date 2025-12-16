namespace IAMModule.Contract.Queries;

public record ModuleGetPaginatedQuery : IPagination, IQuery<PaginatedList<ModuleModel>>
{
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int? ModuleId { get; init; }
    public string? ModuleName { get; init; }

    private ModuleGetPaginatedQuery(
        int? moduleId = default!,
        string? moduleName = default!, int? currentPage = default!, int? pageSize = default!)
    {
        var errors = new List<string>();
        if (moduleId is not null && moduleId <= 0)
            errors.Add($"{nameof(moduleId)} must be greater than zero.");


        if (errors.Any())
            throw new ModuleGetPaginatedQueryException(errors);


        CurrentPage = currentPage ?? 1;
        PageSize = pageSize ?? 10;
        ModuleId = moduleId;
        ModuleName = moduleName;

    }

    public static ModuleGetPaginatedQuery Create(ModuleGetPaginatedRequest request)
    {
        return new ModuleGetPaginatedQuery(
            moduleId: request.ModuleId,
            moduleName: request.ModuleName,
            currentPage: request.CurrentPage,
            pageSize: request.PageSize
        );
    }


    internal class ModuleGetPaginatedQueryException : NotValidDataException<ModuleGetPaginatedQueryException>
    {
        public ModuleGetPaginatedQueryException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}