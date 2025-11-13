namespace UserManagementModule.Entities;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public string Description { get; set; }
    public  string Group { get; set; }

}