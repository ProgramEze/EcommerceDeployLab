using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; private set; }
    public Guid CartId { get; private set; }
    public string CustomerName { get; private set; }
    public string CustomerEmail { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    public decimal Total => _items.Sum(item => item.Subtotal);
    public int TotalItems => _items.Sum(item => item.Quantity);

    private Order()
    {
        CustomerName = string.Empty;
        CustomerEmail = string.Empty;
    }

    public Order(
        Guid cartId,
        string customerName,
        string customerEmail,
        IEnumerable<OrderItem> items
    )
    {
        ValidateCartId(cartId);
        ValidateCustomerName(customerName);
        ValidateCustomerEmail(customerEmail);
        ValidateItems(items);

        Id = Guid.NewGuid();
        CartId = cartId;
        CustomerName = customerName.Trim();
        CustomerEmail = customerEmail.Trim();
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;

        _items.AddRange(items);
    }

    public void Confirm()
    {
        if (Status == OrderStatus.Cancelled)
        {
            throw new DomainException("No se puede confirmar una orden cancelada.");
        }

        if (Status == OrderStatus.Confirmed)
        {
            throw new DomainException("La orden ya está confirmada.");
        }

        Status = OrderStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Confirmed)
        {
            throw new DomainException("No se puede cancelar una orden confirmada.");
        }

        if (Status == OrderStatus.Cancelled)
        {
            throw new DomainException("La orden ya está cancelada.");
        }

        Status = OrderStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
    }

    private static void ValidateCartId(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new DomainException("El identificador del carrito es obligatorio.");
        }
    }

    private static void ValidateCustomerName(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new DomainException("El nombre del cliente es obligatorio.");
        }

        if (customerName.Trim().Length > 150)
        {
            throw new DomainException("El nombre del cliente no puede superar los 150 caracteres.");
        }
    }

    private static void ValidateCustomerEmail(string customerEmail)
    {
        if (string.IsNullOrWhiteSpace(customerEmail))
        {
            throw new DomainException("El email del cliente es obligatorio.");
        }

        if (customerEmail.Trim().Length > 200)
        {
            throw new DomainException("El email del cliente no puede superar los 200 caracteres.");
        }

        if (!customerEmail.Contains('@'))
        {
            throw new DomainException("El email del cliente no tiene un formato válido.");
        }
    }

    private static void ValidateItems(IEnumerable<OrderItem> items)
    {
        if (items is null)
        {
            throw new DomainException("La orden debe tener al menos un producto.");
        }

        if (!items.Any())
        {
            throw new DomainException("La orden debe tener al menos un producto.");
        }
    }
}