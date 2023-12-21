using AutoMapper;
using Discount.Application.Commands;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Handlers;

public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
{
    // create a private readonly IDiscountRepository field _discountRepository
    // create a constructor that takes an IDiscountRepository parameter and assigns it to _discountRepository
    // create a public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken) method
    // inside the method, create a Coupon coupon object and assign it the values from the request
    // call the _discountRepository.CreateDiscount method and pass the coupon object
    // return the coupon object

    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public CreateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = _mapper.Map<Coupon>(request);
        await _discountRepository.CreateDiscount(coupon);
        return _mapper.Map<CouponModel>(coupon);
    }
}
