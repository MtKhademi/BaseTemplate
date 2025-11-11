namespace MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.Requests;

public record ClosingPriceGetListRequestTest(
    string[]? Isins = null,
    string? Isin = null,
    TypeOfClosingPriceIndicatorTest? IndicatorType = null,
    string? StartDate = null,
    string? EndDate = null
);

