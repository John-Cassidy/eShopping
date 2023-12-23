using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    // inject IOrderRepository and IMapper and ILogger<DeleteOrderCommandHandler> and assign to local variables
    public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        // get order by id
        var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
        // if orderToDelete is null throw OrderNotFoundException
        if (orderToDelete == null)
        {
            throw new OrderNotFoundException(nameof(Order), request.Id);
        }
        // use _orderRepository to Delete orderToDelete
        await _orderRepository.DeleteAsync(orderToDelete);
        // log information that orderToDelete is successfully deleted
        _logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");
        // return Unit.Value
        return Unit.Value;
    }
}
