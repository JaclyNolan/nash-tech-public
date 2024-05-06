using MVCDotNetAssignment.BusinessLogics.Repositories;
using MVCDotNetAssignment.BusinessLogics.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "MyAreaNashTech",
    pattern: "{area:exists}/{controller=Rookies}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IPeopleRepository, PeopleRepository>();
    services.AddScoped<IPeopleBusinessLogics, PeopleBusinessLogics>();
}