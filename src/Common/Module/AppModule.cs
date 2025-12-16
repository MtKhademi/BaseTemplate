namespace Infrastructure.Module;

public record AppModule(string name, string? description = default!)
{
    public string Name => name;
    public string? Description => description;

    public static implicit operator string(AppModule module) => module.Name;
    public static implicit operator AppModule(string name) => new AppModule(name);
}
