using IAMModule.Contract.Models;

namespace IAMModule.Contract.Commands;

public record TokenCreateWithRefreshTokenCommand : ICommand<TokenModel>
{
    public string Token { get; init; }
    public string RefreshToken { get; init; }

    public TokenCreateWithRefreshTokenCommand(string token, string refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;


        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(Token))
            errors.Add("Token is required");

        if (string.IsNullOrWhiteSpace(RefreshToken))
            errors.Add("Refresh token is required");

        if (errors.Any())
            throw new TokenCreateWithRefreshTokenCommandException(errors);
    }

    internal class TokenCreateWithRefreshTokenCommandException : NotValidDataException<TokenCreateWithRefreshTokenCommandException>
    {
        public TokenCreateWithRefreshTokenCommandException(IEnumerable<string> errors) : base(errors)
        {
        }

    }

}

