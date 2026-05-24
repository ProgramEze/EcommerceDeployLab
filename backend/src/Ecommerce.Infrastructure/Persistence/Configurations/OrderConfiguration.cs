using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.CartId)
            .IsRequired();

        builder.Property(order => order.CustomerName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(order => order.CustomerEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(order => order.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(order => order.CreatedAt)
            .IsRequired();

        builder.Property(order => order.ConfirmedAt);

        builder.Property(order => order.CancelledAt);

        builder.Ignore(order => order.Total);
        builder.Ignore(order => order.TotalItems);

        builder.HasMany(order => order.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(order => order.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(order => order.CartId);
        builder.HasIndex(order => order.CustomerEmail);
        builder.HasIndex(order => order.Status);
    }
}