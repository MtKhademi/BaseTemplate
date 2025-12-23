namespace ECommerceModule.Contract.Cart.Commands;

public record DeleteCartCommand : ICommand<bool>
{
    public int CartId { get; init; }

    private DeleteCartCommand(int cartId)
    {
        var errors = new List<string>();
        if (cartId <= 0) errors.Add("CartId must be greater than zero.");
        if (errors.Any()) throw new DeleteCartCommandException(errors);
        CartId = cartId;
    }

    public static DeleteCartCommand Create(int cartId) => new(cartId);

    internal class DeleteCartCommandException : NotValidDataException<DeleteCartCommandException>
    {
        public DeleteCartCommandException(IEnumerable<string> errors) : base(errors) { }
    }
}
