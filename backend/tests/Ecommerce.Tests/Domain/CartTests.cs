using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Tests.Domain;

public class CartTests
{
    [Fact]
    public void Constructor_ShouldCreateEmptyCart()
    {
        var cart = new Cart();

        Assert.NotEqual(Guid.Empty, cart.Id);
        Assert.Empty(cart.Items);
        Assert.True(cart.IsEmpty);
        Assert.Equal(0, cart.Total);
        Assert.Equal(0, cart.TotalItems);
        Assert.True(cart.CreatedAt <= DateTime.UtcNow);
        Assert.Null(cart.UpdatedAt);
    }

    [Fact]
    public void AddItem_WhenProductDoesNotExist_ShouldAddNewItem()
    {
        var cart = new Cart();
        var productId = Guid.NewGuid();

        cart.AddItem(
            productId,
            "Notebook Lenovo",
            1200m,
            2
        );

        Assert.Single(cart.Items);
        Assert.False(cart.IsEmpty);
        Assert.Equal(2, cart.TotalItems);
        Assert.Equal(2400m, cart.Total);
        Assert.NotNull(cart.UpdatedAt);

        var item = cart.Items.First();

        Assert.Equal(productId, item.ProductId);
        Assert.Equal("Notebook Lenovo", item.ProductName);
        Assert.Equal(1200m, item.UnitPrice);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(2400m, item.Subtotal);
    }

    [Fact]
    public void AddItem_WhenProductAlreadyExists_ShouldIncreaseQuantity()
    {
        var cart = new Cart();
        var productId = Guid.NewGuid();

        cart.AddItem(productId, "Mouse Logitech", 50m, 2);
        cart.AddItem(productId, "Mouse Logitech", 50m, 3);

        Assert.Single(cart.Items);
        Assert.Equal(5, cart.TotalItems);
        Assert.Equal(250m, cart.Total);

        var item = cart.Items.First();

        Assert.Equal(5, item.Quantity);
    }

    [Fact]
    public void AddItem_WithInvalidQuantity_ShouldThrowDomainException()
    {
        var cart = new Cart();

        var exception = Assert.Throws<DomainException>(() =>
            cart.AddItem(
                Guid.NewGuid(),
                "Producto inválido",
                100m,
                0
            )
        );

        Assert.Equal("La cantidad debe ser mayor que cero.", exception.Message);
    }

    [Fact]
    public void AddItem_WithInvalidPrice_ShouldThrowDomainException()
    {
        var cart = new Cart();

        var exception = Assert.Throws<DomainException>(() =>
            cart.AddItem(
                Guid.NewGuid(),
                "Producto inválido",
                0m,
                1
            )
        );

        Assert.Equal("El precio unitario debe ser mayor que cero.", exception.Message);
    }

    [Fact]
    public void RemoveItem_WhenProductExists_ShouldRemoveItem()
    {
        var cart = new Cart();
        var productId = Guid.NewGuid();

        cart.AddItem(productId, "Teclado Mecánico", 100m, 1);

        cart.RemoveItem(productId);

        Assert.Empty(cart.Items);
        Assert.True(cart.IsEmpty);
        Assert.Equal(0, cart.TotalItems);
        Assert.Equal(0, cart.Total);
        Assert.NotNull(cart.UpdatedAt);
    }

    [Fact]
    public void RemoveItem_WhenProductDoesNotExist_ShouldThrowDomainException()
    {
        var cart = new Cart();

        var exception = Assert.Throws<DomainException>(() =>
            cart.RemoveItem(Guid.NewGuid())
        );

        Assert.Equal("El producto no existe en el carrito.", exception.Message);
    }

    [Fact]
    public void ChangeItemQuantity_WhenProductExists_ShouldChangeQuantity()
    {
        var cart = new Cart();
        var productId = Guid.NewGuid();

        cart.AddItem(productId, "Monitor", 300m, 1);

        cart.ChangeItemQuantity(productId, 4);

        var item = cart.Items.First();

        Assert.Equal(4, item.Quantity);
        Assert.Equal(4, cart.TotalItems);
        Assert.Equal(1200m, cart.Total);
        Assert.NotNull(cart.UpdatedAt);
    }

    [Fact]
    public void ChangeItemQuantity_WithInvalidQuantity_ShouldThrowDomainException()
    {
        var cart = new Cart();
        var productId = Guid.NewGuid();

        cart.AddItem(productId, "Monitor", 300m, 1);

        var exception = Assert.Throws<DomainException>(() =>
            cart.ChangeItemQuantity(productId, 0)
        );

        Assert.Equal("La cantidad debe ser mayor que cero.", exception.Message);
    }

    [Fact]
    public void ChangeItemQuantity_WhenProductDoesNotExist_ShouldThrowDomainException()
    {
        var cart = new Cart();

        var exception = Assert.Throws<DomainException>(() =>
            cart.ChangeItemQuantity(Guid.NewGuid(), 2)
        );

        Assert.Equal("El producto no existe en el carrito.", exception.Message);
    }

    [Fact]
    public void Clear_WhenCartHasItems_ShouldRemoveAllItems()
    {
        var cart = new Cart();

        cart.AddItem(Guid.NewGuid(), "Producto 1", 100m, 2);
        cart.AddItem(Guid.NewGuid(), "Producto 2", 50m, 3);

        cart.Clear();

        Assert.Empty(cart.Items);
        Assert.True(cart.IsEmpty);
        Assert.Equal(0, cart.TotalItems);
        Assert.Equal(0, cart.Total);
        Assert.NotNull(cart.UpdatedAt);
    }
}