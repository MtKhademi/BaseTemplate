namespace Common.DI;


public enum DIScopeType
{
    Transiant = 0,
    Scope = 1,
    Singleton = 2
}

public class DIScopeAttribute : Attribute
{
    public DIScopeType ScopeType { get; set; } = DIScopeType.Scope;

    public DIScopeAttribute(DIScopeType scope = DIScopeType.Scope)
    {
        ScopeType = scope;
    }
}
