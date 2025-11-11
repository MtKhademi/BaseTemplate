namespace IAMModule.Data.Context;

public class IAMModuleDbDataSeeder(
    IAMModuleDbContext _context,
    RoleManager<ApplicationRole> _roleManager,
    UserManager<ApplicationUser> _userManager) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        // Check for pending and apply if any
        await CheckAndApplyPendingMigrationasync();
        // seed roles
        await SeedRolesAsync();
        // // seed admin
        await SeedAdminUsersAsync();

    }


    private async Task CheckAndApplyPendingMigrationasync()
    {
        if ((await _context.Database.GetPendingMigrationsAsync()).Any())
        {
            var xx = await _context.Database.GetPendingMigrationsAsync();

            await _context.Database.MigrateAsync();
        }
    }

    private async Task SeedRolesAsync()
    {
        foreach (var roleName in AppRoles.DefaultRoles)
        {
            if (await _roleManager.Roles.FirstOrDefaultAsync(rol => rol.Name == roleName) is not ApplicationRole role)
            {
                role = new ApplicationRole
                {
                    Name = roleName,
                    Description = $"{roleName} role."
                };
                await _roleManager.CreateAsync(role);
            }

            //Add permission
            if (roleName == AppRoles.Admin)
            {
                // admin 
                await AssignPermissionsToRoleAsync(role, AppPermissions.AdminPermissions);
            }
            else if (roleName == AppRoles.Basic)
            {
                // basic
                await AssignPermissionsToRoleAsync(role, AppPermissions.BasicPermissions);
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(ApplicationRole role, IReadOnlyList<AppPermission> permissions)
    {
        var currentClaim = await _roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!currentClaim.Any(claim => claim.Type == AppClaim.Permission && claim.Value == permission.Name))
            {
                await _context.RoleClaims.AddAsync(new ApplicationRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.Name,
                    Description = permission.Description,
                    Group = permission.Group
                });
                await _context.SaveChangesAsync();
            }
        }
    }

    private async Task SeedAdminUsersAsync()
    {

        foreach (var adminCredential in AppCredentials.AdminUsers)
        {
            await SeedAdminUserAsync(adminCredential);
        }

        var users = await _userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            if (!await _userManager.IsInRoleAsync(user, AppRoles.Basic) &&
                !await _userManager.IsInRoleAsync(user, AppRoles.Admin))
            {
                await _userManager.AddToRolesAsync(user, AppRoles.DefaultRoles);
            }
        }
    }

    private async Task SeedAdminUserAsync(AdminUserCredential adminCredential)
    {
        var adminUser = new ApplicationUser
        {
            FirstName = adminCredential.FirstName,
            LastName = adminCredential.LastName,
            Email = adminCredential.Email,
            UserName = adminCredential.UserName,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            PhoneNumber = adminCredential.PhoneNumber,
            NormalizedEmail = adminCredential.Email.ToUpperInvariant(),
            NormalizedUserName = adminCredential.UserName.ToUpperInvariant(),
            IsActive = true,
            RefreshToken = "",
        };

        if (!await _userManager.Users.AnyAsync(u => u.Email == adminUser.Email))
        {
            var password = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, adminCredential.Password);
            await _userManager.CreateAsync(adminUser);
        }
    }
}