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
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        var cart = CreateCartWithItems();

        cartRepository.Seed(cart);

        var result = await service.CreateFromCartAsync(new CreateOrderDto
        {
            CartId = cart.Id,
            CustomerName = "Ezequiel Díaz",
            CustomerEmail = "diazezequiel777@gmail.com"
        });

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(cart.Id, result.CartId);
        Assert.Equal("Ezequiel Díaz", result.CustomerName);
        Assert.Equal("diazezequiel777@gmail.com", result.CustomerEmail);
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
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

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
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

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
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

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
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_WhenOrdersExist_ShouldReturnOrders()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        orderRepository.Seed(CreateOrder());
        orderRepository.Seed(CreateOrder());

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ConfirmAsync_WhenOrderExists_ShouldConfirmOrderAndDecreaseProductStock()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        var productId = Guid.NewGuid();

        var product = new Product(
            "Mouse Logitech",
            "Mouse inalámbrico.",
            50m,
            10
        );

        SetProductId(product, productId);

        var order = new Order(
            Guid.NewGuid(),
            "Cliente",
            "cliente@example.com",
            new List<OrderItem>
            {
                new OrderItem(
                    productId,
                    "Mouse Logitech",
                    50m,
                    2
                )
            }
        );

        productRepository.Seed(product);
        orderRepository.Seed(order);

        var result = await service.ConfirmAsync(order.Id);

        Assert.NotNull(result);
        Assert.Equal(OrderStatus.Confirmed, result.Status);
        Assert.NotNull(result.ConfirmedAt);
        Assert.Equal(8, product.Stock);
    }

    [Fact]
    public async Task ConfirmAsync_WhenOrderDoesNotExist_ShouldReturnNull()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        var result = await service.ConfirmAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task ConfirmAsync_WhenProductDoesNotExist_ShouldThrowDomainException()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        var order = CreateOrder();

        orderRepository.Seed(order);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.ConfirmAsync(order.Id)
        );

        Assert.Equal("Uno de los productos de la orden ya no existe.", exception.Message);
    }

    [Fact]
    public async Task ConfirmAsync_WhenProductIsInactive_ShouldThrowDomainException()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        var productId = Guid.NewGuid();

        var product = new Product(
            "Mouse Logitech",
            "Mouse inalámbrico.",
            50m,
            10
        );

        SetProductId(product, productId);
        product.Deactivate();

        var order = new Order(
            Guid.NewGuid(),
            "Cliente",
            "cliente@example.com",
            new List<OrderItem>
            {
                new OrderItem(
                    productId,
                    "Mouse Logitech",
                    50m,
                    2
                )
            }
        );

        productRepository.Seed(product);
        orderRepository.Seed(order);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.ConfirmAsync(order.Id)
        );

        Assert.Equal("Uno de los productos de la orden ya no está activo.", exception.Message);
    }

    [Fact]
    public async Task ConfirmAsync_WhenStockIsInsufficient_ShouldThrowDomainException()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        var productId = Guid.NewGuid();

        var product = new Product(
            "Mouse Logitech",
            "Mouse inalámbrico.",
            50m,
            1
        );

        SetProductId(product, productId);

        var order = new Order(
            Guid.NewGuid(),
            "Cliente",
            "cliente@example.com",
            new List<OrderItem>
            {
                new OrderItem(
                    productId,
                    "Mouse Logitech",
                    50m,
                    2
                )
            }
        );

        productRepository.Seed(product);
        orderRepository.Seed(order);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.ConfirmAsync(order.Id)
        );

        Assert.Equal("No hay stock suficiente para confirmar la orden.", exception.Message);
    }

    [Fact]
    public async Task ConfirmAsync_WhenOrderIsAlreadyConfirmed_ShouldThrowDomainExceptionAndNotDecreaseStockTwice()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        var productId = Guid.NewGuid();

        var product = new Product(
            "Mouse Logitech",
            "Mouse inalámbrico.",
            50m,
            10
        );

        SetProductId(product, productId);

        var order = new Order(
            Guid.NewGuid(),
            "Cliente",
            "cliente@example.com",
            new List<OrderItem>
            {
                new OrderItem(
                    productId,
                    "Mouse Logitech",
                    50m,
                    2
                )
            }
        );

        productRepository.Seed(product);
        orderRepository.Seed(order);

        await service.ConfirmAsync(order.Id);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.ConfirmAsync(order.Id)
        );

        Assert.Equal("La orden ya está confirmada.", exception.Message);
        Assert.Equal(8, product.Stock);
    }

    [Fact]
    public async Task CancelAsync_WhenOrderExists_ShouldCancelOrder()
    {
        var orderRepository = new FakeOrderRepository();
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

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
        var productRepository = new FakeProductRepository();
        var service = CreateService(orderRepository, cartRepository, productRepository);

        var result = await service.CancelAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    private static OrderService CreateService(
    FakeOrderRepository orderRepository,
    FakeCartRepository cartRepository,
    FakeProductRepository productRepository
)
    {
        return new OrderService(
            orderRepository,
            cartRepository,
            productRepository,
            new FakeUnitOfWork()
        );
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

    private static void SetProductId(Product product, Guid id)
    {
        typeof(Product)
            .GetProperty(nameof(Product.Id))!
            .SetValue(product, id);
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

    private class FakeProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        public void Seed(Product product)
        {
            _products.Add(product);
        }

        public Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Product>>(_products);
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            var product = _products.FirstOrDefault(product => product.Id == id);

            return Task.FromResult(product);
        }

        public Task AddAsync(Product product)
        {
            _products.Add(product);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Product product)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product)
        {
            _products.Remove(product);

            return Task.CompletedTask;
        }
    }

    private class FakeUnitOfWork : IUnitOfWork
    {
        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            return await action();
        }
    }
}