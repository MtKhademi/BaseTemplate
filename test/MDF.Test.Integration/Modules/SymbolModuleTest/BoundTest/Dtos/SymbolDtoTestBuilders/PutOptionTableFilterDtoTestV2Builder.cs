using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class PutOptionTableFilterDtoTestV2Builder
{
    private readonly PutOptionTableFilterDtoTestV2 _dto = new PutOptionTableFilterDtoTestV2();
    public PutOptionTableFilterDtoTestV2Builder()
    {
    }
    public PutOptionTableFilterDtoTestV2Builder WithIsDeleted(bool? value)
    {
        _dto.IsDeleted = value;
        return this;
    }
    public PutOptionTableFilterDtoTestV2Builder WithSymbolIsin(string? value)
    {
        _dto.BaseSymbolIsin = value;
        return this;
    }
    public PutOptionTableFilterDtoTestV2Builder WithPutOptionSymbolIsin(List<string>? value)
    {
        _dto.Isins = value;
        return this;
    }
    public PutOptionTableFilterDtoTestV2Builder WithApplyDate(string? value)
    {
        _dto.ApplyDate = value;
        return this;
    }
    public PutOptionTableFilterDtoTestV2Builder WithStartDate(string? value)
    {
        _dto.StartDate = value;
        return this;
    }

    public PutOptionTableFilterDtoTestV2 Build() => _dto;
}
