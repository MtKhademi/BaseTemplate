namespace Test.Integration.IAMModuleTest.Requests;

public record RoleCreateRequestTest(
    string? Name = default!,
    string? Description = default!);