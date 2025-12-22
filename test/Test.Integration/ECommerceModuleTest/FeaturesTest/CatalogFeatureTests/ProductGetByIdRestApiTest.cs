using Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;
using Test.Integration.Fixtures;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.CatalogFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "product-get-by-id[REST]")]
public partial class ProductGetByIdRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public ProductGetByIdRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_get_product_by_id()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreateDefault("Books")
             .ProductCreateDefault("Test Book", 29.99m)
             .ProductGetByIdFromLastCreation();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest<ProductResponseTest>>(ScenarioDataKey.ProductGetByIdResponse);
        apiResult.Should().NotBeNull();
        var product = apiResult!.Result;
        product.Should().NotBeNull();
        product.Name.Should().Be("Test Book");
        product.Price.Should().Be(29.99m);
    }

    [Fact]
    public async Task Should_not_be_able_get_nonexistent_product()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .ProductGetById(99999);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_not_be_able_get_product_without_permission()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .RegisterDefaultUser()
             .LoginDefaultUser()
             .ProductGetById(1);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
