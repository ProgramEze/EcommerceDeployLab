using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Tests.Domain;

public class OrderTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreatePendingOrder()
    {
        var cartId = Guid.NewGuid();

        var items = new List<OrderItem>
        {
            new OrderItem(Guid.NewGuid(), "Mouse Logitech", 50m, 2),
            new OrderItem(Guid.NewGuid(), "Teclado Mecánico", 100m, 1)
        };

        var order = new Order(
            cartId,
            "Ezequiel Díaz",
            "ezequiel@example.com",
            items
        );

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(cartId, order.CartId);
        Assert.Equal("Ezequiel Díaz", order.CustomerName);
        Assert.Equal("ezequiel@example.com", order.CustomerEmail);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Equal(2, order.Items.Count);
        Assert.Equal(3, order.TotalItems);
        Assert.Equal(200m, order.Total);
        Assert.True(order.CreatedAt <= DateTime.UtcNow);
        Assert.Null(order.ConfirmedAt);
        Assert.Null(order.CancelledAt);
    }

    [Fact]
    public void Constructor_WithEmptyCartId_ShouldThrowDomainException()
    {
        var items = CreateValidItems();

        var exception = Assert.Throws<DomainException>(() =>
            new Order(
                Guid.Empty,
                "Cliente",
                "cliente@example.com",
                items
            )
        );

        Assert.Equal("El identificador del carrito es obligatorio.", exception.Message);
    }

    [Fact]
    public void Constructor_WithEmptyCustomerName_ShouldThrowDomainException()
    {
        var items = CreateValidItems();

        var exception = Assert.Throws<DomainException>(() =>
            new Order(
                Guid.NewGuid(),
                "",
                "cliente@example.com",
                items
            )
        );

        Assert.Equal("El nombre del cliente es obligatorio.", exception.Message);
    }

    [Fact]
    public void Constructor_WithInvalidEmail_ShouldThrowDomainException()
    {
        var items = CreateValidItems();

        var exception = Assert.Throws<DomainException>(() =>
            new Order(
                Guid.NewGuid(),
                "Cliente",
                "email-invalido",
                items
            )
        );

        Assert.Equal("El email del cliente no tiene un formato válido.", exception.Message);
    }

    [Fact]
    public void Constructor_WithEmptyItems_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Order(
                Guid.NewGuid(),
                "Cliente",
                "cliente@example.com",
                new List<OrderItem>()
            )
        );

        Assert.Equal("La orden debe tener al menos un producto.", exception.Message);
    }

    [Fact]
    public void Confirm_WhenOrderIsPending_ShouldConfirmOrder()
    {
        var order = CreateValidOrder();

        order.Confirm();

        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.NotNull(order.ConfirmedAt);
        Assert.Null(order.CancelledAt);
    }

    [Fact]
    public void Confirm_WhenOrderIsCancelled_ShouldThrowDomainException()
    {
        var order = CreateValidOrder();

        order.Cancel();

        var exception = Assert.Throws<DomainException>(() =>
            order.Confirm()
        );

        Assert.Equal("No se puede confirmar una orden cancelada.", exception.Message);
    }

    [Fact]
    public void Confirm_WhenOrderIsAlreadyConfirmed_ShouldThrowDomainException()
    {
        var order = CreateValidOrder();

        order.Confirm();

        var exception = Assert.Throws<DomainException>(() =>
            order.Confirm()
        );

        Assert.Equal("La orden ya está confirmada.", exception.Message);
    }

    [Fact]
    public void Cancel_WhenOrderIsPending_ShouldCancelOrder()
    {
        var order = CreateValidOrder();

        order.Cancel();

        Assert.Equal(OrderStatus.Cancelled, order.Status);
        Assert.NotNull(order.CancelledAt);
        Assert.Null(order.ConfirmedAt);
    }

    [Fact]
    public void Cancel_WhenOrderIsConfirmed_ShouldThrowDomainException()
    {
        var order = CreateValidOrder();

        order.Confirm();

        var exception = Assert.Throws<DomainException>(() =>
            order.Cancel()
        );

        Assert.Equal("No se puede cancelar una orden confirmada.", exception.Message);
    }

    [Fact]
    public void Cancel_WhenOrderIsAlreadyCancelled_ShouldThrowDomainException()
    {
        var order = CreateValidOrder();

        order.Cancel();

        var exception = Assert.Throws<DomainException>(() =>
            order.Cancel()
        );

        Assert.Equal("La orden ya está cancelada.", exception.Message);
    }

    private static Order CreateValidOrder()
    {
        return new Order(
            Guid.NewGuid(),
            "Cliente",
            "cliente@example.com",
            CreateValidItems()
        );
    }

    private static List<OrderItem> CreateValidItems()
    {
        return new List<OrderItem>
        {
            new OrderItem(Guid.NewGuid(), "Mouse Logitech", 50m, 2)
        };
    }
}