namespace IAMModule.Contract.Models;

public record ApplicationUserModel(
    string? UserId,
    string? FirstName,
    string? LastName,
    string? Email,
    string? UserName,
    string? PhoneNumber,
    bool? EmailConfirmed,
    string? RefreshToken,
    DateTime? RefreshTokenExpiryDate,
    bool? IsActive
)
{
    public ApplicationUserResponse ToResponse()
        => new ApplicationUserResponse(
            UserId: UserId,
            FirstName: FirstName,
            LastName: LastName,
            Email: Email,
            UserName: UserName,
            PhoneNumber: PhoneNumber,
            IsActive: IsActive,
            EmailConfirmed: EmailConfirmed
        );
}