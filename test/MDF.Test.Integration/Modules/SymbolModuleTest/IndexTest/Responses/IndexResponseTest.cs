namespace MDF.Test.Integration.Modules.SymbolModuleTest.IndexTest.Responses;

public sealed record IndexResponseTest(
    int? IndexDataId,
    int? SymbolId,
    string? SymbolIsin,
    string? SymbolName,
    string? DateOfEvent,
    byte? IndexLevelId,
    string? IndexLevelCode,
    string? IndexLevelTitle,
    double? PercentVariation,
    short? SignVariation,
    double? IndexValue,
    double? IndexChange = default!
);