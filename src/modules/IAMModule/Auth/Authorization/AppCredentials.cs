namespace IAMModule.Auth.Authorization;

public static class AppCredentials
{
    public static List<AdminUserCredential> AdminUsers =
        new List<AdminUserCredential>
        {
            new()
            {
                Email = "admin@ss.com",
                UserName = "admin",
                Password = "8585@8585",
                FirstName = "Admin",
                LastName = "Admin",
                PhoneNumber = "+98 9399172444"
            }
        };
}

public class AdminUserCredential
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
}