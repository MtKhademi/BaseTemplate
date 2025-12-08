namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserRoleChangeScenarioTest(
    UserRolesChangeRequestTest request,
    Expression<Func<ApplicationUserResponseTest, bool>>? userSelecte,
    Expression<Func<RoleResponseTest, bool>>? roleSelecte) : IScenarioStep
{

    public async Task ExecuteAsync(ScenarioContext context)
    {

        if (userSelecte is not null)
        {
            var users = context.GetList<ApplicationUserResponseTest>(ScenarioDataKey.Users);
            var user = users.FirstOrDefault(userSelecte.Compile());
            if (user is not null)
            {
                request = request with
                {
                    UserId = user.UserId
                };
            }
            else
            {
                throw new InvalidOperationException("No user found matching the selection criteria.");
            }
        }

        if (roleSelecte is not null)
        {
            var roles = context.GetList<RoleResponseTest>(ScenarioDataKey.Roles);
            var role = roles.FirstOrDefault(roleSelecte.Compile());
            if (role is not null)
            {
                request = request with
                {
                    RoleIds = [role.RoleId!]
                };
            }
            else
            {
                throw new InvalidOperationException("No role found matching the selection criteria.");
            }
        }

        var apiResult = await context.ApiPutRequestAsync<UserRolesChangeRequestTest, UserRoleResponseTest>(
            url: "/iam/api/v1/users/[userId]/roles".Replace("[userId]", request.UserId),
            storeKey: ScenarioDataKey.UserRoleChangeResponse,
            request: request,
            scenarioName: GetType().Name);
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