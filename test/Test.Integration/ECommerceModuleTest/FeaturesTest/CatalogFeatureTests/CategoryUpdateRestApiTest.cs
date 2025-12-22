using Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;
using Test.Integration.Fixtures;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.CatalogFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "category-update[REST]")]
public partial class CategoryUpdateRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public CategoryUpdateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_update_a_category()
    {
        //-ARRANGE
        var updateRequest = new UpdateCategoryRequestTest(
            CategoryId: 0,
            Name: "Updated Electronics",
            ParentId: null
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreateDefault("Electronics")
             .CategoryUpdateLastCreation(updateRequest);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<CategoryResponseTest>>();
        apiResult.Should().NotBeNull();
        var category = apiResult!.Result;
        category.Should().NotBeNull();
        category.Name.Should().Be("Updated Electronics");
    }

    [Fact]
    public async Task Should_be_able_update_category_parent()
    {
        //-ARRANGE
        var updateRequest = new UpdateCategoryRequestTest(Name: "Tablets");

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreateDefault("Electronics")
             .CategoryCreateDefault("Tablets")
             .CategoryGetPaginated()
             .CategoryUpdate(
                request: updateRequest,
                numberChooseForUpdate: 1,
                numberChooseForParent: 0);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest<CategoryResponseTest>>();
        apiResult.Should().NotBeNull();
        var category = apiResult!.Result;
        category.Should().NotBeNull();

        var categories = context.GetList<CategoryResponseTest>(ScenarioDataKey.Categories);
        category.ParentId.Should().Be(categories![0].CategoryId);
    }

    [Fact]
    public async Task Should_not_be_able_update_nonexistent_category()
    {
        //-ARRANGE
        var updateRequest = new UpdateCategoryRequestTest(
            CategoryId: 99999,
            Name: "Updated Category",
            ParentId: null
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryUpdate(categoryId: 99999, request: updateRequest);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_not_be_able_update_category_without_permission()
    {
        //-ARRANGE
        var updateRequest = new UpdateCategoryRequestTest(
            CategoryId: 1,
            Name: "Updated Category",
            ParentId: null
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .RegisterDefaultUser()
             .LoginDefaultUser()
             .CategoryUpdate(categoryId: 1, request: updateRequest);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
