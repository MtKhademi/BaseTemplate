namespace MutualFundModule.Contract.Fund.Responses;

public record FundFeeResponseTest(
    string EventDate,
    double RedemptionFixedFee,
    double RewardPercentage,
    double SubscriptionFixedFee,
    double SubscriptionVariableFee,
    double SubscriptionVariableMaximumFee
    );
