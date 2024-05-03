using Net_Core_Assignment_Day_Middleware.Middlewares;
using Serilog;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorPages();
    //builder.Services.AddSerilog();

    ConfigureService(builder.Services);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //app.UseHsts();
    }

    //app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();
    app.MapControllers();

    app.UseCustomLoginMiddleware();

    app.Run();
} catch (Exception ex)
{
    Log.Fatal(ex, "Fatal Error occured, the app is closed unexpectedly");
} finally
{
    Log.CloseAndFlush();
}


void ConfigureService(IServiceCollection services)
{
    services.AddSerilog();
}