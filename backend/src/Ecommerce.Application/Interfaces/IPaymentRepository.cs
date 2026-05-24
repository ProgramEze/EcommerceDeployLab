using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

public interface IPaymentRepository
{
    Task<IReadOnlyList<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Payment>> GetByOrderIdAsync(Guid orderId);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
}