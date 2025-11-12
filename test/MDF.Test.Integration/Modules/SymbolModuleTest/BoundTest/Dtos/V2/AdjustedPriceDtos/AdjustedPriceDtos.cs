using MDF.Common.Infrastructure;

namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.AdjustedPriceDtos
{
    #region AdjustedPriceCreateDto

    public class AdjustedPriceCreateDto
    {
        public string? StartDateTime { get; set; } = null;
        public string? Isin { get; set; } = null;
        public int? FirmId { get; set; } = null;
        public string? EndDateTime { get; set; } = null;
    }

    #endregion

    #region AdjustedPriceDeleteDto
    public class AdjustedPriceDeleteDTO
    {
        public int? AdjustedPriceId { get; set; }
    }
    #endregion

    #region AdjustedPriceGetDto

    [Obsolete("please don't use it")]
    public class AdjustedPriceGetDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int AnnouncementId { get; set; }
        public int DividendId { get; set; }
        public int CapitalChangeId { get; set; }
        public int AdjustedLastPrice { get; set; }
        public int AdjustedPrice { get; set; }
        public int ClosingPrice { get; set; }
        public int LastTradedPrice { get; set; }
        public bool IsAdjusted { get; set; }
        public bool IsTradable { get; set; }
        public string Date { get; set; }
        public string SymbolName { get; set; }
        public string SymbolIsin { get; set; }
    }

    #endregion

    #region AdjustedPriceFilterDto
    public class AdjustedPriceFilterDto
    {
        public string? DateTime { get; set; }
        public List<string>? Isins { get; set; } = null;
        public bool? IsDeleted { get; set; } = null;
    }
    public class AdjustedPriceTableFilterDto : AdjustedPriceFilterDto, IBasePagination
    {
        public int? CurrentPage { get; set; }
        public int? SizeOfPage { get; set; }
    }


    #endregion

    #region AdjustedPrice Table Dto
    //public class AdjustedPriceTableDto : BaseTable<AdjustedPriceGetDto>
    //{
    //    public AdjustedPriceTableDto()
    //    {

    //    }
    //    public AdjustedPriceTableDto(IBasePagination pagination)
    //    {
    //        this.CurrentPage = pagination.CurrentPage;
    //        this.SizeOfPage = pagination.SizeOfPage;
    //    }
    //}
    #endregion
}
