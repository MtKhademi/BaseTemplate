namespace UserManagementModule.IAM.Authorization;


public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string feature, string action)
        => Policy = AppPermission.NameFor(feature, action);
}