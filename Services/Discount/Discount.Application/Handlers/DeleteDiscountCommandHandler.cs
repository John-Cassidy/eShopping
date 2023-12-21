using Discount.Application.Commands;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handlers;

public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
{
    // create a private readonly IDiscountRepository field _discountRepository
    // create a constructor that takes an IDiscountRepository parameter and assigns it to _discountRepository
    // create a public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken) method
    // inside the method, call the _discountRepository.DeleteDiscount method and pass the request.ProductName
    // return true

    private readonly IDiscountRepository _discountRepository;

    public DeleteDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        return await _discountRepository.DeleteDiscount(request.ProductName);
    }
}
