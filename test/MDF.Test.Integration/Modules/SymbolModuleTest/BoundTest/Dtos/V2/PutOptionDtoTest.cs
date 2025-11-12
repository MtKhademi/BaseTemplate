using MDF.Common.Infrastructure;
using MDF.Modules.Modules.AnnouncementModule.Abstractions;

namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos
{

    public class PutOptionAddDtoV2Test
    {
        public string? SymbolPutOptionIsin { get; set; }
        public string SymbolBaseIsin { get; set; }
        public string? ApplyDate { get; set; }
        public string? StartDate { get; set; }
        public decimal? ApplyPrice { get; set; }
        public ETypeOfPutOptinForFinanceTest? OptionForFinance { get; set; }
    }

    public class PutOptionUpdateDtoV2Test
    {
        public int? PutOptionId { get; set; }
        public string ApplyDate { get; set; }
        public string StartDate { get; set; }
        public decimal? ApplyPrice { get; set; }
        public ETypeOfPutOptinForFinanceTest? OptionForFinance { get; set; }
    }

    public class PutOptionUpdateGroupDtoV2Test
    {
        public List<int>? PutOptionIds { get; set; }
        public string? ApplyNewDate { get; set; }
        public string? StartNewDate { get; set; }
        public decimal? ApplyNewPrice { get; set; }
    }

    public class PutOptionFilterGetDtoTest
    {
        public string? BaseSymbolIsin { get; set; }
        public List<string>? Isins { get; set; }
        public bool? IsDeleted { get; set; }
        public string? ApplyDate { get; set; }
        public string? StartDate { get; set; }
    }

    public class PutOptionTableFilterDtoTestV2 : PutOptionFilterGetDtoTest, IBasePagination
    {
        public int? CurrentPage { get; set; }
        public int? SizeOfPage { get; set; }
    }

    public class PutOptionGetDtoTestV2
    {
        public string? SymbolIsin { get; set; }
        public string? SymbolName { get; set; }

        public string? PutOptionSymbolIsin { get; set; }
        public string? PutOptionSymbolName { get; set; }

        public decimal? ApplyPrice { get; set; }
        public string? State { get; set; }

        public DateTime? ApplyDate { get; set; }
        public DateTime? StartDate { get; set; }

        public string? OptionForWhichFinance { get; set; }
        public ETypeOfPutOptinForFinanceTest? OptionForFinance { get; set; }
        public IEnumerable<PutOptionDetailsDtoTest>? Details { get; set; }
    }

    public class PutOptionGetUIDtoTestV2 : PutOptionGetDtoTestV2, IUIGridRow
    {
        public IList<string> Actions { get; set; } = new List<string>();
    }

    public class PutOptionDetailsDtoTest
    {
        public string? AnnouncementIdFk { get; set; }
        public ETypeOfAnnouncement? AnnouncementType { get; set; }
        public decimal ApplyPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ApplyDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? ConfirmedManualTimeAnnoucement { get; set; }
        public DateTime? OpFiUpdateDate { get; set; }
        public ETypeOfPutOptinForFinanceTest? OptionForFinance { get; set; }
        public short? OpFiUpdateMode { get; set; }
    }


    public class PutOptionHistoryGetUIDtoV2Test : IUIGridRow
    {
        public int? SymbolId { get; set; }
        public string? SymbolName { get; set; }
        public string? SymbolIsin { get; set; }


        public int? SymbolPutOptionId { get; set; }
        public string? SymbolPutOptionName { get; set; }
        public string? SymbolPutOptionIsin { get; set; }
        public string? SymbolPutOptionDateTimeOfCreate { get; set; }

        public int? PutOptionId { get; set; }
        public string? AnnouncementIdFk { get; set; }
        public ETypeOfAnnouncement? AnnouncementType { get; set; } = null;
        public string AnnouncementTypeName { get; set; }

        public int PutOptionSymbolIdFk { get; set; }
        public int? SymbolIdFk { get; set; }
        public decimal? ApplyPrice { get; set; }
        public short? MeetingType { get; set; }
        public string? StartDate { get; set; }
        public string? ApplyDate { get; set; }
        public string? EntryDate { get; set; }
        public string? ModifyDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? ConfirmedManualTimeAnnoucement { get; set; }
        public decimal? ApplyPriceTest1 { get; set; }
        public decimal? ApplyPriceTest2 { get; set; }
        public string? OpFiUpdateDate { get; set; }
        public short? OpFiUpdateMode { get; set; }

        public string? OptionForWhichFinance { get; set; }
        public ETypeOfPutOptinForFinanceTest? OptionForFinance { get; set; }

        public IList<string> Actions { get; set; } = new List<string>();
    }
}
