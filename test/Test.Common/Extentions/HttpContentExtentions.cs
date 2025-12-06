namespace Test.Infrastructure.Extentions;

public static class HttpContentExtentions
{
    public static async Task WriteOnConsoleAsync(this HttpResponseMessage response, ITestOutputHelper outPut,
        string header = "")
    {
        outPut.WriteLine(
            $"============ RESPONSE : {header} ==================== \n" +
            $" -> STATUS : {response.StatusCode}\n" +
            $" -> VALUE : " +
            $"{await response.Content.ReadAsStringAsync()}\n" +
            $"===================================================== \n");
    }

}
