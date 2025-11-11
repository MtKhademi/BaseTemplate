using IAMModule.Contract.Models;

namespace IAMModule.Contract.Commands;

public record UseRegistrationCommand : ICommand<ApplicationUserModel>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string PhoneNumber { get; init; }
    public bool Activate { get; init; }
    public bool AutoConfirmEmail { get; init; }

    public UseRegistrationCommand(string firstName, string lastName, string email, string userName, string password, string confirmPassword, string phoneNumber, bool activate, bool autoConfirmEmail)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        Password = password;
        ConfirmPassword = confirmPassword;
        PhoneNumber = phoneNumber;
        Activate = activate;
        AutoConfirmEmail = autoConfirmEmail;
    }
}