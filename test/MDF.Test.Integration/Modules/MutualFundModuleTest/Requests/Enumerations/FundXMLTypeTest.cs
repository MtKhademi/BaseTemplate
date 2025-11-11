using System.ComponentModel;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Dtos.Enumerations;

public enum FundXMLTypeTest : int
{

    [Description("سهام")]
    JointStock = 1,

    [Description("مختلط")]
    Mixed = 2,

    [Description("درآمد ثابت")]
    FixedIncomeFixed = 3,

    [Description("ETFسهام")]
    StockEtf = 4,

    [Description("جسورانه")]
    VentureCapital = 5,

    [Description(" ETFدرآمد ثابت")]
    FixedIncomeEtf = 6,

    [Description("بازارگردان")]
    MarketMakerFund = 7,

    [Description("ProjectFund")]
    ProjectFund = 8,

    [Description("Unknown")]
    Unknown = 9,

    [Description("زمین و ساختمان")]
    RealEstateFund = 10,

    [Description("اهرمی")]
    Leverage = 11,
}
