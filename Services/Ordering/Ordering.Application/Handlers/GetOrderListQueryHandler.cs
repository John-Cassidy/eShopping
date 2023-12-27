using AutoMapper;
using MediatR;
using Ordering.Application.Exceptions;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    // create constructor to inject services IOrderRepository orderRepository and IMapper mapper and assign to local variables
    public GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<List<OrderResponse>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
        // check if request is null and throw ArgumentNullException if it is
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        // get orders by username
        var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
        // if orderToUpdate is null throw OrderNotFoundException
        if (orderList == null)
        {
            throw new OrderNotFoundException(nameof(List<Ordering.Core.Entities.Order>), orderList);
        }
        // map orderList to List<OrderResponse> and return
        return _mapper.Map<List<OrderResponse>>(orderList);
    }
}
