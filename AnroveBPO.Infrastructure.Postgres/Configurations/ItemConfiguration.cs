using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnroveBPO.Infrastructure.Postgres.Configurations;

public sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("items");

        builder.HasKey(x => x.Id).HasName("pk_item");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(192)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("ux_items_name");

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .HasColumnType("numeric(18,2)");

        builder.Property(x => x.Category)
            .HasColumnName("category")
            .HasMaxLength(96)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnName("code")
            .HasMaxLength(12)
            .HasConversion(
                code => code.Value,
                value => ItemCode.Create(value).Value)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("ux_items_code");
    }
}
