using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;
using Turnit.GenericStore.Data;

namespace Turnit.GenericStore.Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var statusCode = (int)HttpStatusCode.InternalServerError;
                    var message = "Internal Server Error.";
                    var stackTrace = string.Empty;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;

                        message = exception.Message;

                        if (env.IsDevelopment())
                        {
                            stackTrace = exception.StackTrace;
                        }
                        context.Response.StatusCode = statusCode;

                        await context.Response.WriteAsync(new ExceptionDetails()
                        {
                            StatusCode = statusCode,
                            Message = message,
                            StackTrace = stackTrace
                        }.ToString());
                    }
                });
            });
        }
    }
}
