namespace IAMModule.Exceptions;


public class UserAlreadyExistWithPhoneException : AlreadyExistException<UserAlreadyExistWithPhoneException>
{
    public UserAlreadyExistWithPhoneException(string phoneNumber) : base($"This user : {phoneNumber} already exists")
    {
    }
}
