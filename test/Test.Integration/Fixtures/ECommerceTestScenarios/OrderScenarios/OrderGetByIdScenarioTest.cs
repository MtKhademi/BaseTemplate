using Test.Integration.TestScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.OrderScenarios;

internal class OrderGetByIdScenarioTest(long? orderId = null) : IScenarioStep
{
    public async Task ExecuteAsync(ScenarioContext context)
    {
        if (orderId == null || orderId == 0)
        {
            var createdOrder = context.Get<ApiResultTest<OrderResponseTest>>(ScenarioDataKey.OrderCreate);
            if (createdOrder == null || createdOrder.Result is null || createdOrder.Result.OrderId == 0)
            {
                context.TestOutputHelper.WriteLine(createdOrder.ToJson());
                throw new InvalidOperationException("No OrderId provided and no created order found in context.");
            }
            orderId = createdOrder.Result.OrderId;
        }

        var apiResult = await context.ApiGetRequestAsync<OrderResponseTest>(
            url: $"/ecommerce/api/v1/orders/{orderId}",
            storeKey: ScenarioDataKey.OrderGetByIdResponse,
            scenarioName: GetType().Name);

        context.AddToList(ScenarioDataKey.Orders, apiResult);
    }
}

internal static class OrderGetByIdScenarioTestExtensions
{
    public static ScenarioRunner OrderGetById(this ScenarioRunner runner,
        long? orderId = null)
    {
        return runner.AddScenario(new OrderGetByIdScenarioTest(orderId));
    }
}
