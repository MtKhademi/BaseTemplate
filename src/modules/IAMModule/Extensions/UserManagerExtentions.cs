using IAMModule.IAM.Exceptions;

namespace IAMModule.Extensions;

internal static class UserManagerExtentions
{

    /// <summary>
    /// if exist this email get true
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal static async Task<bool> CheckExistEmailAsync(
        this UserManager<ApplicationUser> userManager, string email,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByEmailAsync(email) is not null;
    }


    /// <summary>
    /// if exist this email throw UserAlreadyExistWithEmailException
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="UserAlreadyExistWithEmailException"></exception>
    internal static async Task CheckExistEmailAndThrowAsync(
        this UserManager<ApplicationUser> userManager, string email,
        CancellationToken cancellationToken = default)
    {
        if (await userManager.CheckExistEmailAsync(email, cancellationToken))
            throw new UserAlreadyExistWithEmailException(email);
    }



    /// <summary>
    /// get user by phone number
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal static async Task<ApplicationUser?> FindByPhoneAsync(
        this UserManager<ApplicationUser> userManager, string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return await userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
    }


    /// <summary>
    /// if exist this phone number get true
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal static async Task<bool> CheckExistPhoneAsync(
        this UserManager<ApplicationUser> userManager, string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        return await userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken) is not null;
    }


    /// <summary>
    /// if exist this phone number throw UserAlreadyExistWithPhoneException
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="UserAlreadyExistWithPhoneException"></exception>
    internal static async Task CheckExistPhoneOrThrowAsync(
        this UserManager<ApplicationUser> userManager, string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        if (await userManager.CheckExistPhoneAsync(phoneNumber, cancellationToken))
            throw new UserAlreadyExistWithPhoneException(phoneNumber);
    }


    /// <summary>
    /// if exist this user name get true
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal static async Task<bool> CheckExistUserNameAsync(
        this UserManager<ApplicationUser> userManager, string userName,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByNameAsync(userName) is not null;
    }


    /// <summary>
    /// if exist this user name throw UserAlreadyExistWithUserNameException
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="UserAlreadyExistWithUserNameException"></exception>
    internal static async Task CheckExistUserNameOrThrowAsync(
        this UserManager<ApplicationUser> userManager, string userName,
        CancellationToken cancellationToken = default)
    {
        if (await userManager.CheckExistUserNameAsync(userName, cancellationToken))
            throw new UserAlreadyExistWithUserNameException(userName);
    }

    /// <summary>
    /// if not exist this user name throw UserNotFoundWithUserNameException
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="UserNotFoundWithUserNameException"></exception>
    internal static async Task<ApplicationUser> FindByNameOrThrowAsync(
        this UserManager<ApplicationUser> userManager,
        string userName,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user is null)
            throw new UserNotFoundWithUserNameException(userName);
        return user;
    }
}
