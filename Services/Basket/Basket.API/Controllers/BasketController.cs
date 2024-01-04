using System.Net;
using Basket.API.Controllers;
using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IDiscountGrpcService _discountGrpcService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<BasketController> _logger;
    private readonly ICorrelationIdGenerator _correlationIdGenerator;

    public BasketController(IMediator mediator, IDiscountGrpcService discountGrpcService, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger, ICorrelationIdGenerator correlationIdGenerator)
    {
        _mediator = mediator;
        _discountGrpcService = discountGrpcService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
        _correlationIdGenerator = correlationIdGenerator;
        _logger.LogInformation("CorrelationId {correlationId}:", _correlationIdGenerator.Get());
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

    // create http post action Checkout pass in BasketCheckout basketCheckout parameter returning Task<IActionResult>
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        // create_mediator to GetBasketByUserNameQuery
        var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
        var basket = await _mediator.Send(query);
        // if basket is null return BadRequest
        if (basket == null)
        {
            return BadRequest();
        }
        // create Mapper to get BasketCheckoutEvent from basketCheckout
        var eventMessage = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;        
        eventMessage.CorrelationId = _correlationIdGenerator.Get();
        // send eventMessage to _publishEndpoint
        await _publishEndpoint.Publish(eventMessage);
        // remove basket by basketCheckout.UserName
        var command = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
        var isDeleted = await _mediator.Send(command);
        // return Accepted
        return Accepted();
    }

}
