using MDF.Common.Infrastructure;
using MDF.Common.Infrastructure.TableInfra.TableInfra;
using MDF.Modules.SymbolModule.Depricate.Abstractions;

namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.AdjustedPriceDtos
{
    #region AdjustedPriceCreateDtoV2Test

    public class AdjustedPriceCreateDtoV2Test
    {
        public string? StartDateTime { get; set; } = null;
        public string? Isin { get; set; } = null;
        public int? FirmId { get; set; } = null;
        public string? EndDateTime { get; set; } = null;

    }

    #endregion

    #region AdjustedPriceFilterDto

    public class AdjustedPriceFilterDtoV2Test
    {
        public string? StartDateTime { get; set; }
        public string? EndDateTime { get; set; }

        public string? StartPublishDateTime { get; set; }
        public string? EndPublishDateTime { get; set; }
        public string? Isins { get; set; } = null;
        public bool? IsDeleted { get; set; } = null;
        public string? CodalCode { get; set; } = null;
    }
    public class AdjustedPriceTableFilterDtoV2Test : AdjustedPriceFilterDtoV2Test, IBasePagination
    {
        public int? CurrentPage { get; set; }
        public int? SizeOfPage { get; set; }
    }

    #endregion

    #region AdjustedPriceUpdateDtoV2Test

    public class AdjustedPriceUpdateDtoV2Test
    {
        public int? AdjustedPriceId { get; set; }
        public int? CodalCode { get; set; }
        public decimal? AdjustedPrice { get; set; }
        public decimal? AdjustedLastPrice { get; set; }
        public bool? IsAdjusted { get; set; }
    }

    #endregion

    #region AdjustedPriceDeleteDtoV2Test

    public class AdjustedPriceDeleteDtoV2Test
    {
        public int? AdjustedPriceId { get; set; }
    }

    #endregion

    #region AdjustedPriceGetDtoV2Test

    public class AdjustedPriceGetDtoV2Test
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string? AdjustedPriceDate { get; set; }
        public string? SymbolName { get; set; }
        public string? SymbolIsin { get; set; }
        public decimal AdjustedLastPrice { get; set; }
        public decimal AdjustedPrice { get; set; }
        public decimal ClosingPrice { get; set; }
        public decimal LastTradedPrice { get; set; }
        public int? CapitalChangeCodalCode { get; set; }
        public int? DividendPerShareCodalCode { get; set; }
        public bool IsAdjusted { get; set; }
    }
    public class AdjustedPriceGetUIDtoV2Test : AdjustedPriceGetDtoV2Test, IUIGridRow
    {
        public IList<string> Actions { get; set; } = new List<string>();
    }

    #endregion

    #region AdjustedPriceCheckingV2

    public class AdjustedPriceCheckingDtoV2Test
    {
        public string? DateTime { get; set; }
    }

    #endregion

    #region AdjustedPriceResultCheckingV2

    public record AdjustedPriceResultCheckingDtoV2Test
    {
        public int Step { get; set; }
        public string HeaderSection { get; set; }

        public ETypeOfAdjustedPriceCheckingStateProcess State
        {
            get
            {
                if (Messages is null || Messages.Count == 0)
                    return ETypeOfAdjustedPriceCheckingStateProcess.Pass;
                return ETypeOfAdjustedPriceCheckingStateProcess.Faild;
            }
        }
        public List<string> Messages { get; set; } = null;
    }

    #endregion
}
