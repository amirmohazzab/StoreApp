using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.Configuration
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.Title).HasMaxLength(100);
            builder.Property(p => p.Summary).HasMaxLength(100);
            builder.HasOne(o => o.User).WithMany().HasForeignKey(o => o.CreatedBy);
            builder.HasOne(o => o.User).WithMany().HasForeignKey(o => o.LastModifiedBy);
        }
    }
}
