using MediatR;

namespace Ordering.Application.Commands;

public class DeleteOrderCommand(int id) : IRequest
{
    public int Id { get; set; } = id;
}
