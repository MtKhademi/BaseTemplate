namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.Requests;

public class CapitalChangeCreateRequestTest
{
    public int? CodalCode { get; set; }

    /// <summary>
    /// ارزش اسمی قبل
    /// </summary>
    public long? LastShareValue { get; set; }
    /// <summary>
    /// ارزش اسمی بعد
    /// </summary>
    public long? NewShareValue { get; set; }
    /// <summary>
    /// سرمایه قبلی
    /// </summary>
    public double? LastShareCount { get; set; }
    /// <summary>
    /// سرمایه جدید
    /// </summary>
    public double? NewShareCount { get; set; }


    /// <summary>
    /// مطالبات آورده نقدی
    /// </summary>
    public double? CashIncoming { get; set; }
    /// <summary>
    /// سود انباشته
    /// </summary>
    public double? RetaindedEarning { get; set; }
    /// <summary>
    /// اندوخته
    /// </summary>
    public double? Reserves { get; set; }
    /// <summary>
    /// صرف سهام
    /// </summary>
    public double? SarfSaham { get; set; }
    /// <summary>
    /// مازاد تجدید ارزیابی
    /// </summary>
    public double? RevaluationSurplus { get; set; }


    public bool IsUseSalb { get; set; } = false;

}
