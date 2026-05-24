using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public IReadOnlyList<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    public OrderStatus Status { get; set; }
    public decimal Total { get; set; }
    public int TotalItems { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}