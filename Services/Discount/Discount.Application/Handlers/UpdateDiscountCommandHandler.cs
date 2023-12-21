using AutoMapper;
using Discount.Application.Commands;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Handlers;

public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
{
    // create a private readonly IDiscountRepository field _discountRepository and IMapper field _mapper
    // create a constructor that takes an IDiscountRepository and IMapper parameter and assigns them to _discountRepository and _mapper
    // create a public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken) method
    // inside the method, call the _discountRepository.GetDiscount method and pass the request.Id
    // assign the values from the request to the coupon object
    // call the _discountRepository.UpdateDiscount method and pass the coupon object
    // return the coupon object

    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public UpdateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = _mapper.Map<Coupon>(request);
        await _discountRepository.UpdateDiscount(coupon);
        return _mapper.Map<CouponModel>(coupon);
    }
}
