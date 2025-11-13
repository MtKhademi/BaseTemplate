namespace UserManagementModule.IAM.Exceptions;

public class UserNotFoundWithEmailException : NotFoundException<UserNotFoundWithEmailException>
{
    public UserNotFoundWithEmailException(string email) : base($"there is not exist any user with this email : {email}")
    {
    }
}
