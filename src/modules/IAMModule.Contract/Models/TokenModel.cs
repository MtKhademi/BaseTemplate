namespace IAMModule.Contract.Models;

public record TokenModel(
    string userId,
    string Token,
    string RefreshToken,
    DateTime RefreshTokenExpiryTime
)
{
    public TokenResponse TokenResponse(IDateTimeFormatter dateTimeFormatter) => new TokenResponse(
        Token,
        RefreshToken,
        dateTimeFormatter.FormatDateTime(RefreshTokenExpiryTime)
    );
} 