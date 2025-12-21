using Test.Integration.Fixtures.ECommerceTestScenarios.OrderScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.OrderFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "order-get-paginated[REST]")]
public partial class OrderGetPaginatedRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public OrderGetPaginatedRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_get_orders_paginated()
    {
        //-ARRANGE
        var userId = Guid.NewGuid();
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .OrderCreateDefault(userId: userId)
            .OrderCreateDefault(userId: userId)
            .OrderGetPaginated(pageNumber: 1, pageSize: 10);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<PaginatedListTest<OrderResponseTest>>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.Items.Should().NotBeEmpty();
        apiResult.Result.TotalCount.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Should_be_able_get_orders_by_user_id()
    {
        //-ARRANGE
        var userId = Guid.NewGuid();
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .OrderCreateDefault(userId: userId)
            .OrderCreateDefault(userId: userId)
            .OrderGetPaginated(pageNumber: 1, pageSize: 10, userId: userId);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<PaginatedListTest<OrderResponseTest>>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.Items.Should().NotBeEmpty();
        apiResult.Result.Items.Should().AllSatisfy(order => order.UserId.Should().Be(userId));
    }

    [Fact]
    public async Task Should_not_be_able_get_orders_when_not_logged_in()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .OrderGetPaginated();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_return_empty_list_when_no_orders_exist_for_user()
    {
        //-ARRANGE
        var nonExistentUserId = Guid.NewGuid();
        var runner = new ScenarioRunner(_client, _outPutHelper)
            .LoginAdmin()
            .OrderGetPaginated(userId: nonExistentUserId);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<PaginatedListTest<OrderResponseTest>>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().NotBeNull();
        apiResult.Result.Items.Should().BeEmpty();
        apiResult.Result.TotalCount.Should().Be(0);
    }
}
