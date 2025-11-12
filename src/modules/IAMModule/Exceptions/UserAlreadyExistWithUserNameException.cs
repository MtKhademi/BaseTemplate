namespace IAMModule.Exceptions;

public class UserAlreadyExistWithUserNameException : AlreadyExistException<UserAlreadyExistWithUserNameException>
{
    public UserAlreadyExistWithUserNameException(string userName) : base($"This user : {userName} already exists")
    {
    }
}
