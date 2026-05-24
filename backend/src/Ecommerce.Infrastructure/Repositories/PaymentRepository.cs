using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Payment>> GetAllAsync()
    {
        return await _context.Payments
            .AsNoTracking()
            .OrderByDescending(payment => payment.CreatedAt)
            .ToListAsync();
    }

    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(payment => payment.Id == id);
    }

    public async Task<IReadOnlyList<Payment>> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.Payments
            .AsNoTracking()
            .Where(payment => payment.OrderId == orderId)
            .OrderByDescending(payment => payment.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payment payment)
    {
        await _context.SaveChangesAsync();
    }
}