namespace CacheModule.Contract.Requests;

public record CacheSetRequest(
    string? Key,
    string? Value, 
    int? AbsoluteExpirationRelativeToNowBaseMinute = default!);