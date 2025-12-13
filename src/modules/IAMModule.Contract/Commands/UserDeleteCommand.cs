namespace IAMModule.Contract.Commands;

public record UserDeleteCommand : ICommand<bool>
{
    public string UserId { get; init; }

    private UserDeleteCommand(string userId)
    {

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(userId))
            errors.Add($"{nameof(userId)} is required.");

        if (errors.Any())
            throw new UserDeleteCommandException(errors);

        UserId = userId!;
    }


    public static UserDeleteCommand Create(string? userId) => new UserDeleteCommand(userId: userId ?? "");

    internal class UserDeleteCommandException : NotValidDataException<UserDeleteCommandException>
    {
        public UserDeleteCommandException(IEnumerable<string> errors) :
            base(errors)
        {
        }
    }
}