using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository
    )
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyList<PaymentDto>> GetAllAsync()
    {
        var payments = await _paymentRepository.GetAllAsync();

        return payments
            .Select(MapToDto)
            .ToList();
    }

    public async Task<PaymentDto?> GetByIdAsync(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);

        if (payment is null)
        {
            return null;
        }

        return MapToDto(payment);
    }

    public async Task<IReadOnlyList<PaymentDto>> GetByOrderIdAsync(Guid orderId)
    {
        var payments = await _paymentRepository.GetByOrderIdAsync(orderId);

        return payments
            .Select(MapToDto)
            .ToList();
    }

    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(dto.OrderId);

        if (order is null)
        {
            throw new DomainException("La orden no existe.");
        }

        if (order.Status != OrderStatus.Confirmed)
        {
            throw new DomainException("Solo se puede registrar un pago para una orden confirmada.");
        }

        var payment = new Payment(
            order.Id,
            order.Total,
            dto.Method,
            dto.ExternalReference
        );

        await _paymentRepository.AddAsync(payment);

        return MapToDto(payment);
    }

    public async Task<PaymentDto?> ApproveAsync(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);

        if (payment is null)
        {
            return null;
        }

        payment.Approve();

        await _paymentRepository.UpdateAsync(payment);

        return MapToDto(payment);
    }

    public async Task<PaymentDto?> RejectAsync(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);

        if (payment is null)
        {
            return null;
        }

        payment.Reject();

        await _paymentRepository.UpdateAsync(payment);

        return MapToDto(payment);
    }

    public async Task<PaymentDto?> CancelAsync(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);

        if (payment is null)
        {
            return null;
        }

        payment.Cancel();

        await _paymentRepository.UpdateAsync(payment);

        return MapToDto(payment);
    }

    private static PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            Amount = payment.Amount,
            Method = payment.Method,
            Status = payment.Status,
            ExternalReference = payment.ExternalReference,
            CreatedAt = payment.CreatedAt,
            ApprovedAt = payment.ApprovedAt,
            RejectedAt = payment.RejectedAt,
            CancelledAt = payment.CancelledAt
        };
    }
}