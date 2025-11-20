namespace IAMModule.Contract.Models;

public record RoleModel(
    string Id,
    string Name,
    string Description)
{
    public RoleResponse ToRoleResponse() => new RoleResponse(Id, Name, Description);
}