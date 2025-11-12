using MDF.Common.Infrastructure;
using MDF.Common.Infrastructure.TableInfra.TableInfra;
using MDF.Modules.SymbolModule.Depricate.Abstractions;

namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos;

#region FixedIncomeAddOrUpdateDtoTestV2

public class FixedIncomeAddOrUpdateDtoV2Test
{
    public string? SymbolIsin { get; set; }


    /// <summary>
    /// نوع فرمول
    /// </summary>
    public FormulaType FormulaType { get; set; }


    public int? PublisherId { get; set; }
    public string? PublisherName { get; set; }


    public int? MarketMakerId { get; set; }
    public string? MarketMakerName { get; set; }


    /// <summary>
    /// ضامن
    /// </summary>
    public string? GurantorName { get; set; }
    public int? GurantorId { get; set; }


    /// <summary>
    /// نرخ سود سالانه
    /// </summary>
    public float? InterestRate { get; set; }
    /// <summary>
    /// نرخ بازخرید
    /// </summary>
    public float? RedeemedRate { get; set; }


    /// <summary>
    /// مدت زمان خرید و فروش اوراق 
    /// </summary>
    public double? Duration { get; set; }
    /// <summary>
    /// مبلغ اسمی
    /// </summary>
    public float? NominalValue { get; set; }
    /// <summary>
    /// دوره پرداخت سود
    /// </summary>
    public int? InterestPaymentInterval { get; set; }
    /// <summary>
    /// تاریخ شروع معامله
    /// </summary>
    public string? TradeStartDate { get; set; }
    /// <summary>
    /// تاریخ  سررسید
    /// </summary>
    public string? MaturityDate { get; set; }
    /// <summary>
    ///  تاریخ شروع پذیره نویسی
    /// </summary>
    public string? SubscriptionStartDate { get; set; }
    /// <summary>
    /// تاریخ پایان پذیره نویسی
    /// </summary>
    public string? SubscriptionEndDate { get; set; }
    public string? PublicationDate { get; set; }

    /// <summary>
    /// توضیحات
    /// </summary>
    public string? Description { get; set; }

}

public class FixedIncomeSalafAddOrUpdateDtoV2Test : FixedIncomeAddOrUpdateDtoV2Test
{
    public int? EachContractAmount { get; set; }
    public decimal? BuyConsequentialPrice { get; set; }
    public decimal? SellConsequentialPrice { get; set; }
    public decimal? EachTonNominalPrice { get; set; }
    public decimal? EachTonIPOPrice { get; set; }
    public string? SecondaryTradeStartDate { get; set; }
    public string? SecondaryTradeEndDate { get; set; }
}


#endregion

#region FixIncomeFilterDtoTestV2

public class FixedIncomeFilterDtoTestV2
{
    public string? MaturityDate { get; set; }
    public string? SubscriptionEndDate { get; set; }
    public string? SubscriptionStartDate { get; set; }
    public List<ETypeOfSymbol>? TypeOfSymbols { get; set; }
    public string? Isins { get; set; }
}
public class FixedIncomeTableFilterDtoV2Test : FixedIncomeFilterDtoTestV2, IBasePagination
{
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}

#endregion

#region FixIncomeGetDtoTestV2

public class FixedIncomeGetDtoTestV2
{
    /// <summary>
    /// آیزین
    /// </summary>
    public string? SymbolIsin { get; set; }
    /// <summary>
    /// نام نماد
    /// </summary>
    public string? SymbolName { get; set; }

    public TypeOfSymbolTest TypeOfSymbol { get; set; }
    public string TypeOfSymbolPersianName { get; set; }

    /// <summary>
    /// بازارگردان
    /// </summary>
    public string? MarketMakerName { get; set; }
    public int? MarketMakerId { get; set; }

    /// <summary>
    /// ضامن
    /// </summary>
    public string? GurantorName { get; set; }
    public int? GurantorId { get; set; }


    /// <summary>
    /// ناشر
    /// </summary>
    public string? PublisherName { get; set; }
    public int? PublisherId { get; set; }




    /// <summary>
    /// نرخ سود سالانه
    /// </summary>
    public float? InterestRate { get; set; }
    /// <summary>
    /// نرخ بازخرید
    /// </summary>
    public float? RedeemedRate { get; set; }



    /// <summary>
    /// مدت زمان خرید و فروش اوراق 
    /// </summary>
    public double? Duration { get; set; }
    /// <summary>
    /// مبلغ اسمی
    /// </summary>
    public float? NominalValue { get; set; }

    /// <summary>
    /// دوره پرداخت سود
    /// </summary>
    public int? InterestPaymentInterval { get; set; }


    public string? TradeStartDate { get; set; }
    /// <summary>
    /// تاریخ  سررسید
    /// </summary>
    public string? MaturityDate { get; set; }
    /// <summary>
    ///  تاریخ شروع پذیره نویسی
    /// </summary>
    public string? SubscriptionStartDate { get; set; }
    /// <summary>
    /// تاریخ پایان پذیره نویسی
    /// </summary>
    public string? SubscriptionEndDate { get; set; }
    public string? PublicationDate { get; set; }

    /// <summary>
    /// توضیحات
    /// </summary>
    public string? Description { get; set; }


    /// <summary>
    /// تاریخ های پرداخت سود
    /// </summary>
    public List<string>? InterestPaymentDates { get; set; } = new List<string>();
}
public class FixedIncomeGetUIDtoV2Test : FixedIncomeGetDtoTestV2
{
    public IList<string> Actions { get; set; } = new List<string>();

}

#endregion

#region FixIncomeProfitDailyGetDtoTestV2

public class FixIncomeProfitDailyGetDtoTestV2
{
    public double? ProfitDailyPrice { get; set; }
    public string? ProfitDailyDate { get; set; }
    public string? ModifiedDate { get; set; }
    public double? SubscriptionProfit { get; set; }
    public decimal? NominalPrice { get; set; }
    public bool? HasAdjusted { get; set; }

}
public class FixedIncomeProfitDailyGetUIDtoTestV2 : FixIncomeProfitDailyGetDtoTestV2, IUIGridRow
{
    public IList<string> Actions { get; set; } = new List<string>();
}

#endregion

