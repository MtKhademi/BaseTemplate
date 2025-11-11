namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Requests;

public record SymbolUpdateDtoTest(
    string? Title = default!,
    string? SymbolName = default!,
    string? Isin = default!,
    string? EnSymbol = default!,
    string? SymbolNameTse = default!,
    string? SymbolCodeTse = default!,
    string? CdsSymbolName = default!,
    bool? IsDisabled = default!,
    TypeOfSymbolTest? TypeOfSymbol = default!,
    TypeOfSymbolTest? TypeOfSymbolInTseTmc = default!,
    string? MarketCode = default!,
    byte? BoardCode = default!,
    byte? BoardSecurityExchangeCode = null,
    byte? MarketSecurityExchangeCode = null
    )
{

};
