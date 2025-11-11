namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

internal class InstrumentAddDtoTest
{
    public string? CompanyCode { get; set; }
    public int? InstrumentTypeCode { get; set; }
    public string? EnTitle { get; set; } = null!;
    /// <summary>
    /// کد ISIN ابزار مالی
    /// </summary>
    public string? Isin { get; set; } = null!;
    /// <summary>
    /// تعداد سهام
    /// </summary>
    public long? UnitCount { get; set; }
    /// <summary>
    /// تاریخ ارسال پیام
    /// </summary>
    public string? DateOfEvent { get; set; }
}
