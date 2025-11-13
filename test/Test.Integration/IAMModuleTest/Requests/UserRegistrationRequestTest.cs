namespace Test.Integration.IAMModuleTest.Requests;

public record UserRegistrationRequestTest(
   string? Email,
   string? UserName,
   string? Password,
   string? ConfirmPassword,
   string? PhoneNumber,
   string? FirstName,
   string? LastName
);

