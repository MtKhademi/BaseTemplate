using Moq.Protected;

namespace MDF.Test.Common.Extentions;

public static class MoqExtentions
{

    public static Mock<IHttpClientFactory> CreateHttpClientFactoryMoq(string clientName, HttpMethod method, string returnData, string urlCall)
    {
        var httpClient = MoqExtentions.CreateHttpClientMoq(HttpMethod.Get, returnData, urlCall);

        var clientFactoryMock = new Mock<IHttpClientFactory>();
        clientFactoryMock.Setup(f => f.CreateClient(clientName))
            .Returns(httpClient);

        return clientFactoryMock;

    }
    public static HttpClient CreateHttpClientMoq(HttpMethod method, string returnData, string urlCall)
    {
        var httpHandlerMock = new Mock<HttpMessageHandler>();
        httpHandlerMock.SetupSendAsync(returnData, method, urlCall);

        return new HttpClient(httpHandlerMock.Object);
    }

    public static void SetupSendAsync(this Mock<HttpMessageHandler> handler,
        string? data,
        HttpMethod requestMethod,
        string requestUrl)
    {
        _setupSendAsyncWithoutCheckUrl(handler, data, requestMethod);
    }

    private static void _setupSendAsyncWithoutCheckUrl(this Mock<HttpMessageHandler> handler,
        string? data,
        HttpMethod requestMethod)
    {
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
             ItExpr.Is<HttpRequestMessage>(r =>
                 r.Method == requestMethod
             ),
             ItExpr.IsAny<CancellationToken>()
         ).ReturnsAsync(new HttpResponseMessage
         {
             StatusCode = System.Net.HttpStatusCode.OK,
             Content = new StringContent(data)
         });
    }
}
