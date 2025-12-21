using Test.Integration.Fixtures.ECommerceTestScenarios.OrderScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.OrderFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "order-create[REST]")]
public partial class OrderCreateRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public OrderCreateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_create_an_order()
    {
        //-ARRANGE
        var userId = Guid.NewGuid();
        var request = new CreateOrderRequestTest(
            UserId: userId,
            Items: new List<CreateOrderItemRequestTest>
            {
                new CreateOrderItemRequestTest(
                    ProductId: 1,
                    Quantity: 2,
                    UnitPrice: 10.50m
                ),
                new CreateOrderItemRequestTest(
                    ProductId: 2,
                    Quantity: 1,
                    UnitPrice: 25.00m
                )
            }
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .OrderCreate(request);

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
        order.UserId.Should().Be(request.UserId);
        order.OrderId.Should().BeGreaterThan(0);
        order.Items.Should().HaveCount(2);
        order.TotalAmount.Should().Be(46.00m); // (2 * 10.50) + (1 * 25.00)
        order.Status.Should().Be(OrderStatusTest.Pending);
    }

    [Fact]
    public async Task Should_not_be_able_create_order_with_empty_items()
    {
        //-ARRANGE
        var userId = Guid.NewGuid();
        var dto = new CreateOrderRequestTest(
            UserId: userId,
            Items: new List<CreateOrderItemRequestTest>()
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .OrderCreate(dto);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.ErrorKey.Should().Contain("Exception");
    }

    [Fact]
    public async Task Should_not_be_able_create_order_with_invalid_quantity()
    {
        //-ARRANGE
        var userId = Guid.NewGuid();
        var dto = new CreateOrderRequestTest(
            UserId: userId,
            Items: new List<CreateOrderItemRequestTest>
            {
                new CreateOrderItemRequestTest(
                    ProductId: 1,
                    Quantity: 0,
                    UnitPrice: 10.50m
                )
            }
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .OrderCreate(dto);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_not_be_able_create_order_when_not_logged_in()
    {
        //-ARRANGE
        var userId = Guid.NewGuid();
        var dto = new CreateOrderRequestTest(
            UserId: userId,
            Items: new List<CreateOrderItemRequestTest>
            {
                new CreateOrderItemRequestTest(
                    ProductId: 1,
                    Quantity: 2,
                    UnitPrice: 10.50m
                )
            }
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .OrderCreate(dto);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
