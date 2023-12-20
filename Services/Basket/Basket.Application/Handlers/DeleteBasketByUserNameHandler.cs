using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class DeleteBasketByUserNameHandler : IRequestHandler<DeleteBasketByUserNameCommand, bool>
{
    private readonly IBasketRepository _basketRepository; // Declare the _basketRepository variable

    public DeleteBasketByUserNameHandler(IBasketRepository basketRepository) // Initialize the _basketRepository variable through constructor injection
    {
        _basketRepository = basketRepository;
    }

    public async Task<bool> Handle(DeleteBasketByUserNameCommand request, CancellationToken cancellationToken)
    {
        return await _basketRepository.DeleteBasket(request.UserName);
    }
}
