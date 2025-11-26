namespace Test.Integration.IAMModuleTest.Responses;

public record UserRoleResponseTest(
    string? UserName = default!,
    string UserId = default!,
    string RoleId = default!,
    string RoleName = default!,
    string RoleDescription = default!
);