namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class PutOptionUpdateDtoV2TestBuilder
{
    private readonly PutOptionUpdateDtoV2Test _dto = new PutOptionUpdateDtoV2Test();
    public PutOptionUpdateDtoV2TestBuilder()
    {
    }

    public PutOptionUpdateDtoV2TestBuilder WithPutOptionId(int? value)
    {
        _dto.PutOptionId = value;
        return this;
    }
    public PutOptionUpdateDtoV2TestBuilder WithApplyDate(string value)
    {
        _dto.ApplyDate = value;
        return this;
    }
    public PutOptionUpdateDtoV2TestBuilder WithStartDate(string? value)
    {
        _dto.StartDate = value;
        return this;
    }
    public PutOptionUpdateDtoV2TestBuilder WithApplyPrice(decimal? value)
    {
        _dto.ApplyPrice = value;
        return this;
    }
    public PutOptionUpdateDtoV2TestBuilder WithOptionForFinance(ETypeOfPutOptinForFinanceTest? value)
    {
        _dto.OptionForFinance = value;
        return this;
    }


    public PutOptionUpdateDtoV2Test Build() => _dto;
}
