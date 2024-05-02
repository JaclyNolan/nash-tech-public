using ASP.NET_Core_Fundamental.Middlewares;
using ASP.NET_Core_Fundamental.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NET_Core_Fundamental.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly ICustomDatetimeService _customDatetimeService;

		public IndexModel(ILogger<IndexModel> logger, ICustomDatetimeService customDatetimeService)
		{
			_logger = logger;
			_customDatetimeService = customDatetimeService;
		}

		public string DateTimeFromMiddleware { get; set; } = string.Empty;
		public string DateTimeFromDependency { get; set; } = string.Empty;

        public void OnGet()
		{
			if (HttpContext.Items.TryGetValue(CustomDateTimeMiddleware.HttpContentItemKey, out var dateString))
			{
				DateTimeFromMiddleware = dateString.ToString();
			}
			DateTimeFromDependency = _customDatetimeService.GetDate();
		}
	}
}
