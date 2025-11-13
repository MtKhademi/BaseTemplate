namespace Test.Integration.IAMModuleTest.Responses;

public record TokenResponseTest(
string? Token = default!,
string? RefreshToken = default!,
string? RefreshTokenExpiryTime = default!);
