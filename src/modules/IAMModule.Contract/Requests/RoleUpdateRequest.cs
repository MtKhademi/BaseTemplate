namespace IAMModule.Contract.Requests;

public record RoleUpdateRequest(
    string? roleId,
    string? Name,
    string? Description)
{
    public RoleUpdateCommand ToRoleUpdateCommand() => RoleUpdateCommand.Create(this);
}
