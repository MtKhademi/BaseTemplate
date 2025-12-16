namespace IAMModule.Contract.Responses;

public record PermissionResponse(
    int? PermissionId = default!,
    string? PermissionName = default!,
    string? PermissionDescription = default!,
    int? FeatureId = default!,
    string? FeatureName = default!,
    int? ModuleId = default!,
    string? ModuleName = default!
);