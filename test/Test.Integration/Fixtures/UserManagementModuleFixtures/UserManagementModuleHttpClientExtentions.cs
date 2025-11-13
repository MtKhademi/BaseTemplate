namespace Test.Integration.Fixtures.UserManagementModuleFixtures;

internal static class UserManagementModuleHttpClientExtentions
{
    internal static async Task<ApplicationUserResponseTest> IAMRegister(this HttpClient client, 
        string email = "test@example.com",
        string userName = "testuser", string phoneNumber = "1234567890",
        string password = "P@ssw0rd", string confirmPassword = "P@ssw0rd")
        => await client.IAMRegister(new UserRegistrationRequestTest(
            Email: email,
            UserName: userName,
            Password: password,
            ConfirmPassword: confirmPassword,
            PhoneNumber: phoneNumber,
            FirstName: "Test",
            LastName: "User"
        ));
    internal static async Task<ApplicationUserResponseTest> IAMRegister(this HttpClient client, UserRegistrationRequestTest dto)
    {
        var api = $"/api/iam/v1/register";

        var response = await client.PostAsync(api, dto.ToContentHttp());
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<ApplicationUserResponseTest>>();
        apiResult.Should().NotBeNull();
        var user = apiResult!.Result;
        user.Should().NotBeNull();
        return user!;
    }
}
