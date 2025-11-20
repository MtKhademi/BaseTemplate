namespace IAMModule.Role.Extensions;

internal static class RoleNameValidations
{
    public static bool IsRoleNameValid(this string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return false;
        if (roleName.Length < 3 || roleName.Length > 50)
            return false;
        foreach (char c in roleName)
        {
            if (!char.IsLetterOrDigit(c) && c != '_' && c != '-')
                return false;
        }
        return true;
    }
}