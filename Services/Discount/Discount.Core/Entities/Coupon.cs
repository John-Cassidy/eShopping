namespace Discount.Core.Entities;

public class Coupon(int id = default, string productName = "", string description = "", int amount = default)
{
    public int Id { get; } = id;
    public string ProductName { get; } = productName;
    public string Description { get; } = description;
    public int Amount { get; } = amount;
}