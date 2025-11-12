using MDF.Modules.SymbolModule.Depricate.Abstractions;
using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class FixedIncomeTableFilterDtoV2TestBuilder
{
    private readonly FixedIncomeTableFilterDtoV2Test _dto = new FixedIncomeTableFilterDtoV2Test();
    public FixedIncomeTableFilterDtoV2TestBuilder()
    {
    }
    public FixedIncomeTableFilterDtoV2TestBuilder WithDueDate(string? value)
    {
        _dto.MaturityDate = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV2TestBuilder WithEndOfSubscription(string? value)
    {
        _dto.SubscriptionEndDate = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV2TestBuilder WithStartOfSubscription(string? value)
    {
        _dto.SubscriptionStartDate = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV2TestBuilder WithTypeOfSymbols(List<ETypeOfSymbol>? value)
    {
        _dto.TypeOfSymbols = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV2TestBuilder WithIsin(string? value)
    {
        _dto.Isins = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV2Test Build() => _dto;
}
