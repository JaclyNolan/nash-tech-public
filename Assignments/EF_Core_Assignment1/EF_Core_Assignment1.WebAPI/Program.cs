using EF_Core_Assignment1.Application.Mapper;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Persistance.Contexts;
using EF_Core_Assignment1.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(opt => 
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
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

builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
builder.Services.AddAutoMapper(typeof(NashTechProfile).Assembly);

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<NashTechContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((options) =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:5173") // Add your frontend URL here
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.MigrationDatabase();
app.SeedDatabase();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseAuthorization();

// Use the CORS policy
app.UseCors("MyAllowSpecificOrigin");

app.MapControllers();

app.Run();
