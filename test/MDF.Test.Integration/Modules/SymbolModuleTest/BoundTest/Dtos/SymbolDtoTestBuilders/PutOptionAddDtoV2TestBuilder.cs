namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class PutOptionAddDtoV2TestBuilder
{
    private readonly PutOptionAddDtoV2Test _dto = new PutOptionAddDtoV2Test();
    public PutOptionAddDtoV2TestBuilder()
    {
    }

    public PutOptionAddDtoV2TestBuilder WithSymbolBaseIsin(string? value)
    {
        _dto.SymbolBaseIsin = value;
        return this;
    }
    public PutOptionAddDtoV2TestBuilder WithSymbolPutOptionIsin(string? value)
    {
        _dto.SymbolPutOptionIsin = value;
        return this;
    }
    public PutOptionAddDtoV2TestBuilder WithApplyDate(string value)
    {
        _dto.ApplyDate = value;
        return this;
    }
    public PutOptionAddDtoV2TestBuilder WithStartDate(string? value)
    {
        _dto.StartDate = value;
        return this;
    }
    public PutOptionAddDtoV2TestBuilder WithApplyPrice(decimal? value)
    {
        _dto.ApplyPrice = value;
        return this;
    }

    public PutOptionAddDtoV2TestBuilder WithOptionForFinance(ETypeOfPutOptinForFinanceTest? value)
    {
        _dto.OptionForFinance = value;
        return this;
    }


    public PutOptionAddDtoV2Test Build() => _dto;
}
