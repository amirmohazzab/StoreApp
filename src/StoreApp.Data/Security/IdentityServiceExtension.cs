using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence.Context;
using StoreApp.Data.Repositories;
using StoreApp.Domain.Entities.User;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Security
{
    public static class IdentityServiceExtension
    {
        public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<User>()
                .AddUserManager<UserManager<User>>()
                .AddSignInManager<SignInManager<User>>()
                .AddTokenProvider("MyApp", typeof(DataProtectorTokenProvider<User>))
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddRoleValidator<RoleValidator<Role>>()
                .AddEntityFrameworkStores<StoreAppDbContext>();

            services.Configure(ConfigureOptionsIdentity());

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.SaveToken = true;
              options.TokenValidationParameters = OptionsTokenValidationParameters(configuration);
              options.Events = JwtOptionsEvents();
          });

            services.AddAuthorization(options =>
            {
                // چون داخل Infrastructure هستیم استفاده از scoped provider کاملاً درست است
                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<StoreAppDbContext>();

                var permissions = context.Permissions.ToList();

                foreach (var permission in permissions)
                {
                    options.AddPolicy(permission.Name,
                        policy => policy.Requirements.Add(new PermissionRequirement(permission.Name)));
                }
            });

            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            //    services.AddIdentityCore<User>(options =>
            //    {
            //        options.Password.RequireDigit = true;
            //        options.Password.RequireLowercase = true;
            //        options.Password.RequireNonAlphanumeric = true;
            //        options.Password.RequireUppercase = true;
            //        options.Password.RequiredLength = 5;
            //        options.Password.RequiredUniqueChars = 1;
            //    })
            //.AddRoles<Role>()
            //.AddRoleManager<RoleManager<Role>>()
            //.AddSignInManager<SignInManager<User>>()
            //.AddUserManager<UserManager<User>>()
            //.AddEntityFrameworkStores<StoreAppDbContext>()
            //.AddDefaultTokenProviders();

            //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.SaveToken = true;
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(
            //                Encoding.UTF8.GetBytes(configuration["JWTConfiguration:Key"]!)
            //            ),
            //            ValidateIssuer = true,
            //            ValidIssuer = configuration["JWTConfiguration:Issuer"],

            //            ValidateAudience = configuration.GetValue<bool>("JWTConfiguration:ValidateAudience"),
            //            ValidAudience = configuration["JWTConfiguration:Audience"],

            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.Zero
            //        };
            //    });

            //    services.AddAuthorization();
            //    return services;
        }

        private static TokenValidationParameters OptionsTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                //ValidateIssuerSigningKey = true,
                //IssuerSigningKey =
                //    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfiguration:Key"] ?? string.Empty)),
                //ValidateIssuer = true,
                //ValidIssuer = configuration["JWTConfiguration:Issuer"],
                //ValidateAudience = Convert.ToBoolean(configuration["JWTConfiguration:Audience"]),
                //ValidateLifetime = true,
                //ClockSkew = TimeSpan.Zero,
                //RequireExpirationTime = true

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWTConfiguration:Key"] ?? string.Empty)
                ),
                ValidateIssuer = true,
                ValidIssuer = configuration["JWTConfiguration:Issuer"],

                ValidateAudience = configuration.GetValue<bool>("JWTConfiguration:ValidateAudience"),
                ValidAudience = configuration["JWTConfiguration:Audience"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                //ClockSkew = TimeSpan.FromMinutes(5),
                RequireExpirationTime = true
            };

        }

        private static JwtBearerEvents JwtOptionsEvents()
        {
            return new JwtBearerEvents
            {
                OnAuthenticationFailed = c =>
                {
                    if (!c.Response.HasStarted) 
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        //c.Response.ContentType = "application/json";
                        //return c.Response.WriteAsync("Server Error. please try again");
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();

                    if (!context.Response.HasStarted) 
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        //context.Response.ContentType = "application/json";
                        //var result = JsonConvert.SerializeObject(new ApiToReturn(401, "you are not authenticated"));
                        //return context.Response.WriteAsync(result);
                    }
                    return Task.CompletedTask;
                },
                OnForbidden = context =>
                {
                    if (!context.Response.HasStarted)
                    {
                        context.Response.StatusCode = 403;
                        //context.Response.ContentType = "application/json";
                        //var result = JsonConvert.SerializeObject(new ApiToReturn(403,
                           // "you don't have access, please enter the website"));
                        //return context.Response.WriteAsync(result);
                    }
                    return Task.CompletedTask;
                }
            };
        }

        private static Action<IdentityOptions> ConfigureOptionsIdentity()
        {
            return options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                // options.User.RequireUniqueEmail = true;
            };
        }
    }
}
