using CacheModule.Contract.Requests;

namespace CacheModule.Contract.Commands;

public record CacheSetCommand<T> : ICommand<bool>
    where T : class
{
    public string Key { get; init; }
    public T Value { get; init; }
    public TimeSpan Expiration { get; init; }

    public CacheSetCommand(string key, T? value, TimeSpan? expiration = null)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(key))
            errors.Add($"{nameof(Key)} is required.");
        if (value is null)
            errors.Add($"{nameof(Value)} is required.");
        if (expiration <= TimeSpan.Zero)
            errors.Add($"{nameof(Expiration)} must be greater than zero.");
        if (errors.Any())
            throw new CacheSetCommandException(errors);

        Key = key;
        Value = value!;
        Expiration = expiration ?? TimeSpan.FromMinutes(20);
    }


    public static CacheSetCommand<string> Create(CacheSetRequest request)
        => new CacheSetCommand<string>(
            key: request.Key,
            value: request.Value,
            expiration: request.AbsoluteExpirationRelativeToNowBaseMinute.HasValue ?
                TimeSpan.FromMinutes(request.AbsoluteExpirationRelativeToNowBaseMinute!.Value) : null
        );
    internal class CacheSetCommandException : NotValidDataException<CacheSetCommandException>
    {
        public CacheSetCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}
