namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.FeaturesTest.SymbolTypeGetsTest;

internal class SymbolTypeGetsData : TheoryData<BaseTypeOfSymbolTest?, List<string>>
{
    public SymbolTypeGetsData()
    {
        Add(null, Enum.GetNames<TypeOfSymbolTest>().ToList());

        Add(BaseTypeOfSymbolTest.BOUND, 
            [
                TypeOfSymbolTest.Bond.ToString(),
                TypeOfSymbolTest.Bond_Debentures.ToString(),
                TypeOfSymbolTest.Bond_Ejare.ToString(),
                TypeOfSymbolTest.Bond_GharzolHasane.ToString(),
                TypeOfSymbolTest.Bond_GovahiEhtebarMovaled.ToString(),
                TypeOfSymbolTest.Bond_KhazanehEslami.ToString(),
                TypeOfSymbolTest.Bond_Morabehe.ToString(),
                TypeOfSymbolTest.Bond_Sakok_Ejare.ToString(),
                TypeOfSymbolTest.Bond_Sakok_Morabehe.ToString(),
                TypeOfSymbolTest.Bond_Salaf.ToString(),
            ]);

        Add(BaseTypeOfSymbolTest.OPTION,
            [
                TypeOfSymbolTest.CallOption.ToString(),
                TypeOfSymbolTest.PutOption.ToString(),
                TypeOfSymbolTest.PutEmbedded.ToString(),
                TypeOfSymbolTest.CallEmbedded.ToString(),
                TypeOfSymbolTest.Future.ToString(),
            ]);
    }
}
