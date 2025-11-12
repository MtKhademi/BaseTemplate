using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.IndexDataDtosV2Test;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class IndexDataPricesPerDayFilterGetDtoV2TestBuilder
{
    private readonly IndexDataPricesPerDayFilterGetDtoV2Test _dto = new IndexDataPricesPerDayFilterGetDtoV2Test();
    public IndexDataPricesPerDayFilterGetDtoV2TestBuilder()
    {
    }
    public IndexDataPricesPerDayFilterGetDtoV2TestBuilder WithIsins(string? value)
    {
        _dto.Isin = value;
        return this;
    }
    public IndexDataPricesPerDayFilterGetDtoV2TestBuilder WithStartDateOfEvent(string? value)
    {
        _dto.StartDate = value;
        return this;
    }
    public IndexDataPricesPerDayFilterGetDtoV2TestBuilder WithEndDateOfEvent(string? value)
    {
        _dto.EndDate = value;
        return this;
    }

    public IndexDataPricesPerDayFilterGetDtoV2Test Build() => _dto;
}
