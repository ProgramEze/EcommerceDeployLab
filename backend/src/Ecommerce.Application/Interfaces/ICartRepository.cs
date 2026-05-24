using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(Guid id);
    Task AddAsync(Cart cart);
    Task UpdateAsync(Cart cart);
    Task DeleteAsync(Cart cart);
}