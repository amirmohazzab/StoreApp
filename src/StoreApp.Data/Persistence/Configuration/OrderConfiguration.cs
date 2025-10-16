using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShipToAddress, a =>
            {
                a.WithOwner();
            });

            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.DeliveryMethod).WithMany();
            builder.HasOne(o => o.Portal).WithOne(p => p.Order)
                .HasForeignKey<Portal>(p => p.OrderId);
            builder.Property(oi => oi.SubTotal).HasPrecision(18, 2);
            builder.HasOne(o => o.User).WithMany().HasForeignKey(o => o.CreatedBy);
            builder.HasOne(o => o.User).WithMany().HasForeignKey(o => o.LastModifiedBy);
        }
    }
}
