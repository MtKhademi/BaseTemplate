namespace Test.Integration.Fixtures.TestScenarios.IAMTestScenarios.UserScenarios;

internal class UserUpdateScenarioTest : IScenarioStep
{
    public string ApiEndpoint => $"/iam/api/v1/users";
    private UserUpdateRequestTest _request;
    private Func<ApplicationUserResponseTest, bool>? _userSelecte;
    private int? _userIndex = null;
    public UserUpdateScenarioTest(UserUpdateRequestTest request,
        Expression<Func<ApplicationUserResponseTest, bool>>? userSelecte,
        int? userIndex)
    {
        _request = request;
        _userSelecte = userSelecte?.Compile();
        _userIndex = userIndex;
    }


    public async Task ExecuteAsync(ScenarioContext context)
    {

        var users = context.GetList<ApplicationUserResponseTest>(ScenarioDataKey.Users);
        if (_userSelecte is not null)
        {
            var user = users.SingleOrDefault(_userSelecte);
            if (user is not null)
            {
                _request = _request with
                {
                    UserId = user.UserId
                };
            }
        }
        else if (_userIndex is not null)
        {
            if (_userIndex.Value >= users.Count)
                throw new IndexOutOfRangeException($"User index {_userIndex.Value} is out of range. Total users: {users.Count}");
            var user = users[_userIndex.Value];
            _request = _request with
            {
                UserId = user.UserId
            };
        }

        var response = await context.Client.PutAsync($"{ApiEndpoint}/{_request.UserId}", _request.ToContentHttp());
        await response.WriteOnConsoleAsync(context.TestOutputHelper, "User update");

        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
            return;

        var createdUser = await response.Content.ReadFromJsonAsync<ApiResultTest<ApplicationUserResponseTest>>();
        context.Set(ScenarioDataKey.UserUpdate, createdUser);

        var userUpdated = createdUser!.Result!;
        users.RemoveAll(us => us.UserId == userUpdated.UserId);
        users.Add(userUpdated);
        context.Set(ScenarioDataKey.Users, users);
    }
}

internal static class UserUpdateScenarioTestExtensions
{

    public static ScenarioRunner UserUpdate(this ScenarioRunner runner,
        UserUpdateRequestTest request,
        Expression<Func<ApplicationUserResponseTest, bool>>? userSelecte = default!,
        int? userIndex = null)
        => runner.AddScenario(new UserUpdateScenarioTest(
            request: request,
            userSelecte: userSelecte,
            userIndex: userIndex));
}