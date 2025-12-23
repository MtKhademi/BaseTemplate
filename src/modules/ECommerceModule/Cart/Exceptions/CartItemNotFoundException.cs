namespace ECommerceModule.Cart.Exceptions;

public class CartItemNotFoundException : NotFoundEntityException<CartItemNotFoundException>
{
    public CartItemNotFoundException(int cartItemId)
        : base($"Cart item with id '{cartItemId}' not found.")
    {
    }
}
