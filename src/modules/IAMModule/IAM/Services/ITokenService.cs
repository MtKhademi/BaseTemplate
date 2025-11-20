namespace IAMModule.IAM.Services;

public interface ITokenService
{
    string GenerateRefreshToken();
    Task<string> GenerateTokenAsync(ApplicationUser user);
    Task<bool> SignOutAsync(string userId);
    Task<bool> ValidTokenAsync(string token, string userId);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}


internal class IdentityTokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<IdentityTokenService> _logger;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IAMModuleConfig _UserManagementConfig;
    private readonly IUnitOfWork _unitOfWork;
    public IdentityTokenService(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IAMModuleConfig> userManagementConfig,
        ILogger<IdentityTokenService> logger,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _UserManagementConfig = userManagementConfig.Value;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var accessToken = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
        await _userManager.SetAuthenticationTokenAsync(user, "BankingGateWay", "AccessToken", accessToken);
        return accessToken;
    }


    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rnd = RandomNumberGenerator.Create())
        {
            rnd.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(claims: claims,
            expires: DateTime.UtcNow.AddHours(_UserManagementConfig.TokenExpiryInMinutes),
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        return encryptedToken;
    }
    private SigningCredentials GetSigningCredentials()
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_UserManagementConfig.SecretKey));
        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();

        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role));
            var currentRole = await _roleManager.FindByNameAsync(role);
            var allPermissionsForCurrentRole = await _roleManager.GetClaimsAsync(currentRole);
            permissionClaims.AddRange(allPermissionsForCurrentRole);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
        };

        claims.AddRange(roleClaims);
        claims.AddRange(permissionClaims);

        return claims;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_UserManagementConfig.SecretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    public async Task<bool> ValidTokenAsync(string token, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User not found: {UserId}", userId);
            return false;
        }

        var tokens = await _userManager.GetAuthenticationTokenAsync(user, "BankingGateWay", "AccessToken");
        if (tokens == null || !tokens.Any())
        {
            _logger.LogWarning("No tokens found for user: {UserId}", userId);
            return false;
        }
        return tokens.Contains(token);
    }

    public async Task<bool> SignOutAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            _logger.LogWarning("User not found: {UserId}", userId);
            return true;
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("User is not active: {UserId}", userId);
            return true;
        }

        // Invalidate refresh token by setting it to null and expiry to past date
        await _unitOfWork.CreateTransactionAsync();

        user.RefreshToken = string.Empty;
        user.RefreshTokenExpiryDate = DateTime.MinValue;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new NotValidDataException("Not valid data");
        }

        await _userManager.RemoveAuthenticationTokenAsync(user, "BankingGateWay", "AccessToken");

        await _unitOfWork.CommitAsync();

        _logger.LogInformation("User {UserId} signed out successfully", userId);
        return true;
    }


}
