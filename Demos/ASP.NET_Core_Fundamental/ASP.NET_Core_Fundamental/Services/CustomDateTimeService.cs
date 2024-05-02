namespace ASP.NET_Core_Fundamental.Services
{
    public interface ICustomDatetimeService
    {
        string GetDate();
    }
    public class CustomDateTimeService : ICustomDatetimeService
    {
        private readonly string _date;
        private readonly ILogger<CustomDateTimeService> _logger;
        public CustomDateTimeService(ILogger<CustomDateTimeService> logger)
        {
            _logger = logger;
            _logger.LogInformation("CustomerDateTimeService Constructor");
            _date = DateTime.Now.ToString();
        }
        public string GetDate()
        {
            return _date;
        }
    }
}
