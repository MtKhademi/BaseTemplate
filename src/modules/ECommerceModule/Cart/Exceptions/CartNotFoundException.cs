namespace ECommerceModule.Cart.Exceptions;

public class CartNotFoundException : NotFoundEntityException<CartNotFoundException>
{
    public CartNotFoundException(int cartId)
        : base($"Cart with id '{cartId}' not found.")
    {
    }
}
