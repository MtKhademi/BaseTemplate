namespace IAMModule.Contract.Responses;

public record TokenResponse(
    string Token,
    string RefreshToken,
    string RefreshTokenExpiryTime
);