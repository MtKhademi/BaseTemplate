namespace Test.Integration.IAMModuleTest.Requests;

public record RoleUpdateRequestTest(
    string? RoleId = default!,
    string? Name = default!,
    string? Description = default!);