namespace IAMModule.Contract.Commands;

public record UserCreateCommand : ICommand<ApplicationUserModel>
{
    public string UserName { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    private UserCreateCommand(
        string? userName,
        string? password, string? confirmPassword,
        string? email = default!,
        string? firstName = default!, string? lastName = default!,
        string? phoneNumber = default!)
    {

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(userName))
            errors.Add($"{nameof(userName)} is required.");
        if (string.IsNullOrWhiteSpace(password))
            errors.Add($"{nameof(password)} is required.");
        if (password != confirmPassword)
            errors.Add("Passwords do not match.");

        if (errors.Any())
            throw new UserCreateCommandException(errors);

        UserName = userName!;
        Password = password!;
        ConfirmPassword = confirmPassword!;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }


    public static UserCreateCommand Create(RegistrationRequest request)
        => new UserCreateCommand(
            request.UserName ?? "",
            request.Password ?? "",
            request.ConfirmPassword ?? "");

    public static UserCreateCommand Create(UserCreateRequest request)
        => new UserCreateCommand(
           userName: request.UserName ?? "",
           password: request.Password ?? "",
           confirmPassword: request.ConfirmPassword ?? "",
           email: request.Email ?? "",
           firstName: request.FirstName ?? "",
           lastName: request.LastName ?? "",
           phoneNumber: request.PhoneNumber ?? "");

    internal class UserCreateCommandException : NotValidDataException<UserCreateCommandException>
    {
        public UserCreateCommandException(IEnumerable<string> errors) :
            base(errors)
        {
        }
    }
}