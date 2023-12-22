using System.Net;
using Basket.API.Controllers;
using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Queries;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IMediator mediator, DiscountGrpcService discountGrpcService)
    {
        _mediator = mediator;
        _discountGrpcService = discountGrpcService;
    }

    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasketAsync(string userName)
    {
        var query = new GetBasketByUserNameQuery(userName);
        var basket = await _mediator.Send(query);
        return Ok(basket ?? new ShoppingCartResponse(userName));
    }

    [HttpPost("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasketAsync([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
    {
        // send coupon grpc request to calculate latest prices of product into shopping cart
        foreach (var item in createShoppingCartCommand.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }

        var basket = await _mediator.Send(createShoppingCartCommand);
        return Ok(basket);
    }

    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasketAsync(string userName)
    {
        var command = new DeleteBasketByUserNameCommand(userName);
        return Ok(await _mediator.Send(command));
    }
}
