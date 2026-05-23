using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Entities;

public class Cart
{
    private readonly List<CartItem> _items = new();

    public Guid Id { get; private set; }
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public decimal Total => _items.Sum(item => item.Subtotal);
    public int TotalItems => _items.Sum(item => item.Quantity);
    public bool IsEmpty => !_items.Any();

    private Cart()
    {
    }

    public Cart(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity
    )
    {
        var existingItem = _items.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
            UpdatedAt = DateTime.UtcNow;
            return;
        }

        var item = new CartItem(
            productId,
            productName,
            unitPrice,
            quantity
        );

        _items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid productId)
    {
        ValidateProductId(productId);

        var item = _items.FirstOrDefault(item => item.ProductId == productId);

        if (item is null)
        {
            throw new DomainException("El producto no existe en el carrito.");
        }

        _items.Remove(item);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeItemQuantity(Guid productId, int quantity)
    {
        ValidateProductId(productId);

        var item = _items.FirstOrDefault(item => item.ProductId == productId);

        if (item is null)
        {
            throw new DomainException("El producto no existe en el carrito.");
        }

        item.ChangeQuantity(quantity);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Clear()
    {
        _items.Clear();
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateProductId(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new DomainException("El identificador del producto es obligatorio.");
        }
    }
}