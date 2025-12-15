using Infrastructure.Auth;

namespace IAMModule.Entities;

internal class PermissionEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public AppAction Action { get; set; }

    public int AppFeatureId { get; set; }
    public virtual FeatureEntity AppFeature { get; set; } = default!;
}
