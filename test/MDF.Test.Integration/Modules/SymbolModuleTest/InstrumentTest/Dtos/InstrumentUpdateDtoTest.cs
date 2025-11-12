namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

internal class InstrumentUpdateDtoTest
{
    public int InstrumentId { get; set; }
    public int? InstrumentTypeId { get; set; } = default!;
    public long? UnitCount { get; set; } = default!;
    public string? EnTitle { get; set; } = default!;
}
