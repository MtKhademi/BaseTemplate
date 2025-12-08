namespace IAMModule.Contract.Requests;

public record RoleDeleteCommand : ICommand<bool>
{
    public string RoleId { get; init; }

    private RoleDeleteCommand(string? roleId)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(roleId))
            errors.Add($"{nameof(roleId)} cannot be null or empty.");
        if (errors.Any())
            throw new RoleDeleteCommandException(errors);

        RoleId = roleId!;
    }

    public static RoleDeleteCommand Create(string roleId) => new RoleDeleteCommand(roleId: roleId);

    internal class RoleDeleteCommandException : NotValidDataException<RoleDeleteCommandException>
    {
        public RoleDeleteCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}