namespace Basket.Application.Responses;

public class ShoppingCartItemResponse(int quantity, decimal price, string productId, string imageFile, string productName)
{
    public int Quantity { get; set; } = quantity;
    public string ImageFile { get; set; } = imageFile;
    public decimal Price { get; set; } = price;
    public string ProductId { get; set; } = productId;
    public string ProductName { get; set; } = productName;
}
