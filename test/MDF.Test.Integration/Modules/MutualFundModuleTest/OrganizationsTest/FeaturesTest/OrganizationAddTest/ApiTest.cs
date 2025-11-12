namespace MDF.Test.Integration.Modules.MutualFundModuleTest.OrganizationsTest.FeaturesTest.OrganizationAddTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "organization-add")]
public partial class OrganizationAddTest : BaseTest
{
    private readonly string _apiAddress = $"/api/V4/fund/organization";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public OrganizationAddTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [ClassData(typeof(OrganizationAddDtoNotValidData))]
    public async Task When_call_api_with_not_valid_data_Expect_get_bad_response(OrganizationAddDtoV4Test dto, IEnumerable<string> messagesException)
    {
        //-ARRANGE

        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Messages.Should().BeEquivalentTo(messagesException);
    }


    [Fact]
    public async Task When_not_exist_organization_type_Expect_get_Not_Found_response()
    {
        //-ARRANGE
        var organizationType = await _factory.Repositories.OrganizationTypeAddAsync(code: "1", title: "عنوان نوع سازمان");

        var dto = new OrganizationAddDtoV4Test(OrganizationTypeId: organizationType.OrganizationTypeIdPk, NationalCode: "123",
            RegisterNumber: "123", RegisterDate: "2025-04-07", Title: "123");

        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<OrganizationGetDtoTestV4>();
        apiResult.Should().NotBeNull();
        apiResult.NationalCode.Should().Be("123");
        apiResult.RegisterNumber.Should().Be("123");
        apiResult.RegisterDate.Should().Be("2025-04-07");
        apiResult.Title.Should().Be("123");
    }

}
