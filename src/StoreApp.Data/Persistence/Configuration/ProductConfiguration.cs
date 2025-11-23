using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PictureUrl).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasOne(p => p.ProductType).WithMany().HasForeignKey(p => p.ProductTypeId);
            builder.HasOne(p => p.ProductBrand).WithMany().HasForeignKey(p => p.ProductBrandId);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.Title).HasMaxLength(100);
            builder.Property(p => p.Summary).HasMaxLength(100);
            builder.Property(p => p.OldPrice).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.CreatedByUser).WithMany().HasForeignKey(p => p.CreatedBy)
                .IsRequired(false)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.LastModifiedByUser).WithMany().HasForeignKey(p => p.LastModifiedBy)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.ProductImages).WithOne(i => i.Product).HasForeignKey(i => i.ProductId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Colors).WithOne(c => c.Product).HasForeignKey(c => c.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Sizes).WithOne(s => s.Product).HasForeignKey(s => s.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
