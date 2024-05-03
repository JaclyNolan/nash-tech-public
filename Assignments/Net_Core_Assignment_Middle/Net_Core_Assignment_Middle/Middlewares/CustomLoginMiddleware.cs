using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Net_Core_Assignment_Day_Middleware.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomLoginMiddleware
    {
        private readonly RequestDelegate _next;
        private class Data
        {
            public string Schema { get; set; } = string.Empty;
            public HostString Host { get; set; }
            public string Path { get; set; } = string.Empty;
            public QueryString QueryString { get; set; }
            public string RequestBody { get; set; } = string.Empty;

            public override string ToString()
            {
                return $"New Request: \n" +
                       $"Shema: {this.Schema} \n" +
                       $"Host: {this.Host} \n" +
                       $"Path: {this.Path} \n" +
                       $"QueryString: {this.QueryString} \n" +
                       $"RequestBody: {this.RequestBody} \n";
            }
        }

        public CustomLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Data data = await GetData(httpContext);
            LogData(data);
            await _next(httpContext);
        }

        private async Task<Data> GetData(HttpContext httpContext)
        {
            Data data = new Data();
            data.Schema = httpContext.Request.Scheme;
            data.Host = httpContext.Request.Host;
            data.Path = httpContext.Request.Path;
            data.QueryString = httpContext.Request.QueryString;
            data.RequestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
            return data;
        }
        private void LogData(Data data)
        {
            //To-do: Move log path to appsettings for easier configuration
            Log.Debug($"{data}");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomLoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomLoginMiddleware>();
        }
    }
}
