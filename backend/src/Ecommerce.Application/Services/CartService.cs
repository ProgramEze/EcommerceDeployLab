using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<CartDto> CreateAsync()
    {
        var cart = new Cart();

        await _cartRepository.AddAsync(cart);

        return MapToDto(cart);
    }

    public async Task<CartDto?> GetByIdAsync(Guid id)
    {
        var cart = await _cartRepository.GetByIdAsync(id);

        if (cart is null)
        {
            return null;
        }

        return MapToDto(cart);
    }

    public async Task<CartDto?> AddItemAsync(Guid cartId, AddCartItemDto dto)
    {
        var cart = await _cartRepository.GetByIdAsync(cartId);

        if (cart is null)
        {
            return null;
        }

        cart.AddItem(
            dto.ProductId,
            dto.ProductName,
            dto.UnitPrice,
            dto.Quantity
        );

        await _cartRepository.UpdateAsync(cart);

        return MapToDto(cart);
    }

    public async Task<CartDto?> ChangeItemQuantityAsync(
        Guid cartId,
        Guid productId,
        UpdateCartItemQuantityDto dto
    )
    {
        var cart = await _cartRepository.GetByIdAsync(cartId);

        if (cart is null)
        {
            return null;
        }

        cart.ChangeItemQuantity(productId, dto.Quantity);

        await _cartRepository.UpdateAsync(cart);

        return MapToDto(cart);
    }

    public async Task<CartDto?> RemoveItemAsync(Guid cartId, Guid productId)
    {
        var cart = await _cartRepository.GetByIdAsync(cartId);

        if (cart is null)
        {
            return null;
        }

        cart.RemoveItem(productId);

        await _cartRepository.UpdateAsync(cart);

        return MapToDto(cart);
    }

    public async Task<CartDto?> ClearAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetByIdAsync(cartId);

        if (cart is null)
        {
            return null;
        }

        cart.Clear();

        await _cartRepository.UpdateAsync(cart);

        return MapToDto(cart);
    }

    public async Task<bool> DeleteAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetByIdAsync(cartId);

        if (cart is null)
        {
            return false;
        }

        await _cartRepository.DeleteAsync(cart);

        return true;
    }

    private static CartDto MapToDto(Cart cart)
    {
        return new CartDto
        {
            Id = cart.Id,
            Items = cart.Items.Select(MapItemToDto).ToList(),
            Total = cart.Total,
            TotalItems = cart.TotalItems,
            IsEmpty = cart.IsEmpty,
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt
        };
    }

    private static CartItemDto MapItemToDto(CartItem item)
    {
        return new CartItemDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            UnitPrice = item.UnitPrice,
            Quantity = item.Quantity,
            Subtotal = item.Subtotal
        };
    }
}