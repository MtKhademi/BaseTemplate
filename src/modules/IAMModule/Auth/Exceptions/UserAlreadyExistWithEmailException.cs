namespace IAMModule.Auth.Exceptions;

public class UserAlreadyExistWithEmailException : AlreadyExistException<UserAlreadyExistWithEmailException>
{
    public UserAlreadyExistWithEmailException(string email) : base($"This user : {email} already exists")
    {
    }
}
