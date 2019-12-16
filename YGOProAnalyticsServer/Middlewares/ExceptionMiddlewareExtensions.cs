using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;

namespace YGOProAnalyticsServer.Middlewares
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if(!Directory.Exists("Logs"))
                        {
                            Directory.CreateDirectory("Logs");
                        }
                        
                        using (var streamWriter = File.AppendText($"Logs/ExceptionsLog_{DateTime.Today.Date.ToString("dd-MM-yyyy")}.txt"))
                        {
                            streamWriter.WriteLine($"Current DateTime = {DateTime.Now}");
                            streamWriter.WriteLine(contextFeature.Error);
                            streamWriter.WriteLine();
                        }
                    }
                });
            });
        }
    }
}
