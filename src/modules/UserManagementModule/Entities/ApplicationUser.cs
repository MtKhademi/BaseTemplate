using UserManagementModule.Contract.Models;

namespace UserManagementModule.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDate { get; set; }
    public bool IsActive { get; set; }

    public ApplicationUserModel ToModel()
        => new ApplicationUserModel(
            UserId: Id,
            FirstName: FirstName,
            LastName: LastName,
            Email: Email,
            UserName: UserName,
            PhoneNumber: PhoneNumber,
            EmailConfirmed: EmailConfirmed,
            RefreshToken: RefreshToken,
            RefreshTokenExpiryDate: RefreshTokenExpiryDate,
            IsActive: IsActive
        );
}
