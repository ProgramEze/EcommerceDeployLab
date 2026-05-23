using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Tests.Domain;

public class CartItemTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateCartItem()
    {
        var productId = Guid.NewGuid();

        var item = new CartItem(
            productId,
            "Notebook Lenovo",
            1200m,
            2
        );

        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.Equal(productId, item.ProductId);
        Assert.Equal("Notebook Lenovo", item.ProductName);
        Assert.Equal(1200m, item.UnitPrice);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(2400m, item.Subtotal);
    }

    [Fact]
    public void Constructor_WithEmptyProductId_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new CartItem(
                Guid.Empty,
                "Producto inválido",
                100m,
                1
            )
        );

        Assert.Equal("El identificador del producto es obligatorio.", exception.Message);
    }

    [Fact]
    public void Constructor_WithEmptyProductName_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new CartItem(
                Guid.NewGuid(),
                "",
                100m,
                1
            )
        );

        Assert.Equal("El nombre del producto es obligatorio.", exception.Message);
    }

    [Fact]
    public void Constructor_WithInvalidUnitPrice_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new CartItem(
                Guid.NewGuid(),
                "Producto inválido",
                0m,
                1
            )
        );

        Assert.Equal("El precio unitario debe ser mayor que cero.", exception.Message);
    }

    [Fact]
    public void Constructor_WithInvalidQuantity_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new CartItem(
                Guid.NewGuid(),
                "Producto inválido",
                100m,
                0
            )
        );

        Assert.Equal("La cantidad debe ser mayor que cero.", exception.Message);
    }

    [Fact]
    public void IncreaseQuantity_WithValidQuantity_ShouldIncreaseQuantity()
    {
        var item = new CartItem(
            Guid.NewGuid(),
            "Mouse Logitech",
            50m,
            2
        );

        item.IncreaseQuantity(3);

        Assert.Equal(5, item.Quantity);
        Assert.Equal(250m, item.Subtotal);
    }

    [Fact]
    public void IncreaseQuantity_WithInvalidQuantity_ShouldThrowDomainException()
    {
        var item = new CartItem(
            Guid.NewGuid(),
            "Mouse Logitech",
            50m,
            2
        );

        var exception = Assert.Throws<DomainException>(() =>
            item.IncreaseQuantity(0)
        );

        Assert.Equal("La cantidad debe ser mayor que cero.", exception.Message);
    }

    [Fact]
    public void ChangeQuantity_WithValidQuantity_ShouldChangeQuantity()
    {
        var item = new CartItem(
            Guid.NewGuid(),
            "Teclado Mecánico",
            100m,
            2
        );

        item.ChangeQuantity(5);

        Assert.Equal(5, item.Quantity);
        Assert.Equal(500m, item.Subtotal);
    }

    [Fact]
    public void ChangeQuantity_WithInvalidQuantity_ShouldThrowDomainException()
    {
        var item = new CartItem(
            Guid.NewGuid(),
            "Teclado Mecánico",
            100m,
            2
        );

        var exception = Assert.Throws<DomainException>(() =>
            item.ChangeQuantity(-1)
        );

        Assert.Equal("La cantidad debe ser mayor que cero.", exception.Message);
    }
}