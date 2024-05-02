using ASP.NET_Core_Fundamental.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ASP.NET_Core_Fundamental.Middlewares
{
	// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
	public class CustomDateTimeMiddleware
	{
		private readonly RequestDelegate _next;
		public const string HttpContentItemKey = nameof(HttpContentItemKey);

		public CustomDateTimeMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext, ICustomDatetimeService customDatetimeService)
		{
			//await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

			string value = customDatetimeService.GetDate();
			httpContext.Items.Add(HttpContentItemKey, value);
			await _next(httpContext);
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class CustomerDateTimeMiddlewareExtensions
	{
		public static IApplicationBuilder UseCustomDateTimeMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CustomDateTimeMiddleware>();
		}
	}
}
