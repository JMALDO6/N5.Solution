using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using N5.API.Helpers;
using N5.Application;
using N5.Domain.Interfaces;
using N5.Infrastructure.Elasticsearch;
using N5.Infrastructure.Persistence;
using Nest;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<ApplicationAssemblyMarker>();
});

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<PermissionsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

// Configure Serilog from appsettings.json
builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day);
});

// Elasticsearch configuration
builder.Services.Configure<ElasticsearchSettings>(builder.Configuration.GetSection("Elasticsearch"));
builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ElasticsearchSettings>>().Value;

    var settings = new ConnectionSettings(new Uri(config.Uri))
        .DefaultIndex(config.DefaultIndex);

    return new ElasticClient(settings);
});
builder.Services.AddScoped<IPermissionElasticService, PermissionElasticService>();

var app = builder.Build();

Log.Information("Starting up the application");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();