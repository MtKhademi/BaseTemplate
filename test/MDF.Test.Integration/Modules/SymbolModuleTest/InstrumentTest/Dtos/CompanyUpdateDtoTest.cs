namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

internal class CompanyUpdateDtoTest
{
    public int CompanyId { get; set; }
    public int? IndustrialCategoryId { get; set; } = default!;
    public string? Title { get; set; } = default!;
    public string? CompanyCode { get; set; } = default!;
}
