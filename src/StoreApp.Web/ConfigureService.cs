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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();

            return builder.Services;
        }

        public static IApplicationBuilder AddWebAppService(this WebApplication app)
        {
            //#region seed data and auto migration

            //create scope
            //var scope = app.Services.CreateScope();
            //var services = scope.ServiceProvider;
            //get service
            //var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //var context = services.GetRequiredService<StoreAppDbContext>();
            //auto migrations
            //try
            //{
            //    await context.Database.MigrateAsync();
            //    await GenerateFakeData.SeedDataAsync(context, loggerFactory);
            //}
            //catch (Exception e)
            //{
            //    var logger = loggerFactory.CreateLogger<Program>();
            //    logger.LogError(e, "error exception for migrations");
            //}

            //#endregion

            #region HTTP request pipeline

            app.UseSwaggerDocumentation();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseMiddleware<MiddlewareExceptionHandler>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //await app.RunAsync();

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
