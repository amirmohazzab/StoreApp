using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence.SeedData
{
    public class GenerateFakeData
    {
        public static async Task SeedDataAsync(StoreAppDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!await context.ProductBrands.AnyAsync())
                {
                    List<ProductBrand> brands = ProductBrands();
                    await context.ProductBrands.AddRangeAsync(brands);
                    await context.SaveChangesAsync();
                }

                if (!await context.ProductTypes.AnyAsync())
                {
                    List<ProductType> types = ProductTypes();
                    await context.ProductTypes.AddRangeAsync(types);
                    await context.SaveChangesAsync();
                }

                if (!await context.Products.AnyAsync())
                {
                    List<Product> products = Products();
                    await context.Products.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<GenerateFakeData>();
                logger.LogError(ex, "Error in Seed Data");
            }
        }

        private static List<ProductBrand> ProductBrands()
        {
            return new List<ProductBrand>()
            {
               new()
               {
                  Description = "ProductBrand 1",
                  Summary = "Summary ProductBrand 1",
                  Title = "ProductBrand 1",
               },

               new()
               {
                  Description = "ProductBrand 2",
                  Summary = "Summary ProductBrand 2",
                  Title = "ProductBrand 2",
               }
            };
        }

        private static List<ProductType> ProductTypes()
        {
            return new List<ProductType>()
            {
               new()
               {
                  Description = "ProductType 1",
                  Summary = "Summary ProductType 1",
                  Title = "ProductType 1",
               },

               new()
               {
                  Description = "ProductType 2",
                  Summary = "Summary ProductType 2",
                  Title = "ProductType 2",
               }
             };
        }

        private static List<Product> Products()
        {
            return new List<Product>()
            {
                new()
                {
                   Description = "product 1",
                   PictureUrl = "image.jpg",
                   Price = 15000,
                   Title = "product 1",
                   Summary = "Summary Product 1",
                   ProductBrandId = 1,
                   ProductTypeId = 1
                },

                new()
                {
                   Description = "product 2",
                   PictureUrl = "image.jpg",
                   Price = 10000,
                   Title = "product 2",
                   Summary = "Summary Product 2",
                   ProductBrandId = 2,
                   ProductTypeId = 2
                },

                new()
                {
                   Description = "product 3",
                   PictureUrl = "image.jpg",
                   Price = 25000,
                   Title = "product 3",
                   Summary = "Summary Product 3",
                   ProductBrandId = 3,
                   ProductTypeId = 3
                },

                new()
                {
                   Description = "product 4",
                   PictureUrl = "image.jpg",
                   Price = 12000,
                   Title = "product 4",
                   Summary = "Summary Product 4",
                   ProductBrandId = 4,
                   ProductTypeId = 4
                },

                new()
                {
                   Description = "product 5",
                   PictureUrl = "image.jpg",
                   Price = 30000,
                   Title = "product 5",
                   Summary = "Summary Product 5",
                   ProductBrandId = 5,
                   ProductTypeId = 5
                }
            };
        }
    }
}
