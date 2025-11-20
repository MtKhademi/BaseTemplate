namespace IAMModule.Contract.Requests;

public record RoleCreateRequest(
    string? Name,
    string? Description)
{
    public RoleCreateCommand ToRoleCreateCommand() => RoleCreateCommand.Create(this);
}
