namespace IAMModule.Contract.Requests;

public record RegistrationRequest(
    string? UserName,
    string? Password,
    string? ConfirmPassword)
{
    public UserCreateCommand ToCommand() => UserCreateCommand.Create(this);

}