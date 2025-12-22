using Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;
using FluentAssertions;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.CatalogFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "product-delete[REST]")]
public partial class ProductDeleteRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public ProductDeleteRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_delete_a_product()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreateDefault("Toys")
             .ProductCreateDefault("Action Figure", 19.99m)
             .ProductDeleteLastCreation();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<bool>>();
        apiResult.Should().NotBeNull();
        apiResult.Result.Should().BeTrue();
    }

    [Fact]
    public async Task Should_not_be_able_delete_nonexistent_product()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .ProductDelete(99999);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_not_be_able_delete_product_without_permission()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .RegisterDefaultUser()
             .LoginDefaultUser()
             .ProductDelete(1);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Should_be_able_verify_product_deleted()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreateDefault("Sports")
             .ProductCreateDefault("Tennis Ball", 5.99m, isUseCategoryCreated: true)
             .ProductDeleteLastCreation()
             .ProductGetByIdFromLastCreation();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
