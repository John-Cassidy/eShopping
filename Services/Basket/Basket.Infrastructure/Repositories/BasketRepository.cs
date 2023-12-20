using System.Text.Json;
using Basket.Core;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Infrastructure;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;
    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<ShoppingCart> GetBasket(string userName)
    {
        // use _redisCache.GetStringAsync to get the basket from Redis
        // if the basket is null, return null
        // otherwise, deserialize the basket from JSON and return it
        var basket = await _redisCache.GetStringAsync(userName);
        if (string.IsNullOrEmpty(basket))
        {
            return null;
        }
        return JsonSerializer.Deserialize<ShoppingCart>(basket)!;
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        // serialize the shoppingCart to JSON
        // use _redisCache.SetStringAsync to save the basket to Redis
        // return the shoppingCart
        var basket = JsonSerializer.Serialize(shoppingCart);
        await _redisCache.SetStringAsync(shoppingCart.UserName, basket);
        return shoppingCart;
    }

    public async Task<bool> DeleteBasket(string userName)
    {
        // use _redisCache.RemoveAsync to delete the basket from Redis
        // return true
        await _redisCache.RemoveAsync(userName);
        return true;
    }
}
