using AnroveBPO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnroveBPO.Infrastructure.Postgres.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(x => x.Id).HasName("pk_order_item");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(x => x.ItemId)
            .HasColumnName("item_id")
            .IsRequired();

        builder.Property(x => x.ItemsCount)
            .HasColumnName("items_count")
            .HasConversion<long>()
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(x => x.ItemPrice)
            .HasColumnName("item_price")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.HasIndex(x => new { x.OrderId, x.ItemId })
            .IsUnique()
            .HasDatabaseName("ux_order_items_order_id_item_id");

        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .HasConstraintName("fk_order_items_order_id_orders_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .HasConstraintName("fk_order_items_item_id_items_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
