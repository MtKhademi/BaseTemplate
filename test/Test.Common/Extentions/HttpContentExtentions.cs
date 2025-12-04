namespace Test.Infrastructure.Extentions;

public static class HttpContentExtentions
{
    public static async Task WriteOnConsoleAsync(this HttpResponseMessage response, ITestOutputHelper outPut)
    {
        outPut.WriteLine($"" +
            $"============ RESPONSE ==================== \n" +
            $" -> STATUS : {response.StatusCode}\n" +
            $" -> VALUE : " +
            $"{await response.Content.ReadAsStringAsync()}");
    }

}
