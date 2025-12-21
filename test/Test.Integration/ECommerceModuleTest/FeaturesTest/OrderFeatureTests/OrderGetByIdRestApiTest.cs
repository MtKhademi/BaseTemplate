using Test.Integration.Fixtures.ECommerceTestScenarios.OrderScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;
using Test.Integration.Fixtures;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.OrderFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "order-get-by-id[REST]")]
public partial class OrderGetByIdRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public OrderGetByIdRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_get_order_by_id()
    {
        //-ARRANGE
        var userId = Guid.NewGuid();
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .OrderCreateDefault(userId: userId)
            .OrderGetById();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var orderCreateResult = context.Get<ApiResultTest<OrderResponseTest>>(ScenarioDataKey.OrderCreate)?.Result;
        var orderGetByIdResult = (await context.LastResponse.Content.ReadModelFromJsonAsync<ApiResultTest<OrderResponseTest>>()).Result;

        orderCreateResult.Should().NotBeNull();
        orderGetByIdResult.Should().NotBeNull();

        orderGetByIdResult!.OrderId.Should().Be(orderCreateResult!.OrderId);
        orderGetByIdResult!.UserId.Should().Be(orderCreateResult!.UserId);
        orderGetByIdResult!.TotalAmount.Should().Be(orderCreateResult!.TotalAmount);
        orderGetByIdResult!.Status.Should().Be(orderCreateResult!.Status);
        orderGetByIdResult!.Items.Should().HaveCount(orderCreateResult!.Items.Count);
    }

    [Fact]
    public async Task Should_not_be_able_get_order_when_not_exist()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .OrderGetById(orderId: 999999);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
    }

    [Fact]
    public async Task Should_not_be_able_get_order_when_not_logged_in()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .OrderGetById(orderId: 1);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
