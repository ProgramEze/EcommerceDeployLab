using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Tests.Application;

public class CartServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEmptyCart()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var result = await service.CreateAsync();

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Empty(result.Items);
        Assert.True(result.IsEmpty);
        Assert.Equal(0, result.Total);
        Assert.Equal(0, result.TotalItems);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCartExists_ShouldReturnCart()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var created = await service.CreateAsync();

        var result = await service.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCartDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task AddItemAsync_WhenCartExists_ShouldAddItem()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var cart = await service.CreateAsync();
        var productId = Guid.NewGuid();

        var result = await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = productId,
                ProductName = "Notebook Lenovo",
                UnitPrice = 1200m,
                Quantity = 2
            }
        );

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(2, result.TotalItems);
        Assert.Equal(2400m, result.Total);

        var item = result.Items.First();

        Assert.Equal(productId, item.ProductId);
        Assert.Equal("Notebook Lenovo", item.ProductName);
        Assert.Equal(1200m, item.UnitPrice);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(2400m, item.Subtotal);
    }

    [Fact]
    public async Task AddItemAsync_WhenCartDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var result = await service.AddItemAsync(
            Guid.NewGuid(),
            new AddCartItemDto
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Mouse Logitech",
                UnitPrice = 50m,
                Quantity = 1
            }
        );

        Assert.Null(result);
    }

    [Fact]
    public async Task AddItemAsync_WhenProductAlreadyExists_ShouldIncreaseQuantity()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var cart = await service.CreateAsync();
        var productId = Guid.NewGuid();

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = productId,
                ProductName = "Mouse Logitech",
                UnitPrice = 50m,
                Quantity = 2
            }
        );

        var result = await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = productId,
                ProductName = "Mouse Logitech",
                UnitPrice = 50m,
                Quantity = 3
            }
        );

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(250m, result.Total);
    }

    [Fact]
    public async Task ChangeItemQuantityAsync_WhenCartAndProductExist_ShouldChangeQuantity()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var cart = await service.CreateAsync();
        var productId = Guid.NewGuid();

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = productId,
                ProductName = "Teclado Mecánico",
                UnitPrice = 100m,
                Quantity = 1
            }
        );

        var result = await service.ChangeItemQuantityAsync(
            cart.Id,
            productId,
            new UpdateCartItemQuantityDto
            {
                Quantity = 4
            }
        );

        Assert.NotNull(result);
        Assert.Equal(4, result.TotalItems);
        Assert.Equal(400m, result.Total);
        Assert.Equal(4, result.Items.First().Quantity);
    }

    [Fact]
    public async Task ChangeItemQuantityAsync_WhenCartDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var result = await service.ChangeItemQuantityAsync(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new UpdateCartItemQuantityDto
            {
                Quantity = 2
            }
        );

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveItemAsync_WhenCartAndProductExist_ShouldRemoveItem()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var cart = await service.CreateAsync();
        var productId = Guid.NewGuid();

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = productId,
                ProductName = "Monitor",
                UnitPrice = 300m,
                Quantity = 1
            }
        );

        var result = await service.RemoveItemAsync(cart.Id, productId);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.True(result.IsEmpty);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(0, result.Total);
    }

    [Fact]
    public async Task RemoveItemAsync_WhenCartDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var result = await service.RemoveItemAsync(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        Assert.Null(result);
    }

    [Fact]
    public async Task ClearAsync_WhenCartExists_ShouldRemoveAllItems()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var cart = await service.CreateAsync();

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Producto 1",
                UnitPrice = 100m,
                Quantity = 2
            }
        );

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Producto 2",
                UnitPrice = 50m,
                Quantity = 3
            }
        );

        var result = await service.ClearAsync(cart.Id);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.True(result.IsEmpty);
        Assert.Equal(0, result.Total);
        Assert.Equal(0, result.TotalItems);
    }

    [Fact]
    public async Task ClearAsync_WhenCartDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var result = await service.ClearAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenCartExists_ShouldReturnTrue()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var cart = await service.CreateAsync();

        var result = await service.DeleteAsync(cart.Id);

        Assert.True(result);

        var deleted = await service.GetByIdAsync(cart.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteAsync_WhenCartDoesNotExist_ShouldReturnFalse()
    {
        var repository = new FakeCartRepository();
        var service = new CartService(repository);

        var result = await service.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }

    private class FakeCartRepository : ICartRepository
    {
        private readonly List<Cart> _carts = new();

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
}