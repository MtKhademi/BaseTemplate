namespace IAMModule.Contract.Requests;

public class TokenCreateWithRefreshTokenRequest
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }

    public void Validate()
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(Token))
            errors.Add("Token is required");

        if (string.IsNullOrWhiteSpace(RefreshToken))
            errors.Add("Refresh token is required");

        if (errors.Any())
            throw new TokenCreateWithRefreshTokenRequestException(errors);
    }

    public TokenCreateWithRefreshTokenCommand ToCommand()
    {
        Validate();
        return new TokenCreateWithRefreshTokenCommand(Token, RefreshToken);
    }


    internal class TokenCreateWithRefreshTokenRequestException : NotValidDataException<TokenCreateWithRefreshTokenRequestException>
    {
        public TokenCreateWithRefreshTokenRequestException(IEnumerable<string> errors) : base(errors)
        {
        }

    }

}

