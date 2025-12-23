namespace ECommerceModule.Contract.Cart.Queries;

public record GetCartByIdQuery : IQuery<CartModel>
{
    public int CartId { get; init; }

    private GetCartByIdQuery(int cartId)
    {
        var errors = new List<string>();
        if (cartId <= 0) errors.Add("CartId must be greater than zero.");
        if (errors.Any()) throw new GetCartByIdQueryException(errors);
        CartId = cartId;
    }

    public static GetCartByIdQuery Create(int cartId) => new(cartId);

    internal class GetCartByIdQueryException : NotValidDataException<GetCartByIdQueryException>
    {
        public GetCartByIdQueryException(IEnumerable<string> errors) : base(errors) { }
    }
}
