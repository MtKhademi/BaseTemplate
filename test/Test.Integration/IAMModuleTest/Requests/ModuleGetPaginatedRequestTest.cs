namespace Test.Integration.IAMModuleTest.Requests;

public record ModuleGetPaginatedRequestTest
{
    public int? ModuleId { get; set; }
    public string? ModuleName { get; set; }
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }
}
