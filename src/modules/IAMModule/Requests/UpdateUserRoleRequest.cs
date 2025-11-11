namespace IAMModule.Contract.Requests;

public class UpdateUserRoleRequest
{
    public string UserId { get; set; }
    public List<UserRoleViewModel> Roles { get; set; }
}