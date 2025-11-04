using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.State);
            builder.Property(x => x.City);
            builder.Property(x => x.FirstName);
            builder.Property(x => x.LastName);
            builder.Property(x => x.PostalCode).HasMaxLength(10);
            builder.Property(x => x.Number).HasMaxLength(11).IsRequired();

            builder.HasOne(x => x.User).WithMany(u => u.Addresses)
                .HasForeignKey(x => x.UserId);
        }
    }
}
