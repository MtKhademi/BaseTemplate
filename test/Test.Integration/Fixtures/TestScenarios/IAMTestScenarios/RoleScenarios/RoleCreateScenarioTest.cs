namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.RoleScenarios;

internal class RoleCreateScenarioTest : IRestApiScenarioTest
{
    public string ApiEndpoint => $"/iam/api/v1/roles";
    private readonly RoleCreateRequestTest _request;
    public RoleCreateScenarioTest(RoleCreateRequestTest request)
    {
        _request = request;
    }


    public async Task ExecuteAsync(ScenarioContext context)
    {
        var response = await context.Client.PostAsync(ApiEndpoint, _request.ToContentHttp());
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "Role Create");

        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
            return;

        var role = await response.Content.ReadModelFromJsonAsync<ApiResultTest<RoleResponseTest>>();
        context.Set(ScenarioDataKey.RoleCreate, role!);
        context.AddToList(ScenarioDataKey.Roles, role!.Result);

    }
}

internal static class RoleCreateScenarioTestExtensions
{

    public static ScenarioRunner RoleCreate(this ScenarioRunner runner,
        string? roleName = default!,
        string? roleDescription = default!)
        => RoleCreate(runner, new RoleCreateRequestTest(Name: roleName, Description: roleDescription));

    public static ScenarioRunner RoleCreate(this ScenarioRunner runner, RoleCreateRequestTest request)
        => runner.AddScenario(new RoleCreateScenarioTest(request));
}