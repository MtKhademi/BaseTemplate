using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class PutOptionUpdateGroupDtoV2TestBuilder
{
    private readonly PutOptionUpdateGroupDtoV2Test _dto = new PutOptionUpdateGroupDtoV2Test();
    public PutOptionUpdateGroupDtoV2TestBuilder()
    {
    }


    public PutOptionUpdateGroupDtoV2TestBuilder WithPutOptionIds(List<int>? value)
    {
        _dto.PutOptionIds = value;
        return this;
    }
    public PutOptionUpdateGroupDtoV2TestBuilder WithApplyNewPrice(decimal? value)
    {
        _dto.ApplyNewPrice = value;
        return this;
    }
    public PutOptionUpdateGroupDtoV2TestBuilder WithApplyNewDate(string value)
    {
        _dto.ApplyNewDate = value;
        return this;
    }
    public PutOptionUpdateGroupDtoV2TestBuilder WithStartNewDate(string? value)
    {
        _dto.StartNewDate = value;
        return this;
    }



    public PutOptionUpdateGroupDtoV2Test Build() => _dto;
}
