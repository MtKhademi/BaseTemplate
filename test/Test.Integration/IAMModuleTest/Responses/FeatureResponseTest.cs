namespace Test.Integration.IAMModuleTest.Responses;

public record FeatureResponseTest(
    int? FeatureId = default!,
    string? FeatureName = default!,
    string? FeatureDescription = default!);