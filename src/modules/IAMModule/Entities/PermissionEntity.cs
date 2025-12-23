namespace IAMModule.Entities;

internal class PermissionEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ActionType Action { get; set; }

    public int FeatureId { get; set; }
    public virtual FeatureEntity Feature { get; set; } = default!;


    public PermissionModel ToModel()
    {
        return new PermissionModel(
            PermissionId: Id,
            PermissionName: Name,
            PermissionDescription: Description,
            FeatureId: Feature.Id,
            FeatureName: Feature.Name,
            ModuleId: Feature.Module?.Id,
            ModuleName: Feature.Module?.Name
            );
    }
}
