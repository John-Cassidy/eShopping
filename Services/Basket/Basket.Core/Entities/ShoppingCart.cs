namespace Basket.Core.Entities;

public class ShoppingCart
{
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    public string UserName { get; set; } = "";

    public ShoppingCart()
    {
    }
    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
}
