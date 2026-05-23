using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteAsync(Guid id);

    Task<ProductDto?> IncreaseStockAsync(Guid id, UpdateStockDto dto);
    Task<ProductDto?> DecreaseStockAsync(Guid id, UpdateStockDto dto);
}