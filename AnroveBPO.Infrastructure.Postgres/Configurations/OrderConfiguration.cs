using AnroveBPO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnroveBPO.Infrastructure.Postgres.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(x => x.Id).HasName("pk_order");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder.Property(x => x.OrderDate)
            .HasColumnName("order_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.ShipmentDate)
            .HasColumnName("shipment_date")
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.OrderNumber)
            .HasColumnName("order_number")
            .HasConversion<long>()
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(x => x.OrderStatus)
            .HasColumnName("order_status")
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.HasIndex(x => new { x.CustomerId, x.OrderNumber })
            .IsUnique()
            .HasDatabaseName("ux_orders_customer_id_order_number");

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .HasConstraintName("fk_orders_customer_id_customers_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
