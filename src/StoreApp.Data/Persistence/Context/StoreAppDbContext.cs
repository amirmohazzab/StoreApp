using Microsoft.EntityFrameworkCore;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.Context
{
    public class StoreAppDbContext : DbContext
    {
        public StoreAppDbContext(DbContextOptions<StoreAppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ProductBrand>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<ProductType>().HasQueryFilter(p => p.IsDelete == false);
        }
    }
}
