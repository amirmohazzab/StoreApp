using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.Configuration
{
    public class UserWishlistConfiguration : IEntityTypeConfiguration<UserWishlist>
    {
        public void Configure(EntityTypeBuilder<UserWishlist> builder)
        {
            builder.HasIndex(x => new { x.UserId, x.ProductId }).IsUnique();
            builder.HasOne(x => x.Product)
                 .WithMany(x => x.UserWishlists)
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired(false);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserWishlists)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
