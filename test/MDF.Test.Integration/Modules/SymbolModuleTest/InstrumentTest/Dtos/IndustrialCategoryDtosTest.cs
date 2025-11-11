namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

internal class IndustrialCategoryGetDtoTest
{
    public int? Id { get; set; }
    public int? ParentId { get; set; }
    public string? ParentTitle { get; set; }
    public string? ParentCode { get; set; }
    public string? Title { get; set; } = null!;
    public string? Code { get; set; } = null!;
}

internal class IndustrialCategoryCreateDtoTest
{
    public int? ParentIndustrialCategoryId { get; set; } = default!;
    public string? Title { get; set; } = null!;
    public string Code { get; set; } = default!;
}