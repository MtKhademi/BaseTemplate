using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Requests;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.FeaturesTest.SymbolCreateTest;

internal class NotCorrectDataTest : TheoryData<SymbolCreateDtoTest,List<string>>
{
    public NotCorrectDataTest()
    {
        var dtEvent = (new DateTime(2025, 05, 06, 14, 50, 00));
        var dto = new SymbolCreateDtoTest
        {
            BoardCode = 1,
            BoardName = "تابلو اصلي",

            MarketCode = "NO",
            MarketName = "بازار عادی",

            BoardSecurityExchangeCode = 1,
            MarketSecurityExchangeCode = 1,
            BaseVolume = 10,
            BourseCode = "BourseCode",
            CdsSymbolName = "CdsSymbolName",
            DateOfEvent = dtEvent.GetISOStringDateTime(),
            EnSymbol = "EnSymbol",

            SymbolGroupId = 1,
            InstrumentId = 100,
            Isin = "Isin8585XX",
            Lot = 10,
            MaxQuantityOrder = 10,
            MinQuantityOrder = 10,
            SymbolCodeTse = "SymbolCodeTse",
            SymbolCodeTseSafeEncoding = "SymbolCodeTseSafeEncoding",
            SymbolName = "SymbolName",
            SymbolNameModified = "SymbolNameModified",
            SymbolNameTse = "SymbolNameTse",
            Title = "Title",
            TypeOfSymbol = TypeOfSymbolTest.Bond_Salaf,
            TypeOfSymbolInTseTmc = TypeOfSymbolTest.Commodity
        };
        Add(dto, new List<string>
        {
            "there is not exist any InstrumentEntity =\u003E InstrumentIdPk : 100"
        });


        dtEvent = (new DateTime(2025, 05, 06, 14, 50, 00));
        dto = new SymbolCreateDtoTest
        {
            BoardCode = 10,
            BoardName = "تابلو اصلي",

            MarketCode = "NO",
            MarketName = "بازار عادی",

            BoardSecurityExchangeCode = 1,
            MarketSecurityExchangeCode = 1,
            BaseVolume = 1,
            BourseCode = "BourseCode",
            CdsSymbolName = "CdsSymbolName",
            DateOfEvent = dtEvent.GetISOStringDateTime(),
            EnSymbol = "EnSymbol",

            SymbolGroupId = 1,
            InstrumentId = 1,
            Isin = "Isin8585XX",
            Lot = 10,
            MaxQuantityOrder = 10,
            MinQuantityOrder = 10,
            SymbolCodeTse = "SymbolCodeTse",
            SymbolCodeTseSafeEncoding = "SymbolCodeTseSafeEncoding",
            SymbolName = "SymbolName",
            SymbolNameModified = "SymbolNameModified",
            SymbolNameTse = "SymbolNameTse",
            Title = "Title",
            TypeOfSymbol = TypeOfSymbolTest.Bond_Salaf,
            TypeOfSymbolInTseTmc = TypeOfSymbolTest.Commodity
        };
        Add(dto, new List<string>
        {
            "there is not exist any BoardEntity =\u003E BoardCode : 10"
        });

        dtEvent = (new DateTime(2025, 05, 06, 14, 50, 00));
        dto = new SymbolCreateDtoTest
        {
            BoardCode = 1,
            BoardName = "تابلو اصلي",

            MarketCode = "NBO",
            MarketName = "بازار عادی",

            BoardSecurityExchangeCode = 1,
            MarketSecurityExchangeCode = 1,
            BaseVolume = 1,
            BourseCode = "BourseCode",
            CdsSymbolName = "CdsSymbolName",
            DateOfEvent = dtEvent.GetISOStringDateTime(),
            EnSymbol = "EnSymbol",

            SymbolGroupId = 1,
            InstrumentId = 1,
            Isin = "Isin8585XX",
            Lot = 10,
            MaxQuantityOrder = 10,
            MinQuantityOrder = 10,
            SymbolCodeTse = "SymbolCodeTse",
            SymbolCodeTseSafeEncoding = "SymbolCodeTseSafeEncoding",
            SymbolName = "SymbolName",
            SymbolNameModified = "SymbolNameModified",
            SymbolNameTse = "SymbolNameTse",
            Title = "Title",
            TypeOfSymbol = TypeOfSymbolTest.Bond_Salaf,
            TypeOfSymbolInTseTmc = TypeOfSymbolTest.Commodity
        };
        Add(dto, new List<string>
        {
            "there is not exist any MarketEntity =\u003E Code : NBO"
        });

    }
}
