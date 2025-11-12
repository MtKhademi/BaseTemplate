

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.OrganizationsTest.FeaturesTest.OrganizationGetPaginatedListTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "organization-paginatedList")]
public class OrganizationGetPaginatedListApiTest : BaseTest
{
    private readonly string _apiAddress = $"/api/V4/fund/organization/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public OrganizationGetPaginatedListApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        _outPutHelper = outPutHelper;
        var scope = factory.Services.CreateScope();
    }



    [Fact]
    public async Task When_call_get_Organization_api_Expect_get_ok_response()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedList<OrganizationGetDtoTestV4>>();
        apiResult.Should().NotBeNull();
    }
}
