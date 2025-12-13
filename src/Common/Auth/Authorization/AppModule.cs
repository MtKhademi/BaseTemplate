namespace Infrastructure.Auth.Authorization;

public class AppModule(string name)
{
    public string Name => name;

    public static implicit operator string(AppModule module) => module.Name;
    public static implicit operator AppModule(string name) => new(name);

    public static bool operator ==(AppModule left, AppModule right) => left.Name == right.Name;
    public static bool operator !=(AppModule left, AppModule right) => left.Name != right.Name;

    public override bool Equals(object? obj)
    {
        if (obj is AppModule other)
        {
            return this.Name == other.Name;
        }
        return false;
    }

    public override string ToString() => Name;
}
