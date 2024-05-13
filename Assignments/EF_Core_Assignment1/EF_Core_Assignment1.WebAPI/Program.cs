using EF_Core_Assignment1.Persistance.Contexts;
using EF_Core_Assignment1.Persistance.Seeder;
using EF_Core_Assignment1.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<NashTechContext>(opt =>
{
    //If Migration use appsetting.json value, if Debugging use ENV value provided by dockercompose.override.yml
        if (Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING") == null)
        Environment.SetEnvironmentVariable(
            "SQLSERVER_CONNECTION_STRING", 
            builder.Configuration.GetSection("ConnectionStrings:SqlServer").Value);

    opt.UseSqlServer(Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING"));
    Console.WriteLine(Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NashTechContext>();
    dbContext.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed the database within a new scope
app.MigrationDatabase();
app.SeedDatabase();

app.UseAuthorization();

app.MapControllers();

app.Run();
