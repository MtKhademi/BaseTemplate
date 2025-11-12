using Common.Interfaces;

namespace IAMModule.Contract;

public class IAMModuleConfig : BaseConfig<IAMModuleConfig>
{
    public string Secret { get; set; }
    public int TokenExpiryInMinutes { get; set; }


    public override (bool isValid, IEnumerable<string> errors) IsValid()
    {
        //if (string.IsNullOrWhiteSpace(Secret))
        //    throw new IAMConfigException($"{nameof(Secret)} is required");

        //if (TokenExpiryInMinutes <= 0)
        //    throw new IAMConfigException($"{nameof(TokenExpiryInMinutes)} must be greater than 0");
        return (true, Array.Empty<string>());
    }

    public override void IsValidAndThrow()
    {
    }
}

public class IAMConfigException : NotValidDataException
{
    public IAMConfigException(string message) : base(message, nameof(IAMConfigException))
    {
    }
}