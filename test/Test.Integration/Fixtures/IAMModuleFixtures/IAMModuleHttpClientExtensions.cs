namespace Test.Integration.Fixtures.IAMModuleFixtures;

internal static class IAMModuleHttpClientExtensions
{

    public const string DefaultUserPassword = "P@ssw0rd";
    public const string DefaultUserName = "testuser";

    internal static async Task<ApplicationUserResponseTest> IAMRegister(this HttpClient client,
        string userName = DefaultUserName,
        string password = DefaultUserPassword, string confirmPassword = DefaultUserPassword)
        => await client.IAMRegister(new UserRegistrationRequestTest(
            UserName: userName,
            Password: password,
            ConfirmPassword: confirmPassword));
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
    internal static async Task<ApplicationUserResponseTest> IAMCreateAUser(this HttpClient client,
        string userName,
        string password,
        string confirmPassword,
        string? email = default!,
        string? phone = default!,
        string? firstName = default!,
        string? lastName = default!)
    {
        var api = $"/api/iam/v1/user";
        var response = await client.PostAsync(api, new UserCreateRequestTest
        {
            UserName = userName,
            Password = password,
            ConfirmPassword = confirmPassword,
            Email = email,
            PhoneNumber = phone,
            FirstName = firstName,
            LastName = lastName
        }.ToContentHttp());
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
    internal static async Task IAMRegisterUserAndLoginUser(this HttpClient client,
        string userName = DefaultUserName,
        string password = DefaultUserPassword)
    {
        var user = await client.IAMRegister(userName: userName, password: password);
        await client.IAMLoginUser(userName: userName, password: password);
    }

    internal static async Task<RoleResponseTest> IAMCreateRole(this HttpClient client,
        string roleName,
        string roleDescription = "")
    {
        var api = $"/api/iam/v1/role";
        var response = await client.PostAsync(api, new RoleCreateRequestTest
        {
            Name = roleName,
            Description = roleDescription
        }.ToContentHttp());
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        apiResult.Should().NotBeNull();
        var role = apiResult!.Result;
        role.Should().NotBeNull();
        return role!;
    }
}
