using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Tests.Application;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateProduct()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var dto = new CreateProductDto
        {
            Name = "Notebook",
            Description = "Notebook para trabajo.",
            Price = 1200m,
            Stock = 5,
            ImageUrl = "https://example.com/notebook.jpg",
        };

        var result = await service.CreateAsync(dto);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Notebook", result.Name);
        Assert.Equal("Notebook para trabajo.", result.Description);
        Assert.Equal(1200m, result.Price);
        Assert.Equal(5, result.Stock);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetAllAsync_WhenProductsExist_ShouldReturnProducts()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        await service.CreateAsync(
            new CreateProductDto
            {
                Name = "Mouse",
                Description = "Mouse inalámbrico.",
                Price = 50m,
                Stock = 10,
            }
        );

        await service.CreateAsync(
            new CreateProductDto
            {
                Name = "Teclado",
                Description = "Teclado mecánico.",
                Price = 100m,
                Stock = 7,
            }
        );

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ShouldReturnProduct()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var created = await service.CreateAsync(
            new CreateProductDto
            {
                Name = "Monitor",
                Description = "Monitor 24 pulgadas.",
                Price = 300m,
                Stock = 4,
            }
        );

        var result = await service.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Monitor", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductExists_ShouldUpdateProduct()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var created = await service.CreateAsync(
            new CreateProductDto
            {
                Name = "Producto viejo",
                Description = "Descripción vieja.",
                Price = 100m,
                Stock = 3,
            }
        );

        var updated = await service.UpdateAsync(
            created.Id,
            new UpdateProductDto
            {
                Name = "Producto nuevo",
                Description = "Descripción nueva.",
                Price = 150m,
                ImageUrl = "https://example.com/new.jpg",
            }
        );

        Assert.NotNull(updated);
        Assert.Equal("Producto nuevo", updated.Name);
        Assert.Equal("Descripción nueva.", updated.Description);
        Assert.Equal(150m, updated.Price);
        Assert.Equal("https://example.com/new.jpg", updated.ImageUrl);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var result = await service.UpdateAsync(
            Guid.NewGuid(),
            new UpdateProductDto
            {
                Name = "No existe",
                Description = "No existe.",
                Price = 100m,
            }
        );

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_ShouldReturnTrue()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var created = await service.CreateAsync(
            new CreateProductDto
            {
                Name = "Producto a borrar",
                Description = "Producto temporal.",
                Price = 10m,
                Stock = 1,
            }
        );

        var result = await service.DeleteAsync(created.Id);

        Assert.True(result);

        var deleted = await service.GetByIdAsync(created.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductDoesNotExist_ShouldReturnFalse()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var result = await service.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task IncreaseStockAsync_WhenProductExists_ShouldIncreaseStock()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var created = await service.CreateAsync(new CreateProductDto
        {
            Name = "Mouse",
            Description = "Mouse inalámbrico.",
            Price = 50m,
            Stock = 10
        });

        var result = await service.IncreaseStockAsync(
            created.Id,
            new UpdateStockDto
            {
                Quantity = 5
            }
        );

        Assert.NotNull(result);
        Assert.Equal(15, result.Stock);
    }

    [Fact]
    public async Task DecreaseStockAsync_WhenProductExists_ShouldDecreaseStock()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var created = await service.CreateAsync(new CreateProductDto
        {
            Name = "Teclado",
            Description = "Teclado mecánico.",
            Price = 100m,
            Stock = 10
        });

        var result = await service.DecreaseStockAsync(
            created.Id,
            new UpdateStockDto
            {
                Quantity = 3
            }
        );

        Assert.NotNull(result);
        Assert.Equal(7, result.Stock);
    }

    [Fact]
    public async Task IncreaseStockAsync_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var result = await service.IncreaseStockAsync(
            Guid.NewGuid(),
            new UpdateStockDto
            {
                Quantity = 5
            }
        );

        Assert.Null(result);
    }

    [Fact]
    public async Task DecreaseStockAsync_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var result = await service.DecreaseStockAsync(
            Guid.NewGuid(),
            new UpdateStockDto
            {
                Quantity = 5
            }
        );

        Assert.Null(result);
    }

    private class FakeProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        public Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Product>>(_products);
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

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
