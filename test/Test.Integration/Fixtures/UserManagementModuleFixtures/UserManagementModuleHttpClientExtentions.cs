namespace Test.Integration.Fixtures.UserManagementModuleFixtures;

internal static class UserManagementModuleHttpClientExtentions
{

    public const string DefaultUserPassword = "P@ssw0rd";
    public const string DefaultUserName = "testuser";

    internal static async Task<ApplicationUserResponseTest> IAMRegister(this HttpClient client,
        string email = "test@example.com",
        string userName = DefaultUserName, string phoneNumber = "1234567890",
        string password = DefaultUserPassword, string confirmPassword = DefaultUserPassword,
        string firstName = "testUser", string lastName = "testUser")
        => await client.IAMRegister(new UserRegistrationRequestTest(
            Email: email,
            UserName: userName,
            Password: password,
            ConfirmPassword: confirmPassword,
            PhoneNumber: phoneNumber,
            FirstName: firstName,
            LastName: lastName
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




    internal static async Task IAMLoginAdmin(this HttpClient client) => await client.IAMLoginUser(userName: "admin", password: "8585@8585");
    internal static async Task IAMLoginUser(this HttpClient client,
        string userName = DefaultUserName,
        string password = DefaultUserPassword)
    {
        var response = await client.PostAsync($"/api/iam/v1/login", new LoginRequestTest
        (
            UserName: userName,
            Password: password
        ).ToContentHttp());

        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<TokenResponseTest>>();
        apiResult.Should().NotBeNull();
        var user = apiResult!.Result;
        user.Should().NotBeNull();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiResult!.Result!.Token);
    }

}
