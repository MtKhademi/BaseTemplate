namespace Test.Integration.IAMModuleTest.Requests;

public record UserRegistrationRequestTest(
   string? UserName,
   string? Password,
   string? ConfirmPassword);

