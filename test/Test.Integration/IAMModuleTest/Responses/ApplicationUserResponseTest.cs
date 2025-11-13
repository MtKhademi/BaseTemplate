namespace Test.Integration.IAMModuleTest.Responses;

public record ApplicationUserResponseTest(
   string? UserId = default!,
   string? FirstName = default!,
   string? LastName = default!,
   string? Email = default!,
   string? UserName = default!,
   string? PhoneNumber = default!,
   bool? IsActive = default!,
   bool? EmailConfirmed = default!
);
