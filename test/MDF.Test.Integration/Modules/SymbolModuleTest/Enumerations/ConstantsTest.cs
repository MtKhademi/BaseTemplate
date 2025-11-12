using Test.Integration.ModulesTest.SymbolModuleTest.Enumerations;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.Enumerations;

public static class MarketConstantsTest
{
    public const string NORMAL_MARKET = "NO";
    public const string ODD_LOT_MARKET = "OL";
    public const string BUYING_IN_MARKET = "BY";
    public const string BLOCK_MARKET = "BK";
    public const string INDEX_MARKET = "ID";
    public const string INSTRUMENTS = "UI";
    public const string UNKNOWN = "";

}

public static class TypeOfSymbolConstantsTest
{
    public static List<TypeOfSymbolTest> PutOptionTypesTest = [
                TypeOfSymbolTest.PutOption,
                TypeOfSymbolTest.PutEmbedded,
                TypeOfSymbolTest.CallEmbedded,
                TypeOfSymbolTest.CallOption,
                TypeOfSymbolTest.Future,
                ];

    public static List<TypeOfSymbolTest> FixedIncomeTypesTest = [
                 TypeOfSymbolTest.Bond,
                 TypeOfSymbolTest.Bond_Ejare,
                 TypeOfSymbolTest.Bond_Morabehe,
                 TypeOfSymbolTest.Bond_GovahiEhtebarMovaled,
                 TypeOfSymbolTest.Bond_KhazanehEslami,
                 TypeOfSymbolTest.Bond_Salaf,
                 TypeOfSymbolTest.Bond_Sakok_Morabehe,
                 TypeOfSymbolTest.Bond_Sakok_Ejare,
                 TypeOfSymbolTest.Bond_GharzolHasane,
                 TypeOfSymbolTest.Bond_Debentures,
                ];

    public static List<TypeOfSymbolTest> SalafTypesTest = [
                 TypeOfSymbolTest.Bond_Salaf,
                ];


    public static bool IsBond(this TypeOfSymbolTest value) => FixedIncomeTypesTest.Contains(value);

}
