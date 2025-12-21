using Infrastructure.Web;

namespace ECommerceModule.Contract;

public class ECommerceModuleConfig : BaseConfig<ECommerceModuleConfig>
{
    public string ConnectionString { get; set; } = default!;

    public override (bool isValid, IEnumerable<string> errors) IsValid()
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(ConnectionString))
            errors.Add($"{nameof(ConnectionString)} is required.");

        if (errors.Any())
            return (false, errors);

        return (true, Array.Empty<string>());
    }

    public override void IsValidAndThrow()
    {
        var (isValid, errors) = IsValid();
        if (!isValid)
            throw new ECommerceModuleConfigException(errors);
    }

    internal class ECommerceModuleConfigException : NotValidDataException<ECommerceModuleConfigException>
    {
        public ECommerceModuleConfigException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
