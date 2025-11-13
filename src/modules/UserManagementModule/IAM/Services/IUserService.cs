// namespace UserManagementModule.Services;

// public interface IUserService
// {
//     // Task<UserResponse> RegisterUserAsync(UserRegistrationRequest request);

//     // Task<UserResponse> GetUserByIdAsync(string userId);

//     // Task<IEnumerable<UserResponse>> GetAllUsersAsync();

//     // Task<UserResponse> UpdateUserAsync(UpdateUserRequest request, string userId);


//     Task<UserResponse> ChangeUserPasswordAsync(ChangePasswordRequest request, string userId);

//     // Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest request);
//     //
//     // Task<IResponseWrapper> GetRolesAsync(string userId);
//     //
//     // Task<IResponseWrapper> UpdateUserRolesAsync(UpdateUserRoleRequest request);
// }

// public class UserService(
//     UserManager<ApplicationUser> _userManager,
//     RoleManager<ApplicationRole> _roleManager,
//     ICurrentUserService currentUserService) : IUserService
// {
//     // public async Task<UserResponse> RegisterUserAsync(UserRegistrationRequest request)
//     // {

//     // }

//     // public async Task<UserResponse> GetUserByIdAsync(string userId)
//     // {
//     //     var userInDb = await _userManager.FindByIdAsync(userId);
//     //     if (userInDb is null)
//     //         throw new Exception("User not found");


//     //     return userInDb.ToUserResponse();
//     // }

//     // public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
//     // {
//     //     var users = await _userManager.Users.ToListAsync();
//     //     return users.Select(user => user.ToUserResponse());
//     // }

//     // public async Task<UserResponse> UpdateUserAsync(UpdateUserRequest request, string userId)
//     // {
//     //     var userInDb = await _userManager.FindByIdAsync(userId);
//     //     if (userInDb is null)
//     //         throw new Exception("User not found");

//     //     userInDb.FirstName = request.FirstName;
//     //     userInDb.LastName = request.LastName;
//     //     userInDb.PhoneNumber = request.PhoneNumber;

//     //     var updateResult = await _userManager.UpdateAsync(userInDb);

//     //     if (updateResult.Succeeded)
//     //         return userInDb.ToUserResponse();

//     //     throw new Exception(updateResult.GetErrorsInLine());
//     // }


//     // public async Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest request)
//     // {
//     //     var userInDb = await _userManager.FindByIdAsync(request.UserId);
//     //     if (userInDb is null)
//     //         return await ResponseWrapper<UserResponse>.FailAsync("User not found");
//     //
//     //     userInDb.IsActive = request.Active;
//     //     var identityResult = await _userManager.UpdateAsync(userInDb);
//     //     if (identityResult.Succeeded) return await ResponseWrapper<string>.SuccessAsync("User modified successfully");
//     //     return await ResponseWrapper<string>.FailAsync(identityResult.GetErrors());
//     // }
//     //
//     // public async Task<IResponseWrapper> GetRolesAsync(string userId)
//     // {
//     //     var userRoles = new List<UserRoleViewModel>();
//     //     var userInDb = await _userManager.FindByIdAsync(userId);
//     //     if (userInDb is null)
//     //         return await ResponseWrapper<string>.FailAsync("User not found");
//     //
//     //     var allRoles = await _roleManager.Roles.ToListAsync();
//     //     foreach (var role in allRoles)
//     //     {
//     //         var userRoleVM = new UserRoleViewModel
//     //         {
//     //             RoleName = role.Name,
//     //             RoleDescription = role.Description,
//     //             IsAssignedToUser = false
//     //         };
//     //
//     //         if (await _userManager.IsInRoleAsync(userInDb, role.Name))
//     //         {
//     //             userRoleVM.IsAssignedToUser = true;
//     //         }
//     //
//     //         userRoles.Add(userRoleVM);
//     //     }
//     //
//     //     return await ResponseWrapper<List<UserRoleViewModel>>.SuccessAsync(userRoles);
//     // }
//     //
//     // public async Task<IResponseWrapper> UpdateUserRolesAsync(UpdateUserRoleRequest request)
//     // {
//     //     // cannot un-assign administrator
//     //     // default admin user seeded by addplication cannot be assigned/un-assigned
//     //     var userInDb = await _userManager.FindByIdAsync(request.UserId);
//     //     if (userInDb is null)
//     //         return await ResponseWrapper<string>.FailAsync("User not found");
//     //
//     //     if (userInDb.Email == AppCredentials.DefaultAdminEmail || userInDb.Email == AppCredentials.DefaultBasicEmail)
//     //         return await ResponseWrapper<string>.FailAsync("You cannot change basic data");
//     //
//     //     var roles = await _userManager.GetRolesAsync(userInDb);
//     //     var rolesToBeAssigned = request.Roles.Where(r => r.IsAssignedToUser).ToList();
//     //
//     //     var currentLoggedUser = await _userManager.FindByIdAsync(currentUserService.UserId);
//     //     if (currentLoggedUser is null)
//     //         return await ResponseWrapper<string>.FailAsync("User not found");
//     //
//     //     if (!await _userManager.IsInRoleAsync(currentLoggedUser, AppRoles.Admin))
//     //         return await ResponseWrapper<string>.FailAsync("Only administrators can change roles");
//     //
//     //     var result = await _userManager.RemoveFromRolesAsync(userInDb, roles);
//     //     if (!result.Succeeded)
//     //         return await ResponseWrapper<string>.FailAsync(result.GetErrors());
//     //     
//     //     var resultAddRoles = await _userManager.AddToRolesAsync(userInDb,rolesToBeAssigned.Select(x=>x.RoleName));
//     //     if(!resultAddRoles.Succeeded)
//     //         return await ResponseWrapper<string>.FailAsync(resultAddRoles.GetErrors());
//     //     
//     //     return await ResponseWrapper<string>.SuccessAsync("User roles modified successfully");
//     // }
// }