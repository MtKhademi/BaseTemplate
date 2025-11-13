namespace UserManagementModule.IAM.Exceptions;

public class RoleCreateException : NotValidDataException<RoleCreateException>
{
    public RoleCreateException(List<string> errors) : base(errors)
    {
    }

    public RoleCreateException(IdentityResult result) : base(result.Errors.Select(e => e.Description).ToList())
    {
    }
}
