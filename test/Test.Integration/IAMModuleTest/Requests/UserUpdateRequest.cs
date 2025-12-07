namespace Test.Integration.IAMModuleTest.Requests;

public record UserUpdateRequestTest(
    string? UserId = default!,
    string? Email = default!,
    string? UserName = default!,
    string? Password = default!,
    string? ConfirmPassword = default!,
    string? PhoneNumber = default!,
    string? FirstName = default!,
    string? LastName = default!
);