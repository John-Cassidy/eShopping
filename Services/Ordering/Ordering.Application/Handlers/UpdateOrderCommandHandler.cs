using System.Runtime.Serialization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;

    // create constructor to inject services IOrderRepository orderRepository and IMapper mapper and ILogger<UpdateOrderCommandHandler> logger and assign to local variables
    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        // check if request is null and throw ArgumentNullException if it is
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        // get order by request.Id
        var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
        // if orderToUpdate is null throw OrderNotFoundException
        if (orderToUpdate == null)
        {
            throw new OrderNotFoundException(nameof(Order), request.Id);
        }
        // use _mapper to map request to orderToUpdate
        _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));
        // use _orderRepository to Update orderToUpdate
        await _orderRepository.UpdateAsync(orderToUpdate);
        // log information that orderToUpdate is successfully updated
        _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");
        // return Unit.Value
        return Unit.Value;
    }
}
