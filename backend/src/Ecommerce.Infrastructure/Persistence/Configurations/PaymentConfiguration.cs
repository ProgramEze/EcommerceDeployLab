using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.OrderId)
            .IsRequired();

        builder.Property(payment => payment.Amount)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(payment => payment.Method)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(payment => payment.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(payment => payment.ExternalReference)
            .HasMaxLength(200);

        builder.Property(payment => payment.CreatedAt)
            .IsRequired();

        builder.Property(payment => payment.ApprovedAt);

        builder.Property(payment => payment.RejectedAt);

        builder.Property(payment => payment.CancelledAt);

        builder.HasIndex(payment => payment.OrderId);
        builder.HasIndex(payment => payment.Status);
        builder.HasIndex(payment => payment.Method);
    }
}