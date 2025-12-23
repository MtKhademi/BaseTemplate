namespace IAMModule.Auth.Exceptions;

public class UserNotEmailConfirmException : NotValidDataException<UserNotEmailConfirmException>
{
    public UserNotEmailConfirmException(string email) : base($"this user : {email} is not confirm email, please call admin")
    {
    }
}
