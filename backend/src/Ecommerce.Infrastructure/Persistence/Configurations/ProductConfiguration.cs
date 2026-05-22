using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name).IsRequired().HasMaxLength(100);

        builder.Property(product => product.Description).IsRequired().HasMaxLength(500);

        builder.Property(product => product.Price).IsRequired().HasColumnType("numeric(18,2)");

        builder.Property(product => product.Stock).IsRequired();

        builder.Property(product => product.ImageUrl).HasMaxLength(500);

        builder.Property(product => product.IsActive).IsRequired();

        builder.Property(product => product.CreatedAt).IsRequired();

        builder.Property(product => product.UpdatedAt);

        builder.HasIndex(product => product.Name);
    }
}
