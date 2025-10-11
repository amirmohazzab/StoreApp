using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreApp.Application.Contracts;
using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence;
using StoreApp.Data.Persistence.Context;
using StoreApp.Data.Repositories;
using StoreApp.Data.Security;
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

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddIdentityService(configuration);

            return services;
        }
    }
}
