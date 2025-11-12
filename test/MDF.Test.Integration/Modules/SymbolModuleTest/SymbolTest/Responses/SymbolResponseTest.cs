namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Responses;

public class SymbolResponseTest
{
    public int? SymbolId { get; set; } = default!;
    public int? BaseVolume { get; set; } = default!;
    public string? EnSymbol { get; set; }
    public string? CompanyName { get; set; } = default!;
    public string? CompanyCode { get; set; } = default!;
    public string? SymbolName { get; set; } = default!;
    public string? Isin { get; set; } = default!;
    public string? BourseCode { get; set; } = default!;
    public string? DateOfEvent { get; set; } = default!;
    public string? EntryDate { get; set; } = default!;
    public string? LastModifiedDate { get; set; } = default!;

    public int? FirmId { get; set; } = default!;
    public bool? IsActive => !IsDisabled;
    public bool? IsDisabled { get; set; } = default!;
    public string? DisableDateTime { get; set; } = default!;

    public string? MarketCode { get; set; } = default!;
    public string? MarketName { get; set; } = default!;

    public byte? BoardCode { get; set; }
    public string BoardName { get; set; }

    public string? SecurityExchangeNameForExchangeMarket { get; set; } = default!;
    public byte? SecurityExchangeCodeForExchangeMarket { get; set; } = default!;

    public string SecurityExchangeNameForExchangeBoard { get; set; } = default!;
    public byte? SecurityExchangeCodeForExchangeBoard { get; set; } = default!;

    public TypeOfSymbolTest? TypeOfSymbol { get; set; } = TypeOfSymbolTest.Undefined;
    public TypeOfSymbolTest? TypeOfSymbolInTseTmc { get; set; } = TypeOfSymbolTest.Undefined;
    public string? TypeOfSymbolPersianName => TypeOfSymbol.GetDescription();
    public int? Lot { get; set; }
    public int? MinQuantityOrder { get; set; }
    public int? MaxQuantityOrder { get; set; }
    public string? SymbolNameTse { get; set; }
    public string? Title { get; set; }

    public string? CdsSymbolName { get; set; }

    public string? IndustrialCategoryTitle { get; set; }
    public string? IndustrialCategoryId { get; set; }
    public string? IndustrialCategoryCode { get; set; }
    public string? IndustrialCategoryParentCode { get; set; }
    public int? IndustrialCategoryParentId { get; set; }
    public string? IndustrialCategoryParentTitle { get; set; }


    public string? InstrumentISIN { get; set; }
    public string? InstrumentTitle { get; set; }
    public string? InstrumentType { get; set; }
    public int? InstrumentTypeCode { get; set; }
    public long? InstrumentUnitCount { get; set; }

    public bool IsOldSymbol { get; set; }
    public string? OldSymbolIsin { get; set; }

    public int? EtfType { get; set; }
    public string? EtfTypeTitle { get; set; }
    public long? InstCodeTse { get; set; }

    public byte? SymbolGroupId { get; set; }
    public string? SymbolGroupTitle { get; set; }
    public string? SymbolGroupCode { get; set; }

    public int? SettlementPeriod { get; set; }

}
