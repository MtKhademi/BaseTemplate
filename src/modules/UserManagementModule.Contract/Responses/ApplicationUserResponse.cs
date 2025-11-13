namespace UserManagementModule.Contract.Responses;

public record ApplicationUserResponse(
    string? UserId,
    string? FirstName,
    string? LastName,
    string? Email,
    string? UserName,
    string? PhoneNumber,
    bool? IsActive,
    bool? EmailConfirmed
);