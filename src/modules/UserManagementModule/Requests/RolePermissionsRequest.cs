
namespace UserManagementModule.Contract.Requests;

public class RolePermissionsRequest
{
    public string RoleId { get; set; }
    public List<RoleClaimViewModel> Type { get; set; }
}