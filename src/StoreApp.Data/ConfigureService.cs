using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using StoreApp.Application.Contracts;
using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence;
using StoreApp.Data.Persistence.Context;
using StoreApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("StoreAppConnectionString");

            services.AddDbContext<StoreAppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //services.AddScoped(typeof(IGenericRepository<>), typeof(IGenericRepository<>));
            //services.AddSingleton<IConnectionMultiplexer>(opt =>
            //{
            //    var config = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), ignoreUnknown: true);
            //    return ConnectionMultiplexer.Connect(config);
            //});

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
