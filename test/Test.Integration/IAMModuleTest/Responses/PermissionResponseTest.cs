namespace Test.Integration.IAMModuleTest.Responses;

public record PermissionResponseTest(
    int? PermissionId = default!,
    string? PermissionName = default!,
    string? PermissionDescription = default!,
    int? FeatureId = default!,
    string? FeatureName = default!,
    int? ModuleId = default!,
    string? ModuleName = default!
);