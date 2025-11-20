namespace IAMModule.Role.Exceptions;

public class RoleWithRoleIdNotFoundException : NotFoundException<RoleWithRoleIdNotFoundException>
{
    public RoleWithRoleIdNotFoundException(string roleId) : base($"Role with ID '{roleId}' does not exist.")
    {

    }
}
