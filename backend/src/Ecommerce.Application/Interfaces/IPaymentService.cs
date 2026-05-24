using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

public interface IPaymentService
{
    Task<IReadOnlyList<PaymentDto>> GetAllAsync();
    Task<PaymentDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<PaymentDto>> GetByOrderIdAsync(Guid orderId);
    Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    Task<PaymentDto?> ApproveAsync(Guid id);
    Task<PaymentDto?> RejectAsync(Guid id);
    Task<PaymentDto?> CancelAsync(Guid id);
}