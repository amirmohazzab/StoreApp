using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StoreApp.Application.Contracts;
using StoreApp.Data.Persistence.Context;
using StoreApp.Data.Persistence.SeedData;
using StoreApp.Domain.Exceptions;
using StoreApp.Web.Extensions;
using StoreApp.Web.Middleware;
using StoreApp.Web.Services;

namespace StoreApp.Web
{
    public static class ConfigureService
    {
        public static IServiceCollection AddWebConfigureServices(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddControllers();

            ApiBehaviorOptions(builder);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocumentation();
            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "My API",
            //        Version = "v1"
            //    });
            //});
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(configuration["CorsAddress:AddressHttp"]);
                });
            });

            builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();

            return builder.Services;
        }

        public static async Task<IApplicationBuilder> AddWebAppService(this WebApplication app)
        {
            app.UseMiddleware<MiddlewareExceptionHandler>();

            #region Auto Migration

            //var scope = app.Services.CreateScope();
            //var services = scope.ServiceProvider;
            //var context = services.GetRequiredService<StoreAppDbContext>();
            //var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //try
            //{
            //    await context.Database.MigrateAsync();
            //    await GenerateFakeData.SeedDataAsync(context, loggerFactory);
            //}
            //catch (Exception e)
            //{
            //    var logger = loggerFactory.CreateLogger<Program>();
            //    logger.LogError(e, "Error Exception for migrations");
            //}

            #endregion

            #region HTTP request pipeline
            app.UseSwaggerDocumentation();
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
            //    c.RoutePrefix = string.Empty; // swagger روی root بالا بیاد
            //});
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            #endregion

            return app;
        }

        private static void ApiBehaviorOptions(WebApplicationBuilder builder)
        {
            //TODO check this
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                        .SelectMany(v => v.Value!.Errors)
                        .Select(c => c.ErrorMessage).ToList();

                    return new BadRequestObjectResult(new ApiToReturn(400, errors));
                };
            });
        }
    }
}
