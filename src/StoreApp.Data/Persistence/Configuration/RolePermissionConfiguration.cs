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
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(x => new { x.RoleId, x.PermissionId });

            builder.HasOne(x => x.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(x => x.RoleId);

            builder.HasOne(x => x.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(x => x.PermissionId);
        }
    }
}
