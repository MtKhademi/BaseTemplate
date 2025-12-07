namespace IAMModule.Contract.Commands;

public record UserUpdateCommand : ICommand<ApplicationUserModel>
{
    public string UserId { get; init; }
    public string? UserName { get; init; } = default!;
    public string? Password { get; init; } = default!;
    public string? ConfirmPassword { get; init; } = default!;
    public string? Email { get; init; } = default!;
    public string? PhoneNumber { get; init; } = default!;
    public string? FirstName { get; init; } = default!;
    public string? LastName { get; init; } = default!;

    private UserUpdateCommand(
        string userId,
        string? userName = default!,
        string? password = default!, string? confirmPassword = default!,
        string? email = default!,
        string? firstName = default!, string? lastName = default!,
        string? phoneNumber = default!)
    {

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(userId))
            errors.Add($"{nameof(userId)} is required.");
        if (!string.IsNullOrWhiteSpace(password) &&
                password != confirmPassword)
            errors.Add($"{nameof(password)} and {nameof(confirmPassword)} is not matched.");

        if (errors.Any())
            throw new UserUpdateCommandException(errors);

        UserId = userId!;
        UserName = userName;
        Password = password;
        ConfirmPassword = confirmPassword;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }


    public static UserUpdateCommand Create(UserUpdateRequest request)
        => new UserUpdateCommand(
            userId: request.UserId ?? "",
           userName: request.UserName ?? "",
           password: request.Password ?? "",
           confirmPassword: request.ConfirmPassword ?? "",
           email: request.Email ?? "",
           firstName: request.FirstName ?? "",
           lastName: request.LastName ?? "",
           phoneNumber: request.PhoneNumber ?? "");

    internal class UserUpdateCommandException : NotValidDataException<UserUpdateCommandException>
    {
        public UserUpdateCommandException(IEnumerable<string> errors) :
            base(errors)
        {
        }
    }
}