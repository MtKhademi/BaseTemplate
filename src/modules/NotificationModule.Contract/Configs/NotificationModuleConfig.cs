namespace NotificationModule.Contract.Configs;

public class NotificationModuleConfig : BaseConfig<NotificationModuleConfig>
{
    public SMSConfig? SmsConfig { get; set; }

    public override (bool isValid, IEnumerable<string> errors) IsValid()
    {
        var errors = new List<string>();
        if (SmsConfig != null)
        {
            var (isSmsConfigValid, smsConfigErrors) = SmsConfig.IsValid();
            if (!isSmsConfigValid)
            {
                errors.AddRange(smsConfigErrors.Select(e => $"SmsConfig: {e}"));
            }
        }
        else
        {
            errors.Add("SmsConfig is required.");
        }

        if (errors.Any())
            return (false, errors);

        return (true, Array.Empty<string>());
    }

    public override void IsValidAndThrow()
    {
        var (isValid, errors) = IsValid();
        if (!isValid)
            throw new NotificationModuleConfigException(errors);
    }
    internal class NotificationModuleConfigException : NotValidDataException<NotificationModuleConfigException>
    {
        public NotificationModuleConfigException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
