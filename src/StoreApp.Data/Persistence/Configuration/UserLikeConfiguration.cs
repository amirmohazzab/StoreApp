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
    public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.HasKey(ul => ul.Id);

            builder.HasOne(ul => ul.User)
                   .WithMany(u => u.UserLikes)
                   .HasForeignKey(ul => ul.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ul => ul.Product)
                   .WithMany(p => p.UserLikes)
                   .HasForeignKey(ul => ul.ProductId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ul => ul.Liked).IsRequired().HasDefaultValue(false);
        }
    }
}
