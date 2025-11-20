namespace IAMModule.Entities;

public class ApplicationRole : IdentityRole
{
    public string Description { get; set; }

    public RoleModel ToRoleModel() => new RoleModel(Id: Id, Name: Name, Description: Description);
}
