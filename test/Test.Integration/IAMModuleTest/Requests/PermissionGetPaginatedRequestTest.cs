namespace Test.Integration.IAMModuleTest.Requests;

public record PermissionGetPaginatedRequestTest(
    int? ModuleId = default!,
    int? FeatureId = default!,
    int? CurrentPage = default!,
    int? PageSize = default!);