using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class FixedIncomeSalafAddOrUpdateDtoV2TestBuilder
{
    private readonly FixedIncomeSalafAddOrUpdateDtoV2Test _dto = new FixedIncomeSalafAddOrUpdateDtoV2Test();
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
    {
    }

    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithPublisherId(int? value)
    {
        _dto.PublisherId = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithPublisherName(string value)
    {
        _dto.PublisherName = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithMarketMakerId(int? value)
    {
        _dto.MarketMakerId = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithGurantorId(int? value)
    {
        _dto.GurantorId = value;
        return this;
    }

    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithDescription(string? value)
    {
        _dto.Description = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithSymbolIsin(string? value)
    {
        _dto.SymbolIsin = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithInterestRate(float? value)
    {
        _dto.InterestRate = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithRedeemedRate(float? value)
    {
        _dto.RedeemedRate = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithDuration(double? value)
    {
        _dto.Duration = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithInterestPaymentInterval(int? value)
    {
        _dto.InterestPaymentInterval = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithNominalValue(float? value)
    {
        _dto.NominalValue = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithTradeStartDate(string value)
    {
        _dto.TradeStartDate = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithPublicationDate(string value)
    {
        _dto.PublicationDate = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithSubscriptionStartDate(string value)
    {
        _dto.SubscriptionStartDate = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithSubscriptionEndDate(string value)
    {
        _dto.SubscriptionEndDate = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithMaturityDate(string value)
    {
        _dto.MaturityDate = value;
        return this;
    }


    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithSecondaryTradeStartDate(string value)
    {
        _dto.SecondaryTradeStartDate = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithSecondaryTradeEndDate(string value)
    {
        _dto.SecondaryTradeEndDate = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithEachContractAmount(int? value)
    {
        _dto.EachContractAmount = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithBuyConsequentialPrice(decimal? value)
    {
        _dto.BuyConsequentialPrice = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithSellConsequentialPrice(decimal? value)
    {
        _dto.SellConsequentialPrice = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithEachTonNominalPrice(decimal? value)
    {
        _dto.EachTonNominalPrice = value;
        return this;
    }
    public FixedIncomeSalafAddOrUpdateDtoV2TestBuilder WithEachTonIPOPrice(decimal? value)
    {
        _dto.EachTonIPOPrice = value;
        return this;
    }




    public FixedIncomeSalafAddOrUpdateDtoV2Test Build() => _dto;
}
