namespace IAMModule.Contract;

public class IAMModuleConfig : BaseConfig<IAMModuleConfig>
{
    public string Secret { get; set; }
    public int TokenExpiryInMinutes { get; set; }
    public string ConnectionString { get; set; }


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
            throw new IAMConfigException(errors);
    }
    internal class IAMConfigException : NotValidDataException<IAMConfigException>
    {
        public IAMConfigException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
