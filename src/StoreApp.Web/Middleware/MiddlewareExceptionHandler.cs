using StoreApp.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace StoreApp.Web.Middleware
{
    public class MiddlewareExceptionHandler
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<MiddlewareExceptionHandler> logger;
        private readonly RequestDelegate next;

        public MiddlewareExceptionHandler(
            IWebHostEnvironment env,
            ILogger<MiddlewareExceptionHandler> logger,
            RequestDelegate next)
        {
            this.env = env;
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                if (context.Response.HasStarted)
                {
                    logger.LogError("Response has already started BEFORE middleware wrote!");
                    return; // فقط خروج — هیچ تغییری روی header یا body نمی‌توان زد
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var result = HandleServerError(context, exception, options);

                result = HandleResult(context, exception, result, options);

                await context.Response.WriteAsync(result);
            }
        }

        private static string HandleServerError(HttpContext context, Exception exception, JsonSerializerOptions options)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonSerializer.Serialize(new ApiToReturn(500, exception.Message), options);
            return result;
        }

        private string HandleResult(HttpContext context, Exception exception, string result, JsonSerializerOptions options)
        {
            switch (exception)
            {
                case NotFoundEntityException notFoundEntityException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new ApiToReturn(404,
                        notFoundEntityException.Message, notFoundEntityException.Messages,
                        exception.Message), options);
                    break;

                case BadRequestEntityException badRequestEntityException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new ApiToReturn(400,
                        badRequestEntityException.Message, badRequestEntityException.Messages,
                        exception.Message), options);
                    break;

                case ValidationEntityException validationEntityException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new ApiToReturn(400,
                        validationEntityException.Message, validationEntityException.Messages,
                        exception.Message), options);
                    break;
            }

            return result;
        }
    }
}
