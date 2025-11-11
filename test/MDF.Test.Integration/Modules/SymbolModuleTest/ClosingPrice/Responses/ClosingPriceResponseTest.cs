namespace MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.Responses;

internal record ClosingPriceResponseTest(
    long? ClosingPriceId = null,
    int? SymbolId = null,
    string? SymbolIsin = null,
    string? SymbolName = null,
    byte? IndicatorType = null,
    TypeOfClosingPriceIndicatorTest? TypeOfClosingPriceIndicator = null,
    double? ClosingPrice = null,
    double? LastTradePrice = null,
    int? TotalNumberOfTrade = null,
    long? TotalNumberOfSharesTrade = null,
    long? TotalTradeValue = null,
    int? PercentageThresholdNormalTrade = null,
    int? PercentageThresholdCrossTrade = null,
    int? DailyAverageSharesTrade = null,
    string? DateTimeOfEvent = null,
    string? DateOfEvent = null,
    string? CreatedDate = null
);