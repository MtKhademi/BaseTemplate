namespace MDF.Test.Integration.Modules.SymbolModuleTest.InstrumentTest.Dtos;

internal class CompanyGetDtoTest
{
    /// <summary>
    /// شناسه اصلی جدول
    /// </summary>
    public int? CompanyId { get; set; }
    /// <summary>
    /// عنوان شرکت
    /// </summary>
    public string? Title { get; set; } = null!;
    /// <summary>
    /// کد شرکت
    /// </summary>
    public string? CompanyCode { get; set; } = null!;
    /// <summary>
    /// تاریخ پیام
    /// </summary>
    public string? DateOfEvent { get; set; }
    public int? FirmIdFk { get; set; }



    /// <summary>
    /// شناسه خارجی CompanyType
    /// </summary>
    public byte? CompanyTypeId { get; set; }
    /// <summary>
    /// عنوان
    /// </summary>
    public string? CompanyTypeTitle { get; set; } = null!;
    /// <summary>
    /// کد
    /// </summary>
    public string? CompanyTypeCode { get; set; } = null!;
    /// <summary>
    /// عنوان انگلیسی
    /// </summary>
    public string? CompanyTypeEnTitle { get; set; }



    /// <summary>
    /// شناسه خارجی IndustrialCategory
    /// </summary>
    public int? IndustrialCategoryId { get; set; }
    /// <summary>
    /// عنوان صنعت
    /// </summary>
    public string? IndustrialCategoryTitle { get; set; } = null!;
    /// <summary>
    /// کد صنعت 
    /// </summary>
    public string? IndustrialCategoryCode { get; set; } = null!;
}
