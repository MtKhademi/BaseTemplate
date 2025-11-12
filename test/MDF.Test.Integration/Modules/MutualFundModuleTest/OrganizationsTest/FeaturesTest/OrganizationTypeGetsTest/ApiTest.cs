namespace MDF.Test.Integration.Modules.MutualFundModuleTest.OrganizationsTest.FeaturesTest.OrganizationTypeGetsTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "organization-types")]
public partial class OrganizationTypeGetsTest : BaseTest
{
    private readonly string _apiAddress = $"/api/V4/fund/organization/types";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public OrganizationTypeGetsTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        _outPutHelper = outPutHelper;
        var scope = factory.Services.CreateScope();
    }



    [Fact]
    public async Task When_call_get_OrganizationTypes_api_Expect_get_ok_response()
    {
        //-ARRANGE
        await _factory.Repositories.OrganizationTypeAddAsync(code: "1", title: "با مسئوليت محدود");
        await _factory.Repositories.OrganizationTypeAddAsync(code: "2", title: "تضامنی");
        await _factory.Repositories.OrganizationTypeAddAsync(code: "3", title: "تعاونی");

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<List<OrganizationTypeGetDtoTestV4>>();
        apiResult.Should().NotBeNull();
        apiResult.Should().HaveCount(3);
        apiResult.Any(x => x.Code == "1").Should().BeTrue();
        apiResult.Any(x => x.Title == "با مسئوليت محدود").Should().BeTrue();

        apiResult.Any(x => x.Code == "2").Should().BeTrue();
        apiResult.Any(x => x.Title == "تضامنی").Should().BeTrue();
        
        apiResult.Any(x => x.Code == "3").Should().BeTrue();
        apiResult.Any(x => x.Title == "تعاونی").Should().BeTrue();
    }
}
