namespace Test.Integration.IAMModuleTest.Requests;

public record ChangePasswordRequestTest(
    string? CurrentPassword = default!,
    string? NewPassword = default!,
    string? ConfirmNewPassword = default!);