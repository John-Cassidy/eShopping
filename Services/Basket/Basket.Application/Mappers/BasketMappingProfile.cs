using AutoMapper;
using Basket.Core.Entities;

namespace Basket.Application.Mappers;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        CreateMap<ShoppingCart, Responses.ShoppingCartResponse>().ReverseMap();
        CreateMap<ShoppingCartItem, Responses.ShoppingCartItemResponse>().ReverseMap();
    }
}
