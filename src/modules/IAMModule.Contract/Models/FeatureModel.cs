namespace IAMModule.Contract.Models;

public record FeatureModel(
    int FeatureId,
    string? FeatureName,
    string? FeatureDescription)
{
    public FeatureResponse ToFeatureResponse()
        => new FeatureResponse(
            FeatureId: FeatureId,
            FeatureName: FeatureName,
            FeatureDescription: FeatureDescription);
}