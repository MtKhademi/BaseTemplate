namespace IAMModule.Contract.Queries;

public record RoleGetByRoleIdQuery : IQuery<RoleModel>
{
    public string RoleId { get; init; }

    private RoleGetByRoleIdQuery(string? roleId)
    {
        if (string.IsNullOrWhiteSpace(roleId))
        {
            throw new RoleGetByRoleIdQueryException($"{nameof(roleId)} cannot be null or empty.");
        }

        RoleId = roleId!;
    }

    public static RoleGetByRoleIdQuery Create(string? roleId) => new RoleGetByRoleIdQuery(roleId);

    internal class RoleGetByRoleIdQueryException : NotValidDataException<RoleGetByRoleIdQueryException>
    {
        public RoleGetByRoleIdQueryException(string error) : base(error)
        {
        }
    }
}