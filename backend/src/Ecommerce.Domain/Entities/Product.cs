using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public Product(
        string name,
        string description,
        decimal price,
        int stock,
        string? imageUrl = null
    )
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidatePrice(price);
        ValidateStock(stock);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        Stock = stock;
        ImageUrl = string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string name,
        string description,
        decimal price,
        string? imageUrl = null
    )
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidatePrice(price);

        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        ImageUrl = string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("La cantidad a incrementar debe ser mayor que cero.");
        }

        Stock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("La cantidad a descontar debe ser mayor que cero.");
        }

        if (quantity > Stock)
        {
            throw new DomainException("No hay stock suficiente para realizar esta operación.");
        }

        Stock -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("El nombre del producto es obligatorio.");
        }

        if (name.Trim().Length > 100)
        {
            throw new DomainException(
                "El nombre del producto no puede superar los 100 caracteres."
            );
        }
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("La descripción del producto es obligatoria.");
        }

        if (description.Trim().Length > 500)
        {
            throw new DomainException(
                "La descripción del producto no puede superar los 500 caracteres."
            );
        }
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
        {
            throw new DomainException("El precio del producto debe ser mayor que cero.");
        }
    }

    private static void ValidateStock(int stock)
    {
        if (stock < 0)
        {
            throw new DomainException("El stock del producto no puede ser negativo.");
        }
    }
}
