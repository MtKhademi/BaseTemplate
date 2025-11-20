namespace IAMModule.IAM.Exceptions;

public class UserDontActiveException : NotValidDataException<UserDontActiveException>
{
    public UserDontActiveException(string email) : base($"This user : {email} is not active, please call to admin")
    {
    }
}
