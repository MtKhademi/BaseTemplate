namespace MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.RequestesTest;

public class ERApiFundNavCreateOrUpdateRequestTest
{
    public string? SeoRegisterNumber { get; set; } = default!;
    public FundProviderTest? FundProvider { get; set; } = default!;
    public FundXMLTypeTest? FundXmlType { get; set; } = default!;
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
}
