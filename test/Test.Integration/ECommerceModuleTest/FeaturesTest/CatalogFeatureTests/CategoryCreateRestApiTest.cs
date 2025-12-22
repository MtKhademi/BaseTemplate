using Test.Integration.Fixtures.ECommerceTestScenarios.CatalogScenarios;
using Test.Integration.ECommerceModuleTest.Requestes;

namespace Test.Integration.ECommerceModuleTest.FeaturesTest.CatalogFeatureTests;

[Collection("Collection tests v1")]
[Trait("ECommerce", "category-create[REST]")]
public partial class CategoryCreateRestApiTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;

    public CategoryCreateRestApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _outPutHelper = outPutHelper;
    }

    [Fact]
    public async Task Should_be_able_create_a_category()
    {
        //-ARRANGE
        var request = new CreateCategoryRequestTest(
            Name: "Electronics",
            ParentId: null
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreate(request);

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
        category.Name.Should().Be(request.Name);
        category.ParentId.Should().BeNull();
        category.CategoryId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Should_be_able_create_a_subcategory()
    {
        //-ARRANGE
        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreateDefault("Electronics")
             .CategoryCreateWithParentLastCreation("Smartphones");

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
        category.Name.Should().Be("Smartphones");
        category.ParentId.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Should_not_be_able_create_category_with_empty_name(string name)
    {
        //-ARRANGE
        var request = new CreateCategoryRequestTest(
            Name: name,
            ParentId: null
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .LoginAdmin()
             .CategoryCreate(request);

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
    public async Task Should_not_be_able_create_category_when_not_authenticated()
    {
        //-ARRANGE
        var request = new CreateCategoryRequestTest(
            Name: "Electronics",
            ParentId: null
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .CategoryCreate(request);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_not_be_able_create_category_without_permission()
    {
        //-ARRANGE
        var request = new CreateCategoryRequestTest(
            Name: "Electronics",
            ParentId: null
        );

        var runner = new ScenarioRunner(_client, _outPutHelper)
             .RegisterDefaultUser()
             .LoginDefaultUser()
             .CategoryCreate(request);

        //-ACT
        var context = await runner.RunAsync();

        //-ASSERT
        var response = context.LastResponse!;
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
