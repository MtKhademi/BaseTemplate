namespace IAMModule.Contract.Requests;

public class LoginRequest
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            throw new LoginRequestException("نام کاربری الزامی است.");
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            throw new LoginRequestException("رمز عبور الزامی است.");
        }

        if (Password.Length < 6)
        {
            throw new LoginRequestException("رمز عبور باید حداقل 6 کاراکتر باشد.");
        }
    }


    public LoginCommand ToCommand()
    {
        Validate();
        return new LoginCommand(UserName!, Password!);
    }

    internal class LoginRequestException : NotValidDataException
    {
        public LoginRequestException(string message) : base(message, nameof(LoginRequestException))
        {
        }
    }
}

