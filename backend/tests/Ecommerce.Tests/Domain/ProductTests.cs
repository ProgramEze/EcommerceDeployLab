using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Tests.Domain;

public class ProductTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateProduct()
    {
        var product = new Product(
            "Notebook Gamer",
            "Notebook potente para gaming y trabajo.",
            1500m,
            10,
            "https://example.com/notebook.jpg"
        );

        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal("Notebook Gamer", product.Name);
        Assert.Equal("Notebook potente para gaming y trabajo.", product.Description);
        Assert.Equal(1500m, product.Price);
        Assert.Equal(10, product.Stock);
        Assert.Equal("https://example.com/notebook.jpg", product.ImageUrl);
        Assert.True(product.IsActive);
        Assert.True(product.CreatedAt <= DateTime.UtcNow);
        Assert.Null(product.UpdatedAt);
    }

    [Fact]
    public void Constructor_WithEmptyName_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Product("", "Producto sin nombre.", 100m, 5)
        );

        Assert.Equal("El nombre del producto es obligatorio.", exception.Message);
    }

    [Fact]
    public void Constructor_WithInvalidPrice_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Product("Mouse", "Mouse inalámbrico.", 0m, 5)
        );

        Assert.Equal("El precio del producto debe ser mayor que cero.", exception.Message);
    }

    [Fact]
    public void Constructor_WithNegativeStock_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Product("Teclado", "Teclado mecánico.", 100m, -1)
        );

        Assert.Equal("El stock del producto no puede ser negativo.", exception.Message);
    }

    [Fact]
    public void IncreaseStock_WithValidQuantity_ShouldIncreaseStock()
    {
        var product = new Product("Monitor", "Monitor 24 pulgadas.", 300m, 5);

        product.IncreaseStock(3);

        Assert.Equal(8, product.Stock);
        Assert.NotNull(product.UpdatedAt);
    }

    [Fact]
    public void DecreaseStock_WithValidQuantity_ShouldDecreaseStock()
    {
        var product = new Product("Auriculares", "Auriculares bluetooth.", 80m, 10);

        product.DecreaseStock(4);

        Assert.Equal(6, product.Stock);
        Assert.NotNull(product.UpdatedAt);
    }

    [Fact]
    public void DecreaseStock_WithQuantityGreaterThanStock_ShouldThrowDomainException()
    {
        var product = new Product("Webcam", "Webcam Full HD.", 120m, 2);

        var exception = Assert.Throws<DomainException>(() => product.DecreaseStock(3));

        Assert.Equal("No hay stock suficiente para realizar esta operación.", exception.Message);
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        var product = new Product("Tablet", "Tablet Android.", 250m, 4);

        product.Deactivate();

        Assert.False(product.IsActive);
        Assert.NotNull(product.UpdatedAt);
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrue()
    {
        var product = new Product("Smartphone", "Smartphone gama media.", 500m, 6);

        product.Deactivate();
        product.Activate();

        Assert.True(product.IsActive);
        Assert.NotNull(product.UpdatedAt);
    }

    [Fact]
    public void UpdateDetails_WithValidData_ShouldUpdateProduct()
    {
        var product = new Product("Producto viejo", "Descripción vieja.", 100m, 5);

        product.UpdateDetails(
            "Producto actualizado",
            "Descripción actualizada.",
            200m,
            "https://example.com/producto.jpg"
        );

        Assert.Equal("Producto actualizado", product.Name);
        Assert.Equal("Descripción actualizada.", product.Description);
        Assert.Equal(200m, product.Price);
        Assert.Equal("https://example.com/producto.jpg", product.ImageUrl);
        Assert.NotNull(product.UpdatedAt);
    }
}
