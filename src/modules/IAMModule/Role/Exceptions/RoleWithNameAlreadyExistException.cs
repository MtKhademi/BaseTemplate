namespace IAMModule.Role.Exceptions;

public class RoleWithNameAlreadyExistException : AlreadyExistException<RoleWithNameAlreadyExistException>
{
    public RoleWithNameAlreadyExistException(string roleName) : base($"Role with name '{roleName}' already exists.")
    {

    }
}
