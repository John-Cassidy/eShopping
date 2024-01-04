using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer;

public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
{
    private IMediator _mediator;
    private IMapper _mapper;
    private ILogger<BasketOrderingConsumer> _logger;

    // create constructor with IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumer> logger parameters
    public BasketOrderingConsumer(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumer> logger)
    {
        // set _mediator, _mapper, _logger fields
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        using var scope = _logger.BeginScope("Consuming Basket Checkout Event for {correlationId}",
            context.Message.CorrelationId);
        // create variable basketCheckoutEvent with context.Message
        var basketCheckoutEvent = context.Message;
        // create variable command with _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent)
        var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);
        // create variable result with await _mediator.Send(command)
        var result = await _mediator.Send(command);

        _logger.LogInformation($"Basket checkout event completed!!!");

        // // if result.Succeeded is false
        // if (!result.Succeeded)
        // {
        //     // create variable errorMessage with string.Join(",", result.Errors)
        //     var errorMessage = string.Join(",", result.Errors);
        //     // log errorMessage
        //     _logger.LogError(errorMessage);
        // }
    }
}
