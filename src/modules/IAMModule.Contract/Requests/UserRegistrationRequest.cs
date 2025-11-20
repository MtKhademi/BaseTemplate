namespace IAMModule.Contract.Requests;

public record UserRegistrationRequest(
    string? UserName,
    string? Password,
    string? ConfirmPassword)
{
    public UserCreateCommand ToCommand() => UserCreateCommand.Create(this);

}