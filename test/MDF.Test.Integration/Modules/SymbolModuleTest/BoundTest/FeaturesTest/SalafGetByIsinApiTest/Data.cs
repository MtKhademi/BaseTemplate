namespace Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafGetByIsinApiTest;

internal class SalafGetByIsinNotValidData : TheoryData<string, List<string>>
{
    public SalafGetByIsinNotValidData()
    {
        Add("XFV", [
            "Isin can not be convert to ISIN =\u003E isisn has to bigger or equal than 7 length :: XFV"
  ]);

    }
}

