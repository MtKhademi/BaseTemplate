namespace Test.Integration.IAMModuleTest.Requests;

public record UserRolesChangeRequestTest(
    string? UserId = default!,
    string[]? RoleIds = default!
);