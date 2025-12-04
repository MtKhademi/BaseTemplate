namespace Infrastructure.DI;

public class DIScopeAttribute : Attribute
{
    public DIScopeType ScopeType { get; set; } = DIScopeType.Scope;

    public DIScopeAttribute(DIScopeType scope = DIScopeType.Scope)
    {
        ScopeType = scope;
    }
}
