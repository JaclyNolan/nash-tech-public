using ASP.NET_Core_Fundamental.Middlewares;
using ASP.NET_Core_Fundamental.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCustomDateTimeMiddleware();

app.MapRazorPages();

app.Run();

void ConfigureServices(IServiceCollection services)
{
	services.AddScoped<ICustomDatetimeService, CustomDateTimeService>();
}