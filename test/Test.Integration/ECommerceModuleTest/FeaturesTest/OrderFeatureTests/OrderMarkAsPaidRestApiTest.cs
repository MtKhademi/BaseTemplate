using Test.Integration.Fixtures.ECommerceTestScenarios.OrderScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.OrderFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "order-mark-as-paid[REST]")]
public partial class OrderMarkAsPaidRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public OrderMarkAsPaidRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_mark_order_as_paid()
    {
        //-ARRANGE
        var userId = Guid.NewGuid();
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .OrderCreateDefault(userId: userId)
            .OrderMarkAsPaid();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<OrderResponseTest>>();
        apiResult.Should().NotBeNull();
        var order = apiResult!.Result;
        order.Should().NotBeNull();
        order.Status.Should().Be(OrderStatusTest.Paid);
    }

    [Fact]
    public async Task Should_not_be_able_mark_non_existent_order_as_paid()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .OrderMarkAsPaid(orderId: 999999);

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
    public async Task Should_not_be_able_mark_order_as_paid_when_not_logged_in()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .OrderMarkAsPaid(orderId: 1);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_be_able_verify_order_status_after_marking_as_paid()
    {
        //-ARRANGE
        var userId = Guid.NewGuid();
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .OrderCreateDefault(userId: userId)
            .OrderMarkAsPaid()
            .OrderGetById();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<OrderResponseTest>>();
        apiResult.Should().NotBeNull();
        var order = apiResult!.Result;
        order.Should().NotBeNull();
        order.Status.Should().Be(OrderStatusTest.Paid);
    }
}
