using MDF.Common.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using Test.Integration.ModulesTest.SymbolModuleTest.Enumerations;

namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos;

#region SymbolUpdateDto

public class SymbolUpdateDtoTest
{

    public byte? SecurityExchangeCodeForExchangeBoard { get; set; }
    public byte? BoardCode { get; set; }

    public byte? SecurityExchangeCodeForExchangeMarket { get; set; }
    public string? MarketCode { get; set; }

    public int? InstrumentId { get; set; }
    public byte? SymbolGroupId { get; set; }

    public int? FirmId { get; set; }

    public string? Title { get; set; }
    public string? SymbolName { get; set; } = null!;
    public string? Isin { get; set; } = null!;
    public string? EnSymbol { get; set; } = null!;
    public string? DateOfEvent { get; set; }
    public string? EntryDate { get; set; }
    public string? SymbolNameTse { get; set; }
    public string? SymbolCodeTse { get; set; }
    public string? BourseCode { get; set; }
    public string? SymbolCodeTseSafeEncoding { get; set; }
    public string? CdsSymbolName { get; set; }
    public bool? IsDisabled { get; set; }
    public int? SettlementPeriod { get; set; }
    public byte? ExchangeTypeIdFk { get; set; }
    public string? CreatedDateTime { get; set; }
    public string? LastModifiedDate { get; set; }
    public TypeOfSymbolTest? TypeOfSymbol { get; set; }
}


#endregion

#region SymbolDeleteDto
public class SymbolDeleteDTO
{
    public int? SymbolId { get; set; }
}
#endregion

#region SymbolGetMinimalDto
public class SymbolGetMinimalDto
{
    public string SymbolName { get; set; } = null!;
    public string Isin { get; set; } = null!;
    public bool? IsDisabled { get; set; }
}
#endregion

#region SymbolGetDto
public class SymbolGetDtoV2Test : IUIGridRow
{
    public int SymbolIdPk { get; set; }
    public string CompanyName { get; set; }
    public string SymbolName { get; set; }
    public string Isin { get; set; }
    public DateTime DateOfEvent { get; set; }
    public int FirmId { get; set; }
    public bool IsActive { get; set; }
    public string? DisableDateTime { get; set; }
    public string MarketCode { get; set; }
    public string MarketName { get; set; }
    public string SecurityExchangeName { get; set; }
    public string TypeOfSymbol { get; set; }
    public string TypeOfSymbolPersianName { get; set; }
    public byte? SecurityExchangeCodeForExchangeMarket { get; set; }
    public IList<string> Actions { get; set; } = new List<string>();
}
#endregion

#region SymbolFilterDto


public class SymbolMinimalFilterDto
{
    public bool? IsDisable { get; set; } = null;
    public string? SymbolNameOrIsin { get; set; } = null;
}

public class SymbolFilterDto
{
    public bool? IsDisable { get; set; } = null;
    public string? MarketCode { get; set; }
    public bool? HasFirm { get; set; } = null;
    public int? FirmId { get; set; } = null;
    public string? SymbolNameOrIsin { get; set; } = null;
    public List<string>? Isins { get; set; } = null;
    public List<string>? TypeOfSymbols { get; set; } = default!;
}


public class SymbolTableFilterDto : SymbolFilterDto, IBasePagination
{
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }

}
#endregion

#region SymbolChange


public class SymbolChangeFilterDto
{
    public string? DateTime { get; set; }
}

public class SymbolChangeGetDtoTest
{
    public string? CompanyName { get; set; }
    public string? CompanyCode { get; set; }
    public string? Exchange { get; set; }
    public byte? ExchangeCode { get; set; }
    public byte? BoardCode { get; set; }
    public string? Board { get; set; }
    public string? InstrumentType { get; set; }
    public string? InstrumentISIN { get; set; }
    public int InstrumentTypeCode { get; set; }
    public long UnitCount { get; set; }
    public string? SymbolName { get; set; }
    public string? EnSymbol { get; set; }
    public string? Market { get; set; }
    public string? MarketCode { get; set; }
    public string? SymbolISIN { get; set; }
    public string? IndustrialCategory { get; set; }
    public string? IndustrialCategoryCode { get; set; }
    public int? IndustrialCategoryParentId { get; set; }
    public int SymbolId { get; set; }
    public int LOT { get; set; }
    public int MaxQuantityOrder { get; set; }
    public int MinQuantityOrder { get; set; }
    public int? BaseVolume { get; set; }
    public string? SymbolTitle { get; set; }
    public string? IndustrialCategoryParentTitle { get; set; }
    public string? IndustrialCategoryParentCode { get; set; }
    public Int64? InsCode { get; set; }

    [Column("IsOldSymbol")]
    private int? _IsOldSymbol { get; set; }
    [NotMapped]
    public bool? IsOldSymbol
    {
        get
        {

            return (_IsOldSymbol.HasValue && _IsOldSymbol.Value != 0) ? true : false;
        }
        set
        {
            _IsOldSymbol = value.HasValue && value.Value ? 1 : 0;
        }
    }


    public string? OldSymbolISIN { get; set; }
    public bool? IsDisabled { get; set; }
    public int? SettlementPeriod { get; set; }
    public string? SymbolGroupCode { get; set; }
    public string? SymbolGroupTitle { get; set; }
    public DateTime? EntryDate { get; set; }
    public DateTime? DateOfEvent { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
#endregion
    
