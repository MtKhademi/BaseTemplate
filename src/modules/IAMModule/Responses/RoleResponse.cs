namespace IAMModule.Contract.Responses;

public class RoleResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public static class RoleResponseExtentions
{
    public static RoleResponse ToRoleResponse(this ApplicationRole role)
    {
        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }
}