namespace IAMModule.Entities;

internal class ModuleEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;

    public virtual ICollection<FeatureEntity> Features { get; set; } = default!;


    public ModuleModel ToModel() =>
        new ModuleModel(ModuleId: Id, ModuleName: Name, ModuleDescription: Description);
}
