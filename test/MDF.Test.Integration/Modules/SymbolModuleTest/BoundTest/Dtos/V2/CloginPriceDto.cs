using MDF.Common.Infrastructure;


namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos
{
    #region ClosingPriceAddDto

    public class ClosingPriceCreateDto
    {
        public string? SymbolIsin { get; set; }
        public byte? CloseIndicatorIdFk { get; set; }
        public double? ClosingPrice { get; set; }
        public double? LastTradePrice { get; set; }
        public int? TradesCount { get; set; }
        public long? TradesVolume { get; set; }
        public string? DateTimeOfEvent { get; set; }
    }

    #endregion

    #region ClosingPriceDeleteDto
    //public class ClosingPriceDeleteDTO
    //{
    //}
    #endregion

    #region ClosingPriceGetDto
    public class ClosingPriceGetDto
    {
        /// شناسه اصلی جدول
        /// </summary>
        public int ClosingPriceIdPk { get; set; }
        /// <summary>
        /// شناسه خارجی Symbol
        /// </summary>
        public int SymbolIdFk { get; set; }
        public string SymbolIsin { get; set; }
        public string SymbolName { get; set; }
        /// <summary>
        /// شناسه خارجی CloseIndicator
        /// </summary>
        public byte CloseIndicatorIdFk { get; set; }
        /// <summary>
        /// شناسه خارجی ClosingPriceType
        /// </summary>
        public byte ClosingPriceTypeIdFk { get; set; }
        /// <summary>
        /// قیمت پایانی معامله
        /// </summary>
        public double ClosingPrice1 { get; set; }
        /// <summary>
        /// قیمت پایانی تعدیل نشده
        /// </summary>
        public double ClosingPriceNoAdj { get; set; }
        /// <summary>
        /// قیمت آخرین معامله
        /// </summary>
        public double LastTradePrice { get; set; }
        /// <summary>
        /// آخرین قیمت معامله تعدیل نشده
        /// </summary>
        public double LastTradePriceNoAdj { get; set; }
        /// <summary>
        /// تعداد معامله انجام گرفته تا اکنون(در روز)
        /// </summary>
        public int TotalNumberOfTrade { get; set; }
        /// <summary>
        /// مجموع حجم معامله انجام گرفته تا اکنون(در روز)
        /// </summary>
        public long TotalNumberOfSharesTrade { get; set; }
        /// <summary>
        /// مجموع ارزش معامله انجام گرفته تا اکنون(در روز)
        /// </summary>
        public long TotalTradeValue { get; set; }
        /// <summary>
        /// درصد آستانه معامله عادی
        /// </summary>
        public int PercentageThresholdNormalTrade { get; set; }
        /// <summary>
        /// درصد آستانه معامله Cross
        /// </summary>
        public int PercentageThresholdCrossTrade { get; set; }
        /// <summary>
        /// میانگین روزانه تعداد معامله سهام در روز
        /// </summary>
        public int DailyAverageSharesTrade { get; set; }
        public string DateTimeOfEvent { get; set; }

    }
    #endregion

    #region ClosingPriceFilterDto
    public class ClosingPriceFilterDto
    {
        public string? Isin { get; set; }
        public int? IndicatorType { get; set; }
        public string? StartDateTime { get; set; }
        public string? EndDateTime { get; set; }
    }
    public class ClosingPriceTableFilterDto : ClosingPriceFilterDto, IBasePagination
    {
        public int? CurrentPage { get; set; }
        public int? SizeOfPage { get; set; }
    }
    #endregion

    #region ClosingPrice Table Dto
    //public class ClosingPriceTableDto : BaseTable<ClosingPriceGetDto>
    //{
    //    public ClosingPriceTableDto()
    //    {

    //    }
    //    public ClosingPriceTableDto(IBasePagination pagination)
    //    {
    //        this.CurrentPage = pagination.CurrentPage;
    //        this.SizeOfPage = pagination.SizeOfPage;
    //    }
    //}
    #endregion

}
