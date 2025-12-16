using Infrastructure.Web;

namespace CacheModule.Contract;

public class CacheModuleConfig : BaseConfig<CacheModuleConfig>
{

    public CacheType? CacheType { get; set; }


    public override (bool isValid, IEnumerable<string> errors) IsValid()
    {
        var errors = new List<string>();
        if (CacheType is null)
            errors.Add($"{nameof(CacheType)} is required.");

        if (errors.Any())
            return (false, errors);

        return (true, Array.Empty<string>());
    }

    public override void IsValidAndThrow()
    {
        var (isValid, errors) = IsValid();
        if (!isValid)
            throw new CacheModuleConfigException(errors);
    }
    internal class CacheModuleConfigException : NotValidDataException<CacheModuleConfigException>
    {
        public CacheModuleConfigException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
