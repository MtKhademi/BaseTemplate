using Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.CatalogFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "product-update[REST]")]
public partial class ProductUpdateRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public ProductUpdateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_update_a_product()
    {
        //-ARRANGE
        var updateRequest = new UpdateProductRequestTest(
            ProductId: 0,
            Name: "Updated Product Name",
            Price: 149.99m,
            CategoryId: 1,
            IsActive: false
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreateDefault("Electronics")
             .ProductCreateDefault("Original Product", 99.99m)
             .ProductUpdateLastCreation(updateRequest);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<ProductResponseTest>>();
        apiResult.Should().NotBeNull();
        var product = apiResult!.Result;
        product.Should().NotBeNull();
        product.Name.Should().Be("Updated Product Name");
        product.Price.Should().Be(149.99m);
        product.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Should_not_be_able_update_nonexistent_product()
    {
        //-ARRANGE
        var updateRequest = new UpdateProductRequestTest(
            ProductId: 99999,
            Name: "Updated Product",
            Price: 149.99m,
            CategoryId: 1,
            IsActive: true
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .ProductUpdate(productId: 99999, request: updateRequest);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_not_be_able_update_product_without_permission()
    {
        //-ARRANGE
        var updateRequest = new UpdateProductRequestTest(
            ProductId: 1,
            Name: "Updated Product",
            Price: 149.99m,
            CategoryId: 1,
            IsActive: true
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .RegisterDefaultUser()
             .LoginDefaultUser()
             .ProductUpdateLastCreation(updateRequest);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
