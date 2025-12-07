namespace IAMModule.Contract.Requests;

public record UserUpdateRequest(
    string? UserId = default!,
    string? Email = default!,
    string? UserName = default!,
    string? Password = default!,
    string? ConfirmPassword = default!,
    string? PhoneNumber = default!,
    string? FirstName = default!,
    string? LastName = default!
)
{
    public UserUpdateCommand ToUserUpdateCommand() => UserUpdateCommand.Create(this);

}