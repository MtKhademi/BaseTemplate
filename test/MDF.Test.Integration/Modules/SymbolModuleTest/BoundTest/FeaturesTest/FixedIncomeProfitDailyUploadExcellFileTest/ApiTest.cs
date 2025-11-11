using MDF.Common.Exceptions;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeProfitDailyUploadExcellFileTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "fixed-income-profit-daily-upload-excell-file")]
public partial class FixedIncomeProfitDailyUploadExcellFileTest : BaseTest
{
    private readonly string _apiAddress = $"/api/v4/symbol/bound/fixed-income/profit-daily/[ISIN]/file";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FixedIncomeProfitDailyUploadExcellFileTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("multipart/form-data"));
        _outPutHelper = outPutHelper;
        var scope = _factory.Services.CreateScope();
    }



    [Fact]
    public async Task When_call_api_with_not_exist_symbol_Expect_get_not_found_response()
    {
        //-ARRANGE

        var dt = new DateTime(2024, 11, 24, 13, 38, 0);
        var idateTimeMoq = new Mock<IDateTimeProvider>();
        idateTimeMoq.Setup(x => x.Now).Returns(dt);

        _client = _factory.WithWebHostBuilder(cfg =>
        {

            cfg.ConfigureServices(srv =>
            {
                srv.RemoveAll<IDateTimeProvider>();
                srv.AddScoped<IDateTimeProvider>(_ => idateTimeMoq.Object);
            });

        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);

        await AddRequierAsync();
        var pathFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            nameof(Modules),
            nameof(SymbolModuleTest),
            nameof(BoundTest),
            nameof(FeaturesTest),
            nameof(FixedIncomeProfitDailyUploadExcellFileTest), "profitDaily.xlsx");

        var checkExistExcellFile = File.Exists(pathFile);

        checkExistExcellFile.Should().Be(true);

        using var stream = File.OpenRead(pathFile);


        var symbolIsin = "IRB5AE800017";
        var formContent = new MultipartFormDataContent
        {
            // Send form text values here
            //{new StringContent(symbolIsin),"SymbolIsin" },
            // Send file Here
            {new StreamContent(stream),"file","profitDaily.xlsx"}
        };


        //ACT
        var response = await _client.PostAsync($"{_apiAddress.Replace("[ISIN]", symbolIsin)}", formContent);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //ASSERTION
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var fixedIncome = await _factory.Repositories.FixedIncomeGetByIsinAsync(symbolIsin);
        var profitDailies = await _factory.Repositories.FixedIncomeProfitDailyGetsByIsisnAsync(symbolIsin);


        //-- Check insert by excell
        fixedIncome.FormulaType.Should().Be(100); // EXCELL = 100

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var fixedIncomeProfitDailyList = new List<FixedIncomeProfitDailyTest>();
        using (var excelPack = new ExcelPackage())
        {
            excelPack.Load(stream);

            var ws = excelPack.Workbook.Worksheets[0];

            //Get row details
            int col = 1;
            for (int rowNum = 2; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                col = 1;

                var fixedIncomeProfitDaily = new FixedIncomeProfitDailyTest
                {
                    FixedIncomeIdFk = fixedIncome.FixedIncomeIdPk,
                    HasAdjusted = false,
                    ModifiedDate = dt,
                    NominalPrice = (decimal)fixedIncome.NominalValue
                };
                try
                {
                    fixedIncomeProfitDaily.ProfitDailyDate =
                        ws.GetStringValue(rowNum, col++).ConvertToDateFromPersianDate();
                }
                catch
                {
                    throw new NotValidDataException($"can not read profitDailyDate : Excel[{rowNum},{--col}]");
                }

                try
                {
                    fixedIncomeProfitDaily.ProfitDailyPrice = ws.GetDoubleValue(rowNum, col++);
                    if (fixedIncomeProfitDaily.ProfitDailyPrice == 0)
                        fixedIncomeProfitDaily.HasAdjusted = true;
                }
                catch
                {
                    throw new NotValidDataException($"can not read ProfitDailyPrice : Excel[{rowNum},{--col}]");
                }

                try
                {
                    fixedIncomeProfitDaily.SubscriptionProfit = ws.GetDoubleValue(rowNum, col++);
                }
                catch
                {
                    throw new NotValidDataException($"can not read SubscriptionProfit : Excel[{rowNum},{--col}]");
                }

                fixedIncomeProfitDailyList.Add(fixedIncomeProfitDaily);
            }
        }


        profitDailies.Should().BeEquivalentTo(fixedIncomeProfitDailyList);

    }

}
