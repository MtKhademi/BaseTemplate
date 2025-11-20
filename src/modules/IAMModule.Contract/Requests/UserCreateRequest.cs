namespace IAMModule.Contract.Requests;

public record UserCreateRequest(
    string? Email,
    string? UserName,
    string? Password,
    string? ConfirmPassword,
    string? PhoneNumber,
    string? FirstName,
    string? LastName
)
{
    public UserCreateCommand ToUserCreateCommand() => UserCreateCommand.Create(this);

}