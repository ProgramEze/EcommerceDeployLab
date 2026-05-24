using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? ExternalReference { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public DateTime? RejectedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    private Payment()
    {
    }

    public Payment(
        Guid orderId,
        decimal amount,
        PaymentMethod method,
        string? externalReference = null
    )
    {
        ValidateOrderId(orderId);
        ValidateAmount(amount);
        ValidatePaymentMethod(method);
        ValidateExternalReference(externalReference);

        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Method = method;
        Status = PaymentStatus.Pending;
        ExternalReference = string.IsNullOrWhiteSpace(externalReference)
            ? null
            : externalReference.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        if (Status == PaymentStatus.Approved)
        {
            throw new DomainException("El pago ya está aprobado.");
        }

        if (Status == PaymentStatus.Rejected)
        {
            throw new DomainException("No se puede aprobar un pago rechazado.");
        }

        if (Status == PaymentStatus.Cancelled)
        {
            throw new DomainException("No se puede aprobar un pago cancelado.");
        }

        Status = PaymentStatus.Approved;
        ApprovedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        if (Status == PaymentStatus.Approved)
        {
            throw new DomainException("No se puede rechazar un pago aprobado.");
        }

        if (Status == PaymentStatus.Rejected)
        {
            throw new DomainException("El pago ya está rechazado.");
        }

        if (Status == PaymentStatus.Cancelled)
        {
            throw new DomainException("No se puede rechazar un pago cancelado.");
        }

        Status = PaymentStatus.Rejected;
        RejectedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == PaymentStatus.Approved)
        {
            throw new DomainException("No se puede cancelar un pago aprobado.");
        }

        if (Status == PaymentStatus.Rejected)
        {
            throw new DomainException("No se puede cancelar un pago rechazado.");
        }

        if (Status == PaymentStatus.Cancelled)
        {
            throw new DomainException("El pago ya está cancelado.");
        }

        Status = PaymentStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
    }

    private static void ValidateOrderId(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new DomainException("El identificador de la orden es obligatorio.");
        }
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new DomainException("El importe del pago debe ser mayor que cero.");
        }
    }

    private static void ValidatePaymentMethod(PaymentMethod method)
    {
        if (!Enum.IsDefined(typeof(PaymentMethod), method))
        {
            throw new DomainException("El método de pago no es válido.");
        }
    }

    private static void ValidateExternalReference(string? externalReference)
    {
        if (externalReference is not null && externalReference.Trim().Length > 200)
        {
            throw new DomainException("La referencia externa no puede superar los 200 caracteres.");
        }
    }
}