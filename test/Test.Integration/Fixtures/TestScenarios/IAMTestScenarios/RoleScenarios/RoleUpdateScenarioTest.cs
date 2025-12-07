namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.RoleScenarios;

internal class RoleUpdateScenarioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/roles";
    private RoleUpdateRequestTest _request;
    private Func<RoleResponseTest, bool>? _conditionChoose = null;
    public RoleUpdateScenarioTest(RoleUpdateRequestTest request,
        Expression<Func<RoleResponseTest, bool>>? conditionChoose = null)
    {
        _request = request;
        _conditionChoose = conditionChoose?.Compile() ?? null;
    }


    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (string.IsNullOrWhiteSpace(_request.RoleId))
        {
            if (_conditionChoose is null)
            {
                var roleCreatedResponse = context.Get<ApiResultTest<RoleResponseTest>>(ScenarioDataKey.RoleCreate);
                if (roleCreatedResponse is not null)
                    _request = _request with
                    {
                        RoleId = roleCreatedResponse!.Result!.RoleId
                    };
            }
            else
            {
                var roles = context.GetList<RoleResponseTest>(ScenarioDataKey.Roles);
                var role = roles.SingleOrDefault(_conditionChoose);
                if (role == null)
                    throw new ArgumentNullException("Not found any role with this condition");

                _request = _request with
                {
                    RoleId = role.RoleId
                };
            }
        }

        var response = await context.Client.PutAsync(ApiEndpoint, _request.ToContentHttp());
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Role update");

        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var responseContent = await response.Content.ReadFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        context.Set(ScenarioDataKey.RoleUpdate, responseContent);
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