using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

public interface ICartService
{
    Task<CartDto> CreateAsync();
    Task<CartDto?> GetByIdAsync(Guid id);
    Task<CartDto?> AddItemAsync(Guid cartId, AddCartItemDto dto);
    Task<CartDto?> ChangeItemQuantityAsync(Guid cartId, Guid productId, UpdateCartItemQuantityDto dto);
    Task<CartDto?> RemoveItemAsync(Guid cartId, Guid productId);
    Task<CartDto?> ClearAsync(Guid cartId);
    Task<bool> DeleteAsync(Guid cartId);
}