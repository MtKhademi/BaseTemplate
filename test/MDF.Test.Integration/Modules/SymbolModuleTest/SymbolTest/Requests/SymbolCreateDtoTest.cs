namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Requests;

public class SymbolCreateDtoTest
{
    /// <summary>
    /// شناسه خارجی Instrument
    /// </summary>
    public int InstrumentId { get; set; }



    public byte? BoardCode { get; set; }
    public string BoardName { get; set; }

    public string? MarketCode { get; set; }
    public string MarketName { get; set; } = default!;


    public byte BoardSecurityExchangeCode { get; set; }
    public byte MarketSecurityExchangeCode { get; set; }
    /// <summary>
    /// شناسه خارجی SymbolGroup
    /// </summary>
    public byte SymbolGroupId { get; set; }




    /// <summary>
    /// عنوان نماد
    /// </summary>
    public string? Title { get; set; } = default!;
    /// <summary>
    /// نام نماد
    /// </summary>
    public string? SymbolName { get; set; } = default!;
    /// <summary>
    /// کد معاملاتی  نماد که از ترکیب IR و کدبازار و کد شرکت و کد نماد ایجاد شده است
    /// </summary>
    public string Isin { get; set; } = null!;
    /// <summary>
    /// نام انگلیسی نماد
    /// </summary>
    public string EnSymbol { get; set; } = null!;
    /// <summary>
    /// حداقل مقدار سفارش در هر معامله
    /// </summary>
    public int MinQuantityOrder { get; set; }
    /// <summary>
    /// حداکثر مقدار سفارش در هر معامله
    /// </summary>
    public int MaxQuantityOrder { get; set; }
    /// <summary>
    /// حجم مبنا
    /// </summary>
    public int? BaseVolume { get; set; }
    /// <summary>
    /// تعداد سهام معامله شده باید مضربی از این عدد باشند.
    /// </summary>
    public int Lot { get; set; }
    /// <summary>
    /// تاریخ پیام
    /// </summary>
    public string? DateOfEvent { get; set; }

    public string? SymbolNameTse { get; set; }
    public string? SymbolNameModified { get; set; }
    public string? SymbolCodeTse { get; set; }
    public string? BourseCode { get; set; }
    public string? SymbolCodeTseSafeEncoding { get; set; }
    public string? CdsSymbolName { get; set; }
    public TypeOfSymbolTest TypeOfSymbol { get; set; } = TypeOfSymbolTest.Undefined;
    public TypeOfSymbolTest TypeOfSymbolInTseTmc { get; set; } = TypeOfSymbolTest.Undefined;
}