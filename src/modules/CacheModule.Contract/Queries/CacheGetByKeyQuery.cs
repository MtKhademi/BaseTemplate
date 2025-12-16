using static CacheModule.Contract.Queries.CacheGetByKeyQuery;

namespace CacheModule.Contract.Queries;

public record CacheGetByKeyQueryBase
{
    public string Key { get; init; }
    public CacheGetByKeyQueryBase(string key)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(key))
            errors.Add($"{nameof(key)} is required.");
        if (errors.Any())
            throw new CacheQueryBaseException(errors);

        this.Key = key;
    }


    internal class CacheQueryBaseException : NotValidDataException<CacheQueryBaseException>
    {
        public CacheQueryBaseException(IEnumerable<string> errors) : base(errors)
        {
        }
    }

}

public record CacheGetByKeyQuery(string key) : CacheGetByKeyQueryBase(key), IQuery<string>
{
}

public record CacheGetByKeyQuery<T>(string Key) : CacheGetByKeyQueryBase(Key), IQuery<T>
    where T : class
{ }
