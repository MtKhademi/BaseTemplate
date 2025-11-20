namespace IAMModule.Contract.Requests;

public record RoleCreateCommand : ICommand<RoleModel>
{
    public string Name { get; init; }

    public string Description { get; init; }


    private RoleCreateCommand(string? name, string? description)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(name))
            errors.Add($"{nameof(name)} cannot be null or empty.");
        if (errors.Any())
            throw new RoleCreateCommandException(errors);

        Name = name!;
        Description = description ?? "";
    }

    public static RoleCreateCommand Create(RoleCreateRequest request)
        => new RoleCreateCommand(name: request.Name, description: request.Description);

    internal class RoleCreateCommandException : NotValidDataException<RoleCreateCommandException>
    {
        public RoleCreateCommandException(IEnumerable<string> errors) : base(errors)
        {
        }
    }
}