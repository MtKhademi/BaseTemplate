namespace IAMModule.Entities;

internal class FeatureEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public virtual ICollection<PermissionEntity> Permissions { get; set; } = default!;

    public int ModuleId { get; set; }
    public virtual ModuleEntity? Module { get; set; } = default!;
}
