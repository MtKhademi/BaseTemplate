namespace UserManagementModule.Contract.Requests;

public record UserRegistrationRequest(
    string? Email,
    string? UserName,
    string? Password,
    string? ConfirmPassword,
    string? PhoneNumber,
    string? FirstName,
    string? LastName
)
{
    public UseRegistrationCommand ToCommand()
    {

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(Email))
            errors.Add("Email is required.");

        if (string.IsNullOrWhiteSpace(UserName))
            errors.Add("User name is required.");

        if (string.IsNullOrWhiteSpace(Password))
            errors.Add("Password is required.");

        if (Password != ConfirmPassword)
            errors.Add("Passwords do not match.");

        if (errors.Any())
            throw new UserRegistrationRequestException(errors);


        return new UseRegistrationCommand(
            email: Email!,
            userName: UserName!,
            password: Password!,
            confirmPassword: ConfirmPassword!,
            firstName: FirstName!,
            lastName: LastName!,
            phoneNumber: PhoneNumber!
        );
    }

    internal class UserRegistrationRequestException : NotValidDataException<UserRegistrationRequestException>
    {
        public UserRegistrationRequestException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}