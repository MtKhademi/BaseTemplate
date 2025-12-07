namespace IAMModule.Contract.Responses;

public record UserRoleResponse(
    string UserName,
    string UserId,
    IEnumerable<RoleResponse> Roles
);