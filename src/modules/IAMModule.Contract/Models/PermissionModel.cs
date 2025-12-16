namespace IAMModule.Contract.Models;

public record PermissionModel(
    int PermissionId,
    string PermissionName,
    string PermissionDescription,
    int? FeatureId = default!,
    string? FeatureName = default!,
    int? ModuleId = default!,
    string? ModuleName = default!)
{
    public PermissionResponse ToPermissionResponse()
        => new PermissionResponse(
            PermissionId: PermissionId,
            PermissionName: PermissionName,
            PermissionDescription: PermissionDescription,
            FeatureId: FeatureId,
            FeatureName: FeatureName,
            ModuleId: ModuleId,
            ModuleName: ModuleName
        );
}