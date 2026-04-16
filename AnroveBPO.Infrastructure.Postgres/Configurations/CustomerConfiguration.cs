using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnroveBPO.Infrastructure.Postgres.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(x => x.Id).HasName("pk_customer");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(192)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnName("code")
            .HasMaxLength(9)
            .HasConversion(
                code => code.Value,
                value => CustomerCode.Create(value).Value)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("ux_customers_code");

        builder.Property(x => x.Address)
            .HasColumnName("address")
            .HasConversion(
                _ => "{}",
                _ => new Address())
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.Discount)
            .HasColumnName("discount")
            .HasConversion(
                discount => discount == null ? (double?)null : discount.Value,
                value => value.HasValue ? Discount.Create(value.Value).Value : null)
            .HasColumnType("double precision");
    }
}
