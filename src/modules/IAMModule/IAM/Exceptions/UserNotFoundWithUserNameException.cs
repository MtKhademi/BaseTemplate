namespace IAMModule.IAM.Exceptions;

public class UserNotFoundWithUserNameException : NotFoundException<UserNotFoundWithUserNameException>
{
    public UserNotFoundWithUserNameException(string userName) : base($"there is not exist any user with this userName : {userName}")
    {
    }
}
