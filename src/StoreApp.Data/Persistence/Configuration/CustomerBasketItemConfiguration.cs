using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.Configuration
{
    internal class CustomerBasketItemConfiguration : IEntityTypeConfiguration<CustomerBasketItem>
    {
        public void Configure(EntityTypeBuilder<CustomerBasketItem> builder)
        {
           builder.Property(e => e.Price).HasPrecision(18, 2);

           builder.Property(e => e.Discount).HasPrecision(18, 2);
        }
    }
}
