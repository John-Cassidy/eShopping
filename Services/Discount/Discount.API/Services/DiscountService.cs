using Common.Logging.Correlation;
using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services;

public class DiscountService : Grpc.Protos.DiscountService.DiscountServiceBase
{
    // create a private readonly IMediator _mediator
    private readonly IMediator _mediator;
    // create ILogger<DiscountService> _logger
    private readonly ILogger<DiscountService> _logger;
    private readonly ICorrelationIdGenerator _correlationIdGenerator;

    // inject IMediator and ILogger<DiscountService> into the constructor
    public DiscountService(IMediator mediator, ILogger<DiscountService> logger, ICorrelationIdGenerator correlationIdGenerator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _correlationIdGenerator = correlationIdGenerator ?? throw new ArgumentNullException(nameof(correlationIdGenerator));
        _logger.LogInformation("CorrelationId {correlationId}:", _correlationIdGenerator.Get());
    }

    // override the GetDiscount method
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var query = new GetDiscountQuery(request.ProductName);
        var result = await _mediator.Send(query);
        _logger.LogInformation($"Discount is retrieved for the Product Name: {request.ProductName} and Amount : {result.Amount}");
        return result;
    }

    // override the CreateDiscount method
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var command = new CreateDiscountCommand
        {
            ProductName = request.Coupon.ProductName,
            Amount = request.Coupon.Amount,
            Description = request.Coupon.Description
        };
        var result = await _mediator.Send(command);
        _logger.LogInformation($"Discount is successfully created for Product Name: {result.ProductName} and Amount : {result.Amount}");
        return result;
    }

    // override the DeleteDiscount method
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var command = new DeleteDiscountCommand(request.ProductName);
        var result = await _mediator.Send(command);
        _logger.LogInformation($"Discount is successfully deleted for Product Name: {request.ProductName}");
        return new DeleteDiscountResponse { Success = result };
    }

    // override the UpdateDiscount method
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var command = new UpdateDiscountCommand
        {
            Id = request.Coupon.Id,
            ProductName = request.Coupon.ProductName,
            Amount = request.Coupon.Amount,
            Description = request.Coupon.Description
        };
        var result = await _mediator.Send(command);
        _logger.LogInformation($"Discount is successfully updated for Product Name: {request.Coupon.ProductName} and Amount : {request.Coupon.Amount}");
        return result;
    }
}