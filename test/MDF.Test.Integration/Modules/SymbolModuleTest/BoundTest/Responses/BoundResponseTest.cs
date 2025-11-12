namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

public class BoundResponseTest
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

    public int? FixedIncomeId { get; set; }

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

    public int? CountOfPublished { get; set; }


    /// <summary>
    /// تاریخ های پرداخت سود
    /// </summary>
    public List<string>? InterestPaymentDates { get; set; } = new List<string>();
}


