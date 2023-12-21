using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Commands;

public class CreateDiscountCommand : IRequest<CouponModel>
{
    public string ProductName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
}
