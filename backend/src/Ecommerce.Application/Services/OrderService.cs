using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository
    )
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();

        return orders
            .Select(MapToDto)
            .ToList();
    }

    public async Task<OrderDto?> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order is null)
        {
            return null;
        }

        return MapToDto(order);
    }

    public async Task<OrderDto> CreateFromCartAsync(CreateOrderDto dto)
    {
        var cart = await _cartRepository.GetByIdAsync(dto.CartId);

        if (cart is null)
        {
            throw new DomainException("El carrito no existe.");
        }

        if (cart.IsEmpty)
        {
            throw new DomainException("No se puede crear una orden desde un carrito vacío.");
        }

        var orderItems = cart.Items
            .Select(item =>
                new OrderItem(
                    item.ProductId,
                    item.ProductName,
                    item.UnitPrice,
                    item.Quantity
                )
            )
            .ToList();

        var order = new Order(
            cart.Id,
            dto.CustomerName,
            dto.CustomerEmail,
            orderItems
        );

        await _orderRepository.AddAsync(order);

        return MapToDto(order);
    }

    public async Task<OrderDto?> ConfirmAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order is null)
        {
            return null;
        }

        order.Confirm();

        await _orderRepository.UpdateAsync(order);

        return MapToDto(order);
    }

    public async Task<OrderDto?> CancelAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order is null)
        {
            return null;
        }

        order.Cancel();

        await _orderRepository.UpdateAsync(order);

        return MapToDto(order);
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CartId = order.CartId,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            Items = order.Items.Select(MapItemToDto).ToList(),
            Status = order.Status,
            Total = order.Total,
            TotalItems = order.TotalItems,
            CreatedAt = order.CreatedAt,
            ConfirmedAt = order.ConfirmedAt,
            CancelledAt = order.CancelledAt
        };
    }

    private static OrderItemDto MapItemToDto(OrderItem item)
    {
        return new OrderItemDto
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