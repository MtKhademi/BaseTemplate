using MDF.Modules.Modules.CoreModule.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using TseTmcModule.Configs;
using TseTmcWcfService;

namespace MDF.Test.Integration.Modules.TseTmcModuleTest.FeaturesTest.TseTmcSymbolUpdateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"TSE-TMC", "symbol-update")]
public partial class TseTmcSymbolUpdateTest : BaseTest, IClassFixture<TseTmcMockForCheckUpdate>
{
    private readonly string _apiAddress = $"/api/v4/tse-tmc/symbol";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    TseTmcMockForCheckUpdate _tseTmcMockForUpdate;
    public TseTmcSymbolUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper, TseTmcMockForCheckUpdate tseTmcMockForUpdate) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
        _tseTmcMockForUpdate = tseTmcMockForUpdate;
    }




    [Theory]
    [InlineData("", "", "User cannot be null or empty.", "Password cannot be null or empty.")]
    [InlineData("TSE-USER", "", "Password cannot be null or empty.")]
    [InlineData("", "TSE-PASSWORD", "User cannot be null or empty.")]
    public async Task When_not_valid_TseTmcConfig_Expect_get_bad_response(string user, string password, params string[] errorMessage)
    {
        //-ARRANGE
        _client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Clear existing config sources if you need
                // config.Sources.Clear(); // optional

                // Add in-memory overrides
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["TseTmcConfig:User"] = user,
                    ["TseTmcConfig:Password"] = password,
                });
            });
        }).CreateClient();

        var scope = _factory.Services.CreateScope();
        var configService = scope.ServiceProvider.GetService<IConfigService>();
        await configService.UpdateConfigAsync<TseTmcConfig>(new TseTmcConfig
        {
            User = user,
            Password = password
        });

        //- ACT
        var response = await _client.PutAsync(_apiAddress, null);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.Messages.Should().Contain(errorMessage);
    }

    [Fact]
    public async Task When_not_active_symbol_in_db_Expect_dont_change_its_data()
    {
        //-ARRANGE
        await AddRequierData1Async();
        var nameFileXmlForTest = "TseTmcSymboles1.xml";
        var isin = "KORI555567"; //‌نماد غیر فعال در دیتابیس
        var lstOfIsins = new List<string>();
        var tseMoq = (new Data()).TseTmcMock(nameFileXmlForTest);
        _client = _factory.WithWebHostBuilder(conf =>
        {
            conf.ConfigureServices(ser =>
            {
                ser.RemoveAll<TsePublicV2Soap>();
                ser.AddScoped<TsePublicV2Soap>(_ => tseMoq.Object);

            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        var scope = _factory.Services.CreateScope();

        var symbolsInDbBeforeUpdate = await _factory.Repositories.SymbolGetByIsinAsync(isin);
        var instruments = await (new Data()).GetInstruments(nameFileXmlForTest);


        //-ACT
        var response = await _client.PutAsync(_apiAddress, lstOfIsins.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        scope = _factory.Services.CreateScope();
        var symbolsInDbAfterUpdate = await _factory.Repositories.SymbolGetByIsinAsync(isin);

        symbolsInDbBeforeUpdate.Should().NotBeNull();
        symbolsInDbAfterUpdate.Should().NotBeNull();


        symbolsInDbAfterUpdate.IsDisabled.Should().Be(symbolsInDbBeforeUpdate.IsDisabled);
        symbolsInDbAfterUpdate.DisableDateTime.Should().Be(symbolsInDbBeforeUpdate.DisableDateTime);
        symbolsInDbAfterUpdate.TypeOfSymbol.Should().Be(symbolsInDbBeforeUpdate.TypeOfSymbol);
        symbolsInDbAfterUpdate.SymbolNameTse.Should().Be(symbolsInDbBeforeUpdate.SymbolNameTse);
        symbolsInDbAfterUpdate.SymbolName.Should().Be(symbolsInDbBeforeUpdate.SymbolName);
        symbolsInDbAfterUpdate.Title.Should().Be(symbolsInDbBeforeUpdate.Title);
    }

    [Fact]
    public async Task When_not_active_symbol_in_db_but_force_for_update_Expect_change_its_data()
    {
        //-ARRANGE
        await AddRequierData4Async();
        var nameFileXmlForTest = "TseTmcSymboles6.xml";
        var isin = "KORI5558585"; //‌نماد غیر فعال در دیتابیس
        var lstOfIsins = new List<string>();
        var tseMoq = (new Data()).TseTmcMock(nameFileXmlForTest);
        var dateTimeProviderMoq = new Mock<IDateTimeProvider>();
        dateTimeProviderMoq.Setup(s => s.Now).Returns(new DateTime(2024, 11, 30, 11, 30, 00));
        _client = _factory.WithWebHostBuilder(conf =>
        {
            conf.ConfigureServices(ser =>
            {
                ser.RemoveAll<TsePublicV2Soap>();
                ser.AddScoped<TsePublicV2Soap>(_ => tseMoq.Object);

                ser.RemoveAll<IDateTimeProvider>();
                ser.AddScoped<IDateTimeProvider>(_ => dateTimeProviderMoq.Object);

            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        var scope = _factory.Services.CreateScope();

        var symbolsInDbBeforeUpdate = await _factory.Repositories.SymbolGetByIsinAsync(isin);
        var instruments = await (new Data()).GetInstruments(nameFileXmlForTest);


        //-ACT
        var response = await _client.PutAsync($"{_apiAddress}?isUpdateDisabledSymbol=true", lstOfIsins.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        scope = _factory.Services.CreateScope();
        var symbolsInDbAfterUpdate = await _factory.Repositories.SymbolGetByIsinAsync(isin);

        symbolsInDbBeforeUpdate.Should().NotBeNull();
        symbolsInDbAfterUpdate.Should().NotBeNull();


        symbolsInDbAfterUpdate.IsActive.Should().BeFalse();
        symbolsInDbAfterUpdate.DisableDateTime.Should().Be(new DateTime(2024, 11, 30, 11, 30, 00));
        symbolsInDbAfterUpdate.FirmId.Should().Be(symbolsInDbBeforeUpdate.FirmId.Value);
        ((int)symbolsInDbAfterUpdate.TypeOfSymbol).Should().Be((int)TypeOfSymbolTest.Commodity);
    }

    [Fact]
    public async Task When_active_symbol_in_db_and_deActive_from_TseTmc_Expect_change_its_data_currect()
    {
        //-ARRANGE
        await AddRequierData1Async();
        var nameFileXmlForTest = "TseTmcSymboles2.xml";
        var isin = "KORI555667";
        var dtDisabled = new DateTime(2024, 06, 29, 10, 46, 0);
        var lstOfIsins = new List<string>();
        var tseMoq = (new Data()).TseTmcMock(nameFileXmlForTest);
        var dateTimeProviderMoq = new Mock<IDateTimeProvider>();
        dateTimeProviderMoq.Setup(s => s.Now).Returns(dtDisabled);
        _client = _factory.WithWebHostBuilder(conf =>
        {
            conf.ConfigureServices(ser =>
            {
                ser.RemoveAll<TsePublicV2Soap>();
                ser.AddScoped<TsePublicV2Soap>(_ => tseMoq.Object);

                ser.RemoveAll<IDateTimeProvider>();
                ser.AddScoped<IDateTimeProvider>(_ => dateTimeProviderMoq.Object);

            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        var scope = _factory.Services.CreateScope();

        var symbolsInDbBeforeUpdate = await _factory.Repositories.SymbolGetByIsinAsync(isin);
        var instruments = await (new Data()).GetInstruments(nameFileXmlForTest);


        //-ACT
        var response = await _client.PutAsync(_apiAddress, lstOfIsins.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        scope = _factory.Services.CreateScope();
        var symbolsInDbAfterUpdate = await _factory.Repositories.SymbolGetByIsinAsync(isin);

        symbolsInDbBeforeUpdate.Should().NotBeNull();
        symbolsInDbAfterUpdate.Should().NotBeNull();

        symbolsInDbAfterUpdate.IsActive.Should().BeFalse();
        symbolsInDbAfterUpdate.DisableDateTime.Should().Be(dtDisabled);

    }

    [Fact]
    public async Task When_change_from_TseTmc_Expect_change_only_special_data()
    {
        //-ARRANGE
        await AddRequierData2Async();
        var nameFileXmlForTest = "TseTmcSymboles4.xml";
        var isin = "KORI556068K";
        var dtDisabled = new DateTime(2024, 06, 29, 10, 46, 0);
        var lstOfIsins = new List<string>();
        var tseMoq = (new Data()).TseTmcMock(nameFileXmlForTest);
        var dateTimeProviderMoq = new Mock<IDateTimeProvider>();
        dateTimeProviderMoq.Setup(s => s.Now).Returns(dtDisabled);
        _client = _factory.WithWebHostBuilder(conf =>
        {
            conf.ConfigureServices(ser =>
            {
                ser.RemoveAll<TsePublicV2Soap>();
                ser.AddScoped<TsePublicV2Soap>(_ => tseMoq.Object);

                ser.RemoveAll<IDateTimeProvider>();
                ser.AddScoped<IDateTimeProvider>(_ => dateTimeProviderMoq.Object);

            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        var scope = _factory.Services.CreateScope();

        var symbolsInDbBeforeUpdate = await _factory.Repositories.SymbolGetByIsinAsync(isin);
        var instruments = await (new Data()).GetInstruments(nameFileXmlForTest);


        //-ACT
        var response = await _client.PutAsync(_apiAddress, lstOfIsins.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        scope = _factory.Services.CreateScope();
        var symbolsInDbAfterUpdate = await _factory.Repositories.SymbolGetByIsinAsync(isin);

        symbolsInDbBeforeUpdate.Should().NotBeNull();
        symbolsInDbAfterUpdate.Should().NotBeNull();

        symbolsInDbAfterUpdate.IsActive.Should().BeTrue();
        symbolsInDbAfterUpdate.DisableDateTime.Should().BeNull();
        symbolsInDbAfterUpdate.FirmId.Should().Be(symbolsInDbBeforeUpdate.FirmId.Value);
        ((int)symbolsInDbAfterUpdate.TypeOfSymbol).Should().Be((int)TypeOfSymbolTest.Future);

    }

    [Fact]
    public async Task When_not_exist_symbol_in_my_db_Expect_ok_response_and_it_in_db()
    {
        //-ARRANGE
        var nameFileXmlForTest = "TseTmcSymboles3.xml";
        var isin = "BBVNASDJASDNASD"; //‌این همون ایزین داخل فایل هست
        var lstOfIsins = new List<string>();
        var tseMoq = (new Data()).TseTmcMock(nameFileXmlForTest);
        _client = _factory.WithWebHostBuilder(conf =>
        {
            conf.ConfigureServices(ser =>
            {
                ser.RemoveAll<TsePublicV2Soap>();
                ser.AddScoped<TsePublicV2Soap>(_ => tseMoq.Object);
            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        var instruments = await (new Data()).GetInstruments(nameFileXmlForTest);

        //-ACT
        var response = await _client.PutAsync(_apiAddress, lstOfIsins.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //var symbolInDb = await SymbolGetByIsisnAsync(isin);
        //symbolInDb.Should().NotBeNull();
        //symbolInDb.Isin.Should().Be(isin);
    }

    [Theory]
    // دقت کنید که مقدارها باید در ماک قرار داده شود - در فایلش
    [ClassData(typeof(IsinsForCheckChangeTypeOfSymbolToBonds))]
    public async Task When_update_symbol_Expect_ok_response_and_it_in_db(
        List<(string isin, TypeOfSymbolTest typeOfSymbol, int? firmId, byte? boardCode, string? marketCode, byte? securitiesExchangeCode)> data)
    {
        //-ARRANGE
        _client = _factory.WithWebHostBuilder(conf =>
        {
            conf.ConfigureServices(ser =>
            {
                ser.RemoveAll<TsePublicV2Soap>();
                ser.AddScoped<TsePublicV2Soap>(_ => _tseTmcMockForUpdate.GetTseTmcMo("TseTmcSymboles5.xml").Object);
            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        await AddRequierData3Async();
        //-ACT
        var response = await _client.PutAsync(_apiAddress, (data.Select(x => x.isin).ToList()).ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        foreach (var item in data)
        {
            var symbolInDb = await _factory.Repositories.SymbolGetByIsinAsync(isin: item.isin);

            symbolInDb.Should().NotBeNull();

            symbolInDb.TypeOfSymbol.ToString().Should().Be(item.typeOfSymbol.ToString(),
                $"{item.isin} must be {item.typeOfSymbol} but It is : {symbolInDb.TypeOfSymbol}");

            symbolInDb.TypeOfSymbol.GetDescription().Should().Be(item.typeOfSymbol.GetDescription());

            symbolInDb.FirmId.Should().Be(item.firmId, $"ISIN : {item.isin}");

        }
    }



    [Theory]
    // دقت کنید که مقدارها باید در ماک قرار داده شود - در فایلش
    [ClassData(typeof(SymbolUpdatedFromTseTmcData))]
    public async Task Shoud_be_able_all_data_for_symbol_if_null_from_TseTmc(
        List<(string isin, TypeOfSymbolTest typeOfSymbol, string symbolName, string symbolTitle, string symbolTseName)> data)
    {
        //-ARRANGE
        _client = _factory.WithWebHostBuilder(conf =>
        {
            conf.ConfigureServices(ser =>
            {
                ser.RemoveAll<TsePublicV2Soap>();
                ser.AddScoped<TsePublicV2Soap>(_ => _tseTmcMockForUpdate.GetTseTmcMo("TseTmcSymboles7.xml").Object);
            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        await AddRequierData5Async();
        //-ACT
        var response = await _client.PutAsync(_apiAddress, (data.Select(x => x.isin).ToList()).ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        foreach (var item in data)
        {
            var symbolInDb = await _factory.Repositories.SymbolGetByIsinAsync(isin: item.isin);
            symbolInDb.Should().NotBeNull();

            symbolInDb.TypeOfSymbol.ToString().Should().Be(item.typeOfSymbol.ToString(),
                $"{item.isin} must be {item.typeOfSymbol} but It is : {symbolInDb.TypeOfSymbol}");

            symbolInDb.TypeOfSymbol.GetDescription().Should().Be(item.typeOfSymbol.GetDescription());


            symbolInDb.SymbolName.Should().Be(item.symbolName,
                $"{item.isin} must be {item.symbolName} but It is : {symbolInDb.SymbolName}");
            symbolInDb.Title.Should().Be(item.symbolTitle);
            symbolInDb.SymbolNameTse.Should().Be(item.symbolTseName);
        }
    }


    [Fact]
    public async Task Shoud_be_able_update_data_from_TSETMC_and_not_change_other_items()
    {
        //-ARRANGE
        string isin = "KORI5558585";
        var typeOfSymbol = TypeOfSymbolTest.Stock;
        var typeOfSymbolInTseTmc = TypeOfSymbolTest.Forward_Day;
        var typeOfSymbolInTseTmcAfterUpdate = TypeOfSymbolTest.PutOption;
        string symbolName = "اختیارف سینرژی-20000-14031101";
        string symbolTitle = "اختیارف سینرژی-20000-14031101";
        string symbolTseName = "اختیارف سینرژی-20000-14031101";
        await _factory.Repositories.SymbolAddAsync(isin: isin,
            symbolName: symbolName, 
            typeOfSymbol: typeOfSymbol, 
            typeOfSymbolInTseTmc: typeOfSymbolInTseTmc);
        _client = _factory.WithWebHostBuilder(conf =>
        {
            conf.ConfigureServices(ser =>
            {
                ser.RemoveAll<TsePublicV2Soap>();
                ser.AddScoped<TsePublicV2Soap>(_ => _tseTmcMockForUpdate.GetTseTmcMo("TseTmcSymboles9.xml").Object);
            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);

        //-ACT
        var response = await _client.PutAsync(_apiAddress, new List<string> { isin }.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var symbolInDb = await _factory.Repositories.SymbolGetByIsinAsync(isin: isin);
        symbolInDb.Should().NotBeNull();

        symbolInDb.TypeOfSymbol.ToString().Should().Be(typeOfSymbol.ToString(),
            $"{isin} must be {typeOfSymbol} but It is : {symbolInDb.TypeOfSymbol}");

        symbolInDb.TypeOfSymbol.GetDescription().Should().Be(typeOfSymbol.GetDescription());

        symbolInDb.TypeOfSymbolInTseTmc.ToString().Should().Be(typeOfSymbolInTseTmcAfterUpdate.ToString(),
            $"{isin} must be {typeOfSymbolInTseTmcAfterUpdate} but It is : {symbolInDb.TypeOfSymbolInTseTmc}");

        symbolInDb.SymbolName.Should().Be(symbolName,
            $"{isin} must be {symbolName} but It is : {symbolInDb.SymbolName}");

        symbolInDb.Title.Should().Be(symbolTitle);
        symbolInDb.SymbolNameTse.Should().Be(symbolTseName);
    }


}
