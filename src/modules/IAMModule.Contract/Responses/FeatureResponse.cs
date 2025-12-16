namespace IAMModule.Contract.Responses;

public record FeatureResponse(
    int? FeatureId = default!,
    string? FeatureName = default!,
    string? FeatureDescription = default!);