using Test.Integration.ModulesTest.SymbolModuleTest.Enumerations;

namespace Test.Integration.ModulesTest.SymbolModuleTest.Dtos;


public class ClosingPriceGetDtoTest
{
    /// شناسه اصلی جدول
    /// </summary>
    public int ClosingPriceId { get; set; }
    /// <summary>
    /// شناسه خارجی Symbol
    /// </summary>
    public int SymbolId { get; set; }
    public string SymbolIsin { get; set; }
    public string SymbolName { get; set; }
    /// <summary>
    /// شناسه خارجی CloseIndicator
    /// </summary>
    public byte IndicatorType { get; set; }
    /// <summary>
    /// شناسه خارجی ClosingPriceType
    /// </summary>
    public TypeOfClosingPriceIndicatorTest TypeOfClosingPriceIndicator { get; set; }
    /// <summary>
    /// قیمت پایانی معامله
    /// </summary>
    public double ClosingPrice { get; set; }

    /// <summary>
    /// قیمت آخرین معامله
    /// </summary>
    public double LastTradePrice { get; set; }

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
    public string? DateTimeOfEvent { get; set; }
    /// <summary>
    /// تاریخ رویداد
    /// </summary>
    public string? DateOfEvent { get; set; }

}

