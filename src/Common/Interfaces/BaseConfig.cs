namespace Common.Interfaces;

public abstract class BaseConfig<T> 
    where T : class
{
    public bool IsActive { get; set; } = false;

    public abstract (bool isValid, IEnumerable<string> errors) IsValid();
    public abstract void IsValidAndThrow();
}
