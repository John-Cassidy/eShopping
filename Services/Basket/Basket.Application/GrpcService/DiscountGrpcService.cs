using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService;

public class DiscountGrpcService
{   
    private readonly DiscountService.DiscountServiceClient _discountServiceClient; // Update the type

    public DiscountGrpcService(DiscountService.DiscountServiceClient discountServiceClient) {
        _discountServiceClient = discountServiceClient ?? throw new ArgumentNullException(nameof(discountServiceClient));
    }

    public async Task<CouponModel> GetDiscount(string productName) {
        var discountRequest = new GetDiscountRequest { ProductName = productName };
        return await _discountServiceClient.GetDiscountAsync(discountRequest);
    }
}
