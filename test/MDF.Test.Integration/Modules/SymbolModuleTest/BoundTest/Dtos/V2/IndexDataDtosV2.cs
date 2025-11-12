using MDF.Common.Infrastructure;

namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.IndexDataDtosV2Test;
public class IndexDataFilterGetDtoV2Test
{
    public string? Isin { get; set; }
    public string? DateOfEvent { get; set; }
    public string? StartDateOfEvent { get; set; }
    public string? EndDateOfEvent { get; set; }
}

public class IndexDataPricesPerDayFilterGetDtoV2Test
{
    public string? Isin { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
}


public class IndexDataTableFilterDtoV2Test : IndexDataFilterGetDtoV2Test, IBasePagination
{
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}

public class IndexDataGetDtoV2Test
{
    /// <summary>
    /// نام شاخص
    /// </summary>
    public string SymbolName { get; set; }
    /// <summary>
    /// کد آیزین
    /// </summary>
    public string SymbolIsin { get; set; }
    /// <summary>
    /// تاریخ 
    /// </summary>
    public string DateOfEvent { get; set; }
    /// <summary>
    /// درصد تغییرات شاخص در طول روز نسبت به روز قبل
    /// </summary>
    public float PercentVariation { get; set; }
    /// <summary>
    /// آخرین مقدار شاخص روز
    /// </summary>
    public float LastIndexValue { get; set; }
    /// <summary>
    /// تغییرات شاخص نسبت به روز قبل
    /// </summary>
    public float IndexChanges { get; set; }
}
