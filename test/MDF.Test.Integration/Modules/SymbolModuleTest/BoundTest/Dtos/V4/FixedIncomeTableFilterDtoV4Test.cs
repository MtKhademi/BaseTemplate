using MDF.Common.Infrastructure;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.V4;

public class FixedIncomeTableFilterDtoV4Test : FixedIncomeFilterDtoV4Test, IBasePagination
{
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}

