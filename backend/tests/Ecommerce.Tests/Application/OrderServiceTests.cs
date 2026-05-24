using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Tests.Application;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateFromCartAsync_WhenCartExistsAndHasItems_ShouldCreatePendingOrder()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        var cart = CreateCartWithItems();

        cartRepository.Seed(cart);

        var result = await service.CreateFromCartAsync(new CreateOrderDto
        {
            CartId = cart.Id,
            CustomerName = "Ezequiel Díaz",
            CustomerEmail = "ezequiel@example.com"
        });

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(cart.Id, result.CartId);
        Assert.Equal("Ezequiel Díaz", result.CustomerName);
        Assert.Equal("ezequiel@example.com", result.CustomerEmail);
        Assert.Equal(OrderStatus.Pending, result.Status);
        Assert.Single(result.Items);
        Assert.Equal(2, result.TotalItems);
        Assert.Equal(100m, result.Total);
    }

    [Fact]
    public async Task CreateFromCartAsync_WhenCartDoesNotExist_ShouldThrowDomainException()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.CreateFromCartAsync(new CreateOrderDto
            {
                CartId = Guid.NewGuid(),
                CustomerName = "Cliente",
                CustomerEmail = "cliente@example.com"
            })
        );

        Assert.Equal("El carrito no existe.", exception.Message);
    }

    [Fact]
    public async Task CreateFromCartAsync_WhenCartIsEmpty_ShouldThrowDomainException()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        var cart = new Cart();

        cartRepository.Seed(cart);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.CreateFromCartAsync(new CreateOrderDto
            {
                CartId = cart.Id,
                CustomerName = "Cliente",
                CustomerEmail = "cliente@example.com"
            })
        );

        Assert.Equal("No se puede crear una orden desde un carrito vacío.", exception.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenOrderExists_ShouldReturnOrder()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        var order = CreateOrder();

        orderRepository.Seed(order);

        var result = await service.GetByIdAsync(order.Id);

        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenOrderDoesNotExist_ShouldReturnNull()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_WhenOrdersExist_ShouldReturnOrders()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        orderRepository.Seed(CreateOrder());
        orderRepository.Seed(CreateOrder());

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ConfirmAsync_WhenOrderExists_ShouldConfirmOrder()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        var order = CreateOrder();

        orderRepository.Seed(order);

        var result = await service.ConfirmAsync(order.Id);

        Assert.NotNull(result);
        Assert.Equal(OrderStatus.Confirmed, result.Status);
        Assert.NotNull(result.ConfirmedAt);
    }

    [Fact]
    public async Task ConfirmAsync_WhenOrderDoesNotExist_ShouldReturnNull()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        var result = await service.ConfirmAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task CancelAsync_WhenOrderExists_ShouldCancelOrder()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        var order = CreateOrder();

        orderRepository.Seed(order);

        var result = await service.CancelAsync(order.Id);

        Assert.NotNull(result);
        Assert.Equal(OrderStatus.Cancelled, result.Status);
        Assert.NotNull(result.CancelledAt);
    }

    [Fact]
    public async Task CancelAsync_WhenOrderDoesNotExist_ShouldReturnNull()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var service = CreateService(orderRepository, cartRepository);

        var result = await service.CancelAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    private static OrderService CreateService(
        FakeOrderRepository orderRepository,
        FakeCartRepository cartRepository
    )
    {
        return new OrderService(orderRepository, cartRepository);
    }

    private static Cart CreateCartWithItems()
    {
        var cart = new Cart();

        cart.AddItem(
            Guid.NewGuid(),
            "Mouse Logitech",
            50m,
            2
        );

        return cart;
    }

    private static Order CreateOrder()
    {
        return new Order(
            Guid.NewGuid(),
            "Cliente",
            "cliente@example.com",
            new List<OrderItem>
            {
                new OrderItem(
                    Guid.NewGuid(),
                    "Mouse Logitech",
                    50m,
                    2
                )
            }
        );
    }

    private class FakeCartRepository : ICartRepository
    {
        private readonly List<Cart> _carts = new();

        public void Seed(Cart cart)
        {
            _carts.Add(cart);
        }

        public Task<Cart?> GetByIdAsync(Guid id)
        {
            var cart = _carts.FirstOrDefault(cart => cart.Id == id);

            return Task.FromResult(cart);
        }

        public Task AddAsync(Cart cart)
        {
            _carts.Add(cart);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Cart cart)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Cart cart)
        {
            _carts.Remove(cart);

            return Task.CompletedTask;
        }
    }

    private class FakeOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public void Seed(Order order)
        {
            _orders.Add(order);
        }

        public Task<IReadOnlyList<Order>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Order>>(_orders);
        }

        public Task<Order?> GetByIdAsync(Guid id)
        {
            var order = _orders.FirstOrDefault(order => order.Id == id);

            return Task.FromResult(order);
        }

        public Task AddAsync(Order order)
        {
            _orders.Add(order);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order order)
        {
            return Task.CompletedTask;
        }
    }
}