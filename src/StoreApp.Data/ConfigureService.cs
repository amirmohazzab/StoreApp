using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreApp.Application.Contracts;
using StoreApp.Data.Persistence;
using StoreApp.Data.Persistence.Context;
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
