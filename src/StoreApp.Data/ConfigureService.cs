using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminUserDto;
using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence;
using StoreApp.Data.Persistence.Context;
using StoreApp.Data.Repositories;
using StoreApp.Data.Security;
using StoreApp.Domain.Entities.User;
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
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddIdentityService(configuration);
            
            services.AddSingleton<Cloudinary>(provider =>
            {
                var config = configuration.GetSection("Cloudinary");
                var account = new Account(
                    config["CloudName"],
                    config["ApiKey"],
                    config["ApiSecret"]
                );
                return new Cloudinary(account);
            });
            services.AddSingleton<IImageStorage, CloudinaryImageStorage>();

            return services;
        }
    }
}
