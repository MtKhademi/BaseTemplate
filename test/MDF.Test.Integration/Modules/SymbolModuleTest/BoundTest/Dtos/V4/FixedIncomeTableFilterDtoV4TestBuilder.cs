using MDF.Modules.SymbolModule.Depricate.Abstractions;
using Test.Integration.ModulesTest.SymbolModuleTest.Enumerations;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.V4;

internal class FixedIncomeTableFilterDtoV4TestBuilder
{
    private readonly FixedIncomeTableFilterDtoV4Test _dto = new FixedIncomeTableFilterDtoV4Test();
    public FixedIncomeTableFilterDtoV4TestBuilder()
    {
    }
    public FixedIncomeTableFilterDtoV4TestBuilder WithMaturityDate(string? value)
    {
        _dto.MaturityDate = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV4TestBuilder WithEndOfSubscription(string? value)
    {
        _dto.SubscriptionEndDate = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV4TestBuilder WithStartOfSubscription(string? value)
    {
        _dto.SubscriptionStartDate = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV4TestBuilder WithTypeOfSymbols(List<TypeOfSymbolTest>? value)
    {
        _dto.TypeOfSymbols = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV4TestBuilder WithIsin(string? value)
    {
        _dto.Isins = value;
        return this;
    }
    public FixedIncomeTableFilterDtoV4Test Build() => _dto;
}
