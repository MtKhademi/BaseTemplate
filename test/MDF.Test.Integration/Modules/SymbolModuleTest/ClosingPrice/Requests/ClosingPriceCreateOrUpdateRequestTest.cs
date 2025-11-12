namespace MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.Requests;

public record ClosingPriceCreateOrUpdateRequestTest(
    string? SymbolIsin = null,
    string? TypeOfClosingPriceIndicator = null,
    double? ClosingPrice = null,
    double? LastTradePrice = null,
    int? TotalNumberOfTrade = null,
    long? TotalNumberOfSharesTrade = null,
    string? DateTimeOfEvent = null
);