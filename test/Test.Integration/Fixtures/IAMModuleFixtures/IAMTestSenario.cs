namespace Test.Integration.Fixtures.IAMModuleFixtures;

public class IamTestScenario
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;


    public TokenResponseTest CurrentUserTokenResponse { get; set; }
    public List<ApplicationUserResponseTest> Users { get; set; } = [];
    public List<RoleResponseTest> Roles { get; set; } = [];

    public string? CurrentUserId { get; private set; }
    public string? CurrentUserName { get; private set; }
    public string? CurrentRoleId { get; private set; }

    public IamTestScenario(HttpClient client, ITestOutputHelper output)
    {
        _client = client;
        _output = output;
    }

    // -----------------------------------------------------------
    // LOGIN ADMIN
    // -----------------------------------------------------------
    public IamTestScenario LoginAdmin()
    {
        CurrentUserTokenResponse = _client.IAMLoginAdmin().GetAwaiter().GetResult();
        return this;
    }

    // -----------------------------------------------------------
    // LOGIN ADMIN
    // -----------------------------------------------------------
    public IamTestScenario LoginUser(string userName, string password)
    {
        CurrentUserTokenResponse = _client.IAMLoginUser(userName, password).GetAwaiter().GetResult();
        return this;
    }

    // -----------------------------------------------------------
    // CREATE ROLE
    // -----------------------------------------------------------
    public IamTestScenario CreateRole(
        string roleName = "TEST-ROLE",
        string description = "Test role description")
    {
        var role = _client.IAMCreateRole(roleName, description)
                          .GetAwaiter().GetResult();

        Roles.Add(role);
        return this;
    }

    // -----------------------------------------------------------
    // CREATE USER
    // -----------------------------------------------------------
    public IamTestScenario CreateUser(string username = "testuser",
        string Password = "P@ssw0rd", string ConfirmPassword = "P@ssw0rd",
        string email = "test@example.com", string phoneNumber = "09112223333",
        string firstName = "Test", string lastName = "User")
    {
        var req = new UserCreateRequestTest
        {
            Email = email,
            UserName = username,
            Password = Password,
            ConfirmPassword = ConfirmPassword,
            PhoneNumber = phoneNumber,
            FirstName = firstName,
            LastName = lastName
        };

        var user = _client.IAMCreateAUser(req)
                          .GetAwaiter().GetResult();

        Users.Add(user);

        return this;
    }

    // -----------------------------------------------------------
    // CHANGE CURRENT USER ROLE
    // -----------------------------------------------------------
    public IamTestScenario ChangeCurrentUserRole()
    {
        var request = new UserRolesChangeRequestTest(
            UserId: CurrentUserId!,
            RoleIds: [CurrentRoleId!]);

        var response = _client.PutAsync(
            $"/api/iam/v1/user/{CurrentUserId}/role-change/",
            request.ToContentHttp()
        ).GetAwaiter().GetResult();

        response.WriteOnConsoleAsync(_output)
                .GetAwaiter().GetResult();

        response.EnsureSuccessStatusCode();

        return this;
    }

    // -----------------------------------------------------------
    // GET CURRENT USER ROLES
    // -----------------------------------------------------------
    public List<UserRoleResponseTest> GetCurrentUserRoles()
    {
        var response = _client
            .GetAsync($"/api/IAM/v1/user/{CurrentUserName}/roles")
            .GetAwaiter().GetResult();

        response.WriteOnConsoleAsync(_output)
                .GetAwaiter().GetResult();

        var result = response.Content
            .ReadModelFromJsonAsync<ApiResultTest<List<UserRoleResponseTest>>>()
            .GetAwaiter().GetResult();

        return result!.Result!;
    }
}
