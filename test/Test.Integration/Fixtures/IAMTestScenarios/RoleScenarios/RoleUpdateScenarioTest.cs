using Test.Integration.TestScenarios;

namespace Test.Integration.Fixtures.IAMTestScenarios.RoleScenarios;

internal class RoleUpdateScenarioTest(
    RoleUpdateRequestTest request,
    Expression<Func<RoleResponseTest, bool>>? conditionChoose = null) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (string.IsNullOrWhiteSpace(request.RoleId))
        {
            if (conditionChoose is null)
            {
                var roleCreatedResponse = context.Get<ApiResultTest<RoleResponseTest>>(ScenarioDataKey.RoleCreate);
                if (roleCreatedResponse is not null)
                    request = request with
                    {
                        RoleId = roleCreatedResponse!.Result!.RoleId
                    };
            }
            else
            {
                var roles = context.GetList<RoleResponseTest>(ScenarioDataKey.Roles);
                var role = roles.SingleOrDefault(conditionChoose.Compile());
                if (role == null)
                    throw new ArgumentNullException("Not found any role with this condition");

                request = request with
                {
                    RoleId = role.RoleId
                };
            }
        }

        var apiResult = await context.ApiPutRequestAsync<RoleUpdateRequestTest, RoleResponseTest>(
            url: "/iam/api/v1/roles",
            storeKey: ScenarioDataKey.RoleUpdate,
            request: request,
            scenarioName: GetType().Name);
       
    }
}

internal static class RoleUpdateScenarioTestExtensions
{

    public static ScenarioRunner RoleUpdate(this ScenarioRunner runner,
        string? roleId = default!,
        string? roleName = default,
        string? roleDescription = default!,
        Expression<Func<RoleResponseTest, bool>>? conditionChoose = null)
        => RoleUpdate(runner, new RoleUpdateRequestTest(
            RoleId: roleId,
            Name: roleName,
            Description: roleDescription));

    public static ScenarioRunner RoleUpdate(this ScenarioRunner runner, RoleUpdateRequestTest request,
        Expression<Func<RoleResponseTest, bool>>? conditionChoose = null)
        => runner.AddScenario(new RoleUpdateScenarioTest(request,
            conditionChoose: conditionChoose));

}