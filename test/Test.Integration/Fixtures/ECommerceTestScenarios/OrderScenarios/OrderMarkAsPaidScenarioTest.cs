using Test.Integration.TestScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.Fixtures.ECommerceTestScenarios.OrderScenarios;

internal class OrderMarkAsPaidScenarioTest(long? orderId = null) : IScenarioStep
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

        var request = new MarkOrderAsPaidRequestTest(orderId.Value);

        var apiResult = await context.ApiPutRequestAsync<MarkOrderAsPaidRequestTest, OrderResponseTest>(
            url: $"/ecommerce/api/v1/orders/{orderId}/mark-as-paid",
            storeKey: ScenarioDataKey.OrderMarkAsPaidResponse,
            request: request,
            scenarioName: GetType().Name);
    }
}

internal static class OrderMarkAsPaidScenarioTestExtensions
{
    public static ScenarioRunner OrderMarkAsPaid(this ScenarioRunner runner, long? orderId = null)
        => runner.AddScenario(new OrderMarkAsPaidScenarioTest(orderId));
}
