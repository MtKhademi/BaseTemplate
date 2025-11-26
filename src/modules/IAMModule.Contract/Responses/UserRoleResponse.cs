namespace IAMModule.Contract.Responses;

public record UserRoleResponse(
    string UserName,
    string UserId,
    string RoleId,
    string RoleName,
    string RoleDescription
);