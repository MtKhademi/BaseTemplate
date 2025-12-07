namespace Test.Integration.IAMModuleTest.Responses;

public record UserRoleResponseTest(
    string? UserName = default!,
    string UserId = default!,
    IEnumerable<RoleResponseTest>? Roles = default!
);