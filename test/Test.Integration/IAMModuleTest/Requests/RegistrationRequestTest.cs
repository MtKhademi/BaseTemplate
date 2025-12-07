namespace Test.Integration.IAMModuleTest.Requests;

public record RegistrationRequestTest(
   string? UserName,
   string? Password,
   string? ConfirmPassword);

