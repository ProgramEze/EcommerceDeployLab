using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs;

public class CreatePaymentDto
{
    public Guid OrderId { get; set; }
    public PaymentMethod Method { get; set; }
    public string? ExternalReference { get; set; }
}