using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(item => item.Id);

        builder.Property<Guid>("OrderId")
            .IsRequired();

        builder.Property(item => item.ProductId)
            .IsRequired();

        builder.Property(item => item.ProductName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(item => item.UnitPrice)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Ignore(item => item.Subtotal);

        builder.HasIndex("OrderId");
        builder.HasIndex(item => item.ProductId);
    }
}