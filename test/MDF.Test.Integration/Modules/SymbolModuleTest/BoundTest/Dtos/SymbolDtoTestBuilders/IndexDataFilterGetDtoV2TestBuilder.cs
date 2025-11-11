using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.IndexDataDtosV2Test;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class IndexDataFilterGetDtoV2TestBuilder
{
    private readonly IndexDataFilterGetDtoV2Test _dto = new IndexDataFilterGetDtoV2Test();
    public IndexDataFilterGetDtoV2TestBuilder()
    {
    }
    public IndexDataFilterGetDtoV2TestBuilder WithIsins(string? value)
    {
        _dto.Isin = value;
        return this;
    }
    public IndexDataFilterGetDtoV2TestBuilder WithDateOfEvent(string? value)
    {
        _dto.DateOfEvent = value;
        return this;
    }
    public IndexDataFilterGetDtoV2TestBuilder WithStartDateOfEvent(string? value)
    {
        _dto.StartDateOfEvent = value;
        return this;
    }
    public IndexDataFilterGetDtoV2TestBuilder WithEndDateOfEvent(string? value)
    {
        _dto.EndDateOfEvent = value;
        return this;
    }

    public IndexDataFilterGetDtoV2Test Build() => _dto;
}
