namespace Infrastructure.Module;

public class AppFeature(string name, string? description = default!)
{
    public string Name => name;
    public string? Description => description;

    public static implicit operator string(AppFeature feature) => $"{feature.Name}.{feature.Description}";
    public static implicit operator AppFeature(string feature)
    {
        var splitData = feature.Split('.');
        if (splitData.Length == 2)
        {
            return new AppFeature(splitData[0], splitData[1]);
        }
        else
        {
            return new AppFeature(splitData[0]);
        }
    }

}
