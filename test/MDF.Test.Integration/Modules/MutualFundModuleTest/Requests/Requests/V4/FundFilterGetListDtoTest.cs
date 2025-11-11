namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Dtos.FundDtos.V4;

public class FundFilterGetListDtoTest
{
    public string[]? FundIsinsOrSymbolIsins { get; set; } = default!;
    public string? Title { get; set; } = default!;
    public FundProviderTest? FundProvider { get; set; } = default!;
    public FundProviderTest[]? FundProviders { get; set; } = default!;
    public FundTypeTest? FundType { get; set; } = default!;
    public string? FundIsinOrSymbolIsin { get; set; } = default!;
    public string? FundIsin { get; set; } = default!;
    public string? SymbolIsin { get; set; } = default!;
    public string? SeoregisterNumber { get; set; } = default!;
    public string? Website { get; set; } = default!;
    public string? FromLastChangeFundDate { get; set; } = default!;
    public string? ToLastChangeFundData { get; set; } = default!;
}
