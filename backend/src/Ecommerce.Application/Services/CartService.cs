using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository
    )
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
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

        var product = await _productRepository.GetByIdAsync(dto.ProductId);

        if (product is null)
        {
            throw new DomainException("El producto no existe.");
        }

        if (!product.IsActive)
        {
            throw new DomainException("El producto no está activo.");
        }

        var currentQuantityInCart = cart.Items
            .Where(item => item.ProductId == product.Id)
            .Sum(item => item.Quantity);

        var requestedTotalQuantity = currentQuantityInCart + dto.Quantity;

        if (requestedTotalQuantity > product.Stock)
        {
            throw new DomainException("No hay stock suficiente para agregar esa cantidad al carrito.");
        }

        cart.AddItem(
            product.Id,
            product.Name,
            product.Price,
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