using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Tests.Domain;

public class PaymentTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreatePendingPayment()
    {
        var orderId = Guid.NewGuid();

        var payment = new Payment(
            orderId,
            1500m,
            PaymentMethod.CreditCard,
            "EXT-123"
        );

        Assert.NotEqual(Guid.Empty, payment.Id);
        Assert.Equal(orderId, payment.OrderId);
        Assert.Equal(1500m, payment.Amount);
        Assert.Equal(PaymentMethod.CreditCard, payment.Method);
        Assert.Equal(PaymentStatus.Pending, payment.Status);
        Assert.Equal("EXT-123", payment.ExternalReference);
        Assert.True(payment.CreatedAt <= DateTime.UtcNow);
        Assert.Null(payment.ApprovedAt);
        Assert.Null(payment.RejectedAt);
        Assert.Null(payment.CancelledAt);
    }

    [Fact]
    public void Constructor_WithoutExternalReference_ShouldCreatePaymentWithNullReference()
    {
        var payment = new Payment(
            Guid.NewGuid(),
            100m,
            PaymentMethod.Cash
        );

        Assert.Null(payment.ExternalReference);
    }

    [Fact]
    public void Constructor_WithEmptyOrderId_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Payment(
                Guid.Empty,
                100m,
                PaymentMethod.CreditCard
            )
        );

        Assert.Equal("El identificador de la orden es obligatorio.", exception.Message);
    }

    [Fact]
    public void Constructor_WithInvalidAmount_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Payment(
                Guid.NewGuid(),
                0m,
                PaymentMethod.CreditCard
            )
        );

        Assert.Equal("El importe del pago debe ser mayor que cero.", exception.Message);
    }

    [Fact]
    public void Constructor_WithInvalidPaymentMethod_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Payment(
                Guid.NewGuid(),
                100m,
                (PaymentMethod)999
            )
        );

        Assert.Equal("El método de pago no es válido.", exception.Message);
    }

    [Fact]
    public void Approve_WhenPaymentIsPending_ShouldApprovePayment()
    {
        var payment = CreatePayment();

        payment.Approve();

        Assert.Equal(PaymentStatus.Approved, payment.Status);
        Assert.NotNull(payment.ApprovedAt);
        Assert.Null(payment.RejectedAt);
        Assert.Null(payment.CancelledAt);
    }

    [Fact]
    public void Approve_WhenPaymentIsAlreadyApproved_ShouldThrowDomainException()
    {
        var payment = CreatePayment();

        payment.Approve();

        var exception = Assert.Throws<DomainException>(() =>
            payment.Approve()
        );

        Assert.Equal("El pago ya está aprobado.", exception.Message);
    }

    [Fact]
    public void Approve_WhenPaymentIsRejected_ShouldThrowDomainException()
    {
        var payment = CreatePayment();

        payment.Reject();

        var exception = Assert.Throws<DomainException>(() =>
            payment.Approve()
        );

        Assert.Equal("No se puede aprobar un pago rechazado.", exception.Message);
    }

    [Fact]
    public void Approve_WhenPaymentIsCancelled_ShouldThrowDomainException()
    {
        var payment = CreatePayment();

        payment.Cancel();

        var exception = Assert.Throws<DomainException>(() =>
            payment.Approve()
        );

        Assert.Equal("No se puede aprobar un pago cancelado.", exception.Message);
    }

    [Fact]
    public void Reject_WhenPaymentIsPending_ShouldRejectPayment()
    {
        var payment = CreatePayment();

        payment.Reject();

        Assert.Equal(PaymentStatus.Rejected, payment.Status);
        Assert.NotNull(payment.RejectedAt);
        Assert.Null(payment.ApprovedAt);
        Assert.Null(payment.CancelledAt);
    }

    [Fact]
    public void Reject_WhenPaymentIsApproved_ShouldThrowDomainException()
    {
        var payment = CreatePayment();

        payment.Approve();

        var exception = Assert.Throws<DomainException>(() =>
            payment.Reject()
        );

        Assert.Equal("No se puede rechazar un pago aprobado.", exception.Message);
    }

    [Fact]
    public void Reject_WhenPaymentIsAlreadyRejected_ShouldThrowDomainException()
    {
        var payment = CreatePayment();

        payment.Reject();

        var exception = Assert.Throws<DomainException>(() =>
            payment.Reject()
        );

        Assert.Equal("El pago ya está rechazado.", exception.Message);
    }

    [Fact]
    public void Reject_WhenPaymentIsCancelled_ShouldThrowDomainException()
    {
        var payment = CreatePayment();

        payment.Cancel();

        var exception = Assert.Throws<DomainException>(() =>
            payment.Reject()
        );

        Assert.Equal("No se puede rechazar un pago cancelado.", exception.Message);
    }

    [Fact]
    public void Cancel_WhenPaymentIsPending_ShouldCancelPayment()
    {
        var payment = CreatePayment();

        payment.Cancel();

        Assert.Equal(PaymentStatus.Cancelled, payment.Status);
        Assert.NotNull(payment.CancelledAt);
        Assert.Null(payment.ApprovedAt);
        Assert.Null(payment.RejectedAt);
    }

    [Fact]
    public void Cancel_WhenPaymentIsApproved_ShouldThrowDomainException()
    {
        var payment = CreatePayment();

        payment.Approve();

        var exception = Assert.Throws<DomainException>(() =>
            payment.Cancel()
        );

        Assert.Equal("No se puede cancelar un pago aprobado.", exception.Message);
    }

    [Fact]
    public void Cancel_WhenPaymentIsRejected_ShouldThrowDomainException()
    {
        var payment = CreatePayment();

        payment.Reject();

        var exception = Assert.Throws<DomainException>(() =>
            payment.Cancel()
        );

        Assert.Equal("No se puede cancelar un pago rechazado.", exception.Message);
    }

    [Fact]
    public void Cancel_WhenPaymentIsAlreadyCancelled_ShouldThrowDomainException()
    {
        var payment = CreatePayment();

        payment.Cancel();

        var exception = Assert.Throws<DomainException>(() =>
            payment.Cancel()
        );

        Assert.Equal("El pago ya está cancelado.", exception.Message);
    }

    private static Payment CreatePayment()
    {
        return new Payment(
            Guid.NewGuid(),
            100m,
            PaymentMethod.CreditCard
        );
    }
}