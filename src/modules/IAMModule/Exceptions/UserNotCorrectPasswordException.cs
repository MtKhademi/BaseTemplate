namespace IAMModule.Exceptions;

public class UserNotCorrectPasswordException : NotValidDataException<UserNotCorrectPasswordException>
{
    public UserNotCorrectPasswordException(string email) : base($"This user : {email} has provided a wrong password")
    {
    }
}
