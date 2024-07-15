using ManyEntitiesSender.BLL;
using ManyEntitiesSender.BLL.Settings;
using ManyEntitiesSender.DAL;
using ManyEntitiesSender.DAL.Interfaces;
using ManyEntitiesSender.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddDataAccessLayer(builder);
AddBusinessLayer(builder);
AddOptions(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseMyCaching();

app.MapControllers();

app.Run();

void AddDataAccessLayer(WebApplicationBuilder builder)
{
    string? connectionString = builder.Configuration["ConnectionString:Postgres"];
    if (string.IsNullOrEmpty(connectionString)) {
        connectionString = "Host=localhost;Port=3000;Database=packages;Username=root;Password=tobacco";
    }
    builder.Services.InjectDAL(connectionString);
}

void AddBusinessLayer(WebApplicationBuilder builder)
{
    string? redisConfiguration = builder.Configuration["Redis:Configuration"];
    string? redisInstance = builder.Configuration["Redis:InstanceName"];
    if (string.IsNullOrEmpty(redisConfiguration)) {
        redisConfiguration = "localhost";
    }
    if (string.IsNullOrEmpty(redisInstance)) {
        redisInstance = "local";
    }

    builder.Services.InjectBLL(redisConfiguration, redisInstance);
}

void AddOptions(WebApplicationBuilder builder)
{
    builder.Services.Configure<PackageSettings>(
        builder.Configuration.GetSection("PackageSettings"));
}