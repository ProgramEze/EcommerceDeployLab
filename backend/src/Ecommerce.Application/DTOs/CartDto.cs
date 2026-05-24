namespace Ecommerce.Application.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    public IReadOnlyList<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    public decimal Total { get; set; }
    public int TotalItems { get; set; }
    public bool IsEmpty { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}