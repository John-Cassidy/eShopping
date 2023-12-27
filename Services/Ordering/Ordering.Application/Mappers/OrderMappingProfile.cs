using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Commands;
using Ordering.Application.Responses;
using Ordering.Core.Entities;

namespace Ordering.Application.Mappers;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderResponse>().ReverseMap();
        // create map from CheckoutOrderCommand to Core.Entities.Order
        CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
        // create map from UpdateOrderCommand to Core.Entities.Order
        CreateMap<Order, UpdateOrderCommand>().ReverseMap();
        CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
    }
}
