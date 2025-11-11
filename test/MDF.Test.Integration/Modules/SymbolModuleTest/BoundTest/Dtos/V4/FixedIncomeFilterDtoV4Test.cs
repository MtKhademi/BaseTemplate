using Test.Integration.ModulesTest.SymbolModuleTest.Enumerations;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.V4;

public class FixedIncomeFilterDtoV4Test
{
    public string? MaturityDate { get; set; }
    public string? SubscriptionEndDate { get; set; }
    public string? SubscriptionStartDate { get; set; }
    public List<TypeOfSymbolTest>? TypeOfSymbols { get; set; }
    public string? Isins { get; set; }
}


