namespace Test.Integration.CacheModuleTest.Requests;

public record CacheSetRequestTest(
    string? key,
    string? value, 
    int? absoluteExpirationRelativeToNowBaseMinute = default!);