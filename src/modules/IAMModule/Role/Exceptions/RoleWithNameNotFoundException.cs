namespace IAMModule.Role.Exceptions;

public class RoleWithNameNotFoundException : NotFoundException<RoleWithNameNotFoundException>
{
    public RoleWithNameNotFoundException(string roleName) : base($"Role with name '{roleName}' does not exist.")
    {

    }
}
