namespace NotificationModule.Contract.Configs;

public class SMSConfig : BaseConfig<SMSConfig>
{
    public string? Provider { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }

    public override (bool isValid, IEnumerable<string> errors) IsValid()
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(Provider))
            errors.Add($"{nameof(Provider)} is required.");
        if (string.IsNullOrWhiteSpace(UserName))
            errors.Add($"{nameof(UserName)} is required.");
        if (string.IsNullOrWhiteSpace(Password))
            errors.Add($"{nameof(Password)} is required.");

        if (errors.Any())
            return (false, errors);
        return (true, Array.Empty<string>());
    }

    public override void IsValidAndThrow()
    {
        var (isValid, errors) = IsValid();
        if (!isValid)
            throw new SMSConfigException(errors);
    }

    internal class SMSConfigException : NotValidDataException<SMSConfigException>
    {
        public SMSConfigException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
