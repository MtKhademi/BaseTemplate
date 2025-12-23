namespace ECommerceModule.Contract.Cart.Commands;

public record CreateCartCommand : ICommand<CartModel>
{
    public Guid UserId { get; init; }

    private CreateCartCommand(Guid userId)
    {
        var errors = new List<string>();
        if (userId == Guid.Empty) errors.Add("UserId is required.");
        if (errors.Any()) throw new CreateCartCommandException(errors);
        UserId = userId;
    }

    public static CreateCartCommand Create(Guid userId) => new(userId);
    public static CreateCartCommand Create(CreateCartRequest request) => new(request.UserId);

    internal class CreateCartCommandException : NotValidDataException<CreateCartCommandException>
    {
        public CreateCartCommandException(IEnumerable<string> errors) : base(errors) { }
    }
}
