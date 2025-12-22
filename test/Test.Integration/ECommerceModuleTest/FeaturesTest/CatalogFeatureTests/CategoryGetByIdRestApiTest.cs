using Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;
using Test.Integration.Fixtures;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.CatalogFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "category-get-by-id[REST]")]
public partial class CategoryGetByIdRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public CategoryGetByIdRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_get_category_by_id()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreateDefault("Books")
             .CategoryGetByIdFromLastCreation();

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = context.Get<ApiResultTest<CategoryResponseTest>>(ScenarioDataKey.CategoryGetByIdResponse);
        apiResult.Should().NotBeNull();
        var category = apiResult!.Result;
        category.Should().NotBeNull();
        category.Name.Should().Be("Books");
    }

    [Fact]
    public async Task Should_not_be_able_get_nonexistent_category()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryGetById(99999);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_not_be_able_get_category_without_permission()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .RegisterDefaultUser()
             .LoginDefaultUser()
             .CategoryGetById(1);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
