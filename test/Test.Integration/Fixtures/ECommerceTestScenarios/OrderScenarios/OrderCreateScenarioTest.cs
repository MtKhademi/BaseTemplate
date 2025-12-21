using Test.Integration.TestScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.OrderScenarios;

internal class OrderCreateScenarioTest(CreateOrderRequestTest request) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        var apiResult = await context.ApiPostRequestAsync<CreateOrderRequestTest, OrderResponseTest>(
            url: "/ecommerce/api/v1/orders",
            storeKey: ScenarioDataKey.OrderCreate,
            request: request,
            scenarioName: GetType().Name);

        context.AddToList(ScenarioDataKey.Orders, apiResult);
    }
}

internal static class OrderCreateScenarioTestExtensions
{
    public static ScenarioRunner OrderCreateDefault(this ScenarioRunner runner,
        Guid? userId = null,
        int productId = 1,
        int quantity = 2,
        decimal unitPrice = 10.50m)
        => OrderCreate(runner, userId, productId, quantity, unitPrice);

    public static ScenarioRunner OrderCreate(this ScenarioRunner runner,
        Guid? userId,
        int productId,
        int quantity,
        decimal unitPrice)
    {
        var finalUserId = userId ?? Guid.NewGuid();

        return OrderCreate(runner, new CreateOrderRequestTest(
            UserId: finalUserId,
            Items: new List<CreateOrderItemRequestTest>
            {
                new CreateOrderItemRequestTest(
                    ProductId: productId,
                    Quantity: quantity,
                    UnitPrice: unitPrice
                )
            }
        ));
    }

    public static ScenarioRunner OrderCreate(this ScenarioRunner runner,
        CreateOrderRequestTest request)
    {
        return runner.AddScenario(new OrderCreateScenarioTest(request));
    }
}
