using IAMModule.Contract.Models;
using IAMModule.Contract.Requests;

namespace IAMModule.Contract.Commands;

public record UseRegistrationCommand : ICommand<ApplicationUserModel>
{
    public string Email { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string PhoneNumber { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public UseRegistrationCommand(
        string email, 
        string userName,
        string password, string confirmPassword,
        string firstName, string lastName, 
        string phoneNumber)
    {

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(email))
            errors.Add($"{nameof(email)} is required.");
        if (string.IsNullOrWhiteSpace(userName))
            errors.Add($"{nameof(userName)} is required.");
        if (string.IsNullOrWhiteSpace(password))
            errors.Add($"{nameof(password)} is required.");
        if (password != confirmPassword)
            errors.Add("Passwords do not match.");


        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        Password = password;
        ConfirmPassword = confirmPassword;
        PhoneNumber = phoneNumber;
    }

    internal class UseRegistrationCommandException : NotValidDataException<UseRegistrationCommandException>
    {
        public UseRegistrationCommandException(IEnumerable<string> errors) :
            base(errors)
        {
        }
    }
}