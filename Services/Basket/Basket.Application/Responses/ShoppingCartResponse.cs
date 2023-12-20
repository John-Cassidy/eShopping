﻿namespace Basket.Application.Responses;

public class ShoppingCartResponse
{
    public List<ShoppingCartItemResponse> Items { get; set; } = new List<ShoppingCartItemResponse>();
    public string UserName { get; set; } = "";
    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            foreach (var item in Items)
            {
                totalPrice += item.Price * item.Quantity;
            }
            return totalPrice;
        }
    }
    public ShoppingCartResponse()
    {
    }
    public ShoppingCartResponse(string userName)
    {
        UserName = userName;
    }
}
