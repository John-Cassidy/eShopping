using MediatR;

namespace Discount.Application.Commands;

public class DeleteDiscountCommand : IRequest<bool>
{
    public string ProductName { get; set; } = default!;

    public DeleteDiscountCommand(string productName)
    {
        ProductName = productName;
    }
}
