namespace IAMModule.Contract.Commands;

public record UserChangeStateActiveCommand : ICommand<ApplicationUserModel>
{
    public string UserId { get; init; }

    public UserChangeStateActiveCommand(string userId)
    {

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(userId))
            errors.Add($"{nameof(userId)} is required.");
        if (errors.Any())
            throw new UserChangeStateActiveCommandException(errors);

        UserId = userId!;
    }


    internal class UserChangeStateActiveCommandException : NotValidDataException<UserChangeStateActiveCommandException>
    {
        public UserChangeStateActiveCommandException(IEnumerable<string> errors) :
            base(errors)
        {
        }
    }
}