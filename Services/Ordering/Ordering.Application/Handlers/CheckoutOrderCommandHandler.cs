using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    // create constructor to inject services IOrderRepository orderRepository and IMapper mapper and ILogger<CheckoutOrderCommandHandler> logger and assign to local variables
    public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        // check if request is null and throw ArgumentNullException if it is
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        // map request to Core.Entities.Order
        var orderEntity = _mapper.Map<Order>(request);
        // add orderEntity to orderRepository
        var newOrder = await _orderRepository.AddAsync(orderEntity);
        // log new order created
        _logger.LogInformation($"Order {newOrder.Id} is successfully created.");
        // return newOrder.Id
        return newOrder.Id;
    }
}
