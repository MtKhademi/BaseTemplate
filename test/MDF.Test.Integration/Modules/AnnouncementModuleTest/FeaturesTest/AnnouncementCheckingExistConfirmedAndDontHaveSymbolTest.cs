namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.FeaturesTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "checking-exist-confirmed-and-dont-have-symbol")]
public partial class AnnouncementCheckingExistConfirmedAndDontHaveSymbolTest : BaseTest
{
    private readonly string _api = $"/api/v4/announcement/checking-exist-confirmed-and-dont-have-symbol";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AnnouncementCheckingExistConfirmedAndDontHaveSymbolTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [InlineData("2025")]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(string? date)
    {
        //-ARRANGE
        var api = $"{_api}/{date}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
    }


    //سناریو :‌
    // وقتی یک مجمعی تایید شده است که به هیچ نمادی متصل نیست باید کدال کد آن مجمع بصورت خروجی بیایید
    [Fact]
    public async Task Should_be_able_get_announcementCodalCode_that_dont_have_any_symbol()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 05, 09, 30, 0);
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585, dtPublish: dt);
        var dividend = await _factory.Repositories.DividendPerShareAddAsync(announcementId: announcement.AnnouncementIdPk,
            dtEntry: dt, dtModify: dt, isConfirm: true);

        var api = $"{_api}/{dt.GetISOStringDate()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<string>>();
        apiResult.Should().NotBeNull();
        apiResult.Should().HaveCount(1);
        apiResult.Should().Contain(announcement.Code.ToString());
    }


    // سناریو :‌
    // وقتی مجمع تایید شده است و همه آنها نماد متصل فعال دارند در نتیجه هیچی خروجی نمیآید
    [Fact]
    public async Task When_all_announcement_have_symbol_dont_get_any_data()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 05, 09, 30, 0);
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: "IRB858581", firmId: 1, typeOfSymbol: TypeOfSymbolTest.Stock);
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585, firmId: 1, dtPublish: dt);
        var dividend = await _factory.Repositories.DividendPerShareAddAsync(announcementId: announcement.AnnouncementIdPk,
            dtEntry: dt, dtModify: dt, isConfirm: true);

        var api = $"{_api}/{dt.GetISOStringDate()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<string>>();
        apiResult.Should().NotBeNull();
        apiResult.Should().HaveCount(0);
    }
}
