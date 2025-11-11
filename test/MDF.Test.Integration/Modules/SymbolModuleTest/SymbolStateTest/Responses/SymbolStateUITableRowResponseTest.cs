namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolStateTest.Responses;

public record SymbolStateUITableRowResponseTest(
    int? SymbolStateId = null,
    int? SymbolId = null,
    string? SymbolIsin = default!,
    string? SymbolName = default!,
    string? StateTypeCode = default!,
    string? StateTypeTitle = default!,
    string? DateOfEvent = default!) : IUIGridRow
{
    public IList<string> Actions { get; set; } = [];

}
