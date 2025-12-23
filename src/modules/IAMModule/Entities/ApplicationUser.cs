namespace IAMModule.Entities;

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


    internal void Update(
        string? userName = default!,
        string? firstName = default!,
        string? lastName = default!,
        string? email = default!,
        string? phoneNumber = default!,
        bool? emailConfirmed = default!,
        bool? isActive = default!)
    {
        if (!string.IsNullOrWhiteSpace(userName))
            UserName = userName;
        if (!string.IsNullOrWhiteSpace(firstName))
            FirstName = firstName;
        if (!string.IsNullOrWhiteSpace(lastName))
            LastName = lastName;
        if (!string.IsNullOrWhiteSpace(email))
            Email = email;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
            PhoneNumber = phoneNumber;
        if (emailConfirmed.HasValue)
            EmailConfirmed = emailConfirmed.Value;
        if (isActive.HasValue)
            IsActive = isActive.Value;
    }
    internal void Update(UserUpdateCommand command)
        => Update(
            userName: command.UserName,
            firstName: command.FirstName,
            lastName: command.LastName,
            email: command.Email,
            phoneNumber: command.PhoneNumber
        );
}
