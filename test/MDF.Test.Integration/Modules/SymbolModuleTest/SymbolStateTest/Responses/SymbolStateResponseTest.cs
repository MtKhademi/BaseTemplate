namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolStateTest.Responses;

public record SymbolStateResponseTest(
    int? SymbolStateId = null,
    int? SymbolId = null,
    string? SymbolIsin = default!,
    string? SymbolName = default!,
    string? StateTypeCode = default!,
    string? StateTypeTitle = default!,
    string? DateOfEvent = default!);

