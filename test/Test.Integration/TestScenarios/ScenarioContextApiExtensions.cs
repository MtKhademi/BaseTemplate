using Test.Integration.Fixtures;

namespace Test.Integration.TestScenarios;

public static class ScenarioContextApiExtensions
{
    #region Get request
    public static async Task<ApiResultTest<TResult>?> ApiGetRequestAsync<TResult>(
        this ScenarioContext context,
        string url,
        ScenarioDataKey storeKey,
        ScenarioDataKey? storeListKey = null,
        string? scenarioName = default!) => await context.ApiRequestAsync<object, TResult>(
            method: HttpMethod.Get,
            url: url,
            request: null,
            storeKey: storeKey,
            storeListKey: storeListKey,
            scenarioName: scenarioName);

    #endregion

    #region Delete request
    public static async Task<ApiResultTest<TResult>?> ApiDeleteRequestAsync<TResult>(
        this ScenarioContext context,
        string url,
        ScenarioDataKey storeKey,
        string? scenarioName = default!) => await context.ApiRequestAsync<object, TResult>(
            method: HttpMethod.Delete,
            url: url,
            request: null,
            storeKey: storeKey,
            scenarioName: scenarioName);

    #endregion

    #region Patch request
    public static async Task<ApiResultTest<TResult>?> ApiPatchRequestAsync<TResult>(
        this ScenarioContext context,
        string url,
        ScenarioDataKey storeKey,
        string? scenarioName = default!) => await context.ApiPatchRequestAsync<object, TResult>(
            url: url,
            request: null!,
            storeKey: storeKey,
            scenarioName: scenarioName);

    public static async Task<ApiResultTest<TResult>?> ApiPatchRequestAsync<TRequest, TResult>(
        this ScenarioContext context,
        string url,
        TRequest request,
        ScenarioDataKey storeKey,
        string? scenarioName = default!) => await context.ApiRequestAsync<TRequest, TResult>(
            method: HttpMethod.Patch,
            url: url,
            request: request,
            storeKey: storeKey,
            scenarioName: scenarioName ?? typeof(TRequest).Name);

    #endregion

    #region Put request

    public static async Task<ApiResultTest<TResult>?> ApiPutRequestAsync<TRequest, TResult>(
        this ScenarioContext context,
        string url,
        TRequest request,
        ScenarioDataKey storeKey,
        string? scenarioName = default!) => await context.ApiRequestAsync<TRequest, TResult>(
            method: HttpMethod.Put,
            url: url,
            request: request,
            storeKey: storeKey,
            scenarioName: scenarioName ?? typeof(TRequest).Name);

    #endregion

    #region Post request
    public static async Task<ApiResultTest<TResult>?> ApiPostRequestAsync<TResult>(
        this ScenarioContext context,
        string url,
        ScenarioDataKey storeKey,
        string? scenarioName = default!) => await context.ApiRequestAsync<object, TResult>(
            method: HttpMethod.Post,
            url: url,
            request: null,
            storeKey: storeKey,
            scenarioName: scenarioName);

    public static async Task<ApiResultTest<TResult>?> ApiPostRequestAsync<TRequest, TResult>(
        this ScenarioContext context,
        string url,
        TRequest request,
        ScenarioDataKey storeKey,
        string? scenarioName = default!) => await context.ApiRequestAsync<TRequest, TResult>(
            method: HttpMethod.Post,
            url: url,
            request: request,
            storeKey: storeKey,
            scenarioName: scenarioName ?? typeof(TRequest).Name);

    #endregion

    public static async Task<ApiResultTest<TResult>?> ApiRequestAsync<TRequest, TResult>(
        this ScenarioContext context,
        HttpMethod method,
        string url,
        TRequest request,
        string scenarioName,
        ScenarioDataKey storeKey,
        ScenarioDataKey? storeListKey = null)
    {
        var message = new HttpRequestMessage(method, url)
        {
            Content = request?.ToContentHttp()
        };

        var response = await context.Client.SendAsync(message);
        await response.WriteOnConsoleAsync(context.TestOutputHelper, scenarioName);
        context.SetLastResponse(response);

        if (!response.IsSuccessStatusCode)
            return default;

        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<TResult>>();
        context.Set(storeKey, apiResult);

        if(storeListKey is not null)
        {
            context.AddToList(storeListKey.Value, apiResult);
        }

        return apiResult;
    }
}