using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class FixedIncomeAddOrUpdateDtoV2TestBuilder
{
    private readonly FixedIncomeAddOrUpdateDtoV2Test _dto = new FixedIncomeAddOrUpdateDtoV2Test();
    public FixedIncomeAddOrUpdateDtoV2TestBuilder()
    {
    }

    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithPublisherId(int? value)
    {
        _dto.PublisherId = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithMarketMakerId(int? value)
    {
        _dto.MarketMakerId = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithGurantorId(int? value)
    {
        _dto.GurantorId = value;
        return this;
    }

    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithDescription(string? value)
    {
        _dto.Description = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithSymbolIsin(string? value)
    {
        _dto.SymbolIsin = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithInterestRate(float? value)
    {
        _dto.InterestRate = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithRedeemedRate(float? value)
    {
        _dto.RedeemedRate = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithDuration(double? value)
    {
        _dto.Duration = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithInterestPaymentInterval(int? value)
    {
        _dto.InterestPaymentInterval = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithNominalValue(float? value)
    {
        _dto.NominalValue = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithTradeStartDate(string value)
    {
        _dto.TradeStartDate = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithPublicationDate(string value)
    {
        _dto.PublicationDate = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithSubscriptionStartDate(string value)
    {
        _dto.SubscriptionStartDate = value;
        return this;
    }
    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithSubscriptionEndDate(string value)
    {
        _dto.SubscriptionEndDate = value;
        return this;
    }

    public FixedIncomeAddOrUpdateDtoV2TestBuilder WithMaturityDate(string value)
    {
        _dto.MaturityDate = value;
        return this;
    }
   

    public FixedIncomeAddOrUpdateDtoV2Test Build() => _dto;
}
