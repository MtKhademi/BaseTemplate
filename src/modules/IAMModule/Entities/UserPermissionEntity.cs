namespace IAMModule.Entities;

internal class UserPermissionEntity
{
    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }


    public int PermissionId { get; set; }
    virtual public PermissionEntity Permission { get; set; }


    public bool IsGrant { get; set; } = true;

}
