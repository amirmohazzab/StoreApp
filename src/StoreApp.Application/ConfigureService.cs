using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StoreApp.Application.Common.Mapping;
using System.Text;
using System.Threading.Tasks;
using StoreApp.Application.Common.BehaviorPipes;
using FluentValidation;

namespace StoreApp.Application
{
    public static class ConfigureService
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

           
        }
    }
}
