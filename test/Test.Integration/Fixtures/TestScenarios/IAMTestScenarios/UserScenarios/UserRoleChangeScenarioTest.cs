using Infrastructure.Web.ApiResult;

namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserRoleChangeScenarioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/users/[userId]/roles";
    private UserRolesChangeRequestTest _request;
    private Func<ApplicationUserResponseTest, bool>? _userSelecte;
    private Func<RoleResponseTest, bool>? _roleSelecte;

    public UserRoleChangeScenarioTest(UserRolesChangeRequestTest request,
        Expression<Func<ApplicationUserResponseTest, bool>>? userSelecte,
        Expression<Func<RoleResponseTest, bool>>? roleSelecte)
    {
        _request = request;
        _userSelecte = userSelecte?.Compile();
        _roleSelecte = roleSelecte?.Compile();
    }


    public async Task ExecuteAsync(ScenarioContext context)
    {

        if (_userSelecte is not null)
        {
            var users = context.GetList<ApplicationUserResponseTest>(ScenarioDataKey.Users);
            var user = users.FirstOrDefault(_userSelecte);
            if (user is not null)
            {
                _request = _request with
                {
                    UserId = user.UserId
                };
            }
            else
            {
                throw new InvalidOperationException("No user found matching the selection criteria.");
            }
        }

        if (_roleSelecte is not null)
        {
            var roles = context.GetList<RoleResponseTest>(ScenarioDataKey.Roles);
            var role = roles.FirstOrDefault(_roleSelecte);
            if (role is not null)
            {
                _request = _request with
                {
                    RoleIds = [role.RoleId]
                };
            }
            else
            {
                throw new InvalidOperationException("No role found matching the selection criteria.");
            }
        }

        var response = await context.Client.PutAsync(
            ApiEndpoint.Replace("[userId]", _request.UserId), _request.ToContentHttp());
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "User role update");

        context.SetLastResponse(response);

        var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<UserRoleResponseTest>>();
        context.Set(ScenarioDataKey.UserRoleChangeResponse, apiResult);
    }
}

internal static class UserRoleChangeScenarioTestExtensions
{
    public static ScenarioRunner UserRoleChange(this ScenarioRunner runner,
        UserRolesChangeRequestTest request,
        Expression<Func<ApplicationUserResponseTest, bool>>? userSelecte,
        Expression<Func<RoleResponseTest, bool>>? roleSelecte)
        => runner.AddScenario(new UserRoleChangeScenarioTest(request,
            userSelecte, roleSelecte));


}