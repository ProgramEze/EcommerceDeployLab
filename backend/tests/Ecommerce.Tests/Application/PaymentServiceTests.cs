using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Tests.Application;

public class PaymentServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenOrderExistsAndIsConfirmed_ShouldCreatePendingPayment()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var order = CreateConfirmedOrder();

        orderRepository.Seed(order);

        var result = await service.CreateAsync(new CreatePaymentDto
        {
            OrderId = order.Id,
            Method = PaymentMethod.CreditCard,
            ExternalReference = "EXT-123"
        });

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(order.Id, result.OrderId);
        Assert.Equal(order.Total, result.Amount);
        Assert.Equal(PaymentMethod.CreditCard, result.Method);
        Assert.Equal(PaymentStatus.Pending, result.Status);
        Assert.Equal("EXT-123", result.ExternalReference);
        Assert.Null(result.ApprovedAt);
        Assert.Null(result.RejectedAt);
        Assert.Null(result.CancelledAt);
    }

    [Fact]
    public async Task CreateAsync_WhenOrderDoesNotExist_ShouldThrowDomainException()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.CreateAsync(new CreatePaymentDto
            {
                OrderId = Guid.NewGuid(),
                Method = PaymentMethod.CreditCard
            })
        );

        Assert.Equal("La orden no existe.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenOrderIsNotConfirmed_ShouldThrowDomainException()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var order = CreatePendingOrder();

        orderRepository.Seed(order);

        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            service.CreateAsync(new CreatePaymentDto
            {
                OrderId = order.Id,
                Method = PaymentMethod.CreditCard
            })
        );

        Assert.Equal("Solo se puede registrar un pago para una orden confirmada.", exception.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPaymentExists_ShouldReturnPayment()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var payment = CreatePayment();

        paymentRepository.Seed(payment);

        var result = await service.GetByIdAsync(payment.Id);

        Assert.NotNull(result);
        Assert.Equal(payment.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPaymentDoesNotExist_ShouldReturnNull()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_WhenPaymentsExist_ShouldReturnPayments()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        paymentRepository.Seed(CreatePayment());
        paymentRepository.Seed(CreatePayment());

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByOrderIdAsync_WhenPaymentsExist_ShouldReturnPaymentsForOrder()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var orderId = Guid.NewGuid();

        paymentRepository.Seed(new Payment(orderId, 100m, PaymentMethod.CreditCard));
        paymentRepository.Seed(new Payment(orderId, 200m, PaymentMethod.DebitCard));
        paymentRepository.Seed(new Payment(Guid.NewGuid(), 300m, PaymentMethod.Cash));

        var result = await service.GetByOrderIdAsync(orderId);

        Assert.Equal(2, result.Count);
        Assert.All(result, payment => Assert.Equal(orderId, payment.OrderId));
    }

    [Fact]
    public async Task ApproveAsync_WhenPaymentExists_ShouldApprovePayment()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var payment = CreatePayment();

        paymentRepository.Seed(payment);

        var result = await service.ApproveAsync(payment.Id);

        Assert.NotNull(result);
        Assert.Equal(PaymentStatus.Approved, result.Status);
        Assert.NotNull(result.ApprovedAt);
    }

    [Fact]
    public async Task ApproveAsync_WhenPaymentDoesNotExist_ShouldReturnNull()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var result = await service.ApproveAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task RejectAsync_WhenPaymentExists_ShouldRejectPayment()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var payment = CreatePayment();

        paymentRepository.Seed(payment);

        var result = await service.RejectAsync(payment.Id);

        Assert.NotNull(result);
        Assert.Equal(PaymentStatus.Rejected, result.Status);
        Assert.NotNull(result.RejectedAt);
    }

    [Fact]
    public async Task RejectAsync_WhenPaymentDoesNotExist_ShouldReturnNull()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var result = await service.RejectAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task CancelAsync_WhenPaymentExists_ShouldCancelPayment()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var payment = CreatePayment();

        paymentRepository.Seed(payment);

        var result = await service.CancelAsync(payment.Id);

        Assert.NotNull(result);
        Assert.Equal(PaymentStatus.Cancelled, result.Status);
        Assert.NotNull(result.CancelledAt);
    }

    [Fact]
    public async Task CancelAsync_WhenPaymentDoesNotExist_ShouldReturnNull()
    {
        var paymentRepository = new FakePaymentRepository();
        var orderRepository = new FakeOrderRepository();
        var service = CreateService(paymentRepository, orderRepository);

        var result = await service.CancelAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    private static PaymentService CreateService(
        FakePaymentRepository paymentRepository,
        FakeOrderRepository orderRepository
    )
    {
        return new PaymentService(paymentRepository, orderRepository);
    }

    private static Order CreatePendingOrder()
    {
        return new Order(
            Guid.NewGuid(),
            "Cliente",
            "cliente@example.com",
            new List<OrderItem>
            {
                new OrderItem(
                    Guid.NewGuid(),
                    "Mouse Logitech",
                    50m,
                    2
                )
            }
        );
    }

    private static Order CreateConfirmedOrder()
    {
        var order = CreatePendingOrder();

        order.Confirm();

        return order;
    }

    private static Payment CreatePayment()
    {
        return new Payment(
            Guid.NewGuid(),
            100m,
            PaymentMethod.CreditCard
        );
    }

    private class FakePaymentRepository : IPaymentRepository
    {
        private readonly List<Payment> _payments = new();

        public void Seed(Payment payment)
        {
            _payments.Add(payment);
        }

        public Task<IReadOnlyList<Payment>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Payment>>(_payments);
        }

        public Task<Payment?> GetByIdAsync(Guid id)
        {
            var payment = _payments.FirstOrDefault(payment => payment.Id == id);

            return Task.FromResult(payment);
        }

        public Task<IReadOnlyList<Payment>> GetByOrderIdAsync(Guid orderId)
        {
            var payments = _payments
                .Where(payment => payment.OrderId == orderId)
                .ToList();

            return Task.FromResult<IReadOnlyList<Payment>>(payments);
        }

        public Task AddAsync(Payment payment)
        {
            _payments.Add(payment);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Payment payment)
        {
            return Task.CompletedTask;
        }
    }

    private class FakeOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public void Seed(Order order)
        {
            _orders.Add(order);
        }

        public Task<IReadOnlyList<Order>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Order>>(_orders);
        }

        public Task<Order?> GetByIdAsync(Guid id)
        {
            var order = _orders.FirstOrDefault(order => order.Id == id);

            return Task.FromResult(order);
        }

        public Task AddAsync(Order order)
        {
            _orders.Add(order);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order order)
        {
            return Task.CompletedTask;
        }
    }
}