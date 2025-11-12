namespace IAMModule.Contract.Requests;

public class UserRegistrationRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? Activate { get; set; }
    public bool? AutoConfirmEmail { get; set; }
    public void Validate()
    {

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(FirstName))
            errors.Add("First name is required.");

        if (string.IsNullOrWhiteSpace(LastName))
            errors.Add("Last name is required.");

        if (string.IsNullOrWhiteSpace(Email))
            errors.Add("Email is required.");

        if (string.IsNullOrWhiteSpace(UserName))
            errors.Add("User name is required.");

        if (string.IsNullOrWhiteSpace(Password))
            errors.Add("Password is required.");

        if (Password != ConfirmPassword)
            errors.Add("Passwords do not match.");

        if (Activate == null)
            errors.Add("Activate status is required.");

        if (AutoConfirmEmail == null)
            errors.Add("Auto confirm email status is required.");

        if (errors.Any())
            throw new UserRegistrationRequestException(errors);
    }

    public UseRegistrationCommand ToCommand() =>
        new UseRegistrationCommand(FirstName, LastName, Email, UserName, Password, ConfirmPassword, PhoneNumber, Activate.Value, AutoConfirmEmail.Value);

    internal class UserRegistrationRequestException : NotValidDataException<UserRegistrationRequestException>
    {
        public UserRegistrationRequestException(IEnumerable<string> errors) :
            base(errors)
        {
        }
    }
}

