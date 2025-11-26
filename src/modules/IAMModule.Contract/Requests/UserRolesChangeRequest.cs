namespace IAMModule.Contract.Requests;

public record UserRolesChangeRequest(
    string? UserId = default!,
    string[]? RoleIds = default!
);