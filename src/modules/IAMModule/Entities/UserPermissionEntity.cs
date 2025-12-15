namespace IAMModule.Entities;

internal class UserPermissionEntity
{
    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }


    public int AppPermissionId { get; set; }
    virtual public PermissionEntity AppPermission { get; set; }


    public bool IsGrant { get; set; } = true;

}
