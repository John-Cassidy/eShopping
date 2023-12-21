using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository;

    public CreateShoppingCartCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
    }

    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
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
