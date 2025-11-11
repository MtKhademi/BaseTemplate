namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;

internal class BoundGetPaginatedListRequestTest
{
    public string? MaturityDate { get; set; }
    public string? SubscriptionEndDate { get; set; }
    public string? SubscriptionStartDate { get; set; }
    public List<TypeOfSymbolTest>? TypeOfSymbols { get; set; } = null;
    public string? Isins { get; set; }
    public bool? IsDisable { get; set; }
    /// <summary>
    /// آیا تاریخ های پرداخت سود دریافت شود یا نه
    /// </summary>
    public bool? IsLoadInterestPaymentInterval { get; set; } = false;

    public bool? IsFixedIncomeDataExist { get; set; } = null;

    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}
