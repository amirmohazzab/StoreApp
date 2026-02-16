using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.Basket;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Entities.Order;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.Context
{
    public class StoreAppDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>,
    UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public StoreAppDbContext(DbContextOptions<StoreAppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<CustomerBasket> CustomerBaskets { get; set; }

        public DbSet<CustomerBasketItem> CustomerBasketItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresss { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        public DbSet<UserLike> UserLikes { get; set; }

        public DbSet<ProductReview> Reviews { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<UserWishlist> Wishlists { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }

        public DbSet<ContactConversation> ContactConversations { get; set; }

        public DbSet<ContactAttachment> ContactAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ProductBrand>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ProductType>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<Address>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<Order>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<DeliveryMethod>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<Portal>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ProductReview>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ProductCategory>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ProductSize>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ProductColor>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ProductImage>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ContactMessage>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ContactAttachment>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ContactConversation>().HasQueryFilter(p => p.IsDelete == false);
        }
    }
}


//protected override void OnModelCreating(ModelBuilder modelBuilder)
//{

//    // اعمال فیلتر حذف نرم روی همه Entity هایی که IsDelete دارند
//    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
//    {
//        if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
//        {
//            var method = typeof(AppDbContext)
//                .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
//                .MakeGenericMethod(entityType.ClrType);

//            method.Invoke(null, new object[] { modelBuilder });
//        }
//    }
//}

//private static void SetSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
//    where TEntity : BaseEntity
//{
//    modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDelete);
//}
