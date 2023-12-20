namespace Basket.Core.Entities;

public class ShoppingCartItem(int quantity, decimal price, string productId, string imageFile, string productName)
{
    public int Quantity { get; set; } = quantity;
    public decimal Price { get; set; } = price;
    public string ProductId { get; set; } = productId;
    public string ImageFile { get; set; } = imageFile;
    public string ProductName { get; set; } = productName;
}
