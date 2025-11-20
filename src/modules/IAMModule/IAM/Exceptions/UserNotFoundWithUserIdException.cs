namespace IAMModule.IAM.Exceptions;

public class UserNotFoundWithUserIdException : NotFoundException<UserNotFoundWithUserIdException>
{
    public UserNotFoundWithUserIdException(string userId) : base($"there is not exist any user with this userId : {userId}")
    {
    }
}
