using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Tests.Domain;

public class OrderItemTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateOrderItem()
    {
        var productId = Guid.NewGuid();

        var item = new OrderItem(
            productId,
            "Mouse Logitech",
            50m,
            2
        );

        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.Equal(productId, item.ProductId);
        Assert.Equal("Mouse Logitech", item.ProductName);
        Assert.Equal(50m, item.UnitPrice);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(100m, item.Subtotal);
    }

    [Fact]
    public void Constructor_WithEmptyProductId_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new OrderItem(
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
            new OrderItem(
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
            new OrderItem(
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
            new OrderItem(
                Guid.NewGuid(),
                "Producto inválido",
                100m,
                0
            )
        );

        Assert.Equal("La cantidad debe ser mayor que cero.", exception.Message);
    }
}