using Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.CatalogFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "product-create[REST]")]
public partial class ProductCreateRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public ProductCreateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_create_a_product()
    {
        //-ARRANGE
        var request = new CreateProductRequestTest(
            Name: "Test Product",
            Price: 99.99m,
            IsActive: true
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreateDefault("Electronics")
             .ProductCreate(request, isUseCategoryCreated: true);

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
        product.Name.Should().Be(request.Name);
        product.Price.Should().Be(request.Price);
        product.IsActive.Should().BeTrue();
        product.ProductId.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData("", 10.0, 1)]
    [InlineData(null, 10.0, 1)]
    [InlineData("Product", 0, 1)]
    [InlineData("Product", -10, 1)]
    [InlineData("Product", 10.0, 0)]
    [InlineData("Product", 10.0, -1)]
    public async Task Should_not_be_able_create_product_with_invalid_data(
        string name, decimal price, int categoryId)
    {
        //-ARRANGE
        var request = new CreateProductRequestTest(
            Name: name,
            Price: price,
            CategoryId: categoryId,
            IsActive: true
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .ProductCreate(request);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Should_not_be_able_create_product_when_not_authenticated()
    {
        //-ARRANGE
        var request = new CreateProductRequestTest(
            Name: "Test Product",
            Price: 99.99m,
            CategoryId: 1,
            IsActive: true
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .ProductCreate(request);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_not_be_able_create_product_without_permission()
    {
        //-ARRANGE
        var request = new CreateProductRequestTest(
            Name: "Test Product",
            Price: 99.99m,
            CategoryId: 1,
            IsActive: true
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .RegisterDefaultUser()
             .LoginDefaultUser()
             .ProductCreate(request);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
