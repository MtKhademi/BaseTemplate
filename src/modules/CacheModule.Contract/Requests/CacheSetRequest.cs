namespace CacheModule.Contract.Requests;

public record CacheSetRequest(
    string? key,
    string? value, 
    int? absoluteExpirationRelativeToNowBaseMinute = default!);