namespace IAMModule.Contract.Requests;

public record RoleUpdateCommand : ICommand<RoleModel>
{
    public string RoleId { get; init; }
    public string Name { get; init; }

    public string Description { get; init; }


    private RoleUpdateCommand(
        string? roleId, string? name, string? description)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(roleId))
            errors.Add($"{nameof(roleId)} cannot be null or empty.");
        if (string.IsNullOrWhiteSpace(name))
            errors.Add($"{nameof(name)} cannot be null or empty.");
        if (errors.Any())
            throw new RoleUpdateCommandException(errors);

        RoleId = roleId!;
        Name = name!;
        Description = description ?? "";
    }

    public static RoleUpdateCommand Create(RoleUpdateRequest request)
        => new RoleUpdateCommand(roleId: request.roleId, name: request.Name, description: request.Description);

    internal class RoleUpdateCommandException : NotValidDataException<RoleUpdateCommandException>
    {
        public RoleUpdateCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}