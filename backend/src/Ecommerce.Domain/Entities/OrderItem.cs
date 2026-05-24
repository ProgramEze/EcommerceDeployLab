using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal Subtotal => UnitPrice * Quantity;

    private OrderItem()
    {
        ProductName = string.Empty;
    }

    public OrderItem(
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity
    )
    {
        ValidateProductId(productId);
        ValidateProductName(productName);
        ValidateUnitPrice(unitPrice);
        ValidateQuantity(quantity);

        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    private static void ValidateProductId(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new DomainException("El identificador del producto es obligatorio.");
        }
    }

    private static void ValidateProductName(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException("El nombre del producto es obligatorio.");
        }

        if (productName.Trim().Length > 100)
        {
            throw new DomainException("El nombre del producto no puede superar los 100 caracteres.");
        }
    }

    private static void ValidateUnitPrice(decimal unitPrice)
    {
        if (unitPrice <= 0)
        {
            throw new DomainException("El precio unitario debe ser mayor que cero.");
        }
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("La cantidad debe ser mayor que cero.");
        }
    }
}