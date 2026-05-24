namespace Ecommerce.Application.DTOs;

public class CreateOrderDto
{
    public Guid CartId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
}