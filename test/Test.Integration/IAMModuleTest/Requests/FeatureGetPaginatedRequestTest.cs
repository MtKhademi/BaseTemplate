namespace Test.Integration.IAMModuleTest.Requests;

public record FeatureGetPaginatedRequestTest
{
    public string? FeatureName { get; set; }
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }
}
