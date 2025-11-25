namespace IAMModule.Contract.Requests;

public record UserUpdateRequest(
    string? Email = default!,
    string? UserName = default!,
    string? Password = default!,
    string? ConfirmPassword = default!,
    string? PhoneNumber = default!,
    string? FirstName = default!,
    string? LastName = default!
)
{
    public UserUpdateCommand ToUserUpdateCommand(string userId) => UserUpdateCommand.Create(this, userId);

}