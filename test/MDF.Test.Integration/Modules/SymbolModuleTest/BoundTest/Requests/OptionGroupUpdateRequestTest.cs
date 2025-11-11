namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;


public record OptionGroupUpdateRequestTest(
    int[]? optionIds,
    string? ApplyNewDate,
    string? StartNewDate);