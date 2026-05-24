using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Tests.Application;

public class CartServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEmptyCart()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

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
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var created = await service.CreateAsync();

        var result = await service.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCartDoesNotExist_ShouldReturnNull()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task AddItemAsync_WhenCartExists_ShouldAddItemUsingProductFromCatalog()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var product = CreateProduct(
            name: "Notebook Lenovo",
            description: "Notebook para trabajo.",
            price: 1200m,
            stock: 10
        );

        productRepository.Seed(product);

        var cart = await service.CreateAsync();

        var result = await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = product.Id,
                Quantity = 2
            }
        );

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(2, result.TotalItems);
        Assert.Equal(2400m, result.Total);

        var item = result.Items.First();

        Assert.Equal(product.Id, item.ProductId);
        Assert.Equal("Notebook Lenovo", item.ProductName);
        Assert.Equal(1200m, item.UnitPrice);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(2400m, item.Subtotal);
    }

    [Fact]
    public async Task AddItemAsync_WhenCartDoesNotExist_ShouldReturnNull()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var product = CreateProduct();

        productRepository.Seed(product);

        var result = await service.AddItemAsync(
            Guid.NewGuid(),
            new AddCartItemDto
            {
                ProductId = product.Id,
                Quantity = 1
            }
        );

        Assert.Null(result);
    }

    [Fact]
    public async Task AddItemAsync_WhenProductAlreadyExists_ShouldIncreaseQuantity()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var product = CreateProduct(
            name: "Mouse Logitech",
            description: "Mouse inalámbrico.",
            price: 50m,
            stock: 10
        );

        productRepository.Seed(product);

        var cart = await service.CreateAsync();

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = product.Id,
                Quantity = 2
            }
        );

        var result = await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = product.Id,
                Quantity = 3
            }
        );

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(250m, result.Total);

        var item = result.Items.First();

        Assert.Equal(product.Id, item.ProductId);
        Assert.Equal("Mouse Logitech", item.ProductName);
        Assert.Equal(50m, item.UnitPrice);
        Assert.Equal(5, item.Quantity);
        Assert.Equal(250m, item.Subtotal);
    }

    [Fact]
    public async Task AddItemAsync_WhenProductDoesNotExist_ShouldThrowDomainException()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var cart = await service.CreateAsync();

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.AddItemAsync(
                cart.Id,
                new AddCartItemDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 1
                }
            )
        );

        Assert.Equal("El producto no existe.", exception.Message);
    }

    [Fact]
    public async Task AddItemAsync_WhenProductIsInactive_ShouldThrowDomainException()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var product = CreateProduct();

        product.Deactivate();

        productRepository.Seed(product);

        var cart = await service.CreateAsync();

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.AddItemAsync(
                cart.Id,
                new AddCartItemDto
                {
                    ProductId = product.Id,
                    Quantity = 1
                }
            )
        );

        Assert.Equal("El producto no está activo.", exception.Message);
    }

    [Fact]
    public async Task AddItemAsync_WhenQuantityExceedsStock_ShouldThrowDomainException()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var product = CreateProduct(stock: 3);

        productRepository.Seed(product);

        var cart = await service.CreateAsync();

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.AddItemAsync(
                cart.Id,
                new AddCartItemDto
                {
                    ProductId = product.Id,
                    Quantity = 4
                }
            )
        );

        Assert.Equal("No hay stock suficiente para agregar esa cantidad al carrito.", exception.Message);
    }

    [Fact]
    public async Task AddItemAsync_WhenProductAlreadyInCartAndTotalQuantityExceedsStock_ShouldThrowDomainException()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var product = CreateProduct(stock: 5);

        productRepository.Seed(product);

        var cart = await service.CreateAsync();

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = product.Id,
                Quantity = 3
            }
        );

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.AddItemAsync(
                cart.Id,
                new AddCartItemDto
                {
                    ProductId = product.Id,
                    Quantity = 3
                }
            )
        );

        Assert.Equal("No hay stock suficiente para agregar esa cantidad al carrito.", exception.Message);
    }

    [Fact]
    public async Task ChangeItemQuantityAsync_WhenCartAndProductExist_ShouldChangeQuantity()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var product = CreateProduct(
            name: "Teclado Mecánico",
            description: "Teclado mecánico.",
            price: 100m,
            stock: 10
        );

        productRepository.Seed(product);

        var cart = await service.CreateAsync();

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = product.Id,
                Quantity = 1
            }
        );

        var result = await service.ChangeItemQuantityAsync(
            cart.Id,
            product.Id,
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
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

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
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var product = CreateProduct(
            name: "Monitor",
            description: "Monitor 24 pulgadas.",
            price: 300m,
            stock: 10
        );

        productRepository.Seed(product);

        var cart = await service.CreateAsync();

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = product.Id,
                Quantity = 1
            }
        );

        var result = await service.RemoveItemAsync(cart.Id, product.Id);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.True(result.IsEmpty);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(0, result.Total);
    }

    [Fact]
    public async Task RemoveItemAsync_WhenCartDoesNotExist_ShouldReturnNull()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var result = await service.RemoveItemAsync(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        Assert.Null(result);
    }

    [Fact]
    public async Task ClearAsync_WhenCartExists_ShouldRemoveAllItems()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var product1 = CreateProduct(
            name: "Producto 1",
            description: "Producto 1.",
            price: 100m,
            stock: 10
        );

        var product2 = CreateProduct(
            name: "Producto 2",
            description: "Producto 2.",
            price: 50m,
            stock: 10
        );

        productRepository.Seed(product1);
        productRepository.Seed(product2);

        var cart = await service.CreateAsync();

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = product1.Id,
                Quantity = 2
            }
        );

        await service.AddItemAsync(
            cart.Id,
            new AddCartItemDto
            {
                ProductId = product2.Id,
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
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var result = await service.ClearAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenCartExists_ShouldReturnTrue()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var cart = await service.CreateAsync();

        var result = await service.DeleteAsync(cart.Id);

        Assert.True(result);

        var deleted = await service.GetByIdAsync(cart.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteAsync_WhenCartDoesNotExist_ShouldReturnFalse()
    {
        var cartRepository = new FakeCartRepository();
        var productRepository = new FakeProductRepository();
        var service = CreateService(cartRepository, productRepository);

        var result = await service.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }

    private static CartService CreateService(
        FakeCartRepository cartRepository,
        FakeProductRepository productRepository
    )
    {
        return new CartService(cartRepository, productRepository);
    }

    private static Product CreateProduct(
        string name = "Mouse Logitech",
        string description = "Mouse inalámbrico.",
        decimal price = 50m,
        int stock = 10
    )
    {
        return new Product(
            name,
            description,
            price,
            stock
        );
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
}