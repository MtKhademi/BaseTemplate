namespace Test.Integration.IAMModuleTest.Responses;

public record RoleResponseTest(
    string? RoleId = default!,
    string? RoleName = default!,
    string? RoleDescription = default!);