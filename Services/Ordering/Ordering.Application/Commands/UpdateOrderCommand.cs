using MediatR;

namespace Ordering.Application.Commands;

public class UpdateOrderCommand : IRequest
{
    // create properties based on Ordering.Infrastructure.Entities.Order
    public int Id { get; set; } = default;
    public string UserName { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; } = default;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string CardName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string Expiration { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
    public int PaymentMethod { get; set; } = default;
}
