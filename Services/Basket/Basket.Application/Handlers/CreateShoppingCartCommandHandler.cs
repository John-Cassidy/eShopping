using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IDiscountGrpcService _discountGrpcService;

    public CreateShoppingCartCommandHandler(IBasketRepository basketRepository, IDiscountGrpcService discountGrpcService)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
    }

    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        // send coupon grpc request to calculate latest prices of products into shopping cart
        foreach (var item in request.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }

        var shoppingCartEntity = BasketMapper.Mapper.Map<ShoppingCart>(request);
        if (shoppingCartEntity is null)
        {
            throw new ApplicationException("Issue with mapper");
        }

        var newShoppingCart = await _basketRepository.UpdateBasket(shoppingCartEntity);
        var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(newShoppingCart);
        return shoppingCartResponse;
    }
}
