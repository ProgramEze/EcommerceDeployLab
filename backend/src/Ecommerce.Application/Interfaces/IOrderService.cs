using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

public interface IOrderService
{
    Task<IReadOnlyList<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(Guid id);
    Task<OrderDto> CreateFromCartAsync(CreateOrderDto dto);
    Task<OrderDto?> ConfirmAsync(Guid id);
    Task<OrderDto?> CancelAsync(Guid id);
}