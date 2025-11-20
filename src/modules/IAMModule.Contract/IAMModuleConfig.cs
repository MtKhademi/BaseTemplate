namespace IAMModule.Contract;

public class IAMModuleConfig : BaseConfig<IAMModuleConfig>
{
    public string SecretKey { get; set; }
    public int TokenExpiryInMinutes { get; set; }
    public string ConnectionString { get; set; }


    public override (bool isValid, IEnumerable<string> errors) IsValid()
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(ConnectionString))
            errors.Add($"{nameof(ConnectionString)} is required.");

        if(string.IsNullOrWhiteSpace(SecretKey))
            errors.Add($"{nameof(SecretKey)} is required.");

        if(TokenExpiryInMinutes <= 0)
            errors.Add($"{nameof(TokenExpiryInMinutes)} must be greater than zero.");

        if (errors.Any())
            return (false, errors);

        return (true, Array.Empty<string>());
    }

    public override void IsValidAndThrow()
    {
        var (isValid, errors) = IsValid();
        if (!isValid)
            throw new IAMModuleConfigException(errors);
    }
    internal class IAMModuleConfigException : NotValidDataException<IAMModuleConfigException>
    {
        public IAMModuleConfigException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
