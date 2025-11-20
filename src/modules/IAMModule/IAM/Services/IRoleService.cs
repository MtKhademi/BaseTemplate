//namespace IAMModule.IAM.Services;

//public interface IRoleService
//{
//    Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);
//    Task<IEnumerable<RoleResponse>> GetRolesAsync();
    
//    // Task<IResponseWrapper> GetPermissionsAsync(string roleId);

//}

//class RoleService(
//    UserManager<ApplicationUser> userManager,
//    RoleManager<ApplicationRole> roleManager,
//    IAMModuleDbContext context) : IRoleService
//{
//    public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request)
//    {
//        var roleExist = await roleManager.RoleExistsAsync(request.Name);
//        if (roleExist)
//            throw new Exception("Role already exists"); 

//        var role = new ApplicationRole
//        {
//            Name = request.Name,
//            Description = request.Description
//        };
//        var resultCreate = await roleManager.CreateAsync(role);
//        if (!resultCreate.Succeeded)
//            throw new RoleCreateException(resultCreate);

//        return role.ToRoleResponse();
//    }

//    public async Task<IEnumerable<RoleResponse>> GetRolesAsync()
//    {
//        var allRoles = await roleManager.Roles.ToListAsync();
//        return allRoles.Select(role => role.ToRoleResponse());
//    }

//    // public async Task<IResponseWrapper> GetPermissionsAsync(string roleId)
//    // {
//    //     var roleInDb = await roleManager.FindByIdAsync(roleId);
//    //     if (roleId is null)
//    //         return await ResponseWrapper<string>.FailAsync("Role does not exist");
//    //
//    //     var allPermissions = AppPermissions.AdminPermissions;
//    //     var roleClaimResponse = new RoleClaimResponse
//    //     {
//    //         Role = new RoleResponse
//    //         {
//    //             Id = roleInDb.Id,
//    //             Description = roleInDb.Description,
//    //             Name = roleInDb.Name
//    //         },
//    //         RoleClaims = new()
//    //     };
//    //
//    //     var currentRoleClaims = await GetAllClaimsForRoleAsync(roleId);
//    //     var allPermissionsName = allPermissions.Select(p => p.Name).ToList();
//    //     var currentRoleClaimsValues = currentRoleClaims.Select(c => c.ClaimType).ToList();
//    //
//    //     var currentlyAssignedRoloClaimsNames = allPermissionsName
//    //         .Intersect(currentRoleClaimsValues)
//    //         .ToList();
//    //     foreach (var permission in allPermissions)
//    //     {
//    //         if (currentlyAssignedRoloClaimsNames.Any(c => c == permission.Name))
//    //         {
//    //             roleClaimResponse.RoleClaims.Add(new RoleClaimViewModel()
//    //             {
//    //                 RoleId = roleId,
//    //                 ClaimType = AppClaim.Permission,
//    //                 ClaimValue = permission.Name,
//    //                 Description = permission.Description,
//    //                 Group = permission.Group,
//    //                 IsAssigned = true
//    //             });
//    //         }
//    //         else
//    //         {
//    //             roleClaimResponse.RoleClaims.Add(new RoleClaimViewModel()
//    //             {
//    //                 RoleId = roleId,
//    //                 ClaimType = AppClaim.Permission,
//    //                 ClaimValue = permission.Name,
//    //                 Description = permission.Description,
//    //                 Group = permission.Group,
//    //                 IsAssigned = false
//    //             });
//    //         }
//    //     }
//    //     
//    //     return await ResponseWrapper<RoleClaimResponse>.SuccessAsync(roleClaimResponse, "Role Claims");
//    // }
//    //
//    // private async Task<List<RoleClaimViewModel>> GetAllClaimsForRoleAsync(string roleId)
//    // {
//    //     var roleClaim = await context.RoleClaims
//    //         .Where(rol => rol.RoleId == roleId)
//    //         .ToListAsync();
//    //
//    //     if (roleClaim.Count > 0)
//    //     {
//    //         var mappedRoleClaim = mapper.Map<List<RoleClaimViewModel>>(roleClaim);
//    //         return mappedRoleClaim;
//    //     }
//    //
//    //     return [];
//    // }
//}